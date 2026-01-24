using HarmonyLib;
using MelonLoader;
using UnityEngine;
using TMPro;

namespace MelatoninAccess
{
    // --- Extra Message (Popup/Hint) ---
    [HarmonyPatch(typeof(ExtraMessage), "Activate")]
    public static class ExtraMessage_Activate_Patch
    {
        public static void Postfix(ExtraMessage __instance)
        {
            if (__instance.message != null)
            {
                var tmp = __instance.message.GetComponent<TextMeshPro>();
                if (tmp != null)
                {
                    ScreenReader.Say(tmp.text, true);
                }
            }
        }
    }

    // --- Confirm Modal (Quit Game etc) ---
    [HarmonyPatch(typeof(ConfirmModal), "Activate")]
    public static class ConfirmModal_Activate_Patch
    {
        public static void Postfix(ConfirmModal __instance)
        {
            // Texts: 0=Title? 1=Yes? 2=No?
            // Let's read all of them
            string message = "";
            if (__instance.texts != null)
            {
                foreach(var txt in __instance.texts)
                {
                    var tmp = txt.GetComponent<TextMeshPro>();
                    if (tmp != null && !string.IsNullOrEmpty(tmp.text))
                    {
                        message += tmp.text + " ";
                    }
                }
            }
            ScreenReader.Say(message, true);
        }
    }

    // --- Rebind Modal ---
    [HarmonyPatch(typeof(RebindModal), "Show")]
    public static class RebindModal_Show_Patch
    {
        public static void Postfix(RebindModal __instance)
        {
            if (__instance.message != null)
            {
                var tmp = __instance.message.GetComponent<TextMeshPro>();
                if (tmp != null) ScreenReader.Say(tmp.text, true);
            }
        }
    }
}
