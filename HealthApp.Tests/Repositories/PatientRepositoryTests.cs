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
        [Fact]// RegisterPatient - Success
        public void RegisterPatient_ShouldAddPatient_ToDatabase()
        {
            var patient = new Patient
            {
                PatientId = 1,
                Name = "John Doe",
                PhoneNumber = "9876543210",
                Email = "john.doe@email.com"
            };

            var result = _repository.RegisterPatient(patient);

            Assert.Equal(patient, _patientDb.Patients[5]);
            Assert.Equal("Patient ID 1 added successfully!", result);
        }

        [Fact]// RegisterPatient - Multiple Patients
        public void RegisterPatient_ShouldAllow_MultiplePatients()
        {
            var patient1 = new Patient
            {
                PatientId = 1,
                Name = "John Doe",
                PhoneNumber = "9876543210",
                Email = "john@email.com"
            };

            var patient2 = new Patient
            {
                PatientId = 2,
                Name = "Jane Smith",
                PhoneNumber = "9123456780",
                Email = "jane@email.com"
            };

            _repository.RegisterPatient(patient1);
            _repository.RegisterPatient(patient2);

            Assert.Equal(7, _patientDb.Patients.Count);
        }

        [Fact]// GetPatientById - Success
        public void GetPatientById_ShouldReturnPatient_WhenExists()
        {
            var patient = new Patient
            {
                PatientId = 1,
                Name = "John Doe",
                PhoneNumber = "9876543210",
                Email = "john@email.com"
            };

            _patientDb.Patients.Add(patient);

            var result = _repository.GetPatientById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.PatientId);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal("9876543210", result.PhoneNumber);
            Assert.Equal("john@email.com", result.Email);
        }

        [Fact]// GetPatientById - Not Found
        public void GetPatientById_ShouldReturnNull_WhenNotFound()
        {
            _patientDb.Patients.Add(new Patient
            {
                PatientId = 1,
                Name = "Existing User",
                PhoneNumber = "9999999999",
                Email = "existing@email.com"
            });

            var result = _repository.GetPatientById(999);

            Assert.Null(result);
        }

        [Fact]// GetPatientById - Duplicate IDs
        public void GetPatientById_ShouldReturnFirstMatch_WhenDuplicateIdsExist()
        {
            var patient1 = new Patient
            {
                PatientId = 1,
                Name = "First Entry",
                PhoneNumber = "1111111111",
                Email = "first@email.com"
            };

            var patient2 = new Patient
            {
                PatientId = 1,
                Name = "Second Entry",
                PhoneNumber = "2222222222",
                Email = "second@email.com"
            };

            _patientDb.Patients.Add(patient1);
            _patientDb.Patients.Add(patient2);

            var result = _repository.GetPatientById(1);

            Assert.NotNull(result);
            Assert.Equal("First Entry", result.Name);
        }

        [Fact]// GetAllPatients - Success
        public void GetAllPatients_ShouldReturnAllPatients()
        {
            var patient1 = new Patient
            {
                PatientId = 1,
                Name = "John Doe",
                PhoneNumber = "9876543210",
                Email = "john@email.com"
            };

            var patient2 = new Patient
            {
                PatientId = 2,
                Name = "Jane Smith",
                PhoneNumber = "9123456780",
                Email = "jane@email.com"
            };

            _patientDb.Patients.Add(patient1);
            _patientDb.Patients.Add(patient2);

            var result = _repository.GetAllPatients();

            Assert.NotNull(result);
            Assert.Equal(7, result.Count);
            Assert.Contains(result, p => p.PatientId == 1);
            Assert.Contains(result, p => p.PatientId == 2);
        }

        [Fact]// UpdatePatient - Success
        public void UpdatePatient_ShouldUpdateExistingPatient()
        {
            var patient = new Patient
            {
                PatientId = 1,
                Name = "Old Name",
                PhoneNumber = "1111111111",
                Email = "old@email.com"
            };

            _patientDb.Patients.Add(patient);

            var updatedPatient = new Patient
            {
                PatientId = 1,
                Name = "New Name",
                PhoneNumber = "9999999999",
                Email = "new@email.com"
            };

            var result = _repository.UpdatePatient(patient, updatedPatient);

            var storedPatient = _patientDb.Patients[5];
            Assert.Equal(result.Name, storedPatient.Name);
            Assert.Equal(result.PhoneNumber, storedPatient.PhoneNumber);
            Assert.Equal(result.Email, storedPatient.Email);
        }
    }
}