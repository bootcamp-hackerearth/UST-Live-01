using HealthCareApp.Models;
using HealthCareApp.Shared.Dtos.Patients;
using HealthCareApp.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareApp.Tests.Helpers
{
    public static class PatientTestData
    {
        public static Patient Patient => new()
        {
            PatientId = 1,
            PatientName = "John Smith",
            Email = "john@test.com",
            PhoneNumber = "9876543210",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderType.Male,
            InsuranceID = "INS001",
            CreatedDate = DateTime.Now
        };

        public static PatientDto PatientDto => new()
        {
            PatientId = 1,
            FullName = "John Smith",
            Email = "john@test.com",
            PhoneNumber = "9876543210",
            DateOfBirth = new DateTime(2000, 1, 1).ToString("yyyy-MM-dd"),
            Gender = GenderType.Male,
            InsuranceId = "INS001"
        };

        public static CreatePatientDto CreatePatientDto => new()
        {
            FullName = "John Smith",
            Email = "john@test.com",
            PhoneNumber = "9876543210",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderType.Male,
            InsuranceId = "INS001"
        };

        public static UpdatePatientDto UpdatePatientDto => new()
        {
            FullName = "John Smith Updated",
            Email = "john@test.com",
            PhoneNumber = "9999999999",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderType.Male,
            InsuranceId = "INS999"
        };
    }
}
