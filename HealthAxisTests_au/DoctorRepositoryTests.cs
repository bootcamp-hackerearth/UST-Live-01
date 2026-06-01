using HAP_Pod4_ConsoleApp_au.Data;
using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repositories;
using Xunit;

namespace HealthAxisTests.RepositoryTests
{
    public class DoctorRepositoryTests
    {
        private readonly AppDbContext _dbContext;
        private readonly DoctorRepository _repo;

        public DoctorRepositoryTests()
        {
            _dbContext = new AppDbContext();
            _repo = new DoctorRepository();

            // Seed doctors into repository
            foreach (var doctor in _dbContext.Doctors)
            {
                _repo.AddDoctor(doctor);
            }
        }

        [Fact]
        public void AddDoctor_WhenValid_ShouldAddDoctor()
        {
            // Arrange
            Doctor doctor = new Doctor
            {
                DoctorId = _dbContext.GetNextDoctorId(),
                FullName = "Dr. Chitresh Zope",
                Specialisation = Doctor.SpecialisationOption.Cardiologist,
                YearsOfExperience = 14,
                ConsultationFee = 900,
                IsActive = true
            };

            // Act
            var result = _repo.AddDoctor(doctor);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Dr. Chitresh Zope", result.FullName);
            Assert.Equal(9, _repo.GetAllDoctors().Count);
        }

        [Fact]
        public void GetAllDoctors_ShouldReturnAllDoctors()
        {
            // Act
            var result = _repo.GetAllDoctors();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.Count);
        }

        [Fact]
        public void SearchDoctorBySpecialisation_WhenExists_ShouldReturnDoctors()
        {
            // Act
            var result = _repo.SearchDoctorBySpecialisation(
                Doctor.SpecialisationOption.Pediatrician);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Dr. Neha Iyer", result[0].FullName);
        }

        [Fact]
        public void SearchDoctorBySpecialisation_WhenNotExists_ShouldReturnEmptyList()
        {
            // Act
            var result = _repo.SearchDoctorBySpecialisation(
                Doctor.SpecialisationOption.Psychiatrist);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}