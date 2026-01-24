# Codebase Summary

This file contains a summary of the analyzed files in the `decompiled` directory.

## Core System
- **Custom.cs**: Base class for almost all MonoBehaviours in the game. Provides utility methods for Transform manipulation (`SetPosition`, `SetLocalX`) and Coroutine management.
- **Wrapper.cs**: Inherits from `Custom`. Likely a base class for UI wrappers or scene containers.
- **Manager.cs**: Inherits from `Custom`. Base class for singleton managers.
- **ControlHandler.cs**: Singleton (`mgr`). Handles Input (Keyboard/Gamepad). Detects controller type. Maps keys to actions.
- **SaveManager.cs**: Singleton (`mgr`). Manages `playerData` and `editorData`. Handles serialization (JSON). Stores settings (Volume, Language, Accessibility flags like `isVisualAssisting`).
- **SceneMonitor.cs**: Manages scene loading and transitions.
- **Technician.cs**: Handles Audio/Video settings (Resolution, VSync, Volume).
- **SteamManager.cs**: Handles Steam API integration.

## UI System
- **Interface.cs**: Singleton (`env`). Main entry point for UI. Manages `Cam`, `Submenu`, `Fader`, `Letterbox`, `SideLabel`.
- **Submenu.cs**: Manages a stack/list of `Menu` objects. Handles entering/exiting submenus.
- **Menu.cs**: Represents a single menu screen (e.g., Main Menu, Settings). Contains a list of `Option`s. Handles navigation (`Next`, `Prev`).
- **Option.cs**: Represents a single interactive item in a menu. Contains text labels (`textboxFragment`) and logic for what happens when selected (`Select`). Handles Toggles/Switches via `lightSwitch` sprite.
- **DialogBox.cs**: Displays speech bubbles. Used heavily in Tutorial (`Dream_tutorial.cs`) and Story. Methods: `SetText`, `ChangeDialogState`.
- **LangMenu.cs**: Language selection screen.
- **CommunityMenu.cs**: Handles the "Downloaded Levels" screen. Fetches data from Steam Workshop (`SteamWorkshop.mgr`).
- **ScoreStars.cs**, **ScoreMeter.cs**, **Results.cs**: Handle the results screen after a level.
- **ExtraMessage.cs**: Handles popup messages (likely warnings or hints). Uses `textboxFragment`.
- **ConfirmModal.cs**: Confirmation popup (e.g., Quit Game). Contains prompts and text.
- **RebindModal.cs**: Key rebinding popup.
- **Notification.cs**: Visual notification (sprites only, no text).
- **Instruction.cs**: "Press Space to Start" text on Title Screen.
- **SideLabel.cs**: The text on the side during gameplay (e.g., "Practice", "Tutorial").
- **AchievementsMenu.cs**: Displays achievements list using `CheevoRow` and `ScrollingBar`.
- **SteamWorkshop.cs**: Manages uploading and downloading levels via Steam UGC.

## Game Mechanics (Dreams)
- **Dream.cs**: Base class for all levels. Handles game modes (Practice, Normal). Triggers `SideLabel` for Practice mode.
- **Dream_tutorial.cs**: The tutorial level. Uses `DreamWorld.env.DialogBox` extensively to show instructions based on a state machine.
- **Dream_*.cs**: Individual levels (e.g., `Dream_food.cs`, `Dream_tech.cs`). Each contains specific logic for that rhythm game. In Practice Mode (`gameMode == 0`), they use `DreamWorld.env.DialogBox` for initial instructions and `SideLabel` for persistent prompts ("Press Tab to Skip").
- **McMap.cs**: The player character on the level select map. Handles movement (`Update`).
- **Landmark.cs**: The interactive nodes on the map (Levels). Triggered via `OnTriggerEnter2D`.
- **Neighbourhood.cs**: Container for the map elements (`McMap`, `Landmark`s).
- **ModeMenu.cs**: The popup menu when entering a level on the map (Standard, Practice, Hard).

## Level Editor (LvlEditor / DAW)
- **LvlEditor.cs**: Main controller for the editor. Handles input for navigating the DAW (timeline) and switching modes.
- **EditorUI.cs**: Wrapper for Editor UI.
- **Daw.cs**: Digital Audio Workstation interface. Manages the timeline of bars.
- **BarSlot.cs**: Represents a single bar in the timeline. Contains beat data.
- **CustomizeMenu.cs**: Menu for selecting rhythm actions to place in the DAW.

## Visuals & Animation
- **Fragment.cs**: Base class for animated/audio elements. Handles syncing sound to rhythm (`TriggerSoundDelayedTimeStarted`).
- **spriteFragment.cs**: Specialized fragment for switching sprites (icons, toggles).
- **Fader.cs**: Screen transition fader.
- **Letterbox.cs**: Cinematic bars.

## Accessibility Findings
- **Toggles**: Logic is scattered in `Option.Select/Reverse`. Switches use `lightSwitch.SetState(1)` for ON.
- **Localization**: Handled by `SaveManager.GetLang()`. `textboxFragment` selects text based on this index.
- **Input**: `ControlHandler` uses `InputSystem` but wraps it.
- **Map**: `McMap` moves freely. `Landmark` triggers on collision. `ModeMenu` pops up for level selection.
- **Editor**: Complex grid navigation. Needs hooks in `Daw.cs` (beat movement) and `LvlEditor.cs`.
- **Instructions**:
    - **Title**: `Instruction.cs` (Press Space) / `LangHint.cs` (Language Hint).
    - **Tutorial/Story**: `DialogBox.cs` (Main text bubbles).
    - **Practice**: `SideLabel.cs` (Persistent "Practice" label and "Swap Mode" prompt).
    - **Popups**: `ExtraMessage.cs`.
    - **Teaser**: `CallToAction.cs`.

## Pending Analysis
- Specific accessibility implementation details for `SideLabel` (needs `SideLabelHandler`).
