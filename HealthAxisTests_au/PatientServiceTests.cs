using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repositories;
using HAP_Pod4_ConsoleApp_au.Repository;
using HAP_Pod4_ConsoleApp_au.Services.Impl;
using Xunit;

namespace HealthAxisTests.ServiceTests
{
    public class PatientServiceTests
    {
        private readonly PatientRepository _repository;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _repository = new PatientRepository();
            _service = new PatientService(_repository);

            // Seed Data
            _repository.RegisterPatient(new Patient
            {
                PatientId = 1,
                FullName = "Arun Kumar",
                DateOfBirth = new DateTime(1992, 5, 14),
                Gender = Patient.GenderOptions.Male,
                PhoneNumber = "9876543210",
                Email = "arun@example.com",
                InsuranceID = "INS1001",
                CreatedDate = DateTime.Now
            });

            _repository.RegisterPatient(new Patient
            {
                PatientId = 2,
                FullName = "Meera Nair",
                DateOfBirth = new DateTime(1995, 8, 20),
                Gender = Patient.GenderOptions.Female,
                PhoneNumber = "9876543211",
                Email = "meera@example.com",
                InsuranceID = "INS1002",
                CreatedDate = DateTime.Now
            });
        }

        [Fact]
        public void RegisterPatient_WhenValid_ShouldAddPatient()
        {
            // Arrange
            Patient patient = new Patient
            {
                PatientId = 3,
                FullName = "Rahul Menon",
                DateOfBirth = new DateTime(2000, 1, 10),
                Gender = Patient.GenderOptions.Male,
                PhoneNumber = "9876543212",
                Email = "rahul@example.com",
                InsuranceID = "INS1003",
                CreatedDate = DateTime.Now
            };

            // Act
            var result = _service.RegisterPatient(patient);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Rahul Menon", result.FullName);
            Assert.Equal(3, _service.GetAllPatients().Count);
        }

        [Fact]
        public void GetAllPatients_ShouldReturnAllPatients()
        {
            // Act
            var result = _service.GetAllPatients();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetPatientById_WhenIdExists_ShouldReturnPatient()
        {
            // Act
            var result = _service.GetPatientById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Arun Kumar", result.FullName);
        }

        [Fact]
        public void GetPatientById_WhenIdDoesNotExist_ShouldReturnNull()
        {
            // Act
            var result = _service.GetPatientById(100);

            // Assert
            Assert.Null(result);
        }
    }
}