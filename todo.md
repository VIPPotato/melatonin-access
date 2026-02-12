# Accessibility Todo List

This document outlines the roadmap for making Melatonin 100% accessible to totally blind players.

## 0. v1.0.5 Active Fixes
- [x] **Credits Pause/Resume Narration**
  - Pause credits speech while pause/submenu is open.
  - Resume from the previous unread entry when gameplay returns.
  - Clear narration progress when credits finish or exit.
- [x] **Community Levels One-Utterance Paging**
  - On menu load: announce total/page and first level row in one utterance.
  - On next/previous page: announce page action and first level row in one utterance.
  - Suppress immediate duplicate row re-announcements after auto page reset.
- [x] **Action Key Option Label (Keyboard Binding)**
  - `Change action key` now announces the currently bound keyboard key token only.
  - Controller face button names are no longer used for that settings line.
- [x] **Chapter 1 Tutorial Rhythm Cue Pass**
  - Tech: rapid/double action cues announce as "press twice" instead of collapsing.
  - Followers: replaced spammy press prompts with rhythm guidance and double-press cue.
  - Food: one-time beat-target guidance for 3rd, 5th, and 4th beat sections.
  - Shopping: one-time "repeat audio pattern" guidance instead of continuous spam.
- [x] **Controller Mirrors for Utility Hotkeys**
  - Added map-progress announcement on controller View button (same behavior as `F1`, map-only and silent outside map).
  - Added rhythm-cue toggle on `L3` (same as `F2`) and menu-position toggle on `R3` (same as `F3`), with persisted config updates.
- [x] **Tech/Followers Cue Timing Refinement**
  - Dream tech practice now uses phase-level prompts instead of repeating per-hit "Press {Action}" lines.
  - Dream followers phase-3 briefing now plays 2 beats before the section starts.
- [x] **v1.1 Cue Refinement Pass**
  - Followers phase-3 "press three times after double vibration" cue moved later to land after spring-stop transition.
  - Past camera tutorial hints simplified to plain hold-duration lines and limited to one-shot-per-duration (no repeated spam).
  - Space/Desires hold windows now use release-beat phrasing (for example `release on beat 4/2/7` and `release on beat 9`).
  - Future tutorial now announces a one-time "follow patterns" primer at song start and uses short `Up` wording for up-lane cues.
  - Map teleport arrival now appends a remix-lock requirement line for locked final/remix landmarks in one utterance.
- [ ] **Calibration Feedback Accuracy Validation**
  - Keep current early/late ms narration and validate timing feel with additional playtests.

## 1. Core System & Output
- [ ] **Screen Reader Integration (`ScreenReader.cs`)**
    - Ensure robust connection to Tolk/SAPI.
    - **Suggestion**: Use `ScreenReader.Say(text)` for transient updates and `ScreenReader.Say(text, true)` for interruptive announcements (like entering a new menu).
- [ ] **Input Handling**
    - [ ] Audit `ControlHandler` to ensure all custom input methods are hooked.
    - **Suggestion**: Create a `InputAnnouncer` that (optionally) voices pressed keys for debugging or learning controls.

## 2. User Interface (UI)
- [x] **Main Menus & Submenus (`Menu.cs`, `Option.cs`)**
    - [x] Announce currently selected option name and state (On/Off).
    - [x] Announce menu transitions (entering/exiting submenus) (Implemented in `MenuHandler.cs`).
- [x] **Dialogue System (`DialogBox.cs`)**
    - [x] Announce dialogue text when it appears or changes.
    - [x] Announce speaker names if available.
    - Added coverage for `DialogBox.Show()` and label-aware dialog composition, while keeping debounce to avoid rapid duplicate speech.
- [ ] **Transient Labels**
    - [ ] **Title Screen**: Read "Press Space to Start" (`Instruction.cs`).
    - [ ] **Language Hint**: Read language change prompts (`LangHint.cs`).
    - [ ] **Side Label**: Read "Practice Mode" and "Press Tab to Skip" prompts (`SideLabel.cs`).
- [x] **Popups & Confirmations**
    - [x] Read `ConfirmModal.cs` (Quit game, etc.) (Implemented in `PopupHandler`).
    - [x] Read `ExtraMessage.cs` warnings (Implemented in `PopupHandler`).
- [x] **Achievements (`AchievementsMenu.cs`)**
    - [x] Announce list navigation and details (Implemented in `AchievementsHandler`).
- [x] **Results Screen (`Results.cs`)**
    - [x] Announce Rank, Score, and Accuracy on screen load (Implemented in `ResultsHandler`).
    - [x] Allow navigating details (Early/Late/Perfect counts) (Reads automatically on load).

## 3. Gameplay (The "Dreams")
- [x] **General Rhythm Mechanics (`Dream.cs`)**
    - [x] **Input Cues**: Implemented TTS cues ("Space", "Left", "Right", "Hold") in `RhythmHandler.cs`.
    - [ ] **Beat Cues**: Ensure standard metronome beats are audible (Game has settings, ensure accessible configuration).
    - [ ] **Feedback**: Distinct sounds for Early, Late, Perfect, and Miss (Game likely has this, verify accessibility).
- [ ] **Tutorial Level (`Dream_tutorial.cs`)**
    - [ ] The tutorial uses a state machine to advance. Ensure every state change that updates instructions is vocalized.
    - [ ] Announce when the game waits for user input vs. when it's just demonstrating.
    - [x] Added delayed `DialogBox.SetDialogState` reads to improve first-line capture for tutorial startup text without hardcoded strings.
- [ ] **Practice Mode**
    - [ ] Announce entry into Practice Mode.
    - [ ] Announce "Scored Mode" transition.

## 4. Overworld / Map (`Neighbourhood.cs`)
The current map allows free movement (`McMap.cs`), which is inaccessible.

- [x] **Navigation**
    - **Implemented**: `MapHandler.cs` uses `[` and `]` (Bracket keys) to teleport between levels.
- [x] **Announcements**
    - [x] Announce Level Name and Status (Locked/Unlocked, Grade) when landing on a node.
    - [ ] Announce "Shop" or other points of interest.

## 5. Level Editor (DAW)
- [x] **Grid Navigation**
    - [x] Announce cursor position ("Bar 1, Beat 2").
    - [x] Announce placed notes under cursor ("Left Arrow", "Hold Start").
- [x] **Tools**
    - [x] Announce currently selected tool (Placement, Deletion, Selection).
    - [x] Announce playback state (Playing/Stopped) (Handled via "Level Editor Ready" on return and Game Start announcements).

## 6. Specific Level Support
Some levels might have unique mechanics requiring specific cues.

- [ ] **Contextual Rhythm Cues for Remaining Practice Modes**
    - Add scene-specific, low-noise timing guidance for non-chapter-1 practice levels.
    - Prefer one-shot instructional prompts tied to phrase/section transitions over per-note spam.
    - Validate each cue against in-game audio cues and vibration patterns via playtest logs.
    - `Dream_dating` directional swipe cues and long-countdown variants are now implemented.
    - `Dream_tech` now uses phrase-level cues and selective rapid-double callouts instead of per-hit spam.
    - `Dream_followers` now includes a dedicated phase-3 pre-brief cue.
- [x] **Text-File Tutorial Cue Pack (`melatonin tutorials.txt`)**
    - Applied requested cue behavior to listed dreams only: `Dream_followers`, `Dream_dating`, `Dream_time`, `Dream_space`, `Dream_desires`, `Dream_nature`, `Dream_mind`, `Dream_past`, `Dream_future`.
    - Added hold/release duration narration for hold-window tutorials and camera-specific duration callouts for `Dream_past`.
    - Added `Dream_future` direction-specific cue overrides (`Press Up`, short `Left right`) and stabilized localization coverage for all newly used cue keys.
- [ ] **Dream_food.cs**: Check for specific "Shoot" vs "Jump" visual cues.
- [x] **Dream_tech.cs**: Replaced noisy per-hit practice cues with phrase-level guidance and rapid-double callouts.
- [x] **Dream_dating.cs**: Check for "Swipe" directions.
- [ ] **General Approach**: Audit each `Dream_*.cs` file for `QueueHitWindow` calls and ensure the `hitType` (Left, Right, etc.) has a corresponding audio cue.

## 7. Future Feature Ideas
- [ ] **Live Audio Description for Intro/Outro Video Scenes**
    - Analyze intro/outro video assets to extract timing and visual events.
    - Generate concise scene descriptions and play them live in sync during cutscenes.
    - Keep descriptions interrupt-safe so gameplay/menu announcements still take priority.
- [ ] **Per-Level Audio Descriptions During Gameplay**
    - Add optional descriptive callouts for major visual events in each rhythm level.
    - Keep descriptions short and non-blocking so rhythm cue timing remains accurate.
    - Provide a per-feature toggle in config so players can enable/disable level AD independently.
- [x] **Global "What Can I Press Here?" Hotkey**
    - Add a context-aware help key that speaks available controls for the current screen.
    - Cover menus, map navigation, gameplay/tutorial, and editor contexts.
    - Keep output short and ordered by priority actions.
    - Implemented on `F11` via `ContextHelpHandler.cs` with localized context-specific prompts.
- [ ] **Per-Level Completion Briefing**
    - On level/mode entry, announce a concise summary of objective and key mechanics.
    - Include active mode context (practice, score mode, hard mode, etc.).
    - Ensure briefing is a single utterance to avoid interruption overlap.
- [x] **Cutscene Audio Description Timing Pipeline**
    - Create a data-driven timing format for AD lines (timestamps + text) instead of hardcoding.
    - Add a small validation/test utility to catch timing overlaps and missing entries.
    - Support quick iteration so AD scripts can be refined without code changes.
    - Implemented with `cutscene-ad/manifest.json`, per-cutscene JSON scripts, `scripts/Test-CutsceneAdPipeline.ps1`, and `docs/cutscene-ad-pipeline.md`.
- [x] **Localization QA Checker Script**
    - Add a script that verifies every localization key exists in all supported languages.
    - Validate placeholder parity (for example `{0}`, `{1}` counts) across translations.
    - Integrate the checker into release prep to prevent missing or broken localized lines.
    - Implemented as `scripts/Test-LocalizationQA.ps1` and wired into `scripts/Build-ReleasePackage.ps1`.

## Implementation Priorities
1.  **Framework**: `ScreenReader` and global input hooks.
2.  **Menus**: Essential for navigating to the game.
3.  **Tutorial & Dialogue**: Essential for learning mechanics.
4.  **Map**: Essential for entering levels.
5.  **Gameplay**: The core loop.
6.  **Editor**: Power user feature, lower priority.
