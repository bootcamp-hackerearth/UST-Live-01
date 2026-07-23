pipeline {
    agent any

    environment {
        AWS_REGION = 'ap-south-2'

        // IMPORTANT:
        // This is Elastic Beanstalk Application name, not environment URL.
        // If your EB application name is different, change only this value.
        EB_APPLICATION_NAME = 'HealthCareApp'

        // This is your Elastic Beanstalk environment name.
        EB_ENVIRONMENT_NAME = 'HealthCareApp-dev'

        // Your Jenkins deployment bucket.
        S3_BUCKET = 'healthaxis-jenkins-bucket-847814614822-ap-south-2-an'

        DEPLOY_PACKAGE = 'deploy-package.zip'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
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

        stage('Restore API packages') {
            steps {
                bat 'dotnet restore HealthCareApp.sln'
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
