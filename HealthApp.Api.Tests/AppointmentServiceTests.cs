using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Impl;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
using HealthApp.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HealthApp.Api.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepo;
        private readonly Mock<IPatientRepository> _patientRepo;
        private readonly Mock<IDoctorRepository> _doctorRepo;
        private readonly Mock<IDoctorLeaveRepository> _doctorLeaveRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IPublishEndpoint> _publishEndpoint;
        private readonly Mock<IDistributedCache> _cache;
        private readonly Mock<ILogger<AppointmentService>> _logger;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _appointmentRepo = new Mock<IAppointmentRepository>();
            _patientRepo = new Mock<IPatientRepository>();
            _doctorRepo = new Mock<IDoctorRepository>();
            _doctorLeaveRepo = new Mock<IDoctorLeaveRepository>();
            _mapper = new Mock<IMapper>();
            _publishEndpoint = new Mock<IPublishEndpoint>();
            _cache = new Mock<IDistributedCache>();
            _logger = new Mock<ILogger<AppointmentService>>();

            _cache
                .Setup(cache => cache.GetAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[]?)null);

            _cache
                .Setup(cache => cache.SetAsync(
                    It.IsAny<string>(),
                    It.IsAny<byte[]>(),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _cache
                .Setup(cache => cache.RemoveAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _doctorLeaveRepo
                .Setup(repo => repo.GetLeaveForDateAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((DoctorLeave?)null);

            _publishEndpoint
                .Setup(publisher => publisher.Publish(
                    It.IsAny<AppointmentBookedEvent>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _service = new AppointmentService(
                _appointmentRepo.Object,
                _patientRepo.Object,
                _doctorRepo.Object,
                _doctorLeaveRepo.Object,
                _mapper.Object,
                _publishEndpoint.Object,
                _cache.Object,
                _logger.Object);
        }

        private static AppointmentCreateDto ValidDto() => new()
        {
            PatientId = 1,
            DoctorId = 1,
            ScheduledDate = DateTime.Today.AddDays(1),
            TimeSlot = "10AM"
        };

        private static Patient Patient() => new()
        {
            PatientId = 1,
            FullName = "John"
        };

        private static Doctor Doctor(bool active = true) => new()
        {
            DoctorId = 1,
            FullName = "Dr A",
            IsActive = active,
            Specialisation = SpecialisationType.Cardiologist
        };

        private void SetupPatientUserId()
        {
            _patientRepo
                .Setup(repo => repo.GetPatientUserIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync("patient-user-id-1");
        }

        [Fact]
        public async Task GetAppointmentById_InvalidId_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.GetAppointmentByIdAsync(0));
        }

        [Fact]
        public async Task GetAppointmentById_NotFound_ShouldThrow()
        {
            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.GetAppointmentByIdAsync(1));
        }

        [Fact]
        public async Task GetAppointmentById_Valid_ShouldReturnDto()
        {
            var appt = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1
            };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(appt);

            _patientRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Patient());

            _doctorRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Doctor());

            _mapper.Setup(x => x.Map<AppointmentDto>(appt))
                .Returns(new AppointmentDto { AppointmentId = 1 });

            var result = await _service.GetAppointmentByIdAsync(1);

            Assert.Equal(1, result.AppointmentId);
        }

        [Fact]
        public async Task BookAppointment_NullDto_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.BookAppointmentAsync(null!));
        }

        [Fact]
        public async Task BookAppointment_PastDate_ShouldThrow()
        {
            var dto = ValidDto();
            dto.ScheduledDate = DateTime.Today.AddDays(-1);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(
                () => _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task BookAppointment_PatientNotFound_ShouldThrow()
        {
            var dto = ValidDto();

            _patientRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task BookAppointment_DoctorInactive_ShouldThrow()
        {
            var dto = ValidDto();

            _patientRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Patient());

            SetupPatientUserId();

            _doctorRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Doctor(false));

            await Assert.ThrowsAsync<BusinessRuleViolationException>(
                () => _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task BookAppointment_DoctorOnLeave_ShouldThrow()
        {
            var dto = ValidDto();
            var scheduledDate = DateOnly.FromDateTime(dto.ScheduledDate);

            _patientRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Patient());

            SetupPatientUserId();

            _doctorRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Doctor());

            _doctorLeaveRepo
                .Setup(repo => repo.GetLeaveForDateAsync(
                    1,
                    scheduledDate,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorLeave
                {
                    DoctorLeaveId = 20,
                    DoctorId = 1,
                    StartDate = scheduledDate,
                    EndDate = scheduledDate.AddDays(2),
                    Reason = "Medical conference",
                    CreatedAtUtc = DateTime.UtcNow
                });

            var exception = await Assert.ThrowsAsync<BusinessRuleViolationException>(
                () => _service.BookAppointmentAsync(dto));

            Assert.Contains("The selected doctor is on leave", exception.Message);

            _appointmentRepo.Verify(
                repo => repo.Add(
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task BookAppointment_DoctorSlotBooked_ShouldThrow()
        {
            var dto = ValidDto();

            _patientRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Patient());

            SetupPatientUserId();

            _doctorRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Doctor());

            _appointmentRepo.Setup(x => x.IsDoctorSlotBookedAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<string>(),
                    default))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(
                () => _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task BookAppointment_Valid_ShouldReturnDto()
        {
            var dto = ValidDto();

            var entity = new Appointment
            {
                AppointmentId = 100,
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateOnly.FromDateTime(dto.ScheduledDate),
                TimeSlot = dto.TimeSlot
            };

            _patientRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Patient());

            SetupPatientUserId();

            _doctorRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Doctor());

            _appointmentRepo.Setup(x =>
                    x.HasAppointmentWithDoctorOnSameDayAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<DateOnly>(),
                        default))
                .ReturnsAsync(false);

            _appointmentRepo.Setup(x =>
                    x.HasPatientSlotConflictAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateOnly>(),
                        It.IsAny<string>(),
                        default))
                .ReturnsAsync(false);

            _appointmentRepo.Setup(x =>
                    x.IsDoctorSlotBookedAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateOnly>(),
                        It.IsAny<string>(),
                        default))
                .ReturnsAsync(false);

            _mapper.Setup(x => x.Map<Appointment>(dto))
                .Returns(new Appointment());

            _appointmentRepo.Setup(x => x.Add(
                    It.IsAny<Appointment>(),
                    default))
                .ReturnsAsync(entity);

            _mapper.Setup(x => x.Map<AppointmentDto>(entity))
                .Returns(new AppointmentDto { AppointmentId = 100 });

            var result = await _service.BookAppointmentAsync(dto);

            Assert.Equal(100, result.AppointmentId);

            _cache.Verify(cache => cache.RemoveAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _publishEndpoint.Verify(publisher => publisher.Publish(
                    It.IsAny<AppointmentBookedEvent>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateStatus_InvalidId_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.UpdateAppointmentStatusAsync(0, AppointmentStatus.Pending));
        }

        [Fact]
        public async Task UpdateStatus_Completed_ShouldThrow()
        {
            var appt = new Appointment
            {
                AppointmentId = 1,
                DoctorId = 1,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                Status = AppointmentStatus.Completed
            };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(appt);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(
                () => _service.UpdateAppointmentStatusAsync(1, AppointmentStatus.Cancelled));
        }

        [Fact]
        public async Task UpdateStatus_CancelWithoutReason_ShouldThrow()
        {
            var appt = new Appointment
            {
                AppointmentId = 1,
                DoctorId = 1,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                Status = AppointmentStatus.Pending
            };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(appt);

            await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.UpdateAppointmentStatusAsync(1, AppointmentStatus.Cancelled, null));
        }

        [Fact]
        public async Task UpdateStatus_Valid_ShouldUpdate()
        {
            var appt = new Appointment
            {
                AppointmentId = 1,
                DoctorId = 1,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                Status = AppointmentStatus.Pending
            };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(appt);

            await _service.UpdateAppointmentStatusAsync(1, AppointmentStatus.Confirmed);

            _appointmentRepo.Verify(x => x.Update(
                    1,
                    It.IsAny<Appointment>(),
                    default),
                Times.Once);

            _cache.Verify(cache => cache.RemoveAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Delete_NotCancelled_ShouldThrow()
        {
            var appt = new Appointment
            {
                AppointmentId = 1,
                DoctorId = 1,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                Status = AppointmentStatus.Pending
            };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(appt);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(
                () => _service.DeleteAppointmentAsync(1));
        }

        [Fact]
        public async Task Delete_Valid_ShouldDelete()
        {
            var appt = new Appointment
            {
                AppointmentId = 1,
                DoctorId = 1,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                Status = AppointmentStatus.Cancelled
            };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(appt);

            _appointmentRepo.Setup(x => x.DeleteAsync(1))
                .ReturnsAsync(true);

            await _service.DeleteAppointmentAsync(1);

            _appointmentRepo.Verify(x => x.DeleteAsync(1), Times.Once);

            _cache.Verify(cache => cache.RemoveAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetDoctorAvailability_DoctorInactive_ShouldThrow()
        {
            _doctorRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Doctor(false));

            await Assert.ThrowsAsync<BusinessRuleViolationException>(
                () => _service.GetDoctorAvailabilityAsync(
                    1,
                    DateOnly.FromDateTime(DateTime.Today.AddDays(1))));
        }

        [Fact]
        public async Task GetDoctorAvailability_NormalDate_ShouldReturnSlots()
        {
            var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

            _doctorRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Doctor());

            _appointmentRepo.Setup(x => x.IsDoctorSlotBookedAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<string>(),
                    default))
                .ReturnsAsync(false);

            var result = await _service.GetDoctorAvailabilityAsync(1, date);

            Assert.NotNull(result);
            Assert.Equal(1, result.DoctorId);
            Assert.Equal(date, result.Date);
            Assert.False(result.IsDoctorOnLeave);
            Assert.NotEmpty(result.Slots);
            Assert.All(result.Slots, slot =>
            {
                Assert.True(slot.IsAvailable);
                Assert.Equal("Available", slot.Status);
            });

            _cache.Verify(cache => cache.GetAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _cache.Verify(cache => cache.SetAsync(
                    It.IsAny<string>(),
                    It.IsAny<byte[]>(),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetDoctorAvailability_DoctorOnLeave_ShouldReturnDisabledSlots()
        {
            var date = DateOnly.FromDateTime(DateTime.Today.AddDays(2));

            _doctorRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Doctor());

            _doctorLeaveRepo
                .Setup(repo => repo.GetLeaveForDateAsync(
                    1,
                    date,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DoctorLeave
                {
                    DoctorLeaveId = 10,
                    DoctorId = 1,
                    StartDate = date,
                    EndDate = date.AddDays(2),
                    Reason = "Medical conference",
                    CreatedAtUtc = DateTime.UtcNow
                });

            var result = await _service.GetDoctorAvailabilityAsync(1, date);

            Assert.NotNull(result);
            Assert.True(result.IsDoctorOnLeave);
            Assert.NotEmpty(result.Slots);
            Assert.All(result.Slots, slot =>
            {
                Assert.False(slot.IsAvailable);
                Assert.Equal("DoctorOnLeave", slot.Status);
            });

            _appointmentRepo.Verify(
                repo => repo.IsDoctorSlotBookedAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetDoctorAvailability_InvalidDoctor_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.GetDoctorAvailabilityAsync(
                    0,
                    DateOnly.FromDateTime(DateTime.Today)));
        }

        [Fact]
        public async Task GetDoctorAvailability_DoctorNotFound_ShouldThrow()
        {
            _doctorRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.GetDoctorAvailabilityAsync(
                    1,
                    DateOnly.FromDateTime(DateTime.Today)));
        }

        [Fact]
        public async Task GetAppointments_ShouldReturnAppointments()
        {
            var filter = new AppointmentFilterDto
            {
                DoctorId = null,
                PatientId = null,
                Status = null,
                Date = null,
                FromDate = null,
                ToDate = null,
                OnlyUpcoming = false,
                PageNumber = 1,
                PageSize = 10
            };

            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1
                }
            };

            var appointmentDtos = new List<AppointmentDto>
            {
                new AppointmentDto
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1
                }
            };

            (IEnumerable<Appointment> Items, int TotalCount) repositoryResult =
                (appointments, appointments.Count);

            _appointmentRepo
                .Setup(repo => repo.GetAppointmentsAsync(
                    It.Is<AppointmentFilterDto>(f =>
                        f.DoctorId == null &&
                        f.PatientId == null &&
                        f.Status == null &&
                        f.Date == null &&
                        f.FromDate == null &&
                        f.ToDate == null &&
                        f.OnlyUpcoming == false &&
                        f.PageNumber == 1 &&
                        f.PageSize == 10),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(repositoryResult));

            _patientRepo
                .Setup(repo => repo.GetByIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Patient());

            _doctorRepo
                .Setup(repo => repo.GetByIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Doctor());

            _mapper
                .Setup(mapper => mapper.Map<List<AppointmentDto>>(
                    It.IsAny<IEnumerable<Appointment>>()))
                .Returns(appointmentDtos);

            var result = await _service.GetAppointmentsAsync(filter);

            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
            Assert.Equal(1, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(1, result.Items.First().AppointmentId);

            _appointmentRepo.Verify(repo => repo.GetAppointmentsAsync(
                    It.IsAny<AppointmentFilterDto>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task BookAppointment_InvalidPatientId_ShouldThrow()
        {
            var dto = ValidDto();
            dto.PatientId = 0;

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task BookAppointment_NoTimeSlot_ShouldThrow()
        {
            var dto = ValidDto();
            dto.TimeSlot = "";

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task BookAppointment_SameDoctorSameDay_ShouldThrow()
        {
            var dto = ValidDto();

            _patientRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Patient());

            SetupPatientUserId();

            _doctorRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Doctor());

            _appointmentRepo.Setup(x =>
                    x.HasAppointmentWithDoctorOnSameDayAsync(
                        1,
                        1,
                        It.IsAny<DateOnly>(),
                        default))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
                _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task BookAppointment_PatientSlotConflict_ShouldThrow()
        {
            var dto = ValidDto();

            _patientRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Patient());

            SetupPatientUserId();

            _doctorRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(Doctor());

            _appointmentRepo.Setup(x =>
                    x.HasAppointmentWithDoctorOnSameDayAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<DateOnly>(),
                        default))
                .ReturnsAsync(false);

            _appointmentRepo.Setup(x =>
                    x.HasPatientSlotConflictAsync(
                        It.IsAny<int>(),
                        It.IsAny<DateOnly>(),
                        It.IsAny<string>(),
                        default))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
                _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task UpdateStatus_NotFound_ShouldThrow()
        {
            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.UpdateAppointmentStatusAsync(1, AppointmentStatus.Pending));
        }

        [Fact]
        public async Task Delete_InvalidId_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.DeleteAppointmentAsync(0));
        }

        [Fact]
        public async Task Delete_Failure_ShouldThrow()
        {
            var appt = new Appointment
            {
                AppointmentId = 1,
                DoctorId = 1,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                Status = AppointmentStatus.Cancelled
            };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync(appt);

            _appointmentRepo.Setup(x => x.DeleteAsync(1))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
                _service.DeleteAppointmentAsync(1));
        }
    }
}
