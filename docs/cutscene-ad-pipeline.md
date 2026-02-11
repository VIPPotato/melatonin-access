# Cutscene AD Pipeline

## Goal

Provide a data-driven workflow for intro/outro audio-description timing so scripts can be authored and iterated without code changes.

Current scope:
- Intro/outro cutscene script format and manifest.
- Validation tooling for ordering, overlap, and missing entries.
- No live playback integration yet.

## Files

- `cutscene-ad/manifest.json`: list of supported cutscenes and script file paths.
- `cutscene-ad/scripts/*.json`: one timing script per cutscene.
- `cutscene-ad/templates/cutscene_script.template.json`: starting template for a new script.
- `scripts/Test-CutsceneAdPipeline.ps1`: validator.

## Script Format

Each script file uses:

```json
{
  "schemaVersion": 1,
  "id": "chapter_1_intro",
  "sceneName": "Chapter_1",
  "cutsceneType": "intro",
  "entries": [
    {
      "atSeconds": 0.0,
      "durationSeconds": 2.0,
      "textKey": "ad_chapter_1_intro_001"
    }
  ]
}
```

Field notes:
- `id`: must match manifest cutscene id.
- `sceneName`: scene context used by the game (for example `Chapter_1`).
- `cutsceneType`: `intro` or `outro`.
- `entries`: ordered cue windows.
- `atSeconds`: cue start time from cutscene start.
- `durationSeconds`: active window for that cue (used for overlap checks).
- `textKey`: localization key reserved for the eventual narration layer.

## Validation

Run:

```powershell
pwsh -File .\scripts\Test-CutsceneAdPipeline.ps1
```

Useful options:

```powershell
pwsh -File .\scripts\Test-CutsceneAdPipeline.ps1 -StrictCoverage
pwsh -File .\scripts\Test-CutsceneAdPipeline.ps1 -RequireEntries
pwsh -File .\scripts\Test-CutsceneAdPipeline.ps1 -ValidateLocKeys -LocPath .\Loc.cs
```

Checks include:
- Manifest parse and duplicate cutscene ids.
- Script file existence (strict mode can fail on missing files).
- Entry ordering (`atSeconds` ascending).
- Overlap detection using `atSeconds + durationSeconds`.
- Missing required fields (`atSeconds`, `durationSeconds`, `textKey`).
- Optional localization key existence check (`-ValidateLocKeys`).

## Authoring Workflow

1. Pick cutscene id from `cutscene-ad/manifest.json`.
2. Edit the matching file in `cutscene-ad/scripts/`.
3. Add/update entries with measured timestamps.
4. Run validator.
5. Iterate timestamps and text keys until clean.

