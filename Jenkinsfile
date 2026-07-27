pipeline {
    agent any

    options {
        skipDefaultCheckout(true)
        timestamps()
        disableConcurrentBuilds()
    }

    environment {
        AWS_REGION = 'ap-south-1'
        EB_APPLICATION_NAME = 'HealthAppApi'
        EB_ENVIRONMENT_NAME = 'HealthAppApi-dev'
        S3_BUCKET = 'REPLACE_WITH_YOUR_AP_SOUTH_1_BUCKET_NAME'
        DEPLOY_PACKAGE = 'deploy-package.zip'
        VERSION_LABEL = "jenkins-${BUILD_NUMBER}"
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
                bat 'git log -1 --oneline'
                bat 'git status --short'
            }
        }

        stage('Clean Previous Build') {
            steps {
                bat '''
                if exist artifacts rmdir /S /Q artifacts
                if exist publish rmdir /S /Q publish
                if exist deploy-package.zip del /F /Q deploy-package.zip
                if exist HealthApp.Api\\wwwroot rmdir /S /Q HealthApp.Api\\wwwroot
                if exist HealthApp.Angular\\dist rmdir /S /Q HealthApp.Angular\\dist
                '''
            }
        }

        stage('Verify Tools and Projects') {
            steps {
                bat '''
                where dotnet
                if errorlevel 1 exit /b 1
                where node
                if errorlevel 1 exit /b 1
                where npm
                if errorlevel 1 exit /b 1
                where aws
                if errorlevel 1 exit /b 1
                where jar
                if errorlevel 1 exit /b 1

                if not exist HealthApp.Api\\HealthApp.Api.csproj exit /b 1
                if not exist HealthApp.AdminPortal\\HealthApp.AdminPortal.csproj exit /b 1
                if not exist HealthApp.Api.Tests\\HealthApp.Api.Tests.csproj exit /b 1
                if not exist HealthApp.Angular\\package.json exit /b 1

                echo All required tools and project files were found.
                '''
            }
        }

        stage('Restore .NET Projects') {
            steps {
                bat '''
                dotnet restore HealthApp.Api\\HealthApp.Api.csproj
                if errorlevel 1 exit /b 1
                dotnet restore HealthApp.AdminPortal\\HealthApp.AdminPortal.csproj
                if errorlevel 1 exit /b 1
                dotnet restore HealthApp.Api.Tests\\HealthApp.Api.Tests.csproj
                if errorlevel 1 exit /b 1
                '''
            }
        }

        stage('Build Angular') {
            steps {
                dir('HealthApp.Angular') {
                    bat '''
                    call npm ci
                    if errorlevel 1 exit /b 1
                    call npx ng build --configuration production --base-href /
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }

        stage('Copy Angular into API') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"
                $apiWwwRoot = Join-Path $env:WORKSPACE "HealthApp.Api\\wwwroot"
                $angularDist = Join-Path $env:WORKSPACE "HealthApp.Angular\\dist\\HealthApp.Angular"
                $angularBrowser = Join-Path $angularDist "browser"

                if (Test-Path (Join-Path $angularBrowser "index.html")) {
                    $angularSource = $angularBrowser
                }
                elseif (Test-Path (Join-Path $angularDist "index.html")) {
                    $angularSource = $angularDist
                }
                else {
                    throw "Angular index.html was not found under $angularDist"
                }

                New-Item -ItemType Directory -Path $apiWwwRoot -Force | Out-Null
                Copy-Item (Join-Path $angularSource "*") $apiWwwRoot -Recurse -Force

                if (!(Test-Path (Join-Path $apiWwwRoot "index.html"))) {
                    throw "Angular index.html was not copied into the API."
                }
                Write-Host "Angular copied from $angularSource"
                '''
            }
        }

        stage('Publish Blazor Admin') {
            steps {
                bat '''
                dotnet publish HealthApp.AdminPortal\\HealthApp.AdminPortal.csproj -c Release -o artifacts\\admin-publish --no-restore -p:StaticWebAssetBasePath=admin
                if errorlevel 1 exit /b 1
                '''
            }
        }

        stage('Copy Blazor Admin into API') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"
                $publishRoot = Join-Path $env:WORKSPACE "artifacts\\admin-publish\\wwwroot"
                $nestedAdmin = Join-Path $publishRoot "admin"
                $apiAdmin = Join-Path $env:WORKSPACE "HealthApp.Api\\wwwroot\\admin"

                if (Test-Path (Join-Path $nestedAdmin "index.html")) {
                    $blazorSource = $nestedAdmin
                }
                elseif (Test-Path (Join-Path $publishRoot "index.html")) {
                    $blazorSource = $publishRoot
                }
                else {
                    throw "Blazor index.html was not found under $publishRoot"
                }

                if (Test-Path $apiAdmin) {
                    Remove-Item $apiAdmin -Recurse -Force
                }
                New-Item -ItemType Directory -Path $apiAdmin -Force | Out-Null
                Copy-Item (Join-Path $blazorSource "*") $apiAdmin -Recurse -Force

                $adminIndex = Join-Path $apiAdmin "index.html"
                $framework = Join-Path $apiAdmin "_framework"
                if (!(Test-Path $adminIndex)) { throw "Blazor index.html is missing." }
                if (!(Test-Path $framework)) { throw "Blazor _framework directory is missing." }

                $indexContent = Get-Content $adminIndex -Raw
                if ($indexContent -notmatch '<base href="/admin/"') {
                    throw 'Blazor index.html must contain base href="/admin/".'
                }
                Write-Host "Blazor copied from $blazorSource"
                '''
            }
        }

        stage('Verify Combined Frontend') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"
                $required = @(
                    ".\\HealthApp.Api\\wwwroot\\index.html",
                    ".\\HealthApp.Api\\wwwroot\\admin\\index.html",
                    ".\\HealthApp.Api\\wwwroot\\admin\\_framework"
                )
                foreach ($path in $required) {
                    if (!(Test-Path $path)) { throw "Missing artifact: $path" }
                }
                '''
            }
        }

        stage('Build and Test') {
            steps {
                bat '''
                dotnet build HealthApp.Api\\HealthApp.Api.csproj -c Release --no-restore
                if errorlevel 1 exit /b 1
                dotnet test HealthApp.Api.Tests\\HealthApp.Api.Tests.csproj -c Release --no-restore
                if errorlevel 1 exit /b 1
                '''
            }
        }

        stage('Publish API') {
            steps {
                bat '''
                dotnet publish HealthApp.Api\\HealthApp.Api.csproj -c Release -o publish --no-restore --self-contained false
                if errorlevel 1 exit /b 1
                '''
            }
        }

        stage('Create and Verify Procfile') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"
                Set-Content -Path ".\\publish\\Procfile" -Value "web: dotnet HealthApp.Api.dll" -Encoding Ascii

                $required = @(
                    ".\\publish\\Procfile",
                    ".\\publish\\HealthApp.Api.dll",
                    ".\\publish\\HealthApp.Api.runtimeconfig.json",
                    ".\\publish\\HealthApp.Api.deps.json",
                    ".\\publish\\wwwroot\\index.html",
                    ".\\publish\\wwwroot\\admin\\index.html",
                    ".\\publish\\wwwroot\\admin\\_framework"
                )
                foreach ($path in $required) {
                    if (!(Test-Path $path)) { throw "Missing published artifact: $path" }
                }
                Get-Content ".\\publish\\Procfile"
                '''
            }
        }

        stage('Create Deployment Zip') {
            steps {
                dir('publish') {
                    bat '''
                    jar -cMf ..\\deploy-package.zip .
                    if errorlevel 1 exit /b 1
                    '''
                }
                bat '''
                if not exist deploy-package.zip exit /b 1
                jar -tf deploy-package.zip | findstr /I /C:"Procfile"
                if errorlevel 1 exit /b 1
                jar -tf deploy-package.zip | findstr /I /C:"HealthApp.Api.dll"
                if errorlevel 1 exit /b 1
                jar -tf deploy-package.zip | findstr /I /C:"wwwroot/index.html"
                if errorlevel 1 exit /b 1
                jar -tf deploy-package.zip | findstr /I /C:"wwwroot/admin/index.html"
                if errorlevel 1 exit /b 1
                '''
            }
        }

        stage('Upload Package to S3') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    bat '''
                    aws s3 cp "%DEPLOY_PACKAGE%" "s3://%S3_BUCKET%/healthapp/deploy-package-%BUILD_NUMBER%.zip" --region "%AWS_REGION%"
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }

        stage('Create Elastic Beanstalk Version') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    bat '''
                    aws elasticbeanstalk create-application-version ^
                    --application-name "%EB_APPLICATION_NAME%" ^
                    --version-label "%VERSION_LABEL%" ^
                    --description "Jenkins build %BUILD_NUMBER%" ^
                    --source-bundle S3Bucket=%S3_BUCKET%,S3Key=healthapp/deploy-package-%BUILD_NUMBER%.zip ^
                    --region "%AWS_REGION%"
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }

        stage('Deploy to Elastic Beanstalk') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    bat '''
                    aws elasticbeanstalk update-environment ^
                    --environment-name "%EB_ENVIRONMENT_NAME%" ^
                    --version-label "%VERSION_LABEL%" ^
                    --region "%AWS_REGION%"
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }

        stage('Wait for Elastic Beanstalk') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    powershell '''
                    $ErrorActionPreference = "Stop"
                    $maxChecks = 60
                    $delaySeconds = 15

                    for ($attempt = 1; $attempt -le $maxChecks; $attempt++) {
                        $json = aws elasticbeanstalk describe-environments `
                            --environment-names $env:EB_ENVIRONMENT_NAME `
                            --region $env:AWS_REGION `
                            --output json
                        if ($LASTEXITCODE -ne 0) { throw "Unable to query Elastic Beanstalk." }

                        $result = $json | ConvertFrom-Json
                        $environment = $result.Environments[0]
                        if ($null -eq $environment) { throw "Elastic Beanstalk environment was not found." }

                        Write-Host "Check $attempt - Status: $($environment.Status), Health: $($environment.Health), Version: $($environment.VersionLabel)"

                        if ($environment.Status -eq "Ready" -and $environment.VersionLabel -eq $env:VERSION_LABEL) {
                            if ($environment.Health -eq "Red") {
                                throw "Expected version deployed, but environment health is Red."
                            }
                            Write-Host "Elastic Beanstalk is running the expected version."
                            exit 0
                        }
                        Start-Sleep -Seconds $delaySeconds
                    }
                    throw "Elastic Beanstalk did not reach Ready state with the expected version."
                    '''
                }
            }
        }
    }

    post {
        success {
            echo 'HealthApp Jenkins deployment completed successfully.'
        }
        failure {
            echo 'HealthApp Jenkins deployment failed. Check Jenkins and Elastic Beanstalk logs.'
        }
        always {
            archiveArtifacts artifacts: 'deploy-package.zip', allowEmptyArchive: true, fingerprint: true
        }
    }
}