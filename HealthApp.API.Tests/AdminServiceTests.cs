using AutoMapper;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.AspNetCore.Identity;

using HealthApp.API.Models;
using HealthApp.API.Exceptions;
using HealthApp.API.Identity;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Constants;

namespace HealthApp.API.Tests;

public class AdminServiceTests
{
    private readonly Mock<IDoctorRepository> _doctorRepo;
    private readonly Mock<IAppointmentRepository> _appointmentRepo;
    private readonly Mock<UserManager<ApplicationUser>> _userManager;
    private readonly Mock<IMapper> _mapper;

    private readonly AdminService _service;

    public AdminServiceTests()
    {
        _doctorRepo = new Mock<IDoctorRepository>();
        _appointmentRepo = new Mock<IAppointmentRepository>();
        _mapper = new Mock<IMapper>();

        var userStore = new Mock<IUserStore<ApplicationUser>>();
        _userManager = new Mock<UserManager<ApplicationUser>>(
            userStore.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _service = new AdminService(
            _doctorRepo.Object,
            _appointmentRepo.Object,
            _userManager.Object,
            _mapper.Object);
    }

    #region GetDoctorsAsync

    [Fact]
    public async Task GetDoctorsAsync_ShouldReturnDoctors()
    {
        var doctors = new List<Doctor>
        {
            new Doctor { DoctorId = 1, DoctorName = "Dr A" },
            new Doctor { DoctorId = 2, DoctorName = "Dr B" }
        };

        var dtos = new List<DoctorDto>
        {
            new DoctorDto { DoctorId = 1 },
            new DoctorDto { DoctorId = 2 }
        };

        _doctorRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(doctors);
        _mapper.Setup(x => x.Map<List<DoctorDto>>(doctors)).Returns(dtos);

        var result = await _service.GetDoctorsAsync();

        Assert.Equal(2, result.Count);
        _doctorRepo.Verify(x => x.GetAllAsync(), Times.Once);
    }

    #endregion

    #region CreateDoctorAsync

    [Fact]
    public async Task CreateDoctorAsync_ShouldThrow_WhenEmailExists()
    {
        var dto = GetCreateDto();

        _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync(new ApplicationUser());

        await Assert.ThrowsAsync<ConflictException>(() =>
            _service.CreateDoctorAsync(dto));
    }

    [Fact]
    public async Task CreateDoctorAsync_ShouldCreateDoctor_WhenValid()
    {
        var dto = GetCreateDto();

        var doctor = new Doctor { DoctorId = 1 };
        var dtoResult = new DoctorDto { DoctorId = 1 };

        _userManager.Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync((ApplicationUser)null);

        _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.TemporaryPassword))
            .ReturnsAsync(IdentityResult.Success);

        _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), Roles.Doctor))
            .ReturnsAsync(IdentityResult.Success);

        _mapper.Setup(x => x.Map<Doctor>(dto)).Returns(new Doctor());

        _doctorRepo.Setup(x => x.AddAsync(It.IsAny<Doctor>()))
            .ReturnsAsync(doctor);

        _mapper.Setup(x => x.Map<DoctorDto>(doctor)).Returns(dtoResult);

        var result = await _service.CreateDoctorAsync(dto);

        Assert.NotNull(result);
        Assert.Equal(1, result.DoctorId);

        _doctorRepo.Verify(x => x.AddAsync(It.IsAny<Doctor>()), Times.Once);
    }

    #endregion

    #region UpdateDoctorAsync

    [Fact]
    public async Task UpdateDoctorAsync_ShouldThrow_WhenDoctorNotFound()
    {
        _doctorRepo.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Doctor)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _service.UpdateDoctorAsync(1, GetUpdateDto()));
    }

    [Fact]
    public async Task UpdateDoctorAsync_ShouldUpdateDoctor()
    {
        var existing = new Doctor
        {
            DoctorId = 1,
            UserId = "user1",
            CreatedDate = DateTime.Now.AddYears(-3)
        };

        var updated = new Doctor { DoctorId = 1 };

        _doctorRepo.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existing);

        _mapper.Setup(x => x.Map<Doctor>(It.IsAny<UpdateDoctorDto>()))
            .Returns(new Doctor());

        _doctorRepo.Setup(x => x.UpdateAsync(1, It.IsAny<Doctor>()))
            .ReturnsAsync(updated);

        _mapper.Setup(x => x.Map<DoctorDto>(updated))
            .Returns(new DoctorDto { DoctorId = 1 });

        var result = await _service.UpdateDoctorAsync(1, GetUpdateDto());

        Assert.Equal(1, result.DoctorId);
    }

    #endregion

    #region Appointment Reports

    [Fact]
    public async Task GetAppointmentReportsAsync_ShouldReturnReport()
    {
        var data = new List<Appointment>
        {
            new Appointment { ScheduledDate = DateTime.Today, Status = "Pending" },
            new Appointment { ScheduledDate = DateTime.Today, Status = "Confirmed" }
        };

        _appointmentRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(data);

        var result = await _service.GetAppointmentReportsAsync();

        Assert.Single(result);
        Assert.Equal(1, result[0].Pending);
        Assert.Equal(1, result[0].Confirmed);
    }

    #endregion

    #region Helpers

    private static CreateDoctorDto GetCreateDto()
    {
        return new CreateDoctorDto
        {
            FullName = "Dr Test",
            Email = "doc@test.com",
            TemporaryPassword = "Pass@123",
            PracticeStartDate = DateTime.Today.AddYears(-5),
            ConsultationFee = 500
        };
    }

    private static UpdateDoctorDto GetUpdateDto()
    {
        return new UpdateDoctorDto
        {
            FullName = "Updated Doctor",
            PracticeStartDate = DateTime.Today.AddYears(-2),
            ConsultationFee = 800
        };
    }

    #endregion
}