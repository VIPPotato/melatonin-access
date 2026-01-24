using HarmonyLib;
using MelonLoader;
using TMPro;
using UnityEngine;

namespace MelatoninAccess
{
    // --- Dialogue ---
    [HarmonyPatch(typeof(DialogBox), "SetText")]
    public static class DialogBox_SetText_Patch
    {
        public static void Postfix(DialogBox __instance, string newText)
        {
            if (!string.IsNullOrEmpty(newText))
            {
                DebugLogger.Log(LogCategory.Handler, "DialogBox_SetText", newText);
                ScreenReader.Say(newText, true);
            }
        }
    }

    [HarmonyPatch(typeof(DialogBox), "ChangeDialogState")]
    public static class DialogBox_ChangeDialogState_Patch
    {
        public static void Postfix(DialogBox __instance)
        {
            DebugLogger.Log(LogCategory.Handler, "DialogBox_ChangeDialogState", "Triggered");
            DialogHelper.ReadDialog(__instance);
        }
    }

    [HarmonyPatch(typeof(DialogBox), "SetDialogState")]
    public static class DialogBox_SetDialogState_Patch
    {
        public static void Postfix(DialogBox __instance)
        {
            DebugLogger.Log(LogCategory.Handler, "DialogBox_SetDialogState", "Triggered");
            DialogHelper.ReadDialog(__instance);
        }
    }

    public static class DialogHelper
    {
        public static void ReadDialog(DialogBox box)
        {
            if (box.dialog != null)
            {
                var tmp = box.dialog.GetComponent<TextMeshPro>();
                if (tmp != null)
                {
                    DebugLogger.Log(LogCategory.Game, "DialogBox Text", tmp.text);
                    ScreenReader.Say(tmp.text, true);
                }
            }
        }
    }
}