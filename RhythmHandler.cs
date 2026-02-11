using HarmonyLib;
using MelonLoader;
using System;
using UnityEngine;

namespace MelatoninAccess
{
    // Handles Rhythm Cues and Gameplay Notifications
    public static class RhythmHandler
    {
        private static string _lastSceneName = "";
        private static int _lastBeatTotal = -1;
        private static float _lastActionCueTime = -10f;
        private static float _lastDoubleCueTime = -10f;
        private static bool _followersRhythmPromptSpoken;
        private static bool _shoppingPatternPromptSpoken;
        private static bool _foodThirdBeatPromptSpoken;
        private static bool _foodFifthBeatPromptSpoken;
        private static bool _foodFourthBeatPromptSpoken;

        // --- Hit Window Cues (Pre-cues) ---

        [HarmonyPatch(typeof(Dream), "QueueHitWindow")]
        public static class Dream_QueueHitWindow_Patch
        {
            public static void Postfix(int numBeatsTilHit, bool isHalfBeatAdded)
            {
                if (ShouldAnnounceCues())
                {
                    RefreshTutorialCueState();
                    if (!TryAnnounceTutorialOverride(numBeatsTilHit, isHalfBeatAdded))
                    {
                        ScreenReader.Say(Loc.Get("cue_press_action", GetActionPrompt()), true);
                        _lastActionCueTime = Time.unscaledTime;
                    }
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

        private static bool TryAnnounceTutorialOverride(int numBeatsTilHit, bool isHalfBeatAdded)
        {
            string sceneName = GetActiveSceneName();
            string actionPrompt = GetActionPrompt();

            if (string.Equals(sceneName, "Dream_food", StringComparison.OrdinalIgnoreCase))
            {
                if (numBeatsTilHit == 2 && !_foodThirdBeatPromptSpoken)
                {
                    _foodThirdBeatPromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_food_press_third_beat", actionPrompt), true);
                }
                else if (numBeatsTilHit == 4 && !_foodFifthBeatPromptSpoken)
                {
                    _foodFifthBeatPromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_food_press_fifth_beat", actionPrompt), true);
                }
                else if (numBeatsTilHit == 3 && !_foodFourthBeatPromptSpoken)
                {
                    _foodFourthBeatPromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_food_press_fourth_beat", actionPrompt), true);
                }

                _lastActionCueTime = Time.unscaledTime;
                return true;
            }

            if (string.Equals(sceneName, "Dream_shopping", StringComparison.OrdinalIgnoreCase))
            {
                if (!_shoppingPatternPromptSpoken)
                {
                    _shoppingPatternPromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_shopping_repeat_patterns", actionPrompt), true);
                }

                _lastActionCueTime = Time.unscaledTime;
                return true;
            }

            if (string.Equals(sceneName, "Dream_followers", StringComparison.OrdinalIgnoreCase))
            {
                if (!_followersRhythmPromptSpoken)
                {
                    _followersRhythmPromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_followers_rhythm_stop", actionPrompt), true);
                    _lastActionCueTime = Time.unscaledTime;
                    return true;
                }

                if ((isHalfBeatAdded || IsRapidActionCue(0.9f)) && CanSpeakDoubleCue())
                {
                    ScreenReader.Say(Loc.Get("cue_press_action_twice", actionPrompt), true);
                    _lastActionCueTime = Time.unscaledTime;
                    _lastDoubleCueTime = Time.unscaledTime;
                }

                return true;
            }

            if (string.Equals(sceneName, "Dream_tech", StringComparison.OrdinalIgnoreCase) &&
                (isHalfBeatAdded || IsRapidActionCue(0.9f)) &&
                CanSpeakDoubleCue())
            {
                ScreenReader.Say(Loc.Get("cue_press_action_twice", actionPrompt), true);
                _lastActionCueTime = Time.unscaledTime;
                _lastDoubleCueTime = Time.unscaledTime;
                return true;
            }

            return false;
        }

        private static bool IsRapidActionCue(float thresholdSeconds)
        {
            return Time.unscaledTime - _lastActionCueTime <= thresholdSeconds;
        }

        private static bool CanSpeakDoubleCue()
        {
            return Time.unscaledTime - _lastDoubleCueTime > 0.4f;
        }

        private static void RefreshTutorialCueState()
        {
            string sceneName = GetActiveSceneName();
            int beatTotal = GetBeatTotalSafe();

            bool sceneChanged = !string.Equals(sceneName, _lastSceneName, StringComparison.OrdinalIgnoreCase);
            bool restartedBeatCounter = beatTotal >= 0 && _lastBeatTotal >= 0 && beatTotal < _lastBeatTotal;

            if (sceneChanged || restartedBeatCounter)
            {
                _followersRhythmPromptSpoken = false;
                _shoppingPatternPromptSpoken = false;
                _foodThirdBeatPromptSpoken = false;
                _foodFifthBeatPromptSpoken = false;
                _foodFourthBeatPromptSpoken = false;
                _lastActionCueTime = -10f;
                _lastDoubleCueTime = -10f;
            }

            _lastSceneName = sceneName;
            if (beatTotal >= 0) _lastBeatTotal = beatTotal;
        }

        private static string GetActiveSceneName()
        {
            if (SceneMonitor.mgr == null) return "";
            string sceneName = SceneMonitor.mgr.GetActiveSceneName();
            return sceneName ?? "";
        }

        private static int GetBeatTotalSafe()
        {
            if (Dream.dir == null) return -1;
            try
            {
                return Dream.dir.GetBeatTotal();
            }
            catch
            {
                return -1;
            }
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
