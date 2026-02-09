using HarmonyLib;
using MelonLoader;
using UnityEngine;
using TMPro;
using System.Text;

namespace MelatoninAccess
{
    public static class EditorHandler
    {
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
                ScreenReader.Say(Loc.Get("editor_ready"), true);
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
    }
}
