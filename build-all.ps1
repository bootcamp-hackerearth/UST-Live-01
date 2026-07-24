Write-Host "Building Angular into API wwwroot/angular..." -ForegroundColor Cyan

Push-Location ".\HealthCareApp.UI"

npm install

ng build --configuration production

Pop-Location

Write-Host "Publishing Blazor Admin..." -ForegroundColor Cyan

dotnet publish ".\HealthCareApp.AdminBlazor\HealthCareApp.AdminBlazor.csproj" `
    -c Release `
    -o ".\artifacts\adminblazor"

Write-Host "Copying Blazor Admin wwwroot into API wwwroot/blazor..." -ForegroundColor Cyan

$blazorTargetPath = ".\HealthCareApp\wwwroot\blazor"
$adminWwwrootPath = ".\artifacts\adminblazor\wwwroot"

if (Test-Path $blazorTargetPath) {
    Remove-Item $blazorTargetPath -Recurse -Force
}

New-Item -ItemType Directory -Path $blazorTargetPath | Out-Null

Get-ChildItem $adminWwwrootPath | ForEach-Object {
    Copy-Item $_.FullName $blazorTargetPath -Recurse -Force
}

Write-Host "Creating Blazor framework fallback files..." -ForegroundColor Cyan

$frameworkPath = ".\HealthCareApp\wwwroot\blazor\_framework"

if (!(Test-Path $frameworkPath)) {
    Write-Host "ERROR: Blazor _framework folder was not found at $frameworkPath" -ForegroundColor Red
    exit 1
}

$blazorJs = Get-ChildItem $frameworkPath |
    Where-Object {
        $_.Name.StartsWith("blazor.webassembly") -and
        $_.Name.EndsWith(".js") -and
        $_.Name -ne "blazor.webassembly.js"
    } |
    Select-Object -First 1

if ($blazorJs -ne $null) {
    Copy-Item $blazorJs.FullName "$frameworkPath\blazor.webassembly.js" -Force
}

$bootJson = Get-ChildItem $frameworkPath |
    Where-Object {
        $_.Name.StartsWith("blazor.boot") -and
        $_.Name.EndsWith(".json") -and
        $_.Name -ne "blazor.boot.json"
    } |
    Select-Object -First 1

if ($bootJson -ne $null) {
    Copy-Item $bootJson.FullName "$frameworkPath\blazor.boot.json" -Force
}

if (!(Test-Path "$frameworkPath\blazor.webassembly.js")) {
    Write-Host "ERROR: blazor.webassembly.js was not found." -ForegroundColor Red
    exit 1
}

Write-Host "Publishing Web API..." -ForegroundColor Cyan

dotnet publish ".\HealthCareApp\HealthCareApp.csproj" `
    -c Release `
    -o ".\artifacts\webapi"

Write-Host "Full application build completed successfully." -ForegroundColor Green