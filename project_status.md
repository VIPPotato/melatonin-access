# Project Status

**Current Phase**: Testing & Polish
**Last Update**: 2026-02-11

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
- [x] **README Release/Testing Update (2026-02-10)**: Expanded `README.md` with controller-aware navigation guidance, MelonPreferences toggle documentation, and speech regression script usage examples for post-playtest validation.
- [x] **Runtime Settings Hotkeys + Persistence (2026-02-10)**: Added `F2` runtime toggle for rhythm cue announcements with immediate `MelonPreferences` save, and moved `F12` debug toggle to persistent config-backed state restored at startup.
- [x] **Multilingual Smoke Audit (2026-02-10)**: Verified localization coverage for critical lines (`intro_welcome`, `tutorial_skip_prompt`, `results_stats`, lock reasons, `credits_title`) with placeholder consistency checks across all supported languages.
- [x] **Speech Regression Check Pass (2026-02-10)**: Ran `scripts/Test-SpeechRegression.ps1` against current `MelonLoader\Latest.log` (8 speech lines, 0 errors, 0 warnings).
- [x] **Release Prep Artifacts (2026-02-10)**: Added `CHANGELOG.md`, bumped mod version to `1.1.6`, and packaged `release\MelatoninAccess-v1.1.6.zip` (DLL + README + changelog + regression checker script).
- [x] **Release Tag Prepared (2026-02-10)**: Created local annotated tag `v1.0` at commit `49d1505` per release naming request.
- [x] **Release Dependency Bundling (2026-02-10)**: Added tracked `libs\x86\Tolk.dll` and `libs\x86\nvdaControllerClient32.dll`, updated ignore rules to keep these DLLs in repo, and added `scripts\Build-ReleasePackage.ps1` to generate `v1.0` release ZIP with only `Mods\MelatoninAccess.dll` + required Tolk/NVDA client DLLs.
- [x] **README Install Instructions Expanded (2026-02-10)**: Added explicit step-by-step install guidance for release ZIP and source-build workflows, including exact destination paths for mod and dependency DLLs.
- [x] **README Support + Installer Link Update (2026-02-10)**: Added Buy Me a Coffee support message and direct MelonLoader installer link in release installation steps.
- [x] **Map Teleport Double-Trigger Fix (2026-02-10)**: Added teleport dispatch guard (same-frame + short cooldown) and edge-based gamepad action detection so one key/button press maps to one teleport step.
- [x] **Bugfix Release Published (2026-02-10)**: Released `v1.0.1` on GitHub as latest with map teleport double-jump fix and updated install/readme notes; attached minimal package (`Mods\MelatoninAccess.dll`, `Tolk.dll`, `nvdaControllerClient32.dll`).
- [x] **Settings Value Timing + Results Comment Fix (2026-02-10)**: Adjusted option value-change speech to read after a short post-input delay (prevents stale pre-change value reads in display/calibration-style settings), and extended results narration to include `WavesBox` comment text in addition to score/stats/menu option.
- [x] **Settings/Calibration/Localization Polish (2026-02-10)**: Added resolution-specific post-refresh value announcement logic to avoid stale reads, combined calibration hint with calibration menu opening into one utterance, and localized map dream/level names through `Loc.GetDreamName()` keys across all supported game languages.
- [x] **Bugfix Release Published (2026-02-10)**: Released `v1.0.2` on GitHub as latest with resolution value-timing fix, calibration menu utterance merge, and localized map level-name announcements; attached minimal package (`Mods\MelatoninAccess.dll`, `Tolk.dll`, `nvdaControllerClient32.dll`).
- [x] **Global Menu Position Toggle (2026-02-10)**: Added persistent `F3` runtime toggle (`AnnounceMenuPositions`) that enables/disables position context (`X of Y`) across option menus, language/mode menus, achievements, stage-end options, and downloaded-level row/page narration; added localized toggle confirmations for all supported languages.
- [x] **Release Bundle Loader Config (2026-02-10)**: Added tracked `UserData\Loader.cfg` (copied unchanged from maintainer install) and updated `scripts\Build-ReleasePackage.ps1` to always include it in release ZIPs under `UserData\Loader.cfg`.
- [x] **Release Published (2026-02-10)**: Released `v1.0.3` on GitHub as latest with global `F3` menu-position toggle and bundled `UserData\Loader.cfg` in the release package.
- [x] **Release Notes Formatting Fix (2026-02-10)**: Rewrote GitHub release bodies for `v1.0`, `v1.0.1`, `v1.0.2`, and `v1.0.3` using proper multiline markdown so headings/lists render correctly (removed literal `\n` text artifacts).
- [x] **Map Progress Hotkey + Gamepad Input Source Fix (2026-02-11)**: Added map-only `F1` announcement for current landmark progress (stars collected and stars still needed to pass), and removed `ctrlType` dependency from map teleport gamepad detection so `Action Left/Right` navigation assist keeps working after keyboard input.
- [x] **Map Progress Hotkey Refinement (2026-02-11)**: Updated `F1` to announce map/chapter-level star totals (matching `TotalBox` behavior) instead of nearest-landmark stars; summary now reports total chapter stars and remaining stars to pass threshold.
- [x] **Release Published (2026-02-11)**: Released `v1.0.4` on GitHub as latest with map-summary `F1` behavior and map gamepad input-source handoff fix.
- [x] **v1.0.5 Local Prompt + Mode-Title Pass (2026-02-11)**: Rhythm tutorial cues now announce dynamic action prompts (`Press {Action}` / `Hold {Action}`) based on rebind/controller context instead of hardcoded Space, and mode-menu open narration now prepends the localized full dream title (`Dream about {Level}. Mode menu...`).
- [x] **Localization QA Automation (2026-02-11)**: Added `scripts\Test-LocalizationQA.ps1` to validate full `Loc.cs` coverage and placeholder parity across all supported languages; integrated automatic execution into `scripts\Build-ReleasePackage.ps1` (with `-SkipLocalizationQa` override) and documented usage in `README.md`.
- [x] **Context Help Hotkey (2026-02-11)**: Added global `F11` context-aware control help (`ContextHelpHandler`) with localized prompts for title screen, generic menus, map navigation, mode menu, gameplay, results, and editor contexts.
- [x] **Tutorial/Dialog + Community Loader Follow-up (2026-02-11)**: Added delayed reads to `DialogBox.SetDialogState` to reliably capture initial tutorial text lines, and prevented `editor_ready` from announcing on downloaded/community-level loader path (`LvlEditor` with non-empty `downloadFilePath`).
- [x] **Per-Level Completion Briefing Reverted (2026-02-11)**: Removed mod-generated level-start briefing per user preference to keep startup narration limited to game-provided/tutorial text.
- [x] **Release QA Spot Check (2026-02-11)**: Ran `pwsh -File .\scripts\Test-LocalizationQA.ps1` (116 localization keys parsed, 0 errors, 0 warnings).
- [x] **Packaging QA Gate Verification (2026-02-11)**: Ran `pwsh -File .\scripts\Build-ReleasePackage.ps1`; confirmed localization QA executes in packaging flow and `Build-ReleasePackage.ps1` is configured to exit with the QA failure code on localization check failure.
- [x] **Cutscene AD Timing Pipeline (2026-02-11)**: Added data-driven intro/outro timing scaffold (`cutscene-ad/manifest.json` + per-cutscene JSON files), validator script (`scripts\Test-CutsceneAdPipeline.ps1`) for ordering/overlap/missing-entry checks, initial C# loader/validation models (`CutsceneAdPipeline.cs`), and workflow docs (`docs/cutscene-ad-pipeline.md` + `README.md` section).
- [x] **Runtime Startup Fix (2026-02-11)**: Removed `System.Runtime.Serialization` runtime dependency from `CutsceneAdPipeline` (startup `FileNotFoundException` in MelonLoader), switched to Unity JSON path with `UnityEngine.JSONSerializeModule` reference, and verified clean local build.

## Next Steps
- **Focused Playtest**: Validate end-to-end flow for map mode locks, stage-end locks, advanced menu/timeline narration, and credits scrolling narration.
- **Language Spot Check**: Switch each in-game language and verify key mod-only lines (debug toggles, map lock reasons, teleport conflict hint, results summary) are spoken correctly.
- **Config Spot Check**: Toggle each new `MelonPreferences` setting and verify the targeted announcement group turns on/off without side effects.
- **Hotkey Spot Check**: Verify `F2`, `F3`, and `F12` update behavior immediately and remain in the same state after restarting the game.
- **New Feature Spot Check**: On map scenes, press `F1` and verify it announces chapter/map star totals + remaining stars needed to pass; verify `F1` is silent in non-map scenes.
- **Gamepad Resilience Spot Check**: Use map gamepad teleport (`Action Left/Right`), press any keyboard key, then confirm gamepad teleport still works without needing to open/close pause menu.
- **v1.0.5 Prompt Spot Check**: In tutorial/practice gameplay, rebind Action (for example to `Enter`, `Period`, `Slash`, and a letter key) and verify rhythm cues announce the new action prompt correctly; with controller active, verify prompts use controller action naming.
- **v1.0.5 Mode Menu Spot Check**: From map navigation, open multiple levels and confirm the opening line says the full dream title before mode menu/options (for example `Dream about Money. Mode menu. Practice...`).
- **Context Help Spot Check**: Press `F11` on title screen, in regular menus, on map, in map mode menu, during gameplay, on results, and in editor; verify each context speaks relevant controls and uses current action/cancel prompts.
- **Tutorial Intro Spot Check**: Enter tutorial from chapter menu and verify the first instruction block (including multi-line startup warning text) is announced without hardcoded text dependencies.
- **Community Loader Spot Check**: Start downloaded/community level flow and verify `Level editor ready` is no longer announced before gameplay.
- **Cutscene AD Authoring Step**: Fill `cutscene-ad/scripts/chapter_*_intro.json` and `cutscene-ad/scripts/chapter_*_outro.json` with first-pass timestamp/text-key entries, then run `pwsh -File .\scripts\Test-CutsceneAdPipeline.ps1 -StrictCoverage -RequireEntries`.
- **Post-Release Validation**: Monitor issues/feedback from `v1.0.4` and collect any remaining edge cases from real-world play sessions.
