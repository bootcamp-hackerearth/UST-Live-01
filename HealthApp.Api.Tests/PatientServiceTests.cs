using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Impl;
using HealthApp.Shared.Dtos;
using Moq;
using Xunit;

namespace HealthApp.Api.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _patientService;

        public PatientServiceTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();

            _patientService = new PatientService(
                _patientRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ShouldReturnPatientDtos()
        {
            var patients = new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    FullName = "John Doe",
                    DateOfBirth = new DateOnly(1998, 1, 1),
                    Gender = "Male",
                    PhoneNumber = "9876543210",
                    Email = "john@test.com",
                    InsuranceId = "INS001",
                    CreatedDate = DateTime.Now
                }
            };

            var patientDtos = new List<PatientDto>
            {
                new PatientDto
                {
                    PatientId = 1,
                    FullName = "John Doe",
                    DateOfBirth = new DateOnly(1998, 1, 1),
                    Gender = "Male",
                    PhoneNumber = "9876543210",
                    Email = "john@test.com",
                    InsuranceId = "INS001",
                    CreatedDate = DateTime.Now
                }
            };

            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<PatientDto>>(patients))
                .Returns(patientDtos);

            var result = await _patientService.GetAllPatientsAsync();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("John Doe", result.First().FullName);

            _patientRepositoryMock.Verify(
                repo => repo.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetPatientByIdAsync_WhenIdIsInvalid_ShouldThrowInvalidRequestException()
        {
            var act = async () => await _patientService.GetPatientByIdAsync(0);

            var exception = await Assert.ThrowsAsync<InvalidRequestException>(act);
            Assert.Equal("Valid patient id is required.", exception.Message);

            _patientRepositoryMock.Verify(
                repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetPatientByIdAsync_WhenPatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            int patientId = 10;

            _patientRepositoryMock
                .Setup(repo => repo.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            var act = async () => await _patientService.GetPatientByIdAsync(patientId);

            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(act);
            Assert.Equal("Patient with id '10' was not found.", exception.Message);
        }

        [Fact]
        public async Task GetPatientByIdAsync_WhenPatientExists_ShouldReturnPatientDto()
        {
            int patientId = 1;

            var patient = new Patient
            {
                PatientId = patientId,
                FullName = "John Doe",
                DateOfBirth = new DateOnly(1998, 1, 1),
                Gender = "Male",
                PhoneNumber = "9876543210",
                Email = "john@test.com",
                InsuranceId = "INS001",
                CreatedDate = DateTime.Now
            };

            var patientDto = new PatientDto
            {
                PatientId = patientId,
                FullName = "John Doe",
                DateOfBirth = new DateOnly(1998, 1, 1),
                Gender = "Male",
                PhoneNumber = "9876543210",
                Email = "john@test.com",
                InsuranceId = "INS001",
                CreatedDate = patient.CreatedDate
            };

            _patientRepositoryMock
                .Setup(repo => repo.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(mapper => mapper.Map<PatientDto>(patient))
                .Returns(patientDto);

            var result = await _patientService.GetPatientByIdAsync(patientId);

            Assert.NotNull(result);
            Assert.Equal(patientId, result.PatientId);
            Assert.Equal("John Doe", result.FullName);
            Assert.Equal("john@test.com", result.Email);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenDtoIsNull_ShouldThrowInvalidRequestException()
        {
            var act = async () => await _patientService.RegisterPatientAsync(null!);

            var exception = await Assert.ThrowsAsync<InvalidRequestException>(act);
            Assert.Equal("Patient data is required.", exception.Message);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenNameIsEmpty_ShouldThrowInvalidRequestException()
        {
            var dto = GetValidPatientCreateDto();
            dto.FullName = "";

            var act = async () => await _patientService.RegisterPatientAsync(dto);

            var exception = await Assert.ThrowsAsync<InvalidRequestException>(act);
            Assert.Equal("Patient name is required.", exception.Message);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenDateOfBirthIsMissing_ShouldThrowInvalidRequestException()
        {
            var dto = GetValidPatientCreateDto();
            dto.DateOfBirth = null;

            var act = async () => await _patientService.RegisterPatientAsync(dto);

            var exception = await Assert.ThrowsAsync<InvalidRequestException>(act);
            Assert.Equal("Date of birth is required.", exception.Message);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenGenderIsInvalid_ShouldThrowInvalidRequestException()
        {
            var dto = GetValidPatientCreateDto();
            dto.Gender = "InvalidGender";

            var act = async () => await _patientService.RegisterPatientAsync(dto);

            var exception = await Assert.ThrowsAsync<InvalidRequestException>(act);
            Assert.Equal("Invalid gender.", exception.Message);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenDateOfBirthIsFuture_ShouldThrowBusinessRuleViolationException()
        {
            var dto = GetValidPatientCreateDto();
            dto.DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

            var act = async () => await _patientService.RegisterPatientAsync(dto);

            var exception = await Assert.ThrowsAsync<BusinessRuleViolationException>(act);
            Assert.Equal("Future date is not allowed.", exception.Message);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenEmailAlreadyExists_ShouldThrowDuplicateEntityException()
        {
            var dto = GetValidPatientCreateDto();

            var existingPatients = new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    FullName = "Existing Patient",
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    Gender = "Male",
                    PhoneNumber = "9999999999",
                    Email = "patient@test.com"
                }
            };

            _patientRepositoryMock
                .Setup(repo => repo.GetPatientsAsync(
                    null,
                    "patient@test.com",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingPatients);

            var act = async () => await _patientService.RegisterPatientAsync(dto);

            var exception = await Assert.ThrowsAsync<DuplicateEntityException>(act);
            Assert.Equal("A patient with this email already exists.", exception.Message);

            _patientRepositoryMock.Verify(
                repo => repo.Add(It.IsAny<Patient>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenDuplicatePatientExists_ShouldThrowDuplicateEntityException()
        {
            var dto = GetValidPatientCreateDto();

            _patientRepositoryMock
                .Setup(repo => repo.GetPatientsAsync(
                    null,
                    "patient@test.com",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Patient>());

            _patientRepositoryMock
                .Setup(repo => repo.IsDuplicatePatient(
                    "John Doe",
                    dto.DateOfBirth!.Value.ToDateTime(TimeOnly.MinValue),
                    "patient@test.com",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var act = async () => await _patientService.RegisterPatientAsync(dto);

            var exception = await Assert.ThrowsAsync<DuplicateEntityException>(act);
            Assert.Equal(
                "A patient with same name, date of birth and email already exists.",
                exception.Message);

            _patientRepositoryMock.Verify(
                repo => repo.Add(It.IsAny<Patient>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task RegisterPatientAsync_WhenValid_ShouldAddPatient()
        {
            var dto = GetValidPatientCreateDto();

            var mappedPatient = new Patient
            {
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth!.Value,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                InsuranceId = dto.InsuranceId
            };

            _patientRepositoryMock
                .Setup(repo => repo.GetPatientsAsync(
                    null,
                    "patient@test.com",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Patient>());

            _patientRepositoryMock
                .Setup(repo => repo.IsDuplicatePatient(
                    "John Doe",
                    dto.DateOfBirth!.Value.ToDateTime(TimeOnly.MinValue),
                    "patient@test.com",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(mapper => mapper.Map<Patient>(dto))
                .Returns(mappedPatient);

            _patientRepositoryMock
                .Setup(repo => repo.Add(It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient patient, CancellationToken _) => patient);

            await _patientService.RegisterPatientAsync(dto);

            _patientRepositoryMock.Verify(
                repo => repo.Add(
                    It.Is<Patient>(patient =>
                        patient.FullName == "John Doe" &&
                        patient.DateOfBirth == new DateOnly(1998, 1, 1) &&
                        patient.Gender == "Male" &&
                        patient.PhoneNumber == "9876543210" &&
                        patient.Email == "patient@test.com" &&
                        patient.InsuranceId == "INS001"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task SearchPatientsAsync_ShouldReturnMappedPatientDtos()
        {
            var patients = new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    FullName = "John Doe",
                    DateOfBirth = new DateOnly(1998, 1, 1),
                    Gender = "Male",
                    PhoneNumber = "9876543210",
                    Email = "patient@test.com"
                }
            };

            var patientDtos = new List<PatientDto>
            {
                new PatientDto
                {
                    PatientId = 1,
                    FullName = "John Doe",
                    DateOfBirth = new DateOnly(1998, 1, 1),
                    Gender = "Male",
                    PhoneNumber = "9876543210",
                    Email = "patient@test.com"
                }
            };

            _patientRepositoryMock
                .Setup(repo => repo.GetPatientsAsync(
                    "John",
                    "patient@test.com",
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<PatientDto>>(patients))
                .Returns(patientDtos);

            var result = await _patientService.SearchPatientsAsync(
                "John",
                "patient@test.com");

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("John Doe", result.First().FullName);

            _patientRepositoryMock.Verify(
                repo => repo.GetPatientsAsync(
                    "John",
                    "patient@test.com",
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Register_ShouldThrow_WhenGenderEmpty()
        {
            var dto = GetValidPatientCreateDto();
            dto.Gender = "";

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _patientService.RegisterPatientAsync(dto));
        }

        [Fact]
        public async Task Register_ShouldThrow_WhenPhoneEmpty()
        {
            var dto = GetValidPatientCreateDto();
            dto.PhoneNumber = "";

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _patientService.RegisterPatientAsync(dto));
        }

        [Fact]
        public async Task Register_ShouldThrow_WhenEmailEmpty()
        {
            var dto = GetValidPatientCreateDto();
            dto.Email = "";

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _patientService.RegisterPatientAsync(dto));
        }

        [Fact]
        public async Task Register_ShouldThrow_WhenEmailExists_IgnoreCase()
        {
            var dto = GetValidPatientCreateDto();
            dto.Email = "TEST@TEST.COM";

            _patientRepositoryMock
                .Setup(x => x.GetPatientsAsync(
                    null,
                    It.IsAny<string>(),   
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Patient>
                {
            new Patient { Email = "test@test.com" }
                });

            await Assert.ThrowsAsync<DuplicateEntityException>(() =>
                _patientService.RegisterPatientAsync(dto));
        }


        [Fact]
        public async Task Update_ShouldThrow_WhenDtoNull()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _patientService.UpdatePatientAsync(1, null!));
        }

        [Fact]
        public async Task Update_ShouldThrow_WhenPatientNotFound()
        {
            var dto = GetValidPatientCreateDto();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _patientService.UpdatePatientAsync(1, dto));
        }

        [Fact]
        public async Task Update_ShouldThrow_WhenEmailUsedByAnother()
        {
            var dto = GetValidPatientCreateDto();

            _patientRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Patient { PatientId = 1 });

            _patientRepositoryMock.Setup(x =>
                x.GetPatientsAsync(null, dto.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Patient>
                {
            new Patient { PatientId = 2, Email = dto.Email }
                });

            await Assert.ThrowsAsync<DuplicateEntityException>(() =>
                _patientService.UpdatePatientAsync(1, dto));
        }

        [Fact]
        public async Task Update_ShouldUpdate_WhenValid()
        {
            var dto = GetValidPatientCreateDto();

            var patient = new Patient { PatientId = 1 };

            _patientRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _patientRepositoryMock.Setup(x =>
                x.GetPatientsAsync(null, dto.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Patient>());

            await _patientService.UpdatePatientAsync(1, dto);

            _patientRepositoryMock.Verify(x =>
                x.Update(1, It.IsAny<Patient>()), Times.Once);
        }

        private static PatientCreateDto GetValidPatientCreateDto()
        {
            return new PatientCreateDto
            {
                FullName = "John Doe",
                DateOfBirth = new DateOnly(1998, 1, 1),
                Gender = "Male",
                PhoneNumber = "9876543210",
                Email = "patient@test.com",
                InsuranceId = "INS001"
            };
        }
    }
}