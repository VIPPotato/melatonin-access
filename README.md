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
- Optional toggles for announcement groups via `MelonPreferences`.
- A regression-check script for validating speech logs after playtests.

## Navigation and Controls

- Title/start screen:
  - Press Action (default `Space`) to begin.
  - Press language key (default `Tab`) to change language.
- General menu navigation:
  - Move with `Up`/`Down`.
  - Confirm with Action (default `Space`).
  - Cancel/back with Cancel (default `Esc`).
- Map navigation:
  - Keyboard: `[` and `]` jump to previous/next landmark.
  - Keyboard fallback: if Action is bound to brackets, use `F9` (previous) and `F10` (next).
  - Gamepad: `Action Left` / `Action Right` (commonly `LB` / `RB`) jump between landmarks.
  - Open mode menu with Action, then choose mode with `Up`/`Down` and confirm with Action.
- Results/stage-end menu:
  - Move with `Up`/`Down`.
  - Confirm with Action.

Example flow:
- Use `[` or `]` to pick a nearby map landmark.
- Press `Up`/`Down` to choose the mode.
- Press Action to start.

## Hotkeys

- `F2`: Toggle rhythm cue announcements on/off (saved immediately to MelonPreferences).
- `F3`: Toggle menu position announcements on/off (for example `1 of 4`, saved immediately to MelonPreferences).
- `F12`: Toggle debug logging on/off.
  - State is persisted in MelonPreferences and restored on next launch.

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

## Optional Settings (MelonPreferences)

Settings are stored in `UserData/MelonPreferences.cfg` under category `MelatoninAccess`.

- `AnnounceMapHotspots` (default `true`): map arrival and teleport destination/star lines.
- `AnnounceRhythmCues` (default `true`): gameplay rhythm cues (`Space`, `Left`, `Right`, `Both`, `Hold`).
  - Can be toggled in-game with `F2`.
- `AnnounceMenuPositions` (default `true`): menu position context (`1 of N`) across supported menus.
  - Can be toggled in-game with `F3`.
- `AnnounceTutorialDialog` (default `true`): tutorial and dialog narration.
- `AnnounceCreditsRoll` (default `true`): credits title and scrolling names.
- `DebugModeEnabled` (default `false`): debug logging state used by `F12`.

## Build

```bash
dotnet build MelatoninAccess.csproj
```

The project is configured to auto-copy `MelatoninAccess.dll` to your game `Mods` folder after build when `ModsPath` is valid in `MelatoninAccess.csproj`.

## Release Package

Create release ZIP (mod + required screen-reader DLLs only):

```powershell
pwsh -File .\scripts\Build-ReleasePackage.ps1 -Version "v1.0.2"
```

Package contents:

- `Mods/MelatoninAccess.dll`
- `Tolk.dll`
- `nvdaControllerClient32.dll`

The release ZIP intentionally excludes development docs/tools (for example changelog files and regression test scripts).

## Install (Release ZIP - Recommended)

1. Install MelonLoader for Melatonin:
   - https://github.com/LavaGang/MelonLoader.Installer/releases
2. Download `MelatoninAccess-v1.0.2.zip`.
3. Extract/copy all files from the ZIP into your Melatonin game folder (the folder with `Melatonin.exe`).
4. Confirm these files exist:
   - `<Melatonin folder>/Mods/MelatoninAccess.dll`
   - `<Melatonin folder>/Tolk.dll`
   - `<Melatonin folder>/nvdaControllerClient32.dll`
5. Start the game and confirm you hear the mod loaded announcement.

Important:

- `Tolk.dll` and `nvdaControllerClient32.dll` must be in the main game folder, not in `Mods`.

## Install (From Source Build)

1. Build:

```bash
dotnet build MelatoninAccess.csproj
```

2. If auto-copy is not configured, copy:
   - `bin/Debug/net472/MelatoninAccess.dll` to `<Melatonin folder>/Mods/`
   - `libs/x86/Tolk.dll` to `<Melatonin folder>/`
   - `libs/x86/nvdaControllerClient32.dll` to `<Melatonin folder>/`

## Support

If you like what I do, I would appreciate a coffee:
https://buymeacoffee.com/potatophones

## Speech Regression Check (After Playtest)

1. Enable debug logs with `F12` in-game.
2. Playtest the areas you changed.
3. Run:

```powershell
pwsh -File .\scripts\Test-SpeechRegression.ps1
```

Useful options:

```powershell
pwsh -File .\scripts\Test-SpeechRegression.ps1 -LogPath "D:\games\steam\steamapps\common\Melatonin\MelonLoader\Latest.log"
pwsh -File .\scripts\Test-SpeechRegression.ps1 -RequiredPattern "Tutorial\. Press .+ to skip\."
```

## Requirements

- Melatonin (PC)
- MelonLoader
- Tolk screen reader bridge (`Tolk.dll`) available to the mod runtime
