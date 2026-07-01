using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace HealthAxisCore_Test.Services
{
    public class AdminServiceTests
    {
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AdminService _service;

        public AdminServiceTests()
        {
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _mapperMock = new Mock<IMapper>();

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!);

            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();

            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                roleStoreMock.Object,
                null!,
                null!,
                null!,
                null!);

            _service = new AdminService(
                _doctorRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _appointmentRepositoryMock.Object,
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetDoctorsAsync_ReturnsPagedDoctors()
        {
            var doctors = new List<Doctor>
            {
                CreateDoctor(2, "Second Doctor"),
                CreateDoctor(1, "First Doctor"),
                CreateDoctor(3, "Third Doctor")
            };

            var expectedDtos = new List<DoctorDto>
            {
                new()
                {
                    DoctorId = 1,
                    DoctorName = "First Doctor",
                    Specialisation = "Cardiologist",
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true
                },
                new()
                {
                    DoctorId = 2,
                    DoctorName = "Second Doctor",
                    Specialisation = "Cardiologist",
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true
                }
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(mapper => mapper.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
                .Returns(expectedDtos);

            var result = await _service.GetDoctorsAsync(
                new PaginationQueryDto
                {
                    PageNumber = 1,
                    PageSize = 2
                });

            Assert.Equal(2, result.Items.Count);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(2, result.PageSize);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(2, result.TotalPages);
            Assert.False(result.HasPreviousPage);
            Assert.True(result.HasNextPage);
        }

        [Fact]
        public async Task CreateDoctorAsync_WhenEmailAlreadyExists_ThrowsInvalidException()
        {
            var request = CreateDoctorRequest();

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(new ApplicationUser());

            await Assert.ThrowsAsync<InvalidException>(
                () => _service.CreateDoctorAsync(request));
        }

        [Fact]
        public async Task CreateDoctorAsync_WhenCreateUserFails_ThrowsInvalidException()
        {
            var request = CreateDoctorRequest();
            var doctor = CreateDoctor();

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _mapperMock
                .Setup(mapper => mapper.Map<Doctor>(request))
                .Returns(doctor);

            _doctorRepositoryMock
                .Setup(repository => repository.CreateAsync(doctor, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _userManagerMock
                .Setup(manager => manager.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Create failed"
                    }));

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => _service.CreateDoctorAsync(request));

            Assert.Contains("Create failed", exception.Message);
        }

        [Fact]
        public async Task CreateDoctorAsync_WhenDoctorRoleDoesNotExist_ThrowsInvalidException()
        {
            var request = CreateDoctorRequest();
            var doctor = CreateDoctor();

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _mapperMock
                .Setup(mapper => mapper.Map<Doctor>(request))
                .Returns(doctor);

            _doctorRepositoryMock
                .Setup(repository => repository.CreateAsync(doctor, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _userManagerMock
                .Setup(manager => manager.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Success);

            _roleManagerMock
                .Setup(manager => manager.RoleExistsAsync("Doctor"))
                .ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => _service.CreateDoctorAsync(request));

            Assert.Equal("Doctor role does not exist", exception.Message);
        }

        [Fact]
        public async Task CreateDoctorAsync_WhenAddToRoleFails_ThrowsInvalidException()
        {
            var request = CreateDoctorRequest();
            var doctor = CreateDoctor();

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _mapperMock
                .Setup(mapper => mapper.Map<Doctor>(request))
                .Returns(doctor);

            _doctorRepositoryMock
                .Setup(repository => repository.CreateAsync(doctor, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _userManagerMock
                .Setup(manager => manager.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Success);

            _roleManagerMock
                .Setup(manager => manager.RoleExistsAsync("Doctor"))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(manager => manager.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Doctor"))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Role add failed"
                    }));

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => _service.CreateDoctorAsync(request));

            Assert.Contains("Role add failed", exception.Message);
        }

        [Fact]
        public async Task CreateDoctorAsync_WhenValid_ReturnsDoctorDto()
        {
            var request = CreateDoctorRequest();
            var doctor = CreateDoctor();

            var expectedDto = new DoctorDto
            {
                DoctorId = doctor.DoctorId,
                DoctorName = doctor.DoctorName,
                Specialisation = doctor.Specialisation,
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = doctor.IsActive
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            _mapperMock
                .Setup(mapper => mapper.Map<Doctor>(request))
                .Returns(doctor);

            _doctorRepositoryMock
                .Setup(repository => repository.CreateAsync(doctor, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _userManagerMock
                .Setup(manager => manager.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Success);

            _roleManagerMock
                .Setup(manager => manager.RoleExistsAsync("Doctor"))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(manager => manager.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            _mapperMock
                .Setup(mapper => mapper.Map<DoctorDto>(doctor))
                .Returns(expectedDto);

            var result = await _service.CreateDoctorAsync(request);

            Assert.Equal(expectedDto.DoctorId, result.DoctorId);
            Assert.Equal(expectedDto.DoctorName, result.DoctorName);
            Assert.True(result.IsActive);
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenDoctorNotFound_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateDoctorAsync(1, CreateUpdateDoctorRequest()));
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenUpdateReturnsNull_ThrowsNotFoundException()
        {
            var doctor = CreateDoctor();
            var request = CreateUpdateDoctorRequest();

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(repository => repository.UpdateAsync(1, doctor, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateDoctorAsync(1, request));
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenValid_ReturnsUpdatedDoctorDto()
        {
            var doctor = CreateDoctor();
            var request = CreateUpdateDoctorRequest();

            var expectedDto = new DoctorDto
            {
                DoctorId = 1,
                DoctorName = "Updated Doctor",
                Specialisation = "Neurologist",
                YearsOfExperience = 12,
                ConsultationFee = 1000,
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(repository => repository.UpdateAsync(1, doctor, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(mapper => mapper.Map<DoctorDto>(doctor))
                .Returns(expectedDto);

            var result = await _service.UpdateDoctorAsync(1, request);

            Assert.Equal(expectedDto.DoctorId, result.DoctorId);
            Assert.Equal(expectedDto.DoctorName, result.DoctorName);
        }

        [Fact]
        public async Task GetUsersAsync_WhenRoleIsNull_ReturnsAllUsersWithAllNameBranches()
        {
            var users = new List<ApplicationUser>
            {
                new()
                {
                    Id = "1",
                    Email = "patient@test.com",
                    UserName = "patient@test.com",
                    PatientId = 10,
                    IsActive = true
                },
                new()
                {
                    Id = "2",
                    Email = "doctor@test.com",
                    UserName = "doctor@test.com",
                    DoctorId = 20,
                    IsActive = true
                },
                new()
                {
                    Id = "3",
                    Email = "username@test.com",
                    UserName = "Fallback User",
                    IsActive = true
                },
                new()
                {
                    Id = "4",
                    Email = "emailfallback@test.com",
                    UserName = null,
                    IsActive = false
                }
            };

            SetupUsersQueryable(users);

            _userManagerMock
                .Setup(manager => manager.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync((ApplicationUser user) =>
                {
                    if (user.Id == "1")
                    {
                        return new List<string> { "Patient" };
                    }

                    if (user.Id == "2")
                    {
                        return new List<string> { "Doctor" };
                    }

                    return new List<string>();
                });

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreatePatient(10, "Patient One"));

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(20, It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateDoctor(20, "Doctor One"));

            var result = await _service.GetUsersAsync(
                null,
                new PaginationQueryDto
                {
                    PageNumber = 1,
                    PageSize = 10
                });

            Assert.Equal(4, result.Items.Count);
            Assert.Contains(result.Items, user => user.FullName == "Patient One");
            Assert.Contains(result.Items, user => user.FullName == "Doctor One");
            Assert.Contains(result.Items, user => user.FullName == "Fallback User");
            Assert.Contains(result.Items, user => user.FullName == "emailfallback@test.com");
            Assert.Equal(4, result.TotalCount);
            Assert.Equal(1, result.TotalPages);
        }

        [Fact]
        public async Task GetUsersAsync_WhenRoleIsGiven_ReturnsUsersInRole()
        {
            var usersInRole = new List<ApplicationUser>
            {
                new()
                {
                    Id = "1",
                    Email = "doctor1@test.com",
                    UserName = "doctor1@test.com",
                    DoctorId = 1,
                    IsActive = true
                },
                new()
                {
                    Id = "2",
                    Email = "doctor2@test.com",
                    UserName = "doctor2@test.com",
                    DoctorId = 2,
                    IsActive = false
                }
            };

            _userManagerMock
                .Setup(manager => manager.GetUsersInRoleAsync("Doctor"))
                .ReturnsAsync(usersInRole);

            _userManagerMock
                .Setup(manager => manager.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { "Doctor" });

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateDoctor(1, "Doctor One"));

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateDoctor(2, "Doctor Two"));

            var result = await _service.GetUsersAsync(
                " Doctor ",
                new PaginationQueryDto
                {
                    PageNumber = 1,
                    PageSize = 1
                });

            Assert.Single(result.Items);
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.TotalPages);
            Assert.Equal("Doctor", result.Items[0].Role);
            Assert.Equal("Doctor One", result.Items[0].FullName);
        }

        [Fact]
        public async Task GetAppointmentReportAsync_ReturnsAppointmentReport()
        {
            var report = new List<AppointmentReportDto>
            {
                new()
                {
                    Date = new DateTime(2026, 1, 1),
                    Confirmed = 3,
                    Cancelled = 1,
                    Completed = 5
                }
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAppointmentReportAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(report);

            var result = await _service.GetAppointmentReportAsync();

            Assert.Single(result);
            Assert.Equal(new DateTime(2026, 1, 1), result[0].Date);
            Assert.Equal(3, result[0].Confirmed);
            Assert.Equal(1, result[0].Cancelled);
            Assert.Equal(5, result[0].Completed);
        }

        [Fact]
        public async Task UpdatePatientStatusAsync_WhenPatientNotFound_ThrowsNotFoundException()
        {
            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdatePatientStatusAsync(1, true));
        }

        [Fact]
        public async Task UpdatePatientStatusAsync_WhenUserNotFound_UpdatesPatientAndSaves()
        {
            var patient = CreatePatient(1, "Patient One");

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            SetupUsersQueryable(new List<ApplicationUser>());

            await _service.UpdatePatientStatusAsync(1, false);

            Assert.False(patient.IsActive);

            _patientRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdatePatientStatusAsync_WhenUserUpdateFails_ThrowsInvalidException()
        {
            var patient = CreatePatient(1, "Patient One");

            var user = new ApplicationUser
            {
                Id = "1",
                PatientId = 1,
                Email = "patient@test.com",
                UserName = "patient@test.com",
                IsActive = true
            };

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            SetupUsersQueryable(new List<ApplicationUser> { user });

            _userManagerMock
                .Setup(manager => manager.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Update failed"
                    }));

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => _service.UpdatePatientStatusAsync(1, false));

            Assert.Contains("Update failed", exception.Message);
        }

        [Fact]
        public async Task UpdatePatientStatusAsync_WhenUserExists_UpdatesUserAndPatientAndSaves()
        {
            var patient = CreatePatient(1, "Patient One");

            var user = new ApplicationUser
            {
                Id = "1",
                PatientId = 1,
                Email = "patient@test.com",
                UserName = "patient@test.com",
                IsActive = true
            };

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            SetupUsersQueryable(new List<ApplicationUser> { user });

            _userManagerMock
                .Setup(manager => manager.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            await _service.UpdatePatientStatusAsync(1, false);

            Assert.False(patient.IsActive);
            Assert.False(user.IsActive);

            _userManagerMock.Verify(
                manager => manager.UpdateAsync(user),
                Times.Once);

            _patientRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorStatusAsync_WhenDoctorNotFound_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateDoctorStatusAsync(1, true));
        }

        [Fact]
        public async Task UpdateDoctorStatusAsync_WhenUserNotFound_UpdatesDoctorAndSaves()
        {
            var doctor = CreateDoctor(1, "Doctor One");

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            SetupUsersQueryable(new List<ApplicationUser>());

            await _service.UpdateDoctorStatusAsync(1, false);

            Assert.False(doctor.IsActive);

            _doctorRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorStatusAsync_WhenUserUpdateFails_ThrowsInvalidException()
        {
            var doctor = CreateDoctor(1, "Doctor One");

            var user = new ApplicationUser
            {
                Id = "1",
                DoctorId = 1,
                Email = "doctor@test.com",
                UserName = "doctor@test.com",
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            SetupUsersQueryable(new List<ApplicationUser> { user });

            _userManagerMock
                .Setup(manager => manager.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Doctor user update failed"
                    }));

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => _service.UpdateDoctorStatusAsync(1, false));

            Assert.Contains("Doctor user update failed", exception.Message);
        }

        [Fact]
        public async Task UpdateDoctorStatusAsync_WhenUserExists_UpdatesUserAndDoctorAndSaves()
        {
            var doctor = CreateDoctor(1, "Doctor One");

            var user = new ApplicationUser
            {
                Id = "1",
                DoctorId = 1,
                Email = "doctor@test.com",
                UserName = "doctor@test.com",
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            SetupUsersQueryable(new List<ApplicationUser> { user });

            _userManagerMock
                .Setup(manager => manager.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            await _service.UpdateDoctorStatusAsync(1, false);

            Assert.False(doctor.IsActive);
            Assert.False(user.IsActive);

            _userManagerMock.Verify(
                manager => manager.UpdateAsync(user),
                Times.Once);

            _doctorRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private void SetupUsersQueryable(List<ApplicationUser> users)
        {
            var asyncUsers = new TestAsyncEnumerable<ApplicationUser>(users);

            _userManagerMock
                .Setup(manager => manager.Users)
                .Returns(asyncUsers);
        }

        private static CreateDoctorDto CreateDoctorRequest()
        {
            return new CreateDoctorDto
            {
                DoctorName = "John Doe",
                Specialisation = "Cardiologist",
                YearsOfExperience = 10,
                ConsultationFee = 500,
                Email = "doctor@test.com",
                PhoneNumber = "9876543210",
                Password = "Password123!"
            };
        }

        private static UpdateDoctorDto CreateUpdateDoctorRequest()
        {
            return new UpdateDoctorDto
            {
                DoctorName = "Updated Doctor",
                Specialisation = "Neurologist",
                YearsOfExperience = 12,
                ConsultationFee = 1000,
                IsActive = true
            };
        }

        private static Doctor CreateDoctor(
            int doctorId = 1,
            string doctorName = "John Doe")
        {
            return new Doctor
            {
                DoctorId = doctorId,
                DoctorName = doctorName,
                Specialisation = "Cardiologist",
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };
        }

        private static Patient CreatePatient(
            int patientId = 1,
            string patientName = "John Patient")
        {
            return new Patient
            {
                PatientId = patientId,
                PatientName = patientName,
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = "Male",
                Email = "patient@test.com",
                PhoneNumber = "9999999999",
                InsuranceID = "INS123",
                IsActive = true
            };
        }
    }

    internal sealed class TestAsyncEnumerable<T> :
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

    internal sealed class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
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

    internal sealed class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        public TestAsyncQueryProvider(IQueryProvider inner)
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
                .GetMethod(
                    nameof(IQueryProvider.Execute),
                    genericParameterCount: 1,
                    types: new[] { typeof(Expression) })!
                .MakeGenericMethod(expectedResultType)
                .Invoke(_inner, new object[] { expression });

            return (TResult)typeof(Task)
                .GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(expectedResultType)
                .Invoke(null, new[] { executionResult })!;
        }
    }
}