# Accessibility Todo List

This document outlines the roadmap for making Melatonin 100% accessible to totally blind players.

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
- [ ] **Dialogue System (`DialogBox.cs`)**
    - [ ] Announce dialogue text when it appears or changes.
    - [ ] Announce speaker names if available.
    - **Suggestion**: Patch `DialogBox.SetText()` and `DialogBox.Activate()`. Ensure multiple updates in quick succession don't flood the TTS queue (debounce if necessary).
- [ ] **Transient Labels**
    - [ ] **Title Screen**: Read "Press Space to Start" (`Instruction.cs`).
    - [ ] **Language Hint**: Read language change prompts (`LangHint.cs`).
    - [ ] **Side Label**: Read "Practice Mode" and "Press Tab to Skip" prompts (`SideLabel.cs`).
- [x] **Popups & Confirmations**
    - [x] Read `ConfirmModal.cs` (Quit game, etc.) (Implemented in `PopupHandler`).
    - [x] Read `ExtraMessage.cs` warnings (Implemented in `PopupHandler`).
- [x] **Achievements (`AchievementsMenu.cs`)**
    - [x] Announce list navigation and details (Implemented in `AchievementsHandler`).
- [ ] **Results Screen (`Results.cs`)**
    - [ ] Announce Rank, Score, and Accuracy on screen load.
    - [ ] Allow navigating details (Early/Late/Perfect counts) using arrow keys if they aren't automatically read.

## 3. Gameplay (The "Dreams")
- [x] **General Rhythm Mechanics (`Dream.cs`)**
    - [x] **Input Cues**: Implemented TTS cues ("Space", "Left", "Right", "Hold") in `RhythmHandler.cs`.
    - [ ] **Beat Cues**: Ensure standard metronome beats are audible (Game has settings, ensure accessible configuration).
    - [ ] **Feedback**: Distinct sounds for Early, Late, Perfect, and Miss (Game likely has this, verify accessibility).
- [ ] **Tutorial Level (`Dream_tutorial.cs`)**
    - [ ] The tutorial uses a state machine to advance. Ensure every state change that updates instructions is vocalized.
    - [ ] Announce when the game waits for user input vs. when it's just demonstrating.
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
This is the most complex area.

- [ ] **Grid Navigation**
    - [ ] Announce cursor position ("Bar 1, Beat 2").
    - [ ] Announce placed notes under cursor ("Left Arrow", "Hold Start").
- [ ] **Tools**
    - [ ] Announce currently selected tool (Placement, Deletion, Selection).
    - [ ] Announce playback state (Playing/Stopped).

## 6. Specific Level Support
Some levels might have unique mechanics requiring specific cues.

- [ ] **Dream_food.cs**: Check for specific "Shoot" vs "Jump" visual cues.
- [ ] **Dream_tech.cs**: Check for positional cues.
- [ ] **Dream_dating.cs**: Check for "Swipe" directions.
- [ ] **General Approach**: Audit each `Dream_*.cs` file for `QueueHitWindow` calls and ensure the `hitType` (Left, Right, etc.) has a corresponding audio cue.

## Implementation Priorities
1.  **Framework**: `ScreenReader` and global input hooks.
2.  **Menus**: Essential for navigating to the game.
3.  **Tutorial & Dialogue**: Essential for learning mechanics.
4.  **Map**: Essential for entering levels.
5.  **Gameplay**: The core loop.
6.  **Editor**: Power user feature, lower priority.
