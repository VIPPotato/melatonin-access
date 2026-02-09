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

            ScreenReader.Say(sb.ToString(), true);
        }

        // --- Stage End Menu (Replay/Next) ---
    
        [HarmonyPatch(typeof(StageEndMenu), "Show")]
        public static class StageEndMenu_Show_Patch
        {
            public static void Postfix(StageEndMenu __instance)
            {
                // Delay slightly to let Results read first, or just read options
                // Usually Results read is long, so we might not want to interrupt it immediately.
                // But StageEndMenu shows up at the same time.
                // Let's NOT read Stage Menu immediately if Results is reading. 
                // But the user might want to navigate immediately.
                // We'll let Results read. User can press Up/Down to hear options.
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
            public static void AnnounceSelection(StageEndMenu menu)
            {
                int highlightPosition = Traverse.Create(menu).Field("highlightPosition").GetValue<int>();
                int activeOptionsCount = Traverse.Create(menu).Field("activeOptionsCount").GetValue<int>();
                
                if (menu.labels != null && highlightPosition >= 0 && highlightPosition < menu.labels.Length)
                {
                    var label = menu.labels[highlightPosition];
                    var tmp = label.GetComponent<TextMeshPro>();
                    if (tmp != null)
                    {
                        string text = tmp.text;
                        if (activeOptionsCount > 0)
                        {
                            text = Loc.Get("stage_end_position", text, highlightPosition + 1, activeOptionsCount);
                        }
                        ScreenReader.Say(text, true);
                    }
                }
            }
        }
    }
}
