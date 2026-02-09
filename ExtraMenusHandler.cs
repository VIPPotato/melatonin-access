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
            ScreenReader.Say("Calibration Tool. Adjust offset to match the beat.", true);
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
                    ScreenReader.Say($"Offset {lastText}", true);
                }
            }
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

            ScreenReader.Say("Downloaded Levels. Loading...", true);
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
                
                ScreenReader.Say($"{totalLevels} levels total. Page {pageNum} of {pageTotal}", false);
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
             if (!CommunityMenuHelper.ShouldAnnouncePageAction("Next Page")) return;

             ScreenReader.Say("Next Page", true);
             CommunityMenuHelper.AnnouncePage(__instance);
        }
    }

    [HarmonyPatch(typeof(CommunityMenu), "PrevPage")]
    public static class CommunityMenu_PrevPage_Patch
    {
        public static void Postfix(CommunityMenu __instance)
        {
             if (!CommunityMenuHelper.ShouldAnnouncePageAction("Previous Page")) return;

             ScreenReader.Say("Previous Page", true);
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
                    var tmp = row.GetComponentInChildren<TextMeshPro>();
                    if (tmp != null)
                    {
                        int visibleRows = 0;
                        foreach(var r in menu.LevelRows) if(r.gameObject.activeSelf) visibleRows++;

                        string selection = $"{tmp.text}, item {highlightNum + 1} of {visibleRows}";
                        float now = Time.unscaledTime;
                        if (selection == _lastSelectionText && now - _lastSelectionTime < ExtraMenuDebounce.SelectionCooldown) return;

                        _lastSelectionText = selection;
                        _lastSelectionTime = now;
                        ScreenReader.Say(selection, true);
                     }
                  }
              }
        }

        public static void AnnouncePage(CommunityMenu menu)
        {
            int pageNum = Traverse.Create(menu).Field("pageNum").GetValue<int>();
            int pageTotal = Traverse.Create(menu).Field("pageTotal").GetValue<int>();
            float now = Time.unscaledTime;
            if (pageNum == _lastPageNum && pageTotal == _lastPageTotal && now - _lastPageAnnounceTime < ExtraMenuDebounce.PageActionCooldown) return;

            _lastPageNum = pageNum;
            _lastPageTotal = pageTotal;
            _lastPageAnnounceTime = now;
            ScreenReader.Say($"Page {pageNum} of {pageTotal}", false);
        }
    }
}
