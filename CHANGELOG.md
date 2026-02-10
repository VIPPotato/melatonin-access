# Changelog

## 1.0.3 - 2026-02-10

- Added global `F3` toggle for menu position announcements (for example `1 of 4`), with immediate persistence to MelonPreferences.
- Applied the position toggle across options menus, language/mode menus, stage-end results options, achievements rows, and downloaded-level row/page narration.
- Added localized on/off confirmation messages for the new menu-position toggle in all supported game languages.

## 1.0.2 - 2026-02-10

- Fixed settings value timing for directional options so value announcements are read after the applied change.
- Added a dedicated wait-for-refresh path for resolution changes to avoid stale pre-change reads.
- Included results comment text (`WavesBox` message) in spoken results summary.
- Combined calibration hint text with calibration menu opening announcement into one utterance.
- Localized dream/level name announcements across all game-supported languages.

## 1.0.1 - 2026-02-10

- Fixed map teleport double-trigger where one press (`[`, `]`, `F9`, `F10`) could jump two landmarks.
- Added one-shot teleport dispatch guard and edge-based gamepad teleport input handling.
- Updated README install/release notes and support link text.

## 1.1.6 - 2026-02-10

- Added runtime `F2` toggle for rhythm cue announcements with immediate persistence to MelonPreferences.
- Made `F12` debug mode persistent across sessions using MelonPreferences-backed state.
- Reduced map hotspot speech chatter by debouncing repeated same-landmark arrival lines.
- Fixed teleport star grammar with singular/plural handling (`1 star` vs `N stars`).
- Added optional announcement toggles for map hotspots, rhythm cues, tutorial/dialog narration, and credits narration.
- Added `scripts/Test-SpeechRegression.ps1` to detect common speech regressions from `[SR]` logs.
- Improved translation consistency for startup loaded line and results stats wording.
- Expanded README with controller-aware navigation, runtime toggle behavior, and regression test workflow.

## 1.1.5 - 2026-02-09

- Unified predictable menu announcements into single utterances.
- Updated welcome prompt to localized controller-aware guidance.
- Added/finished credits roll narration while scrolling.
- Improved tutorial dialog coverage (including graphic text path) and punctuation joining.
- Added map/gameplay playtest follow-up fixes (mode/language positions, results merge, snap controls).
