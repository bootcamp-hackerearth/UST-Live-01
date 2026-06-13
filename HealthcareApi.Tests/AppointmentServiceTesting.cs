using HealthcareApi.Exceptions;
using HealthcareApi.Models;
using HealthcareApi.Repositories;
using HealthcareApi.Services.Implementations;
using Moq;
using SharedClasses.Dtos;
using SharedClasses.Enums;
using System;
using System.Collections.Generic;
using Xunit;

namespace HealthcareApi.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IHealthRecordRepository> _healthRecordRepositoryMock;

        private readonly AppointmentService _appointmentService;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();

            _appointmentService = new AppointmentService(
                _appointmentRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _healthRecordRepositoryMock.Object);
        }

        [Fact]
        public void GetAllAppointments_WhenAppointmentsExist_ShouldReturnAppointmentDtos()
        {
            // Arrange
            List<Appointment> appointments = new List<Appointment>
            {
                CreateAppointment()
            };

            _appointmentRepositoryMock
                .Setup(r => r.GetAll())
                .Returns(appointments);

            // Act
            List<AppointmentDto> result = _appointmentService.GetAllAppointments();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(10, result[0].AppointmentId);
            Assert.Equal(1, result[0].PatientId);
            Assert.Equal(2, result[0].DoctorId);
        }

        [Fact]
        public void GetAppointmentById_WhenAppointmentExists_ShouldReturnAppointmentDto()
        {
            // Arrange
            Appointment appointment = CreateAppointment();

            _appointmentRepositoryMock
                .Setup(r => r.GetById(10))
                .Returns(appointment);

            // Act
            AppointmentDto result = _appointmentService.GetAppointmentById(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.AppointmentId);
            Assert.Equal(1, result.PatientId);
            Assert.Equal(2, result.DoctorId);
        }

        [Fact]
        public void GetAppointmentById_WhenAppointmentDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(r => r.GetById(99))
                .Returns((Appointment)null);

            // Act
            EntityNotFoundException exception = Assert.Throws<EntityNotFoundException>(() =>
                _appointmentService.GetAppointmentById(99));

            // Assert
            Assert.Contains("Appointment", exception.Message);
        }

        [Fact]
        public void BookAppointment_WhenDtoIsNull_ShouldThrowAppointmentRuleException()
        {
            // Act
            AppointmentRuleException exception = Assert.Throws<AppointmentRuleException>(() =>
                _appointmentService.BookAppointment(null));

            // Assert
            Assert.Equal("Appointment details are required.", exception.Message);
        }

        [Fact]
        public void BookAppointment_WhenPatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            BookAppointmentDto dto = CreateBookAppointmentDto();

            _patientRepositoryMock
                .Setup(r => r.GetById(dto.PatientId))
                .Returns((Patient)null);

            // Act
            EntityNotFoundException exception = Assert.Throws<EntityNotFoundException>(() =>
                _appointmentService.BookAppointment(dto));

            // Assert
            Assert.Contains("Patient", exception.Message);
        }

        [Fact]
        public void BookAppointment_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            BookAppointmentDto dto = CreateBookAppointmentDto();

            SetupValidPatient(dto.PatientId);

            _doctorRepositoryMock
                .Setup(r => r.GetById(dto.DoctorId))
                .Returns((Doctor)null);

            // Act
            EntityNotFoundException exception = Assert.Throws<EntityNotFoundException>(() =>
                _appointmentService.BookAppointment(dto));

            // Assert
            Assert.Contains("Doctor", exception.Message);
        }

        [Fact]
        public void BookAppointment_WhenDateIsInPast_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            BookAppointmentDto dto = CreateBookAppointmentDto();
            dto.ScheduledDate = DateTime.Today.AddDays(-1);

            SetupValidPatient(dto.PatientId);
            SetupValidDoctor(dto.DoctorId);

            // Act
            AppointmentRuleException exception = Assert.Throws<AppointmentRuleException>(() =>
                _appointmentService.BookAppointment(dto));

            // Assert
            Assert.Equal("Appointment date cannot be in the past.", exception.Message);
        }

        [Fact]
        public void BookAppointment_WhenDateIsAfterThirtyDays_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            BookAppointmentDto dto = CreateBookAppointmentDto();
            dto.ScheduledDate = DateTime.Today.AddDays(31);

            SetupValidPatient(dto.PatientId);
            SetupValidDoctor(dto.DoctorId);

            // Act
            AppointmentRuleException exception = Assert.Throws<AppointmentRuleException>(() =>
                _appointmentService.BookAppointment(dto));

            // Assert
            Assert.Equal("Appointment date must be within the next 30 days.", exception.Message);
        }

        [Fact]
        public void BookAppointment_WhenSlotNumberIsLessThanOne_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            BookAppointmentDto dto = CreateBookAppointmentDto();
            dto.SlotNumber = 0;

            SetupValidPatient(dto.PatientId);
            SetupValidDoctor(dto.DoctorId);

            // Act
            AppointmentRuleException exception = Assert.Throws<AppointmentRuleException>(() =>
                _appointmentService.BookAppointment(dto));

            // Assert
            Assert.Equal("Please select a valid appointment time slot.", exception.Message);
        }

        [Fact]
        public void BookAppointment_WhenSlotNumberIsGreaterThanSixteen_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            BookAppointmentDto dto = CreateBookAppointmentDto();
            dto.SlotNumber = 17;

            SetupValidPatient(dto.PatientId);
            SetupValidDoctor(dto.DoctorId);

            // Act
            AppointmentRuleException exception = Assert.Throws<AppointmentRuleException>(() =>
                _appointmentService.BookAppointment(dto));

            // Assert
            Assert.Equal("Please select a valid appointment time slot.", exception.Message);
        }

        [Fact]
        public void BookAppointment_WhenDoctorIsInactive_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            BookAppointmentDto dto = CreateBookAppointmentDto();

            SetupValidPatient(dto.PatientId);

            Doctor inactiveDoctor = CreateDoctor(dto.DoctorId);
            inactiveDoctor.IsActive = false;

            _doctorRepositoryMock
                .Setup(r => r.GetById(dto.DoctorId))
                .Returns(inactiveDoctor);

            // Act
            AppointmentRuleException exception = Assert.Throws<AppointmentRuleException>(() =>
                _appointmentService.BookAppointment(dto));

            // Assert
            Assert.Equal("Doctor is inactive and cannot accept appointments.", exception.Message);
        }

        [Fact]
        public void BookAppointment_WhenPatientAlreadyHasSameSlotWithAnyDoctor_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            BookAppointmentDto dto = CreateBookAppointmentDto();

            SetupValidPatient(dto.PatientId);
            SetupValidDoctor(dto.DoctorId);

            _appointmentRepositoryMock
                .Setup(r => r.PatientHasActiveAppointmentOnDateAndSlot(
                    dto.PatientId,
                    dto.ScheduledDate.Date,
                    dto.SlotNumber))
                .Returns(true);

            // Act
            AppointmentRuleException exception = Assert.Throws<AppointmentRuleException>(() =>
                _appointmentService.BookAppointment(dto));

            // Assert
            Assert.Equal(
                "Patient already has an appointment during the selected time slot.",
                exception.Message);
        }

        [Fact]
        public void BookAppointment_WhenPatientAlreadyHasAppointmentWithSameDoctorOnSameDate_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            BookAppointmentDto dto = CreateBookAppointmentDto();

            SetupValidPatient(dto.PatientId);
            SetupValidDoctor(dto.DoctorId);

            _appointmentRepositoryMock
                .Setup(r => r.PatientHasActiveAppointmentOnDateAndSlot(
                    dto.PatientId,
                    dto.ScheduledDate.Date,
                    dto.SlotNumber))
                .Returns(false);

            _appointmentRepositoryMock
                .Setup(r => r.PatientHasActiveAppointmentWithDoctorOnDate(
                    dto.PatientId,
                    dto.DoctorId,
                    dto.ScheduledDate.Date))
                .Returns(true);

            // Act
            AppointmentRuleException exception = Assert.Throws<AppointmentRuleException>(() =>
                _appointmentService.BookAppointment(dto));

            // Assert
            Assert.Equal(
                "Patient already has an active appointment with this doctor on the selected date.",
                exception.Message);
        }

        [Fact]
        public void BookAppointment_WhenDoctorSlotAlreadyBooked_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            BookAppointmentDto dto = CreateBookAppointmentDto();

            SetupValidPatient(dto.PatientId);
            SetupValidDoctor(dto.DoctorId);
            SetupNoPatientConflicts(dto);

            _appointmentRepositoryMock
                .Setup(r => r.IsSlotBooked(
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    dto.SlotNumber))
                .Returns(true);

            // Act
            AppointmentRuleException exception = Assert.Throws<AppointmentRuleException>(() =>
                _appointmentService.BookAppointment(dto));

            // Assert
            Assert.Equal("Selected appointment slot is already booked.", exception.Message);
        }

        [Fact]
        public void BookAppointment_WhenValidData_ShouldAddAppointmentAndReturnDto()
        {
            // Arrange
            BookAppointmentDto dto = CreateBookAppointmentDto();

            SetupValidPatient(dto.PatientId);
            SetupValidDoctor(dto.DoctorId);
            SetupNoPatientConflicts(dto);

            _appointmentRepositoryMock
                .Setup(r => r.IsSlotBooked(
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    dto.SlotNumber))
                .Returns(false);

            _appointmentRepositoryMock
                .Setup(r => r.Add(It.IsAny<Appointment>()))
                .Returns((Appointment appointment) =>
                {
                    appointment.AppointmentId = 100;
                    return appointment;
                });

            // Act
            AppointmentDto result = _appointmentService.BookAppointment(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.AppointmentId);
            Assert.Equal(dto.PatientId, result.PatientId);
            Assert.Equal(dto.DoctorId, result.DoctorId);
            Assert.Equal(dto.ScheduledDate.Date, result.ScheduledDate.Date);
            Assert.Equal(dto.SlotNumber, result.SlotNumber);
            Assert.Equal(AppointmentStatus.Pending, result.Status);
        }

        [Fact]
        public void CancelAppointmentByPatient_WhenReasonIsEmpty_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            CancelByPatientDto dto = new CancelByPatientDto
            {
                PatientId = 1,
                Reason = ""
            };

            Appointment appointment = CreateAppointment();
            appointment.Status = AppointmentStatus.Pending;
            appointment.ScheduledDate = DateTime.Today.AddDays(1);

            SetupValidPatient(dto.PatientId);

            _appointmentRepositoryMock
                .Setup(r => r.GetById(appointment.AppointmentId))
                .Returns(appointment);

            // Act
            AppointmentRuleException exception = Assert.Throws<AppointmentRuleException>(() =>
                _appointmentService.CancelAppointmentByPatient(appointment.AppointmentId, dto));

            // Assert
            Assert.Equal("Cancellation reason is required.", exception.Message);
        }

        [Fact]
        public void ConfirmAppointment_WhenAppointmentIsPending_ShouldConfirmAppointment()
        {
            // Arrange
            ConfirmAppointmentDto dto = new ConfirmAppointmentDto
            {
                DoctorId = 2
            };

            Appointment appointment = CreateAppointment();
            appointment.Status = AppointmentStatus.Pending;

            SetupValidDoctor(dto.DoctorId);

            _appointmentRepositoryMock
                .Setup(r => r.GetById(appointment.AppointmentId))
                .Returns(appointment);

            _appointmentRepositoryMock
                .Setup(r => r.Update(appointment.AppointmentId, It.IsAny<Appointment>()))
                .Returns((int id, Appointment updatedAppointment) => updatedAppointment);

            // Act
            AppointmentDto result =
                _appointmentService.ConfirmAppointment(appointment.AppointmentId, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(AppointmentStatus.Confirmed, result.Status);
        }

        [Fact]
        public void CompleteAppointment_WhenAppointmentIsNotToday_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            CompleteAppointmentDto dto = new CompleteAppointmentDto
            {
                DoctorId = 2
            };

            Appointment appointment = CreateAppointment();
            appointment.Status = AppointmentStatus.Confirmed;
            appointment.ScheduledDate = DateTime.Today.AddDays(1);

            SetupValidDoctor(dto.DoctorId);

            _appointmentRepositoryMock
                .Setup(r => r.GetById(appointment.AppointmentId))
                .Returns(appointment);

            // Act
            AppointmentRuleException exception = Assert.Throws<AppointmentRuleException>(() =>
                _appointmentService.CompleteAppointment(appointment.AppointmentId, dto));

            // Assert
            Assert.Equal("Only appointments scheduled for today can be completed.", exception.Message);
        }

        [Fact]
        public void DeleteAppointment_WhenAppointmentHasHealthRecord_ShouldThrowAppointmentRuleException()
        {
            // Arrange
            Appointment appointment = CreateAppointment();

            _appointmentRepositoryMock
                .Setup(r => r.GetById(appointment.AppointmentId))
                .Returns(appointment);

            _healthRecordRepositoryMock
                .Setup(r => r.ExistsByAppointmentId(appointment.AppointmentId))
                .Returns(true);

            // Act
            AppointmentRuleException exception = Assert.Throws<AppointmentRuleException>(() =>
                _appointmentService.DeleteAppointment(appointment.AppointmentId));

            // Assert
            Assert.Equal(
                "This appointment cannot be deleted because it has an associated health record.",
                exception.Message);
        }

        private BookAppointmentDto CreateBookAppointmentDto()
        {
            return new BookAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                SlotNumber = 4
            };
        }

        private Appointment CreateAppointment()
        {
            return new Appointment
            {
                AppointmentId = 10,
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                SlotNumber = 4,
                Status = AppointmentStatus.Pending,
                CancellationReason = string.Empty
            };
        }

        private Patient CreatePatient(int patientId)
        {
            return new Patient
            {
                PatientId = patientId,
                FullName = "Rishab Gorla",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "rishab@test.com",
                InsuranceId = "INS123"
            };
        }

        private Doctor CreateDoctor(int doctorId)
        {
            return new Doctor
            {
                DoctorId = doctorId,
                FullName = "Rahul Sharma",
                Specialisation = GetAnySpecialisation(),
                PracticeStartDate = DateTime.Today.AddYears(-5),
                ConsultationFee = 500,
                IsActive = true
            };
        }

        private void SetupValidPatient(int patientId)
        {
            _patientRepositoryMock
                .Setup(r => r.GetById(patientId))
                .Returns(CreatePatient(patientId));
        }

        private void SetupValidDoctor(int doctorId)
        {
            _doctorRepositoryMock
                .Setup(r => r.GetById(doctorId))
                .Returns(CreateDoctor(doctorId));
        }

        private void SetupNoPatientConflicts(BookAppointmentDto dto)
        {
            _appointmentRepositoryMock
                .Setup(r => r.PatientHasActiveAppointmentOnDateAndSlot(
                    dto.PatientId,
                    dto.ScheduledDate.Date,
                    dto.SlotNumber))
                .Returns(false);

            _appointmentRepositoryMock
                .Setup(r => r.PatientHasActiveAppointmentWithDoctorOnDate(
                    dto.PatientId,
                    dto.DoctorId,
                    dto.ScheduledDate.Date))
                .Returns(false);
        }

        private Specialisation GetAnySpecialisation()
        {
            Array values = Enum.GetValues(typeof(Specialisation));
            return (Specialisation)values.GetValue(0);
        }
    }
}