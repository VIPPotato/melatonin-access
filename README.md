# Melatonin Access

Screen-reader accessibility mod for **Melatonin** (MelonLoader).

## What This Mod Adds

- Spoken menu navigation and option state announcements.
- Rhythm cue announcements in gameplay.
- Tutorial/dialog and popup reading.
- World map navigation support, including fast landmark teleport.
- Level editor narration (cursor, tools, advanced menu, timeline tabs).
- Results and stage-end option announcements, including lock-state reasons.
- Credits roll narration while entries scroll.
- Localization for mod-generated messages across all game-supported languages.

## Navigation and Controls

- General menu navigation:
  - Move with arrow keys.
  - Confirm with your Action key (default `Space`).
  - Cancel/back with your Cancel key (default `Esc`).
- Map navigation:
  - Use `[` and `]` to jump to previous/next landmark.
  - If your Action key is bound to `[` or `]`, use `F9` (previous) and `F10` (next) for teleport.
  - Open a landmark mode menu with Action.
  - Move between mode choices with `Up`/`Down` and confirm with Action.
- Results/stage-end menu:
  - Move with `Up`/`Down`.
  - Confirm with Action.

Example flow:
- Use `[` or `]` to pick a nearby map landmark.
- Press `Up`/`Down` to choose the mode.
- Press Action to start.

## Hotkeys

- `F1`: Re-read mod status/help announcement.
- `F12`: Toggle debug logging on/off.

## Localization

Mod-generated announcements are localized to the same language selected in-game.
Supported language set matches the game language menu:

- English
- Simplified Chinese
- Traditional Chinese
- Japanese
- Korean
- Vietnamese
- French
- German
- Spanish
- Portuguese

## Build

```bash
dotnet build MelatoninAccess.csproj
```

The project is configured to auto-copy `MelatoninAccess.dll` to your game `Mods` folder after build when `ModsPath` is valid in `MelatoninAccess.csproj`.

## Install (Manual)

If auto-copy is not configured, copy:

- `bin/Debug/net472/MelatoninAccess.dll`

to:

- `<Melatonin folder>/Mods/`

## Requirements

- Melatonin (PC)
- MelonLoader
- Tolk screen reader bridge (`Tolk.dll`) available to the mod runtime
