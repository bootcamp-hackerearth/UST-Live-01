pipeline {
    agent any

    environment {
        AWS_REGION = 'ap-south-1'
        EB_APPLICATION_NAME = 'EmployeeAppApi-01'
        EB_ENVIRONMENT_NAME = 'EmployeeAppApi-01-dev'
        S3_BUCKET = 'employeeapp-deploy-reni-2026-574521703934-ap-south-1-an'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build Angular') {
            steps {
                dir('EmployeeApp.NgClient') {
                    bat 'npm ci'
                    bat 'npm run build'
                }
            }
        }

        stage('Publish Blazor') {
            steps {
                bat 'dotnet publish EmployeeClient\\EmployeeClient.csproj -c Release -o blazor-publish-temp'
            }
        }

        stage('Copy Blazor into API wwwroot') {
            steps {
                bat 'if not exist EmployeeApp.Api\\wwwroot\\blazor mkdir EmployeeApp.Api\\wwwroot\\blazor'
                bat 'xcopy /E /Y /I blazor-publish-temp\\wwwroot\\* EmployeeApp.Api\\wwwroot\\blazor\\'
            }
        }

        stage('Publish API') {
            steps {
                bat 'dotnet publish EmployeeApp.Api\\EmployeeApp.Api.csproj -c Release -o publish'
            }
        }

        stage('Zip published output') {
    steps {
        dir('publish') {
            bat 'jar -cMf ../deploy-package.zip .'
        }
    }
}

        stage('Upload to S3 and Deploy to EB') {
            steps {
                withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'aws-deploy-creds']]) {
                    bat "aws s3 cp deploy-package.zip s3://%S3_BUCKET%/deploy-package-%BUILD_NUMBER%.zip --region %AWS_REGION%"

                    bat "aws elasticbeanstalk create-application-version --application-name %EB_APPLICATION_NAME% --version-label v-%BUILD_NUMBER% --source-bundle S3Bucket=%S3_BUCKET%,S3Key=deploy-package-%BUILD_NUMBER%.zip --region %AWS_REGION%"

                    bat "aws elasticbeanstalk update-environment --environment-name %EB_ENVIRONMENT_NAME% --version-label v-%BUILD_NUMBER% --region %AWS_REGION%"
                }
            }
        }
    }
}