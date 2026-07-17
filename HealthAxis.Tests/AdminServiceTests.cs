using FluentAssertions;
using HealthAxis.API.Data;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementation;
using HealthAxis.Shared.DTO;
using HealthAxis.Shared.DTO.AdminDtos;
using HealthAxis.Shared.DTO.CommonDtos;
using HealthAxis.Shared.DTO.DoctorDtos;
using HealthAxis.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Collections;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;

using ApiValidationException = HealthAxis.API.Exceptions.ValidationException;

namespace HealthAxis.API.Tests.Services
{
    public class AdminServiceTests : IDisposable
    {
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly ApplicationDbContext _context;
        private readonly AdminService _service;

        public AdminServiceTests()
        {
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            var users = new List<IdentityUser>
            {
                new()
                {
                    Id = "admin-user-1",
                    Email = "admin@gmail.com",
                    UserName = "admin@gmail.com",
                    PhoneNumber = "9999999999"
                },
                new()
                {
                    Id = "doctor-user-1",
                    Email = "doctor@gmail.com",
                    UserName = "doctor@gmail.com"
                },
                new()
                {
                    Id = "patient-user-1",
                    Email = "patient@gmail.com",
                    UserName = "patient@gmail.com",
                    PhoneNumber = "9876543210"
                }
            };

            var roles = new Dictionary<string, IList<string>>
            {
                { "admin-user-1", new List<string> { "Admin" } },
                { "doctor-user-1", new List<string> { "Doctor" } },
                { "patient-user-1", new List<string> { "Patient" } }
            };

            _userManagerMock = MockUserManager(users, roles);
            _roleManagerMock = MockRoleManager();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _service = new AdminService(
                _doctorRepositoryMock.Object,
                _appointmentRepositoryMock.Object,
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _context);
        }

        [Fact]
        public async Task GetAllDoctorsAsync_WhenDoctorsExist_ReturnsDoctorDtos()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>
                {
                    CreateDoctor(id: 1, fullName: "Dr John"),
                    CreateDoctor(id: 2, fullName: "Dr Smith", userId: "doctor-user-2")
                });

            var result = await _service.GetAllDoctorsAsync();

            result.Should().HaveCount(2);
            result[0].FullName.Should().Be("Dr John");
            result[1].FullName.Should().Be("Dr Smith");
        }

        [Fact]
        public async Task GetAllDoctorsAsync_WhenNoDoctors_ReturnsEmptyList()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>());

            var result = await _service.GetAllDoctorsAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task AddDoctorAsync_WhenDoctorNameIsEmpty_ThrowsValidationException(string fullName)
        {
            var dto = CreateCreateDoctorDto();
            dto.FullName = fullName;

            Func<Task> act = async () => await _service.AddDoctorAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Theory]
        [InlineData("Dr John123")]
        [InlineData("Dr @John")]
        public async Task AddDoctorAsync_WhenDoctorNameHasInvalidCharacters_ThrowsValidationException(string fullName)
        {
            var dto = CreateCreateDoctorDto();
            dto.FullName = fullName;

            Func<Task> act = async () => await _service.AddDoctorAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task AddDoctorAsync_WhenEmailIsEmpty_ThrowsValidationException(string email)
        {
            var dto = CreateCreateDoctorDto();
            dto.Email = email;

            Func<Task> act = async () => await _service.AddDoctorAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task AddDoctorAsync_WhenSpecialisationIsInvalid_ThrowsValidationException()
        {
            var dto = CreateCreateDoctorDto();
            dto.Specialisation = (Specialisation)999;

            Func<Task> act = async () => await _service.AddDoctorAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(61)]
        public async Task AddDoctorAsync_WhenYearsOfExperienceInvalid_ThrowsValidationException(int yearsOfExperience)
        {
            var dto = CreateCreateDoctorDto();
            dto.YearsOfExperience = yearsOfExperience;

            Func<Task> act = async () => await _service.AddDoctorAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Theory]
        [InlineData(99)]
        [InlineData(10001)]
        public async Task AddDoctorAsync_WhenConsultationFeeInvalid_ThrowsValidationException(decimal fee)
        {
            var dto = CreateCreateDoctorDto();
            dto.ConsultationFee = fee;

            Func<Task> act = async () => await _service.AddDoctorAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task AddDoctorAsync_WhenDoctorEmailAlreadyExists_ThrowsBusinessRuleException()
        {
            var dto = CreateCreateDoctorDto();
            dto.Email = "doctor@gmail.com";

            Func<Task> act = async () => await _service.AddDoctorAsync(dto);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task AddDoctorAsync_WhenIdentityCreateFails_ThrowsValidationException()
        {
            var dto = CreateCreateDoctorDto();
            dto.Email = "newdoctor@gmail.com";

            _userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError { Description = "Password error" }));

            Func<Task> act = async () => await _service.AddDoctorAsync(dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task AddDoctorAsync_WhenDoctorRoleDoesNotExist_CreatesRoleAndDoctor()
        {
            var dto = CreateCreateDoctorDto();
            dto.Email = "newdoctor@gmail.com";

            _roleManagerMock
                .Setup(manager => manager.RoleExistsAsync("Doctor"))
                .ReturnsAsync(false);

            _roleManagerMock
                .Setup(manager => manager.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            _doctorRepositoryMock
                .Setup(repository => repository.AddAsync(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor doctor, CancellationToken _) =>
                {
                    doctor.DoctorId = 10;
                    return doctor;
                });

            var result = await _service.AddDoctorAsync(dto);

            result.DoctorId.Should().Be(10);
            result.Email.Should().Be("newdoctor@gmail.com");

            _roleManagerMock.Verify(
                manager => manager.CreateAsync(It.Is<IdentityRole>(role => role.Name == "Doctor")),
                Times.Once);
        }

        [Fact]
        public async Task AddDoctorAsync_WhenValid_ReturnsCreatedDoctor()
        {
            var dto = CreateCreateDoctorDto();
            dto.Email = "newdoctor@gmail.com";

            _doctorRepositoryMock
                .Setup(repository => repository.AddAsync(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor doctor, CancellationToken _) =>
                {
                    doctor.DoctorId = 10;
                    return doctor;
                });

            var result = await _service.AddDoctorAsync(dto);

            result.Should().NotBeNull();
            result.DoctorId.Should().Be(10);
            result.FullName.Should().Be(dto.FullName);
            result.Email.Should().Be(dto.Email);
            result.TemporaryPassword.Should().StartWith("Doctor@");

            _doctorRepositoryMock.Verify(
                repository => repository.AddAsync(It.IsAny<Doctor>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenDoctorNotFound_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            Func<Task> act = async () =>
                await _service.UpdateDoctorAsync(1, CreateUpdateDoctorDto());

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task UpdateDoctorAsync_WhenFullNameEmpty_ThrowsValidationException(string fullName)
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateDoctor());

            var dto = CreateUpdateDoctorDto();
            dto.FullName = fullName;

            Func<Task> act = async () => await _service.UpdateDoctorAsync(1, dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(61)]
        public async Task UpdateDoctorAsync_WhenYearsOfExperienceInvalid_ThrowsValidationException(int yearsOfExperience)
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateDoctor());

            var dto = CreateUpdateDoctorDto();
            dto.YearsOfExperience = yearsOfExperience;

            Func<Task> act = async () => await _service.UpdateDoctorAsync(1, dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task UpdateDoctorAsync_WhenConsultationFeeInvalid_ThrowsValidationException(decimal fee)
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateDoctor());

            var dto = CreateUpdateDoctorDto();
            dto.ConsultationFee = fee;

            Func<Task> act = async () => await _service.UpdateDoctorAsync(1, dto);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenValid_ReturnsUpdatedDoctor()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreateDoctor());

            _doctorRepositoryMock
                .Setup(repository => repository.UpdateAsync(1, It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int _, Doctor doctor, CancellationToken _) => doctor);

            var dto = CreateUpdateDoctorDto();
            dto.FullName = "Dr Updated";

            var result = await _service.UpdateDoctorAsync(1, dto);

            result.Should().NotBeNull();
            result.FullName.Should().Be("Dr Updated");
            result.YearsOfExperience.Should().Be(dto.YearsOfExperience);
            result.ConsultationFee.Should().Be(dto.ConsultationFee);
        }

        [Fact]
        public async Task GetAppointmentReportsAsync_WhenNoAppointments_ReturnsEmptyList()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            var result = await _service.GetAppointmentReportsAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAppointmentReportsAsync_WhenAppointmentsExist_ReturnsGroupedReports()
        {
            var today = DateTime.Today;
            var tomorrow = DateTime.Today.AddDays(1);

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>
                {
                    new() { AppointmentId = 1, ScheduledDate = today, Status = AppointmentStatus.Confirmed },
                    new() { AppointmentId = 2, ScheduledDate = today, Status = AppointmentStatus.Cancelled },
                    new() { AppointmentId = 3, ScheduledDate = today, Status = AppointmentStatus.Completed },
                    new() { AppointmentId = 4, ScheduledDate = today, Status = AppointmentStatus.Pending },
                    new() { AppointmentId = 5, ScheduledDate = tomorrow, Status = AppointmentStatus.Confirmed }
                });

            var result = await _service.GetAppointmentReportsAsync();

            result.Should().HaveCount(2);
            result[0].Date.Should().Be(today);
            result[0].ConfirmedCount.Should().Be(1);
            result[0].CancelledCount.Should().Be(1);
            result[0].CompletedCount.Should().Be(1);
            result[1].Date.Should().Be(tomorrow);
            result[1].ConfirmedCount.Should().Be(1);
        }

        [Fact]
        public async Task GetDoctorsPagedAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            Func<Task> act = async () => await _service.GetDoctorsPagedAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        //[Fact]
        //public async Task GetDoctorsPagedAsync_WhenValid_ReturnsPagedDoctors()
        //{
        //    _doctorRepositoryMock
        //        .Setup(repository => repository.CountAsync())
        //        .ReturnsAsync(1);

        //    _doctorRepositoryMock
        //        .Setup(repository => repository.GetPagedAsync(
        //            It.IsAny<PaginationQueryDto>(),
        //            It.IsAny<Func<Doctor, object>>(),
        //            true))
        //        .ReturnsAsync(new List<Doctor>
        //        {
        //            CreateDoctor()
        //        });

        //    var result = await _service.GetDoctorsPagedAsync(new PaginationQueryDto
        //    {
        //        PageNumber = 0,
        //        PageSize = 100
        //    });

        //    result.Should().NotBeNull();
        //    result.PageNumber.Should().Be(1);
        //    result.PageSize.Should().Be(6);
        //    result.TotalRecords.Should().Be(1);
        //    result.Items.Should().HaveCount(1);
        //    result.Items[0].Email.Should().Be("doctor@gmail.com");
        //}

        [Fact]
        public async Task GetUsersAsync_WhenUsersExist_ReturnsMappedUsers()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>
                {
                    CreateDoctor()
                });

            _context.Patients.Add(CreatePatient());
            await _context.SaveChangesAsync();

            var result = await _service.GetUsersAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().Contain(user => user.Role == "Admin");
            result.Should().Contain(user => user.Role == "Doctor");
            result.Should().Contain(user => user.Role == "Patient");
        }

        [Fact]
        public async Task GetUsersPagedAsync_WhenAdminUserQueryIsNull_ThrowsArgumentNullException()
        {
            Func<Task> act = async () => await _service.GetUsersPagedAsync((AdminUserQueryDto)null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetUsersPagedAsync_WhenSearchAndRoleGiven_ReturnsFilteredUsers()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>
                {
                    CreateDoctor()
                });

            _context.Patients.Add(CreatePatient());
            await _context.SaveChangesAsync();

            var result = await _service.GetUsersPagedAsync(new AdminUserQueryDto
            {
                PageNumber = 1,
                SearchText = "doctor",
                Role = "Doctor"
            });

            result.Should().NotBeNull();
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(6);
            result.TotalRecords.Should().Be(1);
            result.Items.Should().ContainSingle();
            result.Items[0].Role.Should().Be("Doctor");
        }

        [Fact]
        public async Task GetUsersPagedAsync_WhenSearchIsEmpty_ReturnsAllUsers()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>
                {
                    CreateDoctor()
                });

            _context.Patients.Add(CreatePatient());
            await _context.SaveChangesAsync();

            var result = await _service.GetUsersPagedAsync(new AdminUserQueryDto
            {
                PageNumber = 0,
                SearchText = "",
                Role = ""
            });

            result.Should().NotBeNull();
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(6);
            result.TotalRecords.Should().Be(3);
        }

        [Fact]
        public async Task GetUsersPagedAsync_WhenPaginationQueryIsNull_ThrowsArgumentNullException()
        {
            Func<Task> act = async () => await _service.GetUsersPagedAsync((PaginationQueryDto)null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetUsersPagedAsync_WhenPaginationQueryValid_ReturnsPagedUsers()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>
                {
                    CreateDoctor()
                });

            var result = await _service.GetUsersPagedAsync(new PaginationQueryDto
            {
                PageNumber = 0,
                PageSize = 100
            });

            result.Should().NotBeNull();
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(6);
            result.TotalRecords.Should().Be(3);
            result.Items.Should().HaveCount(3);
        }

        [Fact]
        public async Task UpdateAppointmentStatusByAdminAsync_WhenDtoIsNull_ThrowsArgumentNullException()
        {
            Func<Task> act = async () =>
                await _service.UpdateAppointmentStatusByAdminAsync(1, null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateAppointmentStatusByAdminAsync_WhenAppointmentNotFound_ThrowsValidationException()
        {
            Func<Task> act = async () =>
                await _service.UpdateAppointmentStatusByAdminAsync(
                    99,
                    new AdminUpdateAppointmentStatusDto { Status = "Confirmed" });

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task UpdateAppointmentStatusByAdminAsync_WhenStatusInvalid_ThrowsValidationException()
        {
            await SeedAppointmentData(AppointmentStatus.Pending);

            Func<Task> act = async () =>
                await _service.UpdateAppointmentStatusByAdminAsync(
                    1,
                    new AdminUpdateAppointmentStatusDto { Status = "WrongStatus" });

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Theory]
        [InlineData(AppointmentStatus.Completed)]
        [InlineData(AppointmentStatus.Cancelled)]
        public async Task UpdateAppointmentStatusByAdminAsync_WhenCurrentStatusIsFinal_ThrowsValidationException(
            AppointmentStatus status)
        {
            await SeedAppointmentData(status);

            Func<Task> act = async () =>
                await _service.UpdateAppointmentStatusByAdminAsync(
                    1,
                    new AdminUpdateAppointmentStatusDto { Status = "Confirmed" });

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task UpdateAppointmentStatusByAdminAsync_WhenPendingToCompleted_ThrowsValidationException()
        {
            await SeedAppointmentData(AppointmentStatus.Pending);

            Func<Task> act = async () =>
                await _service.UpdateAppointmentStatusByAdminAsync(
                    1,
                    new AdminUpdateAppointmentStatusDto { Status = "Completed" });

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task UpdateAppointmentStatusByAdminAsync_WhenConfirmedToPending_ThrowsValidationException()
        {
            await SeedAppointmentData(AppointmentStatus.Confirmed);

            Func<Task> act = async () =>
                await _service.UpdateAppointmentStatusByAdminAsync(
                    1,
                    new AdminUpdateAppointmentStatusDto { Status = "Pending" });

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task UpdateAppointmentStatusByAdminAsync_WhenPendingToConfirmed_ReturnsUpdatedDetail()
        {
            await SeedAppointmentData(AppointmentStatus.Pending);

            var result = await _service.UpdateAppointmentStatusByAdminAsync(
                1,
                new AdminUpdateAppointmentStatusDto { Status = "Confirmed" });

            result.Should().NotBeNull();
            result.AppointmentId.Should().Be(1);
            result.Status.Should().Be("Confirmed");
            result.PatientName.Should().Be("Mona");
            result.DoctorName.Should().Be("Dr John");
        }

        [Fact]
        public async Task UpdateAppointmentStatusByAdminAsync_WhenConfirmedToCompleted_ReturnsUpdatedDetail()
        {
            await SeedAppointmentData(AppointmentStatus.Confirmed);

            var result = await _service.UpdateAppointmentStatusByAdminAsync(
                1,
                new AdminUpdateAppointmentStatusDto { Status = "Completed" });

            result.Should().NotBeNull();
            result.AppointmentId.Should().Be(1);
            result.Status.Should().Be("Completed");
        }

        [Fact]
        public async Task GetAppointmentDetailsAsync_WhenNoAppointments_ReturnsEmptyList()
        {
            var result = await _service.GetAppointmentDetailsAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAppointmentDetailsAsync_WhenDataExists_ReturnsDetails()
        {
            await SeedAppointmentData(AppointmentStatus.Pending);

            var result = await _service.GetAppointmentDetailsAsync();

            result.Should().HaveCount(1);
            result[0].AppointmentId.Should().Be(1);
            result[0].PatientName.Should().Be("Mona");
            result[0].DoctorName.Should().Be("Dr John");
        }

        [Fact]
        public async Task GetAdminProfileAsync_WhenUserNotFound_ThrowsNotFoundException()
        {
            Func<Task> act = async () => await _service.GetAdminProfileAsync("missing-user");

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetAdminProfileAsync_WhenUserExists_ReturnsProfile()
        {
            var result = await _service.GetAdminProfileAsync("admin-user-1");

            result.Should().NotBeNull();
            result.UserId.Should().Be("admin-user-1");
            result.Email.Should().Be("admin@gmail.com");
            result.UserName.Should().Be("admin@gmail.com");
            result.PhoneNumber.Should().Be("9999999999");
        }

        [Fact]
        public async Task UpdateAdminProfileAsync_WhenDtoIsNull_ThrowsArgumentNullException()
        {
            Func<Task> act = async () =>
                await _service.UpdateAdminProfileAsync("admin-user-1", null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateAdminProfileAsync_WhenUserNotFound_ThrowsNotFoundException()
        {
            Func<Task> act = async () =>
                await _service.UpdateAdminProfileAsync(
                    "missing-user",
                    new UpdateAdminProfileDto
                    {
                        Email = "newadmin@gmail.com",
                        PhoneNumber = "8888888888"
                    });

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateAdminProfileAsync_WhenIdentityUpdateFails_ThrowsValidationException()
        {
            _userManagerMock
                .Setup(manager => manager.UpdateAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError { Description = "Update failed" }));

            Func<Task> act = async () =>
                await _service.UpdateAdminProfileAsync(
                    "admin-user-1",
                    new UpdateAdminProfileDto
                    {
                        Email = "updatedadmin@gmail.com",
                        PhoneNumber = "8888888888"
                    });

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task UpdateAdminProfileAsync_WhenValid_ReturnsUpdatedProfile()
        {
            var result = await _service.UpdateAdminProfileAsync(
                "admin-user-1",
                new UpdateAdminProfileDto
                {
                    Email = "updatedadmin@gmail.com",
                    PhoneNumber = "8888888888"
                });

            result.Should().NotBeNull();
            result.UserId.Should().Be("admin-user-1");
            result.Email.Should().Be("updatedadmin@gmail.com");
            result.UserName.Should().Be("updatedadmin@gmail.com");
            result.PhoneNumber.Should().Be("8888888888");
        }

        [Fact]
        public async Task ChangeAdminPasswordAsync_WhenDtoIsNull_ThrowsArgumentNullException()
        {
            Func<Task> act = async () =>
                await _service.ChangeAdminPasswordAsync("admin-user-1", null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ChangeAdminPasswordAsync_WhenUserNotFound_ThrowsNotFoundException()
        {
            Func<Task> act = async () =>
                await _service.ChangeAdminPasswordAsync(
                    "missing-user",
                    CreateChangePasswordDto());

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ChangeAdminPasswordAsync_WhenIdentityFails_ThrowsValidationException()
        {
            _userManagerMock
                .Setup(manager => manager.ChangePasswordAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError { Description = "Password failed" }));

            Func<Task> act = async () =>
                await _service.ChangeAdminPasswordAsync(
                    "admin-user-1",
                    CreateChangePasswordDto());

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task ChangeAdminPasswordAsync_WhenValid_DoesNotThrow()
        {
            Func<Task> act = async () =>
                await _service.ChangeAdminPasswordAsync(
                    "admin-user-1",
                    CreateChangePasswordDto());

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task GetPatientsAsync_WhenNoPatients_ReturnsEmptyList()
        {
            var result = await _service.GetPatientsAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetPatientsAsync_WhenPatientsExist_ReturnsPatientsOrderedByName()
        {
            _context.Patients.AddRange(
                CreatePatient(id: 2, fullName: "Zara", userId: "patient-user-2"),
                CreatePatient(id: 1, fullName: "Mona", userId: "patient-user-1"));

            await _context.SaveChangesAsync();

            var result = await _service.GetPatientsAsync();

            result.Should().HaveCount(2);
            result[0].FullName.Should().Be("Mona");
            result[1].FullName.Should().Be("Zara");
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenDtoIsNull_ThrowsArgumentNullException()
        {
            Func<Task> act = async () => await _service.UpdatePatientAsync(1, null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenPatientNotFound_ThrowsNotFoundException()
        {
            Func<Task> act = async () =>
                await _service.UpdatePatientAsync(99, CreateUpdateAdminPatientDto());

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenValid_ReturnsUpdatedPatient()
        {
            _context.Patients.Add(CreatePatient());
            await _context.SaveChangesAsync();

            var dto = CreateUpdateAdminPatientDto();
            dto.FullName = "Mona Updated";
            dto.Email = "updatedpatient@gmail.com";
            dto.PhoneNumber = "8888888888";

            var result = await _service.UpdatePatientAsync(1, dto);

            result.Should().NotBeNull();
            result.PatientId.Should().Be(1);
            result.FullName.Should().Be("Mona Updated");
            result.Email.Should().Be("updatedpatient@gmail.com");
            result.PhoneNumber.Should().Be("8888888888");
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenGenderInvalid_KeepsExistingGender()
        {
            _context.Patients.Add(CreatePatient());
            await _context.SaveChangesAsync();

            var dto = CreateUpdateAdminPatientDto();
            dto.Gender = "WrongGender";

            var result = await _service.UpdatePatientAsync(1, dto);

            result.Should().NotBeNull();
            result.Gender.Should().Be(Gender.Female.ToString());
        }

        [Fact]
        public async Task GetPatientAppointmentsAsync_WhenNoAppointments_ReturnsEmptyList()
        {
            var result = await _service.GetPatientAppointmentsAsync(1);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetPatientAppointmentsAsync_WhenAppointmentsExist_ReturnsPatientAppointments()
        {
            await SeedAppointmentData(AppointmentStatus.Confirmed);

            var result = await _service.GetPatientAppointmentsAsync(1);

            result.Should().HaveCount(1);
            result[0].AppointmentId.Should().Be(1);
            result[0].DoctorName.Should().Be("Dr John");
            result[0].Specialisation.Should().Be(Specialisation.Cardiology.ToString());
            result[0].Status.Should().Be("Confirmed");
        }

        private async Task SeedAppointmentData(AppointmentStatus status)
        {
            if (!_context.Patients.Any())
            {
                _context.Patients.Add(CreatePatient());
            }

            if (!_context.Doctors.Any())
            {
                _context.Doctors.Add(CreateDoctor());
            }

            if (!_context.Appointments.Any())
            {
                _context.Appointments.Add(new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = DateTime.Today.AddDays(1),
                    TimeSlot = "10:00 AM - 11:00 AM",
                    Status = status
                });
            }

            await _context.SaveChangesAsync();
        }

        private static Doctor CreateDoctor(
            int id = 1,
            string fullName = "Dr John",
            string userId = "doctor-user-1")
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = fullName,
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true,
                UserId = userId
            };
        }

        private static Patient CreatePatient(
            int id = 1,
            string fullName = "Mona",
            string userId = "patient-user-1")
        {
            return new Patient
            {
                PatientId = id,
                FullName = fullName,
                DateOfBirth = new DateTime(2004, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "9876543210",
                Email = "patient@gmail.com",
                UserId = userId,
                CreatedDate = DateTime.Today
            };
        }

        private static CreateDoctorDto CreateCreateDoctorDto()
        {
            return new CreateDoctorDto
            {
                FullName = "John Smith",
                Email = "newdoctor@gmail.com",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };
        }

        private static UpdateDoctorDto CreateUpdateDoctorDto()
        {
            return new UpdateDoctorDto
            {
                FullName = "Dr John",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };
        }

        private static UpdateAdminPatientDto CreateUpdateAdminPatientDto()
        {
            return new UpdateAdminPatientDto
            {
                FullName = "Mona",
                DateOfBirth = new DateTime(2004, 1, 1),
                Gender = Gender.Female.ToString(),
                PhoneNumber = "9876543210",
                Email = "patient@gmail.com",
                Address = "Hyderabad"
            };
        }

        private static ChangePasswordDto CreateChangePasswordDto()
        {
            return new ChangePasswordDto
            {
                CurrentPassword = "Old@123",
                NewPassword = "New@123",
                ConfirmNewPassword = "New@123"
            };
        }

        private static Mock<UserManager<IdentityUser>> MockUserManager(
            List<IdentityUser>? users = null,
            Dictionary<string, IList<string>>? rolesByUserId = null)
        {
            users ??= new List<IdentityUser>();
            rolesByUserId ??= new Dictionary<string, IList<string>>();

            var store = new Mock<IUserStore<IdentityUser>>();

            var mock = new Mock<UserManager<IdentityUser>>(
                store.Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!);

            mock.Setup(manager => manager.Users)
                .Returns(new TestAsyncEnumerable<IdentityUser>(users));

            mock.Setup(manager => manager.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                    users.FirstOrDefault(user => user.Id == id));

            mock.Setup(manager => manager.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) =>
                    users.FirstOrDefault(user =>
                        string.Equals(
                            user.Email,
                            email,
                            StringComparison.OrdinalIgnoreCase)));

            mock.Setup(manager => manager.GetRolesAsync(It.IsAny<IdentityUser>()))
                .Returns((IdentityUser user) =>
                {
                    if (rolesByUserId.TryGetValue(user.Id, out var roles))
                    {
                        return Task.FromResult(roles);
                    }

                    return Task.FromResult<IList<string>>(new List<string>());
                });

            mock.Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<IdentityUser, string>((user, _) =>
                {
                    if (string.IsNullOrWhiteSpace(user.Id))
                    {
                        user.Id = Guid.NewGuid().ToString();
                    }

                    users.Add(user);
                });

            mock.Setup(manager => manager.AddToRoleAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mock.Setup(manager => manager.AddClaimAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);

            mock.Setup(manager => manager.UpdateAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);

            mock.Setup(manager => manager.ChangePasswordAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mock.Setup(manager => manager.SetAuthenticationTokenAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            return mock;
        }

        private static Mock<RoleManager<IdentityRole>> MockRoleManager()
        {
            var roleStore = new Mock<IRoleStore<IdentityRole>>();

            var mock = new Mock<RoleManager<IdentityRole>>(
                roleStore.Object,
                null!,
                null!,
                null!,
                null!);

            mock.Setup(manager => manager.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            mock.Setup(manager => manager.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            return mock;
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();

            GC.SuppressFinalize(this);
        }
    }

    internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object? Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(
            Expression expression,
            CancellationToken cancellationToken = default)
        {
            var expectedResultType = typeof(TResult).GetGenericArguments()[0];

            var executionResult = typeof(IQueryProvider)
                .GetMethods()
                .First(method =>
                    method.Name == nameof(IQueryProvider.Execute) &&
                    method.IsGenericMethod &&
                    method.GetParameters().Length == 1)
                .MakeGenericMethod(expectedResultType)
                .Invoke(_inner, new object[] { expression });

            return (TResult)typeof(Task)
                .GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(expectedResultType)
                .Invoke(null, new[] { executionResult })!;
        }
    }

    internal class TestAsyncEnumerable<T> :
        EnumerableQuery<T>,
        IAsyncEnumerable<T>,
        IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        {
        }

        public TestAsyncEnumerable(Expression expression)
            : base(expression)
        {
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(
            CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider =>
            new TestAsyncQueryProvider<T>(this);
    }

    internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public T Current => _inner.Current;

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return ValueTask.CompletedTask;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_inner.MoveNext());
        }
    }
}