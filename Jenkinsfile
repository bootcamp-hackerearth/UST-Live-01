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

        stage('Verify Project Files') {
            steps {
                bat '''
                    echo ===== Checking project files =====

                    if not exist HealthAxis.API\\HealthAxis.API.csproj (
                        echo ERROR: HealthAxis API project was not found.
                        exit /b 1
                    )

                    if not exist HealthAxis_Admin\\HealthAxis_Admin.csproj (
                        echo ERROR: HealthAxis Admin project was not found.
                        exit /b 1
                    )

                    if not exist HealthAxis.UI\\package.json (
                        echo ERROR: Angular package.json was not found.
                        exit /b 1
                    )

                    echo.
                    echo ===== Available .NET projects =====
                    dir /S /B *.csproj

                    echo.
                    echo All required project files were found.
                '''
            }
        }

        stage('Clean Previous Build') {
            steps {
                bat '''
                    echo Cleaning previous build files...

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

                    echo Previous build files cleaned successfully.
                '''
            }
        }

        stage('Restore .NET Projects') {
            steps {
                bat '''
                    echo Restoring HealthAxis API...

                    dotnet restore HealthAxis.API\\HealthAxis.API.csproj

                    if errorlevel 1 (
                        echo ERROR: HealthAxis API restore failed.
                        exit /b 1
                    )

                    echo Restoring HealthAxis Admin...

                    dotnet restore HealthAxis_Admin\\HealthAxis_Admin.csproj

                    if errorlevel 1 (
                        echo ERROR: HealthAxis Admin restore failed.
                        exit /b 1
                    )

                    echo .NET restore completed successfully.
                '''
            }
        }

        stage('Build Angular') {
            steps {
                dir('HealthAxis.UI') {
                    bat '''
                        echo Installing Angular dependencies...

                        call npm ci

                        if errorlevel 1 (
                            echo ERROR: npm ci failed.
                            exit /b 1
                        )

                        echo Building Angular production application...

                        call npm run build -- --configuration production

                        if errorlevel 1 (
                            echo ERROR: Angular production build failed.
                            exit /b 1
                        )

                        echo Angular build completed successfully.
                    '''
                }
            }
        }

        stage('Verify Angular Build') {
            steps {
                bat '''
                    if not exist HealthAxis.API\\wwwroot\\angular\\index.html (
                        echo ERROR: Angular index.html was not found.
                        echo Expected path:
                        echo HealthAxis.API\\wwwroot\\angular\\index.html
                        exit /b 1
                    )

                    echo Angular build files found successfully.
                    dir HealthAxis.API\\wwwroot\\angular
                '''
            }
        }

        stage('Publish Blazor Admin') {
            steps {
                bat '''
                    echo Publishing Blazor Admin...

                    dotnet publish HealthAxis_Admin\\HealthAxis_Admin.csproj ^
                    -c Release ^
                    -o blazor-publish-temp ^
                    --no-restore

                    if errorlevel 1 (
                        echo ERROR: Blazor Admin publish failed.
                        exit /b 1
                    )

                    echo Blazor Admin published successfully.
                '''
            }
        }

        stage('Copy Blazor into API') {
            steps {
                bat '''
                    if not exist blazor-publish-temp\\wwwroot\\_framework (
                        echo ERROR: Blazor WebAssembly framework files were not found.
                        exit /b 1
                    )

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

                    if not exist HealthAxis.API\\wwwroot\\admin\\index.html (
                        echo ERROR: Blazor index.html was not copied.
                        exit /b 1
                    )

                    echo Blazor files copied successfully.
                '''
            }
        }

        stage('Publish API') {
            steps {
                bat '''
                    echo Publishing HealthAxis API...

                    dotnet publish HealthAxis.API\\HealthAxis.API.csproj ^
                    -c Release ^
                    -o publish ^
                    --no-restore

                    if errorlevel 1 (
                        echo ERROR: HealthAxis API publish failed.
                        exit /b 1
                    )

                    echo HealthAxis API published successfully.
                '''
            }
        }

        stage('Prepare Elastic Beanstalk Package') {
            steps {
                bat '''
                    echo Creating Elastic Beanstalk Procfile...

                    echo web: dotnet HealthAxis.API.dll> publish\\Procfile

                    if not exist publish\\Procfile (
                        echo ERROR: Procfile was not created.
                        exit /b 1
                    )

                    if not exist publish\\HealthAxis.API.dll (
                        echo ERROR: HealthAxis.API.dll was not found.
                        exit /b 1
                    )

                    if not exist publish\\HealthAxis.API.deps.json (
                        echo ERROR: HealthAxis.API.deps.json was not found.
                        exit /b 1
                    )

                    if not exist publish\\HealthAxis.API.runtimeconfig.json (
                        echo ERROR: HealthAxis.API.runtimeconfig.json was not found.
                        exit /b 1
                    )

                    if not exist publish\\wwwroot\\angular\\index.html (
                        echo ERROR: Angular files were not included in API publish.
                        exit /b 1
                    )

                    if not exist publish\\wwwroot\\admin\\index.html (
                        echo ERROR: Blazor Admin files were not included in API publish.
                        exit /b 1
                    )

                    echo.
                    echo ===== Procfile content =====
                    type publish\\Procfile

                    echo.
                    echo Elastic Beanstalk package files are ready.
                '''
            }
        }

        stage('Create Deployment Zip') {
            steps {
                dir('publish') {
                    bat '''
                        echo Creating deployment ZIP...

                        jar -cMf ..\\deploy-package.zip .

                        if errorlevel 1 (
                            echo ERROR: Creating deployment ZIP failed.
                            exit /b 1
                        )

                        if not exist ..\\deploy-package.zip (
                            echo ERROR: deploy-package.zip was not created.
                            exit /b 1
                        )
                    '''
                }

                bat '''
                    echo.
                    echo ===== Checking deployment ZIP =====

                    jar -tf deploy-package.zip | findstr /I "Procfile"

                    if errorlevel 1 (
                        echo ERROR: Procfile is missing from deployment ZIP.
                        exit /b 1
                    )

                    jar -tf deploy-package.zip | findstr /I "HealthAxis.API.dll"

                    if errorlevel 1 (
                        echo ERROR: HealthAxis.API.dll is missing from deployment ZIP.
                        exit /b 1
                    )

                    jar -tf deploy-package.zip | findstr /I "HealthAxis.API.runtimeconfig.json"

                    if errorlevel 1 (
                        echo ERROR: Runtime configuration is missing from deployment ZIP.
                        exit /b 1
                    )

                    echo Deployment ZIP created and verified successfully.
                '''
            }
        }

        stage('Upload Package to S3') {
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
                            echo ERROR: Uploading deployment package to S3 failed.
                            exit /b 1
                        )

                        echo Deployment package uploaded successfully.
                    '''
                }
            }
        }

        stage('Create Application Version') {
            steps {
                withCredentials([
                    [
                        $class: 'AmazonWebServicesCredentialsBinding',
                        credentialsId: 'aws-deploy-creds'
                    ]
                ]) {
                    bat '''
                        echo Creating Elastic Beanstalk application version v-%BUILD_NUMBER%...

                        aws elasticbeanstalk create-application-version ^
                        --application-name "%EB_APPLICATION_NAME%" ^
                        --version-label "v-%BUILD_NUMBER%" ^
                        --description "Jenkins build %BUILD_NUMBER%" ^
                        --source-bundle S3Bucket=%S3_BUCKET%,S3Key=deploy-package-%BUILD_NUMBER%.zip ^
                        --region %AWS_REGION%

                        if errorlevel 1 (
                            echo ERROR: Creating Elastic Beanstalk application version failed.
                            exit /b 1
                        )

                        echo Application version v-%BUILD_NUMBER% created successfully.
                    '''
                }
            }
        }

        stage('Deploy to Elastic Beanstalk') {
            steps {
                withCredentials([
                    [
                        $class: 'AmazonWebServicesCredentialsBinding',
                        credentialsId: 'aws-deploy-creds'
                    ]
                ]) {
                    bat '''
                        echo Deploying version v-%BUILD_NUMBER%...

                        aws elasticbeanstalk update-environment ^
                        --environment-name "%EB_ENVIRONMENT_NAME%" ^
                        --version-label "v-%BUILD_NUMBER%" ^
                        --region %AWS_REGION%

                        if errorlevel 1 (
                            echo ERROR: Elastic Beanstalk update request failed.
                            exit /b 1
                        )

                        echo Elastic Beanstalk deployment request submitted.
                    '''
                }
            }
        }

        stage('Wait for Deployment Result') {
            steps {
                withCredentials([
                    [
                        $class: 'AmazonWebServicesCredentialsBinding',
                        credentialsId: 'aws-deploy-creds'
                    ]
                ]) {
                    powershell '''
                        $ErrorActionPreference = "Stop"

                        $expectedVersion = "v-$env:BUILD_NUMBER"
                        $maximumChecks = 60
                        $delaySeconds = 15

                        Write-Host "Waiting for Elastic Beanstalk deployment..."
                        Write-Host "Expected version: $expectedVersion"

                        for ($check = 1; $check -le $maximumChecks; $check++) {

                            $response = aws elasticbeanstalk describe-environments `
                                --environment-names $env:EB_ENVIRONMENT_NAME `
                                --region $env:AWS_REGION `
                                --output json | ConvertFrom-Json

                            if ($null -eq $response.Environments -or
                                $response.Environments.Count -eq 0) {
                                throw "Elastic Beanstalk environment was not found."
                            }

                            $environment = $response.Environments[0]

                            $status = $environment.Status
                            $health = $environment.Health
                            $healthStatus = $environment.HealthStatus
                            $runningVersion = $environment.VersionLabel

                            Write-Host ""
                            Write-Host "Check $check of $maximumChecks"
                            Write-Host "Status: $status"
                            Write-Host "Health: $health"
                            Write-Host "Health status: $healthStatus"
                            Write-Host "Running version: $runningVersion"

                            if ($status -eq "Ready") {

                                if ($runningVersion -ne $expectedVersion) {
                                    throw "Deployment failed. Expected $expectedVersion but Elastic Beanstalk is running $runningVersion."
                                }

                                if ($health -eq "Red") {
                                    throw "Deployment completed with Red environment health."
                                }

                                Write-Host ""
                                Write-Host "Elastic Beanstalk deployment completed successfully."
                                Write-Host "Running version: $runningVersion"
                                Write-Host "Environment health: $health"

                                exit 0
                            }

                            Start-Sleep -Seconds $delaySeconds
                        }

                        throw "Timed out while waiting for Elastic Beanstalk deployment."
                    '''
                }
            }
        }
    }

    post {
        success {
            echo 'HealthAxis was built and deployed successfully.'
            echo 'Elastic Beanstalk is running the new application version.'
        }

        failure {
            echo 'HealthAxis deployment failed.'
            echo 'Check the first failed stage in Jenkins Console Output.'
            echo 'For AWS deployment errors, check Elastic Beanstalk Logs and eb-engine.log.'
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
