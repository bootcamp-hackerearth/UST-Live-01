pipeline {
    agent any

    environment {
        // AWS configuration
        AWS_REGION = 'ap-south-2'

        // Replace these with your Elastic Beanstalk details
        EB_APPLICATION_NAME = 'HealthAxisAPI'
        EB_ENVIRONMENT_NAME = 'HealthAxisAPI-dev'

        // Replace with your actual S3 bucket name
        S3_BUCKET = 'jenkins-bucket-379992420468-ap-south-2-an '
    }

    stages {

        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Clean Previous Build') {
            steps {
                bat '''
                    if exist publish rmdir /S /Q publish
                    if exist blazor-publish-temp rmdir /S /Q blazor-publish-temp
                    if exist deploy-package.zip del /F /Q deploy-package.zip
                    if exist HealthAxis.UI\\dist rmdir /S /Q HealthAxis.UI\\dist
                '''
            }
        }

        stage('Restore .NET Projects') {
            steps {
                bat 'dotnet restore HealthAxis.API\\HealthAxis.API.csproj'
                bat 'dotnet restore HealthAxis.Admin\\HealthAxis.Admin.csproj'
            }
        }

        stage('Build Angular') {
            steps {
                dir('HealthAxis.UI') {
                    bat 'call npm ci'
                    bat 'call npm run build -- --configuration production'
                }
            }
        }

        stage('Copy Angular into API wwwroot') {
            steps {
                bat '''
                    if not exist HealthAxis.API\\wwwroot (
                        mkdir HealthAxis.API\\wwwroot
                    )

                    for /d %%D in (HealthAxis.UI\\dist\\*) do (
                        if exist "%%D\\browser" (
                            echo Copying Angular browser build from %%D\\browser
                            xcopy /E /Y /I "%%D\\browser\\*" "HealthAxis.API\\wwwroot\\"
                        ) else (
                            echo Copying Angular build from %%D
                            xcopy /E /Y /I "%%D\\*" "HealthAxis.API\\wwwroot\\"
                        )
                    )
                '''
            }
        }

        stage('Publish Blazor Admin') {
            steps {
                bat '''
                    dotnet publish HealthAxis.Admin\\HealthAxis.Admin.csproj ^
                    -c Release ^
                    -o blazor-publish-temp
                '''
            }
        }

        stage('Copy Blazor into API wwwroot') {
            steps {
                bat '''
                    if not exist HealthAxis.API\\wwwroot\\admin (
                        mkdir HealthAxis.API\\wwwroot\\admin
                    )

                    if exist blazor-publish-temp\\wwwroot (
                        xcopy /E /Y /I ^
                        blazor-publish-temp\\wwwroot\\* ^
                        HealthAxis.API\\wwwroot\\admin\\
                    ) else (
                        echo ERROR: Blazor wwwroot folder was not found.
                        exit /b 1
                    )
                '''
            }
        }

        stage('Publish API') {
            steps {
                bat '''
                    dotnet publish HealthAxis.API\\HealthAxis.API.csproj ^
                    -c Release ^
                    -o publish
                '''
            }
        }

        stage('Create Deployment Zip') {
            steps {
                dir('publish') {
                    bat 'jar -cMf ../deploy-package.zip .'
                }
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
                        aws s3 cp deploy-package.zip ^
                        s3://%S3_BUCKET%/deploy-package-%BUILD_NUMBER%.zip ^
                        --region %AWS_REGION%
                    '''

                    bat '''
                        aws elasticbeanstalk create-application-version ^
                        --application-name "%EB_APPLICATION_NAME%" ^
                        --version-label "v-%BUILD_NUMBER%" ^
                        --source-bundle S3Bucket=%S3_BUCKET%,S3Key=deploy-package-%BUILD_NUMBER%.zip ^
                        --region %AWS_REGION%
                    '''

                    bat '''
                        aws elasticbeanstalk update-environment ^
                        --environment-name "%EB_ENVIRONMENT_NAME%" ^
                        --version-label "v-%BUILD_NUMBER%" ^
                        --region %AWS_REGION%
                    '''
                }
            }
        }
    }

    post {
        success {
            echo 'HealthAxis deployment started successfully.'
        }

        failure {
            echo 'HealthAxis deployment failed. Check the Jenkins Console Output.'
        }

        always {
            archiveArtifacts(
                artifacts: 'deploy-package.zip',
                allowEmptyArchive: true
            )
        }
    }
}
