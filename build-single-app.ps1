$ErrorActionPreference = "Stop"

Write-Host "Cleaning old frontend folders..." -ForegroundColor Cyan

$apiProject = ".\HealthApp.Api\HealthApp.Api.csproj"
$apiWwwroot = ".\HealthApp.Api\wwwroot"

$angularProject = ".\HealthApp.Angular"
$blazorProject = ".\HealthApp.Admin\HealthApp.Admin.csproj"

$artifacts = ".\artifacts"
$blazorPublish = "$artifacts\adminblazor"
$apiPublish = "$artifacts\singleapp"

if (!(Test-Path $apiProject)) {
    Write-Host "ERROR: API project not found at $apiProject" -ForegroundColor Red
    exit 1
}

if (!(Test-Path $angularProject)) {
    Write-Host "ERROR: Angular project folder not found at $angularProject" -ForegroundColor Red
    exit 1
}

if (!(Test-Path $blazorProject)) {
    Write-Host "ERROR: Blazor project not found at $blazorProject" -ForegroundColor Red
    exit 1
}

if (!(Test-Path $apiWwwroot)) {
    New-Item -ItemType Directory -Path $apiWwwroot | Out-Null
}

if (Test-Path "$apiWwwroot\angular") {
    Remove-Item "$apiWwwroot\angular" -Recurse -Force
}

if (Test-Path "$apiWwwroot\blazor") {
    Remove-Item "$apiWwwroot\blazor" -Recurse -Force
}

New-Item -ItemType Directory -Path "$apiWwwroot\angular" | Out-Null
New-Item -ItemType Directory -Path "$apiWwwroot\blazor" | Out-Null

if (!(Test-Path $artifacts)) {
    New-Item -ItemType Directory -Path $artifacts | Out-Null
}

Write-Host "Building Angular..." -ForegroundColor Cyan

Push-Location $angularProject

if (!(Test-Path ".\node_modules")) {
    Write-Host "Installing Angular dependencies..." -ForegroundColor Cyan
    npm install
}

ng build --configuration production --base-href /angular/

Pop-Location

Write-Host "Finding Angular build output..." -ForegroundColor Cyan

$angularIndex = Get-ChildItem "$angularProject\dist" -Recurse -Filter "index.html" |
    Sort-Object FullName |
    Select-Object -First 1

if ($null -eq $angularIndex) {
    Write-Host "ERROR: Angular index.html not found in dist folder." -ForegroundColor Red
    exit 1
}

$angularDist = $angularIndex.Directory.FullName

Write-Host "Copying Angular files from $angularDist to API wwwroot/angular..." -ForegroundColor Cyan

Copy-Item "$angularDist\*" "$apiWwwroot\angular\" -Recurse -Force

Write-Host "Publishing Blazor Admin WebAssembly..." -ForegroundColor Cyan

if (Test-Path $blazorPublish) {
    Remove-Item $blazorPublish -Recurse -Force
}

dotnet publish $blazorProject -c Release -o $blazorPublish

Write-Host "Finding Blazor published index.html..." -ForegroundColor Cyan

$blazorIndex = Get-ChildItem $blazorPublish -Recurse -Filter "index.html" |
    Sort-Object FullName |
    Select-Object -First 1

if ($null -eq $blazorIndex) {
    Write-Host "ERROR: Blazor index.html not found after publish." -ForegroundColor Red
    Get-ChildItem $blazorPublish -Recurse | Select-Object FullName
    exit 1
}

$blazorDist = $blazorIndex.Directory.FullName

Write-Host "Copying Blazor files from $blazorDist to API wwwroot/blazor..." -ForegroundColor Cyan

Copy-Item "$blazorDist\*" "$apiWwwroot\blazor\" -Recurse -Force

Write-Host "Patching Blazor index.html..." -ForegroundColor Cyan

$blazorIndexPath = "$apiWwwroot\blazor\index.html"

if (!(Test-Path $blazorIndexPath)) {
    Write-Host "ERROR: Blazor index.html not found at $blazorIndexPath" -ForegroundColor Red
    exit 1
}

$content = Get-Content $blazorIndexPath -Raw

# Safe string replacements - no regex
$content = $content.Replace('/', '/blazor/')
$content = $content.Replace('/blazor/', '/blazor/')

$content = $content.Replace(
    '_framework/blazor.webassembly#[.{fingerprint}].jsscript>',
    '_framework/blazor.webassembly.jsscript>'
)

Set-Content -Path $blazorIndexPath -Value $content -Encoding UTF8 -Force

$blazorIndexPath = "$apiWwwroot\blazor\index.html"

if (!(Test-Path $blazorIndexPath)) {
    Write-Host "ERROR: Blazor index.html not found at $blazorIndexPath" -ForegroundColor Red
    exit 1
}

$content = Get-Content $blazorIndexPath -Raw


$content = $content -replace '_framework/blazor\.webassembly[^</script>', '_framework/blazor.webassembly.jsscript>'

Set-Content -Path $blazorIndexPath -Value $content -Encoding UTF8 -Force

Write-Host "Creating Blazor framework fallback files..." -ForegroundColor Cyan

$frameworkPath = "$apiWwwroot\blazor\_framework"

if (!(Test-Path $frameworkPath)) {
    Write-Host "ERROR: Blazor _framework folder was not found at $frameworkPath" -ForegroundColor Red
    exit 1
}

$blazorJs = Get-ChildItem $frameworkPath -Filter "blazor.webassembly*.js" |
    Where-Object { $_.Name -ne "blazor.webassembly.js" -and $_.Name -notlike "*.map" } |
    Select-Object -First 1

if ($blazorJs -ne $null) {
    Copy-Item $blazorJs.FullName "$frameworkPath\blazor.webassembly.js" -Force
}

$bootJson = Get-ChildItem $frameworkPath -Filter "blazor.boot*.json" |
    Where-Object { $_.Name -ne "blazor.boot.json" } |
    Select-Object -First 1

if ($bootJson -ne $null) {
    Copy-Item $bootJson.FullName "$frameworkPath\blazor.boot.json" -Force
}

if (!(Test-Path "$frameworkPath\blazor.webassembly.js")) {
    Write-Host "ERROR: blazor.webassembly.js was not found." -ForegroundColor Red
    Get-ChildItem $frameworkPath | Select-Object Name
    exit 1
}

if (!(Test-Path "$frameworkPath\blazor.boot.json")) {
    Write-Host "ERROR: blazor.boot.json was not found." -ForegroundColor Red
    Get-ChildItem $frameworkPath | Select-Object Name
    exit 1
}

Write-Host "Publishing final single Web API application..." -ForegroundColor Cyan

if (Test-Path $apiPublish) {
    Remove-Item $apiPublish -Recurse -Force
}

dotnet publish $apiProject -c Release -o $apiPublish

Write-Host "Single application publish completed successfully." -ForegroundColor Green
Write-Host "Output folder: $apiPublish" -ForegroundColor Green