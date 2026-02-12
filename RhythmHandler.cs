using HarmonyLib;
using System;
using UnityEngine;

namespace MelatoninAccess
{
    // Handles rhythm cues and gameplay notifications.
    public static class RhythmHandler
    {
        private static string _lastSceneName = "";
        private static int _lastBeatTotal = -1;
        private static float _lastActionCueTime = -10f;
        private static float _lastTechDoubleCueTime = -10f;
        private static float _lastTutorialSecondBeatCueTime = -10f;

        private static string _lastQueueSignature = "";
        private static float _lastQueueSignatureTime = -10f;

        private static bool _followersRhythmPromptSpoken;
        private static bool _followersVibrationPromptSpoken;
        private static bool _shoppingPatternPromptSpoken;
        private static bool _datingIntroPromptSpoken;
        private static bool _foodThirdBeatPromptSpoken;
        private static bool _foodFifthBeatPromptSpoken;
        private static bool _foodFourthBeatPromptSpoken;
        private static bool _followersThirdPhasePromptSpoken;
        private static bool _techPhaseOnePromptSpoken;
        private static bool _techPhaseTwoPromptSpoken;
        private static float _lastTechQueueCueTime = -10f;

        // --- Hit Window Cues (pre-cues) ---
        [HarmonyPatch(typeof(Dream), "QueueHitWindow")]
        public static class Dream_QueueHitWindow_Patch
        {
            public static void Postfix(int numBeatsTilHit, bool isHalfBeatAdded)
            {
                if (!ShouldAnnounceCues()) return;

                RefreshTutorialCueState();
                if (TryAnnounceTutorialOverride(numBeatsTilHit, isHalfBeatAdded)) return;

                ScreenReader.Say(Loc.Get("cue_press_action", GetActionPrompt()), true);
                _lastActionCueTime = Time.unscaledTime;
            }
        }

        [HarmonyPatch(typeof(Dream), "QueueLeftHitWindow")]
        public static class Dream_QueueLeftHitWindow_Patch
        {
            public static void Postfix(int numBeatsTilHit, bool isHalfBeatAdded)
            {
                if (ShouldAnnounceCues())
                {
                    RefreshTutorialCueState();
                    if (TryAnnounceDirectionalCue(isLeft: true, numBeatsTilHit, isHalfBeatAdded)) return;
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
                    RefreshTutorialCueState();
                    if (TryAnnounceDirectionalCue(isLeft: false, numBeatsTilHit, isHalfBeatAdded)) return;
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

        [HarmonyPatch(typeof(Dream), "TriggerSong")]
        public static class Dream_TriggerSong_Patch
        {
            public static void Postfix()
            {
                if (!ShouldAnnounceCues()) return;
                RefreshTutorialCueState();

                if (string.Equals(GetActiveSceneName(), "Dream_shopping", StringComparison.OrdinalIgnoreCase) &&
                    !_shoppingPatternPromptSpoken)
                {
                    _shoppingPatternPromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_shopping_repeat_patterns", GetActionPrompt()), true);
                }

                if (string.Equals(GetActiveSceneName(), "Dream_dating", StringComparison.OrdinalIgnoreCase) &&
                    !_datingIntroPromptSpoken)
                {
                    _datingIntroPromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_dating_follow_swipes"), true);
                }
            }
        }

        [HarmonyPatch(typeof(Dream_tutorial), "TriggerState")]
        public static class DreamTutorial_TriggerState_Patch
        {
            public static void Postfix(Dream_tutorial __instance)
            {
                if (!ModConfig.AnnounceRhythmCues || __instance == null) return;

                int state = Traverse.Create(__instance).Field("state").GetValue<int>();
                if (state != 2) return;

                float now = Time.unscaledTime;
                if (now - _lastTutorialSecondBeatCueTime < 0.4f) return;

                _lastTutorialSecondBeatCueTime = now;
                ScreenReader.Say(Loc.Get("cue_tutorial_press_second_beat", GetActionPrompt()), true);
            }
        }

        [HarmonyPatch(typeof(Dream_tech), "OnBeat")]
        public static class DreamTech_OnBeat_Patch
        {
            public static void Postfix(Dream_tech __instance)
            {
                if (__instance == null || !ShouldAnnounceCues()) return;

                RefreshTutorialCueState();

                int phrase = GetPhraseSafe();
                int bar = GetBarSafe();
                int beat = GetBeatSafe();
                string actionPrompt = GetActionPrompt();

                if (!_techPhaseOnePromptSpoken && phrase == 1 && bar == 1 && beat == 1)
                {
                    _techPhaseOnePromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_tech_every_two_beats", actionPrompt), true);
                    return;
                }

                if (!_techPhaseTwoPromptSpoken && phrase == 2 && bar == 1 && beat == 1)
                {
                    _techPhaseTwoPromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_tech_next_three_beats", actionPrompt), true);
                }
            }
        }

        [HarmonyPatch(typeof(Dream_followers), "OnBeat")]
        public static class DreamFollowers_OnBeat_Patch
        {
            public static void Postfix(Dream_followers __instance)
            {
                if (__instance == null || !ShouldAnnounceCues()) return;

                RefreshTutorialCueState();

                int phrase = GetPhraseSafe();
                int bar = GetBarSafe();
                int beat = GetBeatSafe();
                if (_followersThirdPhasePromptSpoken) return;

                // Phrase 3 starts after phrase 2 closes at bar 8 beat 4.
                // Announce this 2 beats earlier so the message finishes before the new section starts.
                if (phrase == 2 && bar == 8 && beat == 2)
                {
                    _followersThirdPhasePromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_followers_third_phase_double_after_vibration", GetActionPrompt()), true);
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
            if (IsDuplicateQueueCue(sceneName, numBeatsTilHit, isHalfBeatAdded)) return true;

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
                }

                if (!_followersVibrationPromptSpoken && IsFollowersVibrationSection())
                {
                    _followersVibrationPromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_followers_vibration_next_beat", actionPrompt), true);
                }

                _lastActionCueTime = Time.unscaledTime;
                return true;
            }

            if (string.Equals(sceneName, "Dream_tech", StringComparison.OrdinalIgnoreCase))
            {
                return TryAnnounceTechCue(actionPrompt);
            }

            return false;
        }

        private static bool TryAnnounceDirectionalCue(bool isLeft, int numBeatsTilHit, bool isHalfBeatAdded)
        {
            string sceneName = GetActiveSceneName();
            if (!string.Equals(sceneName, "Dream_dating", StringComparison.OrdinalIgnoreCase)) return false;

            if (IsDuplicateQueueCue($"{sceneName}|{(isLeft ? "L" : "R")}", numBeatsTilHit, isHalfBeatAdded)) return true;

            if (numBeatsTilHit >= 6)
            {
                ScreenReader.Say(Loc.Get(isLeft ? "cue_swipe_left_long" : "cue_swipe_right_long"), true);
            }
            else
            {
                ScreenReader.Say(Loc.Get(isLeft ? "cue_swipe_left" : "cue_swipe_right"), true);
            }

            return true;
        }

        private static bool TryAnnounceTechCue(string actionPrompt)
        {
            int phrase = GetPhraseSafe();
            if (!_techPhaseOnePromptSpoken && phrase <= 1)
            {
                _techPhaseOnePromptSpoken = true;
                ScreenReader.Say(Loc.Get("cue_tech_every_two_beats", actionPrompt), true);
                return true;
            }

            if (!_techPhaseTwoPromptSpoken && phrase == 2)
            {
                _techPhaseTwoPromptSpoken = true;
                ScreenReader.Say(Loc.Get("cue_tech_next_three_beats", actionPrompt), true);
                return true;
            }

            if (phrase >= 3)
            {
                if (TryAnnounceTechDoubleCue(actionPrompt))
                {
                    return true;
                }
            }

            // For Dream_tech practice, phrase-level guidance is spoken in OnBeat patch.
            // Suppress per-note generic "Press {Action}" chatter from QueueHitWindow.
            return true;
        }

        private static bool TryAnnounceTechDoubleCue(string actionPrompt)
        {
            float now = Time.unscaledTime;
            float delta = now - _lastTechQueueCueTime;
            _lastTechQueueCueTime = now;

            bool looksLikeRapidDouble = delta >= 0.1f && delta <= 0.45f;
            if (!looksLikeRapidDouble) return false;

            if (now - _lastTechDoubleCueTime <= 0.65f) return false;

            _lastTechDoubleCueTime = now;
            ScreenReader.Say(Loc.Get("cue_press_action_twice", actionPrompt), true);
            return true;
        }

        private static bool IsFollowersVibrationSection()
        {
            int phrase = GetPhraseSafe();
            return phrase >= 2;
        }

        private static bool IsDuplicateQueueCue(string sceneName, int numBeatsTilHit, bool isHalfBeatAdded)
        {
            float now = Time.unscaledTime;
            string signature = $"{sceneName}|{numBeatsTilHit}|{isHalfBeatAdded}|{GetPhraseSafe()}|{GetBarSafe()}|{GetBeatSafe()}";
            bool duplicate = signature == _lastQueueSignature && now - _lastQueueSignatureTime < 0.05f;

            _lastQueueSignature = signature;
            _lastQueueSignatureTime = now;
            return duplicate;
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
                _followersVibrationPromptSpoken = false;
                _shoppingPatternPromptSpoken = false;
                _datingIntroPromptSpoken = false;
                _foodThirdBeatPromptSpoken = false;
                _foodFifthBeatPromptSpoken = false;
                _foodFourthBeatPromptSpoken = false;
                _followersThirdPhasePromptSpoken = false;
                _techPhaseOnePromptSpoken = false;
                _techPhaseTwoPromptSpoken = false;
                _lastActionCueTime = -10f;
                _lastTechDoubleCueTime = -10f;
                _lastTechQueueCueTime = -10f;
                _lastQueueSignature = "";
                _lastQueueSignatureTime = -10f;
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

        private static int GetPhraseSafe()
        {
            if (Dream.dir == null) return -1;
            try
            {
                return Dream.dir.GetPhrase();
            }
            catch
            {
                return -1;
            }
        }

        private static int GetBarSafe()
        {
            if (Dream.dir == null) return -1;
            try
            {
                return Dream.dir.GetBar();
            }
            catch
            {
                return -1;
            }
        }

        private static int GetBeatSafe()
        {
            if (Dream.dir == null) return -1;
            try
            {
                return Dream.dir.GetBeat();
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
