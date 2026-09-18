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
# Usage: install-macOS.command [path to Melatonin folder] [path to MelonLoader zip]

set -uo pipefail

say() { printf '%s\n' "$*"; }
fail() { printf 'ERROR: %s\n' "$*" >&2; FAILED=1; }

FAILED=0
SRC="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ML_ZIP_ARG="${2:-}"

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

ML_URL="https://github.com/LavaGang/MelonLoader/releases/latest/download/MelonLoader.macOS.x64.zip"

# Deliberately no scan of ~/Downloads. That folder is TCC-protected, so
# reading it from a .command makes macOS ask "Terminal would like to access
# files in your Downloads folder" -- confusing, and it fails silently if
# declined. A file the user picks in an open panel is granted to us by
# Powerbox instead, with no prompt at all.
ask_for_melonloader() {
    osascript 2>/dev/null <<'AS'
display dialog "MelonLoader is not installed yet.

Melatonin Access needs MelonLoader.macOS.x64.zip.

Do not use the .dmg installer: macOS reports it as damaged. The file is fine, but macOS blocks it because it is not signed by an Apple developer account.

The x64 file is the correct one on Apple Silicon Macs too." ¬
    buttons {"Cancel", "I Have The File", "Download It"} ¬
    default button "Download It" ¬
    with title "Melatonin Access Installer"
return button returned of result
AS
}

pick_melonloader_zip() {
    osascript 2>/dev/null <<'AS'
set f to choose file with prompt "Select MelonLoader.macOS.x64.zip" ¬
    of type {"zip", "public.zip-archive"} ¬
    default location (path to downloads folder)
return POSIX path of f
AS
}

wait_for_download() {
    osascript >/dev/null 2>&1 <<'AS'
display dialog "Your browser is downloading MelonLoader.macOS.x64.zip.

When the download has finished, click Choose File and select it." ¬
    buttons {"Cancel", "Choose File"} ¬
    default button "Choose File" ¬
    with title "Melatonin Access Installer"
AS
}

install_melonloader_from() {
    local zip="$1"
    say "  Using $zip"
    local tmp; tmp="$(mktemp -d)"
    if ! unzip -qo "$zip" -d "$tmp"; then
        fail "could not open that ZIP"; rm -rf "$tmp"; return 1
    fi
    local base="$tmp"
    if [ ! -f "$tmp/MelonLoader.Bootstrap.dylib" ]; then
        local found; found="$(find "$tmp" -name MelonLoader.Bootstrap.dylib -print -quit)"
        [ -n "$found" ] && base="$(dirname "$found")"
    fi
    if [ ! -f "$base/MelonLoader.Bootstrap.dylib" ]; then
        fail "that ZIP does not look like MelonLoader for macOS"
        say "  Make sure you picked MelonLoader.macOS.x64.zip."
        rm -rf "$tmp"; return 1
    fi
    cp "$base/MelonLoader.Bootstrap.dylib" "$GAME/" || { fail "could not copy MelonLoader.Bootstrap.dylib"; rm -rf "$tmp"; return 1; }
    cp "$base/melonloader-launch.sh" "$GAME/"      || { fail "could not copy melonloader-launch.sh"; rm -rf "$tmp"; return 1; }
    rm -rf "$GAME/MelonLoader"
    cp -R "$base/MelonLoader" "$GAME/"             || { fail "could not copy the MelonLoader folder"; rm -rf "$tmp"; return 1; }
    rm -rf "$tmp"
    say "  MelonLoader installed."
    return 0
}

if [ -f "$GAME/MelonLoader.Bootstrap.dylib" ] && [ -f "$GAME/melonloader-launch.sh" ] && [ -d "$GAME/MelonLoader" ]; then
    say "MelonLoader is already installed."
elif [ -n "$ML_ZIP_ARG" ]; then
    say "Installing MelonLoader..."
    install_melonloader_from "$ML_ZIP_ARG" || exit 1
else
    ANSWER="$(ask_for_melonloader)"
    case "$ANSWER" in
        "Download It")
            open "$ML_URL"
            wait_for_download || { say "Cancelled."; exit 1; }
            ZIP="$(pick_melonloader_zip)"
            ;;
        "I Have The File")
            ZIP="$(pick_melonloader_zip)"
            ;;
        *)
            say "Cancelled. MelonLoader is required, so nothing was installed."
            say ""
            say "You can download it yourself from:"
            say "  $ML_URL"
            exit 1
            ;;
    esac

    if [ -z "${ZIP:-}" ]; then
        say "No file chosen, so MelonLoader was not installed."
        exit 1
    fi
    say "Installing MelonLoader..."
    install_melonloader_from "$ZIP" || exit 1
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
