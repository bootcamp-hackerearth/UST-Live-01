pipeline {
    agent any

   *options {
        skipDefaultCheck*ut(true)
        timestamps()
    *   disableConcurrentBuilds()
    }*
    environment {
        DOTNET_*LI_TELEMETRY_OPTOUT = '1'
        *OTNET_NOLOGO = '1'

        AWS_RE*ION = 'ap-south-2'
        EB_APPL*CATION_NAME = 'HealthCareApp'
    *   EB_ENVIRONMENT_NAME = 'HealthCa*eApp-dev'
        S3_BUCKET = 'hea*thaxis-jenkins-bucket-847814614822*ap-south-2-an'
        DEPLOY_PACK*GE = 'deploy-package.zip'
    }

 *  stages {
        stage('Checkout*) {
            steps {
          *     checkout scm
            }
  *     }

        stage('Verify Proj*ct Files') {
            steps {
 *              bat '''
            *   echo Checking required project *iles...

                if not ex*st HealthCareApp\\HealthCareApp.cs*roj (
                    echo ERR*R: HealthCareApp project not found*
                    exit /b 1
   *            )

                if *ot exist HealthCareApp.AdminBlazor*\HealthCareApp.AdminBlazor.csproj *
                    echo ERROR: A*min Blazor project not found.
    *               exit /b 1
         *      )

                if not ex*st HealthCareApp.UI\\package.json *
                    echo ERROR: A*gular package.json not found.
    *               exit /b 1
         *      )

                echo Requ*red project files found.
         *      '''
            }
        }
*        stage('Clean Previous Buil*') {
            steps {
         *      bat '''
                if e*ist artifacts rmdir /S /Q artifact*
                if exist publish *mdir /S /Q publish
               *if exist deploy-package.zip del /F*/Q deploy-package.zip

           *    if exist HealthCareApp\\wwwroot\\angular rmdir /S /Q HealthCareApp\\wwwroot\\angular
                if exist HealthCareApp\\wwwroot\\blazor rmdir /S /Q HealthCareApp\\wwwroot\\blazor
                '''
            }
        }

        stage('Restore .NET Projects') {
            steps {
                bat '''
                dotnet restore HealthCareApp\\HealthCareApp.csproj
                if errorlevel 1 exit /b 1

                dotnet restore HealthCareApp.AdminBlazor\\HealthCareApp.AdminBlazor.csproj
                if errorlevel 1 exit /b 1
                '''
            }
        }

        stage('Build Angular') {
            steps {
                dir('HealthCareApp.UI') {
                    bat '''
                    call npm ci
                    if errorlevel 1 exit /b 1

                    call npx ng build --configuration production
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }

        stage('Verify Angular Build') {
            steps {
                bat '''
                if not exist HealthCareApp\\wwwroot\\angular\\index.html (
                    echo ERROR: Angular build output missing.
                    exit /b 1
                )
                '''
            }
        }

        stage('Publish Blazor Admin') {
            steps {
                bat '''
                dotnet publish HealthCareApp.AdminBlazor\\HealthCareApp.AdminBlazor.csproj -c Release -o artifacts\\adminblazor --no-restore
                if errorlevel 1 exit /b 1
                '''
            }
        }

        stage('Copy Blazor into API wwwroot') {
            steps {
                bat '''
                if not exist artifacts\\adminblazor\\wwwroot\\_framework (
                    echo ERROR: Blazor framework files missing.
                    exit /b 1
                )

                if exist HealthCareApp\\wwwroot\\blazor rmdir /S /Q HealthCareApp\\wwwroot\\blazor

                mkdir HealthCareApp\\wwwroot\\blazor

                xcopy /E /Y /I artifacts\\adminblazor\\wwwroot\\* HealthCareApp\\wwwroot\\blazor\\
                if errorlevel 1 exit /b 1

                if not exist HealthCareApp\\wwwroot\\blazor\\index.html (
                    echo ERROR: Blazor index.html missing after copy.
                    exit /b 1
                )
                '''
            }
        }

        stage('Create Blazor Fallback Files') {
            steps {
                powershell '''
                $ErrorActionPreference = "Stop"

                $frameworkPath = ".\\HealthCareApp\\wwwroot\\blazor\\_framework"

                if (!(Test-Path $frameworkPath)) {
                    throw "Blazor _framework folder missing."
                }

                $blazorJs = Get-ChildItem $frameworkPath -Filter "blazor.webassembly*.js" |
                    Where-Object { $_.Name -ne "blazor.webassembly.js" } |
                    Select-Object -First 1

                if ($null -ne $blazorJs) {
                    Copy-Item $blazorJs.FullName "$frameworkPath\\blazor.webassembly.js" -Force
                }

                $bootJson = Get-ChildItem $frameworkPath -Filter "blazor.boot*.json" |
                    Where-Object { $_.Name -ne "blazor.boot.json" } |
                    Select-Object -First 1

                if ($null -ne $bootJson) {
                    Copy-Item $bootJson.FullName "$frameworkPath\\blazor.boot.json" -Force
                }

                if (!(Test-Path "$frameworkPath\\blazor.webassembly.js")) {
                    throw "blazor.webassembly.js missing."
                }
                '''
            }
        }

        stage('Publish API') {
            steps {
                bat '''
                dotnet publish HealthCareApp\\HealthCareApp.csproj -c Release -o publish --no-restore --self-contained false
                if errorlevel 1 exit /b 1
                '''
            }
        }

        stage('Prepare Elastic Beanstalk Package') {
            steps {
                bat '''
                echo web: dotnet HealthCareApp.dll> publish\\Procfile

                if not exist publish\\Procfile exit /b 1
                if not exist publish\\HealthCareApp.dll exit /b 1
                if not exist publish\\HealthCareApp.runtimeconfig.json exit /b 1
                if not exist publish\\HealthCareApp.deps.json exit /b 1
                if not exist publish\\wwwroot\\angular\\index.html exit /b 1
                if not exist publish\\wwwroot\\blazor\\index.html exit /b 1

                type publish\\Procfile
                '''
            }
        }

        stage('Create Deployment Zip') {
            steps {
                dir('publish') {
                    bat '''
                    jar -cMf ..\\deploy-package.zip .
                    if errorlevel 1 exit /b 1
                    '''
                }

                bat '''
                if not exist deploy-package.zip exit /b 1

                jar -tf deploy-package.zip | findstr /I "Procfile"
                if errorlevel 1 exit /b 1

                jar -tf deploy-package.zip | findstr /I "HealthCareApp.dll"
                if errorlevel 1 exit /b 1

                jar -tf deploy-package.zip | findstr /I "wwwroot/angular/index.html"
                if errorlevel 1 exit /b 1

                jar -tf deploy-package.zip | findstr /I "wwwroot/blazor/index.html"
                if errorlevel 1 exit /b 1
                '''
            }
        }

        stage('Upload Package to S3') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    bat '''
                    aws s3 cp deploy-package.zip s3://%S3_BUCKET%/deploy-package-%BUILD_NUMBER%.zip --region %AWS_REGION%
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }

        stage('Create Application Version') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    bat '''
                    aws elasticbeanstalk create-application-version --application-name "%EB_APPLICATION_NAME%" --version-label "v-%BUILD_NUMBER%" --description "Jenkins build %BUILD_NUMBER%" --source-bundle S3Bucket=%S3_BUCKET%,S3Key=deploy-package-%BUILD_NUMBER%.zip --region %AWS_REGION%
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }

        stage('Deploy to Elastic Beanstalk') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    bat '''
                    aws elasticbeanstalk update-environment --environment-name "%EB_ENVIRONMENT_NAME%" --version-label "v-%BUILD_NUMBER%" --region %AWS_REGION%
                    if errorlevel 1 exit /b 1
                    '''
                }
            }
        }

        stage('Wait for EB Ready') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    powershell '''
                    $ErrorActionPreference = "Stop"

                    $expectedVersion = "v-$env:BUILD_NUMBER"
                    $maxChecks = 60
                    $delaySeconds = 15

                    for ($i = 1; $i -le $maxChecks; $i++) {
                        $result = aws elasticbeanstalk describe-environments --environment-names $env:EB_ENVIRONMENT_NAME --region $env:AWS_REGION --output json | ConvertFrom-Json
                        $envData = $result.Environments[0]

                        $status = $envData.Status
                        $health = $envData.Health
                        $version = $envData.VersionLabel
                        $cname = $envData.CNAME

                        Write-Host "Check $i"
                        Write-Host "Status: $status"
                        Write-Host "Health: $health"
                        Write-Host "Version: $version"

                        if ($status -eq "Ready" -and $version -eq $expectedVersion) {
                            Write-Host "Elastic Beanstalk is running expected version."

                            $healthUrl = "http://$cname/health"
                            Write-Host "Checking $healthUrl"

                            try {
                                $response = Invoke-WebRequest -Uri $healthUrl -UseBasicParsing -TimeoutSec 20
                                Write-Host "Health endpoint returned HTTP $($response.StatusCode)"
                            }
                            catch {
                                Write-Host "Health endpoint check failed: $($_.Exception.Message)"
                            }

                            exit 0
                        }

                        Start-Sleep -Seconds $delaySeconds
                    }

                    throw "Elastic Beanstalk did not reach Ready state with expected version."
                    '''
                }
            }
        }
    }

    post {
        success {
            echo 'HealthAxis Jenkins deployment completed.'
        }

        failure {
            echo 'HealthAxis Jenkins deployment failed. Check console output and EB logs.'
        }

        always {
            archiveArtifacts artifacts: 'deploy-package.zip', allowEmptyArchive: true
        }
    }
}
