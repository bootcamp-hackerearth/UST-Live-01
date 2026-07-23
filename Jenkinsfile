pipeline {
    agent any

    options {
        skipDefaultCheckout(true)
        timestamps()
        disableConcurrentBuilds()
    }

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_NOLOGO = '1'

        AWS_REGION = 'ap-south-2'

        // Enter the exact names from AWS
        EB_APPLICATION_NAME = 'HealthAxisAPI'
        EB_ENVIRONMENT_NAME = 'HealthAxisAPI-dev'
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

                    if not exist HealthAxis.UI\\package.json (
                        echo ERROR: HealthAxis.UI\\package.json was not found.
                        exit /b 1
                    )

                    echo Angular project found.
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

                    if exist HealthAxis.API\\wwwroot\\angular (
                        rmdir /S /Q HealthAxis.API\\wwwroot\\angular
                    )

                    if exist HealthAxis.API\\wwwroot\\admin (
                        rmdir /S /Q HealthAxis.API\\wwwroot\\admin
                    )
                '''
            }
        }

        stage('Restore .NET Projects') {
            steps {
                bat '''
                    dotnet restore HealthAxis.API\\HealthAxis.API.csproj

                    if errorlevel 1 (
                        echo ERROR: API restore failed.
                        exit /b 1
                    )

                    dotnet restore HealthAxis_Admin\\HealthAxis_Admin.csproj

                    if errorlevel 1 (
                        echo ERROR: Admin restore failed.
                        exit /b 1
                    )
                '''
            }
        }

        stage('Build Angular') {
            steps {
                dir('HealthAxis.UI') {
                    bat '''
                        call npm ci

                        if errorlevel 1 (
                            echo ERROR: npm ci failed.
                            exit /b 1
                        )

                        call npm run build -- --configuration production

                        if errorlevel 1 (
                            echo ERROR: Angular production build failed.
                            exit /b 1
                        )
                    '''
                }
            }
        }

        stage('Verify Angular Build') {
            steps {
                bat '''
                    if not exist HealthAxis.API\\wwwroot\\angular\\index.html (
                        echo ERROR: Angular index.html was not found.
                        echo Expected location:
                        echo HealthAxis.API\\wwwroot\\angular\\index.html
                        exit /b 1
                    )

                    echo Angular build found successfully.
                    dir HealthAxis.API\\wwwroot\\angular
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

                    if errorlevel 1 (
                        echo ERROR: Blazor Admin publish failed.
                        exit /b 1
                    )
                '''
            }
        }

        stage('Copy Blazor WebAssembly Files') {
            steps {
                bat '''
                    if exist blazor-publish-temp\\wwwroot\\_framework (
                        echo Blazor WebAssembly application detected.

                        if not exist HealthAxis.API\\wwwroot\\admin (
                            mkdir HealthAxis.API\\wwwroot\\admin
                        )

                        xcopy /E /Y /I ^
                        blazor-publish-temp\\wwwroot\\* ^
                        HealthAxis.API\\wwwroot\\admin\\

                        if errorlevel 1 (
                            echo ERROR: Copying Blazor files failed.
                            exit /b 1
                        )
                    ) else (
                        echo Blazor Server application detected.
                        echo Blazor Server cannot be copied into API wwwroot.
                        echo Admin was built successfully but must be deployed separately.
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

                    if errorlevel 1 (
                        echo ERROR: API publish failed.
                        exit /b 1
                    )
                '''
            }
        }

        stage('Verify Published Output') {
            steps {
                bat '''
                    if not exist publish\\HealthAxis.API.dll (
                        echo ERROR: HealthAxis.API.dll was not found.
                        exit /b 1
                    )

                    if not exist publish\\wwwroot\\angular\\index.html (
                        echo ERROR: Angular files were not included in API publish.
                        exit /b 1
                    )

                    echo ===== Published API files =====
                    dir publish

                    echo.
                    echo ===== Published Angular files =====
                    dir publish\\wwwroot\\angular
                '''
            }
        }

        stage('Create Deployment Zip') {
            steps {
                bat '''
                    powershell -NoProfile -ExecutionPolicy Bypass -Command ^
                    "Compress-Archive -Path '.\\publish\\*' -DestinationPath '.\\deploy-package.zip' -Force"

                    if not exist deploy-package.zip (
                        echo ERROR: Deployment ZIP was not created.
                        exit /b 1
                    )

                    echo Deployment ZIP created successfully.
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
                            echo ERROR: Creating Elastic Beanstalk version failed.
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
            echo 'HealthAxis deployment failed. Check the first failed Jenkins stage.'
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
