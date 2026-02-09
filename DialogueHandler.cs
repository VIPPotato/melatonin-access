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
            DialogHelper.ReadDialog(__instance);
            MelonCoroutines.Start(DialogHelper.ReadDialogDelayed(__instance, 0.18f));
            MelonCoroutines.Start(DialogHelper.ReadDialogDelayed(__instance, 0.42f));
        }
    }

    [HarmonyPatch(typeof(DialogBox), "ChangeDialogState")]
    public static class DialogBox_ChangeDialogState_Patch
    {
        public static void Postfix(DialogBox __instance)
        {
            MelonCoroutines.Start(DialogHelper.ReadDialogDelayed(__instance, 0.2f));
            MelonCoroutines.Start(DialogHelper.ReadDialogDelayed(__instance, 0.45f));
        }
    }

    [HarmonyPatch(typeof(DialogBox), "SetDialogState")]
    public static class DialogBox_SetDialogState_Patch
    {
        public static void Postfix(DialogBox __instance)
        {
            DialogHelper.ReadDialog(__instance);
        }
    }

    // --- New Patches for Activation (Fixes Food/Followers) ---

    [HarmonyPatch(typeof(DialogBox), "Activate")]
    public static class DialogBox_Activate_Patch
    {
        public static void Postfix(DialogBox __instance)
        {
            DialogHelper.ReadDialog(__instance);
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
            if (box == null || box.dialog == null) return;

            var tmp = box.dialog.GetComponent<TextMeshPro>();
            if (tmp != null)
            {
                SpeakDialog(tmp.text, true);
            }
        }

        public static IEnumerator ReadDialogDelayed(DialogBox box, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            ReadDialog(box);
        }
    }
}
