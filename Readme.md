HealthAxis

Overview

HealthAxis is an appointment booking portal developed using ASP.NET Core Web API, Angular, and Blazor. The application streamlines healthcare operations by enabling patients to book appointments, doctors to manage schedules and health records, and administrators to monitor and manage the overall system.

The solution follows a layered architecture consisting of Presentation, Service, and Repository layers, ensuring maintainability, scalability, and separation of concerns. Appointment scheduling includes validation and time-slot conflict prevention, while health records provide centralized medical history management.

Features

• Patient Portal (Angular)
o Patient registration and login
o Book appointments with doctors
o View appointment history
o Access personal health records
o Manage profile information

• Doctor Portal (Angular)
o View and manage appointments
o Update patient health records
o Track consultation history
o Manage availability and schedules

• Admin Portal (Blazor)
o Manage doctors and patients
o Monitor appointments
o Manage system operations
o Access administrative dashboards

• Backend Services (ASP.NET Core Web API)
o RESTful API endpoints
o Business validations
o Appointment conflict prevention
o Health record management
o Secure data handling

Technology Stack

• Backend
o ASP.NET Core Web API
o Entity Framework Core
o SQL Server

• Frontend
o Angular (Patient \& Doctor Portals)
o Blazor (Admin Portal)

• DevOps \& Cloud
o GitHub
o Jenkins CI/CD
o AWS Elastic Beanstalk
o AWS EC2
o AWS S3

Architecture

HealthAxis
│
├── Angular Patient Portal
├── Angular Doctor Portal
├── Blazor Admin Portal
│
├── ASP.NET Core Web API
│    ├── Controllers
│    ├── Services
│    ├── Repositories
│    └── Models
│
├── SQL Server Database
│
├── Jenkins CI/CD
│
└── AWS Deployment
├── Elastic Beanstalk
├── EC2 Instances
└── S3 Bucket

Local Setup

Prerequisites
• Visual Studio 2026
• .NET SDK
• Node.js
• Angular CLI
• SQL Server Management Studio (SSMS)

Steps to Run
• Clone or download the repository.
• Open the solution (.sln/.slnx) in Visual Studio.
• Restore NuGet packages.
• Install Angular dependencies:
o npm install		
• Execute the SQL script generated from SSMS to create the database.
• Configure the connection string in appsettings.json.
• Set the following as startup projects:
o ASP.NET Core Web API
o Angular Frontend
o Blazor Admin Portal
• Build the solution.
• Run the application using F5 or Ctrl + F5.

AWS Deployment

HealthAxis is deployed on AWS using a fully automated CI/CD pipeline.

Deployment Workflow
• Developers push code changes to GitHub.
• Jenkins automatically detects repository changes.
• The CI/CD pipeline starts automatically and builds the application.
• Build artifacts are generated and uploaded to an Amazon S3 Bucket.
• Jenkins deploys the latest build to AWS Elastic Beanstalk.
• Elastic Beanstalk updates the application running on Amazon EC2 instances.
• The latest version of the application becomes available without manual deployment.

Database Deployment
• Database scripts are generated using SQL Server Management Studio (SSMS).
• SQL scripts are stored in an Amazon S3 Bucket.
• Scripts are executed when database setup or updates are required.

CI/CD Pipeline

GitHub
│
▼
Jenkins Trigger
│
▼
Build \& Publish
│
▼
Amazon S3
│
▼
Elastic Beanstalk
│
▼
EC2 Instance
│
▼
Application Live

Key Highlights
• Multi-frontend architecture using Angular and Blazor
• ASP.NET Core Web API backend
• Appointment scheduling with conflict prevention
• Health record management
• Layered architecture for maintainability
• Automated CI/CD using Jenkins
• AWS deployment using Elastic Beanstalk and EC2
• Artifact and database script storage in Amazon S3
• GitHub integration for continuous deployment

