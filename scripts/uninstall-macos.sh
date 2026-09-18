#!/bin/bash
#
# Melatonin Access - macOS uninstaller.
#
# Shipped in the release ZIP as uninstall-macOS.command, which Finder opens in
# Terminal. Offers to remove Melatonin Access, MelonLoader, or both.
#
# MelonLoader ships no uninstaller of its own in MelonLoader.macOS.x64.zip,
# which is the download these instructions use, so removing it is offered here.
#
# Usage: uninstall-macOS.command [path to Melatonin folder]

set -uo pipefail

say() { printf '%s\n' "$*"; }

# Returns non-zero if the dialog could not be shown, so callers can fall back
# to holding the Terminal window open instead of vanishing silently.
dialog() {
    local title="$1" body="$2"
    osascript >/dev/null 2>&1 <<AS
display dialog "$(printf '%s' "$body" | sed 's/\\/\\\\/g; s/"/\\"/g')" ¬
    buttons {"OK"} default button "OK" with title "$title"
AS
}

die() {
    say "$1"
    dialog "Melatonin Access Uninstaller" "$1" || hold
    exit "${2:-1}"
}

# The window is only held open when dialogs are unavailable; otherwise the
# report is in a dialog and Terminal can close as the user's profile dictates.
hold() { say ""; say "Press Return to close this window."; read -r _ || true; }

SRC="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
DIALOGS_WORK=1

say "Melatonin Access - macOS uninstaller"
say "===================================="
say ""

# --- find the game ----------------------------------------------------------

find_game() {
    [ -n "${1:-}" ] && { [ -d "$1/Melatonin.app" ] && printf '%s' "$1"; return; }
    local stock="$HOME/Library/Application Support/Steam/steamapps/common/Melatonin"
    [ -d "$stock/Melatonin.app" ] && { printf '%s' "$stock"; return; }
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
[ -n "$GAME" ] || die "Could not find Melatonin.

Looked in the usual Steam folder and in any other Steam library on this Mac."
say "Found the game:"
say "  $GAME"
say ""

# --- what is actually installed ---------------------------------------------

MOD_PARTS=("Mods/MelatoninAccess.dll" "Mods/libprism.dylib" "Mods/cutscene-ad" "Mods/localization")
ML_PARTS=("MelonLoader.Bootstrap.dylib" "MelonLoader.Bootstrap.dylib.dSYM" "melonloader-launch.sh" "MelonLoader")

MOD_PRESENT=0; for p in "${MOD_PARTS[@]}"; do [ -e "$GAME/$p" ] && MOD_PRESENT=1; done
ML_PRESENT=0;  for p in "${ML_PARTS[@]}";  do [ -e "$GAME/$p" ] && ML_PRESENT=1; done

if [ "$MOD_PRESENT" = "0" ] && [ "$ML_PRESENT" = "0" ]; then
    say "Neither Melatonin Access nor MelonLoader is installed."
    dialog "Melatonin Access Uninstaller" "Nothing to remove.

Neither Melatonin Access nor MelonLoader is installed in:
$GAME" || hold
    exit 0
fi

# --- choose what to remove --------------------------------------------------
#
# One plain question per component, rather than checkboxes.
#
# An AppleScript list does not announce its selection state to VoiceOver, and
# a Cocoa NSAlert with real checkboxes needs a properly launched app run loop
# that osascript does not give it -- the window appears with no controls and
# stops responding. A "display dialog" with named buttons is the one thing
# that is reliably announced, so each component gets its own question and the
# answer IS the state: there is nothing for VoiceOver to have to report.

ask_remove() {
    local what="$1" detail="$2"
    osascript 2>/dev/null <<AS
display dialog "Remove $what?

$detail" ¬
    buttons {"Cancel", "Keep", "Remove"} ¬
    default button "Keep" ¬
    with title "Uninstall Melatonin Access and / or MelonLoader"
return button returned of result
AS
}

confirm_removal() {
    local summary="$1"
    osascript 2>/dev/null <<AS
display dialog "About to remove:

$summary

This cannot be undone." ¬
    buttons {"Cancel", "Uninstall"} ¬
    default button "Cancel" ¬
    with title "Uninstall Melatonin Access and / or MelonLoader"
return button returned of result
AS
}

REMOVE_MOD=0; REMOVE_ML=0

if [ "$MOD_PRESENT" = "1" ]; then
    ANS="$(ask_remove "Melatonin Access" "The mod, its speech library, its cutscene and localization data, and your Melatonin Access settings. This cannot be undone.")"
    [ -z "$ANS" ] && DIALOGS_WORK=0
    [ "$ANS" = "Cancel" ] && { say "Cancelled. Nothing was removed."; exit 0; }
    [ "$ANS" = "Remove" ] && REMOVE_MOD=1
fi

if [ "$DIALOGS_WORK" = "1" ] && [ "$ML_PRESENT" = "1" ]; then
    ANS="$(ask_remove "MelonLoader" "The mod loader itself. Any other mods you have will stop loading too, and you will need to clear the Steam launch options.")"
    [ -z "$ANS" ] && DIALOGS_WORK=0
    [ "$ANS" = "Cancel" ] && { say "Cancelled. Nothing was removed."; exit 0; }
    [ "$ANS" = "Remove" ] && REMOVE_ML=1
fi

if [ "$DIALOGS_WORK" = "0" ]; then
    # No GUI: ask in the terminal and keep the window open.
    say "Could not show the dialogs, so asking here instead."
    say ""
    REMOVE_MOD=0; REMOVE_ML=0
    [ "$MOD_PRESENT" = "1" ] && { printf 'Remove Melatonin Access? [y/N] '; read -r a; [ "${a:-}" = "y" ] && REMOVE_MOD=1; }
    [ "$ML_PRESENT" = "1" ] && { printf 'Remove MelonLoader? [y/N] '; read -r a; [ "${a:-}" = "y" ] && REMOVE_ML=1; }
fi

if [ "$REMOVE_MOD" = "0" ] && [ "$REMOVE_ML" = "0" ]; then
    say "Nothing was selected, so nothing was removed."
    dialog "Melatonin Access Uninstaller" "Nothing was selected, so nothing was removed." || hold
    exit 0
fi

if [ "$DIALOGS_WORK" = "1" ]; then
    SUMMARY=""
    [ "$REMOVE_MOD" = "1" ] && SUMMARY="$SUMMARY- Melatonin Access
"
    [ "$REMOVE_ML" = "1" ] && SUMMARY="$SUMMARY- MelonLoader
"
    [ "$(confirm_removal "$SUMMARY")" = "Uninstall" ] || {
        say "Cancelled. Nothing was removed."
        exit 0
    }
fi

# --- remove -----------------------------------------------------------------

REMOVED=""; KEPT=""

# The mod keeps its settings in one category inside MelonPreferences.cfg,
# which every MelonLoader mod shares. Strip that category rather than the
# file, so another mod's settings are not collateral damage, and drop the
# file only when nothing else is left in it.
remove_mod_settings() {
    local cfg="$GAME/UserData/MelonPreferences.cfg"
    [ -f "$cfg" ] || return 0
    awk '/^\[/ { inmod = ($0 == "[MelatoninAccess]") } !inmod { print }' "$cfg" > "$cfg.tmp" 2>/dev/null || return 1
    if [ -n "$(grep -v '^[[:space:]]*$' "$cfg.tmp" 2>/dev/null)" ]; then
        mv "$cfg.tmp" "$cfg" && {
            say "  removed the Melatonin Access settings from UserData/MelonPreferences.cfg"
            REMOVED="$REMOVED
- Melatonin Access settings (from UserData/MelonPreferences.cfg)"
        }
    else
        rm -f "$cfg" "$cfg.tmp" && {
            say "  removed UserData/MelonPreferences.cfg"
            REMOVED="$REMOVED
- UserData/MelonPreferences.cfg"
        }
    fi
}

if [ "$REMOVE_MOD" = "1" ]; then
    say "Removing Melatonin Access..."
    for p in "${MOD_PARTS[@]}"; do
        [ -e "$GAME/$p" ] || continue
        rm -rf "$GAME/$p" && { say "  removed $p"; REMOVED="$REMOVED
- $p"; }
    done
    remove_mod_settings
    # Other mods live in Mods/ too, so the folder itself stays.
    KEPT="$KEPT
- Your other mods in Mods/"
fi

if [ "$REMOVE_ML" = "1" ]; then
    say "Removing MelonLoader..."
    for p in "${ML_PARTS[@]}"; do
        [ -e "$GAME/$p" ] || continue
        rm -rf "$GAME/$p" && { say "  removed $p"; REMOVED="$REMOVED
- $p"; }
    done
    # Plugins/ and UserLibs/ are MelonLoader's, but may hold things the user
    # put there. Remove only when empty.
    for d in Plugins UserLibs; do
        [ -d "$GAME/$d" ] || continue
        if [ -z "$(ls -A "$GAME/$d" 2>/dev/null)" ]; then
            rmdir "$GAME/$d" 2>/dev/null && { say "  removed $d (was empty)"; REMOVED="$REMOVED
- $d"; }
        else
            KEPT="$KEPT
- $d/ (not empty)"
        fi
    done
    # UserData holds MelonPreferences.cfg, which is where mod settings live.
    # Loader.cfg is MelonLoader's own; take it with MelonLoader and drop
    # UserData when nothing else is using it.
    [ -f "$GAME/UserData/Loader.cfg" ] && rm -f "$GAME/UserData/Loader.cfg" && {
        say "  removed UserData/Loader.cfg"; REMOVED="$REMOVED
- UserData/Loader.cfg"; }
    if [ -d "$GAME/UserData" ]; then
        if [ -z "$(ls -A "$GAME/UserData" 2>/dev/null)" ]; then
            rmdir "$GAME/UserData" 2>/dev/null && { say "  removed UserData (was empty)"; REMOVED="$REMOVED
- UserData"; }
        else
            KEPT="$KEPT
- UserData/ (other mods' settings)"
        fi
    fi
fi

say ""

# --- report -----------------------------------------------------------------

WARN=""
if [ "$REMOVE_ML" = "1" ]; then
    WARN="

IMPORTANT: clear the Steam launch options now.

Steam is still set to start the game through melonloader-launch.sh, which has just been removed. Until you clear that box the game will not start at all.

In Steam, select Melatonin, open Properties, go to the General tab, and empty the Launch Options box."
elif [ "$REMOVE_MOD" = "1" ]; then
    WARN="

MelonLoader is still installed, so you can leave the Steam launch options alone."
fi

say "Done."
[ -n "$REMOVED" ] && { say "Removed:"; say "$REMOVED"; }
[ -n "$KEPT" ] && { say ""; say "Left alone:"; say "$KEPT"; }
[ -n "$WARN" ] && say "$WARN"

dialog "Melatonin Access Uninstaller" "Done.

Removed:$REMOVED
${KEPT:+
Left alone:$KEPT}"

# Its own dialog, not a paragraph at the end of the summary: this one has to
# be acted on or the game stops launching entirely.
if [ "$REMOVE_ML" = "1" ]; then
    dialog "Clear the Steam launch options" "MelonLoader has been removed.

Steam is still set to start the game through melonloader-launch.sh, which no longer exists. Until you clear that setting the game will not start at all.

In Steam: select Melatonin, open Properties, go to the General tab, and empty the Launch Options box."
fi

if [ "$DIALOGS_WORK" = "0" ]; then
    hold
fi
exit 0
