[CmdletBinding()]
param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug",

    [string]$ProjectPath = (Join-Path (Split-Path -Parent $PSScriptRoot) "MelatoninAccess.csproj"),

    [string]$GamePath = "L:\SteamLibrary\steamapps\common\Melatonin",

    [string]$Framework = "net472"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $ProjectPath)) {
    throw "Project file not found: $ProjectPath"
}

if (-not (Test-Path -LiteralPath $GamePath)) {
    throw "Game path not found: $GamePath"
}

$melonLoaderPath = Join-Path $GamePath "MelonLoader"
$gameManagedPath = Join-Path $GamePath "Melatonin_Data\Managed"

if (-not (Test-Path -LiteralPath $melonLoaderPath)) {
    throw "MelonLoader path not found: $melonLoaderPath"
}

if (-not (Test-Path -LiteralPath (Join-Path $gameManagedPath "Assembly-CSharp.dll"))) {
    throw "Managed game DLLs not found: $gameManagedPath"
}

$resolvedProjectPath = (Resolve-Path -LiteralPath $ProjectPath).Path
$projectRoot = Split-Path -Parent $resolvedProjectPath
$assemblyName = "MelatoninAccess"
$outputDllPath = Join-Path $projectRoot "bin\$Configuration\$Framework\$assemblyName.dll"

Write-Host "Building $assemblyName ($Configuration)..."

$buildArgs = @(
    "build",
    $resolvedProjectPath,
    "-c", $Configuration,
    "-nologo",
    "-v", "minimal",
    "-p:ModsPath=",
    "-p:MelonLoaderPath=$melonLoaderPath",
    "-p:GameManagedPath=$gameManagedPath"
)

& dotnet @buildArgs
if ($LASTEXITCODE -ne 0) {
    throw "Build failed with exit code $LASTEXITCODE."
}

if (-not (Test-Path -LiteralPath $outputDllPath)) {
    throw "Build succeeded but the output DLL was not found: $outputDllPath"
}

Write-Host "Build succeeded."
Write-Host "Output DLL: $outputDllPath"
