using MelonLoader;
using UnityEngine;
using UnityEngine.InputSystem; 
using MelatoninAccess;

[assembly: MelonInfo(typeof(MelatoninAccess.MelatoninAccessMod), "Melatonin Access", "1.0.5", "Gemini")]
[assembly: MelonGame("Half Asleep", "Melatonin")]

namespace MelatoninAccess
{
    public class MelatoninAccessMod : MelonMod
    {
        public static bool DebugMode = false;

        public override void OnInitializeMelon()
        {
            ScreenReader.Initialize();
            Loc.Initialize();
            ModConfig.Initialize();
            DebugMode = ModConfig.DebugModeEnabled;

            // Menu & Options
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(MenuHandler.MenuTitle_Activate_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(MenuHandler.Option_Enable_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(MenuHandler.Option_Select_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(MenuHandler.Option_Reverse_Patch));
            
            // Start Screen
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(TitleScreen_Awake_Patch)); 
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(TitleScreen_Update_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(LangMenu_Activate_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(LangMenu_Descend_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(LangMenu_Ascend_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(LangMenu_Select_Patch));
            
            // Extra Menus
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(CalibrationTool_Activate_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(CalibrationTool_Update_Patch));
            
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(CommunityMenu_Activate_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(CommunityMenu_Descend_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(CommunityMenu_Ascend_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(CommunityMenu_NextPage_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(CommunityMenu_PrevPage_Patch));

            // Map
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(MapHandler.Landmark_Trigger_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(MapHandler.Landmark_Update_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(MapHandler.ModeMenu_Transition_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(MapHandler.ModeMenu_NavigateUp_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(MapHandler.ModeMenu_NavigateDown_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(MapHandler.McMap_Update_Patch));
            
            // Popups
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(ExtraMessage_Activate_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(ConfirmModal_Activate_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(RebindModal_Show_Patch));
            
            // Achievements
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(AchievementsHandler.AchievementsMenu_Activate_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(AchievementsHandler.AchievementsMenu_Descend_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(AchievementsHandler.AchievementsMenu_Ascend_Patch));
            
            // Dialogue & Tutorial
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(DialogBox_SetText_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(DialogBox_ChangeDialogState_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(DialogBox_SetDialogState_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(DialogBox_ChangeToGraphic_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(DialogBox_Show_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(DialogBox_Activate_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(DialogBox_ActivateDelayed_Patch));
            
            // Results & Stage End
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(ResultsHandler.Results_Activate_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(ResultsHandler.StageEndMenu_Show_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(ResultsHandler.StageEndMenu_Next_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(ResultsHandler.StageEndMenu_Prev_Patch));

            // Credits
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(CreditsHandler.Credits_Show_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(CreditsHandler.Credits_TransitionLogoCompanyToCreator_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(CreditsHandler.Credits_ScrollList_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(CreditsHandler.Creditor_ExitToTitle_Patch));
             
            // Side Labels
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(SideLabel_ShowAsPractice_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(SideLabel_ActivateAsTutorial_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(SideLabel_ShowAsEdited_Patch));

            // Rhythm & Gameplay
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(RhythmHandler.Dream_QueueHitWindow_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(RhythmHandler.Dream_QueueLeftHitWindow_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(RhythmHandler.Dream_QueueRightHitWindow_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(RhythmHandler.Dream_QueueLeftRightHitWindow_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(RhythmHandler.Dream_QueueHoldReleaseWindow_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(RhythmHandler.Dream_TriggerSong_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(RhythmHandler.DreamTutorial_TriggerState_Patch));

            // Level Editor
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.Daw_IncreaseBeat_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.Daw_DecreaseBeat_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.Daw_IncreaseBar_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.Daw_DecreaseBar_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.Daw_SetCodeOnBeat_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.Daw_RemoveCodeOnBeat_Patch));
            
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.CustomizeMenu_Activate_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.CustomizeMenu_Nav1_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.CustomizeMenu_Nav2_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.CustomizeMenu_Nav3_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.CustomizeMenu_Nav4_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.CustomizeMenu_Nav5_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.CustomizeMenu_Nav6_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.LvlEditor_Start_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.AdvancedMenu_Activate_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.AdvancedMenu_SwapTab_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.AdvancedMenu_NextRow_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.AdvancedMenu_PrevRow_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.AdvancedMenu_Increase_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.AdvancedMenu_Decrease_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.AdvancedMenu_Increment_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.AdvancedMenu_Diminish_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.TimelineTabs_Show_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.TimelineTabs_NextTab_Patch));
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(EditorHandler.TimelineTabs_PrevTab_Patch));

            MelonLogger.Msg("Melatonin Access Initialized");
        }

        public override void OnUpdate()
        {
            if (Keyboard.current == null) return;

            if (Keyboard.current.f2Key.wasPressedThisFrame)
            {
                bool enabled = ModConfig.ToggleRhythmCues();
                ScreenReader.Say(enabled ? Loc.Get("rhythm_cues_enabled") : Loc.Get("rhythm_cues_disabled"), true);
                MelonLogger.Msg($"Rhythm cue announcements {(enabled ? "enabled" : "disabled")}.");
            }

            if (Keyboard.current.f3Key.wasPressedThisFrame)
            {
                bool enabled = ModConfig.ToggleMenuPositions();
                ScreenReader.Say(enabled ? Loc.Get("menu_positions_enabled") : Loc.Get("menu_positions_disabled"), true);
                MelonLogger.Msg($"Menu position announcements {(enabled ? "enabled" : "disabled")}.");
            }

            if (Keyboard.current.f11Key.wasPressedThisFrame)
            {
                ContextHelpHandler.TryAnnounceContextHelp();
            }

            if (Keyboard.current.f12Key.wasPressedThisFrame)
            {
                DebugMode = ModConfig.ToggleDebugMode();
                ScreenReader.Say(DebugMode ? Loc.Get("debug_enabled") : Loc.Get("debug_disabled"), true);
                MelonLogger.Msg($"Debug mode {(DebugMode ? "enabled" : "disabled")}.");
            }
        }
    }
}
