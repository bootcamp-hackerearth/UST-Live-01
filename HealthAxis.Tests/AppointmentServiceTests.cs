using AutoMapper;
using HealthAxis.API.Events;
using FluentAssertions;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementation;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.AppointmentDtos;
using HealthAxis.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HealthAxis.API.Messaging;
using Moq;
using System.Threading;

using ApiValidationException = HealthAxis.API.Exceptions.ValidationException;

namespace HealthAxis.API.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AppointmentService>> _loggerMock;
        private readonly Mock<IEventPublisher> _eventPublisherMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<AppointmentService>>();
            _eventPublisherMock = new Mock<IEventPublisher>();
            _notificationServiceMock = new Mock<INotificationService>();
            _eventPublisherMock.SetReturnsDefault(Task.CompletedTask);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentDto>(It.IsAny<Appointment>()))
                .Returns((Appointment appointment) => MapAppointmentDto(appointment));

            _service = CreateService();
        }

        [Fact]
        public async Task GetAllAsync_WhenNoAppointments_ReturnsEmptyList()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            SetupPatientDoctorLists();

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_WhenAppointmentsExist_ReturnsMappedAppointments()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>
                {
                    CreateAppointment(id: 1, patientId: 1, doctorId: 1),
                    CreateAppointment(id: 2, patientId: 2, doctorId: 2)
                });

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Patient>
                {
                    CreatePatient(id: 1, fullName: "Mona"),
                    CreatePatient(id: 2, fullName: "Riya")
                });

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>
                {
                    CreateDoctor(id: 1, fullName: "Dr John"),
                    CreateDoctor(id: 2, fullName: "Dr Smith")
                });

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result[0].PatientName.Should().Be("Mona");
            result[0].DoctorName.Should().Be("Dr John");
            result[1].PatientName.Should().Be("Riya");
            result[1].DoctorName.Should().Be("Dr Smith");
        }

        [Fact]
        public async Task GetAllAsync_WhenPatientAndDoctorMissing_UsesFallbackValues()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>
                {
                    CreateAppointment(id: 1, patientId: 99, doctorId: 88)
                });

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>());

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(1);
            result[0].PatientName.Should().Be("Not assigned");
            result[0].DoctorName.Should().Be("Not assigned");
        }

        [Fact]
        public async Task GetByIdAsync_WhenAppointmentDoesNotExist_ReturnsNull()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Appointment?)null);

            var result = await _service.GetByIdAsync(99);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenAppointmentExists_ReturnsAppointmentDto()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment());

            SetupPatientDoctorLists();

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.AppointmentId.Should().Be(1);
            result.PatientName.Should().Be("Mona");
            result.DoctorName.Should().Be("Dr John");
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientAndDoctorMissing_UsesFallbackValues()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(patientId: 99, doctorId: 99));

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>());

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.PatientName.Should().Be("Not assigned");
            result.DoctorName.Should().Be("Not assigned");
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenPatientDoesNotExist_ThrowsNotFoundException()
        {
            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            Func<Task> act = async () => await _service.GetByPatientIdAsync(1);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenPatientHasNoAppointments_ReturnsEmptyList()
        {
            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreatePatient());

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            SetupPatientDoctorLists();

            var result = await _service.GetByPatientIdAsync(1);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenAppointmentsExist_ReturnsOnlyPatientAppointmentsOrderedByDateDescending()
        {
            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreatePatient());

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>
                {
                    CreateAppointment(id: 1, patientId: 1, date: DateTime.Today.AddDays(1), timeSlot: "11:00 AM - 12:00 PM"),
                    CreateAppointment(id: 2, patientId: 1, date: DateTime.Today.AddDays(5), timeSlot: "10:00 AM - 11:00 AM"),
                    CreateAppointment(id: 3, patientId: 2, date: DateTime.Today.AddDays(10), timeSlot: "09:00 AM - 10:00 AM")
                });

            SetupPatientDoctorLists();

            var result = await _service.GetByPatientIdAsync(1);

            result.Should().HaveCount(2);
            result[0].AppointmentId.Should().Be(2);
            result[1].AppointmentId.Should().Be(1);
            result.All(appointment => appointment.PatientId == 1).Should().BeTrue();
        }

        [Fact]
        public async Task GetByDoctorIdAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            Func<Task> act = async () => await _service.GetByDoctorIdAsync(1);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetByDoctorIdAsync_WhenDoctorHasNoAppointments_ReturnsEmptyList()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateDoctor());

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            SetupPatientDoctorLists();

            var result = await _service.GetByDoctorIdAsync(1);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByDoctorIdAsync_WhenAppointmentsExist_ReturnsOnlyDoctorAppointmentsOrderedByDateAscending()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateDoctor());

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>
                {
                    CreateAppointment(id: 1, doctorId: 1, date: DateTime.Today.AddDays(5)),
                    CreateAppointment(id: 2, doctorId: 1, date: DateTime.Today.AddDays(1)),
                    CreateAppointment(id: 3, doctorId: 2, date: DateTime.Today.AddDays(2))
                });

            SetupPatientDoctorLists();

            var result = await _service.GetByDoctorIdAsync(1);

            result.Should().HaveCount(2);
            result[0].AppointmentId.Should().Be(2);
            result[1].AppointmentId.Should().Be(1);
            result.All(appointment => appointment.DoctorId == 1).Should().BeTrue();
        }

        [Fact]
        public async Task AddAsync_WhenDtoIsNull_ThrowsArgumentNullException()
        {
            Func<Task> act = async () => await _service.AddAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddAsync_WhenPatientIdInvalid_ThrowsValidationException_ForZero()
        {
            var dto = CreateCreateAppointmentDto();
            dto.PatientId = 0;

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task AddAsync_WhenPatientIdInvalid_ThrowsValidationException_ForNegative()
        {
            var dto = CreateCreateAppointmentDto();
            dto.PatientId = -1;

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task AddAsync_WhenDoctorIdInvalid_ThrowsValidationException_ForZero()
        {
            var dto = CreateCreateAppointmentDto();
            dto.DoctorId = 0;

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task AddAsync_WhenDoctorIdInvalid_ThrowsValidationException_ForNegative()
        {
            var dto = CreateCreateAppointmentDto();
            dto.DoctorId = -1;

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task AddAsync_WhenDateIsInPast_ThrowsValidationException()
        {
            var dto = CreateCreateAppointmentDto();
            dto.ScheduledDate = DateTime.Today.AddDays(-1);

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task AddAsync_WhenDateIsMoreThanSixMonthsAhead_ThrowsValidationException()
        {
            var dto = CreateCreateAppointmentDto();
            dto.ScheduledDate = DateTime.Today.AddMonths(6).AddDays(1);

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task AddAsync_WhenTimeSlotIsEmpty_ThrowsValidationException_ForEmptyValue()
        {
            var dto = CreateCreateAppointmentDto();
            dto.TimeSlot = string.Empty;

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task AddAsync_WhenTimeSlotIsEmpty_ThrowsValidationException_ForWhiteSpaceValue()
        {
            var dto = CreateCreateAppointmentDto();
            dto.TimeSlot = " ";

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task AddAsync_WhenTimeSlotIsInvalid_ThrowsValidationException()
        {
            var dto = CreateCreateAppointmentDto();
            dto.TimeSlot = "01:00 AM - 02:00 AM";

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task AddAsync_WhenTodayPastTimeSlot_ThrowsValidationException()
        {
            if (DateTime.Now.TimeOfDay <= new TimeSpan(9, 0, 0))
            {
                return;
            }

            var dto = CreateCreateAppointmentDto();
            dto.ScheduledDate = DateTime.Today;
            dto.TimeSlot = "09:00 AM - 10:00 AM";

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task AddAsync_WhenPatientDoesNotExist_ThrowsNotFoundException()
        {
            var dto = CreateCreateAppointmentDto();

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync((Patient?)null);

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task AddAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            var dto = CreateCreateAppointmentDto();

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(CreatePatient());

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync((Doctor?)null);

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task AddAsync_WhenDoctorIsInactive_ThrowsBusinessRuleException()
        {
            var dto = CreateCreateAppointmentDto();

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(CreatePatient());

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(CreateDoctor(isActive: false));

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task AddAsync_WhenPatientAlreadyBookedSameDoctorOnSameDateWithDifferentSlot_ThrowsAppointmentConflictException_ForPendingAppointment()
        {
            SetupValidAddDependencies();

            var dto = CreateCreateAppointmentDto();

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>
                {
                    CreateAppointment(
                        id: 10,
                        patientId: dto.PatientId,
                        doctorId: dto.DoctorId,
                        date: dto.ScheduledDate,
                        timeSlot: "11:00 AM - 12:00 PM",
                        status: AppointmentStatus.Pending)
                });

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentConflictException>()
                .WithMessage(
                    "You already have an appointment with this doctor on the selected date. Please choose another doctor or date.");
        }

        [Fact]
        public async Task AddAsync_WhenPatientAlreadyBookedSameDoctorOnSameDateWithDifferentSlot_ThrowsAppointmentConflictException_ForConfirmedAppointment()
        {
            SetupValidAddDependencies();

            var dto = CreateCreateAppointmentDto();

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>
                {
                    CreateAppointment(
                        id: 10,
                        patientId: dto.PatientId,
                        doctorId: dto.DoctorId,
                        date: dto.ScheduledDate,
                        timeSlot: "11:00 AM - 12:00 PM",
                        status: AppointmentStatus.Confirmed)
                });

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentConflictException>()
                .WithMessage(
                    "You already have an appointment with this doctor on the selected date. Please choose another doctor or date.");
        }

        [Fact]
        public async Task AddAsync_WhenPatientCompletedAppointmentWithSameDoctorOnSameDate_ThrowsAppointmentConflictException()
        {
            SetupValidAddDependencies();

            var dto = CreateCreateAppointmentDto();

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>
                {
                    CreateAppointment(
                        id: 10,
                        patientId: dto.PatientId,
                        doctorId: dto.DoctorId,
                        date: dto.ScheduledDate,
                        timeSlot: "09:00 AM - 10:00 AM",
                        status: AppointmentStatus.Completed)
                });

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentConflictException>()
                .WithMessage(
                    "You already have an appointment with this doctor on the selected date. Please choose another doctor or date.");
        }

        [Fact]
        public async Task AddAsync_WhenCancelledAppointmentExistsWithSameDoctorOnSameDate_AllowsBooking()
        {
            SetupSuccessfulAddDependencies(
                CreateAppointment(
                    id: 10,
                    patientId: 1,
                    doctorId: 1,
                    date: DateTime.Today.AddDays(1),
                    timeSlot: "11:00 AM - 12:00 PM",
                    status: AppointmentStatus.Cancelled));

            var result = await _service.AddAsync(
                CreateCreateAppointmentDto());

            result.Should().NotBeNull();
            result.AppointmentId.Should().Be(25);
        }

        [Fact]
        public async Task AddAsync_WhenDoctorAlreadyBookedWithActiveStatus_ThrowsAppointmentConflictException_ForPendingAppointment()
        {
            SetupValidAddDependencies();

            var dto = CreateCreateAppointmentDto();

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>
                {
                    CreateAppointment(
                        id: 10,
                        patientId: 2,
                        doctorId: dto.DoctorId,
                        date: dto.ScheduledDate,
                        timeSlot: dto.TimeSlot,
                        status: AppointmentStatus.Pending)
                });

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentConflictException>()
                .WithMessage(
                    "This doctor already has an appointment for the selected date and time slot. Please choose another slot.");
        }

        [Fact]
        public async Task AddAsync_WhenDoctorAlreadyBookedWithActiveStatus_ThrowsAppointmentConflictException_ForConfirmedAppointment()
        {
            SetupValidAddDependencies();

            var dto = CreateCreateAppointmentDto();

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>
                {
                    CreateAppointment(
                        id: 10,
                        patientId: 2,
                        doctorId: dto.DoctorId,
                        date: dto.ScheduledDate,
                        timeSlot: dto.TimeSlot,
                        status: AppointmentStatus.Confirmed)
                });

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentConflictException>()
                .WithMessage(
                    "This doctor already has an appointment for the selected date and time slot. Please choose another slot.");
        }

        [Fact]
        public async Task AddAsync_WhenPatientAlreadyBookedWithActiveStatus_ThrowsAppointmentConflictException_ForPendingAppointment()
        {
            SetupValidAddDependencies();

            var dto = CreateCreateAppointmentDto();

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>
                {
                    CreateAppointment(
                        id: 10,
                        patientId: dto.PatientId,
                        doctorId: 2,
                        date: dto.ScheduledDate,
                        timeSlot: dto.TimeSlot,
                        status: AppointmentStatus.Pending)
                });

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentConflictException>()
                .WithMessage(
                    "You already have an appointment at this date and time slot.");
        }

        [Fact]
        public async Task AddAsync_WhenPatientAlreadyBookedWithActiveStatus_ThrowsAppointmentConflictException_ForConfirmedAppointment()
        {
            SetupValidAddDependencies();

            var dto = CreateCreateAppointmentDto();

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>
                {
                    CreateAppointment(
                        id: 10,
                        patientId: dto.PatientId,
                        doctorId: 2,
                        date: dto.ScheduledDate,
                        timeSlot: dto.TimeSlot,
                        status: AppointmentStatus.Confirmed)
                });

            Func<Task> act = async () => await _service.AddAsync(dto);

            await act.Should()
                .ThrowAsync<AppointmentConflictException>()
                .WithMessage(
                    "You already have an appointment at this date and time slot.");
        }



        [Fact]
        public async Task UpdateStatusAsync_WhenDtoIsNull_ThrowsArgumentNullException()
        {
            Func<Task> act = async () => await _service.UpdateStatusAsync(1, null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenAppointmentDoesNotExist_ThrowsNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Confirmed
                });

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenStatusEnumInvalid_ThrowsValidationException()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(status: AppointmentStatus.Pending));

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = (AppointmentStatus)999
                });

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenCurrentStatusIsFinal_ThrowsBusinessRuleException_ForCompletedAppointment()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(status: AppointmentStatus.Completed));

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Confirmed
                });

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenCurrentStatusIsFinal_ThrowsBusinessRuleException_ForCancelledAppointment()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(status: AppointmentStatus.Cancelled));

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Confirmed
                });

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenPendingTransitionInvalid_ThrowsBusinessRuleException_WhenStatusIsStillPending()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(status: AppointmentStatus.Pending));

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Pending
                });

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenPendingTransitionInvalid_ThrowsBusinessRuleException_WhenChangingDirectlyToCompleted()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(status: AppointmentStatus.Pending));

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Completed
                });

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenConfirmedTransitionInvalid_ThrowsBusinessRuleException_WhenChangingBackToPending()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(status: AppointmentStatus.Confirmed));

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Pending
                });

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenConfirmedTransitionInvalid_ThrowsBusinessRuleException_WhenStatusIsStillConfirmed()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(status: AppointmentStatus.Confirmed));

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Confirmed
                });

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenCancelledWithReason_TrimsReason()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(status: AppointmentStatus.Pending));

            Appointment? capturedAppointment = null;

            _appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(1, It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
                .Callback<int, Appointment, CancellationToken>((_, appointment, _) => capturedAppointment = appointment)
                .ReturnsAsync((int _, Appointment appointment, CancellationToken _) => appointment);

            SetupPatientDoctorLists();

            await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Cancelled,
                    CancellationReason = " Patient is unavailable "
                });

            capturedAppointment.Should().NotBeNull();
            capturedAppointment!.CancellationReason.Should().Be("Patient is unavailable");
        }


        [Fact]
        public async Task UpdateStatusAsync_WhenStatusIsNotCancelled_ClearsCancellationReason()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    status: AppointmentStatus.Pending,
                    cancellationReason: "Old reason"));

            Appointment? capturedAppointment = null;

            _appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(1, It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
                .Callback<int, Appointment, CancellationToken>((_, appointment, _) => capturedAppointment = appointment)
                .ReturnsAsync((int _, Appointment appointment, CancellationToken _) => appointment);

            SetupPatientDoctorLists();

            await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Confirmed
                });

            capturedAppointment.Should().NotBeNull();
            capturedAppointment!.CancellationReason.Should().BeNull();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenRepositoryUpdateReturnsNull_ThrowsNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(status: AppointmentStatus.Pending));

            _appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(1, It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Confirmed
                });

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_WhenAppointmentDoesNotExist_ThrowsNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            Func<Task> act = async () => await _service.DeleteAsync(1);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_WhenAppointmentIsNotPending_ThrowsBusinessRuleException_ForConfirmedAppointment()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(status: AppointmentStatus.Confirmed));

            Func<Task> act = async () => await _service.DeleteAsync(1);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task DeleteAsync_WhenAppointmentIsNotPending_ThrowsBusinessRuleException_ForCompletedAppointment()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(status: AppointmentStatus.Completed));

            Func<Task> act = async () => await _service.DeleteAsync(1);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task DeleteAsync_WhenAppointmentIsNotPending_ThrowsBusinessRuleException_ForCancelledAppointment()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(status: AppointmentStatus.Cancelled));

            Func<Task> act = async () => await _service.DeleteAsync(1);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task DeleteAsync_WhenRepositoryReturnsNull_ReturnsNull()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(status: AppointmentStatus.Pending));

            _appointmentRepositoryMock
                .Setup(repository => repository.DeleteAsync(1))
                .ReturnsAsync((Appointment?)null);

            var result = await _service.DeleteAsync(1);

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_WhenPendingAppointmentDeleted_ReturnsDeletedAppointment()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(status: AppointmentStatus.Pending));

            _appointmentRepositoryMock
                .Setup(repository => repository.DeleteAsync(1))
                .ReturnsAsync(CreateAppointment(status: AppointmentStatus.Pending));

            SetupPatientDoctorLists();

            var result = await _service.DeleteAsync(1);

            result.Should().NotBeNull();
            result!.AppointmentId.Should().Be(1);
            result.Status.Should().Be(AppointmentStatus.Pending);
        }


        [Fact]
        public async Task AddAsync_WhenValid_SavesPublishesAndReturnsAppointment()
        {
            SetupSuccessfulAddDependencies();

            var result = await _service.AddAsync(
                CreateCreateAppointmentDto());

            result.AppointmentId.Should().Be(25);
            result.PatientName.Should().Be("Mona");
            result.DoctorName.Should().Be("Dr John");
            result.Status.Should().Be(AppointmentStatus.Pending);

            _appointmentRepositoryMock.Verify(
                repository => repository.AddAsync(
                    It.Is<Appointment>(appointment =>
                        appointment.PatientId == 1 &&
                        appointment.DoctorId == 1 &&
                        appointment.TimeSlot == "10:00 AM - 11:00 AM" &&
                        appointment.Status == AppointmentStatus.Pending),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task AddAsync_WhenTimeSlotHasOuterSpaces_NormalizesBeforeSaving()
        {
            SetupSuccessfulAddDependencies();
            var dto = CreateCreateAppointmentDto();
            dto.TimeSlot = " 10:00 AM - 11:00 AM ";

            var result = await _service.AddAsync(dto);

            result.TimeSlot.Should().Be("10:00 AM - 11:00 AM");
        }

        [Fact]
        public async Task AddAsync_WhenOnlyCancelledConflictExists_AllowsBooking()
        {
            SetupSuccessfulAddDependencies(
                CreateAppointment(
                    id: 10,
                    patientId: 2,
                    doctorId: 1,
                    status: AppointmentStatus.Cancelled));

            var result = await _service.AddAsync(
                CreateCreateAppointmentDto());

            result.Should().NotBeNull();
            result.AppointmentId.Should().Be(25);
        }

        [Fact]
        public async Task AddAsync_WhenOnlyCompletedPatientConflictExists_AllowsBooking()
        {
            SetupSuccessfulAddDependencies(
                CreateAppointment(
                    id: 10,
                    patientId: 1,
                    doctorId: 2,
                    status: AppointmentStatus.Completed));

            var result = await _service.AddAsync(
                CreateCreateAppointmentDto());

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task AddAsync_WhenRepositoryThrowsNonSqlDbUpdateException_PropagatesException()
        {
            SetupValidAddDependencies();
            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());
            _appointmentRepositoryMock
                .Setup(repository => repository.AddAsync(
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException("Database failure"));

            Func<Task> act = async () => await _service.AddAsync(
                CreateCreateAppointmentDto());

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [Fact]
        public async Task AddAsync_WhenInformationLoggingIsEnabled_CompletesSuccessfully()
        {
            _loggerMock
                .Setup(logger => logger.IsEnabled(LogLevel.Information))
                .Returns(true);
            SetupSuccessfulAddDependencies();
            var service = CreateService();

            var result = await service.AddAsync(
                CreateCreateAppointmentDto());

            result.AppointmentId.Should().Be(25);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenStatusIsSame_ThrowsBusinessRuleException()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    status: AppointmentStatus.Pending));

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Pending
                });

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenPendingAppointmentAlreadyStarted_CannotConfirm()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    date: DateTime.Today.AddDays(-1),
                    status: AppointmentStatus.Pending));

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Confirmed
                });

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenExistingTimeSlotIsMalformed_ThrowsValidationException()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    date: DateTime.Today.AddDays(-1),
                    timeSlot: "wrong-slot",
                    status: AppointmentStatus.Pending));

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Confirmed
                });

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenCancellationReasonMissing_ThrowsValidationException()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    status: AppointmentStatus.Pending));

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Cancelled,
                    CancellationReason = " "
                });

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenCancellationReasonIsTooLong_ThrowsValidationException()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    status: AppointmentStatus.Pending));

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Cancelled,
                    CancellationReason = new string('A', 5000)
                });

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenConfirmedCancellationIsWithinCutoff_ThrowsBusinessRuleException()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    date: DateTime.Today.AddDays(-1),
                    status: AppointmentStatus.Confirmed));

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Cancelled,
                    CancellationReason = "Emergency"
                });

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenConfirmedCancellationIsBeforeCutoff_CancelsAppointment()
        {
            SetupSuccessfulStatusUpdate(CreateAppointment(
                date: DateTime.Today.AddDays(2),
                status: AppointmentStatus.Confirmed));

            var result = await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Cancelled,
                    CancellationReason = "Patient request"
                });

            result.Status.Should().Be(AppointmentStatus.Cancelled);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenConfirmedAppointmentHasNotStarted_CannotComplete()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    date: DateTime.Today.AddDays(1),
                    status: AppointmentStatus.Confirmed));

            Func<Task> act = async () => await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Completed
                });

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenConfirmedAppointmentStarted_CompletesAppointment()
        {
            SetupSuccessfulStatusUpdate(CreateAppointment(
                date: DateTime.Today.AddDays(-1),
                status: AppointmentStatus.Confirmed));

            var result = await _service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Completed
                });

            result.Status.Should().Be(AppointmentStatus.Completed);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenNotificationServiceExists_StillReturnsUpdatedAppointment()
        {
            SetupSuccessfulStatusUpdate(CreateAppointment(
                status: AppointmentStatus.Pending));
            var service = CreateService(
                _notificationServiceMock.Object);

            var result = await service.UpdateStatusAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = AppointmentStatus.Confirmed
                });

            result.Status.Should().Be(AppointmentStatus.Confirmed);
        }


        private AppointmentService CreateService(
            INotificationService? notificationService = null)
        {
            return new AppointmentService(
                _appointmentRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _eventPublisherMock.Object,
                notificationService);
        }

        private void SetupSuccessfulAddDependencies(
            params Appointment[] existingAppointments)
        {
            SetupValidAddDependencies();
            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(existingAppointments.ToList());
            _appointmentRepositoryMock
                .Setup(repository => repository.AddAsync(
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment appointment, CancellationToken _) =>
                {
                    appointment.AppointmentId = 25;
                    return appointment;
                });
            SetupPatientDoctorLists();
        }

        private void SetupSuccessfulStatusUpdate(
            Appointment appointment)
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(appointment);
            _appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    1,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int _, Appointment updated, CancellationToken _) => updated);
            SetupPatientDoctorLists();
        }

        private void SetupValidAddDependencies()
        {
            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreatePatient());

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateDoctor());
        }

        private void SetupPatientDoctorLists()
        {
            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Patient>
                {
                    CreatePatient(id: 1, fullName: "Mona"),
                    CreatePatient(id: 2, fullName: "Riya")
                });

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>
                {
                    CreateDoctor(id: 1, fullName: "Dr John"),
                    CreateDoctor(id: 2, fullName: "Dr Smith")
                });
        }

        private static CreateAppointmentDto CreateCreateAppointmentDto()
        {
            return new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM - 11:00 AM"
            };
        }

        private static Appointment CreateAppointment(
            int id = 1,
            int patientId = 1,
            int doctorId = 1,
            DateTime? date = null,
            string timeSlot = "10:00 AM - 11:00 AM",
            AppointmentStatus status = AppointmentStatus.Pending,
            string? cancellationReason = null)
        {
            return new Appointment
            {
                AppointmentId = id,
                PatientId = patientId,
                DoctorId = doctorId,
                ScheduledDate = date ?? DateTime.Today.AddDays(1),
                TimeSlot = timeSlot,
                Status = status,
                CancellationReason = cancellationReason
            };
        }

        private static Patient CreatePatient(
            int id = 1,
            string fullName = "Mona")
        {
            return new Patient
            {
                PatientId = id,
                FullName = fullName,
                DateOfBirth = new DateTime(2004, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "9876543210",
                Email = "patient@gmail.com",
                UserId = "patient-user-1",
                CreatedDate = DateTime.Today
            };
        }

        private static Doctor CreateDoctor(
            int id = 1,
            string fullName = "Dr John",
            bool isActive = true)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = fullName,
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = isActive,
                UserId = "doctor-user-1"
            };
        }

        private static AppointmentDto MapAppointmentDto(Appointment appointment)
        {
            return new AppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = appointment.TimeSlot,
                Status = appointment.Status
            };
        }
    }
}