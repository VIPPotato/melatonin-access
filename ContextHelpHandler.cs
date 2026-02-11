using HarmonyLib;
using UnityEngine;

namespace MelatoninAccess
{
    internal static class ContextHelpHandler
    {
        public static void TryAnnounceContextHelp()
        {
            string help = GetContextHelp();
            if (string.IsNullOrWhiteSpace(help)) return;
            ScreenReader.Say(help, true);
        }

        private static string GetContextHelp()
        {
            if (IsMapModeMenuOpen())
            {
                return Loc.Get("help_mode_menu", GetActionPrompt(), GetCancelPrompt());
            }

            if (IsOnMap())
            {
                return Loc.Get("help_map", GetActionPrompt());
            }

            if (IsResultsScreenOpen())
            {
                return Loc.Get("help_results", GetActionPrompt(), GetCancelPrompt());
            }

            if (IsSubmenuOpen())
            {
                return Loc.Get("help_menu", GetActionPrompt(), GetCancelPrompt());
            }

            if (LvlEditor.dir != null)
            {
                return Loc.Get("help_editor", GetActionPrompt(), GetCancelPrompt());
            }

            if (Dream.dir != null)
            {
                return Loc.Get("help_gameplay", GetPausePrompt());
            }

            if (SceneMonitor.mgr != null && SceneMonitor.mgr.GetActiveSceneName() == "TitleScreen")
            {
                return Loc.Get("help_title_screen", GetActionPrompt(), GetSwapPrompt());
            }

            return "";
        }

        private static bool IsMapModeMenuOpen()
        {
            if (!IsOnMap()) return false;
            var modeMenu = Map.env.Neighbourhood.McMap.ModeMenu;
            return modeMenu != null && modeMenu.CheckIsTranstioned();
        }

        private static bool IsOnMap()
        {
            return Map.env != null &&
                   Map.env.Neighbourhood != null &&
                   Map.env.Neighbourhood.McMap != null;
        }

        private static bool IsResultsScreenOpen()
        {
            if (Interface.env == null || Interface.env.Results == null) return false;
            return Traverse.Create(Interface.env.Results).Field("isEnabled").GetValue<bool>();
        }

        private static bool IsSubmenuOpen()
        {
            return Interface.env != null &&
                   Interface.env.Submenu != null &&
                   Interface.env.Submenu.CheckIsActivated();
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

        private static string GetCancelPrompt()
        {
            int ctrlType = ControlHandler.mgr != null ? ControlHandler.mgr.GetCtrlType() : 0;
            if (ctrlType == 1) return "B";
            if (ctrlType == 2) return "Circle";
            return Loc.Get("key_escape");
        }

        private static string GetSwapPrompt()
        {
            int ctrlType = ControlHandler.mgr != null ? ControlHandler.mgr.GetCtrlType() : 0;
            if (ctrlType == 1) return "Y";
            if (ctrlType == 2) return "Triangle";
            return Loc.Get("key_tab");
        }

        private static string GetPausePrompt()
        {
            int ctrlType = ControlHandler.mgr != null ? ControlHandler.mgr.GetCtrlType() : 0;
            if (ctrlType > 0) return Loc.Get("key_start");
            return Loc.Get("key_escape");
        }
    }
}
