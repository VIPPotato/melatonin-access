param(
    [string]$LocPath = ".\Loc.cs",
    [int]$ExpectedLanguageCount = 10
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $LocPath)) {
    Write-Host "ERROR: Localization file not found: $LocPath"
    exit 1
}

$languageNames = @("en", "zh-Hans", "zh-Hant", "ja", "ko", "vi", "fr", "de", "es", "pt")
if ($ExpectedLanguageCount -ne $languageNames.Count) {
    Write-Host "ERROR: ExpectedLanguageCount must match configured language list length ($($languageNames.Count))."
    exit 1
}

function Unquote-CSharpString {
    param([Parameter(Mandatory = $true)][string]$Quoted)
    if ($Quoted.Length -lt 2) { return $Quoted }
    $inner = $Quoted.Substring(1, $Quoted.Length - 2)
    return [regex]::Unescape($inner)
}

function Get-PlaceholderIndexes {
    param([Parameter(Mandatory = $true)][string]$Text)
    $normalized = $Text -replace "\{\{", "" -replace "\}\}", ""
    $indexes = New-Object System.Collections.Generic.HashSet[string]
    foreach ($match in [regex]::Matches($normalized, "\{(\d+)(?:[^}]*)\}")) {
        [void]$indexes.Add($match.Groups[1].Value)
    }

    return [string[]]@($indexes | Sort-Object)
}

$raw = Get-Content -LiteralPath $LocPath -Raw
$addMatches = [regex]::Matches(
    $raw,
    "Add\s*\(\s*(?<args>.*?)\)\s*;",
    [System.Text.RegularExpressions.RegexOptions]::Singleline
)

if ($addMatches.Count -eq 0) {
    Write-Host "ERROR: No Add(...) localization entries were parsed from $LocPath"
    exit 1
}

$errors = New-Object System.Collections.Generic.List[string]
$warnings = New-Object System.Collections.Generic.List[string]

$entries = New-Object System.Collections.Generic.List[object]
$seen = @{}

for ($entryIndex = 0; $entryIndex -lt $addMatches.Count; $entryIndex++) {
    $argsText = $addMatches[$entryIndex].Groups["args"].Value
    $stringTokens = [regex]::Matches($argsText, '"(?:\\.|[^"\\])*"')
    if ($stringTokens.Count -eq 0) {
        continue
    }

    $values = New-Object System.Collections.Generic.List[string]
    foreach ($token in $stringTokens) {
        $values.Add((Unquote-CSharpString -Quoted $token.Value))
    }

    $expectedArgCount = $ExpectedLanguageCount + 1
    if ($values.Count -ne $expectedArgCount) {
        $errors.Add("Entry $($entryIndex + 1) has $($values.Count) string args, expected $expectedArgCount.")
        continue
    }

    $key = $values[0]
    if ($seen.ContainsKey($key)) {
        $errors.Add("Duplicate localization key '$key' found at entries $($seen[$key]) and $($entryIndex + 1).")
    }
    else {
        $seen[$key] = $entryIndex + 1
    }

    $translations = @()
    for ($i = 1; $i -lt $values.Count; $i++) {
        $translations += $values[$i]
    }

    for ($langIndex = 0; $langIndex -lt $translations.Count; $langIndex++) {
        if ([string]::IsNullOrWhiteSpace($translations[$langIndex])) {
            $warnings.Add("Key '$key' has empty translation for $($languageNames[$langIndex]).")
        }
    }

    $entries.Add([pscustomobject]@{
        Key          = $key
        Translations = $translations
    })
}

foreach ($entry in $entries) {
    $referencePlaceholders = @(Get-PlaceholderIndexes -Text $entry.Translations[0])
    for ($langIndex = 1; $langIndex -lt $entry.Translations.Count; $langIndex++) {
        $langPlaceholders = @(Get-PlaceholderIndexes -Text $entry.Translations[$langIndex])
        $placeholderDiff = @(Compare-Object -ReferenceObject $referencePlaceholders -DifferenceObject $langPlaceholders)
        $hasDifferentCount = $referencePlaceholders.Count -ne $langPlaceholders.Count
        $hasDifferentSet = $placeholderDiff.Count -gt 0
        if ($hasDifferentCount -or $hasDifferentSet) {
            $errors.Add(
                ("Placeholder mismatch for key '{0}' in {1}. Expected [{2}] from en, got [{3}]." -f `
                    $entry.Key, `
                    $languageNames[$langIndex], `
                    ($referencePlaceholders -join ", "), `
                    ($langPlaceholders -join ", "))
            )
        }
    }
}

Write-Host ("Localization keys parsed: {0}" -f $entries.Count)
Write-Host ("Errors: {0}" -f $errors.Count)
Write-Host ("Warnings: {0}" -f $warnings.Count)

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
    foreach ($errorItem in $errors) {
        Write-Host "- $errorItem"
    }
    exit 1
}

Write-Host ""
Write-Host "PASS: Localization QA checks passed."
exit 0
