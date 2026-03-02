param(
    [string]$LocalizationDir = ".\localization",
    [int]$ExpectedLanguageCount = 10
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $LocalizationDir -PathType Container)) {
    Write-Host "ERROR: Localization directory not found: $LocalizationDir"
    exit 1
}

$languageNames = @("en", "zh-Hans", "zh-Hant", "ja", "ko", "vi", "fr", "de", "es", "pt")
if ($ExpectedLanguageCount -ne $languageNames.Count) {
    Write-Host "ERROR: ExpectedLanguageCount must match configured language list length ($($languageNames.Count))."
    exit 1
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

$errors = New-Object System.Collections.Generic.List[string]
$warnings = New-Object System.Collections.Generic.List[string]
$translationsByLang = @{}

foreach ($lang in $languageNames) {
    $path = Join-Path $LocalizationDir ("loc.{0}.json" -f $lang)
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) {
        $errors.Add("Missing localization file: $path")
        continue
    }

    $parsed = $null
    try {
        $parsed = (Get-Content -LiteralPath $path -Raw) | ConvertFrom-Json -Depth 20
    }
    catch {
        $errors.Add("Failed to parse localization JSON '$path': $($_.Exception.Message)")
        continue
    }

    if ($null -eq $parsed) {
        $errors.Add("Parsed localization JSON is null: $path")
        continue
    }

    $entries = @($parsed.entries)
    if ($entries.Count -eq 0) {
        $errors.Add("Localization file has no entries: $path")
        $translationsByLang[$lang] = @{}
        continue
    }

    $map = @{}
    for ($i = 0; $i -lt $entries.Count; $i++) {
        $entry = $entries[$i]
        if ($null -eq $entry) {
            $warnings.Add("Null entry in '$path' at index $i.")
            continue
        }

        $key = [string]$entry.key
        $value = [string]$entry.value
        if ([string]::IsNullOrWhiteSpace($key)) {
            $errors.Add("Entry $i in '$path' has empty key.")
            continue
        }

        if ($map.ContainsKey($key)) {
            $errors.Add("Duplicate key '$key' in '$path'.")
            continue
        }

        if ([string]::IsNullOrWhiteSpace($value)) {
            $warnings.Add("Key '$key' has empty translation in '$path'.")
        }

        $map[$key] = $value
    }

    $translationsByLang[$lang] = $map
}

if (-not $translationsByLang.ContainsKey("en")) {
    $errors.Add("English localization map could not be loaded.")
}
else {
    $englishMap = $translationsByLang["en"]
    $englishKeys = @($englishMap.Keys)

    foreach ($lang in $languageNames) {
        if ($lang -eq "en") { continue }
        if (-not $translationsByLang.ContainsKey($lang)) { continue }

        $langMap = $translationsByLang[$lang]

        foreach ($key in $englishKeys) {
            if (-not $langMap.ContainsKey($key)) {
                $errors.Add("Missing key '$key' in language '$lang'.")
                continue
            }

            $referencePlaceholders = @(Get-PlaceholderIndexes -Text ([string]$englishMap[$key]))
            $langPlaceholders = @(Get-PlaceholderIndexes -Text ([string]$langMap[$key]))
            $placeholderDiff = @(Compare-Object -ReferenceObject $referencePlaceholders -DifferenceObject $langPlaceholders)
            $hasDifferentCount = $referencePlaceholders.Count -ne $langPlaceholders.Count
            $hasDifferentSet = $placeholderDiff.Count -gt 0
            if ($hasDifferentCount -or $hasDifferentSet) {
                $errors.Add(
                    ("Placeholder mismatch for key '{0}' in {1}. Expected [{2}] from en, got [{3}]." -f `
                        $key, `
                        $lang, `
                        ($referencePlaceholders -join ", "), `
                        ($langPlaceholders -join ", "))
                )
            }
        }

        foreach ($key in $langMap.Keys) {
            if (-not $englishMap.ContainsKey($key)) {
                $warnings.Add("Extra key '$key' exists in '$lang' but not in 'en'.")
            }
        }
    }
}

$parsedKeyCount = 0
if ($translationsByLang.ContainsKey("en")) {
    $parsedKeyCount = @($translationsByLang["en"].Keys).Count
}

Write-Host ("Localization keys parsed (en): {0}" -f $parsedKeyCount)
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
