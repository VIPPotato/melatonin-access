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
