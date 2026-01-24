using HarmonyLib;
using MelonLoader;
using TMPro;
using UnityEngine;
using System.Collections;

namespace MelatoninAccess
{
    // --- Tutorial & Dialogues ---
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

    // --- Results & Scores ---
    [HarmonyPatch(typeof(Results), "Activate")]
    public static class Results_Activate_Patch
    {
        public static void Postfix(Results __instance)
        {
            ScreenReader.Say("Stage Complete", true);
        }
    }

    [HarmonyPatch(typeof(ScoreMessage), "Show")]
    public static class ScoreMessage_Show_Patch
    {
        public static void Postfix(ScoreMessage __instance)
        {
            MelonCoroutines.Start(ReadScoreDelayed(__instance));
        }

        private static IEnumerator ReadScoreDelayed(ScoreMessage msg)
        {
            yield return new WaitForSecondsRealtime(0.2f); 
            
            string message = "";
            if (msg.title != null)
            {
                var tmp = msg.title.GetComponent<TextMeshPro>();
                if (tmp != null) message += tmp.text + ". ";
            }
            if (msg.subtitle != null && msg.subtitle.CheckIsMeshRendered())
            {
                var tmp = msg.subtitle.GetComponent<TextMeshPro>();
                if (tmp != null) message += tmp.text;
            }
            ScreenReader.Say(message, false);
        }
    }

    // --- Stage End Menu (Replay/Next) ---
    
    [HarmonyPatch(typeof(StageEndMenu), "Show")]
    public static class StageEndMenu_Show_Patch
    {
        public static void Postfix(StageEndMenu __instance)
        {
            ScreenReader.Say("Stage Menu", true);
            StageEndMenuHelper.AnnounceSelection(__instance);
        }
    }

    [HarmonyPatch(typeof(StageEndMenu), "Next")]
    public static class StageEndMenu_Next_Patch
    {
        public static void Postfix(StageEndMenu __instance)
        {
            StageEndMenuHelper.AnnounceSelection(__instance);
        }
    }

    [HarmonyPatch(typeof(StageEndMenu), "Prev")]
    public static class StageEndMenu_Prev_Patch
    {
        public static void Postfix(StageEndMenu __instance)
        {
            StageEndMenuHelper.AnnounceSelection(__instance);
        }
    }

    public static class StageEndMenuHelper
    {
        public static void AnnounceSelection(StageEndMenu menu)
        {
            int highlightPosition = Traverse.Create(menu).Field("highlightPosition").GetValue<int>();
            int activeOptionsCount = Traverse.Create(menu).Field("activeOptionsCount").GetValue<int>();
            
            if (menu.labels != null && highlightPosition >= 0 && highlightPosition < menu.labels.Length)
            {
                var label = menu.labels[highlightPosition];
                var tmp = label.GetComponent<TextMeshPro>();
                if (tmp != null)
                {
                    string text = tmp.text;
                    if (activeOptionsCount > 0)
                    {
                        text += $", {highlightPosition + 1} of {activeOptionsCount}";
                    }
                    ScreenReader.Say(text, true);
                }
            }
        }
    }
}
