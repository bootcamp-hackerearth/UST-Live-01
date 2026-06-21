using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HealthApp.Api.Services.Impl;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Dtos;
using HealthApp.Api.Models;
using HealthApp.Api.Enums;
using HealthApp.Api.Exceptions;

namespace HealthApp.Api.Tests.Services
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _healthRepo;
        private readonly Mock<IAppointmentRepository> _appointmentRepo;
        private readonly Mock<IPatientRepository> _patientRepo;
        private readonly Mock<IDoctorRepository> _doctorRepo;
        private readonly Mock<IMapper> _mapper;

        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            _healthRepo = new Mock<IHealthRecordRepository>();
            _appointmentRepo = new Mock<IAppointmentRepository>();
            _patientRepo = new Mock<IPatientRepository>();
            _doctorRepo = new Mock<IDoctorRepository>();
            _mapper = new Mock<IMapper>();

            _service = new HealthRecordService(
                _healthRepo.Object,
                _appointmentRepo.Object,
                _patientRepo.Object,
                _doctorRepo.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task AddAsync_ShouldAddRecord_WhenValidWithoutAppointment()
        {
            var dto = new HealthRecordCreateDto
            {
                PatientId = 1,
                DoctorId = 2,
                VisitDate = DateOnly.FromDateTime(DateTime.Today),
                Diagnosis = "Flu",
                Prescription = "Medicine"
            };

            _patientRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Patient());
            _doctorRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new Doctor());

            _mapper.Setup(m => m.Map<HealthRecord>(dto))
                .Returns(new HealthRecord());

            await _service.AddAsync(dto);

            _healthRepo.Verify(x => x.Add(It.IsAny<HealthRecord>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenPatientNotFound()
        {
            var dto = new HealthRecordCreateDto
            {
                PatientId = 1,
                DoctorId = 2,
                VisitDate = DateOnly.FromDateTime(DateTime.Today),
                Diagnosis = "Test",
                Prescription = "Test"
            };

            _patientRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenDoctorNotFound()
        {
            var dto = new HealthRecordCreateDto
            {
                PatientId = 1,
                DoctorId = 2,
                VisitDate = DateOnly.FromDateTime(DateTime.Today),
                Diagnosis = "Test",
                Prescription = "Test"
            };

            _patientRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Patient());
            _doctorRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenAppointmentNotFound()
        {
            var dto = new HealthRecordCreateDto
            {
                PatientId = 1,
                DoctorId = 2,
                AppointmentId = 10,
                VisitDate = DateOnly.FromDateTime(DateTime.Today),
                Diagnosis = "Test",
                Prescription = "Test"
            };

            _patientRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Patient());
            _doctorRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new Doctor());

            _appointmentRepo.Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenAppointmentNotConfirmed()
        {
            var dto = new HealthRecordCreateDto
            {
                PatientId = 1,
                DoctorId = 2,
                AppointmentId = 10,
                VisitDate = DateOnly.FromDateTime(DateTime.Today),
                Diagnosis = "Test",
                Prescription = "Test"
            };

            var appointment = new Appointment
            {
                PatientId = 1,
                DoctorId = 2,
                Status = AppointmentStatus.Pending
            };

            _patientRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Patient());
            _doctorRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new Doctor());

            _appointmentRepo.Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(() => _service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenDuplicateRecordExists()
        {
            var dto = new HealthRecordCreateDto
            {
                PatientId = 1,
                DoctorId = 2,
                AppointmentId = 10,
                VisitDate = DateOnly.FromDateTime(DateTime.Today),
                Diagnosis = "Test",
                Prescription = "Test"
            };

            var appointment = new Appointment
            {
                AppointmentId = 10,
                PatientId = 1,
                DoctorId = 2,
                Status = AppointmentStatus.Confirmed
            };

            _patientRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Patient());
            _doctorRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new Doctor());

            _appointmentRepo.Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(appointment);

            _healthRepo.Setup(x => x.GetHealthRecordsAsync(null, 10))
                .ReturnsAsync(new List<HealthRecord> { new HealthRecord() });

            await Assert.ThrowsAsync<DuplicateEntityException>(() => _service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ShouldCompleteAppointment_WhenValid()
        {
            var dto = new HealthRecordCreateDto
            {
                PatientId = 1,
                DoctorId = 2,
                AppointmentId = 10,
                VisitDate = DateOnly.FromDateTime(DateTime.Today),
                Diagnosis = "Cold",
                Prescription = "Medicine"
            };

            var appointment = new Appointment
            {
                AppointmentId = 10,
                PatientId = 1,
                DoctorId = 2,
                Status = AppointmentStatus.Confirmed
            };

            _patientRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Patient());
            _doctorRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new Doctor());
            _appointmentRepo.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(appointment);

            _healthRepo.Setup(x => x.GetHealthRecordsAsync(null, 10))
                .ReturnsAsync(new List<HealthRecord>());

            _mapper.Setup(m => m.Map<HealthRecord>(dto))
                .Returns(new HealthRecord());

            await _service.AddAsync(dto);

            _appointmentRepo.Verify(x =>
                x.Update(10, It.Is<Appointment>(a => a.Status == AppointmentStatus.Completed)),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRecord()
        {
            var record = new HealthRecord
            {
                RecordId = 1,
                PatientId = 1,
                DoctorId = 2
            };

            _healthRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(record);
            _patientRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Patient());
            _doctorRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new Doctor());

            _mapper.Setup(m => m.Map<HealthRecordDto>(record))
                .Returns(new HealthRecordDto { RecordId = 1 });

            var result = await _service.GetByIdAsync(1);

            Assert.Equal(1, result.RecordId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenInvalidId()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() => _service.GetByIdAsync(0));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenNotFound()
        {
            _healthRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((HealthRecord?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.GetByIdAsync(1));
        }

        [Fact]
        public async Task ExistsByAppointmentId_ShouldReturnTrue()
        {
            _healthRepo.Setup(x => x.GetHealthRecordsAsync(null, 10))
                .ReturnsAsync(new List<HealthRecord> { new HealthRecord() });

            var result = await _service.ExistsByAppointmentIdAsync(10);

            Assert.True(result);
        }

        [Fact]
        public async Task ExistsByAppointmentId_ShouldThrow_WhenInvalid()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.ExistsByAppointmentIdAsync(0));
        }

        [Fact]
        public async Task GetByPatientId_ShouldReturnRecords()
        {
            var records = new List<HealthRecord>
            {
                new HealthRecord { PatientId = 1 }
            };

            _healthRepo.Setup(x => x.GetHealthRecordsAsync(1, null))
                .ReturnsAsync(records);

            _mapper.Setup(m => m.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(new List<HealthRecordDto>
                {
                    new HealthRecordDto { RecordId = 1 }
                });

            var result = await _service.GetByPatientIdAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByPatientId_ShouldThrow_WhenInvalid()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.GetByPatientIdAsync(0));
        }
        [Fact]
        public async Task GetAllAsync_ShouldReturnRecords()
        {
            var records = new List<HealthRecord>
    {
        new HealthRecord { PatientId = 1, DoctorId = 2 }
    };

            _healthRepo.Setup(x => x.GetHealthRecordsAsync())
                .ReturnsAsync(records);

            _patientRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _doctorRepo.Setup(x => x.GetByIdAsync(2))
                .ReturnsAsync(new Doctor());

            _mapper.Setup(x => x.Map<IEnumerable<HealthRecordDto>>(records))
                .Returns(new List<HealthRecordDto> { new HealthRecordDto() });

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenDtoNull()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.AddAsync(null!));
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenDiagnosisMissing()
        {
            var dto = new HealthRecordCreateDto
            {
                PatientId = 1,
                DoctorId = 1,
                VisitDate = DateOnly.FromDateTime(DateTime.Today),
                Diagnosis = "",
                Prescription = "test"
            };

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenPatientMismatch()
        {
            var dto = new HealthRecordCreateDto
            {
                PatientId = 1,
                DoctorId = 2,
                AppointmentId = 10,
                VisitDate = DateOnly.FromDateTime(DateTime.Today),
                Diagnosis = "Test",
                Prescription = "Test"
            };

            var appointment = new Appointment
            {
                AppointmentId = 10,
                PatientId = 99,
                DoctorId = 2,
                Status = AppointmentStatus.Confirmed
            };

            _patientRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Patient());
            _doctorRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new Doctor());
            _appointmentRepo.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(appointment);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
                _service.AddAsync(dto));
        }

        [Fact]
        public async Task ExistsByAppointmentId_ShouldReturnFalse()
        {
            _healthRepo.Setup(x => x.GetHealthRecordsAsync(null, 10))
                .ReturnsAsync(new List<HealthRecord>());

            var result = await _service.ExistsByAppointmentIdAsync(10);

            Assert.False(result);
        }

        [Fact]
        public async Task GetPatientHistory_ShouldReturnSameAsGetByPatientId()
        {
            _healthRepo.Setup(x => x.GetHealthRecordsAsync(1, null))
                .ReturnsAsync(new List<HealthRecord>());

            _mapper.Setup(x => x.Map<IEnumerable<HealthRecordDto>>(It.IsAny<List<HealthRecord>>()))
                .Returns(new List<HealthRecordDto>());

            var result = await _service.GetPatientHistoryAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetById_ShouldLoadAppointment()
        {
            var record = new HealthRecord
            {
                RecordId = 1,
                PatientId = 1,
                DoctorId = 1,
                AppointmentId = 10
            };

            _healthRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(record);
            _patientRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Patient());
            _doctorRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Doctor());
            _appointmentRepo.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(new Appointment());

            _mapper.Setup(x => x.Map<HealthRecordDto>(record))
                .Returns(new HealthRecordDto());

            await _service.GetByIdAsync(1);

            _appointmentRepo.Verify(x => x.GetByIdAsync(10), Times.Once);
        }


    }
}