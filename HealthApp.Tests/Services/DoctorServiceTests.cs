using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using HealthApp.ConsoleApp.Services;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Exceptions;

namespace HealthApp.Tests.Services
{
    // Test class for DoctorService to validate doctor management functionalities
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _mockRepo;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _mockRepo = new Mock<IDoctorRepository>();
            _service = new DoctorService(_mockRepo.Object);
        }

        private static Doctor GetSampleDoctor(int id)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = "Doctor " + id,
                Specialisation = "General"
            };
        }

        // AddDoctor
        [Fact]
        public void AddDoctor_ShouldAssignIdAndAddDoctor()
        {
            var doctors = new List<Doctor>
            {
                GetSampleDoctor(101),
                GetSampleDoctor(102)
            };

            var newDoctor = GetSampleDoctor(0);

            _mockRepo.Setup(r => r.GetAllDoctors()).Returns(doctors);
            _mockRepo.Setup(r => r.AddDoctor(It.IsAny<Doctor>()))
                     .Returns("Doctor added successfully");

            var result = _service.AddDoctor(newDoctor);

            Assert.Equal(103, newDoctor.DoctorId);
            Assert.Contains("successfully", result);
        }

        [Fact]
        public void GetAllDoctors_ShouldReturnDoctors_WhenDataExists()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                GetSampleDoctor(101),
                GetSampleDoctor(102)
            };

            _mockRepo.Setup(r => r.GetAllDoctors()).Returns(doctors);

            // Act
            var result = _service.GetAllDoctors();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetAllDoctors_ShouldThrowException_WhenNoDoctorsExist()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllDoctors())
                    .Returns(new List<Doctor>());

            // Act & Assert
            Assert.Throws<DoctorNotFoundException>(() => _service.GetAllDoctors());
        }


        // GetDoctorById
        [Fact]
        public void GetDoctorById_ShouldReturnDoctor()
        {
            var doctor = GetSampleDoctor(101);

            _mockRepo.Setup(r => r.GetDoctorById(101)).Returns(doctor);

            var result = _service.GetDoctorById(101);

            Assert.NotNull(result);
            Assert.Equal(101, result.DoctorId);
        }

        // GetDoctorById - Exception
        [Fact]
        public void GetDoctorById_ShouldThrowException_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetDoctorById(999)).Returns((Doctor?)null);

            Assert.Throws<DoctorNotFoundException>(() => _service.GetDoctorById(999));
        }

        // GetDoctorsBySpecialisation
        [Fact]
        public void GetDoctorsBySpecialisation_ShouldReturnDoctors()
        {
            var doctors = new List<Doctor>
            {
                GetSampleDoctor(101),
                GetSampleDoctor(102)
            };

            _mockRepo.Setup(r => r.GetDoctorsBySpecialisation("General"))
                     .Returns(doctors);

            var result = _service.GetDoctorsBySpecialisation("General");

            Assert.Equal(2, result.Count);
        }

        // GetDoctorsBySpecialisation - Exception
        [Fact]
        public void GetDoctorsBySpecialisation_ShouldThrowException_WhenNoDoctors()
        {
            _mockRepo.Setup(r => r.GetDoctorsBySpecialisation("Cardiology"))
                     .Returns(new List<Doctor>());

            Assert.Throws<SpecialisationNotFoundException>(() =>
                _service.GetDoctorsBySpecialisation("Cardiology"));
        }

        // UpdateDoctor
        [Fact]
        public void UpdateDoctor_ShouldUpdateDoctor()
        {
            var existing = GetSampleDoctor(101);
            var updated = GetSampleDoctor(101);
            updated.FullName = "Updated Name";

            _mockRepo.Setup(r => r.GetDoctorById(101)).Returns(existing);
            _mockRepo.Setup(r => r.UpdateDoctor(existing, updated)).Returns(updated);

            var result = _service.UpdateDoctor(updated);

            Assert.Equal("Updated Name", result.FullName);
        }

        // UpdateDoctor - Exception
        [Fact]
        public void UpdateDoctor_ShouldThrowException_WhenDoctorNotFound()
        {
            var doctor = GetSampleDoctor(999);

            _mockRepo.Setup(r => r.GetDoctorById(999)).Returns((Doctor?)null);

            Assert.Throws<DoctorNotFoundException>(() => _service.UpdateDoctor(doctor));
        }

        // DoctorIdGenerator
        [Fact]
        public void DoctorIdGenerator_ShouldReturnNextId()
        {
            var doctors = new List<Doctor>
            {
                GetSampleDoctor(101),
                GetSampleDoctor(102)
            };

            var result = DoctorService.DoctorIdGenerator(doctors);

            Assert.Equal(103, result);
        }

        // DoctorIdGenerator - Empty List
        [Fact]
        public void DoctorIdGenerator_ShouldReturn101_WhenEmpty()
        {
            var doctors = new List<Doctor>();

            var result = DoctorService.DoctorIdGenerator(doctors);

            Assert.Equal(101, result);
        }
    }
}