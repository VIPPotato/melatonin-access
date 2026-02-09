# Melatonin Tutorial Text Extraction

This file contains extracted in-game tutorial/help text and related context.
It is extracted game content, not mod documentation.

Extraction date: 2026-02-09

## Sources

- Decompiled logic: `decompiled/Dream_tutorial.cs`, `decompiled/DialogBox.cs`, `decompiled/TutorialWorld.cs`, `decompiled/Option.cs`, `decompiled/Submenu.cs`, `decompiled/textboxFragment.cs`
- Runtime evidence: `D:\games\steam\steamapps\common\Melatonin\MelonLoader\Latest.log`
- Asset text scan (binary): `D:\games\steam\steamapps\common\Melatonin\Melatonin_Data\level0`

## Extracted English Tutorial/Help Texts

### Entry and menu texts

- `tutorial` (menu label)
  Source: `decompiled/Option.cs:342`, `decompiled/Submenu.cs:220`, `Melatonin_Data\level0` (e.g. line 294 via `rg -a -n -o`)
- `skip tutorial` (menu option)
  Source: `decompiled/Option.cs:353`, `Melatonin_Data\level0` line 294
- `rewatch intro` (menu option)
  Source: `decompiled/Option.cs:402`, `Melatonin_Data\level0` line 288
- `Press           to start. English` (runtime announcement; key placeholder resolved at runtime)
  Source: `MelonLoader\Latest.log` (`[SR] Announcing Intro` / `[SR] Press ... to start`)

### Tutorial guidance and tips

- `The phone vibrations will act as cues for the more tricky parts.`
  Source: `MelonLoader\Latest.log` (`[GAME] [DialogBox Text]`)
- `Displays the timing circle outside of practice`
  Source: `Melatonin_Data\level0` line 217
- `Try restarting practice if you need more time to learn the cues`
  Source: `Melatonin_Data\level0` line 439
- `Keep it up!`
  Source: `Melatonin_Data\level0` line 445

### Additional gameplay help/instruction text recovered in deep asset pass

- `Enables a metronome during all gameplay`
  Source: `Melatonin_Data\level0` line 212
- `Your score gets penalized less for early and late hits`
  Source: `Melatonin_Data\level0` line 169
- `Level auto-restarts if you get an “early”, ‘late”, or “miss”`
  Source: `Melatonin_Data\level0` line 216
- `You’re ready for score mode!`
  Source: `Melatonin_Data\level0` line 440
- `Score 2 stars or more to unlock the next scene`
  Source: `Melatonin_Data\level0` line 443
- `Just about perfect`
  Source: `Melatonin_Data\level0` line 447
- `score mode`
  Source: `Melatonin_Data\level0` line 602
- `perfects only`
  Source: `Melatonin_Data\level0` line 274
- `Press a new key to remap the action key`
  Source: `Melatonin_Data\level0` line 189 (related settings/help text)

### Tutorial mode labels seen during runtime

- `P R A C T I C E`
  Source: `MelonLoader\Latest.log`
- `FINISHED. PRACTICE. Perfect: ..., Late: ..., Early: ..., Miss: ...`
  Source: `MelonLoader\Latest.log`

## Tutorial Flow and Mechanics (from decompiled code)

- Tutorial scene starts in `Dream_tutorial` and opens with `DialogBox` state 0.
  Source: `decompiled/Dream_tutorial.cs:24-32`
- Progression is state-based (`state` increments on action input).
  Source: `decompiled/Dream_tutorial.cs:54-62`, `decompiled/Dream_tutorial.cs:147-420`
- Dialog text contains runtime placeholders:
  - `[]` replaced with `A` / `X` / `SPACE` depending on control type.
    Source: `decompiled/Dream_tutorial.cs:198-210`, `decompiled/Dream_tutorial.cs:343-355`
  - `[1]` and `[2]` replaced with `L/R`, `A/D`, or `LEFT/RIGHT`.
    Source: `decompiled/Dream_tutorial.cs:295-311`
- Extra input while sub-info is open rewinds or restarts tutorial progression.
  Source: `decompiled/Dream_tutorial.cs:64-75`, `decompiled/Dream_tutorial.cs:92-142`
- Tutorial triggers action sounds and feedback cues through `TutorialWorld`.
  Source: `decompiled/Dream_tutorial.cs:590-610`, `decompiled/TutorialWorld.cs:31-48`

## Language Notes

- The text table in `Melatonin_Data\level0` contains multilingual variants for tutorial/help labels and tips.
- English text is explicitly present and was prioritized here.
- The deep asset pass confirmed multilingual rows for each extracted English line (Chinese, Traditional Chinese, Japanese, Korean, Vietnamese, French, German, Spanish, Portuguese).

## Gaps and Limitations

- Most dialog state bodies (`DialogBox.SetDialogState(0..13)`) are serialized in Unity assets, not plain text files in `StreamingAssets` or `Resources`.
- Decompilation shows where text is selected, but not full dialog strings for every state.
- UnityPy could resolve script/component names, but custom MonoBehaviour field layouts are stripped in this build, so direct `textboxFragment.states` decoding was incomplete.
- Full extraction of every dialog state line would require a Unity asset deserializer with working script field metadata for this game build.
