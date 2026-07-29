using AutoMapper;
using FluentAssertions;
using HealthAxisApplicn.Dto.Doctors;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Repositories;
using HealthAxisApplicn.Services.Impl;
using Moq;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class DoctorServiceTests
{
    private readonly Mock<IDoctorRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DoctorService _service;

    public DoctorServiceTests()
    {
        _repoMock = new Mock<IDoctorRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new DoctorService(
            _repoMock.Object,
            _mapperMock.Object);

    }

    [Fact]
    public async Task CreateAsync_Should_CreateDoctor()
    {
        var createDto = new CreateDoctorDto { DoctorName = "Dr John" };
        var doctor = new Doctor { DoctorName = "Dr John" };
        var dto = new DoctorDto { DoctorName = "Dr John" };

        _mapperMock.Setup(m => m.Map<Doctor>(createDto)).Returns(doctor);
        _repoMock.Setup(r => r.CreateAsync(doctor, default)).ReturnsAsync(doctor);
        _mapperMock.Setup(m => m.Map<DoctorDto>(doctor)).Returns(dto);

        var result = await _service.CreateAsync(createDto);

        result.Should().NotBeNull();
        result.DoctorName.Should().Be("Dr John");
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_List()
    {
        var doctors = new List<Doctor> { new Doctor { DoctorId = 1 } };
        var dtos = new List<DoctorDto> { new DoctorDto { DoctorId = 1 } };

        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(doctors);
        _mapperMock.Setup(m => m.Map<List<DoctorDto>>(doctors)).Returns(dtos);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Doctor()
    {
        var doctor = new Doctor { DoctorId = 1 };
        var dto = new DoctorDto { DoctorId = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(doctor);
        _mapperMock.Setup(m => m.Map<DoctorDto?>(doctor)).Returns(dto);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.DoctorId.Should().Be(1);
    }

    [Fact]
    public async Task GetActiveDoctorsAsync_Should_Return_List()
    {
        var doctors = new List<Doctor> { new Doctor { DoctorId = 1, IsActive = true } };
        var dtos = new List<DoctorDto> { new DoctorDto { DoctorId = 1, IsActive = true } };

        _repoMock.Setup(r => r.GetActiveDoctorsAsync(default)).ReturnsAsync(doctors);
        _mapperMock.Setup(m => m.Map<List<DoctorDto>>(doctors)).Returns(dtos);

        var result = await _service.GetActiveDoctorsAsync();

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task DeactivateDoctorAsync_Should_Deactivate_When_Found()
    {
        var doctor = new Doctor { DoctorId = 1, IsActive = true };
        var updated = new Doctor { DoctorId = 1, IsActive = false };
        var dto = new DoctorDto { DoctorId = 1, IsActive = false };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(doctor);
        _repoMock.Setup(r => r.UpdateAsync(1, doctor, default)).ReturnsAsync(updated);
        _mapperMock.Setup(m => m.Map<DoctorDto>(updated)).Returns(dto);

        var result = await _service.DeactivateDoctorAsync(1);

        result.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task DeactivateDoctorAsync_Should_Throw_When_NotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<System.Exception>(() => _service.DeactivateDoctorAsync(1));
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdateDoctor_When_Found()
    {
        var existing = new Doctor { DoctorId = 1, DoctorName = "Old" };

        var updateDto = new UpdateDoctorDto
        {
            DoctorName = "New",
            Specialisation = "Cardiology",
            YearsOfExperience = 5,
            ConsultationFee = 500,
            IsActive = true
        };

        var updated = new Doctor { DoctorId = 1, DoctorName = "New" };
        var resultDto = new DoctorDto { DoctorName = "New" };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.UpdateAsync(1, existing, default)).ReturnsAsync(updated);
        _mapperMock.Setup(m => m.Map<DoctorDto?>(updated)).Returns(resultDto);

        var result = await _service.UpdateAsync(1, updateDto);

        result.DoctorName.Should().Be("New");
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_NotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync((Doctor?)null);

        await Assert.ThrowsAsync<System.Exception>(() => _service.UpdateAsync(1, new UpdateDoctorDto()));
    }

    [Fact]
    public async Task SearchBySpecialisationAsync_Should_Return_List()
    {
        var doctors = new List<Doctor> { new Doctor { Specialisation = "Cardiology" } };
        var dtos = new List<DoctorDto> { new DoctorDto { Specialisation = "Cardiology" } };

        _repoMock.Setup(r => r.SearchBySpecialisationAsync("Cardiology", default)).ReturnsAsync(doctors);
        _mapperMock.Setup(m => m.Map<List<DoctorDto>>(doctors)).Returns(dtos);

        var result = await _service.SearchBySpecialisationAsync("Cardiology");

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task SearchByNameAsync_Should_Return_List()
    {
        var doctors = new List<Doctor> { new Doctor { DoctorName = "Dr John" } };
        var dtos = new List<DoctorDto> { new DoctorDto { DoctorName = "Dr John" } };

        _repoMock.Setup(r => r.SearchByNameAsync("John", default)).ReturnsAsync(doctors);
        _mapperMock.Setup(m => m.Map<List<DoctorDto>>(doctors)).Returns(dtos);

        var result = await _service.SearchByNameAsync("John");

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Doctor_Not_Found()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99, default))
            .ReturnsAsync((Doctor?)null);

        _mapperMock.Setup(m => m.Map<DoctorDto?>((Doctor?)null))
            .Returns((DoctorDto?)null);

        var result = await _service.GetByIdAsync(99);

        result.Should().BeNull();
    }

    [Fact]
    public async Task ToggleActiveAsync_Should_Return_False_When_Doctor_Not_Found()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1, default))
            .ReturnsAsync((Doctor?)null);

        var result = await _service.ToggleActiveAsync(1);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task ToggleActiveAsync_Should_Deactivate_Active_Doctor()
    {
        var doctor = new Doctor
        {
            DoctorId = 1,
            IsActive = true
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default))
            .ReturnsAsync(doctor);

        _repoMock.Setup(r => r.UpdateAsync(1, doctor, default))
            .ReturnsAsync(doctor);

        var result = await _service.ToggleActiveAsync(1);

        result.Should().BeTrue();
        doctor.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task ToggleActiveAsync_Should_Activate_Inactive_Doctor()
    {
        var doctor = new Doctor
        {
            DoctorId = 1,
            IsActive = false
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default))
            .ReturnsAsync(doctor);

        _repoMock.Setup(r => r.UpdateAsync(1, doctor, default))
            .ReturnsAsync(doctor);

        var result = await _service.ToggleActiveAsync(1);

        result.Should().BeTrue();
        doctor.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetByUserIdAsync_Should_Return_Doctor()
    {
        var doctor = new Doctor
        {
            DoctorId = 1,
            UserId = "user123"
        };

        _repoMock.Setup(r => r.GetByUserIdAsync("user123"))
            .ReturnsAsync(doctor);

        var result = await _service.GetByUserIdAsync("user123");

        result.Should().NotBeNull();
        result!.UserId.Should().Be("user123");
    }

    [Fact]
    public async Task GetByUserIdAsync_Should_Return_Null_When_Not_Found()
    {
        _repoMock.Setup(r => r.GetByUserIdAsync("missing"))
            .ReturnsAsync((Doctor?)null);

        var result = await _service.GetByUserIdAsync("missing");

        result.Should().BeNull();
    }

    [Fact]
    public async Task SearchAsync_Should_Return_Doctors()
    {
        var doctors = new List<Doctor>
    {
        new() { DoctorId = 1 }
    };

        var dtos = new List<DoctorDto>
    {
        new() { DoctorId = 1 }
    };

        _repoMock.Setup(r => r.SearchAsync("Cardio", default))
            .ReturnsAsync(doctors);

        _mapperMock.Setup(m => m.Map<List<DoctorDto>>(doctors))
            .Returns(dtos);

        var result = await _service.SearchAsync("Cardio");

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task SearchAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(r => r.SearchAsync("xyz", default))
            .ReturnsAsync(new List<Doctor>());

        _mapperMock.Setup(m => m.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
            .Returns(new List<DoctorDto>());

        var result = await _service.SearchAsync("xyz");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task FilterAsync_Should_Return_Filtered_Doctors()
    {
        var doctors = new List<Doctor>
    {
        new() { DoctorId = 1 }
    };

        var dtos = new List<DoctorDto>
    {
        new() { DoctorId = 1 }
    };

        _repoMock.Setup(r => r.FilterAsync("John", "Cardiology"))
            .ReturnsAsync(doctors);

        _mapperMock.Setup(m => m.Map<List<DoctorDto>>(doctors))
            .Returns(dtos);

        var result = await _service.FilterAsync("John", "Cardiology");

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task FilterAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(r => r.FilterAsync("NoDoctor", "Unknown"))
            .ReturnsAsync(new List<Doctor>());

        _mapperMock.Setup(m => m.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
            .Returns(new List<DoctorDto>());

        var result = await _service.FilterAsync("NoDoctor", "Unknown");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_All_Doctor_Fields()
    {
        var existing = new Doctor
        {
            DoctorId = 1
        };

        var dto = new UpdateDoctorDto
        {
            DoctorName = "Updated",
            Email = "updated@test.com",
            Specialisation = "Cardiology",
            YearsOfExperience = 15,
            ConsultationFee = 1200,
            IsActive = true
        };

        _repoMock.Setup(r => r.GetByIdAsync(1, default))
            .ReturnsAsync(existing);

        _repoMock.Setup(r => r.UpdateAsync(1, existing, default))
            .ReturnsAsync(existing);

        _mapperMock.Setup(m => m.Map<DoctorDto?>(existing))
            .Returns(new DoctorDto());

        await _service.UpdateAsync(1, dto);

        existing.DoctorName.Should().Be("Updated");
        existing.Email.Should().Be("updated@test.com");
        existing.Specialisation.Should().Be("Cardiology");
        existing.YearsOfExperience.Should().Be(15);
        existing.ConsultationFee.Should().Be(1200);
    }

    [Fact]
    public async Task ToggleActiveAsync_Should_Call_Update_Once()
    {
        var doctor = new Doctor
        {
            DoctorId = 1,
            IsActive = true
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(doctor);

        _repoMock.Setup(x =>
            x.UpdateAsync(1, doctor, default))
            .ReturnsAsync(doctor);

        await _service.ToggleActiveAsync(1);

        _repoMock.Verify(
            x => x.UpdateAsync(
                1,
                doctor,
                default),
            Times.Once);
    }

    [Fact]
    public async Task DeactivateDoctorAsync_Should_Set_IsActive_To_False()
    {
        var doctor = new Doctor
        {
            DoctorId = 1,
            IsActive = true
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(doctor);

        _repoMock.Setup(x =>
            x.UpdateAsync(1, doctor, default))
            .ReturnsAsync(doctor);

        _mapperMock.Setup(x =>
            x.Map<DoctorDto>(doctor))
            .Returns(new DoctorDto());

        await _service.DeactivateDoctorAsync(1);

        doctor.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task SearchByNameAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(x =>
            x.SearchByNameAsync("xyz", default))
            .ReturnsAsync(new List<Doctor>());

        _mapperMock.Setup(x =>
            x.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
            .Returns(new List<DoctorDto>());

        var result = await _service.SearchByNameAsync("xyz");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchBySpecialisationAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(x =>
            x.SearchBySpecialisationAsync("Unknown", default))
            .ReturnsAsync(new List<Doctor>());

        _mapperMock.Setup(x =>
            x.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
            .Returns(new List<DoctorDto>());

        var result =
            await _service.SearchBySpecialisationAsync("Unknown");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateAsync_Should_Call_Repository_Once()
    {
        var dto = new CreateDoctorDto();
        var doctor = new Doctor();

        _mapperMock.Setup(x => x.Map<Doctor>(dto))
            .Returns(doctor);

        _repoMock.Setup(x => x.CreateAsync(doctor, default))
            .ReturnsAsync(doctor);

        _mapperMock.Setup(x => x.Map<DoctorDto>(doctor))
            .Returns(new DoctorDto());

        await _service.CreateAsync(dto);

        _repoMock.Verify(
            x => x.CreateAsync(doctor, default),
            Times.Once);
    }

    [Fact]
    public async Task ToggleActiveAsync_Should_Call_GetById_Once()
    {
        var doctor = new Doctor
        {
            DoctorId = 1,
            IsActive = true
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(doctor);

        _repoMock.Setup(x => x.UpdateAsync(1, doctor, default))
            .ReturnsAsync(doctor);

        await _service.ToggleActiveAsync(1);

        _repoMock.Verify(
            x => x.GetByIdAsync(1, default),
            Times.Once);
    }

    [Fact]
    public async Task DeactivateDoctorAsync_Should_Call_Update_Once()
    {
        var doctor = new Doctor
        {
            DoctorId = 1,
            IsActive = true
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(doctor);

        _repoMock.Setup(x => x.UpdateAsync(1, doctor, default))
            .ReturnsAsync(doctor);

        _mapperMock.Setup(x => x.Map<DoctorDto>(doctor))
            .Returns(new DoctorDto());

        await _service.DeactivateDoctorAsync(1);

        _repoMock.Verify(
            x => x.UpdateAsync(1, doctor, default),
            Times.Once);
    }

    [Fact]
    public async Task GetActiveDoctorsAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(x =>
            x.GetActiveDoctorsAsync(default))
            .ReturnsAsync(new List<Doctor>());

        _mapperMock.Setup(x =>
            x.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
            .Returns(new List<DoctorDto>());

        var result = await _service.GetActiveDoctorsAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(x =>
            x.GetAllAsync(default))
            .ReturnsAsync(new List<Doctor>());

        _mapperMock.Setup(x =>
            x.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
            .Returns(new List<DoctorDto>());

        var result = await _service.GetAllAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task FilterAsync_Should_Call_Repository()
    {
        _repoMock.Setup(x =>
            x.FilterAsync("John", "Cardiology"))
            .ReturnsAsync(new List<Doctor>());

        _mapperMock.Setup(x =>
            x.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
            .Returns(new List<DoctorDto>());

        await _service.FilterAsync("John", "Cardiology");

        _repoMock.Verify(
            x => x.FilterAsync("John", "Cardiology"),
            Times.Once);
    }

    [Fact]
    public async Task SearchAsync_Should_Call_Repository()
    {
        _repoMock.Setup(x =>
            x.SearchAsync("Cardio", default))
            .ReturnsAsync(new List<Doctor>());

        _mapperMock.Setup(x =>
            x.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
            .Returns(new List<DoctorDto>());

        await _service.SearchAsync("Cardio");

        _repoMock.Verify(
            x => x.SearchAsync("Cardio", default),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_Set_IsActive_Field()
    {
        var doctor = new Doctor
        {
            DoctorId = 1,
            IsActive = false
        };

        var dto = new UpdateDoctorDto
        {
            DoctorName = "Doctor",
            Email = "doctor@test.com",
            Specialisation = "Cardiology",
            YearsOfExperience = 10,
            ConsultationFee = 500,
            IsActive = true
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(doctor);

        _repoMock.Setup(x => x.UpdateAsync(1, doctor, default))
            .ReturnsAsync(doctor);

        _mapperMock.Setup(x => x.Map<DoctorDto?>(doctor))
            .Returns(new DoctorDto());

        await _service.UpdateAsync(1, dto);

        doctor.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAsync_Should_Call_Mapper_For_Doctor()
    {
        var dto = new CreateDoctorDto();
        var doctor = new Doctor();

        _mapperMock.Setup(x => x.Map<Doctor>(dto))
            .Returns(doctor);

        _repoMock.Setup(x => x.CreateAsync(doctor, default))
            .ReturnsAsync(doctor);

        _mapperMock.Setup(x => x.Map<DoctorDto>(doctor))
            .Returns(new DoctorDto());

        await _service.CreateAsync(dto);

        _mapperMock.Verify(
            x => x.Map<Doctor>(dto),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Should_Call_Mapper_For_Return_Dto()
    {
        var dto = new CreateDoctorDto();
        var doctor = new Doctor();

        _mapperMock.Setup(x => x.Map<Doctor>(dto))
            .Returns(doctor);

        _repoMock.Setup(x => x.CreateAsync(doctor, default))
            .ReturnsAsync(doctor);

        _mapperMock.Setup(x => x.Map<DoctorDto>(doctor))
            .Returns(new DoctorDto());

        await _service.CreateAsync(dto);

        _mapperMock.Verify(
            x => x.Map<DoctorDto>(doctor),
            Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_Should_Call_Repository_Once()
    {
        _repoMock.Setup(x =>
            x.GetAllAsync(default))
            .ReturnsAsync(new List<Doctor>());

        _mapperMock.Setup(x =>
            x.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
            .Returns(new List<DoctorDto>());

        await _service.GetAllAsync();

        _repoMock.Verify(
            x => x.GetAllAsync(default),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Call_Repository()
    {
        _repoMock.Setup(x =>
            x.GetByIdAsync(1, default))
            .ReturnsAsync(new Doctor());

        _mapperMock.Setup(x =>
            x.Map<DoctorDto?>(It.IsAny<Doctor>()))
            .Returns(new DoctorDto());

        await _service.GetByIdAsync(1);

        _repoMock.Verify(
            x => x.GetByIdAsync(1, default),
            Times.Once);
    }

    [Fact]
    public async Task GetActiveDoctorsAsync_Should_Call_Repository()
    {
        _repoMock.Setup(x =>
            x.GetActiveDoctorsAsync(default))
            .ReturnsAsync(new List<Doctor>());

        _mapperMock.Setup(x =>
            x.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
            .Returns(new List<DoctorDto>());

        await _service.GetActiveDoctorsAsync();

        _repoMock.Verify(
            x => x.GetActiveDoctorsAsync(default),
            Times.Once);
    }

    [Fact]
    public async Task SearchByNameAsync_Should_Call_Repository()
    {
        _repoMock.Setup(x =>
            x.SearchByNameAsync("John", default))
            .ReturnsAsync(new List<Doctor>());

        _mapperMock.Setup(x =>
            x.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
            .Returns(new List<DoctorDto>());

        await _service.SearchByNameAsync("John");

        _repoMock.Verify(
            x => x.SearchByNameAsync("John", default),
            Times.Once);
    }

    [Fact]
    public async Task SearchBySpecialisationAsync_Should_Call_Repository()
    {
        _repoMock.Setup(x =>
            x.SearchBySpecialisationAsync(
                "Cardiology",
                default))
            .ReturnsAsync(new List<Doctor>());

        _mapperMock.Setup(x =>
            x.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
            .Returns(new List<DoctorDto>());

        await _service.SearchBySpecialisationAsync(
            "Cardiology");

        _repoMock.Verify(
            x => x.SearchBySpecialisationAsync(
                "Cardiology",
                default),
            Times.Once);
    }

    [Fact]
    public async Task GetByUserIdAsync_Should_Call_Repository()
    {
        _repoMock.Setup(x =>
            x.GetByUserIdAsync("user1"))
            .ReturnsAsync(new Doctor());

        await _service.GetByUserIdAsync("user1");

        _repoMock.Verify(
            x => x.GetByUserIdAsync("user1"),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_Call_Update_Repository()
    {
        var doctor = new Doctor { DoctorId = 1 };

        var dto = new UpdateDoctorDto
        {
            DoctorName = "Updated",
            Specialisation = "Cardiology",
            Email = "doctor@test.com",
            YearsOfExperience = 10,
            ConsultationFee = 1000,
            IsActive = true
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(doctor);

        _repoMock.Setup(x => x.UpdateAsync(1, doctor, default))
            .ReturnsAsync(doctor);

        _mapperMock.Setup(x => x.Map<DoctorDto?>(doctor))
            .Returns(new DoctorDto());

        await _service.UpdateAsync(1, dto);

        _repoMock.Verify(
            x => x.UpdateAsync(1, doctor, default),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Mapped_Dto()
    {
        var doctor = new Doctor { DoctorId = 1 };

        var dto = new UpdateDoctorDto
        {
            DoctorName = "Updated"
        };

        var returnedDto = new DoctorDto
        {
            DoctorName = "Updated"
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(doctor);

        _repoMock.Setup(x => x.UpdateAsync(1, doctor, default))
            .ReturnsAsync(doctor);

        _mapperMock.Setup(x => x.Map<DoctorDto?>(doctor))
            .Returns(returnedDto);

        var result = await _service.UpdateAsync(1, dto);

        result!.DoctorName.Should().Be("Updated");
    }

    [Fact]
    public async Task FilterAsync_Should_Call_Mapper()
    {
        var doctors = new List<Doctor>();

        _repoMock.Setup(x =>
            x.FilterAsync("John", "Cardiology"))
            .ReturnsAsync(doctors);

        _mapperMock.Setup(x =>
            x.Map<List<DoctorDto>>(doctors))
            .Returns(new List<DoctorDto>());

        await _service.FilterAsync("John", "Cardiology");

        _mapperMock.Verify(
            x => x.Map<List<DoctorDto>>(doctors),
            Times.Once);
    }

    [Fact]
    public async Task SearchAsync_Should_Call_Mapper()
    {
        var doctors = new List<Doctor>();

        _repoMock.Setup(x =>
            x.SearchAsync("test", default))
            .ReturnsAsync(doctors);

        _mapperMock.Setup(x =>
            x.Map<List<DoctorDto>>(doctors))
            .Returns(new List<DoctorDto>());

        await _service.SearchAsync("test");

        _mapperMock.Verify(
            x => x.Map<List<DoctorDto>>(doctors),
            Times.Once);
    }

    [Fact]
    public async Task GetActiveDoctorsAsync_Should_Call_Mapper()
    {
        var doctors = new List<Doctor>();

        _repoMock.Setup(x =>
            x.GetActiveDoctorsAsync(default))
            .ReturnsAsync(doctors);

        _mapperMock.Setup(x =>
            x.Map<List<DoctorDto>>(doctors))
            .Returns(new List<DoctorDto>());

        await _service.GetActiveDoctorsAsync();

        _mapperMock.Verify(
            x => x.Map<List<DoctorDto>>(doctors),
            Times.Once);
    }


}