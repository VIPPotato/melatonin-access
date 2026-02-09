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
                ScreenReader.Say(Loc.Get("credits_title"), true);
            }
        }

        [HarmonyPatch(typeof(Credits), "TransitionLogoCompanyToCreator")]
        public static class Credits_TransitionLogoCompanyToCreator_Patch
        {
            public static void Postfix(Credits __instance)
            {
                MelonCoroutines.Start(CreditsNarrationHelper.AnnounceCreatorDelayed(__instance));
            }
        }

        [HarmonyPatch(typeof(Credits), "ScrollList")]
        public static class Credits_ScrollList_Patch
        {
            public static void Postfix(Credits __instance)
            {
                MelonCoroutines.Start(CreditsNarrationHelper.NarrateScrollingEntries(__instance));
            }
        }

        private static class CreditsNarrationHelper
        {
            private static bool _isNarrating;
            private static readonly HashSet<string> _spokenEntries = new HashSet<string>();

            public static void Reset()
            {
                _isNarrating = false;
                _spokenEntries.Clear();
            }

            public static IEnumerator AnnounceCreatorDelayed(Credits credits)
            {
                yield return new WaitForSecondsRealtime(0.72f);

                string entry = BuildEntry(credits, 0);
                if (string.IsNullOrWhiteSpace(entry)) yield break;

                _spokenEntries.Add(entry);
                ScreenReader.Say(entry, false);
            }

            public static IEnumerator NarrateScrollingEntries(Credits credits)
            {
                if (_isNarrating) yield break;

                _isNarrating = true;
                yield return new WaitForSecondsRealtime(0.9f);

                List<string> entries = CollectEntries(credits);
                if (entries.Count == 0)
                {
                    _isNarrating = false;
                    yield break;
                }

                float scrollDuration = Mathf.Max(credits.GetScrollDuration(), 1f);
                float interval = Mathf.Clamp(scrollDuration / Mathf.Max(entries.Count, 1), 0.7f, 2f);

                foreach (string entry in entries)
                {
                    if (string.IsNullOrWhiteSpace(entry) || _spokenEntries.Contains(entry)) continue;

                    _spokenEntries.Add(entry);
                    ScreenReader.Say(entry, false);
                    yield return new WaitForSecondsRealtime(interval);
                }

                _isNarrating = false;
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
