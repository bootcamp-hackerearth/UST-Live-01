using AutoMapper;
using FluentAssertions;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services;
using HealthCareApp.Services.Interface;
using HealthCareApp.Shared.Constants;
using HealthCareApp.Shared.Dtos.Doctors;
using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using HealthCareApp.Services;

namespace HealthCareApp.Testing.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> repositoryMock;
        private readonly Mock<IAppointmentRepository> appointmentRepositoryMock;
        private readonly Mock<ICacheService> cacheServiceMock;
        private readonly Mock<IDoctorLeaveService> doctorLeaveServiceMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<UserManager<IdentityUser>> userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> roleManagerMock;
        private readonly Mock<ILogger<DoctorService>> loggerMock;

        private readonly DoctorService doctorService;

        public DoctorServiceTests()
        {
            repositoryMock = new Mock<IDoctorRepository>();
            appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            cacheServiceMock = new Mock<ICacheService>();
            doctorLeaveServiceMock = new Mock<IDoctorLeaveService>();
            mapperMock = new Mock<IMapper>();
            userManagerMock = CreateUserManagerMock();
            roleManagerMock = CreateRoleManagerMock();
            loggerMock = new Mock<ILogger<DoctorService>>();

            SetupMapper();
            SetupDefaultAvailabilityDependencies();

            doctorService = new DoctorService(new DoctorServiceDependencies
            {
                Repository = repositoryMock.Object,
                AppointmentRepository = appointmentRepositoryMock.Object,
                Mapper = mapperMock.Object,
                UserManager = userManagerMock.Object,
                RoleManager = roleManagerMock.Object,
                CacheService = cacheServiceMock.Object,
                DoctorLeaveService = doctorLeaveServiceMock.Object,
                Logger = loggerMock.Object
            });
        }

        [Fact]
        public async Task GetAllDoctorsAsync_ShouldReturnMappedDoctors()
        {
            var doctors = GetDoctors();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var result = await doctorService.GetAllDoctorsAsync();

            result.Should().HaveCount(4);
            result[0].FullName.Should().Be("Rishi Doctor");

            repositoryMock.Verify(
                repository => repository.GetAllAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetAllDoctorsPagedAsync_WhenQueryIsNull_ShouldUseDefaultPagination()
        {
            var doctors = GetDoctors();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var result = await doctorService.GetAllDoctorsPagedAsync(null!);

            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalRecords.Should().Be(4);
            result.TotalPages.Should().Be(1);
            result.Items.Should().HaveCount(4);
        }

        [Fact]
        public async Task GetAllDoctorsPagedAsync_WhenInvalidPageValues_ShouldNormalizeValues()
        {
            var doctors = GetDoctors();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var query = new DoctorPaginationQueryDto
            {
                PageNumber = 0,
                PageSize = 0
            };

            var result = await doctorService.GetAllDoctorsPagedAsync(query);

            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task GetAllDoctorsPagedAsync_WhenPageSizeGreaterThan100_ShouldCapPageSize()
        {
            var doctors = GetDoctors();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var query = new DoctorPaginationQueryDto
            {
                PageNumber = 1,
                PageSize = 500
            };

            var result = await doctorService.GetAllDoctorsPagedAsync(query);

            result.PageSize.Should().Be(100);
        }

        [Fact]
        public async Task GetAllDoctorsPagedAsync_ShouldFilterBySearchTermUsingName()
        {
            var doctors = GetDoctors();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var query = new DoctorPaginationQueryDto
            {
                SearchTerm = "rishi"
            };

            var result = await doctorService.GetAllDoctorsPagedAsync(query);

            result.TotalRecords.Should().Be(1);
            result.Items.Single().FullName.Should().Be("Rishi Doctor");
        }

        [Fact]
        public async Task GetAllDoctorsPagedAsync_ShouldFilterBySearchTermUsingEmail()
        {
            var doctors = GetDoctors();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var query = new DoctorPaginationQueryDto
            {
                SearchTerm = "meera.doctor@example.com"
            };

            var result = await doctorService.GetAllDoctorsPagedAsync(query);

            result.TotalRecords.Should().Be(1);
            result.Items.Single().Email.Should().Be("meera.doctor@example.com");
        }

        [Fact]
        public async Task GetAllDoctorsPagedAsync_ShouldFilterBySpecialisation()
        {
            var doctors = GetDoctors();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var query = new DoctorPaginationQueryDto
            {
                Specialisation = SpecialisationType.Cardiologist
            };

            var result = await doctorService.GetAllDoctorsPagedAsync(query);

            result.TotalRecords.Should().Be(1);
            result.Items.Single().Specialisation.Should().Be(SpecialisationType.Cardiologist);
        }

        [Fact]
        public async Task GetAllDoctorsPagedAsync_ShouldFilterByIsActive()
        {
            var doctors = GetDoctors();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var query = new DoctorPaginationQueryDto
            {
                IsActive = false
            };

            var result = await doctorService.GetAllDoctorsPagedAsync(query);

            result.TotalRecords.Should().Be(1);
            result.Items.Should().OnlyContain(doctor => doctor.IsActive == false);
        }

        [Fact]
        public async Task GetAllDoctorsPagedAsync_ShouldApplyPagination()
        {
            var doctors = GetDoctors();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var query = new DoctorPaginationQueryDto
            {
                PageNumber = 2,
                PageSize = 2
            };

            var result = await doctorService.GetAllDoctorsPagedAsync(query);

            result.PageNumber.Should().Be(2);
            result.PageSize.Should().Be(2);
            result.TotalRecords.Should().Be(4);
            result.TotalPages.Should().Be(2);
            result.Items.Should().HaveCount(2);
            result.Items.First().DoctorId.Should().Be(3);
        }

        [Fact]
        public async Task GetAllActiveDoctorsAsync_ShouldReturnMappedActiveDoctors()
        {
            var activeDoctors = GetDoctors()
                .Where(doctor => doctor.IsActive)
                .ToList();

            repositoryMock
                .Setup(repository => repository.GetAllActiveAsync())
                .ReturnsAsync(activeDoctors);

            var result = await doctorService.GetAllActiveDoctorsAsync();

            result.Should().HaveCount(3);
            result.Should().OnlyContain(doctor => doctor.IsActive);
        }

        [Fact]
        public async Task GetDoctorByIdAsync_WhenDoctorIdIsInvalid_ShouldThrowBusinessRuleException()
        {
            Func<Task> action = async () =>
                await doctorService.GetDoctorByIdAsync(0);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Please provide a valid doctor reference.");
        }

        [Fact]
        public async Task GetDoctorByIdAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            repositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await doctorService.GetDoctorByIdAsync(99);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetDoctorByIdAsync_WhenDoctorExists_ShouldReturnMappedDoctor()
        {
            var doctor = GetDoctors().First();

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(doctor.DoctorId))
                .ReturnsAsync(doctor);

            var result = await doctorService.GetDoctorByIdAsync(doctor.DoctorId);

            result.DoctorId.Should().Be(doctor.DoctorId);
            result.FullName.Should().Be(doctor.DoctorName);
        }

        [Fact]
        public async Task GetDoctorsBySpecialisationAsync_ShouldReturnMappedDoctors()
        {
            var doctors = GetDoctors()
                .Where(doctor => doctor.Specialisation == SpecialisationType.Cardiologist)
                .ToList();

            repositoryMock
                .Setup(repository => repository.GetBySpecialisationAsync(SpecialisationType.Cardiologist))
                .ReturnsAsync(doctors);

            var result = await doctorService.GetDoctorsBySpecialisationAsync(
                SpecialisationType.Cardiologist);

            result.Should().HaveCount(1);
            result.Single().Specialisation.Should().Be(SpecialisationType.Cardiologist);
        }

        [Fact]
        public async Task GetActiveDoctorsBySpecialisationAsync_ShouldReturnMappedDoctors()
        {
            var doctors = GetDoctors()
                .Where(doctor =>
                    doctor.Specialisation == SpecialisationType.GeneralPractitioner &&
                    doctor.IsActive)
                .ToList();

            repositoryMock
                .Setup(repository =>
                    repository.GetActiveBySpecialisationAsync(SpecialisationType.GeneralPractitioner))
                .ReturnsAsync(doctors);

            var result = await doctorService.GetActiveDoctorsBySpecialisationAsync(
                SpecialisationType.GeneralPractitioner);

            result.Should().OnlyContain(doctor =>
                doctor.Specialisation == SpecialisationType.GeneralPractitioner &&
                doctor.IsActive);
        }

        [Fact]
        public async Task CreateDoctorByAdminAsync_WhenDtoIsNull_ShouldThrowBusinessRuleException()
        {
            Func<Task> action = async () =>
                await doctorService.CreateDoctorByAdminAsync(null!);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Doctor details are required.");
        }

        [Fact]
        public async Task CreateDoctorByAdminAsync_WhenFullNameIsEmpty_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidCreateDoctorDto();
            dto.FullName = "";

            Func<Task> action = async () =>
                await doctorService.CreateDoctorByAdminAsync(dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Doctor full name is required.");
        }

        [Fact]
        public async Task CreateDoctorByAdminAsync_WhenFullNameHasNumbers_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidCreateDoctorDto();
            dto.FullName = "Doctor123";

            Func<Task> action = async () =>
                await doctorService.CreateDoctorByAdminAsync(dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Doctor name can contain only letters and single spaces between words.");
        }

        [Fact]
        public async Task CreateDoctorByAdminAsync_WhenEmailIsEmpty_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidCreateDoctorDto();
            dto.Email = "";

            Func<Task> action = async () =>
                await doctorService.CreateDoctorByAdminAsync(dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Doctor email is required.");
        }

        [Fact]
        public async Task CreateDoctorByAdminAsync_WhenPracticeStartDateIsFuture_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidCreateDoctorDto();
            dto.PracticeStartDate = DateTime.Today.AddDays(1);

            Func<Task> action = async () =>
                await doctorService.CreateDoctorByAdminAsync(dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Practice start date cannot be in the future.");
        }

        [Fact]
        public async Task CreateDoctorByAdminAsync_WhenConsultationFeeIsInvalid_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidCreateDoctorDto();
            dto.ConsultationFee = 0;

            Func<Task> action = async () =>
                await doctorService.CreateDoctorByAdminAsync(dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Consultation fee must be between 1 and 100,000.");
        }

        [Fact]
        public async Task CreateDoctorByAdminAsync_WhenDoctorEmailAlreadyExists_ShouldThrowConflictException()
        {
            var dto = GetValidCreateDoctorDto();
            string normalizedEmail = dto.Email.Trim().ToLower();

            repositoryMock
                .Setup(repository => repository.ExistsByEmailAsync(normalizedEmail))
                .ReturnsAsync(true);

            Func<Task> action = async () =>
                await doctorService.CreateDoctorByAdminAsync(dto);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("A doctor with this email already exists.");
        }

        [Fact]
        public async Task CreateDoctorByAdminAsync_WhenLoginAccountAlreadyExists_ShouldThrowConflictException()
        {
            var dto = GetValidCreateDoctorDto();
            string normalizedEmail = dto.Email.Trim().ToLower();

            repositoryMock
                .Setup(repository => repository.ExistsByEmailAsync(normalizedEmail))
                .ReturnsAsync(false);

            userManagerMock
                .Setup(manager => manager.FindByEmailAsync(normalizedEmail))
                .ReturnsAsync(new IdentityUser
                {
                    Email = normalizedEmail,
                    UserName = normalizedEmail
                });

            Func<Task> action = async () =>
                await doctorService.CreateDoctorByAdminAsync(dto);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("A login account with this email already exists.");
        }

        [Fact]
        public async Task CreateDoctorByAdminAsync_WhenIdentityUserCreationFails_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidCreateDoctorDto();
            string normalizedEmail = dto.Email.Trim().ToLower();

            repositoryMock
                .Setup(repository => repository.ExistsByEmailAsync(normalizedEmail))
                .ReturnsAsync(false);

            userManagerMock
                .Setup(manager => manager.FindByEmailAsync(normalizedEmail))
                .ReturnsAsync((IdentityUser?)null);

            userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Identity creation failed."
                    }));

            Func<Task> action = async () =>
                await doctorService.CreateDoctorByAdminAsync(dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Identity creation failed.");
        }

        [Fact]
        public async Task CreateDoctorByAdminAsync_WhenAddToRoleFails_ShouldDeleteIdentityUserAndThrowBusinessRuleException()
        {
            var dto = GetValidCreateDoctorDto();
            string normalizedEmail = dto.Email.Trim().ToLower();

            repositoryMock
                .Setup(repository => repository.ExistsByEmailAsync(normalizedEmail))
                .ReturnsAsync(false);

            userManagerMock
                .Setup(manager => manager.FindByEmailAsync(normalizedEmail))
                .ReturnsAsync((IdentityUser?)null);

            userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            roleManagerMock
                .Setup(manager => manager.RoleExistsAsync("Doctor"))
                .ReturnsAsync(true);

            userManagerMock
                .Setup(manager => manager.AddToRoleAsync(
                    It.IsAny<IdentityUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Role assignment failed."
                    }));

            userManagerMock
                .Setup(manager => manager.DeleteAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);

            Func<Task> action = async () =>
                await doctorService.CreateDoctorByAdminAsync(dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Role assignment failed.");

            userManagerMock.Verify(
                manager => manager.DeleteAsync(It.IsAny<IdentityUser>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateDoctorByAdminAsync_WhenRoleDoesNotExist_ShouldCreateRoleAndCreateDoctor()
        {
            var dto = GetValidCreateDoctorDto();

            SetupSuccessfulDoctorCreation(dto);

            roleManagerMock
                .Setup(manager => manager.RoleExistsAsync("Doctor"))
                .ReturnsAsync(false);

            roleManagerMock
                .Setup(manager => manager.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await doctorService.CreateDoctorByAdminAsync(dto);

            result.DoctorId.Should().Be(10);

            roleManagerMock.Verify(
                manager => manager.CreateAsync(It.Is<IdentityRole>(role =>
                    role.Name == "Doctor")),
                Times.Once);
        }

        [Fact]
        public async Task CreateDoctorByAdminAsync_WhenValid_ShouldCreateDoctorAndReturnCreatedResponse()
        {
            var dto = GetValidCreateDoctorDto();

            SetupSuccessfulDoctorCreation(dto);

            var result = await doctorService.CreateDoctorByAdminAsync(dto);

            result.DoctorId.Should().Be(10);
            result.DoctorName.Should().Be(dto.FullName);
            result.Email.Should().Be(dto.Email.ToLower());
            result.TemporaryPassword.Should().Contain("@");
            result.Message.Should().Be("Doctor account created successfully.");

            repositoryMock.Verify(
                repository => repository.CreateAsync(
                    It.Is<Doctor>(doctor =>
                        doctor.DoctorName == dto.FullName.Trim() &&
                        doctor.Email == dto.Email.Trim().ToLower() &&
                        doctor.IsActive &&
                        doctor.CreatedDate != default),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenDoctorIdIsInvalid_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidUpdateDoctorDto();

            Func<Task> action = async () =>
                await doctorService.UpdateDoctorAsync(0, dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Please provide a valid doctor reference.");
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenDtoIsNull_ShouldThrowBusinessRuleException()
        {
            Func<Task> action = async () =>
                await doctorService.UpdateDoctorAsync(1, null!);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Doctor details are required.");
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            var dto = GetValidUpdateDoctorDto();

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await doctorService.UpdateDoctorAsync(99, dto);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenRepositoryReturnsNull_ShouldThrowEntityNotFoundException()
        {
            var dto = GetValidUpdateDoctorDto();
            var existingDoctor = GetDoctors().First();

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(existingDoctor.DoctorId))
                .ReturnsAsync(existingDoctor);

            repositoryMock
                .Setup(repository => repository.UpdateAsync(
                    existingDoctor.DoctorId,
                    It.IsAny<Doctor>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await doctorService.UpdateDoctorAsync(existingDoctor.DoctorId, dto);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenValid_ShouldUpdateDoctorAndReturnMappedDoctor()
        {
            var dto = GetValidUpdateDoctorDto();
            var existingDoctor = GetDoctors().First();

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(existingDoctor.DoctorId))
                .ReturnsAsync(existingDoctor);

            repositoryMock
                .Setup(repository => repository.UpdateAsync(
                    existingDoctor.DoctorId,
                    It.IsAny<Doctor>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int doctorId, Doctor doctor, CancellationToken cancellationToken) =>
                {
                    doctor.DoctorId = doctorId;
                    return doctor;
                });

            var result = await doctorService.UpdateDoctorAsync(existingDoctor.DoctorId, dto);

            result.DoctorId.Should().Be(existingDoctor.DoctorId);
            result.FullName.Should().Be(dto.FullName);

            repositoryMock.Verify(
                repository => repository.UpdateAsync(
                    existingDoctor.DoctorId,
                    It.Is<Doctor>(doctor =>
                        doctor.DoctorId == existingDoctor.DoctorId &&
                        doctor.Email == existingDoctor.Email &&
                        doctor.IdentityUserId == existingDoctor.IdentityUserId &&
                        doctor.CreatedDate == existingDoctor.CreatedDate),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteDoctorAsync_WhenDoctorIdIsInvalid_ShouldThrowBusinessRuleException()
        {
            Func<Task> action = async () =>
                await doctorService.DeleteDoctorAsync(0);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Please provide a valid doctor reference.");
        }

        [Fact]
        public async Task DeleteDoctorAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            repositoryMock
                .Setup(repository => repository.DeleteAsync(99))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await doctorService.DeleteDoctorAsync(99);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task DeleteDoctorAsync_WhenDoctorExists_ShouldReturnMappedDoctor()
        {
            var doctor = GetDoctors().First();

            repositoryMock
                .Setup(repository => repository.DeleteAsync(doctor.DoctorId))
                .ReturnsAsync(doctor);

            var result = await doctorService.DeleteDoctorAsync(doctor.DoctorId);

            result.DoctorId.Should().Be(doctor.DoctorId);
        }

        [Fact]
        public async Task GetMyProfileAsync_WhenIdentityUserIdIsEmpty_ShouldThrowBusinessRuleException()
        {
            Func<Task> action = async () =>
                await doctorService.GetMyProfileAsync("");

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Invalid logged-in user.");
        }

        [Fact]
        public async Task GetMyProfileAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            repositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync("identity-1"))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await doctorService.GetMyProfileAsync("identity-1");

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetMyProfileAsync_WhenDoctorExists_ShouldReturnMappedDoctor()
        {
            var doctor = GetDoctors().First();
            doctor.IdentityUserId = "identity-1";

            repositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync("identity-1"))
                .ReturnsAsync(doctor);

            var result = await doctorService.GetMyProfileAsync("identity-1");

            result.DoctorId.Should().Be(doctor.DoctorId);
        }

        [Fact]
        public async Task GetDoctorAvailabilityAsync_WhenDoctorIdIsInvalid_ShouldThrowBusinessRuleException()
        {
            Func<Task> action = async () =>
                await doctorService.GetDoctorAvailabilityAsync(0, DateTime.Today);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Please provide a valid doctor reference.");
        }

        [Fact]
        public async Task GetDoctorAvailabilityAsync_WhenDoctorDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            repositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            Func<Task> action = async () =>
                await doctorService.GetDoctorAvailabilityAsync(99, DateTime.Today);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetDoctorAvailabilityAsync_WhenDoctorIsInactive_ShouldThrowBusinessRuleException()
        {
            var doctor = GetDoctors()
                .First(existingDoctor => !existingDoctor.IsActive);

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(doctor.DoctorId))
                .ReturnsAsync(doctor);

            Func<Task> action = async () =>
                await doctorService.GetDoctorAvailabilityAsync(doctor.DoctorId, DateTime.Today);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Doctor is inactive and not available for appointments.");
        }

        [Fact]
        public async Task GetDoctorAvailabilityAsync_WhenDoctorIsActiveAndDateIsNull_ShouldReturnAllSlotsAsNotBooked()
        {
            var doctor = GetDoctors()
                .First(existingDoctor => existingDoctor.IsActive);

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(doctor.DoctorId))
                .ReturnsAsync(doctor);

            var result = await doctorService.GetDoctorAvailabilityAsync(doctor.DoctorId, null);

            result.DoctorId.Should().Be(doctor.DoctorId);
            result.IsDoctorOnLeave.Should().BeFalse();
            result.Date.Should().BeEmpty();
            result.Slots.Should().HaveCount(TimeSlots.Slots.Count);

            result.Slots.Select(slot => slot.TimeSlot)
                .Should()
                .BeEquivalentTo(TimeSlots.Slots);

            result.Slots.Should().OnlyContain(slot => slot.IsBooked == false);

            appointmentRepositoryMock.Verify(
                repository => repository.GetBookedTimeSlotsByDoctorAndDateAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetDoctorAvailabilityAsync_WhenDoctorIsActiveAndDateProvided_ShouldReturnBookedSlotStatus()
        {
            var doctor = GetDoctors()
                .First(existingDoctor => existingDoctor.IsActive);

            var selectedDate = DateTime.Today.AddDays(1);

            var bookedSlots = new List<string>
            {
                "09:00 AM - 09:30 AM",
                "10:00 AM - 10:30 AM"
            };

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(doctor.DoctorId))
                .ReturnsAsync(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetBookedTimeSlotsByDoctorAndDateAsync(
                    doctor.DoctorId,
                    selectedDate.Date,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookedSlots);

            var result = await doctorService.GetDoctorAvailabilityAsync(
                doctor.DoctorId,
                selectedDate);

            result.DoctorId.Should().Be(doctor.DoctorId);
            result.Date.Should().Be(selectedDate.ToString("yyyy-MM-dd"));
            result.IsDoctorOnLeave.Should().BeFalse();
            result.Slots.Should().HaveCount(TimeSlots.Slots.Count);

            result.Slots
                .Where(slot => bookedSlots.Contains(slot.TimeSlot))
                .Should()
                .OnlyContain(slot => slot.IsBooked);

            result.Slots
                .Where(slot => !bookedSlots.Contains(slot.TimeSlot))
                .Should()
                .OnlyContain(slot => !slot.IsBooked);

            appointmentRepositoryMock.Verify(
                repository => repository.GetBookedTimeSlotsByDoctorAndDateAsync(
                    doctor.DoctorId,
                    selectedDate.Date,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetDoctorAvailabilityAsync_WhenDoctorIsOnLeave_ShouldReturnAllSlotsAsBooked()
        {
            var doctor = GetDoctors()
                .First(existingDoctor => existingDoctor.IsActive);

            var selectedDate = DateTime.Today.AddDays(1);

            doctorLeaveServiceMock
                .Setup(service => service.IsDoctorOnLeaveAsync(
                    doctor.DoctorId,
                    selectedDate.Date))
                .ReturnsAsync(true);

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(doctor.DoctorId))
                .ReturnsAsync(doctor);

            var result = await doctorService.GetDoctorAvailabilityAsync(
                doctor.DoctorId,
                selectedDate);

            result.DoctorId.Should().Be(doctor.DoctorId);
            result.Date.Should().Be(selectedDate.ToString("yyyy-MM-dd"));
            result.IsDoctorOnLeave.Should().BeTrue();
            result.Slots.Should().HaveCount(TimeSlots.Slots.Count);
            result.Slots.Should().OnlyContain(slot => slot.IsBooked);

            appointmentRepositoryMock.Verify(
                repository => repository.GetBookedTimeSlotsByDoctorAndDateAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        private void SetupDefaultAvailabilityDependencies()
        {
            doctorLeaveServiceMock
                .Setup(service => service.IsDoctorOnLeaveAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>()))
                .ReturnsAsync(false);

            cacheServiceMock
                .Setup(cache => cache.GetAsync<DoctorAvailabilityResponseDto>(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((DoctorAvailabilityResponseDto?)null);

            cacheServiceMock
                .Setup(cache => cache.SetAsync(
                    It.IsAny<string>(),
                    It.IsAny<DoctorAvailabilityResponseDto>(),
                    It.IsAny<TimeSpan>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            cacheServiceMock
                .Setup(cache => cache.RemoveAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        private void SetupSuccessfulDoctorCreation(CreateDoctorDto dto)
        {
            string normalizedEmail = dto.Email.Trim().ToLower();

            repositoryMock
                .Setup(repository => repository.ExistsByEmailAsync(normalizedEmail))
                .ReturnsAsync(false);

            userManagerMock
                .Setup(manager => manager.FindByEmailAsync(normalizedEmail))
                .ReturnsAsync((IdentityUser?)null);

            userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            roleManagerMock
                .Setup(manager => manager.RoleExistsAsync("Doctor"))
                .ReturnsAsync(true);

            userManagerMock
                .Setup(manager => manager.AddToRoleAsync(
                    It.IsAny<IdentityUser>(),
                    "Doctor"))
                .ReturnsAsync(IdentityResult.Success);

            repositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<Doctor>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor doctor, CancellationToken cancellationToken) =>
                {
                    doctor.DoctorId = 10;
                    return doctor;
                });
        }

        private void SetupMapper()
        {
            mapperMock
                .Setup(mapper => mapper.Map<List<DoctorDto>>(It.IsAny<object>()))
                .Returns((object source) =>
                {
                    var doctors = ((IEnumerable<Doctor>)source).ToList();
                    return doctors.Select(MapToDoctorDto).ToList();
                });

            mapperMock
                .Setup(mapper => mapper.Map<DoctorDto>(It.IsAny<object>()))
                .Returns((object source) =>
                    MapToDoctorDto((Doctor)source));

            mapperMock
                .Setup(mapper => mapper.Map<Doctor>(It.IsAny<object>()))
                .Returns((object source) =>
                {
                    return source switch
                    {
                        CreateDoctorDto createDto => new Doctor
                        {
                            DoctorName = createDto.FullName,
                            Email = createDto.Email,
                            Specialisation = createDto.Specialisation,
                            ConsultationFee = (int)createDto.ConsultationFee,
                            IsActive = true
                        },

                        UpdateDoctorDto updateDto => new Doctor
                        {
                            DoctorName = updateDto.FullName,
                            Email = "updated.doctor@example.com",
                            Specialisation = updateDto.Specialisation,
                            ConsultationFee = (int)updateDto.ConsultationFee,
                            IsActive = updateDto.IsActive
                        },

                        _ => new Doctor
                        {
                            DoctorName = "Default Doctor",
                            Email = "default.doctor@example.com",
                            Specialisation = SpecialisationType.GeneralPractitioner,
                            ConsultationFee = 500,
                            IsActive = true
                        }
                    };
                });
        }

        private static DoctorDto MapToDoctorDto(Doctor doctor)
        {
            return new DoctorDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.DoctorName,
                Email = doctor.Email,
                Specialisation = doctor.Specialisation,
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = doctor.IsActive
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
                    IdentityUserId = "identity-1",
                    CreatedDate = new DateTime(2026, 1, 1)
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
                    IdentityUserId = "identity-2",
                    CreatedDate = new DateTime(2026, 1, 2)
                },

                new Doctor
                {
                    DoctorId = 3,
                    DoctorName = "Arun Doctor",
                    Email = "arun.doctor@example.com",
                    Specialisation = SpecialisationType.Dermatologist,
                    YearsOfExperience = 3,
                    ConsultationFee = 700,
                    IsActive = false,
                    IdentityUserId = "identity-3",
                    CreatedDate = new DateTime(2026, 1, 3)
                },

                new Doctor
                {
                    DoctorId = 4,
                    DoctorName = "Sara Doctor",
                    Email = "sara.doctor@example.com",
                    Specialisation = SpecialisationType.GeneralPractitioner,
                    YearsOfExperience = 6,
                    ConsultationFee = 650,
                    IsActive = true,
                    IdentityUserId = "identity-4",
                    CreatedDate = new DateTime(2026, 1, 4)
                }
            };
        }

        private static CreateDoctorDto GetValidCreateDoctorDto()
        {
            return new CreateDoctorDto
            {
                FullName = "New Doctor",
                Email = "new.doctor@example.com",
                Specialisation = SpecialisationType.GeneralPractitioner,
                PracticeStartDate = DateTime.Today.AddYears(-5),
                ConsultationFee = 600
            };
        }

        private static UpdateDoctorDto GetValidUpdateDoctorDto()
        {
            return new UpdateDoctorDto
            {
                FullName = "Updated Doctor",
                Specialisation = SpecialisationType.Dermatologist,
                PracticeStartDate = DateTime.Today.AddYears(-4),
                ConsultationFee = 800,
                IsActive = true
            };
        }

        private static Mock<UserManager<IdentityUser>> CreateUserManagerMock()
        {
            var store = new Mock<IUserStore<IdentityUser>>();

            return new Mock<UserManager<IdentityUser>>(
                store.Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!);
        }

        private static Mock<RoleManager<IdentityRole>> CreateRoleManagerMock()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();

            return new Mock<RoleManager<IdentityRole>>(
                store.Object,
                null!,
                null!,
                null!,
                null!);
        }
    }
}
