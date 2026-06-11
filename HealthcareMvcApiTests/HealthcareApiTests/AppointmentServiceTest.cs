using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using SharedClasses.Dtos;
using SharedClasses.Enums;
using HealthcareApi.Exceptions;
using HealthcareApi.Models;
using HealthcareApi.Repositories;
using HealthcareApi.Services.Implementations;

namespace HealthcareMvcApiTests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepoMock;
        private readonly Mock<IPatientRepository> _patientRepoMock;
        private readonly Mock<IDoctorRepository> _doctorRepoMock;
        private readonly Mock<IHealthRecordRepository> _healthRecordRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AppointmentService _sut;

        public AppointmentServiceTests()
        {
            _appointmentRepoMock = new Mock<IAppointmentRepository>();
            _patientRepoMock = new Mock<IPatientRepository>();
            _doctorRepoMock = new Mock<IDoctorRepository>();
            _healthRecordRepoMock = new Mock<IHealthRecordRepository>();
            _mapperMock = new Mock<IMapper>();

            _sut = new AppointmentService(
                _appointmentRepoMock.Object,
                _patientRepoMock.Object,
                _doctorRepoMock.Object,
                _healthRecordRepoMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public void BookAppointment_ValidRequest_ReturnsAppointmentDto()
        {
            // Arrange
            var bookingDto = new BookAppointmentDto
            {
                PatientId = 10,
                DoctorId = 20,
                ScheduledDate = DateTime.Today.AddDays(1)
            };

            var patient = new Patient { PatientId = 10 };
            var doctor = new Doctor { DoctorId = 20, IsActive = true, OffDays = new List<DateTime>() };

            var savedEntity = new Appointment(1, 10, 20, DateTime.Today.AddDays(1), 1, AppointmentStatus.Pending, "");
            var resultDto = new AppointmentDto { AppointmentId = 1, PatientId = 10, DoctorId = 20, SlotNumber = 1, Status = AppointmentStatus.Pending };

            _patientRepoMock.Setup(r => r.GetById(bookingDto.PatientId)).Returns(patient);
            _doctorRepoMock.Setup(r => r.GetById(bookingDto.DoctorId)).Returns(doctor);

            _appointmentRepoMock.Setup(r => r.PatientHasActiveAppointmentWithDoctorOnDate(
                bookingDto.PatientId, bookingDto.DoctorId, bookingDto.ScheduledDate.Date)).Returns(false);

            _appointmentRepoMock.Setup(r => r.IsSlotBooked(bookingDto.DoctorId, bookingDto.ScheduledDate.Date, 1)).Returns(false);
            _appointmentRepoMock.Setup(r => r.Add(It.IsAny<Appointment>())).Returns(savedEntity);
            _mapperMock.Setup(m => m.Map<AppointmentDto>(savedEntity)).Returns(resultDto);
            SetupPatientAndDoctorNames();

            // Act

            var result = _sut.BookAppointment(bookingDto);

            // Assert
            result.Should().NotBeNull();
            result.AppointmentId.Should().Be(1);
            result.SlotNumber.Should().Be(1);
            result.Status.Should().Be(AppointmentStatus.Pending);

            _appointmentRepoMock.Verify(r => r.Add(It.Is<Appointment>(a =>
                a.PatientId == 10 &&
                a.DoctorId == 20 &&
                a.SlotNumber == 1 &&
                a.Status == AppointmentStatus.Pending
            )), Times.Once);
        }

        [Fact]
        public void BookAppointment_InactiveDoctor_ThrowsAppointmentRuleException()
        {
            // Arrange
            var bookingDto = new BookAppointmentDto { PatientId = 10, DoctorId = 20, ScheduledDate = DateTime.Today.AddDays(1) };
            var patient = new Patient { PatientId = 10 };
            var doctor = new Doctor { DoctorId = 20, IsActive = false }; // Doctor is inactive

            _patientRepoMock.Setup(r => r.GetById(bookingDto.PatientId)).Returns(patient);
            _doctorRepoMock.Setup(r => r.GetById(bookingDto.DoctorId)).Returns(doctor);

            // Act
            Action act = () => _sut.BookAppointment(bookingDto);

            // Assert
            act.Should().Throw<AppointmentRuleException>()
               .WithMessage("Doctor is inactive and cannot accept appointments.");
        }

        [Fact]
        public void BookAppointment_AllSlotsBooked_ThrowsAppointmentRuleException()
        {
            // Arrange
            var bookingDto = new BookAppointmentDto { PatientId = 10, DoctorId = 20, ScheduledDate = DateTime.Today.AddDays(1) };
            var patient = new Patient { PatientId = 10 };
            var doctor = new Doctor { DoctorId = 20, IsActive = true, OffDays = new List<DateTime>() };

            _patientRepoMock.Setup(r => r.GetById(bookingDto.PatientId)).Returns(patient);
            _doctorRepoMock.Setup(r => r.GetById(bookingDto.DoctorId)).Returns(doctor);

            // All 10 slots booked
            _appointmentRepoMock.Setup(r => r.IsSlotBooked(bookingDto.DoctorId, bookingDto.ScheduledDate.Date, It.IsAny<int>())).Returns(true);

            // Act
            Action act = () => _sut.BookAppointment(bookingDto);

            // Assert
            act.Should().Throw<AppointmentRuleException>()
               .WithMessage("Doctor has reached the maximum number of appointments for this date.");
        }

        [Fact]
        public void BookAppointment_DuplicateAppointment_ThrowsAppointmentRuleException()
        {
            // Arrange
            var bookingDto = new BookAppointmentDto { PatientId = 10, DoctorId = 20, ScheduledDate = DateTime.Today.AddDays(1) };
            var patient = new Patient { PatientId = 10 };
            var doctor = new Doctor { DoctorId = 20, IsActive = true };

            _patientRepoMock.Setup(r => r.GetById(bookingDto.PatientId)).Returns(patient);
            _doctorRepoMock.Setup(r => r.GetById(bookingDto.DoctorId)).Returns(doctor);
            _appointmentRepoMock.Setup(r => r.PatientHasActiveAppointmentWithDoctorOnDate(
                bookingDto.PatientId, bookingDto.DoctorId, bookingDto.ScheduledDate.Date)).Returns(true);

            // Act
            Action act = () => _sut.BookAppointment(bookingDto);

            // Assert
            act.Should().Throw<AppointmentRuleException>()
               .WithMessage("Patient already has an active appointment with this doctor on the selected date.");
        }

        [Fact]
        public void DeleteAppointment_HasHealthRecord_ThrowsAppointmentRuleException()
        {
            // Arrange
            int appointmentId = 5;
            var appointment = new Appointment { AppointmentId = appointmentId };

            _appointmentRepoMock.Setup(r => r.GetById(appointmentId)).Returns(appointment);
            _healthRecordRepoMock.Setup(r => r.ExistsByAppointmentId(appointmentId)).Returns(true);

            // Act
            Action act = () => _sut.DeleteAppointment(appointmentId);

            // Assert
            act.Should().Throw<AppointmentRuleException>()
               .WithMessage("This appointment cannot be deleted because it has an associated health record.");

            _appointmentRepoMock.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void ConfirmAppointment_ValidPendingAppointment_ReturnsConfirmedAppointmentDto()
        {
            // Arrange
            int appointmentId = 8;
            var confirmDto = new ConfirmAppointmentDto { DoctorId = 20 };
            var doctor = new Doctor { DoctorId = 20 };
            var existingAppointment = new Appointment { AppointmentId = appointmentId, DoctorId = 20, Status = AppointmentStatus.Pending };

            _doctorRepoMock.Setup(r => r.GetById(confirmDto.DoctorId)).Returns(doctor);
            _appointmentRepoMock.Setup(r => r.GetById(appointmentId)).Returns(existingAppointment);
            _appointmentRepoMock.Setup(r => r.Update(appointmentId, It.IsAny<Appointment>())).Returns(existingAppointment);
            SetupPatientAndDoctorNames();

            // Act
            _sut.ConfirmAppointment(appointmentId, confirmDto);

            // Assert
            existingAppointment.Status.Should().Be(AppointmentStatus.Confirmed);
            _appointmentRepoMock.Verify(r => r.Update(appointmentId, It.Is<Appointment>(a => a.Status == AppointmentStatus.Confirmed)), Times.Once);
        }

        [Fact]
        public void ConfirmAppointment_WrongDoctorId_ThrowsAppointmentRuleException()
        {
            // Arrange
            int appointmentId = 8;
            var confirmDto = new ConfirmAppointmentDto { DoctorId = 99 }; // Wrong Doctor ID
            var doctor = new Doctor { DoctorId = 99 };
            var existingAppointment = new Appointment { AppointmentId = appointmentId, DoctorId = 20 };

            _doctorRepoMock.Setup(r => r.GetById(confirmDto.DoctorId)).Returns(doctor);
            _appointmentRepoMock.Setup(r => r.GetById(appointmentId)).Returns(existingAppointment);

            // Act
            Action act = () => _sut.ConfirmAppointment(appointmentId, confirmDto);

            // Assert
            act.Should().Throw<AppointmentRuleException>()
               .WithMessage("This appointment does not belong to the selected doctor.");
        }

        [Fact]
        public void CancelAppointmentByPatient_PastOrSameDayDate_ThrowsAppointmentRuleException()
        {
            // Arrange
            int appointmentId = 3;
            var cancelDto = new CancelByPatientDto { PatientId = 10, Reason = "Too busy" };
            var patient = new Patient { PatientId = 10 };
            var appointment = new Appointment
            {
                AppointmentId = appointmentId,
                PatientId = 10,
                ScheduledDate = DateTime.Today, // Today (Not allowed for patient cancellation)
                Status = AppointmentStatus.Pending
            };

            _patientRepoMock.Setup(r => r.GetById(cancelDto.PatientId)).Returns(patient);
            _appointmentRepoMock.Setup(r => r.GetById(appointmentId)).Returns(appointment);

            // Act
            Action act = () => _sut.CancelAppointmentByPatient(appointmentId, cancelDto);

            // Assert
            act.Should().Throw<AppointmentRuleException>()
               .WithMessage("Patients can cancel appointments only before the appointment date.");
        }

        [Fact]
        public void CancelAppointmentByDoctor_Success_UpdatesStatusAndAppendsTag()
        {
            // Arrange
            int appointmentId = 3;
            var cancelDto = new CancelByDoctorDto { DoctorId = 20, Reason = "Doctor Emergency" };
            var doctor = new Doctor { DoctorId = 20 };
            var appointment = new Appointment
            {
                AppointmentId = appointmentId,
                DoctorId = 20,
                ScheduledDate = DateTime.Today,
                Status = AppointmentStatus.Pending
            };

            _doctorRepoMock.Setup(r => r.GetById(cancelDto.DoctorId)).Returns(doctor);
            _appointmentRepoMock.Setup(r => r.GetById(appointmentId)).Returns(appointment);
            _appointmentRepoMock.Setup(r => r.Update(appointmentId, It.IsAny<Appointment>())).Returns(appointment);
            SetupPatientAndDoctorNames();

            // Act
            _sut.CancelAppointmentByDoctor(appointmentId, cancelDto);

            // Assert
            appointment.Status.Should().Be(AppointmentStatus.Cancelled);
            appointment.CancellationReason.Should().Be("Doctor Emergency - Cancelled by Doctor");
        }

        [Fact]
        public void CompleteAppointment_NotToday_ThrowsAppointmentRuleException()
        {
            // Arrange
            int appointmentId = 12;
            var completeDto = new CompleteAppointmentDto { DoctorId = 20 };
            var doctor = new Doctor { DoctorId = 20 };
            var appointment = new Appointment
            {
                AppointmentId = appointmentId,
                DoctorId = 20,
                ScheduledDate = DateTime.Today.AddDays(1), // Tomorrow's appointment
                Status = AppointmentStatus.Confirmed
            };

            _doctorRepoMock.Setup(r => r.GetById(completeDto.DoctorId)).Returns(doctor);
            _appointmentRepoMock.Setup(r => r.GetById(appointmentId)).Returns(appointment);

            // Act
            Action act = () => _sut.CompleteAppointment(appointmentId, completeDto);

            // Assert
            act.Should().Throw<AppointmentRuleException>()
               .WithMessage("Only appointments scheduled for today can be completed.");
        }
        private void SetupPatientAndDoctorNames(
            int patientId = 1,
            int doctorId = 1,
            string patientName = "Jane Doe",
            string doctorName = "Doctor Adams")
        {
            _patientRepoMock.Setup(r => r.GetById(patientId))
                .Returns(new Patient
                {
                    PatientId = patientId,
                    FullName = patientName
                });

            _doctorRepoMock.Setup(r => r.GetById(doctorId))
                .Returns(new Doctor
                {
                    DoctorId = doctorId,
                    FullName = doctorName,
                    IsActive = true
                });
        }
    }

}