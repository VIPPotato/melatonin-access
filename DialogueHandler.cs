using HarmonyLib;
using MelonLoader;
using TMPro;
using UnityEngine;
using System.Collections;

namespace MelatoninAccess
{
    // --- Dialogue ---
    [HarmonyPatch(typeof(DialogBox), "SetText")]
    public static class DialogBox_SetText_Patch
    {
        public static void Postfix(DialogBox __instance, string newText)
        {
            DialogHelper.ReadDialogText(newText);
        }
    }

    [HarmonyPatch(typeof(DialogBox), "ChangeDialogState")]
    public static class DialogBox_ChangeDialogState_Patch
    {
        public static void Postfix(DialogBox __instance)
        {
            MelonCoroutines.Start(DialogHelper.ReadDialogDelayed(__instance, 0.2f));
        }
    }

    [HarmonyPatch(typeof(DialogBox), "SetDialogState")]
    public static class DialogBox_SetDialogState_Patch
    {
        public static void Postfix(DialogBox __instance)
        {
            MelonCoroutines.Start(DialogHelper.ReadDialogDelayed(__instance, 0.12f));
        }
    }

    [HarmonyPatch(typeof(DialogBox), "ChangeToGraphic")]
    public static class DialogBox_ChangeToGraphic_Patch
    {
        public static void Postfix(DialogBox __instance, int graphicNum, int newStateLeft, int newStateRight)
        {
            DialogHelper.ReadGraphicDialog(__instance);
        }
    }

    [HarmonyPatch(typeof(DialogBox), "Show")]
    public static class DialogBox_Show_Patch
    {
        public static void Postfix(DialogBox __instance)
        {
            MelonCoroutines.Start(DialogHelper.ReadDialogDelayed(__instance, 0.12f));
        }
    }

    [HarmonyPatch(typeof(DialogBox), "Activate")]
    public static class DialogBox_Activate_Patch
    {
        public static void Postfix(DialogBox __instance)
        {
            MelonCoroutines.Start(DialogHelper.ReadDialogDelayed(__instance, 0.08f));
        }
    }

    [HarmonyPatch(typeof(DialogBox), "ActivateDelayed")]
    public static class DialogBox_ActivateDelayed_Patch
    {
        public static void Postfix(DialogBox __instance, float delta)
        {
            // Wait for the delay (approx) then read.
            // The game waits (0.11667f - delta).
            float waitTime = Mathf.Max(0.11667f - delta, 0f) + 0.05f; 
            MelonCoroutines.Start(DialogHelper.ReadDialogDelayed(__instance, waitTime));
        }
    }

    public static class DialogHelper
    {
        private const float DialogRepeatBlockSeconds = 0.8f;
        private static string _lastDialogText = "";
        private static float _lastDialogTime = -10f;

        private static void SpeakDialog(string text, bool interrupt)
        {
            if (!ModConfig.AnnounceTutorialDialog) return;
            if (string.IsNullOrWhiteSpace(text)) return;

            text = text.Replace("\r", " ").Replace("\n", " ").Trim();
            if (string.IsNullOrEmpty(text)) return;

            float now = Time.unscaledTime;
            if (text == _lastDialogText && now - _lastDialogTime < DialogRepeatBlockSeconds) return;

            _lastDialogText = text;
            _lastDialogTime = now;
            ScreenReader.Say(text, interrupt);
        }

        public static void ReadDialogText(string text)
        {
            SpeakDialog(text, true);
        }

        public static void ReadDialog(DialogBox box)
        {
            if (box == null) return;

            string dialogText = GetFragmentText(box.dialog);
            if (string.IsNullOrWhiteSpace(dialogText))
            {
                ReadGraphicDialog(box);
                return;
            }

            SpeakDialog(dialogText, true);
        }

        public static void ReadGraphicDialog(DialogBox box)
        {
            if (box == null) return;

            string left = GetFragmentText(box.leftGraphicText);
            string right = GetFragmentText(box.rightGraphicText);
            if (string.IsNullOrWhiteSpace(left) && string.IsNullOrWhiteSpace(right)) return;

            string text = string.IsNullOrWhiteSpace(left)
                ? right
                : string.IsNullOrWhiteSpace(right)
                    ? left
                    : JoinDialogParts(left, right);

            SpeakDialog(text, true);
        }

        public static IEnumerator ReadDialogDelayed(DialogBox box, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            ReadDialog(box);
        }

        public static IEnumerator ReadGraphicDialogDelayed(DialogBox box, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            ReadGraphicDialog(box);
        }

        private static string GetFragmentText(textboxFragment fragment)
        {
            if (fragment == null) return "";

            var tmp = fragment.GetComponent<TextMeshPro>();
            if (tmp == null || string.IsNullOrWhiteSpace(tmp.text)) return "";

            return tmp.text.Trim();
        }

        private static string JoinDialogParts(string left, string right)
        {
            if (string.IsNullOrWhiteSpace(left)) return right;
            if (string.IsNullOrWhiteSpace(right)) return left;

            char last = left[left.Length - 1];
            bool hasSentencePunctuation = last == '.' || last == '!' || last == '?' || last == '…';
            return hasSentencePunctuation
                ? $"{left} {right}"
                : $"{left}. {right}";
        }
    }
}
