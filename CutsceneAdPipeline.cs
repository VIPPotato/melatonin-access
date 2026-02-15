using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MelatoninAccess
{
    [Serializable]
    public sealed class CutsceneAdManifest
    {
        public int schemaVersion = 1;

        public CutsceneAdCutsceneRef[] cutscenes = new CutsceneAdCutsceneRef[0];
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

        public CutsceneAdCue[] entries = new CutsceneAdCue[0];
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
                if (manifest == null || manifest.cutscenes == null || manifest.cutscenes.Length == 0)
                {
                    if (!TryParseManifestFallback(json, out CutsceneAdManifest fallbackManifest, out string fallbackError))
                    {
                        if (manifest == null)
                        {
                            error = $"Manifest JSON could not be parsed. Fallback parser error: {fallbackError}";
                            return false;
                        }
                    }
                    else
                    {
                        manifest = fallbackManifest;
                    }
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
                if (script == null || script.entries == null || script.entries.Length == 0)
                {
                    if (!TryParseScriptFallback(json, out CutsceneAdScript fallbackScript, out string fallbackError))
                    {
                        if (script == null)
                        {
                            error = $"Script JSON could not be parsed. Fallback parser error: {fallbackError}";
                            return false;
                        }
                    }
                    else
                    {
                        script = fallbackScript;
                    }
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

            if (manifest.cutscenes.Length == 0)
            {
                error = "Manifest cutscenes array is empty.";
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

            if (script.entries.Length == 0)
            {
                error = $"Script '{script.id}' has empty entries array.";
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

        private static bool TryParseManifestFallback(string json, out CutsceneAdManifest manifest, out string error)
        {
            manifest = null;
            error = "";

            if (!TryExtractArrayValue(json, "cutscenes", out string cutscenesArray, out error))
            {
                return false;
            }

            List<string> objectJsons = ExtractObjectArrayItems(cutscenesArray);
            var refs = new List<CutsceneAdCutsceneRef>();
            for (int i = 0; i < objectJsons.Count; i++)
            {
                string objectJson = objectJsons[i];
                string id = ReadStringProperty(objectJson, "id");
                string scriptPath = ReadStringProperty(objectJson, "scriptPath");
                if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(scriptPath))
                {
                    continue;
                }

                refs.Add(new CutsceneAdCutsceneRef
                {
                    id = id,
                    sceneName = ReadStringProperty(objectJson, "sceneName"),
                    cutsceneType = ReadStringProperty(objectJson, "cutsceneType"),
                    scriptPath = scriptPath,
                    sourceRef = ReadStringProperty(objectJson, "sourceRef")
                });
            }

            manifest = new CutsceneAdManifest();
            manifest.cutscenes = refs.ToArray();
            return true;
        }

        private static bool TryParseScriptFallback(string json, out CutsceneAdScript script, out string error)
        {
            script = null;
            error = "";

            if (!TryExtractArrayValue(json, "entries", out string entriesArray, out error))
            {
                return false;
            }

            List<string> entryObjects = ExtractObjectArrayItems(entriesArray);
            var entries = new List<CutsceneAdCue>();
            for (int i = 0; i < entryObjects.Count; i++)
            {
                string entryJson = entryObjects[i];
                if (!TryReadFloatProperty(entryJson, "atSeconds", out float atSeconds))
                {
                    continue;
                }

                if (!TryReadFloatProperty(entryJson, "durationSeconds", out float durationSeconds))
                {
                    continue;
                }

                string textKey = ReadStringProperty(entryJson, "textKey");
                if (string.IsNullOrWhiteSpace(textKey))
                {
                    continue;
                }

                entries.Add(new CutsceneAdCue
                {
                    atSeconds = atSeconds,
                    durationSeconds = durationSeconds,
                    textKey = textKey
                });
            }

            script = new CutsceneAdScript
            {
                id = ReadStringProperty(json, "id"),
                sceneName = ReadStringProperty(json, "sceneName"),
                cutsceneType = ReadStringProperty(json, "cutsceneType"),
                entries = entries.ToArray()
            };
            return true;
        }

        private static bool TryExtractArrayValue(string json, string propertyName, out string arrayBody, out string error)
        {
            arrayBody = "";
            error = "";

            if (string.IsNullOrWhiteSpace(json))
            {
                error = "JSON content is empty.";
                return false;
            }

            int propertyIndex = json.IndexOf($"\"{propertyName}\"", StringComparison.Ordinal);
            if (propertyIndex < 0)
            {
                error = $"Property '{propertyName}' not found.";
                return false;
            }

            int colonIndex = json.IndexOf(':', propertyIndex);
            if (colonIndex < 0)
            {
                error = $"Property '{propertyName}' has no value separator.";
                return false;
            }

            int valueStart = SkipWhitespace(json, colonIndex + 1);
            if (valueStart < 0 || valueStart >= json.Length || json[valueStart] != '[')
            {
                error = $"Property '{propertyName}' is not an array.";
                return false;
            }

            if (!TryFindMatchingBracket(json, valueStart, '[', ']', out int valueEnd))
            {
                error = $"Property '{propertyName}' array is not closed.";
                return false;
            }

            arrayBody = json.Substring(valueStart + 1, valueEnd - valueStart - 1);
            return true;
        }

        private static List<string> ExtractObjectArrayItems(string arrayBody)
        {
            var items = new List<string>();
            if (string.IsNullOrWhiteSpace(arrayBody))
            {
                return items;
            }

            int index = 0;
            while (index < arrayBody.Length)
            {
                index = SkipWhitespace(arrayBody, index);
                if (index < 0 || index >= arrayBody.Length)
                {
                    break;
                }

                if (arrayBody[index] == ',')
                {
                    index++;
                    continue;
                }

                if (arrayBody[index] != '{')
                {
                    index++;
                    continue;
                }

                if (!TryFindMatchingBracket(arrayBody, index, '{', '}', out int endIndex))
                {
                    break;
                }

                items.Add(arrayBody.Substring(index, endIndex - index + 1));
                index = endIndex + 1;
            }

            return items;
        }

        private static bool TryFindMatchingBracket(string text, int startIndex, char openChar, char closeChar, out int endIndex)
        {
            endIndex = -1;
            if (string.IsNullOrEmpty(text) || startIndex < 0 || startIndex >= text.Length || text[startIndex] != openChar)
            {
                return false;
            }

            int depth = 0;
            bool inString = false;
            bool escaping = false;

            for (int i = startIndex; i < text.Length; i++)
            {
                char ch = text[i];
                if (inString)
                {
                    if (escaping)
                    {
                        escaping = false;
                        continue;
                    }

                    if (ch == '\\')
                    {
                        escaping = true;
                        continue;
                    }

                    if (ch == '"')
                    {
                        inString = false;
                    }

                    continue;
                }

                if (ch == '"')
                {
                    inString = true;
                    continue;
                }

                if (ch == openChar)
                {
                    depth++;
                    continue;
                }

                if (ch == closeChar)
                {
                    depth--;
                    if (depth == 0)
                    {
                        endIndex = i;
                        return true;
                    }
                }
            }

            return false;
        }

        private static int SkipWhitespace(string text, int index)
        {
            if (string.IsNullOrEmpty(text))
            {
                return -1;
            }

            int i = index;
            while (i < text.Length && char.IsWhiteSpace(text[i]))
            {
                i++;
            }

            return i;
        }

        private static string ReadStringProperty(string json, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(json) || string.IsNullOrWhiteSpace(propertyName))
            {
                return "";
            }

            string pattern = "\""
                + Regex.Escape(propertyName)
                + "\"\\s*:\\s*\"(?<value>(?:\\\\.|[^\"\\\\])*)\"";
            Match match = Regex.Match(json, pattern);
            if (!match.Success)
            {
                return "";
            }

            string raw = match.Groups["value"].Value;
            return Regex.Unescape(raw);
        }

        private static bool TryReadFloatProperty(string json, string propertyName, out float value)
        {
            value = 0f;
            if (string.IsNullOrWhiteSpace(json) || string.IsNullOrWhiteSpace(propertyName))
            {
                return false;
            }

            string pattern = "\""
                + Regex.Escape(propertyName)
                + "\"\\s*:\\s*(?<value>-?\\d+(?:\\.\\d+)?(?:[eE][+-]?\\d+)?)";
            Match match = Regex.Match(json, pattern);
            if (!match.Success)
            {
                return false;
            }

            return float.TryParse(
                match.Groups["value"].Value,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out value);
        }
    }
}
