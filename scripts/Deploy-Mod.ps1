[CmdletBinding()]
param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug",

    [string]$ProjectPath = (Join-Path (Split-Path -Parent $PSScriptRoot) "MelatoninAccess.csproj"),

    [string]$GamePath = "L:\SteamLibrary\steamapps\common\Melatonin",

    [string]$Framework = "net472",

    [string]$ModsPath
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Copy-DirectoryContents {
    param(
        [Parameter(Mandatory = $true)]
        [string]$SourcePath,

        [Parameter(Mandatory = $true)]
        [string]$DestinationPath,

        [Parameter(Mandatory = $true)]
        [string]$Label
    )

    if (-not (Test-Path -LiteralPath $SourcePath)) {
        Write-Host "Skipped $Label (not found): $SourcePath"
        return
    }

    $items = @(Get-ChildItem -LiteralPath $SourcePath -Force)
    if ($items.Count -eq 0) {
        Write-Host "Skipped $Label (empty): $SourcePath"
        return
    }

    New-Item -ItemType Directory -Path $DestinationPath -Force | Out-Null

    foreach ($item in $items) {
        Copy-Item -LiteralPath $item.FullName -Destination $DestinationPath -Recurse -Force
    }

    Write-Host "Copied $Label to $DestinationPath"
}

if (-not (Test-Path -LiteralPath $ProjectPath)) {
    throw "Project file not found: $ProjectPath"
}

if (-not (Test-Path -LiteralPath $GamePath)) {
    throw "Game path not found: $GamePath"
}

if ([string]::IsNullOrWhiteSpace($ModsPath)) {
    $ModsPath = Join-Path $GamePath "Mods"
}

$buildScriptPath = Join-Path $PSScriptRoot "Build-Mod.ps1"
if (-not (Test-Path -LiteralPath $buildScriptPath)) {
    throw "Build script not found: $buildScriptPath"
}

& $buildScriptPath -Configuration $Configuration -ProjectPath $ProjectPath -GamePath $GamePath -Framework $Framework
if ($LASTEXITCODE -ne 0) {
    throw "Build failed with exit code $LASTEXITCODE."
}

$resolvedProjectPath = (Resolve-Path -LiteralPath $ProjectPath).Path
$projectRoot = Split-Path -Parent $resolvedProjectPath
$outputDllPath = Join-Path $projectRoot "bin\$Configuration\$Framework\MelatoninAccess.dll"

if (-not (Test-Path -LiteralPath $outputDllPath)) {
    throw "Built DLL not found: $outputDllPath"
}

New-Item -ItemType Directory -Path $ModsPath -Force | Out-Null

Copy-Item -LiteralPath $outputDllPath -Destination $ModsPath -Force
Write-Host "Copied DLL to $ModsPath"

Copy-DirectoryContents -SourcePath (Join-Path $projectRoot "localization") -DestinationPath (Join-Path $ModsPath "localization") -Label "localization files"
Copy-DirectoryContents -SourcePath (Join-Path $projectRoot "cutscene-ad") -DestinationPath (Join-Path $ModsPath "cutscene-ad") -Label "cutscene AD files"

Write-Host "Deploy complete."
