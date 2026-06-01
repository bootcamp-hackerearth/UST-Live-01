using HAP_Pod4_ConsoleApp_au.Exceptions;
using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repositories;
using HAP_Pod4_ConsoleApp_au.Services.Impl;
using Xunit;

namespace HealthAxisTests.ServiceTests
{
    public class AppointmentServiceTests
    {
        private readonly AppointmentRepository _repo;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _repo = new AppointmentRepository();

            _service =
                new AppointmentService(_repo);
        }

        [Fact]
        public void BookAppointment_ValidSlot_ShouldCreateAppointment()
        {
            Patient patient = new Patient
            {
                PatientId = 1,
                FullName = "Arun"
            };

            Doctor doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Dr. Priya"
            };

            Appointment appointment = new Appointment
            {
                AppointmentId = 1,
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = DateTime.Now.AddDays(1),
                TimeSlot =
                    Appointment.TimeSlotOption.TenAMToTwelvePM
            };

            var result =
                _service.BookAppointment(appointment);

            Assert.NotNull(result);

            Assert.Single(
                _repo.GetAllAppointments());
        }

        [Fact]
        public void BookAppointment_PastDate_ShouldThrowPastDateException()
        {
            Patient patient = new Patient
            {
                PatientId = 1,
                FullName = "Arun"
            };

            Doctor doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Dr. Priya"
            };

            Appointment appointment = new Appointment
            {
                AppointmentId = 2,
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = DateTime.Now.AddDays(-1),
                TimeSlot =
                    Appointment.TimeSlotOption.TwoPMToFourPM
            };

            Assert.Throws<AppointmentConflictException>(
                () => _service.BookAppointment(appointment));
        }

        [Fact]
        public void BookAppointment_SlotAlreadyTaken_ShouldThrowConflictException()
        {
            Patient patient1 = new Patient
            {
                PatientId = 1,
                FullName = "Arun"
            };

            Patient patient2 = new Patient
            {
                PatientId = 2,
                FullName = "Meera"
            };

            Doctor doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Dr. Priya"
            };

            Appointment firstAppointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Patient = patient1,
                    Doctor = doctor,
                    ScheduledDate = DateTime.Now.AddDays(1),
                    TimeSlot =
                        Appointment.TimeSlotOption
                        .TenAMToTwelvePM
                };

            _service.BookAppointment(firstAppointment);

            Appointment secondAppointment =
                new Appointment
                {
                    AppointmentId = 2,
                    Patient = patient2,
                    Doctor = doctor,
                    ScheduledDate =
                        firstAppointment.ScheduledDate,
                    TimeSlot =
                        Appointment.TimeSlotOption
                        .TenAMToTwelvePM
                };

            Assert.Throws<AppointmentConflictException>(
                () => _service.BookAppointment(secondAppointment));
        }

        [Fact]
        public void CancelAppointment_ExistingId_ShouldUpdateStatusToCancelled()
        {
            Patient patient = new Patient
            {
                PatientId = 1,
                FullName = "Arun"
            };

            Doctor doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Dr. Priya"
            };

            Appointment appointment = new Appointment
            {
                AppointmentId = 10,
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = DateTime.Now.AddDays(1),
                TimeSlot =
                    Appointment.TimeSlotOption
                    .FourPMToSixPM
            };

            _service.BookAppointment(appointment);

            bool result =
                _service.CancelAppointment(
                    10,
                    "Emergency");

            Assert.True(result);

            var updatedAppointment =
                _service.GetAppointmentById(10);
            Assert.NotNull(updatedAppointment);
            Assert.Equal(
                Appointment.StatusOption.Cancelled, updatedAppointment.Status);
        }

        [Fact]
        public void GetAppointmentsByPatient_ShouldReturnOnlyPatientAppointments()
        {
            Patient patient1 = new Patient
            {
                PatientId = 1,
                FullName = "Arun"
            };

            Patient patient2 = new Patient
            {
                PatientId = 2,
                FullName = "Meera"
            };

            Doctor doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Dr. Priya"
            };

            _service.BookAppointment(new Appointment
            {
                AppointmentId = 1,
                Patient = patient1,
                Doctor = doctor,
                ScheduledDate = DateTime.Now.AddDays(1),
                TimeSlot =
                    Appointment.TimeSlotOption
                    .TenAMToTwelvePM
            });

            _service.BookAppointment(new Appointment
            {
                AppointmentId = 2,
                Patient = patient2,
                Doctor = doctor,
                ScheduledDate = DateTime.Now.AddDays(2),
                TimeSlot =
                    Appointment.TimeSlotOption
                    .TwoPMToFourPM
            });

            var result =
                _service.GetAppointmentsBypatient(1);

            Assert.Single(result);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.Equal(
                "Arun",
                result[0].Patient.FullName);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}