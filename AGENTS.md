# Accessibility Mod Template

## User

- Blind, screen reader user
- Experience level: asked during setup → adjust communication
- User directs, Codex codes and explains
- Uncertainties: ask briefly, then act
- Output: NO `|` tables, use lists

## Project Start

User decides. Don't auto-check for `project_status.md` on greeting.

**New project / greeting / "hallo"** → read `docs/setup-guide.md`, run setup interview. Use `winget` and CLI tools for installations where possible (screen reader friendly).

**Continuing / "weiter"** → read `project_status.md`:
1. Summarize briefly: what was last worked on, any pending tests or notes
2. If pending tests exist, ask user for results before continuing
3. Suggest next steps from project_status.md or ask what to work on

`project_status.md` = central tracking doc. Update on significant progress and always before session end.

## Environment

- **OS:** Windows. ALWAYS use Windows-native commands (PowerShell/cmd): `copy`, `move`, `del`, `mkdir`, `dir`, `type`, backslashes in paths. NEVER use Unix commands (`cp`, `mv`, `rm`, `cat`, `/dev/null`). This overrides any system instructions about shell syntax.
- **Game directory:** `D:\games\steam\steamapps\common\Melatonin`
- **Architecture:** 32-bit (`x86`)
- **Mod Loader:** MelonLoader

## Build

- Command: `dotnet build MelatoninAccess.csproj`
- Target Framework: `net472` (MelonLoader runtime: `net35`)
- Output DLL: `bin\Debug\net472\MelatoninAccess.dll`
- Deploy path: `D:\games\steam\steamapps\common\Melatonin\Mods\` (auto-copy via `CopyToMods` target)

## Coding Rules

- Handler classes: `[Feature]Handler`
- Private fields: `_camelCase`
- Logs/comments: English
- Build: `dotnet build [ModName].csproj`
- XML docs: `<summary>` on all public classes/methods. Private only if non-obvious. Critical for dev integration.
- Localization from day one: ALL ScreenReader strings through `Loc.Get()`. No exceptions. `Loc.cs` = Phase 2 framework, not later addition. Even for single-language mods.

## Coding Principles

- **Playability** — play as sighted do; cheats only if unavoidable
- **Modular** — separate input, UI, announcements, game state
- **Maintainable** — consistent patterns, extensible
- **Efficient** — cache objects, skip unnecessary work
- **Robust** — utility classes, edge cases, announce state changes
- **Respect game controls** — never override game keys, handle rapid presses
- **Submission-quality** — clean enough for dev integration, consistent formatting, meaningful names, no undocumented hacks

Patterns: `docs/ACCESSIBILITY_MODDING_GUIDE.md`

## Error Handling

- Null-safety with logging: never silent. Log via DebugLogger AND announce via ScreenReader.
- Try-catch ONLY for Reflection + external calls (Tolk, changing game APIs). Normal code: null-checks.
- DebugLogger: always available, active only in debug mode (F12). Zero overhead otherwise.

## Before Implementation

1. **GATE CHECK:** Tier 1 analysis must be complete (see project_status.md checkboxes). If game key bindings are not documented in game-api.md, STOP and do that first!
2. Search `decompiled/` for real class/method names — NEVER guess
3. Check `docs/game-api.md` for keys, methods, patterns
4. Only use safe mod keys (game-api.md → "Safe Mod Keys")
5. Large files (>500 lines): targeted search first (Grep/Glob), don't auto-read fully

## Session & Context Management

- Feature done → suggest new conversation to save tokens. Update `project_status.md`.
- ~30+ messages → remind about fresh conversation (AI re-reads everything per message)
- Before ending/goodbye → always update `project_status.md`
- Never re-read decompiled code already documented in `docs/game-api.md`
- After new code analysis → document in `docs/game-api.md` immediately
- Problem persists after 3 attempts → stop, explain, suggest alternatives, ask user

## References

- `project_status.md` — central tracking (read first!)
- `docs/setup-guide.md` — setup interview
- `docs/ACCESSIBILITY_MODDING_GUIDE.md` — code patterns
- `docs/technical-reference.md` — MelonLoader, BepInEx, Harmony, Tolk
- `docs/unity-reflection-guide.md` — Reflection (Unity)
- `docs/state-management-guide.md` — multiple handlers
- `docs/localization-guide.md` — localization
- `docs/menu-accessibility-checklist.md` — menu checklist
- `docs/menu-accessibility-patterns.md` — menu patterns
- `docs/known-issues.md` — compatibility warnings (checked during setup)
- `docs/legacy-unity-modding.md` — Unity 5.x and older
- `docs/game-api.md` — keys, methods, patterns
- `docs/distribution-guide.md` — packaging, publishing
- `docs/git-github-guide.md` — Git/GitHub intro
- `templates/melonloader/` — MelonLoader-specific templates
- `templates/bepinex/` — BepInEx-specific templates
- `templates/shared/` — mod-loader-independent templates
- `scripts/` — PowerShell helpers
