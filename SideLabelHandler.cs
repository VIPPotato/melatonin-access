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
            SideLabelHelper.AnnounceLabel(__instance.practiceText);
        }
    }

    [HarmonyPatch(typeof(SideLabel), "ActivateAsTutorial")]
    public static class SideLabel_ActivateAsTutorial_Patch
    {
        public static void Postfix(SideLabel __instance)
        {
            SideLabelHelper.AnnounceLabel(__instance.practiceText);
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
    }
}
