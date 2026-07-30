# HealthCare Management System

The HealthCare Management System is a role-based web application for patients, doctors, and administrators. It uses Angular for the patient and doctor portals, Blazor WebAssembly for the administrator portal, and ASP.NET Core Web API for authentication, business logic, messaging, and data access.

## Application URLs

After starting the application, use the following routes:

```text
/          Main entry point; redirects to Angular
/angular   Patient and doctor portal
/blazor    Administrator portal
/api       ASP.NET Core API endpoints
```

## Main Features

### Patient

- Register a new patient account
- Log in using email and password
- View the patient dashboard
- View and edit the patient profile
- Book appointments with doctors
- View upcoming and previous appointments
- View health and medical records
- Log out securely

### Doctor

- Log in using doctor credentials
- View the doctor dashboard
- View and manage appointments
- Confirm, complete, or cancel appointments
- View patient-related information
- Manage doctor availability and leave
- Log out securely

### Administrator

- Log in through the common Angular login portal
- Redirect to the Blazor administrator portal
- View dashboard statistics
- View total, active, and inactive doctors
- View patients and appointments
- Add and manage doctors
- Manage patient and appointment information
- Log out and return to the Angular login portal

## Technology Stack

### Frontend

- Angular
- TypeScript
- Reactive Forms
- Bootstrap and Bootstrap Icons
- Blazor WebAssembly
- Razor Components

### Backend

- ASP.NET Core Web API
- C#
- Entity Framework Core
- ASP.NET Core Identity
- JWT Bearer Authentication
- AutoMapper
- Repository and Service patterns
- Global exception handling
- Swagger/OpenAPI
- Serilog

### Database and Messaging

- Microsoft SQL Server
- RabbitMQ
- MassTransit

### DevOps and Cloud

- GitHub
- Jenkins CI/CD
- Sonar code-quality analysis
- Amazon EC2
- Amazon S3
- AWS Elastic Beanstalk
- AWS IAM
- Amazon VPC
- EC2 security groups

## Solution Structure

```text
HealthCare/
├── HealthCare.Api/
│   ├── Consumers/
│   ├── Controllers/
│   ├── Data/
│   ├── Exceptions/
│   ├── Mapping/
│   ├── Middleware/
│   ├── Models/
│   ├── Repositories/
│   ├── Services/
│   ├── wwwroot/
│   │   ├── angular/
│   │   └── blazor/
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── appsettings.Production.json
│   └── Program.cs
│
├── HealthCare.UsersAngular/
│   ├── src/
│   │   ├── app/
│   │   └── index.html
│   ├── angular.json
│   ├── package.json
│   └── package-lock.json
│
├── HealthCare.AdminBlazor/
│   ├── Components/
│   ├── Services/
│   ├── wwwroot/
│   ├── App.razor
│   ├── Program.cs
│   └── HealthCare.AdminBlazor.csproj
│
├── HealthCare.Shared/
│   ├── DTOs/
│   ├── Contracts/
│   └── HealthCare.Shared.csproj
│
└── Jenkinsfile
```

Folder names can vary slightly depending on the current solution structure.

## Architecture

The application follows a layered architecture:

```text
Angular or Blazor frontend
          |
          v
ASP.NET Core API controllers
          |
          v
Service layer
          |
          v
Repository layer
          |
          v
Entity Framework Core
          |
          v
SQL Server
```

### Layer Responsibilities

- Controllers receive HTTP requests and return HTTP responses.
- Services contain application and business logic.
- Repositories perform database operations.
- Entity Framework Core maps application entities to SQL Server tables.
- DTOs transfer data safely between the API and frontend applications.
- AutoMapper maps entities and DTOs.

## Authentication and Authorization

The application uses ASP.NET Core Identity and JWT authentication.

Authentication flow:

```text
User enters email and password
          |
          v
API validates credentials
          |
          v
API creates a JWT access token
          |
          v
Frontend stores the token
          |
          v
Frontend sends the token with protected API requests
```

Protected requests use the following header:

```http
Authorization: Bearer <access-token>
```

Application roles include:

```text
Admin
Doctor
Patient
```

ASP.NET Core Identity and JWT protect application features. AWS IAM separately controls access to AWS resources and deployment operations.

## Database

SQL Server stores persistent application data, including:

- Identity users and roles
- Patients
- Doctors
- Appointments
- Health records
- Doctor leave and availability information

The API obtains the database connection string using:

```text
ConnectionStrings:HealthCareDbConnection
```

### Development Configuration Example

```json
{
  "ConnectionStrings": {
    "HealthCareDbConnection": "Server=localhost,1433;Database=HealthCareDb;User Id=<SQL_USER>;Password=<SQL_PASSWORD>;Encrypt=True;TrustServerCertificate=True"
  }
}
```

### Production Configuration Example

```json
{
  "ConnectionStrings": {
    "HealthCareDbConnection": "Server=<SQL_PRIVATE_IP>,1433;Database=HealthCareDb;User Id=<SQL_USER>;Password=<SQL_PASSWORD>;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
  },
  "Jwt": {
    "Issuer": "HealthCare.Api",
    "Audience": "HealthCare.Api",
    "Key": "<JWT_SIGNING_KEY>",
    "AccessTokenExpirationMinutes": 15
  },
  "SeedData": {
    "AdminEmail": "<ADMIN_EMAIL>",
    "AdminPassword": "<ADMIN_PASSWORD>"
  }
}
```

Do not commit real database passwords, JWT keys, RabbitMQ passwords, AWS access keys, or administrator passwords to source control.

## RabbitMQ Messaging

RabbitMQ supports asynchronous appointment-related processing.

```text
Appointment booked
        |
        v
API saves appointment
        |
        v
API publishes a message
        |
        v
RabbitMQ queue stores the message
        |
        v
AppointmentBookedConsumer processes the message
```

RabbitMQ configuration uses:

```json
{
  "RabbitMq": {
    "HostName": "<RABBITMQ_HOST>",
    "Port": 5672,
    "UserName": "<RABBITMQ_USER>",
    "Password": "<RABBITMQ_PASSWORD>",
    "VirtualHost": "/",
    "HealthCareQueue": "HealthCare.appointment.booked.queue"
  }
}
```

SQL Server stores permanent data. RabbitMQ transports messages between application processes.

## Prerequisites

Install the following tools before running the complete solution locally:

- Git
- .NET SDK matching the project target framework
- Node.js and npm
- Angular CLI
- Microsoft SQL Server
- SQL Server Management Studio or another SQL client
- RabbitMQ Server
- Visual Studio or Visual Studio Code

For CI/CD and AWS deployment, also install or configure:

- Jenkins
- Java Development Kit
- AWS CLI
- SonarQube or SonarCloud integration
- Jenkins AWS Credentials plugin

## Local Setup

### 1. Clone the Repository

```bash
git clone <REPOSITORY_URL>
cd HealthCare
```

### 2. Configure SQL Server

Create the `HealthCareDb` database and execute the supplied database schema and seed-data script.

Verify that SQL Server:

- Is running
- Has TCP/IP enabled
- Is listening on port 1433
- Accepts the configured SQL credentials

### 3. Configure RabbitMQ

Start RabbitMQ and create the required application user and permissions.

Verify that RabbitMQ is listening on:

```text
5672
```

### 4. Configure API Settings

Update the local development settings in:

```text
HealthCare.Api/appsettings.Development.json
```

Provide local values for:

- SQL Server connection string
- JWT issuer, audience, and key
- Seed administrator details

Configure RabbitMQ in:

```text
HealthCare.Api/appsettings.json
```

### 5. Restore and Build the API

```bash
cd HealthCare.Api
dotnet restore
dotnet build
```

### 6. Build Angular

```bash
cd ../HealthCare.UsersAngular
npm install
npm run build
```

The Angular build output must be available under:

```text
HealthCare.Api/wwwroot/angular
```

### 7. Publish Blazor

```bash
cd ../HealthCare.AdminBlazor
dotnet publish -c Release -o ../../blazor-publish-temp
```

Copy the contents of:

```text
blazor-publish-temp/wwwroot
```

into:

```text
HealthCare.Api/wwwroot/blazor
```

The folder name must remain lowercase because Linux paths are case-sensitive.

### 8. Run the API

```bash
cd ../HealthCare.Api
dotnet run --launch-profile http
```

Open:

```text
http://localhost:5133/
http://localhost:5133/angular
http://localhost:5133/blazor
```

## Same-Origin API Configuration

Angular and Blazor should call the backend using relative API paths:

```text
/api/auth
/api/patients
/api/doctors
/api/appointments
/api/records
```

Do not hardcode local development URLs such as:

```text
https://localhost:7206/api
```

Using relative URLs allows the same frontend build to work locally and after AWS deployment.

The Blazor `HttpClient` is configured to use the website root as its API base address, while Blazor itself remains hosted under `/blazor`.

## Jenkins CI/CD Pipeline

The Jenkins pipeline automates the following process:

```text
Checkout source code
        |
        v
Run Sonar analysis
        |
        v
Build Angular
        |
        v
Publish Blazor
        |
        v
Copy frontend artifacts into API wwwroot
        |
        v
Publish ASP.NET Core API
        |
        v
Verify package contents
        |
        v
Create deployment ZIP
        |
        v
Upload ZIP to S3
        |
        v
Create Elastic Beanstalk application version
        |
        v
Update the Elastic Beanstalk environment
```

The deployment ZIP contains:

```text
HealthCare.Api.dll
appsettings.json
appsettings.Production.json
wwwroot/
├── angular/
└── blazor/
```

Each package uses the Jenkins build number to create a unique S3 object and Elastic Beanstalk version label.

## AWS Architecture

The AWS deployment uses the following services:

- EC2 hosts SQL Server, RabbitMQ, Jenkins, and Elastic Beanstalk application infrastructure.
- S3 stores database scripts and application deployment ZIP files.
- Elastic Beanstalk deploys and manages the ASP.NET Core application environment.
- IAM grants Jenkins controlled permission to upload to S3 and deploy application versions.
- VPC provides private communication among application, database, and messaging resources.
- Security groups control access to application, database, RabbitMQ, and administration ports.

### AWS Deployment Flow

```text
Developer pushes code to GitHub
           |
           v
Jenkins builds the solution
           |
           v
Jenkins creates a deployment ZIP
           |
           v
Jenkins uploads the ZIP to S3
           |
           v
Jenkins creates an Elastic Beanstalk version
           |
           v
Elastic Beanstalk downloads the ZIP
           |
           v
Application runs on Elastic Beanstalk EC2 infrastructure
           |
           +----> SQL Server on EC2, port 1433
           |
           +----> RabbitMQ on EC2, port 5672
```

## Required AWS Security Rules

Recommended backend inbound rules:

```text
SQL Server
Protocol: TCP
Port: 1433
Source: Elastic Beanstalk EC2 security group

RabbitMQ
Protocol: TCP
Port: 5672
Source: Elastic Beanstalk EC2 security group

Windows administration, when required
Protocol: TCP
Port: 3389
Source: Authorized administrator IP only
```

Do not expose SQL Server or RabbitMQ to `0.0.0.0/0`.

## Logging and Troubleshooting

Serilog writes structured application and request logs.

In Elastic Beanstalk, useful logs include:

```text
/var/log/web.stdout.log
/var/log/eb-engine.log
/var/log/nginx/access.log
/var/log/nginx/error.log
```

### 502 Bad Gateway

A 502 response usually means nginx is running but the ASP.NET Core application process is unavailable.

Common causes:

- Missing production configuration
- SQL Server connection timeout
- Application startup failure
- Invalid JWT configuration
- Missing application files

### SQL Server Timeout

Verify:

- Correct SQL Server private IP
- Both instances have a valid VPC network path
- TCP port 1433 is allowed from the Elastic Beanstalk EC2 security group
- Windows Firewall permits TCP 1433
- SQL Server TCP/IP is enabled
- SQL Server is listening on port 1433

### Blazor Static Files Return 404

Verify that the published API contains:

```text
wwwroot/blazor/index.html
wwwroot/blazor/_framework
wwwroot/blazor/css
```

Use lowercase `blazor` consistently on Linux.

### Angular Static Files Return 404

Verify that the published API contains:

```text
wwwroot/angular/index.html
```

Rebuild Angular before publishing the API.

## Security Notes

- Never commit production secrets to Git.
- Use strong, rotated credentials.
- Prefer IAM roles with temporary credentials for AWS workloads.
- Restrict SQL Server and RabbitMQ ports to approved security groups.
- Use HTTPS for production traffic.
- Avoid sending JWT access tokens in query strings in a production system.
- Protect administrator seed credentials.
- Keep SQL Server, RabbitMQ, EC2 operating systems, and dependencies updated.

## Future Improvements

- Store secrets in AWS Secrets Manager or Systems Manager Parameter Store
- Migrate SQL Server from EC2 to Amazon RDS
- Use a managed message broker where appropriate
- Add automated unit and integration tests
- Add CloudWatch dashboards and alerts
- Configure HTTPS using AWS Certificate Manager and a load balancer
- Add automated deployment health verification and rollback
- Deploy across multiple Availability Zones
- Improve backup and disaster-recovery procedures

## Presentation Summary

The HealthCare Management System combines Angular, Blazor WebAssembly, ASP.NET Core, SQL Server, RabbitMQ, Jenkins, Sonar, and AWS services into one role-based healthcare platform. Jenkins automates the build and deployment process, S3 stores deployment artifacts, Elastic Beanstalk manages the application environment, and EC2 provides the underlying compute resources. The VPC and security groups provide controlled private communication between the application, SQL Server, and RabbitMQ. JWT and ASP.NET Core Identity protect application endpoints and enforce Admin, Doctor, and Patient roles.

## License

This project is intended for educational, demonstration, and internal project purposes. Add an appropriate license before public or commercial distribution.
