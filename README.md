# Melatonin Access

Screen-reader accessibility mod for **Melatonin** using **MelonLoader**.

## Quick Install

1. Install MelonLoader for Melatonin:
   https://github.com/LavaGang/MelonLoader.Installer/releases
2. Download the latest release ZIP, for example `MelatoninAccess-v1.2.0.zip`.
3. Open the ZIP, press `Ctrl+A`, press `Ctrl+C`, then paste everything into your Melatonin folder, the folder that contains `Melatonin.exe`.
4. Confirm these files exist after pasting:
   - `Mods/MelatoninAccess.dll`
   - `Mods/cutscene-ad/manifest.json`
   - `Mods/localization/loc.en.json`
   - `Tolk.dll`
   - `nvdaControllerClient32.dll`
   - `UserData/Loader.cfg`
5. Start the game and confirm you hear the mod loaded announcement.

Important:
- `Tolk.dll` and `nvdaControllerClient32.dll` must stay in the main game folder beside `Melatonin.exe`, not inside `Mods`.
- `cutscene-ad` and `localization` must stay inside the `Mods` folder.

## What It Adds

- Spoken menu navigation and option state announcements.
- Contextual tutorial and gameplay cue announcements.
- Tutorial, dialog, and popup reading.
- Map navigation support, including fast landmark teleport.
- Results and stage-end announcements, including lock reasons.
- Credits narration while entries scroll.
- Full level editor narration, including cursor, tools, advanced menu, and timeline tabs.
- Mod-generated speech in all game-supported languages.
- Toggleable announcement groups through `MelonPreferences`.

## Main Controls

- Title screen: press Action to begin. Press the language key, default `Tab`, to change language.
- Menus: `Up` and `Down` move, Action confirms, Cancel goes back.
- Map: `[` and `]` jump between landmarks.
- Map fallback: if Action is bound to `[` or `]`, use `F9` for previous and `F10` for next.
- Gamepad map jump: use `Action Left` and `Action Right`, commonly `LB` and `RB`.
- Map summary: press `F1` on map scenes to hear total stars collected and how many more are needed to pass.
- Context help: press `F11` to hear the available controls for the current screen.

## Hotkeys

- `F1`: on map scenes only, speak chapter and map star totals.
- `F2`: turn contextual cue announcements on or off.
- `F3`: turn menu position announcements on or off.
- `F11`: speak context help for the current screen.
- `F12`: turn debug logging on or off.

`F2`, `F3`, and `F12` save immediately and keep their state after restart.

## Languages

Mod-generated announcements follow the in-game language. Supported languages match the game language menu:

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

## Optional Settings

Settings are stored in `UserData/MelonPreferences.cfg` under the `MelatoninAccess` category.

- `AnnounceMapHotspots`: map arrival and teleport destination speech.
- `AnnounceRhythmCues`: contextual tutorial and gameplay cues.
- `AnnounceMenuPositions`: position context such as `1 of 4`.
- `AnnounceTutorialDialog`: tutorial and dialog narration.
- `AnnounceCreditsRoll`: credits title and scrolling names.
- `DebugModeEnabled`: debug logging state used by `F12`.

Note:
- The config key `AnnounceRhythmCues` is a legacy name. It controls contextual cues.

## For Maintainers

### Build And Deploy

Use the wrapper scripts instead of raw `dotnet build`.

Build only:

```powershell
pwsh -File .\scripts\Build-Mod.ps1
```

Build and copy the mod into the game `Mods` folder:

```powershell
pwsh -File .\scripts\Deploy-Mod.ps1
```

The local scripts currently default to:

```text
L:\SteamLibrary\steamapps\common\Melatonin
```

If your install is somewhere else, pass `-GamePath`.

### Build A Release ZIP

```powershell
pwsh -File .\scripts\Build-ReleasePackage.ps1 -Version "v1.2.0"
```

This creates a copy-paste-ready ZIP with this runtime layout:

- `Mods/MelatoninAccess.dll`
- `Mods/cutscene-ad/manifest.json`
- `Mods/cutscene-ad/scripts/*.json`
- `Mods/localization/loc.<lang>.json`
- `Tolk.dll`
- `nvdaControllerClient32.dll`
- `UserData/Loader.cfg`

The release ZIP intentionally leaves out development docs, logs, and regression scripts.

### QA Scripts

Speech regression check:

```powershell
pwsh -File .\scripts\Test-SpeechRegression.ps1
```

Localization coverage check:

```powershell
pwsh -File .\scripts\Test-LocalizationQA.ps1
```

Cutscene audio-description data check:

```powershell
pwsh -File .\scripts\Test-CutsceneAdPipeline.ps1
```

For cutscene authoring details, see `docs/cutscene-ad-pipeline.md`.

### Research Tool

Extract Unity assets from the installed game:

```powershell
python .\scripts\extract_unity_assets.py --output-dir .\artifacts\asset-extract
```

## Support

If you like the project, you can support it here:
https://buymeacoffee.com/potatophones

## Thanks

- Thanks to **luyi** for testing and Chinese localization fixes.
- Thanks to **dreamburguer** for Spanish localization fixes.
