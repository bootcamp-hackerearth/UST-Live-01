@echo off

SET SONAR_TOKEN=sqp_e8670218ff194727b266a5ea479c69999725c1f1
SET SONAR_URL=http://localhost:9000
SET PROJECT_KEY=HealthCare-APP

echo Starting Sonar analysis...

dotnet sonarscanner begin ^
  /k:"%PROJECT_KEY%" ^
  /d:sonar.host.url="%SONAR_URL%" ^
  /d:sonar.login="%SONAR_TOKEN%" ^
  /d:sonar.exclusions="**/bin/**,**/obj/**,**/Migrations/**" ^
  /d:sonar.coverage.exclusions="**/bin/**,**/obj/**,**/Migrations/**,**/Controllers/**,**/Data/**,**/DTOs/**,**/Mapping/**,**/Models/**,**/Properties/**,**/Repositories/**,**/Program.cs,**/Exceptions/**,**/Middleware/**" ^
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