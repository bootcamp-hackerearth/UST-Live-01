using AutoMapper;
using HealthAxis.API.DTOs.Appointments;
using HealthAxis.API.Enums;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;
using HealthAxis.API.Services;
using Moq;

namespace HealthAxis.API.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AppointmentService _appointmentService;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();

            _appointmentService = new AppointmentService(
                _appointmentRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_WhenScheduledDateIsInPast_ThrowsInvalidOperationException()
        {
            // Arrange
            AppointmentCreateDto createDto = CreateAppointmentCreateDto();
            createDto.ScheduledDate = DateTime.Today.AddDays(-1);

            // Act & Assert
            InvalidOperationException exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.CreateAsync(createDto));

            Assert.Equal("Past dates are not allowed.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenTimeSlotIsEmpty_ThrowsInvalidOperationException()
        {
            // Arrange
            AppointmentCreateDto createDto = CreateAppointmentCreateDto();
            createDto.TimeSlot = string.Empty;

            // Act & Assert
            InvalidOperationException exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.CreateAsync(createDto));

            Assert.Equal("Time slot is required.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenTimeSlotIsWhiteSpace_ThrowsInvalidOperationException()
        {
            // Arrange
            AppointmentCreateDto createDto = CreateAppointmentCreateDto();
            createDto.TimeSlot = "   ";

            // Act & Assert
            InvalidOperationException exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.CreateAsync(createDto));

            Assert.Equal("Time slot is required.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenScheduledDateIsTodayAndTimeSlotIsPast_ThrowsInvalidOperationException()
        {
            // Arrange
            AppointmentCreateDto createDto = CreateAppointmentCreateDto();
            createDto.ScheduledDate = DateTime.Today;
            createDto.TimeSlot = "00:00";

            // Act & Assert
            InvalidOperationException exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.CreateAsync(createDto));

            Assert.Equal("Past time slots are not allowed.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenTodayTimeSlotFormatIsInvalid_ThrowsInvalidOperationException()
        {
            // Arrange
            AppointmentCreateDto createDto = CreateAppointmentCreateDto();
            createDto.ScheduledDate = DateTime.Today;
            createDto.TimeSlot = "Invalid Slot";

            // Act & Assert
            InvalidOperationException exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.CreateAsync(createDto));

            Assert.Equal("Past time slots are not allowed.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenScheduledDateIsMoreThanSixMonthsAhead_ThrowsInvalidOperationException()
        {
            // Arrange
            AppointmentCreateDto createDto = CreateAppointmentCreateDto();
            createDto.ScheduledDate = DateTime.Today.AddMonths(6).AddDays(1);

            // Act & Assert
            InvalidOperationException exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.CreateAsync(createDto));

            Assert.Equal(
                "Appointments can only be booked up to 6 months in advance.",
                exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenDoctorDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            AppointmentCreateDto createDto = CreateAppointmentCreateDto();

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    createDto.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            // Act & Assert
            InvalidOperationException exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.CreateAsync(createDto));

            Assert.Equal("Doctor not found.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenDoctorIsInactive_ThrowsInvalidOperationException()
        {
            // Arrange
            AppointmentCreateDto createDto = CreateAppointmentCreateDto();

            Doctor doctor = new()
            {
                DoctorId = createDto.DoctorId,
                FullName = "Dr. Inactive",
                IsActive = false
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    createDto.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            // Act & Assert
            InvalidOperationException exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.CreateAsync(createDto));

            Assert.Equal(
                "This doctor is currently inactive and cannot accept appointments.",
                exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenPatientDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            AppointmentCreateDto createDto = CreateAppointmentCreateDto();

            Doctor doctor = CreateActiveDoctor(createDto.DoctorId);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    createDto.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    createDto.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            // Act & Assert
            InvalidOperationException exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.CreateAsync(createDto));

            Assert.Equal("Patient not found.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenDoctorAlreadyBookedForSameSlot_ThrowsInvalidOperationException()
        {
            // Arrange
            AppointmentCreateDto createDto = CreateAppointmentCreateDto();

            Doctor doctor = CreateActiveDoctor(createDto.DoctorId);
            Patient patient = CreatePatient(createDto.PatientId);

            List<Appointment> existingAppointments = new()
            {
                new Appointment
                {
                    AppointmentId = 1,
                    DoctorId = createDto.DoctorId,
                    PatientId = 99,
                    ScheduledDate = createDto.ScheduledDate.Date,
                    TimeSlot = createDto.TimeSlot,
                    Status = AppointmentStatus.Scheduled
                }
            };

            SetupDoctorPatientAndAppointments(createDto, doctor, patient, existingAppointments);

            // Act & Assert
            InvalidOperationException exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.CreateAsync(createDto));

            Assert.Equal(
                "This doctor is already booked for the selected date and time slot.",
                exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenDoctorBookingIsCancelled_AllowsNewAppointment()
        {
            // Arrange
            AppointmentCreateDto createDto = CreateAppointmentCreateDto();

            Doctor doctor = CreateActiveDoctor(createDto.DoctorId);
            Patient patient = CreatePatient(createDto.PatientId);

            List<Appointment> existingAppointments = new()
            {
                new Appointment
                {
                    AppointmentId = 1,
                    DoctorId = createDto.DoctorId,
                    PatientId = 99,
                    ScheduledDate = createDto.ScheduledDate.Date,
                    TimeSlot = createDto.TimeSlot,
                    Status = AppointmentStatus.Cancelled
                }
            };

            Appointment appointmentToCreate = new()
            {
                PatientId = createDto.PatientId,
                DoctorId = createDto.DoctorId,
                ScheduledDate = createDto.ScheduledDate,
                TimeSlot = createDto.TimeSlot
            };

            Appointment createdAppointment = new()
            {
                AppointmentId = 10,
                PatientId = createDto.PatientId,
                DoctorId = createDto.DoctorId,
                ScheduledDate = createDto.ScheduledDate.Date,
                TimeSlot = createDto.TimeSlot,
                Status = AppointmentStatus.Scheduled
            };

            SetupDoctorPatientAndAppointments(createDto, doctor, patient, existingAppointments);

            _mapperMock
                .Setup(mapper => mapper.Map<Appointment>(createDto))
                .Returns(appointmentToCreate);

            _appointmentRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdAppointment);

            // Act
            AppointmentReadDto result =
                await _appointmentService.CreateAsync(createDto);

            // Assert
            Assert.Equal(10, result.AppointmentId);
            Assert.Equal(AppointmentStatus.Scheduled, result.Status);

            _appointmentRepositoryMock.Verify(
                repository => repository.CreateAsync(
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenPatientAlreadyBookedForSameSlot_ThrowsInvalidOperationException()
        {
            // Arrange
            AppointmentCreateDto createDto = CreateAppointmentCreateDto();

            Doctor doctor = CreateActiveDoctor(createDto.DoctorId);
            Patient patient = CreatePatient(createDto.PatientId);

            List<Appointment> existingAppointments = new()
            {
                new Appointment
                {
                    AppointmentId = 1,
                    DoctorId = 99,
                    PatientId = createDto.PatientId,
                    ScheduledDate = createDto.ScheduledDate.Date,
                    TimeSlot = createDto.TimeSlot,
                    Status = AppointmentStatus.Confirmed
                }
            };

            SetupDoctorPatientAndAppointments(createDto, doctor, patient, existingAppointments);

            // Act & Assert
            InvalidOperationException exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _appointmentService.CreateAsync(createDto));

            Assert.Equal(
                "You already have an appointment booked at this date and time slot.",
                exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenValidRequest_CreatesAppointmentWithScheduledStatusAndReturnsNames()
        {
            // Arrange
            AppointmentCreateDto createDto = CreateAppointmentCreateDto();

            Doctor doctor = CreateActiveDoctor(createDto.DoctorId);
            Patient patient = CreatePatient(createDto.PatientId);

            Appointment mappedAppointment = new()
            {
                PatientId = createDto.PatientId,
                DoctorId = createDto.DoctorId,
                ScheduledDate = createDto.ScheduledDate,
                TimeSlot = createDto.TimeSlot
            };

            Appointment createdAppointment = new()
            {
                AppointmentId = 100,
                PatientId = createDto.PatientId,
                DoctorId = createDto.DoctorId,
                ScheduledDate = createDto.ScheduledDate.Date,
                TimeSlot = createDto.TimeSlot,
                Status = AppointmentStatus.Scheduled,
                CancellationReason = string.Empty
            };

            SetupDoctorPatientAndAppointments(
                createDto,
                doctor,
                patient,
                new List<Appointment>());

            _mapperMock
                .Setup(mapper => mapper.Map<Appointment>(createDto))
                .Returns(mappedAppointment);

            _appointmentRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdAppointment);

            // Act
            AppointmentReadDto result =
                await _appointmentService.CreateAsync(createDto);

            // Assert
            Assert.Equal(100, result.AppointmentId);
            Assert.Equal(createDto.PatientId, result.PatientId);
            Assert.Equal(patient.FullName, result.PatientName);
            Assert.Equal(createDto.DoctorId, result.DoctorId);
            Assert.Equal(doctor.FullName, result.DoctorName);
            Assert.Equal(createDto.ScheduledDate.Date, result.ScheduledDate);
            Assert.Equal(createDto.TimeSlot, result.TimeSlot);
            Assert.Equal(AppointmentStatus.Scheduled, result.Status);

            _appointmentRepositoryMock.Verify(
                repository => repository.CreateAsync(
                    It.Is<Appointment>(appointment =>
                        appointment.ScheduledDate == createDto.ScheduledDate.Date &&
                        appointment.Status == AppointmentStatus.Scheduled),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAllWithDetailsAsync_WhenAppointmentsExist_ReturnsAppointmentsWithDoctorAndPatientNamesOrderedByDateDescending()
        {
            // Arrange
            DateTime olderDate = DateTime.Today.AddDays(1);
            DateTime newerDate = DateTime.Today.AddDays(3);

            List<Appointment> appointments = new()
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = olderDate,
                    TimeSlot = "10:00",
                    Status = AppointmentStatus.Scheduled
                },
                new Appointment
                {
                    AppointmentId = 2,
                    PatientId = 2,
                    DoctorId = 2,
                    ScheduledDate = newerDate,
                    TimeSlot = "11:00",
                    Status = AppointmentStatus.Confirmed
                }
            };

            List<Doctor> doctors = new()
            {
                new Doctor { DoctorId = 1, FullName = "Dr. One" },
                new Doctor { DoctorId = 2, FullName = "Dr. Two" }
            };

            List<Patient> patients = new()
            {
                new Patient { PatientId = 1, FullName = "Patient One" },
                new Patient { PatientId = 2, FullName = "Patient Two" }
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patients);

            // Act
            List<AppointmentReadDto> result =
                await _appointmentService.GetAllWithDetailsAsync();

            // Assert
            Assert.Equal(2, result.Count);

            Assert.Equal(2, result[0].AppointmentId);
            Assert.Equal(newerDate, result[0].ScheduledDate);
            Assert.Equal("Dr. Two", result[0].DoctorName);
            Assert.Equal("Patient Two", result[0].PatientName);

            Assert.Equal(1, result[1].AppointmentId);
            Assert.Equal(olderDate, result[1].ScheduledDate);
            Assert.Equal("Dr. One", result[1].DoctorName);
            Assert.Equal("Patient One", result[1].PatientName);
        }

        [Fact]
        public async Task GetAllWithDetailsAsync_WhenDoctorOrPatientMissing_ReturnsUnknownNames()
        {
            // Arrange
            List<Appointment> appointments = new()
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 99,
                    DoctorId = 88,
                    ScheduledDate = DateTime.Today.AddDays(1),
                    TimeSlot = "10:00",
                    Status = AppointmentStatus.Scheduled
                }
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Doctor>());

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Patient>());

            // Act
            List<AppointmentReadDto> result =
                await _appointmentService.GetAllWithDetailsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Unknown Doctor", result[0].DoctorName);
            Assert.Equal("Unknown Patient", result[0].PatientName);
        }

        [Fact]
        public async Task GetAppointmentsByPatientIdAsync_WhenAppointmentsExist_ReturnsOnlyPatientAppointmentsOrderedByDateDescending()
        {
            // Arrange
            const int patientId = 1;

            DateTime olderDate = DateTime.Today.AddDays(1);
            DateTime newerDate = DateTime.Today.AddDays(4);

            List<Appointment> appointments = new()
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = patientId,
                    DoctorId = 1,
                    ScheduledDate = olderDate,
                    TimeSlot = "10:00",
                    Status = AppointmentStatus.Scheduled
                },
                new Appointment
                {
                    AppointmentId = 2,
                    PatientId = 2,
                    DoctorId = 1,
                    ScheduledDate = DateTime.Today.AddDays(2),
                    TimeSlot = "11:00",
                    Status = AppointmentStatus.Scheduled
                },
                new Appointment
                {
                    AppointmentId = 3,
                    PatientId = patientId,
                    DoctorId = 2,
                    ScheduledDate = newerDate,
                    TimeSlot = "12:00",
                    Status = AppointmentStatus.Confirmed
                }
            };

            SetupDoctorsAndPatientsForListMapping();

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            // Act
            List<AppointmentReadDto> result =
                await _appointmentService.GetAppointmentsByPatientIdAsync(patientId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(3, result[0].AppointmentId);
            Assert.Equal(newerDate, result[0].ScheduledDate);
            Assert.Equal(1, result[1].AppointmentId);
            Assert.Equal(olderDate, result[1].ScheduledDate);
            Assert.All(result, item => Assert.Equal(patientId, item.PatientId));
        }

        [Fact]
        public async Task GetAppointmentsByDoctorIdAsync_WhenAppointmentsExist_ReturnsOnlyDoctorAppointmentsOrderedByDateDescending()
        {
            // Arrange
            const int doctorId = 1;

            DateTime olderDate = DateTime.Today.AddDays(1);
            DateTime newerDate = DateTime.Today.AddDays(5);

            List<Appointment> appointments = new()
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = doctorId,
                    ScheduledDate = olderDate,
                    TimeSlot = "10:00",
                    Status = AppointmentStatus.Scheduled
                },
                new Appointment
                {
                    AppointmentId = 2,
                    PatientId = 2,
                    DoctorId = 2,
                    ScheduledDate = DateTime.Today.AddDays(2),
                    TimeSlot = "11:00",
                    Status = AppointmentStatus.Scheduled
                },
                new Appointment
                {
                    AppointmentId = 3,
                    PatientId = 3,
                    DoctorId = doctorId,
                    ScheduledDate = newerDate,
                    TimeSlot = "12:00",
                    Status = AppointmentStatus.Completed
                }
            };

            SetupDoctorsAndPatientsForListMapping();

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            // Act
            List<AppointmentReadDto> result =
                await _appointmentService.GetAppointmentsByDoctorIdAsync(doctorId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(3, result[0].AppointmentId);
            Assert.Equal(newerDate, result[0].ScheduledDate);
            Assert.Equal(1, result[1].AppointmentId);
            Assert.Equal(olderDate, result[1].ScheduledDate);
            Assert.All(result, item => Assert.Equal(doctorId, item.DoctorId));
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenAppointmentDoesNotExist_ReturnsNull()
        {
            // Arrange
            const int appointmentId = 404;

            AppointmentStatusUpdateDto statusUpdateDto = new()
            {
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            // Act
            AppointmentReadDto? result =
                await _appointmentService.UpdateStatusAsync(
                    appointmentId,
                    statusUpdateDto);

            // Assert
            Assert.Null(result);

            _appointmentRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenStatusIsConfirmed_ConfirmsAppointmentAndReturnsDto()
        {
            // Arrange
            const int appointmentId = 1;

            Appointment appointment = CreateAppointment(appointmentId);
            appointment.Status = AppointmentStatus.Scheduled;

            AppointmentStatusUpdateDto statusUpdateDto = new()
            {
                Status = AppointmentStatus.Confirmed
            };

            SetupAppointmentStatusUpdate(appointmentId, appointment);

            // Act
            AppointmentReadDto? result =
                await _appointmentService.UpdateStatusAsync(
                    appointmentId,
                    statusUpdateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(AppointmentStatus.Confirmed, result.Status);

            _appointmentRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenStatusIsCancelled_CancelsAppointmentWithReasonAndReturnsDto()
        {
            // Arrange
            const int appointmentId = 1;

            Appointment appointment = CreateAppointment(appointmentId);
            appointment.Status = AppointmentStatus.Scheduled;

            AppointmentStatusUpdateDto statusUpdateDto = new()
            {
                Status = AppointmentStatus.Cancelled,
                CancellationReason = "Patient unavailable"
            };

            SetupAppointmentStatusUpdate(appointmentId, appointment);

            // Act
            AppointmentReadDto? result =
                await _appointmentService.UpdateStatusAsync(
                    appointmentId,
                    statusUpdateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(AppointmentStatus.Cancelled, result.Status);
            Assert.Equal("Patient unavailable", result.CancellationReason);

            _appointmentRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenStatusIsCompleted_CompletesAppointmentAndReturnsDto()
        {
            // Arrange
            const int appointmentId = 1;

            Appointment appointment = CreateAppointment(appointmentId);
            appointment.Status = AppointmentStatus.Confirmed;

            AppointmentStatusUpdateDto statusUpdateDto = new()
            {
                Status = AppointmentStatus.Completed
            };

            SetupAppointmentStatusUpdate(appointmentId, appointment);

            // Act
            AppointmentReadDto? result =
                await _appointmentService.UpdateStatusAsync(
                    appointmentId,
                    statusUpdateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(AppointmentStatus.Completed, result.Status);

            _appointmentRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenStatusIsOtherStatus_UpdatesStatusDirectlyAndReturnsDto()
        {
            // Arrange
            const int appointmentId = 1;

            Appointment appointment = CreateAppointment(appointmentId);
            appointment.Status = AppointmentStatus.Confirmed;

            AppointmentStatusUpdateDto statusUpdateDto = new()
            {
                Status = AppointmentStatus.Scheduled
            };

            SetupAppointmentStatusUpdate(appointmentId, appointment);

            // Act
            AppointmentReadDto? result =
                await _appointmentService.UpdateStatusAsync(
                    appointmentId,
                    statusUpdateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(AppointmentStatus.Scheduled, result.Status);

            _appointmentRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenDoctorOrPatientMissing_ReturnsUnknownNames()
        {
            // Arrange
            const int appointmentId = 1;

            Appointment appointment = CreateAppointment(appointmentId);
            appointment.DoctorId = 88;
            appointment.PatientId = 99;

            AppointmentStatusUpdateDto statusUpdateDto = new()
            {
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            // Act
            AppointmentReadDto? result =
                await _appointmentService.UpdateStatusAsync(
                    appointmentId,
                    statusUpdateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Unknown Doctor", result.DoctorName);
            Assert.Equal("Unknown Patient", result.PatientName);
        }

        [Fact]
        public async Task GetAppointmentReportAsync_WhenAppointmentsExist_ReturnsGroupedReportOrderedByDate()
        {
            // Arrange
            DateTime firstDate = DateTime.Today.AddDays(1);
            DateTime secondDate = DateTime.Today.AddDays(2);

            List<Appointment> appointments = new()
            {
                new Appointment
                {
                    AppointmentId = 1,
                    ScheduledDate = firstDate,
                    Status = AppointmentStatus.Scheduled
                },
                new Appointment
                {
                    AppointmentId = 2,
                    ScheduledDate = firstDate,
                    Status = AppointmentStatus.Confirmed
                },
                new Appointment
                {
                    AppointmentId = 3,
                    ScheduledDate = firstDate,
                    Status = AppointmentStatus.Cancelled
                },
                new Appointment
                {
                    AppointmentId = 4,
                    ScheduledDate = firstDate,
                    Status = AppointmentStatus.Completed
                },
                new Appointment
                {
                    AppointmentId = 5,
                    ScheduledDate = secondDate,
                    Status = AppointmentStatus.Scheduled
                }
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            // Act
            List<AppointmentReportDto> result =
                await _appointmentService.GetAppointmentReportAsync();

            // Assert
            Assert.Equal(2, result.Count);

            Assert.Equal(firstDate.Date, result[0].Date);
            Assert.Equal(4, result[0].TotalCount);
            Assert.Equal(1, result[0].ScheduledCount);
            Assert.Equal(1, result[0].ConfirmedCount);
            Assert.Equal(1, result[0].CancelledCount);
            Assert.Equal(1, result[0].CompletedCount);

            Assert.Equal(secondDate.Date, result[1].Date);
            Assert.Equal(1, result[1].TotalCount);
            Assert.Equal(1, result[1].ScheduledCount);
            Assert.Equal(0, result[1].ConfirmedCount);
            Assert.Equal(0, result[1].CancelledCount);
            Assert.Equal(0, result[1].CompletedCount);
        }

        [Fact]
        public async Task GetAppointmentReportAsync_WhenNoAppointmentsExist_ReturnsEmptyReport()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Appointment>());

            // Act
            List<AppointmentReportDto> result =
                await _appointmentService.GetAppointmentReportAsync();

            // Assert
            Assert.Empty(result);
        }

        private static AppointmentCreateDto CreateAppointmentCreateDto()
        {
            return new AppointmentCreateDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00"
            };
        }

        private static Doctor CreateActiveDoctor(int doctorId)
        {
            return new Doctor
            {
                DoctorId = doctorId,
                FullName = "Dr. Test",
                IsActive = true
            };
        }

        private static Patient CreatePatient(int patientId)
        {
            return new Patient
            {
                PatientId = patientId,
                FullName = "Patient Test"
            };
        }

        private static Appointment CreateAppointment(int appointmentId)
        {
            return new Appointment
            {
                AppointmentId = appointmentId,
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00",
                Status = AppointmentStatus.Scheduled,
                CancellationReason = string.Empty
            };
        }

        private void SetupDoctorPatientAndAppointments(
            AppointmentCreateDto createDto,
            Doctor doctor,
            Patient patient,
            List<Appointment> appointments)
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    createDto.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    createDto.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);
        }

        private void SetupDoctorsAndPatientsForListMapping()
        {
            List<Doctor> doctors = new()
            {
                new Doctor { DoctorId = 1, FullName = "Dr. One" },
                new Doctor { DoctorId = 2, FullName = "Dr. Two" },
                new Doctor { DoctorId = 3, FullName = "Dr. Three" }
            };

            List<Patient> patients = new()
            {
                new Patient { PatientId = 1, FullName = "Patient One" },
                new Patient { PatientId = 2, FullName = "Patient Two" },
                new Patient { PatientId = 3, FullName = "Patient Three" }
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patients);
        }

        private void SetupAppointmentStatusUpdate(
            int appointmentId,
            Appointment appointment)
        {
            Doctor doctor = new()
            {
                DoctorId = appointment.DoctorId,
                FullName = "Dr. Test"
            };

            Patient patient = new()
            {
                PatientId = appointment.PatientId,
                FullName = "Patient Test"
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(repository => repository.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);
        }
    }
}