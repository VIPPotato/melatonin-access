param(
    [string]$ManifestPath = ".\cutscene-ad\manifest.json",
    [switch]$StrictCoverage,
    [switch]$RequireEntries,
    [switch]$ValidateLocKeys,
    [string]$LocPath = ".\Loc.cs"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Get-Prop {
    param(
        [Parameter(Mandatory = $true)]$Object,
        [Parameter(Mandatory = $true)][string]$Name
    )

    if ($null -eq $Object) { return $null }
    if ($Object.PSObject.Properties.Name -contains $Name) { return ,$Object.$Name }
    return $null
}

function Add-Issue {
    param(
        [Parameter(Mandatory = $true)][object]$List,
        [Parameter(Mandatory = $true)][string]$Message
    )

    $List.Add($Message) | Out-Null
}

if (-not (Test-Path -LiteralPath $ManifestPath)) {
    Write-Host "ERROR: Manifest not found: $ManifestPath"
    exit 1
}

$errors = New-Object System.Collections.Generic.List[string]
$warnings = New-Object System.Collections.Generic.List[string]
$locKeys = New-Object System.Collections.Generic.HashSet[string]

$manifestDir = Split-Path -Parent (Resolve-Path -LiteralPath $ManifestPath)
$manifestRaw = Get-Content -LiteralPath $ManifestPath -Raw

try {
    $manifest = $manifestRaw | ConvertFrom-Json -Depth 20
}
catch {
    Write-Host "ERROR: Failed to parse manifest JSON: $($_.Exception.Message)"
    exit 1
}

$cutscenes = Get-Prop -Object $manifest -Name "cutscenes"
if ($null -eq $cutscenes) {
    Write-Host "ERROR: Manifest has no 'cutscenes' array."
    exit 1
}

$cutsceneList = @($cutscenes)
$seenIds = @{}

if ($ValidateLocKeys.IsPresent) {
    if (-not (Test-Path -LiteralPath $LocPath)) {
        Add-Issue -List $errors -Message "Loc.cs not found: $LocPath"
    }
    else {
        $locRaw = Get-Content -LiteralPath $LocPath -Raw
        $keyMatches = [regex]::Matches($locRaw, 'Add\(\s*"(?<key>[^"]+)"\s*,')
        foreach ($match in $keyMatches) {
            $locKeys.Add($match.Groups["key"].Value) | Out-Null
        }
    }
}

for ($i = 0; $i -lt $cutsceneList.Count; $i++) {
    $cutscene = $cutsceneList[$i]
    $id = [string](Get-Prop -Object $cutscene -Name "id")
    $sceneName = [string](Get-Prop -Object $cutscene -Name "sceneName")
    $cutsceneType = [string](Get-Prop -Object $cutscene -Name "cutsceneType")
    $scriptPathRel = [string](Get-Prop -Object $cutscene -Name "scriptPath")

    if ([string]::IsNullOrWhiteSpace($id)) {
        Add-Issue -List $errors -Message "Manifest cutscene index $i has empty id."
        continue
    }

    if ($seenIds.ContainsKey($id)) {
        Add-Issue -List $errors -Message "Duplicate cutscene id in manifest: '$id'."
        continue
    }
    $seenIds[$id] = $true

    if ([string]::IsNullOrWhiteSpace($scriptPathRel)) {
        Add-Issue -List $errors -Message "Cutscene '$id' has empty scriptPath."
        continue
    }

    $scriptPath = Join-Path $manifestDir $scriptPathRel
    if (-not (Test-Path -LiteralPath $scriptPath)) {
        $message = "Cutscene '$id' script file missing: $scriptPathRel"
        if ($StrictCoverage.IsPresent) {
            Add-Issue -List $errors -Message $message
        }
        else {
            Add-Issue -List $warnings -Message $message
        }
        continue
    }

    try {
        $script = (Get-Content -LiteralPath $scriptPath -Raw) | ConvertFrom-Json -Depth 20
    }
    catch {
        Add-Issue -List $errors -Message "Cutscene '$id' script JSON parse failed: $($_.Exception.Message)"
        continue
    }

    $scriptId = [string](Get-Prop -Object $script -Name "id")
    if ($scriptId -ne $id) {
        Add-Issue -List $errors -Message "Cutscene '$id' script id mismatch: '$scriptId'."
    }

    $scriptSceneName = [string](Get-Prop -Object $script -Name "sceneName")
    if (-not [string]::IsNullOrWhiteSpace($sceneName) -and $scriptSceneName -ne $sceneName) {
        Add-Issue -List $warnings -Message "Cutscene '$id' sceneName differs from manifest ('$scriptSceneName' vs '$sceneName')."
    }

    $scriptCutsceneType = [string](Get-Prop -Object $script -Name "cutsceneType")
    if (-not [string]::IsNullOrWhiteSpace($cutsceneType) -and $scriptCutsceneType -ne $cutsceneType) {
        Add-Issue -List $warnings -Message "Cutscene '$id' cutsceneType differs from manifest ('$scriptCutsceneType' vs '$cutsceneType')."
    }

    $entries = Get-Prop -Object $script -Name "entries"
    if ($null -eq $entries) {
        Add-Issue -List $errors -Message "Cutscene '$id' has no entries array."
        continue
    }

    $entryList = @($entries)
    if ($entryList.Count -eq 0) {
        if ($RequireEntries.IsPresent -or $StrictCoverage.IsPresent) {
            Add-Issue -List $errors -Message "Cutscene '$id' has zero entries."
        }
        continue
    }

    $previousAt = -1.0
    $previousEnd = -1.0

    for ($entryIndex = 0; $entryIndex -lt $entryList.Count; $entryIndex++) {
        $entry = $entryList[$entryIndex]

        $atValue = Get-Prop -Object $entry -Name "atSeconds"
        if ($null -eq $atValue) {
            Add-Issue -List $errors -Message "Cutscene '$id' entry $entryIndex missing atSeconds."
            continue
        }

        $atSeconds = 0.0
        if (-not [double]::TryParse($atValue.ToString(), [ref]$atSeconds)) {
            Add-Issue -List $errors -Message "Cutscene '$id' entry $entryIndex has invalid atSeconds: '$atValue'."
            continue
        }

        if ($atSeconds -lt 0) {
            Add-Issue -List $errors -Message "Cutscene '$id' entry $entryIndex has negative atSeconds."
        }

        $durationValue = Get-Prop -Object $entry -Name "durationSeconds"
        if ($null -eq $durationValue) {
            Add-Issue -List $errors -Message "Cutscene '$id' entry $entryIndex missing durationSeconds."
            continue
        }

        $durationSeconds = 0.0
        if (-not [double]::TryParse($durationValue.ToString(), [ref]$durationSeconds)) {
            Add-Issue -List $errors -Message "Cutscene '$id' entry $entryIndex has invalid durationSeconds: '$durationValue'."
            continue
        }

        if ($durationSeconds -le 0) {
            Add-Issue -List $errors -Message "Cutscene '$id' entry $entryIndex has non-positive durationSeconds."
        }

        $textKey = [string](Get-Prop -Object $entry -Name "textKey")
        if ([string]::IsNullOrWhiteSpace($textKey)) {
            Add-Issue -List $errors -Message "Cutscene '$id' entry $entryIndex has empty textKey."
        }
        elseif ($ValidateLocKeys.IsPresent -and $locKeys.Count -gt 0 -and -not $locKeys.Contains($textKey)) {
            $message = "Cutscene '$id' entry $entryIndex textKey not found in Loc.cs: '$textKey'."
            if ($StrictCoverage.IsPresent) {
                Add-Issue -List $errors -Message $message
            }
            else {
                Add-Issue -List $warnings -Message $message
            }
        }

        if ($previousAt -ge 0 -and $atSeconds -lt $previousAt) {
            Add-Issue -List $errors -Message "Cutscene '$id' entry $entryIndex is out of order (atSeconds must be ascending)."
        }

        if ($previousEnd -ge 0 -and $atSeconds -lt $previousEnd) {
            Add-Issue -List $errors -Message "Cutscene '$id' entry $entryIndex overlaps previous entry window."
        }

        $previousAt = $atSeconds
        $previousEnd = $atSeconds + $durationSeconds
    }
}

Write-Host "Cutscenes in manifest: $($cutsceneList.Count)"
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
Write-Host "PASS: Cutscene AD pipeline validation passed."
exit 0
