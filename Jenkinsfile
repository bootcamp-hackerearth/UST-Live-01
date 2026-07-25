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

    /*
     * After the first manual build succeeds, this trigger checks
     * the configured SCM for changes approximately every 5 minutes.
     */
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
        REPOSITORY_BRANCH = 'main'

        API_PROJECT = 'HealthAxisApi\\HealthAxisCore_Api.csproj'
        BLAZOR_PROJECT = 'HealthAxisAdminLayout\\HealthAxisAdminLayout.csproj'
        ANGULAR_PROJECT = 'HealthAxis_AngularProj'

        PUBLISH_DIRECTORY = 'publish'
        DEPLOY_PACKAGE = 'deploy-package.zip'

        /*
         * Required on this workstation so Node.js uses
         * the Windows system certificate store.
         */
        NODE_OPTIONS = '--use-system-ca'

        /*
         * Prevent Git from waiting for invisible interactive input.
         * The GitHub repository is public, so no credential is needed.
         */
        GIT_TERMINAL_PROMPT = '0'
        GIT_ASKPASS = 'echo'
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

                echo 'Checking out the public HealthAxis repository.'

                timeout(
                    time: 10,
                    unit: 'MINUTES'
                ) {
                    checkout([
                        $class: 'GitSCM',

                        branches: [
                            [
                                name: "*/${env.REPOSITORY_BRANCH}"
                            ]
                        ],

                        userRemoteConfigs: [
                            [
                                url: env.REPOSITORY_URL
                            ]
                        ],

                        extensions: [
                            [
                                $class: 'CloneOption',
                                shallow: true,
                                depth: 1,
                                noTags: true,
                                honorRefspec: false,
                                timeout: 10
                            ],
                            [
                                $class: 'CheckoutOption',
                                timeout: 10
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
                        echo The HealthAxis project may not be located at the repository root.
                        exit /b 1
                    )

                    if not exist "%API_PROJECT%" (
                        echo ERROR: HealthAxis API project was not found:
                        echo %API_PROJECT%
                        exit /b 1
                    )

                    if not exist "%BLAZOR_PROJECT%" (
                        echo ERROR: Blazor project was not found:
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
                        echo Commit package-lock.json so Jenkins can use npm ci.
                        exit /b 1
                    )

                    echo.
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

                    if exist artifacts (
                        rmdir /S /Q artifacts
                    )

                    if exist publish (
                        rmdir /S /Q publish
                    )

                    if exist deploy-package.zip (
                        del /F /Q deploy-package.zip
                    )

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

        stage('Build Angular and Blazor') {
            steps {
                timeout(
                    time: 30,
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
                        echo ERROR: Blazor _framework directory was not generated.
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
                        @{
                            Path = Join-Path `
                                $publishDirectory `
                                "HealthAxisCore_Api.dll"

                            Description =
                                "Published HealthAxis API DLL"
                        },
                        @{
                            Path = Join-Path `
                                $publishDirectory `
                                "HealthAxisCore_Api.runtimeconfig.json"

                            Description =
                                "HealthAxis runtime configuration"
                        },
                        @{
                            Path = Join-Path `
                                $publishDirectory `
                                "HealthAxisCore_Api.deps.json"

                            Description =
                                "HealthAxis dependency configuration"
                        },
                        @{
                            Path = Join-Path `
                                $publishDirectory `
                                "wwwroot\\angular\\index.html"

                            Description =
                                "Published Angular index.html"
                        },
                        @{
                            Path = Join-Path `
                                $publishDirectory `
                                "wwwroot\\blazor\\index.html"

                            Description =
                                "Published Blazor index.html"
                        },
                        @{
                            Path = Join-Path `
                                $publishDirectory `
                                "wwwroot\\blazor\\_framework"

                            Description =
                                "Published Blazor framework directory"
                        }
                    )

                    foreach ($requiredPath in $requiredPaths) {
                        if (
                            -not (
                                Test-Path `
                                    -LiteralPath $requiredPath.Path
                            )
                        ) {
                            throw (
                                $requiredPath.Description +
                                " was not found: " +
                                $requiredPath.Path
                            )
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
                    if errorlevel 1 (
                        echo ERROR: Procfile is missing from ZIP root.
                        exit /b 1
                    )

                    jar -tf deploy-package.zip | findstr /I "HealthAxisCore_Api.dll"
                    if errorlevel 1 (
                        echo ERROR: HealthAxis API DLL is missing from ZIP.
                        exit /b 1
                    )

                    jar -tf deploy-package.zip | findstr /I "HealthAxisCore_Api.runtimeconfig.json"
                    if errorlevel 1 (
                        echo ERROR: runtimeconfig.json is missing from ZIP.
                        exit /b 1
                    )

                    jar -tf deploy-package.zip | findstr /I "wwwroot/angular/index.html"
                    if errorlevel 1 (
                        echo ERROR: Angular index.html is missing from ZIP.
                        exit /b 1
                    )

                    jar -tf deploy-package.zip | findstr /I "wwwroot/blazor/index.html"
                    if errorlevel 1 (
                        echo ERROR: Blazor index.html is missing from ZIP.
                        exit /b 1
                    )

                    echo Deployment ZIP verified successfully.
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
                            echo Verify aws-deploy-creds and AWS certificate trust.
                            exit /b 1
                        )
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

                        echo ==========================================
                        echo Verifying AWS deployment resources
                        echo ==========================================

                        aws s3api head-bucket ^
                            --bucket "%S3_BUCKET%" ^
                            --region "%AWS_REGION%"

                        if errorlevel 1 (
                            echo ERROR: Jenkins cannot access the S3 deployment bucket.
                            exit /b 1
                        )

                        for /f "delims=" %%A in ('
                            aws elasticbeanstalk describe-applications ^
                                --application-names "%EB_APPLICATION_NAME%" ^
                                --region "%AWS_REGION%" ^
                                --query "Applications[0].ApplicationName" ^
                                --output text
                        ') do set "FOUND_APPLICATION=%%A"

                        if /I not "%FOUND_APPLICATION%"=="%EB_APPLICATION_NAME%" (
                            echo ERROR: Elastic Beanstalk application was not found.
                            echo Expected: %EB_APPLICATION_NAME%
                            echo Found: %FOUND_APPLICATION%
                            exit /b 1
                        )

                        for /f "delims=" %%E in ('
                            aws elasticbeanstalk describe-environments ^
                                --environment-names "%EB_ENVIRONMENT_NAME%" ^
                                --region "%AWS_REGION%" ^
                                --query "Environments[0].EnvironmentName" ^
                                --output text
                        ') do set "FOUND_ENVIRONMENT=%%E"

                        if /I not "%FOUND_ENVIRONMENT%"=="%EB_ENVIRONMENT_NAME%" (
                            echo ERROR: Elastic Beanstalk environment was not found.
                            echo Expected: %EB_ENVIRONMENT_NAME%
                            echo Found: %FOUND_ENVIRONMENT%
                            exit /b 1
                        )

                        echo AWS resources verified successfully.
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

                        set "VERSION_LABEL=healthaxis-%BUILD_NUMBER%"
                        set "S3_KEY=healthaxis/deploy-package-%BUILD_NUMBER%.zip"

                        echo ==========================================
                        echo Creating Elastic Beanstalk application version
                        echo ==========================================

                        aws elasticbeanstalk create-application-version ^
                            --application-name "%EB_APPLICATION_NAME%" ^
                            --version-label "%VERSION_LABEL%" ^
                            --description "HealthAxis Jenkins build %BUILD_NUMBER%" ^
                            --source-bundle S3Bucket=%S3_BUCKET%,S3Key=%S3_KEY% ^
                            --region "%AWS_REGION%"

                        if errorlevel 1 (
                            echo ERROR: Elastic Beanstalk application version creation failed.
                            exit /b 1
                        )

                        echo Created version:
                        echo %VERSION_LABEL%
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

                        set "VERSION_LABEL=healthaxis-%BUILD_NUMBER%"

                        echo ==========================================
                        echo Deploying to Elastic Beanstalk
                        echo ==========================================

                        aws elasticbeanstalk update-environment ^
                            --environment-name "%EB_ENVIRONMENT_NAME%" ^
                            --version-label "%VERSION_LABEL%" ^
                            --region "%AWS_REGION%"

                        if errorlevel 1 (
                            echo ERROR: Elastic Beanstalk environment update failed.
                            exit /b 1
                        )

                        echo Deployment request accepted.
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
                        time: 20,
                        unit: 'MINUTES'
                    ) {
                        powershell '''
                            $ErrorActionPreference = "Stop"

                            $expectedVersion =
                                "healthaxis-$env:BUILD_NUMBER"

                            $maximumChecks = 80
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
                                    throw (
                                        "Unable to query the " +
                                        "Elastic Beanstalk environment."
                                    )
                                }

                                $result =
                                    $environmentJson |
                                    ConvertFrom-Json

                                if (
                                    $null -eq $result.Environments -or
                                    $result.Environments.Count -eq 0
                                ) {
                                    throw (
                                        "Elastic Beanstalk environment " +
                                        "was not found."
                                    )
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
                                            "Elastic Beanstalk deployed " +
                                            "the version, but environment " +
                                            "health is Red."
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
                                "Elastic Beanstalk did not reach " +
                                "Ready state with the expected version."
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
