using HarmonyLib;
using MelonLoader;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

namespace MelatoninAccess
{
    // --- Start Screen & Language Menu ---

    [HarmonyPatch(typeof(TitleScreen), "Awake")]
    public static class TitleScreen_Awake_Patch
    {
        public static void Postfix()
        {
            DebugLogger.Log(LogCategory.State, "TitleScreen Awake - Resetting Announcement");
            TitleScreen_Update_Patch.ResetAnnouncement();
        }
    }

    [HarmonyPatch(typeof(TitleScreen), "Update")]
    public static class TitleScreen_Update_Patch
    {
        private static bool announced = false;

        public static void Postfix(TitleScreen __instance)
        {
            // F1 Force Read
            if (Keyboard.current != null && Keyboard.current.f1Key.wasPressedThisFrame)
            {
                DebugLogger.Log(LogCategory.Input, "F1 pressed - Force reading intro");
                MelonCoroutines.Start(AnnounceDelayed(true));
                return;
            }

            bool isLangUiAvailable = Traverse.Create(__instance).Field("isLangUiAvailable").GetValue<bool>();
            
            if (isLangUiAvailable && !announced)
            {
                bool isLangMenuActivated = false;
                if (TitleWorld.env != null && TitleWorld.env.LangMenu != null)
                {
                    isLangMenuActivated = Traverse.Create(TitleWorld.env.LangMenu).Field("isActivated").GetValue<bool>();
                }

                if (!Interface.env.Submenu.CheckIsActivated() && !isLangMenuActivated)
                {
                    announced = true;
                    DebugLogger.Log(LogCategory.State, "TitleScreen Ready - Starting Announcement");
                    MelonCoroutines.Start(AnnounceDelayed());
                }
            }
        }

        private static IEnumerator AnnounceDelayed(bool force = false)
        {
            if (!force) yield return new WaitForSecondsRealtime(0.5f);
            
            string message = "";
            if (TitleWorld.env != null)
            {
                if (TitleWorld.env.Instruction != null && TitleWorld.env.Instruction.label != null)
                {
                    var startTmp = TitleWorld.env.Instruction.label.GetComponent<TextMeshPro>();
                    if (startTmp != null && !string.IsNullOrEmpty(startTmp.text)) message += startTmp.text + ". ";
                }
                
                if (TitleWorld.env.LangHint != null && TitleWorld.env.LangHint.label != null)
                {
                    var langTmp = TitleWorld.env.LangHint.label.GetComponent<TextMeshPro>();
                    if (langTmp != null && !string.IsNullOrEmpty(langTmp.text)) message += langTmp.text;
                }
            }

            if (string.IsNullOrEmpty(message))
            {
                string action = GetActionPrompt();
                message = $"Melatonin Access Ready. Press {action} to Start. Press Tab or Triangle/Y for Language.";
            }

            DebugLogger.Log(LogCategory.ScreenReader, $"Announcing Intro: {message}");
            ScreenReader.Say(message, true);
        }

        public static void ResetAnnouncement()
        {
            announced = false;
        }

        private static string GetActionPrompt()
        {
            int ctrlType = ControlHandler.mgr.GetCtrlType();
            if (ctrlType == 1) return "A";
            if (ctrlType == 2) return "Cross";
            return "Space";
        }
    }

    [HarmonyPatch(typeof(LangMenu), "Select")]
    public static class LangMenu_Select_Patch
    {
        public static void Postfix()
        {
            TitleScreen_Update_Patch.ResetAnnouncement();
            ScreenReader.Say("Language Selected.", true);
        }
    }

    [HarmonyPatch(typeof(LangMenu), "Activate")]
    public static class LangMenu_Activate_Patch
    {
        public static void Postfix(LangMenu __instance)
        {
            ScreenReader.Say("Language Menu", true);
            LangMenuHelper.AnnounceSelectedLang(__instance);
        }
    }

    [HarmonyPatch(typeof(LangMenu), "Descend")]
    public static class LangMenu_Descend_Patch
    {
        public static void Postfix(LangMenu __instance)
        {
             LangMenuHelper.AnnounceSelectedLang(__instance);
        }
    }

    [HarmonyPatch(typeof(LangMenu), "Ascend")]
    public static class LangMenu_Ascend_Patch
    {
        public static void Postfix(LangMenu __instance)
        {
             LangMenuHelper.AnnounceSelectedLang(__instance);
        }
    }

    public static class LangMenuHelper
    {
        public static void AnnounceSelectedLang(LangMenu menu)
        {
             int highlightNum = Traverse.Create(menu).Field("highlightNum").GetValue<int>();
             if (menu.langs != null && highlightNum >= 0 && highlightNum < menu.langs.Length)
             {
                 var tmp = menu.langs[highlightNum].GetComponent<TextMeshPro>();
                 if (tmp != null)
                 {
                     ScreenReader.Say(tmp.text, true);
                 }
             }
        }
    }
}
