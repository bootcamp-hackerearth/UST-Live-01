using AutoMapper;
using FluentAssertions;
using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.Enums;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementations;
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
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock = new Mock<IRepository<Appointment>>();
            _doctorRepositoryMock = new Mock<IRepository<Doctor>>();
            _patientRepositoryMock = new Mock<IRepository<Patient>>();
            _mapperMock = new Mock<IMapper>();

            _service = new AppointmentService(
                _appointmentRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _mapperMock.Object);
        }

        private CreateAppointmentDto GetCreateAppointmentDto()
        {
            return new CreateAppointmentDto
            {
                PatientId = 3,
                DoctorId = 6,
                ScheduledDate = DateTime.Today.AddDays(2),
                TimeSlot = "10:00 AM"
            };
        }

        [Fact]
        public async Task AddAsync_WhenDateIsPast_ThrowsValidationException()
        {
            var dto = GetCreateAppointmentDto();
            dto.ScheduledDate = DateTime.Today.AddDays(-1);

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Appointments cannot be booked for past dates.");
        }

        [Fact]
        public async Task AddAsync_WhenDateExceedsSixMonths_ThrowsValidationException()
        {
            var dto = GetCreateAppointmentDto();
            dto.ScheduledDate = DateTime.Today.AddMonths(7);

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Appointments can only be booked up to 6 months in advance.");
        }

        [Fact]
        public async Task AddAsync_WhenTimeSlotIsEmpty_ThrowsValidationException()
        {
            var dto = GetCreateAppointmentDto();
            dto.TimeSlot = "";

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Time slot is required.");
        }

        [Fact]
        public async Task AddAsync_WhenPatientDoesNotExist_ThrowsNotFoundException()
        {
            var dto = GetCreateAppointmentDto();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Patient not found.");
        }

        [Fact]
        public async Task AddAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            var dto = GetCreateAppointmentDto();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(new Patient
                {
                    PatientId = dto.PatientId,
                    FullName = "Kiran"
                });

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Doctor not found.");
        }

        [Fact]
        public async Task AddAsync_WhenDoctorIsInactive_ThrowsValidationException()
        {
            var dto = GetCreateAppointmentDto();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(new Patient
                {
                    PatientId = dto.PatientId,
                    FullName = "Kiran"
                });

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(new Doctor
                {
                    DoctorId = dto.DoctorId,
                    FullName = "Dr Nevin",
                    IsActive = false
                });

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Appointments cannot be booked with inactive doctors.");
        }

        [Fact]
        public async Task AddAsync_WhenSlotAlreadyBooked_ThrowsValidationException()
        {
            var dto = GetCreateAppointmentDto();

            var existingAppointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 10,
                    DoctorId = dto.DoctorId,
                    ScheduledDate = dto.ScheduledDate,
                    TimeSlot = dto.TimeSlot,
                    Status = AppointmentStatus.Confirmed
                }
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(new Patient
                {
                    PatientId = dto.PatientId,
                    FullName = "Kiran"
                });

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(new Doctor
                {
                    DoctorId = dto.DoctorId,
                    FullName = "Dr Nevin",
                    IsActive = true
                });

            _appointmentRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(existingAppointments);

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Selected time slot is already booked.");
        }

        [Fact]
        public async Task AddAsync_WhenValidAppointment_ReturnsAppointmentDto()
        {
            var dto = GetCreateAppointmentDto();

            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                ScheduledDate = dto.ScheduledDate,
                TimeSlot = dto.TimeSlot
            };

            var appointmentDto = new AppointmentDto
            {
                AppointmentId = 1,
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                ScheduledDate = dto.ScheduledDate,
                TimeSlot = dto.TimeSlot,
                Status = AppointmentStatus.Pending
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(new Patient
                {
                    PatientId = dto.PatientId,
                    FullName = "Kiran"
                });

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(new Doctor
                {
                    DoctorId = dto.DoctorId,
                    FullName = "Dr Nevin",
                    IsActive = true
                });

            _appointmentRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            _mapperMock
                .Setup(x => x.Map<Appointment>(dto))
                .Returns(appointment);

            _appointmentRepositoryMock
                .Setup(x => x.AddAsync(appointment))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(appointmentDto);

            var result = await _service.AddAsync(dto);

            result.Should().NotBeNull();
            result.AppointmentId.Should().Be(1);
            result.Status.Should().Be(AppointmentStatus.Pending);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenAppointmentDoesNotExist_ThrowsNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Appointment?)null);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Confirmed
            };

            Func<Task> action = async () => await _service.UpdateStatusAsync(99, dto);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Appointment not found.");
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenPendingToConfirmed_ReturnsConfirmedAppointment()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 3,
                DoctorId = 6,
                ScheduledDate = DateTime.Today.AddDays(2),
                TimeSlot = "10:00 AM",
                Status = AppointmentStatus.Pending
            };

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Confirmed
            };

            var appointmentDto = new AppointmentDto
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(x => x.UpdateAsync(1, appointment, It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(appointmentDto);

            var result = await _service.UpdateStatusAsync(1, dto);

            result.Should().NotBeNull();
            result.Status.Should().Be(AppointmentStatus.Confirmed);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenPendingToCompleted_ThrowsValidationException()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 3,
                DoctorId = 6,
                ScheduledDate = DateTime.Today.AddDays(2),
                TimeSlot = "10:00 AM",
                Status = AppointmentStatus.Pending
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Completed
            };

            Func<Task> action = async () => await _service.UpdateStatusAsync(1, dto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Pending appointments must be confirmed before completion.");
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenAppointmentAlreadyCancelled_ThrowsValidationException()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Cancelled
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Completed
            };

            Func<Task> action = async () => await _service.UpdateStatusAsync(1, dto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Cancelled appointments cannot be modified.");
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenAppointmentAlreadyCompleted_ThrowsValidationException()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Completed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Cancelled
            };

            Func<Task> action = async () => await _service.UpdateStatusAsync(1, dto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Completed appointments cannot be modified.");
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenSameStatus_ThrowsValidationException()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Pending
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Pending
            };

            Func<Task> action = async () => await _service.UpdateStatusAsync(1, dto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Appointment is already Pending.");
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenRevertingToPending_ThrowsValidationException()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Pending
            };

            Func<Task> action = async () => await _service.UpdateStatusAsync(1, dto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Cannot revert appointment to pending.");
        }

        [Fact]
        public async Task DeleteAsync_WhenAppointmentDoesNotExist_ThrowsNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () => await _service.DeleteAsync(99);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Appointment not found.");
        }

        [Fact]
        public async Task DeleteAsync_WhenAppointmentCompleted_ThrowsValidationException()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Completed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            Func<Task> action = async () => await _service.DeleteAsync(1);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Completed appointments cannot be deleted.");
        }

        [Fact]
        public async Task DeleteAsync_WhenAppointmentConfirmed_ThrowsValidationException()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            Func<Task> action = async () => await _service.DeleteAsync(1);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Confirmed appointments cannot be deleted.");
        }

        [Fact]
        public async Task DeleteAsync_WhenAppointmentPending_ReturnsTrue()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Pending
            };

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
    }
}