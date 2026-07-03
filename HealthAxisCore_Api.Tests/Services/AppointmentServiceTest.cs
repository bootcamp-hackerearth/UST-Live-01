using AutoMapper;
using FluentAssertions;
using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.Enums;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Implementations;
using Moq;
using Xunit;

namespace HealthAxisCore_Api.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new AppointmentService(
                _appointmentRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        // ------------------------------------------------------------
        // GetAllAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedAppointments()
        {
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 10,
                    DoctorId = 20,
                    ScheduledDate = DateTime.Today.AddDays(1),
                    TimeSlot = "10:00",
                    Status = AppointmentStatus.Pending
                }
            };

            var mappedAppointments = new List<AppointmentResponseDto>
            {
                new AppointmentResponseDto
                {
                    AppointmentId = 1,
                    PatientId = 10,
                    DoctorId = 20,
                    ScheduledDate = DateTime.Today.AddDays(1),
                    TimeSlot = "10:00",
                    Status = AppointmentStatus.Pending
                }
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments))
                .Returns(mappedAppointments);

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().AppointmentId.Should().Be(1);

            _appointmentRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        // ------------------------------------------------------------
        // GetPagedAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task GetPagedAsync_WhenPageNumberLessThanOne_ShouldDefaultToOne()
        {
            var appointments = GetSampleAppointments();

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(appointments);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                0,
                10,
                null,
                null,
                null,
                null
            );

            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalCount.Should().Be(appointments.Count);
        }

        [Fact]
        public async Task GetPagedAsync_WhenPageSizeLessThanOne_ShouldDefaultToTen()
        {
            var appointments = GetSampleAppointments();

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(appointments);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                0,
                null,
                null,
                null,
                null
            );

            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task GetPagedAsync_WhenPageSizeGreaterThanHundred_ShouldLimitToHundred()
        {
            var appointments = GetSampleAppointments();

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(appointments);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                200,
                null,
                null,
                null,
                null
            );

            result.PageSize.Should().Be(100);
        }

        [Fact]
        public async Task GetPagedAsync_WithSearchByAppointmentId_ShouldReturnMatchingAppointments()
        {
            var appointments = GetSampleAppointments();

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(appointments);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                "1",
                null,
                null,
                null
            );

            result.TotalCount.Should().BeGreaterThan(0);
            result.Items.Should().Contain(x => x.AppointmentId == 1);
        }

        [Fact]
        public async Task GetPagedAsync_WithSearchByPatientId_ShouldReturnMatchingAppointments()
        {
            var appointments = GetSampleAppointments();

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(appointments);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                "101",
                null,
                null,
                null
            );

            result.TotalCount.Should().Be(1);
            result.Items.First().PatientId.Should().Be(101);
        }

        [Fact]
        public async Task GetPagedAsync_WithSearchByDoctorId_ShouldReturnMatchingAppointments()
        {
            var appointments = GetSampleAppointments();

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(appointments);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                "201",
                null,
                null,
                null
            );

            result.TotalCount.Should().BeGreaterThan(0);
            result.Items.Should().Contain(x => x.DoctorId == 201);
        }

        [Fact]
        public async Task GetPagedAsync_WithSearchByTimeSlot_ShouldReturnMatchingAppointments()
        {
            var appointments = GetSampleAppointments();

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(appointments);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                "10:00",
                null,
                null,
                null
            );

            result.TotalCount.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetPagedAsync_WithStatusFilter_ShouldReturnOnlyMatchingStatus()
        {
            var appointments = GetSampleAppointments();

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(appointments);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                null,
                AppointmentStatus.Pending,
                null,
                null
            );

            result.Items.Should().OnlyContain(x => x.Status == AppointmentStatus.Pending);
        }

        [Fact]
        public async Task GetPagedAsync_WithStartDate_ShouldReturnAppointmentsAfterStartDate()
        {
            var appointments = GetSampleAppointments();

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(appointments);

            SetupPagedMapper();

            var startDate = DateTime.Today.AddDays(2);

            var result = await _service.GetPagedAsync(
                1,
                10,
                null,
                null,
                startDate,
                null
            );

            result.Items.Should().OnlyContain(x => x.ScheduledDate.Date >= startDate.Date);
        }

        [Fact]
        public async Task GetPagedAsync_WithEndDate_ShouldReturnAppointmentsBeforeEndDate()
        {
            var appointments = GetSampleAppointments();

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(appointments);

            SetupPagedMapper();

            var endDate = DateTime.Today.AddDays(2);

            var result = await _service.GetPagedAsync(
                1,
                10,
                null,
                null,
                null,
                endDate
            );

            result.Items.Should().OnlyContain(x => x.ScheduledDate.Date <= endDate.Date);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldApplyPagination()
        {
            var appointments = GetSampleAppointments();

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(appointments);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                2,
                null,
                null,
                null,
                null
            );

            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(appointments.Count);
            result.TotalPages.Should().Be((int)Math.Ceiling(appointments.Count / 2.0));
        }

        // ------------------------------------------------------------
        // GetByIdAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task GetByIdAsync_WhenAppointmentExists_ShouldReturnMappedAppointment()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 100,
                DoctorId = 200,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00",
                Status = AppointmentStatus.Pending
            };

            var response = new AppointmentResponseDto
            {
                AppointmentId = 1,
                PatientId = 100,
                DoctorId = 200,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00",
                Status = AppointmentStatus.Pending
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentResponseDto>(appointment))
                .Returns(response);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.AppointmentId.Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_WhenAppointmentDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            var act = async () => await _service.GetByIdAsync(1);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Appointment not found");
        }

        // ------------------------------------------------------------
        // CreateAsync Validation
        // ------------------------------------------------------------

        [Fact]
        public async Task CreateAsync_WhenDtoIsNull_ShouldThrowAppointmentRuleException()
        {
            var act = async () => await _service.CreateAsync(null!);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Appointment details are required.");
        }

        [Fact]
        public async Task CreateAsync_WhenTimeSlotIsEmpty_ShouldThrowAppointmentRuleException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = ""
            };

            var act = async () => await _service.CreateAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Time slot is required.");
        }

        [Fact]
        public async Task CreateAsync_WhenTimeSlotIsWhitespace_ShouldThrowAppointmentRuleException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "   "
            };

            var act = async () => await _service.CreateAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Time slot is required.");
        }

        [Fact]
        public async Task CreateAsync_WhenTimeSlotFormatIsInvalid_ShouldThrowAppointmentRuleException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "invalid-time-format"
            };

            var act = async () => await _service.CreateAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Invalid time slot format.");
        }

        [Fact]
        public async Task CreateAsync_WhenScheduledDateTimeIsInPast_ShouldThrowAppointmentRuleException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(-1),
                TimeSlot = "10:00"
            };

            var act = async () => await _service.CreateAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Previous date or past time slot cannot be booked.");
        }

        [Fact]
        public async Task CreateAsync_WhenScheduledDateIsMoreThanThirtyDaysAhead_ShouldThrowAppointmentRuleException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(31),
                TimeSlot = "10:00"
            };

            var act = async () => await _service.CreateAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Appointments can only be booked up to 30 days in advance.");
        }

        [Fact]
        public async Task CreateAsync_WhenDoctorIsUnavailable_ShouldThrowAppointmentRuleException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00"
            };

            _doctorRepositoryMock
                .Setup(repo => repo.IsDoctorAvailable(dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(false);

            var act = async () => await _service.CreateAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Doctor not available for the selected date");
        }

        [Fact]
        public async Task CreateAsync_WhenDoctorSlotAlreadyBooked_ShouldThrowAppointmentRuleException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 2,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00"
            };

            var existingAppointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = dto.ScheduledDate.Date,
                    TimeSlot = "10:00",
                    Status = AppointmentStatus.Pending
                }
            };

            _doctorRepositoryMock
                .Setup(repo => repo.IsDoctorAvailable(dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(true);

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(existingAppointments);

            var act = async () => await _service.CreateAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("This doctor is already booked for the selected date and time slot. Please choose another slot.");
        }

        [Fact]
        public async Task CreateAsync_WhenDoctorSlotAlreadyBookedWithTimeRange_ShouldThrowAppointmentRuleException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 2,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00"
            };

            var existingAppointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = dto.ScheduledDate.Date,
                    TimeSlot = "10:00 - 10:30",
                    Status = AppointmentStatus.Confirmed
                }
            };

            _doctorRepositoryMock
                .Setup(repo => repo.IsDoctorAvailable(dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(true);

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(existingAppointments);

            var act = async () => await _service.CreateAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>();
        }

        [Fact]
        public async Task CreateAsync_WhenPatientHasSameDoctorSameDateSameSlot_ShouldThrowAppointmentRuleException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00"
            };

            var existingAppointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = dto.ScheduledDate.Date,
                    TimeSlot = "10:00",
                    Status = AppointmentStatus.Pending
                }
            };

            _doctorRepositoryMock
                .Setup(repo => repo.IsDoctorAvailable(dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(true);

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(existingAppointments);

            var act = async () => await _service.CreateAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>();
        }

        [Fact]
        public async Task CreateAsync_WhenPatientHasSameDoctorSameDateDifferentSlot_ShouldThrowAppointmentRuleException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "11:00"
            };

            var existingAppointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = dto.ScheduledDate.Date,
                    TimeSlot = "10:00",
                    Status = AppointmentStatus.Pending
                }
            };

            _doctorRepositoryMock
                .Setup(repo => repo.IsDoctorAvailable(dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(true);

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(existingAppointments);

            var act = async () => await _service.CreateAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("You already have an active appointment with this doctor on the selected date.");
        }

        [Fact]
        public async Task CreateAsync_WhenPatientHasAppointmentWithDifferentDoctorSameDate_ShouldThrowAppointmentRuleException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "11:00"
            };

            var existingAppointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = dto.ScheduledDate.Date,
                    TimeSlot = "10:00",
                    Status = AppointmentStatus.Pending
                }
            };

            _doctorRepositoryMock
                .Setup(repo => repo.IsDoctorAvailable(dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(true);

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(existingAppointments);

            var act = async () => await _service.CreateAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("You already have an active appointment on this date. Please choose another date.");
        }

        [Fact]
        public async Task CreateAsync_ShouldIgnoreCancelledAppointmentsWhenCheckingConflicts()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00"
            };

            var existingAppointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = dto.ScheduledDate.Date,
                    TimeSlot = "10:00",
                    Status = AppointmentStatus.Cancelled
                }
            };

            var appointmentEntity = new Appointment();

            var response = new AppointmentResponseDto
            {
                AppointmentId = 100,
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                ScheduledDate = dto.ScheduledDate.Date,
                TimeSlot = "10:00",
                Status = AppointmentStatus.Pending
            };

            _doctorRepositoryMock
                .Setup(repo => repo.IsDoctorAvailable(dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(true);

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(existingAppointments);

            _mapperMock
                .Setup(mapper => mapper.Map<Appointment>(dto))
                .Returns(appointmentEntity);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentResponseDto>(appointmentEntity))
                .Returns(response);

            var result = await _service.CreateAsync(dto);

            result.Should().NotBeNull();
            result.Status.Should().Be(AppointmentStatus.Pending);

            _appointmentRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Appointment>()),
                Times.Once
            );
        }

        [Fact]
        public async Task CreateAsync_WithValidRequest_ShouldCreateAppointment()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00"
            };

            var appointmentEntity = new Appointment();

            var response = new AppointmentResponseDto
            {
                AppointmentId = 10,
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                ScheduledDate = dto.ScheduledDate.Date,
                TimeSlot = "10:00",
                Status = AppointmentStatus.Pending
            };

            _doctorRepositoryMock
                .Setup(repo => repo.IsDoctorAvailable(dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(true);

            _appointmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            _mapperMock
                .Setup(mapper => mapper.Map<Appointment>(dto))
                .Returns(appointmentEntity);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentResponseDto>(appointmentEntity))
                .Returns(response);

            var result = await _service.CreateAsync(dto);

            result.Should().NotBeNull();
            result.PatientId.Should().Be(dto.PatientId);
            result.DoctorId.Should().Be(dto.DoctorId);
            result.Status.Should().Be(AppointmentStatus.Pending);

            appointmentEntity.ScheduledDate.Should().Be(dto.ScheduledDate.Date);
            appointmentEntity.TimeSlot.Should().Be("10:00");
            appointmentEntity.Status.Should().Be(AppointmentStatus.Pending);
            appointmentEntity.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

            _appointmentRepositoryMock.Verify(
                repo => repo.AddAsync(appointmentEntity),
                Times.Once
            );
        }

        // ------------------------------------------------------------
        // DeleteAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task DeleteAsync_WhenAppointmentExists_ShouldDeleteAndReturnTrue()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(true);

            var result = await _service.DeleteAsync(1);

            result.Should().BeTrue();

            _appointmentRepositoryMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenAppointmentDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(false);

            var act = async () => await _service.DeleteAsync(1);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Appointment not found");

            _appointmentRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        // ------------------------------------------------------------
        // GetByDoctorAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task GetByDoctorAsync_WhenAppointmentsExist_ShouldReturnMappedAppointments()
        {
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    DoctorId = 5
                }
            };

            var mappedAppointments = new List<AppointmentResponseDto>
            {
                new AppointmentResponseDto
                {
                    AppointmentId = 1,
                    DoctorId = 5
                }
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByDoctor(5))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments))
                .Returns(mappedAppointments);

            var result = await _service.GetByDoctorAsync(5);

            result.Should().HaveCount(1);
            result.First().DoctorId.Should().Be(5);
        }

        [Fact]
        public async Task GetByDoctorAsync_WhenNoAppointments_ShouldThrowEntityNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByDoctor(5))
                .ReturnsAsync(new List<Appointment>());

            var act = async () => await _service.GetByDoctorAsync(5);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("No appointments found for this doctor");
        }

        [Fact]
        public async Task GetByDoctorAsync_WhenRepositoryReturnsNull_ShouldThrowEntityNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByDoctor(5))
                .ReturnsAsync((IEnumerable<Appointment>?)null);

            var act = async () => await _service.GetByDoctorAsync(5);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("No appointments found for this doctor");
        }

        // ------------------------------------------------------------
        // GetByPatientAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task GetByPatientAsync_WhenAppointmentsExist_ShouldReturnMappedAppointments()
        {
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 8
                }
            };

            var mappedAppointments = new List<AppointmentResponseDto>
            {
                new AppointmentResponseDto
                {
                    AppointmentId = 1,
                    PatientId = 8
                }
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetByPatient(8))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments))
                .Returns(mappedAppointments);

            var result = await _service.GetByPatientAsync(8);

            result.Should().HaveCount(1);
            result.First().PatientId.Should().Be(8);
        }

        [Fact]
        public async Task GetByPatientAsync_WhenNoAppointments_ShouldThrowEntityNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByPatient(8))
                .ReturnsAsync(new List<Appointment>());

            var act = async () => await _service.GetByPatientAsync(8);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("No appointments found for this patient");
        }

        [Fact]
        public async Task GetByPatientAsync_WhenRepositoryReturnsNull_ShouldThrowEntityNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByPatient(8))
                .ReturnsAsync((IEnumerable<Appointment>?)null);

            var act = async () => await _service.GetByPatientAsync(8);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("No appointments found for this patient");
        }

        // ------------------------------------------------------------
        // FilterAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task FilterAsync_WhenAppointmentsExist_ShouldReturnMappedAppointments()
        {
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Pending
                }
            };

            var mappedAppointments = new List<AppointmentResponseDto>
            {
                new AppointmentResponseDto
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Pending
                }
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.FilterAppointments(
                    AppointmentStatus.Pending,
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>()))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments))
                .Returns(mappedAppointments);

            var result = await _service.FilterAsync(
                AppointmentStatus.Pending,
                DateTime.Today,
                DateTime.Today.AddDays(5)
            );

            result.Should().HaveCount(1);
            result.First().Status.Should().Be(AppointmentStatus.Pending);
        }

        [Fact]
        public async Task FilterAsync_WhenNoAppointments_ShouldThrowEntityNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.FilterAppointments(
                    It.IsAny<AppointmentStatus?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>()))
                .ReturnsAsync(new List<Appointment>());

            var act = async () => await _service.FilterAsync(
                AppointmentStatus.Pending,
                DateTime.Today,
                DateTime.Today.AddDays(5)
            );

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("No appointments found for given criteria");
        }

        [Fact]
        public async Task FilterAsync_WhenRepositoryReturnsNull_ShouldThrowEntityNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.FilterAppointments(
                    It.IsAny<AppointmentStatus?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>()))
                .ReturnsAsync((IEnumerable<Appointment>?)null);

            var act = async () => await _service.FilterAsync(
                AppointmentStatus.Pending,
                DateTime.Today,
                DateTime.Today.AddDays(5)
            );

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("No appointments found for given criteria");
        }

        // ------------------------------------------------------------
        // CancelAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task CancelAsync_WhenAppointmentNotFound_ShouldThrowEntityNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            var act = async () => await _service.CancelAsync(1, "reason");

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Appointment not found");
        }

        [Fact]
        public async Task CancelAsync_WhenAppointmentAlreadyCancelled_ShouldThrowAppointmentRuleException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Cancelled
                });

            var act = async () => await _service.CancelAsync(1, "reason");

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Appointment already cancelled");
        }

        [Fact]
        public async Task CancelAsync_WhenAppointmentCompleted_ShouldThrowAppointmentRuleException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Completed
                });

            var act = async () => await _service.CancelAsync(1, "reason");

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Cannot cancel a completed appointment");
        }

        [Fact]
        public async Task CancelAsync_WhenAppointmentIsValid_ShouldCancelAndReturnTrue()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Pending
                });

            var result = await _service.CancelAsync(1, "Cancelled by doctor");

            result.Should().BeTrue();

            _appointmentRepositoryMock.Verify(
                repo => repo.CancelAppointment(1, "Cancelled by doctor"),
                Times.Once
            );
        }

        // ------------------------------------------------------------
        // ConfirmAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task ConfirmAsync_WhenAppointmentNotFound_ShouldThrowEntityNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            var act = async () => await _service.ConfirmAsync(1);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Appointment not found");
        }

        [Fact]
        public async Task ConfirmAsync_WhenAppointmentCompleted_ShouldThrowAppointmentRuleException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Completed
                });

            var act = async () => await _service.ConfirmAsync(1);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Cannot confirm a completed appointment");
        }

        [Fact]
        public async Task ConfirmAsync_WhenAppointmentCancelled_ShouldThrowAppointmentRuleException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Cancelled
                });

            var act = async () => await _service.ConfirmAsync(1);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Cannot confirm a cancelled appointment");
        }

        [Fact]
        public async Task ConfirmAsync_WhenAppointmentAlreadyConfirmed_ShouldThrowAppointmentRuleException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Confirmed
                });

            var act = async () => await _service.ConfirmAsync(1);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Appointment is already confirmed");
        }

        [Fact]
        public async Task ConfirmAsync_WhenAppointmentPending_ShouldConfirmAndReturnTrue()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Pending
                });

            var result = await _service.ConfirmAsync(1);

            result.Should().BeTrue();

            _appointmentRepositoryMock.Verify(
                repo => repo.ConfirmAppointment(1),
                Times.Once
            );
        }

        // ------------------------------------------------------------
        // CompleteAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task CompleteAsync_WhenAppointmentNotFound_ShouldThrowEntityNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            var act = async () => await _service.CompleteAsync(1);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Appointment not found");
        }

        [Fact]
        public async Task CompleteAsync_WhenAppointmentAlreadyCompleted_ShouldThrowAppointmentRuleException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Completed
                });

            var act = async () => await _service.CompleteAsync(1);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Appointment is already completed");
        }

        [Fact]
        public async Task CompleteAsync_WhenAppointmentCancelled_ShouldThrowAppointmentRuleException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Cancelled
                });

            var act = async () => await _service.CompleteAsync(1);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Cannot complete a cancelled appointment");
        }

        [Fact]
        public async Task CompleteAsync_WhenAppointmentPending_ShouldThrowAppointmentRuleException()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Pending
                });

            var act = async () => await _service.CompleteAsync(1);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Only confirmed appointments can be completed");
        }

        [Fact]
        public async Task CompleteAsync_WhenAppointmentConfirmed_ShouldCompleteAndReturnTrue()
        {
            _appointmentRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new Appointment
                {
                    AppointmentId = 1,
                    Status = AppointmentStatus.Confirmed
                });

            var result = await _service.CompleteAsync(1);

            result.Should().BeTrue();

            _appointmentRepositoryMock.Verify(
                repo => repo.CompleteAppointment(1),
                Times.Once
            );
        }

        // ------------------------------------------------------------
        // GetBookedSlotsAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task GetBookedSlotsAsync_WhenDoctorIdInvalid_ShouldThrowAppointmentRuleException()
        {
            var act = async () => await _service.GetBookedSlotsAsync(0, DateTime.Today);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Doctor ID is required.");
        }

        [Fact]
        public async Task GetBookedSlotsAsync_WhenDateIsDefault_ShouldThrowAppointmentRuleException()
        {
            var act = async () => await _service.GetBookedSlotsAsync(1, default);

            await act.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Appointment date is required.");
        }

        [Fact]
        public async Task GetBookedSlotsAsync_ShouldReturnNormalizedDistinctOrderedSlots()
        {
            var bookedSlots = new List<string>
            {
                "10:00",
                "10:00",
                "09:00 - 09:30",
                "11:00 AM",
                "",
                "   "
            };

            _appointmentRepositoryMock
                .Setup(repo => repo.GetBookedSlotsAsync(1, DateTime.Today))
                .ReturnsAsync(bookedSlots);

            var result = await _service.GetBookedSlotsAsync(1, DateTime.Today);

            result.Should().NotBeNull();
            result.Should().OnlyHaveUniqueItems();
            result.Should().NotContain(string.Empty);
            result.Should().Contain("09:00");
            result.Should().Contain("10:00");
        }

        // ------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------

        private static List<Appointment> GetSampleAppointments()
        {
            return new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 101,
                    DoctorId = 201,
                    ScheduledDate = DateTime.Today.AddDays(1),
                    TimeSlot = "10:00",
                    Status = AppointmentStatus.Pending
                },
                new Appointment
                {
                    AppointmentId = 2,
                    PatientId = 102,
                    DoctorId = 201,
                    ScheduledDate = DateTime.Today.AddDays(2),
                    TimeSlot = "11:00",
                    Status = AppointmentStatus.Confirmed
                },
                new Appointment
                {
                    AppointmentId = 3,
                    PatientId = 103,
                    DoctorId = 202,
                    ScheduledDate = DateTime.Today.AddDays(3),
                    TimeSlot = "12:00",
                    Status = AppointmentStatus.Cancelled
                },
                new Appointment
                {
                    AppointmentId = 4,
                    PatientId = 104,
                    DoctorId = 203,
                    ScheduledDate = DateTime.Today.AddDays(4),
                    TimeSlot = "13:00",
                    Status = AppointmentStatus.Completed
                }
            };
        }

        private void SetupPagedMapper()
        {
            _mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentResponseDto>>(It.IsAny<List<Appointment>>()))
                .Returns((List<Appointment> source) =>
                    source.Select(a => new AppointmentResponseDto
                    {
                        AppointmentId = a.AppointmentId,
                        PatientId = a.PatientId,
                        DoctorId = a.DoctorId,
                        ScheduledDate = a.ScheduledDate,
                        TimeSlot = a.TimeSlot?.ToString(),
                        Status = a.Status
                    }).ToList()
                );
        }
    }
}
