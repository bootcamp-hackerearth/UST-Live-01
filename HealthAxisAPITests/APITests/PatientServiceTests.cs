using AutoMapper;
using FluentAssertions;
using HealthAxisApplicn.Dto.HealthRecords;
using HealthAxisApplicn.Dto.Patients;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Repositories;
using HealthAxisApplicn.Services.Impl;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class PatientServiceTests
{
    private readonly Mock<IPatientRepository> _repoMock;
    private readonly Mock<IHealthRecordRepository> _healthRecordRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly PatientService _service;

    public PatientServiceTests()
    {

        _repoMock = new Mock<IPatientRepository>();
        _healthRecordRepoMock = new Mock<IHealthRecordRepository>();
        _mapperMock = new Mock<IMapper>();

        _service = new PatientService(
            _repoMock.Object,
            _healthRecordRepoMock.Object,
            _mapperMock.Object
        );

    }

    [Fact]
    public async Task CreateAsync_Should_Create_Patient()
    {
        var dto = new CreatePatientDto { PatientName = "John" };
        var patient = new Patient { PatientName = "John" };
        var resultDto = new PatientDto { PatientName = "John" };

        _mapperMock.Setup(m => m.Map<Patient>(dto)).Returns(patient);
        _repoMock.Setup(r => r.CreateAsync(patient, default)).ReturnsAsync(patient);
        _mapperMock.Setup(m => m.Map<PatientDto>(patient)).Returns(resultDto);

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.PatientName.Should().Be("John");
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_List()
    {
        var patients = new List<Patient> { new Patient { PatientId = 1 } };
        var patientDtos = new List<PatientDto> { new PatientDto { PatientId = 1 } };

        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(patients);
        _mapperMock.Setup(m => m.Map<List<PatientDto>>(patients)).Returns(patientDtos);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Patient()
    {
        var patient = new Patient { PatientId = 1 };
        var dto = new PatientDto { PatientId = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(patient);
        _mapperMock.Setup(m => m.Map<PatientDto>(patient)).Returns(dto);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.PatientId.Should().Be(1);
    }

    [Fact]
    public async Task DeactivatePatientAsync_Should_Deactivate_When_Active()
    {
        var patient = new Patient { PatientId = 1, IsActive = true };
        var updated = new Patient { PatientId = 1, IsActive = false };
        var dto = new PatientDto { PatientId = 1, IsActive = false };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(patient);
        _repoMock.Setup(r => r.UpdateAsync(1, patient, default)).ReturnsAsync(updated);
        _mapperMock.Setup(m => m.Map<PatientDto>(updated)).Returns(dto);

        var result = await _service.DeactivatePatientAsync(1);

        result.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task DeactivatePatientAsync_Should_Throw_When_NotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<System.Exception>(() => _service.DeactivatePatientAsync(1));
    }

    [Fact]
    public async Task ToggleActiveAsync_Should_Activate_When_Inactive()
    {
        var patient = new Patient
        {
            PatientId = 1,
            IsActive = false
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default))
                 .ReturnsAsync(patient);

        _repoMock.Setup(r => r.UpdateAsync(1, patient, default))
                 .ReturnsAsync(patient);

        _mapperMock.Setup(m => m.Map<PatientDto>(patient))
                   .Returns(new PatientDto { PatientId = 1, IsActive = true });

        var result = await _service.DeactivatePatientAsync(1);

        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_When_Found()
    {
        var existing = new Patient
        {
            PatientId = 1,
            PatientName = "Old",
            Email = "old@test.com"
        };

        var dto = new UpdatePatientDto
        {
            PatientName = "New",
            DateOfBirth = DateTime.Today.AddYears(-20),
            Gender = "Male",
            Email = "new@test.com",
            PhoneNo = "+919876543210"
        };

        var updated = new Patient
        {
            PatientId = 1,
            PatientName = "New",
            Email = "new@test.com"
        };

        var resultDto = new PatientDto
        {
            PatientId = 1,
            PatientName = "New",
            Email = "new@test.com"
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default))
                 .ReturnsAsync(existing);

        _repoMock.Setup(r => r.SearchByEmailAsync(dto.Email, default))
                 .ReturnsAsync((Patient?)null); // ✅ no duplicate email

        _repoMock.Setup(r => r.UpdateAsync(1, existing, default))
                 .ReturnsAsync(updated);

        _mapperMock.Setup(m => m.Map<PatientDto>(updated))
                   .Returns(resultDto);

        var result = await _service.UpdateAsync(1, dto);

        result!.PatientName.Should().Be("New");
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_NotFound()
    {
        var dto = new UpdatePatientDto
        {
            PatientName = "New",
            DateOfBirth = DateTime.Today.AddYears(-20),
            Gender = "Male",
            Email = "new@test.com",
            PhoneNo = "+919876543210"
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default))
                 .ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, dto));
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Email_Already_Exists()
    {
        var existing = new Patient { PatientId = 1, Email = "old@test.com" };

        var dto = new UpdatePatientDto
        {
            PatientName = "New",
            DateOfBirth = DateTime.Today.AddYears(-20),
            Gender = "Male",
            Email = "duplicate@test.com",
            PhoneNo = "+919876543210"
        };

        var otherPatient = new Patient { PatientId = 2, Email = "duplicate@test.com" };

        _repoMock.Setup(r => r.GetByIdAsync(1, default))
                 .ReturnsAsync(existing);

        _repoMock.Setup(r => r.SearchByEmailAsync(dto.Email, default))
                 .ReturnsAsync(otherPatient); // ❌ duplicate found

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, dto));
    }

    [Fact]
    public async Task SearchByEmailAsync_Should_Return_Result()
    {
        var patient = new Patient { Email = "test@mail.com" };
        var dto = new PatientDto { Email = "test@mail.com" };

        _repoMock.Setup(r => r.SearchByEmailAsync("test@mail.com", default)).ReturnsAsync(patient);
        _mapperMock.Setup(m => m.Map<PatientDto>(patient)).Returns(dto);

        var result = await _service.SearchByEmailAsync("test@mail.com");

        result.Email.Should().Be("test@mail.com");
    }

    [Fact]
    public async Task SearchByPatientNameAsync_Should_Return_List()
    {
        var patients = new List<Patient> { new Patient { PatientName = "John" } };
        var dtos = new List<PatientDto> { new PatientDto { PatientName = "John" } };

        _repoMock.Setup(r => r.SearchByNameAsync("john", default)).ReturnsAsync(patients);
        _mapperMock.Setup(m => m.Map<List<PatientDto>>(patients)).Returns(dtos);

        var result = await _service.SearchByPatientNameAsync("john");

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_DateOfBirth_Is_Future()
    {
        var dto = new UpdatePatientDto
        {
            PatientName = "John",
            DateOfBirth = DateTime.Today.AddDays(1),
            Gender = "Male",
            Email = "john@test.com",
            PhoneNo = "+919876543210"
        };

        var ex = await Assert.ThrowsAsync<Exception>(
            () => _service.UpdateAsync(1, dto));

        ex.Message.Should().Be("Invalid date of birth");
    }

    [Fact]
    public async Task UpdateAsync_Should_Not_Call_Update_When_DOB_Invalid()
    {
        var dto = new UpdatePatientDto
        {
            DateOfBirth = DateTime.Today.AddDays(5)
        };

        await Assert.ThrowsAsync<Exception>(
            () => _service.UpdateAsync(1, dto));

        _repoMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<int>(),
                It.IsAny<Patient>(),
                default),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_Should_Allow_Same_Email_For_Same_Patient()
    {
        var dto = new UpdatePatientDto
        {
            PatientName = "John",
            DateOfBirth = DateTime.Today.AddYears(-20),
            Gender = "Male",
            Email = "john@test.com",
            PhoneNo = "+919876543210"
        };

        var patient = new Patient
        {
            PatientId = 1,
            Email = "john@test.com"
        };

        _repoMock.Setup(x => x.SearchByEmailAsync(dto.Email, default))
            .ReturnsAsync(patient);

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(patient);

        _repoMock.Setup(x => x.UpdateAsync(1, patient, default))
            .ReturnsAsync(patient);

        _mapperMock.Setup(x => x.Map<PatientDto>(patient))
            .Returns(new PatientDto());

        await _service.UpdateAsync(1, dto);

        _repoMock.Verify(x => x.UpdateAsync(1, patient, default), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_NotFound()
    {
        _repoMock.Setup(x => x.GetByIdAsync(99, default))
            .ReturnsAsync((Patient?)null);

        _mapperMock.Setup(x => x.Map<PatientDto>(null))
            .Returns((PatientDto?)null);

        var result = await _service.GetByIdAsync(99);

        result.Should().BeNull();
    }

    [Fact]
    public async Task SearchByEmailAsync_Should_Return_Null_When_NotFound()
    {
        _repoMock.Setup(x =>
                x.SearchByEmailAsync("missing@test.com", default))
            .ReturnsAsync((Patient?)null);

        _mapperMock.Setup(x => x.Map<PatientDto>(null))
            .Returns((PatientDto?)null);

        var result =
            await _service.SearchByEmailAsync("missing@test.com");

        result.Should().BeNull();
    }

    [Fact]
    public async Task SearchByPhoneNumberAsync_Should_Return_Null_When_NotFound()
    {
        _repoMock.Setup(x =>
                x.SearchByPhoneNumberAsync("999", default))
            .ReturnsAsync((Patient?)null);

        _mapperMock.Setup(x => x.Map<PatientDto>(null))
            .Returns((PatientDto?)null);

        var result =
            await _service.SearchByPhoneNumberAsync("999");

        result.Should().BeNull();
    }

    [Fact]
    public async Task SearchAsync_Should_Return_Filtered_List()
    {
        var patients = new List<Patient>
    {
        new() { PatientId = 1 }
    };

        var dtos = new List<PatientDto>
    {
        new() { PatientId = 1 }
    };

        _repoMock.Setup(x =>
                x.SearchAsync("john", null, default))
            .ReturnsAsync(patients);

        _mapperMock.Setup(x =>
                x.Map<List<PatientDto>>(patients))
            .Returns(dtos);

        var result = await _service.SearchAsync("john", null);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByUserIdAsync_Should_Return_Patient()
    {
        var patient = new Patient
        {
            PatientId = 1,
            UserId = "user1"
        };

        _repoMock.Setup(x => x.GetByUserIdAsync("user1"))
            .ReturnsAsync(patient);

        var result = await _service.GetByUserIdAsync("user1");

        result.Should().NotBeNull();
        result!.UserId.Should().Be("user1");
    }

    [Fact]
    public async Task GetPatientDetailsForDoctorAsync_Should_Return_Null_When_Patient_Not_Found()
    {
        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync((Patient?)null);

        var result =
            await _service.GetPatientDetailsForDoctorAsync(2, 1);

        result.Should().BeNull();

        _healthRecordRepoMock.Verify(
            x => x.GetRecordsForDoctorPatientAsync(
                It.IsAny<int>(),
                It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateAsync_Should_Call_Repository_Once()
    {
        var dto = new CreatePatientDto();
        var patient = new Patient();

        _mapperMock.Setup(x => x.Map<Patient>(dto))
            .Returns(patient);

        _repoMock.Setup(x => x.CreateAsync(patient, default))
            .ReturnsAsync(patient);

        _mapperMock.Setup(x => x.Map<PatientDto>(patient))
            .Returns(new PatientDto());

        await _service.CreateAsync(dto);

        _repoMock.Verify(
            x => x.CreateAsync(patient, default),
            Times.Once);
    }

    [Fact]
    public async Task GetPatientDetailsForDoctorAsync_Should_Return_Patient_Details()
    {
        var patient = new Patient
        {
            PatientId = 1,
            PatientName = "John"
        };

        var patientDto = new PatientDto
        {
            PatientId = 1,
            PatientName = "John"
        };

        var records = new List<HealthRecord>();

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(patient);

        _healthRecordRepoMock.Setup(x =>
            x.GetRecordsForDoctorPatientAsync(10, 1))
            .ReturnsAsync(records);

        _mapperMock.Setup(x => x.Map<PatientDto>(patient))
            .Returns(patientDto);

        _mapperMock.Setup(x =>
            x.Map<List<HealthRecordDto>>(records))
            .Returns(new List<HealthRecordDto>());

        var result =
            await _service.GetPatientDetailsForDoctorAsync(10, 1);

        result.Should().NotBeNull();
        result!.Patient.PatientId.Should().Be(1);
    }

    [Fact]
    public async Task GetPatientDetailsForDoctorAsync_Should_Return_HealthRecords()
    {
        var patient = new Patient { PatientId = 1 };

        var records = new List<HealthRecord>
    {
        new(),
        new()
    };

        var mappedRecords = new List<HealthRecordDto>
    {
        new(),
        new()
    };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(patient);

        _healthRecordRepoMock.Setup(x =>
            x.GetRecordsForDoctorPatientAsync(5, 1))
            .ReturnsAsync(records);

        _mapperMock.Setup(x => x.Map<PatientDto>(patient))
            .Returns(new PatientDto());

        _mapperMock.Setup(x =>
            x.Map<List<HealthRecordDto>>(records))
            .Returns(mappedRecords);

        var result =
            await _service.GetPatientDetailsForDoctorAsync(5, 1);

        result!.HealthRecords.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetPatientDetailsForDoctorAsync_Should_Return_Empty_Record_List()
    {
        var patient = new Patient { PatientId = 1 };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(patient);

        _healthRecordRepoMock.Setup(x =>
            x.GetRecordsForDoctorPatientAsync(1, 1))
            .ReturnsAsync(new List<HealthRecord>());

        _mapperMock.Setup(x => x.Map<PatientDto>(patient))
            .Returns(new PatientDto());

        _mapperMock.Setup(x =>
            x.Map<List<HealthRecordDto>>(It.IsAny<List<HealthRecord>>()))
            .Returns(new List<HealthRecordDto>());

        var result =
            await _service.GetPatientDetailsForDoctorAsync(1, 1);

        result!.HealthRecords.Should().BeEmpty();
    }

    [Fact]
    public async Task GetPatientDetailsForDoctorAsync_Should_Call_HealthRecordRepository()
    {
        var patient = new Patient { PatientId = 1 };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(patient);

        _healthRecordRepoMock.Setup(x =>
            x.GetRecordsForDoctorPatientAsync(3, 1))
            .ReturnsAsync(new List<HealthRecord>());

        _mapperMock.Setup(x => x.Map<PatientDto>(patient))
            .Returns(new PatientDto());

        _mapperMock.Setup(x =>
            x.Map<List<HealthRecordDto>>(It.IsAny<List<HealthRecord>>()))
            .Returns(new List<HealthRecordDto>());

        await _service.GetPatientDetailsForDoctorAsync(3, 1);

        _healthRecordRepoMock.Verify(
            x => x.GetRecordsForDoctorPatientAsync(3, 1),
            Times.Once);
    }

    [Fact]
    public async Task GetPatientDetailsForDoctorAsync_Should_Call_PatientRepository_Once()
    {
        var patient = new Patient { PatientId = 1 };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(patient);

        _healthRecordRepoMock.Setup(x =>
            x.GetRecordsForDoctorPatientAsync(5, 1))
            .ReturnsAsync(new List<HealthRecord>());

        _mapperMock.Setup(x => x.Map<PatientDto>(patient))
            .Returns(new PatientDto());

        _mapperMock.Setup(x =>
            x.Map<List<HealthRecordDto>>(It.IsAny<List<HealthRecord>>()))
            .Returns(new List<HealthRecordDto>());

        await _service.GetPatientDetailsForDoctorAsync(5, 1);

        _repoMock.Verify(
            x => x.GetByIdAsync(1, default),
            Times.Once);
    }

    [Fact]
    public async Task SearchAsync_Should_Return_Empty_List_When_No_Match()
    {
        _repoMock.Setup(x =>
            x.SearchAsync("abc", "123", default))
            .ReturnsAsync(new List<Patient>());

        _mapperMock.Setup(x =>
            x.Map<List<PatientDto>>(It.IsAny<List<Patient>>()))
            .Returns(new List<PatientDto>());

        var result = await _service.SearchAsync("abc", "123");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchAsync_Should_Search_By_Name_And_Phone()
    {
        var patients = new List<Patient>
    {
        new() { PatientId = 1 }
    };

        var dtos = new List<PatientDto>
    {
        new() { PatientId = 1 }
    };

        _repoMock.Setup(x =>
            x.SearchAsync("john", "+91", default))
            .ReturnsAsync(patients);

        _mapperMock.Setup(x =>
            x.Map<List<PatientDto>>(patients))
            .Returns(dtos);

        var result =
            await _service.SearchAsync("john", "+91");

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByUserIdAsync_Should_Return_Null_When_Not_Exists()
    {
        _repoMock.Setup(x => x.GetByUserIdAsync("missing"))
            .ReturnsAsync((Patient?)null);

        var result = await _service.GetByUserIdAsync("missing");

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Preserve_PatientId()
    {
        var patient = new Patient
        {
            PatientId = 1,
            Email = "old@test.com"
        };

        var dto = new UpdatePatientDto
        {
            PatientName = "New",
            DateOfBirth = DateTime.Today.AddYears(-20),
            Gender = "Male",
            Email = "new@test.com",
            PhoneNo = "+919876543210"
        };

        _repoMock.Setup(x => x.SearchByEmailAsync(dto.Email, default))
            .ReturnsAsync((Patient?)null);

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(patient);

        _repoMock.Setup(x =>
            x.UpdateAsync(1, patient, default))
            .ReturnsAsync(patient);

        _mapperMock.Setup(x => x.Map<PatientDto>(patient))
            .Returns(new PatientDto { PatientId = 1 });

        var result = await _service.UpdateAsync(1, dto);

        result!.PatientId.Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_All_Fields()
    {
        var patient = new Patient
        {
            PatientId = 1,
            Email = "old@test.com"
        };

        var dto = new UpdatePatientDto
        {
            PatientName = "John Updated",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = "Male",
            Email = "updated@test.com",
            PhoneNo = "+919999999999",
            InsuranceID = "INS1234"
        };

        _repoMock.Setup(x => x.SearchByEmailAsync(dto.Email, default))
            .ReturnsAsync((Patient?)null);

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(patient);

        _repoMock.Setup(x =>
            x.UpdateAsync(1, patient, default))
            .ReturnsAsync(patient);

        _mapperMock.Setup(x => x.Map<PatientDto>(patient))
            .Returns(new PatientDto());

        await _service.UpdateAsync(1, dto);

        patient.PatientName.Should().Be("John Updated");
        patient.Email.Should().Be("updated@test.com");
        patient.PhoneNo.Should().Be("+919999999999");
        patient.InsuranceID.Should().Be("INS1234");
    }


}