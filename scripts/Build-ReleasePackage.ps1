param(
    [string]$Version = "v1.0.1",
    [string]$Configuration = "Debug",
    [switch]$KeepStage
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot
$modDll = Join-Path $projectRoot "bin\$Configuration\net472\MelatoninAccess.dll"
$tolkDll = Join-Path $projectRoot "libs\x86\Tolk.dll"
$nvdaDll = Join-Path $projectRoot "libs\x86\nvdaControllerClient32.dll"

if (-not (Test-Path -LiteralPath $modDll)) {
    Write-Host "ERROR: Mod DLL not found: $modDll"
    Write-Host "Build first with: dotnet build MelatoninAccess.csproj"
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

$releaseDir = Join-Path $projectRoot "release"
$stageDir = Join-Path $releaseDir "MelatoninAccess-$Version"
$modsDir = Join-Path $stageDir "Mods"
$zipPath = Join-Path $releaseDir "MelatoninAccess-$Version.zip"

if (Test-Path -LiteralPath $stageDir) {
    Remove-Item -LiteralPath $stageDir -Recurse -Force
}

New-Item -ItemType Directory -Path $modsDir -Force | Out-Null

Copy-Item -LiteralPath $modDll -Destination (Join-Path $modsDir "MelatoninAccess.dll") -Force
Copy-Item -LiteralPath $tolkDll -Destination (Join-Path $stageDir "Tolk.dll") -Force
Copy-Item -LiteralPath $nvdaDll -Destination (Join-Path $stageDir "nvdaControllerClient32.dll") -Force

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
