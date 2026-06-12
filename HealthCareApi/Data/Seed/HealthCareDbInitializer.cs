using HealthCareApi.Data.Context;
using HealthCareApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace HealthCareApi.Data.Seed
{
    public class HealthCareDbInitializer : CreateDatabaseIfNotExists<HealthCareDbContext>
    {
        protected override void Seed(HealthCareDbContext context)
        {
            var users = new List<User>
    {
        new User { Email = "doc1@test.com", PasswordHash = "hash", Role = "Doctor" },
        new User { Email = "doc2@test.com", PasswordHash = "hash", Role = "Doctor" },
        new User { Email = "pat1@test.com", PasswordHash = "hash", Role = "Patient" },
        new User { Email = "pat2@test.com", PasswordHash = "hash", Role = "Patient" }
    };

            context.Users.AddRange(users);
            context.SaveChanges();

            // Doctors (1-to-1)
            var doctors = new List<Doctor>
    {
        new Doctor { UserId = users[0].UserId, FullName = "Dr. Arjun Mehta", Specialisation = "Cardiology", YearsOfExperience = 12, ConsultationFee = 800, IsActive = true, CreatedDate = DateTime.UtcNow },
        new Doctor { UserId = users[1].UserId, FullName = "Dr. Priya Ramesh", Specialisation = "Dermatology", YearsOfExperience = 8, ConsultationFee = 600, IsActive = true, CreatedDate = DateTime.UtcNow }
    };

            context.Doctors.AddRange(doctors);

            // Patients (1-to-many)
            var patients = new List<Patient>
    {
        new Patient { UserId = users[2].UserId, FullName = "Ananya Krishnan", DateOfBirth = new DateTime(1990, 4, 12), Gender = "Female", PhoneNumber = "9876543210", Email = "ananya@example.com", InsuranceId = "INS001", CreatedDate = DateTime.UtcNow },

        new Patient { UserId = users[2].UserId, FullName = "Second Patient", DateOfBirth = new DateTime(1995, 6, 10), Gender = "Male", PhoneNumber = "9999999999", Email = "second@example.com", InsuranceId = "INS004", CreatedDate = DateTime.UtcNow },

        new Patient { UserId = users[3].UserId, FullName = "Vikram Patel", DateOfBirth = new DateTime(1985, 9, 23), Gender = "Male", PhoneNumber = "9123456780", Email = "vikram@example.com", InsuranceId = "INS002", CreatedDate = DateTime.UtcNow }
    };

            context.Patients.AddRange(patients);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}