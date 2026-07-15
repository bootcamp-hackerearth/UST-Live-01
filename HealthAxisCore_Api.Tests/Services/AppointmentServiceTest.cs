using AutoMapper;
using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.Enums;
using HealthAxisCore_Api.Contracts;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Repositories.Interface;
using HealthAxisCore_Api.Services.Implementations;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;

namespace HealthAxisCore_Api.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IDoctorLeaveRepository> _doctorLeaveRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly Mock<ILogger<AppointmentService>> _loggerMock;

        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _doctorLeaveRepositoryMock = new Mock<IDoctorLeaveRepository>();
            _mapperMock = new Mock<IMapper>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _loggerMock = new Mock<ILogger<AppointmentService>>();

            _service = new AppointmentService(
                _appointmentRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _doctorLeaveRepositoryMock.Object,
                _mapperMock.Object,
                _publishEndpointMock.Object,
                _loggerMock.Object);
        }

        // ============================================================
        // TEST HELPERS
        // ============================================================

        private static CreateAppointmentDto CreateValidDto(
            int patientId = 1,
            int doctorId = 2,
            int daysFromToday = 1,
            string timeSlot = "10:00")
        {
            return new CreateAppointmentDto
            {
                PatientId = patientId,
                DoctorId = doctorId,
                ScheduledDate = DateTime.Today.AddDays(daysFromToday),
                TimeSlot = timeSlot
            };
        }

        private static Appointment CreateAppointment(
            int appointmentId = 1,
            int patientId = 1,
            int doctorId = 2,
            AppointmentStatus status = AppointmentStatus.Pending,
            int daysFromToday = 1,
            string timeSlot = "10:00")
        {
            return new Appointment
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                DoctorId = doctorId,
                ScheduledDate = DateTime.Today.AddDays(daysFromToday),
                TimeSlot = timeSlot,
                Status = status,
                CreatedDate = DateTime.Now
            };
        }

        private void SetupDoctorAvailable(CreateAppointmentDto dto)
        {
            _doctorRepositoryMock
                .Setup(repository => repository.IsDoctorAvailable(
                    dto.DoctorId,
                    dto.ScheduledDate))
                .ReturnsAsync(true);

            _doctorLeaveRepositoryMock
                .Setup(repository => repository.GetActiveLeaveForDateAsync(
                    dto.DoctorId,
                    dto.ScheduledDate))
                .ReturnsAsync((DoctorLeave?)null);
        }

        // ============================================================
        // GET ALL
        // ============================================================

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedAppointments()
        {
            var appointments = new List<Appointment>
            {
                CreateAppointment(1),
                CreateAppointment(2)
            };

            var expectedDtos = new List<AppointmentResponseDto>
            {
                new(),
                new()
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments))
                .Returns(expectedDtos);

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            _appointmentRepositoryMock.Verify(
                repository => repository.GetAllAsync(),
                Times.Once);

            _mapperMock.Verify(
                mapper => mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments),
                Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyCollection_WhenNoAppointmentsExist()
        {
            var appointments = new List<Appointment>();
            var expectedDtos = new List<AppointmentResponseDto>();

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments))
                .Returns(expectedDtos);

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        // ============================================================
        // GET PAGED
        // ============================================================

        [Fact]
        public async Task GetPagedAsync_ShouldUseDefaultPageNumber_WhenPageNumberIsInvalid()
        {
            var appointments = new List<Appointment>
            {
                CreateAppointment(1)
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentResponseDto>>(
                    It.IsAny<List<Appointment>>()))
                .Returns(new List<AppointmentResponseDto> { new() });

            var result = await _service.GetPagedAsync(
                0,
                10,
                null,
                null,
                null,
                null);

            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldUseDefaultPageSize_WhenPageSizeIsInvalid()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            _mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentResponseDto>>(
                    It.IsAny<List<Appointment>>()))
                .Returns(new List<AppointmentResponseDto>());

            var result = await _service.GetPagedAsync(
                1,
                0,
                null,
                null,
                null,
                null);

            Assert.Equal(10, result.PageSize);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldCapPageSizeAtOneHundred()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            _mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentResponseDto>>(
                    It.IsAny<List<Appointment>>()))
                .Returns(new List<AppointmentResponseDto>());

            var result = await _service.GetPagedAsync(
                1,
                500,
                null,
                null,
                null,
                null);

            Assert.Equal(100, result.PageSize);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldFilterByAppointmentIdSearch()
        {
            var appointments = new List<Appointment>
            {
                CreateAppointment(100),
                CreateAppointment(200)
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentResponseDto>>(
                    It.Is<List<Appointment>>(items =>
                        items.Count == 1 &&
                        items[0].AppointmentId == 100)))
                .Returns(new List<AppointmentResponseDto> { new() });

            var result = await _service.GetPagedAsync(
                1,
                10,
                "100",
                null,
                null,
                null);

            Assert.Equal(1, result.TotalCount);
            Assert.Single(result.Items);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldFilterByPatientIdSearch()
        {
            var appointments = new List<Appointment>
            {
                CreateAppointment(1, patientId: 500),
                CreateAppointment(2, patientId: 700)
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentResponseDto>>(
                    It.Is<List<Appointment>>(items =>
                        items.Count == 1 &&
                        items[0].PatientId == 500)))
                .Returns(new List<AppointmentResponseDto> { new() });

            var result = await _service.GetPagedAsync(
                1,
                10,
                "500",
                null,
                null,
                null);

            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldFilterByDoctorIdSearch()
        {
            var appointments = new List<Appointment>
            {
                CreateAppointment(1, doctorId: 800),
                CreateAppointment(2, doctorId: 900)
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentResponseDto>>(
                    It.Is<List<Appointment>>(items =>
                        items.Count == 1 &&
                        items[0].DoctorId == 800)))
                .Returns(new List<AppointmentResponseDto> { new() });

            var result = await _service.GetPagedAsync(
                1,
                10,
                "800",
                null,
                null,
                null);

            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldFilterByTimeSlotSearch()
        {
            var appointments = new List<Appointment>
            {
                CreateAppointment(1, timeSlot: "10:00 AM"),
                CreateAppointment(2, timeSlot: "02:00 PM")
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentResponseDto>>(
                    It.Is<List<Appointment>>(items =>
                        items.Count == 1 &&
                        items[0].AppointmentId == 1)))
                .Returns(new List<AppointmentResponseDto> { new() });

            var result = await _service.GetPagedAsync(
                1,
                10,
                "10:00",
                null,
                null,
                null);

            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldFilterByStatus()
        {
            var appointments = new List<Appointment>
            {
                CreateAppointment(1, status: AppointmentStatus.Pending),
                CreateAppointment(2, status: AppointmentStatus.Confirmed),
                CreateAppointment(3, status: AppointmentStatus.Cancelled)
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentResponseDto>>(
                    It.Is<List<Appointment>>(items =>
                        items.Count == 1 &&
                        items[0].Status == AppointmentStatus.Confirmed)))
                .Returns(new List<AppointmentResponseDto> { new() });

            var result = await _service.GetPagedAsync(
                1,
                10,
                null,
                AppointmentStatus.Confirmed,
                null,
                null);

            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldFilterByStartAndEndDates()
        {
            var appointments = new List<Appointment>
            {
                CreateAppointment(1, daysFromToday: 1),
                CreateAppointment(2, daysFromToday: 5),
                CreateAppointment(3, daysFromToday: 10)
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentResponseDto>>(
                    It.Is<List<Appointment>>(items =>
                        items.Count == 1 &&
                        items[0].AppointmentId == 2)))
                .Returns(new List<AppointmentResponseDto> { new() });

            var result = await _service.GetPagedAsync(
                1,
                10,
                null,
                null,
                DateTime.Today.AddDays(3),
                DateTime.Today.AddDays(7));

            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldReturnCorrectPagination()
        {
            var appointments = Enumerable.Range(1, 12)
                .Select(index => CreateAppointment(
                    appointmentId: index,
                    daysFromToday: index))
                .ToList();

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentResponseDto>>(
                    It.IsAny<List<Appointment>>()))
                .Returns((List<Appointment> items) =>
                    items.Select(_ => new AppointmentResponseDto()).ToList());

            var result = await _service.GetPagedAsync(
                2,
                5,
                null,
                null,
                null,
                null);

            Assert.Equal(12, result.TotalCount);
            Assert.Equal(3, result.TotalPages);
            Assert.Equal(2, result.PageNumber);
            Assert.Equal(5, result.PageSize);
            Assert.Equal(5, result.Items.Count);
        }

        // ============================================================
        // GET BY ID
        // ============================================================

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenAppointmentDoesNotExist()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Appointment?)null);

            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.GetByIdAsync(99));

            Assert.Equal("Appointment not found", exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedAppointment()
        {
            var appointment = CreateAppointment(1);
            var expectedDto = new AppointmentResponseDto();

            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentResponseDto>(appointment))
                .Returns(expectedDto);

            var result = await _service.GetByIdAsync(1);

            Assert.Same(expectedDto, result);

            _mapperMock.Verify(
                mapper => mapper.Map<AppointmentResponseDto>(appointment),
                Times.Once);
        }

        // ============================================================
        // CREATE
        // ============================================================

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenDtoIsNull()
        {
            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CreateAsync(null!));

            Assert.Equal(
                "Appointment details are required.",
                exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CreateAsync_ShouldThrow_WhenTimeSlotIsMissing(string? timeSlot)
        {
            var dto = CreateValidDto();
            dto.TimeSlot = timeSlot!;

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CreateAsync(dto));

            Assert.Equal("Time slot is required.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenTimeSlotFormatIsInvalid()
        {
            var dto = CreateValidDto(timeSlot: "invalid-time");

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CreateAsync(dto));

            Assert.Equal("Invalid time slot format.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenAppointmentDateIsInPast()
        {
            var dto = CreateValidDto(
                daysFromToday: -1,
                timeSlot: "10:00");

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CreateAsync(dto));

            Assert.Equal(
                "Previous date or past time slot cannot be booked.",
                exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenTodayTimeSlotHasPassed()
        {
            var pastTime = DateTime.Now.AddMinutes(-5).ToString("HH:mm");

            var dto = CreateValidDto(
                daysFromToday: 0,
                timeSlot: pastTime);

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CreateAsync(dto));

            Assert.Equal(
                "Previous date or past time slot cannot be booked.",
                exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenDateIsBeyondThirtyDays()
        {
            var dto = CreateValidDto(daysFromToday: 31);

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CreateAsync(dto));

            Assert.Equal(
                "Appointments can only be booked up to 30 days in advance.",
                exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenDoctorIsNotAvailable()
        {
            var dto = CreateValidDto();

            _doctorRepositoryMock
                .Setup(repository => repository.IsDoctorAvailable(
                    dto.DoctorId,
                    dto.ScheduledDate))
                .ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CreateAsync(dto));

            Assert.Equal(
                "Doctor not available for the selected date",
                exception.Message);

            _doctorLeaveRepositoryMock.Verify(
                repository => repository.GetActiveLeaveForDateAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenDoctorIsOnLeave()
        {
            var dto = CreateValidDto();

            var leave = new DoctorLeave
            {
                LeaveId = 1,
                DoctorId = dto.DoctorId,
                StartDate = dto.ScheduledDate.Date,
                EndDate = dto.ScheduledDate.Date.AddDays(2),
                Reason = "Personal leave",
                CreatedDate = DateTime.Now
            };

            _doctorRepositoryMock
                .Setup(repository => repository.IsDoctorAvailable(
                    dto.DoctorId,
                    dto.ScheduledDate))
                .ReturnsAsync(true);

            _doctorLeaveRepositoryMock
                .Setup(repository => repository.GetActiveLeaveForDateAsync(
                    dto.DoctorId,
                    dto.ScheduledDate))
                .ReturnsAsync(leave);

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CreateAsync(dto));

            Assert.Contains(
                leave.StartDate.ToString("dd-MMM-yyyy"),
                exception.Message);

            Assert.Contains(
                leave.EndDate.ToString("dd-MMM-yyyy"),
                exception.Message);

            Assert.Contains(
                "Please choose another doctor",
                exception.Message);

            _appointmentRepositoryMock.Verify(
                repository => repository.AddAsync(It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenDoctorSlotIsAlreadyBooked()
        {
            var dto = CreateValidDto(
                patientId: 10,
                doctorId: 20,
                timeSlot: "10:00");

            SetupDoctorAvailable(dto);

            var existingAppointments = new List<Appointment>
            {
                CreateAppointment(
                    appointmentId: 1,
                    patientId: 99,
                    doctorId: dto.DoctorId,
                    status: AppointmentStatus.Pending,
                    daysFromToday: 1,
                    timeSlot: "10:00")
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(existingAppointments);

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CreateAsync(dto));

            Assert.Contains(
                "already booked",
                exception.Message,
                StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task CreateAsync_ShouldIgnoreCancelledAppointment_WhenCheckingDoctorSlot()
        {
            var dto = CreateValidDto(
                patientId: 10,
                doctorId: 20,
                timeSlot: "10:00");

            SetupDoctorAvailable(dto);

            var cancelledAppointment = CreateAppointment(
                appointmentId: 1,
                patientId: 99,
                doctorId: dto.DoctorId,
                status: AppointmentStatus.Cancelled,
                daysFromToday: 1,
                timeSlot: "10:00");

            var mappedAppointment = CreateAppointment(
                appointmentId: 2,
                patientId: dto.PatientId,
                doctorId: dto.DoctorId,
                status: AppointmentStatus.Pending,
                daysFromToday: 1,
                timeSlot: "10:00");

            var responseDto = new AppointmentResponseDto();

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment> { cancelledAppointment });

            _mapperMock
                .Setup(mapper => mapper.Map<Appointment>(dto))
                .Returns(mappedAppointment);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(new Patient
                {
                    PatientId = dto.PatientId,
                    PatientName = "Test Patient",
                    Email = "patient@test.com",
                    PhoneNumber = "9876543210"

                });

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentResponseDto>(mappedAppointment))
                .Returns(responseDto);

            _publishEndpointMock
                .Setup(endpoint => endpoint.Publish(
                    It.IsAny<AppointmentBookedEvent>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            Assert.Same(responseDto, result);
        }

        [Fact]
        public async Task CreateAsync_ShouldIgnoreCompletedAppointment_WhenCheckingConflicts()
        {
            var dto = CreateValidDto(
                patientId: 10,
                doctorId: 20);

            SetupDoctorAvailable(dto);

            var completedAppointment = CreateAppointment(
                appointmentId: 1,
                patientId: dto.PatientId,
                doctorId: dto.DoctorId,
                status: AppointmentStatus.Completed,
                daysFromToday: 1,
                timeSlot: dto.TimeSlot);

            var mappedAppointment = CreateAppointment(
                appointmentId: 2,
                patientId: dto.PatientId,
                doctorId: dto.DoctorId);

            var responseDto = new AppointmentResponseDto();

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment> { completedAppointment });

            _mapperMock
                .Setup(mapper => mapper.Map<Appointment>(dto))
                .Returns(mappedAppointment);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(new Patient
                {
                    PatientId = dto.PatientId,
                    PatientName = "Test Patient",
                    Email = "patient@test.com",
                    PhoneNumber = "9876543210"
                });

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentResponseDto>(mappedAppointment))
                .Returns(responseDto);

            _publishEndpointMock
                .Setup(endpoint => endpoint.Publish(
                    It.IsAny<AppointmentBookedEvent>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            Assert.Same(responseDto, result);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenPatientHasSameDoctorSameDateAndTime()
        {
            var dto = CreateValidDto(
                patientId: 10,
                doctorId: 20,
                timeSlot: "10:00");

            SetupDoctorAvailable(dto);

            var existingAppointment = CreateAppointment(
                appointmentId: 1,
                patientId: dto.PatientId,
                doctorId: dto.DoctorId,
                status: AppointmentStatus.Confirmed,
                daysFromToday: 1,
                timeSlot: "10:00");

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment> { existingAppointment });

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CreateAsync(dto));

            Assert.Contains(
                "same date and time slot",
                exception.Message,
                StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenPatientHasSameDoctorOnSameDateDifferentTime()
        {
            var dto = CreateValidDto(
                patientId: 10,
                doctorId: 20,
                timeSlot: "10:00");

            SetupDoctorAvailable(dto);

            var existingAppointment = CreateAppointment(
                appointmentId: 1,
                patientId: dto.PatientId,
                doctorId: dto.DoctorId,
                status: AppointmentStatus.Pending,
                daysFromToday: 1,
                timeSlot: "11:00");

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment> { existingAppointment });

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CreateAsync(dto));

            Assert.Contains(
                "already have an active appointment with this doctor",
                exception.Message,
                StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenPatientHasAppointmentWithAnotherDoctorSameDate()
        {
            var dto = CreateValidDto(
                patientId: 10,
                doctorId: 20,
                timeSlot: "10:00");

            SetupDoctorAvailable(dto);

            var existingAppointment = CreateAppointment(
                appointmentId: 1,
                patientId: dto.PatientId,
                doctorId: 999,
                status: AppointmentStatus.Pending,
                daysFromToday: 1,
                timeSlot: "11:00");

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment> { existingAppointment });

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CreateAsync(dto));

            Assert.Equal(
                "You already have an active appointment on this date. Please choose another date.",
                exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreatePendingAppointmentAndPublishEvent()
        {
            var dto = CreateValidDto(
                patientId: 10,
                doctorId: 20,
                timeSlot: "10:00");

            SetupDoctorAvailable(dto);

            var mappedAppointment = CreateAppointment(
                appointmentId: 50,
                patientId: dto.PatientId,
                doctorId: dto.DoctorId,
                status: AppointmentStatus.Confirmed,
                daysFromToday: 1,
                timeSlot: "10:00");

            var patient = new Patient
            {
                PatientId = dto.PatientId,
                PatientName = "Test Patient",
                Email = "patient@test.com",
                PhoneNumber = "9876543210"
            };

            var responseDto = new AppointmentResponseDto();

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            _mapperMock
                .Setup(mapper => mapper.Map<Appointment>(dto))
                .Returns(mappedAppointment);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentResponseDto>(mappedAppointment))
                .Returns(responseDto);

            _publishEndpointMock
                .Setup(endpoint => endpoint.Publish(
                    It.IsAny<AppointmentBookedEvent>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            Assert.Same(responseDto, result);
            Assert.Equal(AppointmentStatus.Pending, mappedAppointment.Status);
            Assert.Equal(dto.ScheduledDate.Date, mappedAppointment.ScheduledDate);
            Assert.Equal("10:00", mappedAppointment.TimeSlot);
            Assert.NotEqual(default, mappedAppointment.CreatedDate);

            _appointmentRepositoryMock.Verify(
                repository => repository.AddAsync(
                    It.Is<Appointment>(appointment =>
                        appointment.PatientId == dto.PatientId &&
                        appointment.DoctorId == dto.DoctorId &&
                        appointment.Status == AppointmentStatus.Pending &&
                        appointment.TimeSlot == "10:00")),
                Times.Once);

            _publishEndpointMock.Verify(
                endpoint => endpoint.Publish(
                    It.Is<AppointmentBookedEvent>(eventMessage =>
                        eventMessage.AppointmentId == mappedAppointment.AppointmentId &&
                        eventMessage.PatientName == patient.PatientName &&
                        eventMessage.DoctorId == dto.DoctorId &&
                        eventMessage.ScheduledDate == dto.ScheduledDate.Date &&
                        eventMessage.TimeSlot == "10:00"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldUseFallbackPatientName_WhenPatientNotFound()
        {
            var dto = CreateValidDto(
                patientId: 10,
                doctorId: 20);

            SetupDoctorAvailable(dto);

            var mappedAppointment = CreateAppointment(
                appointmentId: 50,
                patientId: dto.PatientId,
                doctorId: dto.DoctorId);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            _mapperMock
                .Setup(mapper => mapper.Map<Appointment>(dto))
                .Returns(mappedAppointment);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync((Patient?)null);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentResponseDto>(mappedAppointment))
                .Returns(new AppointmentResponseDto());

            _publishEndpointMock
                .Setup(endpoint => endpoint.Publish(
                    It.IsAny<AppointmentBookedEvent>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _service.CreateAsync(dto);

            _publishEndpointMock.Verify(
                endpoint => endpoint.Publish(
                    It.Is<AppointmentBookedEvent>(eventMessage =>
                        eventMessage.PatientName == $"Patient #{dto.PatientId}"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldNormalizeTimeSlotContainingRange()
        {
            var dto = CreateValidDto(timeSlot: "10:00 - 10:30");

            SetupDoctorAvailable(dto);

            var mappedAppointment = CreateAppointment(
                patientId: dto.PatientId,
                doctorId: dto.DoctorId,
                timeSlot: dto.TimeSlot);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            _mapperMock
                .Setup(mapper => mapper.Map<Appointment>(dto))
                .Returns(mappedAppointment);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync((Patient?)null);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentResponseDto>(mappedAppointment))
                .Returns(new AppointmentResponseDto());

            _publishEndpointMock
                .Setup(endpoint => endpoint.Publish(
                    It.IsAny<AppointmentBookedEvent>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _service.CreateAsync(dto);

            Assert.Equal("10:00", mappedAppointment.TimeSlot);
        }

        // ============================================================
        // DELETE
        // ============================================================

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenAppointmentDoesNotExist()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.Exists(99))
                .ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.DeleteAsync(99));

            Assert.Equal("Appointment not found", exception.Message);

            _appointmentRepositoryMock.Verify(
                repository => repository.DeleteAsync(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAppointment_WhenAppointmentExists()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.Exists(1))
                .ReturnsAsync(true);

            var result = await _service.DeleteAsync(1);

            Assert.True(result);

            _appointmentRepositoryMock.Verify(
                repository => repository.DeleteAsync(1),
                Times.Once);
        }

        // ============================================================
        // GET BY DOCTOR
        // ============================================================

        [Fact]
        public async Task GetByDoctorAsync_ShouldThrow_WhenNoAppointmentsExist()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByDoctor(2))
                .ReturnsAsync(new List<Appointment>());

            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.GetByDoctorAsync(2));

            Assert.Equal(
                "No appointments found for this doctor",
                exception.Message);
        }

        [Fact]
        public async Task GetByDoctorAsync_ShouldReturnMappedAppointments()
        {
            var appointments = new List<Appointment>
            {
                CreateAppointment(1, doctorId: 2),
                CreateAppointment(2, doctorId: 2)
            };

            var expectedDtos = new List<AppointmentResponseDto>
            {
                new(),
                new()
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetByDoctor(2))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments))
                .Returns(expectedDtos);

            var result = await _service.GetByDoctorAsync(2);

            Assert.Equal(2, result.Count());
        }

        // ============================================================
        // GET BY PATIENT
        // ============================================================

        [Fact]
        public async Task GetByPatientAsync_ShouldThrow_WhenNoAppointmentsExist()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByPatient(1))
                .ReturnsAsync(new List<Appointment>());

            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.GetByPatientAsync(1));

            Assert.Equal(
                "No appointments found for this patient",
                exception.Message);
        }

        [Fact]
        public async Task GetByPatientAsync_ShouldReturnMappedAppointments()
        {
            var appointments = new List<Appointment>
            {
                CreateAppointment(1, patientId: 1),
                CreateAppointment(2, patientId: 1)
            };

            var expectedDtos = new List<AppointmentResponseDto>
            {
                new(),
                new()
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetByPatient(1))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments))
                .Returns(expectedDtos);

            var result = await _service.GetByPatientAsync(1);

            Assert.Equal(2, result.Count());
        }

        // ============================================================
        // FILTER
        // ============================================================

        [Fact]
        public async Task FilterAsync_ShouldThrow_WhenNoAppointmentsMatch()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.FilterAppointments(
                    AppointmentStatus.Confirmed,
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>()))
                .ReturnsAsync(new List<Appointment>());

            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.FilterAsync(
                    AppointmentStatus.Confirmed,
                    null,
                    null));

            Assert.Equal(
                "No appointments found for given criteria",
                exception.Message);
        }

        [Fact]
        public async Task FilterAsync_ShouldReturnMappedAppointments()
        {
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(7);

            var appointments = new List<Appointment>
            {
                CreateAppointment(
                    1,
                    status: AppointmentStatus.Confirmed)
            };

            var expectedDtos = new List<AppointmentResponseDto>
            {
                new()
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.FilterAppointments(
                    AppointmentStatus.Confirmed,
                    startDate,
                    endDate))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments))
                .Returns(expectedDtos);

            var result = await _service.FilterAsync(
                AppointmentStatus.Confirmed,
                startDate,
                endDate);

            Assert.Single(result);
        }

        // ============================================================
        // CANCEL
        // ============================================================

        [Fact]
        public async Task CancelAsync_ShouldThrow_WhenAppointmentDoesNotExist()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Appointment?)null);

            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.CancelAsync(99, "Test reason"));

            Assert.Equal("Appointment not found", exception.Message);
        }

        [Fact]
        public async Task CancelAsync_ShouldThrow_WhenAppointmentIsAlreadyCancelled()
        {
            var appointment = CreateAppointment(
                status: AppointmentStatus.Cancelled);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CancelAsync(1, "Test reason"));

            Assert.Equal("Appointment already cancelled", exception.Message);
        }

        [Fact]
        public async Task CancelAsync_ShouldThrow_WhenDoctorIsUnavailable()
        {
            var appointment = CreateAppointment(
                status: AppointmentStatus.DoctorUnavailable);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CancelAsync(1, "Test reason"));

            Assert.Equal("Appointment already cancelled", exception.Message);
        }

        [Fact]
        public async Task CancelAsync_ShouldThrow_WhenAppointmentIsCompleted()
        {
            var appointment = CreateAppointment(
                status: AppointmentStatus.Completed);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CancelAsync(1, "Test reason"));

            Assert.Equal(
                "Cannot cancel a completed appointment",
                exception.Message);
        }

        [Theory]
        [InlineData(AppointmentStatus.Pending)]
        [InlineData(AppointmentStatus.Confirmed)]
        public async Task CancelAsync_ShouldCancelActiveAppointment(
            AppointmentStatus status)
        {
            var appointment = CreateAppointment(status: status);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var result = await _service.CancelAsync(
                1,
                "Cancelled by patient");

            Assert.True(result);

            _appointmentRepositoryMock.Verify(
                repository => repository.CancelAppointment(
                    1,
                    "Cancelled by patient"),
                Times.Once);
        }

        // ============================================================
        // CONFIRM
        // ============================================================

        [Fact]
        public async Task ConfirmAsync_ShouldThrow_WhenAppointmentDoesNotExist()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Appointment?)null);

            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.ConfirmAsync(99));

            Assert.Equal("Appointment not found", exception.Message);
        }

        [Fact]
        public async Task ConfirmAsync_ShouldThrow_WhenAppointmentIsCompleted()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    status: AppointmentStatus.Completed));

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.ConfirmAsync(1));

            Assert.Equal(
                "Cannot confirm a completed appointment",
                exception.Message);
        }

        [Fact]
        public async Task ConfirmAsync_ShouldThrow_WhenAppointmentIsCancelled()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    status: AppointmentStatus.Cancelled));

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.ConfirmAsync(1));

            Assert.Equal(
                "Cannot confirm a cancelled appointment",
                exception.Message);
        }

        [Fact]
        public async Task ConfirmAsync_ShouldThrow_WhenDoctorIsUnavailable()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    status: AppointmentStatus.DoctorUnavailable));

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.ConfirmAsync(1));

            Assert.Equal(
                "Cannot confirm an appointment cancelled due to doctor unavailability",
                exception.Message);
        }

        [Fact]
        public async Task ConfirmAsync_ShouldThrow_WhenAppointmentIsAlreadyConfirmed()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    status: AppointmentStatus.Confirmed));

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.ConfirmAsync(1));

            Assert.Equal(
                "Appointment is already confirmed",
                exception.Message);
        }

        [Fact]
        public async Task ConfirmAsync_ShouldConfirmPendingAppointment()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    status: AppointmentStatus.Pending));

            var result = await _service.ConfirmAsync(1);

            Assert.True(result);

            _appointmentRepositoryMock.Verify(
                repository => repository.ConfirmAppointment(1),
                Times.Once);
        }

        // ============================================================
        // COMPLETE
        // ============================================================

        [Fact]
        public async Task CompleteAsync_ShouldThrow_WhenAppointmentDoesNotExist()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Appointment?)null);

            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.CompleteAsync(99));

            Assert.Equal("Appointment not found", exception.Message);
        }

        [Fact]
        public async Task CompleteAsync_ShouldThrow_WhenAppointmentAlreadyCompleted()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    status: AppointmentStatus.Completed));

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CompleteAsync(1));

            Assert.Equal(
                "Appointment is already completed",
                exception.Message);
        }

        [Fact]
        public async Task CompleteAsync_ShouldThrow_WhenAppointmentIsCancelled()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    status: AppointmentStatus.Cancelled));

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CompleteAsync(1));

            Assert.Equal(
                "Cannot complete a cancelled appointment",
                exception.Message);
        }

        [Fact]
        public async Task CompleteAsync_ShouldThrow_WhenDoctorIsUnavailable()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    status: AppointmentStatus.DoctorUnavailable));

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CompleteAsync(1));

            Assert.Equal(
                "Cannot complete an appointment cancelled due to doctor unavailability",
                exception.Message);
        }

        [Fact]
        public async Task CompleteAsync_ShouldThrow_WhenAppointmentIsPending()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    status: AppointmentStatus.Pending));

            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.CompleteAsync(1));

            Assert.Equal(
                "Only confirmed appointments can be completed",
                exception.Message);
        }

        [Fact]
        public async Task CompleteAsync_ShouldCompleteConfirmedAppointment()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    status: AppointmentStatus.Confirmed));

            var result = await _service.CompleteAsync(1);

            Assert.True(result);

            _appointmentRepositoryMock.Verify(
                repository => repository.CompleteAppointment(1),
                Times.Once);
        }

        // ============================================================
        // GET BOOKED SLOTS
        // ============================================================

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public async Task GetBookedSlotsAsync_ShouldThrow_WhenDoctorIdIsInvalid(
            int doctorId)
        {
            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.GetBookedSlotsAsync(
                    doctorId,
                    DateTime.Today));

            Assert.Equal("Doctor ID is required.", exception.Message);
        }

        [Fact]
        public async Task GetBookedSlotsAsync_ShouldThrow_WhenDateIsDefault()
        {
            var exception = await Assert.ThrowsAsync<AppointmentRuleException>(
                () => _service.GetBookedSlotsAsync(
                    1,
                    default));

            Assert.Equal(
                "Appointment date is required.",
                exception.Message);
        }

        [Fact]
        public async Task GetBookedSlotsAsync_ShouldNormalizeRemoveDuplicatesAndSort()
        {
            var date = DateTime.Today.AddDays(1);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetBookedSlotsAsync(1, date))
                .ReturnsAsync(new List<string>
                {
                    "11:00",
                    "09:00",
                    "10:00 - 10:30",
                    "11:00",
                    "09:00 AM",
                    ""
                });

            var result = (await _service.GetBookedSlotsAsync(
                1,
                date)).ToList();

            Assert.Equal(3, result.Count);
            Assert.Equal("09:00", result[0]);
            Assert.Equal("10:00", result[1]);
            Assert.Equal("11:00", result[2]);
        }

        [Fact]
        public async Task GetBookedSlotsAsync_ShouldReturnEmptyList_WhenNoSlotsExist()
        {
            var date = DateTime.Today.AddDays(1);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetBookedSlotsAsync(1, date))
                .ReturnsAsync(new List<string>());

            var result = await _service.GetBookedSlotsAsync(1, date);

            Assert.Empty(result);
        }
    }
}