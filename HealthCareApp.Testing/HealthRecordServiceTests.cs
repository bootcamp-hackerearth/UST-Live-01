using AutoMapper;
using FluentAssertions;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services;
using HealthCareApp.Shared.Dtos.HealthRecords;
using HealthCareApp.Shared.Enums;
using Moq;

namespace HealthCareApp.Testing.Services
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> healthRecordRepositoryMock;

        private readonly Mock<IPatientRepository> patientRepositoryMock;

        private readonly Mock<IDoctorRepository> doctorRepositoryMock;

        private readonly Mock<IAppointmentRepository> appointmentRepositoryMock;

        private readonly Mock<IMapper> mapperMock;

        private readonly HealthRecordService healthRecordService;

        public HealthRecordServiceTests()
        {
            healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();

            patientRepositoryMock = new Mock<IPatientRepository>();

            doctorRepositoryMock = new Mock<IDoctorRepository>();

            appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            mapperMock = new Mock<IMapper>();

            SetupMapper();

            healthRecordService = new HealthRecordService(
                healthRecordRepositoryMock.Object,
                patientRepositoryMock.Object,
                doctorRepositoryMock.Object,
                appointmentRepositoryMock.Object,
                mapperMock.Object);
        }

        [Fact]
        public async Task GetAllHealthRecordsAsync_ShouldReturnMappedHealthRecords()
        {
            var healthRecords = GetHealthRecords();

            healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(healthRecords);

            var result = await healthRecordService.GetAllHealthRecordsAsync();

            result.Should().HaveCount(2);

            result[0].Diagnosis.Should().Be("Fever");
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_WhenIdIsInvalid_ShouldThrowHealthRecordRuleException()
        {
            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordByIdAsync(0);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Please provide a valid health record reference.");
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_WhenRecordDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((HealthRecord?)null);

            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordByIdAsync(99);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_WhenRecordExists_ShouldReturnMappedRecord()
        {
            var healthRecord = GetHealthRecords().First();

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(healthRecord.HealthRecordId))
                .ReturnsAsync(healthRecord);

            var result = await healthRecordService.GetHealthRecordByIdAsync(
                healthRecord.HealthRecordId);

            result.HealthRecordId.Should().Be(healthRecord.HealthRecordId);

            result.Diagnosis.Should().Be(healthRecord.Diagnosis);
        }

        [Fact]
        public async Task GetHealthRecordsByPatientIdAsync_WhenPatientExists_ShouldReturnRecords()
        {
            var patient = GetPatient();

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(patient.PatientId))
                .ReturnsAsync(patient);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(
                    patient.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetHealthRecords());

            var result = await healthRecordService.GetHealthRecordsByPatientIdAsync(
                patient.PatientId);

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetHealthRecordsByPatientIdAsync_WhenPatientIdIsInvalid_ShouldThrowHealthRecordRuleException()
        {
            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordsByPatientIdAsync(0);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Please provide a valid patient reference.");
        }

        [Fact]
        public async Task GetHealthRecordsByPatientIdAsync_WhenPatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordsByPatientIdAsync(99);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetHealthRecordsByDoctorIdAsync_WhenDoctorExists_ShouldReturnRecords()
        {
            var doctor = GetDoctor();

            doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(doctor.DoctorId))
                .ReturnsAsync(doctor);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByDoctorIdAsync(
                    doctor.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetHealthRecords());

            var result = await healthRecordService.GetHealthRecordsByDoctorIdAsync(
                doctor.DoctorId);

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetHealthRecordsByDoctorIdAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordsByDoctorIdAsync(99);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetHealthRecordsByAppointmentIdAsync_WhenAppointmentExists_ShouldReturnRecords()
        {
            var appointment = GetConfirmedPastAppointment();

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(appointment.AppointmentId))
                .ReturnsAsync(appointment);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByAppointmentIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetHealthRecords());

            var result = await healthRecordService.GetHealthRecordsByAppointmentIdAsync(
                appointment.AppointmentId);

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenDtoIsNull_ShouldThrowHealthRecordRuleException()
        {
            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(null!);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Health record details are required.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenPatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            var dto = GetValidAddHealthRecordDto();

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenAppointmentDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            var dto = GetValidAddHealthRecordDto();

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenAppointmentDoesNotBelongToPatient_ShouldThrowHealthRecordRuleException()
        {
            var dto = GetValidAddHealthRecordDto();

            var appointment = GetConfirmedPastAppointment();

            appointment.PatientId = 999;

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Appointment does not belong to the selected patient.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            var dto = GetValidAddHealthRecordDto();

            dto.DoctorId = 1;

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(GetConfirmedPastAppointment());

            doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenAppointmentDoctorDoesNotMatchDtoDoctor_ShouldThrowHealthRecordRuleException()
        {
            var dto = GetValidAddHealthRecordDto();

            dto.DoctorId = 99;

            var appointment = GetConfirmedPastAppointment();

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync(new Doctor
                {
                    DoctorId = 99,
                    DoctorName = "Other Doctor",
                    Email = "other@example.com",
                    Specialisation = SpecialisationType.GeneralPractitioner,
                    YearsOfExperience = 3,
                    ConsultationFee = 500,
                    IsActive = true
                });

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Appointment does not belong to the selected doctor.");
        }

        [Theory]
        [InlineData(AppointmentStatus.Cancelled, "Health record cannot be added for a cancelled appointment.")]
        [InlineData(AppointmentStatus.Pending, "Health record cannot be added for a pending appointment.")]
        [InlineData(AppointmentStatus.Completed, "Health record already exists or appointment is already completed.")]
        public async Task AddHealthRecordAsync_WhenAppointmentStatusIsInvalid_ShouldThrowHealthRecordRuleException(
            AppointmentStatus status,
            string expectedMessage)
        {
            var dto = GetValidAddHealthRecordDto();

            var appointment = GetConfirmedPastAppointment();

            appointment.Status = status;

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage(expectedMessage);
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenAppointmentIsFutureDate_ShouldThrowHealthRecordRuleException()
        {
            var dto = GetValidAddHealthRecordDto();

            var appointment = GetConfirmedPastAppointment();

            appointment.ScheduledDate = DateTime.Today.AddDays(1);

            dto.VisitDate = appointment.ScheduledDate;

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Health record cannot be added before the appointment date.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenVisitDateDoesNotMatchAppointmentDate_ShouldThrowHealthRecordRuleException()
        {
            var dto = GetValidAddHealthRecordDto();

            dto.VisitDate = DateTime.Today.AddDays(-2);

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(GetConfirmedPastAppointment());

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Visit date must match the appointment scheduled date.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenRecordAlreadyExistsForAppointment_ShouldThrowConflictException()
        {
            var dto = GetValidAddHealthRecordDto();

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(GetConfirmedPastAppointment());

            healthRecordRepositoryMock
                .Setup(repository => repository.ExistsByAppointmentIdAsync(
                    dto.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Health record already exists for this appointment.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenDiagnosisIsEmpty_ShouldThrowHealthRecordRuleException()
        {
            var dto = GetValidAddHealthRecordDto();

            dto.Diagnosis = "";

            SetupValidAddHealthRecordDependencies(dto);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Diagnosis details are required.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenPrescriptionIsEmpty_ShouldThrowHealthRecordRuleException()
        {
            var dto = GetValidAddHealthRecordDto();

            dto.Prescription = "";

            SetupValidAddHealthRecordDependencies(dto);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Prescription details are required.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenValid_ShouldCreateHealthRecordAndCompleteAppointment()
        {
            var dto = GetValidAddHealthRecordDto();

            var appointment = GetConfirmedPastAppointment();

            SetupValidAddHealthRecordDependencies(dto, appointment);

            healthRecordRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<HealthRecord>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((HealthRecord healthRecord, CancellationToken cancellationToken) =>
                {
                    healthRecord.HealthRecordId = 10;

                    return healthRecord;
                });

            appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    appointment.AppointmentId,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int appointmentId, Appointment updatedAppointment, CancellationToken cancellationToken) =>
                {
                    updatedAppointment.AppointmentId = appointmentId;

                    return updatedAppointment;
                });

            var result = await healthRecordService.AddHealthRecordAsync(dto);

            result.HealthRecordId.Should().Be(10);

            result.PatientId.Should().Be(appointment.PatientId);

            result.DoctorId.Should().Be(appointment.DoctorId);

            appointment.Status.Should().Be(AppointmentStatus.Completed);

            appointmentRepositoryMock.Verify(
                repository => repository.UpdateAsync(
                    appointment.AppointmentId,
                    It.Is<Appointment>(updatedAppointment =>
                        updatedAppointment.Status == AppointmentStatus.Completed),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenIdIsInvalid_ShouldThrowHealthRecordRuleException()
        {
            var dto = GetValidUpdateHealthRecordDto();

            Func<Task> action = async () =>
                await healthRecordService.UpdateHealthRecordAsync(0, dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Please provide a valid health record reference.");
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenDtoIsNull_ShouldThrowHealthRecordRuleException()
        {
            Func<Task> action = async () =>
                await healthRecordService.UpdateHealthRecordAsync(1, null!);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Health record details are required.");
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenHealthRecordDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            var dto = GetValidUpdateHealthRecordDto();

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((HealthRecord?)null);

            Func<Task> action = async () =>
                await healthRecordService.UpdateHealthRecordAsync(99, dto);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenAppointmentDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            var dto = GetValidUpdateHealthRecordDto();

            var healthRecord = GetHealthRecords().First();

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(healthRecord.HealthRecordId))
                .ReturnsAsync(healthRecord);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(healthRecord.AppointmentId))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await healthRecordService.UpdateHealthRecordAsync(healthRecord.HealthRecordId, dto);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenValid_ShouldUpdateRecord()
        {
            var dto = GetValidUpdateHealthRecordDto();

            var healthRecord = GetHealthRecords().First();

            var appointment = GetConfirmedPastAppointment();

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(healthRecord.HealthRecordId))
                .ReturnsAsync(healthRecord);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(healthRecord.AppointmentId))
                .ReturnsAsync(appointment);

            healthRecordRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<HealthRecord>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int healthRecordId, HealthRecord updatedRecord, CancellationToken cancellationToken) =>
                {
                    updatedRecord.HealthRecordId = healthRecordId;

                    return updatedRecord;
                });

            var result = await healthRecordService.UpdateHealthRecordAsync(
                healthRecord.HealthRecordId,
                dto);

            result.HealthRecordId.Should().Be(healthRecord.HealthRecordId);

            result.Diagnosis.Should().Be(dto.Diagnosis);
        }

        [Fact]
        public async Task DeleteHealthRecordAsync_WhenIdIsInvalid_ShouldThrowHealthRecordRuleException()
        {
            Func<Task> action = async () =>
                await healthRecordService.DeleteHealthRecordAsync(0);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Please provide a valid health record reference.");
        }

        [Fact]
        public async Task DeleteHealthRecordAsync_WhenRecordDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            healthRecordRepositoryMock
                .Setup(repository => repository.DeleteAsync(99))
                .ReturnsAsync((HealthRecord?)null);

            Func<Task> action = async () =>
                await healthRecordService.DeleteHealthRecordAsync(99);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task DeleteHealthRecordAsync_WhenRecordExists_ShouldReturnMappedRecord()
        {
            var healthRecord = GetHealthRecords().First();

            healthRecordRepositoryMock
                .Setup(repository => repository.DeleteAsync(healthRecord.HealthRecordId))
                .ReturnsAsync(healthRecord);

            var result = await healthRecordService.DeleteHealthRecordAsync(
                healthRecord.HealthRecordId);

            result.HealthRecordId.Should().Be(healthRecord.HealthRecordId);
        }

        [Fact]
        public async Task GetMyHealthRecordsForPatientAsync_WhenIdentityUserIdIsInvalid_ShouldThrowBusinessRuleException()
        {
            Func<Task> action = async () =>
                await healthRecordService.GetMyHealthRecordsForPatientAsync("");

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Invalid logged-in user.");
        }

        [Fact]
        public async Task GetHealthRecordByIdForPatientAsync_WhenRecordBelongsToPatient_ShouldReturnRecord()
        {
            var patient = GetPatient();

            var healthRecord = GetHealthRecords().First();

            patientRepositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync(
                    "patient-user",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(healthRecord.HealthRecordId))
                .ReturnsAsync(healthRecord);

            var result = await healthRecordService.GetHealthRecordByIdForPatientAsync(
                healthRecord.HealthRecordId,
                "patient-user");

            result.HealthRecordId.Should().Be(healthRecord.HealthRecordId);
        }

        [Fact]
        public async Task GetHealthRecordByIdForPatientAsync_WhenRecordDoesNotBelongToPatient_ShouldThrowForbiddenAccessException()
        {
            var patient = GetPatient();

            var healthRecord = GetHealthRecords().First();

            healthRecord.PatientId = 999;

            patientRepositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync(
                    "patient-user",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(healthRecord.HealthRecordId))
                .ReturnsAsync(healthRecord);

            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordByIdForPatientAsync(
                    healthRecord.HealthRecordId,
                    "patient-user");

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Patients can access only their own health records.");
        }

        [Fact]
        public async Task GetHealthRecordByIdForDoctorAsync_WhenRecordDoesNotBelongToDoctor_ShouldThrowForbiddenAccessException()
        {
            var doctor = GetDoctor();

            var healthRecord = GetHealthRecords().First();

            healthRecord.DoctorId = 999;

            doctorRepositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync(
                    "doctor-user",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(healthRecord.HealthRecordId))
                .ReturnsAsync(healthRecord);

            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordByIdForDoctorAsync(
                    healthRecord.HealthRecordId,
                    "doctor-user");

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Doctors can access only their own health records.");
        }

        [Fact]
        public async Task AddHealthRecordForDoctorAsync_WhenAppointmentDoesNotBelongToDoctor_ShouldThrowForbiddenAccessException()
        {
            var dto = GetValidAddHealthRecordDto();

            var doctor = GetDoctor();

            var appointment = GetConfirmedPastAppointment();

            appointment.DoctorId = 999;

            doctorRepositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync(
                    "doctor-user",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordForDoctorAsync(
                    dto,
                    "doctor-user");

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Doctors can add health records only for their own appointments.");
        }

        [Fact]
        public async Task UpdateHealthRecordForDoctorAsync_WhenRecordDoesNotBelongToDoctor_ShouldThrowForbiddenAccessException()
        {
            var dto = GetValidUpdateHealthRecordDto();

            var doctor = GetDoctor();

            var healthRecord = GetHealthRecords().First();

            healthRecord.DoctorId = 999;

            doctorRepositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync(
                    "doctor-user",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(healthRecord.HealthRecordId))
                .ReturnsAsync(healthRecord);

            Func<Task> action = async () =>
                await healthRecordService.UpdateHealthRecordForDoctorAsync(
                    healthRecord.HealthRecordId,
                    dto,
                    "doctor-user");

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Doctors can update only their own health records.");
        }

        private void SetupValidAddHealthRecordDependencies(
            AddHealthRecordDto dto,
            Appointment? appointment = null)
        {
            appointment ??= GetConfirmedPastAppointment();

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            healthRecordRepositoryMock
                .Setup(repository => repository.ExistsByAppointmentIdAsync(
                    dto.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
        }

        private void SetupMapper()
        {
            mapperMock
                .Setup(mapper => mapper.Map<List<HealthRecordDto>>(It.IsAny<object>()))
                .Returns((object source) =>
                {
                    var records = ((IEnumerable<HealthRecord>)source).ToList();

                    return records.Select(MapToHealthRecordDto).ToList();
                });

            mapperMock
                .Setup(mapper => mapper.Map<HealthRecordDto>(It.IsAny<object>()))
                .Returns((object source) =>
                    MapToHealthRecordDto((HealthRecord)source));

            mapperMock
                .Setup(mapper => mapper.Map<HealthRecord>(It.IsAny<object>()))
                .Returns((object source) =>
                {
                    return source switch
                    {
                        AddHealthRecordDto addDto => new HealthRecord
                        {
                            PatientId = addDto.PatientId,
                            DoctorId = addDto.DoctorId,
                            AppointmentId = addDto.AppointmentId,
                            VisitDate = addDto.VisitDate,
                            Diagnosis = addDto.Diagnosis,
                            Prescription = addDto.Prescription,
                            Notes = addDto.Notes
                        },

                        _ => new HealthRecord
                        {
                            PatientId = 1,
                            DoctorId = 1,
                            AppointmentId = 1,
                            VisitDate = DateTime.Today,
                            Diagnosis = "Default Diagnosis",
                            Prescription = "Default Prescription",
                            Notes = "Default Notes"
                        }
                    };
                });

            mapperMock
                .Setup(mapper => mapper.Map(
                    It.IsAny<UpdateHealthRecordDto>(),
                    It.IsAny<HealthRecord>()))
                .Callback<UpdateHealthRecordDto, HealthRecord>((dto, record) =>
                {
                    record.VisitDate = dto.VisitDate;
                    record.Diagnosis = dto.Diagnosis;
                    record.Prescription = dto.Prescription;
                    record.Notes = dto.Notes;
                })
                .Returns((UpdateHealthRecordDto dto, HealthRecord record) => record);
        }

        private static HealthRecordDto MapToHealthRecordDto(HealthRecord healthRecord)
        {
            return new HealthRecordDto
            {
                HealthRecordId = healthRecord.HealthRecordId,

                PatientId = healthRecord.PatientId,

                DoctorId = healthRecord.DoctorId,

                AppointmentId = healthRecord.AppointmentId,

                VisitDate = healthRecord.VisitDate.ToString("yyyy-MM-dd"),

                Diagnosis = healthRecord.Diagnosis,

                Prescription = healthRecord.Prescription,

                Notes = healthRecord.Notes
            };
        }

        private static List<HealthRecord> GetHealthRecords()
        {
            return new List<HealthRecord>
            {
                new HealthRecord
                {
                    HealthRecordId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    AppointmentId = 1,
                    VisitDate = DateTime.Today.AddDays(-1),
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Rest",
                    CreatedDate = DateTime.Today.AddDays(-1)
                },

                new HealthRecord
                {
                    HealthRecordId = 2,
                    PatientId = 1,
                    DoctorId = 1,
                    AppointmentId = 2,
                    VisitDate = DateTime.Today.AddDays(-2),
                    Diagnosis = "Cold",
                    Prescription = "Steam",
                    Notes = "Hydration",
                    CreatedDate = DateTime.Today.AddDays(-2)
                }
            };
        }

        private static Patient GetPatient()
        {
            return new Patient
            {
                PatientId = 1,
                PatientName = "Rishi Patient",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = GenderType.Male,
                Email = "rishi@example.com",
                PhoneNumber = "9876543210",
                InsuranceID = "INS100",
                IdentityUserId = "patient-user",
                CreatedDate = DateTime.Today.AddYears(-1)
            };
        }

        private static Doctor GetDoctor()
        {
            return new Doctor
            {
                DoctorId = 1,
                DoctorName = "Rishi Doctor",
                Email = "rishi.doctor@example.com",
                Specialisation = SpecialisationType.GeneralPractitioner,
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IdentityUserId = "doctor-user",
                IsActive = true,
                CreatedDate = DateTime.Today.AddYears(-1)
            };
        }

        private static Appointment GetConfirmedPastAppointment()
        {
            return new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(-1),
                TimeSlot = "10:00 AM - 10:30 AM",
                Status = AppointmentStatus.Confirmed,
                CreatedDate = DateTime.Today.AddDays(-2)
            };
        }

        private static AddHealthRecordDto GetValidAddHealthRecordDto()
        {
            return new AddHealthRecordDto
            {
                PatientId = 1,

                DoctorId = null,

                AppointmentId = 1,

                VisitDate = DateTime.Today.AddDays(-1),

                Diagnosis = "Fever",

                Prescription = "Paracetamol",

                Notes = "Take rest"
            };
        }

        private static UpdateHealthRecordDto GetValidUpdateHealthRecordDto()
        {
            return new UpdateHealthRecordDto
            {
                VisitDate = DateTime.Today.AddDays(-1),

                Diagnosis = "Updated Fever",

                Prescription = "Updated Medicine",

                Notes = "Updated Notes"
            };
        }
    }
}