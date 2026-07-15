@echo off

SET SONAR_TOKEN=sqp_3139a89bb886f951e0ef5dd77df2e2d9881ef7c5
SET SONAR_URL=http://localhost:9000
SET PROJECT_KEY=Healthcare.netcore

echo Starting Sonar analysis...

dotnet sonarscanner begin ^
  /k:"%PROJECT_KEY%" ^
  /d:sonar.host.url="%SONAR_URL%" ^
  /d:sonar.login="%SONAR_TOKEN%" ^
  /d:sonar.exclusions="**/bin/**,**/obj/**,**/Migrations/**,**/*.css" ^
  /d:sonar.coverage.exclusions="**/bin/**,**/obj/**,**/Migrations/**,**/Middleware/**,**/Options/**,**/Controllers/**,**/Exceptions/**,**/Data/**,**/Consumers/**,**/DTOs/**,**/Mapping/**,**/Mappings/**,**/Models/**,**/Properties/**,**/Repository/**,**/HealthcareAxis.Shared/**,**/HealthAxis.UI/**,**/AdminWebApp/**,**/Program.cs,**/BackgroundServices/HeartbeatService.cs,**/BackgroundServices/NotificationCleanupService.cs,**/Mappings/MappingProfile.cs" ^
  /d:sonar.cs.opencover.reportsPaths="TestResults/**/coverage.opencover.xml"

IF %ERRORLEVEL% NEQ 0 (
  echo Sonar begin failed!
  exit /b %ERRORLEVEL%
)

echo Building project...

dotnet build

IF %ERRORLEVEL% NEQ 0 (
  echo Build failed!
  exit /b %ERRORLEVEL%
)

echo Testing project...

dotnet test --no-build --collect:"XPlat Code Coverage" --results-directory TestResults -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover

echo Ending Sonar analysis...

dotnet sonarscanner end ^
  /d:sonar.login="%SONAR_TOKEN%"

IF %ERRORLEVEL% NEQ 0 (
  echo Sonar end failed!
  exit /b %ERRORLEVEL%
)