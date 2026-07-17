using AutoMapper;
using HealthApp.Api.Data;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Dependencies;
using HealthApp.Api.Services.Impl;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HealthApp.Api.Tests
{
    public class DoctorLeaveServiceTests : IDisposable
    {
        private readonly Mock<IDoctorLeaveRepository>
            _doctorLeaveRepository;
        private readonly Mock<IDoctorRepository> _doctorRepository;
        private readonly Mock<IAppointmentRepository>
            _appointmentRepository;
        private readonly Mock<IPatientRepository> _patientRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IPublishEndpoint> _publishEndpoint;
        private readonly Mock<IDistributedCache> _cache;
        private readonly Mock<ILogger<DoctorLeaveService>> _logger;
        private readonly HealthAppDbContext _context;
        private readonly DoctorLeaveService _service;

        public DoctorLeaveServiceTests()
        {
            _doctorLeaveRepository =
                new Mock<IDoctorLeaveRepository>();
            _doctorRepository = new Mock<IDoctorRepository>();
            _appointmentRepository =
                new Mock<IAppointmentRepository>();
            _patientRepository = new Mock<IPatientRepository>();
            _mapper = new Mock<IMapper>();
            _publishEndpoint = new Mock<IPublishEndpoint>();
            _cache = new Mock<IDistributedCache>();
            _logger = new Mock<ILogger<DoctorLeaveService>>();

            var dbOptions =
                new DbContextOptionsBuilder<HealthAppDbContext>()
                    .Options;

            _context = new HealthAppDbContext(dbOptions);

            var dependencies = new DoctorLeaveServiceDependencies(
                _context,
                _doctorLeaveRepository.Object,
                _doctorRepository.Object,
                _appointmentRepository.Object,
                _patientRepository.Object);

            _service = new DoctorLeaveService(
                dependencies,
                _mapper.Object,
                _publishEndpoint.Object,
                _cache.Object,
                _logger.Object);
        }

        [Fact]
        public async Task PreviewLeaveAsync_ReturnsPendingConfirmedAndTotalCounts()
        {
            const int doctorId = 7;
            var startDate = DateOnly.FromDateTime(
                DateTime.Today.AddDays(5));
            var endDate = startDate.AddDays(2);
            var dto = CreateLeaveDto(startDate, endDate);
            var doctor = CreateDoctor(doctorId);

            var affectedAppointments = new List<Appointment>
            {
                CreateAppointment(
                    1,
                    doctorId,
                    startDate,
                    AppointmentStatus.Pending),
                CreateAppointment(
                    2,
                    doctorId,
                    startDate.AddDays(1),
                    AppointmentStatus.Pending),
                CreateAppointment(
                    3,
                    doctorId,
                    endDate,
                    AppointmentStatus.Confirmed)
            };

            SetupValidDoctorAndNonOverlappingLeave(
                doctorId,
                doctor);

            _appointmentRepository
                .Setup(repository =>
                    repository
                        .GetActiveAppointmentsForDoctorDateRangeAsync(
                            doctorId,
                            startDate,
                            endDate,
                            It.IsAny<CancellationToken>()))
                .ReturnsAsync(affectedAppointments);

            var result = await _service.PreviewLeaveAsync(
                doctorId,
                dto);

            Assert.Equal(doctorId, result.DoctorId);
            Assert.Equal(startDate, result.StartDate);
            Assert.Equal(endDate, result.EndDate);
            Assert.Equal(2, result.PendingAppointmentCount);
            Assert.Equal(1, result.ConfirmedAppointmentCount);
            Assert.Equal(3, result.TotalAffectedAppointmentCount);
            Assert.Contains("3 active appointment(s)", result.Message);
        }

        [Fact]
        public async Task PreviewLeaveAsync_WhenNoAppointments_ReturnsZeroCounts()
        {
            const int doctorId = 4;
            var startDate = DateOnly.FromDateTime(
                DateTime.Today.AddDays(3));
            var dto = CreateLeaveDto(startDate, startDate);
            var doctor = CreateDoctor(doctorId);

            SetupValidDoctorAndNonOverlappingLeave(
                doctorId,
                doctor);

            _appointmentRepository
                .Setup(repository =>
                    repository
                        .GetActiveAppointmentsForDoctorDateRangeAsync(
                            doctorId,
                            startDate,
                            startDate,
                            It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Appointment>());

            var result = await _service.PreviewLeaveAsync(
                doctorId,
                dto);

            Assert.Equal(0, result.PendingAppointmentCount);
            Assert.Equal(0, result.ConfirmedAppointmentCount);
            Assert.Equal(0, result.TotalAffectedAppointmentCount);
            Assert.Equal(
                "No active appointments will be affected by this leave.",
                result.Message);
        }

        [Fact]
        public async Task PreviewLeaveAsync_WithSingleDayLeave_UsesSameStartAndEndDate()
        {
            const int doctorId = 3;
            var leaveDate = DateOnly.FromDateTime(
                DateTime.Today.AddDays(7));
            var dto = CreateLeaveDto(leaveDate, leaveDate);
            var doctor = CreateDoctor(doctorId);

            SetupValidDoctorAndNonOverlappingLeave(
                doctorId,
                doctor);

            _appointmentRepository
                .Setup(repository =>
                    repository
                        .GetActiveAppointmentsForDoctorDateRangeAsync(
                            doctorId,
                            leaveDate,
                            leaveDate,
                            It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Appointment>
                {
                    CreateAppointment(
                        10,
                        doctorId,
                        leaveDate,
                        AppointmentStatus.Confirmed)
                });

            var result = await _service.PreviewLeaveAsync(
                doctorId,
                dto);

            Assert.Equal(leaveDate, result.StartDate);
            Assert.Equal(leaveDate, result.EndDate);
            Assert.Equal(1, result.ConfirmedAppointmentCount);
            Assert.Equal(1, result.TotalAffectedAppointmentCount);
        }

        [Fact]
        public async Task PreviewLeaveAsync_WhenLeaveOverlaps_ThrowsBusinessRuleViolationException()
        {
            const int doctorId = 5;
            var startDate = DateOnly.FromDateTime(
                DateTime.Today.AddDays(4));
            var endDate = startDate.AddDays(2);
            var dto = CreateLeaveDto(startDate, endDate);
            var doctor = CreateDoctor(doctorId);

            _doctorRepository
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _doctorLeaveRepository
                .Setup(repository =>
                    repository.HasOverlappingLeaveAsync(
                        doctorId,
                        startDate,
                        endDate,
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<
                BusinessRuleViolationException>(
                () => _service.PreviewLeaveAsync(doctorId, dto));

            Assert.Equal(
                "The selected leave dates overlap with an existing leave record.",
                exception.Message);

            _appointmentRepository.Verify(
                repository =>
                    repository
                        .GetActiveAppointmentsForDoctorDateRangeAsync(
                            It.IsAny<int>(),
                            It.IsAny<DateOnly>(),
                            It.IsAny<DateOnly>(),
                            It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task PreviewLeaveAsync_WhenStartDateIsPast_ThrowsBusinessRuleViolationException()
        {
            const int doctorId = 2;
            var startDate = DateOnly.FromDateTime(
                DateTime.Today.AddDays(-1));
            var dto = CreateLeaveDto(
                startDate,
                startDate.AddDays(1));

            var exception = await Assert.ThrowsAsync<
                BusinessRuleViolationException>(
                () => _service.PreviewLeaveAsync(doctorId, dto));

            Assert.Equal(
                "Leave start date cannot be in the past.",
                exception.Message);

            _doctorRepository.Verify(
                repository => repository.GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task PreviewLeaveAsync_WhenEndDateIsBeforeStartDate_ThrowsBusinessRuleViolationException()
        {
            const int doctorId = 8;
            var startDate = DateOnly.FromDateTime(
                DateTime.Today.AddDays(5));
            var dto = CreateLeaveDto(
                startDate,
                startDate.AddDays(-1));

            var exception = await Assert.ThrowsAsync<
                BusinessRuleViolationException>(
                () => _service.PreviewLeaveAsync(doctorId, dto));

            Assert.Equal(
                "Leave end date cannot be before the start date.",
                exception.Message);
        }

        [Fact]
        public async Task PreviewLeaveAsync_WhenReasonIsMissing_ThrowsInvalidRequestException()
        {
            const int doctorId = 6;
            var leaveDate = DateOnly.FromDateTime(
                DateTime.Today.AddDays(5));
            var dto = CreateLeaveDto(leaveDate, leaveDate);
            dto.Reason = " ";

            var exception = await Assert.ThrowsAsync<
                InvalidRequestException>(
                () => _service.PreviewLeaveAsync(doctorId, dto));

            Assert.Equal(
                "Leave reason is required.",
                exception.Message);
        }

        [Fact]
        public async Task PreviewLeaveAsync_WhenDoctorDoesNotExist_ThrowsEntityNotFoundException()
        {
            const int doctorId = 99;
            var startDate = DateOnly.FromDateTime(
                DateTime.Today.AddDays(5));
            var dto = CreateLeaveDto(startDate, startDate);

            _doctorRepository
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.PreviewLeaveAsync(doctorId, dto));

            _doctorLeaveRepository.Verify(
                repository => repository.HasOverlappingLeaveAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        private void SetupValidDoctorAndNonOverlappingLeave(
            int doctorId,
            Doctor doctor)
        {
            _doctorRepository
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _doctorLeaveRepository
                .Setup(repository =>
                    repository.HasOverlappingLeaveAsync(
                        doctorId,
                        It.IsAny<DateOnly>(),
                        It.IsAny<DateOnly>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
        }

        private static DoctorLeaveCreateDto CreateLeaveDto(
            DateOnly startDate,
            DateOnly endDate)
        {
            return new DoctorLeaveCreateDto
            {
                StartDate = startDate,
                EndDate = endDate,
                Reason = "Medical conference"
            };
        }

        private static Doctor CreateDoctor(int doctorId)
        {
            return new Doctor
            {
                DoctorId = doctorId,
                FullName = "Dr. Test Doctor",
                IsActive = true
            };
        }

        private static Appointment CreateAppointment(
            int appointmentId,
            int doctorId,
            DateOnly scheduledDate,
            AppointmentStatus status)
        {
            return new Appointment
            {
                AppointmentId = appointmentId,
                DoctorId = doctorId,
                PatientId = appointmentId + 100,
                ScheduledDate = scheduledDate,
                TimeSlot = "09:00 AM - 09:30 AM",
                Status = status
            };
        }
    }
}