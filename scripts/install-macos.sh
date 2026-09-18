#!/bin/bash
#
# Melatonin Access - macOS installer.
#
# Shipped in the release ZIP as install-macOS.command, which Finder opens in
# Terminal. It installs the mod (offering to fetch MelonLoader if it is
# missing), clears the quarantine flags that stop macOS loading either of
# them, and puts the Steam launch options on the clipboard.
#
# Progress is printed to Terminal, but everything the user has to act on is
# also shown in a dialog: Terminal can be configured to close its window the
# instant a script exits, which would otherwise take the output with it.
#
# The one thing it cannot do is set the Steam launch options itself: they live
# in Steam's own config, which is unsafe to edit behind Steam's back.
#
# Usage: install-macOS.command [path to Melatonin folder] [path to MelonLoader zip]

set -uo pipefail

say() { printf '%s\n' "$*"; }

# Terminal may be set to close its window the moment a script exits cleanly
# (Settings > Profiles > Shell > "When the shell exits"). With that on, nothing
# printed here is readable afterwards, so anything the user must actually see
# goes through a dialog as well.
# Returns non-zero if the dialog could not be shown, so callers can fall back
# to holding the Terminal window open instead of vanishing silently.
dialog() {
    local title="$1" body="$2"
    osascript >/dev/null 2>&1 <<AS
display dialog "$(printf '%s' "$body" | sed 's/\\/\\\\/g; s/"/\\"/g')" ¬
    buttons {"OK"} default button "OK" with title "$title"
AS
}

fail() { printf 'ERROR: %s\n' "$*" >&2; FAILED=1; FAIL_MSG="${FAIL_MSG:-}$*
"; }

die() {
    say "$1"
    dialog "Melatonin Access Installer" "$1" || {
        say ""
        say "Press Return to close this window."
        read -r _ || true
    }
    exit "${2:-1}"
}

FAILED=0
FAIL_MSG=""
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
    die "Could not find Melatonin.

Looked in the usual Steam folder and in any other Steam library on this Mac.

If the game is installed somewhere unusual, run this installer from Terminal and pass the folder that contains Melatonin.app, for example:

\"$SRC/install-macOS.command\" \"/Volumes/Games/steamapps/common/Melatonin\""
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
tell me to activate
display dialog "MelonLoader is not installed yet.

Melatonin Access needs MelonLoader.macOS.x64.zip.

Do not use the .dmg installer: macOS reports it as damaged. The file is fine, but macOS blocks it because it is not signed by an Apple developer account.

The x64 file is the correct one on Apple Silicon Macs too.

Choose Download It and this installer will fetch it for you. Choose I Have The File if you have already downloaded it." ¬
    buttons {"Cancel", "I Have The File", "Download It"} ¬
    default button "Download It" ¬
    with title "Melatonin Access Installer"
return button returned of result
AS
}

pick_melonloader_zip() {
    osascript 2>/dev/null <<'AS'
tell me to activate
set f to choose file with prompt "Select MelonLoader.macOS.x64.zip" ¬
    of type {"zip", "public.zip-archive"} ¬
    default location (path to downloads folder)
return POSIX path of f
AS
}

# Downloaded here rather than in a browser. Handing focus to another app is
# the problem: osascript cannot make itself a foreground application, so once
# focus has gone its dialogs are not real windows -- they only reach VoiceOver
# through the system dialogs list, and every dialog after that has to be hunted
# down again. Fetching the file ourselves means focus never leaves.
download_melonloader() {
    local dest="$1"
    say "Downloading MelonLoader (about 25 MB)..."
    say "  from $ML_URL"
    curl -fL --progress-bar -o "$dest" "$ML_URL" || return 1
    [ -s "$dest" ] || return 1
    say "  download finished."
    return 0
}

offer_manual_download() {
    osascript 2>/dev/null <<'AS'
display dialog "The download did not succeed.

You can download MelonLoader.macOS.x64.zip yourself from the MelonLoader releases page, then open this installer again and choose I Have The File." ¬
    buttons {"Open The Page", "Set Up Later"} ¬
    default button "Open The Page" ¬
    with title "Melatonin Access Installer"
return button returned of result
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
    install_melonloader_from "$ML_ZIP_ARG" || die "MelonLoader could not be installed.

$FAIL_MSG"
else
    ANSWER="$(ask_for_melonloader)"
    case "$ANSWER" in
        "Download It")
            DLDIR="$(mktemp -d)"
            if download_melonloader "$DLDIR/MelonLoader.macOS.x64.zip"; then
                ZIP="$DLDIR/MelonLoader.macOS.x64.zip"
            else
                say "  download failed."
                if [ "$(offer_manual_download)" = "Open The Page" ]; then
                    open "https://github.com/LavaGang/MelonLoader/releases"
                fi
                say "Nothing was installed."
                exit 0
            fi
            ;;
        "I Have The File")
            ZIP="$(pick_melonloader_zip)"
            ;;
        *)
            die "Cancelled. MelonLoader is required, so nothing was installed.

You can download it yourself from:
$ML_URL"
            ;;
    esac

    if [ -z "${ZIP:-}" ]; then
        die "No file was chosen, so MelonLoader was not installed."
    fi
    say "Installing MelonLoader..."
    install_melonloader_from "$ZIP" || die "MelonLoader could not be installed.

$FAIL_MSG"
fi
say ""

# --- 3. the mod -------------------------------------------------------------

copy_payload() {
    local src="$1"
    [ -d "$src/Mods" ] || return 1
    mkdir -p "$GAME/Mods" || return 1
    cp -R "$src/Mods/." "$GAME/Mods/" 2>/dev/null || return 1
    if [ -d "$src/UserData" ]; then
        mkdir -p "$GAME/UserData" || return 1
        cp -R "$src/UserData/." "$GAME/UserData/" 2>/dev/null || return 1
    fi
    return 0
}

# Downloads, Desktop and Documents are protected by macOS. A script opened
# from one of them can stat its own folder but not read the files in it, so
# the copy fails with no prompt the user can act on. A folder the user picks
# in an open panel is granted to us by Powerbox, which is the way out.
ask_for_payload_folder() {
    osascript 2>/dev/null <<'AS'
tell me to activate
display dialog "macOS is not letting this installer read the mod files.

That happens when they are in Downloads, Desktop or Documents, which macOS protects.

Click Choose Folder and select the folder this installer is in, the one containing Mods and UserData. Picking it yourself is what gives macOS permission." ¬
    buttons {"Cancel", "Choose Folder"} ¬
    default button "Choose Folder" ¬
    with title "Melatonin Access Installer"
set f to choose folder with prompt "Select the folder containing Mods and UserData"
return POSIX path of f
AS
}

say "Installing the mod..."
if copy_payload "$SRC"; then
    say "  Mod files copied."
else
    say "  Could not read the mod files from $SRC - asking for access..."
    PICKED="$(ask_for_payload_folder)"
    PICKED="${PICKED%/}"
    if [ -n "$PICKED" ] && copy_payload "$PICKED"; then
        say "  Mod files copied."
    else
        die "The mod files could not be copied.

They should sit next to this installer, in folders named Mods and UserData.

If they do, macOS is blocking access to them. Either:

- Move the whole extracted folder out of Downloads, for example into your home folder, and open the installer again, or
- Allow Terminal to reach the folder in System Settings, under Privacy & Security, Files and Folders."
    fi
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
    die "Something went wrong and the mod is not fully installed.

$FAIL_MSG"
fi

# --- 6. the one manual step -------------------------------------------------

LAUNCH="\"$GAME/melonloader-launch.sh\" %command%"

say "Installed successfully."
say ""
say "One step left, and it has to be done in Steam:"
say ""
say "  1. In Steam, select Melatonin and open its Properties."
say "  2. On the General tab, find the Launch Options box."
say "  3. Put this line in it:"
say ""
say "$LAUNCH"
say ""
say "  4. Close Properties and start the game from Steam."
say ""

# The launch options line goes in the dialog's text field rather than in its
# message, so VoiceOver reads it as its own element instead of running it on
# from the surrounding sentence, and it can be selected and copied in place.
#
# The clipboard is only touched if the user asks: taking it without asking
# would throw away whatever they had on it.
launch_dialog() {
    local note="$1"
    osascript 2>/dev/null <<AS
display dialog "${note}The mod is installed.

One step is left, and it has to be done in Steam.

1. In Steam, select Melatonin and open its Properties.
2. On the General tab, find the Launch Options box.
3. Put the line below into that box.
4. Close Properties and start the game from Steam.

This is how MelonLoader works on macOS: Steam does not set the variable it needs, so every MelonLoader game is launched through a wrapper script in its own folder. Without that line the game runs unmodded." ¬
    default answer "$(printf '%s' "$LAUNCH" | sed 's/\\/\\\\/g; s/"/\\"/g')" ¬
    buttons {"Copy To Clipboard", "Done"} ¬
    default button "Done" ¬
    with title "Melatonin Access Installer"
return button returned of result
AS
}

NOTE=""
DIALOGS_WORK=1
FIRST=1
while :; do
    BTN="$(launch_dialog "$NOTE")"
    if [ "$FIRST" = "1" ] && [ -z "$BTN" ]; then
        # No GUI: the instructions above are the only copy the user gets, so
        # the window has to stay open.
        DIALOGS_WORK=0
        break
    fi
    FIRST=0
    case "$BTN" in
        "Copy To Clipboard")
            printf '%s' "$LAUNCH" | pbcopy 2>/dev/null \
                && NOTE="Copied. Paste it into Steam with Command-V.

" \
                || NOTE="Could not reach the clipboard. Copy the line from the text field instead.

"
            say "Copied the launch options line to the clipboard."
            ;;
        *)
            break
            ;;
    esac
done

# Everything the user needs is in the dialog, so let Terminal close on its own.
# Only when no dialog could be shown does the window have to be held.
if [ "$DIALOGS_WORK" = "0" ]; then
    say "Press Return to close this window."
    read -r _ || true
fi
exit 0
