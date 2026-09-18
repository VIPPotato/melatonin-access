# Speech library (macOS)

`libprism.dylib` is [Prism](https://github.com/ethindp/prism), a unified
native abstraction over screen readers and TTS engines. On macOS it drives
VoiceOver, with AVSpeech as a fallback. Universal binary (x86_64 + arm64).

**Mozilla Public License 2.0**, redistributed unmodified. Prism in turn
incorporates components under their own licenses (simdutf, fmt, dr_wav,
concurrentqueue, the NVDA controller client, and others); the full texts live
in the Prism repository's `LICENSES/` directory.

## Why not Tolk here

Tolk is Windows-only, so `ScreenReader` uses it only on Windows. Prism is not
used on Windows in turn because it publishes no 32-bit build, and the Windows
build of Melatonin is a 32-bit process (its bundled `Tolk.dll` and
`nvdaControllerClient32.dll` are both i386, and a 64-bit process cannot load
a 32-bit DLL).

## Placement

`ScreenReader` locates the dylib at runtime by probing, in order:

    <Mods>/libprism.dylib
    <Mods>/lib/libprism.dylib
    <game folder>/libprism.dylib
    <game folder>/lib/libprism.dylib

then falling back to dyld's own search paths. Dropping it next to
`MelatoninAccess.dll` in `Mods/` is the simplest option.

If macOS quarantines the file (anything downloaded through a browser gets
`com.apple.quarantine`), `dlopen` refuses it and the mod runs silently with
only a log line explaining why. Clear it with:

    xattr -d com.apple.quarantine libprism.dylib

or allow it once from System Settings, Privacy & Security.
