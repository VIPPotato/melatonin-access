#!/bin/bash
#
# Melatonin Access - macOS installer.
#
# Shipped in the release ZIP as install-macOS.command, which runs on a
# double-click in Finder. It installs the mod (and MelonLoader, if it can find
# the download), clears the quarantine flags that stop macOS loading either of
# them, and puts the Steam launch options on the clipboard.
#
# The one thing it cannot do is set the Steam launch options itself: they live
# in Steam's own config, which is unsafe to edit behind Steam's back.
#
# Usage: install-macOS.command [path to Melatonin folder]

set -uo pipefail

say() { printf '%s\n' "$*"; }
fail() { printf 'ERROR: %s\n' "$*" >&2; FAILED=1; }

FAILED=0
SRC="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

say "Melatonin Access - macOS installer"
say "=================================="
say ""

# --- 1. find the game -------------------------------------------------------

find_game() {
    [ -n "${1:-}" ] && { [ -d "$1/Melatonin.app" ] && printf '%s' "$1"; return; }

    local stock="$HOME/Library/Application Support/Steam/steamapps/common/Melatonin"
    [ -d "$stock/Melatonin.app" ] && { printf '%s' "$stock"; return; }

    # Steam libraries on other drives are listed in libraryfolders.vdf.
    local vdf="$HOME/Library/Application Support/Steam/steamapps/libraryfolders.vdf"
    [ -f "$vdf" ] || return
    local line path
    while IFS= read -r line; do
        path="$(printf '%s' "$line" | sed -n 's/.*"path"[^"]*"\(.*\)"/\1/p')"
        [ -n "$path" ] && [ -d "$path/steamapps/common/Melatonin/Melatonin.app" ] && {
            printf '%s' "$path/steamapps/common/Melatonin"; return; }
    done < "$vdf"
}

GAME="$(find_game "${1:-}")"
if [ -z "$GAME" ]; then
    say "Could not find Melatonin."
    say ""
    say "Looked in the usual Steam folder and in any other Steam libraries."
    say "If the game is somewhere unusual, run this installer again and pass"
    say "the folder that contains Melatonin.app, for example:"
    say ""
    say "  \"$SRC/install-macOS.command\" \"/Volumes/Games/steamapps/common/Melatonin\""
    exit 1
fi
say "Found the game:"
say "  $GAME"
say ""

# --- 2. MelonLoader ---------------------------------------------------------

if [ -f "$GAME/MelonLoader.Bootstrap.dylib" ] && [ -f "$GAME/melonloader-launch.sh" ]; then
    say "MelonLoader is already installed."
else
    say "MelonLoader is not installed yet. Looking for the download..."
    ML_ZIP=""
    for cand in "$HOME/Downloads/MelonLoader.macOS.x64.zip" "$SRC/MelonLoader.macOS.x64.zip"; do
        [ -f "$cand" ] && { ML_ZIP="$cand"; break; }
    done

    if [ -z "$ML_ZIP" ]; then
        say ""
        say "Could not find MelonLoader.macOS.x64.zip."
        say ""
        say "Please download it from:"
        say "  https://github.com/LavaGang/MelonLoader/releases"
        say ""
        say "Download the file named MelonLoader.macOS.x64.zip."
        say "Do NOT download MelonLoader.Installer.MacOS.dmg - macOS will say"
        say "it is damaged. The file is fine; macOS blocks it because it is not"
        say "signed by an Apple developer account. The ZIP avoids that."
        say ""
        say "The x64 file is correct on Apple Silicon Macs too."
        say ""
        say "Leave it in your Downloads folder, then run this installer again."
        exit 1
    fi

    say "  Using $ML_ZIP"
    TMP="$(mktemp -d)"
    if ! unzip -qo "$ML_ZIP" -d "$TMP"; then
        fail "could not open $ML_ZIP"; rm -rf "$TMP"; exit 1
    fi
    # The zip may or may not have a top-level folder.
    BASE="$TMP"
    [ -f "$TMP/MelonLoader.Bootstrap.dylib" ] || BASE="$(dirname "$(find "$TMP" -name MelonLoader.Bootstrap.dylib -print -quit)")"
    if [ ! -f "$BASE/MelonLoader.Bootstrap.dylib" ]; then
        fail "that ZIP does not look like MelonLoader for macOS"; rm -rf "$TMP"; exit 1
    fi
    cp "$BASE/MelonLoader.Bootstrap.dylib" "$GAME/" || fail "could not copy MelonLoader.Bootstrap.dylib"
    cp "$BASE/melonloader-launch.sh" "$GAME/"      || fail "could not copy melonloader-launch.sh"
    rm -rf "$GAME/MelonLoader"
    cp -R "$BASE/MelonLoader" "$GAME/"             || fail "could not copy the MelonLoader folder"
    rm -rf "$TMP"
    say "  MelonLoader installed."
fi
say ""

# --- 3. the mod -------------------------------------------------------------

if [ -d "$SRC/Mods" ]; then
    say "Installing the mod..."
    mkdir -p "$GAME/Mods"
    cp -R "$SRC/Mods/." "$GAME/Mods/" || fail "could not copy the Mods folder"
    [ -d "$SRC/UserData" ] && { mkdir -p "$GAME/UserData"; cp -R "$SRC/UserData/." "$GAME/UserData/" || fail "could not copy UserData"; }
    say "  Mod files copied."
else
    say "No Mods folder next to this installer, so the mod itself was not copied."
    say "Run this from the folder you extracted the release ZIP into."
fi
say ""

# --- 4. unblock and make runnable -------------------------------------------

say "Letting macOS load the files..."
xattr -dr com.apple.quarantine "$GAME" 2>/dev/null
chmod +x "$GAME/melonloader-launch.sh" 2>/dev/null
say "  Done."
say ""

# --- 5. check ---------------------------------------------------------------

for f in "Melatonin.app" "MelonLoader.Bootstrap.dylib" "melonloader-launch.sh" \
         "MelonLoader" "Mods/MelatoninAccess.dll" "Mods/libprism.dylib"; do
    [ -e "$GAME/$f" ] || fail "missing after install: $f"
done

if [ "$FAILED" != "0" ]; then
    say ""
    say "Something went wrong. See the errors above."
    exit 1
fi

# --- 6. the one manual step -------------------------------------------------

LAUNCH="\"$GAME/melonloader-launch.sh\" %command%"
printf '%s' "$LAUNCH" | pbcopy 2>/dev/null && COPIED=1 || COPIED=0

say "Installed successfully."
say ""
say "One step left, and it has to be done in Steam."
say ""
say "Steam will not load the mod on its own, so Steam needs to be told to"
say "start the game through MelonLoader."
say ""
say "  1. In Steam, right-click Melatonin and choose Properties."
say "  2. Go to the General tab and find the Launch Options box."
say "  3. Paste this line into it:"
say ""
say "     $LAUNCH"
say ""
if [ "$COPIED" = "1" ]; then
    say "That line has been copied to your clipboard, so you can paste it"
    say "straight into the box with Command-V."
    say ""
fi
say "  4. Close the Properties window and start the game from Steam."
say ""
say "You should hear the mod announce itself. If the game starts but says"
say "nothing, run this installer again - it will re-clear the file blocks -"
say "and double-check the Launch Options line."
exit 0
