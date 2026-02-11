# Melatonin Game API

## Overview
- **Engine:** Unity 2020.2.3f1
- **Mod Loader:** MelonLoader

## Singleton Access Points
- `Interface.env`: Main UI entry point.
- `ControlHandler.mgr`: Input management (Unity Input System).
- `SaveManager.mgr`: Game settings, localization, save data.
- `SceneMonitor.mgr`: Scene tracking.
- `TutorialWorld.env`: Tutorial specific management.

## Game Key Bindings
- **Movement/Menu Navigation**: Arrow keys or WASD (configurable).
- **Action/Select**: Space (default, rebindable).
- **Cancel/Back**: Escape or R (default, rebindable).
- **Pause**: Escape or Start button.
- **Gameplay**: Space, Arrow keys, WASD, and others depending on level.
- **Action-key token source**: `SaveManager.mgr.GetActionKey()` returns tokens written by `ControlHandler.GetKeyFromKeyControl` (`SPACE`, `ENTER`, letters, `[`, `]`, `PERIOD`, `SLASH`, etc.), useful for spoken prompt localization.

## Safe Mod Keys
- `F1` - `F12`: Unused by the game.
- `0` - `9`: Unused by the game.

## UI System
- **Base Class**: `Wrapper` -> `Custom` -> `MonoBehaviour`.
- **Interface**: `Interface.env` manages submenus.
- **Menus**: `Submenu` contains a list of `Menu` objects.
- **Menu Items**: `Menu` contains a list of `Option` objects.
- **Text**: `Option` uses `textboxFragment` for labels (`label`, `num`, `tip`).
- **Localization**: `textboxFragment.SetState(int)` uses `SaveManager.GetLang()` to pick translations.

## Game Mechanics
- Rhythm-based gameplay.
- Different worlds/dreams represented by `[Name]World` classes.
- Tutorial: `Dream_tutorial` class and `TutorialWorld.env`.

## Dream GameMode Mapping
- `Dream.gameMode` values used at level start (`Dream.Start()`):
  - `0`: Practice (`SideLabel.ShowAsPractice`, no score meter).
  - `1`: Score mode.
  - `2`: Hard mode.
  - `3`: Score remix mode.
  - `4`: Hard remix mode.
  - `5`: Tutorial mode.
  - `6`: Editor test mode (`SideLabel.ShowAsEdited` path).
  - `7`: Community/downloaded level mode.
- References: `decompiled/Dream.cs:159-203`, `decompiled/Dream.cs:1698-1700`.

## Tutorial Analysis
- **Scene/entry point**: Tutorial runs in `Dream_tutorial`.
  - Start path: `Option` functions call `Interface.env.ExitTo("Dream_tutorial")` with `Dream.SetGameMode(5)`.
  - References: `decompiled/Option.cs:870`, `decompiled/Option.cs:993`.
- **How tutorial is started in UI**:
  - Menu label key `tutorial` (`Option` state name).
  - References: `decompiled/Option.cs:342`, `decompiled/Submenu.cs:220`.
- **Can tutorial be skipped?**
  - Yes, `skipTutorial` exists as an option label and appears in asset text tables.
  - References: `decompiled/Option.cs:353`, `Melatonin_Data/level0` text table.
- **Tutorial interaction model**:
  - State machine progression (`state` 0..19) in `Dream_tutorial`.
  - Action press advances state; extra press with sub-info open rewinds/restarts sections.
  - References: `decompiled/Dream_tutorial.cs:54-75`, `decompiled/Dream_tutorial.cs:147-420`.
- **Tutorial text source model**:
  - Dialog/menu text is read through `textboxFragment.states[].translations[SaveManager.GetLang()]`.
  - Most full dialog strings are serialized in Unity assets, not plain `.cs` code.
  - References: `decompiled/textboxFragment.cs:14`, `decompiled/textboxFragment.cs:96-106`.
- **Extracted tutorial/help texts**:
  - See `docs/tutorial-texts.md` for the extracted text list with source context.
  - Deep asset scan (`Melatonin_Data/level0`) recovered additional guidance lines like metronome/timing-circle help, score-mode readiness, and auto-restart penalty behavior.

## Event Hooks
- `Menu.Next()` / `Menu.Prev()`: Good for announcing menu navigation.
- `Option.Enable()`: Called when an option is highlighted.
- `Submenu.Activate()`: Called when a menu opens.

## Language API Notes
- `SaveManager.GetLang()` returns current language index (`playerData.lang`).
- `SaveManager.mgr.SetLang(int)` updates language from `LangMenu.Select()`.
- Current project assumes index mapping:
  - `0` English
  - `1` Simplified Chinese
  - `2` Traditional Chinese
  - `3` Japanese
  - `4` Korean
  - `5` Vietnamese
  - `6` French
  - `7` German
  - `8` Spanish
  - `9` Portuguese
- References: `decompiled/SaveManager.cs:978-986`, `decompiled/LangMenu.cs:137-144`, `decompiled/LangMenu.cs:255-317`.

## Editor-Specific Hooks
- `AdvancedMenu`:
  - Open/close: `Activate()`, `Deactivate()`.
  - Navigation: `NextRow()`, `PrevRow()`, `SwapTab()`.
  - Value changes: `Increase()`, `Decrease()`, `Increment()`, `Diminish()`.
  - Useful state getters: `GetTabNum()`, `GetRowNum()`, `CheckIsCustomized()`.
- `TimelineTabs`:
  - `Show()`, `NextTab()`, `PrevTab()`, `GetCharType()`.
- `LvlEditor.Update()` routes all editor input through these methods, so patching them is safer than patching raw input.
- References: `decompiled/AdvancedMenu.cs`, `decompiled/TimelineTabs.cs`, `decompiled/LvlEditor.cs:182-337`.

## Lock-State Logic
- **Map mode locks** (in `Landmark.Update()`):
  - Option index `1`: requires `starScore > 0` or remix landmark.
  - Option index `2`: requires `starScore >= 2`.
  - Option index `3`: requires `starScore >= 2` and full game (`Builder.mgr.CheckIsFullGame()`).
  - Failed action plays only blocked SFX by default.
  - For simple "can pass" checks on a standard landmark, the effective threshold is `1` star (`starScore > 0`).
- **Stage end locks**:
  - In `StageEndMenu.Show(gameMode)`, icon/label for index `0` is locked in modes `1` and `3` when chapter score `< 2`.
  - In `Results.Update()`, selecting locked paths only plays blocked SFX.
- References: `decompiled/Landmark.cs:64-87`, `decompiled/ModeMenu.cs:179-220`, `decompiled/StageEndMenu.cs:78-177`, `decompiled/Results.cs:182-214`.

## Credits Flow
- Credits sequence is driven by `Creditor.Starting()`:
  - `Credits.Show()` -> logo transitions -> `Credits.ScrollList()` -> exit to title.
- Scroll duration is exposed by `Credits.GetScrollDuration()` and the actual list animation is `lister.TriggerAnim("scroll")`.
- References: `decompiled/Creditor.cs:18-39`, `decompiled/Credits.cs:102-126`.

## Key-Rebind Collision Note
- The game allows rebinding Action to `[` and `]` (`ControlHandler.GetStringFromKey` / rebind checks), which can conflict with mod map teleport if teleport also uses brackets.
- References: `decompiled/ControlHandler.cs:157-163`, `decompiled/ControlHandler.cs:357-358`, `decompiled/ControlHandler.cs:458-464`.

## Mode Menu Dream Context
- `ModeMenu` stores the currently selected map dream in private field `dreamName`.
- `dreamName` is assigned at the start of `ModeMenu.Transitioning(newDreamName, ...)`, before `isTransitioned` is set.
- For mode-menu opening narration, mods can read `dreamName` via reflection and prepend a localized "Dream about {Level}" context line.
- References: `decompiled/ModeMenu.cs:38`, `decompiled/ModeMenu.cs:155-158`, `decompiled/ModeMenu.cs:226`.

## Input-Source Switching Note
- `ControlHandler.Update()` sets `ctrlType = 0` (keyboard) on any keyboard key press.
- Gamepad-only helpers in mods should not depend exclusively on `GetCtrlType() > 0`, because controller hardware input can still be active while `ctrlType` is temporarily keyboard.
- Safer approach for map navigation helpers: read shoulder/trigger states from `Gamepad.current` directly when you only want gamepad actions.
- References: `decompiled/ControlHandler.cs:57-70`, `decompiled/ControlHandler.cs:535-585`.

## Map Progress Summary
- `TotalBox` visual map summary uses chapter totals from `SaveManager.GetChapterEarnedStars(Chapter.GetActiveChapterNum())` (plus rings/perfects).
- Map activation checks use an 8-star threshold on chapters 1-4 (`GetChapterEarnedStars(chapter) >= 8`) for chapter map progression/remix gating.
- References: `decompiled/TotalBox.cs:28-30`, `decompiled/Map.cs:66-83`.

## Intro/Outro Cutscene Entry Points
- Chapter intro replay path:
  - `Option` case `33` sets `Chapter.ToggleIsEnteringWithIntro(true)` and exits to `Chapter_{chapterNum}`.
  - References: `decompiled/Option.cs:1065-1067`.
- Chapter outro replay path:
  - `Option` case `34` sets `Chapter.ToggleIsEnteringWithOutro(true)` and exits to `Chapter_{chapterNum}` when unlocked.
  - References: `decompiled/Option.cs:1070-1080`.
- Intro timeline implementation:
  - `Chapter.EnteringWithIntro()` coroutine, chapter-specific branches for chapter 1-5.
  - References: `decompiled/Chapter.cs:101-244`.
- Outro timeline implementation:
  - `Chapter.ExitingToNextChapter()` coroutine, chapter-specific branches for chapter 1-5.
  - References: `decompiled/Chapter.cs:319-451`.
