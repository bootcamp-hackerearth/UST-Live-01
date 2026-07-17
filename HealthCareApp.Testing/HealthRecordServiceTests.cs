using AutoMapper;
using FluentAssertions;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services;
using HealthCareApp.Shared.Dtos.HealthRecords;
using HealthCareApp.Shared.Dtos.Pagination;
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
            healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetHealthRecords());

            var result = await healthRecordService.GetAllHealthRecordsAsync();

            result.Should().HaveCount(4);
            result.First().HealthRecordId.Should().Be(1);
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_WhenIdInvalid_ShouldThrowHealthRecordRuleException()
        {
            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordByIdAsync(0);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Please provide a valid health record reference.");
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_WhenNotFound_ShouldThrowEntityNotFoundException()
        {
            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HealthRecord?)null);

            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordByIdAsync(99);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetHealthRecordByIdAsync_WhenExists_ShouldReturnMappedHealthRecord()
        {
            var healthRecord = GetHealthRecords().First();

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecord);

            var result = await healthRecordService.GetHealthRecordByIdAsync(healthRecord.HealthRecordId);

            result.HealthRecordId.Should().Be(healthRecord.HealthRecordId);
        }

        [Fact]
        public async Task GetHealthRecordsByPatientIdAsync_WhenPatientInvalid_ShouldThrow()
        {
            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordsByPatientIdAsync(0);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Please provide a valid patient reference.");
        }

        [Fact]
        public async Task GetHealthRecordsByPatientIdAsync_WhenPatientNotFound_ShouldThrow()
        {
            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordsByPatientIdAsync(99);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetHealthRecordsByPatientIdAsync_WhenPatientExists_ShouldReturnMappedRecords()
        {
            SetupPatientExists(1);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetHealthRecords()
                    .Where(record => record.PatientId == 1)
                    .ToList());

            var result = await healthRecordService.GetHealthRecordsByPatientIdAsync(1);

            result.Should().OnlyContain(record => record.PatientId == 1);
        }

        [Fact]
        public async Task GetHealthRecordsByDoctorIdAsync_WhenDoctorInvalid_ShouldThrow()
        {
            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordsByDoctorIdAsync(0);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Please provide a valid doctor reference.");
        }

        [Fact]
        public async Task GetHealthRecordsByDoctorIdAsync_WhenDoctorNotFound_ShouldThrow()
        {
            doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordsByDoctorIdAsync(99);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetHealthRecordsByDoctorIdAsync_WhenDoctorExists_ShouldReturnMappedRecords()
        {
            SetupDoctorExists(1);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByDoctorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetHealthRecords()
                    .Where(record => record.DoctorId == 1)
                    .ToList());

            var result = await healthRecordService.GetHealthRecordsByDoctorIdAsync(1);

            result.Should().OnlyContain(record => record.DoctorId == 1);
        }

        [Fact]
        public async Task GetHealthRecordsByAppointmentIdAsync_WhenAppointmentInvalid_ShouldThrow()
        {
            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordsByAppointmentIdAsync(0);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Please provide a valid appointment reference.");
        }

        [Fact]
        public async Task GetHealthRecordsByAppointmentIdAsync_WhenAppointmentNotFound_ShouldThrow()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordsByAppointmentIdAsync(99);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetHealthRecordsByAppointmentIdAsync_WhenAppointmentExists_ShouldReturnMappedRecords()
        {
            var appointment = GetAppointments().First();

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByAppointmentIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetHealthRecords()
                    .Where(record => record.AppointmentId == appointment.AppointmentId)
                    .ToList());

            var result = await healthRecordService.GetHealthRecordsByAppointmentIdAsync(
                appointment.AppointmentId);

            result.Should().OnlyContain(record => record.AppointmentId == appointment.AppointmentId);
        }

        [Fact]
        public async Task GetMyHealthRecordsForPatientAsync_WhenIdentityEmpty_ShouldThrow()
        {
            Func<Task> action = async () =>
                await healthRecordService.GetMyHealthRecordsForPatientAsync("");

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Invalid logged-in user.");
        }

        [Fact]
        public async Task GetMyHealthRecordsForPatientAsync_WhenPatientNotFound_ShouldThrow()
        {
            patientRepositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync(
                    "missing-patient",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () =>
                await healthRecordService.GetMyHealthRecordsForPatientAsync("missing-patient");

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetMyHealthRecordsForPatientAsync_WhenPatientExists_ShouldReturnOwnRecords()
        {
            var patient = GetPatients().First();

            SetupLoggedInPatient(patient);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(
                    patient.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetHealthRecords()
                    .Where(record => record.PatientId == patient.PatientId)
                    .ToList());

            var result = await healthRecordService.GetMyHealthRecordsForPatientAsync(
                patient.IdentityUserId!);

            result.Should().OnlyContain(record => record.PatientId == patient.PatientId);
        }

        [Fact]
        public async Task GetMyHealthRecordsForDoctorAsync_WhenIdentityEmpty_ShouldThrow()
        {
            Func<Task> action = async () =>
                await healthRecordService.GetMyHealthRecordsForDoctorAsync("");

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Invalid logged-in user.");
        }

        [Fact]
        public async Task GetMyHealthRecordsForDoctorAsync_WhenDoctorNotFound_ShouldThrow()
        {
            doctorRepositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync(
                    "missing-doctor",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await healthRecordService.GetMyHealthRecordsForDoctorAsync("missing-doctor");

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetMyHealthRecordsForDoctorAsync_WhenDoctorExists_ShouldReturnOwnRecords()
        {
            var doctor = GetDoctors().First();

            SetupLoggedInDoctor(doctor);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByDoctorIdAsync(
                    doctor.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetHealthRecords()
                    .Where(record => record.DoctorId == doctor.DoctorId)
                    .ToList());

            var result = await healthRecordService.GetMyHealthRecordsForDoctorAsync(
                doctor.IdentityUserId!);

            result.Should().OnlyContain(record => record.DoctorId == doctor.DoctorId);
        }

        [Fact]
        public async Task GetMyHealthRecordsForPatientPagedAsync_WhenQueryNull_ShouldReturnPagedRecords()
        {
            var patient = GetPatients().First();

            SetupLoggedInPatient(patient);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(
                    patient.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetHealthRecords()
                    .Where(record => record.PatientId == patient.PatientId)
                    .ToList());

            var result = await healthRecordService.GetMyHealthRecordsForPatientPagedAsync(
                patient.IdentityUserId!,
                null!);

            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.Items.Should().OnlyContain(record => record.PatientId == patient.PatientId);
        }

        [Fact]
        public async Task GetMyHealthRecordsForPatientPagedAsync_ShouldFilterSearchDateAndPageSize()
        {
            var patient = GetPatients().First();

            SetupLoggedInPatient(patient);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(
                    patient.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetHealthRecords()
                    .Where(record => record.PatientId == patient.PatientId)
                    .ToList());

            var query = new HealthRecordPaginationQueryDto
            {
                PageNumber = 0,
                PageSize = 500,
                SearchTerm = "Fever",
                VisitDate = DateTime.Today.AddDays(-1)
            };

            var result = await healthRecordService.GetMyHealthRecordsForPatientPagedAsync(
                patient.IdentityUserId!,
                query);

            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(100);
            result.Items.Should().OnlyContain(record => record.Diagnosis.Contains("Fever"));
        }

        [Fact]
        public async Task GetMyHealthRecordsForDoctorPagedAsync_WhenQueryNull_ShouldReturnPagedRecords()
        {
            var doctor = GetDoctors().First();

            SetupLoggedInDoctor(doctor);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByDoctorIdAsync(
                    doctor.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetHealthRecords()
                    .Where(record => record.DoctorId == doctor.DoctorId)
                    .ToList());

            var result = await healthRecordService.GetMyHealthRecordsForDoctorPagedAsync(
                doctor.IdentityUserId!,
                null!);

            result.PageNumber.Should().Be(1);
            result.Items.Should().OnlyContain(record => record.DoctorId == doctor.DoctorId);
        }

        [Theory]
        [InlineData("Rishi Patient")]
        [InlineData("Rishi Doctor")]
        [InlineData("Fever")]
        [InlineData("Paracetamol")]
        [InlineData("1")]
        public async Task GetMyHealthRecordsForDoctorPagedAsync_ShouldFilterBySearchTerm(string searchTerm)
        {
            var doctor = GetDoctors().First();

            SetupLoggedInDoctor(doctor);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByDoctorIdAsync(
                    doctor.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetHealthRecords()
                    .Where(record => record.DoctorId == doctor.DoctorId)
                    .ToList());

            var query = new HealthRecordPaginationQueryDto
            {
                SearchTerm = searchTerm
            };

            var result = await healthRecordService.GetMyHealthRecordsForDoctorPagedAsync(
                doctor.IdentityUserId!,
                query);

            result.TotalRecords.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetHealthRecordByIdForPatientAsync_WhenOwner_ShouldReturnRecord()
        {
            var patient = GetPatients().First();
            var healthRecord = GetHealthRecords().First();

            SetupLoggedInPatient(patient);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecord);

            var result = await healthRecordService.GetHealthRecordByIdForPatientAsync(
                healthRecord.HealthRecordId,
                patient.IdentityUserId!);

            result.HealthRecordId.Should().Be(healthRecord.HealthRecordId);
        }

        [Fact]
        public async Task GetHealthRecordByIdForPatientAsync_WhenRecordNotFound_ShouldThrow()
        {
            var patient = GetPatients().First();

            SetupLoggedInPatient(patient);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HealthRecord?)null);

            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordByIdForPatientAsync(99, patient.IdentityUserId!);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetHealthRecordByIdForPatientAsync_WhenNotOwner_ShouldThrowForbidden()
        {
            var patient = GetPatients().First();
            var healthRecord = GetHealthRecords().First();

            healthRecord.PatientId = 999;

            SetupLoggedInPatient(patient);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecord);

            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordByIdForPatientAsync(
                    healthRecord.HealthRecordId,
                    patient.IdentityUserId!);

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Patients can access only their own health records.");
        }

        [Fact]
        public async Task GetHealthRecordByIdForDoctorAsync_WhenOwner_ShouldReturnRecord()
        {
            var doctor = GetDoctors().First();
            var healthRecord = GetHealthRecords().First();

            SetupLoggedInDoctor(doctor);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecord);

            var result = await healthRecordService.GetHealthRecordByIdForDoctorAsync(
                healthRecord.HealthRecordId,
                doctor.IdentityUserId!);

            result.HealthRecordId.Should().Be(healthRecord.HealthRecordId);
        }

        [Fact]
        public async Task GetHealthRecordByIdForDoctorAsync_WhenRecordNotFound_ShouldThrow()
        {
            var doctor = GetDoctors().First();

            SetupLoggedInDoctor(doctor);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HealthRecord?)null);

            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordByIdForDoctorAsync(99, doctor.IdentityUserId!);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetHealthRecordByIdForDoctorAsync_WhenNotOwner_ShouldThrowForbidden()
        {
            var doctor = GetDoctors().First();
            var healthRecord = GetHealthRecords().First();

            healthRecord.DoctorId = 999;

            SetupLoggedInDoctor(doctor);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecord);

            Func<Task> action = async () =>
                await healthRecordService.GetHealthRecordByIdForDoctorAsync(
                    healthRecord.HealthRecordId,
                    doctor.IdentityUserId!);

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Doctors can access only their own health records.");
        }

        [Fact]
        public async Task GetHealthRecordsByAppointmentIdForDoctorAsync_WhenAppointmentNotOwner_ShouldThrowForbidden()
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
                await healthRecordService.GetHealthRecordsByAppointmentIdForDoctorAsync(
                    appointment.AppointmentId,
                    doctor.IdentityUserId!);

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Doctors can access health records only for their own appointments.");
        }

        [Fact]
        public async Task GetHealthRecordsByAppointmentIdForDoctorAsync_WhenOwner_ShouldReturnRecords()
        {
            var doctor = GetDoctors().First();
            var appointment = GetAppointments().First();

            SetupLoggedInDoctor(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByAppointmentIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetHealthRecords()
                    .Where(record => record.AppointmentId == appointment.AppointmentId)
                    .ToList());

            var result = await healthRecordService.GetHealthRecordsByAppointmentIdForDoctorAsync(
                appointment.AppointmentId,
                doctor.IdentityUserId!);

            result.Should().OnlyContain(record => record.AppointmentId == appointment.AppointmentId);
        }

        [Fact]
        public async Task GetPatientHealthRecordsForTreatingDoctorAsync_WhenDoctorDoesNotTreatPatient_ShouldThrow()
        {
            var doctor = GetDoctors().First();

            SetupLoggedInDoctor(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByDoctorIdAsync(
                    doctor.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Appointment>());

            Func<Task> action = async () =>
                await healthRecordService.GetPatientHealthRecordsForTreatingDoctorAsync(
                    1,
                    doctor.IdentityUserId!);

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Doctors can view health records only for patients they are treating or have treated.");
        }

        [Fact]
        public async Task GetPatientHealthRecordsForTreatingDoctorAsync_WhenDoctorTreatsPatient_ShouldReturnRecords()
        {
            var doctor = GetDoctors().First();
            var patient = GetPatients().First();

            SetupLoggedInDoctor(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByDoctorIdAsync(
                    doctor.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment =>
                        appointment.DoctorId == doctor.DoctorId &&
                        appointment.PatientId == patient.PatientId &&
                        appointment.Status == AppointmentStatus.Confirmed)
                    .ToList());

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(
                    patient.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetHealthRecords()
                    .Where(record => record.PatientId == patient.PatientId)
                    .ToList());

            var result = await healthRecordService.GetPatientHealthRecordsForTreatingDoctorAsync(
                patient.PatientId,
                doctor.IdentityUserId!);

            result.Should().OnlyContain(record => record.PatientId == patient.PatientId);
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenDtoNull_ShouldThrow()
        {
            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(null!);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Health record details are required.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenPatientInvalid_ShouldThrow()
        {
            var dto = GetValidAddHealthRecordDto();
            dto.PatientId = 0;

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Please provide a valid patient reference.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenAppointmentInvalid_ShouldThrow()
        {
            var dto = GetValidAddHealthRecordDto();
            dto.AppointmentId = 0;

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Please provide a valid appointment reference.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenPatientNotFound_ShouldThrow()
        {
            var dto = GetValidAddHealthRecordDto();

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenAppointmentNotFound_ShouldThrow()
        {
            var dto = GetValidAddHealthRecordDto();

            SetupPatientExists(dto.PatientId);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.AppointmentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenAppointmentDoesNotBelongToPatient_ShouldThrow()
        {
            var dto = GetValidAddHealthRecordDto();
            var appointment = GetConfirmedAppointmentForToday();

            appointment.PatientId = 999;

            SetupPatientExists(dto.PatientId);
            SetupAppointment(appointment);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Appointment does not belong to the selected patient.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenDoctorIdInvalid_ShouldThrow()
        {
            var dto = GetValidAddHealthRecordDto();
            dto.DoctorId = 0;

            var appointment = GetConfirmedAppointmentForToday();

            SetupPatientExists(dto.PatientId);
            SetupAppointment(appointment);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Please provide a valid doctor reference.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenDoctorNotFound_ShouldThrow()
        {
            var dto = GetValidAddHealthRecordDto();
            var appointment = GetConfirmedAppointmentForToday();

            SetupPatientExists(dto.PatientId);
            SetupAppointment(appointment);

            doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.DoctorId!.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenAppointmentDoesNotBelongToDoctor_ShouldThrow()
        {
            var dto = GetValidAddHealthRecordDto();
            dto.DoctorId = 2;

            var appointment = GetConfirmedAppointmentForToday();
            appointment.DoctorId = 1;

            SetupPatientExists(dto.PatientId);
            SetupAppointment(appointment);
            SetupDoctorExists(2);

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
        public async Task AddHealthRecordAsync_WhenAppointmentStatusInvalid_ShouldThrow(
            AppointmentStatus status,
            string expectedMessage)
        {
            var dto = GetValidAddHealthRecordDto();
            var appointment = GetConfirmedAppointmentForToday();

            appointment.Status = status;

            SetupPatientExists(dto.PatientId);
            SetupAppointment(appointment);
            SetupDoctorExists(dto.DoctorId!.Value);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage(expectedMessage);
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenAppointmentDateFuture_ShouldThrow()
        {
            var dto = GetValidAddHealthRecordDto();
            var appointment = GetConfirmedAppointmentForToday();

            appointment.ScheduledDate = DateTime.Today.AddDays(1);
            dto.VisitDate = appointment.ScheduledDate;

            SetupPatientExists(dto.PatientId);
            SetupAppointment(appointment);
            SetupDoctorExists(dto.DoctorId!.Value);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Health record cannot be added before the appointment date.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenVisitDateMismatch_ShouldThrow()
        {
            var dto = GetValidAddHealthRecordDto();
            dto.VisitDate = DateTime.Today.AddDays(-2);

            var appointment = GetConfirmedAppointmentForToday();

            SetupPatientExists(dto.PatientId);
            SetupAppointment(appointment);
            SetupDoctorExists(dto.DoctorId!.Value);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Visit date must match the appointment scheduled date.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenHealthRecordAlreadyExists_ShouldThrow()
        {
            var dto = GetValidAddHealthRecordDto();
            var appointment = GetConfirmedAppointmentForToday();

            SetupPatientExists(dto.PatientId);
            SetupAppointment(appointment);
            SetupDoctorExists(dto.DoctorId!.Value);

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

        [Theory]
        [InlineData("", "Prescription", null, "Diagnosis details are required.")]
        [InlineData("Diagnosis", "", null, "Prescription details are required.")]
        public async Task AddHealthRecordAsync_WhenRequiredTextInvalid_ShouldThrow(
            string diagnosis,
            string prescription,
            string? notes,
            string expectedMessage)
        {
            var dto = GetValidAddHealthRecordDto();

            dto.Diagnosis = diagnosis;
            dto.Prescription = prescription;
            dto.Notes = notes ?? string.Empty;

            var appointment = GetConfirmedAppointmentForToday();

            SetupPatientExists(dto.PatientId);
            SetupAppointment(appointment);
            SetupDoctorExists(dto.DoctorId!.Value);
            SetupHealthRecordDoesNotExist(dto.AppointmentId);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage(expectedMessage);
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenDiagnosisTooLong_ShouldThrow()
        {
            var dto = GetValidAddHealthRecordDto();
            dto.Diagnosis = new string('A', 501);

            SetupValidAddHealthRecordDependencies(dto);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Diagnosis details must not exceed 500 characters.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenPrescriptionTooLong_ShouldThrow()
        {
            var dto = GetValidAddHealthRecordDto();
            dto.Prescription = new string('A', 501);

            SetupValidAddHealthRecordDependencies(dto);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Prescription details must not exceed 500 characters.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenNotesTooLong_ShouldThrow()
        {
            var dto = GetValidAddHealthRecordDto();
            dto.Notes = new string('A', 1001);

            SetupValidAddHealthRecordDependencies(dto);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordAsync(dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Additional notes must not exceed 1000 characters.");
        }

        [Fact]
        public async Task AddHealthRecordAsync_WhenValid_ShouldCreateRecordAndCompleteAppointment()
        {
            var dto = GetValidAddHealthRecordDto();

            SetupValidAddHealthRecordDependencies(dto);

            healthRecordRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<HealthRecord>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((HealthRecord healthRecord, CancellationToken ct) =>
                {
                    healthRecord.HealthRecordId = 100;
                    return healthRecord;
                });

            appointmentRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    dto.AppointmentId,
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, Appointment appointment, CancellationToken ct) => appointment);

            var result = await healthRecordService.AddHealthRecordAsync(dto);

            result.HealthRecordId.Should().Be(100);

            appointmentRepositoryMock.Verify(
                repository => repository.UpdateAsync(
                    dto.AppointmentId,
                    It.Is<Appointment>(appointment => appointment.Status == AppointmentStatus.Completed),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task AddHealthRecordForDoctorAsync_WhenDtoNull_ShouldThrow()
        {
            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordForDoctorAsync(null!, "doctor-identity");

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Health record details are required.");
        }

        [Fact]
        public async Task AddHealthRecordForDoctorAsync_WhenAppointmentNotOwn_ShouldThrow()
        {
            var doctor = GetDoctors().First();
            var dto = GetValidAddHealthRecordDto();
            var appointment = GetConfirmedAppointmentForToday();

            appointment.DoctorId = 999;

            SetupLoggedInDoctor(doctor);
            SetupAppointment(appointment);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordForDoctorAsync(dto, doctor.IdentityUserId!);

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Doctors can add health records only for their own appointments.");
        }

        [Fact]
        public async Task AddHealthRecordForDoctorAsync_WhenPatientMismatch_ShouldThrow()
        {
            var doctor = GetDoctors().First();
            var dto = GetValidAddHealthRecordDto();
            dto.PatientId = 999;

            var appointment = GetConfirmedAppointmentForToday();

            SetupLoggedInDoctor(doctor);
            SetupAppointment(appointment);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordForDoctorAsync(dto, doctor.IdentityUserId!);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Health record patient must match the appointment patient.");
        }

        [Fact]
        public async Task AddHealthRecordForDoctorAsync_WhenDtoDoctorMismatch_ShouldThrow()
        {
            var doctor = GetDoctors().First();
            var dto = GetValidAddHealthRecordDto();
            dto.DoctorId = 999;

            var appointment = GetConfirmedAppointmentForToday();

            SetupLoggedInDoctor(doctor);
            SetupAppointment(appointment);

            Func<Task> action = async () =>
                await healthRecordService.AddHealthRecordForDoctorAsync(dto, doctor.IdentityUserId!);

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Health record doctor must match the logged-in doctor.");
        }

      
        [Fact]
        public async Task UpdateHealthRecordAsync_WhenIdInvalid_ShouldThrow()
        {
            var dto = GetValidUpdateHealthRecordDto();

            Func<Task> action = async () =>
                await healthRecordService.UpdateHealthRecordAsync(0, dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Please provide a valid health record reference.");
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenDtoNull_ShouldThrow()
        {
            Func<Task> action = async () =>
                await healthRecordService.UpdateHealthRecordAsync(1, null!);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Health record details are required.");
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenHealthRecordNotFound_ShouldThrow()
        {
            var dto = GetValidUpdateHealthRecordDto();

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HealthRecord?)null);

            Func<Task> action = async () =>
                await healthRecordService.UpdateHealthRecordAsync(99, dto);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenAppointmentNotFound_ShouldThrow()
        {
            var dto = GetValidUpdateHealthRecordDto();
            var healthRecord = GetHealthRecords().First();

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecord);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    healthRecord.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await healthRecordService.UpdateHealthRecordAsync(healthRecord.HealthRecordId, dto);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenAppointmentDateFuture_ShouldThrow()
        {
            var dto = GetValidUpdateHealthRecordDto();
            var healthRecord = GetHealthRecords().First();
            var appointment = GetConfirmedAppointmentForToday();

            appointment.ScheduledDate = DateTime.Today.AddDays(1);
            dto.VisitDate = appointment.ScheduledDate;

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecord);

            SetupAppointment(appointment);

            Func<Task> action = async () =>
                await healthRecordService.UpdateHealthRecordAsync(healthRecord.HealthRecordId, dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Health record cannot be updated before the appointment date.");
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenVisitDateMismatch_ShouldThrow()
        {
            var dto = GetValidUpdateHealthRecordDto();
            dto.VisitDate = DateTime.Today.AddDays(-2);

            var healthRecord = GetHealthRecords().First();
            var appointment = GetConfirmedAppointmentForToday();

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecord);

            SetupAppointment(appointment);

            Func<Task> action = async () =>
                await healthRecordService.UpdateHealthRecordAsync(healthRecord.HealthRecordId, dto);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Visit date must match the appointment scheduled date.");
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenRepositoryReturnsNull_ShouldThrow()
        {
            var dto = GetValidUpdateHealthRecordDto();
            var healthRecord = GetHealthRecords().First();
            var appointment = GetConfirmedAppointmentForToday();

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecord);

            SetupAppointment(appointment);

            healthRecordRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<HealthRecord>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((HealthRecord?)null);

            Func<Task> action = async () =>
                await healthRecordService.UpdateHealthRecordAsync(healthRecord.HealthRecordId, dto);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenValid_ShouldUpdateRecord()
        {
            var dto = GetValidUpdateHealthRecordDto();
            var healthRecord = GetHealthRecords().First();
            var appointment = GetConfirmedAppointmentForToday();

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecord);

            SetupAppointment(appointment);

            healthRecordRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<HealthRecord>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, HealthRecord record, CancellationToken ct) =>
                {
                    record.HealthRecordId = id;
                    return record;
                });

            var result = await healthRecordService.UpdateHealthRecordAsync(healthRecord.HealthRecordId, dto);

            result.HealthRecordId.Should().Be(healthRecord.HealthRecordId);
            result.Diagnosis.Should().Be(dto.Diagnosis);
        }

        [Fact]
        public async Task UpdateHealthRecordForDoctorAsync_WhenDtoNull_ShouldThrow()
        {
            Func<Task> action = async () =>
                await healthRecordService.UpdateHealthRecordForDoctorAsync(1, null!, "doctor-identity");

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Health record details are required.");
        }

        [Fact]
        public async Task UpdateHealthRecordForDoctorAsync_WhenNotOwner_ShouldThrowForbidden()
        {
            var doctor = GetDoctors().First();
            var healthRecord = GetHealthRecords().First();

            healthRecord.DoctorId = 999;

            SetupLoggedInDoctor(doctor);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecord);

            Func<Task> action = async () =>
                await healthRecordService.UpdateHealthRecordForDoctorAsync(
                    healthRecord.HealthRecordId,
                    GetValidUpdateHealthRecordDto(),
                    doctor.IdentityUserId!);

            await action.Should()
                .ThrowAsync<ForbiddenAccessException>()
                .WithMessage("Doctors can update only their own health records.");
        }

        [Fact]
        public async Task UpdateHealthRecordForDoctorAsync_WhenOwner_ShouldUpdate()
        {
            var doctor = GetDoctors().First();
            var healthRecord = GetHealthRecords().First();
            var appointment = GetConfirmedAppointmentForToday();
            var dto = GetValidUpdateHealthRecordDto();

            healthRecord.DoctorId = doctor.DoctorId;

            SetupLoggedInDoctor(doctor);

            healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecord);

            SetupAppointment(appointment);

            healthRecordRepositoryMock
                .Setup(repository => repository.UpdateAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<HealthRecord>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, HealthRecord record, CancellationToken ct) =>
                {
                    record.HealthRecordId = id;
                    return record;
                });

            var result = await healthRecordService.UpdateHealthRecordForDoctorAsync(
                healthRecord.HealthRecordId,
                dto,
                doctor.IdentityUserId!);

            result.HealthRecordId.Should().Be(healthRecord.HealthRecordId);
        }

        [Fact]
        public async Task DeleteHealthRecordAsync_WhenIdInvalid_ShouldThrow()
        {
            Func<Task> action = async () =>
                await healthRecordService.DeleteHealthRecordAsync(0);

            await action.Should()
                .ThrowAsync<HealthRecordRuleException>()
                .WithMessage("Please provide a valid health record reference.");
        }

        [Fact]
        public async Task DeleteHealthRecordAsync_WhenNotFound_ShouldThrow()
        {
            healthRecordRepositoryMock
                .Setup(repository => repository.DeleteAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HealthRecord?)null);

            Func<Task> action = async () =>
                await healthRecordService.DeleteHealthRecordAsync(99);

            await action.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task DeleteHealthRecordAsync_WhenValid_ShouldReturnDeletedRecord()
        {
            var healthRecord = GetHealthRecords().First();

            healthRecordRepositoryMock
                .Setup(repository => repository.DeleteAsync(
                    healthRecord.HealthRecordId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecord);

            var result = await healthRecordService.DeleteHealthRecordAsync(healthRecord.HealthRecordId);

            result.HealthRecordId.Should().Be(healthRecord.HealthRecordId);
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
                .Returns((object source) => MapToHealthRecordDto((HealthRecord)source));

            mapperMock
                .Setup(mapper => mapper.Map<HealthRecord>(It.IsAny<object>()))
                .Returns((object source) =>
                {
                    return source switch
                    {
                        AddHealthRecordDto dto => new HealthRecord
                        {
                            PatientId = dto.PatientId,
                            DoctorId = dto.DoctorId,
                            AppointmentId = dto.AppointmentId,
                            VisitDate = dto.VisitDate,
                            Diagnosis = dto.Diagnosis,
                            Prescription = dto.Prescription,
                            Notes = dto.Notes
                        },

                        UpdateHealthRecordDto dto => new HealthRecord
                        {
                            VisitDate = dto.VisitDate,
                            Diagnosis = dto.Diagnosis,
                            Prescription = dto.Prescription,
                            Notes = dto.Notes
                        },

                        _ => new HealthRecord()
                    };
                });

            mapperMock
                .Setup(mapper => mapper.Map(
                    It.IsAny<UpdateHealthRecordDto>(),
                    It.IsAny<HealthRecord>()))
                .Returns((UpdateHealthRecordDto dto, HealthRecord record) =>
                {
                    record.VisitDate = dto.VisitDate;
                    record.Diagnosis = dto.Diagnosis;
                    record.Prescription = dto.Prescription;
                    record.Notes = dto.Notes;

                    return record;
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

        private void SetupDoctorExists(int doctorId)
        {
            doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    doctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetDoctors().First(doctor => doctor.DoctorId == doctorId));
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

        private void SetupAppointment(Appointment appointment)
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    appointment.AppointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment);
        }

        private void SetupHealthRecordDoesNotExist(int appointmentId)
        {
            healthRecordRepositoryMock
                .Setup(repository => repository.ExistsByAppointmentIdAsync(
                    appointmentId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
        }

        private void SetupValidAddHealthRecordDependencies(AddHealthRecordDto dto)
        {
            var appointment = GetConfirmedAppointmentForToday();

            SetupPatientExists(dto.PatientId);
            SetupAppointment(appointment);

            if (dto.DoctorId is not null)
            {
                SetupDoctorExists(dto.DoctorId.Value);
            }

            SetupHealthRecordDoesNotExist(dto.AppointmentId);
        }

        private static AddHealthRecordDto GetValidAddHealthRecordDto()
        {
            return new AddHealthRecordDto
            {
                PatientId = 1,
                DoctorId = 1,
                AppointmentId = 1,
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Drink water"
            };
        }

        private static UpdateHealthRecordDto GetValidUpdateHealthRecordDto()
        {
            return new UpdateHealthRecordDto
            {
                VisitDate = DateTime.Today,
                Diagnosis = "Updated Fever",
                Prescription = "Updated Prescription",
                Notes = "Updated Notes"
            };
        }

        private static Appointment GetConfirmedAppointmentForToday()
        {
            return new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                Patient = GetPatients().First(),
                DoctorId = 1,
                Doctor = GetDoctors().First(),
                ScheduledDate = DateTime.Today,
                TimeSlot = "09:00 AM - 09:30 AM",
                Status = AppointmentStatus.Confirmed
            };
        }

        private static HealthRecordDto MapToHealthRecordDto(HealthRecord record)
        {
            return new HealthRecordDto
            {
                HealthRecordId = record.HealthRecordId,
                PatientId = record.PatientId,
                PatientName = record.Patient?.PatientName ?? "Patient",
                DoctorId = record.DoctorId,
                DoctorName = record.Doctor?.DoctorName ?? "Doctor",
                AppointmentId = record.AppointmentId,
                Diagnosis = record.Diagnosis,
                Prescription = record.Prescription,
                Notes = record.Notes
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
                    ScheduledDate = DateTime.Today,
                    TimeSlot = "09:00 AM - 09:30 AM",
                    Status = AppointmentStatus.Confirmed
                },

                new Appointment
                {
                    AppointmentId = 2,
                    PatientId = 2,
                    Patient = patients[1],
                    DoctorId = 1,
                    Doctor = doctors[0],
                    ScheduledDate = DateTime.Today.AddDays(-1),
                    TimeSlot = "09:30 AM - 10:00 AM",
                    Status = AppointmentStatus.Completed
                },

                new Appointment
                {
                    AppointmentId = 3,
                    PatientId = 2,
                    Patient = patients[1],
                    DoctorId = 2,
                    Doctor = doctors[1],
                    ScheduledDate = DateTime.Today.AddDays(-2),
                    TimeSlot = "10:00 AM - 10:30 AM",
                    Status = AppointmentStatus.Cancelled
                }
            };
        }

        private static List<HealthRecord> GetHealthRecords()
        {
            var patients = GetPatients();
            var doctors = GetDoctors();
            var appointments = GetAppointments();

            return new List<HealthRecord>
            {
                new HealthRecord
                {
                    HealthRecordId = 1,
                    PatientId = 1,
                    Patient = patients[0],
                    DoctorId = 1,
                    Doctor = doctors[0],
                    AppointmentId = 1,
                    Appointment = appointments[0],
                    VisitDate = DateTime.Today.AddDays(-1),
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Drink water",
                    CreatedDate = DateTime.Today.AddDays(-1)
                },

                new HealthRecord
                {
                    HealthRecordId = 2,
                    PatientId = 1,
                    Patient = patients[0],
                    DoctorId = 1,
                    Doctor = doctors[0],
                    AppointmentId = 2,
                    Appointment = appointments[1],
                    VisitDate = DateTime.Today.AddDays(-2),
                    Diagnosis = "Cold",
                    Prescription = "Rest",
                    Notes = "Steam inhalation",
                    CreatedDate = DateTime.Today.AddDays(-2)
                },

                new HealthRecord
                {
                    HealthRecordId = 3,
                    PatientId = 2,
                    Patient = patients[1],
                    DoctorId = 2,
                    Doctor = doctors[1],
                    AppointmentId = 3,
                    Appointment = appointments[2],
                    VisitDate = DateTime.Today.AddDays(-3),
                    Diagnosis = "Headache",
                    Prescription = "Pain relief",
                    Notes = "Follow up if needed",
                    CreatedDate = DateTime.Today.AddDays(-3)
                },

                new HealthRecord
                {
                    HealthRecordId = 4,
                    PatientId = 2,
                    Patient = patients[1],
                    DoctorId = 1,
                    Doctor = doctors[0],
                    AppointmentId = 4,
                    VisitDate = DateTime.Today.AddDays(-4),
                    Diagnosis = "Back Pain",
                    Prescription = "Exercise",
                    Notes = "Physiotherapy",
                    CreatedDate = DateTime.Today.AddDays(-4)
                }
            };
        }
    }
}