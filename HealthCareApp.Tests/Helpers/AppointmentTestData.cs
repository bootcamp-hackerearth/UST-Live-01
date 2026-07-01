using HealthCareApp.Models;
using HealthCareApp.Shared.Dtos.Appointments;
using HealthCareApp.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareApp.Tests.Helpers
{
    public static class AppointmentTestData
    {
        public static Appointment Appointment => new()
        {
            AppointmentId = 1,
            PatientId = 1,
            DoctorId = 1,
            ScheduledDate = DateTime.Today.AddDays(2),
            TimeSlot = "09:00 AM - 09:30 AM",
            Status = AppointmentStatus.Pending,
            CreatedDate = DateTime.Today,

            Patient = new Patient
            {
                PatientId = 1,
                PatientName = "John Doe",
                Email = "john@test.com",
                PhoneNumber = "9876543210"
            },

            Doctor = new Doctor
            {
                DoctorId = 1,
                DoctorName = "Dr Smith",
                Email = "smith@test.com"
            }
        };

        public static List<Appointment> AppointmentList =>
        [
            Appointment,

            new Appointment
            {
                AppointmentId = 2,
                PatientId = 2,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(3),
                TimeSlot = "10:00 AM - 10:30 AM",
                Status = AppointmentStatus.Confirmed,
                CreatedDate = DateTime.Today,

                Patient = new Patient
                {
                    PatientId = 2,
                    PatientName = "Alice",
                    Email = "alice@test.com",
                    PhoneNumber = "1234567890"
                },

                Doctor = new Doctor
                {
                    DoctorId = 1,
                    DoctorName = "Dr Smith"
                }
            }
        ];

        public static AppointmentDto AppointmentDto => new()
        {
            AppointmentId = 1,
            PatientId = 1,
            PatientName = "John Doe",
            DoctorId = 1,
            DoctorName = "Dr Smith",
            ScheduledDate = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd"),
            TimeSlot = "09:00 AM - 09:30 AM",
            Status = AppointmentStatus.Pending
        };

        public static List<AppointmentDto> AppointmentDtoList =>
        [
            AppointmentDto
        ];

        public static BookAppointmentDto BookAppointmentDto => new()
        {
            PatientId = 1,
            DoctorId = 1,
            ScheduledDate = DateTime.Today.AddDays(2),
            TimeSlot = "09:00 AM - 09:30 AM"
        };

        public static UpdateAppointmentDto UpdateAppointmentDto => new()
        {
            AppointmentId = "1",
            PatientId = 1,
            DoctorId = 1,
            ScheduledDate = DateTime.Today.AddDays(5),
            TimeSlot = "11:00 AM - 11:30 AM"
        };

        public static CancelAppointmentDto CancelAppointmentDto => new()
        {
            AppointmentId = 1,
            Reason = "Patient is not available."
        };

        public static ConfirmAppointmentDto ConfirmAppointmentDto => new()
        {
            DoctorId = 1
        };

        public static CompleteAppointmentDto CompleteAppointmentDto => new()
        {
            DoctorId = 1
        };
    }

}
