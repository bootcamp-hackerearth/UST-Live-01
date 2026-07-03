using AutoMapper;
using FluentAssertions;
using HealthAxis.Shared.DTOs.Patient;
using HealthAxis.Shared.Enums;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Implementations;
using Moq;
using Xunit;

namespace HealthAxisCore_Api.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new PatientService(
                _patientRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        // ------------------------------------------------------------
        // GetAllAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task GetAllAsync_WhenPatientsExist_ShouldReturnMappedPatients()
        {
            var patients = GetSamplePatients();

            var mappedPatients = patients
                .Select(ToPatientResponseDto)
                .ToList();

            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<PatientResponseDto>>(patients))
                .Returns(mappedPatients);

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(patients.Count);
            result.First().PatientId.Should().Be(1);

            _patientRepositoryMock.Verify(
                repo => repo.GetAllAsync(),
                Times.Once
            );

            _mapperMock.Verify(
                mapper => mapper.Map<IEnumerable<PatientResponseDto>>(patients),
                Times.Once
            );
        }

        [Fact]
        public async Task GetAllAsync_WhenNoPatientsExist_ShouldReturnEmptyMappedList()
        {
            var patients = new List<Patient>();
            var mappedPatients = new List<PatientResponseDto>();

            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<PatientResponseDto>>(patients))
                .Returns(mappedPatients);

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _patientRepositoryMock.Verify(
                repo => repo.GetAllAsync(),
                Times.Once
            );
        }

        // ------------------------------------------------------------
        // GetPagedAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task GetPagedAsync_WhenPageNumberLessThanOne_ShouldDefaultToOne()
        {
            var patients = GetSamplePatients();

            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(patients);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                0,
                10,
                null,
                null
            );

            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalCount.Should().Be(patients.Count);
        }

        [Fact]
        public async Task GetPagedAsync_WhenPageSizeLessThanOne_ShouldDefaultToTen()
        {
            var patients = GetSamplePatients();

            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(patients);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                0,
                null,
                null
            );

            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task GetPagedAsync_WhenPageSizeGreaterThanHundred_ShouldLimitToHundred()
        {
            var patients = GetSamplePatients();

            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(patients);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                150,
                null,
                null
            );

            result.PageSize.Should().Be(100);
        }

        [Fact]
        public async Task GetPagedAsync_WithSearchByPatientName_ShouldReturnMatchingPatient()
        {
            var patients = GetSamplePatients();

            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(patients);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                "Asha",
                null
            );

            result.TotalCount.Should().Be(1);
            result.Items.First().PatientName.Should().Be("Asha Kumar");
        }

        [Fact]
        public async Task GetPagedAsync_WithSearchByEmail_ShouldReturnMatchingPatient()
        {
            var patients = GetSamplePatients();

            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(patients);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                "ravi",
                null
            );

            result.TotalCount.Should().Be(1);
            result.Items.First().Email.Should().Be("ravi@test.com");
        }

        [Fact]
        public async Task GetPagedAsync_WithSearchByPhoneNumber_ShouldReturnMatchingPatient()
        {
            var patients = GetSamplePatients();

            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(patients);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                "9876543210",
                null
            );

            result.TotalCount.Should().Be(1);
            result.Items.First().PhoneNumber.Should().Be("9876543210");
        }

        [Fact]
        public async Task GetPagedAsync_WithGenderFilter_ShouldReturnMatchingPatients()
        {
            var patients = GetSamplePatients();

            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(patients);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                null,
                "Female"
            );

            result.Items.Should().OnlyContain(x => x.Gender == GenderType.Female);
        }

        [Fact]
        public async Task GetPagedAsync_WithGenderAll_ShouldNotFilterByGender()
        {
            var patients = GetSamplePatients();

            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(patients);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                null,
                "All"
            );

            result.TotalCount.Should().Be(patients.Count);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldApplyPagination()
        {
            var patients = GetSamplePatients();

            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(patients);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                2,
                null,
                null
            );

            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(patients.Count);
            result.TotalPages.Should().Be((int)Math.Ceiling(patients.Count / 2.0));
        }

        [Fact]
        public async Task GetPagedAsync_ShouldOrderPatientsByName()
        {
            var patients = GetSamplePatients();

            _patientRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(patients);

            SetupPagedMapper();

            var result = await _service.GetPagedAsync(
                1,
                10,
                null,
                null
            );

            result.Items.First().PatientName.Should().Be("Asha Kumar");
        }

        // ------------------------------------------------------------
        // GetByIdAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task GetByIdAsync_WhenPatientExists_ShouldReturnMappedPatient()
        {
            var patient = new Patient
            {
                PatientId = 1,
                PatientName = "Asha Kumar",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Gender = GenderType.Female,
                Email = "asha@test.com",
                PhoneNumber = "9876543210",
                InsuranceID = "INS001"
            };

            var mappedPatient = ToPatientResponseDto(patient);

            _patientRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(mapper => mapper.Map<PatientResponseDto>(patient))
                .Returns(mappedPatient);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.PatientId.Should().Be(1);
            result.PatientName.Should().Be("Asha Kumar");
            result.Email.Should().Be("asha@test.com");
            result.PhoneNumber.Should().Be("9876543210");
            result.Gender.Should().Be(GenderType.Female);

            _patientRepositoryMock.Verify(
                repo => repo.GetByIdAsync(1),
                Times.Once
            );

            _mapperMock.Verify(
                mapper => mapper.Map<PatientResponseDto>(patient),
                Times.Once
            );
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            _patientRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            var act = async () => await _service.GetByIdAsync(1);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Patient not found");

            _mapperMock.Verify(
                mapper => mapper.Map<PatientResponseDto>(It.IsAny<Patient>()),
                Times.Never
            );
        }

        // ------------------------------------------------------------
        // CreateAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task CreateAsync_WhenDuplicatePatientExists_ShouldThrowBusinessRuleException()
        {
            var dto = new CreatePatientDto
            {
                PatientName = "Asha Kumar",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Gender = GenderType.Female,
                Email = "asha@test.com",
                PhoneNumber = "9876543210",
                InsuranceID = "INS001"
            };

            _patientRepositoryMock
                .Setup(repo => repo.IsDuplicate(dto.Email, dto.PhoneNumber))
                .ReturnsAsync(true);

            var act = async () => await _service.CreateAsync(dto);

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Patient already exists with same email or phone number");

            _patientRepositoryMock.Verify(
                repo => repo.IsDuplicate(dto.Email, dto.PhoneNumber),
                Times.Once
            );

            _patientRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Patient>()),
                Times.Never
            );

            _mapperMock.Verify(
                mapper => mapper.Map<Patient>(It.IsAny<CreatePatientDto>()),
                Times.Never
            );
        }

        [Fact]
        public async Task CreateAsync_WhenValidRequest_ShouldCreatePatientAndReturnMappedResponse()
        {
            var dto = new CreatePatientDto
            {
                PatientName = "Asha Kumar",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Gender = GenderType.Female,
                Email = "asha@test.com",
                PhoneNumber = "9876543210",
                InsuranceID = "INS001"
            };

            var patient = new Patient
            {
                PatientName = dto.PatientName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                InsuranceID = dto.InsuranceID
            };

            var response = new PatientResponseDto
            {
                PatientId = 101,
                PatientName = dto.PatientName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            _patientRepositoryMock
                .Setup(repo => repo.IsDuplicate(dto.Email, dto.PhoneNumber))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(mapper => mapper.Map<Patient>(dto))
                .Returns(patient);

            _patientRepositoryMock
                .Setup(repo => repo.AddAsync(patient))
                .Callback<Patient>(p => p.PatientId = 101)
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(mapper => mapper.Map<PatientResponseDto>(patient))
                .Returns(response);

            var result = await _service.CreateAsync(dto);

            result.Should().NotBeNull();
            result.PatientId.Should().Be(101);
            result.PatientName.Should().Be(dto.PatientName);
            result.Email.Should().Be(dto.Email);
            result.PhoneNumber.Should().Be(dto.PhoneNumber);
            result.Gender.Should().Be(dto.Gender);

            patient.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

            _patientRepositoryMock.Verify(
                repo => repo.IsDuplicate(dto.Email, dto.PhoneNumber),
                Times.Once
            );

            _patientRepositoryMock.Verify(
                repo => repo.AddAsync(patient),
                Times.Once
            );

            _mapperMock.Verify(
                mapper => mapper.Map<Patient>(dto),
                Times.Once
            );

            _mapperMock.Verify(
                mapper => mapper.Map<PatientResponseDto>(patient),
                Times.Once
            );
        }

        // ------------------------------------------------------------
        // UpdateAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task UpdateAsync_WhenPatientExists_ShouldUpdatePatientAndReturnTrue()
        {
            var patient = new Patient
            {
                PatientId = 1,
                PatientName = "Old Name",
                DateOfBirth = DateTime.Today.AddYears(-30),
                Gender = GenderType.Male,
                Email = "old@test.com",
                PhoneNumber = "1111111111",
                InsuranceID = "OLDINS"
            };

            var dto = new UpdatePatientDto
            {
                PatientName = "New Name",
                DateOfBirth = DateTime.Today.AddYears(-28),
                Gender = GenderType.Female,
                Email = "new@test.com",
                PhoneNumber = "9999999999",
                InsuranceID = "NEWINS"
            };

            _patientRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(mapper => mapper.Map(dto, patient))
                .Callback<UpdatePatientDto, Patient>((source, destination) =>
                {
                    destination.PatientName = source.PatientName;
                    destination.DateOfBirth = source.DateOfBirth;
                    destination.Gender = source.Gender;
                    destination.Email = source.Email;
                    destination.PhoneNumber = source.PhoneNumber;
                    destination.InsuranceID = source.InsuranceID;
                })
                .Returns(patient);

            _patientRepositoryMock
                .Setup(repo => repo.UpdateAsync(patient))
                .Returns(Task.CompletedTask);

            var result = await _service.UpdateAsync(1, dto);

            result.Should().BeTrue();

            patient.PatientName.Should().Be(dto.PatientName);
            patient.DateOfBirth.Should().Be(dto.DateOfBirth);
            patient.Gender.Should().Be(dto.Gender);
            patient.Email.Should().Be(dto.Email);
            patient.PhoneNumber.Should().Be(dto.PhoneNumber);
            patient.InsuranceID.Should().Be(dto.InsuranceID);

            _patientRepositoryMock.Verify(
                repo => repo.GetByIdAsync(1),
                Times.Once
            );

            _mapperMock.Verify(
                mapper => mapper.Map(dto, patient),
                Times.Once
            );

            _patientRepositoryMock.Verify(
                repo => repo.UpdateAsync(patient),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateAsync_WhenPatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            var dto = new UpdatePatientDto
            {
                PatientName = "New Name",
                DateOfBirth = DateTime.Today.AddYears(-28),
                Gender = GenderType.Female,
                Email = "new@test.com",
                PhoneNumber = "9999999999",
                InsuranceID = "NEWINS"
            };

            _patientRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            var act = async () => await _service.UpdateAsync(1, dto);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Patient not found");

            _patientRepositoryMock.Verify(
                repo => repo.UpdateAsync(It.IsAny<Patient>()),
                Times.Never
            );

            _mapperMock.Verify(
                mapper => mapper.Map(It.IsAny<UpdatePatientDto>(), It.IsAny<Patient>()),
                Times.Never
            );
        }

        // ------------------------------------------------------------
        // DeleteAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task DeleteAsync_WhenPatientExists_ShouldDeletePatientAndReturnTrue()
        {
            _patientRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(true);

            _patientRepositoryMock
                .Setup(repo => repo.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            var result = await _service.DeleteAsync(1);

            result.Should().BeTrue();

            _patientRepositoryMock.Verify(
                repo => repo.Exists(1),
                Times.Once
            );

            _patientRepositoryMock.Verify(
                repo => repo.DeleteAsync(1),
                Times.Once
            );
        }

        [Fact]
        public async Task DeleteAsync_WhenPatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            _patientRepositoryMock
                .Setup(repo => repo.Exists(1))
                .ReturnsAsync(false);

            var act = async () => await _service.DeleteAsync(1);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Patient not found");

            _patientRepositoryMock.Verify(
                repo => repo.DeleteAsync(It.IsAny<int>()),
                Times.Never
            );
        }

        // ------------------------------------------------------------
        // SearchAsync
        // ------------------------------------------------------------

        [Fact]
        public async Task SearchAsync_WhenPatientsFound_ShouldReturnMappedPatients()
        {
            var patients = new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    PatientName = "Asha Kumar",
                    DateOfBirth = DateTime.Today.AddYears(-25),
                    Gender = GenderType.Female,
                    Email = "asha@test.com",
                    PhoneNumber = "9876543210",
                    InsuranceID = "INS001"
                }
            };

            var mappedPatients = patients
                .Select(ToPatientResponseDto)
                .ToList();

            _patientRepositoryMock
                .Setup(repo => repo.SearchPatients("Asha", "asha@test.com"))
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<PatientResponseDto>>(patients))
                .Returns(mappedPatients);

            var result = await _service.SearchAsync("Asha", "asha@test.com");

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().PatientName.Should().Be("Asha Kumar");

            _patientRepositoryMock.Verify(
                repo => repo.SearchPatients("Asha", "asha@test.com"),
                Times.Once
            );

            _mapperMock.Verify(
                mapper => mapper.Map<IEnumerable<PatientResponseDto>>(patients),
                Times.Once
            );
        }

        [Fact]
        public async Task SearchAsync_WhenNoPatientsFound_ShouldReturnEmptyMappedList()
        {
            var patients = new List<Patient>();
            var mappedPatients = new List<PatientResponseDto>();

            _patientRepositoryMock
                .Setup(repo => repo.SearchPatients(null, null))
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<PatientResponseDto>>(patients))
                .Returns(mappedPatients);

            var result = await _service.SearchAsync(null, null);

            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _patientRepositoryMock.Verify(
                repo => repo.SearchPatients(null, null),
                Times.Once
            );
        }

        // ------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------

        private static List<Patient> GetSamplePatients()
        {
            return new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    PatientName = "Ravi Menon",
                    DateOfBirth = DateTime.Today.AddYears(-35),
                    Gender = GenderType.Male,
                    Email = "ravi@test.com",
                    PhoneNumber = "1111111111",
                    InsuranceID = "INS001"
                },
                new Patient
                {
                    PatientId = 2,
                    PatientName = "Asha Kumar",
                    DateOfBirth = DateTime.Today.AddYears(-25),
                    Gender = GenderType.Female,
                    Email = "asha@test.com",
                    PhoneNumber = "9876543210",
                    InsuranceID = "INS002"
                },
                new Patient
                {
                    PatientId = 3,
                    PatientName = "Meera Nair",
                    DateOfBirth = DateTime.Today.AddYears(-30),
                    Gender = GenderType.Female,
                    Email = "meera@test.com",
                    PhoneNumber = "2222222222",
                    InsuranceID = "INS003"
                },
                new Patient
                {
                    PatientId = 4,
                    PatientName = "John Mathew",
                    DateOfBirth = DateTime.Today.AddYears(-40),
                    Gender = GenderType.Male,
                    Email = "john@test.com",
                    PhoneNumber = "3333333333",
                    InsuranceID = "INS004"
                }
            };
        }

        private void SetupPagedMapper()
        {
            _mapperMock
                .Setup(mapper => mapper.Map<List<PatientResponseDto>>(It.IsAny<List<Patient>>()))
                .Returns((List<Patient> source) =>
                    source.Select(ToPatientResponseDto).ToList()
                );
        }

        private static PatientResponseDto ToPatientResponseDto(Patient patient)
        {
            return new PatientResponseDto
            {
                PatientId = patient.PatientId,
                PatientName = patient.PatientName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber
            };
        }
    }
}

