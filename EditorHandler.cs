using HarmonyLib;
using MelonLoader;
using UnityEngine;
using TMPro;
using System.Text;

namespace MelatoninAccess
{
    public static class EditorHandler
    {
        private const float RepeatBlockSeconds = 0.2f;
        private static string _lastAdvancedAnnouncement = "";
        private static float _lastAdvancedAnnouncementTime = -10f;
        private static string _lastTimelineAnnouncement = "";
        private static float _lastTimelineAnnouncementTime = -10f;

        // --- DAW Cursor Movement ---

        [HarmonyPatch(typeof(Daw), "IncreaseBeat")]
        public static class Daw_IncreaseBeat_Patch
        {
            public static void Postfix(Daw __instance) => AnnounceCursor(__instance);
        }

        [HarmonyPatch(typeof(Daw), "DecreaseBeat")]
        public static class Daw_DecreaseBeat_Patch
        {
            public static void Postfix(Daw __instance) => AnnounceCursor(__instance);
        }

        [HarmonyPatch(typeof(Daw), "IncreaseBar")]
        public static class Daw_IncreaseBar_Patch
        {
            public static void Postfix(Daw __instance) => AnnounceCursor(__instance);
        }

        [HarmonyPatch(typeof(Daw), "DecreaseBar")]
        public static class Daw_DecreaseBar_Patch
        {
            public static void Postfix(Daw __instance) => AnnounceCursor(__instance);
        }

        private static void AnnounceCursor(Daw daw)
        {
            int phrase = daw.GetPhraseNum();
            int bar = daw.GetBarNum();
            int beat = daw.GetBeatNum();
            string code = daw.GetCodeOnBeat();

            if (string.IsNullOrEmpty(code))
            {
                ScreenReader.Say(Loc.Get("editor_cursor_empty", phrase, bar, beat), true);
                return;
            }

            ScreenReader.Say(Loc.Get("editor_cursor_content", phrase, bar, beat, code), true);
        }

        // --- Editing ---

        [HarmonyPatch(typeof(Daw), "SetCodeOnBeat")]
        public static class Daw_SetCodeOnBeat_Patch
        {
            public static void Postfix(string codeAdded, char dataType)
            {
                ScreenReader.Say(Loc.Get("editor_placed", codeAdded), true);
            }
        }

        [HarmonyPatch(typeof(Daw), "RemoveCodeOnBeat")]
        public static class Daw_RemoveCodeOnBeat_Patch
        {
            public static void Postfix()
            {
                ScreenReader.Say(Loc.Get("editor_removed"), true);
            }
        }

        // --- Tool Selection (CustomizeMenu) ---

        [HarmonyPatch(typeof(CustomizeMenu), "Activate")]
        public static class CustomizeMenu_Activate_Patch
        {
            public static void Postfix(CustomizeMenu __instance)
            {
                ScreenReader.Say(Loc.Get("editor_tool_select"), true);
                AnnounceTool(__instance);
            }
        }

        [HarmonyPatch(typeof(CustomizeMenu), "HighlightNextColumn")]
        public static class CustomizeMenu_Nav1_Patch { public static void Postfix(CustomizeMenu __instance) => AnnounceTool(__instance); }

        [HarmonyPatch(typeof(CustomizeMenu), "HighlightPrevColumn")]
        public static class CustomizeMenu_Nav2_Patch { public static void Postfix(CustomizeMenu __instance) => AnnounceTool(__instance); }

        [HarmonyPatch(typeof(CustomizeMenu), "HighlightNextRow")]
        public static class CustomizeMenu_Nav3_Patch { public static void Postfix(CustomizeMenu __instance) => AnnounceTool(__instance); }

        [HarmonyPatch(typeof(CustomizeMenu), "HighlightPrevRow")]
        public static class CustomizeMenu_Nav4_Patch { public static void Postfix(CustomizeMenu __instance) => AnnounceTool(__instance); }

        [HarmonyPatch(typeof(CustomizeMenu), "NextList")]
        public static class CustomizeMenu_Nav5_Patch 
        { 
            public static void Postfix(CustomizeMenu __instance) 
            {
                AnnouncePage(__instance);
                AnnounceTool(__instance); 
            } 
        }

        [HarmonyPatch(typeof(CustomizeMenu), "PrevList")]
        public static class CustomizeMenu_Nav6_Patch 
        { 
            public static void Postfix(CustomizeMenu __instance) 
            {
                AnnouncePage(__instance);
                AnnounceTool(__instance); 
            } 
        }

        [HarmonyPatch(typeof(LvlEditor), "Start")]
        public static class LvlEditor_Start_Patch
        {
            public static void Postfix()
            {
                string downloadFilePath = Traverse.Create(typeof(LvlEditor)).Field("downloadFilePath").GetValue<string>();
                if (!string.IsNullOrWhiteSpace(downloadFilePath)) return;
                ScreenReader.Say(Loc.Get("editor_ready"), true);
            }
        }

        // --- Advanced Menu ---

        [HarmonyPatch(typeof(AdvancedMenu), "Activate")]
        public static class AdvancedMenu_Activate_Patch
        {
            public static void Postfix(AdvancedMenu __instance)
            {
                AnnounceAdvancedSelection(__instance, includeTabTitle: true, includeMenuTitle: true, interrupt: true);
            }
        }

        [HarmonyPatch(typeof(AdvancedMenu), "SwapTab")]
        public static class AdvancedMenu_SwapTab_Patch
        {
            public static void Postfix(AdvancedMenu __instance)
            {
                AnnounceAdvancedSelection(__instance, includeTabTitle: true, includeMenuTitle: false, interrupt: true);
            }
        }

        [HarmonyPatch(typeof(AdvancedMenu), "NextRow")]
        public static class AdvancedMenu_NextRow_Patch
        {
            public static void Postfix(AdvancedMenu __instance)
            {
                AnnounceAdvancedSelection(__instance, includeTabTitle: false, includeMenuTitle: false, interrupt: true);
            }
        }

        [HarmonyPatch(typeof(AdvancedMenu), "PrevRow")]
        public static class AdvancedMenu_PrevRow_Patch
        {
            public static void Postfix(AdvancedMenu __instance)
            {
                AnnounceAdvancedSelection(__instance, includeTabTitle: false, includeMenuTitle: false, interrupt: true);
            }
        }

        [HarmonyPatch(typeof(AdvancedMenu), "Increase")]
        public static class AdvancedMenu_Increase_Patch
        {
            public static void Postfix(AdvancedMenu __instance)
            {
                AnnounceAdvancedSelection(__instance, includeTabTitle: false, includeMenuTitle: false, interrupt: true);
            }
        }

        [HarmonyPatch(typeof(AdvancedMenu), "Decrease")]
        public static class AdvancedMenu_Decrease_Patch
        {
            public static void Postfix(AdvancedMenu __instance)
            {
                AnnounceAdvancedSelection(__instance, includeTabTitle: false, includeMenuTitle: false, interrupt: true);
            }
        }

        [HarmonyPatch(typeof(AdvancedMenu), "Increment")]
        public static class AdvancedMenu_Increment_Patch
        {
            public static void Postfix(AdvancedMenu __instance)
            {
                AnnounceAdvancedSelection(__instance, includeTabTitle: false, includeMenuTitle: false, interrupt: true);
            }
        }

        [HarmonyPatch(typeof(AdvancedMenu), "Diminish")]
        public static class AdvancedMenu_Diminish_Patch
        {
            public static void Postfix(AdvancedMenu __instance)
            {
                AnnounceAdvancedSelection(__instance, includeTabTitle: false, includeMenuTitle: false, interrupt: true);
            }
        }

        // --- Timeline Tabs ---

        [HarmonyPatch(typeof(TimelineTabs), "Show")]
        public static class TimelineTabs_Show_Patch
        {
            public static void Postfix(TimelineTabs __instance)
            {
                AnnounceTimelineTab(__instance);
            }
        }

        [HarmonyPatch(typeof(TimelineTabs), "NextTab")]
        public static class TimelineTabs_NextTab_Patch
        {
            public static void Postfix(TimelineTabs __instance)
            {
                AnnounceTimelineTab(__instance);
            }
        }

        [HarmonyPatch(typeof(TimelineTabs), "PrevTab")]
        public static class TimelineTabs_PrevTab_Patch
        {
            public static void Postfix(TimelineTabs __instance)
            {
                AnnounceTimelineTab(__instance);
            }
        }

        private static void AnnounceTool(CustomizeMenu menu)
        {
            var item = menu.GetHighlightedCustomzieItem();
            if (item != null && item.title != null)
            {
                var tmp = item.title.GetComponent<TextMeshPro>();
                if (tmp != null)
                {
                    ScreenReader.Say(tmp.text, true);
                }
            }
        }

        private static void AnnouncePage(CustomizeMenu menu)
        {
            if (menu.title != null)
            {
                var tmp = menu.title.GetComponent<TextMeshPro>();
                if (tmp != null)
                {
                    ScreenReader.Say(Loc.Get("editor_page", tmp.text), true);
                }
            }
        }

        private static void AnnounceAdvancedSelection(AdvancedMenu menu, bool includeTabTitle, bool includeMenuTitle, bool interrupt)
        {
            if (menu == null) return;

            int tab = menu.GetTabNum();
            int row = menu.GetRowNum();

            string rowText = tab == 0
                ? GetFragmentText(menu.labels_music, row)
                : GetFragmentText(menu.labels_share, row);

            string valueText = "";
            if (tab == 0 && row >= 1 && row <= 3)
            {
                valueText = GetFragmentText(menu.numbers, row - 1);
            }

            string announcement = rowText;
            if (!string.IsNullOrWhiteSpace(valueText))
            {
                announcement = string.IsNullOrWhiteSpace(announcement)
                    ? valueText
                    : $"{announcement}. {valueText}";
            }

            if (includeTabTitle)
            {
                string title = GetFragmentText(menu.titles, tab);
                if (!string.IsNullOrWhiteSpace(title))
                {
                    announcement = string.IsNullOrWhiteSpace(announcement)
                        ? title
                        : $"{title}. {announcement}";
                }
            }

            if (includeMenuTitle)
            {
                announcement = string.IsNullOrWhiteSpace(announcement)
                    ? Loc.Get("advanced_menu")
                    : $"{Loc.Get("advanced_menu")}. {announcement}";
            }

            if (string.IsNullOrWhiteSpace(announcement)) return;

            float now = Time.unscaledTime;
            if (announcement == _lastAdvancedAnnouncement && now - _lastAdvancedAnnouncementTime < RepeatBlockSeconds) return;

            _lastAdvancedAnnouncement = announcement;
            _lastAdvancedAnnouncementTime = now;
            ScreenReader.Say(announcement, interrupt);
        }

        private static void AnnounceTimelineTab(TimelineTabs tabs)
        {
            if (tabs == null) return;

            int index = tabs.GetCharType() switch
            {
                'd' => 0,
                'u' => 1,
                'e' => 2,
                't' => 3,
                _ => 0
            };

            string announcement = GetFragmentText(tabs.labels, index);
            if (string.IsNullOrWhiteSpace(announcement)) return;

            float now = Time.unscaledTime;
            if (announcement == _lastTimelineAnnouncement && now - _lastTimelineAnnouncementTime < RepeatBlockSeconds) return;

            _lastTimelineAnnouncement = announcement;
            _lastTimelineAnnouncementTime = now;
            ScreenReader.Say(announcement, true);
        }

        private static string GetFragmentText(textboxFragment[] fragments, int index)
        {
            if (fragments == null || index < 0 || index >= fragments.Length) return "";
            return GetFragmentText(fragments[index]);
        }

        private static string GetFragmentText(textboxFragment fragment)
        {
            if (fragment == null) return "";

            var tmp = fragment.GetComponent<TextMeshPro>();
            return tmp != null && !string.IsNullOrWhiteSpace(tmp.text)
                ? tmp.text.Trim()
                : "";
        }
    }
}
