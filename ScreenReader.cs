using System;
using System.Runtime.InteropServices;
using MelonLoader;
using MelatoninAccess;
using UnityEngine;

public static class ScreenReader
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
    private static extern bool Tolk_Speak([MarshalAs(UnmanagedType.LPWStr)] string str, bool interrupt);

    [DllImport("Tolk.dll", CharSet = CharSet.Unicode)]
    private static extern bool Tolk_Silence();

    private static string lastText = "";
    private static float lastTime = 0f;

    public static void Initialize()
    {
        Tolk_Load();
        if (Tolk_IsLoaded())
        {
            MelonLogger.Msg("Tolk loaded successfully.");
            Say("Melatonin Access Mod Loaded", true);
        }
        else
        {
            MelonLogger.Error("Failed to load Tolk.");
        }
    }

    public static void Unload()
    {
        Tolk_Unload();
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
        Tolk_Output(text, interrupt);
    }

    public static void Stop()
    {
        Tolk_Silence();
    }
}
