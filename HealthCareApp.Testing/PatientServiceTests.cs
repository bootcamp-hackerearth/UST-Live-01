using AutoMapper;
using FluentAssertions;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services;
using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Dtos.Patients;
using HealthCareApp.Shared.Enums;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace HealthCareApp.Testing.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> repositoryMock;

        private readonly Mock<IMapper> mapperMock;

        private readonly PatientService patientService;
        private readonly Mock<IDistributedCache> cacheMock;

        public PatientServiceTests()
        {
            repositoryMock = new Mock<IPatientRepository>();

            mapperMock = new Mock<IMapper>();
            cacheMock = new Mock<IDistributedCache>();

            SetupMapper();

            patientService = new PatientService(
                repositoryMock.Object,
                mapperMock.Object,
                cacheMock.Object);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ShouldReturnMappedPatients()
        {
            var patients = GetPatients();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var result = await patientService.GetAllPatientsAsync();

            result.Should().HaveCount(4);

            result[0].FullName.Should().Be("Rishi Patient");

            repositoryMock.Verify(
                repository => repository.GetAllAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetAllPatientsPagedAsync_WhenQueryIsNull_ShouldUseDefaultPagination()
        {
            var patients = GetPatients();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var result = await patientService.GetAllPatientsPagedAsync(null!);

            result.PageNumber.Should().Be(1);

            result.PageSize.Should().Be(10);

            result.TotalRecords.Should().Be(4);

            result.TotalPages.Should().Be(1);

            result.Items.Should().HaveCount(4);
        }

        [Fact]
        public async Task GetAllPatientsPagedAsync_WhenPageNumberAndPageSizeAreInvalid_ShouldNormalizeValues()
        {
            var patients = GetPatients();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var query = new PatientPaginationQueryDto
            {
                PageNumber = 0,
                PageSize = 0
            };

            var result = await patientService.GetAllPatientsPagedAsync(query);

            result.PageNumber.Should().Be(1);

            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task GetAllPatientsPagedAsync_WhenPageSizeIsGreaterThan100_ShouldCapPageSizeTo100()
        {
            var patients = GetPatients();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var query = new PatientPaginationQueryDto
            {
                PageNumber = 1,
                PageSize = 500
            };

            var result = await patientService.GetAllPatientsPagedAsync(query);

            result.PageSize.Should().Be(100);
        }

        [Fact]
        public async Task GetAllPatientsPagedAsync_ShouldFilterBySearchTermUsingName()
        {
            var patients = GetPatients();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var query = new PatientPaginationQueryDto
            {
                SearchTerm = "rishi"
            };

            var result = await patientService.GetAllPatientsPagedAsync(query);

            result.TotalRecords.Should().Be(1);

            result.Items.Single().FullName.Should().Be("Rishi Patient");
        }

        [Fact]
        public async Task GetAllPatientsPagedAsync_ShouldFilterBySearchTermUsingEmail()
        {
            var patients = GetPatients();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var query = new PatientPaginationQueryDto
            {
                SearchTerm = "meera@example.com"
            };

            var result = await patientService.GetAllPatientsPagedAsync(query);

            result.TotalRecords.Should().Be(1);

            result.Items.Single().Email.Should().Be("meera@example.com");
        }

        [Fact]
        public async Task GetAllPatientsPagedAsync_ShouldFilterBySearchTermUsingPhoneNumber()
        {
            var patients = GetPatients();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var query = new PatientPaginationQueryDto
            {
                SearchTerm = "9876543210"
            };

            var result = await patientService.GetAllPatientsPagedAsync(query);

            result.TotalRecords.Should().Be(1);

            result.Items.Single().PhoneNumber.Should().Be("9876543210");
        }

        [Fact]
        public async Task GetAllPatientsPagedAsync_ShouldFilterBySearchTermUsingInsuranceId()
        {
            var patients = GetPatients();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var query = new PatientPaginationQueryDto
            {
                SearchTerm = "INS200"
            };

            var result = await patientService.GetAllPatientsPagedAsync(query);

            result.TotalRecords.Should().Be(1);

            result.Items.Single().FullName.Should().Be("Meera Patient");
        }

        [Fact]
        public async Task GetAllPatientsPagedAsync_ShouldFilterByGender()
        {
            var patients = GetPatients();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var query = new PatientPaginationQueryDto
            {
                Gender = GenderType.Female
            };

            var result = await patientService.GetAllPatientsPagedAsync(query);

            result.TotalRecords.Should().Be(2);

            result.Items.Should().OnlyContain(patient =>
                patient.Gender == GenderType.Female);
        }

        [Fact]
        public async Task GetAllPatientsPagedAsync_WhenHasInsuranceIsTrue_ShouldReturnOnlyInsuredPatients()
        {
            var patients = GetPatients();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var query = new PatientPaginationQueryDto
            {
                HasInsurance = true
            };

            var result = await patientService.GetAllPatientsPagedAsync(query);

            result.TotalRecords.Should().Be(2);

            result.Items.Should().OnlyContain(patient =>
                !string.IsNullOrWhiteSpace(patient.InsuranceId));
        }

        [Fact]
        public async Task GetAllPatientsPagedAsync_WhenHasInsuranceIsFalse_ShouldReturnOnlyUninsuredPatients()
        {
            var patients = GetPatients();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var query = new PatientPaginationQueryDto
            {
                HasInsurance = false
            };

            var result = await patientService.GetAllPatientsPagedAsync(query);

            result.TotalRecords.Should().Be(2);

            result.Items.Should().OnlyContain(patient =>
                string.IsNullOrWhiteSpace(patient.InsuranceId));
        }

        [Fact]
        public async Task GetAllPatientsPagedAsync_ShouldApplyPagination()
        {
            var patients = GetPatients();

            repositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var query = new PatientPaginationQueryDto
            {
                PageNumber = 2,
                PageSize = 2
            };

            var result = await patientService.GetAllPatientsPagedAsync(query);

            result.PageNumber.Should().Be(2);

            result.PageSize.Should().Be(2);

            result.TotalRecords.Should().Be(4);

            result.TotalPages.Should().Be(2);

            result.Items.Should().HaveCount(2);

            result.Items.First().PatientId.Should().Be(3);
        }

        [Fact]
        public async Task GetPatientByIdAsync_WhenPatientIdIsInvalid_ShouldThrowBusinessRuleException()
        {
            Func<Task> action = async () =>
                await patientService.GetPatientByIdAsync(0);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Please provide a valid patient reference.");
        }

        [Fact]
        public async Task GetPatientByIdAsync_WhenPatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            repositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () =>
                await patientService.GetPatientByIdAsync(99);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetPatientByIdAsync_WhenPatientExists_ShouldReturnMappedPatient()
        {
            var patient = GetPatients().First();

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(patient.PatientId))
                .ReturnsAsync(patient);

            var result = await patientService.GetPatientByIdAsync(patient.PatientId);

            result.PatientId.Should().Be(patient.PatientId);

            result.FullName.Should().Be(patient.PatientName);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenDtoIsNull_ShouldThrowBusinessRuleException()
        {
            Func<Task> action = async () =>
                await patientService.RegisterPatientAsync(null!);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Patient details are required.");
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenPatientNameIsEmpty_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidCreatePatientDto();

            dto.FullName = "";

            Func<Task> action = async () =>
                await patientService.RegisterPatientAsync(dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Patient name is required.");
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenDateOfBirthIsBeforeMinimumDate_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidCreatePatientDto();

            dto.DateOfBirth = new DateTime(1899, 12, 31);

            Func<Task> action = async () =>
                await patientService.RegisterPatientAsync(dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Date of birth cannot be before 01 Jan 1900.");
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenDateOfBirthIsFutureDate_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidCreatePatientDto();

            dto.DateOfBirth = DateTime.Today.AddDays(1);

            Func<Task> action = async () =>
                await patientService.RegisterPatientAsync(dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Date of birth cannot be a future date.");
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenEmailIsEmpty_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidCreatePatientDto();

            dto.Email = "";

            Func<Task> action = async () =>
                await patientService.RegisterPatientAsync(dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Email address is required.");
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenPhoneNumberIsEmpty_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidCreatePatientDto();

            dto.PhoneNumber = "";

            Func<Task> action = async () =>
                await patientService.RegisterPatientAsync(dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Phone number is required.");
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenDuplicatePatientExists_ShouldThrowConflictException()
        {
            var dto = GetValidCreatePatientDto();

            repositoryMock
                .Setup(repository => repository.IsDuplicatePatientAsync(
                    dto.FullName.Trim().ToLower(),
                    dto.Email.Trim().ToLower(),
                    dto.PhoneNumber.Trim(),
                    dto.DateOfBirth.Date,
                    It.IsAny<int?>()))
                .ReturnsAsync(true);

            Func<Task> action = async () =>
                await patientService.RegisterPatientAsync(dto);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("A patient with similar details already exists.");
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenPatientIdIsInvalid_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidUpdatePatientDto();

            Func<Task> action = async () =>
                await patientService.UpdatePatientAsync(0, dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Please provide a valid patient reference.");
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenDtoIsNull_ShouldThrowBusinessRuleException()
        {
            Func<Task> action = async () =>
                await patientService.UpdatePatientAsync(1, null!);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Patient details are required.");
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenExistingPatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            var dto = GetValidUpdatePatientDto();

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () =>
                await patientService.UpdatePatientAsync(99, dto);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenDuplicatePatientExists_ShouldThrowConflictException()
        {
            var dto = GetValidUpdatePatientDto();

            var existingPatient = GetPatients().First();

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(existingPatient.PatientId))
                .ReturnsAsync(existingPatient);

            repositoryMock
                .Setup(repository => repository.IsDuplicatePatientAsync(
                    dto.FullName.Trim().ToLower(),
                    dto.Email.Trim().ToLower(),
                    dto.PhoneNumber.Trim(),
                    dto.DateOfBirth.Date,
                    existingPatient.PatientId))
                .ReturnsAsync(true);

            Func<Task> action = async () =>
                await patientService.UpdatePatientAsync(existingPatient.PatientId, dto);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Another patient with similar details already exists.");
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenRepositoryReturnsNull_ShouldThrowEntityNotFoundException()
        {
            var dto = GetValidUpdatePatientDto();

            var existingPatient = GetPatients().First();

            repositoryMock
                .Setup(repository => repository.GetByIdAsync(existingPatient.PatientId))
                .ReturnsAsync(existingPatient);

            repositoryMock
                .Setup(repository => repository.IsDuplicatePatientAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<int?>()))
                .ReturnsAsync(false);

            repositoryMock
                .Setup(repository => repository.UpdateAsync(
                    existingPatient.PatientId,
                    It.IsAny<Patient>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () =>
                await patientService.UpdatePatientAsync(existingPatient.PatientId, dto);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

       

        [Fact]
        public async Task GetMyProfileAsync_WhenIdentityUserIdIsEmpty_ShouldThrowBusinessRuleException()
        {
            Func<Task> action = async () =>
                await patientService.GetMyProfileAsync("");

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Invalid logged-in user.");
        }

        [Fact]
        public async Task GetMyProfileAsync_WhenPatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            repositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync("identity-1"))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () =>
                await patientService.GetMyProfileAsync("identity-1");

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetMyProfileAsync_WhenPatientExists_ShouldReturnMappedPatient()
        {
            var patient = GetPatients().First();

            patient.IdentityUserId = "identity-1";

            repositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync("identity-1"))
                .ReturnsAsync(patient);

            var result = await patientService.GetMyProfileAsync("identity-1");

            result.PatientId.Should().Be(patient.PatientId);

            result.FullName.Should().Be(patient.PatientName);
        }

        [Fact]
        public async Task UpdateMyProfileAsync_WhenIdentityUserIdIsEmpty_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidUpdatePatientDto();

            Func<Task> action = async () =>
                await patientService.UpdateMyProfileAsync("", dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Invalid logged-in user.");
        }

        [Fact]
        public async Task UpdateMyProfileAsync_WhenDtoIsNull_ShouldThrowBusinessRuleException()
        {
            Func<Task> action = async () =>
                await patientService.UpdateMyProfileAsync("identity-1", null!);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Patient details are required.");
        }

        [Fact]
        public async Task UpdateMyProfileAsync_WhenDateOfBirthIsBeforeMinimumDate_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidUpdatePatientDto();

            dto.DateOfBirth = new DateTime(1899, 12, 31);

            Func<Task> action = async () =>
                await patientService.UpdateMyProfileAsync("identity-1", dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Date of birth cannot be before 01 Jan 1900.");
        }

        [Fact]
        public async Task UpdateMyProfileAsync_WhenDateOfBirthIsFutureDate_ShouldThrowBusinessRuleException()
        {
            var dto = GetValidUpdatePatientDto();

            dto.DateOfBirth = DateTime.Today.AddDays(1);

            Func<Task> action = async () =>
                await patientService.UpdateMyProfileAsync("identity-1", dto);

            await action.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Date of birth cannot be a future date.");
        }

        [Fact]
        public async Task UpdateMyProfileAsync_WhenExistingPatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            var dto = GetValidUpdatePatientDto();

            repositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync("identity-1"))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () =>
                await patientService.UpdateMyProfileAsync("identity-1", dto);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task UpdateMyProfileAsync_WhenRepositoryReturnsNull_ShouldThrowEntityNotFoundException()
        {
            var dto = GetValidUpdatePatientDto();

            var existingPatient = GetPatients().First();

            existingPatient.IdentityUserId = "identity-1";

            repositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync("identity-1"))
                .ReturnsAsync(existingPatient);

            repositoryMock
                .Setup(repository => repository.UpdateAsync(
                    existingPatient.PatientId,
                    It.IsAny<Patient>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () =>
                await patientService.UpdateMyProfileAsync("identity-1", dto);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task UpdateMyProfileAsync_WhenValid_ShouldUpdateProfileAndReturnMappedPatient()
        {
            var dto = GetValidUpdatePatientDto();

            var existingPatient = GetPatients().First();

            existingPatient.IdentityUserId = "identity-1";

            existingPatient.Email = "original@example.com";

            repositoryMock
                .Setup(repository => repository.GetByIdentityUserIdAsync("identity-1"))
                .ReturnsAsync(existingPatient);

            repositoryMock
                .Setup(repository => repository.UpdateAsync(
                    existingPatient.PatientId,
                    It.IsAny<Patient>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((int patientId, Patient patient, CancellationToken cancellationToken) =>
                {
                    patient.PatientId = patientId;

                    return patient;
                });

            var result = await patientService.UpdateMyProfileAsync(
                "identity-1",
                dto);

            result.PatientId.Should().Be(existingPatient.PatientId);

            repositoryMock.Verify(
                repository => repository.UpdateAsync(
                    existingPatient.PatientId,
                    It.Is<Patient>(patient =>
                        patient.PatientId == existingPatient.PatientId &&
                        patient.IdentityUserId == existingPatient.IdentityUserId &&
                        patient.Email == existingPatient.Email &&
                        patient.CreatedDate == existingPatient.CreatedDate),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private void SetupMapper()
        {
            mapperMock
                .Setup(mapper => mapper.Map<List<PatientDto>>(It.IsAny<object>()))
                .Returns((object source) =>
                {
                    var patients = ((IEnumerable<Patient>)source).ToList();

                    return patients.Select(MapToPatientDto).ToList();
                });

            mapperMock
                .Setup(mapper => mapper.Map<PatientDto>(It.IsAny<object>()))
                .Returns((object source) =>
                    MapToPatientDto((Patient)source));

            mapperMock
                .Setup(mapper => mapper.Map<Patient>(It.IsAny<object>()))
                .Returns((object source) =>
                {
                    return source switch
                    {
                        CreatePatientDto createDto => new Patient
                        {
                            PatientName = createDto.FullName,
                            DateOfBirth = createDto.DateOfBirth,
                            Gender = createDto.Gender,
                            Email = createDto.Email,
                            PhoneNumber = createDto.PhoneNumber,
                            InsuranceID = createDto.InsuranceId
                        },

                        UpdatePatientDto updateDto => new Patient
                        {
                            PatientName = updateDto.FullName,
                            DateOfBirth = updateDto.DateOfBirth,
                            Gender = updateDto.Gender,
                            Email = updateDto.Email,
                            PhoneNumber = updateDto.PhoneNumber,
                            InsuranceID = updateDto.InsuranceId
                        },

                        _ => new Patient
                        {
                            PatientName = "Default Patient",
                            Email = "default@example.com",
                            PhoneNumber = "9876500000"
                        }
                    };
                });
        }

        private static PatientDto MapToPatientDto(Patient patient)
        {
            return new PatientDto
            {
                PatientId = patient.PatientId,

                FullName = patient.PatientName,

                DateOfBirth = patient.DateOfBirth.ToString("yyyy-MM-dd"),

                Gender = patient.Gender,

                Email = patient.Email,

                PhoneNumber = patient.PhoneNumber,

                InsuranceId = patient.InsuranceID,

                CreatedDate = patient.CreatedDate.ToString("yyyy-MM-dd")
            };
        }

        private static List<Patient> GetPatients()
        {
            return new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    PatientName = "Rishi Patient",
                    DateOfBirth = new DateTime(2000, 1, 1),
                    Gender = GenderType.Male,
                    Email = "rishi@example.com",
                    PhoneNumber = "9876543210",
                    InsuranceID = "INS100",
                    CreatedDate = new DateTime(2026, 1, 1)
                },

                new Patient
                {
                    PatientId = 2,
                    PatientName = "Meera Patient",
                    DateOfBirth = new DateTime(1998, 5, 10),
                    Gender = GenderType.Female,
                    Email = "meera@example.com",
                    PhoneNumber = "9876543211",
                    InsuranceID = "INS200",
                    CreatedDate = new DateTime(2026, 1, 2)
                },

                new Patient
                {
                    PatientId = 3,
                    PatientName = "Arun Patient",
                    DateOfBirth = new DateTime(1995, 3, 15),
                    Gender = GenderType.Male,
                    Email = "arun@example.com",
                    PhoneNumber = "9876543212",
                    InsuranceID = null,
                    CreatedDate = new DateTime(2026, 1, 3)
                },

                new Patient
                {
                    PatientId = 4,
                    PatientName = "Sara Patient",
                    DateOfBirth = new DateTime(1997, 7, 20),
                    Gender = GenderType.Female,
                    Email = "sara@example.com",
                    PhoneNumber = "9876543213",
                    InsuranceID = "",
                    CreatedDate = new DateTime(2026, 1, 4)
                }
            };
        }

        private static CreatePatientDto GetValidCreatePatientDto()
        {
            return new CreatePatientDto
            {
                FullName = "New Patient",

                DateOfBirth = new DateTime(2001, 2, 2),

                Gender = GenderType.Male,

                Email = "newpatient@example.com",

                PhoneNumber = "9876500000",

                InsuranceId = "INS999"
            };
        }

        private static UpdatePatientDto GetValidUpdatePatientDto()
        {
            return new UpdatePatientDto
            {
                FullName = "Updated Patient",

                DateOfBirth = new DateTime(2002, 3, 3),

                Gender = GenderType.Female,

                Email = "updatedpatient@example.com",

                PhoneNumber = "9876511111",

                InsuranceId = "INS888"
            };
        }
    }
}