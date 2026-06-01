USE HealthApp;

CREATE TABLE Patients (
    PatientId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    InsuranceId NVARCHAR(50),
    CreatedDate DATETIME2 DEFAULT GETDATE()
);


CREATE TABLE Doctors (
    DoctorId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Specialisation NVARCHAR(50) NOT NULL,
    YearsOfExperience INT NOT NULL,
    ConsultationFee DECIMAL(10,2) NOT NULL,
    IsActive BIT DEFAULT 1
);

CREATE TABLE Appointments (
    AppointmentId INT IDENTITY(1,1) PRIMARY KEY,
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    ScheduledDate DATE NOT NULL,
    TimeSlot NVARCHAR(20) NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    CancellationReason NVARCHAR(500),

    CONSTRAINT FK_Appointments_Patient 
        FOREIGN KEY (PatientId) REFERENCES Patients(PatientId),

    CONSTRAINT FK_Appointments_Doctor 
        FOREIGN KEY (DoctorId) REFERENCES Doctors(DoctorId)
);

CREATE TABLE HealthRecords (
    RecordId INT IDENTITY(1,1) PRIMARY KEY,
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    VisitDate DATETIME2 NOT NULL,
    Diagnosis NVARCHAR(500) NOT NULL,
    Prescription NVARCHAR(500) NOT NULL,
    Notes NVARCHAR(1000),

    CONSTRAINT FK_HealthRecords_Patient 
        FOREIGN KEY (PatientId) REFERENCES Patients(PatientId),

    CONSTRAINT FK_HealthRecords_Doctor 
        FOREIGN KEY (DoctorId) REFERENCES Doctors(DoctorId)
);

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(256) NOT NULL,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Patient', 'Doctor', 'Admin')),
    ReferenceId INT NOT NULL
);












