$ErrorActionPreference = "Stop"

# Run all paths relative to the location of this script.
$solutionRoot = $PSScriptRoot

$angularProject = Join-Path `
    $solutionRoot `
    "HealthAxis_AngularProj"

$blazorProject = Join-Path `
    $solutionRoot `
    "HealthAxisAdminLayout\HealthAxisAdminLayout.csproj"

$apiProject = Join-Path `
    $solutionRoot `
    "HealthAxisApi"

$artifactsDirectory = Join-Path `
    $solutionRoot `
    "artifacts"

$blazorPublishDirectory = Join-Path `
    $artifactsDirectory `
    "adminblazor"

$angularDestination = Join-Path `
    $apiProject `
    "wwwroot\angular"

$blazorDestination = Join-Path `
    $apiProject `
    "wwwroot\blazor"

function Assert-PathExists {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path,

        [Parameter(Mandatory = $true)]
        [string]$Description
    )

    if (-not (Test-Path -LiteralPath $Path)) {
        throw "$Description was not found: $Path"
    }
}

function Reset-Directory {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path
    )

    if (Test-Path -LiteralPath $Path) {
        Remove-Item `
            -LiteralPath $Path `
            -Recurse `
            -Force
    }

    New-Item `
        -ItemType Directory `
        -Path $Path `
        -Force |
        Out-Null
}

Write-Host ""
Write-Host "Validating project paths..." `
    -ForegroundColor Cyan

Assert-PathExists `
    -Path $angularProject `
    -Description "Angular project folder"

Assert-PathExists `
    -Path (Join-Path $angularProject "angular.json") `
    -Description "Angular angular.json"

Assert-PathExists `
    -Path (Join-Path $angularProject "package.json") `
    -Description "Angular package.json"

Assert-PathExists `
    -Path $blazorProject `
    -Description "Blazor project file"

Assert-PathExists `
    -Path $apiProject `
    -Description "API project folder"

New-Item `
    -ItemType Directory `
    -Path $artifactsDirectory `
    -Force |
    Out-Null

# ============================================================
# BUILD ANGULAR
# ============================================================

Write-Host ""
Write-Host "Building Angular for /angular/..." `
    -ForegroundColor Cyan

Push-Location $angularProject

try {
    if (Test-Path -LiteralPath ".\package-lock.json") {
        Write-Host "Running npm ci..." `
            -ForegroundColor DarkCyan

        npm ci
    }
    else {
        Write-Host "Running npm install..." `
            -ForegroundColor DarkCyan

        npm install
    }

    if ($LASTEXITCODE -ne 0) {
        throw "Angular package installation failed."
    }

    Write-Host "Running Angular production build..." `
        -ForegroundColor DarkCyan

    npx ng build `
        --configuration production `
        --base-href /angular/

    if ($LASTEXITCODE -ne 0) {
        throw "Angular production build failed."
    }
}
finally {
    Pop-Location
}

# ============================================================
# FIND ANGULAR OUTPUT
# ============================================================

Write-Host ""
Write-Host "Finding Angular build output..." `
    -ForegroundColor Cyan

$angularDistDirectory = Join-Path `
    $angularProject `
    "dist"

Assert-PathExists `
    -Path $angularDistDirectory `
    -Description "Angular dist folder"

$angularIndexFiles = @(
    Get-ChildItem `
        -Path $angularDistDirectory `
        -Recurse `
        -File `
        -Filter "index.html"
)

if ($angularIndexFiles.Count -eq 0) {
    throw "Angular index.html was not found inside the dist folder."
}

$angularIndexFile = $angularIndexFiles |
    Where-Object {
        $_.Directory.Name -eq "browser"
    } |
    Select-Object -First 1

if ($null -eq $angularIndexFile) {
    $angularIndexFile = $angularIndexFiles |
        Select-Object -First 1
}

$angularOutputDirectory =
    $angularIndexFile.Directory.FullName

Write-Host "Angular output found at:" `
    -ForegroundColor Green

Write-Host $angularOutputDirectory `
    -ForegroundColor Gray

$angularIndexContent = Get-Content `
    -LiteralPath $angularIndexFile.FullName `
    -Raw

if (
    $angularIndexContent -notmatch
    '/angular/'
) {
    throw "Angular index.html does not contain base href /angular/."
}

# ============================================================
# COPY ANGULAR INTO API WWWROOT
# ============================================================

Write-Host ""
Write-Host "Copying Angular into API wwwroot/angular..." `
    -ForegroundColor Cyan

Reset-Directory `
    -Path $angularDestination

Copy-Item `
    -Path (Join-Path $angularOutputDirectory "*") `
    -Destination $angularDestination `
    -Recurse `
    -Force

Assert-PathExists `
    -Path (Join-Path $angularDestination "index.html") `
    -Description "Copied Angular index.html"

Write-Host "Angular artifact copied successfully." `
    -ForegroundColor Green

# ============================================================
# PUBLISH BLAZOR WEBASSEMBLY
# ============================================================

Write-Host ""
Write-Host "Publishing Blazor Admin..." `
    -ForegroundColor Cyan

Reset-Directory `
    -Path $blazorPublishDirectory

dotnet publish `
    $blazorProject `
    -c Release `
    -o $blazorPublishDirectory

if ($LASTEXITCODE -ne 0) {
    throw "Blazor publish failed."
}

# IMPORTANT:
# Actual publish output is under wwwroot\blazor

$blazorPublishedWwwroot = Join-Path `
    $blazorPublishDirectory `
    "wwwroot\blazor"

$publishedBlazorIndex = Join-Path `
    $blazorPublishedWwwroot `
    "index.html"

$publishedBlazorFramework = Join-Path `
    $blazorPublishedWwwroot `
    "_framework"

$publishedBlazorSettings = Join-Path `
    $blazorPublishedWwwroot `
    "appsettings.json"

Assert-PathExists `
    -Path $blazorPublishedWwwroot `
    -Description "Published Blazor output folder"

Assert-PathExists `
    -Path $publishedBlazorIndex `
    -Description "Published Blazor index.html"

Assert-PathExists `
    -Path $publishedBlazorFramework `
    -Description "Published Blazor _framework folder"

Assert-PathExists `
    -Path $publishedBlazorSettings `
    -Description "Published Blazor appsettings.json"

$blazorIndexContent = Get-Content `
    -LiteralPath $publishedBlazorIndex `
    -Raw

if (
    $blazorIndexContent -notmatch
    '/blazor/'
) {
    throw "Blazor index.html does not contain base href /blazor/."
}

$blazorSettings = Get-Content `
    -LiteralPath $publishedBlazorSettings `
    -Raw |
    ConvertFrom-Json

if ($blazorSettings.ApiSettings.BaseUrl -ne "/api/") {
    throw "Blazor ApiSettings BaseUrl must be /api/."
}

if (
    $blazorSettings.NavigationSettings.AngularLoginUrl `
        -ne "/angular/login"
) {
    throw "Blazor AngularLoginUrl must be /angular/login."
}

# ============================================================
# COPY BLAZOR INTO API WWWROOT
# ============================================================

Write-Host ""
Write-Host "Copying Blazor into API wwwroot/blazor..." `
    -ForegroundColor Cyan

Reset-Directory `
    -Path $blazorDestination

Copy-Item `
    -Path (Join-Path $blazorPublishedWwwroot "*") `
    -Destination $blazorDestination `
    -Recurse `
    -Force

Assert-PathExists `
    -Path (Join-Path $blazorDestination "index.html") `
    -Description "Copied Blazor index.html"

Assert-PathExists `
    -Path (Join-Path $blazorDestination "_framework") `
    -Description "Copied Blazor _framework folder"

Assert-PathExists `
    -Path (Join-Path $blazorDestination "appsettings.json") `
    -Description "Copied Blazor appsettings.json"

Write-Host "Blazor artifact copied successfully." `
    -ForegroundColor Green

# ============================================================
# FINAL RESULT
# ============================================================

Write-Host ""
Write-Host "Frontend build completed successfully." `
    -ForegroundColor Green

Write-Host ""
Write-Host "Angular copied to:" `
    -ForegroundColor Cyan

Write-Host $angularDestination `
    -ForegroundColor Gray

Write-Host ""
Write-Host "Blazor copied to:" `
    -ForegroundColor Cyan

Write-Host $blazorDestination `
    -ForegroundColor Gray

Write-Host ""
Write-Host "The API now contains both frontend artifacts." `
    -ForegroundColor Green