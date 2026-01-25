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
            if (!string.IsNullOrEmpty(newText))
            {
                ScreenReader.Say(newText, true);
            }
        }
    }

    [HarmonyPatch(typeof(DialogBox), "ChangeDialogState")]
    public static class DialogBox_ChangeDialogState_Patch
    {
        public static void Postfix(DialogBox __instance)
        {
            // If it's delayed, this might read old text. 
            // But usually ChangeDialogState updates text via SetState internally immediately?
            // Checking the code: ChangeDialogState IS a coroutine if isDelayed=true.
            // But we can't easily detect the delay here without traversing.
            // Safe bet: Read it. If it changes later, SetText or SetDialogState might catch it?
            // Actually, ChangeDialogState calls SetDialogState internally.
            // So we might get double reads if we patch both.
            // But SetDialogState is the primitive.
            // Let's stick to SetDialogState for the state change.
            // But if ChangeDialogState is used, it eventually calls SetDialogState.
            // So we don't need to patch ChangeDialogState if SetDialogState covers it?
            // Wait, SetDialogState is void. ChangeDialogState is void (starts coroutine).
            // The coroutine calls dialog.SetState().
            // Does dialog.SetState() trigger our SetText patch? No, SetState sets textMeshPro.text directly.
            // So we DO need to hook SetState or the methods calling it.
            // Or better: DialogBox.SetDialogState calls dialog.SetState().
            // We patched DialogBox.SetDialogState.
            // But ChangeDialogState calls dialog.SetState DIRECTLY inside its coroutine (it's a private IEnumerator).
            // We can't patch private enumerators easily.
            // We should rely on Activate/ActivateDelayed for the "Initial" read (Food/Followers issue).
            // And for state changes, maybe polling or hooking textboxFragment.SetState is cleaner?
            // No, hooking textboxFragment.SetState is global and noisy.
            
            // Let's keep the hook, but maybe delay it?
            MelonCoroutines.Start(ReadDialogDelayed(__instance, 0.2f));
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
            MelonCoroutines.Start(ReadDialogDelayed(__instance, waitTime));
        }
    }

    public static class DialogHelper
    {
        public static void ReadDialog(DialogBox box)
        {
            if (box.dialog != null)
            {
                var tmp = box.dialog.GetComponent<TextMeshPro>();
                if (tmp != null && !string.IsNullOrEmpty(tmp.text))
                {
                    ScreenReader.Say(tmp.text, true);
                }
            }
        }
    }

    public static IEnumerator ReadDialogDelayed(DialogBox box, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        DialogHelper.ReadDialog(box);
    }
}
