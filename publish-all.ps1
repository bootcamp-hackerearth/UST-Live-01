$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$solutionRoot = $PSScriptRoot

$angularProjectDirectory = Join-Path $solutionRoot "HealthApp.Angular"
$blazorProjectFile = Join-Path $solutionRoot "HealthApp.AdminBlazor\HealthApp.AdminBlazor.csproj"
$apiProjectFile = Join-Path $solutionRoot "HealthApp.API\HealthApp.API.csproj"
$apiWwwroot = Join-Path $solutionRoot "HealthApp.API\wwwroot"

$artifactsDirectory = Join-Path $solutionRoot "artifacts"
$angularOutputDirectory = Join-Path $artifactsDirectory "angular"
$blazorOutputDirectory = Join-Path $artifactsDirectory "adminblazor"
$deploymentDirectory = Join-Path $artifactsDirectory "deployment"
$blazorDestination = Join-Path $apiWwwroot "blazor"

function Assert-LastCommandSucceeded {
    param(
        [Parameter(Mandatory = $true)]
        [string]$ErrorMessage
    )

    if ($LASTEXITCODE -ne 0) {
        throw $ErrorMessage
    }
}

function Assert-PathExists {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path,

        [Parameter(Mandatory = $true)]
        [string]$ErrorMessage
    )

    if (-not (Test-Path -LiteralPath $Path)) {
        throw "$ErrorMessage Path: $Path"
    }
}

Write-Host "Validating project paths..." -ForegroundColor Cyan

Assert-PathExists -Path $angularProjectDirectory -ErrorMessage "Angular project directory was not found."
Assert-PathExists -Path $blazorProjectFile -ErrorMessage "Blazor project file was not found."
Assert-PathExists -Path $apiProjectFile -ErrorMessage "API project file was not found."

Write-Host "Cleaning previous artifacts..." -ForegroundColor Cyan

if (Test-Path -LiteralPath $artifactsDirectory) {
    Remove-Item -LiteralPath $artifactsDirectory -Recurse -Force
}

New-Item -ItemType Directory -Path $artifactsDirectory -Force | Out-Null
New-Item -ItemType Directory -Path $apiWwwroot -Force | Out-Null

Write-Host "Cleaning API wwwroot..." -ForegroundColor Cyan

Get-ChildItem -LiteralPath $apiWwwroot -Force -ErrorAction SilentlyContinue |
    Remove-Item -Recurse -Force

Write-Host "Restoring .NET projects..." -ForegroundColor Cyan

dotnet restore $apiProjectFile
Assert-LastCommandSucceeded -ErrorMessage "API restore failed."

dotnet restore $blazorProjectFile
Assert-LastCommandSucceeded -ErrorMessage "Blazor restore failed."

Write-Host "Building Angular for production..." -ForegroundColor Cyan

Push-Location $angularProjectDirectory

try {
    $packageLockFile = Join-Path $angularProjectDirectory "package-lock.json"

    if (Test-Path -LiteralPath $packageLockFile) {
        npm ci
    }
    else {
        npm install
    }

    Assert-LastCommandSucceeded -ErrorMessage "Angular package installation failed."

    npx ng build --configuration production --base-href / --output-path $angularOutputDirectory
    Assert-LastCommandSucceeded -ErrorMessage "Angular production build failed."
}
finally {
    Pop-Location
}

$angularBrowserDirectory = Join-Path $angularOutputDirectory "browser"

if (Test-Path -LiteralPath (Join-Path $angularBrowserDirectory "index.html")) {
    $angularFilesDirectory = $angularBrowserDirectory
}
elseif (Test-Path -LiteralPath (Join-Path $angularOutputDirectory "index.html")) {
    $angularFilesDirectory = $angularOutputDirectory
}
else {
    $angularIndexFile = Get-ChildItem `
        -LiteralPath $angularOutputDirectory `
        -Filter "index.html" `
        -File `
        -Recurse |
        Select-Object -First 1

    if ($null -eq $angularIndexFile) {
        throw "Angular index.html was not found under $angularOutputDirectory"
    }

    $angularFilesDirectory = $angularIndexFile.Directory.FullName
}

Write-Host "Copying Angular files into API wwwroot..." -ForegroundColor Cyan

Copy-Item `
    -Path (Join-Path $angularFilesDirectory "*") `
    -Destination $apiWwwroot `
    -Recurse `
    -Force

Assert-PathExists `
    -Path (Join-Path $apiWwwroot "index.html") `
    -ErrorMessage "Angular index.html was not copied into API wwwroot."

Write-Host "Publishing Blazor Admin..." -ForegroundColor Cyan

dotnet publish `
    $blazorProjectFile `
    --configuration Release `
    --output $blazorOutputDirectory `
    --no-restore

Assert-LastCommandSucceeded -ErrorMessage "Blazor Admin publish failed."

$publishedBlazorWwwroot = Join-Path $blazorOutputDirectory "wwwroot"
$publishedBlazorIndex = Join-Path $publishedBlazorWwwroot "index.html"
$publishedBlazorFramework = Join-Path $publishedBlazorWwwroot "_framework"

Assert-PathExists -Path $publishedBlazorIndex -ErrorMessage "Published Blazor index.html was not found."
Assert-PathExists -Path $publishedBlazorFramework -ErrorMessage "Published Blazor _framework directory was not found."

Write-Host "Configuring Blazor base path as /blazor/..." -ForegroundColor Cyan

$blazorIndexContent = Get-Content -LiteralPath $publishedBlazorIndex -Raw

if ($blazorIndexContent -notmatch '<base\s+href="[^"]*"\s*/?>') {
    throw "The Blazor index.html file does not contain a base href element."
}

$blazorIndexContent = $blazorIndexContent -replace `
    '<base\s+href="[^"]*"\s*/?>', `
    '<base href="/blazor/" />'

Set-Content `
    -LiteralPath $publishedBlazorIndex `
    -Value $blazorIndexContent `
    -Encoding utf8

New-Item -ItemType Directory -Path $blazorDestination -Force | Out-Null

Write-Host "Copying Blazor Admin into API wwwroot/blazor..." -ForegroundColor Cyan

Copy-Item `
    -Path (Join-Path $publishedBlazorWwwroot "*") `
    -Destination $blazorDestination `
    -Recurse `
    -Force

Assert-PathExists `
    -Path (Join-Path $blazorDestination "index.html") `
    -ErrorMessage "Blazor index.html was not copied into API wwwroot/blazor."

Assert-PathExists `
    -Path (Join-Path $blazorDestination "_framework") `
    -ErrorMessage "Blazor _framework was not copied into API wwwroot/blazor."

Write-Host "Publishing the combined API application..." -ForegroundColor Cyan

dotnet publish `
    $apiProjectFile `
    --configuration Release `
    --output $deploymentDirectory `
    --no-restore

Assert-LastCommandSucceeded -ErrorMessage "Combined API publish failed."

$deployedAngularIndex = Join-Path $deploymentDirectory "wwwroot\index.html"
$deployedBlazorIndex = Join-Path $deploymentDirectory "wwwroot\blazor\index.html"
$deployedBlazorFramework = Join-Path $deploymentDirectory "wwwroot\blazor\_framework"
$deployedApiDll = Join-Path $deploymentDirectory "HealthApp.API.dll"

Write-Host "Validating final deployment..." -ForegroundColor Cyan

Assert-PathExists -Path $deployedApiDll -ErrorMessage "HealthApp.API.dll is missing from the deployment."
Assert-PathExists -Path $deployedAngularIndex -ErrorMessage "Angular index.html is missing from the deployment."
Assert-PathExists -Path $deployedBlazorIndex -ErrorMessage "Blazor index.html is missing from the deployment."
Assert-PathExists -Path $deployedBlazorFramework -ErrorMessage "Blazor _framework is missing from the deployment."

Write-Host "" 
Write-Host "Combined HealthApp publish completed successfully." -ForegroundColor Green
Write-Host "Deployment folder: $deploymentDirectory" -ForegroundColor Yellow
Write-Host "Angular route: /" -ForegroundColor Cyan
Write-Host "Blazor Admin route: /blazor/" -ForegroundColor Cyan
Write-Host "API route: /api/" -ForegroundColor Cyan
Write-Host "" 
Write-Host "Run the published application with:" -ForegroundColor Green
Write-Host "dotnet `"$deployedApiDll`"" -ForegroundColor Yellow
