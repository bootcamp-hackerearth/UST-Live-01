pipeline {
    agent any

    environment {
        AWS_REGION = 'ap-south-2'

        EB_APPLICATION_NAME = 'HealthCareApp'
        EB_ENVIRONMENT_NAME = 'HealthCareApp-dev'

        S3_BUCKET = 'healthaxis-jenkins-bucket-847814614822-ap-south-2-an'
        DEPLOY_PACKAGE = 'deploy-package.zip'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Show workspace files') {
            steps {
                bat 'dir /b'
            }
        }

        stage('Clean old deployment files') {
            steps {
                bat '''
                if exist artifacts rmdir /S /Q artifacts
                if exist publish rmdir /S /Q publish
                if exist deploy-package.zip del /Q deploy-package.zip

                if exist HealthCareApp\\wwwroot\\angular rmdir /S /Q HealthCareApp\\wwwroot\\angular
                if exist HealthCareApp\\wwwroot\\blazor rmdir /S /Q HealthCareApp\\wwwroot\\blazor
                '''
            }
        }

        stage('Restore .NET projects') {
            steps {
                bat 'dotnet restore HealthCareApp\\HealthCareApp.csproj'
                bat 'dotnet restore HealthCareApp.AdminBlazor\\HealthCareApp.AdminBlazor.csproj'
            }
        }

        stage('Install Angular packages') {
            steps {
                dir('HealthCareApp.UI') {
                    bat 'npm ci'
                }
            }
        }

        stage('Build Angular') {
            steps {
                dir('HealthCareApp.UI') {
                    bat 'npx ng build --configuration production'
                }
            }
        }

        stage('Publish Blazor Admin') {
            steps {
                bat 'dotnet publish HealthCareApp.AdminBlazor\\HealthCareApp.AdminBlazor.csproj -c Release -o artifacts\\adminblazor'
            }
        }

        stage('Copy Blazor Admin into API wwwroot') {
            steps {
                bat '''
                if exist HealthCareApp\\wwwroot\\blazor rmdir /S /Q HealthCareApp\\wwwroot\\blazor
                mkdir HealthCareApp\\wwwroot\\blazor
                xcopy /E /Y /I artifacts\\adminblazor\\wwwroot\\* HealthCareApp\\wwwroot\\blazor\\
                '''
            }
        }

        stage('Create Blazor fallback files') {
            steps {
                powershell '''
                $frameworkPath = ".\\HealthCareApp\\wwwroot\\blazor\\_framework"

                if (!(Test-Path $frameworkPath)) {
                    Write-Error "Blazor _framework folder was not found at $frameworkPath"
                    exit 1
                }

                $blazorJs = Get-ChildItem $frameworkPath -Filter "blazor.webassembly*.js" |
                    Where-Object { $_.Name -ne "blazor.webassembly.js" } |
                    Select-Object -First 1

                if ($blazorJs -ne $null) {
                    Copy-Item $blazorJs.FullName "$frameworkPath\\blazor.webassembly.js" -Force
                }

                $bootJson = Get-ChildItem $frameworkPath -Filter "blazor.boot*.json" |
                    Where-Object { $_.Name -ne "blazor.boot.json" } |
                    Select-Object -First 1

                if ($bootJson -ne $null) {
                    Copy-Item $bootJson.FullName "$frameworkPath\\blazor.boot.json" -Force
                }

                if (!(Test-Path "$frameworkPath\\blazor.webassembly.js")) {
                    Write-Error "blazor.webassembly.js was not found."
                    exit 1
                }
                '''
            }
        }

        stage('Publish API') {
            steps {
                bat 'dotnet publish HealthCareApp\\HealthCareApp.csproj -c Release -o publish --self-contained false'
            }
        }

        stage('Create Elastic Beanstalk Procfile') {
            steps {
                powershell '''
                $procfilePath = ".\\publish\\Procfile"
                Set-Content -Path $procfilePath -Value "web: dotnet HealthCareApp.dll" -NoNewline -Encoding ASCII

                if (!(Test-Path $procfilePath)) {
                    Write-Error "Procfile was not created."
                    exit 1
                }

                Write-Host "Procfile content:"
                Get-Content $procfilePath
                '''
            }
        }

        stage('Verify publish output') {
            steps {
                bat '''
                echo Checking publish folder...
                dir publish

                if not exist publish\\HealthCareApp.dll exit /b 1
                if not exist publish\\HealthCareApp.runtimeconfig.json exit /b 1
                if not exist publish\\Procfile exit /b 1
                if not exist publish\\wwwroot\\angular exit /b 1
                if not exist publish\\wwwroot\\blazor exit /b 1
                '''
            }
        }

        stage('Zip published output') {
            steps {
                dir('publish') {
                    bat 'powershell -NoProfile -Command "Compress-Archive -Path * -DestinationPath ..\\deploy-package.zip -Force"'
                }
            }
        }

        stage('Upload to S3 and Deploy to Elastic Beanstalk') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    bat 'aws s3 cp %DEPLOY_PACKAGE% s3://%S3_BUCKET%/deploy-package-%BUILD_NUMBER%.zip --region %AWS_REGION%'

                    bat 'aws elasticbeanstalk create-application-version --application-name %EB_APPLICATION_NAME% --version-label v-%BUILD_NUMBER% --source-bundle S3Bucket=%S3_BUCKET%,S3Key=deploy-package-%BUILD_NUMBER%.zip --region %AWS_REGION%'

                    bat 'aws elasticbeanstalk update-environment --environment-name %EB_ENVIRONMENT_NAME% --version-label v-%BUILD_NUMBER% --region %AWS_REGION%'
                }
            }
        }
    }

    post {
        success {
            echo 'HealthAxis deployment completed successfully.'
        }

        failure {
            echo 'HealthAxis deployment failed. Check Jenkins console output.'
        }
    }
}
