using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using MelonLoader;
using MelatoninAccess;
using UnityEngine;

/// <summary>
/// Speech output. The public surface (Initialize/Say/Stop/Unload) is the same
/// on every platform; only the backend behind it differs.
///
///   Windows - Tolk, exactly as before (32-bit Tolk.dll + nvdaControllerClient32.dll).
///   macOS   - Prism (https://github.com/ethindp/prism), which speaks through
///             VoiceOver and AVSpeech.
///
/// Tolk is Windows-only, so on macOS the old build loaded fine and then threw
/// DllNotFoundException out of Initialize, leaving the mod running but silent.
/// Prism is not used on Windows because it publishes no 32-bit build and the
/// Windows game is a 32-bit process.
/// </summary>
public static class ScreenReader
{
    private interface IBackend
    {
        string Name { get; }
        bool Load();
        void Output(string text, bool interrupt);
        void Silence();
        void Unload();
    }

    // ---------------------------------------------------------------- Windows

    private sealed class TolkBackend : IBackend
    {
        [DllImport("Tolk.dll", CharSet = CharSet.Unicode)]
        private static extern void Tolk_Load();

        [DllImport("Tolk.dll", CharSet = CharSet.Unicode)]
        private static extern void Tolk_Unload();

        [DllImport("Tolk.dll", CharSet = CharSet.Unicode)]
        private static extern bool Tolk_IsLoaded();

        [DllImport("Tolk.dll", CharSet = CharSet.Unicode)]
        private static extern bool Tolk_Output([MarshalAs(UnmanagedType.LPWStr)] string str, bool interrupt);

        [DllImport("Tolk.dll", CharSet = CharSet.Unicode)]
        private static extern bool Tolk_Silence();

        public string Name { get { return "Tolk"; } }

        public bool Load()
        {
            Tolk_Load();
            return Tolk_IsLoaded();
        }

        public void Output(string text, bool interrupt) { Tolk_Output(text, interrupt); }
        public void Silence() { Tolk_Silence(); }
        public void Unload() { Tolk_Unload(); }
    }

    // ------------------------------------------------------------------ macOS

    private sealed class PrismBackend : IBackend
    {
        private const int RTLD_NOW = 2;

        [DllImport("libSystem.dylib")]
        private static extern IntPtr dlopen(string path, int mode);
        [DllImport("libSystem.dylib")]
        private static extern IntPtr dlsym(IntPtr handle, string symbol);
        [DllImport("libSystem.dylib")]
        private static extern IntPtr dlerror();

        // Bound through dlsym rather than [DllImport] so the dylib can be found
        // by absolute path: it ships beside the mod, which is not on any search
        // path dyld consults.
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr FnInit(IntPtr config);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FnShutdown(IntPtr ctx);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr FnCreateBest(IntPtr ctx);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr FnBackendName(IntPtr backend);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int FnOutput(IntPtr backend, byte[] utf8Text,
                                      [MarshalAs(UnmanagedType.I1)] bool interrupt);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int FnStop(IntPtr backend);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FnBackendFree(IntPtr backend);

        private IntPtr lib = IntPtr.Zero;
        private IntPtr ctx = IntPtr.Zero;
        private IntPtr backend = IntPtr.Zero;
        private string backendName = "Prism";

        private FnShutdown fnShutdown;
        private FnOutput fnOutput;
        private FnStop fnStop;
        private FnBackendFree fnBackendFree;

        public string Name { get { return backendName; } }

        private static string[] CandidatePaths()
        {
            var dirs = new System.Collections.Generic.List<string>();
            try
            {
                string asm = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (!string.IsNullOrEmpty(asm))
                {
                    dirs.Add(asm);                                  // Mods/
                    dirs.Add(Path.Combine(asm, "lib"));             // Mods/lib/
                    string parent = Path.GetDirectoryName(asm);
                    if (!string.IsNullOrEmpty(parent))
                    {
                        dirs.Add(parent);                           // game root
                        dirs.Add(Path.Combine(parent, "lib"));
                    }
                }
            }
            catch { }
            try { dirs.Add(Directory.GetCurrentDirectory()); } catch { }

            var paths = new System.Collections.Generic.List<string>();
            foreach (string d in dirs)
            {
                if (!string.IsNullOrEmpty(d)) paths.Add(Path.Combine(d, "libprism.dylib"));
            }
            // Last resort: let dyld search its own paths.
            paths.Add("libprism.dylib");
            return paths.ToArray();
        }

        private static string LastDlError()
        {
            IntPtr e = dlerror();
            return e == IntPtr.Zero ? "unknown error" : Marshal.PtrToStringAnsi(e);
        }

        private T Bind<T>(string symbol) where T : class
        {
            IntPtr p = dlsym(lib, symbol);
            if (p == IntPtr.Zero)
                throw new EntryPointNotFoundException("libprism.dylib is missing " + symbol);
            return Marshal.GetDelegateForFunctionPointer(p, typeof(T)) as T;
        }

        public bool Load()
        {
            string tried = "";
            foreach (string path in CandidatePaths())
            {
                lib = dlopen(path, RTLD_NOW);
                if (lib != IntPtr.Zero) break;
                tried += "\n  " + path + " -> " + LastDlError();
            }
            if (lib == IntPtr.Zero)
            {
                MelonLogger.Error("Could not load libprism.dylib. Speech is unavailable." +
                                  " Tried:" + tried);
                return false;
            }

            var fnInit = Bind<FnInit>("prism_init");
            var fnCreateBest = Bind<FnCreateBest>("prism_registry_create_best");
            var fnBackendName = Bind<FnBackendName>("prism_backend_name");
            fnOutput = Bind<FnOutput>("prism_backend_output");
            fnStop = Bind<FnStop>("prism_backend_stop");
            fnBackendFree = Bind<FnBackendFree>("prism_backend_free");
            fnShutdown = Bind<FnShutdown>("prism_shutdown");

            // A null config selects Prism's defaults. prism_config_init returns
            // the struct by value, which does not marshal dependably here.
            ctx = fnInit(IntPtr.Zero);
            if (ctx == IntPtr.Zero)
            {
                MelonLogger.Error("prism_init returned null; speech is unavailable.");
                return false;
            }

            backend = fnCreateBest(ctx);
            if (backend == IntPtr.Zero)
            {
                MelonLogger.Error("Prism found no usable speech backend on this machine.");
                fnShutdown(ctx);
                ctx = IntPtr.Zero;
                return false;
            }

            IntPtr namePtr = fnBackendName(backend);
            if (namePtr != IntPtr.Zero)
            {
                string n = Marshal.PtrToStringAnsi(namePtr);
                if (!string.IsNullOrEmpty(n)) backendName = "Prism (" + n + ")";
            }
            return true;
        }

        public void Output(string text, bool interrupt)
        {
            if (backend == IntPtr.Zero || fnOutput == null) return;
            // Prism takes null-terminated UTF-8; build the bytes explicitly
            // rather than relying on default string marshaling.
            byte[] utf8 = Encoding.UTF8.GetBytes(text + "\0");
            fnOutput(backend, utf8, interrupt);
        }

        public void Silence()
        {
            if (backend != IntPtr.Zero && fnStop != null) fnStop(backend);
        }

        public void Unload()
        {
            if (backend != IntPtr.Zero && fnBackendFree != null) { fnBackendFree(backend); backend = IntPtr.Zero; }
            if (ctx != IntPtr.Zero && fnShutdown != null) { fnShutdown(ctx); ctx = IntPtr.Zero; }
        }
    }

    // ----------------------------------------------------------------- shared

    private static IBackend backend;
    private static bool loaded;
    private static string lastText = "";
    private static float lastTime = 0f;

    private static bool IsMac()
    {
        try
        {
            if (Application.platform == RuntimePlatform.OSXPlayer ||
                Application.platform == RuntimePlatform.OSXEditor)
                return true;
        }
        catch { }
        // Mono reports MacOSX or, on older versions, Unix.
        int p = (int)Environment.OSVersion.Platform;
        return p == 6 || (p == 4 && Directory.Exists("/System/Library/CoreServices"));
    }

    public static void Initialize()
    {
        backend = IsMac() ? (IBackend)new PrismBackend() : new TolkBackend();

        try
        {
            loaded = backend.Load();
        }
        catch (Exception e)
        {
            loaded = false;
            MelonLogger.Error("Speech backend failed to load: " + e.Message);
        }

        if (loaded)
        {
            MelonLogger.Msg(backend.Name + " loaded successfully.");
            Say(Loc.Get("mod_loaded"), true);
        }
        else
        {
            MelonLogger.Error("Failed to load a speech backend; the mod will run silently.");
        }
    }

    public static void Unload()
    {
        if (backend == null) return;
        try { backend.Unload(); } catch { }
        loaded = false;
    }

    public static void Say(string text, bool interrupt = false)
    {
        if (string.IsNullOrEmpty(text)) return;

        text = string.Join(" ", text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries));
        if (string.IsNullOrEmpty(text)) return;

        // Debounce exact repetitions within 0.5s
        float now = Time.unscaledTime;
        if (now <= 0f) now = Time.time;
        if (text == lastText && now - lastTime < 0.5f) return;

        lastText = text;
        lastTime = now;

        DebugLogger.LogScreenReader(text);

        if (!loaded || backend == null) return;
        try { backend.Output(text, interrupt); }
        catch (Exception e) { MelonLogger.Error("Speech output failed: " + e.Message); }
    }

    public static void Stop()
    {
        if (!loaded || backend == null) return;
        try { backend.Silence(); } catch { }
    }
}
