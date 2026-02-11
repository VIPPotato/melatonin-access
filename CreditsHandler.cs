using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using MelonLoader;
using TMPro;
using UnityEngine;

namespace MelatoninAccess
{
    /// <summary>
    /// Narrates credits sections and names while the credits list is scrolling.
    /// </summary>
    public static class CreditsHandler
    {
        [HarmonyPatch(typeof(Credits), "Show")]
        public static class Credits_Show_Patch
        {
            public static void Postfix(Credits __instance)
            {
                CreditsNarrationHelper.Reset();
                if (!ModConfig.AnnounceCreditsRoll) return;
                ScreenReader.Say(Loc.Get("credits_title"), true);
            }
        }

        [HarmonyPatch(typeof(Credits), "TransitionLogoCompanyToCreator")]
        public static class Credits_TransitionLogoCompanyToCreator_Patch
        {
            public static void Postfix(Credits __instance)
            {
                if (!ModConfig.AnnounceCreditsRoll) return;
                MelonCoroutines.Start(CreditsNarrationHelper.AnnounceCreatorDelayed(__instance));
            }
        }

        [HarmonyPatch(typeof(Credits), "ScrollList")]
        public static class Credits_ScrollList_Patch
        {
            public static void Postfix(Credits __instance)
            {
                if (!ModConfig.AnnounceCreditsRoll) return;
                MelonCoroutines.Start(CreditsNarrationHelper.NarrateScrollingEntries(__instance));
            }
        }

        private static class CreditsNarrationHelper
        {
            private static bool _isNarrating;
            private static readonly List<string> _entries = new List<string>();
            private static int _nextEntryIndex;
            private static float _entryInterval = 1f;

            public static void Reset()
            {
                _isNarrating = false;
                _entries.Clear();
                _nextEntryIndex = 0;
                _entryInterval = 1f;
            }

            public static IEnumerator AnnounceCreatorDelayed(Credits credits)
            {
                yield return new WaitForSecondsRealtime(0.72f);
                if (!PrepareEntries(credits)) yield break;
                if (!ShouldNarrationSessionAlive(credits)) yield break;
                if (IsPausedBySubmenu()) yield break;
                if (_nextEntryIndex >= _entries.Count) yield break;

                string entry = _entries[_nextEntryIndex];
                if (!string.IsNullOrWhiteSpace(entry))
                {
                    ScreenReader.Say(entry, false);
                }

                _nextEntryIndex++;
            }

            public static IEnumerator NarrateScrollingEntries(Credits credits)
            {
                if (_isNarrating) yield break;
                if (!PrepareEntries(credits)) yield break;

                _isNarrating = true;
                yield return WaitWithPauseSupport(0.9f, credits);

                if (!ShouldNarrationSessionAlive(credits))
                {
                    ClearProgress();
                    _isNarrating = false;
                    yield break;
                }

                while (_nextEntryIndex < _entries.Count)
                {
                    if (!ShouldNarrationSessionAlive(credits))
                    {
                        ClearProgress();
                        break;
                    }

                    if (IsPausedBySubmenu())
                    {
                        yield return null;
                        continue;
                    }

                    string entry = _entries[_nextEntryIndex];
                    _nextEntryIndex++;
                    if (!string.IsNullOrWhiteSpace(entry))
                    {
                        ScreenReader.Say(entry, false);
                    }

                    if (_nextEntryIndex >= _entries.Count) break;

                    yield return WaitWithPauseSupport(_entryInterval, credits);
                }

                if (_nextEntryIndex >= _entries.Count)
                {
                    ClearProgress();
                }

                _isNarrating = false;
            }

            private static IEnumerator WaitWithPauseSupport(float seconds, Credits credits)
            {
                float elapsed = 0f;
                while (elapsed < seconds)
                {
                    if (!ShouldNarrationSessionAlive(credits)) yield break;

                    if (!IsPausedBySubmenu())
                    {
                        elapsed += Time.unscaledDeltaTime;
                    }

                    yield return null;
                }
            }

            private static bool PrepareEntries(Credits credits)
            {
                if (_entries.Count > 0) return true;

                _entries.Clear();
                _nextEntryIndex = 0;
                _entryInterval = 1f;

                List<string> collectedEntries = CollectEntries(credits);
                if (collectedEntries.Count == 0) return false;

                _entries.AddRange(collectedEntries);
                float scrollDuration = Mathf.Max(credits.GetScrollDuration(), 1f);
                _entryInterval = Mathf.Clamp(scrollDuration / Mathf.Max(_entries.Count, 1), 0.7f, 2f);
                return true;
            }

            private static void ClearProgress()
            {
                _entries.Clear();
                _nextEntryIndex = 0;
                _entryInterval = 1f;
            }

            private static bool ShouldNarrationSessionAlive(Credits credits)
            {
                if (!ModConfig.AnnounceCreditsRoll) return false;
                if (credits == null) return false;
                if (!credits.gameObject.activeInHierarchy) return false;
                return true;
            }

            private static bool IsPausedBySubmenu()
            {
                return Interface.env != null &&
                       Interface.env.Submenu != null &&
                       Interface.env.Submenu.CheckIsActivated();
            }

            private static List<string> CollectEntries(Credits credits)
            {
                var entries = new List<string>();
                if (credits == null || credits.names == null) return entries;

                for (int i = 0; i < credits.names.Length; i++)
                {
                    string entry = BuildEntry(credits, i);
                    if (!string.IsNullOrWhiteSpace(entry))
                    {
                        entries.Add(entry);
                    }
                }

                return entries;
            }

            private static string BuildEntry(Credits credits, int index)
            {
                if (credits == null) return "";

                string label = GetText(credits.labels, index);
                string name = GetText(credits.names, index);

                if (string.IsNullOrWhiteSpace(label)) return name;
                if (string.IsNullOrWhiteSpace(name)) return label;
                return $"{label}. {name}";
            }

            private static string GetText(textboxFragment[] fragments, int index)
            {
                if (fragments == null || index < 0 || index >= fragments.Length) return "";

                textboxFragment fragment = fragments[index];
                if (fragment == null) return "";

                var tmp = fragment.GetComponent<TextMeshPro>();
                if (tmp == null || string.IsNullOrWhiteSpace(tmp.text)) return "";

                return tmp.text.Trim();
            }
        }
    }
}
