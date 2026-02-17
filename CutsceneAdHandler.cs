using System;
using System.Collections.Generic;
using System.IO;
using MelonLoader;
using UnityEngine;

namespace MelatoninAccess
{
    internal static class CutsceneAdHandler
    {
        private const string HandlerName = "CutsceneAdHandler";

        private static readonly Dictionary<string, CutsceneAdScript> _scriptsByCutsceneKey =
            new Dictionary<string, CutsceneAdScript>(StringComparer.OrdinalIgnoreCase);

        private static bool _initialized;
        private static bool _scriptsLoaded;
        private static string _activeCutsceneKey = "";
        private static CutsceneAdScript _activeScript;
        private static float _activeCutsceneStartTime = -1f;
        private static int _nextCueIndex;
        private static float _nextLoadAttemptTime;
        private static float _lastUnavailableLogTime = -10f;
        private static string _lastLoadError = "";
        private static string _lastReportedLoadError = "";

        internal static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            LoadScripts();
        }

        internal static void Update()
        {
            if (!_initialized)
            {
                Initialize();
            }

            if (!ModConfig.AnnounceTutorialDialog)
            {
                ResetActiveCutscene();
                return;
            }

            if (Chapter.dir == null)
            {
                ResetActiveCutscene();
                return;
            }

            bool isIntro = Chapter.dir.CheckIsCutsceneIntro();
            bool isOutro = Chapter.dir.CheckIsCutsceneOutro();
            if (!isIntro && !isOutro)
            {
                ResetActiveCutscene();
                return;
            }

            if (!_scriptsLoaded)
            {
                float now = Time.unscaledTime;
                if (now >= _nextLoadAttemptTime)
                {
                    LoadScripts();
                    _nextLoadAttemptTime = now + 5f;
                }

                if (_scriptsLoaded)
                {
                    return;
                }

                float nowUnavailable = Time.unscaledTime;
                if (nowUnavailable - _lastUnavailableLogTime >= 5f)
                {
                    _lastUnavailableLogTime = nowUnavailable;
                    string suffix = string.IsNullOrWhiteSpace(_lastLoadError)
                        ? ""
                        : $" Last error: {_lastLoadError}";
                    DebugLogger.Log(LogCategory.Handler, HandlerName, $"Cutscene AD scripts are not loaded.{suffix}");
                }
                return;
            }

            string sceneName = GetActiveSceneName();
            string cutsceneType = isIntro ? "intro" : "outro";
            string cutsceneKey = BuildCutsceneKey(sceneName, cutsceneType);
            if (string.IsNullOrWhiteSpace(cutsceneKey))
            {
                ResetActiveCutscene();
                return;
            }

            if (!string.Equals(_activeCutsceneKey, cutsceneKey, StringComparison.OrdinalIgnoreCase))
            {
                StartCutscene(cutsceneKey);
            }

            if (_activeScript == null || _activeScript.entries == null || _activeScript.entries.Length == 0)
            {
                return;
            }

            float elapsed = Time.time - _activeCutsceneStartTime;
            while (_nextCueIndex < _activeScript.entries.Length)
            {
                CutsceneAdCue cue = _activeScript.entries[_nextCueIndex];
                if (cue == null)
                {
                    _nextCueIndex++;
                    continue;
                }

                if (elapsed < cue.atSeconds)
                {
                    break;
                }

                string spoken = Loc.Get(cue.textKey);
                if (string.IsNullOrWhiteSpace(spoken) || string.Equals(spoken, cue.textKey, StringComparison.Ordinal))
                {
                    DebugLogger.Log(
                        LogCategory.Handler,
                        HandlerName,
                        $"Missing localization for cutscene cue '{cue.textKey}'.");
                    _nextCueIndex++;
                    continue;
                }

                ScreenReader.Say(spoken, false);
                DebugLogger.Log(
                    LogCategory.Handler,
                    HandlerName,
                    $"Spoke cutscene cue '{cue.textKey}' at t={elapsed:0.00}s.");

                _nextCueIndex++;
            }
        }

        private static void LoadScripts()
        {
            _scriptsByCutsceneKey.Clear();
            _scriptsLoaded = false;
            _lastLoadError = "";

            string manifestPath = ResolveManifestPath();
            if (string.IsNullOrWhiteSpace(manifestPath))
            {
                SetLoadFailure("Manifest not found.");
                return;
            }
            MelonLogger.Msg($"[{HandlerName}] Using manifest: {manifestPath}");

            if (!CutsceneAdPipeline.TryLoadManifest(manifestPath, out CutsceneAdManifest manifest, out string manifestError))
            {
                SetLoadFailure($"Manifest load failed: {manifestError}");
                return;
            }

            string manifestDir = Path.GetDirectoryName(manifestPath) ?? "";
            if (manifest.cutscenes == null || manifest.cutscenes.Length == 0)
            {
                SetLoadFailure("Manifest has no cutscene entries.");
                return;
            }
            MelonLogger.Msg($"[{HandlerName}] Manifest entries: {manifest.cutscenes.Length}");

            foreach (CutsceneAdCutsceneRef cutsceneRef in manifest.cutscenes)
            {
                if (cutsceneRef == null) continue;
                if (string.IsNullOrWhiteSpace(cutsceneRef.scriptPath)) continue;

                string normalizedScriptPath = cutsceneRef.scriptPath
                    .Replace('/', Path.DirectorySeparatorChar)
                    .Replace('\\', Path.DirectorySeparatorChar);
                string absoluteScriptPath = Path.Combine(manifestDir, normalizedScriptPath);

                if (!CutsceneAdPipeline.TryLoadScript(absoluteScriptPath, out CutsceneAdScript script, out string scriptError))
                {
                    DebugLogger.Log(LogCategory.Handler, HandlerName, $"Script load failed: {scriptError}");
                    continue;
                }

                string scriptScene = !string.IsNullOrWhiteSpace(script.sceneName)
                    ? script.sceneName
                    : cutsceneRef.sceneName;
                string scriptType = !string.IsNullOrWhiteSpace(script.cutsceneType)
                    ? script.cutsceneType
                    : cutsceneRef.cutsceneType;

                string key = BuildCutsceneKey(scriptScene, scriptType);
                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }

                _scriptsByCutsceneKey[key] = script;
            }

            _scriptsLoaded = _scriptsByCutsceneKey.Count > 0;
            if (!_scriptsLoaded)
            {
                SetLoadFailure("Manifest loaded but no valid scripts were available.");
                return;
            }

            _lastReportedLoadError = "";
            MelonLogger.Msg($"[{HandlerName}] Loaded {_scriptsByCutsceneKey.Count} cutscene scripts.");
            DebugLogger.Log(LogCategory.Handler, HandlerName, $"Cutscene AD scripts loaded: {_scriptsByCutsceneKey.Count}.");
        }

        private static void StartCutscene(string cutsceneKey)
        {
            _activeCutsceneKey = cutsceneKey;
            _activeCutsceneStartTime = Time.time;
            _nextCueIndex = 0;

            if (_scriptsByCutsceneKey.TryGetValue(cutsceneKey, out CutsceneAdScript script))
            {
                _activeScript = script;
                DebugLogger.Log(LogCategory.Handler, HandlerName, $"Tracking cutscene: {cutsceneKey}");
            }
            else
            {
                _activeScript = null;
                DebugLogger.Log(LogCategory.Handler, HandlerName, $"No script for cutscene: {cutsceneKey}");
            }
        }

        private static void ResetActiveCutscene()
        {
            _activeCutsceneKey = "";
            _activeScript = null;
            _activeCutsceneStartTime = -1f;
            _nextCueIndex = 0;
        }

        private static string ResolveManifestPath()
        {
            string assemblyDir = Path.GetDirectoryName(typeof(CutsceneAdHandler).Assembly.Location) ?? "";
            string assemblyParent = "";
            if (!string.IsNullOrWhiteSpace(assemblyDir))
            {
                DirectoryInfo parent = Directory.GetParent(assemblyDir);
                assemblyParent = parent != null ? parent.FullName : "";
            }

            string gameRoot = ResolveGameRootDirectory();
            string cwd = Directory.GetCurrentDirectory();
            string appBaseDir = AppDomain.CurrentDomain.BaseDirectory ?? "";

            string[] candidates =
            {
                Path.Combine(assemblyDir, "cutscene-ad", "manifest.json"),
                Path.Combine(assemblyParent, "cutscene-ad", "manifest.json"),
                Path.Combine(gameRoot, "Mods", "cutscene-ad", "manifest.json"),
                Path.Combine(gameRoot, "cutscene-ad", "manifest.json"),
                Path.Combine(appBaseDir, "Mods", "cutscene-ad", "manifest.json"),
                Path.Combine(appBaseDir, "cutscene-ad", "manifest.json"),
                Path.Combine(cwd, "Mods", "cutscene-ad", "manifest.json"),
                Path.Combine(cwd, "cutscene-ad", "manifest.json")
            };

            foreach (string candidate in candidates)
            {
                if (string.IsNullOrWhiteSpace(candidate)) continue;
                if (!File.Exists(candidate)) continue;
                return candidate;
            }

            return "";
        }

        private static void SetLoadFailure(string reason)
        {
            _lastLoadError = reason ?? "Unknown error.";
            if (!string.Equals(_lastReportedLoadError, _lastLoadError, StringComparison.Ordinal))
            {
                _lastReportedLoadError = _lastLoadError;
                MelonLogger.Warning($"[{HandlerName}] {_lastLoadError}");
            }
            DebugLogger.Log(LogCategory.Handler, HandlerName, $"Cutscene AD load failure: {_lastLoadError}");
        }

        private static string GetActiveSceneName()
        {
            if (SceneMonitor.mgr == null) return "";
            string sceneName = SceneMonitor.mgr.GetActiveSceneName();
            return sceneName ?? "";
        }

        private static string ResolveGameRootDirectory()
        {
            string dataPath = Application.dataPath ?? "";
            if (string.IsNullOrWhiteSpace(dataPath))
            {
                return "";
            }

            string normalizedDataPath = dataPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            if (!normalizedDataPath.EndsWith("_Data", StringComparison.OrdinalIgnoreCase))
            {
                return "";
            }

            DirectoryInfo parent = Directory.GetParent(normalizedDataPath);
            return parent != null ? parent.FullName : "";
        }

        private static string BuildCutsceneKey(string sceneName, string cutsceneType)
        {
            if (string.IsNullOrWhiteSpace(sceneName) || string.IsNullOrWhiteSpace(cutsceneType))
            {
                return "";
            }

            return $"{sceneName.Trim()}|{cutsceneType.Trim().ToLowerInvariant()}";
        }
    }
}
