Healthcare Management System

A full-stack Healthcare Management System built with ASP.NET Core Web API, Angular, Blazor Server, SQL Server, RabbitMQ, JWT Authentication, Jenkins, and AWS. Features

Patient Portal (Angular):

• User Registration

• User Login with JWT Authentication

• View Doctor Availability

• Book Appointments

• View Appointment History

• View Health Records

• Responsive UI

• Edit Profile

Doctor Portal (Angular):

• Doctor Login

• View Daily Appointments

• Manage Appointment Status

• View Patient Details

• View Patient Health Records

• Manage Availability and Leave

• Add Health Records

Admin Portal (Blazor):

• Dashboard

• Manage Doctors

• Manage Patients

• Manage Appointments

• View Reports

Backend

• RESTful APIs

• JWT Authentication & Authorization

• Entity Framework Core

• Repository Pattern

• Dependency Injection

• AutoMapper

• Fluent Validation

• Global Exception Handling

• Serilog Logging

Additional Features

• RabbitMQ

• AWS Elastic Beanstalk

• Amazon S3

• CI/CD using Jenkins

Technology Stack

• Angular

• Blazor Server

• ASP.NET Core Web API

• SQL Server

• RabbitMQ

• Jenkins

• AWS Elastic Beanstalk

• Amazon EC2

• Amazon S3

Getting Started (Local Development)

Step 1: Clone the Repository

Clone the project from GitHub to your local machine.

git clone

Navigate to the project directory after cloning.

Step 2: Restore NuGet Packages

Restore all required NuGet packages and project dependencies before building the solution.

dotnet restore

Step 3: Configure the Database

Update the SQL Server connection string in the appsettings.json file with your local database credentials.

Apply the Entity Framework Core migrations to create the required database schema.

dotnet ef database update

Step 4: Start RabbitMQ

Ensure the RabbitMQ server is installed and running. RabbitMQ is used for asynchronous message processing, such as appointment notifications and background tasks.

Step 5: Run the ASP.NET Core Web API

Start the backend API.

dotnet run

By default, the API will be available at:

https://localhost:5001

(or the URL displayed in the terminal)

Step 6: Run the Angular Portals

Open the Angular project directory and install all required Node.js packages.

npm install

Start the Angular development server.

ng serve

The application will be available at:

http://localhost:4200

If the Patient Portal and Doctor Portal are separate Angular projects, repeat these steps for each project.

Step 7: Run the Blazor Admin Portal

Navigate to the Blazor Admin project directory and start the application.

dotnet run

Open the URL displayed in the terminal (typically https://localhost:) to access the Admin Portal.

AWS Deployment

The application is deployed to AWS using a Continuous Integration and Continuous Deployment (CI/CD) pipeline powered by Jenkins.

The deployment process is as follows:

Step 1: Push Code to GitHub

After implementing new features or bug fixes, the latest source code is committed and pushed to the GitHub repository. This serves as the source for the deployment pipeline.

Step 2: Jenkins Builds the Application

Jenkins automatically detects changes in the GitHub repository and triggers the CI/CD pipeline. During this stage, Jenkins:

• Clones the latest source code.

• Restores .NET and Node.js dependencies.

• Builds the ASP.NET Core Web API, Angular applications, and Blazor application.

• Executes the required build and publish commands.

• Generates a deployment package containing the application files.

Step 3: Upload Deployment Package to Amazon S3

After a successful build, Jenkins uploads the generated deployment package (ZIP file) to an Amazon S3 bucket. Amazon S3 acts as a storage location for application versions that are deployed through AWS Elastic Beanstalk.

Step 4: Deploy Using AWS Elastic Beanstalk

AWS Elastic Beanstalk retrieves the deployment package from Amazon S3 and deploys it to Amazon EC2 instances. Elastic Beanstalk automatically manages the deployment process, including:

• Provisioning EC2 instances.

• Configuring the application environment.

• Managing the web server and runtime.

• Performing application updates with minimal manual intervention.

Step 5: Access the Application

Once the deployment is completed successfully, the application becomes available through the AWS Elastic Beanstalk environment URL. Users can access the deployed application directly using this public endpoint.

Author

Suhana Ali