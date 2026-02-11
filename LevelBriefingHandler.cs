using HarmonyLib;
using MelonLoader;
using UnityEngine;
using System.Collections;

namespace MelatoninAccess
{
    public static class LevelBriefingHandler
    {
        private const float BriefingDelaySeconds = 0.4f;
        private const float BriefingRepeatBlockSeconds = 0.6f;

        private static bool _suppressPracticePrompt;
        private static string _lastBriefing = "";
        private static float _lastBriefingTime = -10f;

        public static bool ShouldSuppressPracticePrompt()
        {
            return _suppressPracticePrompt;
        }

        [HarmonyPatch(typeof(Dream), "Start")]
        public static class Dream_Start_Patch
        {
            public static void Prefix(Dream __instance)
            {
                _suppressPracticePrompt = __instance != null && __instance.GetGameMode() == 0;
            }

            public static void Postfix(Dream __instance)
            {
                _suppressPracticePrompt = false;
                MelonCoroutines.Start(AnnounceBriefingDelayed(__instance));
            }
        }

        private static IEnumerator AnnounceBriefingDelayed(Dream dream)
        {
            yield return new WaitForSecondsRealtime(BriefingDelaySeconds);

            if (dream == null || Dream.dir != dream) yield break;

            string briefing = BuildBriefingText(dream);
            if (string.IsNullOrWhiteSpace(briefing)) yield break;

            float now = Time.unscaledTime;
            if (briefing == _lastBriefing && now - _lastBriefingTime < BriefingRepeatBlockSeconds) yield break;

            _lastBriefing = briefing;
            _lastBriefingTime = now;
            ScreenReader.Say(briefing, true);
        }

        private static string BuildBriefingText(Dream dream)
        {
            string levelName = GetLevelName();
            int gameMode = dream.GetGameMode();

            string modeKey = GetModeKey(gameMode);
            string objective = GetObjectiveText(gameMode);
            return Loc.Get("level_briefing_line", levelName, Loc.Get(modeKey), objective);
        }

        private static string GetLevelName()
        {
            string sceneName = SceneMonitor.mgr != null ? SceneMonitor.mgr.GetActiveSceneName() : "";
            if (string.IsNullOrWhiteSpace(sceneName)) return Loc.Get("unknown_level");

            if (sceneName.StartsWith("Dream_", System.StringComparison.OrdinalIgnoreCase))
            {
                return Loc.GetDreamName(sceneName.Substring("Dream_".Length));
            }

            if (sceneName.StartsWith("LvlEditor_", System.StringComparison.OrdinalIgnoreCase))
            {
                return Loc.GetDreamName(sceneName.Substring("LvlEditor_".Length));
            }

            return sceneName;
        }

        private static string GetModeKey(int gameMode)
        {
            return gameMode switch
            {
                0 => "mode_practice",
                1 => "mode_score",
                2 => "mode_hard",
                3 => "mode_score_remix",
                4 => "mode_hard_remix",
                5 => "mode_tutorial",
                6 => "mode_editor_test",
                7 => "mode_community",
                _ => "mode_score"
            };
        }

        private static string GetObjectiveText(int gameMode)
        {
            return gameMode switch
            {
                0 => Loc.Get("objective_practice", SideLabelHelper.GetSkipPromptLabel()),
                1 => Loc.Get("objective_score"),
                2 => Loc.Get("objective_hard"),
                3 => Loc.Get("objective_score"),
                4 => Loc.Get("objective_hard"),
                5 => Loc.Get("objective_tutorial"),
                6 => Loc.Get("objective_editor_test"),
                7 => Loc.Get("objective_community"),
                _ => Loc.Get("objective_default")
            };
        }
    }
}
