using Moq;
using S3_HealthAxis.Shared.DTOs.Appointment;
using S3_HealthAxis.Shared.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Implementation;
using Xunit;

namespace S3_HealthAxis.Tests.ServiceTests
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
        public async Task GetAllAsync_ShouldReturnMappedAppointmentDetails()
        {
            var appointments = new List<Appointment>
            {
                BuildAppointment(1, 10, 20, AppointmentStatus.Pending),
                BuildAppointment(2, 11, 21, AppointmentStatus.Completed)
            };

            _appointmentRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(appointments);

            var result = (await _service.GetAllAsync()).ToList();

            Assert.Equal(2, result.Count);

            Assert.Equal(1, result[0].AppointmentId);
            Assert.Equal(10, result[0].PatientId);
            Assert.Equal("Patient 10", result[0].PatientName);
            Assert.Equal(20, result[0].DoctorId);
            Assert.Equal("Doctor 20", result[0].DoctorName);
            Assert.Equal((int)AppointmentStatus.Pending, result[0].Status);

            Assert.Equal(2, result[1].AppointmentId);
            Assert.Equal((int)AppointmentStatus.Completed, result[1].Status);

            _appointmentRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoAppointments_ShouldReturnEmptyList()
        {
            _appointmentRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            var result = (await _service.GetAllAsync()).ToList();

            Assert.Empty(result);
            _appointmentRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenAppointmentExists_ShouldReturnMappedAppointmentDetails()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Confirmed);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.AppointmentId);
            Assert.Equal("Patient 10", result.PatientName);
            Assert.Equal("Doctor 20", result.DoctorName);
            Assert.Equal((int)AppointmentStatus.Confirmed, result.Status);

            _appointmentRepositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenAppointmentDoesNotExist_ShouldReturnNull()
        {
            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Appointment?)null);

            var result = await _service.GetByIdAsync(99);

            Assert.Null(result);
            _appointmentRepositoryMock.Verify(r => r.GetByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task GetPatientHistoryAsync_ShouldReturnMappedPatientHistory()
        {
            var appointments = new List<Appointment>
            {
                BuildAppointment(1, 10, 20, AppointmentStatus.Completed),
                BuildAppointment(2, 10, 21, AppointmentStatus.Pending)
            };

            _appointmentRepositoryMock
                .Setup(r => r.GetByPatientIdAsync(10))
                .ReturnsAsync(appointments);

            var result = (await _service.GetPatientHistoryAsync(10)).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].AppointmentId);
            Assert.Equal(20, result[0].DoctorId);
            Assert.Equal("Doctor 20", result[0].DoctorName);
            Assert.Equal((int)AppointmentStatus.Completed, result[0].Status);

            _appointmentRepositoryMock.Verify(r => r.GetByPatientIdAsync(10), Times.Once);
        }

        [Fact]
        public async Task GetDoctorTodayScheduleAsync_ShouldReturnMappedSchedule()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Pending);

            _appointmentRepositoryMock
                .Setup(r => r.GetDoctorTodayScheduleAsync(
                    20,
                    DateOnly.FromDateTime(DateTime.Today)))
                .ReturnsAsync(new List<Appointment> { appointment });

            var result = (await _service.GetDoctorTodayScheduleAsync(20)).ToList();

            Assert.Single(result);
            Assert.Equal(1, result[0].AppointmentId);
            Assert.Equal("Patient 10", result[0].PatientName);
            Assert.False(result[0].HasHealthRecord);
        }

        [Fact]
        public async Task GetDoctorTodayScheduleAsync_WhenHealthRecordExists_ShouldSetHasHealthRecordTrue()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Completed);

            appointment.HealthRecord = new HealthRecord
            {
                HealthRecordId = 100,
                AppointmentId = 1,
                PatientId = 10,
                DoctorId = 20,
                Diagnosis = "Fever",
                Prescription = "Medicine",
                Notes = "Rest"
            };

            _appointmentRepositoryMock
                .Setup(r => r.GetDoctorTodayScheduleAsync(
                    20,
                    DateOnly.FromDateTime(DateTime.Today)))
                .ReturnsAsync(new List<Appointment> { appointment });

            var result = (await _service.GetDoctorTodayScheduleAsync(20)).ToList();

            Assert.Single(result);
            Assert.True(result[0].HasHealthRecord);
        }

        [Fact]
        public async Task GetDoctorWeekScheduleAsync_ShouldReturnMappedSchedule()
        {
            var startDate = DateOnly.FromDateTime(DateTime.Today);
            var endDate = startDate.AddDays(6);

            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Confirmed);

            _appointmentRepositoryMock
                .Setup(r => r.GetDoctorWeekScheduleAsync(20, startDate, endDate))
                .ReturnsAsync(new List<Appointment> { appointment });

            var result = (await _service.GetDoctorWeekScheduleAsync(20, startDate, endDate)).ToList();

            Assert.Single(result);
            Assert.Equal(1, result[0].AppointmentId);
            Assert.Equal((int)AppointmentStatus.Confirmed, result[0].Status);
        }

        [Fact]
        public async Task GetDoctorUpcomingScheduleAsync_ShouldCallWeekScheduleForNextSevenDays()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var endDate = today.AddDays(7);

            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Pending);

            _appointmentRepositoryMock
                .Setup(r => r.GetDoctorWeekScheduleAsync(20, today, endDate))
                .ReturnsAsync(new List<Appointment> { appointment });

            var result = (await _service.GetDoctorUpcomingScheduleAsync(20)).ToList();

            Assert.Single(result);
            _appointmentRepositoryMock.Verify(r => r.GetDoctorWeekScheduleAsync(20, today, endDate), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithValidData_ShouldCreateAppointment()
        {
            var dto = BuildValidCreateDto();

            SetupValidPatientAndDoctor();
            SetupNoCreateConflicts(dto.PatientId, dto.DoctorId, dto.ScheduledDate, dto.TimeSlot);

            Appointment? addedAppointment = null;

            _appointmentRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Appointment>()))
                .Callback<Appointment>(a =>
                {
                    a.AppointmentId = 101;
                    addedAppointment = a;
                })
                .Returns(Task.CompletedTask);

            _appointmentRepositoryMock
                .Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            Assert.Equal(101, result.AppointmentId);
            Assert.Equal(dto.PatientId, result.PatientId);
            Assert.Equal(dto.DoctorId, result.DoctorId);
            Assert.Equal(dto.ScheduledDate, result.ScheduledDate);
            Assert.Equal(dto.TimeSlot, result.TimeSlot);
            Assert.Equal((int)AppointmentStatus.Pending, result.Status);

            Assert.NotNull(addedAppointment);
            Assert.Equal(AppointmentStatus.Pending, addedAppointment.Status);

            _appointmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Appointment>()), Times.Once);
            _appointmentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenPatientNotFound_ShouldThrowKeyNotFoundException()
        {
            var dto = BuildValidCreateDto();

            _patientRepositoryMock
                .Setup(r => r.GetByIdAsync(dto.PatientId))
                .ReturnsAsync((Patient?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CreateAsync(dto));

            Assert.Equal("Patient not found.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenPatientInactive_ShouldThrowInvalidOperationException()
        {
            var dto = BuildValidCreateDto();

            SetupValidPatientAndDoctor(patientActive: false);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));

            Assert.Equal("Inactive patients cannot book appointments.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenDoctorNotFound_ShouldThrowKeyNotFoundException()
        {
            var dto = BuildValidCreateDto();

            _patientRepositoryMock
                .Setup(r => r.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(BuildPatient(dto.PatientId, true));

            _doctorRepositoryMock
                .Setup(r => r.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync((Doctor?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CreateAsync(dto));

            Assert.Equal("Doctor not found.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenDoctorInactive_ShouldThrowInvalidOperationException()
        {
            var dto = BuildValidCreateDto();

            SetupValidPatientAndDoctor(doctorActive: false);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));

            Assert.Equal("Inactive doctor.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenDateIsPast_ShouldThrowArgumentException()
        {
            var dto = BuildValidCreateDto();
            dto.ScheduledDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1);

            SetupValidPatientAndDoctor();

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));

            Assert.Equal("Appointment date cannot be in the past.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenDateIsBeyondNextThirtyDays_ShouldThrowArgumentException()
        {
            var dto = BuildValidCreateDto();
            dto.ScheduledDate = DateOnly.FromDateTime(DateTime.Today).AddDays(31);

            SetupValidPatientAndDoctor();

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));

            Assert.Equal("Appointments can be booked only for the next 30 days.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenTimeSlotInvalid_ShouldThrowArgumentException()
        {
            var dto = BuildValidCreateDto();
            dto.TimeSlot = 999;

            SetupValidPatientAndDoctor();

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));

            Assert.Equal("Invalid appointment slot.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenTodaySlotAlreadyPassed_ShouldThrowInvalidOperationException_WhenPossible()
        {
            var now = TimeOnly.FromDateTime(DateTime.Now);

            if (now <= new TimeOnly(10, 0))
            {
                return;
            }

            var dto = BuildValidCreateDto();
            dto.ScheduledDate = DateOnly.FromDateTime(DateTime.Today);
            dto.TimeSlot = (int)AppointmentTimeSlot.TenAM;

            SetupValidPatientAndDoctor();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));

            Assert.Equal("Selected time slot has already passed.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenPatientAlreadyHasSameDoctorSameDate_ShouldThrowInvalidOperationException()
        {
            var dto = BuildValidCreateDto();

            SetupValidPatientAndDoctor();

            _appointmentRepositoryMock
                .Setup(r => r.ExistsSamePatientSameDoctorSameDateAsync(dto.PatientId, dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));

            Assert.Equal("Patient already has an appointment with this doctor on the selected date.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenPatientAlreadyHasSameSlotSameDate_ShouldThrowInvalidOperationException()
        {
            var dto = BuildValidCreateDto();

            SetupValidPatientAndDoctor();

            _appointmentRepositoryMock
                .Setup(r => r.ExistsSamePatientSameDoctorSameDateAsync(dto.PatientId, dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(r => r.ExistsSamePatientSameSlotSameDateAsync(dto.PatientId, dto.ScheduledDate, dto.TimeSlot))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));

            Assert.Equal("Patient already has another appointment in this time slot.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenDoctorAlreadyBookedSameSlotSameDate_ShouldThrowInvalidOperationException()
        {
            var dto = BuildValidCreateDto();

            SetupValidPatientAndDoctor();

            _appointmentRepositoryMock
                .Setup(r => r.ExistsSamePatientSameDoctorSameDateAsync(dto.PatientId, dto.DoctorId, dto.ScheduledDate))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(r => r.ExistsSamePatientSameSlotSameDateAsync(dto.PatientId, dto.ScheduledDate, dto.TimeSlot))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(r => r.ExistsSameDoctorSameSlotSameDateAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));

            Assert.Equal("Doctor is already booked for this time slot.", exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldUpdateAppointment()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Pending);

            var dto = new UpdateAppointmentDto
            {
                DoctorId = 21,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today).AddDays(2),
                TimeSlot = (int)AppointmentTimeSlot.ElevenAM
            };

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _patientRepositoryMock
                .Setup(r => r.GetByIdAsync(10))
                .ReturnsAsync(BuildPatient(10, true));

            _doctorRepositoryMock
                .Setup(r => r.GetByIdAsync(21))
                .ReturnsAsync(BuildDoctor(21, true));

            SetupNoUpdateConflicts(1, 10, 21, dto.ScheduledDate, dto.TimeSlot);

            await _service.UpdateAsync(1, dto);

            Assert.Equal(21, appointment.DoctorId);
            Assert.Equal(dto.ScheduledDate, appointment.ScheduledDate);
            Assert.Equal(AppointmentTimeSlot.ElevenAM, appointment.TimeSlot);

            _appointmentRepositoryMock.Verify(r => r.UpdateAsync(appointment), Times.Once);
            _appointmentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenAppointmentNotFound_ShouldThrowKeyNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            var dto = new UpdateAppointmentDto
            {
                DoctorId = 20,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
                TimeSlot = (int)AppointmentTimeSlot.TenAM
            };

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(1, dto));

            Assert.Equal("Appointment 1 not found.", exception.Message);
        }

        [Theory]
        [InlineData(AppointmentStatus.Completed, "Completed appointments cannot be modified.")]
        [InlineData(AppointmentStatus.Cancelled, "Cancelled appointments cannot be modified.")]
        public async Task UpdateAsync_WhenCompletedOrCancelled_ShouldThrowInvalidOperationException(
            AppointmentStatus status,
            string expectedMessage)
        {
            var appointment = BuildAppointment(1, 10, 20, status);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentDto
            {
                DoctorId = 20,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
                TimeSlot = (int)AppointmentTimeSlot.TenAM
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(1, dto));

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public async Task ConfirmAsync_WhenPending_ShouldSetStatusToConfirmed()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Pending);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.ConfirmAsync(1);

            Assert.Equal(AppointmentStatus.Confirmed, appointment.Status);
            _appointmentRepositoryMock.Verify(r => r.UpdateAsync(appointment), Times.Once);
            _appointmentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ConfirmAsync_WhenAppointmentNotFound_ShouldThrowKeyNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ConfirmAsync(1));
        }

        [Fact]
        public async Task ConfirmAsync_WhenNotPending_ShouldThrowInvalidOperationException()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Confirmed);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ConfirmAsync(1));

            Assert.Equal("Only pending appointments can be confirmed.", exception.Message);
        }

        [Fact]
        public async Task CompleteAsync_WhenConfirmed_ShouldSetStatusToCompleted()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Confirmed);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.CompleteAsync(1);

            Assert.Equal(AppointmentStatus.Completed, appointment.Status);
            _appointmentRepositoryMock.Verify(r => r.UpdateAsync(appointment), Times.Once);
            _appointmentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CompleteAsync_WhenAppointmentNotFound_ShouldThrowKeyNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CompleteAsync(1));
        }

        [Fact]
        public async Task CompleteAsync_WhenNotConfirmed_ShouldThrowInvalidOperationException()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Pending);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CompleteAsync(1));

            Assert.Equal("Only confirmed appointments can be completed.", exception.Message);
        }

        [Fact]
        public async Task CancelAsync_WhenValid_ShouldCancelAppointmentAndTrimReason()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Pending);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new CancelAppointmentDto
            {
                CancellationReason = " Patient unavailable "
            };

            await _service.CancelAsync(1, dto);

            Assert.Equal(AppointmentStatus.Cancelled, appointment.Status);
            Assert.Equal("Patient unavailable", appointment.CancellationReason);

            _appointmentRepositoryMock.Verify(r => r.UpdateAsync(appointment), Times.Once);
            _appointmentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CancelAsync_WhenAppointmentNotFound_ShouldThrowKeyNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            var dto = new CancelAppointmentDto
            {
                CancellationReason = "Reason"
            };

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CancelAsync(1, dto));
        }

        [Fact]
        public async Task CancelAsync_WhenCompleted_ShouldThrowInvalidOperationException()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Completed);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new CancelAppointmentDto
            {
                CancellationReason = "Reason"
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CancelAsync(1, dto));

            Assert.Equal("Completed appointments cannot be cancelled.", exception.Message);
        }

        [Fact]
        public async Task CancelAsync_WhenAlreadyCancelled_ShouldThrowInvalidOperationException()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Cancelled);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new CancelAppointmentDto
            {
                CancellationReason = "Reason"
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CancelAsync(1, dto));

            Assert.Equal("Appointment already cancelled.", exception.Message);
        }

        [Fact]
        public async Task CancelAsync_WhenReasonIsBlank_ShouldThrowArgumentException()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Pending);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new CancelAppointmentDto
            {
                CancellationReason = " "
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CancelAsync(1, dto));

            Assert.Equal("Cancellation reason is required.", exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenAppointmentNotFound_ShouldThrowKeyNotFoundException()
        {
            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Confirmed
            };

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateStatusAsync(1, dto));
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenInvalidStatus_ShouldThrowArgumentException()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Pending);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = 999
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateStatusAsync(1, dto));

            Assert.Equal("Invalid appointment status.", exception.Message);
        }

        [Theory]
        [InlineData(AppointmentStatus.Completed, "Completed appointments cannot be modified.")]
        [InlineData(AppointmentStatus.Cancelled, "Cancelled appointments cannot be modified.")]
        public async Task UpdateStatusAsync_WhenCurrentAppointmentIsCompletedOrCancelled_ShouldThrowInvalidOperationException(
            AppointmentStatus currentStatus,
            string expectedMessage)
        {
            var appointment = BuildAppointment(1, 10, 20, currentStatus);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Confirmed
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateStatusAsync(1, dto));

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenChangingToPending_ShouldThrowInvalidOperationException()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Confirmed);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Pending
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateStatusAsync(1, dto));

            Assert.Equal("Cannot manually change appointment back to Pending.", exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenPendingToConfirmed_ShouldUpdateStatus()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Pending);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Confirmed
            };

            await _service.UpdateStatusAsync(1, dto);

            Assert.Equal(AppointmentStatus.Confirmed, appointment.Status);
            _appointmentRepositoryMock.Verify(r => r.UpdateAsync(appointment), Times.Once);
            _appointmentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenConfirmedFromNonPending_ShouldThrowInvalidOperationException()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Confirmed);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Confirmed
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateStatusAsync(1, dto));

            Assert.Equal("Only pending appointments can be confirmed.", exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenConfirmedToCompleted_ShouldUpdateStatus()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Confirmed);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Completed
            };

            await _service.UpdateStatusAsync(1, dto);

            Assert.Equal(AppointmentStatus.Completed, appointment.Status);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenCompletedFromNonConfirmed_ShouldThrowInvalidOperationException()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Pending);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Completed
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateStatusAsync(1, dto));

            Assert.Equal("Only confirmed appointments can be completed.", exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenCancelledWithoutReason_ShouldThrowArgumentException()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Pending);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Cancelled,
                CancellationReason = " "
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateStatusAsync(1, dto));

            Assert.Equal("Cancellation reason is required.", exception.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenCancelledWithReason_ShouldCancelAndTrimReason()
        {
            var appointment = BuildAppointment(1, 10, 20, AppointmentStatus.Pending);

            _appointmentRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = (int)AppointmentStatus.Cancelled,
                CancellationReason = " Doctor unavailable "
            };

            await _service.UpdateStatusAsync(1, dto);

            Assert.Equal(AppointmentStatus.Cancelled, appointment.Status);
            Assert.Equal("Doctor unavailable", appointment.CancellationReason);
        }

        [Fact]
        public async Task GetDoctorPatientsAsync_WhenDoctorNotFound_ShouldThrowKeyNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(r => r.GetByIdAsync(20))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetDoctorPatientsAsync(20));
        }

        [Fact]
        public async Task GetDoctorPatientsAsync_ShouldGroupPatientsAndCalculateLatestVisit()
        {
            var doctor = BuildDoctor(20, true);

            var patientOne = BuildPatient(10, true);
            patientOne.FullName = "Charlie Patient";

            var patientTwo = BuildPatient(11, true);
            patientTwo.FullName = "Alice Patient";

            var appointments = new List<Appointment>
            {
                BuildAppointment(
                    1,
                    10,
                    20,
                    AppointmentStatus.Completed,
                    DateOnly.FromDateTime(DateTime.Today).AddDays(-5),
                    AppointmentTimeSlot.TenAM,
                    patientOne,
                    doctor),

                BuildAppointment(
                    2,
                    10,
                    20,
                    AppointmentStatus.Completed,
                    DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
                    AppointmentTimeSlot.ThreePM,
                    patientOne,
                    doctor),

                BuildAppointment(
                    3,
                    11,
                    20,
                    AppointmentStatus.Completed,
                    DateOnly.FromDateTime(DateTime.Today).AddDays(-3),
                    AppointmentTimeSlot.ElevenAM,
                    patientTwo,
                    doctor)
            };

            _doctorRepositoryMock
                .Setup(r => r.GetByIdAsync(20))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(r => r.GetDoctorPatientAppointmentsAsync(20))
                .ReturnsAsync(appointments);

            var result = (await _service.GetDoctorPatientsAsync(20)).ToList();

            Assert.Equal(2, result.Count);

            Assert.Equal("Alice Patient", result[0].FullName);
            Assert.Equal(1, result[0].TotalAppointments);

            Assert.Equal("Charlie Patient", result[1].FullName);
            Assert.Equal(2, result[1].TotalAppointments);
            Assert.Equal(DateOnly.FromDateTime(DateTime.Today).AddDays(-1), result[1].LastVisitDate);
        }

        [Fact]
        public async Task GetDoctorPatientsAsync_WhenAppointmentPatientIsNull_ShouldIgnoreThatAppointment()
        {
            var doctor = BuildDoctor(20, true);

            var patient = BuildPatient(10, true);

            var appointments = new List<Appointment>
            {
                BuildAppointment(1, 10, 20, AppointmentStatus.Completed, patient: patient, doctor: doctor),
                new Appointment
                {
                    AppointmentId = 2,
                    PatientId = 99,
                    Patient = null!,
                    DoctorId = 20,
                    Doctor = doctor,
                    ScheduledDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
                    TimeSlot = AppointmentTimeSlot.TenAM,
                    Status = AppointmentStatus.Completed
                }
            };

            _doctorRepositoryMock
                .Setup(r => r.GetByIdAsync(20))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(r => r.GetDoctorPatientAppointmentsAsync(20))
                .ReturnsAsync(appointments);

            var result = (await _service.GetDoctorPatientsAsync(20)).ToList();

            Assert.Single(result);
            Assert.Equal(10, result[0].PatientId);
        }

        private static CreateAppointmentDto BuildValidCreateDto()
        {
            return new CreateAppointmentDto
            {
                PatientId = 10,
                DoctorId = 20,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
                TimeSlot = (int)AppointmentTimeSlot.TenAM
            };
        }

        private void SetupValidPatientAndDoctor(
            bool patientActive = true,
            bool doctorActive = true)
        {
            _patientRepositoryMock
                .Setup(r => r.GetByIdAsync(10))
                .ReturnsAsync(BuildPatient(10, patientActive));

            _doctorRepositoryMock
                .Setup(r => r.GetByIdAsync(20))
                .ReturnsAsync(BuildDoctor(20, doctorActive));
        }

        private void SetupNoCreateConflicts(
            int patientId,
            int doctorId,
            DateOnly date,
            int timeSlot)
        {
            _appointmentRepositoryMock
                .Setup(r => r.ExistsSamePatientSameDoctorSameDateAsync(patientId, doctorId, date))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(r => r.ExistsSamePatientSameSlotSameDateAsync(patientId, date, timeSlot))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(r => r.ExistsSameDoctorSameSlotSameDateAsync(doctorId, date, timeSlot))
                .ReturnsAsync(false);
        }

        private void SetupNoUpdateConflicts(
            int appointmentId,
            int patientId,
            int doctorId,
            DateOnly date,
            int timeSlot)
        {
            _appointmentRepositoryMock
                .Setup(r => r.ExistsSamePatientSameDoctorSameDateAsync(patientId, doctorId, date, appointmentId))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(r => r.ExistsSamePatientSameSlotSameDateAsync(patientId, date, timeSlot, appointmentId))
                .ReturnsAsync(false);

            _appointmentRepositoryMock
                .Setup(r => r.ExistsSameDoctorSameSlotSameDateAsync(doctorId, date, timeSlot, appointmentId))
                .ReturnsAsync(false);
        }

        private static Patient BuildPatient(int id, bool isActive)
        {
            return new Patient
            {
                PatientId = id,
                FullName = $"Patient {id}",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = $"patient{id}@test.com",
                InsuranceNumber = $"INS{id}",
                IsActive = isActive
            };
        }

        private static Doctor BuildDoctor(int id, bool isActive)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = $"Doctor {id}",
                Email = $"doctor{id}@test.com",
                Specialisation = (DoctorSpecialisation)1,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = isActive
            };
        }

        private static Appointment BuildAppointment(
            int appointmentId,
            int patientId,
            int doctorId,
            AppointmentStatus status,
            DateOnly? date = null,
            AppointmentTimeSlot slot = AppointmentTimeSlot.TenAM,
            Patient? patient = null,
            Doctor? doctor = null)
        {
            var actualPatient = patient ?? BuildPatient(patientId, true);
            var actualDoctor = doctor ?? BuildDoctor(doctorId, true);

            return new Appointment
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                Patient = actualPatient,
                DoctorId = doctorId,
                Doctor = actualDoctor,
                ScheduledDate = date ?? DateOnly.FromDateTime(DateTime.Today).AddDays(1),
                TimeSlot = slot,
                Status = status
            };
        }
    }
}