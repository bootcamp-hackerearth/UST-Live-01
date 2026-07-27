using AutoMapper;
using FluentAssertions;
using HealthAxisApplicn.Dto.Doctors;
using HealthAxisApplicn.Dto.HealthRecords;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Repositories;
using HealthAxisApplicn.Services.Impl;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class HealthRecordServiceTests
{
    private readonly Mock<IHealthRecordRepository> _repoMock;
    private readonly Mock<IAppointmentRepository> _appointmentRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly HealthRecordService _service;

    public HealthRecordServiceTests()
    {
        _repoMock = new Mock<IHealthRecordRepository>();
        _appointmentRepoMock = new Mock<IAppointmentRepository>();
        _mapperMock = new Mock<IMapper>();

        _service = new HealthRecordService(
            _repoMock.Object,
            _appointmentRepoMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Record_When_Valid()
    {
        var record = new HealthRecord
        {
            HealthRecordId = 1,
            AppointmentId = 1
        };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Completed"
        };

        var dto = new UpdateHealthRecordDto
        {
            Diagnosis = "Flu",
            Prescription = "Medicine"
        };

        var updated = new HealthRecord();

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(record);
        _appointmentRepoMock.Setup(a => a.GetByIdAsync(1, default)).ReturnsAsync(appointment);
        _repoMock.Setup(r => r.UpdateAsync(1, record, default)).ReturnsAsync(updated);
        _mapperMock.Setup(m => m.Map<HealthRecordDto>(updated)).Returns(new HealthRecordDto());

        var result = await _service.UpdateAsync(1, dto);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Not_Completed()
    {
        var record = new HealthRecord { HealthRecordId = 1, AppointmentId = 1 };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Pending"
        };

        var dto = new UpdateHealthRecordDto
        {
            Diagnosis = "Flu",
            Prescription = "Med"
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(record);
        _appointmentRepoMock.Setup(a => a.GetByIdAsync(1, default)).ReturnsAsync(appointment);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, dto));
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Diagnosis_Empty()
    {
        var record = new HealthRecord { HealthRecordId = 1, AppointmentId = 1 };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Completed"
        };

        var dto = new UpdateHealthRecordDto
        {
            Diagnosis = "",
            Prescription = "Med"
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(record);
        _appointmentRepoMock.Setup(a => a.GetByIdAsync(1, default)).ReturnsAsync(appointment);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, dto));
    }


    [Fact]
    public async Task GetAllAsync_Should_Return_List()
    {
        var list = new List<HealthRecord> { new HealthRecord() };
        var dtos = new List<HealthRecordDto> { new HealthRecordDto() };

        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(list);
        _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(list)).Returns(dtos);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Record()
    {
        var record = new HealthRecord { HealthRecordId = 1 };
        var dto = new HealthRecordDto { HealthRecordId = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(record);
        _mapperMock.Setup(m => m.Map<HealthRecordDto?>(record)).Returns(dto);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.HealthRecordId.Should().Be(1);
    }

    [Fact]
    public async Task GetRecordsByPatientIdAsync_Should_Return_List()
    {
        var list = new List<HealthRecord> { new HealthRecord() };
        var dtos = new List<HealthRecordDto> { new HealthRecordDto() };

        _repoMock.Setup(r => r.GetRecordsByPatientIdAsync(1, default)).ReturnsAsync(list);
        _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(list)).Returns(dtos);

        var result = await _service.GetRecordsByPatientIdAsync(1);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetRecordsByDoctorIdAsync_Should_Return_List()
    {
        var list = new List<HealthRecord> { new HealthRecord() };
        var dtos = new List<HealthRecordDto> { new HealthRecordDto() };

        _repoMock.Setup(r => r.GetRecordsByDoctorIdAsync(1, default)).ReturnsAsync(list);
        _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(list)).Returns(dtos);

        var result = await _service.GetRecordsByDoctorIdAsync(1);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Prescription_Empty()
    {
        var record = new HealthRecord
        {
            HealthRecordId = 1,
            AppointmentId = 1
        };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Completed"
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default))
            .ReturnsAsync(record);

        _appointmentRepoMock.Setup(a => a.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        var dto = new UpdateHealthRecordDto
        {
            Diagnosis = "Flu",
            Prescription = ""
        };

        await Assert.ThrowsAsync<Exception>(
            () => _service.UpdateAsync(1, dto));
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Notes()
    {
        var record = new HealthRecord
        {
            HealthRecordId = 1,
            AppointmentId = 1
        };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Completed"
        };

        var dto = new UpdateHealthRecordDto
        {
            Diagnosis = "Flu",
            Prescription = "Medicine",
            Notes = "Patient recovering"
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default))
            .ReturnsAsync(record);

        _appointmentRepoMock.Setup(a => a.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        _repoMock.Setup(r => r.UpdateAsync(1, record, default))
            .ReturnsAsync(record);

        _mapperMock.Setup(m => m.Map<HealthRecordDto>(record))
            .Returns(new HealthRecordDto());

        await _service.UpdateAsync(1, dto);

        record.Notes.Should().Be("Patient recovering");
    }

    [Fact]
    public async Task UpdateAsync_Should_Call_Update_Repository_Once()
    {
        var record = new HealthRecord
        {
            HealthRecordId = 1,
            AppointmentId = 1
        };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Completed"
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default))
            .ReturnsAsync(record);

        _appointmentRepoMock.Setup(a => a.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        _repoMock.Setup(r => r.UpdateAsync(1, record, default))
            .ReturnsAsync(record);

        _mapperMock.Setup(m => m.Map<HealthRecordDto>(record))
            .Returns(new HealthRecordDto());

        await _service.UpdateAsync(1, new UpdateHealthRecordDto
        {
            Diagnosis = "Diagnosis",
            Prescription = "Prescription"
        });

        _repoMock.Verify(
            r => r.UpdateAsync(1, record, default),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99, default))
            .ReturnsAsync((HealthRecord?)null);

        _mapperMock.Setup(m => m.Map<HealthRecordDto?>((HealthRecord?)null))
            .Returns((HealthRecordDto?)null);

        var result = await _service.GetByIdAsync(99);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateFromAppointment_Should_Create_Record()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 2,
            DoctorId = 3,
            ScheduledDate = DateTime.Today
        };

        _repoMock.Setup(r =>
            r.ExistsForAppointmentAsync(1, default))
            .ReturnsAsync(false);

        await _service.CreateFromAppointment(appointment);

        _repoMock.Verify(
            r => r.CreateAsync(
                It.IsAny<HealthRecord>(),
                default),
            Times.Once);
    }

    [Fact]
    public async Task CreateFromAppointment_Should_Not_Create_When_Record_Exists()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1
        };

        _repoMock.Setup(r =>
            r.ExistsForAppointmentAsync(1, default))
            .ReturnsAsync(true);

        await _service.CreateFromAppointment(appointment);

        _repoMock.Verify(
            r => r.CreateAsync(
                It.IsAny<HealthRecord>(),
                default),
            Times.Never);
    }

    [Fact]
    public async Task GetByAppointmentIdAsync_Should_Return_Record()
    {
        var record = new HealthRecord
        {
            AppointmentId = 1
        };

        var dto = new HealthRecordDto
        {
            AppointmentId = 1
        };

        _repoMock.Setup(r =>
            r.GetByAppointmentIdAsync(1, default))
            .ReturnsAsync(record);

        _mapperMock.Setup(m =>
            m.Map<HealthRecordDto?>(record))
            .Returns(dto);

        var result =
            await _service.GetByAppointmentIdAsync(1);

        result.Should().NotBeNull();
        result!.AppointmentId.Should().Be(1);
    }

    [Fact]
    public async Task GetByAppointmentIdAsync_Should_Return_Null()
    {
        _repoMock.Setup(r =>
            r.GetByAppointmentIdAsync(100, default))
            .ReturnsAsync((HealthRecord?)null);

        _mapperMock.Setup(m =>
            m.Map<HealthRecordDto?>((HealthRecord?)null))
            .Returns((HealthRecordDto?)null);

        var result =
            await _service.GetByAppointmentIdAsync(100);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetDoctorPatientsAsync_Should_Return_List()
    {
        var patients = new List<DoctorPatientListDto>
    {
        new()
        {
            PatientId = 1,
            PatientName = "John"
        }
    };

        _repoMock.Setup(r =>
            r.GetDoctorPatientsAsync(1))
            .ReturnsAsync(patients);

        var result =
            await _service.GetDoctorPatientsAsync(1);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Null_When_Record_Not_Found()
    {
        // Arrange
        _repoMock.Setup(x =>
            x.GetByIdAsync(999, default))
            .ReturnsAsync((HealthRecord?)null);

        // Act
        var result = await _service.UpdateAsync(
            999,
            new UpdateHealthRecordDto());

        // Assert
        result.Should().BeNull();

        _appointmentRepoMock.Verify(
            x => x.GetByIdAsync(
                It.IsAny<int>(),
                default),
            Times.Never);

        _repoMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<int>(),
                It.IsAny<HealthRecord>(),
                default),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Diagnosis()
    {
        var record = new HealthRecord
        {
            AppointmentId = 1
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(record);

        _appointmentRepoMock.Setup(x =>
            x.GetByIdAsync(1, default))
            .ReturnsAsync(new Appointment
            {
                Status = "Completed"
            });

        _repoMock.Setup(x =>
            x.UpdateAsync(1, record, default))
            .ReturnsAsync(record);

        _mapperMock.Setup(x =>
            x.Map<HealthRecordDto>(record))
            .Returns(new HealthRecordDto());

        await _service.UpdateAsync(
            1,
            new UpdateHealthRecordDto
            {
                Diagnosis = "Diabetes",
                Prescription = "Medicine"
            });

        record.Diagnosis.Should().Be("Diabetes");
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Prescription()
    {
        var record = new HealthRecord
        {
            AppointmentId = 1
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(record);

        _appointmentRepoMock.Setup(x =>
            x.GetByIdAsync(1, default))
            .ReturnsAsync(new Appointment
            {
                Status = "Completed"
            });

        _repoMock.Setup(x =>
            x.UpdateAsync(1, record, default))
            .ReturnsAsync(record);

        _mapperMock.Setup(x =>
            x.Map<HealthRecordDto>(record))
            .Returns(new HealthRecordDto());

        await _service.UpdateAsync(
            1,
            new UpdateHealthRecordDto
            {
                Diagnosis = "Flu",
                Prescription = "Paracetamol"
            });

        record.Prescription.Should().Be("Paracetamol");
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(x =>
            x.GetAllAsync(default))
            .ReturnsAsync(new List<HealthRecord>());

        _mapperMock.Setup(x =>
            x.Map<List<HealthRecordDto>>(
                It.IsAny<List<HealthRecord>>()))
            .Returns(new List<HealthRecordDto>());

        var result = await _service.GetAllAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetRecordsByPatientIdAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(x =>
            x.GetRecordsByPatientIdAsync(
                1,
                default))
            .ReturnsAsync(new List<HealthRecord>());

        _mapperMock.Setup(x =>
            x.Map<List<HealthRecordDto>>(
                It.IsAny<List<HealthRecord>>()))
            .Returns(new List<HealthRecordDto>());

        var result =
            await _service.GetRecordsByPatientIdAsync(1);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetRecordsByDoctorIdAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(x =>
            x.GetRecordsByDoctorIdAsync(
                1,
                default))
            .ReturnsAsync(new List<HealthRecord>());

        _mapperMock.Setup(x =>
            x.Map<List<HealthRecordDto>>(
                It.IsAny<List<HealthRecord>>()))
            .Returns(new List<HealthRecordDto>());

        var result =
            await _service.GetRecordsByDoctorIdAsync(1);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateFromAppointment_Should_Copy_DoctorId()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1,
            DoctorId = 55,
            PatientId = 10
        };

        HealthRecord? savedRecord = null;

        _repoMock.Setup(x =>
            x.ExistsForAppointmentAsync(
                1,
                default))
            .ReturnsAsync(false);

        _repoMock.Setup(x =>
            x.CreateAsync(
                It.IsAny<HealthRecord>(),
                default))
            .Callback<HealthRecord, CancellationToken>(
                (r, _) => savedRecord = r)
            .ReturnsAsync(new HealthRecord());

        await _service.CreateFromAppointment(
            appointment);

        savedRecord!.DoctorId.Should().Be(55);
    }

    [Fact]
    public async Task CreateFromAppointment_Should_Copy_PatientId()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 99
        };

        HealthRecord? captured = null;

        _repoMock.Setup(x =>
            x.ExistsForAppointmentAsync(
                1,
                default))
            .ReturnsAsync(false);

        _repoMock.Setup(x =>
            x.CreateAsync(
                It.IsAny<HealthRecord>(),
                default))
            .Callback<HealthRecord, CancellationToken>(
                (r, _) => captured = r)
            .ReturnsAsync(new HealthRecord());

        await _service.CreateFromAppointment(
            appointment);

        captured!.PatientId.Should().Be(99);
    }

    [Fact]
    public async Task CreateFromAppointment_Should_Copy_AppointmentId()
    {
        var appointment = new Appointment
        {
            AppointmentId = 888
        };

        HealthRecord? captured = null;

        _repoMock.Setup(x =>
            x.ExistsForAppointmentAsync(
                888,
                default))
            .ReturnsAsync(false);

        _repoMock.Setup(x =>
            x.CreateAsync(
                It.IsAny<HealthRecord>(),
                default))
            .Callback<HealthRecord, CancellationToken>(
                (r, _) => captured = r)
            .ReturnsAsync(new HealthRecord());

        await _service.CreateFromAppointment(
            appointment);

        captured!.AppointmentId.Should().Be(888);
    }

    [Fact]
    public async Task GetDoctorPatientsAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(x =>
            x.GetDoctorPatientsAsync(1))
            .ReturnsAsync(new List<DoctorPatientListDto>());

        var result =
            await _service.GetDoctorPatientsAsync(1);

        result.Should().BeEmpty();
    }


}