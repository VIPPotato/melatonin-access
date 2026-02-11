using HarmonyLib;
using MelonLoader;
using UnityEngine;
using TMPro;

namespace MelatoninAccess
{
    // --- Side Label (Persistent Gameplay Text) ---
    [HarmonyPatch(typeof(SideLabel), "ShowAsPractice")]
    public static class SideLabel_ShowAsPractice_Patch
    {
        public static void Postfix(SideLabel __instance)
        {
            SideLabelHelper.AnnounceTutorialStart();
        }
    }

    [HarmonyPatch(typeof(SideLabel), "ActivateAsTutorial")]
    public static class SideLabel_ActivateAsTutorial_Patch
    {
        public static void Postfix(SideLabel __instance)
        {
            SideLabelHelper.AnnounceTutorialStart();
        }
    }

    [HarmonyPatch(typeof(SideLabel), "ShowAsEdited")]
    public static class SideLabel_ShowAsEdited_Patch
    {
        public static void Postfix(SideLabel __instance)
        {
            SideLabelHelper.AnnounceLabel(__instance.skipText);
        }
    }

    public static class SideLabelHelper
    {
        public static void AnnounceTutorialStart()
        {
            if (LevelBriefingHandler.ShouldSuppressPracticePrompt()) return;
            ScreenReader.Say(Loc.Get("tutorial_skip_prompt", GetSkipPrompt()), true);
        }

        public static string GetSkipPromptLabel()
        {
            return GetSkipPrompt();
        }

        public static void AnnounceLabel(textboxFragment fragment)
        {
            if (fragment != null)
            {
                var tmp = fragment.GetComponent<TextMeshPro>();
                if (tmp != null)
                {
                    ScreenReader.Say(tmp.text, true);
                }
            }
        }

        private static string GetSkipPrompt()
        {
            if (ControlHandler.mgr == null) return "Tab";

            int ctrlType = ControlHandler.mgr.GetCtrlType();
            if (ctrlType == 1) return "Y";
            if (ctrlType == 2) return "Triangle";
            return "Tab";
        }
    }
}
