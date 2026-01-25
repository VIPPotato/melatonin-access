using HarmonyLib;
using MelonLoader;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace MelatoninAccess
{
    public static class MenuHandler
    {
        private static float lastTitleTime = 0f;

        // --- Menu Title ---
        [HarmonyPatch(typeof(MenuTitle), "Activate")]
        public static class MenuTitle_Activate_Patch
        {
            public static void Postfix(MenuTitle __instance)
            {
                if (__instance.title != null)
                {
                    var tmp = __instance.title.GetComponent<TextMeshPro>();
                    if (tmp != null)
                    {
                        lastTitleTime = Time.time;
                        ScreenReader.Say(tmp.text + " Menu", true);
                    }
                }
            }
        }

        // --- Option Navigation ---
        [HarmonyPatch(typeof(Option), "Enable")]
        public static class Option_Enable_Patch
        {
            public static void Postfix(Option __instance)
            {
                if (__instance.label != null)
                {
                    var tmp = __instance.label.GetComponent<TextMeshPro>();
                    if (tmp != null)
                    {
                        string text = tmp.text;
                        
                        int functionNum = Traverse.Create(__instance).Field("functionNum").GetValue<int>();
                        int chapterNum = __instance.chapterNum; 

                        if (functionNum == 9)
                        {
                            if (SaveManager.mgr.GetChapterNum() < chapterNum)
                            {
                                text = $"Chapter {chapterNum} Locked";
                            }
                        }

                        // Check if it's a slider
                        bool isSlider = false;
                        if (__instance.num != null && __instance.num.CheckIsMeshRendered())
                        {
                            var numTmp = __instance.num.GetComponent<TextMeshPro>();
                            if (numTmp != null && !string.IsNullOrEmpty(numTmp.text))
                            {
                                if (numTmp.text.Trim() != "9")
                                {
                                    text += " slider " + numTmp.text;
                                    isSlider = true;
                                }
                            }
                        }

                        if (!isSlider && __instance.tip != null && __instance.tip.CheckIsMeshRendered())
                        {
                            var tipTmp = __instance.tip.GetComponent<TextMeshPro>();
                            if (tipTmp != null && !string.IsNullOrEmpty(tipTmp.text))
                            {
                                text += ". " + tipTmp.text;
                            }
                        }
                        
                        // Append Switch State (On/Off)
                        if (__instance.lightSwitch != null && __instance.lightSwitch.CheckIsSpriteRendered())
                        {
                            bool state = false;
                            bool known = true;
                            switch (functionNum)
                            {
                                case 12: // Fullscreen
                                    text += ": " + (Screen.fullScreen ? "Fullscreen" : "Windowed");
                                    known = false;
                                    break;
                                case 18: // Visual Assist
                                    state = SaveManager.mgr.CheckIsVisualAssisting();
                                    break;
                                case 19: // Audio Assist
                                    state = SaveManager.mgr.CheckIsAudioAssisting();
                                    break;
                                case 37: // WASD
                                    state = SaveManager.mgr.CheckIsDirectionKeysAlt();
                                    break;
                                case 41: // Vibration
                                    state = !SaveManager.mgr.CheckIsVibrationDisabled();
                                    break;
                                case 42: // Wiggle Room
                                    state = SaveManager.mgr.CheckIsBiggerHitWindows();
                                    break;
                                case 44: // Warmth
                                    state = SaveManager.mgr.CheckIsWarmth();
                                    break;
                                case 45: // Easy Scoring
                                    state = SaveManager.mgr.CheckIsEasyScoring();
                                    break;
                                case 47: // Perfects Only
                                    state = SaveManager.mgr.CheckIsPerfectsOnly();
                                    break;
                                case 50: // V-Sync
                                    state = SaveManager.mgr.CheckIsVsynced();
                                    break;
                                default:
                                    known = false;
                                    break;
                            }
                            
                            if (known)
                            {
                                text += ": " + (state ? "On" : "Off");
                            }
                        }

                        // Position in Menu (X of Y)
                        if (__instance.transform.parent != null)
                        {
                            var menu = __instance.transform.parent.GetComponent<Menu>();
                            if (menu != null)
                            {
                                var functioningOptions = Traverse.Create(menu).Field("functioningOptions").GetValue<List<Option>>();
                                if (functioningOptions != null)
                                {
                                    int index = functioningOptions.IndexOf(__instance);
                                    int count = functioningOptions.Count;
                                    if (index >= 0)
                                    {
                                        text += $", {index + 1} of {count}";
                                    }
                                }
                            }
                        }

                        // If menu title was just announced (< 0.5s ago), queue this announcement.
                        // Otherwise (navigation), interrupt.
                        bool interrupt = (Time.time - lastTitleTime > 0.5f);
                        ScreenReader.Say(text, interrupt);
                    }
                }
            }
        }

        // --- Option Interaction ---
        [HarmonyPatch(typeof(Option), "Select")]
        public static class Option_Select_Patch
        {
            public static void Postfix(Option __instance)
            {
                OptionHelper.AnnounceValueChange(__instance);
            }
        }

        [HarmonyPatch(typeof(Option), "Reverse")]
        public static class Option_Reverse_Patch
        {
            public static void Postfix(Option __instance)
            {
                OptionHelper.AnnounceValueChange(__instance);
            }
        }

        public static class OptionHelper
        {
            public static void AnnounceValueChange(Option option)
            {
                int functionNum = Traverse.Create(option).Field("functionNum").GetValue<int>();

                if (option.num != null && option.num.CheckIsMeshRendered())
                {
                    var numTmp = option.num.GetComponent<TextMeshPro>();
                    if (numTmp != null && numTmp.text.Trim() != "9")
                    {
                        ScreenReader.Say("Slider " + numTmp.text, true);
                        return;
                    }
                }

                bool state = false;
                bool isToggle = false;

                switch (functionNum)
                {
                    case 12: 
                        ScreenReader.Say(Screen.fullScreen ? "Fullscreen" : "Windowed", true);
                        return;
                    case 18: 
                        isToggle = true;
                        state = SaveManager.mgr.CheckIsVisualAssisting();
                        break;
                    case 19: 
                        isToggle = true;
                        state = SaveManager.mgr.CheckIsAudioAssisting();
                        break;
                    case 37: 
                        isToggle = true;
                        state = SaveManager.mgr.CheckIsDirectionKeysAlt();
                        break;
                    case 41: 
                        isToggle = true;
                        state = !SaveManager.mgr.CheckIsVibrationDisabled();
                        break;
                    case 42: 
                        isToggle = true;
                        state = SaveManager.mgr.CheckIsBiggerHitWindows();
                        break;
                    case 44: 
                        isToggle = true;
                        state = SaveManager.mgr.CheckIsWarmth();
                        break;
                    case 45: 
                        isToggle = true;
                        state = SaveManager.mgr.CheckIsEasyScoring();
                        break;
                    case 47: 
                        isToggle = true;
                        state = SaveManager.mgr.CheckIsPerfectsOnly();
                        break;
                    case 50: 
                        isToggle = true;
                        state = SaveManager.mgr.CheckIsVsynced();
                        break;
                }

                if (isToggle)
                {
                     ScreenReader.Say(state ? "On" : "Off", true);
                }
            }
        }
    }
}