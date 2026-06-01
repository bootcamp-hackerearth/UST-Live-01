using HAP_Pod4_ConsoleApp_au.Data;
using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repositories;
using HAP_Pod4_ConsoleApp_au.Repository;
using Xunit;

namespace HealthAxisTests.RepositoryTests
{
    public class PatientRepositoryTests
    {
        private readonly AppDbContext _dbContext;
        private readonly PatientRepository _repo;

        public PatientRepositoryTests()
        {
            _dbContext = new AppDbContext();
            _repo = new PatientRepository();

            // Seed patients into repository
            foreach (var patient in _dbContext.Patients)
            {
                _repo.RegisterPatient(patient);
            }
        }

        [Fact]
        public void RegisterPatient_WhenValid_ShouldAddPatient()
        {
            // Arrange
            Patient patient = new Patient
            {
                PatientId = _dbContext.GetNextPatientId(),
                FullName = "Rohit Sharma",
                DateOfBirth = new DateTime(1995, 5, 10),
                Gender = Patient.GenderOptions.Male,
                PhoneNumber = "9876543299",
                Email = "rohit@example.com",
                InsuranceID = "INS2001",
                CreatedDate = DateTime.Now
            };

            // Act
            var result = _repo.RegisterPatient(patient);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Rohit Sharma", result.FullName);
            Assert.Equal(6, _repo.GetAllPatients().Count);
        }

        [Fact]
        public void GetAllPatients_ShouldReturnAllPatients()
        {
            // Act
            var result = _repo.GetAllPatients();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void GetPatientById_WhenIdExists_ShouldReturnPatient()
        {
            // Act
            var result = _repo.GetPatientById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Arun Kumar", result.FullName);
        }

        [Fact]
        public void GetPatientById_WhenIdDoesNotExist_ShouldReturnNull()
        {
            // Act
            var result = _repo.GetPatientById(100);

            // Assert
            Assert.Null(result);
        }
    }
}