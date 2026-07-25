name: Build and Deploy to Elastic Beanstalk

on:
  workflow_dispatch:
  push:
    branches:
      - sprint5

env:
  AWS_REGION: ap-south-2
  EB_APPLICATION_NAME: HealthAxisApi-dev
  EB_ENVIRONMENT_NAME: HealthAxisApi-dev
  S3_BUCKET: my-jenkins-bucket-945125812699-ap-south-2-an

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: '20'

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'

      # Build Angular
      - name: Build Angular app
        working-directory: ./HealthAxis.Angular
        run: |
          npm ci
          npm run build

      # Publish Blazor WASM
      - name: Publish Blazor client
        run: |
          dotnet publish HealthAxis.Admin/HealthAxis.Admin.csproj \
          -c Release \
          -o ./blazor-publish-temp

      # Copy Blazor files to API wwwroot
      - name: Copy Blazor output into API wwwroot
        run: |
          mkdir -p ./HealthAxis.Api/wwwroot/blazor
          cp -r ./blazor-publish-temp/wwwroot/* ./HealthAxis.Api/wwwroot/blazor/

      # Copy Angular dist into API wwwroot
      - name: Copy Angular output into API wwwroot
        run: |
          mkdir -p ./HealthAxis.Api/wwwroot/angular
          cp -r ./HealthAxis.Angular/dist/* ./HealthAxis.Api/wwwroot/angular/ || true

      # Publish API
      - name: Publish API
        run: |
          dotnet publish HealthAxis.Api/HealthAxis.Api.csproj \
          -c Release \
          -o ./publish

      # Verify publish output
      - name: Verify Publish Output
        run: |
          echo "----- Publish Folder -----"
          ls -la ./publish
          echo "----- Runtime Config Files -----"
          find ./publish -name "*.runtimeconfig.json"

      # Verify Procfile exists
      - name: Verify Procfile
        run: |
          cat ./publish/Procfile

      # Package deployment
      - name: Zip deployment package
        working-directory: ./publish
        run: zip -r ../deploy-package.zip .

      # Configure AWS
      - name: Configure AWS credentials
        uses: aws-actions/configure-aws-credentials@v4
        with:
          aws-access-key-id: ${{ secrets.AWS_ACCESS_KEY_ID }}
          aws-secret-access-key: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
          aws-region: ${{ env.AWS_REGION }}

      # Upload to S3
      - name: Upload package to S3
        run: |
          aws s3 cp deploy-package.zip \
          s3://${{ env.S3_BUCKET }}/deploy-package-${{ github.run_number }}.zip

      # Create EB version
      - name: Create Elastic Beanstalk Version
        run: |
          aws elasticbeanstalk create-application-version \
            --application-name "${{ env.EB_APPLICATION_NAME }}" \
            --version-label "gha-${{ github.run_number }}" \
            --source-bundle \
            S3Bucket="${{ env.S3_BUCKET }}",S3Key="deploy-package-${{ github.run_number }}.zip"

      # Deploy
      - name: Deploy to Elastic Beanstalk
        run: |
          aws elasticbeanstalk update-environment \
            --environment-name "${{ env.EB_ENVIRONMENT_NAME }}" \
            --version-label "gha-${{ github.run_number }}"