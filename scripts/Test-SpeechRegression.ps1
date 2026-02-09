param(
    [string]$LogPath = "D:\games\steam\steamapps\common\Melatonin\MelonLoader\Latest.log",
    [string[]]$RequiredPattern = @(),
    [string[]]$ForbiddenPattern = @(
        "\b1 stars\b",
        "upwards\.\.",
        "^Practice\. Press .+ to skip\.$"
    )
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $LogPath)) {
    Write-Host "ERROR: Log not found: $LogPath"
    exit 1
}

$rawLines = Get-Content -LiteralPath $LogPath
$speechLines = New-Object System.Collections.Generic.List[string]

foreach ($line in $rawLines) {
    if ($line -match "\[SR\]\s*(.+)$") {
        $speechLines.Add($Matches[1].Trim())
    }
}

if ($speechLines.Count -eq 0) {
    Write-Host "ERROR: No [SR] lines found in log. Enable debug mode (F12) before playtesting."
    exit 1
}

$errors = New-Object System.Collections.Generic.List[string]
$warnings = New-Object System.Collections.Generic.List[string]

for ($i = 1; $i -lt $speechLines.Count; $i++) {
    if ($speechLines[$i] -eq $speechLines[$i - 1]) {
        $errors.Add("Consecutive duplicate [SR] line at speech index $($i + 1): '$($speechLines[$i])'")
    }
}

foreach ($pattern in $ForbiddenPattern) {
    for ($i = 0; $i -lt $speechLines.Count; $i++) {
        if ($speechLines[$i] -match $pattern) {
            $errors.Add("Forbidden pattern '$pattern' matched at speech index $($i + 1): '$($speechLines[$i])'")
        }
    }
}

foreach ($pattern in $RequiredPattern) {
    $found = $false
    foreach ($speech in $speechLines) {
        if ($speech -match $pattern) {
            $found = $true
            break
        }
    }

    if (-not $found) {
        $errors.Add("Required pattern not found: '$pattern'")
    }
}

for ($i = 0; $i -lt ($speechLines.Count - 1); $i++) {
    if ($speechLines[$i] -match "menu\.$" -and $speechLines[$i + 1] -match "(1 of|1/|item 1)") {
        $warnings.Add("Possible split menu intro at speech indexes $($i + 1)-$($i + 2): '$($speechLines[$i])' + '$($speechLines[$i + 1])'")
    }
}

Write-Host "Speech lines checked: $($speechLines.Count)"
Write-Host "Errors: $($errors.Count)"
Write-Host "Warnings: $($warnings.Count)"

if ($warnings.Count -gt 0) {
    Write-Host ""
    Write-Host "Warnings:"
    foreach ($warning in $warnings) {
        Write-Host "- $warning"
    }
}

if ($errors.Count -gt 0) {
    Write-Host ""
    Write-Host "Errors:"
    foreach ($errorLine in $errors) {
        Write-Host "- $errorLine"
    }
    exit 1
}

Write-Host ""
Write-Host "PASS: No speech regressions matched."
exit 0
