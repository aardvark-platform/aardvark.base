param(
    [Parameter(Mandatory = $true)]
    [string[]]$CoverageReports
)

$ErrorActionPreference = "Stop"

function Normalize-RepoPath([string]$path) {
    return $path.Replace('\', '/')
}

function Normalize-CoveragePath([string]$path) {
    $normalized = $path.Replace('\', '/')
    if ($normalized.StartsWith('src/')) {
        return $normalized.Substring(4)
    }
    return $normalized
}

function Test-FullConditionCoverage([string]$conditionCoverage) {
    if ([string]::IsNullOrWhiteSpace($conditionCoverage)) { return $true }
    return $conditionCoverage.Trim().StartsWith('100%')
}

$implementationPattern = '^(src/Aardvark\.Base/.+_auto\.cs|src/Aardvark\.Geometry/PolyRegion2d\.fs)$'

$diffLines = git -c core.safecrlf=false -c core.autocrlf=false diff --ignore-space-at-eol --unified=0 --no-color HEAD -- src/Aardvark.Base src/Aardvark.Geometry

$changedLines = @{}
$currentFile = $null

foreach ($line in $diffLines) {
    if ($line -like '+++ b/*') {
        $candidate = $line.Substring(6)
        $candidate = Normalize-RepoPath $candidate
        if ($candidate -match $implementationPattern) {
            $currentFile = $candidate
            if (-not $changedLines.ContainsKey($currentFile)) {
                $changedLines[$currentFile] = New-Object 'System.Collections.Generic.HashSet[int]'
            }
        }
        else {
            $currentFile = $null
        }
        continue
    }

    if (-not $currentFile) { continue }

    if ($line -match '^@@ -\d+(?:,\d+)? \+(\d+)(?:,(\d+))? @@') {
        $start = [int]$matches[1]
        $count = if ($matches[2]) { [int]$matches[2] } else { 1 }
        if ($count -eq 0) { continue }

        for ($i = 0; $i -lt $count; $i++) {
            [void]$changedLines[$currentFile].Add($start + $i)
        }
    }
}

if ($changedLines.Count -eq 0) {
    Write-Host "No changed implementation files matched the coverage verifier."
    exit 0
}

$coverageData = @{}

foreach ($reportPath in $CoverageReports) {
    [xml]$report = Get-Content $reportPath

    foreach ($class in $report.coverage.packages.package.classes.class) {
        $file = Normalize-CoveragePath $class.filename
        if (-not $coverageData.ContainsKey($file)) {
            $coverageData[$file] = @{}
        }

        foreach ($lineNode in $class.lines.line) {
            $number = [int]$lineNode.number
            if (-not $coverageData[$file].ContainsKey($number)) {
                $coverageData[$file][$number] = @()
            }

            $coverageData[$file][$number] += [pscustomobject]@{
                Hits = [int]$lineNode.hits
                Branch = [bool]::Parse([string]$lineNode.branch)
                ConditionCoverage = [string]$lineNode.'condition-coverage'
            }
        }
    }
}

$errors = @()
$coveredExecutableLines = 0
$instrumentedChangedLines = 0

foreach ($repoFile in ($changedLines.Keys | Sort-Object)) {
    $coverageFile = Normalize-CoveragePath $repoFile

    if (-not $coverageData.ContainsKey($coverageFile)) {
        $errors += "No coverage data found for changed file '$repoFile'."
        continue
    }

    foreach ($line in ($changedLines[$repoFile] | Sort-Object)) {
        if (-not $coverageData[$coverageFile].ContainsKey($line)) {
            continue
        }

        $instrumentedChangedLines++
        $entries = $coverageData[$coverageFile][$line]
        $hitEntries = @($entries | Where-Object { $_.Hits -gt 0 })
        $hasHits = $hitEntries.Count -gt 0
        $partialBranchEntries = if ($hasHits) {
            @($hitEntries | Where-Object { $_.Branch -and -not (Test-FullConditionCoverage $_.ConditionCoverage) })
        }
        else {
            @()
        }
        $fullBranchCoverage = $partialBranchEntries.Count -eq 0

        if ($hasHits -and $fullBranchCoverage) {
            $coveredExecutableLines++
        }
        else {
            $details = $entries | ForEach-Object {
                if ($_.Branch) {
                    "hits=$($_.Hits), branch=$($_.ConditionCoverage)"
                }
                else {
                    "hits=$($_.Hits)"
                }
            }

            $errors += "Uncovered changed line: ${repoFile}:$line ($($details -join '; '))"
        }
    }
}

if ($instrumentedChangedLines -eq 0) {
    $errors += "No instrumented changed lines were found. Coverage input does not match the changed implementation files."
}

if ($errors.Count -gt 0) {
    $errors | ForEach-Object { Write-Error $_ }
    exit 1
}

Write-Host "Changed-line coverage verified: $coveredExecutableLines / $instrumentedChangedLines instrumented changed lines fully covered."
