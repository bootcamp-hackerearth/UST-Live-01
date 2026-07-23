pipeline {
    agent any

    options {
        // Jenkins already performs checkout automatically unless this is set.
        // We use our own Checkout stage below.
        skipDefaultCheckout(true)

        timestamps()
        disableConcurrentBuilds()
    }

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_NOLOGO = '1'

        // AWS region
        AWS_REGION = 'ap-south-2'

        // Replace these with the exact values from Elastic Beanstalk
        EB_APPLICATION_NAME = 'HealthAxisAPI'
        EB_ENVIRONMENT_NAME = 'HealthAxisAPI-dev'

        // Replace this with the exact S3 bucket name
        S3_BUCKET = 'jenkins-bucket-379992420468-ap-south-2-an'
    }

    stages {

        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Show Project Files') {
            steps {
                bat '''
                    echo ===== Available .NET projects =====
                    dir /S /B *.csproj

                    echo.
                    echo ===== Angular project =====
                    if exist HealthAxis.UI\\package.json (
                        echo HealthAxis.UI project found.
                    ) else (
                        echo ERROR: HealthAxis.UI\\package.json was not found.
                        exit /b 1
                    )
                '''
            }
        }

        stage('Clean Previous Build') {
            steps {
                bat '''
                    if exist publish (
                        rmdir /S /Q publish
                    )

                    if exist blazor-publish-temp (
                        rmdir /S /Q blazor-publish-temp
                    )

                    if exist deploy-package.zip (
                        del /F /Q deploy-package.zip
                    )

                    if exist HealthAxis.UI\\dist (
                        rmdir /S /Q HealthAxis.UI\\dist
                    )

                    if exist HealthAxis.API\\wwwroot (
                        rmdir /S /Q HealthAxis.API\\wwwroot
                    )

                    mkdir HealthAxis.API\\wwwroot
                '''
            }
        }

        stage('Restore .NET Projects') {
            steps {
                bat '''
                    dotnet restore HealthAxis.API\\HealthAxis.API.csproj

                    dotnet restore HealthAxis_Admin\\HealthAxis_Admin.csproj
                '''
            }
        }

        stage('Build Angular') {
            steps {
                dir('HealthAxis.UI') {
                    bat '''
                        call npm ci

                        call npm run build -- --configuration production
                    '''
                }
            }
        }

        stage('Copy Angular into API wwwroot') {
            steps {
                bat '''
                    powershell -NoProfile -ExecutionPolicy Bypass -Command "$indexFile = Get-ChildItem -Path 'HealthAxis.UI\\dist' -Recurse -Filter 'index.html' | Select-Object -First 1; if ($null -eq $indexFile) { throw 'Angular build failed: index.html was not found inside HealthAxis.UI\\dist.' }; $sourceFolder = $indexFile.Directory.FullName; Write-Host ('Angular build source: ' + $sourceFolder); Copy-Item -Path (Join-Path $sourceFolder '*') -Destination 'HealthAxis.API\\wwwroot' -Recurse -Force"
                '''
            }
        }

        stage('Publish Blazor Admin') {
            steps {
                bat '''
                    dotnet publish HealthAxis_Admin\\HealthAxis_Admin.csproj ^
                    -c Release ^
                    -o blazor-publish-temp ^
                    --no-restore
                '''
            }
        }

        stage('Copy Blazor into API wwwroot') {
            steps {
                bat '''
                    if exist blazor-publish-temp\\wwwroot\\_framework (
                        echo Blazor WebAssembly project detected.

                        if not exist HealthAxis.API\\wwwroot\\admin (
                            mkdir HealthAxis.API\\wwwroot\\admin
                        )

                        xcopy /E /Y /I ^
                        blazor-publish-temp\\wwwroot\\* ^
                        HealthAxis.API\\wwwroot\\admin\\
                    ) else (
                        echo Blazor Server project detected.
                        echo It cannot run by copying only wwwroot into the API.
                        echo Admin was built successfully, but it will need a separate deployment.
                    )
                '''
            }
        }

        stage('Publish API') {
            steps {
                bat '''
                    dotnet publish HealthAxis.API\\HealthAxis.API.csproj ^
                    -c Release ^
                    -o publish ^
                    --no-restore
                '''
            }
        }

        stage('Verify Published Output') {
            steps {
                bat '''
                    if not exist publish\\HealthAxis.API.dll (
                        echo ERROR: HealthAxis.API.dll was not found in the publish folder.
                        exit /b 1
                    )

                    echo Published API files:
                    dir publish
                '''
            }
        }

        stage('Create Deployment Zip') {
            steps {
                bat '''
                    powershell -NoProfile -ExecutionPolicy Bypass -Command "Compress-Archive -Path '.\\publish\\*' -DestinationPath '.\\deploy-package.zip' -Force"

                    if not exist deploy-package.zip (
                        echo ERROR: Deployment ZIP was not created.
                        exit /b 1
                    )
                '''
            }
        }

        stage('Upload to S3 and Deploy to Elastic Beanstalk') {
            steps {
                withCredentials([
                    [
                        $class: 'AmazonWebServicesCredentialsBinding',
                        credentialsId: 'aws-deploy-creds'
                    ]
                ]) {
                    bat '''
                        echo Uploading deployment package to S3...

                        aws s3 cp deploy-package.zip ^
                        s3://%S3_BUCKET%/deploy-package-%BUILD_NUMBER%.zip ^
                        --region %AWS_REGION%

                        if errorlevel 1 (
                            echo ERROR: Upload to S3 failed.
                            exit /b 1
                        )
                    '''

                    bat '''
                        echo Creating Elastic Beanstalk application version...

                        aws elasticbeanstalk create-application-version ^
                        --application-name "%EB_APPLICATION_NAME%" ^
                        --version-label "v-%BUILD_NUMBER%" ^
                        --source-bundle S3Bucket=%S3_BUCKET%,S3Key=deploy-package-%BUILD_NUMBER%.zip ^
                        --region %AWS_REGION%

                        if errorlevel 1 (
                            echo ERROR: Creating application version failed.
                            exit /b 1
                        )
                    '''

                    bat '''
                        echo Updating Elastic Beanstalk environment...

                        aws elasticbeanstalk update-environment ^
                        --environment-name "%EB_ENVIRONMENT_NAME%" ^
                        --version-label "v-%BUILD_NUMBER%" ^
                        --region %AWS_REGION%

                        if errorlevel 1 (
                            echo ERROR: Updating Elastic Beanstalk environment failed.
                            exit /b 1
                        )
                    '''
                }
            }
        }
    }

    post {
        success {
            echo 'HealthAxis build and deployment started successfully.'
        }

        failure {
            echo 'HealthAxis deployment failed. Check the failed stage in Jenkins Console Output.'
        }

        always {
            archiveArtifacts(
                artifacts: 'deploy-package.zip',
                allowEmptyArchive: true,
                fingerprint: true
            )
        }
    }
}
