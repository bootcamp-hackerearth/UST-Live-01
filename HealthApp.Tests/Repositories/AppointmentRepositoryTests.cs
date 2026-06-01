using System;
using System.Collections.Generic;
using Xunit;
using HealthApp.ConsoleApp.Repositories;
using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Databases;

namespace HealthApp.Tests.Repositories
{
    public class AppointmentRepositoryTests
    {
        private readonly AppointmentDb _appointmentDb;
        private readonly AppointmentRepository _repository;

        public AppointmentRepositoryTests()
        {
            _appointmentDb = new AppointmentDb
            {
                Appointments = new List<Appointment>()
            };

            _repository = new AppointmentRepository(_appointmentDb);
        }

        private static Patient GetSamplePatient(int id)
        {
            return new Patient
            {
                PatientId = id,
                FullName = "Patient " + id,
                PhoneNumber = "9999999999",
                Email = "patient@test.com",
                InsuranceId = "sadf23423"
            };
        }

        private static Doctor GetSampleDoctor(int id)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = "Doctor " + id,
                Specialisation = "General"
            };
        }

        // AddAppointment
        [Fact]
        public void AddAppointment_ShouldAddAppointment()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Patient = GetSamplePatient(1),
                Doctor = GetSampleDoctor(1),
                ScheduledDate = DateTime.Now.Date,
                TimeSlot = "10:00 AM"
            };

            _repository.AddAppointment(appointment);


            Assert.Single(_appointmentDb.Appointments);
            Assert.Equal(appointment, _appointmentDb.Appointments[0]);
        }

        // GetAllAppointments
        [Fact]
        public void GetAllAppointments_ShouldReturnAllAppointments()
        {
            _appointmentDb.Appointments.Add(new Appointment
            {
                AppointmentId = 1,
                Patient = GetSamplePatient(1),
                Doctor = GetSampleDoctor(1),
                ScheduledDate = DateTime.Now,
                TimeSlot = "9:00 AM"
            });

            _appointmentDb.Appointments.Add(new Appointment
            {
                AppointmentId = 2,
                Patient = GetSamplePatient(2),
                Doctor = GetSampleDoctor(2),
                ScheduledDate = DateTime.Now,
                TimeSlot = "11:00 AM"
            });

            var result = _repository.GetAllAppointments();

            Assert.Equal(2, result.Count);
        }

        // GetAppointmentById
        [Fact]
        public void GetAppointmentById_ShouldReturnAppointment()
        {
            _appointmentDb.Appointments.Add(new Appointment
            {
                AppointmentId = 1,
                Patient = GetSamplePatient(1),
                Doctor = GetSampleDoctor(1),
                ScheduledDate = DateTime.Now,
                TimeSlot = "10:00 AM"
            });

            var result = _repository.GetAppointmentById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.AppointmentId);
        }

        // UpdateAppointment
        [Fact]
        public void UpdateAppointment_ShouldUpdateAppointmentDetails()
        {
            var existing = new Appointment
            {
                AppointmentId = 1,
                Patient = GetSamplePatient(1),
                Doctor = GetSampleDoctor(1),
                ScheduledDate = DateTime.Now.AddDays(-1),
                TimeSlot = "9:00 AM"
            };

            var updated = new Appointment
            {
                AppointmentId = 1,
                Patient = GetSamplePatient(2),
                Doctor = GetSampleDoctor(2),
                ScheduledDate = DateTime.Now,
                TimeSlot = "2:00 PM"
            };

            var result = _repository.UpdateAppointment(existing, updated);

            Assert.Equal("Patient 2", result.Patient.FullName);
            Assert.Equal("Doctor 2", result.Doctor.FullName);
            Assert.Equal("2:00 PM", result.TimeSlot);
        }

        // GetAppointmentsByPatientId
        [Fact]
        public void GetAppointmentsByPatientId_ShouldReturnMatchingAppointments()
        {
            var patient = GetSamplePatient(1);

            _appointmentDb.Appointments.Add(new Appointment
            {
                AppointmentId = 1,
                Patient = patient,
                Doctor = GetSampleDoctor(1),
                ScheduledDate = DateTime.Now,
                TimeSlot = "10:00 AM"
            });

            _appointmentDb.Appointments.Add(new Appointment
            {
                AppointmentId = 2,
                Patient = GetSamplePatient(2),
                Doctor = GetSampleDoctor(2),
                ScheduledDate = DateTime.Now,
                TimeSlot = "11:00 AM"
            });

            var result = _repository.GetAppointmentsByPatientId(1);

            Assert.Single(result);
        }

        // GetAppointmentsByDoctorId
        [Fact]
        public void GetAppointmentsByDoctorId_ShouldReturnMatchingAppointments()
        {
            var doctor = GetSampleDoctor(1);

            _appointmentDb.Appointments.Add(new Appointment
            {
                AppointmentId = 1,
                Patient = GetSamplePatient(1),
                Doctor = doctor,
                ScheduledDate = DateTime.Now,
                TimeSlot = "10:00 AM"
            });

            _appointmentDb.Appointments.Add(new Appointment
            {
                AppointmentId = 2,
                Patient = GetSamplePatient(2),
                Doctor = GetSampleDoctor(2),
                ScheduledDate = DateTime.Now,
                TimeSlot = "11:00 AM"
            });

            var result = _repository.GetAppointmentsByDoctorId(1);

            Assert.Single(result);
        }
    }
}