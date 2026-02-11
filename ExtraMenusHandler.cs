using HarmonyLib;
using MelonLoader;
using TMPro;
using UnityEngine;
using System.Collections;

namespace MelatoninAccess
{
    public static class ExtraMenuDebounce
    {
        public const float ActivationCooldown = 0.5f;
        public const float PageActionCooldown = 0.35f;
        public const float SelectionCooldown = 0.35f;

        public static float LastCalibrationActivationTime = -10f;
    }

    // --- Calibration Tool ---
    [HarmonyPatch(typeof(CalibrationTool), "Activate")]
    public static class CalibrationTool_Activate_Patch
    {
        public static void Postfix(CalibrationTool __instance)
        {
            float now = Time.unscaledTime;
            if (now - ExtraMenuDebounce.LastCalibrationActivationTime < ExtraMenuDebounce.ActivationCooldown) return;

            ExtraMenuDebounce.LastCalibrationActivationTime = now;
            ScreenReader.Say(Loc.Get("calibration_tool_intro"), true);
            CalibrationHelper.AnnounceCalibration(__instance);
        }
    }

    [HarmonyPatch(typeof(CalibrationTool), "Update")]
    public static class CalibrationTool_Update_Patch
    {
        static string lastText = "";

        public static void Postfix(CalibrationTool __instance)
        {
            bool isActivated = Traverse.Create(__instance).Field("isActivated").GetValue<bool>();
            if (!isActivated) return;

            if (__instance.number != null && __instance.number.CheckIsMeshRendered())
            {
                var tmp = __instance.number.GetComponent<TextMeshPro>();
                if (tmp != null && tmp.text != lastText)
                {
                    lastText = tmp.text;
                    ScreenReader.Say(Loc.Get("calibration_offset", lastText), true);
                }
            }
        }
    }

    [HarmonyPatch(typeof(PingBar), "StopTimer")]
    public static class PingBar_StopTimer_Patch
    {
        public static void Prefix(PingBar __instance)
        {
            if (CalibrationTool.env == null || !CalibrationTool.env.CheckIsActivated()) return;

            float timer = Traverse.Create(__instance).Field("timer").GetValue<float>();
            int deltaMs = Mathf.RoundToInt((timer - 0.11667f) * 1000f);
            int absMs = Mathf.Abs(deltaMs);

            if (absMs <= 5)
            {
                ScreenReader.Say(Loc.Get("calibration_timing_on_time"), true);
                return;
            }

            string key = deltaMs < 0 ? "calibration_timing_early_ms" : "calibration_timing_late_ms";
            ScreenReader.Say(Loc.Get(key, absMs), true);
        }
    }

    public static class CalibrationHelper
    {
        private static string _lastDescription = "";
        private static float _lastDescriptionTime = -10f;

        public static void AnnounceCalibration(CalibrationTool tool)
        {
              if (tool.description != null)
              {
                  var tmp = tool.description.GetComponent<TextMeshPro>();
                 if (tmp != null && !string.IsNullOrWhiteSpace(tmp.text))
                 {
                     string text = tmp.text.Trim();
                     float now = Time.unscaledTime;
                     if (text == _lastDescription && now - _lastDescriptionTime < ExtraMenuDebounce.ActivationCooldown) return;

                     _lastDescription = text;
                     _lastDescriptionTime = now;
                     ScreenReader.Say(text);
                 }
              }
        }
    }

    // --- Community Menu (Downloaded Levels) ---
    [HarmonyPatch(typeof(CommunityMenu), "Activate")]
    public static class CommunityMenu_Activate_Patch
    {
        private static bool isAnnouncing = false;

        public static void Postfix(CommunityMenu __instance)
        {
            if (isAnnouncing) return;
            isAnnouncing = true;

            ScreenReader.Say(Loc.Get("downloaded_levels_loading"), true);
            MelonCoroutines.Start(AnnounceWhenLoaded(__instance));
        }

        private static IEnumerator AnnounceWhenLoaded(CommunityMenu menu)
        {
            // Wait for download to finish
            yield return new WaitUntil(() => !SteamWorkshop.mgr.CheckIsDownloading());
            yield return new WaitForSecondsRealtime(0.2f); // Buffer

            if (menu.CheckIsActivated())
            {
                int totalLevels = Traverse.Create(menu).Field("totalLevels").GetValue<int>();
                int pageNum = Traverse.Create(menu).Field("pageNum").GetValue<int>();
                int pageTotal = Traverse.Create(menu).Field("pageTotal").GetValue<int>();

                string summary = ModConfig.AnnounceMenuPositions
                    ? Loc.Get("downloaded_levels_page_total", totalLevels, pageNum, pageTotal)
                    : Loc.Get("downloaded_levels_total", totalLevels);
                ScreenReader.Say(summary, false);
            }
            
            isAnnouncing = false;
        }
    }

    [HarmonyPatch(typeof(CommunityMenu), "Deactivate")]
    public static class CommunityMenu_Deactivate_Patch
    {
        public static void Postfix()
        {
            // Reset state if needed, though isAnnouncing handles local loop
        }
    }

    [HarmonyPatch(typeof(CommunityMenu), "Descend")]
    public static class CommunityMenu_Descend_Patch
    {
        public static void Postfix(CommunityMenu __instance)
        {
             CommunityMenuHelper.AnnounceSelection(__instance);
        }
    }

    [HarmonyPatch(typeof(CommunityMenu), "Ascend")]
    public static class CommunityMenu_Ascend_Patch
    {
        public static void Postfix(CommunityMenu __instance)
        {
             CommunityMenuHelper.AnnounceSelection(__instance);
        }
    }

    [HarmonyPatch(typeof(CommunityMenu), "NextPage")]
    public static class CommunityMenu_NextPage_Patch
    {
        public static void Postfix(CommunityMenu __instance)
        {
             if (!CommunityMenuHelper.ShouldAnnouncePageAction(Loc.Get("next_page"))) return;

             ScreenReader.Say(Loc.Get("next_page"), true);
             CommunityMenuHelper.AnnouncePage(__instance);
        }
    }

    [HarmonyPatch(typeof(CommunityMenu), "PrevPage")]
    public static class CommunityMenu_PrevPage_Patch
    {
        public static void Postfix(CommunityMenu __instance)
        {
             if (!CommunityMenuHelper.ShouldAnnouncePageAction(Loc.Get("previous_page"))) return;

             ScreenReader.Say(Loc.Get("previous_page"), true);
             CommunityMenuHelper.AnnouncePage(__instance);
        }
    }

    public static class CommunityMenuHelper
    {
        private static string _lastPageAction = "";
        private static float _lastPageActionTime = -10f;
        private static string _lastSelectionText = "";
        private static float _lastSelectionTime = -10f;
        private static int _lastPageNum = -1;
        private static int _lastPageTotal = -1;
        private static float _lastPageAnnounceTime = -10f;

        public static bool ShouldAnnouncePageAction(string action)
        {
            float now = Time.unscaledTime;
            if (action == _lastPageAction && now - _lastPageActionTime < ExtraMenuDebounce.PageActionCooldown) return false;

            _lastPageAction = action;
            _lastPageActionTime = now;
            return true;
        }

        public static void AnnounceSelection(CommunityMenu menu)
        {
              int highlightNum = Traverse.Create(menu).Field("highlightNum").GetValue<int>();
              if (menu.LevelRows != null && highlightNum >= 0 && highlightNum < menu.LevelRows.Length)
              {
                 var row = menu.LevelRows[highlightNum];
                 if (row != null)
                 {
                    int visibleRows = 0;
                    foreach(var r in menu.LevelRows) if(r.gameObject.activeSelf) visibleRows++;

                    string rowText = BuildCommunityRowText(row);
                    string selection = ModConfig.AnnounceMenuPositions
                        ? Loc.Get("item_of", rowText, highlightNum + 1, visibleRows)
                        : rowText;
                    float now = Time.unscaledTime;
                    if (selection == _lastSelectionText && now - _lastSelectionTime < ExtraMenuDebounce.SelectionCooldown) return;

                    _lastSelectionText = selection;
                    _lastSelectionTime = now;
                    ScreenReader.Say(selection, true);
                  }
              }
        }

        public static void AnnouncePage(CommunityMenu menu)
        {
            if (!ModConfig.AnnounceMenuPositions) return;

            int pageNum = Traverse.Create(menu).Field("pageNum").GetValue<int>();
            int pageTotal = Traverse.Create(menu).Field("pageTotal").GetValue<int>();
            float now = Time.unscaledTime;
            if (pageNum == _lastPageNum && pageTotal == _lastPageTotal && now - _lastPageAnnounceTime < ExtraMenuDebounce.PageActionCooldown) return;

            _lastPageNum = pageNum;
            _lastPageTotal = pageTotal;
            _lastPageAnnounceTime = now;
            ScreenReader.Say(Loc.Get("page_of", pageNum, pageTotal), false);
        }

        private static string BuildCommunityRowText(LevelRow row)
        {
            string title = CleanFragmentText(row != null ? row.title : null);
            string subtitle = CleanFragmentText(row != null ? row.subtitle : null);
            string tags = CleanFragmentText(row != null ? row.tags : null);

            if (string.IsNullOrWhiteSpace(title)) title = Loc.Get("unknown_level");
            string combined = title;

            if (!string.IsNullOrWhiteSpace(subtitle) && !string.Equals(subtitle, title, System.StringComparison.OrdinalIgnoreCase))
            {
                combined += ". " + subtitle;
            }

            if (!string.IsNullOrWhiteSpace(tags) &&
                !string.Equals(tags, title, System.StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(tags, subtitle, System.StringComparison.OrdinalIgnoreCase))
            {
                combined += ". " + tags;
            }

            return combined;
        }

        private static string CleanFragmentText(textboxFragment fragment)
        {
            if (fragment == null) return "";

            var tmp = fragment.GetComponent<TextMeshPro>();
            if (tmp == null || string.IsNullOrWhiteSpace(tmp.text)) return "";

            return tmp.text
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Replace("<br>", ", ")
                .Replace("<br/>", ", ")
                .Replace("<BR>", ", ")
                .Trim();
        }
    }
}
