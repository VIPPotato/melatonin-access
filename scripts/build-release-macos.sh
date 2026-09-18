#!/bin/bash
#
# Build the macOS release package for Melatonin Access.
#
# The macOS counterpart of Build-ReleasePackage.ps1. Same staged layout, with
# the platform's native speech library in place of the Windows ones:
#
#   MelatoninAccess-<version>/
#   |-- Mods/
#   |   |-- MelatoninAccess.dll
#   |   |-- libprism.dylib          (Windows ships Tolk.dll + nvdaControllerClient32.dll)
#   |   |-- cutscene-ad/{manifest.json,scripts/}
#   |   `-- localization/
#   |-- UserData/Loader.cfg
#   |-- install-macOS.command
#   `-- README-macOS.txt
#
# Usage:
#   scripts/build-release-macos.sh [--version v1.3.0] [--configuration Release]
#                                  [--no-build] [--keep-stage] [--skip-qa]

set -euo pipefail

VERSION="v1.3.0"
CONFIGURATION="Release"
DO_BUILD=1
KEEP_STAGE=0
SKIP_QA=0

while [ $# -gt 0 ]; do
    case "$1" in
        --version)       VERSION="$2"; shift 2 ;;
        --configuration) CONFIGURATION="$2"; shift 2 ;;
        --no-build)      DO_BUILD=0; shift ;;
        --keep-stage)    KEEP_STAGE=1; shift ;;
        --skip-qa)       SKIP_QA=1; shift ;;
        -h|--help)       sed -n '2,25p' "$0"; exit 0 ;;
        *) echo "ERROR: unknown option '$1'" >&2; exit 2 ;;
    esac
done

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

MOD_DLL="$ROOT/bin/$CONFIGURATION/net472/MelatoninAccess.dll"
PRISM_DYLIB="$ROOT/libs/macos/libprism.dylib"
CUTSCENE_MANIFEST="$ROOT/cutscene-ad/manifest.json"
CUTSCENE_SCRIPTS="$ROOT/cutscene-ad/scripts"
LOCALIZATION="$ROOT/localization"

LOADER_CFG=""
for c in "$ROOT/UserData/Loader.cfg" "$ROOT/UserConfig/Loader.cfg"; do
    [ -f "$c" ] && { LOADER_CFG="$c"; break; }
done

fail() { echo "ERROR: $*" >&2; exit 1; }

# --- build ------------------------------------------------------------------

if [ "$DO_BUILD" = "1" ]; then
    command -v msbuild >/dev/null 2>&1 || fail "msbuild not found. Install Mono, or pass --no-build."
    echo "Building ($CONFIGURATION)..."
    # Mono's msbuild prints a ".NET Core SDK not found" notice on every
    # invocation. It is harmless here -- the net472 build does not use one --
    # but it reads like a failure, so filter it out while keeping real output.
    noise='It was not possible to find any installed .NET Core SDKs|Did you mean to run .NET Core SDK|aka.ms/dotnet-download'
    msbuild "$ROOT/MelatoninAccess.csproj" /t:Restore /v:quiet /nologo 2>&1 | grep -Ev "$noise" || true
    msbuild "$ROOT/MelatoninAccess.csproj" /t:Build /p:Configuration="$CONFIGURATION" /v:quiet /nologo 2>&1 | grep -Ev "$noise" || true
fi

# --- preflight --------------------------------------------------------------

[ -f "$MOD_DLL" ]           || fail "Mod DLL not found: $MOD_DLL (build first, or drop --no-build)"
[ -f "$PRISM_DYLIB" ]       || fail "Speech library not found: $PRISM_DYLIB"
[ -f "$CUTSCENE_MANIFEST" ] || fail "Cutscene AD manifest not found: $CUTSCENE_MANIFEST"
[ -d "$CUTSCENE_SCRIPTS" ]  || fail "Cutscene AD scripts folder not found: $CUTSCENE_SCRIPTS"
[ -d "$LOCALIZATION" ]      || fail "Localization folder not found: $LOCALIZATION"
[ -n "$LOADER_CFG" ]        || fail "Loader config not found (checked UserData/ and UserConfig/)"
[ -f "$ROOT/scripts/install-macos.sh" ] || fail "Installer not found: $ROOT/scripts/install-macos.sh"

# A release without both slices silently breaks one kind of Mac: dyld refuses
# an architecture mismatch and the mod runs with no speech at all.
ARCHS="$(lipo -archs "$PRISM_DYLIB" 2>/dev/null || echo "")"
case "$ARCHS" in
    *x86_64*arm64*|*arm64*x86_64*) : ;;
    *) fail "libprism.dylib must be universal (x86_64 + arm64); found: ${ARCHS:-unknown}" ;;
esac
echo "Speech library: universal ($ARCHS)"

# --- QA ---------------------------------------------------------------------

if [ "$SKIP_QA" = "1" ]; then
    echo "QA checks skipped (--skip-qa)."
    QA_NOTE="skipped by request"
elif command -v pwsh >/dev/null 2>&1; then
    echo "Running localization QA check..."
    pwsh -File "$ROOT/scripts/Test-LocalizationQA.ps1" -LocalizationDir "$LOCALIZATION"
    echo "Running cutscene AD QA check..."
    pwsh -File "$ROOT/scripts/Test-CutsceneAdPipeline.ps1" \
        -ManifestPath "$CUTSCENE_MANIFEST" -StrictCoverage -RequireEntries \
        -ValidateLocKeys -LocalizationDir "$LOCALIZATION"
    QA_NOTE="passed"
else
    echo
    echo "WARNING: pwsh not installed, so the localization and cutscene QA"
    echo "         checks did NOT run. They are PowerShell and validate data"
    echo "         that is platform-independent, so a Windows build of the"
    echo "         same commit covers them. Install PowerShell, or run them"
    echo "         on Windows, before publishing this package."
    echo
    QA_NOTE="NOT RUN (pwsh unavailable)"
fi

# --- stage ------------------------------------------------------------------

RELEASE_DIR="$ROOT/release"
STAGE="$RELEASE_DIR/MelatoninAccess-$VERSION-macOS"
ZIP="$RELEASE_DIR/MelatoninAccess-$VERSION-macOS.zip"

rm -rf "$STAGE"
mkdir -p "$STAGE/Mods/cutscene-ad" "$STAGE/UserData"

cp "$MOD_DLL" "$STAGE/Mods/MelatoninAccess.dll"
cp "$PRISM_DYLIB" "$STAGE/Mods/libprism.dylib"
cp "$CUTSCENE_MANIFEST" "$STAGE/Mods/cutscene-ad/manifest.json"
cp -R "$CUTSCENE_SCRIPTS" "$STAGE/Mods/cutscene-ad/scripts"
cp -R "$LOCALIZATION" "$STAGE/Mods/localization"
cp "$LOADER_CFG" "$STAGE/UserData/Loader.cfg"

# Shipped as .command so Finder runs it in Terminal on a double-click; users
# never have to type or paste a shell command.
cp "$ROOT/scripts/install-macos.sh" "$STAGE/install-macOS.command"
chmod +x "$STAGE/install-macOS.command"

# Anything that reached this machine through a browser carries a quarantine
# flag, and a quarantined dylib cannot be dlopen'd. Don't ship it.
xattr -cr "$STAGE" 2>/dev/null || true

cat > "$STAGE/README-macOS.txt" <<'TXT'
Melatonin Access - macOS
========================

Easiest way: double-click install-macOS.command
------------------------------------------------

It finds the game, installs the mod, installs MelonLoader if it finds
MelonLoader.macOS.x64.zip in your Downloads folder, unblocks the files, and
copies the Steam launch options line to your clipboard. Then follow the one
remaining step it prints, which has to be done in Steam itself.

If Finder refuses to run it, right-click it and choose Open, then confirm.

Doing it by hand instead
------------------------

Requires MelonLoader (https://github.com/LavaGang/MelonLoader) installed into
the Melatonin game folder. Download MelonLoader.macOS.x64.zip, not the
installer DMG - macOS reports the DMG as damaged because it is not signed by
an Apple developer account.

1. Copy the Mods and UserData folders from this archive into the game folder:

     ~/Library/Application Support/Steam/steamapps/common/Melatonin/

2. Steam will not load MelonLoader on its own, because injection happens
   through DYLD_INSERT_LIBRARIES, which Steam does not set. Point Steam at
   MelonLoader's launch wrapper instead:

     Steam -> right-click Melatonin -> Properties -> General -> Launch Options

     "/Users/YOURNAME/Library/Application Support/Steam/steamapps/common/Melatonin/melonloader-launch.sh" %command%

   Write the path out in full: Steam does not expand ~ or $HOME here, and a
   relative path fails with a generic launch error. Keep the quotes and the
   trailing %command%.

3. Launch from Steam. Speech goes through VoiceOver.

If the mod is silent
--------------------

macOS quarantines files downloaded through a browser, and refuses to load a
quarantined library. Clear it:

    cd ~/Library/Application\ Support/Steam/steamapps/common/Melatonin
    xattr -dr com.apple.quarantine Mods/libprism.dylib

or allow it once from System Settings -> Privacy & Security.

To confirm what happened, check MelonLoader's log:

    tail -30 ~/Library/Application\ Support/Steam/steamapps/common/Melatonin/MelonLoader/Latest.log

"Prism (VoiceOver (macOS)) loaded successfully" means speech is working.
TXT

# --- zip --------------------------------------------------------------------

rm -f "$ZIP"
( cd "$STAGE" && zip -q -r -X "$ZIP" . -x '.DS_Store' -x '__MACOSX/*' )

[ "$KEEP_STAGE" = "1" ] || rm -rf "$STAGE"

echo
echo "Created release package:"
echo "  $ZIP"
echo "  Size: $(wc -c < "$ZIP" | tr -d ' ') bytes"
echo "  QA:   $QA_NOTE"
[ "$KEEP_STAGE" = "1" ] && echo "  Stage kept: $STAGE"
exit 0
