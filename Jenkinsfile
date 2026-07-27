pipeline {
    agent any

    options {
        skipDefaultCheckout(true)
        timestamps()
        disableConcurrentBuilds()
    }

    environment {
        AWS_REGION = 'ap-south-2'
        EB_APPLICATION_NAME = 'HealthAppAPI2'
        EB_ENVIRONMENT_NAME = 'HealthAppAPI2-dev'
        S3_BUCKET = 'jenkins-healthapp-251714435665-ap-south-2-an'
        DEPLOY_PACKAGE = 'deploy-package.zip'
    }

    stages {
        stage('Checkout Clean Workspace') {
            steps {
                deleteDir()
                checkout scm
            }
        }

        stage('Verify Sources') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"

                $required = @(
                    "./HealthApp.API/HealthApp.API.csproj",
                    "./HealthApp.AdminBlazor/HealthApp.AdminBlazor.csproj",
                    "./HealthApp.Angular/package.json",
                    "./publish-all.ps1"
                )

                foreach ($path in $required) {
                    if (!(Test-Path -LiteralPath $path)) {
                        throw "Required source file is missing: $path"
                    }
                }
                '''
            }
        }

        stage('Build Combined Application') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"
                & "./publish-all.ps1"
                if ($LASTEXITCODE -ne 0) {
                    exit $LASTEXITCODE
                }
                '''
            }
        }

        stage('Verify Combined Output') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"

                $required = @(
                    "./artifacts/deployment/HealthApp.API.dll",
                    "./artifacts/deployment/HealthApp.API.runtimeconfig.json",
                    "./artifacts/deployment/HealthApp.API.deps.json",
                    "./artifacts/deployment/Procfile",
                    "./artifacts/deployment/wwwroot/index.html",
                    "./artifacts/deployment/wwwroot/blazor/index.html",
                    "./artifacts/deployment/wwwroot/blazor/css/app.css",
                    "./artifacts/deployment/wwwroot/blazor/lib/bootstrap/dist/css/bootstrap.min.css",
                    "./artifacts/deployment/wwwroot/blazor/HealthApp.AdminBlazor.styles.css",
                    "./artifacts/deployment/wwwroot/blazor/_framework/blazor.webassembly.js"
                )

                foreach ($path in $required) {
                    if (!(Test-Path -LiteralPath $path)) {
                        throw "Required deployment file is missing: $path"
                    }
                }

                $index = Get-Content -LiteralPath "./artifacts/deployment/wwwroot/blazor/index.html" -Raw

                if (!$index.Contains('<base href="/blazor/" />')) {
                    throw "Blazor base href is not /blazor/."
                }
                '''
            }
        }

        stage('Create Deployment Zip') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"

                if (Test-Path -LiteralPath "./deploy-package.zip") {
                    Remove-Item -LiteralPath "./deploy-package.zip" -Force
                }

                Compress-Archive -Path "./artifacts/deployment/*" -DestinationPath "./deploy-package.zip" -Force

                if (!(Test-Path -LiteralPath "./deploy-package.zip")) {
                    throw "Deployment ZIP was not created."
                }
                '''
            }
        }

        stage('Verify Deployment Zip') {
            steps {
                bat '''
                jar -tf deploy-package.zip | findstr /X /I "Procfile"
                if errorlevel 1 exit /b 1

                jar -tf deploy-package.zip | findstr /X /I "HealthApp.API.dll"
                if errorlevel 1 exit /b 1

                jar -tf deploy-package.zip | findstr /I "wwwroot/index.html"
                if errorlevel 1 exit /b 1

                jar -tf deploy-package.zip | findstr /I "wwwroot/blazor/index.html"
                if errorlevel 1 exit /b 1

                jar -tf deploy-package.zip | findstr /I "wwwroot/blazor/_framework/blazor.webassembly.js"
                if errorlevel 1 exit /b 1
                '''
            }
        }

        stage('Upload Package and Create EB Version') {
            steps {
                withCredentials([[
                    $class: 'AmazonWebServicesCredentialsBinding',
                    credentialsId: 'aws-deploy-creds'
                ]]) {
                    bat '''
                    aws s3 cp %DEPLOY_PACKAGE% s3://%S3_BUCKET%/deploy-package-%BUILD_NUMBER%.zip --region %AWS_REGION%
                    if errorlevel 1 exit /b 1

                    aws elasticbeanstalk create-application-version --application-name "%EB_APPLICATION_NAME%" --version-label "v-%BUILD_NUMBER%" --description "Jenkins build %BUILD_NUMBER%" --source-bundle S3Bucket=%S3_BUCKET%,S3Key=deploy-package-%BUILD_NUMBER%.zip --region %AWS_REGION%
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }

        stage('Deploy to Elastic Beanstalk') {
            steps {
                withCredentials([[
                    $class: 'AmazonWebServicesCredentialsBinding',
                    credentialsId: 'aws-deploy-creds'
                ]]) {
                    bat '''
                    aws elasticbeanstalk update-environment --environment-name "%EB_ENVIRONMENT_NAME%" --version-label "v-%BUILD_NUMBER%" --region %AWS_REGION%
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }

        stage('Wait for Elastic Beanstalk') {
            steps {
                withCredentials([[
                    $class: 'AmazonWebServicesCredentialsBinding',
                    credentialsId: 'aws-deploy-creds'
                ]]) {
                    powershell '''
                    $ErrorActionPreference = "Stop"
                    $expectedVersion = "v-$env:BUILD_NUMBER"
                    $maximumChecks = 60

                    for ($check = 1; $check -le $maximumChecks; $check++) {
                        $json = aws elasticbeanstalk describe-environments --environment-names $env:EB_ENVIRONMENT_NAME --region $env:AWS_REGION --output json

                        if ($LASTEXITCODE -ne 0) {
                            throw "Unable to query Elastic Beanstalk."
                        }

                        $result = $json | ConvertFrom-Json
                        $environment = $result.Environments[0]

                        Write-Host "Check $check - Status=$($environment.Status), Health=$($environment.Health), Version=$($environment.VersionLabel)"

                        if (
                            $environment.Status -eq "Ready" -and
                            $environment.VersionLabel -eq $expectedVersion
                        ) {
                            Write-Host "Elastic Beanstalk is running $expectedVersion."
                            exit 0
                        }

                        Start-Sleep -Seconds 15
                    }

                    throw "Elastic Beanstalk did not reach Ready state with $expectedVersion."
                    '''
                }
            }
        }
    }

    post {
        success {
            echo 'HealthApp deployment completed successfully.'
        }

        failure {
            echo 'HealthApp deployment failed. Check Jenkins output and Elastic Beanstalk logs.'
        }

        always {
            archiveArtifacts artifacts: 'deploy-package.zip', allowEmptyArchive: true
        }
    }
}
