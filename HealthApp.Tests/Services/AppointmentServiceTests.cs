using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using HealthApp.ConsoleApp.Services;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Exceptions;

namespace HealthApp.Tests.Services
{
    // Test class for AppointmentService to validate appointment management functionalities
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _mockRepo;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _mockRepo = new Mock<IAppointmentRepository>();
            _service = new AppointmentService(_mockRepo.Object);
        }
        // MockDoctor class to simulate doctor availability for testing
        public bool IsAvailableOverride { get; set; } = true;

        public bool IsAvailable(DateTime date)
        {
            return IsAvailableOverride;
        }
        private static Patient GetSamplePatient(int id)
        {
            return new Patient
            {
                PatientId = id,
                Name = "Patient " + id,
                PhoneNumber = "9999999999",
                Email = "patient@test.com"
            };
        }

        private static Doctor GetSampleDoctor(int id, bool isAvailable = true)
        {
            var mockDoctor = new Mock<Doctor>();

            mockDoctor.Object.DoctorId = id;
            mockDoctor.Object.Name = "Doctor " + id;
            mockDoctor.Object.Specialisation = "General";

            mockDoctor.Setup(d => d.IsAvailable(It.IsAny<DateTime>()))
                    .Returns(isAvailable);

            return mockDoctor.Object;
        }

        private static Appointment GetSampleAppointment(int id)
        {
            return new Appointment
            {
                AppointmentId = id,
                Patient = GetSamplePatient(1),
                Doctor = GetSampleDoctor(1),
                ScheduledDate = DateTime.Now.AddDays(1),
                TimeSlot = "10:00 AM",
                Status = AppointmentStatus.Pending
            };
        }

        // BookAppointment - Success
        [Fact]
        public void BookAppointment_ShouldBookSuccessfully()
        {
            var patient = GetSamplePatient(1);
            var doctor = GetSampleDoctor(1, true);

            var appointments = new List<Appointment>();

            _mockRepo.Setup(r => r.GetAllAppointments()).Returns(appointments);
            _mockRepo.Setup(r => r.AddAppointment(It.IsAny<Appointment>()))
                     .Returns("added");

            var result = _service.BookAppointment(patient, doctor, DateTime.Now.AddDays(1), "10:00 AM");

            Assert.Contains("added", result);
        }

        // BookAppointment - Past Date
        [Fact]
        public void BookAppointment_ShouldThrow_WhenPastDate()
        {
            var patient = GetSamplePatient(1);
            var doctor = GetSampleDoctor(1);

            Assert.Throws<PastDateException>(() =>
                _service.BookAppointment(patient, doctor, DateTime.Now.AddDays(-1), "10:00 AM"));
        }

        // BookAppointment - Doctor Unavailable
        [Fact]
        public void BookAppointment_ShouldThrow_WhenDoctorUnavailable()
        {
            var patient = GetSamplePatient(1);
            var doctor = GetSampleDoctor(1, false);

            Assert.Throws<DoctorUnavailableException>(() =>
                _service.BookAppointment(patient, doctor, DateTime.Now.AddDays(1), "10:00 AM"));
        }

        // BookAppointment - Slot Taken
        [Fact]
        public void BookAppointment_ShouldThrow_WhenSlotTaken()
        {
            var patient = GetSamplePatient(1);
            var doctor = GetSampleDoctor(1, true);

            var existing = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 101,
                    Doctor = doctor,
                    Patient = patient,
                    ScheduledDate = DateTime.Now.AddDays(1),
                    TimeSlot = "10:00 AM",
                    Status = AppointmentStatus.Pending
                }
            };

            _mockRepo.Setup(r => r.GetAllAppointments()).Returns(existing);

            Assert.Throws<AppointmentConflictException>(() =>
                _service.BookAppointment(patient, doctor, DateTime.Now.AddDays(1), "10:00 AM"));
        }

        // GetAppointmentsByPatientId
        [Fact]
        public void GetAppointmentsByPatientId_ShouldReturnAppointments()
        {
            var list = new List<Appointment> { GetSampleAppointment(1) };

            _mockRepo.Setup(r => r.GetAppointmentsByPatientId(1)).Returns(list);

            var result = _service.GetAppointmentsByPatientId(1);

            Assert.Single(result);
        }

        // GetAppointmentsByPatientId - Exception
        [Fact]
        public void GetAppointmentsByPatientId_ShouldThrow_WhenEmpty()
        {
            _mockRepo.Setup(r => r.GetAppointmentsByPatientId(1))
                     .Returns(new List<Appointment>());

            Assert.Throws<AppointmentNotFoundException>(() =>
                _service.GetAppointmentsByPatientId(1));
        }

        // GetAppointmentsByDoctorId
        [Fact]
        public void GetAppointmentsByDoctorId_ShouldReturnAppointments()
        {
            var list = new List<Appointment> { GetSampleAppointment(1) };

            _mockRepo.Setup(r => r.GetAppointmentsByDoctorId(1)).Returns(list);

            var result = _service.GetAppointmentsByDoctorId(1);

            Assert.Single(result);
        }

        // GetAppointmentsByDoctorId - Exception
        [Fact]
        public void GetAppointmentsByDoctorId_ShouldThrow_WhenEmpty()
        {
            _mockRepo.Setup(r => r.GetAppointmentsByDoctorId(1))
                     .Returns(new List<Appointment>());

            Assert.Throws<AppointmentNotFoundException>(() =>
                _service.GetAppointmentsByDoctorId(1));
        }

        // GetAppointmentById
        [Fact]
        public void GetAppointmentById_ShouldReturnAppointment()
        {
            var appointment = GetSampleAppointment(1);

            _mockRepo.Setup(r => r.GetAppointmentById(1)).Returns(appointment);

            var result = _service.GetAppointmentById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.AppointmentId);
        }

        // GetAppointmentById - Exception
        [Fact]
        public void GetAppointmentById_ShouldThrow_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetAppointmentById(1)).Returns((Appointment?)null);

            Assert.Throws<AppointmentNotFoundException>(() =>
                _service.GetAppointmentById(1));
        }

        // CancelAppointment
        [Fact]
        public void CancelAppointment_ShouldCancel()
        {
            var appointment = GetSampleAppointment(1);

            _mockRepo.Setup(r => r.GetAppointmentById(1)).Returns(appointment);

            var result = _service.CancelAppointment(1, "Reason");

            Assert.Contains("cancelled successfully", result);
            Assert.Equal(AppointmentStatus.Cancelled, appointment.Status);
        }

        // ConfirmAppointment
        [Fact]
        public void ConfirmAppointment_ShouldConfirm()
        {
            var appointment = GetSampleAppointment(1);

            _mockRepo.Setup(r => r.GetAppointmentById(1)).Returns(appointment);

            var result = _service.ConfirmAppointment(1);

            Assert.Equal(AppointmentStatus.Confirmed, appointment.Status);
        }

        // GetUpcomingAppointments
        [Fact]
        public void GetUpcomingAppointments_ShouldReturnList()
        {
            var list = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    Patient = GetSamplePatient(1),
                    Doctor = GetSampleDoctor(1),
                    ScheduledDate = DateTime.Now.AddDays(1),
                    TimeSlot = "10 AM",
                    Status = AppointmentStatus.Confirmed
                }
            };

            _mockRepo.Setup(r => r.GetAllAppointments()).Returns(list);

            var result = _service.GetUpcomingAppointments();

            Assert.Single(result);
        }

        // UpdateAppointment
        [Fact]
        public void UpdateAppointment_ShouldUpdate()
        {
            var existing = GetSampleAppointment(1);
            var updated = GetSampleAppointment(1);

            _mockRepo.Setup(r => r.GetAppointmentById(1)).Returns(existing);
            _mockRepo.Setup(r => r.UpdateAppointment(existing, updated))
                     .Returns(updated);

            var result = _service.UpdateAppointment(updated);

            Assert.Equal(1, result.AppointmentId);
        }

        // UpdateAppointment - Exception
        [Fact]
        public void UpdateAppointment_ShouldThrow_WhenNotFound()
        {
            var appointment = GetSampleAppointment(999);

            _mockRepo.Setup(r => r.GetAppointmentById(999))
                     .Returns((Appointment?)null);

            Assert.Throws<AppointmentNotFoundException>(() =>
                _service.UpdateAppointment(appointment));
        }
    }
}