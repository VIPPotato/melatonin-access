using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace MelatoninAccess
{
    // Handles Rhythm Cues and Gameplay Notifications
    public static class RhythmHandler
    {
        // --- Hit Window Cues (Pre-cues) ---

        [HarmonyPatch(typeof(Dream), "QueueHitWindow")]
        public static class Dream_QueueHitWindow_Patch
        {
            public static void Postfix(int numBeatsTilHit, bool isHalfBeatAdded)
            {
                // Standard Action (Space/A)
                ScreenReader.Say("Space", true);
            }
        }

        [HarmonyPatch(typeof(Dream), "QueueLeftHitWindow")]
        public static class Dream_QueueLeftHitWindow_Patch
        {
            public static void Postfix(int numBeatsTilHit, bool isHalfBeatAdded)
            {
                ScreenReader.Say("Left", true);
            }
        }

        [HarmonyPatch(typeof(Dream), "QueueRightHitWindow")]
        public static class Dream_QueueRightHitWindow_Patch
        {
            public static void Postfix(int numBeatsTilHit, bool isHalfBeatAdded)
            {
                ScreenReader.Say("Right", true);
            }
        }

        [HarmonyPatch(typeof(Dream), "QueueLeftRightHitWindow")]
        public static class Dream_QueueLeftRightHitWindow_Patch
        {
            public static void Postfix(int numBeatsTilHit, bool isHalfBeatAdded)
            {
                ScreenReader.Say("Both", true);
            }
        }

        [HarmonyPatch(typeof(Dream), "QueueHoldReleaseWindow")]
        public static class Dream_QueueHoldReleaseWindow_Patch
        {
            public static void Postfix(int numBeatsTilHold, int numBeatsTilRelease)
            {
                ScreenReader.Say("Hold", true);
            }
        }

        // --- Feedback (Optional - Game has sounds, but maybe we need verbal confirmation) ---
        // Uncomment if distinct feedback is needed beyond game SFX.

        /*
        [HarmonyPatch(typeof(Dream), "OnHit")]
        public static class Dream_OnHit_Patch
        {
            public static void Postfix(Dream __instance)
            {
                // Access protected field 'accuracy' via Reflection or Traverse if needed.
                // For now, let's rely on game SFX.
            }
        }
        */

        // --- Beat / Metronome (Optional) ---
        /*
        [HarmonyPatch(typeof(Dream), "TriggerBeat")]
        public static class Dream_TriggerBeat_Patch
        {
            public static void Postfix(bool toggle)
            {
                 // Could play a click sound here if accessibility metronome is needed
            }
        }
        */
    }
}
