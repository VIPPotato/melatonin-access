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
$GAME"
    hold; exit 0
fi

# --- choose what to remove --------------------------------------------------
#
# Real Cocoa checkboxes (NSButtonTypeSwitch), not an AppleScript list.
# VoiceOver announces a list row's selection state unreliably, but reads a
# checkbox as "checkbox, checked"/"unchecked". The Uninstall button is
# disabled whenever both are unchecked, and re-enabled as soon as one is.
choose_targets() {
    osascript -l JavaScript 2>/dev/null <<JS
ObjC.import('Cocoa');
function run() {
  var showMod = $MOD_PRESENT, showML = $ML_PRESENT;
  var okButton = null, cbMod = null, cbML = null;
  function anyChecked() {
    return (cbMod && cbMod.state === \$.NSControlStateValueOn) ||
           (cbML  && cbML.state  === \$.NSControlStateValueOn);
  }
  if (\$.UninstallHandler === undefined) {
    ObjC.registerSubclass({
      name: 'UninstallHandler',
      methods: {
        'toggled:': {
          types: ['void', ['id']],
          implementation: function (sender) { okButton.enabled = anyChecked(); }
        }
      }
    });
  }
  // osascript is a background process by default, so a raw NSAlert opens
  // behind everything and never takes focus - VoiceOver then never announces
  // it, which looks exactly like no window appearing at all. Becoming a
  // regular foreground app fixes that.
  var app = \$.NSApplication.sharedApplication;
  app.setActivationPolicy(\$.NSApplicationActivationPolicyRegular);
  app.activateIgnoringOtherApps(true);

  var handler = \$.UninstallHandler.alloc.init;
  var alert = \$.NSAlert.alloc.init;
  alert.messageText = "Uninstall Melatonin Access and / or MelonLoader";
  alert.informativeText = "Tick what you want removed, then choose Uninstall.";
  var rows = (showMod ? 1 : 0) + (showML ? 1 : 0);
  var view = \$.NSView.alloc.initWithFrame(\$.NSMakeRect(0, 0, 340, rows * 24));
  var y = (rows - 1) * 24;
  function mk(title) {
    var b = \$.NSButton.alloc.initWithFrame(\$.NSMakeRect(0, y, 340, 20));
    b.setButtonType(\$.NSButtonTypeSwitch);
    b.title = title;
    b.state = \$.NSControlStateValueOff;
    b.target = handler;
    b.action = 'toggled:';
    view.addSubview(b);
    y -= 24;
    return b;
  }
  if (showMod) cbMod = mk("Melatonin Access");
  if (showML)  cbML  = mk("MelonLoader");
  alert.accessoryView = view;
  alert.addButtonWithTitle("Uninstall");
  alert.addButtonWithTitle("Cancel");
  okButton = alert.buttons.objectAtIndex(0);
  okButton.enabled = false;
  alert.window.initialFirstResponder = (cbMod ? cbMod : cbML);
  alert.window.makeKeyAndOrderFront(null);
  var r = alert.runModal;
  if (r !== \$.NSAlertFirstButtonReturn) return "CANCEL";
  var out = [];
  if (cbMod && cbMod.state === \$.NSControlStateValueOn) out.push("MOD");
  if (cbML  && cbML.state  === \$.NSControlStateValueOn) out.push("ML");
  return out.length ? out.join(",") : "CANCEL";
}
JS
}

CHOICES="$(choose_targets)"
DIALOGS_WORK=1
[ -z "$CHOICES" ] && DIALOGS_WORK=0

if [ "$DIALOGS_WORK" = "0" ]; then
    # No GUI available: fall back to the terminal, and keep the window open.
    say "Could not show the uninstall dialog, so asking here instead."
    say ""
    [ "$MOD_PRESENT" = "1" ] && { printf 'Remove Melatonin Access? [y/N] '; read -r a; [ "${a:-}" = "y" ] && CHOICES="MOD"; }
    [ "$ML_PRESENT" = "1" ] && { printf 'Remove MelonLoader? [y/N] '; read -r a; [ "${a:-}" = "y" ] && CHOICES="${CHOICES:+$CHOICES,}ML"; }
    [ -z "$CHOICES" ] && CHOICES="CANCEL"
fi

if [ "$CHOICES" = "CANCEL" ]; then
    say "Cancelled. Nothing was removed."
    [ "$DIALOGS_WORK" = "0" ] && hold
    exit 0
fi

# Exact tokens only. Substring matching would let any unexpected output from
# osascript select something for deletion.
REMOVE_MOD=0; REMOVE_ML=0
OIFS="$IFS"; IFS=','
for tok in $CHOICES; do
    case "$tok" in
        MOD) REMOVE_MOD=1 ;;
        ML)  REMOVE_ML=1 ;;
        *)   say "Ignoring unrecognised selection: $tok" ;;
    esac
done
IFS="$OIFS"
if [ "$REMOVE_MOD" = "0" ] && [ "$REMOVE_ML" = "0" ]; then
    die "Nothing was selected, so nothing was removed."
fi

# --- remove -----------------------------------------------------------------

REMOVED=""; KEPT=""

if [ "$REMOVE_MOD" = "1" ]; then
    say "Removing Melatonin Access..."
    for p in "${MOD_PARTS[@]}"; do
        [ -e "$GAME/$p" ] || continue
        rm -rf "$GAME/$p" && { say "  removed $p"; REMOVED="$REMOVED
- $p"; }
    done
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
    [ -d "$GAME/UserData" ] && KEPT="$KEPT
- UserData/ (your settings)"
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
Left alone:$KEPT}$WARN"

[ "$DIALOGS_WORK" = "0" ] && hold
exit 0
