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
        private static float _lastResetTime = -10f;

        public static void Postfix()
        {
            float now = Time.unscaledTime;
            if (now - _lastResetTime < 0.5f) return;

            _lastResetTime = now;
            DebugLogger.Log(LogCategory.State, "TitleScreen Awake - Resetting Announcement");
            TitleScreen_Update_Patch.ResetAnnouncement();
        }
    }

    [HarmonyPatch(typeof(TitleScreen), "Update")]
    public static class TitleScreen_Update_Patch
    {
        private const float IntroRepeatBlockSeconds = 1.0f;
        private static bool announced = false;
        private static string _lastIntroMessage = "";
        private static float _lastIntroTime = -10f;

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
                message = Loc.Get("intro_fallback", action);
            }

            float now = Time.unscaledTime;
            if (message == _lastIntroMessage && now - _lastIntroTime < IntroRepeatBlockSeconds) yield break;

            _lastIntroMessage = message;
            _lastIntroTime = now;

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
            Loc.RefreshLanguage();
            ScreenReader.Say(Loc.Get("language_selected"), true);
        }
    }

    [HarmonyPatch(typeof(LangMenu), "Activate")]
    public static class LangMenu_Activate_Patch
    {
        private static float _lastActivationTime = -10f;

        public static void Postfix(LangMenu __instance)
        {
            float now = Time.unscaledTime;
            if (now - _lastActivationTime < 0.5f) return;

            _lastActivationTime = now;
            ScreenReader.Say(Loc.Get("language_menu"), true);
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
        private const float LanguageRepeatBlockSeconds = 0.5f;
        private static string _lastAnnouncedLanguage = "";
        private static float _lastAnnouncedLanguageTime = -10f;

        public static void AnnounceSelectedLang(LangMenu menu)
        {
             int highlightNum = Traverse.Create(menu).Field("highlightNum").GetValue<int>();
             if (menu.langs != null && highlightNum >= 0 && highlightNum < menu.langs.Length)
             {
                 var tmp = menu.langs[highlightNum].GetComponent<TextMeshPro>();
                 if (tmp != null && !string.IsNullOrWhiteSpace(tmp.text))
                 {
                     string language = tmp.text.Trim();
                     float now = Time.unscaledTime;
                     if (language == _lastAnnouncedLanguage && now - _lastAnnouncedLanguageTime < LanguageRepeatBlockSeconds) return;

                     _lastAnnouncedLanguage = language;
                     _lastAnnouncedLanguageTime = now;
                     ScreenReader.Say(language, true);
                 }
             }
        }
    }
}
