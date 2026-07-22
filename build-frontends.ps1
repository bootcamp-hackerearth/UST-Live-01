Write-Host "Building Angular into API wwwroot/angular..." -ForegroundColor Cyan

Set-Location ".\HealthCareApp.UI"
ng build --configuration production

Set-Location ".."

Write-Host "Publishing Blazor Admin..." -ForegroundColor Cyan

dotnet publish .\HealthCareApp.AdminBlazor\HealthCareApp.AdminBlazor.csproj -c Release -o .\artifacts\adminblazor

Write-Host "Copying Blazor Admin wwwroot into API wwwroot/blazor..." -ForegroundColor Cyan

if (Test-Path ".\HealthCareApp\wwwroot\blazor") {
    Remove-Item ".\HealthCareApp\wwwroot\blazor" -Recurse -Force
}

New-Item -ItemType Directory -Path ".\HealthCareApp\wwwroot\blazor" | Out-Null

Copy-Item ".\artifacts\adminblazor\wwwroot\*" ".\HealthCareApp\wwwroot\blazor\" -Recurse -Force

Write-Host "Creating Blazor framework fallback files..." -ForegroundColor Cyan

$frameworkPath = ".\HealthCareApp\wwwroot\blazor\_framework"

if (!(Test-Path $frameworkPath)) {
    Write-Host "ERROR: Blazor _framework folder was not found at $frameworkPath" -ForegroundColor Red
    exit 1
}

$blazorJs = Get-ChildItem $frameworkPath -Filter "blazor.webassembly*.js" |
    Where-Object { $_.Name -ne "blazor.webassembly.js" } |
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
    exit 1
}

Write-Host "Frontend build completed successfully." -ForegroundColor Green