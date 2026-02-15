using HarmonyLib;
using System;
using UnityEngine;

namespace MelatoninAccess
{
    // Handles contextual tutorial cues and gameplay notifications.
    public static class RhythmHandler
    {
        private static string _lastSceneName = "";
        private static int _lastBeatTotal = -1;
        private static float _lastTechDoubleCueTime = -10f;
        private static float _lastTutorialSecondBeatCueTime = -10f;
        private static float _lastNatureTriplePromptTime = -10f;
        private static float _lastMindTriplePromptTime = -10f;

        private static string _lastQueueSignature = "";
        private static float _lastQueueSignatureTime = -10f;

        private static bool _followersPhaseOnePromptSpoken;
        private static bool _followersPhaseTwoPromptSpoken;
        private static bool _followersPhaseThreePromptSpoken;
        private static bool _shoppingPatternPromptSpoken;
        private static bool _datingIntroPromptSpoken;
        private static bool _futurePatternPromptSpoken;
        private static bool _foodThirdBeatPromptSpoken;
        private static bool _foodFifthBeatPromptSpoken;
        private static bool _foodFourthBeatPromptSpoken;
        private static bool _foodSeventhBeatPromptSpoken;
        private static bool _techPhaseOnePromptSpoken;
        private static bool _techPhaseTwoPromptSpoken;
        private static bool _timePortalGapPromptSpoken;
        private static bool _timeSixthSeventhPromptSpoken;
        private static int _timeSixthSeventhSectionCount;
        private static float _timeSixthSeventhSectionTime = -10f;
        private static bool _pastOneBeatHintSpoken;
        private static bool _pastHalfBeatHintSpoken;
        private static bool _pastTwoBeatHintSpoken;
        private static bool _scoreModeStartPromptSpoken;
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
                    RefreshTutorialCueState();
                    if (TryAnnounceCombinedDirectionalCue(numBeatsTilHit, isHalfBeatAdded)) return;
                    ScreenReader.Say(Loc.Get("cue_both"), true);
                }
            }
        }

        [HarmonyPatch(typeof(Dream), "QueueHoldReleaseWindow")]
        public static class Dream_QueueHoldReleaseWindow_Patch
        {
            public static void Postfix(int numBeatsTilHold, int numBeatsTilRelease, bool isHalfBeatAddedToHold, bool isHalfBeatAddedToRelease)
            {
                if (ShouldAnnounceCues())
                {
                    RefreshTutorialCueState();
                    if (TryAnnounceHoldReleaseOverride(numBeatsTilHold, numBeatsTilRelease, isHalfBeatAddedToHold, isHalfBeatAddedToRelease)) return;
                    ScreenReader.Say(Loc.Get("cue_hold_action", GetActionPrompt()), true);
                }
            }
        }

        [HarmonyPatch(typeof(Dream), "TriggerSong")]
        public static class Dream_TriggerSong_Patch
        {
            public static void Postfix()
            {
                if (!ModConfig.AnnounceRhythmCues || Dream.dir == null) return;
                RefreshTutorialCueState();
                TryAnnounceModeStartPrompt();

                if (!ShouldAnnounceCues()) return;

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

                if (string.Equals(GetActiveSceneName(), "Dream_future", StringComparison.OrdinalIgnoreCase) &&
                    !_futurePatternPromptSpoken)
                {
                    _futurePatternPromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_future_follow_patterns"), true);
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

                if (!_followersPhaseOnePromptSpoken && phrase == 1 && bar == 1 && beat == 1)
                {
                    _followersPhaseOnePromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_followers_phase_one_start_stop", GetActionPrompt()), true);
                    return;
                }

                if (!_followersPhaseTwoPromptSpoken && phrase == 2 && bar == 1 && beat == 1)
                {
                    _followersPhaseTwoPromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_followers_phase_two_resume_after_vibration"), true);
                    return;
                }

                // Phrase 3 starts after phrase 2 closes at bar 8 beat 4.
                // Speak this after the spring-stop transition so it lands closer to the section start.
                if (!_followersPhaseThreePromptSpoken && phrase == 3 && bar == 1 && beat == 1)
                {
                    _followersPhaseThreePromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_followers_phase_three_press_thrice", GetActionPrompt()), true);
                }
            }
        }

        private static bool ShouldAnnounceCues()
        {
            return ModConfig.AnnounceRhythmCues && Dream.dir != null && Dream.dir.GetGameMode() == 0;
        }

        private static void TryAnnounceModeStartPrompt()
        {
            int gameMode = Dream.dir.GetGameMode();
            if (gameMode != 1 && gameMode != 3) return;
            if (_scoreModeStartPromptSpoken) return;

            _scoreModeStartPromptSpoken = true;
            ScreenReader.Say(Loc.Get("score_mode_started"), true);
        }

        private static bool TryAnnounceTutorialOverride(int numBeatsTilHit, bool isHalfBeatAdded)
        {
            string sceneName = GetActiveSceneName();
            if (IsDuplicateQueueCue(sceneName, numBeatsTilHit, isHalfBeatAdded)) return true;

            string actionPrompt = GetActionPrompt();

            if (string.Equals(sceneName, "Dream_future", StringComparison.OrdinalIgnoreCase))
            {
                ScreenReader.Say(Loc.Get("cue_press_up"), true);
                return true;
            }

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
                else if (numBeatsTilHit == 6 && !_foodSeventhBeatPromptSpoken)
                {
                    _foodSeventhBeatPromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_food_press_seventh_beat", actionPrompt), true);
                }

                return true;
            }

            if (string.Equals(sceneName, "Dream_shopping", StringComparison.OrdinalIgnoreCase))
            {
                if (!_shoppingPatternPromptSpoken)
                {
                    _shoppingPatternPromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_shopping_repeat_patterns", actionPrompt), true);
                }

                return true;
            }

            if (string.Equals(sceneName, "Dream_followers", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(sceneName, "Dream_tech", StringComparison.OrdinalIgnoreCase))
            {
                return TryAnnounceTechCue(actionPrompt);
            }

            if (string.Equals(sceneName, "Dream_nature", StringComparison.OrdinalIgnoreCase) &&
                TryAnnounceNatureTripleCue(actionPrompt, numBeatsTilHit, isHalfBeatAdded))
            {
                return true;
            }

            if (string.Equals(sceneName, "Dream_mind", StringComparison.OrdinalIgnoreCase) &&
                TryAnnounceMindTripleCue(actionPrompt, numBeatsTilHit, isHalfBeatAdded))
            {
                return true;
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

        private static bool TryAnnounceCombinedDirectionalCue(int numBeatsTilHit, bool isHalfBeatAdded)
        {
            string sceneName = GetActiveSceneName();
            if (!string.Equals(sceneName, "Dream_future", StringComparison.OrdinalIgnoreCase)) return false;

            if (IsDuplicateQueueCue($"{sceneName}|LR", numBeatsTilHit, isHalfBeatAdded)) return true;

            ScreenReader.Say(Loc.Get("cue_left_right_short"), true);
            return true;
        }

        private static bool TryAnnounceHoldReleaseOverride(int numBeatsTilHold, int numBeatsTilRelease, bool isHalfBeatAddedToHold, bool isHalfBeatAddedToRelease)
        {
            string sceneName = GetActiveSceneName();
            bool isHoldReleaseTutorialScene =
                string.Equals(sceneName, "Dream_time", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(sceneName, "Dream_space", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(sceneName, "Dream_desires", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(sceneName, "Dream_past", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(sceneName, "Dream_nature", StringComparison.OrdinalIgnoreCase);

            if (!isHoldReleaseTutorialScene) return false;

            string actionPrompt = GetActionPrompt();

            if (string.Equals(sceneName, "Dream_time", StringComparison.OrdinalIgnoreCase) &&
                TryAnnounceTimeHoldCue(actionPrompt, numBeatsTilHold, numBeatsTilRelease, isHalfBeatAddedToHold, isHalfBeatAddedToRelease))
            {
                return true;
            }

            if ((string.Equals(sceneName, "Dream_space", StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(sceneName, "Dream_desires", StringComparison.OrdinalIgnoreCase)) &&
                TryAnnounceReleaseBeatCue(
                    actionPrompt,
                    numBeatsTilHold,
                    numBeatsTilRelease,
                    isHalfBeatAddedToHold,
                    isHalfBeatAddedToRelease))
            {
                return true;
            }

            if (string.Equals(sceneName, "Dream_past", StringComparison.OrdinalIgnoreCase) &&
                TryAnnouncePastCameraHoldCue(actionPrompt, numBeatsTilHold, numBeatsTilRelease, isHalfBeatAddedToHold, isHalfBeatAddedToRelease))
            {
                return true;
            }

            string durationLabel = BuildHoldDurationLabel(numBeatsTilHold, numBeatsTilRelease, isHalfBeatAddedToHold, isHalfBeatAddedToRelease);
            if (string.IsNullOrWhiteSpace(durationLabel)) return false;

            ScreenReader.Say(Loc.Get("cue_hold_release_duration", actionPrompt, durationLabel), true);
            return true;
        }

        private static bool TryAnnounceNatureTripleCue(string actionPrompt, int numBeatsTilHit, bool isHalfBeatAdded)
        {
            float now = Time.unscaledTime;

            if (numBeatsTilHit == 2 && !isHalfBeatAdded)
            {
                _lastNatureTriplePromptTime = now;
                ScreenReader.Say(Loc.Get("cue_nature_water_press_thrice", actionPrompt), true);
                return true;
            }

            if (now - _lastNatureTriplePromptTime <= 1.25f &&
                ((numBeatsTilHit == 2 && isHalfBeatAdded) || numBeatsTilHit == 3))
            {
                return true;
            }

            return false;
        }

        private static bool TryAnnounceMindTripleCue(string actionPrompt, int numBeatsTilHit, bool isHalfBeatAdded)
        {
            float now = Time.unscaledTime;
            if (numBeatsTilHit == 1 && isHalfBeatAdded)
            {
                _lastMindTriplePromptTime = now;
                ScreenReader.Say(Loc.Get("cue_mind_triple_offbeat", actionPrompt), true);
                return true;
            }

            if (numBeatsTilHit == 2 && now - _lastMindTriplePromptTime <= 1.25f)
            {
                return true;
            }

            return false;
        }

        private static bool TryAnnounceReleaseBeatCue(
            string actionPrompt,
            int numBeatsTilHold,
            int numBeatsTilRelease,
            bool isHalfBeatAddedToHold,
            bool isHalfBeatAddedToRelease)
        {
            double holdMoment = numBeatsTilHold + (isHalfBeatAddedToHold ? 0.5d : 0d);
            double releaseMoment = numBeatsTilRelease + (isHalfBeatAddedToRelease ? 0.5d : 0d);
            double releaseBeat = releaseMoment - holdMoment + 1d;
            if (releaseBeat < 1d) return false;

            int beatNumber = Math.Max(1, (int)Math.Round(releaseBeat));
            ScreenReader.Say(Loc.Get("cue_hold_release_on_beat", actionPrompt, beatNumber), true);
            return true;
        }

        private static bool TryAnnouncePastCameraHoldCue(string actionPrompt, int numBeatsTilHold, int numBeatsTilRelease, bool isHalfBeatAddedToHold, bool isHalfBeatAddedToRelease)
        {
            double holdMoment = numBeatsTilHold + (isHalfBeatAddedToHold ? 0.5d : 0d);
            double releaseMoment = numBeatsTilRelease + (isHalfBeatAddedToRelease ? 0.5d : 0d);
            double duration = releaseMoment - holdMoment;

            if (Math.Abs(duration - 1d) < 0.01d)
            {
                if (!_pastOneBeatHintSpoken)
                {
                    _pastOneBeatHintSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_past_camera_first_sound", actionPrompt), true);
                }
                return true;
            }

            if (Math.Abs(duration - 0.5d) < 0.01d)
            {
                if (!_pastHalfBeatHintSpoken)
                {
                    _pastHalfBeatHintSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_past_camera_second_sound", actionPrompt), true);
                }
                return true;
            }

            if (Math.Abs(duration - 2d) < 0.01d)
            {
                if (!_pastTwoBeatHintSpoken)
                {
                    _pastTwoBeatHintSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_past_camera_third_sound", actionPrompt), true);
                }
                return true;
            }

            return false;
        }

        private static string BuildHoldDurationLabel(int numBeatsTilHold, int numBeatsTilRelease, bool isHalfBeatAddedToHold, bool isHalfBeatAddedToRelease)
        {
            double holdMoment = numBeatsTilHold + (isHalfBeatAddedToHold ? 0.5d : 0d);
            double releaseMoment = numBeatsTilRelease + (isHalfBeatAddedToRelease ? 0.5d : 0d);
            double duration = releaseMoment - holdMoment;
            if (duration < 0.49d) duration = 0.5d;

            if (Math.Abs(duration - 0.5d) < 0.01d) return Loc.Get("duration_half_beat");
            if (Math.Abs(duration - 1d) < 0.01d) return Loc.Get("duration_one_beat");

            int rounded = Math.Max(2, (int)Math.Round(duration));
            return Loc.Get("duration_n_beats", rounded);
        }

        private static bool TryAnnounceTechCue(string actionPrompt)
        {
            int phrase = GetPhraseSafe();

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

        private static bool TryAnnounceTimeHoldCue(string actionPrompt, int numBeatsTilHold, int numBeatsTilRelease, bool isHalfBeatAddedToHold, bool isHalfBeatAddedToRelease)
        {
            bool isPortalGapCue =
                numBeatsTilHold == 2 &&
                numBeatsTilRelease == 3 &&
                isHalfBeatAddedToHold &&
                !isHalfBeatAddedToRelease;

            if (isPortalGapCue)
            {
                if (!_timePortalGapPromptSpoken)
                {
                    _timePortalGapPromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_time_portal_gap_hold_release", actionPrompt), true);
                }
                return true;
            }

            bool isSixthSeventhCue =
                numBeatsTilHold == 1 &&
                numBeatsTilRelease == 2 &&
                isHalfBeatAddedToHold &&
                !isHalfBeatAddedToRelease;

            if (isSixthSeventhCue)
            {
                float now = Time.unscaledTime;
                if (now - _timeSixthSeventhSectionTime > 3f)
                {
                    _timeSixthSeventhSectionTime = now;
                    _timeSixthSeventhSectionCount++;
                }

                // This signature appears in multiple teaching sections.
                // Keep the hint for the last section (second distinct section hit).
                if (_timeSixthSeventhSectionCount < 2)
                {
                    return true;
                }

                if (!_timeSixthSeventhPromptSpoken)
                {
                    _timeSixthSeventhPromptSpoken = true;
                    ScreenReader.Say(Loc.Get("cue_time_sixth_seventh_hold_release", actionPrompt), true);
                }
                return true;
            }

            return false;
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
                _followersPhaseOnePromptSpoken = false;
                _followersPhaseTwoPromptSpoken = false;
                _followersPhaseThreePromptSpoken = false;
                _shoppingPatternPromptSpoken = false;
                _datingIntroPromptSpoken = false;
                _futurePatternPromptSpoken = false;
                _foodThirdBeatPromptSpoken = false;
                _foodFifthBeatPromptSpoken = false;
                _foodFourthBeatPromptSpoken = false;
                _foodSeventhBeatPromptSpoken = false;
                _techPhaseOnePromptSpoken = false;
                _techPhaseTwoPromptSpoken = false;
                _timePortalGapPromptSpoken = false;
                _timeSixthSeventhPromptSpoken = false;
                _timeSixthSeventhSectionCount = 0;
                _timeSixthSeventhSectionTime = -10f;
                _pastOneBeatHintSpoken = false;
                _pastHalfBeatHintSpoken = false;
                _pastTwoBeatHintSpoken = false;
                _scoreModeStartPromptSpoken = false;
                _lastTechDoubleCueTime = -10f;
                _lastTechQueueCueTime = -10f;
                _lastNatureTriplePromptTime = -10f;
                _lastMindTriplePromptTime = -10f;
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
