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
        stage('Checkout Source') {
            steps {
                deleteDir()

                checkout([
                    $class: 'GitSCM',
                    branches: scm.branches,
                    userRemoteConfigs: scm.userRemoteConfigs,
                    doGenerateSubmoduleConfigurations: false,
                    extensions: [
                        [
                            $class: 'CloneOption',
                            shallow: true,
                            depth: 1,
                            noTags: true,
                            honorRefspec: true,
                            timeout: 30
                        ],
                        [
                            $class: 'CheckoutOption',
                            timeout: 30
                        ]
                    ]
                ])
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

                $indexPath = "./artifacts/deployment/wwwroot/blazor/index.html"
                $indexContent = Get-Content -LiteralPath $indexPath -Raw

                if (!$indexContent.Contains('<base href="/blazor/" />')) {
                    throw "Blazor base href is not /blazor/."
                }
                '''
            }
        }

        stage('Create Deployment Zip') {
            steps {
                bat '''
                if exist deploy-package.zip del /F /Q deploy-package.zip

                pushd artifacts\deployment
                jar -cMf ..\..\deploy-package.zip .
                if errorlevel 1 exit /b 1
                popd

                if not exist deploy-package.zip (
                    echo ERROR: Deployment ZIP was not created.
                    exit /b 1
                )
                '''
            }
        }

        stage('Verify Deployment Zip') {
            steps {
                bat '''
                jar -tf deploy-package.zip > zip-contents.txt
                if errorlevel 1 exit /b 1

                findstr /X /I /C:"Procfile" zip-contents.txt
                if errorlevel 1 (
                    echo ERROR: Procfile is missing from deployment ZIP.
                    exit /b 1
                )

                findstr /X /I /C:"HealthApp.API.dll" zip-contents.txt
                if errorlevel 1 (
                    echo ERROR: HealthApp.API.dll is missing from deployment ZIP.
                    exit /b 1
                )

                findstr /I /C:"wwwroot/index.html" zip-contents.txt
                if errorlevel 1 (
                    echo ERROR: Angular index.html is missing from deployment ZIP.
                    exit /b 1
                )

                findstr /I /C:"wwwroot/blazor/index.html" zip-contents.txt
                if errorlevel 1 (
                    echo ERROR: Blazor index.html is missing from deployment ZIP.
                    exit /b 1
                )

                findstr /I /C:"wwwroot/blazor/css/app.css" zip-contents.txt
                if errorlevel 1 (
                    echo ERROR: Blazor app.css is missing from deployment ZIP.
                    exit /b 1
                )

                findstr /I /C:"wwwroot/blazor/lib/bootstrap/dist/css/bootstrap.min.css" zip-contents.txt
                if errorlevel 1 (
                    echo ERROR: Bootstrap CSS is missing from deployment ZIP.
                    exit /b 1
                )

                findstr /I /C:"wwwroot/blazor/HealthApp.AdminBlazor.styles.css" zip-contents.txt
                if errorlevel 1 (
                    echo ERROR: Blazor scoped stylesheet is missing from deployment ZIP.
                    exit /b 1
                )

                findstr /I /C:"wwwroot/blazor/_framework/blazor.webassembly.js" zip-contents.txt
                if errorlevel 1 (
                    echo ERROR: Blazor startup JavaScript is missing from deployment ZIP.
                    exit /b 1
                )
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
