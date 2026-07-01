using AutoMapper;
using FluentAssertions;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services.Impl;
using HealthCareApp.Shared.Dtos.Appointments;
using HealthCareApp.Shared.Enums;
using HealthCareApp.Tests.Helpers;
using Moq;
using Xunit;

namespace HealthCareApp.Tests.Services
{

    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepository = new();
        private readonly Mock<IPatientRepository> _patientRepository = new();
        private readonly Mock<IDoctorRepository> _doctorRepository = new();
        private readonly Mock<IHealthRecordRepository> _healthRecordRepository = new();
        private readonly Mock<IMapper> _mapper = new();

        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _service = new AppointmentService(
                _appointmentRepository.Object,
                _patientRepository.Object,
                _doctorRepository.Object,
                _healthRecordRepository.Object,
                _mapper.Object);
        }

        [Fact]
        public async Task GetAllAppointmentsAsync_ShouldReturnAppointments()
        {
            // Arrange

            var appointments = AppointmentTestData.AppointmentList;
            var appointmentDtos = AppointmentTestData.AppointmentDtoList;

            _appointmentRepository
                .Setup(x => x.GetAllAsync(default))
                .ReturnsAsync(appointments);

            _mapper
                .Setup(x => x.Map<List<AppointmentDto>>(appointments))
                .Returns(appointmentDtos);

            // Act

            var result = await _service.GetAllAppointmentsAsync();

            // Assert

            result.Should().BeEquivalentTo(appointmentDtos);

            _appointmentRepository.Verify(
                x => x.GetAllAsync(default),
                Times.Once);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WhenAppointmentExists_ShouldReturnAppointment()
        {
            // Arrange

            var appointment = AppointmentTestData.Appointment;
            var dto = AppointmentTestData.AppointmentDto;

            _appointmentRepository
                .Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(appointment);

            _mapper
                .Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(dto);

            // Act

            var result = await _service.GetAppointmentByIdAsync(1);

            // Assert

            result.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WhenIdIsInvalid_ShouldThrowAppointmentRuleException()
        {
            Func<Task> act = async () =>
                await _service.GetAppointmentByIdAsync(0);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>();
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WhenAppointmentDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            _appointmentRepository
                .Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync((Models.Appointment?)null);

            Func<Task> act = async () =>
                await _service.GetAppointmentByIdAsync(1);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetAppointmentsByPatientIdAsync_ShouldReturnAppointments()
        {
            // Arrange

            var patient = PatientTestData.Patient;
            var appointments = AppointmentTestData.AppointmentList;
            var dtos = AppointmentTestData.AppointmentDtoList;

            _patientRepository
                .Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(patient);

            _appointmentRepository
                .Setup(x => x.GetByPatientIdAsync(1, default))
                .ReturnsAsync(appointments);

            _mapper
                .Setup(x => x.Map<List<AppointmentDto>>(appointments))
                .Returns(dtos);

            // Act

            var result = await _service.GetAppointmentsByPatientIdAsync(1);

            // Assert

            result.Should().BeEquivalentTo(dtos);
        }


        [Fact]
        public async Task GetAppointmentsByPatientIdAsync_WhenPatientNotFound_ShouldThrowEntityNotFoundException()
        {
            _patientRepository
                .Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync((Models.Patient?)null);

            Func<Task> act = async () =>
                await _service.GetAppointmentsByPatientIdAsync(1);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetAppointmentsByDoctorIdAsync_ShouldReturnAppointments()
        {
            // Arrange

            var doctor = DoctorTestData.Doctor;
            var appointments = AppointmentTestData.AppointmentList;
            var dtos = AppointmentTestData.AppointmentDtoList;

            _doctorRepository
                .Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(doctor);

            _appointmentRepository
                .Setup(x => x.GetByDoctorIdAsync(1, default))
                .ReturnsAsync(appointments);

            _mapper
                .Setup(x => x.Map<List<AppointmentDto>>(appointments))
                .Returns(dtos);

            // Act

            var result = await _service.GetAppointmentsByDoctorIdAsync(1);

            // Assert

            result.Should().BeEquivalentTo(dtos);
        }


        [Fact]
        public async Task GetAppointmentsByDoctorIdAsync_WhenDoctorNotFound_ShouldThrowEntityNotFoundException()
        {
            _doctorRepository
                .Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync((Models.Doctor?)null);

            Func<Task> act = async () =>
                await _service.GetAppointmentsByDoctorIdAsync(1);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>();
        }



        [Fact]
        public async Task GetAppointmentsByStatusAsync_ShouldReturnAppointments()
        {
            // Arrange

            var appointments = AppointmentTestData.AppointmentList;
            var dtos = AppointmentTestData.AppointmentDtoList;

            _appointmentRepository
                .Setup(x => x.GetByStatusAsync(AppointmentStatus.Pending, default))
                .ReturnsAsync(appointments);

            _mapper
                .Setup(x => x.Map<List<AppointmentDto>>(appointments))
                .Returns(dtos);

            // Act

            var result = await _service.GetAppointmentsByStatusAsync(AppointmentStatus.Pending);

            // Assert

            result.Should().BeEquivalentTo(dtos);
        }


        [Fact]
        public async Task GetUpcomingAppointmentsByPatientIdAsync_ShouldReturnAppointments()
        {
            // Arrange
            var patient = new Patient
            {
                PatientId = 1,
                PatientName = "John",
                Email = "john@example.com",
                PhoneNumber = "1234567890"
            };

            _patientRepository
                .Setup(r => r.GetByIdAsync(1, default))
                .ReturnsAsync(patient);

            _appointmentRepository
                .Setup(r => r.GetUpcomingAppointmentsByPatientIdAsync(1, default))
                .ReturnsAsync(AppointmentTestData.AppointmentList);

            _mapper
                .Setup(m => m.Map<List<AppointmentDto>>(AppointmentTestData.AppointmentList))
                .Returns(AppointmentTestData.AppointmentDtoList);

            // Act
            var result = await _service.GetUpcomingAppointmentsByPatientIdAsync(1);

            // Assert
            result.Should().HaveCount(1);
        }


        [Fact]
        public async Task GetUpcomingAppointmentsByDoctorIdAsync_ShouldReturnAppointments()
        {
            // Arrange
            var doctor = new Doctor
            {
                DoctorId = 1,
                DoctorName = "Dr Smith"
            };

            _doctorRepository
                .Setup(r => r.GetByIdAsync(1, default))
                .ReturnsAsync(doctor);

            _appointmentRepository
                .Setup(r => r.GetUpcomingAppointmentsByDoctorIdAsync(1, default))
                .ReturnsAsync(AppointmentTestData.AppointmentList);

            _mapper
                .Setup(m => m.Map<List<AppointmentDto>>(AppointmentTestData.AppointmentList))
                .Returns(AppointmentTestData.AppointmentDtoList);

            // Act
            var result = await _service.GetUpcomingAppointmentsByDoctorIdAsync(1);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetPendingAppointmentsByPatientIdAsync_ShouldReturnAppointments()
        {
            // Arrange
            var patient = new Patient
            {
                PatientId = 1,
                PatientName = "John Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890"
            };

            _patientRepository
                .Setup(r => r.GetByIdAsync(1, default))
                .ReturnsAsync(patient);

            _appointmentRepository
                .Setup(r => r.GetPendingAppointmentsByPatientIdAsync(1, default))
                .ReturnsAsync(AppointmentTestData.AppointmentList);

            _mapper
                .Setup(m => m.Map<List<AppointmentDto>>(AppointmentTestData.AppointmentList))
                .Returns(AppointmentTestData.AppointmentDtoList);

            // Act
            var result = await _service.GetPendingAppointmentsByPatientIdAsync(1);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetPendingAppointmentsByDoctorIdAsync_ShouldReturnAppointments()
        {
            // Arrange
            var doctor = new Doctor
            {
                DoctorId = 1
            };

            _doctorRepository
                .Setup(r => r.GetByIdAsync(1, default))
                .ReturnsAsync(doctor);

            _appointmentRepository
                .Setup(r => r.GetPendingAppointmentsByDoctorIdAsync(1, default))
                .ReturnsAsync(AppointmentTestData.AppointmentList);

            _mapper
                .Setup(m => m.Map<List<AppointmentDto>>(AppointmentTestData.AppointmentList))
                .Returns(AppointmentTestData.AppointmentDtoList);

            // Act
            var result = await _service.GetPendingAppointmentsByDoctorIdAsync(1);

            // Assert
            result.Should().HaveCount(1);
        }


        [Fact]
        public async Task GetTodayConfirmedAppointmentsByDoctorIdAsync_ShouldReturnAppointments()
        {
            // Arrange
            var doctor = new Doctor
            {
                DoctorId = 1
            };

            _doctorRepository
                .Setup(r => r.GetByIdAsync(1, default))
                .ReturnsAsync(doctor);

            _appointmentRepository
                .Setup(r => r.GetTodayConfirmedAppointmentsByDoctorIdAsync(1, default))
                .ReturnsAsync(AppointmentTestData.AppointmentList);

            _mapper
                .Setup(m => m.Map<List<AppointmentDto>>(AppointmentTestData.AppointmentList))
                .Returns(AppointmentTestData.AppointmentDtoList);

            // Act
            var result = await _service.GetTodayConfirmedAppointmentsByDoctorIdAsync(1);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetAppointmentsByPatientIdAsync_InvalidId_ShouldThrow()
        {
            // Act
            Func<Task> act = async () =>
                await _service.GetAppointmentsByPatientIdAsync(0);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task GetAppointmentsByDoctorIdAsync_InvalidId_ShouldThrow()
        {
            // Act
            Func<Task> act = async () =>
                await _service.GetAppointmentsByDoctorIdAsync(0);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task BookAppointmentAsync_ShouldBookAppointment()
        {
            // Arrange

            var dto = AppointmentTestData.BookAppointmentDto;

           
var patient = new Patient
{
    PatientId = 1,
    PatientName = "John Doe",
    Email = "john.doe@example.com",
    PhoneNumber = "1234567890"
};

           
            var doctor = new Doctor
            {
                DoctorId = 1,
                IsActive = true
            };

            var appointment = AppointmentTestData.Appointment;

            var appointmentDto = AppointmentTestData.AppointmentDto;

            _patientRepository
                .Setup(r => r.GetByIdAsync(dto.PatientId, default))
                .ReturnsAsync(patient);

            _doctorRepository
                .Setup(r => r.GetByIdAsync(dto.DoctorId, default))
                .ReturnsAsync(doctor);

            _appointmentRepository
                .Setup(r => r.IsSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot,
                    default))
                .ReturnsAsync(false);

            _appointmentRepository
                .Setup(r => r.PatientHasActiveAppointmentWithDoctorOnDateAsync(
                    dto.PatientId,
                    dto.DoctorId,
                    dto.ScheduledDate,
                    default))
                .ReturnsAsync(false);

            _appointmentRepository
                .Setup(r => r.PatientHasActiveAppointmentOnDateAndSlotAsync(
                    dto.PatientId,
                    dto.ScheduledDate,
                    dto.TimeSlot,
                    default))
                .ReturnsAsync(false);

            _mapper
                .Setup(m => m.Map<Appointment>(dto))
                .Returns(appointment);

            _appointmentRepository
                .Setup(r => r.CreateAsync(It.IsAny<Appointment>(), default))
                .ReturnsAsync(appointment);

            _mapper
                .Setup(m => m.Map<AppointmentDto>(appointment))
                .Returns(appointmentDto);

            // Act

            var result = await _service.BookAppointmentAsync(dto);

            // Assert

            result.Should().NotBeNull();

            result.AppointmentId.Should().Be(1);

            _appointmentRepository.Verify(
                r => r.CreateAsync(It.IsAny<Appointment>(), default),
                Times.Once);
        }

        [Fact]
        public async Task BookAppointmentAsync_InvalidPatient_ShouldThrow()
        {
            // Arrange

            var dto = AppointmentTestData.BookAppointmentDto;

            _patientRepository
                .Setup(r => r.GetByIdAsync(dto.PatientId, default))
                .ReturnsAsync((Patient?)null);

            // Act

            Func<Task> act = async () =>
                await _service.BookAppointmentAsync(dto);

            // Assert

            await act.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task BookAppointmentAsync_InactiveDoctor_ShouldThrow()
        {
            // Arrange

            var dto = AppointmentTestData.BookAppointmentDto;

             _patientRepository
                .Setup(r => r.GetByIdAsync(dto.PatientId, default))
                .ReturnsAsync(new Patient
                {
                    PatientId = dto.PatientId,
                    PatientName = "John Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "1234567890"
                });

            _doctorRepository
                .Setup(r => r.GetByIdAsync(dto.DoctorId, default))
                .ReturnsAsync(new Doctor
                {
                    DoctorId = dto.DoctorId,
                    IsActive = false
                });

            // Act

            Func<Task> act = async () =>
                await _service.BookAppointmentAsync(dto);

            // Assert

            await act.Should()
                .ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_ShouldConfirmAppointment()
        {
            // Arrange

            var appointment = AppointmentTestData.Appointment;

            appointment.Status = AppointmentStatus.Pending;

            var dto = AppointmentTestData.AppointmentDto;

            _appointmentRepository
                .Setup(r => r.GetByIdAsync(1, default))
                .ReturnsAsync(appointment);

            _appointmentRepository
                .Setup(r => r.UpdateAsync(1, appointment, default))
                .ReturnsAsync(appointment);

            _mapper
                .Setup(m => m.Map<AppointmentDto>(appointment))
                .Returns(dto);

            // Act

            var result = await _service.ConfirmAppointmentAsync(1);

            // Assert

            appointment.Status.Should().Be(AppointmentStatus.Confirmed);

            result.Should().NotBeNull();
        }


        [Fact]
        public async Task CompleteAppointmentAsync_ShouldCompleteAppointment()
        {
            // Arrange

            var appointment = AppointmentTestData.Appointment;

            appointment.Status = AppointmentStatus.Confirmed;

            var dto = AppointmentTestData.AppointmentDto;

            _appointmentRepository
                .Setup(r => r.GetByIdAsync(1, default))
                .ReturnsAsync(appointment);

            _appointmentRepository
                .Setup(r => r.UpdateAsync(1, appointment, default))
                .ReturnsAsync(appointment);

            _mapper
                .Setup(m => m.Map<AppointmentDto>(appointment))
                .Returns(dto);

            // Act

            var result = await _service.CompleteAppointmentAsync(1);

            // Assert

            appointment.Status.Should().Be(AppointmentStatus.Completed);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task CancelAppointmentAsync_ShouldCancelAppointment()
        {
            // Arrange

            var cancelDto = AppointmentTestData.CancelAppointmentDto;

            var appointment = AppointmentTestData.Appointment;

            appointment.Status = AppointmentStatus.Pending;

            var dto = AppointmentTestData.AppointmentDto;

            _appointmentRepository
                .Setup(r => r.GetByIdAsync(1, default))
                .ReturnsAsync(appointment);

            _appointmentRepository
                .Setup(r => r.UpdateAsync(1, appointment, default))
                .ReturnsAsync(appointment);

            _mapper
                .Setup(m => m.Map<AppointmentDto>(appointment))
                .Returns(dto);

            // Act

            var result = await _service.CancelAppointmentAsync(cancelDto);

            // Assert

            appointment.Status.Should().Be(AppointmentStatus.Cancelled);

            appointment.CancellationReason.Should().Be(cancelDto.Reason);

            result.Should().NotBeNull();
        }


        [Fact]
        public async Task DeleteAppointmentAsync_ShouldDeleteAppointment()
        {
            // Arrange

            var appointment = AppointmentTestData.Appointment;

            var dto = AppointmentTestData.AppointmentDto;

            _appointmentRepository
                .Setup(r => r.DeleteAsync(1, default))
                .ReturnsAsync(appointment);

            _mapper
                .Setup(m => m.Map<AppointmentDto>(appointment))
                .Returns(dto);

            // Act

            var result = await _service.DeleteAppointmentAsync(1);

            // Assert

            result.Should().NotBeNull();

            result.AppointmentId.Should().Be(1);

            _appointmentRepository.Verify(
                r => r.DeleteAsync(1, default),
                Times.Once);
        }

    }
}

