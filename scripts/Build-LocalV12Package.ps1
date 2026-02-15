param(
    [string]$Configuration = "Debug",
    [switch]$SkipLocalizationQa,
    [switch]$SkipCutsceneQa
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot
$releaseDir = Join-Path $projectRoot "release"
$version = "v1.2"
$releaseScript = Join-Path $projectRoot "scripts\Build-ReleasePackage.ps1"

if (-not (Test-Path -LiteralPath $releaseDir)) {
    New-Item -ItemType Directory -Path $releaseDir -Force | Out-Null
}

# Keep local beta artifacts clean by replacing prior v1.2 test outputs.
Get-ChildItem -LiteralPath $releaseDir -File -Filter "MelatoninAccess-v1.2*.zip" -ErrorAction SilentlyContinue |
    Remove-Item -Force -ErrorAction Stop

Get-ChildItem -LiteralPath $releaseDir -Directory -ErrorAction SilentlyContinue |
    Where-Object { $_.Name -like "MelatoninAccess-v1.2*" } |
    Remove-Item -Recurse -Force -ErrorAction Stop

if (-not (Test-Path -LiteralPath $releaseScript)) {
    Write-Host "ERROR: Release packaging script not found: $releaseScript"
    exit 1
}

& $releaseScript `
    -Version $version `
    -Configuration $Configuration `
    -SkipLocalizationQa:$SkipLocalizationQa.IsPresent `
    -SkipCutsceneQa:$SkipCutsceneQa.IsPresent

if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

$zipPath = Join-Path $releaseDir "MelatoninAccess-$version.zip"
if (-not (Test-Path -LiteralPath $zipPath)) {
    Write-Host "ERROR: Expected package was not created: $zipPath"
    exit 1
}

Write-Host "Local tester package ready:"
Write-Host $zipPath
