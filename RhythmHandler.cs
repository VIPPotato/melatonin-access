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
                if (Dream.dir != null && Dream.dir.GetGameMode() == 0)
                {
                    ScreenReader.Say("Space", true);
                }
            }
        }

        [HarmonyPatch(typeof(Dream), "QueueLeftHitWindow")]
        public static class Dream_QueueLeftHitWindow_Patch
        {
            public static void Postfix(int numBeatsTilHit, bool isHalfBeatAdded)
            {
                if (Dream.dir != null && Dream.dir.GetGameMode() == 0)
                {
                    ScreenReader.Say("Left", true);
                }
            }
        }

        [HarmonyPatch(typeof(Dream), "QueueRightHitWindow")]
        public static class Dream_QueueRightHitWindow_Patch
        {
            public static void Postfix(int numBeatsTilHit, bool isHalfBeatAdded)
            {
                if (Dream.dir != null && Dream.dir.GetGameMode() == 0)
                {
                    ScreenReader.Say("Right", true);
                }
            }
        }

        [HarmonyPatch(typeof(Dream), "QueueLeftRightHitWindow")]
        public static class Dream_QueueLeftRightHitWindow_Patch
        {
            public static void Postfix(int numBeatsTilHit, bool isHalfBeatAdded)
            {
                if (Dream.dir != null && Dream.dir.GetGameMode() == 0)
                {
                    ScreenReader.Say("Both", true);
                }
            }
        }

        [HarmonyPatch(typeof(Dream), "QueueHoldReleaseWindow")]
        public static class Dream_QueueHoldReleaseWindow_Patch
        {
            public static void Postfix(int numBeatsTilHold, int numBeatsTilRelease)
            {
                if (Dream.dir != null && Dream.dir.GetGameMode() == 0)
                {
                    ScreenReader.Say("Hold", true);
                }
            }
        }
    }
}