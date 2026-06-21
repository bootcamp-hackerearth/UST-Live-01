using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using HealthAxisApplicn.Services.Impl;
using HealthAxisApplicn.Repositories;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Dto.Doctors;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DoctorServiceTests
{
    private readonly Mock<IDoctorRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DoctorService _service;

    public DoctorServiceTests()
    {
        _repoMock = new Mock<IDoctorRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new DoctorService(_repoMock.Object, _mapperMock.Object);
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
}