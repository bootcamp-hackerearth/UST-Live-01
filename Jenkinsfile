pipeline {
    agent any

    options {
        skipDefaultCheckout(true)
        timestamps()
        disableConcurrentBuilds()

        buildDiscarder(
            logRotator(
                numToKeepStr: '20',
                artifactNumToKeepStr: '10'
            )
        )
    }

    triggers {
        pollSCM('H/5 * * * *')
    }

    environment {
        AWS_REGION = 'ap-southeast-2'
        AWS_DEFAULT_REGION = 'ap-southeast-2'

        EB_APPLICATION_NAME = 'HealthAxis-app'
        EB_ENVIRONMENT_NAME = 'HealthAxis-app-dev'

        S3_BUCKET = 'heathaxis-jenkins-bucket-527133285403-ap-southeast-2-an'

        REPOSITORY_URL = 'https://github.com/bootcamp-hackerearth/UST-Live-01.git'
        REPOSITORY_BRANCH = 'Feature/Sprint5_Pod1_Ayushi'

        API_PROJECT = 'HealthAxisApi\\HealthAxisCore_Api.csproj'
        BLAZOR_PROJECT = 'HealthAxisAdminLayout\\HealthAxisAdminLayout.csproj'
        ANGULAR_PROJECT = 'HealthAxis_AngularProj'

        PUBLISH_DIRECTORY = 'publish'
        DEPLOY_PACKAGE = 'deploy-package.zip'

        NODE_OPTIONS = '--use-system-ca'

        AWS_CA_BUNDLE = 'C:\\ProgramData\\Jenkins\\.jenkins\\certs\\company-ca-bundle.pem'

        GIT_TERMINAL_PROMPT = '0'
        GIT_ASKPASS = 'echo'

        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_NOLOGO = '1'
    }

    stages {
        stage('Clean Workspace and Checkout') {
            steps {
                echo 'Cleaning the Jenkins workspace.'

                cleanWs(
                    deleteDirs: true,
                    notFailBuild: false,
                    disableDeferredWipeout: true
                )

                echo 'Checking out the HealthAxis feature branch.'

                timeout(
                    time: 20,
                    unit: 'MINUTES'
                ) {
                    checkout([
                        $class: 'GitSCM',

                        branches: [
                            [
                                name: '*/Feature/Sprint5_Pod1_Ayushi'
                            ]
                        ],

                        userRemoteConfigs: [
                            [
                                url: 'https://github.com/bootcamp-hackerearth/UST-Live-01.git',

                                refspec:
                                    '+refs/heads/Feature/Sprint5_Pod1_Ayushi:' +
                                    'refs/remotes/origin/Feature/Sprint5_Pod1_Ayushi'
                            ]
                        ],

                        extensions: [
                            [
                                $class: 'CloneOption',
                                shallow: true,
                                depth: 1,
                                noTags: true,
                                honorRefspec: true,
                                timeout: 20
                            ],
                            [
                                $class: 'CheckoutOption',
                                timeout: 20
                            ]
                        ]
                    ])
                }

                bat '''
                    @echo off

                    echo ==========================================
                    echo Checked-out Git revision
                    echo ==========================================

                    git rev-parse HEAD
                    if errorlevel 1 exit /b 1

                    git branch --show-current
                    if errorlevel 1 exit /b 1

                    git log -1 --pretty=oneline
                    if errorlevel 1 exit /b 1
                '''
            }
        }

        stage('Verify Required Tools') {
            steps {
                bat '''
                    @echo off

                    echo ==========================================
                    echo Verifying required build tools
                    echo ==========================================

                    dotnet --version
                    if errorlevel 1 exit /b 1

                    git --version
                    if errorlevel 1 exit /b 1

                    node --version
                    if errorlevel 1 exit /b 1

                    call npm --version
                    if errorlevel 1 exit /b 1

                    aws --version
                    if errorlevel 1 exit /b 1

                    java -version
                    if errorlevel 1 exit /b 1

                    jar --version
                    if errorlevel 1 exit /b 1

                    powershell.exe -NoProfile -Command "$PSVersionTable.PSVersion"
                    if errorlevel 1 exit /b 1

                    echo.
                    echo All required tools are available.
                '''
            }
        }

        stage('Verify Node HTTPS') {
            steps {
                bat '''
                    @echo off

                    echo ==========================================
                    echo Verifying Node HTTPS connectivity to AWS
                    echo ==========================================

                    node -e "require('https').get('https://sts.ap-southeast-2.amazonaws.com',res=>console.log('HTTPS OK:',res.statusCode)).on('error',error=>{console.error(error);process.exit(1)})"

                    if errorlevel 1 (
                        echo ERROR: Node.js cannot establish trusted HTTPS connectivity to AWS.
                        exit /b 1
                    )
                '''
            }
        }

        stage('Verify Project Files') {
            steps {
                bat '''
                    @echo off

                    echo ==========================================
                    echo Verifying HealthAxis project files
                    echo ==========================================

                    if not exist build-frontends.ps1 (
                        echo ERROR: build-frontends.ps1 was not found.
                        exit /b 1
                    )

                    if not exist "%API_PROJECT%" (
                        echo ERROR: HealthAxis API project was not found.
                        echo %API_PROJECT%
                        exit /b 1
                    )

                    if not exist "%BLAZOR_PROJECT%" (
                        echo ERROR: Blazor project was not found.
                        echo %BLAZOR_PROJECT%
                        exit /b 1
                    )

                    if not exist HealthAxisAdminLayout\\wwwroot\\index.html (
                        echo ERROR: Blazor source index.html was not found.
                        exit /b 1
                    )

                    if not exist HealthAxisAdminLayout\\wwwroot\\appsettings.json (
                        echo ERROR: Blazor appsettings.json was not found.
                        exit /b 1
                    )

                    if not exist "%ANGULAR_PROJECT%\\package.json" (
                        echo ERROR: Angular package.json was not found.
                        exit /b 1
                    )

                    if not exist "%ANGULAR_PROJECT%\\angular.json" (
                        echo ERROR: Angular angular.json was not found.
                        exit /b 1
                    )

                    if not exist "%ANGULAR_PROJECT%\\package-lock.json" (
                        echo ERROR: Angular package-lock.json was not found.
                        exit /b 1
                    )

                    echo All required HealthAxis project files were found.
                '''
            }
        }

        stage('Clean Previous Generated Outputs') {
            steps {
                bat '''
                    @echo off

                    echo ==========================================
                    echo Removing previous generated outputs
                    echo ==========================================

                    if exist artifacts rmdir /S /Q artifacts
                    if exist publish rmdir /S /Q publish
                    if exist deploy-package.zip del /F /Q deploy-package.zip

                    if exist HealthAxisApi\\wwwroot\\angular (
                        rmdir /S /Q HealthAxisApi\\wwwroot\\angular
                    )

                    if exist HealthAxisApi\\wwwroot\\blazor (
                        rmdir /S /Q HealthAxisApi\\wwwroot\\blazor
                    )

                    echo Previous generated outputs removed.
                '''
            }
        }

        stage('Restore API Dependencies') {
            steps {
                timeout(
                    time: 15,
                    unit: 'MINUTES'
                ) {
                    bat '''
                        @echo off

                        echo ==========================================
                        echo Restoring HealthAxis API dependencies
                        echo ==========================================

                        dotnet restore "%API_PROJECT%"

                        if errorlevel 1 (
                            echo ERROR: API dependency restore failed.
                            exit /b 1
                        )
                    '''
                }
            }
        }

        stage('Build Angular and Blazor') {
            steps {
                timeout(
                    time: 40,
                    unit: 'MINUTES'
                ) {
                    bat '''
                        @echo off

                        echo ==========================================
                        echo Building Angular and Blazor frontends
                        echo ==========================================

                        powershell.exe ^
                            -NoLogo ^
                            -NoProfile ^
                            -NonInteractive ^
                            -ExecutionPolicy Bypass ^
                            -File ".\\build-frontends.ps1"

                        if errorlevel 1 (
                            echo ERROR: Frontend build script failed.
                            exit /b 1
                        )
                    '''
                }
            }
        }

        stage('Verify Frontend Artifacts') {
            steps {
                bat '''
                    @echo off

                    echo ==========================================
                    echo Verifying generated frontend artifacts
                    echo ==========================================

                    if not exist HealthAxisApi\\wwwroot\\angular\\index.html (
                        echo ERROR: Angular index.html was not generated.
                        exit /b 1
                    )

                    if not exist HealthAxisApi\\wwwroot\\blazor\\index.html (
                        echo ERROR: Blazor index.html was not generated.
                        exit /b 1
                    )

                    if not exist HealthAxisApi\\wwwroot\\blazor\\_framework (
                        echo ERROR: Blazor _framework was not generated.
                        exit /b 1
                    )

                    if not exist HealthAxisApi\\wwwroot\\blazor\\appsettings.json (
                        echo ERROR: Blazor appsettings.json was not generated.
                        exit /b 1
                    )

                    echo Angular and Blazor artifacts verified successfully.
                '''
            }
        }

        stage('Publish HealthAxis API') {
            steps {
                timeout(
                    time: 20,
                    unit: 'MINUTES'
                ) {
                    bat '''
                        @echo off

                        echo ==========================================
                        echo Publishing HealthAxis API
                        echo ==========================================

                        dotnet publish "%API_PROJECT%" ^
                            -c Release ^
                            -o "%PUBLISH_DIRECTORY%" ^
                            --no-restore ^
                            --self-contained false

                        if errorlevel 1 (
                            echo ERROR: HealthAxis API publish failed.
                            exit /b 1
                        )
                    '''
                }
            }
        }

        stage('Verify Published Application') {
            steps {
                powershell '''
                    $ErrorActionPreference = "Stop"

                    $publishDirectory =
                        Join-Path $env:WORKSPACE "publish"

                    $requiredPaths = @(
                        "HealthAxisCore_Api.dll",
                        "HealthAxisCore_Api.runtimeconfig.json",
                        "HealthAxisCore_Api.deps.json",
                        "wwwroot\\angular\\index.html",
                        "wwwroot\\blazor\\index.html",
                        "wwwroot\\blazor\\_framework"
                    )

                    foreach ($relativePath in $requiredPaths) {
                        $fullPath =
                            Join-Path `
                                $publishDirectory `
                                $relativePath

                        if (-not (Test-Path -LiteralPath $fullPath)) {
                            throw "Required publish output was not found: $fullPath"
                        }
                    }

                    Write-Host "Published application verified successfully."
                '''
            }
        }

        stage('Create Procfile') {
            steps {
                powershell '''
                    $ErrorActionPreference = "Stop"

                    $procfile =
                        Join-Path `
                            $env:WORKSPACE `
                            "publish\\Procfile"

                    Set-Content `
                        -LiteralPath $procfile `
                        -Value "web: dotnet HealthAxisCore_Api.dll" `
                        -Encoding Ascii

                    if (-not (Test-Path -LiteralPath $procfile)) {
                        throw "Procfile was not created."
                    }

                    Write-Host "Procfile contents:"
                    Get-Content -LiteralPath $procfile
                '''
            }
        }

        stage('Create Deployment ZIP') {
            steps {
                dir('publish') {
                    bat '''
                        @echo off

                        echo ==========================================
                        echo Creating Elastic Beanstalk deployment ZIP
                        echo ==========================================

                        jar -cMf ..\\deploy-package.zip .

                        if errorlevel 1 (
                            echo ERROR: Deployment ZIP creation failed.
                            exit /b 1
                        )
                    '''
                }
            }
        }

        stage('Verify Deployment ZIP') {
            steps {
                bat '''
                    @echo off

                    echo ==========================================
                    echo Verifying deployment ZIP contents
                    echo ==========================================

                    if not exist deploy-package.zip (
                        echo ERROR: deploy-package.zip was not created.
                        exit /b 1
                    )

                    jar -tf deploy-package.zip | findstr /I /X "Procfile"
                    if errorlevel 1 exit /b 1

                    jar -tf deploy-package.zip | findstr /I /X "HealthAxisCore_Api.dll"
                    if errorlevel 1 exit /b 1

                    jar -tf deploy-package.zip | findstr /I /X "HealthAxisCore_Api.runtimeconfig.json"
                    if errorlevel 1 exit /b 1

                    jar -tf deploy-package.zip | findstr /I /X "wwwroot/angular/index.html"
                    if errorlevel 1 exit /b 1

                    jar -tf deploy-package.zip | findstr /I /X "wwwroot/blazor/index.html"
                    if errorlevel 1 exit /b 1

                    echo Deployment ZIP verified successfully.
                '''
            }
        }

        stage('Verify AWS Certificate Bundle') {
            steps {
                bat '''
                    @echo off

                    echo ==========================================
                    echo Verifying AWS CA certificate bundle
                    echo ==========================================

                    echo AWS CA bundle:
                    echo %AWS_CA_BUNDLE%

                    if not exist "%AWS_CA_BUNDLE%" (
                        echo ERROR: AWS CA bundle was not found.
                        echo Expected file:
                        echo %AWS_CA_BUNDLE%
                        exit /b 1
                    )

                    findstr /C:"BEGIN CERTIFICATE" "%AWS_CA_BUNDLE%" > nul

                    if errorlevel 1 (
                        echo ERROR: AWS CA bundle is not PEM encoded.
                        exit /b 1
                    )

                    echo AWS CA certificate bundle was found.
                '''
            }
        }

        stage('Verify AWS Credentials') {
            steps {
                withCredentials([
                    [
                        $class: 'AmazonWebServicesCredentialsBinding',
                        credentialsId: 'aws-deploy-creds'
                    ]
                ]) {
                    bat '''
                        @echo off

                        echo ==========================================
                        echo Verifying Jenkins AWS credentials
                        echo ==========================================

                        aws sts get-caller-identity ^
                            --region "%AWS_REGION%"

                        if errorlevel 1 (
                            echo ERROR: Jenkins cannot authenticate to AWS.
                            echo Check the CA bundle and aws-deploy-creds.
                            exit /b 1
                        )

                        echo Jenkins successfully authenticated to AWS.
                    '''
                }
            }
        }

        stage('Verify AWS Resources') {
            steps {
                withCredentials([
                    [
                        $class: 'AmazonWebServicesCredentialsBinding',
                        credentialsId: 'aws-deploy-creds'
                    ]
                ]) {
                    bat '''
                        @echo off
                        setlocal EnableExtensions EnableDelayedExpansion

                        echo ==========================================
                        echo Verifying AWS deployment resources
                        echo ==========================================

                        aws s3api head-bucket ^
                            --bucket "%S3_BUCKET%" ^
                            --region "%AWS_REGION%"

                        if errorlevel 1 (
                            echo ERROR: Jenkins cannot access the S3 bucket.
                            exit /b 1
                        )

                        set "FOUND_APPLICATION="

                        for /f "usebackq delims=" %%A in (`aws elasticbeanstalk describe-applications --application-names "%EB_APPLICATION_NAME%" --region "%AWS_REGION%" --query "Applications[0].ApplicationName" --output text`) do (
                            set "FOUND_APPLICATION=%%A"
                        )

                        if /I not "!FOUND_APPLICATION!"=="%EB_APPLICATION_NAME%" (
                            echo ERROR: Elastic Beanstalk application was not found.
                            echo Expected: %EB_APPLICATION_NAME%
                            echo Found: !FOUND_APPLICATION!
                            exit /b 1
                        )

                        set "FOUND_ENVIRONMENT="

                        for /f "usebackq delims=" %%E in (`aws elasticbeanstalk describe-environments --environment-names "%EB_ENVIRONMENT_NAME%" --region "%AWS_REGION%" --query "Environments[0].EnvironmentName" --output text`) do (
                            set "FOUND_ENVIRONMENT=%%E"
                        )

                        if /I not "!FOUND_ENVIRONMENT!"=="%EB_ENVIRONMENT_NAME%" (
                            echo ERROR: Elastic Beanstalk environment was not found.
                            echo Expected: %EB_ENVIRONMENT_NAME%
                            echo Found: !FOUND_ENVIRONMENT!
                            exit /b 1
                        )

                        echo AWS resources verified successfully.

                        endlocal
                    '''
                }
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
                        @echo off
                        setlocal

                        set "S3_KEY=healthaxis/deploy-package-%BUILD_NUMBER%.zip"

                        echo ==========================================
                        echo Uploading deployment package to S3
                        echo ==========================================

                        aws s3 cp ^
                            "%DEPLOY_PACKAGE%" ^
                            "s3://%S3_BUCKET%/%S3_KEY%" ^
                            --region "%AWS_REGION%" ^
                            --only-show-errors

                        if errorlevel 1 (
                            echo ERROR: Deployment package upload failed.
                            exit /b 1
                        )

                        echo Uploaded:
                        echo s3://%S3_BUCKET%/%S3_KEY%

                        endlocal
                    '''
                }
            }
        }

        stage('Create Elastic Beanstalk Version') {
            steps {
                withCredentials([
                    [
                        $class: 'AmazonWebServicesCredentialsBinding',
                        credentialsId: 'aws-deploy-creds'
                    ]
                ]) {
                    bat '''
                        @echo off
                        setlocal

                        set "VERSION_LABEL=healthaxis-%BUILD_NUMBER%"
                        set "S3_KEY=healthaxis/deploy-package-%BUILD_NUMBER%.zip"

                        echo ==========================================
                        echo Creating Elastic Beanstalk version
                        echo ==========================================

                        aws elasticbeanstalk create-application-version ^
                            --application-name "%EB_APPLICATION_NAME%" ^
                            --version-label "%VERSION_LABEL%" ^
                            --description "HealthAxis Jenkins build %BUILD_NUMBER%" ^
                            --source-bundle S3Bucket=%S3_BUCKET%,S3Key=%S3_KEY% ^
                            --region "%AWS_REGION%"

                        if errorlevel 1 (
                            echo ERROR: Application version creation failed.
                            exit /b 1
                        )

                        echo Created version: %VERSION_LABEL%

                        endlocal
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
                        @echo off
                        setlocal

                        set "VERSION_LABEL=healthaxis-%BUILD_NUMBER%"

                        echo ==========================================
                        echo Deploying to Elastic Beanstalk
                        echo ==========================================

                        aws elasticbeanstalk update-environment ^
                            --environment-name "%EB_ENVIRONMENT_NAME%" ^
                            --version-label "%VERSION_LABEL%" ^
                            --region "%AWS_REGION%"

                        if errorlevel 1 (
                            echo ERROR: Elastic Beanstalk update failed.
                            exit /b 1
                        )

                        echo Deployment request accepted.

                        endlocal
                    '''
                }
            }
        }

        stage('Wait for Elastic Beanstalk') {
            steps {
                withCredentials([
                    [
                        $class: 'AmazonWebServicesCredentialsBinding',
                        credentialsId: 'aws-deploy-creds'
                    ]
                ]) {
                    timeout(
                        time: 25,
                        unit: 'MINUTES'
                    ) {
                        powershell '''
                            $ErrorActionPreference = "Stop"

                            $expectedVersion =
                                "healthaxis-$env:BUILD_NUMBER"

                            $maximumChecks = 100
                            $delaySeconds = 15

                            for (
                                $check = 1;
                                $check -le $maximumChecks;
                                $check++
                            ) {
                                $environmentJson =
                                    aws elasticbeanstalk `
                                        describe-environments `
                                        --environment-names `
                                            $env:EB_ENVIRONMENT_NAME `
                                        --region `
                                            $env:AWS_REGION `
                                        --output json

                                if ($LASTEXITCODE -ne 0) {
                                    throw "Unable to query Elastic Beanstalk."
                                }

                                $result =
                                    $environmentJson |
                                    ConvertFrom-Json

                                if (
                                    $null -eq $result.Environments -or
                                    $result.Environments.Count -eq 0
                                ) {
                                    throw "Elastic Beanstalk environment was not found."
                                }

                                $environment =
                                    $result.Environments[0]

                                $status =
                                    $environment.Status

                                $health =
                                    $environment.Health

                                $healthStatus =
                                    $environment.HealthStatus

                                $version =
                                    $environment.VersionLabel

                                $cname =
                                    $environment.CNAME

                                Write-Host ""
                                Write-Host "Check: $check"
                                Write-Host "Status: $status"
                                Write-Host "Health: $health"
                                Write-Host "Health status: $healthStatus"
                                Write-Host "Version: $version"
                                Write-Host "Expected version: $expectedVersion"
                                Write-Host "CNAME: $cname"

                                if (
                                    $status -eq "Ready" -and
                                    $version -eq $expectedVersion
                                ) {
                                    if ($health -eq "Red") {
                                        throw (
                                            "The expected version was deployed, " +
                                            "but environment health is Red."
                                        )
                                    }

                                    Write-Host ""
                                    Write-Host (
                                        "Elastic Beanstalk is running " +
                                        "the expected HealthAxis version."
                                    )

                                    exit 0
                                }

                                Start-Sleep `
                                    -Seconds $delaySeconds
                            }

                            throw (
                                "Elastic Beanstalk did not reach Ready " +
                                "state with the expected version."
                            )
                        '''
                    }
                }
            }
        }

        stage('Display Final Environment') {
            steps {
                withCredentials([
                    [
                        $class: 'AmazonWebServicesCredentialsBinding',
                        credentialsId: 'aws-deploy-creds'
                    ]
                ]) {
                    bat '''
                        @echo off

                        echo ==========================================
                        echo Final Elastic Beanstalk environment
                        echo ==========================================

                        aws elasticbeanstalk describe-environments ^
                            --environment-names "%EB_ENVIRONMENT_NAME%" ^
                            --region "%AWS_REGION%" ^
                            --query "Environments[0].[ApplicationName,EnvironmentName,Status,Health,HealthStatus,VersionLabel,CNAME]" ^
                            --output table

                        if errorlevel 1 exit /b 1
                    '''
                }
            }
        }
    }

    post {
        success {
            echo 'HealthAxis Jenkins deployment completed successfully.'

            archiveArtifacts(
                artifacts: 'deploy-package.zip',
                fingerprint: true,
                allowEmptyArchive: false
            )
        }

        failure {
            echo 'HealthAxis Jenkins deployment failed.'
            echo 'Review the first failing Jenkins stage.'
            echo 'Also review Elastic Beanstalk Events and Logs.'
        }

        always {
            echo "Completed Jenkins build ${env.BUILD_NUMBER}."
        }
    }
}
