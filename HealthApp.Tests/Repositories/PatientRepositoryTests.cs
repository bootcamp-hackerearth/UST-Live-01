using System;
using Xunit;
using HealthApp.ConsoleApp.Repositories;
using HealthApp.ConsoleApp.Databases;
using HealthApp.ConsoleApp.Models;

namespace HealthApp.Tests.Repositories
{
    // Test class for PatientRepository to validate patient management functionalities
    public class PatientRepositoryTests
    {
        private readonly PatientDb _patientDb;
        private readonly PatientRepository _repository;

        public PatientRepositoryTests()
        {
            _patientDb = new PatientDb();
            _repository = new PatientRepository(_patientDb);
        }

        [Fact]
        public void RegisterPatient_ShouldAddPatient_ToDatabase()
        {
            var patient = new Patient
            {
                PatientId = 1,
                FullName = "John Doe",
                PhoneNumber = "9876543210",
                Email = "john.doe@email.com",
                InsuranceId = "sadf23423"
            };

            var result = _repository.RegisterPatient(patient);

            Assert.Single(_patientDb.Patients);
            Assert.Equal(patient, _patientDb.Patients[0]);
            Assert.Equal("Patient ID 1 added successfully!", result);
        }

        [Fact]
        public void GetPatientByName_ShouldReturnMatchingPatients_IgnoringCase()
        {
            // Arrange
            _patientDb.Patients.AddRange(new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    FullName = "John Doe",
                    PhoneNumber = "1111111111",
                    Email = "john@email.com",
                    InsuranceId = "INS1"
                },
                new Patient
                {
                    PatientId = 2,
                    FullName = "Jane Doe",
                    PhoneNumber = "2222222222",
                    Email = "jane@email.com",
                    InsuranceId = "INS2"
                },
                new Patient
                {
                    PatientId = 3,
                    FullName = "Alice Smith",
                    PhoneNumber = "3333333333",
                    Email = "alice@email.com",
                    InsuranceId = "INS3"
                }
            });

            // Act
            var result = _repository.GetPatientByName("doe");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.FullName == "John Doe");
            Assert.Contains(result, p => p.FullName == "Jane Doe");
        }

        [Fact]
        public void RegisterPatient_ShouldAllow_MultiplePatients()
        {
            var patient1 = new Patient
            {
                PatientId = 1,
                FullName = "John Doe",
                PhoneNumber = "9876543210",
                Email = "john@email.com",
                InsuranceId = "sadf23423"
            };

            var patient2 = new Patient
            {
                PatientId = 2,
                FullName = "Jane Smith",
                PhoneNumber = "9123456780",
                Email = "jane@email.com",
                InsuranceId = "sadf23423"
            };

            _repository.RegisterPatient(patient1);
            _repository.RegisterPatient(patient2);

            Assert.Equal(2, _patientDb.Patients.Count);
        }

        [Fact]
        public void GetPatientById_ShouldReturnPatient_WhenExists()
        {
            var patient = new Patient
            {
                PatientId = 1,
                FullName = "John Doe",
                PhoneNumber = "9876543210",
                Email = "john@email.com",
                InsuranceId = "sadf23423"
            };

            _patientDb.Patients.Add(patient);

            var result = _repository.GetPatientById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.PatientId);
            Assert.Equal("John Doe", result.FullName);
            Assert.Equal("9876543210", result.PhoneNumber);
            Assert.Equal("john@email.com", result.Email);
        }

        [Fact]
        public void GetPatientById_ShouldReturnNull_WhenNotFound()
        {
            _patientDb.Patients.Add(new Patient
            {
                PatientId = 1,
                FullName = "Existing User",
                PhoneNumber = "9999999999",
                Email = "existing@email.com",
                InsuranceId = "sadf23423"
            });

            var result = _repository.GetPatientById(999);

            Assert.Null(result);
        }

        [Fact]
        public void GetPatientById_ShouldReturnFirstMatch_WhenDuplicateIdsExist()
        {
            var patient1 = new Patient
            {
                PatientId = 1,
                FullName = "First Entry",
                PhoneNumber = "1111111111",
                Email = "first@email.com",
                InsuranceId = "sadf23423"
            };

            var patient2 = new Patient
            {
                PatientId = 1,
                FullName = "Second Entry",
                PhoneNumber = "2222222222",
                Email = "second@email.com",
                InsuranceId = "sadf23423"
            };

            _patientDb.Patients.Add(patient1);
            _patientDb.Patients.Add(patient2);

            var result = _repository.GetPatientById(1);

            Assert.NotNull(result);
            Assert.Equal("First Entry", result.FullName);
        }

        [Fact]
        public void GetAllPatients_ShouldReturnAllPatients()
        {
            var patient1 = new Patient
            {
                PatientId = 1,
                FullName = "John Doe",
                PhoneNumber = "9876543210",
                Email = "john@email.com",
                InsuranceId = "sadf23423"
            };

            var patient2 = new Patient
            {
                PatientId = 2,
                FullName = "Jane Smith",
                PhoneNumber = "9123456780",
                Email = "jane@email.com",
                InsuranceId = "sadf23423"
            };

            _patientDb.Patients.Add(patient1);
            _patientDb.Patients.Add(patient2);

            var result = _repository.GetAllPatients();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.PatientId == 1);
            Assert.Contains(result, p => p.PatientId == 2);
        }

        [Fact]
        public void UpdatePatient_ShouldUpdateExistingPatient()
        {
            var patient = new Patient
            {
                PatientId = 1,
                FullName = "Old Name",
                PhoneNumber = "1111111111",
                Email = "old@email.com",
                InsuranceId = "sadf23423"
            };

            _patientDb.Patients.Add(patient);

            var updatedPatient = new Patient
            {
                PatientId = 1,
                FullName = "New Name",
                PhoneNumber = "9999999999",
                Email = "new@email.com",
                InsuranceId = "sadf23423"
            };

            var result = _repository.UpdatePatient(patient, updatedPatient);

            var storedPatient = _patientDb.Patients[0];
            Assert.Equal(result.FullName, storedPatient.FullName);
            Assert.Equal(result.PhoneNumber, storedPatient.PhoneNumber);
            Assert.Equal(result.Email, storedPatient.Email);
        }
    }
}