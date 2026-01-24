using HarmonyLib;
using MelonLoader;
using UnityEngine;
using TMPro;

namespace MelatoninAccess
{
    public static class AchievementsHandler
    {
        [HarmonyPatch(typeof(AchievementsMenu), "Activate")]
        public static class AchievementsMenu_Activate_Patch
        {
            public static void Postfix(AchievementsMenu __instance)
            {
                ScreenReader.Say("Achievements Menu", true);
                AnnounceHighlight(__instance);
            }
        }

        [HarmonyPatch(typeof(AchievementsMenu), "Descend")]
        public static class AchievementsMenu_Descend_Patch
        {
            public static void Postfix(AchievementsMenu __instance)
            {
                AnnounceHighlight(__instance);
            }
        }

        [HarmonyPatch(typeof(AchievementsMenu), "Ascend")]
        public static class AchievementsMenu_Ascend_Patch
        {
            public static void Postfix(AchievementsMenu __instance)
            {
                AnnounceHighlight(__instance);
            }
        }

        private static void AnnounceHighlight(AchievementsMenu menu)
        {
            int highlightNum = Traverse.Create(menu).Field("highlightNum").GetValue<int>();
            CheevoRow[] rows = menu.CheevoRows;

            if (rows != null && highlightNum >= 0 && highlightNum < rows.Length)
            {
                var row = rows[highlightNum];
                if (row != null)
                {
                    string title = GetText(row.title);
                    string desc = GetText(row.description);
                    
                    // Check if completed (checkmark state)
                    if (row.checkmark != null)
                    {
                        // Check logic here if needed, but title "?????" seems sufficient for now.
                    }

                    if (title == "?????")
                    {
                         ScreenReader.Say($"Locked Achievement. {highlightNum + 1} of {rows.Length}", true);
                    }
                    else
                    {
                         ScreenReader.Say($"{title}: {desc}. {highlightNum + 1} of {rows.Length}", true);
                    }
                }
            }
        }

        private static string GetText(textboxFragment fragment)
        {
            if (fragment == null) return "";
            var tmp = fragment.GetComponent<TextMeshPro>();
            return tmp != null ? tmp.text : "";
        }
    }
}
