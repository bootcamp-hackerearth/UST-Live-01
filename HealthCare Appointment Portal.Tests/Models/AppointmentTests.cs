using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Models
{
    public class AppointmentTests
    {
        [Fact]
        public void Appointment_WithValidData_IsValid()
        {
            var appointment = new Appointment
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM",
                Status = AppointmentStatus.Pending
            };

            var context =
                new ValidationContext(appointment);

            var results =
                new List<ValidationResult>();

            var isValid =
                Validator.TryValidateObject(
                    appointment,
                    context,
                    results,
                    true);

            Assert.True(isValid);
        }

        [Fact]
        public void Confirm_SetsStatusToConfirmed()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending
            };

            appointment.Confirm();

            Assert.Equal(
                AppointmentStatus.Confirmed,
                appointment.Status);
        }

        [Fact]
        public void Cancel_SetsStatusToCancelled()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending
            };

            appointment.Cancel("Patient Request");

            Assert.Equal(
                AppointmentStatus.Cancelled,
                appointment.Status);

            Assert.Equal(
                "Patient Request",
                appointment.CancellationReason);
        }

        [Fact]
        public void Complete_SetsStatusToCompleted()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Confirmed
            };

            appointment.Complete();

            Assert.Equal(
                AppointmentStatus.Completed,
                appointment.Status);
        }

        [Fact]
        public void IsUpcoming_WhenFutureDate_ReturnsTrue()
        {
            var appointment = new Appointment
            {
                ScheduledDate =
                    DateTime.Today.AddDays(1),
                Status =
                    AppointmentStatus.Pending
            };

            var result =
                appointment.IsUpcoming();

            Assert.True(result);
        }

        [Fact]
        public void IsUpcoming_WhenTodayDate_ReturnsTrue()
        {
            var appointment = new Appointment
            {
                ScheduledDate =
                    DateTime.Today,
                Status =
                    AppointmentStatus.Confirmed
            };

            var result =
                appointment.IsUpcoming();

            Assert.True(result);
        }

        [Fact]
        public void IsUpcoming_WhenCancelled_ReturnsFalse()
        {
            var appointment = new Appointment
            {
                ScheduledDate =
                    DateTime.Today.AddDays(1),
                Status =
                    AppointmentStatus.Cancelled
            };

            var result =
                appointment.IsUpcoming();

            Assert.False(result);
        }

        [Fact]
        public void IsUpcoming_WhenPastDate_ReturnsFalse()
        {
            var appointment = new Appointment
            {
                ScheduledDate =
                    DateTime.Today.AddDays(-1),
                Status =
                    AppointmentStatus.Confirmed
            };

            var result =
                appointment.IsUpcoming();

            Assert.False(result);
        }

        [Fact]
        public void IsCancelled_WhenCancelled_ReturnsTrue()
        {
            var appointment = new Appointment
            {
                Status =
                    AppointmentStatus.Cancelled
            };

            var result =
                appointment.IsCancelled();

            Assert.True(result);
        }

        [Fact]
        public void IsCancelled_WhenNotCancelled_ReturnsFalse()
        {
            var appointment = new Appointment
            {
                Status =
                    AppointmentStatus.Pending
            };

            var result =
                appointment.IsCancelled();

            Assert.False(result);
        }

        [Fact]
        public void IsCompleted_WhenCompleted_ReturnsTrue()
        {
            var appointment = new Appointment
            {
                Status =
                    AppointmentStatus.Completed
            };

            var result =
                appointment.IsCompleted();

            Assert.True(result);
        }

        [Fact]
        public void IsCompleted_WhenNotCompleted_ReturnsFalse()
        {
            var appointment = new Appointment
            {
                Status =
                    AppointmentStatus.Confirmed
            };

            var result =
                appointment.IsCompleted();

            Assert.False(result);
        }

        [Fact]
        public void Appointment_WithoutTimeSlot_IsInvalid()
        {
            var appointment = new Appointment
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today,
                TimeSlot = string.Empty,
                Status = AppointmentStatus.Pending
            };

            var context =
                new ValidationContext(
                    appointment);

            var results =
                new List<ValidationResult>();

            var isValid =
                Validator.TryValidateObject(
                    appointment,
                    context,
                    results,
                    true);

            Assert.False(isValid);
        }
    }
}