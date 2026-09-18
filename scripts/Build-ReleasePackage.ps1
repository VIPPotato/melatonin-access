param(
    [string]$Version = "v1.4",
    [string]$Configuration = "Debug",
    [switch]$KeepStage,
    [switch]$SkipLocalizationQa,
    [switch]$SkipCutsceneQa,
    [switch]$SkipMacOs
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot
$modDll = Join-Path $projectRoot "bin\$Configuration\net472\MelatoninAccess.dll"
$tolkDll = Join-Path $projectRoot "libs\x86\Tolk.dll"
$nvdaDll = Join-Path $projectRoot "libs\x86\nvdaControllerClient32.dll"
$cutsceneAdDir = Join-Path $projectRoot "cutscene-ad"
$cutsceneManifestPath = Join-Path $cutsceneAdDir "manifest.json"
$cutsceneScriptsDir = Join-Path $cutsceneAdDir "scripts"
$localizationDir = Join-Path $projectRoot "localization"
$loaderCfgCandidates = @(
    (Join-Path $projectRoot "UserData\Loader.cfg"),
    (Join-Path $projectRoot "UserConfig\Loader.cfg")
)
$loaderCfg = $loaderCfgCandidates | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1
$locQaScript = Join-Path $projectRoot "scripts\Test-LocalizationQA.ps1"
$cutsceneQaScript = Join-Path $projectRoot "scripts\Test-CutsceneAdPipeline.ps1"

if (-not (Test-Path -LiteralPath $modDll)) {
    Write-Host "ERROR: Mod DLL not found: $modDll"
    Write-Host "Build first with: pwsh -File .\\scripts\\Build-Mod.ps1"
    exit 1
}

if (-not (Test-Path -LiteralPath $tolkDll)) {
    Write-Host "ERROR: Dependency not found: $tolkDll"
    exit 1
}

if (-not (Test-Path -LiteralPath $nvdaDll)) {
    Write-Host "ERROR: Dependency not found: $nvdaDll"
    exit 1
}

if (-not (Test-Path -LiteralPath $cutsceneAdDir)) {
    Write-Host "ERROR: Cutscene AD folder not found: $cutsceneAdDir"
    exit 1
}

if (-not (Test-Path -LiteralPath $cutsceneManifestPath)) {
    Write-Host "ERROR: Cutscene AD manifest not found: $cutsceneManifestPath"
    exit 1
}

if (-not (Test-Path -LiteralPath $cutsceneScriptsDir)) {
    Write-Host "ERROR: Cutscene AD scripts folder not found: $cutsceneScriptsDir"
    exit 1
}

if (-not (Test-Path -LiteralPath $localizationDir)) {
    Write-Host "ERROR: Localization folder not found: $localizationDir"
    exit 1
}

if ([string]::IsNullOrWhiteSpace($loaderCfg)) {
    Write-Host "ERROR: Loader config not found. Checked:"
    foreach ($candidate in $loaderCfgCandidates) {
        Write-Host "- $candidate"
    }
    exit 1
}

if (-not $SkipLocalizationQa.IsPresent) {
    if (-not (Test-Path -LiteralPath $locQaScript)) {
        Write-Host "ERROR: Localization QA script not found: $locQaScript"
        exit 1
    }

    Write-Host "Running localization QA check..."
    & $locQaScript -LocalizationDir $localizationDir
    if ($LASTEXITCODE -ne 0) {
        Write-Host "ERROR: Localization QA check failed."
        exit $LASTEXITCODE
    }
}

if (-not $SkipCutsceneQa.IsPresent) {
    if (-not (Test-Path -LiteralPath $cutsceneQaScript)) {
        Write-Host "ERROR: Cutscene QA script not found: $cutsceneQaScript"
        exit 1
    }

    Write-Host "Running cutscene AD QA check..."
    & $cutsceneQaScript `
        -ManifestPath (Join-Path $projectRoot "cutscene-ad\manifest.json") `
        -StrictCoverage `
        -RequireEntries `
        -ValidateLocKeys `
        -LocalizationDir $localizationDir
    if ($LASTEXITCODE -ne 0) {
        Write-Host "ERROR: Cutscene AD QA check failed."
        exit $LASTEXITCODE
    }
}

$releaseDir = Join-Path $projectRoot "release"
$stageDir = Join-Path $releaseDir "MelatoninAccess-$Version"
$modsDir = Join-Path $stageDir "Mods"
$userDataDir = Join-Path $stageDir "UserData"
$zipPath = Join-Path $releaseDir "MelatoninAccess-$Version.zip"

if (Test-Path -LiteralPath $stageDir) {
    Remove-Item -LiteralPath $stageDir -Recurse -Force
}

New-Item -ItemType Directory -Path $modsDir -Force | Out-Null
New-Item -ItemType Directory -Path $userDataDir -Force | Out-Null
New-Item -ItemType Directory -Path (Join-Path $modsDir "cutscene-ad") -Force | Out-Null

Copy-Item -LiteralPath $modDll -Destination (Join-Path $modsDir "MelatoninAccess.dll") -Force
Copy-Item -LiteralPath $cutsceneManifestPath -Destination (Join-Path $modsDir "cutscene-ad\manifest.json") -Force
Copy-Item -LiteralPath $cutsceneScriptsDir -Destination (Join-Path $modsDir "cutscene-ad\scripts") -Recurse -Force
Copy-Item -LiteralPath $localizationDir -Destination (Join-Path $modsDir "localization") -Recurse -Force
Copy-Item -LiteralPath $tolkDll -Destination (Join-Path $stageDir "Tolk.dll") -Force
Copy-Item -LiteralPath $nvdaDll -Destination (Join-Path $stageDir "nvdaControllerClient32.dll") -Force
Copy-Item -LiteralPath $loaderCfg -Destination (Join-Path $userDataDir "Loader.cfg") -Force

if (Test-Path -LiteralPath $zipPath) {
    Remove-Item -LiteralPath $zipPath -Force
}

Compress-Archive -Path (Join-Path $stageDir "*") -DestinationPath $zipPath -Force

$stageKept = $KeepStage.IsPresent
if (-not $stageKept) {
    Remove-Item -LiteralPath $stageDir -Recurse -Force
}

$zip = Get-Item -LiteralPath $zipPath
Write-Host "Created release package:"
Write-Host $zip.FullName
Write-Host ("Size: {0} bytes" -f $zip.Length)
if ($stageKept) {
    Write-Host ("Stage directory kept: {0}" -f $stageDir)
}

$prismDll = Join-Path $projectRoot "libs\macos\libprism.dylib"
$installMacSh = Join-Path $projectRoot "scripts\install-macos.sh"
$uninstallMacSh = Join-Path $projectRoot "scripts\uninstall-macos.sh"

if (-not $SkipMacOs.IsPresent -and (Test-Path -LiteralPath $prismDll) -and (Test-Path -LiteralPath $installMacSh) -and (Test-Path -LiteralPath $uninstallMacSh)) {
    $stageMacDir = Join-Path $releaseDir "MelatoninAccess-$Version-macOS"
    $modsMacDir = Join-Path $stageMacDir "Mods"
    $userDataMacDir = Join-Path $stageMacDir "UserData"
    $zipMacPath = Join-Path $releaseDir "MelatoninAccess-$Version-macOS.zip"

    if (Test-Path -LiteralPath $stageMacDir) {
        Remove-Item -LiteralPath $stageMacDir -Recurse -Force
    }

    New-Item -ItemType Directory -Path $modsMacDir -Force | Out-Null
    New-Item -ItemType Directory -Path $userDataMacDir -Force | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $modsMacDir "cutscene-ad") -Force | Out-Null

    Copy-Item -LiteralPath $modDll -Destination (Join-Path $modsMacDir "MelatoninAccess.dll") -Force
    Copy-Item -LiteralPath $prismDll -Destination (Join-Path $modsMacDir "libprism.dylib") -Force
    Copy-Item -LiteralPath $cutsceneManifestPath -Destination (Join-Path $modsMacDir "cutscene-ad\manifest.json") -Force
    Copy-Item -LiteralPath $cutsceneScriptsDir -Destination (Join-Path $modsMacDir "cutscene-ad\scripts") -Recurse -Force
    Copy-Item -LiteralPath $localizationDir -Destination (Join-Path $modsMacDir "localization") -Recurse -Force
    Copy-Item -LiteralPath $loaderCfg -Destination (Join-Path $userDataMacDir "Loader.cfg") -Force

    $installContent = [System.IO.File]::ReadAllText($installMacSh).Replace("`r`n", "`n")
    [System.IO.File]::WriteAllText((Join-Path $stageMacDir "install-macOS.command"), $installContent, (New-Object System.Text.UTF8Encoding($false)))

    $uninstallContent = [System.IO.File]::ReadAllText($uninstallMacSh).Replace("`r`n", "`n")
    [System.IO.File]::WriteAllText((Join-Path $stageMacDir "uninstall-macOS.command"), $uninstallContent, (New-Object System.Text.UTF8Encoding($false)))

    $macReadme = @"
Melatonin Access - macOS
========================

Easiest way: open install-macOS.command
---------------------------------------

In Finder, select install-macOS.command and press Command-O.

It finds the game, installs the mod, and unblocks the files so macOS will
load them. Then follow the one remaining step it shows, which has to be done
in Steam itself. The launch options line appears in a text field you can read
and copy from, with a Copy To Clipboard button if you want it there.

If MelonLoader is not installed yet, it offers to open the download page for
you, and then lets you pick the downloaded ZIP from a file dialog.

If Finder refuses to run it, right-click it and choose Open, then confirm.

To remove the mod later, open uninstall-macOS.command. It asks about
Melatonin Access and MelonLoader in turn, and removes only what you say
Remove to. MelonLoader does not come with an uninstaller of its own.

If you extracted this archive into Downloads, Desktop or Documents, macOS may
block the installer from reading the mod files. It will notice and ask you to
choose the folder yourself, which is what grants it permission. Extracting to
your home folder instead avoids the whole thing.

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

   Write the path out in full: Steam does not expand ~ or `$HOME here, and a
   relative path fails with a generic launch error. Keep the quotes and the
   trailing %command%.

3. Launch from Steam. Speech goes through VoiceOver.

Not yet tested on macOS
-----------------------

The level editor. Everything else has been played through and works. The
editor loads, but its narration has not been exercised properly, so please
report anything that misbehaves there.

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
"@.Replace("`r`n", "`n")
    [System.IO.File]::WriteAllText((Join-Path $stageMacDir "README-macOS.txt"), $macReadme, (New-Object System.Text.UTF8Encoding($false)))

    if (Test-Path -LiteralPath $zipMacPath) {
        Remove-Item -LiteralPath $zipMacPath -Force
    }

    $pyAvailable = $false
    try {
        & python --version | Out-Null
        if ($LASTEXITCODE -eq 0) { $pyAvailable = $true }
    } catch { }

    if ($pyAvailable) {
        $pyScript = @"
import os, zipfile
stage_dir = r'$stageMacDir'
zip_path = r'$zipMacPath'
with zipfile.ZipFile(zip_path, 'w', zipfile.ZIP_DEFLATED) as zf:
    for root, dirs, files in os.walk(stage_dir):
        for file in sorted(files):
            full_path = os.path.join(root, file)
            rel_path = os.path.relpath(full_path, stage_dir).replace('\\', '/')
            zi = zipfile.ZipInfo(rel_path, date_time=(2026, 9, 18, 0, 0, 0))
            zi.compress_type = zipfile.ZIP_DEFLATED
            zi.create_system = 3
            if file.endswith('.command'):
                zi.external_attr = 0o100755 << 16
            else:
                zi.external_attr = 0o100644 << 16
            with open(full_path, 'rb') as f:
                zf.writestr(zi, f.read())
"@
        & python -c $pyScript
    } else {
        Compress-Archive -Path (Join-Path $stageMacDir "*") -DestinationPath $zipMacPath -Force
    }

    if (-not $stageKept) {
        Remove-Item -LiteralPath $stageMacDir -Recurse -Force
    }

    $macZip = Get-Item -LiteralPath $zipMacPath
    Write-Host "Created macOS release package:"
    Write-Host $macZip.FullName
    Write-Host ("Size: {0} bytes" -f $macZip.Length)
    if ($stageKept) {
        Write-Host ("Stage directory kept: {0}" -f $stageMacDir)
    }
}
