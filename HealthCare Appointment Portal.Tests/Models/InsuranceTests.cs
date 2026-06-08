using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Models
{
    public class InsuranceTests
    {
        [Fact]
        public void Insurance_WithValidData_IsValid()
        {
            var insurance = new Insurance
            {
                PatientId = 1,
                ProviderName = "ABC Insurance",
                PolicyNumber = "POL123",
                CoverageAmount = 100000,
                ExpiryDate = DateTime.Today.AddDays(30),
                Status = InsuranceStatus.Active
            };

            var context =
                new ValidationContext(insurance);

            var results =
                new List<ValidationResult>();

            var isValid =
                Validator.TryValidateObject(
                    insurance,
                    context,
                    results,
                    true);

            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void Insurance_WithoutProviderName_IsInvalid()
        {
            var insurance = new Insurance
            {
                PatientId = 1,
                ProviderName = string.Empty,
                PolicyNumber = "POL123",
                CoverageAmount = 100000,
                ExpiryDate = DateTime.Today.AddDays(30),
                Status = InsuranceStatus.Active
            };

            var context =
                new ValidationContext(insurance);

            var results =
                new List<ValidationResult>();

            var isValid =
                Validator.TryValidateObject(
                    insurance,
                    context,
                    results,
                    true);

            Assert.False(isValid);
        }

        [Fact]
        public void Insurance_WithoutPolicyNumber_IsInvalid()
        {
            var insurance = new Insurance
            {
                PatientId = 1,
                ProviderName = "ABC Insurance",
                PolicyNumber = string.Empty,
                CoverageAmount = 100000,
                ExpiryDate = DateTime.Today.AddDays(30),
                Status = InsuranceStatus.Active
            };

            var context =
                new ValidationContext(insurance);

            var results =
                new List<ValidationResult>();

            var isValid =
                Validator.TryValidateObject(
                    insurance,
                    context,
                    results,
                    true);

            Assert.False(isValid);
        }

        [Fact]
        public void IsExpired_WhenExpiryDateIsPast_ReturnsTrue()
        {
            var insurance = new Insurance
            {
                ExpiryDate =
                    DateTime.Today.AddDays(-1)
            };

            var result =
                insurance.IsExpired();

            Assert.True(result);
        }

        [Fact]
        public void IsExpired_WhenExpiryDateIsToday_ReturnsFalse()
        {
            var insurance = new Insurance
            {
                ExpiryDate =
                    DateTime.Today
            };

            var result =
                insurance.IsExpired();

            Assert.False(result);
        }

        [Fact]
        public void IsExpired_WhenExpiryDateIsFuture_ReturnsFalse()
        {
            var insurance = new Insurance
            {
                ExpiryDate =
                    DateTime.Today.AddDays(10)
            };

            var result =
                insurance.IsExpired();

            Assert.False(result);
        }

        [Fact]
        public void IsActive_WhenStatusActiveAndNotExpired_ReturnsTrue()
        {
            var insurance = new Insurance
            {
                Status = InsuranceStatus.Active,
                ExpiryDate = DateTime.Today.AddDays(10)
            };

            var result =
                insurance.IsActive();

            Assert.True(result);
        }

        [Fact]
        public void IsActive_WhenStatusNotActive_ReturnsFalse()
        {
            var insurance = new Insurance
            {
                Status = InsuranceStatus.Pending,
                ExpiryDate = DateTime.Today.AddDays(10)
            };

            var result =
                insurance.IsActive();

            Assert.False(result);
        }

        [Fact]
        public void IsActive_WhenExpired_ReturnsFalse()
        {
            var insurance = new Insurance
            {
                Status = InsuranceStatus.Active,
                ExpiryDate = DateTime.Today.AddDays(-1)
            };

            var result =
                insurance.IsActive();

            Assert.False(result);
        }

        [Fact]
        public void DaysUntilExpiry_WhenFutureDate_ReturnsDays()
        {
            var insurance = new Insurance
            {
                ExpiryDate = DateTime.Today.AddDays(15)
            };

            var result =
                insurance.DaysUntilExpiry();

            Assert.Equal(15, result);
        }

        [Fact]
        public void DaysUntilExpiry_WhenToday_ReturnsZero()
        {
            var insurance = new Insurance
            {
                ExpiryDate = DateTime.Today
            };

            var result =
                insurance.DaysUntilExpiry();

            Assert.Equal(0, result);
        }

        [Fact]
        public void DaysUntilExpiry_WhenExpired_ReturnsZero()
        {
            var insurance = new Insurance
            {
                ExpiryDate = DateTime.Today.AddDays(-10)
            };

            var result =
                insurance.DaysUntilExpiry();

            Assert.Equal(0, result);
        }
    }
}