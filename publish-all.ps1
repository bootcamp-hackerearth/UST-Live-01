$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$root = $PSScriptRoot
$angular = Join-Path $root "HealthApp.Angular"
$blazorProject = Join-Path $root "HealthApp.AdminBlazor\HealthApp.AdminBlazor.csproj"
$apiProject = Join-Path $root "HealthApp.API\HealthApp.API.csproj"
$apiWwwroot = Join-Path $root "HealthApp.API\wwwroot"
$artifacts = Join-Path $root "artifacts"
$angularOut = Join-Path $artifacts "angular"
$blazorOut = Join-Path $artifacts "adminblazor"
$deployOut = Join-Path $artifacts "deployment"

function Assert-Ok([string]$message) {
    if ($LASTEXITCODE -ne 0) { throw $message }
}
function Assert-Exists([string]$path) {
    if (!(Test-Path -LiteralPath $path)) { throw "Missing required path: $path" }
}

Assert-Exists $angular
Assert-Exists $blazorProject
Assert-Exists $apiProject

Write-Host "Cleaning prior output..." -ForegroundColor Cyan
if (Test-Path $artifacts) { Remove-Item $artifacts -Recurse -Force }
if (Test-Path $apiWwwroot) {
    Get-ChildItem $apiWwwroot -Force -ErrorAction SilentlyContinue | Remove-Item -Recurse -Force
}
New-Item -ItemType Directory -Path $artifacts -Force | Out-Null
New-Item -ItemType Directory -Path $apiWwwroot -Force | Out-Null

Write-Host "Restoring .NET projects..." -ForegroundColor Cyan
dotnet restore $apiProject
Assert-Ok "API restore failed."
dotnet restore $blazorProject
Assert-Ok "Blazor restore failed."

Write-Host "Building Angular..." -ForegroundColor Cyan
Push-Location $angular
try {
    if (Test-Path "package-lock.json") { npm ci } else { npm install }
    Assert-Ok "Angular dependency installation failed."
    npx ng build --configuration production --base-href / --output-path $angularOut
    Assert-Ok "Angular build failed."
}
finally { Pop-Location }

$angularBrowser = Join-Path $angularOut "browser"
if (Test-Path (Join-Path $angularBrowser "index.html")) {
    $angularFiles = $angularBrowser
}
elseif (Test-Path (Join-Path $angularOut "index.html")) {
    $angularFiles = $angularOut
}
else {
    $foundIndex = Get-ChildItem $angularOut -Filter "index.html" -File -Recurse | Select-Object -First 1
    if ($null -eq $foundIndex) { throw "Angular index.html was not found." }
    $angularFiles = $foundIndex.Directory.FullName
}
Copy-Item (Join-Path $angularFiles "*") $apiWwwroot -Recurse -Force
Assert-Exists (Join-Path $apiWwwroot "index.html")

Write-Host "Publishing Blazor..." -ForegroundColor Cyan
dotnet publish $blazorProject -c Release -o $blazorOut --no-restore
Assert-Ok "Blazor publish failed."

$blazorWwwroot = Join-Path $blazorOut "wwwroot"
$blazorIndex = Join-Path $blazorWwwroot "index.html"
Assert-Exists $blazorIndex
Assert-Exists (Join-Path $blazorWwwroot "_framework")

Write-Host "Changing Blazor base href to /blazor/..." -ForegroundColor Cyan
$html = Get-Content $blazorIndex -Raw
if ($html -notmatch '<base\s+href="[^"]*"\s*/?>') { throw "Blazor base href is missing." }
$html = $html -replace '<base\s+href="[^"]*"\s*/?>', '<base href="/blazor/" />'
Set-Content $blazorIndex $html -Encoding utf8
$html = Get-Content $blazorIndex -Raw
if ($html -notmatch '<base\s+href="/blazor/"\s*/?>') { throw "Blazor base href rewrite failed." }
if ($html -notmatch '_framework/blazor\.webassembly\.js') { throw "Blazor startup script is missing." }

$blazorDest = Join-Path $apiWwwroot "blazor"
New-Item -ItemType Directory -Path $blazorDest -Force | Out-Null
Copy-Item (Join-Path $blazorWwwroot "*") $blazorDest -Recurse -Force

$blazorRequired = @(
    "index.html",
    "css\app.css",
    "lib\bootstrap\dist\css\bootstrap.min.css",
    "HealthApp.AdminBlazor.styles.css",
    "_framework\blazor.webassembly.js"
)
foreach ($relative in $blazorRequired) { Assert-Exists (Join-Path $blazorDest $relative) }

Write-Host "Publishing combined API..." -ForegroundColor Cyan
dotnet publish $apiProject -c Release -o $deployOut --no-restore --self-contained false
Assert-Ok "API publish failed."
Set-Content (Join-Path $deployOut "Procfile") "web: dotnet HealthApp.API.dll" -Encoding ascii

$deployRequired = @(
    "HealthApp.API.dll",
    "HealthApp.API.runtimeconfig.json",
    "HealthApp.API.deps.json",
    "Procfile",
    "wwwroot\index.html",
    "wwwroot\blazor\index.html",
    "wwwroot\blazor\css\app.css",
    "wwwroot\blazor\lib\bootstrap\dist\css\bootstrap.min.css",
    "wwwroot\blazor\HealthApp.AdminBlazor.styles.css",
    "wwwroot\blazor\_framework\blazor.webassembly.js"
)
foreach ($relative in $deployRequired) { Assert-Exists (Join-Path $deployOut $relative) }

$finalIndex = Get-Content (Join-Path $deployOut "wwwroot\blazor\index.html") -Raw
if ($finalIndex -notmatch '<base\s+href="/blazor/"\s*/?>') { throw "Final Blazor base href is incorrect." }

Write-Host "Combined publish succeeded: $deployOut" -ForegroundColor Green
