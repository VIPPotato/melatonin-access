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
- **Controller utility buttons exposed by Unity Input System**:
  - `Gamepad.selectButton`: View/Back button.
  - `Gamepad.leftStickButton`: Left stick click (L3).
  - `Gamepad.rightStickButton`: Right stick click (R3).
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
- **Menu title fragments**:
  - `LangMenu.title` and `AchievementsMenu.title` are live localized UI fragments and should be preferred for spoken menu titles over mod-owned title strings.
  - References: `decompiled/LangMenu.cs:16`, `decompiled/AchievementsMenu.cs:18`.

## DialogBox Hooks
- `DialogBox.Show()` can present dialog content immediately (without `Activate()`), so accessibility hooks should include `Show` plus text/state changes.
- `DialogBox.Activate()` and `DialogBox.ActivateDelayed()` control panel transitions and optional speaker SFX but do not inherently guarantee new text state assignment.
- `DialogBox.SetDialogState()` / `ChangeDialogState()` update localized dialog text states; many dream scripts call these before or around activation.
- `DialogBox.SetText()` is used for dynamic token replacement (`[]`, `[1]`, `[2]`) based on keyboard/controller context.
- `DialogBox` also exposes a `label` (`textboxFragment`) that can carry additional context (for example, speaker/context tags) when rendered.
- References: `decompiled/DialogBox.cs:47-87`, `decompiled/DialogBox.cs:188-248`, `decompiled/Dream_tutorial.cs:28-44`, `decompiled/Dream_tutorial.cs:198-210`, `decompiled/Dream_tutorial.cs:295-311`.

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
- **Initial metronome gameplay start**:
  - In `Dream_tutorial.TriggerState()`, `state == 2` starts the first playable metronome segment (`TriggerSong()`) and enables tutorial side-label UI.
  - This transition point is suitable for one-time spoken timing hints like "press on second beat" before the first hit window sequence begins.
  - References: `decompiled/Dream_tutorial.cs:212-219`, `decompiled/Dream_tutorial.cs:466-517`.

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

## Achievement Lock Detection
- `AchievementsMenu.Activate()` always sets title/description text for rows 5-13, even when still locked.
- Locked state is represented by `CheevoRow.checkmark` sprite state: `0` locked, `1` checked/unlocked (`CheevoRow.Check()` sets state `1`).
- Accessibility lock announcements should not rely only on title text (`?????`) because that pattern only covers early story achievements.
- References: `decompiled/AchievementsMenu.cs:74-160`, `decompiled/CheevoRow.cs:57-60`, `decompiled/spriteFragment.cs:100-173`.

## Calibration Timing Feedback
- `PingBar.StopTimer()` computes timing from `timer - 0.11667f` (center of timing window) and places the marker accordingly.
- This delta can be converted to milliseconds for spoken "early/late" calibration feedback.
- References: `decompiled/PingBar.cs:109-123`.

## Map Input Availability
- `McMap` movement input is controlled by private `isEnabled`, set to `true` only after `McMap.Introducing()` completes.
- During chapter intro/outro transitions, map visuals can be present while navigation should remain blocked (`Chapter.CheckIsCutsceneIntro/Outro()`).
- References: `decompiled/McMap.cs:372-387`, `decompiled/McMap.cs:412-424`, `decompiled/Chapter.cs:101-245`, `decompiled/Chapter.cs:319-451`, `decompiled/Chapter.cs:568-575`.

## Credits Flow
- Credits sequence is driven by `Creditor.Starting()`:
  - `Credits.Show()` -> logo transitions -> `Credits.ScrollList()` -> exit to title.
- Scroll duration is exposed by `Credits.GetScrollDuration()` and the actual list animation is `lister.TriggerAnim("scroll")`.
- Opening a pause/submenu during credits does not reset the internal credits list state; mods can pause/resume narration safely instead of restarting from the top.
- References: `decompiled/Creditor.cs:18-39`, `decompiled/Credits.cs:102-126`.

## Community Menu Row Layout
- `CommunityMenu.LevelRows[0]` is a banner row (`ActivateAsBanner`) and does not expose level metadata text fields.
- Downloaded level rows start at index `1` and are populated by `ConfigingRows()` with title/author/tags.
- `NextPage()` / `PrevPage()` reset `highlightNum` to `0` first, then repopulate rows asynchronously through `ConfigContent()`.
- References: `decompiled/CommunityMenu.cs:67-99`, `decompiled/CommunityMenu.cs:222-263`, `decompiled/CommunityMenu.cs:362-387`, `decompiled/LevelRow.cs:48-68`.

## Chapter 1 Practice Cue Patterns
- `Dream_food` practice queues tutorial timing with:
  - `QueueHitWindow(2)` (target on 3rd beat),
  - `QueueHitWindow(4)` (target on 5th beat),
  - `QueueHitWindow(3)` (target on 4th beat),
  - `QueueHitWindow(6)` (target on 7th beat).
- `Dream_food` uses `QueueHitWindow(...)` only for these practice prompts; decompiled gameplay path does not expose separate jump-vs-shoot input queue types in this dream.
- `Dream_tech` practice `OnSequence()` always queues `QueueHitWindow(1)` (except `beat == 4` branches), so per-note spoken cues become noisy; phase-level guidance is better than repeating every hit window.
- `Dream_tech` phrase boundaries can be tracked through `Dream.dir.GetPhrase()` during practice; phrase 1/2 are suitable for one-shot section briefings, while later rapid sections benefit from selective `press twice` callouts.
- `Dream_followers` includes rapid patterns in `sequences[4]` using `QueueHitWindow(1)`, `QueueHitWindow(1, isHalfBeatAdded: true)`, and a follow-up `QueueHitWindow(2)`.
- `Dream_followers` second teaching phase transition appears at `phrase == 2`, `bar == 1`, `beat == 2` (`InfluencerLand.env.Reframe(30, "k", ...)`), useful for contextual "audio cue + vibration" guidance.
- `Dream_followers` third teaching phase starts after phrase-2 close (`phrase == 2`, `bar == 8`, `beat == 4`), so announcing phase-3 guidance at `phrase == 2`, `bar == 8`, `beat == 2` gives ~2 beats of lead time.
- `Dream_shopping` practice repeatedly uses `QueueHitWindow(4)` / `QueueHitWindow(8)` while visual/audio store patterns are presented, making it suitable for a concise "follow repeating audio pattern" instruction.
- References: `decompiled/Dream_food.cs:114-255`, `decompiled/Dream_tech.cs:115-220`, `decompiled/Dream_followers.cs:140-190`, `decompiled/Dream_followers.cs:241-390`, `decompiled/Dream_shopping.cs:115-233`.

## Map Dream Name Source Note
- `Landmark` stores only internal key `dreamName` (for example `food`, `dating`) and `DreamTitle` visuals are sprite-based (`spriteFragment`), not guaranteed text fragments.
- Because of that, map dream-name speech should prefer live UI text when a readable `TextMeshPro` node is available and otherwise fall back to mod localization.
- References: `decompiled/Landmark.cs:5-16`, `decompiled/Landmark.cs:255-258`, `decompiled/DreamTitle.cs:8-45`.

## Dream_dating Practice Cue Patterns
- `Dream_dating` practice uses directional hit windows only (`QueueLeftHitWindow`, `QueueRightHitWindow`), so level cues should speak swipe direction rather than generic action prompts.
- Core directional patterns:
  - `QueueLeftHitWindow(3)` / `QueueRightHitWindow(3)` for standard countdown swipes.
  - `QueueLeftHitWindow(6)` / `QueueRightHitWindow(6)` for longer-countdown swipes (good spot for "after long cue" phrasing).
- Song start path:
  - `Dream_dating.Starting()` calls `TriggerSong()` after the pre-song tutorial dialog gate, so one-time "follow swipes" prompts can safely be attached to `TriggerSong`.
- References: `decompiled/Dream_dating.cs:16-44`, `decompiled/Dream_dating.cs:86-219`.

## Text-File Tutorial Cue Batch (v1.1)
- This pass maps `melatonin tutorials.txt` requests to decompiled gameplay queues for these scenes only:
  - `Dream_followers`: section-level guidance moved to phrase timing (`phrase 1` start/stop spring cue, `phrase 2` resume-after-vibration cue, and phase-3 pre-brief at `phrase 2, bar 8, beat 2`).
  - `Dream_dating`: directional swipe speech on queue direction, including long-countdown variants (`QueueLeft/RightHitWindow(6)`).
  - `Dream_time`, `Dream_space`, `Dream_desires`: use `QueueHoldReleaseWindow(...)` arguments to announce hold + release duration in beats.
  - `Dream_nature`: water sprout triple sequence detected from `QueueHitWindow(2)`, `(2, half)`, `(3)` and condensed to one "press thrice" line; soak sections still use hold/release duration cues.
  - `Dream_mind`: triple off-beat section detected from `QueueHitWindow(1)`, `(1, half)`, `(2)` and spoken as one off-beat triple instruction.
  - `Dream_past`: camera-specific hold cues mapped from distinct hold durations:
    - `QueueHoldReleaseWindow(4, 5)` -> hold 1 beat
    - `QueueHoldReleaseWindow(4, 4, false, true)` -> hold half beat
    - `QueueHoldReleaseWindow(4, 6)` -> hold 2 beats
  - `Dream_future`: `QueueHitWindow(...)` is the up input and `QueueLeftRightHitWindow(...)` is both lateral inputs, so spoken cues should be "Press Up" and a short "Left right".
- References: `decompiled/Dream_followers.cs:140-394`, `decompiled/Dream_dating.cs:82-205`, `decompiled/Dream_time.cs:157-315`, `decompiled/Dream_space.cs:130-226`, `decompiled/Dream_desires.cs:105-140`, `decompiled/Dream_nature.cs:100-139`, `decompiled/Dream_mind.cs:75-211`, `decompiled/Dream_past.cs:98-159`, `decompiled/Dream_future.cs:98-148`.

## v1.1 Cue Refinement Notes
- `Dream_followers` phase-3 callout can be moved from phrase-2 pre-roll to `phrase == 3, bar == 1, beat == 1` when players prefer hearing it after the spring-stop transition.
- `Dream_space` and `Dream_desires` hold windows are clearer when announced as phase-relative release beat (`numBeatsTilRelease - numBeatsTilHold + 1`) instead of absolute queue beat values.
- `Dream_time` tutorial can be split into two contextual hold/release hints by queue signature:
  - `QueueHoldReleaseWindow(2, 3, isHalfBeatAddedToHold: true)` -> portal-gap buzz instruction (`hold on cue, release on next cue`)
  - `QueueHoldReleaseWindow(1, 2, isHalfBeatAddedToHold: true)` -> late-section sixth/seventh cue instruction
- `QueueHoldReleaseWindow(1, 2, isHalfBeatAddedToHold: true)` appears in multiple teaching sections (`sequences[1]` and `sequences[3]` paths), so mods should gate narration to the later section rather than first match.
- Practical gate for `Dream_time` final teaching:
  - Read `Dream.sequences` (private float array on `Dream`) and treat `sequences[3] > 0` as the final teaching segment.
  - Speak sixth/seventh guidance only when that segment is active; suppress the same signature in earlier `sequences[1]`.
- `Dream_future` supports an additional one-time `TriggerSong()` primer line ("follow patterns") while keeping directional/up queue callouts.
- `Dream_past` queue windows repeat frequently; one-shot-per-duration hints (1 beat, half beat, 2 beats) reduce tutorial spam while preserving all unique timing hints.
- References: `decompiled/Dream_followers.cs:140-190`, `decompiled/Dream_time.cs:157-315`, `decompiled/Dream_space.cs:130-226`, `decompiled/Dream_desires.cs:105-140`, `decompiled/Dream_future.cs:98-148`, `decompiled/Dream_past.cs:98-159`.

## Remix Landmark Lock State
- Remix landmark lock state on map is represented by private `Landmark.isDisabled` after `Landmark.Show()`.
- `Landmark.isRemix` is set when the landmark equals `Neighbourhood.GetRemixLandmark()`.
- For non-final remix landmarks, `Landmark.Show()` sets `isDisabled = true` until map unlock conditions enable the landmark.
- This allows teleport narration to append a lock requirement line when snapping to a locked remix landmark.
- References: `decompiled/Landmark.cs:44-55`, `decompiled/Landmark.cs:112-119`, `decompiled/Neighbourhood.cs:40-43`, `decompiled/Map.cs:66-83`.

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

## Cutscene Runtime Detection
- `Chapter.dir.CheckIsCutsceneIntro()` and `Chapter.dir.CheckIsCutsceneOutro()` expose active chapter transition state.
- `SceneMonitor.mgr.GetActiveSceneName()` returns active scene names like `Chapter_1` through `Chapter_5` during intro/outro playback.
- For data-driven cutscene AD, combine `sceneName + cutsceneType` (intro/outro) as a stable lookup key.
- Intro timing caveat:
  - `Chapter.EnteringWithIntro()` sets `isCutsceneIntro = false` before `Map.env.Activate()` (after `SetComposition("transitionFromCentered")` and ~1.35s wait).
  - Any intro AD cue placed after that flag flip is skipped by runtime cutscene-state checks, so map-transient cues must be timed before the intro flag clears.
- References: `decompiled/Chapter.cs:568-575`, `decompiled/SceneMonitor.cs:73-76`.

## Cutscene Asset Extraction Notes
- Unity asset files under `Melatonin_Data` contain exportable `AnimationClip`, `AudioClip`, `Sprite`, and `Texture2D` objects that can be extracted with `scripts/extract_unity_assets.py`.
- In this build, many object names are stripped/empty, so exported file names are primarily identified by source file + `pathId` (with sample names preserved where available, especially for `AudioClip`).
- `MonoBehaviour` typetree decoding is partial: many objects fail with truncated/short payload errors, but a subset is readable and includes script pointer metadata (`m_Script.m_PathID`), useful for targeted follow-up mapping.
- Baseline full extraction output path: `artifacts/asset-extract` (metadata index: `artifacts/asset-extract/assets.jsonl`, summary: `artifacts/asset-extract/summary.json`).
