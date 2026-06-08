using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Models
{
    public class DoctorTests
    {
        [Fact]
        public void Doctor_WithValidData_IsValid()
        {
            var doctor = new Doctor
            {
                FullName = "John Doe",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };

            var context =
                new ValidationContext(doctor);

            var results =
                new List<ValidationResult>();

            var isValid =
                Validator.TryValidateObject(
                    doctor,
                    context,
                    results,
                    true);

            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void Doctor_WithoutFullName_IsInvalid()
        {
            var doctor = new Doctor
            {
                FullName = "",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 10,
                ConsultationFee = 500
            };

            var context =
                new ValidationContext(doctor);

            var results =
                new List<ValidationResult>();

            var isValid =
                Validator.TryValidateObject(
                    doctor,
                    context,
                    results,
                    true);

            Assert.False(isValid);
        }

        [Fact]
        public void Activate_SetsIsActiveTrue()
        {
            var doctor = new Doctor
            {
                IsActive = false
            };

            doctor.Activate();

            Assert.True(
                doctor.IsActive);
        }

        [Fact]
        public void Deactivate_SetsIsActiveFalse()
        {
            var doctor = new Doctor
            {
                IsActive = true
            };

            doctor.Deactivate();

            Assert.False(
                doctor.IsActive);
        }

        [Fact]
        public void IsAvailable_WhenNoAppointments_ReturnsTrue()
        {
            var doctor = new Doctor();

            var result =
                doctor.IsAvailable(
                    DateTime.Today,
                    "10:00 AM");

            Assert.True(result);
        }

        [Fact]
        public void IsAvailable_WhenAppointmentExists_ReturnsFalse()
        {
            var doctor = new Doctor
            {
                Appointments =
                new List<Appointment>
                {
                    new Appointment
                    {
                        ScheduledDate =
                            DateTime.Today,
                        TimeSlot =
                            "10:00 AM",
                        Status =
                            AppointmentStatus.Confirmed
                    }
                }
            };

            var result =
                doctor.IsAvailable(
                    DateTime.Today,
                    "10:00 AM");

            Assert.False(result);
        }

        [Fact]
        public void IsAvailable_WhenAppointmentCancelled_ReturnsTrue()
        {
            var doctor = new Doctor
            {
                Appointments =
                new List<Appointment>
                {
                    new Appointment
                    {
                        ScheduledDate =
                            DateTime.Today,
                        TimeSlot =
                            "10:00 AM",
                        Status =
                            AppointmentStatus.Cancelled
                    }
                }
            };

            var result =
                doctor.IsAvailable(
                    DateTime.Today,
                    "10:00 AM");

            Assert.True(result);
        }

        [Fact]
        public void GetUpcomingAppointmentCount_ReturnsCount()
        {
            var doctor = new Doctor
            {
                Appointments =
                new List<Appointment>
                {
                    new Appointment
                    {
                        ScheduledDate =
                            DateTime.Today.AddDays(1),
                        Status =
                            AppointmentStatus.Confirmed
                    },
                    new Appointment
                    {
                        ScheduledDate =
                            DateTime.Today.AddDays(2),
                        Status =
                            AppointmentStatus.Pending
                    },
                    new Appointment
                    {
                        ScheduledDate =
                            DateTime.Today.AddDays(3),
                        Status =
                            AppointmentStatus.Cancelled
                    }
                }
            };

            var result =
                doctor.GetUpcomingAppointmentCount();

            Assert.Equal(2, result);
        }

        [Fact]
        public void GetUpcomingAppointmentCount_ReturnsZero()
        {
            var doctor = new Doctor
            {
                Appointments =
                new List<Appointment>
                {
                    new Appointment
                    {
                        ScheduledDate =
                            DateTime.Today.AddDays(-1),
                        Status =
                            AppointmentStatus.Completed
                    },
                    new Appointment
                    {
                        ScheduledDate =
                            DateTime.Today.AddDays(1),
                        Status =
                            AppointmentStatus.Cancelled
                    }
                }
            };

            var result =
                doctor.GetUpcomingAppointmentCount();

            Assert.Equal(0, result);
        }

        [Fact]
        public void GetUpcomingAppointmentCount_CountsTodayAppointments()
        {
            var doctor = new Doctor
            {
                Appointments =
                new List<Appointment>
                {
                    new Appointment
                    {
                        ScheduledDate =
                            DateTime.Today,
                        Status =
                            AppointmentStatus.Confirmed
                    }
                }
            };

            var result =
                doctor.GetUpcomingAppointmentCount();

            Assert.Equal(1, result);
        }
    }
}