using AutoMapper;
using FluentAssertions;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services;
using HealthCareApp.Services.Impl;
using HealthCareApp.Services.Interface;
using HealthCareApp.Shared.Constants;
using HealthCareApp.Shared.Dtos.Appointments;
using HealthCareApp.Shared.Dtos.DoctorLeaves;
using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Enums;
using HealthCareApp.Shared.Events;
using MassTransit;
using Moq;

namespace HealthCareApp.Testing.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> appointmentRepositoryMock;
        private readonly Mock<IPatientRepository> patientRepositoryMock;
        private readonly Mock<IDoctorRepository> doctorRepositoryMock;
        private readonly Mock<IHealthRecordRepository> healthRecordRepositoryMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IBus> busMock;
        private readonly Mock<ICacheService> cacheServiceMock;
        private readonly Mock<IDoctorLeaveService> doctorLeaveServiceMock;

        private readonly AppointmentService appointmentService;

        public AppointmentServiceTests()
        {
            appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            patientRepositoryMock = new Mock<IPatientRepository>();
            doctorRepositoryMock = new Mock<IDoctorRepository>();
            healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();
            mapperMock = new Mock<IMapper>();
            busMock = new Mock<IBus>();
            cacheServiceMock = new Mock<ICacheService>();
            doctorLeaveServiceMock = new Mock<IDoctorLeaveService>();

            SetupMapper();

            appointmentService = new AppointmentService(
                appointmentRepositoryMock.Object,
                patientRepositoryMock.Object,
                doctorRepositoryMock.Object,
                healthRecordRepositoryMock.Object,
                mapperMock.Object,
                busMock.Object,
                cacheServiceMock.Object,
                doctorLeaveServiceMock.Object);
        }

        [Fact]
        public async Task GetDailyStatusSummaryAsync_ShouldReturnCorrectCounts()
        {
            var date = DateTime.Today;

            appointmentRepositoryMock
                .Setup(repository => repository.GetAppointmentsByDateAsync(
                    date,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments());

            var result = await appointmentService.GetDailyStatusSummaryAsync(date);

            result.Date.Should().Be(date.ToString("yyyy-MM-dd"));
            result.Total.Should().Be(5);
            result.Pending.Should().Be(2);
            result.Confirmed.Should().Be(1);
            result.Completed.Should().Be(1);
            result.Cancelled.Should().Be(1);
        }

        [Fact]
        public async Task GetAppointmentFilterOptionsAsync_ShouldReturnDistinctPatientsAndDoctors()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetAppointmentsForFilterOptionsAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments());

            var result = await appointmentService.GetAppointmentFilterOptionsAsync();

            result.Patients.Should().HaveCount(2);
            result.Doctors.Should().HaveCount(2);
            result.Patients.Select(patient => patient.Name).Should().Contain("Rishi Patient");
            result.Doctors.Select(doctor => doctor.Name).Should().Contain("Rishi Doctor");
        }

        [Fact]
        public async Task GetAllAppointmentsAsync_ShouldReturnMappedAppointments()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments());

            var result = await appointmentService.GetAllAppointmentsAsync();

            result.Should().HaveCount(5);
            result.First().AppointmentId.Should().Be(1);
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_WhenQueryIsNull_ShouldUseDefaults()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments());

            var result = await appointmentService.GetAllAppointmentsPagedAsync(null!);

            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalRecords.Should().Be(5);
            result.TotalPages.Should().Be(1);
            result.Items.Should().HaveCount(5);
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_WhenInvalidPageValues_ShouldNormalize()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments());

            var query = new AppointmentPaginationQueryDto
            {
                PageNumber = 0,
                PageSize = 0
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_WhenPageSizeGreaterThan100_ShouldCapPageSize()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments());

            var query = new AppointmentPaginationQueryDto
            {
                PageNumber = 1,
                PageSize = 500
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.PageSize.Should().Be(100);
        }

        [Theory]
        [InlineData("Rishi Patient")]
        [InlineData("Rishi Doctor")]
        [InlineData("09:00 AM")]
        [InlineData("not available")]
        public async Task GetAllAppointmentsPagedAsync_ShouldFilterBySearchTerm(string searchTerm)
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments());

            var query = new AppointmentPaginationQueryDto
            {
                SearchTerm = searchTerm
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.TotalRecords.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_ShouldFilterByPatientId()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments());

            var query = new AppointmentPaginationQueryDto
            {
                PatientId = 1
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.Items.Should().OnlyContain(appointment => appointment.PatientId == 1);
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_ShouldFilterByDoctorId()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments());

            var query = new AppointmentPaginationQueryDto
            {
                DoctorId = 1
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.Items.Should().OnlyContain(appointment => appointment.DoctorId == 1);
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_ShouldFilterByStatus()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments());

            var query = new AppointmentPaginationQueryDto
            {
                Status = AppointmentStatus.Pending
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.Items.Should().OnlyContain(appointment =>
                appointment.Status == AppointmentStatus.Pending);
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_ShouldFilterByScheduledDate()
        {
            var date = DateTime.Today.AddDays(1);

            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments());

            var query = new AppointmentPaginationQueryDto
            {
                ScheduledDate = date
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.Items.Should().OnlyContain(appointment =>
                appointment.ScheduledDate == date.ToString("yyyy-MM-dd"));
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_ShouldFilterByUpcomingOnly()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments());

            var query = new AppointmentPaginationQueryDto
            {
                UpcomingOnly = true
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.Items.Should().OnlyContain(appointment =>
                DateTime.Parse(appointment.ScheduledDate).Date >= DateTime.Today &&
                appointment.Status != AppointmentStatus.Cancelled &&
                appointment.Status != AppointmentStatus.Completed);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WhenIdInvalid_ShouldThrowAppointmentRuleException()
        {
            Func<Task> action = async () =>
                await appointmentService.GetAppointmentByIdAsync(0);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Please provide a valid appointment reference.");
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WhenNotFound_ShouldThrowEntityNotFoundException()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    99,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await appointmentService.GetAppointmentByIdAsync(99);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WhenExists_ShouldReturnAppointment()
        {
            var appointment = GetAppointments().First();

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var result = await appointmentService.GetAppointmentByIdAsync(appointment.AppointmentId);

            result.AppointmentId.Should().Be(appointment.AppointmentId);
        }

        [Fact]
        public async Task GetAppointmentsByPatientIdAsync_WhenPatientExists_ShouldReturnAppointments()
        {
            SetupPatientExists(1);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.PatientId == 1)
                    .ToList());

            var result = await appointmentService.GetAppointmentsByPatientIdAsync(1);

            result.Should().OnlyContain(appointment => appointment.PatientId == 1);
        }

        [Fact]
        public async Task GetAppointmentsByPatientIdAsync_WhenPatientInvalid_ShouldThrow()
        {
            Func<Task> action = async () =>
                await appointmentService.GetAppointmentsByPatientIdAsync(0);

            await action.Should().ThrowAsync<AppointmentRuleException>();
        }

        [Fact]
        public async Task GetAppointmentsByPatientIdAsync_WhenPatientNotFound_ShouldThrow()
        {
            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    99,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () =>
                await appointmentService.GetAppointmentsByPatientIdAsync(99);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetAppointmentsByDoctorIdAsync_WhenDoctorExists_ShouldReturnAppointments()
        {
            SetupDoctorExists(1, true);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByDoctorIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.DoctorId == 1)
                    .ToList());

            var result = await appointmentService.GetAppointmentsByDoctorIdAsync(1);

            result.Should().OnlyContain(appointment => appointment.DoctorId == 1);
        }

        [Fact]
        public async Task GetAppointmentsByDoctorIdAsync_WhenDoctorInvalid_ShouldThrow()
        {
            Func<Task> action = async () =>
                await appointmentService.GetAppointmentsByDoctorIdAsync(0);

            await action.Should().ThrowAsync<AppointmentRuleException>();
        }

        [Fact]
        public async Task GetAppointmentsByDoctorIdAsync_WhenDoctorNotFound_ShouldThrow()
        {
            doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    99,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await appointmentService.GetAppointmentsByDoctorIdAsync(99);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetAppointmentsByStatusAsync_ShouldReturnAppointments()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetByStatusAsync(
                    AppointmentStatus.Pending,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.Status == AppointmentStatus.Pending)
                    .ToList());

            var result = await appointmentService.GetAppointmentsByStatusAsync(AppointmentStatus.Pending);

            result.Should().OnlyContain(appointment => appointment.Status == AppointmentStatus.Pending);
        }

        [Fact]
        public async Task GetUpcomingAppointmentsAsync_ShouldReturnMappedAppointments()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetUpcomingAppointmentsAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.Status == AppointmentStatus.Pending)
                    .ToList());

            var result = await appointmentService.GetUpcomingAppointmentsAsync();

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetUpcomingAppointmentsByPatientIdAsync_ShouldReturnMappedAppointments()
        {
            SetupPatientExists(1);

            appointmentRepositoryMock
                .Setup(repository => repository.GetUpcomingAppointmentsByPatientIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.PatientId == 1)
                    .ToList());

            var result = await appointmentService.GetUpcomingAppointmentsByPatientIdAsync(1);

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetUpcomingAppointmentsByDoctorIdAsync_ShouldReturnMappedAppointments()
        {
            SetupDoctorExists(1, true);

            appointmentRepositoryMock
                .Setup(repository => repository.GetUpcomingAppointmentsByDoctorIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.DoctorId == 1)
                    .ToList());

            var result = await appointmentService.GetUpcomingAppointmentsByDoctorIdAsync(1);

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetPendingAppointmentsByPatientIdAsync_ShouldReturnMappedAppointments()
        {
            SetupPatientExists(1);

            appointmentRepositoryMock
                .Setup(repository => repository.GetPendingAppointmentsByPatientIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment =>
                        appointment.PatientId == 1 &&
                        appointment.Status == AppointmentStatus.Pending)
                    .ToList());

            var result = await appointmentService.GetPendingAppointmentsByPatientIdAsync(1);

            result.Should().OnlyContain(appointment =>
                appointment.PatientId == 1 &&
                appointment.Status == AppointmentStatus.Pending);
        }

        [Fact]
        public async Task GetPendingAppointmentsByDoctorIdAsync_ShouldReturnMappedAppointments()
        {
            SetupDoctorExists(1, true);

            appointmentRepositoryMock
                .Setup(repository => repository.GetPendingAppointmentsByDoctorIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment =>
                        appointment.DoctorId == 1 &&
                        appointment.Status == AppointmentStatus.Pending)
                    .ToList());

            var result = await appointmentService.GetPendingAppointmentsByDoctorIdAsync(1);

            result.Should().OnlyContain(appointment =>
                appointment.DoctorId == 1 &&
                appointment.Status == AppointmentStatus.Pending);
        }

        [Fact]
        public async Task GetTodayConfirmedAppointmentsByDoctorIdAsync_ShouldReturnMappedAppointments()
        {
            SetupDoctorExists(1, true);

            var patients = GetPatients();
            var doctors = GetDoctors();

            var appointments = new List<Appointment>
    {
        new Appointment
        {
            AppointmentId = 20,
            PatientId = 1,
            Patient = patients[0],
            DoctorId = 1,
            Doctor = doctors[0],
            ScheduledDate = DateTime.Today,
            TimeSlot = TimeSlots.Slots[0],
            Status = AppointmentStatus.Confirmed
        }
    };

            appointmentRepositoryMock
                .Setup(repository => repository.GetTodayConfirmedAppointmentsByDoctorIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            var result = await appointmentService.GetTodayConfirmedAppointmentsByDoctorIdAsync(1);

            result.Should().HaveCount(1);
            result[0].Status.Should().Be(AppointmentStatus.Confirmed);
        }


        [Fact]
        public async Task BookAppointmentAsync_WhenDtoNull_ShouldThrow()
        {
            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(null!);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Appointment details are required.");
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenDoctorInactive_ShouldThrow()
        {
            var dto = GetValidBookAppointmentDto();

            SetupPatientExists(dto.PatientId);
            SetupDoctorExists(dto.DoctorId, false);

            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Doctor is inactive. Appointment cannot be booked.");
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenDateIsPast_ShouldThrow()
        {
            var dto = GetValidBookAppointmentDto();
            dto.ScheduledDate = DateTime.Today.AddDays(-1);

            SetupPatientExists(dto.PatientId);
            SetupDoctorExists(dto.DoctorId, true);

            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Appointment date cannot be in the past.");
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenTimeSlotEmpty_ShouldThrow()
        {
            var dto = GetValidBookAppointmentDto();
            dto.TimeSlot = "";

            SetupPatientExists(dto.PatientId);
            SetupDoctorExists(dto.DoctorId, true);

            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Time slot is required.");
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenTimeSlotInvalid_ShouldThrow()
        {
            var dto = GetValidBookAppointmentDto();
            dto.TimeSlot = "Invalid Slot";

            SetupPatientExists(dto.PatientId);
            SetupDoctorExists(dto.DoctorId, true);

            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Invalid time slot selected.");
        }
        [Fact]
        public async Task BookAppointmentAsync_WhenSlotBooked_ShouldThrowConflictException()
        {
            var dto = GetValidBookAppointmentDto();

            SetupPatientExists(dto.PatientId);

            SetupDoctorExists(dto.DoctorId, true);

            doctorLeaveServiceMock
                .Setup(service => service.GetDoctorLeaveStatusAsync(
                    dto.DoctorId,
                    DateOnly.FromDateTime(dto.ScheduledDate.Date)))
                .ReturnsAsync(new DoctorLeaveStatusDto
                {
                    IsDoctorOnLeave = false,
                    Message = string.Empty
                });

            appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("This time slot is already booked for the selected doctor.");
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenPatientHasSameSlot_ShouldThrowConflictException()
        {
            var dto = GetValidBookAppointmentDto();

            SetupPatientExists(dto.PatientId);

            SetupDoctorExists(dto.DoctorId, true);

            SetupSlotNotBooked(dto);

            doctorLeaveServiceMock
                .Setup(service => service.GetDoctorLeaveStatusAsync(
                    dto.DoctorId,
                    DateOnly.FromDateTime(dto.ScheduledDate.Date)))
                .ReturnsAsync(new DoctorLeaveStatusDto
                {
                    IsDoctorOnLeave = false,
                    Message = string.Empty
                });

            appointmentRepositoryMock
                .Setup(repository => repository.PatientHasActiveAppointmentOnDateAndSlotAsync(
                    dto.PatientId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Patient already has an active appointment in this time slot.");
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenPatientHasAppointmentWithDoctor_ShouldThrowConflictException()
        {
            var dto = GetValidBookAppointmentDto();

            SetupPatientExists(dto.PatientId);

            SetupDoctorExists(dto.DoctorId, true);

            SetupSlotNotBooked(dto);

            SetupPatientNoSameSlot(dto);

            doctorLeaveServiceMock
                .Setup(service => service.GetDoctorLeaveStatusAsync(
                    dto.DoctorId,
                    DateOnly.FromDateTime(dto.ScheduledDate.Date)))
                .ReturnsAsync(new DoctorLeaveStatusDto
                {
                    IsDoctorOnLeave = false,
                    Message = string.Empty
                });

            appointmentRepositoryMock
                .Setup(repository => repository.PatientHasActiveAppointmentWithDoctorOnDateAsync(
                    dto.PatientId,
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Patient already has an active appointment with this doctor on the selected date.");
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenValid_ShouldCreateAppointment()
        {
            var dto = GetValidBookAppointmentDto();

            SetupSuccessfulBooking(dto);

            doctorLeaveServiceMock
                .Setup(service => service.GetDoctorLeaveStatusAsync(
                    dto.DoctorId,
                    DateOnly.FromDateTime(dto.ScheduledDate.Date)))
                .ReturnsAsync(new DoctorLeaveStatusDto
                {
                    IsDoctorOnLeave = false,
                    Message = string.Empty
                });

            var result = await appointmentService.BookAppointmentAsync(dto);

            result.AppointmentId.Should().Be(100);
            result.Status.Should().Be(AppointmentStatus.Pending);

            appointmentRepositoryMock.Verify(
                repository => repository.CreateAsync(
                    It.Is<Appointment>(appointment =>
                        appointment.PatientId == dto.PatientId &&
                        appointment.DoctorId == dto.DoctorId &&
                        appointment.TimeSlot == dto.TimeSlot &&
                        appointment.Status == AppointmentStatus.Pending &&
                        appointment.CancellationReason == null),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenAppointmentIdInvalid_ShouldThrow()
        {
            var dto = GetValidUpdateAppointmentDto();

            Func<Task> action = async () =>
                await appointmentService.UpdateAppointmentAsync(0, dto);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Please provide a valid appointment reference.");
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenDtoNull_ShouldThrow()
        {
            Func<Task> action = async () =>
                await appointmentService.UpdateAppointmentAsync(1, null!);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Appointment details are required.");
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenAppointmentDoesNotExist_ShouldThrow()
        {
            var dto = GetValidUpdateAppointmentDto();

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    99,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await appointmentService.UpdateAppointmentAsync(99, dto);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenPatientDoesNotExist_ShouldThrow()
        {
            var dto = GetValidUpdateAppointmentDto();
            var appointment = GetAppointments().First();

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    dto.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () =>
                await appointmentService.UpdateAppointmentAsync(appointment.AppointmentId, dto);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenDoctorDoesNotExist_ShouldThrow()
        {
            var dto = GetValidUpdateAppointmentDto();
            var appointment = GetAppointments().First();

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            SetupPatientExists(dto.PatientId);

            doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    dto.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await appointmentService.UpdateAppointmentAsync(appointment.AppointmentId, dto);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenDoctorInactive_ShouldThrow()
        {
            var dto = GetValidUpdateAppointmentDto();
            var appointment = GetAppointments().First();

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            SetupPatientExists(dto.PatientId);
            SetupDoctorExists(dto.DoctorId, false);

            Func<Task> action = async () =>
                await appointmentService.UpdateAppointmentAsync(appointment.AppointmentId, dto);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Doctor is inactive. Appointment cannot be booked.");
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenDateIsPast_ShouldThrow()
        {
            var dto = GetValidUpdateAppointmentDto();
            dto.ScheduledDate = DateTime.Today.AddDays(-1);

            var appointment = GetAppointments().First();

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            SetupPatientExists(dto.PatientId);
            SetupDoctorExists(dto.DoctorId, true);

            Func<Task> action = async () =>
                await appointmentService.UpdateAppointmentAsync(appointment.AppointmentId, dto);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Appointment date cannot be in the past.");
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenTimeSlotInvalid_ShouldThrow()
        {
            var dto = GetValidUpdateAppointmentDto();
            dto.TimeSlot = "Wrong Slot";

            var appointment = GetAppointments().First();

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            SetupPatientExists(dto.PatientId);
            SetupDoctorExists(dto.DoctorId, true);

            Func<Task> action = async () =>
                await appointmentService.UpdateAppointmentAsync(appointment.AppointmentId, dto);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Invalid time slot selected.");
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenNewSlotAlreadyTaken_ShouldThrowConflict()
        {
            var dto = GetValidUpdateAppointmentDto();
            dto.TimeSlot = TimeSlots.Slots[1];

            var existingAppointment = GetAppointments().First();
            existingAppointment.TimeSlot = TimeSlots.Slots[0];

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    existingAppointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingAppointment);

            SetupPatientExists(dto.PatientId);
            SetupDoctorExists(dto.DoctorId, true);

            appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Func<Task> action = async () =>
                await appointmentService.UpdateAppointmentAsync(existingAppointment.AppointmentId, dto);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("This time slot is already booked for the selected doctor.");
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenSameExistingSlot_ShouldUpdateSuccessfully()
        {
            var existingAppointment = GetAppointments().First();

            var dto = new UpdateAppointmentDto
            {
                PatientId = existingAppointment.PatientId,
                DoctorId = existingAppointment.DoctorId,
                ScheduledDate = existingAppointment.ScheduledDate,
                TimeSlot = existingAppointment.TimeSlot
            };

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    existingAppointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingAppointment);

            SetupPatientExists(dto.PatientId);
            SetupDoctorExists(dto.DoctorId, true);

            appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    existingAppointment.AppointmentId,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, Appointment appointment, CancellationToken ct) =>
                {
                    appointment.AppointmentId = id;
                    return appointment;
                });

            var result = await appointmentService.UpdateAppointmentAsync(
                existingAppointment.AppointmentId,
                dto);

            result.AppointmentId.Should().Be(existingAppointment.AppointmentId);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenRepositoryReturnsNull_ShouldThrowEntityNotFound()
        {
            var dto = GetValidUpdateAppointmentDto();
            var appointment = GetAppointments().First();

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            SetupPatientExists(dto.PatientId);
            SetupDoctorExists(dto.DoctorId, true);

            appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    appointment.AppointmentId,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await appointmentService.UpdateAppointmentAsync(appointment.AppointmentId, dto);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenIdInvalid_ShouldThrow()
        {
            Func<Task> action = async () =>
                await appointmentService.ConfirmAppointmentAsync(0);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Please provide a valid appointment reference.");
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenAppointmentNotFound_ShouldThrow()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    99,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await appointmentService.ConfirmAppointmentAsync(99);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenCancelled_ShouldThrow()
        {
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Cancelled);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            Func<Task> action = async () =>
                await appointmentService.ConfirmAppointmentAsync(appointment.AppointmentId);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Cancelled appointment cannot be confirmed.");
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenCompleted_ShouldThrow()
        {
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Completed);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            Func<Task> action = async () =>
                await appointmentService.ConfirmAppointmentAsync(appointment.AppointmentId);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Completed appointment cannot be confirmed again.");
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenAlreadyConfirmed_ShouldThrow()
        {
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Confirmed);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            Func<Task> action = async () =>
                await appointmentService.ConfirmAppointmentAsync(appointment.AppointmentId);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Appointment is already confirmed.");
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenValid_ShouldUpdateStatus()
        {
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Pending);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    appointment.AppointmentId,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, Appointment updatedAppointment, CancellationToken ct) => updatedAppointment);

            var result = await appointmentService.ConfirmAppointmentAsync(appointment.AppointmentId);

            result.Status.Should().Be(AppointmentStatus.Confirmed);
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenRepositoryReturnsNull_ShouldThrow()
        {
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Pending);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    appointment.AppointmentId,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await appointmentService.ConfirmAppointmentAsync(appointment.AppointmentId);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task CompleteAppointmentAsync_WhenIdInvalid_ShouldThrow()
        {
            Func<Task> action = async () =>
                await appointmentService.CompleteAppointmentAsync(0);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Please provide a valid appointment reference.");
        }

        [Fact]
        public async Task CompleteAppointmentAsync_WhenAppointmentNotFound_ShouldThrow()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    99,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await appointmentService.CompleteAppointmentAsync(99);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task CompleteAppointmentAsync_WhenCancelled_ShouldThrow()
        {
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Cancelled);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            Func<Task> action = async () =>
                await appointmentService.CompleteAppointmentAsync(appointment.AppointmentId);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Cancelled appointment cannot be completed.");
        }

        [Fact]
        public async Task CompleteAppointmentAsync_WhenAlreadyCompleted_ShouldThrow()
        {
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Completed);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            Func<Task> action = async () =>
                await appointmentService.CompleteAppointmentAsync(appointment.AppointmentId);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Appointment is already completed.");
        }

        [Fact]
        public async Task CompleteAppointmentAsync_WhenStatusNotConfirmed_ShouldThrow()
        {
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Pending);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            Func<Task> action = async () =>
                await appointmentService.CompleteAppointmentAsync(appointment.AppointmentId);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Only confirmed appointments can be completed.");
        }

        [Fact]
        public async Task CompleteAppointmentAsync_WhenValid_ShouldUpdateStatus()
        {
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Confirmed);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    appointment.AppointmentId,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, Appointment updatedAppointment, CancellationToken ct) => updatedAppointment);

            var result = await appointmentService.CompleteAppointmentAsync(appointment.AppointmentId);

            result.Status.Should().Be(AppointmentStatus.Completed);
        }

        [Fact]
        public async Task CompleteAppointmentAsync_WhenRepositoryReturnsNull_ShouldThrow()
        {
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Confirmed);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    appointment.AppointmentId,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await appointmentService.CompleteAppointmentAsync(appointment.AppointmentId);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenDtoNull_ShouldThrow()
        {
            Func<Task> action = async () =>
                await appointmentService.CancelAppointmentAsync(null!);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Cancellation details are required.");
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenAppointmentIdInvalid_ShouldThrow()
        {
            var dto = new CancelAppointmentDto
            {
                AppointmentId = 0,
                Reason = "Cancel"
            };

            Func<Task> action = async () =>
                await appointmentService.CancelAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Please provide a valid appointment reference.");
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenReasonEmpty_ShouldThrow()
        {
            var dto = new CancelAppointmentDto
            {
                AppointmentId = 1,
                Reason = ""
            };

            Func<Task> action = async () =>
                await appointmentService.CancelAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Cancellation reason is required.");
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenReasonTooLong_ShouldThrow()
        {
            var dto = new CancelAppointmentDto
            {
                AppointmentId = 1,
                Reason = new string('A', 201)
            };

            Func<Task> action = async () =>
                await appointmentService.CancelAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Cancellation reason cannot exceed 200 characters.");
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenAppointmentNotFound_ShouldThrow()
        {
            var dto = new CancelAppointmentDto
            {
                AppointmentId = 99,
                Reason = "Cancel"
            };

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    99,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await appointmentService.CancelAppointmentAsync(dto);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenCompleted_ShouldThrow()
        {
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Completed);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var dto = new CancelAppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                Reason = "Not available"
            };

            Func<Task> action = async () =>
                await appointmentService.CancelAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Completed appointment cannot be cancelled.");
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenAlreadyCancelled_ShouldThrow()
        {
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Cancelled);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var dto = new CancelAppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                Reason = "Not available"
            };

            Func<Task> action = async () =>
                await appointmentService.CancelAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Appointment is already cancelled.");
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenValid_ShouldCancelAppointment()
        {
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Pending);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    appointment.AppointmentId,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, Appointment updatedAppointment, CancellationToken ct) => updatedAppointment);

            var dto = new CancelAppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                Reason = "Not available"
            };

            var result = await appointmentService.CancelAppointmentAsync(dto);

            result.Status.Should().Be(AppointmentStatus.Cancelled);
            result.CancellationReason.Should().Be("Not available");
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenRepositoryReturnsNull_ShouldThrow()
        {
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Pending);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    appointment.AppointmentId,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            var dto = new CancelAppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                Reason = "Cancel"
            };

            Func<Task> action = async () =>
                await appointmentService.CancelAppointmentAsync(dto);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task DeleteAppointmentAsync_WhenIdInvalid_ShouldThrow()
        {
            Func<Task> action = async () =>
                await appointmentService.DeleteAppointmentAsync(0);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Please provide a valid appointment reference.");
        }

        [Fact]
        public async Task DeleteAppointmentAsync_WhenAppointmentNotFound_ShouldThrow()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    99,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await appointmentService.DeleteAppointmentAsync(99);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task DeleteAppointmentAsync_WhenHasHealthRecord_ShouldThrowConflictException()
        {
            var appointment = GetAppointments().First();

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            healthRecordRepositoryMock
                .Setup(repository => repository.ExistsByAppointmentIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Func<Task> action = async () =>
                await appointmentService.DeleteAppointmentAsync(appointment.AppointmentId);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("This appointment cannot be deleted because it has an associated health record.");
        }

        [Fact]
        public async Task DeleteAppointmentAsync_WhenValid_ShouldDelete()
        {
            var appointment = GetAppointments().First();

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            healthRecordRepositoryMock
                .Setup(repository => repository.ExistsByAppointmentIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            appointmentRepositoryMock
                .Setup(repository => repository.DeleteAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var result = await appointmentService.DeleteAppointmentAsync(appointment.AppointmentId);

            result.AppointmentId.Should().Be(appointment.AppointmentId);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_WhenRepositoryReturnsNull_ShouldThrow()
        {
            var appointment = GetAppointments().First();

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            healthRecordRepositoryMock
                .Setup(repository => repository.ExistsByAppointmentIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            appointmentRepositoryMock
                .Setup(repository => repository.DeleteAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await appointmentService.DeleteAppointmentAsync(appointment.AppointmentId);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetMyAppointmentsForPatientAsync_ShouldReturnLoggedInPatientAppointments()
        {
            var patient = GetPatients().First();

            SetupLoggedInPatient(patient);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(
                    patient.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.PatientId == patient.PatientId)
                    .ToList());

            var result = await appointmentService.GetMyAppointmentsForPatientAsync("patient-identity");

            result.Should().OnlyContain(appointment => appointment.PatientId == patient.PatientId);
        }

        [Fact]
        public async Task GetMyAppointmentsForPatientPagedAsync_WhenQueryNull_ShouldReturnPagedAppointments()
        {
            var patient = GetPatients().First();

            SetupLoggedInPatient(patient);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(
                    patient.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.PatientId == patient.PatientId)
                    .ToList());

            var result = await appointmentService.GetMyAppointmentsForPatientPagedAsync(
                "patient-identity",
                null!);

            result.PageNumber.Should().Be(1);
            result.Items.Should().OnlyContain(appointment => appointment.PatientId == patient.PatientId);
        }

        [Fact]
        public async Task GetMyAppointmentsForPatientPagedAsync_ShouldApplyFilters()
        {
            var patient = GetPatients().First();

            SetupLoggedInPatient(patient);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(
                    patient.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.PatientId == patient.PatientId)
                    .ToList());

            var query = new AppointmentPaginationQueryDto
            {
                PageNumber = 1,
                PageSize = 1,
                SearchTerm = "Rishi Doctor",
                Status = AppointmentStatus.Pending,
                ScheduledDate = DateTime.Today.AddDays(1),
                UpcomingOnly = true
            };

            var result = await appointmentService.GetMyAppointmentsForPatientPagedAsync(
                "patient-identity",
                query);

            result.PageSize.Should().Be(1);
            result.Items.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetMyAppointmentsForDoctorAsync_ShouldReturnLoggedInDoctorAppointments()
        {
            var doctor = GetDoctors().First();

            SetupLoggedInDoctor(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByDoctorIdAsync(
                    doctor.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.DoctorId == doctor.DoctorId)
                    .ToList());

            var result = await appointmentService.GetMyAppointmentsForDoctorAsync("doctor-identity");

            result.Should().OnlyContain(appointment => appointment.DoctorId == doctor.DoctorId);
        }

        [Fact]
        public async Task GetMyAppointmentsForDoctorPagedAsync_WhenQueryNull_ShouldReturnPagedAppointments()
        {
            var doctor = GetDoctors().First();

            SetupLoggedInDoctor(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByDoctorIdAsync(
                    doctor.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.DoctorId == doctor.DoctorId)
                    .ToList());

            var result = await appointmentService.GetMyAppointmentsForDoctorPagedAsync(
                "doctor-identity",
                null!);

            result.PageNumber.Should().Be(1);
            result.Items.Should().OnlyContain(appointment => appointment.DoctorId == doctor.DoctorId);
        }

        [Fact]
        public async Task GetMyUpcomingAppointmentsForPatientAsync_ShouldReturnMappedAppointments()
        {
            var patient = GetPatients().First();

            SetupLoggedInPatient(patient);

            appointmentRepositoryMock
                .Setup(repository => repository.GetUpcomingAppointmentsByPatientIdAsync(
                    patient.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.PatientId == patient.PatientId)
                    .ToList());

            var result = await appointmentService.GetMyUpcomingAppointmentsForPatientAsync("patient-identity");

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetMyPendingAppointmentsForPatientAsync_ShouldReturnMappedAppointments()
        {
            var patient = GetPatients().First();

            SetupLoggedInPatient(patient);

            appointmentRepositoryMock
                .Setup(repository => repository.GetPendingAppointmentsByPatientIdAsync(
                    patient.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment =>
                        appointment.PatientId == patient.PatientId &&
                        appointment.Status == AppointmentStatus.Pending)
                    .ToList());

            var result = await appointmentService.GetMyPendingAppointmentsForPatientAsync("patient-identity");

            result.Should().OnlyContain(appointment => appointment.Status == AppointmentStatus.Pending);
        }

        [Fact]
        public async Task GetMyUpcomingAppointmentsForDoctorAsync_ShouldReturnMappedAppointments()
        {
            var doctor = GetDoctors().First();

            SetupLoggedInDoctor(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetUpcomingAppointmentsByDoctorIdAsync(
                    doctor.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.DoctorId == doctor.DoctorId)
                    .ToList());

            var result = await appointmentService.GetMyUpcomingAppointmentsForDoctorAsync("doctor-identity");

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetMyPendingAppointmentsForDoctorAsync_ShouldReturnMappedAppointments()
        {
            var doctor = GetDoctors().First();

            SetupLoggedInDoctor(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetPendingAppointmentsByDoctorIdAsync(
                    doctor.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment =>
                        appointment.DoctorId == doctor.DoctorId &&
                        appointment.Status == AppointmentStatus.Pending)
                    .ToList());

            var result = await appointmentService.GetMyPendingAppointmentsForDoctorAsync("doctor-identity");

            result.Should().OnlyContain(appointment => appointment.Status == AppointmentStatus.Pending);
        }

        [Fact]
        public async Task GetMyTodayConfirmedAppointmentsForDoctorAsync_ShouldReturnMappedAppointments()
        {
            var doctors = GetDoctors();
            var patients = GetPatients();

            var doctor = doctors[0];

            SetupLoggedInDoctor(doctor);

            var appointments = new List<Appointment>
    {
        new Appointment
        {
            AppointmentId = 700,
            PatientId = 1,
            Patient = patients[0],
            DoctorId = doctor.DoctorId,
            Doctor = doctor,
            ScheduledDate = DateTime.Today,
            TimeSlot = TimeSlots.Slots[0],
            Status = AppointmentStatus.Confirmed
        }
    };

            appointmentRepositoryMock
                .Setup(repository => repository.GetTodayConfirmedAppointmentsByDoctorIdAsync(
                    doctor.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            var result = await appointmentService.GetMyTodayConfirmedAppointmentsForDoctorAsync("doctor-identity");

            result.Should().HaveCount(1);
            result[0].Status.Should().Be(AppointmentStatus.Confirmed);
        }

        [Fact]
        public async Task GetAppointmentByIdForPatientAsync_WhenOwner_ShouldReturnAppointment()
        {
            var patient = GetPatients().First();
            var appointment = GetAppointments().First();

            SetupLoggedInPatient(patient);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var result = await appointmentService.GetAppointmentByIdForPatientAsync(
                appointment.AppointmentId,
                "patient-identity");

            result.AppointmentId.Should().Be(appointment.AppointmentId);
        }

        [Fact]
        public async Task GetAppointmentByIdForPatientAsync_WhenNotOwner_ShouldThrowForbiddenAccessException()
        {
            var patient = GetPatients().First();
            var appointment = GetAppointments().First();
            appointment.PatientId = 999;

            SetupLoggedInPatient(patient);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            Func<Task> action = async () =>
                await appointmentService.GetAppointmentByIdForPatientAsync(
                    appointment.AppointmentId,
                    "patient-identity");

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Patients can access only their own appointments.");
        }

        [Fact]
        public async Task BookAppointmentForPatientAsync_WhenDtoNull_ShouldThrow()
        {
            Func<Task> action = async () =>
                await appointmentService.BookAppointmentForPatientAsync(null!, "patient-identity");

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Appointment details are required.");
        }
        [Fact]
        public async Task BookAppointmentForPatientAsync_ShouldIgnoreBodyPatientIdAndUseLoggedInPatient()
        {
            var dto = GetValidBookAppointmentDto();

            dto.PatientId = 999;

            var patients = GetPatients();

            var patient = patients[0];

            SetupLoggedInPatient(patient);

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    patient.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            SetupDoctorExists(dto.DoctorId, true);

            SetupSlotNotBookedForPatient(patient.PatientId, dto);

            doctorLeaveServiceMock
                .Setup(service => service.GetDoctorLeaveStatusAsync(
                    dto.DoctorId,
                    DateOnly.FromDateTime(dto.ScheduledDate.Date)))
                .ReturnsAsync(new DoctorLeaveStatusDto
                {
                    IsDoctorOnLeave = false,
                    Message = string.Empty
                });

            appointmentRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment appointment, CancellationToken ct) =>
                {
                    appointment.AppointmentId = 500;
                    return appointment;
                });

            cacheServiceMock
                .Setup(service => service.RemoveAsync(
                    It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            busMock
                .Setup(bus => bus.Publish(
                    It.IsAny<AppointmentBookedEvent>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await appointmentService.BookAppointmentForPatientAsync(
                dto,
                "patient-identity");

            result.PatientId.Should().Be(patient.PatientId);

            dto.PatientId.Should().Be(patient.PatientId);
        }

        [Fact]
        public async Task CancelAppointmentForPatientAsync_WhenDtoNull_ShouldThrow()
        {
            Func<Task> action = async () =>
                await appointmentService.CancelAppointmentForPatientAsync(null!, "patient-identity");

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Cancellation details are required.");
        }

        [Fact]
        public async Task CancelAppointmentForPatientAsync_WhenOwner_ShouldCancel()
        {
            var patient = GetPatients().First();
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Pending);
            appointment.PatientId = patient.PatientId;

            SetupLoggedInPatient(patient);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    appointment.AppointmentId,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, Appointment updatedAppointment, CancellationToken ct) => updatedAppointment);

            var dto = new CancelAppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                Reason = "Cancel"
            };

            var result = await appointmentService.CancelAppointmentForPatientAsync(dto, "patient-identity");

            result.Status.Should().Be(AppointmentStatus.Cancelled);
        }

        [Fact]
        public async Task CancelAppointmentForPatientAsync_WhenNotOwner_ShouldThrowForbiddenAccessException()
        {
            var patient = GetPatients().First();
            var appointment = GetAppointments().First();
            appointment.PatientId = 999;

            SetupLoggedInPatient(patient);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var dto = new CancelAppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                Reason = "Cancel"
            };

            Func<Task> action = async () =>
                await appointmentService.CancelAppointmentForPatientAsync(dto, "patient-identity");

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Patients can cancel only their own appointments.");
        }

        [Fact]
        public async Task GetAppointmentByIdForDoctorAsync_WhenOwner_ShouldReturnAppointment()
        {
            var doctor = GetDoctors().First();
            var appointment = GetAppointments().First();

            SetupLoggedInDoctor(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var result = await appointmentService.GetAppointmentByIdForDoctorAsync(
                appointment.AppointmentId,
                "doctor-identity");

            result.AppointmentId.Should().Be(appointment.AppointmentId);
        }

        [Fact]
        public async Task GetAppointmentByIdForDoctorAsync_WhenNotOwner_ShouldThrowForbiddenAccessException()
        {
            var doctor = GetDoctors().First();
            var appointment = GetAppointments().First();
            appointment.DoctorId = 999;

            SetupLoggedInDoctor(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            Func<Task> action = async () =>
                await appointmentService.GetAppointmentByIdForDoctorAsync(
                    appointment.AppointmentId,
                    "doctor-identity");

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Doctors can access only their own appointments.");
        }

        [Fact]
        public async Task ConfirmAppointmentForDoctorAsync_WhenOwner_ShouldConfirm()
        {
            var doctor = GetDoctors().First();
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Pending);
            appointment.DoctorId = doctor.DoctorId;

            SetupLoggedInDoctor(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    appointment.AppointmentId,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, Appointment updatedAppointment, CancellationToken ct) => updatedAppointment);

            var result = await appointmentService.ConfirmAppointmentForDoctorAsync(
                appointment.AppointmentId,
                "doctor-identity");

            result.Status.Should().Be(AppointmentStatus.Confirmed);
        }

        [Fact]
        public async Task ConfirmAppointmentForDoctorAsync_WhenNotOwner_ShouldThrowForbiddenAccessException()
        {
            var doctor = GetDoctors().First();
            var appointment = GetAppointments().First();
            appointment.DoctorId = 999;

            SetupLoggedInDoctor(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            Func<Task> action = async () =>
                await appointmentService.ConfirmAppointmentForDoctorAsync(
                    appointment.AppointmentId,
                    "doctor-identity");

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Doctors can confirm only their own appointments.");
        }

        [Fact]
        public async Task CompleteAppointmentForDoctorAsync_WhenOwner_ShouldComplete()
        {
            var doctor = GetDoctors().First();
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Confirmed);
            appointment.DoctorId = doctor.DoctorId;

            SetupLoggedInDoctor(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    appointment.AppointmentId,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, Appointment updatedAppointment, CancellationToken ct) => updatedAppointment);

            var result = await appointmentService.CompleteAppointmentForDoctorAsync(
                appointment.AppointmentId,
                "doctor-identity");

            result.Status.Should().Be(AppointmentStatus.Completed);
        }

        [Fact]
        public async Task CompleteAppointmentForDoctorAsync_WhenNotOwner_ShouldThrowForbiddenAccessException()
        {
            var doctor = GetDoctors().First();
            var appointment = GetAppointments().First();
            appointment.DoctorId = 999;

            SetupLoggedInDoctor(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            Func<Task> action = async () =>
                await appointmentService.CompleteAppointmentForDoctorAsync(
                    appointment.AppointmentId,
                    "doctor-identity");

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Doctors can complete only their own appointments.");
        }

        [Fact]
        public async Task CancelAppointmentForDoctorAsync_WhenDtoNull_ShouldThrow()
        {
            Func<Task> action = async () =>
                await appointmentService.CancelAppointmentForDoctorAsync(null!, "doctor-identity");

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Cancellation details are required.");
        }

        [Fact]
        public async Task CancelAppointmentForDoctorAsync_WhenOwner_ShouldCancel()
        {
            var doctor = GetDoctors().First();
            var appointment = GetAppointmentWithStatus(AppointmentStatus.Pending);
            appointment.DoctorId = doctor.DoctorId;

            SetupLoggedInDoctor(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    appointment.AppointmentId,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, Appointment updatedAppointment, CancellationToken ct) => updatedAppointment);

            var dto = new CancelAppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                Reason = "Cancel"
            };

            var result = await appointmentService.CancelAppointmentForDoctorAsync(dto, "doctor-identity");

            result.Status.Should().Be(AppointmentStatus.Cancelled);
        }

        [Fact]
        public async Task CancelAppointmentForDoctorAsync_WhenNotOwner_ShouldThrowForbiddenAccessException()
        {
            var doctor = GetDoctors().First();
            var appointment = GetAppointments().First();
            appointment.DoctorId = 999;

            SetupLoggedInDoctor(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var dto = new CancelAppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                Reason = "Cancel"
            };

            Func<Task> action = async () =>
                await appointmentService.CancelAppointmentForDoctorAsync(dto, "doctor-identity");

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Doctors can cancel only their own appointments.");
        }

        [Fact]
        public async Task GetLoggedInPatient_WhenIdentityUserIdEmpty_ShouldThrow()
        {
            Func<Task> action = async () =>
                await appointmentService.GetMyAppointmentsForPatientAsync("");

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Invalid logged-in user.");
        }

        [Fact]
        public async Task GetLoggedInPatient_WhenPatientNotFound_ShouldThrow()
        {
            patientRepositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync(
                    "missing-patient",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () =>
                await appointmentService.GetMyAppointmentsForPatientAsync("missing-patient");

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetLoggedInDoctor_WhenIdentityUserIdEmpty_ShouldThrow()
        {
            Func<Task> action = async () =>
                await appointmentService.GetMyAppointmentsForDoctorAsync("");

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Invalid logged-in user.");
        }

        [Fact]
        public async Task GetLoggedInDoctor_WhenDoctorNotFound_ShouldThrow()
        {
            doctorRepositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync(
                    "missing-doctor",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await appointmentService.GetMyAppointmentsForDoctorAsync("missing-doctor");

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        private void SetupMapper()
        {
            mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentDto>>(It.IsAny<object>()))
                .Returns((object source) =>
                {
                    var appointments = ((IEnumerable<Appointment>)source).ToList();
                    return appointments.Select(MapToAppointmentDto).ToList();
                });

            mapperMock
                .Setup(mapper => mapper.Map<AppointmentDto>(It.IsAny<object>()))
                .Returns((object source) => MapToAppointmentDto((Appointment)source));

            mapperMock
                .Setup(mapper => mapper.Map<Appointment>(It.IsAny<object>()))
                .Returns((object source) =>
                {
                    return source switch
                    {
                        BookAppointmentDto dto => new Appointment
                        {
                            PatientId = dto.PatientId,
                            DoctorId = dto.DoctorId,
                            ScheduledDate = dto.ScheduledDate,
                            TimeSlot = dto.TimeSlot
                        },

                        UpdateAppointmentDto dto => new Appointment
                        {
                            PatientId = dto.PatientId,
                            DoctorId = dto.DoctorId,
                            ScheduledDate = dto.ScheduledDate,
                            TimeSlot = dto.TimeSlot
                        },

                        _ => new Appointment()
                    };
                });

            mapperMock
                .Setup(mapper => mapper.Map(
                    It.IsAny<UpdateAppointmentDto>(),
                    It.IsAny<Appointment>()))
                .Returns((UpdateAppointmentDto dto, Appointment appointment) =>
                {
                    appointment.PatientId = dto.PatientId;
                    appointment.DoctorId = dto.DoctorId;
                    appointment.ScheduledDate = dto.ScheduledDate;
                    appointment.TimeSlot = dto.TimeSlot;

                    return appointment;
                });
        }

        private void SetupPatientExists(int patientId)
        {
            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    patientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetPatients().First(patient => patient.PatientId == patientId));
        }

        private void SetupDoctorExists(int doctorId, bool isActive)
        {
            var doctor = GetDoctors().First(existingDoctor => existingDoctor.DoctorId == doctorId);
            doctor.IsActive = isActive;

            doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);
        }

        private void SetupLoggedInPatient(Patient patient)
        {
            patientRepositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync(
                    patient.IdentityUserId!,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);
        }

        private void SetupLoggedInDoctor(Doctor doctor)
        {
            doctorRepositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync(
                    doctor.IdentityUserId!,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);
        }

        private void SetupSlotNotBooked(BookAppointmentDto dto)
        {
            appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
        }

        private void SetupPatientNoSameSlot(BookAppointmentDto dto)
        {
            appointmentRepositoryMock
                .Setup(repository => repository.PatientHasActiveAppointmentOnDateAndSlotAsync(
                    dto.PatientId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
        }

        private void SetupPatientNoDoctorConflict(BookAppointmentDto dto)
        {
            appointmentRepositoryMock
                .Setup(repository => repository.PatientHasActiveAppointmentWithDoctorOnDateAsync(
                    dto.PatientId,
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
        }

        private void SetupSlotNotBookedForPatient(int patientId, BookAppointmentDto dto)
        {
            appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            appointmentRepositoryMock
                .Setup(repository => repository.PatientHasActiveAppointmentOnDateAndSlotAsync(
                    patientId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            appointmentRepositoryMock
                .Setup(repository => repository.PatientHasActiveAppointmentWithDoctorOnDateAsync(
                    patientId,
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
        }

        private void SetupSuccessfulBooking(BookAppointmentDto dto)
        {
            SetupPatientExists(dto.PatientId);
            SetupDoctorExists(dto.DoctorId, true);
            SetupSlotNotBooked(dto);
            SetupPatientNoSameSlot(dto);
            SetupPatientNoDoctorConflict(dto);

            appointmentRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment appointment, CancellationToken cancellationToken) =>
                {
                    appointment.AppointmentId = 100;
                    return appointment;
                });
        }

        private static Appointment GetAppointmentWithStatus(AppointmentStatus status)
        {
            var appointment = GetAppointments().First();
            appointment.Status = status;
            appointment.CancellationReason = null;

            return appointment;
        }

        private static BookAppointmentDto GetValidBookAppointmentDto()
        {
            return new BookAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = TimeSlots.Slots[0]
            };
        }

        private static UpdateAppointmentDto GetValidUpdateAppointmentDto()
        {
            return new UpdateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(2),
                TimeSlot = TimeSlots.Slots[1]
            };
        }

        private static AppointmentDto MapToAppointmentDto(Appointment appointment)
        {
            return new AppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient?.PatientName ?? "Patient",
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor?.DoctorName ?? "Doctor",
                ScheduledDate = appointment.ScheduledDate.ToString("yyyy-MM-dd"),
                TimeSlot = appointment.TimeSlot,
                Status = appointment.Status,
                CancellationReason = appointment.CancellationReason
            };
        }

        private static List<Patient> GetPatients()
        {
            return new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    PatientName = "Rishi Patient",
                    Email = "rishi.patient@example.com",
                    PhoneNumber = "9876543210",
                    IdentityUserId = "patient-identity"
                },

                new Patient
                {
                    PatientId = 2,
                    PatientName = "Meera Patient",
                    Email = "meera.patient@example.com",
                    PhoneNumber = "9876543211",
                    IdentityUserId = "patient-identity-2"
                }
            };
        }

        private static List<Doctor> GetDoctors()
        {
            return new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 1,
                    DoctorName = "Rishi Doctor",
                    Email = "rishi.doctor@example.com",
                    Specialisation = SpecialisationType.GeneralPractitioner,
                    YearsOfExperience = 5,
                    ConsultationFee = 500,
                    IsActive = true,
                    IdentityUserId = "doctor-identity"
                },

                new Doctor
                {
                    DoctorId = 2,
                    DoctorName = "Meera Doctor",
                    Email = "meera.doctor@example.com",
                    Specialisation = SpecialisationType.Cardiologist,
                    YearsOfExperience = 8,
                    ConsultationFee = 900,
                    IsActive = true,
                    IdentityUserId = "doctor-identity-2"
                }
            };
        }

        private static List<Appointment> GetAppointments()
        {
            var patients = GetPatients();
            var doctors = GetDoctors();

            return new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    Patient = patients[0],
                    DoctorId = 1,
                    Doctor = doctors[0],
                    ScheduledDate = DateTime.Today.AddDays(1),
                    TimeSlot = "09:00 AM - 09:30 AM",
                    Status = AppointmentStatus.Pending
                },

                new Appointment
                {
                    AppointmentId = 2,
                    PatientId = 1,
                    Patient = patients[0],
                    DoctorId = 2,
                    Doctor = doctors[1],
                    ScheduledDate = DateTime.Today.AddDays(2),
                    TimeSlot = "09:30 AM - 10:00 AM",
                    Status = AppointmentStatus.Confirmed
                },

                new Appointment
                {
                    AppointmentId = 3,
                    PatientId = 2,
                    Patient = patients[1],
                    DoctorId = 1,
                    Doctor = doctors[0],
                    ScheduledDate = DateTime.Today.AddDays(3),
                    TimeSlot = "10:00 AM - 10:30 AM",
                    Status = AppointmentStatus.Completed
                },

                new Appointment
                {
                    AppointmentId = 4,
                    PatientId = 2,
                    Patient = patients[1],
                    DoctorId = 2,
                    Doctor = doctors[1],
                    ScheduledDate = DateTime.Today.AddDays(4),
                    TimeSlot = "10:30 AM - 11:00 AM",
                    Status = AppointmentStatus.Cancelled,
                    CancellationReason = "not available"
                },

                new Appointment
                {
                    AppointmentId = 5,
                    PatientId = 1,
                    Patient = patients[0],
                    DoctorId = 1,
                    Doctor = doctors[0],
                    ScheduledDate = DateTime.Today.AddDays(5),
                    TimeSlot = "11:00 AM - 11:30 AM",
                    Status = AppointmentStatus.Pending
                }
            };
        }
    }
}