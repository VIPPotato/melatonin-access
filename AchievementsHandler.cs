using HarmonyLib;
using MelonLoader;
using UnityEngine;
using TMPro;

namespace MelatoninAccess
{
    public static class AchievementsHandler
    {
        private const float AchievementAnnouncementCooldown = 0.5f;
        private const float InitialHighlightSuppressSeconds = 1.2f;
        private static float _lastMenuAnnouncementTime = -10f;
        private static int _lastHighlightNum = -1;
        private static string _lastAnnouncement = "";
        private static float _lastAnnouncementTime = -10f;
        private static int _suppressedInitialHighlight = -1;
        private static float _suppressedInitialHighlightUntil = -10f;

        [HarmonyPatch(typeof(AchievementsMenu), "Activate")]
        public static class AchievementsMenu_Activate_Patch
        {
            public static void Postfix(AchievementsMenu __instance)
            {
                float now = Time.unscaledTime;
                if (now - _lastMenuAnnouncementTime < AchievementAnnouncementCooldown) return;

                _lastMenuAnnouncementTime = now;
                if (TryBuildHighlightAnnouncement(__instance, out string highlightAnnouncement, out int highlightNum))
                {
                    _lastHighlightNum = highlightNum;
                    _lastAnnouncement = highlightAnnouncement;
                    _lastAnnouncementTime = now;
                    _suppressedInitialHighlight = highlightNum;
                    _suppressedInitialHighlightUntil = now + InitialHighlightSuppressSeconds;
                    ScreenReader.Say($"{Loc.Get("achievements_menu")}. {highlightAnnouncement}", true);
                    return;
                }

                ScreenReader.Say(Loc.Get("achievements_menu"), true);
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

            if (TryBuildHighlightAnnouncement(menu, out string announcement, out int highlightNum))
            {
                float now = Time.unscaledTime;
                if (highlightNum == _suppressedInitialHighlight && now <= _suppressedInitialHighlightUntil) return;

                if (now > _suppressedInitialHighlightUntil)
                {
                    _suppressedInitialHighlight = -1;
                    _suppressedInitialHighlightUntil = -10f;
                }

                if (highlightNum == _lastHighlightNum && now - _lastAnnouncementTime < AchievementAnnouncementCooldown) return;
                if (announcement == _lastAnnouncement && now - _lastAnnouncementTime < AchievementAnnouncementCooldown) return;

                _lastHighlightNum = highlightNum;
                _lastAnnouncement = announcement;
                _lastAnnouncementTime = now;
                ScreenReader.Say(announcement, true);
            }
        }

        private static bool TryBuildHighlightAnnouncement(AchievementsMenu menu, out string announcement, out int highlightNum)
        {
            announcement = "";
            highlightNum = -1;
            if (menu == null) return false;

            highlightNum = Traverse.Create(menu).Field("highlightNum").GetValue<int>();
            CheevoRow[] rows = menu.CheevoRows;
            if (rows == null || highlightNum < 0 || highlightNum >= rows.Length) return false;

            var row = rows[highlightNum];
            if (row == null) return false;

            string title = GetText(row.title);
            string desc = GetText(row.description);

            bool includePosition = ModConfig.AnnounceMenuPositions;
            announcement = title == "?????"
                ? (includePosition ? Loc.Get("locked_achievement", highlightNum + 1, rows.Length) : Loc.Get("locked_achievement_plain"))
                : (includePosition ? Loc.Get("achievement_with_desc", title, desc, highlightNum + 1, rows.Length) : Loc.Get("achievement_with_desc_plain", title, desc));

            return !string.IsNullOrWhiteSpace(announcement);
        }

        private static string GetText(textboxFragment fragment)
        {
            if (fragment == null) return "";
            var tmp = fragment.GetComponent<TextMeshPro>();
            return tmp != null ? tmp.text : "";
        }
    }
}
