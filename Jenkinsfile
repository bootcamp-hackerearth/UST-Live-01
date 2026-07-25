pipeline {
    agent any
 
    options {
        skipDefaultCheckout(true)
        timestamps()
        disableConcurrentBuilds()
    }
 
    environment {
        AWS_REGION = 'ap-south-1'
        EB_APPLICATION_NAME = 'HealthCareApp2'
        EB_ENVIRONMENT_NAME = 'HealthCareApp2-dev'
        S3_BUCKET = 'health-axis-bucket-412058343281-ap-south-1-an'
        DEPLOY_PACKAGE = 'deploy-package.zip'
    }
 
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
 
        stage('Clean Previous Build') {
            steps {
                bat '''
                if exist artifacts rmdir /S /Q artifacts
                if exist publish rmdir /S /Q publish
                if exist deploy-package.zip del /F /Q deploy-package.zip
 
                if exist HealthCareApp\\wwwroot\\angular rmdir /S /Q HealthCareApp\\wwwroot\\angular
                if exist HealthCareApp\\wwwroot\\blazor rmdir /S /Q HealthCareApp\\wwwroot\\blazor
                '''
            }
        }
 
        stage('Verify Project Files') {
            steps {
                bat '''
                if not exist HealthCareApp\\HealthCareApp.csproj (
                    echo ERROR: HealthCareApp project not found.
                    exit /b 1
                )
 
                if not exist HealthCareApp.AdminBlazor\\HealthCareApp.AdminBlazor.csproj (
                    echo ERROR: HealthCareApp.AdminBlazor project not found.
                    exit /b 1
                )
 
                if not exist HealthCareApp.UI\\package.json (
                    echo ERROR: HealthCareApp.UI package.json not found.
                    exit /b 1
                )
 
                echo Required project files found.
                '''
            }
        }
 
        stage('Restore .NET Projects') {
            steps {
                bat '''
                dotnet restore HealthCareApp\\HealthCareApp.csproj
                if errorlevel 1 exit /b 1
 
                dotnet restore HealthCareApp.AdminBlazor\\HealthCareApp.AdminBlazor.csproj
                if errorlevel 1 exit /b 1
                '''
            }
        }
 
        stage('Build Angular') {
            steps {
                dir('HealthCareApp.UI') {
                    bat '''
                    call npm ci
                    if errorlevel 1 exit /b 1
 
                    call npx ng build --configuration production
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }
 
        stage('Verify Angular Build') {
            steps {
                bat '''
                if not exist HealthCareApp\\wwwroot\\angular\\index.html (
                    echo ERROR: Angular output missing.
                    exit /b 1
                )
                '''
            }
        }
 
        stage('Publish Blazor Admin') {
            steps {
                bat '''
                dotnet publish HealthCareApp.AdminBlazor\\HealthCareApp.AdminBlazor.csproj -c Release -o artifacts\\adminblazor --no-restore
                if errorlevel 1 exit /b 1
                '''
            }
        }
 
        stage('Copy Blazor into API') {
            steps {
                bat '''
                if not exist artifacts\\adminblazor\\wwwroot\\_framework (
                    echo ERROR: Blazor framework files missing.
                    exit /b 1
                )
 
                if exist HealthCareApp\\wwwroot\\blazor rmdir /S /Q HealthCareApp\\wwwroot\\blazor
                mkdir HealthCareApp\\wwwroot\\blazor
 
                xcopy /E /Y /I artifacts\\adminblazor\\wwwroot\\* HealthCareApp\\wwwroot\\blazor\\
                if errorlevel 1 exit /b 1
 
                if not exist HealthCareApp\\wwwroot\\blazor\\index.html (
                    echo ERROR: Blazor index.html missing.
                    exit /b 1
                )
                '''
            }
        }
 
        stage('Create Blazor Fallback Files') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"
 
                $frameworkPath = ".\\HealthCareApp\\wwwroot\\blazor\\_framework"
 
                if (!(Test-Path $frameworkPath)) {
                    throw "Blazor framework folder missing."
                }
 
                $blazorJs = Get-ChildItem $frameworkPath -Filter "blazor.webassembly*.js" |
                    Where-Object { $_.Name -ne "blazor.webassembly.js" } |
                    Select-Object -First 1
 
                if ($null -ne $blazorJs) {
                    Copy-Item $blazorJs.FullName "$frameworkPath\\blazor.webassembly.js" -Force
                }
 
                $bootJson = Get-ChildItem $frameworkPath -Filter "blazor.boot*.json" |
                    Where-Object { $_.Name -ne "blazor.boot.json" } |
                    Select-Object -First 1
 
                if ($null -ne $bootJson) {
                    Copy-Item $bootJson.FullName "$frameworkPath\\blazor.boot.json" -Force
                }
                '''
            }
        }
 
        stage('Verify Frontend Files') {
            steps {
                bat '''
                if not exist HealthCareApp\\wwwroot\\angular\\index.html exit /b 1
                if not exist HealthCareApp\\wwwroot\\blazor\\index.html exit /b 1
                '''
            }
        }
 
        stage('Publish API') {
            steps {
                bat '''
                dotnet publish HealthCareApp\\HealthCareApp.csproj -c Release -o publish --no-restore --self-contained false
                if errorlevel 1 exit /b 1
                '''
            }
        }
 
        stage('Create Procfile') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"
 
                Set-Content -Path ".\\publish\\Procfile" -Value "web: dotnet HealthCareApp.dll" -Encoding ASCII
 
                if (!(Test-Path ".\\publish\\Procfile")) {
                    throw "Procfile missing."
                }
 
                if (!(Test-Path ".\\publish\\HealthCareApp.dll")) {
                    throw "HealthCareApp.dll missing."
                }
 
                if (!(Test-Path ".\\publish\\HealthCareApp.runtimeconfig.json")) {
                    throw "runtimeconfig missing."
                }
 
                if (!(Test-Path ".\\publish\\HealthCareApp.deps.json")) {
                    throw "deps json missing."
                }
 
                if (!(Test-Path ".\\publish\\wwwroot\\angular\\index.html")) {
                    throw "Angular missing from publish."
                }
 
                if (!(Test-Path ".\\publish\\wwwroot\\blazor\\index.html")) {
                    throw "Blazor missing from publish."
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
 
                jar -tf deploy-package.zip | findstr /I "Procfile"
                if errorlevel 1 exit /b 1
 
                jar -tf deploy-package.zip | findstr /I "HealthCareApp.dll"
                if errorlevel 1 exit /b 1
 
                jar -tf deploy-package.zip | findstr /I "HealthCareApp.runtimeconfig.json"
                if errorlevel 1 exit /b 1
 
                jar -tf deploy-package.zip | findstr /I "wwwroot/angular/index.html"
                if errorlevel 1 exit /b 1
 
                jar -tf deploy-package.zip | findstr /I "wwwroot/blazor/index.html"
                if errorlevel 1 exit /b 1
                '''
            }
        }
 
        stage('Upload Package to S3') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    bat '''
                    aws s3 cp deploy-package.zip s3://%S3_BUCKET%/deploy-package-%BUILD_NUMBER%.zip --region %AWS_REGION%
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }
 
        stage('Create Application Version') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    bat '''
                    aws elasticbeanstalk create-application-version --application-name "%EB_APPLICATION_NAME%" --version-label "v-%BUILD_NUMBER%" --description "Jenkins build %BUILD_NUMBER%" --source-bundle S3Bucket=%S3_BUCKET%,S3Key=deploy-package-%BUILD_NUMBER%.zip --region %AWS_REGION%
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }
 
        stage('Deploy to Elastic Beanstalk') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    bat '''
                    aws elasticbeanstalk update-environment --environment-name "%EB_ENVIRONMENT_NAME%" --version-label "v-%BUILD_NUMBER%" --region %AWS_REGION%
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }
 
        stage('Wait for EB Ready') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    powershell '''
                    $ErrorActionPreference = "Stop"
 
                    $expectedVersion = "v-$env:BUILD_NUMBER"
                    $maxChecks = 60
                    $delaySeconds = 15
 
                    for ($i = 1; $i -le $maxChecks; $i++) {
                        $json = aws elasticbeanstalk describe-environments --environment-names $env:EB_ENVIRONMENT_NAME --region $env:AWS_REGION --output json
                        $result = $json | ConvertFrom-Json
                        $envData = $result.Environments[0]
 
                        $status = $envData.Status
                        $health = $envData.Health
                        $version = $envData.VersionLabel
 
                        Write-Host "Check $i"
                        Write-Host "Status: $status"
                        Write-Host "Health: $health"
                        Write-Host "Version: $version"
 
                        if ($status -eq "Ready" -and $version -eq $expectedVersion) {
                            Write-Host "Elastic Beanstalk is running expected version."
                            exit 0
                        }
 
                        Start-Sleep -Seconds $delaySeconds
                    }
 
                    throw "Elastic Beanstalk did not reach Ready state with expected version."
                    '''
                }
            }
        }
    }
 
    post {
        success {
            echo 'HealthAxis Jenkins deployment completed.'
        }
 
        failure {
            echo 'HealthAxis Jenkins deployment failed. Check console output and EB logs.'
        }
 
        always {
            archiveArtifacts artifacts: 'deploy-package.zip', allowEmptyArchive: true
        }
    }
}