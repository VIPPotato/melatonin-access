using HarmonyLib;
using MelonLoader;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Collections;
using UnityEngine.InputSystem; 

namespace MelatoninAccess
{
    public static class MapHandler
    {
        private const float ModeAnnouncementCooldown = 0.4f;
        private const float TeleportConflictHintCooldown = 2.0f;
        private const float TeleportDispatchCooldown = 0.08f;
        private const float LandmarkRepeatCooldown = 0.75f;
        private static float _lastModeMenuTitleTime = -10f;
        private static string _lastModeAnnouncement = "";
        private static float _lastModeAnnounceTime = -10f;
        private static string _lastLockReasonAnnouncement = "";
        private static float _lastLockReasonTime = -10f;
        private static float _lastTeleportConflictHintTime = -10f;
        private static int _lastTeleportDispatchFrame = -1;
        private static float _lastTeleportDispatchTime = -10f;
        private static bool _wasGamepadPrevDown;
        private static bool _wasGamepadNextDown;
        private static string _lastLandmarkAnnouncement = "";
        private static float _lastLandmarkAnnouncementTime = -10f;

        // --- Landmark & Mode Menu ---

        [HarmonyPatch(typeof(Landmark), "OnTriggerEnter2D")]
        public static class Landmark_Trigger_Patch
        {
            public static void Postfix(Landmark __instance)
            {
                if (!ModConfig.AnnounceMapHotspots) return;
                if (MapTeleporter.IsTeleporting) return;

                string name = FormatDreamName(__instance.dreamName);
                if (ShouldSkipLandmarkAnnouncement(name)) return;
                ScreenReader.Say(Loc.Get("arrived_at", name), true);
            }
        }

        [HarmonyPatch(typeof(Landmark), "Update")]
        public static class Landmark_Update_Patch
        {
            public static void Postfix()
            {
                if (ControlHandler.mgr == null || !ControlHandler.mgr.CheckIsActionPressed()) return;
                if (Map.env == null || Map.env.Neighbourhood == null || Map.env.Neighbourhood.McMap == null) return;

                ModeMenu menu = Map.env.Neighbourhood.McMap.ModeMenu;
                if (menu == null || !menu.CheckIsActivated() || !menu.CheckIsTranstioned()) return;

                string lockReason = GetModeLockReason(menu, menu.GetActiveItemNum());
                if (string.IsNullOrWhiteSpace(lockReason)) return;

                float now = Time.unscaledTime;
                if (lockReason == _lastLockReasonAnnouncement && now - _lastLockReasonTime < ModeAnnouncementCooldown) return;

                _lastLockReasonAnnouncement = lockReason;
                _lastLockReasonTime = now;
                ScreenReader.Say(lockReason, true);
            }
        }

        [HarmonyPatch(typeof(ModeMenu), "Transition")]
        public static class ModeMenu_Transition_Patch
        {
            public static void Postfix(ModeMenu __instance)
            {
                float now = Time.unscaledTime;
                if (now - _lastModeMenuTitleTime < ModeAnnouncementCooldown) return;

                _lastModeMenuTitleTime = now;
                AnnounceMode(__instance, includeMenuTitle: true);
            }
        }

        [HarmonyPatch(typeof(ModeMenu), "NavigateUp")]
        public static class ModeMenu_NavigateUp_Patch
        {
            public static void Postfix(ModeMenu __instance)
            {
                AnnounceMode(__instance, includeMenuTitle: false);
            }
        }

        [HarmonyPatch(typeof(ModeMenu), "NavigateDown")]
        public static class ModeMenu_NavigateDown_Patch
        {
            public static void Postfix(ModeMenu __instance)
            {
                AnnounceMode(__instance, includeMenuTitle: false);
            }
        }

        private static void AnnounceMode(ModeMenu menu, bool includeMenuTitle = false)
        {
            int activeItemNum = Traverse.Create(menu).Field("activeItemNum").GetValue<int>();
            var modeLabels = Traverse.Create(menu).Field("modeLabels").GetValue<textboxFragment[]>();

            if (modeLabels != null && activeItemNum >= 0 && activeItemNum < modeLabels.Length)
            {
                var label = modeLabels[activeItemNum];
                var tmp = label.GetComponent<TextMeshPro>();
                if (tmp != null && !string.IsNullOrWhiteSpace(tmp.text))
                {
                    string modeText = tmp.text.Trim();
                    string lockReason = GetModeLockReason(menu, activeItemNum);
                    if (!string.IsNullOrWhiteSpace(lockReason))
                    {
                        modeText = $"{modeText}. {lockReason}";
                    }

                    int position = GetModePosition(menu, activeItemNum);
                    int total = GetModeOptionsCount(menu);
                    if (ModConfig.AnnounceMenuPositions && position > 0 && total > 0)
                    {
                        modeText = $"{modeText}, {Loc.Get("order_of", position, total)}";
                    }

                    if (includeMenuTitle)
                    {
                        string dreamTitle = GetModeDreamTitle(menu);
                        if (!string.IsNullOrWhiteSpace(dreamTitle))
                        {
                            modeText = $"{Loc.Get("dream_about_level", dreamTitle)}. {Loc.Get("mode_menu")}. {modeText}";
                        }
                        else
                        {
                            modeText = $"{Loc.Get("mode_menu")}. {modeText}";
                        }
                    }

                    float now = Time.unscaledTime;
                    if (modeText == _lastModeAnnouncement && now - _lastModeAnnounceTime < ModeAnnouncementCooldown) return;

                    _lastModeAnnouncement = modeText;
                    _lastModeAnnounceTime = now;
                    ScreenReader.Say(modeText, true);
                }
            }
        }

        // --- Navigation Assist & Teleport ---

        [HarmonyPatch(typeof(McMap), "Update")]
        public static class McMap_Update_Patch
        {
            public static void Postfix(McMap __instance)
            {
                bool assistAllowed = IsMapNavigationAssistAllowed(__instance);

                bool f1Pressed = Keyboard.current != null && Keyboard.current.f1Key.wasPressedThisFrame;
                bool gamepadViewPressed = Gamepad.current != null && Gamepad.current.selectButton.wasPressedThisFrame;
                if ((f1Pressed || gamepadViewPressed) && assistAllowed)
                {
                    AnnounceCurrentMapProgress(__instance);
                }

                if (!assistAllowed) return;

                if (__instance != null && __instance.ModeMenu != null && __instance.ModeMenu.CheckIsTranstioned()) return;

                bool leftBracketPressed = Keyboard.current != null && Keyboard.current.leftBracketKey.wasPressedThisFrame;
                bool rightBracketPressed = Keyboard.current != null && Keyboard.current.rightBracketKey.wasPressedThisFrame;
                bool f9Pressed = Keyboard.current != null && Keyboard.current.f9Key.wasPressedThisFrame;
                bool f10Pressed = Keyboard.current != null && Keyboard.current.f10Key.wasPressedThisFrame;
                bool gamepadPrevDown = Gamepad.current != null && (Gamepad.current.leftShoulder.isPressed || Gamepad.current.leftTrigger.isPressed);
                bool gamepadNextDown = Gamepad.current != null && (Gamepad.current.rightShoulder.isPressed || Gamepad.current.rightTrigger.isPressed);
                bool gamepadPrevPressed = gamepadPrevDown && !_wasGamepadPrevDown;
                bool gamepadNextPressed = gamepadNextDown && !_wasGamepadNextDown;
                _wasGamepadPrevDown = gamepadPrevDown;
                _wasGamepadNextDown = gamepadNextDown;

                string actionKey = SaveManager.mgr != null ? SaveManager.mgr.GetActionKey() : "";
                bool actionUsesLeftBracket = actionKey == "[";
                bool actionUsesRightBracket = actionKey == "]";

                if ((leftBracketPressed && actionUsesLeftBracket) || (rightBracketPressed && actionUsesRightBracket))
                {
                    AnnounceTeleportConflictHint();
                }

                if ((leftBracketPressed && !actionUsesLeftBracket) || f9Pressed || gamepadPrevPressed)
                {
                    if (!TryConsumeTeleportDispatch()) return;
                    MapTeleporter.TeleportToPrev(__instance);
                }
                else if ((rightBracketPressed && !actionUsesRightBracket) || f10Pressed || gamepadNextPressed)
                {
                    if (!TryConsumeTeleportDispatch()) return;
                    MapTeleporter.TeleportToNext(__instance);
                }
            }
        }

        private static bool IsMapNavigationAssistAllowed(McMap map)
        {
            if (map == null) return false;
            if (Map.env == null || Map.env.Neighbourhood == null) return false;

            bool isMapEnabled = Traverse.Create(map).Field("isEnabled").GetValue<bool>();
            if (!isMapEnabled) return false;

            if (Chapter.dir != null && (Chapter.dir.CheckIsCutsceneIntro() || Chapter.dir.CheckIsCutsceneOutro()))
            {
                return false;
            }

            return true;
        }

        private static void AnnounceCurrentMapProgress(McMap player)
        {
            if (player == null || SaveManager.mgr == null) return;
            if (Map.env == null || Map.env.Neighbourhood == null) return;

            int chapterNum = Chapter.GetActiveChapterNum();
            if (chapterNum <= 0) return;

            int starsCollected = Mathf.Max(0, SaveManager.mgr.GetChapterEarnedStars(chapterNum));
            int starsRequired = GetChapterStarsRequiredToPass(chapterNum);
            int starsNeeded = Mathf.Max(0, starsRequired - starsCollected);

            ScreenReader.Say(Loc.Get("map_progress_status", starsCollected, starsNeeded), true);
        }

        public static class MapTeleporter
        {
            public static bool IsTeleporting = false;
            private static int currentIndex = -1;

            public static void TeleportToNext(McMap player)
            {
                Teleport(player, 1);
            }

            public static void TeleportToPrev(McMap player)
            {
                Teleport(player, -1);
            }

            private static void Teleport(McMap player, int direction)
            {
                if (Map.env == null || Map.env.Neighbourhood == null) return;

                var landmarks = GetPlayableLandmarks(Map.env.Neighbourhood);
                if (landmarks.Length == 0) return;

                if (currentIndex < 0 || currentIndex >= landmarks.Length)
                {
                    currentIndex = GetNearestLandmarkIndex(player.transform.position, landmarks);
                }

                currentIndex = (currentIndex + direction + landmarks.Length) % landmarks.Length;
                
                Landmark target = landmarks[currentIndex];
                
                IsTeleporting = true;
                
                // Adjusted Offset: Center (y) instead of y - 1f
                Vector3 targetPos = target.GetLocalPosition();
                player.SetLocalPosition(targetPos.x, targetPos.y); 
                
                if (Interface.env != null && Interface.env.Cam != null)
                {
                    Interface.env.Cam.SetPosition(targetPos.x, targetPos.y); 
                }

                string name = FormatDreamName(target.dreamName);
                int stars = SaveManager.mgr.GetScore("Dream_" + target.dreamName);
                if (ModConfig.AnnounceMapHotspots)
                {
                    string starKey = stars == 1 ? "teleport_arrived_one_star" : "teleport_arrived_stars";
                    string line = Loc.Get(starKey, name, stars);
                    if (IsLockedRemixLandmark(target))
                    {
                        line = $"{line} {Loc.Get("locked_requires_two_stars_each_dream")}";
                    }

                    ScreenReader.Say(line, true);
                }

                MelonCoroutines.Start(ResetTeleportFlag());
            }

            private static IEnumerator ResetTeleportFlag()
            {
                yield return new WaitForSecondsRealtime(1.0f);
                IsTeleporting = false;
            }
        }

        private static Landmark[] GetPlayableLandmarks(Neighbourhood neighbourhood)
        {
            if (neighbourhood == null || neighbourhood.Landmarks == null || neighbourhood.Landmarks.Length == 0)
            {
                return new Landmark[0];
            }

            return neighbourhood.Landmarks.Where(IsPlayableLevelLandmark).ToArray();
        }

        private static bool IsPlayableLevelLandmark(Landmark landmark)
        {
            if (landmark == null || string.IsNullOrWhiteSpace(landmark.dreamName))
            {
                return false;
            }

            string dreamName = landmark.dreamName.Trim();
            return AccessTools.TypeByName("Dream_" + dreamName) != null;
        }

        private static bool IsLockedRemixLandmark(Landmark landmark)
        {
            if (landmark == null) return false;

            bool isRemix = Traverse.Create(landmark).Field("isRemix").GetValue<bool>();
            bool isDisabled = Traverse.Create(landmark).Field("isDisabled").GetValue<bool>();
            return isRemix && isDisabled;
        }

        private static string FormatDreamName(string rawName)
        {
            string liveUiName = TryGetDreamNameFromLiveUi(rawName);
            if (!string.IsNullOrWhiteSpace(liveUiName))
            {
                return liveUiName;
            }

            return Loc.GetDreamName(rawName);
        }

        private static string TryGetDreamNameFromLiveUi(string rawName)
        {
            if (string.IsNullOrWhiteSpace(rawName)) return "";
            if (Map.env == null || Map.env.Neighbourhood == null || Map.env.Neighbourhood.Landmarks == null) return "";

            Landmark[] landmarks = Map.env.Neighbourhood.Landmarks;
            for (int i = 0; i < landmarks.Length; i++)
            {
                Landmark landmark = landmarks[i];
                if (landmark == null) continue;
                if (!string.Equals(landmark.dreamName, rawName, StringComparison.OrdinalIgnoreCase)) continue;

                if (landmark.DreamTitle != null && TryExtractFirstReadableText(landmark.DreamTitle.transform, out string dreamTitleText))
                {
                    return dreamTitleText;
                }

                if (TryExtractFirstReadableText(landmark.transform, out string landmarkText))
                {
                    return landmarkText;
                }

                return "";
            }

            return "";
        }

        private static bool TryExtractFirstReadableText(Transform root, out string text)
        {
            text = "";
            if (root == null) return false;

            TextMeshPro[] labels = root.GetComponentsInChildren<TextMeshPro>(true);
            for (int i = 0; i < labels.Length; i++)
            {
                if (labels[i] == null) continue;

                string candidate = labels[i].text;
                if (string.IsNullOrWhiteSpace(candidate)) continue;

                candidate = candidate.Replace("\n", " ").Trim();
                if (candidate.Length == 0) continue;
                if (!ContainsLetter(candidate)) continue;

                text = candidate;
                return true;
            }

            return false;
        }

        private static bool ContainsLetter(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsLetter(value[i])) return true;
            }

            return false;
        }

        private static string GetModeDreamTitle(ModeMenu menu)
        {
            if (menu == null) return "";

            string rawDreamName = Traverse.Create(menu).Field("dreamName").GetValue<string>();
            return FormatDreamName(rawDreamName);
        }

        private static void AnnounceTeleportConflictHint()
        {
            if (!ModConfig.AnnounceMapHotspots) return;
            float now = Time.unscaledTime;
            if (now - _lastTeleportConflictHintTime < TeleportConflictHintCooldown) return;

            _lastTeleportConflictHintTime = now;
            ScreenReader.Say(Loc.Get("teleport_conflict_hint"), true);
        }

        private static bool ShouldSkipLandmarkAnnouncement(string name)
        {
            float now = Time.unscaledTime;
            if (name == _lastLandmarkAnnouncement && now - _lastLandmarkAnnouncementTime < LandmarkRepeatCooldown)
            {
                return true;
            }

            _lastLandmarkAnnouncement = name;
            _lastLandmarkAnnouncementTime = now;
            return false;
        }

        private static bool TryConsumeTeleportDispatch()
        {
            int frame = Time.frameCount;
            float now = Time.unscaledTime;
            if (frame == _lastTeleportDispatchFrame) return false;
            if (now - _lastTeleportDispatchTime < TeleportDispatchCooldown) return false;

            _lastTeleportDispatchFrame = frame;
            _lastTeleportDispatchTime = now;
            return true;
        }

        private static int GetChapterStarsRequiredToPass(int chapterNum)
        {
            // Map progression target used by chapter maps (remix unlock/checkpoint threshold).
            if (chapterNum >= 1 && chapterNum <= 4) return 8;
            return 0;
        }

        private static int GetNearestLandmarkIndex(Vector3 position, Landmark[] landmarks)
        {
            int best = 0;
            float minDist = float.MaxValue;
            for (int i = 0; i < landmarks.Length; i++)
            {
                if (landmarks[i] == null) continue;

                float distance = Vector3.Distance(position, landmarks[i].transform.position);
                if (distance < minDist)
                {
                    minDist = distance;
                    best = i;
                }
            }

            return best;
        }

        private static string GetModeLockReason(ModeMenu menu, int activeItemNum)
        {
            int starScore = Traverse.Create(menu).Field("starScore").GetValue<int>();
            bool isRemix = Traverse.Create(menu).Field("isRemix").GetValue<bool>();
            bool isFullGame = Builder.mgr != null && Builder.mgr.CheckIsFullGame();

            return activeItemNum switch
            {
                1 when !(starScore > 0 || isRemix) => Loc.Get("locked_requires_one_star"),
                2 when starScore < 2 => Loc.Get("locked_requires_two_stars"),
                3 when starScore < 2 && !isFullGame => Loc.Get("locked_requires_two_stars_full_game"),
                3 when starScore < 2 => Loc.Get("locked_requires_two_stars"),
                3 when !isFullGame => Loc.Get("locked_requires_full_game"),
                _ => ""
            };
        }

        private static int GetModePosition(ModeMenu menu, int activeItemNum)
        {
            bool isRemix = Traverse.Create(menu).Field("isRemix").GetValue<bool>();
            if (!isRemix) return activeItemNum + 1;
            return activeItemNum;
        }

        private static int GetModeOptionsCount(ModeMenu menu)
        {
            bool isRemix = Traverse.Create(menu).Field("isRemix").GetValue<bool>();
            return isRemix ? 3 : 4;
        }
    }
}
