using HarmonyLib;
using MelonLoader;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Text;

namespace MelatoninAccess
{
    public static class ResultsHandler
    {
        [HarmonyPatch(typeof(Results), "Activate")]
        public static class Results_Activate_Patch
        {
            public static void Postfix(Results __instance)
            {
                MelonCoroutines.Start(ReadResultsDelayed(__instance));
            }
        }

        private static IEnumerator ReadResultsDelayed(Results results)
        {
            yield return new WaitForSecondsRealtime(0.5f); // Wait for animations

            StringBuilder sb = new StringBuilder();
            
            // Title / Score Message
            if (results.ScoreMessage != null)
            {
                if (results.ScoreMessage.title != null)
                {
                    var tmp = results.ScoreMessage.title.GetComponent<TextMeshPro>();
                    if (tmp != null) sb.Append(tmp.text + ". ");
                }
                if (results.ScoreMessage.subtitle != null && results.ScoreMessage.subtitle.CheckIsMeshRendered())
                {
                    var tmp = results.ScoreMessage.subtitle.GetComponent<TextMeshPro>();
                    if (tmp != null) sb.Append(tmp.text + ". ");
                }
            }

            // Stats
            // Counter 0: Perfect, 1: Late, 2: Early, 3: Miss
            if (Dream.dir != null)
            {
                int perfect = Dream.dir.GetCounter(0);
                int late = Dream.dir.GetCounter(1);
                int early = Dream.dir.GetCounter(2);
                int miss = Dream.dir.GetCounter(3);

                sb.Append(Loc.Get("results_stats", perfect, late, early, miss));
            }

            if (results.StageEndMenu != null &&
                StageEndMenuHelper.TryBuildSelectionAnnouncement(results.StageEndMenu, out string firstOptionAnnouncement))
            {
                if (sb.Length > 0) sb.Append(" ");
                sb.Append(firstOptionAnnouncement);
            }

            ScreenReader.Say(sb.ToString(), true);
        }

        // --- Stage End Menu (Replay/Next) ---
    
        [HarmonyPatch(typeof(StageEndMenu), "Show")]
        public static class StageEndMenu_Show_Patch
        {
            public static void Postfix(StageEndMenu __instance)
            {
                // Initial option is announced together with the results summary.
            }
        }

        [HarmonyPatch(typeof(StageEndMenu), "Next")]
        public static class StageEndMenu_Next_Patch
        {
            public static void Postfix(StageEndMenu __instance)
            {
                StageEndMenuHelper.AnnounceSelection(__instance);
            }
        }

        [HarmonyPatch(typeof(StageEndMenu), "Prev")]
        public static class StageEndMenu_Prev_Patch
        {
            public static void Postfix(StageEndMenu __instance)
            {
                StageEndMenuHelper.AnnounceSelection(__instance);
            }
        }

        public static class StageEndMenuHelper
        {
            private const float StageEndRepeatBlockSeconds = 0.35f;
            private static string _lastStageEndAnnouncement = "";
            private static float _lastStageEndAnnouncementTime = -10f;

            public static void AnnounceSelection(StageEndMenu menu)
            {
                if (!TryBuildSelectionAnnouncement(menu, out string text)) return;

                float now = Time.unscaledTime;
                if (text == _lastStageEndAnnouncement && now - _lastStageEndAnnouncementTime < StageEndRepeatBlockSeconds) return;

                _lastStageEndAnnouncement = text;
                _lastStageEndAnnouncementTime = now;
                ScreenReader.Say(text, true);
            }

            public static bool TryBuildSelectionAnnouncement(StageEndMenu menu, out string announcement)
            {
                announcement = "";
                if (menu == null) return false;

                int highlightPosition = Traverse.Create(menu).Field("highlightPosition").GetValue<int>();
                int activeOptionsCount = GetVisibleOptionsCount(menu);
                if (menu.labels == null || highlightPosition < 0 || highlightPosition >= menu.labels.Length) return false;

                var label = menu.labels[highlightPosition];
                var tmp = label != null ? label.GetComponent<TextMeshPro>() : null;
                if (tmp == null || string.IsNullOrWhiteSpace(tmp.text)) return false;

                string text = tmp.text.Trim();
                if (activeOptionsCount > 0)
                {
                    text = Loc.Get("stage_end_position", text, highlightPosition + 1, activeOptionsCount);
                }

                string lockReason = GetLockReason(menu, highlightPosition);
                if (!string.IsNullOrWhiteSpace(lockReason))
                {
                    text = $"{text}. {lockReason}";
                }

                announcement = text;
                return true;
            }

            private static int GetVisibleOptionsCount(StageEndMenu menu)
            {
                if (menu.labels == null) return 0;

                int count = 0;
                foreach (var label in menu.labels)
                {
                    if (label != null && label.CheckIsMeshRendered())
                    {
                        count++;
                    }
                }
                return count;
            }

            private static string GetLockReason(StageEndMenu menu, int highlightPosition)
            {
                if (highlightPosition != 0) return "";

                int gameMode = Traverse.Create(menu).Field("gameMode").GetValue<int>();
                if (gameMode != 1 && gameMode != 3) return "";

                string sceneName = SceneMonitor.mgr != null ? SceneMonitor.mgr.GetActiveSceneName() : "";
                if (string.IsNullOrEmpty(sceneName) || SaveManager.mgr == null) return "";

                return SaveManager.mgr.GetScore(sceneName) >= 2
                    ? ""
                    : Loc.Get("locked_requires_two_stars");
            }
        }
    }
}
