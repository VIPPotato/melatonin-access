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
            string action = GetActionPrompt();
            string language = GetLanguagePrompt();
            string message = Loc.Get("intro_welcome", action, language);

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

        private static string GetLanguagePrompt()
        {
            int ctrlType = ControlHandler.mgr.GetCtrlType();
            if (ctrlType == 1) return "Y";
            if (ctrlType == 2) return "Triangle";
            return "Tab";
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
            LangMenuHelper.AnnounceSelectedLang(__instance, includeMenuTitle: true);
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

        public static void AnnounceSelectedLang(LangMenu menu, bool includeMenuTitle = false)
        {
             int highlightNum = Traverse.Create(menu).Field("highlightNum").GetValue<int>();
             if (menu.langs != null && highlightNum >= 0 && highlightNum < menu.langs.Length)
             {
                 var tmp = menu.langs[highlightNum].GetComponent<TextMeshPro>();
                 if (tmp != null && !string.IsNullOrWhiteSpace(tmp.text))
                 {
                      string language = tmp.text.Trim();
                      string languageWithContext = language;
                      if (ModConfig.AnnounceMenuPositions)
                      {
                          string position = Loc.Get("order_of", highlightNum + 1, menu.langs.Length);
                          languageWithContext = $"{language}, {position}";
                      }

                      string menuTitle = GetMenuTitleText(menu);
                      string announcement = includeMenuTitle
                          ? $"{menuTitle}. {languageWithContext}"
                          : languageWithContext;

                      float now = Time.unscaledTime;
                      if (announcement == _lastAnnouncedLanguage && now - _lastAnnouncedLanguageTime < LanguageRepeatBlockSeconds) return;

                      _lastAnnouncedLanguage = announcement;
                       _lastAnnouncedLanguageTime = now;
                       ScreenReader.Say(announcement, true);
                   }
              }
        }

        private static string GetMenuTitleText(LangMenu menu)
        {
            if (menu != null && menu.title != null)
            {
                var titleTmp = menu.title.GetComponent<TextMeshPro>();
                if (titleTmp != null && !string.IsNullOrWhiteSpace(titleTmp.text))
                {
                    return titleTmp.text.Trim();
                }
            }

            return Loc.Get("language_menu");
        }
    }
}
