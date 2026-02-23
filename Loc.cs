using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MelonLoader;
using UnityEngine;

namespace MelatoninAccess
{
    /// <summary>
    /// Centralized localization for all mod-generated screen reader strings.
    /// Language follows the in-game language index from SaveManager.GetLang().
    /// </summary>
    public static class Loc
    {
        private const int LanguageCount = 10;
        private const int EnglishLang = 0;
        private const int LocalizationSchemaVersion = 1;
        private const string LocalizationFolderName = "localization";
        private const string HandlerName = "Loc";

        private static readonly string[] LanguageCodes =
        {
            "en",
            "zh-Hans",
            "zh-Hant",
            "ja",
            "ko",
            "vi",
            "fr",
            "de",
            "es",
            "pt"
        };

        private static readonly Dictionary<string, string>[] _translations = new Dictionary<string, string>[LanguageCount];
        private static bool _initialized;
        private static int _currentLang = EnglishLang;

        [Serializable]
        private sealed class LocalizationFile
        {
            public int schemaVersion = LocalizationSchemaVersion;

            public string language = "";

            public LocalizationEntry[] entries = new LocalizationEntry[0];
        }

        [Serializable]
        private sealed class LocalizationEntry
        {
            public string key = "";

            public string value = "";
        }

        /// <summary>
        /// Initializes translation dictionaries.
        /// </summary>
        public static void Initialize()
        {
            if (_initialized) return;

            for (int i = 0; i < LanguageCount; i++)
            {
                _translations[i] = new Dictionary<string, string>();
            }

            LoadFromJsonFiles();
            RefreshLanguage();
            _initialized = true;
        }

        /// <summary>
        /// Refreshes the current language from game settings.
        /// </summary>
        public static void RefreshLanguage()
        {
            _currentLang = NormalizeLanguageIndex(GetGameLanguageIndex());
        }

        /// <summary>
        /// Gets a localized string by key.
        /// </summary>
        public static string Get(string key)
        {
            if (!_initialized) Initialize();

            int latestLang = NormalizeLanguageIndex(GetGameLanguageIndex());
            if (latestLang != _currentLang)
            {
                _currentLang = latestLang;
            }

            if (_translations[_currentLang].TryGetValue(key, out string value))
            {
                return value;
            }

            if (_translations[EnglishLang].TryGetValue(key, out string english))
            {
                return english;
            }

            return key;
        }

        /// <summary>
        /// Gets a localized string by key and formats it with placeholders.
        /// </summary>
        public static string Get(string key, params object[] args)
        {
            string template = Get(key);
            try
            {
                return string.Format(template, args);
            }
            catch
            {
                return template;
            }
        }

        /// <summary>
        /// Gets a localized level/dream name from the internal dream key.
        /// </summary>
        public static string GetDreamName(string rawName)
        {
            if (string.IsNullOrWhiteSpace(rawName)) return Get("unknown_level");

            string normalized = rawName.Trim().ToLowerInvariant();
            string key = "dream_name_" + normalized;
            string localized = Get(key);
            if (localized != key) return localized;

            if (normalized.Length == 1) return normalized.ToUpperInvariant();
            return char.ToUpperInvariant(normalized[0]) + normalized.Substring(1);
        }

        private static void LoadFromJsonFiles()
        {
            string localizationDir = ResolveLocalizationDirectory();
            if (string.IsNullOrWhiteSpace(localizationDir))
            {
                MelonLogger.Warning($"[{HandlerName}] Localization directory not found.");
                return;
            }

            int totalLoaded = 0;
            for (int langIndex = 0; langIndex < LanguageCodes.Length; langIndex++)
            {
                string languageCode = LanguageCodes[langIndex];
                string filePath = Path.Combine(localizationDir, $"loc.{languageCode}.json");
                int loadedForLanguage = LoadLanguageFile(langIndex, languageCode, filePath);
                totalLoaded += loadedForLanguage;
            }

            MelonLogger.Msg($"[{HandlerName}] Loaded {totalLoaded} localization entries from JSON.");
        }

        private static int LoadLanguageFile(int langIndex, string languageCode, string filePath)
        {
            if (!File.Exists(filePath))
            {
                MelonLogger.Warning($"[{HandlerName}] Missing localization file for {languageCode}: {filePath}");
                return 0;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(json) && json[0] == '\uFEFF')
                {
                    json = json.Substring(1);
                }

                LocalizationFile file = JsonUtility.FromJson<LocalizationFile>(json);

                if (file == null || file.entries == null || file.entries.Length == 0)
                {
                    if (!TryParseLocalizationFallback(json, out LocalizationFile fallbackFile, out string fallbackError))
                    {
                        if (file == null)
                        {
                            MelonLogger.Warning(
                                $"[{HandlerName}] Failed to parse localization JSON: {filePath}. Fallback parser error: {fallbackError}");
                        }
                        else
                        {
                            MelonLogger.Warning(
                                $"[{HandlerName}] No localization entries found in {filePath}. Fallback parser error: {fallbackError}");
                        }

                        return 0;
                    }

                    file = fallbackFile;
                    MelonLogger.Warning($"[{HandlerName}] JsonUtility returned empty localization data for {filePath}. Using fallback parser.");
                }

                if (file.schemaVersion != LocalizationSchemaVersion)
                {
                    MelonLogger.Warning(
                        $"[{HandlerName}] Unexpected schema version in {filePath}: {file.schemaVersion} (expected {LocalizationSchemaVersion}).");
                }

                int loaded = 0;
                Dictionary<string, string> dictionary = _translations[langIndex];
                foreach (LocalizationEntry entry in file.entries)
                {
                    if (entry == null || string.IsNullOrWhiteSpace(entry.key))
                    {
                        continue;
                    }

                    dictionary[entry.key] = entry.value ?? "";
                    loaded++;
                }

                return loaded;
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[{HandlerName}] Failed to load localization file {filePath}: {ex.Message}");
                return 0;
            }
        }

        private static bool TryParseLocalizationFallback(string json, out LocalizationFile file, out string error)
        {
            file = null;
            error = "";

            if (string.IsNullOrWhiteSpace(json))
            {
                error = "Localization JSON is empty.";
                return false;
            }

            MatchCollection matches = Regex.Matches(
                json,
                "\\{\\s*\"key\"\\s*:\\s*\"(?<key>(?:\\\\.|[^\"\\\\])*)\"\\s*,\\s*\"value\"\\s*:\\s*\"(?<value>(?:\\\\.|[^\"\\\\])*)\"\\s*\\}",
                RegexOptions.Singleline);

            if (matches.Count == 0)
            {
                error = "No localization key/value entries were found.";
                return false;
            }

            var entries = new LocalizationEntry[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                string key = DecodeJsonString(matches[i].Groups["key"].Value);
                string value = DecodeJsonString(matches[i].Groups["value"].Value);
                entries[i] = new LocalizationEntry
                {
                    key = key,
                    value = value
                };
            }

            file = new LocalizationFile
            {
                schemaVersion = LocalizationSchemaVersion,
                language = "",
                entries = entries
            };

            return true;
        }

        private static string DecodeJsonString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            try
            {
                return Regex.Unescape(text);
            }
            catch
            {
                return text;
            }
        }

        private static string ResolveLocalizationDirectory()
        {
            string assemblyDir = Path.GetDirectoryName(typeof(Loc).Assembly.Location) ?? "";
            string assemblyParent = "";
            if (!string.IsNullOrWhiteSpace(assemblyDir))
            {
                DirectoryInfo parent = Directory.GetParent(assemblyDir);
                assemblyParent = parent != null ? parent.FullName : "";
            }

            string gameRoot = ResolveGameRootDirectory();
            string cwd = Directory.GetCurrentDirectory();
            string appBaseDir = AppDomain.CurrentDomain.BaseDirectory ?? "";
            string englishFileName = $"loc.{LanguageCodes[EnglishLang]}.json";

            string[] candidates =
            {
                Path.Combine(assemblyDir, LocalizationFolderName),
                Path.Combine(assemblyParent, LocalizationFolderName),
                Path.Combine(gameRoot, "Mods", LocalizationFolderName),
                Path.Combine(gameRoot, LocalizationFolderName),
                Path.Combine(appBaseDir, "Mods", LocalizationFolderName),
                Path.Combine(appBaseDir, LocalizationFolderName),
                Path.Combine(cwd, "Mods", LocalizationFolderName),
                Path.Combine(cwd, LocalizationFolderName)
            };

            foreach (string candidate in candidates)
            {
                if (string.IsNullOrWhiteSpace(candidate)) continue;
                if (!Directory.Exists(candidate)) continue;

                string englishPath = Path.Combine(candidate, englishFileName);
                if (File.Exists(englishPath))
                {
                    return candidate;
                }
            }

            return "";
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

        private static int GetGameLanguageIndex()
        {
            try
            {
                return SaveManager.GetLang();
            }
            catch
            {
                return EnglishLang;
            }
        }

        private static int NormalizeLanguageIndex(int lang)
        {
            return (lang >= 0 && lang < LanguageCount) ? lang : EnglishLang;
        }
    }
}
