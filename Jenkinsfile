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

        stage('Clean Previous Build') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"

                $paths = @(
                    "./artifacts",
                    "./publish",
                    "./blazor-publish-temp",
                    "./deploy-package.zip",
                    "./HealthApp.API/wwwroot"
                )

                foreach ($path in $paths) {
                    if (Test-Path -LiteralPath $path) {
                        Remove-Item -LiteralPath $path -Recurse -Force
                    }
                }

                New-Item -ItemType Directory -Path "./HealthApp.API/wwwroot" -Force | Out-Null
                '''
            }
        }

        stage('Verify Project Files') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"

                $required = @(
                    "./HealthApp.API/HealthApp.API.csproj",
                    "./HealthApp.AdminBlazor/HealthApp.AdminBlazor.csproj",
                    "./HealthApp.Angular/package.json"
                )

                foreach ($path in $required) {
                    if (!(Test-Path -LiteralPath $path)) {
                        throw "Required project file is missing: $path"
                    }
                }
                '''
            }
        }

        stage('Restore .NET Projects') {
            steps {
                bat '''
                dotnet restore HealthApp.API/HealthApp.API.csproj
                if errorlevel 1 exit /b 1

                dotnet restore HealthApp.AdminBlazor/HealthApp.AdminBlazor.csproj
                if errorlevel 1 exit /b 1
                '''
            }
        }

        stage('Build Angular') {
            steps {
                dir('HealthApp.Angular') {
                    bat '''
                    call npm install
                    if errorlevel 1 exit /b 1

                    call npx ng build --configuration production --base-href / --output-path ../artifacts/angular
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }

        stage('Copy Angular into API') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"

                $angularOutput = "./artifacts/angular"
                $angularBrowserOutput = "./artifacts/angular/browser"
                $apiWwwroot = "./HealthApp.API/wwwroot"

                if (Test-Path -LiteralPath "$angularBrowserOutput/index.html") {
                    $angularFiles = $angularBrowserOutput
                }
                elseif (Test-Path -LiteralPath "$angularOutput/index.html") {
                    $angularFiles = $angularOutput
                }
                else {
                    $indexFile = Get-ChildItem -LiteralPath $angularOutput -Filter "index.html" -File -Recurse | Select-Object -First 1

                    if ($null -eq $indexFile) {
                        throw "Angular index.html was not found."
                    }

                    $angularFiles = $indexFile.Directory.FullName
                }

                Copy-Item -Path "$angularFiles/*" -Destination $apiWwwroot -Recurse -Force

                if (!(Test-Path -LiteralPath "$apiWwwroot/index.html")) {
                    throw "Angular index.html was not copied into API wwwroot."
                }
                '''
            }
        }

        stage('Publish Blazor') {
            steps {
                bat '''
                dotnet publish HealthApp.AdminBlazor/HealthApp.AdminBlazor.csproj -c Release -o blazor-publish-temp --no-restore
                if errorlevel 1 exit /b 1
                '''
            }
        }

        stage('Configure and Copy Blazor into API') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"

                $source = "./blazor-publish-temp/wwwroot"
                $indexPath = "$source/index.html"
                $destination = "./HealthApp.API/wwwroot/blazor"

                if (!(Test-Path -LiteralPath $indexPath)) {
                    throw "Published Blazor index.html is missing."
                }

                if (!(Test-Path -LiteralPath "$source/_framework")) {
                    throw "Published Blazor framework folder is missing."
                }

                $indexContent = Get-Content -LiteralPath $indexPath -Raw

                if ($indexContent.Contains('<base href="/" />')) {
                    $indexContent = $indexContent.Replace('<base href="/" />', '<base href="/blazor/" />')
                }
                elseif (!$indexContent.Contains('<base href="/blazor/" />')) {
                    throw "Blazor index.html does not contain the expected base href."
                }

                Set-Content -LiteralPath $indexPath -Value $indexContent -Encoding utf8

                $verifiedContent = Get-Content -LiteralPath $indexPath -Raw

                if (!$verifiedContent.Contains('<base href="/blazor/" />')) {
                    throw "Blazor base href was not changed to /blazor/."
                }

                New-Item -ItemType Directory -Path $destination -Force | Out-Null
                Copy-Item -Path "$source/*" -Destination $destination -Recurse -Force

                $required = @(
                    "$destination/index.html",
                    "$destination/css/app.css",
                    "$destination/lib/bootstrap/dist/css/bootstrap.min.css",
                    "$destination/HealthApp.AdminBlazor.styles.css",
                    "$destination/_framework/blazor.webassembly.js"
                )

                foreach ($path in $required) {
                    if (!(Test-Path -LiteralPath $path)) {
                        throw "Required Blazor file is missing: $path"
                    }
                }
                '''
            }
        }

        stage('Publish API') {
            steps {
                bat '''
                dotnet publish HealthApp.API/HealthApp.API.csproj -c Release -o publish --no-restore --self-contained false
                if errorlevel 1 exit /b 1
                '''
            }
        }

        stage('Create Procfile') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"

                Set-Content -LiteralPath "./publish/Procfile" -Value "web: dotnet HealthApp.API.dll" -Encoding ascii

                $required = @(
                    "./publish/Procfile",
                    "./publish/HealthApp.API.dll",
                    "./publish/HealthApp.API.runtimeconfig.json",
                    "./publish/HealthApp.API.deps.json",
                    "./publish/wwwroot/index.html",
                    "./publish/wwwroot/blazor/index.html",
                    "./publish/wwwroot/blazor/css/app.css",
                    "./publish/wwwroot/blazor/HealthApp.AdminBlazor.styles.css",
                    "./publish/wwwroot/blazor/_framework/blazor.webassembly.js"
                )

                foreach ($path in $required) {
                    if (!(Test-Path -LiteralPath $path)) {
                        throw "Required publish file is missing: $path"
                    }
                }

                $blazorIndex = Get-Content -LiteralPath "./publish/wwwroot/blazor/index.html" -Raw

                if (!$blazorIndex.Contains('<base href="/blazor/" />')) {
                    throw "Published Blazor base href is incorrect."
                }
                '''
            }
        }

        stage('Create Deployment Zip') {
            steps {
                dir('publish') {
                    bat '''
                    jar -cMf ../deploy-package.zip .
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }

        stage('Verify Deployment Zip') {
            steps {
                bat '''
                jar -tf deploy-package.zip > zip-contents.txt
                if errorlevel 1 exit /b 1

                findstr /X /I /C:"Procfile" zip-contents.txt
                if errorlevel 1 exit /b 1

                findstr /X /I /C:"HealthApp.API.dll" zip-contents.txt
                if errorlevel 1 exit /b 1

                findstr /I /C:"wwwroot/index.html" zip-contents.txt
                if errorlevel 1 exit /b 1

                findstr /I /C:"wwwroot/blazor/index.html" zip-contents.txt
                if errorlevel 1 exit /b 1

                findstr /I /C:"wwwroot/blazor/css/app.css" zip-contents.txt
                if errorlevel 1 exit /b 1

                findstr /I /C:"wwwroot/blazor/HealthApp.AdminBlazor.styles.css" zip-contents.txt
                if errorlevel 1 exit /b 1

                findstr /I /C:"wwwroot/blazor/_framework/blazor.webassembly.js" zip-contents.txt
                if errorlevel 1 exit /b 1
                '''
            }
        }

        stage('Upload Package to S3') {
            steps {
                withCredentials([[
                    $class: 'AmazonWebServicesCredentialsBinding',
                    credentialsId: 'aws-deploy-creds'
                ]]) {
                    bat '''
                    aws s3 cp deploy-package.zip s3://%S3_BUCKET%/deploy-package-%BUILD_NUMBER%.zip --region %AWS_REGION%
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }

        stage('Create Application Version') {
            steps {
                withCredentials([[
                    $class: 'AmazonWebServicesCredentialsBinding',
                    credentialsId: 'aws-deploy-creds'
                ]]) {
                    bat '''
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

        stage('Wait for EB Ready') {
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

                        if ($environment.Status -eq "Ready" -and $environment.VersionLabel -eq $expectedVersion) {
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
            echo 'HealthApp Jenkins deployment completed.'
        }

        failure {
            echo 'HealthApp Jenkins deployment failed. Check the failed stage and Elastic Beanstalk logs.'
        }

        always {
            archiveArtifacts artifacts: 'deploy-package.zip', allowEmptyArchive: true
        }
    }
}
