using HarmonyLib;
using MelonLoader;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections;
using UnityEngine.InputSystem; 

namespace MelatoninAccess
{
    public static class MapHandler
    {
        private const float ModeAnnouncementCooldown = 0.4f;
        private static float _lastModeMenuTitleTime = -10f;
        private static int _lastModeIndex = -1;
        private static float _lastModeAnnounceTime = -10f;

        // --- Landmark & Mode Menu ---

        [HarmonyPatch(typeof(Landmark), "OnTriggerEnter2D")]
        public static class Landmark_Trigger_Patch
        {
            public static void Postfix(Landmark __instance)
            {
                if (MapTeleporter.IsTeleporting) return;

                string name = FormatDreamName(__instance.dreamName);
                ScreenReader.Say(Loc.Get("arrived_at", name), true);
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
                ScreenReader.Say(Loc.Get("mode_menu"), true);
                AnnounceMode(__instance);
            }
        }

        [HarmonyPatch(typeof(ModeMenu), "NavigateUp")]
        public static class ModeMenu_NavigateUp_Patch
        {
            public static void Postfix(ModeMenu __instance)
            {
                AnnounceMode(__instance);
            }
        }

        [HarmonyPatch(typeof(ModeMenu), "NavigateDown")]
        public static class ModeMenu_NavigateDown_Patch
        {
            public static void Postfix(ModeMenu __instance)
            {
                AnnounceMode(__instance);
            }
        }

        private static void AnnounceMode(ModeMenu menu)
        {
            int activeItemNum = Traverse.Create(menu).Field("activeItemNum").GetValue<int>();
            var modeLabels = Traverse.Create(menu).Field("modeLabels").GetValue<textboxFragment[]>();

            if (modeLabels != null && activeItemNum >= 0 && activeItemNum < modeLabels.Length)
            {
                var label = modeLabels[activeItemNum];
                var tmp = label.GetComponent<TextMeshPro>();
                if (tmp != null && !string.IsNullOrWhiteSpace(tmp.text))
                {
                    float now = Time.unscaledTime;
                    if (activeItemNum == _lastModeIndex && now - _lastModeAnnounceTime < ModeAnnouncementCooldown) return;

                    _lastModeIndex = activeItemNum;
                    _lastModeAnnounceTime = now;
                    ScreenReader.Say(tmp.text.Trim(), true);
                }
            }
        }

        // --- Navigation Assist & Teleport ---

        [HarmonyPatch(typeof(McMap), "Update")]
        public static class McMap_Update_Patch
        {
            public static void Postfix(McMap __instance)
            {
                if (Keyboard.current == null) return;

                if (Keyboard.current.leftBracketKey.wasPressedThisFrame)
                {
                    MapTeleporter.TeleportToPrev(__instance);
                }
                else if (Keyboard.current.rightBracketKey.wasPressedThisFrame)
                {
                    MapTeleporter.TeleportToNext(__instance);
                }
            }
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
                
                var landmarks = Map.env.Neighbourhood.Landmarks;
                if (landmarks == null || landmarks.Length == 0) return;

                if (currentIndex < 0 || currentIndex >= landmarks.Length)
                {
                    currentIndex = GetNearestIndex(player.transform.position, landmarks);
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
                ScreenReader.Say(Loc.Get("teleport_arrived_stars", name, stars), true); // Removed "Jump to"

                MelonCoroutines.Start(ResetTeleportFlag());
            }

            private static int GetNearestIndex(Vector3 pos, Landmark[] landmarks)
            {
                int best = 0;
                float minDist = float.MaxValue;
                for (int i = 0; i < landmarks.Length; i++)
                {
                    float d = Vector3.Distance(pos, landmarks[i].transform.position);
                    if (d < minDist)
                    {
                        minDist = d;
                        best = i;
                    }
                }
                return best;
            }

            private static IEnumerator ResetTeleportFlag()
            {
                yield return new WaitForSecondsRealtime(1.0f);
                IsTeleporting = false;
            }
        }

        private static string FormatDreamName(string rawName)
        {
            if (string.IsNullOrEmpty(rawName)) return Loc.Get("unknown_level");
            if (rawName.Length == 1) return rawName.ToUpper();
            return char.ToUpper(rawName[0]) + rawName.Substring(1);
        }
    }
}
