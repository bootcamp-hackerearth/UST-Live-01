using AutoMapper;
using FluentAssertions;
using HealthAxis.API.Events;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementations;
using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.Enums;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using ValidationException = HealthAxis.API.Exceptions.ValidationException;

namespace Healthcare.netcore.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IRepository<Appointment>> _appointmentRepositoryMock;
        private readonly Mock<IRepository<Doctor>> _doctorRepositoryMock;
        private readonly Mock<IRepository<Patient>> _patientRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly Mock<ILogger<AppointmentService>> _loggerMock;

        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock = new Mock<IRepository<Appointment>>();
            _doctorRepositoryMock = new Mock<IRepository<Doctor>>();
            _patientRepositoryMock = new Mock<IRepository<Patient>>();
            _mapperMock = new Mock<IMapper>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _cacheMock = new Mock<IDistributedCache>();
            _loggerMock = new Mock<ILogger<AppointmentService>>();

            _service = new AppointmentService(
                _appointmentRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _mapperMock.Object,
                _publishEndpointMock.Object,
                _loggerMock.Object,
                _cacheMock.Object);
        }

        private static Doctor GetDoctor(bool active = true)
        {
            return new Doctor
            {
                DoctorId = 1,
                FullName = "Dr Nevin",
                IsActive = active
            };
        }

        private static Patient GetPatient()
        {
            return new Patient
            {
                PatientId = 1,
                FullName = "John"
            };
        }

        private static Appointment GetAppointment()
        {
            return new Appointment
            {
                AppointmentId = 1,
                DoctorId = 1,
                PatientId = 1,
                ScheduledDate = DateTime.Today.AddDays(2),
                TimeSlot = "09:00",
                Status = AppointmentStatus.Pending
            };
        }

        private static AppointmentDto GetAppointmentDto()
        {
            return new AppointmentDto
            {
                AppointmentId = 1,
                DoctorId = 1,
                PatientId = 1,
                ScheduledDate = DateTime.Today.AddDays(2),
                TimeSlot = "09:00",
                Status = AppointmentStatus.Pending
            };
        }

        private static CreateAppointmentDto GetCreateDto()
        {
            return new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(2),
                TimeSlot = "09:00"
            };
        }

        [Fact]
        public async Task GetAllAsync_WhenAppointmentsExist_ReturnsList()
        {
            var appointments = new List<Appointment>
            {
                GetAppointment()
            };

            var dtos = new List<AppointmentDto>
            {
                GetAppointmentDto()
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<AppointmentDto>>(appointments))
                .Returns(dtos);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(1);
            result.First().AppointmentId.Should().Be(1);
        }

        [Fact]
        public async Task GetAllAsync_WhenEmpty_ReturnsEmpty()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            _mapperMock
                .Setup(x => x.Map<IEnumerable<AppointmentDto>>(It.IsAny<IEnumerable<Appointment>>()))
                .Returns(new List<AppointmentDto>());

            var result = await _service.GetAllAsync();

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task AddAsync_WhenDateIsPast_ThrowsValidationException()
        {
            var dto = GetCreateDto();
            dto.ScheduledDate = DateTime.Today.AddDays(-1);

            Func<Task> act = async () =>
                await _service.AddAsync(dto);

            await act.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Appointments cannot be booked for past dates.");
        }

        [Fact]
        public async Task AddAsync_WhenDateBeyondSixMonths_ThrowsValidationException()
        {
            var dto = GetCreateDto();
            dto.ScheduledDate = DateTime.Today.AddMonths(7);

            Func<Task> act = async () =>
                await _service.AddAsync(dto);

            await act.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Appointments can only be booked up to 6 months in advance.");
        }

        [Fact]
        public async Task AddAsync_WhenTimeSlotEmpty_ThrowsValidationException()
        {
            var dto = GetCreateDto();
            dto.TimeSlot = "";

            Func<Task> act = async () =>
                await _service.AddAsync(dto);

            await act.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Time slot is required.");
        }

        [Fact]
        public async Task AddAsync_WhenPatientNotFound_ThrowsNotFoundException()
        {
            var dto = GetCreateDto();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync((Patient?)null);

            Func<Task> act = async () =>
                await _service.AddAsync(dto);

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Patient not found.");
        }

        [Fact]
        public async Task AddAsync_WhenDoctorNotFound_ThrowsNotFoundException()
        {
            var dto = GetCreateDto();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync((Doctor?)null);

            Func<Task> act = async () =>
                await _service.AddAsync(dto);

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Doctor not found.");
        }

        [Fact]
        public async Task AddAsync_WhenDoctorInactive_ThrowsValidationException()
        {
            var dto = GetCreateDto();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(GetDoctor(false));

            Func<Task> act = async () =>
                await _service.AddAsync(dto);

            await act.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Appointments cannot be booked with inactive doctors.");
        }
        [Fact]
        public async Task AddAsync_WhenSlotAlreadyBooked_ThrowsValidationException()
        {
            var dto = GetCreateDto();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(GetDoctor());

            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 2,
                    DoctorId = dto.DoctorId,
                    PatientId = 2,
                    ScheduledDate = dto.ScheduledDate,
                    TimeSlot = dto.TimeSlot,
                    Status = AppointmentStatus.Pending
                }
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(appointments);

            Func<Task> act = async () =>
                await _service.AddAsync(dto);

            await act.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Selected time slot is already booked.");
        }

        [Fact]
        public async Task AddAsync_WhenAppointmentIsValid_ReturnsAppointmentDto()
        {
            var dto = GetCreateDto();

            var appointment = GetAppointment();

            var appointmentDto = GetAppointmentDto();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(GetDoctor());

            _appointmentRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            _mapperMock
                .Setup(x => x.Map<Appointment>(dto))
                .Returns(appointment);

            _appointmentRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Appointment>()))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(x => x.Map<AppointmentDto>(It.IsAny<Appointment>()))
                .Returns(appointmentDto);

            var result = await _service.AddAsync(dto);

            result.Should().NotBeNull();
            result.AppointmentId.Should().Be(1);

            _appointmentRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Appointment>()),
                Times.Once);

            _publishEndpointMock.Verify(
                x => x.Publish(
                    It.IsAny<AppointmentBookedEvent>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task AddAsync_WhenPublishThrows_ShouldStillReturnAppointment()
        {
            var dto = GetCreateDto();

            var appointment = GetAppointment();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(GetDoctor());

            _appointmentRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            _mapperMock
                .Setup(x => x.Map<Appointment>(dto))
                .Returns(appointment);

            _appointmentRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Appointment>()))
                .ReturnsAsync(appointment);

            _publishEndpointMock
                .Setup(x => x.Publish(
                    It.IsAny<AppointmentBookedEvent>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("RabbitMQ Error"));

            _mapperMock
                .Setup(x => x.Map<AppointmentDto>(It.IsAny<Appointment>()))
                .Returns(GetAppointmentDto());

            var result = await _service.AddAsync(dto);

            result.Should().NotBeNull();

            _appointmentRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Appointment>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateStatus_WhenAppointmentNotFound_ThrowsNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(100))
                .ReturnsAsync((Appointment?)null);

            Func<Task> act = async () =>
                await _service.UpdateStatusAsync(
                    100,
                    new UpdateAppointmentStatusDto
                    {
                        Status = AppointmentStatus.Confirmed
                    });

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Appointment not found.");
        }

        [Fact]
        public async Task UpdateStatus_WhenAlreadyCancelled_ThrowsValidationException()
        {
            var appointment = GetAppointment();
            appointment.Status = AppointmentStatus.Cancelled;

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            Func<Task> act = async () =>
                await _service.UpdateStatusAsync(
                    1,
                    new UpdateAppointmentStatusDto
                    {
                        Status = AppointmentStatus.Confirmed
                    });

            await act.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Cancelled appointments cannot be modified.");
        }

        [Fact]
        public async Task UpdateStatus_WhenAlreadyCompleted_ThrowsValidationException()
        {
            var appointment = GetAppointment();
            appointment.Status = AppointmentStatus.Completed;

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            Func<Task> act = async () =>
                await _service.UpdateStatusAsync(
                    1,
                    new UpdateAppointmentStatusDto
                    {
                        Status = AppointmentStatus.Cancelled
                    });

            await act.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Completed appointments cannot be modified.");
        }

        [Fact]
        public async Task UpdateStatus_WhenSameStatus_ThrowsValidationException()
        {
            var appointment = GetAppointment();

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            Func<Task> act = async () =>
                await _service.UpdateStatusAsync(
                    1,
                    new UpdateAppointmentStatusDto
                    {
                        Status = AppointmentStatus.Pending
                    });

            await act.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Appointment is already Pending.");
        }
        [Fact]
        public async Task UpdateStatus_WhenPendingToCompleted_ThrowsValidationException()
        {
            var appointment = GetAppointment();
            appointment.Status = AppointmentStatus.Pending;

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            Func<Task> act = async () =>
                await _service.UpdateStatusAsync(
                    1,
                    new UpdateAppointmentStatusDto
                    {
                        Status = AppointmentStatus.Completed
                    });

            await act.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Pending appointments must be confirmed before completion.");
        }

        [Fact]
        public async Task UpdateStatus_ToConfirmed_ReturnsUpdatedAppointment()
        {
            var appointment = GetAppointment();

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(x => x.UpdateAsync(
                    1,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(x => x.Map<AppointmentDto>(It.IsAny<Appointment>()))
                .Returns(GetAppointmentDto());

            var result = await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Confirmed
                });

            result.Should().NotBeNull();

            _appointmentRepositoryMock.Verify(x =>
                x.UpdateAsync(
                    1,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateStatus_ToCancelled_ReturnsAppointment()
        {
            var appointment = GetAppointment();

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(x => x.UpdateAsync(
                    1,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(x => x.Map<AppointmentDto>(It.IsAny<Appointment>()))
                .Returns(GetAppointmentDto());

            var result = await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Cancelled,
                    CancellationReason = "Patient request"
                });

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateStatus_ToCompleted_ReturnsAppointment()
        {
            var appointment = GetAppointment();
            appointment.Status = AppointmentStatus.Confirmed;

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(x => x.UpdateAsync(
                    1,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(x => x.Map<AppointmentDto>(It.IsAny<Appointment>()))
                .Returns(GetAppointmentDto());

            var result = await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Completed
                });

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_WhenAppointmentNotFound_ThrowsNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(50))
                .ReturnsAsync((Appointment?)null);

            Func<Task> act = async () =>
                await _service.DeleteAsync(50);

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Appointment not found.");
        }

        [Fact]
        public async Task Delete_WhenCompletedAppointment_ThrowsValidationException()
        {
            var appointment = GetAppointment();
            appointment.Status = AppointmentStatus.Completed;

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            Func<Task> act = async () =>
                await _service.DeleteAsync(1);

            await act.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Completed appointments cannot be deleted.");
        }

        [Fact]
        public async Task Delete_WhenConfirmedAppointment_ThrowsValidationException()
        {
            var appointment = GetAppointment();
            appointment.Status = AppointmentStatus.Confirmed;

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            Func<Task> act = async () =>
                await _service.DeleteAsync(1);

            await act.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Confirmed appointments cannot be deleted.");
        }

        [Fact]
        public async Task Delete_WhenPendingAppointment_ReturnsTrue()
        {
            var appointment = GetAppointment();

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(x => x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            var result = await _service.DeleteAsync(1);

            result.Should().BeTrue();

            _appointmentRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldCallRepositoryOnce()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            _mapperMock
                .Setup(x => x.Map<IEnumerable<AppointmentDto>>(It.IsAny<IEnumerable<Appointment>>()))
                .Returns(new List<AppointmentDto>());

            await _service.GetAllAsync();

            _appointmentRepositoryMock.Verify(
                x => x.GetAllAsync(),
                Times.Once);
        }
    }
}