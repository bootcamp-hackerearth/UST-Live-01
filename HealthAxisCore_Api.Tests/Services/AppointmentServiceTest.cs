using System.Text.Json;

using AutoMapper;

using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.Enums;

using HealthAxisCore_Api.Constants;
using HealthAxisCore_Api.Contracts;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Repositories.Interface;
using HealthAxisCore_Api.Services.Implementations;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Moq;

namespace HealthAxisCore_Api.Tests.Services
{
    public sealed class AppointmentServiceTests
        : IDisposable
    {
        private readonly Mock<IAppointmentRepository>
            _appointmentRepositoryMock;

        private readonly Mock<IDoctorRepository>
            _doctorRepositoryMock;

        private readonly Mock<IPatientRepository>
            _patientRepositoryMock;

        private readonly Mock<IDoctorLeaveRepository>
            _doctorLeaveRepositoryMock;

        private readonly Mock<IMapper>
            _mapperMock;

        private readonly Mock<ILogger<AppointmentService>>
            _loggerMock;

        private readonly SqliteConnection _connection;

        private readonly HealthAppDbContext _dbContext;

        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock =
                new Mock<IAppointmentRepository>();

            _doctorRepositoryMock =
                new Mock<IDoctorRepository>();

            _patientRepositoryMock =
                new Mock<IPatientRepository>();

            _doctorLeaveRepositoryMock =
                new Mock<IDoctorLeaveRepository>();

            _mapperMock =
                new Mock<IMapper>();

            _loggerMock =
                new Mock<ILogger<AppointmentService>>();

            /*
             * Foreign keys are disabled only for these unit tests.
             * The tests focus on AppointmentService and the outbox
             * transaction, not Patient/Doctor relational integrity.
             *
             * SQLite still provides a relational database and real
             * transaction support.
             */
            _connection = new SqliteConnection(
                "Data Source=:memory:;Foreign Keys=False");

            _connection.Open();

            var databaseOptions =
                new DbContextOptionsBuilder<
                    HealthAppDbContext>()
                    .UseSqlite(_connection)
                    .Options;

            _dbContext =
                new HealthAppDbContext(
                    databaseOptions);

            _dbContext.Database.EnsureCreated();

            _service = new AppointmentService(
                _appointmentRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _doctorLeaveRepositoryMock.Object,
                _dbContext,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        // ============================================================
        // TEST CLEANUP
        // ============================================================

        public void Dispose()
        {
            _dbContext.Dispose();
            _connection.Dispose();

            GC.SuppressFinalize(this);
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
                ScheduledDate =
                    DateTime.Today.AddDays(
                        daysFromToday),
                TimeSlot = timeSlot
            };
        }

        private static Appointment CreateAppointment(
            int appointmentId = 1,
            int patientId = 1,
            int doctorId = 2,
            AppointmentStatus status =
                AppointmentStatus.Pending,
            int daysFromToday = 1,
            string timeSlot = "10:00")
        {
            return new Appointment
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                DoctorId = doctorId,
                ScheduledDate =
                    DateTime.Today.AddDays(
                        daysFromToday),
                TimeSlot = timeSlot,
                Status = status,
                CreatedDate = DateTime.UtcNow
            };
        }

        private static Patient CreatePatient(
            int patientId = 1,
            string patientName = "Test Patient")
        {
            return new Patient
            {
                PatientId = patientId,
                PatientName = patientName,
                Email =
                    $"patient{patientId}@test.com",
                PhoneNumber = "9876543210"
            };
        }

        private void SetupDoctorAvailable(
            CreateAppointmentDto dto)
        {
            _doctorRepositoryMock
                .Setup(repository =>
                    repository.IsDoctorAvailable(
                        dto.DoctorId,
                        dto.ScheduledDate))
                .ReturnsAsync(true);

            _doctorLeaveRepositoryMock
                .Setup(repository =>
                    repository
                        .GetActiveLeaveForDateAsync(
                            dto.DoctorId,
                            dto.ScheduledDate))
                .ReturnsAsync(
                    (DoctorLeave?)null);
        }

        private void SetupSuccessfulCreation(
            CreateAppointmentDto dto,
            Appointment mappedAppointment,
            Patient? patient,
            AppointmentResponseDto responseDto,
            IEnumerable<Appointment>? existingAppointments =
                null)
        {
            SetupDoctorAvailable(dto);

            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetAllAsync())
                .ReturnsAsync(
                    existingAppointments?.ToList() ??
                    new List<Appointment>());

            _mapperMock
                .Setup(mapper =>
                    mapper.Map<Appointment>(dto))
                .Returns(mappedAppointment);

            _patientRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(
                        dto.PatientId))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(mapper =>
                    mapper.Map<AppointmentResponseDto>(
                        mappedAppointment))
                .Returns(responseDto);
        }

        private async Task<OutboxMessage>
            GetSingleOutboxMessageAsync()
        {
            return await _dbContext.OutboxMessages
                .AsNoTracking()
                .SingleAsync();
        }

        private async Task<AppointmentBookedEvent>
            GetSingleOutboxEventAsync()
        {
            var outboxMessage =
                await GetSingleOutboxMessageAsync();

            var eventMessage =
                JsonSerializer.Deserialize<
                    AppointmentBookedEvent>(
                        outboxMessage.Payload);

            return Assert.IsType<
                AppointmentBookedEvent>(
                    eventMessage);
        }

        // ============================================================
        // GET ALL
        // ============================================================

        [Fact]
        public async Task
            GetAllAsync_ShouldReturnMappedAppointments()
        {
            var appointments =
                new List<Appointment>
                {
                    CreateAppointment(1),
                    CreateAppointment(2)
                };

            var expectedDtos =
                new List<AppointmentResponseDto>
                {
                    new(),
                    new()
                };

            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper =>
                    mapper.Map<
                        IEnumerable<
                            AppointmentResponseDto>>(
                                appointments))
                .Returns(expectedDtos);

            var result =
                await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            _appointmentRepositoryMock.Verify(
                repository =>
                    repository.GetAllAsync(),
                Times.Once);

            _mapperMock.Verify(
                mapper =>
                    mapper.Map<
                        IEnumerable<
                            AppointmentResponseDto>>(
                                appointments),
                Times.Once);
        }

        [Fact]
        public async Task
            GetAllAsync_ShouldReturnEmptyCollection_WhenNoAppointmentsExist()
        {
            var appointments =
                new List<Appointment>();

            var expectedDtos =
                new List<AppointmentResponseDto>();

            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper =>
                    mapper.Map<
                        IEnumerable<
                            AppointmentResponseDto>>(
                                appointments))
                .Returns(expectedDtos);

            var result =
                await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        // ============================================================
        // GET PAGED
        // ============================================================

        [Fact]
        public async Task
            GetPagedAsync_ShouldUseDefaultPageNumber_WhenInvalid()
        {
            var appointments =
                new List<Appointment>
                {
                    CreateAppointment(1)
                };

            SetupPagedMapping(appointments);

            var result =
                await _service.GetPagedAsync(
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
        public async Task
            GetPagedAsync_ShouldUseDefaultPageSize_WhenInvalid()
        {
            SetupPagedMapping(
                new List<Appointment>());

            var result =
                await _service.GetPagedAsync(
                    1,
                    0,
                    null,
                    null,
                    null,
                    null);

            Assert.Equal(10, result.PageSize);
        }

        [Fact]
        public async Task
            GetPagedAsync_ShouldCapPageSizeAtOneHundred()
        {
            SetupPagedMapping(
                new List<Appointment>());

            var result =
                await _service.GetPagedAsync(
                    1,
                    500,
                    null,
                    null,
                    null,
                    null);

            Assert.Equal(100, result.PageSize);
        }

        [Fact]
        public async Task
            GetPagedAsync_ShouldFilterByAppointmentId()
        {
            var appointments =
                new List<Appointment>
                {
                    CreateAppointment(100),
                    CreateAppointment(200)
                };

            SetupPagedMapping(appointments);

            var result =
                await _service.GetPagedAsync(
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
        public async Task
            GetPagedAsync_ShouldFilterByPatientId()
        {
            var appointments =
                new List<Appointment>
                {
                    CreateAppointment(
                        1,
                        patientId: 500),

                    CreateAppointment(
                        2,
                        patientId: 700)
                };

            SetupPagedMapping(appointments);

            var result =
                await _service.GetPagedAsync(
                    1,
                    10,
                    "500",
                    null,
                    null,
                    null);

            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task
            GetPagedAsync_ShouldFilterByDoctorId()
        {
            var appointments =
                new List<Appointment>
                {
                    CreateAppointment(
                        1,
                        doctorId: 800),

                    CreateAppointment(
                        2,
                        doctorId: 900)
                };

            SetupPagedMapping(appointments);

            var result =
                await _service.GetPagedAsync(
                    1,
                    10,
                    "800",
                    null,
                    null,
                    null);

            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task
            GetPagedAsync_ShouldFilterByTimeSlot()
        {
            var appointments =
                new List<Appointment>
                {
                    CreateAppointment(
                        1,
                        timeSlot: "10:00 AM"),

                    CreateAppointment(
                        2,
                        timeSlot: "02:00 PM")
                };

            SetupPagedMapping(appointments);

            var result =
                await _service.GetPagedAsync(
                    1,
                    10,
                    "10:00",
                    null,
                    null,
                    null);

            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task
            GetPagedAsync_ShouldFilterByStatus()
        {
            var appointments =
                new List<Appointment>
                {
                    CreateAppointment(
                        1,
                        status:
                            AppointmentStatus.Pending),

                    CreateAppointment(
                        2,
                        status:
                            AppointmentStatus.Confirmed),

                    CreateAppointment(
                        3,
                        status:
                            AppointmentStatus.Cancelled)
                };

            SetupPagedMapping(appointments);

            var result =
                await _service.GetPagedAsync(
                    1,
                    10,
                    null,
                    AppointmentStatus.Confirmed,
                    null,
                    null);

            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task
            GetPagedAsync_ShouldFilterByStartDate()
        {
            var appointments =
                new List<Appointment>
                {
                    CreateAppointment(
                        1,
                        daysFromToday: 1),

                    CreateAppointment(
                        2,
                        daysFromToday: 5)
                };

            SetupPagedMapping(appointments);

            var result =
                await _service.GetPagedAsync(
                    1,
                    10,
                    null,
                    null,
                    DateTime.Today.AddDays(3),
                    null);

            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task
            GetPagedAsync_ShouldFilterByEndDate()
        {
            var appointments =
                new List<Appointment>
                {
                    CreateAppointment(
                        1,
                        daysFromToday: 1),

                    CreateAppointment(
                        2,
                        daysFromToday: 5)
                };

            SetupPagedMapping(appointments);

            var result =
                await _service.GetPagedAsync(
                    1,
                    10,
                    null,
                    null,
                    null,
                    DateTime.Today.AddDays(2));

            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task
            GetPagedAsync_ShouldFilterByDateRange()
        {
            var appointments =
                new List<Appointment>
                {
                    CreateAppointment(
                        1,
                        daysFromToday: 1),

                    CreateAppointment(
                        2,
                        daysFromToday: 5),

                    CreateAppointment(
                        3,
                        daysFromToday: 10)
                };

            SetupPagedMapping(appointments);

            var result =
                await _service.GetPagedAsync(
                    1,
                    10,
                    null,
                    null,
                    DateTime.Today.AddDays(3),
                    DateTime.Today.AddDays(7));

            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task
            GetPagedAsync_ShouldReturnCorrectPagination()
        {
            var appointments =
                Enumerable.Range(1, 12)
                    .Select(index =>
                        CreateAppointment(
                            appointmentId: index,
                            daysFromToday: index))
                    .ToList();

            SetupPagedMapping(appointments);

            var result =
                await _service.GetPagedAsync(
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

        [Fact]
        public async Task
            GetPagedAsync_ShouldOrderByDateDescending()
        {
            var appointments =
                new List<Appointment>
                {
                    CreateAppointment(
                        1,
                        daysFromToday: 1),

                    CreateAppointment(
                        2,
                        daysFromToday: 3),

                    CreateAppointment(
                        3,
                        daysFromToday: 2)
                };

            List<Appointment>? mappedItems = null;

            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper =>
                    mapper.Map<
                        List<AppointmentResponseDto>>(
                            It.IsAny<
                                List<Appointment>>()))
                .Callback<List<Appointment>>(
                    items => mappedItems = items)
                .Returns(
                    new List<AppointmentResponseDto>
                    {
                        new(),
                        new(),
                        new()
                    });

            await _service.GetPagedAsync(
                1,
                10,
                null,
                null,
                null,
                null);

            Assert.NotNull(mappedItems);

            Assert.Equal(
                new[] { 2, 3, 1 },
                mappedItems
                    .Select(item =>
                        item.AppointmentId)
                    .ToArray());
        }

        private void SetupPagedMapping(
            List<Appointment> appointments)
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper =>
                    mapper.Map<
                        List<AppointmentResponseDto>>(
                            It.IsAny<
                                List<Appointment>>()))
                .Returns(
                    (List<Appointment> items) =>
                        items
                            .Select(_ =>
                                new AppointmentResponseDto())
                            .ToList());
        }

        // ============================================================
        // GET BY ID
        // ============================================================

        [Fact]
        public async Task
            GetByIdAsync_ShouldThrow_WhenNotFound()
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(99))
                .ReturnsAsync(
                    (Appointment?)null);

            var exception =
                await Assert.ThrowsAsync<
                    EntityNotFoundException>(
                        () =>
                            _service.GetByIdAsync(
                                99));

            Assert.Equal(
                "Appointment not found",
                exception.Message);
        }

        [Fact]
        public async Task
            GetByIdAsync_ShouldReturnMappedAppointment()
        {
            var appointment =
                CreateAppointment(1);

            var expectedDto =
                new AppointmentResponseDto();

            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(mapper =>
                    mapper.Map<
                        AppointmentResponseDto>(
                            appointment))
                .Returns(expectedDto);

            var result =
                await _service.GetByIdAsync(1);

            Assert.Same(expectedDto, result);
        }

        // ============================================================
        // CREATE VALIDATION
        // ============================================================

        [Fact]
        public async Task
            CreateAsync_ShouldThrow_WhenDtoIsNull()
        {
            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.CreateAsync(
                                null!));

            Assert.Equal(
                "Appointment details are required.",
                exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task
            CreateAsync_ShouldThrow_WhenTimeSlotMissing(
                string? timeSlot)
        {
            var dto =
                CreateValidDto();

            dto.TimeSlot =
                timeSlot!;

            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.CreateAsync(
                                dto));

            Assert.Equal(
                "Time slot is required.",
                exception.Message);
        }

        [Fact]
        public async Task
            CreateAsync_ShouldThrow_WhenTimeSlotInvalid()
        {
            var dto =
                CreateValidDto(
                    timeSlot: "invalid-time");

            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.CreateAsync(
                                dto));

            Assert.Equal(
                "Invalid time slot format.",
                exception.Message);
        }

        [Fact]
        public async Task
            CreateAsync_ShouldThrow_WhenDateInPast()
        {
            var dto =
                CreateValidDto(
                    daysFromToday: -1);

            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.CreateAsync(
                                dto));

            Assert.Equal(
                "Previous date or past time slot " +
                "cannot be booked.",
                exception.Message);
        }

        [Fact]
        public async Task
            CreateAsync_ShouldThrow_WhenTodayTimePassed()
        {
            var pastTime =
                DateTime.Now
                    .AddMinutes(-5)
                    .ToString("HH:mm");

            var dto =
                CreateValidDto(
                    daysFromToday: 0,
                    timeSlot: pastTime);

            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.CreateAsync(
                                dto));

            Assert.Equal(
                "Previous date or past time slot " +
                "cannot be booked.",
                exception.Message);
        }

        [Fact]
        public async Task
            CreateAsync_ShouldThrow_WhenBeyondThirtyDays()
        {
            var dto =
                CreateValidDto(
                    daysFromToday: 31);

            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.CreateAsync(
                                dto));

            Assert.Equal(
                "Appointments can only be booked " +
                "up to 30 days in advance.",
                exception.Message);
        }

        [Fact]
        public async Task
            CreateAsync_ShouldAllowDateExactlyThirtyDaysAhead()
        {
            var dto =
                CreateValidDto(
                    daysFromToday: 30);

            var appointment =
                CreateAppointment(
                    appointmentId: 0,
                    patientId: dto.PatientId,
                    doctorId: dto.DoctorId,
                    daysFromToday: 30);

            var response =
                new AppointmentResponseDto();

            SetupSuccessfulCreation(
                dto,
                appointment,
                CreatePatient(dto.PatientId),
                response);

            var result =
                await _service.CreateAsync(dto);

            Assert.Same(response, result);

            Assert.Single(
                await _dbContext.OutboxMessages
                    .ToListAsync());
        }

        [Fact]
        public async Task
            CreateAsync_ShouldThrow_WhenDoctorUnavailable()
        {
            var dto =
                CreateValidDto();

            _doctorRepositoryMock
                .Setup(repository =>
                    repository.IsDoctorAvailable(
                        dto.DoctorId,
                        dto.ScheduledDate))
                .ReturnsAsync(false);

            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.CreateAsync(
                                dto));

            Assert.Equal(
                "Doctor not available for " +
                "the selected date",
                exception.Message);

            _doctorLeaveRepositoryMock.Verify(
                repository =>
                    repository
                        .GetActiveLeaveForDateAsync(
                            It.IsAny<int>(),
                            It.IsAny<DateTime>()),
                Times.Never);
        }

        [Fact]
        public async Task
            CreateAsync_ShouldThrow_WhenDoctorOnLeave()
        {
            var dto =
                CreateValidDto();

            var leave =
                new DoctorLeave
                {
                    LeaveId = 1,
                    DoctorId = dto.DoctorId,
                    StartDate =
                        dto.ScheduledDate.Date,
                    EndDate =
                        dto.ScheduledDate.Date
                            .AddDays(2),
                    Reason = "Personal leave",
                    CreatedDate =
                        DateTime.UtcNow
                };

            _doctorRepositoryMock
                .Setup(repository =>
                    repository.IsDoctorAvailable(
                        dto.DoctorId,
                        dto.ScheduledDate))
                .ReturnsAsync(true);

            _doctorLeaveRepositoryMock
                .Setup(repository =>
                    repository
                        .GetActiveLeaveForDateAsync(
                            dto.DoctorId,
                            dto.ScheduledDate))
                .ReturnsAsync(leave);

            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.CreateAsync(
                                dto));

            Assert.Contains(
                leave.StartDate.ToString(
                    "dd-MMM-yyyy"),
                exception.Message);

            Assert.Contains(
                leave.EndDate.ToString(
                    "dd-MMM-yyyy"),
                exception.Message);

            Assert.Empty(
                await _dbContext.Appointments
                    .ToListAsync());

            Assert.Empty(
                await _dbContext.OutboxMessages
                    .ToListAsync());
        }

        [Fact]
        public async Task
            CreateAsync_ShouldThrow_WhenDoctorSlotBooked()
        {
            var dto =
                CreateValidDto(
                    patientId: 10,
                    doctorId: 20,
                    timeSlot: "10:00");

            SetupDoctorAvailable(dto);

            var existingAppointments =
                new List<Appointment>
                {
                    CreateAppointment(
                        appointmentId: 1,
                        patientId: 99,
                        doctorId: dto.DoctorId,
                        status:
                            AppointmentStatus.Pending,
                        daysFromToday: 1,
                        timeSlot: "10:00")
                };

            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetAllAsync())
                .ReturnsAsync(
                    existingAppointments);

            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.CreateAsync(
                                dto));

            Assert.Contains(
                "already booked",
                exception.Message,
                StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task
            CreateAsync_ShouldThrow_WhenPatientSameDoctorSameSlot()
        {
            var dto =
                CreateValidDto(
                    patientId: 10,
                    doctorId: 20,
                    timeSlot: "10:00");

            SetupDoctorAvailable(dto);

            var existingAppointment =
                CreateAppointment(
                    appointmentId: 1,
                    patientId: dto.PatientId,
                    doctorId: dto.DoctorId,
                    status:
                        AppointmentStatus.Confirmed,
                    daysFromToday: 1,
                    timeSlot: "10:00");

            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetAllAsync())
                .ReturnsAsync(
                    new List<Appointment>
                    {
                        existingAppointment
                    });

            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.CreateAsync(
                                dto));

            /*
             * The doctor slot check occurs before the
             * patient-specific duplicate check.
             */
            Assert.Contains(
                "already booked",
                exception.Message,
                StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task
            CreateAsync_ShouldThrow_WhenPatientSameDoctorDifferentSlot()
        {
            var dto =
                CreateValidDto(
                    patientId: 10,
                    doctorId: 20,
                    timeSlot: "10:00");

            SetupDoctorAvailable(dto);

            var existingAppointment =
                CreateAppointment(
                    appointmentId: 1,
                    patientId: dto.PatientId,
                    doctorId: dto.DoctorId,
                    status:
                        AppointmentStatus.Pending,
                    daysFromToday: 1,
                    timeSlot: "11:00");

            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetAllAsync())
                .ReturnsAsync(
                    new List<Appointment>
                    {
                        existingAppointment
                    });

            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.CreateAsync(
                                dto));

            Assert.Contains(
                "already have an active appointment " +
                "with this doctor",
                exception.Message,
                StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task
            CreateAsync_ShouldThrow_WhenPatientHasAnotherDoctorSameDate()
        {
            var dto =
                CreateValidDto(
                    patientId: 10,
                    doctorId: 20);

            SetupDoctorAvailable(dto);

            var existingAppointment =
                CreateAppointment(
                    appointmentId: 1,
                    patientId: dto.PatientId,
                    doctorId: 999,
                    status:
                        AppointmentStatus.Pending,
                    daysFromToday: 1,
                    timeSlot: "11:00");

            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetAllAsync())
                .ReturnsAsync(
                    new List<Appointment>
                    {
                        existingAppointment
                    });

            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.CreateAsync(
                                dto));

            Assert.Equal(
                "You already have an active appointment " +
                "on this date. Please choose another date.",
                exception.Message);
        }

        // ============================================================
        // CREATE SUCCESS AND OUTBOX
        // ============================================================

        [Fact]
        public async Task
            CreateAsync_ShouldSaveAppointmentAndPendingOutbox()
        {
            var dto =
                CreateValidDto(
                    patientId: 10,
                    doctorId: 20,
                    timeSlot: "10:00");

            var appointment =
                CreateAppointment(
                    appointmentId: 0,
                    patientId: dto.PatientId,
                    doctorId: dto.DoctorId,
                    status:
                        AppointmentStatus.Confirmed,
                    timeSlot: "unmapped");

            var patient =
                CreatePatient(
                    dto.PatientId,
                    "Test Patient");

            var response =
                new AppointmentResponseDto();

            SetupSuccessfulCreation(
                dto,
                appointment,
                patient,
                response);

            var result =
                await _service.CreateAsync(dto);

            Assert.Same(response, result);

            Assert.True(
                appointment.AppointmentId > 0);

            Assert.Equal(
                AppointmentStatus.Pending,
                appointment.Status);

            Assert.Equal(
                dto.ScheduledDate.Date,
                appointment.ScheduledDate);

            Assert.Equal(
                "10:00",
                appointment.TimeSlot);

            Assert.NotEqual(
                default,
                appointment.CreatedDate);

            var storedAppointment =
                await _dbContext.Appointments
                    .AsNoTracking()
                    .SingleAsync();

            Assert.Equal(
                appointment.AppointmentId,
                storedAppointment.AppointmentId);

            var outboxMessage =
                await GetSingleOutboxMessageAsync();

            Assert.Equal(
                OutboxMessageStatuses.Pending,
                outboxMessage.Status);

            Assert.Equal(
                nameof(AppointmentBookedEvent),
                outboxMessage.EventType);

            Assert.Equal(
                0,
                outboxMessage.RetryCount);

            Assert.NotEqual(
                Guid.Empty,
                outboxMessage.EventId);

            Assert.Null(
                outboxMessage.PublishedDate);

            Assert.Null(
                outboxMessage.LastAttemptDate);

            Assert.Null(
                outboxMessage.NextRetryDate);

            Assert.Null(
                outboxMessage.ErrorMessage);

            _appointmentRepositoryMock.Verify(
                repository =>
                    repository.AddAsync(
                        It.IsAny<Appointment>()),
                Times.Never);
        }

        [Fact]
        public async Task
            CreateAsync_ShouldStoreCorrectOutboxPayload()
        {
            var dto =
                CreateValidDto(
                    patientId: 10,
                    doctorId: 20,
                    timeSlot: "10:00 - 10:30");

            var appointment =
                CreateAppointment(
                    appointmentId: 0,
                    patientId: dto.PatientId,
                    doctorId: dto.DoctorId,
                    timeSlot: dto.TimeSlot);

            var patient =
                CreatePatient(
                    dto.PatientId,
                    "Test Patient");

            SetupSuccessfulCreation(
                dto,
                appointment,
                patient,
                new AppointmentResponseDto());

            await _service.CreateAsync(dto);

            var outboxMessage =
                await GetSingleOutboxMessageAsync();

            var eventMessage =
                await GetSingleOutboxEventAsync();

            Assert.Equal(
                outboxMessage.EventId,
                eventMessage.EventId);

            Assert.Equal(
                appointment.AppointmentId,
                eventMessage.AppointmentId);

            Assert.Equal(
                dto.PatientId,
                eventMessage.PatientId);

            Assert.Equal(
                patient.PatientName,
                eventMessage.PatientName);

            Assert.Equal(
                dto.DoctorId,
                eventMessage.DoctorId);

            Assert.Equal(
                dto.ScheduledDate.Date,
                eventMessage.ScheduledDate);

            Assert.Equal(
                "10:00",
                eventMessage.TimeSlot);
        }

        [Fact]
        public async Task
            CreateAsync_ShouldUseFallbackPatientName_WhenPatientMissing()
        {
            var dto =
                CreateValidDto(
                    patientId: 10,
                    doctorId: 20);

            var appointment =
                CreateAppointment(
                    appointmentId: 0,
                    patientId: dto.PatientId,
                    doctorId: dto.DoctorId);

            SetupSuccessfulCreation(
                dto,
                appointment,
                patient: null,
                new AppointmentResponseDto());

            await _service.CreateAsync(dto);

            var eventMessage =
                await GetSingleOutboxEventAsync();

            Assert.Equal(
                $"Patient #{dto.PatientId}",
                eventMessage.PatientName);
        }

        [Fact]
        public async Task
            CreateAsync_ShouldNormalizeRangeTimeSlot()
        {
            var dto =
                CreateValidDto(
                    timeSlot: "10:00 - 10:30");

            var appointment =
                CreateAppointment(
                    appointmentId: 0,
                    patientId: dto.PatientId,
                    doctorId: dto.DoctorId,
                    timeSlot: dto.TimeSlot);

            SetupSuccessfulCreation(
                dto,
                appointment,
                CreatePatient(dto.PatientId),
                new AppointmentResponseDto());

            await _service.CreateAsync(dto);

            Assert.Equal(
                "10:00",
                appointment.TimeSlot);

            var eventMessage =
                await GetSingleOutboxEventAsync();

            Assert.Equal(
                "10:00",
                eventMessage.TimeSlot);
        }

        [Fact]
        public async Task
            CreateAsync_ShouldIgnoreCancelledAppointment()
        {
            var dto =
                CreateValidDto(
                    patientId: 10,
                    doctorId: 20);

            var cancelledAppointment =
                CreateAppointment(
                    appointmentId: 1,
                    patientId: 99,
                    doctorId: dto.DoctorId,
                    status:
                        AppointmentStatus.Cancelled,
                    timeSlot: dto.TimeSlot);

            var newAppointment =
                CreateAppointment(
                    appointmentId: 0,
                    patientId: dto.PatientId,
                    doctorId: dto.DoctorId);

            var response =
                new AppointmentResponseDto();

            SetupSuccessfulCreation(
                dto,
                newAppointment,
                CreatePatient(dto.PatientId),
                response,
                new[] { cancelledAppointment });

            var result =
                await _service.CreateAsync(dto);

            Assert.Same(response, result);

            Assert.Single(
                await _dbContext.OutboxMessages
                    .ToListAsync());
        }

        [Fact]
        public async Task
            CreateAsync_ShouldIgnoreCompletedAppointment()
        {
            var dto =
                CreateValidDto(
                    patientId: 10,
                    doctorId: 20);

            var completedAppointment =
                CreateAppointment(
                    appointmentId: 1,
                    patientId: dto.PatientId,
                    doctorId: dto.DoctorId,
                    status:
                        AppointmentStatus.Completed,
                    timeSlot: dto.TimeSlot);

            var newAppointment =
                CreateAppointment(
                    appointmentId: 0,
                    patientId: dto.PatientId,
                    doctorId: dto.DoctorId);

            SetupSuccessfulCreation(
                dto,
                newAppointment,
                CreatePatient(dto.PatientId),
                new AppointmentResponseDto(),
                new[] { completedAppointment });

            await _service.CreateAsync(dto);

            Assert.Single(
                await _dbContext.OutboxMessages
                    .ToListAsync());
        }

        // ============================================================
        // DELETE
        // ============================================================

        [Fact]
        public async Task
            DeleteAsync_ShouldThrow_WhenNotFound()
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.Exists(99))
                .ReturnsAsync(false);

            var exception =
                await Assert.ThrowsAsync<
                    EntityNotFoundException>(
                        () =>
                            _service.DeleteAsync(
                                99));

            Assert.Equal(
                "Appointment not found",
                exception.Message);

            _appointmentRepositoryMock.Verify(
                repository =>
                    repository.DeleteAsync(
                        It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task
            DeleteAsync_ShouldDelete_WhenFound()
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.Exists(1))
                .ReturnsAsync(true);

            var result =
                await _service.DeleteAsync(1);

            Assert.True(result);

            _appointmentRepositoryMock.Verify(
                repository =>
                    repository.DeleteAsync(1),
                Times.Once);
        }

        // ============================================================
        // GET BY DOCTOR
        // ============================================================

        [Fact]
        public async Task
            GetByDoctorAsync_ShouldThrow_WhenEmpty()
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByDoctor(2))
                .ReturnsAsync(
                    new List<Appointment>());

            var exception =
                await Assert.ThrowsAsync<
                    EntityNotFoundException>(
                        () =>
                            _service.GetByDoctorAsync(
                                2));

            Assert.Equal(
                "No appointments found for this doctor",
                exception.Message);
        }

        [Fact]
        public async Task
            GetByDoctorAsync_ShouldReturnMappedItems()
        {
            var appointments =
                new List<Appointment>
                {
                    CreateAppointment(
                        1,
                        doctorId: 2),

                    CreateAppointment(
                        2,
                        doctorId: 2)
                };

            var expectedDtos =
                new List<AppointmentResponseDto>
                {
                    new(),
                    new()
                };

            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByDoctor(2))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper =>
                    mapper.Map<
                        IEnumerable<
                            AppointmentResponseDto>>(
                                appointments))
                .Returns(expectedDtos);

            var result =
                await _service.GetByDoctorAsync(2);

            Assert.Equal(2, result.Count());
        }

        // ============================================================
        // GET BY PATIENT
        // ============================================================

        [Fact]
        public async Task
            GetByPatientAsync_ShouldThrow_WhenEmpty()
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByPatient(1))
                .ReturnsAsync(
                    new List<Appointment>());

            var exception =
                await Assert.ThrowsAsync<
                    EntityNotFoundException>(
                        () =>
                            _service.GetByPatientAsync(
                                1));

            Assert.Equal(
                "No appointments found for this patient",
                exception.Message);
        }

        [Fact]
        public async Task
            GetByPatientAsync_ShouldReturnMappedItems()
        {
            var appointments =
                new List<Appointment>
                {
                    CreateAppointment(
                        1,
                        patientId: 1),

                    CreateAppointment(
                        2,
                        patientId: 1)
                };

            var expectedDtos =
                new List<AppointmentResponseDto>
                {
                    new(),
                    new()
                };

            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByPatient(1))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper =>
                    mapper.Map<
                        IEnumerable<
                            AppointmentResponseDto>>(
                                appointments))
                .Returns(expectedDtos);

            var result =
                await _service.GetByPatientAsync(1);

            Assert.Equal(2, result.Count());
        }

        // ============================================================
        // FILTER
        // ============================================================

        [Fact]
        public async Task
            FilterAsync_ShouldThrow_WhenEmpty()
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.FilterAppointments(
                        AppointmentStatus.Confirmed,
                        It.IsAny<DateTime?>(),
                        It.IsAny<DateTime?>()))
                .ReturnsAsync(
                    new List<Appointment>());

            var exception =
                await Assert.ThrowsAsync<
                    EntityNotFoundException>(
                        () =>
                            _service.FilterAsync(
                                AppointmentStatus.Confirmed,
                                null,
                                null));

            Assert.Equal(
                "No appointments found for given criteria",
                exception.Message);
        }

        [Fact]
        public async Task
            FilterAsync_ShouldReturnMappedItems()
        {
            var startDate =
                DateTime.Today;

            var endDate =
                DateTime.Today.AddDays(7);

            var appointments =
                new List<Appointment>
                {
                    CreateAppointment(
                        1,
                        status:
                            AppointmentStatus.Confirmed)
                };

            var expectedDtos =
                new List<AppointmentResponseDto>
                {
                    new()
                };

            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.FilterAppointments(
                        AppointmentStatus.Confirmed,
                        startDate,
                        endDate))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper =>
                    mapper.Map<
                        IEnumerable<
                            AppointmentResponseDto>>(
                                appointments))
                .Returns(expectedDtos);

            var result =
                await _service.FilterAsync(
                    AppointmentStatus.Confirmed,
                    startDate,
                    endDate);

            Assert.Single(result);
        }

        // ============================================================
        // CANCEL
        // ============================================================

        [Fact]
        public async Task
            CancelAsync_ShouldThrow_WhenNotFound()
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(99))
                .ReturnsAsync(
                    (Appointment?)null);

            var exception =
                await Assert.ThrowsAsync<
                    EntityNotFoundException>(
                        () =>
                            _service.CancelAsync(
                                99,
                                "Test reason"));

            Assert.Equal(
                "Appointment not found",
                exception.Message);
        }

        [Theory]
        [InlineData(AppointmentStatus.Cancelled)]
        [InlineData(
            AppointmentStatus.DoctorUnavailable)]
        public async Task
            CancelAsync_ShouldThrow_WhenAlreadyCancelled(
                AppointmentStatus status)
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(1))
                .ReturnsAsync(
                    CreateAppointment(
                        status: status));

            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.CancelAsync(
                                1,
                                "Test reason"));

            Assert.Equal(
                "Appointment already cancelled",
                exception.Message);
        }

        [Fact]
        public async Task
            CancelAsync_ShouldThrow_WhenCompleted()
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(1))
                .ReturnsAsync(
                    CreateAppointment(
                        status:
                            AppointmentStatus.Completed));

            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.CancelAsync(
                                1,
                                "Test reason"));

            Assert.Equal(
                "Cannot cancel a completed appointment",
                exception.Message);
        }

        [Theory]
        [InlineData(AppointmentStatus.Pending)]
        [InlineData(AppointmentStatus.Confirmed)]
        public async Task
            CancelAsync_ShouldCancelActiveAppointment(
                AppointmentStatus status)
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(1))
                .ReturnsAsync(
                    CreateAppointment(
                        status: status));

            var result =
                await _service.CancelAsync(
                    1,
                    "Cancelled by patient");

            Assert.True(result);

            _appointmentRepositoryMock.Verify(
                repository =>
                    repository.CancelAppointment(
                        1,
                        "Cancelled by patient"),
                Times.Once);
        }

        // ============================================================
        // CONFIRM
        // ============================================================

        [Fact]
        public async Task
            ConfirmAsync_ShouldThrow_WhenNotFound()
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(99))
                .ReturnsAsync(
                    (Appointment?)null);

            await Assert.ThrowsAsync<
                EntityNotFoundException>(
                    () =>
                        _service.ConfirmAsync(99));
        }

        [Theory]
        [InlineData(
            AppointmentStatus.Completed,
            "Cannot confirm a completed appointment")]
        [InlineData(
            AppointmentStatus.Cancelled,
            "Cannot confirm a cancelled appointment")]
        [InlineData(
            AppointmentStatus.DoctorUnavailable,
            "Cannot confirm an appointment cancelled " +
            "due to doctor unavailability")]
        [InlineData(
            AppointmentStatus.Confirmed,
            "Appointment is already confirmed")]
        public async Task
            ConfirmAsync_ShouldRejectInvalidStatus(
                AppointmentStatus status,
                string expectedMessage)
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(1))
                .ReturnsAsync(
                    CreateAppointment(
                        status: status));

            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.ConfirmAsync(1));

            Assert.Equal(
                expectedMessage,
                exception.Message);
        }

        [Fact]
        public async Task
            ConfirmAsync_ShouldConfirmPendingAppointment()
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(1))
                .ReturnsAsync(
                    CreateAppointment(
                        status:
                            AppointmentStatus.Pending));

            var result =
                await _service.ConfirmAsync(1);

            Assert.True(result);

            _appointmentRepositoryMock.Verify(
                repository =>
                    repository.ConfirmAppointment(1),
                Times.Once);
        }

        // ============================================================
        // COMPLETE
        // ============================================================

        [Fact]
        public async Task
            CompleteAsync_ShouldThrow_WhenNotFound()
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(99))
                .ReturnsAsync(
                    (Appointment?)null);

            await Assert.ThrowsAsync<
                EntityNotFoundException>(
                    () =>
                        _service.CompleteAsync(99));
        }

        [Theory]
        [InlineData(
            AppointmentStatus.Completed,
            "Appointment is already completed")]
        [InlineData(
            AppointmentStatus.Cancelled,
            "Cannot complete a cancelled appointment")]
        [InlineData(
            AppointmentStatus.DoctorUnavailable,
            "Cannot complete an appointment cancelled " +
            "due to doctor unavailability")]
        [InlineData(
            AppointmentStatus.Pending,
            "Only confirmed appointments can be completed")]
        public async Task
            CompleteAsync_ShouldRejectInvalidStatus(
                AppointmentStatus status,
                string expectedMessage)
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(1))
                .ReturnsAsync(
                    CreateAppointment(
                        status: status));

            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.CompleteAsync(1));

            Assert.Equal(
                expectedMessage,
                exception.Message);
        }

        [Fact]
        public async Task
            CompleteAsync_ShouldCompleteConfirmedAppointment()
        {
            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetByIdAsync(1))
                .ReturnsAsync(
                    CreateAppointment(
                        status:
                            AppointmentStatus.Confirmed));

            var result =
                await _service.CompleteAsync(1);

            Assert.True(result);

            _appointmentRepositoryMock.Verify(
                repository =>
                    repository.CompleteAppointment(1),
                Times.Once);
        }

        // ============================================================
        // BOOKED SLOTS
        // ============================================================

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public async Task
            GetBookedSlotsAsync_ShouldThrow_WhenDoctorIdInvalid(
                int doctorId)
        {
            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.GetBookedSlotsAsync(
                                doctorId,
                                DateTime.Today));

            Assert.Equal(
                "Doctor ID is required.",
                exception.Message);
        }

        [Fact]
        public async Task
            GetBookedSlotsAsync_ShouldThrow_WhenDateDefault()
        {
            var exception =
                await Assert.ThrowsAsync<
                    AppointmentRuleException>(
                        () =>
                            _service.GetBookedSlotsAsync(
                                1,
                                default));

            Assert.Equal(
                "Appointment date is required.",
                exception.Message);
        }

        [Fact]
        public async Task
            GetBookedSlotsAsync_ShouldNormalizeDeduplicateAndSort()
        {
            var date =
                DateTime.Today.AddDays(1);

            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetBookedSlotsAsync(
                        1,
                        date))
                .ReturnsAsync(
                    new List<string>
                    {
                        "11:00",
                        "09:00",
                        "10:00 - 10:30",
                        "11:00",
                        "09:00 AM",
                        ""
                    });

            var result =
                (await _service.GetBookedSlotsAsync(
                    1,
                    date))
                .ToList();

            Assert.Equal(3, result.Count);
            Assert.Equal("09:00", result[0]);
            Assert.Equal("10:00", result[1]);
            Assert.Equal("11:00", result[2]);
        }

        [Fact]
        public async Task
            GetBookedSlotsAsync_ShouldReturnEmpty_WhenNoSlots()
        {
            var date =
                DateTime.Today.AddDays(1);

            _appointmentRepositoryMock
                .Setup(repository =>
                    repository.GetBookedSlotsAsync(
                        1,
                        date))
                .ReturnsAsync(
                    new List<string>());

            var result =
                await _service.GetBookedSlotsAsync(
                    1,
                    date);

            Assert.Empty(result);
        }
    }
}