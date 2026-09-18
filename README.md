# Melatonin Access

Screen-reader accessibility mod for **Melatonin** using **MelonLoader**.

## Install On Windows

1. Install MelonLoader for Melatonin:
   https://github.com/LavaGang/MelonLoader.Installer/releases
2. Download the latest release ZIP, for example `MelatoninAccess-v1.3.0.zip`.
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

## Install On macOS

Speech goes through VoiceOver. Turn VoiceOver on before you start the game.

Your Melatonin folder is:

```text
~/Library/Application Support/Steam/steamapps/common/Melatonin
```

### The easy way

The macOS release ZIP contains an installer. Download and open
`MelatoninAccess-v1.3.0-macOS.zip`, then **open `install-macOS.command`**. In
Finder, select it and press Command-O, or use VoiceOver's Open command.

It finds the game, installs the mod, and unblocks everything so macOS will
load it. Then it shows you the one remaining step, which has to be done in
Steam itself: the launch options line appears in a text field you can read
and copy from, with a Copy To Clipboard button if you want it on the
clipboard.

If MelonLoader is not installed yet, the installer offers to open the
download page for you, and then lets you pick the downloaded ZIP from a file
dialog. It does not go looking through your Downloads folder, because macOS
would ask Terminal for permission to read it.

If Finder refuses to run it, right-click it, choose Open, and confirm.

If you extracted the ZIP into Downloads, Desktop or Documents, macOS may stop
the installer reading the mod files out of it. The installer notices and asks
you to choose the folder yourself, which is what grants permission. Extracting
somewhere else, such as your home folder, avoids it entirely.

If you would rather do it by hand, or the installer cannot find something,
the full steps are below.

### Step 1: Install MelonLoader

**Download the ZIP, not the DMG.** MelonLoader's macOS installer is a `.dmg`
file, and macOS refuses to open it with a message saying it is damaged. The
file is fine. macOS blocks it because the app is not signed by an Apple
developer account. The ZIP avoids the problem entirely.

1. Go to https://github.com/LavaGang/MelonLoader/releases
2. Download **`MelonLoader.macOS.x64.zip`**. Do not download
   `MelonLoader.Installer.MacOS.dmg`.

   This is the correct file on both Intel and Apple Silicon Macs. Steam runs
   Melatonin in Intel mode on Apple Silicon, so the x64 build is the one that
   matches. There is no separate Apple Silicon download.
3. Open the ZIP. Copy these three items into your Melatonin folder, beside
   `Melatonin.app`:
   - `MelonLoader.Bootstrap.dylib`
   - `melonloader-launch.sh`
   - the `MelonLoader` folder
4. Open Terminal and run these two commands. The first lets macOS load the
   files you just copied, the second makes the launch script runnable:

   ```bash
   xattr -dr com.apple.quarantine ~/Library/Application\ Support/Steam/steamapps/common/Melatonin
   chmod +x ~/Library/Application\ Support/Steam/steamapps/common/Melatonin/melonloader-launch.sh
   ```

### Step 2: Tell Steam to use MelonLoader

Steam will not load the mod on its own. You have to point it at MelonLoader's
launch script.

1. In Steam, right-click **Melatonin**, choose **Properties**, then **General**.
2. Find the **Launch Options** box and paste this in, replacing `YOURNAME`
   with your Mac user name:

   ```text
   "/Users/YOURNAME/Library/Application Support/Steam/steamapps/common/Melatonin/melonloader-launch.sh" %command%
   ```

   Type the path out in full. Steam does not understand `~` here, and a
   shortened path will fail.
3. Close the window.

### Step 3: Install the mod

1. Download the macOS release ZIP, for example
   `MelatoninAccess-v1.3.0-macOS.zip`.
2. Open the ZIP and copy the `Mods` and `UserData` folders into your
   Melatonin folder.
3. Confirm these files exist after copying:
   - `Mods/MelatoninAccess.dll`
   - `Mods/libprism.dylib`
   - `Mods/cutscene-ad/manifest.json`
   - `Mods/localization/loc.en.json`
   - `UserData/Loader.cfg`
4. Run this command so macOS will load the speech library:

   ```bash
   xattr -dr com.apple.quarantine ~/Library/Application\ Support/Steam/steamapps/common/Melatonin/Mods
   ```

5. Start Melatonin from Steam. You should hear the mod loaded announcement.

On macOS the speech library is `Mods/libprism.dylib`, and it goes **inside**
the `Mods` folder. There is no `Tolk.dll` or `nvdaControllerClient32.dll` on
macOS; those are Windows only.

### If the game starts but says nothing

Almost always one of two things. Check the MelonLoader log:

```bash
tail -30 ~/Library/Application\ Support/Steam/steamapps/common/Melatonin/MelonLoader/Latest.log
```

- **The log has no entry from this launch.** Steam did not use MelonLoader.
  Re-check the Launch Options in Step 2, especially the quotes, the full
  path, and the ` %command%` at the end.
- **The log says the speech library could not be loaded.** macOS is still
  blocking `libprism.dylib`. Run the `xattr` command from Step 3 again, or
  open System Settings, go to Privacy & Security, and choose **Allow Anyway**
  next to the blocked file.

When it is working, the log contains:

```text
Prism (VoiceOver (macOS)) loaded successfully
```

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
- Map summary: press `F1` on map scenes to hear total stars, rings, and perfect runs. On chapters 1-4 it also says how many more stars are needed to pass.
- Context help: press `F11` to hear the available controls for the current screen.

## Hotkeys

- `F1`: on map scenes only, speak chapter progress totals for stars, rings, and perfect runs.
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

#### Building on macOS

The PowerShell scripts are Windows only. On macOS, build with Mono's msbuild
directly. No .NET SDK is needed.

```bash
msbuild MelatoninAccess.csproj /t:Restore
msbuild MelatoninAccess.csproj /t:Build /p:Configuration=Release
```

The project finds the game automatically in the stock Steam location. Point
it elsewhere with `/p:GameRoot="/path/to/Melatonin"`. The build copies the
mod into the game's `Mods` folder when that folder exists.

### Build A Release ZIP

```powershell
pwsh -File .\scripts\Build-ReleasePackage.ps1 -Version "v1.3.0"
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

#### macOS Release ZIP

```bash
./scripts/build-release-macos.sh --version v1.3.0
```

This produces `release/MelatoninAccess-<version>-macOS.zip` with the same
layout, except that the speech library is `Mods/libprism.dylib` instead of
the Windows `Tolk.dll` and `nvdaControllerClient32.dll`, and a
`README-macOS.txt` is included for end users.

Two things the script does deliberately:

- It refuses to package `libprism.dylib` unless it is a universal binary
  (x86_64 and arm64). A single-architecture build works on the machine that
  made it and silently fails on other Macs, leaving users with a mod that
  loads but never speaks.
- It clears extended attributes before zipping, so a quarantine flag on the
  build machine is not baked into the published archive.

The localization and cutscene QA checks are PowerShell. They run only if
`pwsh` is installed; otherwise the script warns and records `QA: NOT RUN` in
its summary. Those checks cover platform-independent data, so a Windows build
of the same commit covers them.

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
