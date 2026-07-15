using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Implementation;
using Moq;
using System.Security.Claims;
using Xunit;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using HealthAxisCore_Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly Mock<IDistributedCache> _distributedCacheMock;
        private readonly AppDbContext _dbContext;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();

            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _distributedCacheMock = new Mock<IDistributedCache>();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(options);

            _service = new AppointmentService(
                _appointmentRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _mapperMock.Object,
                _publishEndpointMock.Object,
                _distributedCacheMock.Object,
                _dbContext);
        }

        [Fact]
        public async Task GetAppointmentsAsync_WhenUserIsPatient_UsesPatientIdFromClaims()
        {
            var user = CreateUser("Patient", patientId: 10);

            var appointments = new List<Appointment>
            {
                CreateAppointment(patientId: 10)
            };

            var expectedDtos = new List<AppointmentDto>
            {
                CreateAppointmentDto(patientId: 10)
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAppointmentsAsync(
                    10,
                    null,
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentDto>>(appointments))
                .Returns(expectedDtos);

            var result = await _service.GetAppointmentsAsync(
                patientId: 99,
                doctorId: null,
                date: null,
                user: user);

            Assert.Single(result);
            Assert.Equal(10, result[0].PatientId);
        }

        [Fact]
        public async Task GetAppointmentsAsync_WhenPatientClaimMissing_ThrowsUnauthorizedException()
        {
            var user = CreateUser("Patient");

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.GetAppointmentsAsync(null, null, null, user));

            Assert.Equal("PatientId claim missing", exception.Message);
        }

        [Fact]
        public async Task GetAppointmentsAsync_WhenUserIsDoctor_UsesDoctorIdFromClaims()
        {
            var user = CreateUser("Doctor", doctorId: 20);

            var appointments = new List<Appointment>
            {
                CreateAppointment(doctorId: 20)
            };

            var expectedDtos = new List<AppointmentDto>
            {
                CreateAppointmentDto(doctorId: 20)
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAppointmentsAsync(
                    null,
                    20,
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentDto>>(appointments))
                .Returns(expectedDtos);

            var result = await _service.GetAppointmentsAsync(
                patientId: null,
                doctorId: 99,
                date: null,
                user: user);

            Assert.Single(result);
            Assert.Equal(20, result[0].DoctorId);
        }

        [Fact]
        public async Task GetAppointmentsAsync_WhenDoctorClaimMissing_ThrowsUnauthorizedException()
        {
            var user = CreateUser("Doctor");

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.GetAppointmentsAsync(null, null, null, user));

            Assert.Equal("DoctorId claim missing", exception.Message);
        }

        [Fact]
        public async Task GetAppointmentsAsync_WhenUserIsAdmin_UsesPassedFilters()
        {
            var user = CreateUser("Admin");

            var date = DateTime.UtcNow.Date.AddDays(2);

            var appointments = new List<Appointment>
            {
                CreateAppointment(patientId: 10, doctorId: 20, scheduledDate: date)
            };

            var expectedDtos = new List<AppointmentDto>
            {
                CreateAppointmentDto(patientId: 10, doctorId: 20, scheduledDate: date)
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAppointmentsAsync(
                    10,
                    20,
                    date,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentDto>>(appointments))
                .Returns(expectedDtos);

            var result = await _service.GetAppointmentsAsync(
                patientId: 10,
                doctorId: 20,
                date: date,
                user: user);

            Assert.Single(result);
            Assert.Equal(10, result[0].PatientId);
            Assert.Equal(20, result[0].DoctorId);
        }

        [Fact]
        public async Task CreateAsync_WhenPatientClaimMissing_ThrowsUnauthorizedException()
        {
            var user = CreateUser("Patient");

            var request = CreateAppointmentRequest();

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.CreateAsync(request, user));

            Assert.Equal("PatientId claim missing", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenPatientAlreadyBookedAtSameTime_ThrowsInvalidException()
        {
            var user = CreateUser("Patient", patientId: 10);

            var request = CreateAppointmentRequest();

            _appointmentRepositoryMock
                .Setup(repository => repository.PatientHasAppointmentAtSlotAsync(
                    10,
                    request.ScheduledDate,
                    request.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => _service.CreateAsync(request, user));

            Assert.Equal(
                "You already have an appointment booked at this date and time.",
                exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenDoctorNotFound_ThrowsNotFoundException()
        {
            var user = CreateUser("Patient", patientId: 10);

            var request = CreateAppointmentRequest();

            _appointmentRepositoryMock
                .Setup(repository => repository.PatientHasAppointmentAtSlotAsync(
                    10,
                    request.ScheduledDate,
                    request.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    request.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _service.CreateAsync(request, user));

            Assert.Equal("Doctor not found", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenDoctorIsInactive_ThrowsInvalidException()
        {
            var user = CreateUser("Patient", patientId: 10);

            var request = CreateAppointmentRequest();

            _appointmentRepositoryMock
                .Setup(repository => repository.PatientHasAppointmentAtSlotAsync(
                    10,
                    request.ScheduledDate,
                    request.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    request.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateDoctor(isActive: false));

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => _service.CreateAsync(request, user));

            Assert.Equal("Cannot book appointment with inactive doctor", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenScheduledDateIsPast_ThrowsInvalidException()
        {
            var user = CreateUser("Patient", patientId: 10);

            var request = CreateAppointmentRequest(
                scheduledDate: DateTime.UtcNow.Date.AddDays(-1));

            _appointmentRepositoryMock
                .Setup(repository => repository.PatientHasAppointmentAtSlotAsync(
                    10,
                    request.ScheduledDate,
                    request.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    request.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateDoctor());

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => _service.CreateAsync(request, user));

            Assert.Equal("Cannot book past date", exception.Message);
        }

        [Theory]
        [InlineData("08:00")]
        [InlineData("17:00")]
        [InlineData("18:00")]
        public async Task CreateAsync_WhenSlotOutsideWorkingHours_ThrowsInvalidException(
            string timeSlot)
        {
            var user = CreateUser("Patient", patientId: 10);

            var request = CreateAppointmentRequest(
                timeSlot: timeSlot);

            _appointmentRepositoryMock
                .Setup(repository => repository.PatientHasAppointmentAtSlotAsync(
                    10,
                    request.ScheduledDate,
                    request.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    request.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateDoctor());

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => _service.CreateAsync(request, user));

            Assert.Equal(
                "Doctor working hours are from 09:00 to 17:00",
                exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenTimeSlotFormatInvalid_ThrowsFormatException()
        {
            var user = CreateUser("Patient", patientId: 10);

            var request = CreateAppointmentRequest(
                timeSlot: "invalid-slot");

            _appointmentRepositoryMock
                .Setup(repository => repository.PatientHasAppointmentAtSlotAsync(
                    10,
                    request.ScheduledDate,
                    request.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    request.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateDoctor());

            var exception = await Assert.ThrowsAsync<FormatException>(
                () => _service.CreateAsync(request, user));

            Assert.Equal("Invalid time slot format", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenDoctorAlreadyBooked_ThrowsInvalidException()
        {
            var user = CreateUser("Patient", patientId: 10);

            var request = CreateAppointmentRequest();

            SetupCreateBaseValidUntilDoctorAvailability(
                userPatientId: 10,
                request: request);

            _appointmentRepositoryMock
                .Setup(repository => repository.DoctorHasAppointmentAtSlotAsync(
                    request.DoctorId,
                    request.ScheduledDate,
                    request.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => _service.CreateAsync(request, user));

            Assert.Equal(
                "Doctor already has an appointment at this time slot",
                exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenSlotNotAvailable_ThrowsInvalidException()
        {
            var user = CreateUser("Patient", patientId: 10);

            var request = CreateAppointmentRequest(timeSlot: "10:00");

            SetupCreateBaseValidUntilDoctorAvailability(
                userPatientId: 10,
                request: request);

            _appointmentRepositoryMock
                .Setup(repository => repository.DoctorHasAppointmentAtSlotAsync(
                    request.DoctorId,
                    request.ScheduledDate,
                    request.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _doctorRepositoryMock
                .Setup(repository => repository.GetAvailableSlotsAsync(
                    request.DoctorId,
                    request.ScheduledDate,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<string> { "09:00" });

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => _service.CreateAsync(request, user));

            Assert.Equal("Slot not available", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenValidAndDetailsExist_ReturnsMappedAppointmentDto()
        {
            var user = CreateUser("Patient", patientId: 10);

            var request = CreateAppointmentRequest();

            SetupCreateBaseValidUntilDoctorAvailability(
                userPatientId: 10,
                request: request);

            _appointmentRepositoryMock
                .Setup(repository => repository.DoctorHasAppointmentAtSlotAsync(
                    request.DoctorId,
                    request.ScheduledDate,
                    request.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _doctorRepositoryMock
                .Setup(repository => repository.GetAvailableSlotsAsync(
                    request.DoctorId,
                    request.ScheduledDate,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<string> { request.TimeSlot });

            var mappedAppointment = new Appointment
            {
                DoctorId = request.DoctorId,
                Doctor = CreateDoctor(request.DoctorId),
                PatientId = 0,
                Patient = CreatePatient(),
                ScheduledDate = request.ScheduledDate,
                TimeSlot = request.TimeSlot,
                Status = string.Empty,
                CancellationReason = string.Empty
            };

            var savedAppointment = CreateAppointment(
                appointmentId: 100,
                patientId: 10,
                doctorId: request.DoctorId,
                scheduledDate: request.ScheduledDate,
                timeSlot: request.TimeSlot,
                status: "Pending");

            var detailedAppointment = CreateAppointment(
                appointmentId: 100,
                patientId: 10,
                doctorId: request.DoctorId,
                scheduledDate: request.ScheduledDate,
                timeSlot: request.TimeSlot,
                status: "Pending");

            var expectedDto = CreateAppointmentDto(
                appointmentId: 100,
                patientId: 10,
                doctorId: request.DoctorId,
                scheduledDate: request.ScheduledDate,
                timeSlot: request.TimeSlot,
                status: "Pending");

            _mapperMock
                .Setup(mapper => mapper.Map<Appointment>(request))
                .Returns(mappedAppointment);

            _appointmentRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(savedAppointment);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetDetailsAsync(
                    savedAppointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(detailedAppointment);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentDto>(detailedAppointment))
                .Returns(expectedDto);

            var result = await _service.CreateAsync(request, user);

            Assert.Equal(100, result.AppointmentId);
            Assert.Equal(10, mappedAppointment.PatientId);
            Assert.Equal("Pending", mappedAppointment.Status);
            Assert.Equal(string.Empty, mappedAppointment.CancellationReason);
        }

        [Fact]
        public async Task CreateAsync_WhenDetailsMissing_MapsSavedAppointment()
        {
            var user = CreateUser("Patient", patientId: 10);

            var request = CreateAppointmentRequest();

            SetupCreateBaseValidUntilDoctorAvailability(
                userPatientId: 10,
                request: request);

            _appointmentRepositoryMock
                .Setup(repository => repository.DoctorHasAppointmentAtSlotAsync(
                    request.DoctorId,
                    request.ScheduledDate,
                    request.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _doctorRepositoryMock
                .Setup(repository => repository.GetAvailableSlotsAsync(
                    request.DoctorId,
                    request.ScheduledDate,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<string> { request.TimeSlot });

            var mappedAppointment = CreateAppointment(
                appointmentId: 0,
                patientId: 10,
                doctorId: request.DoctorId,
                scheduledDate: request.ScheduledDate,
                timeSlot: request.TimeSlot);

            var savedAppointment = CreateAppointment(
                appointmentId: 101,
                patientId: 10,
                doctorId: request.DoctorId,
                scheduledDate: request.ScheduledDate,
                timeSlot: request.TimeSlot);

            var expectedDto = CreateAppointmentDto(
                appointmentId: 101,
                patientId: 10,
                doctorId: request.DoctorId,
                scheduledDate: request.ScheduledDate,
                timeSlot: request.TimeSlot);

            _mapperMock
                .Setup(mapper => mapper.Map<Appointment>(request))
                .Returns(mappedAppointment);

            _appointmentRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(savedAppointment);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetDetailsAsync(
                    savedAppointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentDto>(savedAppointment))
                .Returns(expectedDto);

            var result = await _service.CreateAsync(request, user);

            Assert.Equal(101, result.AppointmentId);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenAppointmentNotFound_ThrowsNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetDetailsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            var user = CreateUser("Patient", patientId: 10);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = "Not available"
            };

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateStatusAsync(1, request, user));

            Assert.Equal("Appointment not found", exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenPatientClaimMissing_ThrowsUnauthorizedException()
        {
            var appointment = CreateAppointment(patientId: 10);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetDetailsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var user = CreateUser("Patient");

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = "Reason"
            };

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.UpdateStatusAsync(1, request, user));

            Assert.Equal("PatientId claim missing", exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenPatientUpdatesAnotherAppointment_ThrowsUnauthorizedException()
        {
            var appointment = CreateAppointment(patientId: 10);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetDetailsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var user = CreateUser("Patient", patientId: 99);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = "Reason"
            };

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.UpdateStatusAsync(1, request, user));

            Assert.Equal("You can update only your own appointment", exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenPatientTriesNonCancelStatus_ThrowsUnauthorizedException()
        {
            var appointment = CreateAppointment(patientId: 10);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetDetailsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var user = CreateUser("Patient", patientId: 10);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Completed"
            };

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.UpdateStatusAsync(1, request, user));

            Assert.Equal("Patients can only cancel appointments", exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenDoctorClaimMissing_ThrowsUnauthorizedException()
        {
            var appointment = CreateAppointment(doctorId: 20);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetDetailsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var user = CreateUser("Doctor");

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Completed"
            };

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.UpdateStatusAsync(1, request, user));

            Assert.Equal("DoctorId claim missing", exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenDoctorUpdatesAnotherAppointment_ThrowsUnauthorizedException()
        {
            var appointment = CreateAppointment(doctorId: 20);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetDetailsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var user = CreateUser("Doctor", doctorId: 99);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Completed"
            };

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.UpdateStatusAsync(1, request, user));

            Assert.Equal("You can update only your own appointment", exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenDoctorUsesInvalidStatus_ThrowsUnauthorizedException()
        {
            var appointment = CreateAppointment(doctorId: 20);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetDetailsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var user = CreateUser("Doctor", doctorId: 20);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = "Reason"
            };

            // AppointmentService now allows doctors to cancel appointments.
            // Ensure the appointment is tracked by the in-memory DbContext so ArchiveAndRemoveCancelledAppointmentAsync can delete it.
            await _dbContext.Appointments.AddAsync(appointment);
            await _dbContext.SaveChangesAsync();

            var result = await _service.UpdateStatusAsync(1, request, user);

            Assert.Equal("Cancelled", result.Status);
            Assert.Equal("Reason", result.CancellationReason);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenCancellationReasonMissing_ThrowsInvalidException()
        {
            var appointment = CreateAppointment(
                patientId: 10,
                scheduledDate: DateTime.UtcNow.Date.AddDays(1),
                timeSlot: "16:00");

            _appointmentRepositoryMock
                .Setup(repository => repository.GetDetailsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var user = CreateUser("Patient", patientId: 10);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = ""
            };

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => _service.UpdateStatusAsync(1, request, user));

            Assert.Equal("Cancellation reason is required", exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenCancellationWithinTwoHours_ThrowsInvalidException()
        {
            var nearFuture = DateTime.UtcNow.AddHours(1);

            var appointment = CreateAppointment(
                patientId: 10,
                scheduledDate: nearFuture.Date,
                timeSlot: nearFuture.ToString("HH:mm"));

            _appointmentRepositoryMock
                .Setup(repository => repository.GetDetailsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var user = CreateUser("Patient", patientId: 10);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = "Emergency"
            };

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => _service.UpdateStatusAsync(1, request, user));

            Assert.Equal(
                "Cannot cancel appointment within 2 hours before the slot time",
                exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenAppointmentAlreadyCancelled_ThrowsInvalidException()
        {
            var appointment = CreateAppointment(
                patientId: 10,
                scheduledDate: DateTime.UtcNow.Date.AddDays(3),
                timeSlot: "16:00",
                status: "Cancelled");

            _appointmentRepositoryMock
                .Setup(repository => repository.GetDetailsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var user = CreateUser("Patient", patientId: 10);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = "Reason"
            };

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => _service.UpdateStatusAsync(1, request, user));

            Assert.Equal("Cancelled appointment cannot be updated", exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenCompletedAppointmentChangedToOtherStatus_ThrowsInvalidException()
        {
            var appointment = CreateAppointment(
                doctorId: 20,
                status: "Completed");

            _appointmentRepositoryMock
                .Setup(repository => repository.GetDetailsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            var user = CreateUser("Doctor", doctorId: 20);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Confirmed"
            };

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => _service.UpdateStatusAsync(1, request, user));

            Assert.Equal("Completed appointment cannot be changed", exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenUpdateReturnsNull_ThrowsNotFoundException()
        {
            var appointment = CreateAppointment(
                doctorId: 20,
                status: "Pending");

            _appointmentRepositoryMock
                .Setup(repository => repository.GetDetailsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    1,
                    appointment,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            var user = CreateUser("Doctor", doctorId: 20);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Confirmed"
            };

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateStatusAsync(1, request, user));

            Assert.Equal("Appointment not found", exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenDoctorConfirmsAppointment_ReturnsUpdatedDto()
        {
            var appointment = CreateAppointment(
                appointmentId: 1,
                doctorId: 20,
                status: "Pending");

            var updatedAppointment = CreateAppointment(
                appointmentId: 1,
                doctorId: 20,
                status: "Confirmed");

            var expectedDto = CreateAppointmentDto(
                appointmentId: 1,
                doctorId: 20,
                status: "Confirmed");

            _appointmentRepositoryMock
                .SetupSequence(repository => repository.GetDetailsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment)
                .ReturnsAsync(updatedAppointment);

            _appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    1,
                    appointment,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedAppointment);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentDto>(updatedAppointment))
                .Returns(expectedDto);

            var user = CreateUser("Doctor", doctorId: 20);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Confirmed"
            };

            var result = await _service.UpdateStatusAsync(1, request, user);

            Assert.Equal("Confirmed", result.Status);
            Assert.Equal("Confirmed", appointment.Status);
            Assert.Equal(string.Empty, appointment.CancellationReason);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenPatientCancelsAppointment_ReturnsUpdatedDto()
        {
            var appointment = CreateAppointment(
                appointmentId: 1,
                patientId: 10,
                scheduledDate: DateTime.UtcNow.Date.AddDays(3),
                timeSlot: "16:00",
                status: "Pending");

            var updatedAppointment = CreateAppointment(
                appointmentId: 1,
                patientId: 10,
                scheduledDate: appointment.ScheduledDate,
                timeSlot: appointment.TimeSlot,
                status: "Cancelled");

            var expectedDto = CreateAppointmentDto(
                appointmentId: 1,
                patientId: 10,
                scheduledDate: appointment.ScheduledDate,
                timeSlot: appointment.TimeSlot,
                status: "Cancelled");

            _appointmentRepositoryMock
                .SetupSequence(repository => repository.GetDetailsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment)
                .ReturnsAsync(updatedAppointment);

            _appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    1,
                    appointment,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedAppointment);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentDto>(updatedAppointment))
                .Returns(expectedDto);

            var user = CreateUser("Patient", patientId: 10);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = "Personal reason"
            };

            // Persist appointment so ArchiveAndRemoveCancelledAppointmentAsync can remove it from the in-memory DB
            await _dbContext.Appointments.AddAsync(appointment);
            await _dbContext.SaveChangesAsync();

            var result = await _service.UpdateStatusAsync(1, request, user);

            Assert.Equal("Cancelled", result.Status);
            Assert.Equal("Cancelled", result.Status);
            Assert.Equal("Personal reason", result.CancellationReason);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenUpdatedDetailsMissing_MapsUpdatedAppointment()
        {
            var appointment = CreateAppointment(
                appointmentId: 1,
                doctorId: 20,
                status: "Confirmed");

            var updatedAppointment = CreateAppointment(
                appointmentId: 1,
                doctorId: 20,
                status: "Completed");

            var expectedDto = CreateAppointmentDto(
                appointmentId: 1,
                doctorId: 20,
                status: "Completed");

            _appointmentRepositoryMock
                .SetupSequence(repository => repository.GetDetailsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment)
                .ReturnsAsync(updatedAppointment);

            _appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    1,
                    appointment,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedAppointment);

            _mapperMock
                .Setup(mapper => mapper.Map<AppointmentDto>(updatedAppointment))
                .Returns(expectedDto);

            var user = CreateUser("Doctor", doctorId: 20);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Completed"
            };

            // No separate GetDetailsAsync setup for updated appointment; sequence above returns updatedAppointment on second call

            var result = await _service.UpdateStatusAsync(1, request, user);

            Assert.Equal("Completed", result.Status);
        }

        [Fact]
        public async Task DeleteAsync_WhenAppointmentNotFound_ThrowsNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.DeleteAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _service.DeleteAsync(1));

            Assert.Equal("Appointment not found", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_WhenAppointmentExists_DeletesSuccessfully()
        {
            var appointment = CreateAppointment();

            _appointmentRepositoryMock
                .Setup(repository => repository.DeleteAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            await _service.DeleteAsync(1);

            _appointmentRepositoryMock.Verify(
                repository => repository.DeleteAsync(1, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private void SetupCreateBaseValidUntilDoctorAvailability(
            int userPatientId,
            CreateAppointmentDto request)
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.PatientHasAppointmentAtSlotAsync(
                    userPatientId,
                    request.ScheduledDate,
                    request.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    request.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateDoctor(request.DoctorId));
        }

        private static ClaimsPrincipal CreateUser(
            string role,
            int? patientId = null,
            int? doctorId = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, role)
            };

            if (patientId.HasValue)
            {
                claims.Add(new Claim("PatientId", patientId.Value.ToString()));
            }

            if (doctorId.HasValue)
            {
                claims.Add(new Claim("DoctorId", doctorId.Value.ToString()));
            }

            return new ClaimsPrincipal(
                new ClaimsIdentity(claims, "TestAuth"));
        }

        private static CreateAppointmentDto CreateAppointmentRequest(
            int doctorId = 20,
            DateTime? scheduledDate = null,
            string timeSlot = "09:00")
        {
            return new CreateAppointmentDto
            {
                DoctorId = doctorId,
                ScheduledDate = scheduledDate ?? DateTime.UtcNow.Date.AddDays(1),
                TimeSlot = timeSlot
            };
        }

        private static Appointment CreateAppointment(
            int appointmentId = 1,
            int patientId = 10,
            int doctorId = 20,
            DateTime? scheduledDate = null,
            string timeSlot = "09:00",
            string status = "Pending")
        {
            return new Appointment
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                Patient = CreatePatient(patientId),
                DoctorId = doctorId,
                Doctor = CreateDoctor(doctorId),
                ScheduledDate = scheduledDate ?? DateTime.UtcNow.Date.AddDays(1),
                TimeSlot = timeSlot,
                Status = status,
                CancellationReason = string.Empty
            };
        }

        private static AppointmentDto CreateAppointmentDto(
            int appointmentId = 1,
            int patientId = 10,
            int doctorId = 20,
            DateTime? scheduledDate = null,
            string timeSlot = "09:00",
            string status = "Pending")
        {
            return new AppointmentDto
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                PatientName = "Patient One",
                DoctorId = doctorId,
                DoctorName = "Doctor One",
                Specialisation = "Cardiologist",
                ScheduledDate = scheduledDate ?? DateTime.UtcNow.Date.AddDays(1),
                TimeSlot = timeSlot,
                Status = status,
                CancellationReason = string.Empty
            };
        }

        private static Patient CreatePatient(int patientId = 10)
        {
            return new Patient
            {
                PatientId = patientId,
                PatientName = "Patient One",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = "Male",
                Email = "patient@test.com",
                PhoneNumber = "9876543210",
                InsuranceID = "INS001",
                IsActive = true
            };
        }

        private static Doctor CreateDoctor(
            int doctorId = 20,
            bool isActive = true)
        {
            return new Doctor
            {
                DoctorId = doctorId,
                DoctorName = "Doctor One",
                Specialisation = "Cardiologist",
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = isActive
            };
        }
    }
}