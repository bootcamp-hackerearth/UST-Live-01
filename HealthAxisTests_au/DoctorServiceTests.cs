using System;
using System.Collections.Generic;
using HAP_Pod4_ConsoleApp_au.Data;
using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repositories;
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
            Doctor doctor = new Doctor
            {
                FullName = "Dr. Chitresh Zope",
                Specialisation = Doctor.SpecialisationOption.Cardiologist,
                YearsOfExperience = 12,
                ConsultationFee = 1000,
                IsActive = true
            };

            Doctor result = _service.AddDoctor(doctor);

            Assert.NotNull(result);
            Assert.Equal("Dr. Chitresh Zope", result.FullName);
            Assert.Equal(9, _service.GetAllDoctors().Count);
        }

        [Fact]
        public void AddDoctor_WhenDoctorIsNull_ShouldThrowException()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _service.AddDoctor(null!);
            });

            Assert.Equal("doctor", ex.ParamName);
        }

        [Fact]
        public void AddDoctor_WhenDoctorNameIsEmpty_ShouldThrowException()
        {
            Doctor doctor = new Doctor
            {
                FullName = string.Empty,
                Specialisation = Doctor.SpecialisationOption.Neurologist
            };

            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
            {
                _service.AddDoctor(doctor);
            });

            Assert.Contains("Doctor's FullName cannot be empty.", ex.Message);
        }

        [Fact]
        public void AddDoctor_WhenDoctorNameIsWhitespaceOnly_ShouldThrowException()
        {
            Doctor doctor = new Doctor
            {
                FullName = "   ",
                Specialisation = Doctor.SpecialisationOption.Neurologist
            };

            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
            {
                _service.AddDoctor(doctor);
            });

            Assert.Contains("Doctor's FullName cannot be empty.", ex.Message);
        }

        [Fact]
        public void AddDoctor_WhenDoctorAlreadyExists_ShouldThrowException()
        {
            Doctor doctor = new Doctor
            {
                FullName = "Dr. Priya Sharma",
                Specialisation = Doctor.SpecialisationOption.Cardiologist,
                YearsOfExperience = 10,
                ConsultationFee = 900,
                IsActive = true
            };

            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() =>
            {
                _service.AddDoctor(doctor);
            });

            Assert.Equal("Doctor already exists.", ex.Message);
        }

        [Fact]
        public void AddDoctor_WhenDoctorExistsWithDifferentCase_ShouldThrowException()
        {
            Doctor doctor = new Doctor
            {
                FullName = "dr. priya sharma",
                Specialisation = Doctor.SpecialisationOption.Cardiologist,
                YearsOfExperience = 10,
                ConsultationFee = 900,
                IsActive = true
            };

            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() =>
            {
                _service.AddDoctor(doctor);
            });

            Assert.Equal("Doctor already exists.", ex.Message);
        }

        // --- NEW TEST: Covers the missing Context null check in AddDoctor ---
        [Fact]
        public void AddDoctor_WhenContextIsNull_ShouldThrowInvalidOperationException()
        {
            // Arrange: Initialize service with the repository constructor instead of DbContext
            var repoService = new DoctorService(new DoctorRepository());
            Doctor doctor = new Doctor
            {
                FullName = "Dr. Test Context Null",
                Specialisation = Doctor.SpecialisationOption.Neurologist
            };

            // Act & Assert
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() =>
            {
                repoService.AddDoctor(doctor);
            });

            Assert.Equal("AppDbContext is not initialized.", ex.Message);
        }

        [Fact]
        public void GetAllDoctors_ShouldReturnAllDoctors()
        {
            List<Doctor> result = _service.GetAllDoctors();

            Assert.NotNull(result);
            Assert.Equal(8, result.Count);
        }

        // --- NEW TEST: Covers the _repository?.GetAllDoctors() path ---
        [Fact]
        public void GetAllDoctors_WhenContextIsNull_ShouldUseRepository()
        {
            var repoService = new DoctorService(new DoctorRepository());

            List<Doctor> result = repoService.GetAllDoctors();

            Assert.NotNull(result);
        }

        // --- NEW TEST: Covers the ?? new List<Doctor>() fallback ---
        [Fact]
        public void GetAllDoctors_WhenBothContextAndRepositoryAreNull_ShouldReturnEmptyList()
        {
            var nullService = new DoctorService((IDoctorRepository)null!);

            List<Doctor> result = nullService.GetAllDoctors();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void SearchDoctorBySpecialisation_WhenExists_ShouldReturnDoctor()
        {
            List<Doctor> result = _service.SearchDoctorBySpecialisation(Doctor.SpecialisationOption.Pediatrician);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Dr. Neha Iyer", result[0].FullName);
        }

        [Fact]
        public void SearchDoctorBySpecialisation_WhenMultipleMatch_ShouldReturnAllMatches()
        {
            Doctor secondPediatrician = new Doctor
            {
                FullName = "Dr. Sarah Smith",
                Specialisation = Doctor.SpecialisationOption.Pediatrician,
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            };

            _service.AddDoctor(secondPediatrician);

            List<Doctor> result = _service.SearchDoctorBySpecialisation(Doctor.SpecialisationOption.Pediatrician);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, d => d.FullName == "Dr. Neha Iyer");
            Assert.Contains(result, d => d.FullName == "Dr. Sarah Smith");
        }

        [Fact]
        public void SearchDoctorBySpecialisation_WhenNotExists_ShouldReturnEmptyList()
        {
            List<Doctor> result = _service.SearchDoctorBySpecialisation(Doctor.SpecialisationOption.Psychiatrist);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        // --- NEW TEST: Covers the _repository?.SearchDoctorBySpecialisation() path ---
        [Fact]
        public void SearchDoctorBySpecialisation_WhenContextIsNull_ShouldUseRepository()
        {
            var repoService = new DoctorService(new DoctorRepository());

            List<Doctor> result = repoService.SearchDoctorBySpecialisation(Doctor.SpecialisationOption.Pediatrician);

            Assert.NotNull(result);
        }

        // --- NEW TEST: Covers the ?? new List<Doctor>() fallback for Search ---
        [Fact]
        public void SearchDoctorBySpecialisation_WhenBothContextAndRepoAreNull_ShouldReturnEmptyList()
        {
            var nullService = new DoctorService((IDoctorRepository)null!);

            List<Doctor> result = nullService.SearchDoctorBySpecialisation(Doctor.SpecialisationOption.Neurologist);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}