using FluentAssertions;
using Moq;
using S3_HealthAxisApi.DTOs.Appointment;
using S3_HealthAxisApi.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Implementation;
using Xunit;

namespace S3_HealthAxis.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();

            _service = new AppointmentService(
                _appointmentRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _doctorRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                CreateAppointment(
                    appointmentId: 1,
                    patientId: 101,
                    doctorId: 201,
                    scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                    timeSlot: AppointmentTimeSlot.TenAM,
                    status: AppointmentStatus.Pending),
                CreateAppointment(
                    appointmentId: 2,
                    patientId: 102,
                    doctorId: 202,
                    scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                    timeSlot: AppointmentTimeSlot.ElevenAM,
                    status: AppointmentStatus.Confirmed,
                    cancellationReason: null)
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(appointments);

            // Act
            var result = (await _service.GetAllAsync()).ToList();

            // Assert
            result.Should().HaveCount(2);

            result[0].AppointmentId.Should().Be(1);
            result[0].PatientId.Should().Be(101);
            result[0].DoctorId.Should().Be(201);
            result[0].ScheduledDate.Should().Be(DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
            result[0].TimeSlot.Should().Be((int)AppointmentTimeSlot.TenAM);
            result[0].Status.Should().Be((int)AppointmentStatus.Pending);

            result[1].AppointmentId.Should().Be(2);
            result[1].PatientId.Should().Be(102);
            result[1].DoctorId.Should().Be(202);
            result[1].TimeSlot.Should().Be((int)AppointmentTimeSlot.ElevenAM);
            result[1].Status.Should().Be((int)AppointmentStatus.Confirmed);

            _appointmentRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
            _appointmentRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenAppointmentDoesNotExist()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(999))
                .ReturnsAsync((Appointment?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();

            _appointmentRepositoryMock.Verify(x => x.GetByIdAsync(999), Times.Once);
            _appointmentRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedAppointmentDetails_WhenAppointmentExists()
        {
            // Arrange
            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 111,
                doctorId: 222,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(3)),
                timeSlot: AppointmentTimeSlot.TwelvePM,
                status: AppointmentStatus.Confirmed,
                patientFullName: "Rahul Sharma",
                doctorFullName: "Dr. Meera",
                cancellationReason: "Previous reason");

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(appointment);

            // Act
            var result = await _service.GetByIdAsync(10);

            // Assert
            result.Should().NotBeNull();
            result!.AppointmentId.Should().Be(10);
            result.PatientId.Should().Be(111);
            result.PatientName.Should().Be("Rahul Sharma");
            result.DoctorId.Should().Be(222);
            result.DoctorName.Should().Be("Dr. Meera");
            result.ScheduledDate.Should().Be(DateOnly.FromDateTime(DateTime.Today.AddDays(3)));
            result.TimeSlot.Should().Be((int)AppointmentTimeSlot.TwelvePM);
            result.Status.Should().Be((int)AppointmentStatus.Confirmed);
            result.CancellationReason.Should().Be("Previous reason");

            _appointmentRepositoryMock.Verify(x => x.GetByIdAsync(10), Times.Once);
            _appointmentRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetPatientHistoryAsync_ShouldReturnMappedHistory()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                CreateAppointment(
                    appointmentId: 1,
                    patientId: 1001,
                    doctorId: 5001,
                    scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(-10)),
                    timeSlot: AppointmentTimeSlot.TenAM,
                    status: AppointmentStatus.Completed,
                    doctorFullName: "Dr. Adams"),
                CreateAppointment(
                    appointmentId: 2,
                    patientId: 1001,
                    doctorId: 5002,
                    scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
                    timeSlot: AppointmentTimeSlot.ElevenAM,
                    status: AppointmentStatus.Cancelled,
                    doctorFullName: "Dr. Xavier")
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(1001))
                .ReturnsAsync(appointments);

            // Act
            var result = (await _service.GetPatientHistoryAsync(1001)).ToList();

            // Assert
            result.Should().HaveCount(2);

            result[0].AppointmentId.Should().Be(1);
            result[0].DoctorId.Should().Be(5001);
            result[0].DoctorName.Should().Be("Dr. Adams");
            result[0].TimeSlot.Should().Be((int)AppointmentTimeSlot.TenAM);
            result[0].Status.Should().Be((int)AppointmentStatus.Completed);

            result[1].AppointmentId.Should().Be(2);
            result[1].DoctorId.Should().Be(5002);
            result[1].DoctorName.Should().Be("Dr. Xavier");
            result[1].TimeSlot.Should().Be((int)AppointmentTimeSlot.ElevenAM);
            result[1].Status.Should().Be((int)AppointmentStatus.Cancelled);

            _appointmentRepositoryMock.Verify(x => x.GetByPatientIdAsync(1001), Times.Once);
            _appointmentRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetDoctorTodayScheduleAsync_ShouldReturnMappedSchedule()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);

            var appointments = new List<Appointment>
            {
                CreateAppointment(
                    appointmentId: 1,
                    patientId: 301,
                    doctorId: 401,
                    scheduledDate: today,
                    timeSlot: AppointmentTimeSlot.OnePM,
                    status: AppointmentStatus.Pending,
                    patientFullName: "Patient One")
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetDoctorTodayScheduleAsync(401, today))
                .ReturnsAsync(appointments);

            // Act
            var result = (await _service.GetDoctorTodayScheduleAsync(401)).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].AppointmentId.Should().Be(1);
            result[0].PatientId.Should().Be(301);
            result[0].PatientName.Should().Be("Patient One");
            result[0].ScheduledDate.Should().Be(today);
            result[0].TimeSlot.Should().Be((int)AppointmentTimeSlot.OnePM);
            result[0].Status.Should().Be((int)AppointmentStatus.Pending);

            _appointmentRepositoryMock.Verify(x => x.GetDoctorTodayScheduleAsync(401, today), Times.Once);
            _appointmentRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetDoctorWeekScheduleAsync_ShouldReturnMappedSchedule()
        {
            // Arrange
            var startDate = DateOnly.FromDateTime(DateTime.Today);
            var endDate = startDate.AddDays(7);

            var appointments = new List<Appointment>
            {
                CreateAppointment(
                    appointmentId: 11,
                    patientId: 901,
                    doctorId: 777,
                    scheduledDate: startDate.AddDays(2),
                    timeSlot: AppointmentTimeSlot.ThreePM,
                    status: AppointmentStatus.Confirmed,
                    patientFullName: "Week Patient")
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetDoctorWeekScheduleAsync(777, startDate, endDate))
                .ReturnsAsync(appointments);

            // Act
            var result = (await _service.GetDoctorWeekScheduleAsync(777, startDate, endDate)).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].AppointmentId.Should().Be(11);
            result[0].PatientName.Should().Be("Week Patient");
            result[0].TimeSlot.Should().Be((int)AppointmentTimeSlot.ThreePM);
            result[0].Status.Should().Be((int)AppointmentStatus.Confirmed);

            _appointmentRepositoryMock.Verify(x => x.GetDoctorWeekScheduleAsync(777, startDate, endDate), Times.Once);
            _appointmentRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetDoctorUpcomingScheduleAsync_ShouldCallWeekScheduleWithTodayAndNext7Days()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);
            var endDate = today.AddDays(7);

            var appointments = new List<Appointment>
            {
                CreateAppointment(
                    appointmentId: 20,
                    patientId: 100,
                    doctorId: 200,
                    scheduledDate: today.AddDays(1),
                    timeSlot: AppointmentTimeSlot.TwoPM,
                    status: AppointmentStatus.Pending,
                    patientFullName: "Upcoming Patient")
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetDoctorWeekScheduleAsync(200, today, endDate))
                .ReturnsAsync(appointments);

            // Act
            var result = (await _service.GetDoctorUpcomingScheduleAsync(200)).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].PatientName.Should().Be("Upcoming Patient");
            result[0].TimeSlot.Should().Be((int)AppointmentTimeSlot.TwoPM);

            _appointmentRepositoryMock.Verify(x => x.GetDoctorWeekScheduleAsync(200, today, endDate), Times.Once);
            _appointmentRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateAppointment_WhenBookingIsValid()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                TimeSlot = (int)AppointmentTimeSlot.TenAM
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(CreatePatient(isActive: true));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(CreateDoctor(isActive: true));

            _appointmentRepositoryMock
                .Setup(x => x.ExistsSamePatientSameDoctorSameDateAsync(dto.PatientId, dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(x => x.ExistsSamePatientSameSlotSameDateAsync(dto.PatientId, dto.ScheduledDate, dto.TimeSlot))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(x => x.ExistsSameDoctorSameSlotSameDateAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            _appointmentRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.PatientId.Should().Be(1);
            result.DoctorId.Should().Be(2);
            result.ScheduledDate.Should().Be(dto.ScheduledDate);
            result.TimeSlot.Should().Be(dto.TimeSlot);
            result.Status.Should().Be((int)AppointmentStatus.Pending);

            _appointmentRepositoryMock.Verify(
                x => x.AddAsync(It.Is<Appointment>(a =>
                    a.PatientId == dto.PatientId &&
                    a.DoctorId == dto.DoctorId &&
                    a.ScheduledDate == dto.ScheduledDate &&
                    a.TimeSlot == (AppointmentTimeSlot)dto.TimeSlot &&
                    a.Status == AppointmentStatus.Pending)),
                Times.Once);

            _appointmentRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowKeyNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                TimeSlot = (int)AppointmentTimeSlot.TenAM
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync((Patient?)null);

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Patient not found*");

            _appointmentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Appointment>()), Times.Never);
            _appointmentRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenPatientIsInactive()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                TimeSlot = (int)AppointmentTimeSlot.TenAM
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(CreatePatient(isActive: false));

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Inactive patients cannot book appointments*");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowKeyNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                TimeSlot = (int)AppointmentTimeSlot.TenAM
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(CreatePatient(isActive: true));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync((Doctor?)null);

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Doctor not found*");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenDoctorIsInactive()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                TimeSlot = (int)AppointmentTimeSlot.TenAM
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(CreatePatient(isActive: true));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(CreateDoctor(isActive: false));

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Inactive doctor*");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenDateIsInPast()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
                TimeSlot = (int)AppointmentTimeSlot.TenAM
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(CreatePatient(isActive: true));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(CreateDoctor(isActive: true));

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*past*");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenTimeSlotIsInvalid()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                TimeSlot = 999
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(CreatePatient(isActive: true));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(CreateDoctor(isActive: true));

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Invalid appointment slot*");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenSamePatientSameDoctorSameDateExists()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientId = 7,
                DoctorId = 8,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                TimeSlot = (int)AppointmentTimeSlot.ElevenAM
            };

            _patientRepositoryMock.Setup(x => x.GetByIdAsync(dto.PatientId)).ReturnsAsync(CreatePatient(true));
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(dto.DoctorId)).ReturnsAsync(CreateDoctor(true));

            _appointmentRepositoryMock
                .Setup(x => x.ExistsSamePatientSameDoctorSameDateAsync(dto.PatientId, dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*appointment with this doctor on the selected date*");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenSamePatientSameSlotSameDateExists()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientId = 7,
                DoctorId = 8,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                TimeSlot = (int)AppointmentTimeSlot.ElevenAM
            };

            _patientRepositoryMock.Setup(x => x.GetByIdAsync(dto.PatientId)).ReturnsAsync(CreatePatient(true));
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(dto.DoctorId)).ReturnsAsync(CreateDoctor(true));

            _appointmentRepositoryMock
                .Setup(x => x.ExistsSamePatientSameDoctorSameDateAsync(dto.PatientId, dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(x => x.ExistsSamePatientSameSlotSameDateAsync(dto.PatientId, dto.ScheduledDate, dto.TimeSlot))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*another appointment in this time slot*");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenSameDoctorSameSlotSameDateExists()
        {
            // Arrange
            var dto = new CreateAppointmentDto
            {
                PatientId = 7,
                DoctorId = 8,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                TimeSlot = (int)AppointmentTimeSlot.ElevenAM
            };

            _patientRepositoryMock.Setup(x => x.GetByIdAsync(dto.PatientId)).ReturnsAsync(CreatePatient(true));
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(dto.DoctorId)).ReturnsAsync(CreateDoctor(true));

            _appointmentRepositoryMock
                .Setup(x => x.ExistsSamePatientSameDoctorSameDateAsync(dto.PatientId, dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(x => x.ExistsSamePatientSameSlotSameDateAsync(dto.PatientId, dto.ScheduledDate, dto.TimeSlot))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(x => x.ExistsSameDoctorSameSlotSameDateAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Doctor is already booked*");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenAppointmentDoesNotExist()
        {
            // Arrange
            var dto = new UpdateAppointmentDto
            {
                DoctorId = 2,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                TimeSlot = (int)AppointmentTimeSlot.TenAM
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Appointment?)null);

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(99, dto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Appointment 99 not found*");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenAppointmentIsCompleted()
        {
            // Arrange
            var dto = new UpdateAppointmentDto
            {
                DoctorId = 2,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                TimeSlot = (int)AppointmentTimeSlot.TenAM
            };

            var appointment = CreateAppointment(
                appointmentId: 50,
                patientId: 1,
                doctorId: 2,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.ElevenAM,
                status: AppointmentStatus.Completed);

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(50))
                .ReturnsAsync(appointment);

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(50, dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Completed appointments cannot be modified*");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenAppointmentIsCancelled()
        {
            // Arrange
            var dto = new UpdateAppointmentDto
            {
                DoctorId = 2,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                TimeSlot = (int)AppointmentTimeSlot.TenAM
            };

            var appointment = CreateAppointment(
                appointmentId: 51,
                patientId: 1,
                doctorId: 2,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.ElevenAM,
                status: AppointmentStatus.Cancelled);

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(51))
                .ReturnsAsync(appointment);

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(51, dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Cancelled appointments cannot be modified*");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateAppointment_WhenValid_AndExcludeCurrentAppointmentFromConflictChecks()
        {
            // Arrange
            var appointmentId = 60;
            var dto = new UpdateAppointmentDto
            {
                DoctorId = 9,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
                TimeSlot = (int)AppointmentTimeSlot.ThreePM
            };

            var existingAppointment = CreateAppointment(
                appointmentId: appointmentId,
                patientId: 15,
                doctorId: 2,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Pending);

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(appointmentId))
                .ReturnsAsync(existingAppointment);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(existingAppointment.PatientId))
                .ReturnsAsync(CreatePatient(true));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(CreateDoctor(true));

            _appointmentRepositoryMock
                .Setup(x => x.ExistsSamePatientSameDoctorSameDateAsync(existingAppointment.PatientId, dto.DoctorId, dto.ScheduledDate, appointmentId))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(x => x.ExistsSamePatientSameSlotSameDateAsync(existingAppointment.PatientId, dto.ScheduledDate, dto.TimeSlot, appointmentId))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(x => x.ExistsSameDoctorSameSlotSameDateAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot, appointmentId))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            _appointmentRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(appointmentId, dto);

            // Assert
            existingAppointment.DoctorId.Should().Be(dto.DoctorId);
            existingAppointment.ScheduledDate.Should().Be(dto.ScheduledDate);
            existingAppointment.TimeSlot.Should().Be((AppointmentTimeSlot)dto.TimeSlot);

            _appointmentRepositoryMock.Verify(
                x => x.ExistsSamePatientSameDoctorSameDateAsync(existingAppointment.PatientId, dto.DoctorId, dto.ScheduledDate, appointmentId),
                Times.Once);

            _appointmentRepositoryMock.Verify(
                x => x.ExistsSamePatientSameSlotSameDateAsync(existingAppointment.PatientId, dto.ScheduledDate, dto.TimeSlot, appointmentId),
                Times.Once);

            _appointmentRepositoryMock.Verify(
                x => x.ExistsSameDoctorSameSlotSameDateAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot, appointmentId),
                Times.Once);

            _appointmentRepositoryMock.Verify(x => x.UpdateAsync(existingAppointment), Times.Once);
            _appointmentRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            var appointmentId = 61;
            var dto = new UpdateAppointmentDto
            {
                DoctorId = 9,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
                TimeSlot = (int)AppointmentTimeSlot.ThreePM
            };

            var existingAppointment = CreateAppointment(
                appointmentId: appointmentId,
                patientId: 15,
                doctorId: 2,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Pending);

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(appointmentId)).ReturnsAsync(existingAppointment);
            _patientRepositoryMock.Setup(x => x.GetByIdAsync(existingAppointment.PatientId)).ReturnsAsync((Patient?)null);

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(appointmentId, dto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Patient not found*");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            var appointmentId = 62;
            var dto = new UpdateAppointmentDto
            {
                DoctorId = 9,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
                TimeSlot = (int)AppointmentTimeSlot.ThreePM
            };

            var existingAppointment = CreateAppointment(
                appointmentId: appointmentId,
                patientId: 15,
                doctorId: 2,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Pending);

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(appointmentId)).ReturnsAsync(existingAppointment);
            _patientRepositoryMock.Setup(x => x.GetByIdAsync(existingAppointment.PatientId)).ReturnsAsync(CreatePatient(true));
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(dto.DoctorId)).ReturnsAsync((Doctor?)null);

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(appointmentId, dto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Doctor not found*");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenDoctorSlotConflicts()
        {
            // Arrange
            var appointmentId = 63;
            var dto = new UpdateAppointmentDto
            {
                DoctorId = 9,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
                TimeSlot = (int)AppointmentTimeSlot.ThreePM
            };

            var existingAppointment = CreateAppointment(
                appointmentId: appointmentId,
                patientId: 15,
                doctorId: 2,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Pending);

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(appointmentId)).ReturnsAsync(existingAppointment);
            _patientRepositoryMock.Setup(x => x.GetByIdAsync(existingAppointment.PatientId)).ReturnsAsync(CreatePatient(true));
            _doctorRepositoryMock.Setup(x => x.GetByIdAsync(dto.DoctorId)).ReturnsAsync(CreateDoctor(true));

            _appointmentRepositoryMock
                .Setup(x => x.ExistsSamePatientSameDoctorSameDateAsync(existingAppointment.PatientId, dto.DoctorId, dto.ScheduledDate, appointmentId))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(x => x.ExistsSamePatientSameSlotSameDateAsync(existingAppointment.PatientId, dto.ScheduledDate, dto.TimeSlot, appointmentId))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(x => x.ExistsSameDoctorSameSlotSameDateAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot, appointmentId))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(appointmentId, dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Doctor is already booked*");
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrowKeyNotFoundException_WhenAppointmentDoesNotExist()
        {
            // Arrange
            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(100))
                .ReturnsAsync((Appointment?)null);

            // Act
            Func<Task> act = async () => await _service.UpdateStatusAsync(100, dto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Appointment 100 not found*");
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrowArgumentException_WhenStatusIsInvalid()
        {
            // Arrange
            var dto = new UpdateAppointmentStatusDto
            {
                Status = 999
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    appointmentId: 1,
                    patientId: 1,
                    doctorId: 1,
                    scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                    timeSlot: AppointmentTimeSlot.TenAM,
                    status: AppointmentStatus.Pending));

            // Act
            Func<Task> act = async () => await _service.UpdateStatusAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Invalid appointment status*");
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrowInvalidOperationException_WhenCurrentStatusIsCompleted()
        {
            // Arrange
            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Cancelled,
                CancellationReason = "Late"
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    appointmentId: 1,
                    patientId: 1,
                    doctorId: 1,
                    scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                    timeSlot: AppointmentTimeSlot.TenAM,
                    status: AppointmentStatus.Completed));

            // Act
            Func<Task> act = async () => await _service.UpdateStatusAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Completed appointments cannot be modified*");
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrowInvalidOperationException_WhenCurrentStatusIsCancelled()
        {
            // Arrange
            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    appointmentId: 1,
                    patientId: 1,
                    doctorId: 1,
                    scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                    timeSlot: AppointmentTimeSlot.TenAM,
                    status: AppointmentStatus.Cancelled));

            // Act
            Func<Task> act = async () => await _service.UpdateStatusAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Cancelled appointments cannot be modified*");
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrowInvalidOperationException_WhenTryingToSetPending()
        {
            // Arrange
            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Pending
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(CreateAppointment(
                    appointmentId: 1,
                    patientId: 1,
                    doctorId: 1,
                    scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                    timeSlot: AppointmentTimeSlot.TenAM,
                    status: AppointmentStatus.Pending));

            // Act
            Func<Task> act = async () => await _service.UpdateStatusAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*back to Pending*");
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldSetConfirmed_WhenCurrentStatusIsPending()
        {
            // Arrange
            var appointment = CreateAppointment(
                appointmentId: 1,
                patientId: 1,
                doctorId: 1,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Pending);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(appointment);
            _appointmentRepositoryMock.Setup(x => x.UpdateAsync(appointment)).Returns(Task.CompletedTask);
            _appointmentRepositoryMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateStatusAsync(1, dto);

            // Assert
            appointment.Status.Should().Be(AppointmentStatus.Confirmed);
            _appointmentRepositoryMock.Verify(x => x.UpdateAsync(appointment), Times.Once);
            _appointmentRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrowInvalidOperationException_WhenConfirmingNonPendingAppointment()
        {
            // Arrange
            var appointment = CreateAppointment(
                appointmentId: 1,
                patientId: 1,
                doctorId: 1,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Confirmed);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(appointment);

            // Act
            Func<Task> act = async () => await _service.UpdateStatusAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Only pending appointments can be confirmed*");
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldSetCompleted_WhenCurrentStatusIsConfirmed()
        {
            // Arrange
            var appointment = CreateAppointment(
                appointmentId: 1,
                patientId: 1,
                doctorId: 1,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Confirmed);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Completed
            };

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(appointment);
            _appointmentRepositoryMock.Setup(x => x.UpdateAsync(appointment)).Returns(Task.CompletedTask);
            _appointmentRepositoryMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateStatusAsync(1, dto);

            // Assert
            appointment.Status.Should().Be(AppointmentStatus.Completed);
            _appointmentRepositoryMock.Verify(x => x.UpdateAsync(appointment), Times.Once);
            _appointmentRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrowInvalidOperationException_WhenCompletingNonConfirmedAppointment()
        {
            // Arrange
            var appointment = CreateAppointment(
                appointmentId: 1,
                patientId: 1,
                doctorId: 1,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Pending);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Completed
            };

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(appointment);

            // Act
            Func<Task> act = async () => await _service.UpdateStatusAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Only confirmed appointments can be completed*");
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrowArgumentException_WhenCancellingWithoutReason()
        {
            // Arrange
            var appointment = CreateAppointment(
                appointmentId: 1,
                patientId: 1,
                doctorId: 1,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Pending);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Cancelled,
                CancellationReason = "   "
            };

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(appointment);

            // Act
            Func<Task> act = async () => await _service.UpdateStatusAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Cancellation reason is required*");
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldSetCancelledAndTrimReason_WhenReasonIsValid()
        {
            // Arrange
            var appointment = CreateAppointment(
                appointmentId: 1,
                patientId: 1,
                doctorId: 1,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Pending);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Cancelled,
                CancellationReason = "  Out of station  "
            };

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(appointment);
            _appointmentRepositoryMock.Setup(x => x.UpdateAsync(appointment)).Returns(Task.CompletedTask);
            _appointmentRepositoryMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateStatusAsync(1, dto);

            // Assert
            appointment.Status.Should().Be(AppointmentStatus.Cancelled);
            appointment.CancellationReason.Should().Be("Out of station");
            _appointmentRepositoryMock.Verify(x => x.UpdateAsync(appointment), Times.Once);
            _appointmentRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ConfirmAsync_ShouldThrowKeyNotFoundException_WhenAppointmentDoesNotExist()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync((Appointment?)null);

            // Act
            Func<Task> act = async () => await _service.ConfirmAsync(10);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task ConfirmAsync_ShouldThrowInvalidOperationException_WhenAppointmentIsNotPending()
        {
            // Arrange
            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 1,
                doctorId: 2,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Confirmed);

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(appointment);

            // Act
            Func<Task> act = async () => await _service.ConfirmAsync(10);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Only pending appointments can be confirmed*");
        }

        [Fact]
        public async Task ConfirmAsync_ShouldSetConfirmed_WhenAppointmentIsPending()
        {
            // Arrange
            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 1,
                doctorId: 2,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Pending);

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(appointment);
            _appointmentRepositoryMock.Setup(x => x.UpdateAsync(appointment)).Returns(Task.CompletedTask);
            _appointmentRepositoryMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _service.ConfirmAsync(10);

            // Assert
            appointment.Status.Should().Be(AppointmentStatus.Confirmed);
            _appointmentRepositoryMock.Verify(x => x.UpdateAsync(appointment), Times.Once);
            _appointmentRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CompleteAsync_ShouldThrowKeyNotFoundException_WhenAppointmentDoesNotExist()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync((Appointment?)null);

            // Act
            Func<Task> act = async () => await _service.CompleteAsync(10);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task CompleteAsync_ShouldThrowInvalidOperationException_WhenAppointmentIsNotConfirmed()
        {
            // Arrange
            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 1,
                doctorId: 2,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Pending);

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(appointment);

            // Act
            Func<Task> act = async () => await _service.CompleteAsync(10);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Only confirmed appointments can be completed*");
        }

        [Fact]
        public async Task CompleteAsync_ShouldSetCompleted_WhenAppointmentIsConfirmed()
        {
            // Arrange
            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 1,
                doctorId: 2,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Confirmed);

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(appointment);
            _appointmentRepositoryMock.Setup(x => x.UpdateAsync(appointment)).Returns(Task.CompletedTask);
            _appointmentRepositoryMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _service.CompleteAsync(10);

            // Assert
            appointment.Status.Should().Be(AppointmentStatus.Completed);
            _appointmentRepositoryMock.Verify(x => x.UpdateAsync(appointment), Times.Once);
            _appointmentRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CancelAsync_ShouldThrowKeyNotFoundException_WhenAppointmentDoesNotExist()
        {
            // Arrange
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync((Appointment?)null);

            var dto = new CancelAppointmentDto
            {
                CancellationReason = "Emergency"
            };

            // Act
            Func<Task> act = async () => await _service.CancelAsync(10, dto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task CancelAsync_ShouldThrowInvalidOperationException_WhenAppointmentIsCompleted()
        {
            // Arrange
            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 1,
                doctorId: 2,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Completed);

            var dto = new CancelAppointmentDto
            {
                CancellationReason = "Emergency"
            };

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(appointment);

            // Act
            Func<Task> act = async () => await _service.CancelAsync(10, dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Completed appointments cannot be cancelled*");
        }

        [Fact]
        public async Task CancelAsync_ShouldThrowInvalidOperationException_WhenAppointmentAlreadyCancelled()
        {
            // Arrange
            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 1,
                doctorId: 2,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Cancelled);

            var dto = new CancelAppointmentDto
            {
                CancellationReason = "Emergency"
            };

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(appointment);

            // Act
            Func<Task> act = async () => await _service.CancelAsync(10, dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Appointment already cancelled*");
        }

        [Fact]
        public async Task CancelAsync_ShouldThrowArgumentException_WhenReasonIsMissing()
        {
            // Arrange
            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 1,
                doctorId: 2,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Pending);

            var dto = new CancelAppointmentDto
            {
                CancellationReason = "   "
            };

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(appointment);

            // Act
            Func<Task> act = async () => await _service.CancelAsync(10, dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Cancellation reason is required*");
        }

        [Fact]
        public async Task CancelAsync_ShouldSetCancelledAndTrimReason_WhenValid()
        {
            // Arrange
            var appointment = CreateAppointment(
                appointmentId: 10,
                patientId: 1,
                doctorId: 2,
                scheduledDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                timeSlot: AppointmentTimeSlot.TenAM,
                status: AppointmentStatus.Pending);

            var dto = new CancelAppointmentDto
            {
                CancellationReason = "  Patient requested cancellation  "
            };

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(appointment);
            _appointmentRepositoryMock.Setup(x => x.UpdateAsync(appointment)).Returns(Task.CompletedTask);
            _appointmentRepositoryMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _service.CancelAsync(10, dto);

            // Assert
            appointment.Status.Should().Be(AppointmentStatus.Cancelled);
            appointment.CancellationReason.Should().Be("Patient requested cancellation");
            _appointmentRepositoryMock.Verify(x => x.UpdateAsync(appointment), Times.Once);
            _appointmentRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        private static Appointment CreateAppointment(
            int appointmentId,
            int patientId,
            int doctorId,
            DateOnly scheduledDate,
            AppointmentTimeSlot timeSlot,
            AppointmentStatus status,
            string patientFullName = "Test Patient",
            string doctorFullName = "Test Doctor",
            string? cancellationReason = null)
        {
            return new Appointment
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                DoctorId = doctorId,
                ScheduledDate = scheduledDate,
                TimeSlot = timeSlot,
                Status = status,
                CancellationReason = cancellationReason,
                Patient = CreatePatient(true, patientFullName),
                Doctor = CreateDoctor(true, doctorFullName)
            };
        }

        private static Patient CreatePatient(bool isActive, string fullName = "Test Patient")
        {
            var patient = new Patient();

            SetPropertyIfExists(patient, "IsActive", isActive);
            SetFullNameIfPossible(patient, fullName);

            return patient;
        }

        private static Doctor CreateDoctor(bool isActive, string fullName = "Test Doctor")
        {
            var doctor = new Doctor();

            SetPropertyIfExists(doctor, "IsActive", isActive);
            SetFullNameIfPossible(doctor, fullName);

            return doctor;
        }

        private static void SetFullNameIfPossible(object target, string fullName)
        {
            var type = target.GetType();

            var fullNameProperty = type.GetProperty("FullName");
            if (fullNameProperty != null && fullNameProperty.CanWrite)
            {
                fullNameProperty.SetValue(target, fullName);
                return;
            }

            var firstNameProperty = type.GetProperty("FirstName");
            var lastNameProperty = type.GetProperty("LastName");

            if (firstNameProperty != null && firstNameProperty.CanWrite &&
                lastNameProperty != null && lastNameProperty.CanWrite)
            {
                var parts = fullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                var firstName = parts.Length > 0 ? parts[0] : fullName;
                var lastName = parts.Length > 1 ? parts[1] : string.Empty;

                firstNameProperty.SetValue(target, firstName);
                lastNameProperty.SetValue(target, lastName);
            }
        }

        private static void SetPropertyIfExists(object target, string propertyName, object? value)
        {
            var property = target.GetType().GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(target, value);
            }
        }
    }
}