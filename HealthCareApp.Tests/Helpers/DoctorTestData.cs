using HealthCareApp.Models;
using HealthCareApp.Shared.Dtos.Doctors;
using HealthCareApp.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareApp.Tests.Helpers
{
    public static class DoctorTestData
    {
        
            public static List<Doctor> Doctors => new()
        {
            new Doctor
            {
                DoctorId = 1,
                DoctorName = "John Smith",
                Email = "john@test.com",
                Specialisation = SpecialisationType.Cardiologist,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IdentityUserId = "doctor1",
                IsActive = true,
                CreatedDate = DateTime.Now
            },

            new Doctor
            {
                DoctorId = 2,
                DoctorName = "Alice Brown",
                Email = "alice@test.com",
                Specialisation = SpecialisationType.Dermatologist,
                YearsOfExperience = 6,
                ConsultationFee = 700,
                IdentityUserId = "doctor2",
                IsActive = false,
                CreatedDate = DateTime.Now
            }
        };

            public static Doctor Doctor => Doctors.First();

            public static DoctorDto DoctorDto => new()
            {
                DoctorId = 1,
                FullName= "John Smith",
                Email = "john@test.com",
                Specialisation = SpecialisationType.Cardiologist,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };

            public static List<DoctorDto> DoctorDtos => new()
        {
            DoctorDto,
            new DoctorDto
            {
                DoctorId = 2,
                FullName = "Alice Brown",
                Email = "alice@test.com",
                Specialisation = SpecialisationType.Dermatologist,
                YearsOfExperience = 6,
                ConsultationFee = 700,
                IsActive = false
            }
        };

            public static CreateDoctorDto CreateDoctorDto => new()
            {
                FullName = "David Wilson",
                Email = "david@test.com",
                Specialisation = SpecialisationType.Neurologist,
                PracticeStartDate = DateTime.Today.AddYears(-8),
                ConsultationFee = 1000
            };

            public static UpdateDoctorDto UpdateDoctorDto => new()
            {
                FullName = "David Updated",
                Specialisation = SpecialisationType.Neurologist,
                PracticeStartDate = DateTime.Today.AddYears(-10),
                ConsultationFee = 1200
            };
        }
    }
