@echo off

SET SONAR_TOKEN=sqp_b61782a03476aa03c3278b028523161716b1421e
SET SONAR_URL=http://localhost:9000
SET PROJECT_KEY=HealthCare-Sprint-3

echo Starting Sonar analysis...

dotnet sonarscanner begin ^
  /k:"%PROJECT_KEY%" ^
  /d:sonar.host.url="%SONAR_URL%" ^
  /d:sonar.login="%SONAR_TOKEN%" ^
  /d:sonar.exclusions="**/bin/**,**/obj/**,**/Migrations/**,**/wwwroot/**,**/*.css" ^
  /d:sonar.coverage.exclusions="**/bin/**,**/obj/**,**/Migrations/**,**/wwwroot/**,**/Middleware/**,**/Controllers/**,**/Exceptions/**,**/Data/**,**/DTOs/**,**/Mapping/**,**/Models/**,**/Properties/**,**/Repositories/**,**/HealthCare.Shared/**,**/HealthCare.UsersAngular/**,**/HealthCare.AdminBlazor/**,**/BackgroundServices/**,**/Program.cs" ^
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