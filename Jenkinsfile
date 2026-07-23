pipeline {
    agent any

    environment {
        AWS_REGION = 'ap-south-2'

        // Change this only if your Elastic Beanstalk application name is different
        EB_APPLICATION_NAME = 'HealthCareApp'

        // Your EB environment name
        EB_ENVIRONMENT_NAME = 'HealthCareApp-dev'

        // Your Jenkins deployment S3 bucket
        S3_BUCKET = 'healthaxis-jenkins-bucket-847814614822-ap-south-2-an'

        DEPLOY_PACKAGE = 'deploy-package.zip'
    }

    stages {
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

        stage('Build Angular and Blazor') {
            steps {
                bat 'powershell -ExecutionPolicy Bypass -File .\\build-frontends.ps1'
            }
        }

        stage('Publish API') {
            steps {
                bat 'dotnet publish HealthCareApp\\HealthCareApp.csproj -c Release -o publish'
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
