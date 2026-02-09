using HarmonyLib;
using MelonLoader;
using UnityEngine;
using TMPro;

namespace MelatoninAccess
{
    public static class AchievementsHandler
    {
        private const float AchievementAnnouncementCooldown = 0.5f;
        private static float _lastMenuAnnouncementTime = -10f;
        private static int _lastHighlightNum = -1;
        private static string _lastAnnouncement = "";
        private static float _lastAnnouncementTime = -10f;

        [HarmonyPatch(typeof(AchievementsMenu), "Activate")]
        public static class AchievementsMenu_Activate_Patch
        {
            public static void Postfix(AchievementsMenu __instance)
            {
                float now = Time.unscaledTime;
                if (now - _lastMenuAnnouncementTime < AchievementAnnouncementCooldown) return;

                _lastMenuAnnouncementTime = now;
                ScreenReader.Say(Loc.Get("achievements_menu"), true);
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
            if (menu == null) return;

            int highlightNum = Traverse.Create(menu).Field("highlightNum").GetValue<int>();
            CheevoRow[] rows = menu.CheevoRows;

            if (rows != null && highlightNum >= 0 && highlightNum < rows.Length)
            {
                float now = Time.unscaledTime;
                if (highlightNum == _lastHighlightNum && now - _lastAnnouncementTime < AchievementAnnouncementCooldown) return;

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

                    string announcement = title == "?????"
                        ? Loc.Get("locked_achievement", highlightNum + 1, rows.Length)
                        : Loc.Get("achievement_with_desc", title, desc, highlightNum + 1, rows.Length);

                    if (announcement == _lastAnnouncement && now - _lastAnnouncementTime < AchievementAnnouncementCooldown) return;

                    _lastHighlightNum = highlightNum;
                    _lastAnnouncement = announcement;
                    _lastAnnouncementTime = now;
                    ScreenReader.Say(announcement, true);
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
