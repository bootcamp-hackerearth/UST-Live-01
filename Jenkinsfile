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

        stage('Verify Project Files') {
            steps {
                bat '''
                echo ===== Checking project files =====

                if not exist HealthCareApp\\HealthCareApp.csproj (
                    echo ERROR: HealthCareApp API project was not found.
                    exit /b 1
                )

                if not exist HealthCareApp.AdminBlazor\\HealthCareApp.AdminBlazor.csproj (
                    echo ERROR: HealthCareApp Admin Blazor project was not found.
                    exit /b 1
                )

                if not exist HealthCareApp.UI\\package.json (
                    echo ERROR: Angular package.json was not found.
                    exit /b 1
                )

                echo ===== Available .NET projects =====
                dir /S /B *.csproj

                ec*o All required project files were *ound.
                '''
        *   }
        }

        stage('Cle*n Previous Build') {
            s*eps {
                bat '''
    *           echo Cleaning previous *uild files...

                if *xist artifacts rmdir /S /Q artifac*s
                if exist publish*rmdir /S /Q publish
              * if exist deploy-package.zip del /* /Q deploy-package.zip

          *     if exist HealthCareApp\\wwwro*t\\angular rmdir /S /Q HealthCareA*p\\wwwroot\\angular
              * if exist HealthCareApp\\wwwroot\\*lazor rmdir /S /Q HealthCareApp\\w*wroot\\blazor

                ech* Previous build files cleaned succ*ssfully.
                '''
     *      }
        }

        stage('*estore .NET Projects') {
         *  steps {
                bat '''
*               echo Restoring Heal*hCareApp API...
                dotnet restore HealthCareApp\\HealthCareApp.csproj

                if errorlevel 1 (
                    echo ERROR: HealthCareApp API restore failed.
                    exit /b 1
                )

                echo Restoring HealthCareApp Admin Blazor...
                dotnet restore HealthCareApp.AdminBlazor\\HealthCareApp.AdminBlazor.csproj

                if errorlevel 1 (
                    echo ERROR: HealthCareApp Admin Blazor restore failed.
                    exit /b 1
                )

                echo .NET restore completed successfully.
                '''
            }
        }

        stage('Build Angular') {
            steps {
                dir('HealthCareApp.UI') {
                    bat '''
                    echo Installing Angular dependencies...
                    call npm ci

                    if errorlevel 1 (
                        echo ERROR: npm ci failed.
                        exit /b 1
                    )

                    echo Building Angular production application...
                    call npx ng build --configuration production

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
                if not exist HealthCareApp\\wwwroot\\angular\\index.html (
                    echo ERROR: Angular index.html was not found.
                    echo Expected path: HealthCareApp\\wwwroot\\angular\\index.html
                    exit /b 1
                )

                echo Angular build files found successfully.
                dir HealthCareApp\\wwwroot\\angular
                '''
            }
        }

        stage('Publish Blazor Admin') {
            steps {
                bat '''
                echo Publishing Blazor Admin...

                dotnet publish HealthCareApp.AdminBlazor\\HealthCareApp.AdminBlazor.csproj -c Release -o artifacts\\adminblazor --no-restore

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
                if not exist artifacts\\adminblazor\\wwwroot\\_framework (
                    echo ERROR: Blazor WebAssembly framework files were not found.
                    exit /b 1
                )

                if exist HealthCareApp\\wwwroot\\blazor rmdir /S /Q HealthCareApp\\wwwroot\\blazor

                mkdir HealthCareApp\\wwwroot\\blazor

                xcopy /E /Y /I artifacts\\adminblazor\\wwwroot\\* HealthCareApp\\wwwroot\\blazor\\

                if errorlevel 1 (
                    echo ERROR: Copying Blazor files failed.
                    exit /b 1
                )

                if not exist HealthCareApp\\wwwroot\\blazor\\index.html (
                    echo ERROR: Blazor index.html was not copied.
                    exit /b 1
                )

                echo Blazor files copied successfully.
                '''
            }
        }

        stage('Create Blazor Fallback Files') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"

                $frameworkPath = ".\\HealthCareApp\\wwwroot\\blazor\\_framework"

                if (!(Test-Path $frameworkPath)) {
                    throw "Blazor _framework folder was not found at $frameworkPath"
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
                    throw "blazor.webassembly.js was not found."
                }

                Write-Host "Blazor fallback files created successfully."
                '''
            }
        }

        stage('Publish API') {
            steps {
                bat '''
                echo Publishing HealthCareApp API...

                dotnet publish HealthCareApp\\HealthCareApp.csproj -c Release -o publish --no-restore --self-contained false

                if errorlevel 1 (
                    echo ERROR: HealthCareApp API publish failed.
                    exit /b 1
                )

                echo HealthCareApp API published successfully.
                '''
            }
        }

        stage('Prepare Elastic Beanstalk Package') {
            steps {
                bat '''
                echo Creating Elastic Beanstalk Procfile...

                echo web: dotnet HealthCareApp.dll> publish\\Procfile

                if not exist publish\\Procfile (
                    echo ERROR: Procfile was not created.
                    exit /b 1
                )

                if not exist publish\\HealthCareApp.dll (
                    echo ERROR: HealthCareApp.dll was not found.
                    exit /b 1
                )

                if not exist publish\\HealthCareApp.deps.json (
                    echo ERROR: HealthCareApp.deps.json was not found.
                    exit /b 1
                )

                if not exist publish\\HealthCareApp.runtimeconfig.json (
                    echo ERROR: HealthCareApp.runtimeconfig.json was not found.
                    exit /b 1
                )

                if not exist publish\\wwwroot\\angular\\index.html (
                    echo ERROR: Angular files were not included in API publish.
                    exit /b 1
                )

                if not exist publish\\wwwroot\\blazor\\index.html (
                    echo ERROR: Blazor Admin files were not included in API publish.
                    exit /b 1
                )

                echo ===== Procfile content =====
                type publish\\Procfile

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
                echo ===== Checking deployment ZIP =====

                jar -tf deploy-package.zip | findstr /I "Procfile"
                if errorlevel 1 (
                    echo ERROR: Procfile is missing from deployment ZIP.
                    exit /b 1
                )

                jar -tf deploy-package.zip | findstr /I "HealthCareApp.dll"
                if errorlevel 1 (
                    echo ERROR: HealthCareApp.dll is missing from deployment ZIP.
                    exit /b 1
                )

                jar -tf deploy-package.zip | findstr /I "HealthCareApp.runtimeconfig.json"
                if errorlevel 1 (
                    echo ERROR: Runtime config is missing from deployment ZIP.
                    exit /b 1
                )

                jar -tf deploy-package.zip | findstr /I "wwwroot/angular/index.html"
                if errorlevel 1 (
                    echo ERROR: Angular index.html is missing from deployment ZIP.
                    exit /b 1
                )

                jar -tf deploy-package.zip | findstr /I "wwwroot/blazor/index.html"
                if errorlevel 1 (
                    echo ERROR: Blazor index.html is missing from deployment ZIP.
                    exit /b 1
                )

                echo Deployment ZIP created and verified successfully.
                '''
            }
        }

        stage('Upload Package to S3') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    bat '''
                    echo Uploading deployment package to S3...

                    aws s3 cp deploy-package.zip s3://%S3_BUCKET%/deploy-package-%BUILD_NUMBER%.zip --region %AWS_REGION%

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
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    bat '''
                    echo Creating Elastic Beanstalk application version v-%BUILD_NUMBER%...

                    aws elasticbeanstalk create-application-version --application-name "%EB_APPLICATION_NAME%" --version-label "v-%BUILD_NUMBER%" --description "Jenkins build %BUILD_NUMBER%" --source-bundle S3Bucket=%S3_BUCKET%,S3Key=deploy-package-%BUILD_NUMBER%.zip --region %AWS_REGION%

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
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    bat '''
                    echo Deploying version v-%BUILD_NUMBER%...

                    aws elasticbeanstalk update-environment --environment-name "%EB_ENVIRONMENT_NAME%" --version-label "v-%BUILD_NUMBER%" --region %AWS_REGION%

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
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    powershell '''
                    $ErrorActionPreference = "Stop"

                    $expectedVersion = "v-$env:BUILD_NUMBER"
                    $maximumChecks = 60
                    $delaySeconds = 15
                    $environmentReady = $false
                    $environmentCname = ""

                    Write-Host "Waiting for Elastic Beanstalk deployment..."
                    Write-Host "Expected version: $expectedVersion"

                    for ($check = 1; $check -le $maximumChecks; $check++) {
                        $json = aws elasticbeanstalk describe-environments --environment-names $env:EB_ENVIRONMENT_NAME --region $env:AWS_REGION --output json
                        $response = $json | ConvertFrom-Json

                        if ($null -eq $response.Environments -or $response.Environments.Count -eq 0) {
                            throw "Elastic Beanstalk environment was not found."
                        }

                        $environment = $response.Environments[0]

                        $status = $environment.Status
                        $health = $environment.Health
                        $runningVersion = $environment.VersionLabel
                        $environmentCname = $environment.CNAME

                        Write-Host ""
                        Write-Host "Check $check of $maximumChecks"
                        Write-Host "Status: $status"
                        Write-Host "Health: $health"
                        Write-Host "Running version: $runningVersion"
                        Write-Host "CNAME: $environmentCname"

                        if ($status -eq "Ready" -and $runningVersion -eq $expectedVersion) {
                            $environmentReady = $true
                            break
                        }

                        Start-Sleep -Seconds $delaySeconds
                    }

                    if (-not $environmentReady) {
                        throw "Deployment did not reach Ready state with expected version $expectedVersion."
                    }

                    Write-Host ""
                    Write-Host "Elastic Beanstalk is running expected version $expectedVersion."
                    Write-Host "Now checking application /health endpoint..."

                    $healthUrl = "http://$environmentCname/health"
                    Write-Host "Health URL: $healthUrl"

                    $healthSuccess = $false

                    for ($i = 1; $i -le 10; $i++) {
                        try {
                            $result = Invoke-WebRequest -Uri $healthUrl -UseBasicParsing -TimeoutSec 20

                            Write-Host "Health check attempt $i returned HTTP $($result.StatusCode)."

                            if ($result.StatusCode -eq 200) {
                                $healthSuccess = $true
                                break
                            }
                        }
                        catch {
                            Write-Host "Health check attempt $i failed: $($_.Exception.Message)"
                        }

                        Start-Sleep -Seconds 10
                    }

                    if (-not $healthSuccess) {
                        throw "Application /health endpoint did not return HTTP 200."
                    }

                    Write-Host ""
                    Write-Host "Deployment completed successfully."
                    Write-Host "Elastic Beanstalk version: $expectedVersion"
                    Write-Host "Application health endpoint is working."

                    exit 0
                    '''
                }
            }
        }
    }

    post {
        success {
            echo 'HealthAxis was built and deployed successfully.'
            echo 'Elastic Beanstalk is running the new application version.'
            echo 'The /health endpoint returned HTTP 200.'
        }

        failure {
            echo 'HealthAxis deployment failed.'
            echo 'Check the first failed stage in Jenkins Console Output.'
            echo 'If only EB color is Red but /health works, check EB health configuration separately.'
        }

        always {
            archiveArtifacts(
                artifacts: 'deploy-package.zip',
                allowEmptyArchive: true
            )
        }
    }
}
