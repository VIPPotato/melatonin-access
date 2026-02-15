using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MelatoninAccess
{
    [Serializable]
    public sealed class CutsceneAdManifest
    {
        public int schemaVersion = 1;

        public CutsceneAdCutsceneRef[] cutscenes = Array.Empty<CutsceneAdCutsceneRef>();
    }

    [Serializable]
    public sealed class CutsceneAdCutsceneRef
    {
        public string id = "";

        public string sceneName = "";

        public string cutsceneType = "";

        public string scriptPath = "";

        public string sourceRef = "";
    }

    [Serializable]
    public sealed class CutsceneAdScript
    {
        public int schemaVersion = 1;

        public string id = "";

        public string sceneName = "";

        public string cutsceneType = "";

        public CutsceneAdCue[] entries = Array.Empty<CutsceneAdCue>();
    }

    [Serializable]
    public sealed class CutsceneAdCue
    {
        public float atSeconds = 0f;

        public float durationSeconds = 0f;

        public string textKey = "";
    }

    internal static class CutsceneAdPipeline
    {
        public static bool TryLoadManifest(string manifestPath, out CutsceneAdManifest manifest, out string error)
        {
            manifest = null;
            error = "";

            if (string.IsNullOrWhiteSpace(manifestPath))
            {
                error = "Manifest path is empty.";
                return false;
            }

            if (!File.Exists(manifestPath))
            {
                error = $"Manifest not found: {manifestPath}";
                return false;
            }

            try
            {
                string json = File.ReadAllText(manifestPath);
                manifest = Deserialize<CutsceneAdManifest>(json);
                if (manifest == null)
                {
                    error = "Manifest JSON could not be parsed.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = $"Manifest load error: {ex.Message}";
                return false;
            }

            return ValidateManifest(manifest, out error);
        }

        public static bool TryLoadScript(string scriptPath, out CutsceneAdScript script, out string error)
        {
            script = null;
            error = "";

            if (string.IsNullOrWhiteSpace(scriptPath))
            {
                error = "Script path is empty.";
                return false;
            }

            if (!File.Exists(scriptPath))
            {
                error = $"Script not found: {scriptPath}";
                return false;
            }

            try
            {
                string json = File.ReadAllText(scriptPath);
                script = Deserialize<CutsceneAdScript>(json);
                if (script == null)
                {
                    error = "Script JSON could not be parsed.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = $"Script load error: {ex.Message}";
                return false;
            }

            return ValidateScript(script, out error);
        }

        public static bool ValidateManifest(CutsceneAdManifest manifest, out string error)
        {
            error = "";
            if (manifest == null)
            {
                error = "Manifest is null.";
                return false;
            }

            if (manifest.cutscenes == null)
            {
                error = "Manifest cutscenes array is null.";
                return false;
            }

            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < manifest.cutscenes.Length; i++)
            {
                CutsceneAdCutsceneRef cutscene = manifest.cutscenes[i];
                if (cutscene == null)
                {
                    error = $"Manifest cutscene at index {i} is null.";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(cutscene.id))
                {
                    error = $"Manifest cutscene at index {i} has empty id.";
                    return false;
                }

                if (!seenIds.Add(cutscene.id))
                {
                    error = $"Manifest has duplicate cutscene id: {cutscene.id}";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(cutscene.scriptPath))
                {
                    error = $"Manifest cutscene '{cutscene.id}' has empty scriptPath.";
                    return false;
                }
            }

            return true;
        }

        public static bool ValidateScript(CutsceneAdScript script, out string error)
        {
            error = "";
            if (script == null)
            {
                error = "Script is null.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(script.id))
            {
                error = "Script id is empty.";
                return false;
            }

            if (script.entries == null)
            {
                error = $"Script '{script.id}' has null entries array.";
                return false;
            }

            float previousStart = -1f;
            float previousEnd = -1f;

            for (int i = 0; i < script.entries.Length; i++)
            {
                CutsceneAdCue cue = script.entries[i];
                if (cue == null)
                {
                    error = $"Script '{script.id}' has null entry at index {i}.";
                    return false;
                }

                if (cue.atSeconds < 0f)
                {
                    error = $"Script '{script.id}' entry {i} has negative atSeconds.";
                    return false;
                }

                if (cue.durationSeconds <= 0f)
                {
                    error = $"Script '{script.id}' entry {i} must have positive durationSeconds.";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(cue.textKey))
                {
                    error = $"Script '{script.id}' entry {i} has empty textKey.";
                    return false;
                }

                if (previousStart >= 0f && cue.atSeconds < previousStart)
                {
                    error = $"Script '{script.id}' entry {i} is out of order (atSeconds must be ascending).";
                    return false;
                }

                if (previousEnd >= 0f && cue.atSeconds < previousEnd)
                {
                    error = $"Script '{script.id}' entry {i} overlaps previous cue window.";
                    return false;
                }

                previousStart = cue.atSeconds;
                previousEnd = cue.atSeconds + cue.durationSeconds;
            }

            return true;
        }

        private static T Deserialize<T>(string json) where T : class
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            return JsonUtility.FromJson<T>(json);
        }
    }
}
