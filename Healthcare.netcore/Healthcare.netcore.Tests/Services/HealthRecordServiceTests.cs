using AutoMapper;
using FluentAssertions;
using HealthAxis.Shared.DTOs.HealthRecord;
using HealthAxis.Shared.Enums;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementations;
using Moq;
using ValidationException = HealthAxis.API.Exceptions.ValidationException;

namespace Healthcare.netcore.Tests.Services
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _healthRecordRepositoryMock;
        private readonly Mock<IRepository<Appointment>> _appointmentRepositoryMock;
        private readonly Mock<IRepository<Patient>> _patientRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            _healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();
            _appointmentRepositoryMock = new Mock<IRepository<Appointment>>();
            _patientRepositoryMock = new Mock<IRepository<Patient>>();
            _mapperMock = new Mock<IMapper>();

            _service = new HealthRecordService(
                _healthRecordRepositoryMock.Object,
                _appointmentRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _mapperMock.Object);
        }

        private static CreateHealthRecordDto GetCreateDto()
        {
            return new CreateHealthRecordDto
            {
                AppointmentId = 1,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Take rest"
            };
        }

        private static Appointment GetCompletedAppointment()
        {
            return new Appointment
            {
                AppointmentId = 1,
                PatientId = 3,
                DoctorId = 6,
                ScheduledDate = DateTime.Today,
                TimeSlot = "10:00 AM",
                Status = AppointmentStatus.Completed
            };
        }

        private static  HealthRecord GetHealthRecord()
        {
            return new HealthRecord
            {
                RecordId = 1,
                AppointmentId = 1,
                PatientId = 3,
                DoctorId = 6,
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Take rest"
            };
        }

        private static HealthRecordDto GetHealthRecordDto()
        {
            return new HealthRecordDto
            {
                RecordId = 1,
                AppointmentId = 1,
                PatientId = 3,
                DoctorId = 6,
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Take rest"
            };
        }

        [Fact]
        public async Task AddAsync_WhenAppointmentDoesNotExist_ThrowsNotFoundException()
        {
            var dto = GetCreateDto();

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Appointment not found.");
        }

        [Fact]
        public async Task AddAsync_WhenAppointmentNotCompleted_ThrowsValidationException()
        {
            var dto = GetCreateDto();

            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 3,
                DoctorId = 6,
                ScheduledDate = DateTime.Today,
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Health records can only be created for completed appointments.");
        }

        [Fact]
        public async Task AddAsync_WhenHealthRecordAlreadyExists_ThrowsValidationException()
        {
            var dto = GetCreateDto();

            var appointment = GetCompletedAppointment();

            var existingRecords = new List<HealthRecord>
            {
                new HealthRecord
                {
                    RecordId = 1,
                    AppointmentId = dto.AppointmentId,
                    PatientId = 3,
                    DoctorId = 6
                }
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync(existingRecords);

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Health record already exists for this appointment.");
        }

        [Fact]
        public async Task AddAsync_WhenValidRequest_ReturnsHealthRecordDto()
        {
            var dto = GetCreateDto();

            var appointment = GetCompletedAppointment();

            var healthRecord = GetHealthRecord();

            var healthRecordDto = GetHealthRecordDto();

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync(new List<HealthRecord>());

            _healthRecordRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<HealthRecord>()))
                .ReturnsAsync(healthRecord);

            _mapperMock
                .Setup(x => x.Map<HealthRecordDto>(It.IsAny<HealthRecord>()))
                .Returns(healthRecordDto);

            var result = await _service.AddAsync(dto);

            result.Should().NotBeNull();
            result.RecordId.Should().Be(1);
            result.PatientId.Should().Be(3);
            result.DoctorId.Should().Be(6);
            result.Diagnosis.Should().Be("Fever");

            _healthRecordRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<HealthRecord>()),
                Times.Once);
        }

        [Fact]
        public async Task AddAsync_WhenValidRequest_CopiesAppointmentDetailsToHealthRecord()
        {
            var dto = GetCreateDto();

            var appointment = GetCompletedAppointment();

            HealthRecord? capturedRecord = null;

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync(new List<HealthRecord>());

            _healthRecordRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<HealthRecord>()))
                .Callback<HealthRecord>(record =>
                {
                    capturedRecord = record;
                })
                .ReturnsAsync(GetHealthRecord());

            _mapperMock
                .Setup(x => x.Map<HealthRecordDto>(It.IsAny<HealthRecord>()))
                .Returns(GetHealthRecordDto());

            await _service.AddAsync(dto);

            capturedRecord.Should().NotBeNull();
            capturedRecord!.AppointmentId.Should().Be(appointment.AppointmentId);
            capturedRecord.PatientId.Should().Be(appointment.PatientId);
            capturedRecord.DoctorId.Should().Be(appointment.DoctorId);
            capturedRecord.VisitDate.Should().Be(appointment.ScheduledDate);
            capturedRecord.Diagnosis.Should().Be(dto.Diagnosis);
            capturedRecord.Prescription.Should().Be(dto.Prescription);
            capturedRecord.Notes.Should().Be(dto.Notes);
        }

        [Fact]
        public async Task GetByIdAsync_WhenRecordExists_ReturnsHealthRecordDto()
        {
            var record = GetHealthRecord();

            var dto = GetHealthRecordDto();

            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(record);

            _mapperMock
                .Setup(x => x.Map<HealthRecordDto>(record))
                .Returns(dto);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result.RecordId.Should().Be(1);
            result.Diagnosis.Should().Be("Fever");
        }

        [Fact]
        public async Task GetByIdAsync_WhenRecordDoesNotExist_ThrowsNotFoundException()
        {
            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((HealthRecord?)null);

            Func<Task> action = async () => await _service.GetByIdAsync(99);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Health record not found.");
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenPatientDoesNotExist_ThrowsNotFoundException()
        {
            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () => await _service.GetByPatientIdAsync(99);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Patient not found.");
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenPatientExists_ReturnsHealthRecords()
        {
            var patient = new Patient
            {
                PatientId = 3,
                FullName = "Kiran"
            };

            var records = new List<HealthRecord>
            {
                GetHealthRecord()
            };

            var dtos = new List<HealthRecordDto>
            {
                GetHealthRecordDto()
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(3))
                .ReturnsAsync(patient);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(3))
                .ReturnsAsync(records);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(dtos);

            var result = await _service.GetByPatientIdAsync(3);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Diagnosis.Should().Be("Fever");
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenPatientExistsButNoRecords_ReturnsEmptyList()
        {
            var patient = new Patient
            {
                PatientId = 3,
                FullName = "Kiran"
            };

            var records = new List<HealthRecord>();

            var dtos = new List<HealthRecordDto>();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(3))
                .ReturnsAsync(patient);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(3))
                .ReturnsAsync(records);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(dtos);

            var result = await _service.GetByPatientIdAsync(3);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
        [Fact]
        public async Task GetByIdAsync_WhenRepositoryFails_ThrowsException()
        {
            _healthRecordRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> action = async () => await _service.GetByIdAsync(1);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }
        [Fact]
        public async Task GetByPatientIdAsync_WhenPatientRepositoryFails_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(3))
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> action = async () => await _service.GetByPatientIdAsync(3);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }
        [Fact]
        public async Task GetByPatientIdAsync_WhenHealthRecordRepositoryFails_ThrowsException()
        {
            var patient = new Patient
            {
                PatientId = 3,
                FullName = "Kiran"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(3))
                .ReturnsAsync(patient);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(3))
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> action = async () => await _service.GetByPatientIdAsync(3);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }
        [Fact]
        public async Task AddAsync_WhenAppointmentRepositoryFails_ThrowsException()
        {
            var dto = GetCreateDto();

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }
        [Fact]
        public async Task AddAsync_WhenExistingRecordCheckFails_ThrowsException()
        {
            var dto = GetCreateDto();
            var appointment = GetCompletedAppointment();

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }
        [Fact]
        public async Task AddAsync_WhenAddRepositoryFails_ThrowsException()
        {
            var dto = GetCreateDto();
            var appointment = GetCompletedAppointment();

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync(new List<HealthRecord>());

            _healthRecordRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<HealthRecord>()))
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> action = async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }
        [Fact]
        public async Task AddAsync_WhenValidRequest_VerifiesAppointmentLookup()
        {
            var dto = GetCreateDto();
            var appointment = GetCompletedAppointment();

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync(new List<HealthRecord>());

            _healthRecordRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<HealthRecord>()))
                .ReturnsAsync(GetHealthRecord());

            _mapperMock
                .Setup(x => x.Map<HealthRecordDto>(It.IsAny<HealthRecord>()))
                .Returns(GetHealthRecordDto());

            await _service.AddAsync(dto);

            _appointmentRepositoryMock.Verify(
                x => x.GetByIdAsync(dto.AppointmentId),
                Times.Once);
        }
        [Fact]
        public async Task AddAsync_WhenValidRequest_VerifiesDuplicateRecordCheck()
        {
            var dto = GetCreateDto();
            var appointment = GetCompletedAppointment();

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync(new List<HealthRecord>());

            _healthRecordRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<HealthRecord>()))
                .ReturnsAsync(GetHealthRecord());

            _mapperMock
                .Setup(x => x.Map<HealthRecordDto>(It.IsAny<HealthRecord>()))
                .Returns(GetHealthRecordDto());

            await _service.AddAsync(dto);

            _healthRecordRepositoryMock.Verify(
                x => x.GetByAppointmentIdAsync(dto.AppointmentId),
                Times.Once);
        }
        [Fact]
        public async Task AddAsync_WhenNotesAreEmpty_CreatesRecordWithEmptyNotes()
        {
            var dto = GetCreateDto();
            dto.Notes = string.Empty;

            var appointment = GetCompletedAppointment();

            HealthRecord? capturedRecord = null;

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.AppointmentId))
                .ReturnsAsync(appointment);

            _healthRecordRepositoryMock
                .Setup(x => x.GetByAppointmentIdAsync(dto.AppointmentId))
                .ReturnsAsync(new List<HealthRecord>());

            _healthRecordRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<HealthRecord>()))
                .Callback<HealthRecord>(record =>
                {
                    capturedRecord = record;
                })
                .ReturnsAsync(GetHealthRecord());

            _mapperMock
                .Setup(x => x.Map<HealthRecordDto>(It.IsAny<HealthRecord>()))
                .Returns(GetHealthRecordDto());

            await _service.AddAsync(dto);

            capturedRecord.Should().NotBeNull();
            capturedRecord!.Notes.Should().BeEmpty();
        }
    }
}