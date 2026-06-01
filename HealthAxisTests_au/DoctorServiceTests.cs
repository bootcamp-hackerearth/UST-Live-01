using HAP_Pod4_ConsoleApp_au.Data;
using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Services.Impl;
using Xunit;

namespace HealthAxisTests.ServiceTests
{
    public class DoctorServiceTests
    {
        private readonly AppDbContext _dbContext;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _dbContext = new AppDbContext();
            _service = new DoctorService(_dbContext);
        }

        [Fact]
        public void AddDoctor_WhenValid_ShouldAddDoctor()
        {
            // Arrange
            Doctor doctor = new Doctor
            {
                FullName = "Dr. Chitresh Zope",
                Specialisation =
                    Doctor.SpecialisationOption.Cardiologist,
                YearsOfExperience = 12,
                ConsultationFee = 1000,
                IsActive = true
            };

            // Act
            var result = _service.AddDoctor(doctor);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Dr. Chitresh Zope", result.FullName);
            Assert.Equal(9, _service.GetAllDoctors().Count);
        }

        [Fact]
        public void AddDoctor_WhenDoctorIsNull_ShouldThrowException()
        {
            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() =>
            {
                _ = _service.AddDoctor(null!);
            });

            Assert.Equal(
                "Doctor object cannot be null.",
                ex.Message);
        }

        [Fact]
        public void AddDoctor_WhenDoctorNameIsEmpty_ShouldThrowException()
        {
            // Arrange
            Doctor doctor = new Doctor
            {
                FullName = "",
                Specialisation =
                    Doctor.SpecialisationOption.Neurologist
            };

            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() =>
            {
                _service.AddDoctor(doctor);
            });

            Assert.Equal(
                "Doctor name cannot be empty.",
                ex.Message);
        }

        [Fact]
        public void AddDoctor_WhenDoctorAlreadyExists_ShouldThrowException()
        {
            // Arrange
            Doctor doctor = new Doctor
            {
                FullName = "Dr. Priya Sharma",
                Specialisation =
                    Doctor.SpecialisationOption.Cardiologist,
                YearsOfExperience = 10,
                ConsultationFee = 900,
                IsActive = true
            };

            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() =>
            {
                _service.AddDoctor(doctor);
            });

            Assert.Equal(
                "Doctor already exists.",
                ex.Message);
        }

        [Fact]
        public void GetAllDoctors_ShouldReturnAllDoctors()
        {
            // Act
            var result = _service.GetAllDoctors();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.Count);
        }

        [Fact]
        public void SearchDoctorBySpecialisation_WhenExists_ShouldReturnDoctor()
        {
            // Act
            var result = _service.SearchDoctorBySpecialisation(
                Doctor.SpecialisationOption.Pediatrician);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(
                "Dr. Neha Iyer",
                result[0].FullName);
        }

        [Fact]
        public void SearchDoctorBySpecialisation_WhenNotExists_ShouldReturnEmptyList()
        {
            // Act
            var result = _service.SearchDoctorBySpecialisation(
                Doctor.SpecialisationOption.Psychiatrist);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}