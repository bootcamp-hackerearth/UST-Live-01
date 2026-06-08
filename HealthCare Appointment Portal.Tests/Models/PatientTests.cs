using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Models
{
    public class PatientTests
    {
        [Fact]
        public void GetAge_ReturnsCorrectAge()
        {
            var patient = new Patient
            {
                DateOfBirth = DateTime.Today.AddYears(-25)
            };

            var result =
                patient.GetAge();

            Assert.Equal(25, result);
        }

        [Fact]
        public void GetAge_BeforeBirthday_ReturnsCorrectAge()
        {
            var patient = new Patient
            {
                DateOfBirth =
                    DateTime.Today.AddYears(-25)
                    .AddDays(1)
            };

            var result =
                patient.GetAge();

            Assert.Equal(24, result);
        }

        [Fact]
        public void ValidateDateOfBirth_WithFutureDate_ReturnsValidationError()
        {
            var result =
                Patient.ValidateDateOfBirth(
                    DateTime.Today.AddDays(1),
                    new ValidationContext(
                        new object()));

            Assert.NotEqual(
                ValidationResult.Success,
                result);
        }

        [Fact]
        public void ValidateDateOfBirth_WithValidDate_ReturnsSuccess()
        {
            var result =
                Patient.ValidateDateOfBirth(
                    DateTime.Today.AddYears(-20),
                    new ValidationContext(
                        new object()));

            Assert.Equal(
                ValidationResult.Success,
                result);
        }

        [Fact]
        public void Patient_WithValidData_IsValid()
        {
            var patient = new Patient
            {
                FullName = "John Doe",
                DateOfBirth =
                    new DateTime(
                        2000,
                        1,
                        1),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "john@gmail.com"
            };

            var context =
                new ValidationContext(
                    patient);

            var results =
                new List<ValidationResult>();

            var isValid =
                Validator.TryValidateObject(
                    patient,
                    context,
                    results,
                    true);

            Assert.True(
                isValid);

            Assert.Empty(
                results);
        }

        [Fact]
        public void Patient_WithoutFullName_IsInvalid()
        {
            var patient = new Patient
            {
                FullName = string.Empty,
                DateOfBirth =
                    new DateTime(
                        2000,
                        1,
                        1),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "john@gmail.com"
            };

            var context =
                new ValidationContext(
                    patient);

            var results =
                new List<ValidationResult>();

            var isValid =
                Validator.TryValidateObject(
                    patient,
                    context,
                    results,
                    true);

            Assert.False(
                isValid);
        }

        [Fact]
        public void Patient_WithInvalidEmail_IsInvalid()
        {
            var patient = new Patient
            {
                FullName = "John Doe",
                DateOfBirth =
                    new DateTime(
                        2000,
                        1,
                        1),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "invalid-email"
            };

            var context =
                new ValidationContext(
                    patient);

            var results =
                new List<ValidationResult>();

            var isValid =
                Validator.TryValidateObject(
                    patient,
                    context,
                    results,
                    true);

            Assert.False(
                isValid);
        }

        [Fact]
        public void Patient_WithFutureDateOfBirth_IsInvalid()
        {
            var patient = new Patient
            {
                FullName = "John Doe",
                DateOfBirth =
                    DateTime.Today.AddDays(1),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "john@gmail.com"
            };

            var context =
                new ValidationContext(
                    patient);

            var results =
                new List<ValidationResult>();

            var isValid =
                Validator.TryValidateObject(
                    patient,
                    context,
                    results,
                    true);

            Assert.False(
                isValid);
        }
    }
}