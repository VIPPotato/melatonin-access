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
                if (ShouldAnnounceCues())
                {
                    ScreenReader.Say(Loc.Get("cue_press_action", GetActionPrompt()), true);
                }
            }
        }

        [HarmonyPatch(typeof(Dream), "QueueLeftHitWindow")]
        public static class Dream_QueueLeftHitWindow_Patch
        {
            public static void Postfix(int numBeatsTilHit, bool isHalfBeatAdded)
            {
                if (ShouldAnnounceCues())
                {
                    ScreenReader.Say(Loc.Get("cue_left"), true);
                }
            }
        }

        [HarmonyPatch(typeof(Dream), "QueueRightHitWindow")]
        public static class Dream_QueueRightHitWindow_Patch
        {
            public static void Postfix(int numBeatsTilHit, bool isHalfBeatAdded)
            {
                if (ShouldAnnounceCues())
                {
                    ScreenReader.Say(Loc.Get("cue_right"), true);
                }
            }
        }

        [HarmonyPatch(typeof(Dream), "QueueLeftRightHitWindow")]
        public static class Dream_QueueLeftRightHitWindow_Patch
        {
            public static void Postfix(int numBeatsTilHit, bool isHalfBeatAdded)
            {
                if (ShouldAnnounceCues())
                {
                    ScreenReader.Say(Loc.Get("cue_both"), true);
                }
            }
        }

        [HarmonyPatch(typeof(Dream), "QueueHoldReleaseWindow")]
        public static class Dream_QueueHoldReleaseWindow_Patch
        {
            public static void Postfix(int numBeatsTilHold, int numBeatsTilRelease)
            {
                if (ShouldAnnounceCues())
                {
                    ScreenReader.Say(Loc.Get("cue_hold_action", GetActionPrompt()), true);
                }
            }
        }

        private static bool ShouldAnnounceCues()
        {
            return ModConfig.AnnounceRhythmCues && Dream.dir != null && Dream.dir.GetGameMode() == 0;
        }

        private static string GetActionPrompt()
        {
            int ctrlType = ControlHandler.mgr != null ? ControlHandler.mgr.GetCtrlType() : 0;
            if (ctrlType == 1) return "A";
            if (ctrlType == 2) return "Cross";

            string key = SaveManager.mgr != null ? SaveManager.mgr.GetActionKey() : "SPACE";
            if (string.IsNullOrWhiteSpace(key)) return Loc.Get("cue_space");

            return key.Trim().ToUpperInvariant() switch
            {
                "SPACE" => Loc.Get("cue_space"),
                "ENTER" => Loc.Get("key_enter"),
                "PERIOD" => Loc.Get("key_period"),
                "SLASH" => Loc.Get("key_slash"),
                _ => key.Trim()
            };
        }
    }
}
