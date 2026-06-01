using System;
using System.Collections.Generic;
using Xunit;
using HealthApp.ConsoleApp.Repositories;
using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Databases;
using HealthApp.ConsoleApp.Exceptions;

namespace HealthApp.Tests.Repositories
{
    // Test class for DoctorRepository to validate doctor management functionalities
    public class DoctorRepositoryTests
    {
        private readonly DoctorDb _doctorDb;
        private readonly DoctorRepository _repository;

        public DoctorRepositoryTests()
        {
            _doctorDb = new DoctorDb
            {
                Doctors = new List<Doctor>()
            };

            _repository = new DoctorRepository(_doctorDb);
        }

        // AddDoctor - Success
        [Fact]
        public void AddDoctor_ShouldAddDoctor()
        {
            var doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Dr. Smith",
                Specialisation = "Cardiology"
            };

            var result = _repository.AddDoctor(doctor);

            Assert.Single(_doctorDb.Doctors);
            Assert.Contains("added successfully", result);
        }

        // GetDoctorById - Success
        [Fact]
        public void GetDoctorById_ShouldReturnDoctor()
        {
            _doctorDb.Doctors.Add(new Doctor
            {
                DoctorId = 1,
                FullName = "Dr. A",
                Specialisation = "Neurology"
            });

            var result = _repository.GetDoctorById(1);

            Assert.NotNull(result);
            Assert.Equal("Dr. A", result.FullName);
        }

        // GetDoctorById - Not Found
        [Fact]
        public void GetDoctorById_ShouldReturnNull_WhenNotFound()
        {
            var result = _repository.GetDoctorById(99);

            Assert.Null(result);
        }

        // GetDoctorsBySpecialisation - Success
        [Fact]
        public void GetDoctorsBySpecialisation_ShouldReturnMatchingDoctors()
        {
            _doctorDb.Doctors.Add(new Doctor
            {
                DoctorId = 1,
                FullName = "Dr. X",
                Specialisation = "Cardiology"
            });

            _doctorDb.Doctors.Add(new Doctor
            {
                DoctorId = 2,
                FullName = "Dr. Y",
                Specialisation = "Neurology"
            });

            var result = _repository.GetDoctorsBySpecialisation("cardiology");

            Assert.Single(result);
        }

        // GetDoctorsBySpecialisation - No Matches
        [Fact]
        public void GetDoctorsBySpecialisation_ShouldReturnEmptyList_WhenNoMatch()
        {
            var result = _repository.GetDoctorsBySpecialisation("Oncology");

            Assert.Empty(result);
        }

        // UpdateDoctor - Success
        [Fact]
        public void UpdateDoctor_ShouldUpdateDoctorDetails()
        {
            var existing = new Doctor
            {
                DoctorId = 1,
                FullName = "Old Name",
                Specialisation = "General"
            };

            var updated = new Doctor
            {
                DoctorId = 1,
                FullName = "New Name",
                Specialisation = "Ortho",
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };

            var result = _repository.UpdateDoctor(existing, updated);

            Assert.Equal("New Name", result.FullName);
            Assert.Equal("Ortho", result.Specialisation);
            Assert.Equal(10, result.YearsOfExperience);
        }

        // GetAllDoctors - Success
        [Fact]
        public void GetAllDoctors_ShouldReturnAllDoctors()
        {
            _doctorDb.Doctors.Add(new Doctor
            {
                DoctorId = 1,
                FullName = "Doc1",
                Specialisation = "General"
            });

            _doctorDb.Doctors.Add(new Doctor
            {
                DoctorId = 2,
                FullName = "Doc2",
                Specialisation = "Cardiology"
            });

            var result = _repository.GetAllDoctors();

            Assert.Equal(2, result.Count);
        }

        // GetAllDoctors - Empty Case
        [Fact]
        public void GetAllDoctors_ShouldReturnEmptyList_WhenNoDoctors()
        {
            var result = _repository.GetAllDoctors();

            Assert.Empty(result);
        }
    }
}