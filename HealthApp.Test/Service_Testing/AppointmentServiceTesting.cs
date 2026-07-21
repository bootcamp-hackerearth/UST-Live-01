using AutoMapper;
using FluentAssertions;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Messaging.Events;
using HealthApp.Api.Messaging.Publisher;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Impl;
using HealthApp.Api.Service.Interface;
using HealthApp.Shared.Dto;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text;
using System.Text.Json;

namespace HealthApp.Test.Service_Testing
{
    public class AppointmentServiceTesting
    {
        private readonly Mock<IAppointmentRepository> _repo;
        private readonly Mock<IPatientRepository> _patientRepo;
        private readonly Mock<IDoctorRepository> _doctorRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IAppointmentEventPublisher> _appointmentPublisher;
        private readonly Mock<IDoctorLeaveRepository> _doctorLeaveRepo;
        private readonly Mock<INotificationService> _notificationService;
        private readonly Mock<IDistributedCache> _cache;

        private readonly AppointmentService _service;

        public AppointmentServiceTesting()
        {
            _repo = new Mock<IAppointmentRepository>();
            _patientRepo = new Mock<IPatientRepository>();
            _doctorRepo = new Mock<IDoctorRepository>();
            _mapper = new Mock<IMapper>();
            _appointmentPublisher = new Mock<IAppointmentEventPublisher>();
            _doctorLeaveRepo = new Mock<IDoctorLeaveRepository>();
            _notificationService = new Mock<INotificationService>();
            _cache = new Mock<IDistributedCache>();

            _notificationService
                .Setup(x => x.CreateAsync(It.IsAny<NotificationCreateDto>()))
                .ReturnsAsync(new NotificationDto());

            _cache
                .Setup(x => x.GetAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[]?)null);

            _cache
                .Setup(x => x.SetAsync(
                    It.IsAny<string>(),
                    It.IsAny<byte[]>(),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _cache
                .Setup(x => x.RemoveAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _service = new AppointmentService(
                _repo.Object,
                _patientRepo.Object,
                _doctorRepo.Object,
                _mapper.Object,
                _notificationService.Object,
                _appointmentPublisher.Object,
                _doctorLeaveRepo.Object,
                _cache.Object
            );
        }

       
        [Fact]
        public async Task Add_ShouldThrow_WhenSlotBooked()
        {
            var dto = new AppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _patientRepo.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync(new Patient { PatientId = 5 });

            _doctorLeaveRepo.Setup(x => x.IsDoctorOnLeaveAsync(dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(false);

            _repo.Setup(x => x.IsSlotBookedAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<string>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<AppointmentRuleException>(() =>
                _service.Add(dto, "user1"));
        }

        [Fact]
        public async Task Add_ShouldThrow_WhenPatientNotFound()
        {
            var dto = new AppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _patientRepo.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.Add(dto, "user1"));
        }

        [Fact]
        public async Task Add_ShouldThrow_WhenDoctorOnLeave()
        {
            var dto = new AppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _patientRepo.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync(new Patient { PatientId = 5 });

            _doctorLeaveRepo.Setup(x => x.IsDoctorOnLeaveAsync(dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<AppointmentRuleException>(() =>
                _service.Add(dto, "user1"));
        }

        [Fact]
        public async Task Confirm_ShouldUpdateStatus_AndCreateNotification_AndRemoveCache()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 5,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM",
                Status = "Confirmed"
            };

            var patient = new Patient
            {
                PatientId = 5,
                FullName = "Patient One",
                IdentityUserId = "patient-user-id"
            };

            var doctor = new Doctor
            {
                DoctorId = 2,
                FullName = "Doctor One"
            };

            _repo.Setup(x => x.UpdateStatusAsync(1, "Confirmed"))
                .ReturnsAsync(appointment);

            _patientRepo.Setup(x => x.getbyidAsync(5))
                .ReturnsAsync(patient);

            _doctorRepo.Setup(x => x.getbyidAsync(2))
                .ReturnsAsync(doctor);

            _mapper.Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(new AppointmentDto());

            var result = await _service.ConfirmAppointment(1);

            result.Should().NotBeNull();

            _notificationService.Verify(x =>
                x.CreateAsync(It.Is<NotificationCreateDto>(n =>
                    n.UserId == "patient-user-id" &&
                    n.Title == "Appointment Confirmed" &&
                    n.EventType == "AppointmentConfirmed")),
                Times.Once);

            _cache.Verify(x => x.RemoveAsync(
                    It.Is<string>(key =>
                        key == $"appointment:doctor:availability:{appointment.DoctorId}:{appointment.ScheduledDate.Date:yyyyMMdd}"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Confirm_ShouldUpdateStatus_WithoutNotification_WhenPatientHasNoIdentityUserId()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 5,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM",
                Status = "Confirmed"
            };

            _repo.Setup(x => x.UpdateStatusAsync(1, "Confirmed"))
                .ReturnsAsync(appointment);

            _patientRepo.Setup(x => x.getbyidAsync(5))
                .ReturnsAsync(new Patient
                {
                    PatientId = 5,
                    IdentityUserId = ""
                });

            _doctorRepo.Setup(x => x.getbyidAsync(2))
                .ReturnsAsync(new Doctor { DoctorId = 2 });

            _mapper.Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(new AppointmentDto());

            var result = await _service.ConfirmAppointment(1);

            result.Should().NotBeNull();

            _notificationService.Verify(x =>
                x.CreateAsync(It.IsAny<NotificationCreateDto>()),
                Times.Never);
        }

        [Fact]
        public async Task Confirm_ShouldThrow_WhenNotFound()
        {
            _repo.Setup(x => x.UpdateStatusAsync(1, "Confirmed"))
                .ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.ConfirmAppointment(1));
        }

        [Fact]
        public async Task Complete_ShouldUpdateStatus_AndRemoveAvailabilityCache()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1)
            };

            _repo.Setup(x => x.UpdateStatusAsync(1, "Completed"))
                .ReturnsAsync(appointment);

            _mapper.Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(new AppointmentDto());

            var result = await _service.CompleteAppointment(1);

            result.Should().NotBeNull();

            _cache.Verify(x => x.RemoveAsync(
                    It.Is<string>(key =>
                        key == $"appointment:doctor:availability:{appointment.DoctorId}:{appointment.ScheduledDate.Date:yyyyMMdd}"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Complete_ShouldThrow_WhenNotFound()
        {
            _repo.Setup(x => x.UpdateStatusAsync(1, "Completed"))
                .ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.CompleteAppointment(1));
        }

        [Fact]
        public async Task Cancel_ShouldReturnAppointment_AndCreateNotification_AndRemoveCache()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 5,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "11:00 AM",
                Status = "Cancelled"
            };

            var patient = new Patient
            {
                PatientId = 5,
                FullName = "Patient One",
                IdentityUserId = "patient-user-id"
            };

            var doctor = new Doctor
            {
                DoctorId = 2,
                FullName = "Doctor One"
            };

            _repo.Setup(x => x.CancelAppointmentAsync(1, "Reason"))
                .ReturnsAsync(appointment);

            _patientRepo.Setup(x => x.getbyidAsync(5))
                .ReturnsAsync(patient);

            _doctorRepo.Setup(x => x.getbyidAsync(2))
                .ReturnsAsync(doctor);

            _mapper.Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(new AppointmentDto());

            var result = await _service.CancelAppointment(1, "Reason");

            result.Should().NotBeNull();

            _notificationService.Verify(x =>
                x.CreateAsync(It.Is<NotificationCreateDto>(n =>
                    n.UserId == "patient-user-id" &&
                    n.Title == "Appointment Cancelled" &&
                    n.EventType == "AppointmentCancelled" &&
                    n.Message.Contains("Reason"))),
                Times.Once);

            _cache.Verify(x => x.RemoveAsync(
                    It.Is<string>(key =>
                        key == $"appointment:doctor:availability:{appointment.DoctorId}:{appointment.ScheduledDate.Date:yyyyMMdd}"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Cancel_ShouldReturnAppointment_WithoutNotification_WhenPatientMissing()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 5,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "11:00 AM",
                Status = "Cancelled"
            };

            _repo.Setup(x => x.CancelAppointmentAsync(1, "Reason"))
                .ReturnsAsync(appointment);

            _patientRepo.Setup(x => x.getbyidAsync(5))
                .ReturnsAsync((Patient?)null);

            _doctorRepo.Setup(x => x.getbyidAsync(2))
                .ReturnsAsync(new Doctor());

            _mapper.Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(new AppointmentDto());

            var result = await _service.CancelAppointment(1, "Reason");

            result.Should().NotBeNull();

            _notificationService.Verify(x =>
                x.CreateAsync(It.IsAny<NotificationCreateDto>()),
                Times.Never);
        }

        [Fact]
        public async Task Cancel_ShouldThrow_WhenReasonEmpty()
        {
            await Assert.ThrowsAsync<AppointmentRuleException>(() =>
                _service.CancelAppointment(1, ""));
        }

        [Fact]
        public async Task Cancel_ShouldThrow_WhenNotFound()
        {
            _repo.Setup(x => x.CancelAppointmentAsync(1, "Reason"))
                .ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.CancelAppointment(1, "Reason"));
        }

        [Fact]
        public async Task CheckAvailability_ShouldReturnFromCache_WhenCachedDataExists()
        {
            var cachedResponse = new DoctorAvailabilityResponseDto
            {
                DoctorId = 1,
                Date = DateTime.Today,
                IsDoctorOnLeave = false,
                Message = "Doctor is available for the selected date.",
                Slots = new List<DoctorSlotDto>
                {
                    new DoctorSlotDto
                    {
                        TimeSlot = "10:00 AM",
                        IsAvailable = true,
                        Status = "Available"
                    }
                }
            };

            var bytes = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(cachedResponse));

            _cache.Setup(x => x.GetAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(bytes);

            var result = await _service.CheckDoctorAvailability(1, DateTime.Today);

            result.Should().NotBeNull();
            result.DoctorId.Should().Be(1);
            result.Slots.Should().ContainSingle();

            _doctorLeaveRepo.Verify(x =>
                x.IsDoctorOnLeaveAsync(It.IsAny<int>(), It.IsAny<DateTime>()),
                Times.Never);

            _repo.Verify(x =>
                x.GetBookedSlotsAsync(It.IsAny<int>(), It.IsAny<DateTime>()),
                Times.Never);

            _cache.Verify(x =>
                x.SetAsync(
                    It.IsAny<string>(),
                    It.IsAny<byte[]>(),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task CheckAvailability_ShouldSetCache_WhenCacheMiss()
        {
            _cache.Setup(x => x.GetAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[]?)null);

            _doctorLeaveRepo.Setup(x => x.IsDoctorOnLeaveAsync(
                    1,
                    It.IsAny<DateTime>()))
                .ReturnsAsync(false);

            _repo.Setup(x => x.GetBookedSlotsAsync(
                    1,
                    It.IsAny<DateTime>()))
                .ReturnsAsync(new List<string> { "10:00 AM" });

            var result = await _service.CheckDoctorAvailability(1, DateTime.Today);

            result.Should().NotBeNull();

            result.Slots.Should().Contain(x =>
                x.TimeSlot == "10:00 AM" &&
                x.Status == "Booked");

            _cache.Verify(x => x.SetAsync(
                    It.Is<string>(key =>
                        key == $"appointment:doctor:availability:1:{DateTime.Today:yyyyMMdd}"),
                    It.IsAny<byte[]>(),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CheckAvailability_ShouldReturnDoctorOnLeaveSlots_AndSetCache()
        {
            _doctorLeaveRepo.Setup(x => x.IsDoctorOnLeaveAsync(
                    1,
                    It.IsAny<DateTime>()))
                .ReturnsAsync(true);

            var result = await _service.CheckDoctorAvailability(1, DateTime.Today);

            result.Should().NotBeNull();
            result.IsDoctorOnLeave.Should().BeTrue();
            result.Slots.Should().OnlyContain(x =>
                x.Status == "Doctor On Leave" &&
                x.IsAvailable == false);

            _cache.Verify(x => x.SetAsync(
                    It.IsAny<string>(),
                    It.IsAny<byte[]>(),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CheckAvailability_ShouldReturnEmpty_WhenNull()
        {
            _doctorLeaveRepo.Setup(x => x.IsDoctorOnLeaveAsync(
                    1,
                    It.IsAny<DateTime>()))
                .ReturnsAsync(false);

            _repo.Setup(x => x.GetBookedSlotsAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>()))
                .ReturnsAsync((List<string>?)null!);

            var result = await _service.CheckDoctorAvailability(1, DateTime.Now);

            result.Should().NotBeNull();
            result.Slots.Should().NotBeEmpty();
            result.Slots.Should().OnlyContain(x => x.Status == "Available");
        }

        [Fact]
        public async Task GetAppointmentById_ShouldThrow_WhenNotFound()
        {
            _repo.Setup(x => x.getbyidAsync(1))
                .ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.GetAppointmentById(1));
        }

        [Fact]
        public async Task GetAppointmentById_ShouldReturnDto()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                DoctorId = 1,
                PatientId = 1
            };

            _repo.Setup(x => x.getbyidAsync(1))
                .ReturnsAsync(appointment);

            _patientRepo.Setup(x => x.getbyidAsync(1))
                .ReturnsAsync(new Patient());

            _doctorRepo.Setup(x => x.getbyidAsync(1))
                .ReturnsAsync(new Doctor());

            _mapper.Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(new AppointmentDto());

            var result = await _service.GetAppointmentById(1);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAppointmentById_ShouldNotLoadNavigation_WhenAlreadyLoaded()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                DoctorId = 1,
                PatientId = 1,
                Patient = new Patient(),
                Doctor = new Doctor()
            };

            _repo.Setup(x => x.getbyidAsync(1))
                .ReturnsAsync(appointment);

            _mapper.Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(new AppointmentDto());

            var result = await _service.GetAppointmentById(1);

            result.Should().NotBeNull();

            _patientRepo.Verify(x => x.getbyidAsync(It.IsAny<int>()), Times.Never);
            _doctorRepo.Verify(x => x.getbyidAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetAppointmentsByDoctor_ShouldReturnList()
        {
            _doctorRepo.Setup(x => x.GetByIdentityUserIdAsync("doc1"))
                .ReturnsAsync(new Doctor { DoctorId = 1 });

            _repo.Setup(x => x.GetByDoctorIdAsync(1))
                .ReturnsAsync(new List<Appointment>());

            _mapper.Setup(x => x.Map<List<AppointmentDto>>(It.IsAny<object>()))
                .Returns(new List<AppointmentDto>());

            var result = await _service.GetAppointmentsByDoctorAsync("doc1");

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAppointmentsByDoctor_ShouldThrow_WhenDoctorNotFound()
        {
            _doctorRepo.Setup(x => x.GetByIdentityUserIdAsync("doc"))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.GetAppointmentsByDoctorAsync("doc"));
        }

        [Fact]
        public async Task GetAppointmentsByUser_ShouldReturnList()
        {
            _patientRepo.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync(new Patient { PatientId = 1 });

            _repo.Setup(x => x.GetByPatientIdAsync(1))
                .ReturnsAsync(new List<Appointment>());

            _mapper.Setup(x => x.Map<List<AppointmentDto>>(It.IsAny<object>()))
                .Returns(new List<AppointmentDto>());

            var result = await _service.GetAppointmentsByUserAsync("user1");

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAppointmentsByUser_ShouldThrow_WhenPatientNotFound()
        {
            _patientRepo.Setup(x => x.GetByIdentityUserIdAsync("user"))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.GetAppointmentsByUserAsync("user"));
        }

        [Fact]
        public async Task GetUpcomingAppointments_ShouldReturnList()
        {
            var list = new List<Appointment>
            {
                new Appointment()
            };

            _repo.Setup(x => x.GetUpcomingByDoctorAsync(
                    1,
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()))
                .ReturnsAsync(list);

            _mapper.Setup(x => x.Map<List<AppointmentDto>>(It.IsAny<object>()))
                .Returns(new List<AppointmentDto>());

            var result = await _service.GetUpcomingAppointmentsByDoctor(
                1,
                DateTime.Now,
                DateTime.Now.AddDays(1));

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task IsSlotBooked_ShouldReturnTrue()
        {
            _repo.Setup(x => x.IsSlotBookedAsync(
                    1,
                    It.IsAny<DateTime>(),
                    "10:00 AM"))
                .ReturnsAsync(true);

            var result = await _service.IsSlotBooked(
                1,
                DateTime.Now,
                "10:00 AM");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsSlotBooked_ShouldReturnFalse()
        {
            _repo.Setup(x => x.IsSlotBookedAsync(
                    1,
                    It.IsAny<DateTime>(),
                    "10:00 AM"))
                .ReturnsAsync(false);

            var result = await _service.IsSlotBooked(
                1,
                DateTime.Now,
                "10:00 AM");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetPagedAppointments_ShouldThrow_InvalidPageNumber()
        {
            await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.GetPagedAppointments(0, 10));
        }

        [Fact]
        public async Task GetPagedAppointments_ShouldThrow_InvalidPageSize()
        {
            await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.GetPagedAppointments(1, 0));
        }

        [Fact]
        public async Task GetPagedAppointments_ShouldReturnData()
        {
            _repo.Setup(x => x.GetPagedAppointmentsAsync(1, 10))
                .ReturnsAsync((new List<Appointment>(), 1));

            _mapper.Setup(x => x.Map<List<AppointmentDto>>(It.IsAny<object>()))
                .Returns(new List<AppointmentDto>());

            var result = await _service.GetPagedAppointments(1, 10);

            result.TotalCount.Should().Be(1);
        }

        [Fact]
        public async Task Filtered_ShouldThrow_WhenIdsMissing()
        {
            await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.GetAppointmentsByPatientAndDoctorPaged(
                    null,
                    null,
                    1,
                    10));
        }

        [Fact]
        public async Task Filtered_ShouldThrow_InvalidPageNumber()
        {
            await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.GetAppointmentsByPatientAndDoctorPaged(
                    1,
                    null,
                    0,
                    10));
        }

        [Fact]
        public async Task Filtered_ShouldThrow_InvalidPageSize()
        {
            await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.GetAppointmentsByPatientAndDoctorPaged(
                    1,
                    null,
                    1,
                    0));
        }

        [Fact]
        public async Task Filtered_ShouldReturnData()
        {
            _repo.Setup(x => x.GetByPatientAndDoctor(
                    1,
                    null,
                    1,
                    10))
                .ReturnsAsync((new List<Appointment>(), 1));

            _mapper.Setup(x => x.Map<List<AppointmentDto>>(It.IsAny<object>()))
                .Returns(new List<AppointmentDto>());

            var result =
                await _service.GetAppointmentsByPatientAndDoctorPaged(
                    1,
                    null,
                    1,
                    10);

            result.TotalCount.Should().Be(1);
        }
    }
}