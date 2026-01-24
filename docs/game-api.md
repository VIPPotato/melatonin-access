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

## Event Hooks
- `Menu.Next()` / `Menu.Prev()`: Good for announcing menu navigation.
- `Option.Enable()`: Called when an option is highlighted.
- `Submenu.Activate()`: Called when a menu opens.
