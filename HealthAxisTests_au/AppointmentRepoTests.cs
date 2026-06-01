using System;
using System.Collections.Generic;
using HAP_Pod4_ConsoleApp_au.Data;
using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repositories;
using Xunit;

namespace HealthAxisTests.RepositoryTests
{
    public class AppointmentRepositoryTests
    {
        private readonly AppDbContext _dbcontext;
        private readonly AppointmentRepository _repo;

        public AppointmentRepositoryTests()
        {
            _dbcontext = new AppDbContext();
            _repo = new AppointmentRepository();
        }

        [Fact]
        public void BookAppointment_ShouldAddAppointment()
        {
            Appointment appointment = new Appointment
            {
                AppointmentId = _dbcontext.GetNextAppointmentId(),
                Patient = _dbcontext.Patients[0],
                Doctor = _dbcontext.Doctors[0],
                ScheduledDate = DateTime.Now.AddDays(1),
                TimeSlot = Appointment.TimeSlotOption.TenAMToTwelvePM
            };

            var result = _repo.BookAppointment(appointment);

            Assert.NotNull(result);
            Assert.Single(_repo.GetAllAppointments());
        }

        [Fact]
        public void GetAppointmentById_ShouldReturnAppointment()
        {
            Appointment appointment = new Appointment
            {
                AppointmentId = 1,
                Patient = _dbcontext.Patients[0],
                Doctor = _dbcontext.Doctors[0],
                ScheduledDate = DateTime.Now.AddDays(1),
                TimeSlot = Appointment.TimeSlotOption.TwoPMToFourPM
            };

            _repo.BookAppointment(appointment);

            var result = _repo.GetAppointmentById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.AppointmentId);
        }

        [Fact]
        public void GetAppointmentById_WhenNotExists_ShouldReturnNull()
        {
            var result = _repo.GetAppointmentById(999);

            Assert.Null(result);
        }

        [Fact]
        public void CancelAppointment_ShouldUpdateStatus()
        {
            Appointment appointment = new Appointment
            {
                AppointmentId = 1,
                Patient = _dbcontext.Patients[0],
                Doctor = _dbcontext.Doctors[0],
                ScheduledDate = DateTime.Now.AddDays(1),
                TimeSlot = Appointment.TimeSlotOption.FourPMToSixPM
            };

            _repo.BookAppointment(appointment);

            bool result = _repo.CancelAppointment(1, "Emergency");

            Assert.True(result);

            var updated = _repo.GetAppointmentById(1);

            Assert.NotNull(updated);
            Assert.Equal(Appointment.StatusOption.Cancelled, updated.Status);
        }

        [Fact]
        public void CancelAppointment_WhenAppointmentDoesNotExist_ShouldReturnFalse()
        {
            bool result = _repo.CancelAppointment(999, "Not feeling well");

            Assert.False(result);
        }

        [Fact]
        public void GetAppointmentsByPatient_ShouldReturnOnlyPatientAppointments()
        {
            // Arrange
            _repo.BookAppointment(new Appointment { AppointmentId = 1, Patient = new Patient { PatientId = 10 } });
            _repo.BookAppointment(new Appointment { AppointmentId = 2, Patient = new Patient { PatientId = 20 } });
            _repo.BookAppointment(new Appointment { AppointmentId = 3, Patient = new Patient { PatientId = 10 } });

            // Act
            var result = _repo.GetAppointmentsByPatient(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, a => Assert.Equal(10, a.Patient!.PatientId));
        }

        [Fact]
        public void GetAppointmentsByPatient_WhenNoAppointments_ShouldReturnEmptyList()
        {
            // Act
            var result = _repo.GetAppointmentsByPatient(999);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetAppointmentsByDoctor_ShouldReturnOnlyDoctorAppointments()
        {
            // Arrange
            _repo.BookAppointment(new Appointment { AppointmentId = 1, Doctor = new Doctor { DoctorId = 5 } });
            _repo.BookAppointment(new Appointment { AppointmentId = 2, Doctor = new Doctor { DoctorId = 99 } });
            _repo.BookAppointment(new Appointment { AppointmentId = 3, Doctor = new Doctor { DoctorId = 5 } });

            // Act
            var result = _repo.GetAppointmentsByDoctor(5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, a => Assert.Equal(5, a.Doctor!.DoctorId));
        }

        [Fact]
        public void GetAppointmentsByDoctor_WhenNoAppointments_ShouldReturnEmptyList()
        {
            // Act
            var result = _repo.GetAppointmentsByDoctor(999);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetAllAppointments_ShouldReturnAllAddedAppointments()
        {
            // Arrange
            _repo.BookAppointment(new Appointment { AppointmentId = 1 });
            _repo.BookAppointment(new Appointment { AppointmentId = 2 });

            // Act
            var result = _repo.GetAllAppointments();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}