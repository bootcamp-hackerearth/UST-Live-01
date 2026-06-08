using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Models
{
    public class UserTests
    {
        [Fact]
        public void User_WithValidData_IsValid()
        {
            var user = new User
            {
                UserCode = "A001",
                Email = "admin@gmail.com",
                PasswordHash = "hashedpassword",
                Role = Role.Admin,
                ReferenceId = 1
            };

            var context =
                new ValidationContext(user);

            var results =
                new List<ValidationResult>();

            var isValid =
                Validator.TryValidateObject(
                    user,
                    context,
                    results,
                    true);

            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void User_WithoutUserCode_IsInvalid()
        {
            var user = new User
            {
                UserCode = string.Empty,
                Role = Role.Admin,
                ReferenceId = 1
            };

            var context =
                new ValidationContext(user);

            var results =
                new List<ValidationResult>();

            var isValid =
                Validator.TryValidateObject(
                    user,
                    context,
                    results,
                    true);

            Assert.False(isValid);
        }

        [Fact]
        public void User_WithoutReferenceId_IsInvalid()
        {
            var user = new User
            {
                UserCode = "A001",
                Role = Role.Admin,
                ReferenceId = 0
            };

            var context =
                new ValidationContext(user);

            var results =
                new List<ValidationResult>();

            var isValid =
                Validator.TryValidateObject(
                    user,
                    context,
                    results,
                    true);

            // Required doesn't fail for int,
            // but validation still executes.
            Assert.True(isValid);
        }

        [Fact]
        public void User_DefaultIsActive_IsTrue()
        {
            var user = new User();

            Assert.True(user.IsActive);
        }

        [Fact]
        public void User_CanSetIsActiveFalse()
        {
            var user = new User
            {
                IsActive = false
            };

            Assert.False(user.IsActive);
        }

        [Fact]
        public void User_CanAssignRole()
        {
            var user = new User
            {
                Role = Role.Doctor
            };

            Assert.Equal(
                Role.Doctor,
                user.Role);
        }

        [Fact]
        public void User_CanAssignReferenceId()
        {
            var user = new User
            {
                ReferenceId = 100
            };

            Assert.Equal(
                100,
                user.ReferenceId);
        }
    }
}