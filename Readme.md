# HealthAxis - Healthcare Management Platform

HealthAxis is a full-stack healthcare management platform built with ASP.NET Core Web API, Angular, Blazor WebAssembly, Entity Framework Core, SQL Server, RabbitMQ, Serilog, Elasticsearch/Kibana, AWS Elastic Beanstalk, EC2, S3, GitHub, and Jenkins CI/CD.

The application supports role-based workflows for Patients, Doctors, and Admin users. It includes patient registration, secure JWT login, doctor onboarding by Admin, appointment booking, doctor availability, appointment status management, health record management, messaging/notification flow, structured logging, and automated cloud deployment.

---

## Table of Contents

1. [Project Overview](#project-overview)
2. [Modules](#modules)
3. [Technology Stack](#technology-stack)
4. [Architecture Overview](#architecture-overview)
5. [Prerequisites](#prerequisites)
6. [Local Setup and Run Procedure](#local-setup-and-run-procedure)
7. [Frontend Build Procedure](#frontend-build-procedure)
8. [Jenkins CI/CD Deployment](#jenkins-cicd-deployment)
9. [AWS Deployment Overview](#aws-deployment-overview)
10. [Environment Variables and Secrets](#environment-variables-and-secrets)
11. [Logging and Monitoring](#logging-and-monitoring)
12. [Common Troubleshooting](#common-troubleshooting)
13. [AI Usage Disclosure](#ai-usage-disclosure)
14. [Project Status](#project-status)

---

## Project Overview

HealthAxis was developed as a healthcare management system with separate user experiences for Patients, Doctors, and Admins.

The backend is built using ASP.NET Core Web API and follows a layered architecture with DTOs, services, repositories, Entity Framework Core, AutoMapper, JWT authentication, role-based authorization, and global exception handling.

The Patient and Doctor portals are built with Angular, while the Admin portal is built using Blazor WebAssembly. For deployment, Angular and Blazor static files are placed inside the API `wwwroot` folder so that the full application can be deployed as a single Elastic Beanstalk application package.

---

## Modules

### Patient Portal - Angular

Patient users can:

- Register and log in securely
- View their dashboard
- Book appointments with active doctors
- View appointments with filters and pagination
- Cancel appointments based on business rules
- View health records
- Update profile information
- Change password

### Doctor Portal - Angular

Doctor users can:

- Log in securely
- Complete first-time password change flow if required
- View dashboard summary
- View appointment lists
- Confirm pending appointments
- Add health records for confirmed appointments
- Complete appointments after health record creation
- View patient health record history
- Change password

### Admin Portal - Blazor WebAssembly

Admin users can:

- Access a protected Admin dashboard
- View dashboard metrics
- Manage doctors
- Add doctors and view generated temporary password
- Edit doctor details
- Activate/deactivate doctors
- View patients
- View appointments
- Use toast notifications and HealthAxis-themed UI

### Backend API - ASP.NET Core

The backend provides APIs for:

- Authentication and JWT token generation
- Patient registration and profile management
- Doctor management
- Appointment booking and status changes
- Health record creation and retrieval
- Admin dashboard reporting
- Role-based access control
- Business validation
- Global exception handling
- RabbitMQ-based messaging/notification flow

---

## Technology Stack

### Backend

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Authentication
- Role-Based Authorization
- AutoMapper
- Repository Pattern
- Service Layer Pattern
- Global Exception Handling
- Swagger

### Frontend

- Angular
- TypeScript
- HTML
- CSS
- Blazor WebAssembly
- Bootstrap
- JavaScript Interop

### Messaging, Logging, and Observability

- RabbitMQ
- Serilog
- Elasticsearch
- Kibana
- AWS CloudWatch / Elastic Beanstalk logs

### Cloud and DevOps

- AWS Elastic Beanstalk
- AWS EC2
- AWS S3
- AWS Secrets Manager
- Jenkins
- Git
- GitHub

### Caching

- Redis/Garnet-compatible caching exploration

---

## Architecture Overview

```text
Angular Patient Portal
        |
Angular Doctor Portal
        |
Blazor Admin Portal
        |
        v
ASP.NET Core Web API
        |
        +--> SQL Server using EF Core
        |
        +--> RabbitMQ for messaging
        |
        +--> Serilog logs
        |
        +--> Elasticsearch/Kibana
```

For deployment, frontend outputs are copied into the API project:

```text
HealthCareApp/wwwroot/angular
HealthCareApp/wwwroot/blazor
```

This allows a single deployment package to serve:

```text
/angular  -> Angular Patient/Doctor portal
/blazor   -> Blazor Admin portal
/api      -> ASP.NET Core APIs
/swagger  -> API documentation
/health   -> Health check endpoint
```

---

## Prerequisites

Install the following tools before running the project locally:

- .NET SDK
- Node.js
- npm
- SQL Server
- Visual Studio or Visual Studio Code
- Git
- RabbitMQ if testing messaging locally
- Optional: Elasticsearch and Kibana for logging
- Optional: Redis/Garnet for caching exploration
- Optional: AWS CLI for deployment-related testing

---

## Local Setup and Run Procedure

### 1. Clone the repository

```powershell
git clone https://github.com/bootcamp-hackerearth/UST-Live-01.git
cd UST-Live-01
```

Checkout the working branch:

```powershell
git checkout Feature/Sprint5_Pod1_RishabnathGorla
```

---

### 2. Configure backend settings

Create or update local configuration in `appsettings.Development.json` or use user secrets.

Example development configuration:

```json
{
  "ConnectionStrings": {
    "DbCon": "Server=localhost;Database=HealthAxisDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Issuer": "HealthCareApp",
    "Audience": "HealthCareApp",
    "Key": "YOUR_LOCAL_DEVELOPMENT_JWT_KEY",
    "AccessTokenExpirationMinutes": 15
  },
  "RabbitMq": {
    "Host": "localhost",
    "Port": 5672,
    "VirtualHost": "/",
    "Username": "guest",
    "Password": "guest",
    "TestQueue": "healthaxis-test-queue",
    "AppointmentBookedQueue": "appointment-booked-queue"
  }
}
```

Do not commit real production secrets.

---

### 3. Restore backend packages

```powershell
dotnet restore HealthCareApp\HealthCareApp.csproj
```

Restore Blazor Admin packages:

```powershell
dotnet restore HealthCareApp.AdminBlazor\HealthCareApp.AdminBlazor.csproj
```

---

### 4. Apply database migrations

If EF Core CLI is available:

```powershell
dotnet ef database update --project HealthCareApp
```

If migrations are managed through Visual Studio Package Manager Console:

```powershell
Update-Database
```

---

### 5. Run the backend API

```powershell
dotnet run --project HealthCareApp
```

Swagger should be available at:

```text
https://localhost:<api-port>/swagger
```

Health check should be available at:

```text
https://localhost:<api-port>/health
```

---

### 6. Run Angular frontend locally

Navigate to Angular project:

```powershell
cd HealthCareApp.UI
```

Install packages:

```powershell
npm install
```

Run Angular locally:

```powershell
npm start
```

Typical Angular local URL:

```text
http://localhost:4200
```

---

### 7. Run Blazor Admin locally

From repository root:

```powershell
dotnet run --project HealthCareApp.AdminBlazor
```

Blazor Admin will run on the configured local Blazor URL.

---

## Frontend Build Procedure

Angular production build:

```powershell
cd HealthCareApp.UI
npx ng build --configuration production
```

The Angular output is configured to be placed under:

```text
HealthCareApp/wwwroot/angular
```

Publish Blazor Admin:

```powershell
dotnet publish HealthCareApp.AdminBlazor\HealthCareApp.AdminBlazor.csproj -c Release -o artifacts\adminblazor
```

Copy Blazor published output into API:

```powershell
mkdir HealthCareApp\wwwroot\blazor
xcopy /E /Y /I artifacts\adminblazor\wwwroot\* HealthCareApp\wwwroot\blazor\
```

A helper script can also be used:

```powershell
powershell -ExecutionPolicy Bypass -File .\build-frontends.ps1
```

---

## Jenkins CI/CD Deployment

The Jenkins pipeline automates the complete deployment flow.

### Jenkins pipeline steps

```text
1. Checkout latest code from GitHub
2. Clean previous generated files
3. Verify required project files
4. Restore .NET projects
5. Install Angular dependencies using npm ci
6. Build Angular production files
7. Publish Blazor Admin
8. Copy Blazor output into API wwwroot
9. Verify frontend files
10. Publish ASP.NET Core API
11. Create Elastic Beanstalk Procfile
12. Create deployment ZIP
13. Upload ZIP to S3
14. Create Elastic Beanstalk application version
15. Deploy version to Elastic Beanstalk environment
16. Wait until Elastic Beanstalk runs expected version
```

### Important Jenkins commands

```powershell
dotnet restore
npm ci
npx ng build --configuration production
dotnet publish
jar -cMf
aws s3 cp
aws elasticbeanstalk create-application-version
aws elasticbeanstalk update-environment
```

### Jenkins deployment package

The final ZIP contains:

```text
HealthCareApp.dll
HealthCareApp.runtimeconfig.json
HealthCareApp.deps.json
Procfile
wwwroot/angular
wwwroot/blazor
```

---

## AWS Deployment Overview

```text
GitHub
  |
  v
Jenkins
  |
  v
Build Angular + Publish Blazor + Publish API
  |
  v
Create deployment ZIP
  |
  v
Upload ZIP to S3
  |
  v
Create Elastic Beanstalk Application Version
  |
  v
Deploy to Elastic Beanstalk Environment
  |
  v
EC2 App Instance
  |
  +--> EC2 SQL Server
  |
  +--> EC2 RabbitMQ
```

### Production URLs

Replace `<elastic-beanstalk-url>` with the active Elastic Beanstalk URL.

```text
Angular Portal:
http://<elastic-beanstalk-url>/angular

Blazor Admin Portal:
http://<elastic-beanstalk-url>/blazor

Swagger:
http://<elastic-beanstalk-url>/swagger

Health Check:
http://<elastic-beanstalk-url>/health
```

---

## Environment Variables and Secrets

Production configuration should be managed using Elastic Beanstalk environment properties and AWS Secrets Manager.

### Non-sensitive values can be plain text

```text
ASPNETCORE_ENVIRONMENT=Production
RabbitMq__Host=<private-ip>
RabbitMq__Port=5672
RabbitMq__VirtualHost=/
RabbitMq__TestQueue=healthaxis-test-queue
RabbitMq__AppointmentBookedQueue=appointment-booked-queue
Jwt__Issuer=HealthCareApp
Jwt__Audience=HealthCareApp
Jwt__AccessTokenExpirationMinutes=15
```

### Sensitive values should use AWS Secrets Manager

```text
ConnectionStrings__DbCon
RabbitMq__Username
RabbitMq__Password
Jwt__Key
```

Recommended separate Secrets Manager secrets:

```text
healthaxis/prod/DbCon
healthaxis/prod/RabbitMqUsername
healthaxis/prod/RabbitMqPassword
healthaxis/prod/JwtKey
```

Elastic Beanstalk environment properties can then reference these Secrets Manager values.

---

## Logging and Monitoring

The project uses structured logging and cloud logs for debugging.

Tools:

- Serilog
- Elasticsearch
- Kibana
- Elastic Beanstalk logs
- CloudWatch logs

Useful log keywords:

```text
Exception
Error
RabbitMQ
MassTransit
AppointmentBooked
Consumer
Outbox
401
403
500
```

---

## Common Troubleshooting

### Jenkins: `ng` not recognized

Use:

```powershell
npx ng build --configuration production
```

### Jenkins: `npm ci` lock mismatch

Run locally:

```powershell
cd HealthCareApp.UI
npm install
```

Then commit updated lock file:

```powershell
git add package-lock.json package.json
git commit -m "Fix Angular package lock sync"
git push
```

### Elastic Beanstalk deployment failed

Check:

```text
Elastic Beanstalk -> Events
Elastic Beanstalk -> Logs -> Full logs
/var/log/eb-engine.log
```

### Clipboard copy button fails in deployed Blazor

The browser Clipboard API may fail on HTTP. A JavaScript fallback helper through Blazor JS Interop is used. The long-term production fix is enabling HTTPS for the Elastic Beanstalk URL.

### Browser shows old UI after deployment

Use hard refresh:

```text
Ctrl + Shift + R
```

or open the app in private/incognito mode.

---

## AI Usage Disclosure

AI tools were used as a supporting aid during this project for:

- Clarifying technical concepts
- Understanding Jenkins and AWS deployment errors
- Improving Code structure
- Reviewing possible troubleshooting approaches
- Revising Core Concepts

Implementation decisions, code changes, testing, deployment validation, and final verification were performed manually by the developer. AI was used as a learning and productivity assistant, not as a replacement for development ownership.

---

## Project Status

### Completed

- ASP.NET Core backend
- Angular Patient and Doctor portals
- Blazor WebAssembly Admin portal
- JWT authentication and role-based authorization
- EF Core with SQL Server
- RabbitMQ integration
- Serilog logging
- Jenkins CI/CD pipeline
- AWS Elastic Beanstalk deployment
- S3 deployment package flow
- EC2-hosted SQL Server and RabbitMQ
- Initial AWS Secrets Manager setup


## Summary

HealthAxis is a cloud-deployed healthcare management platform built with ASP.NET Core, Angular, Blazor, SQL Server, RabbitMQ, and AWS. It supports secure role-based workflows for Patients, Doctors, and Admins. The project demonstrates full-stack development, backend architecture, frontend integration, CI/CD automation, cloud deployment, messaging, logging, security, and production-readiness practices.
