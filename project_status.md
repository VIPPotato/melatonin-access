# Project Status

**Current Phase**: Testing & Polish
**Last Update**: 2026-02-09

## Completed
- [x] **Core System**: `ScreenReader` (Tolk), `MelatoninAccess` (MelonLoader Mod).
- [x] **Menu Accessibility**: `MenuHandler` announces all menu options, titles, and toggles.
- [x] **Gameplay Accessibility**: `RhythmHandler` provides audio cues for all rhythm inputs.
- [x] **Tutorial Accessibility**: `DialogueHandler` reads all dialog boxes.
- [x] **Map Accessibility**: `MapHandler` implements teleport navigation using Bracket keys.
- [x] **Level Editor**: `EditorHandler` announces cursor position, tools, and placement.
- [x] **Achievements**: `AchievementsHandler` reads the achievement list.
- [x] **Build & Install**: Successfully compiled and deployed to Game Mods folder.
- [x] **Setup Refresh (2026-02-09)**: Re-ran setup checks, filled environment placeholders in `AGENTS.md`, and added `CopyToMods` auto-deploy target to `MelatoninAccess.csproj` (validator: 16 OK, 0 warnings, 0 errors).
- [x] **Tutorial Extraction (2026-02-09)**: Extracted tutorial/help text and flow context into `docs/tutorial-texts.md`; updated `docs/game-api.md` with tutorial start path, skip behavior, and text-source notes. Deep asset pass recovered additional gameplay instruction lines from `Melatonin_Data\level0` (metronome, timing circle, score mode readiness, and auto-restart behavior).
- [x] **Duplicate Announcement Reduction (2026-02-09)**: Added targeted debounce for dialog text (`DialogueHandler`), language/start screen (`StartScreenHandler`), and map mode menu (`MapHandler`), plus stronger whitespace/time normalization in `ScreenReader`. Build succeeded and DLL auto-copied to game `Mods` folder.
- [x] **Duplicate Announcement Reduction Pass 2 (2026-02-09)**: Removed additional duplicate bursts in `AchievementsHandler` and `ExtraMenusHandler` (community next/prev page and calibration activation/description) with short cooldown-based dedupe. Build succeeded and deployed.
- [x] **Localization Framework (2026-02-09)**: Added `Loc.cs`, language auto-refresh via `SaveManager.GetLang()`, and routed mod-originated announcements through localization keys for all game-supported languages.
- [x] **Editor Completeness Pass (2026-02-09)**: Added narration for `AdvancedMenu` (activate, tab swap, row changes, value changes) and `TimelineTabs` (`Show`, `NextTab`, `PrevTab`).
- [x] **Lock-State & Map Input Completeness (2026-02-09)**: Added explicit spoken lock reasons for map mode and stage-end options, plus bracket-action conflict handling with `F9/F10` teleport fallback.
- [x] **Debug Toggle Fix (2026-02-09)**: `DebugMode` now defaults OFF and is toggled at runtime with `F12`, with spoken on/off confirmation.
- [x] **Credits Accessibility (2026-02-09)**: Added credits sequence narration, including scrolling credit entries.
- [x] **README Added (2026-02-09)**: Wrote `README.md` with controls, navigation flow (including bracket/up-arrow usage), localization coverage, and install/build instructions.
- [x] **Menu Speech Unification + Welcome Prompt Update (2026-02-09)**: Combined predictable opening announcements into single utterances (menu title + first item where applicable), and replaced the title-screen welcome line with a localized controller-aware prompt (`Press [Action] to begin, Press [Language Key] to change language`).
- [x] **Playtest Follow-up Fixes (2026-02-09)**: Removed `F1` behavior, added position announcements for language/mode menus, merged results + initial stage-end choice into one utterance, improved tutorial dialog capture in followers, changed tutorial start label to a localized skip prompt, and added gamepad snap-to-hotspot input (`Action Left/Right`, e.g. LB/RB).
- [x] **Followers Tutorial Graphic Text Fix (2026-02-09)**: Added `DialogBox.ChangeToGraphic` narration path for left/right graphic tutorial text blocks and reinforced practice-start wording as localized tutorial skip prompt.
- [x] **Graphic Dialog Punctuation Cleanup (2026-02-10)**: Adjusted dialog part joining to avoid duplicate punctuation when graphic tutorial lines already end with sentence punctuation.
- [x] **Map Hotspot Chatter Reduction (2026-02-10)**: Added same-landmark debounce in `MapHandler` so rapid collider re-entries no longer spam repeated hotspot arrival lines.
- [x] **Teleport Star Grammar Fix (2026-02-10)**: Added singular star localization key and switched teleport arrival speech to use singular/plural variants correctly.
- [x] **Optional Announcement Toggles (2026-02-10)**: Added `ModConfig.cs` using `MelonPreferences` with toggles for map hotspot speech, rhythm cues, tutorial/dialog narration, and credits narration.
- [x] **Speech Regression Checker Script (2026-02-10)**: Added `scripts/Test-SpeechRegression.ps1` to scan MelonLoader logs for consecutive duplicate SR lines and known forbidden patterns.
- [x] **Localization Consistency Pass (2026-02-10)**: Improved non-English coverage for startup loaded message, results summary wording, and Vietnamese space cue label.

## Next Steps
- **Focused Playtest**: Validate end-to-end flow for map mode locks, stage-end locks, advanced menu/timeline narration, and credits scrolling narration.
- **Language Spot Check**: Switch each in-game language and verify key mod-only lines (debug toggles, map lock reasons, teleport conflict hint, results summary) are spoken correctly.
- **Config Spot Check**: Toggle each new `MelonPreferences` setting and verify the targeted announcement group turns on/off without side effects.
- **Release Prep**: If playtest passes, package current DLL and tag a release build.
