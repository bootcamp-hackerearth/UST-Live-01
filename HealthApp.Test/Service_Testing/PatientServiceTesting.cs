using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Impl;
using HealthApp.Shared.Dto;
using Moq;
using Xunit;

namespace HealthApp.Test.Service_Testing
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _repoMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new PatientService(
                _repoMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task AddPatientAsync_Should_Return_Patient()
        {
            var dto = new PatientDto
            {
                FullName = "John",
                Email = "john@test.com"
            };

            var patient = new Patient();

            _repoMock.Setup(x => x.EmailExistsAsync(dto.Email))
                .ReturnsAsync(false);

            _mapperMock.Setup(x => x.Map<Patient>(dto))
                .Returns(patient);

            _repoMock.Setup(x => x.addAsync(patient))
                .ReturnsAsync(patient);

            _mapperMock.Setup(x => x.Map<PatientDto>(patient))
                .Returns(dto);

            var result = await _service.AddPatientAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Email, result.Email);
        }

        [Fact]
        public async Task AddPatientAsync_Should_Throw_When_Patient_Null()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.AddPatientAsync(null!));
        }

        [Fact]
        public async Task AddPatientAsync_Should_Throw_When_Name_Empty()
        {
            var dto = new PatientDto
            {
                FullName = "",
                Email = "john@test.com"
            };

            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.AddPatientAsync(dto));
        }

        [Fact]
        public async Task AddPatientAsync_Should_Throw_When_Email_Empty()
        {
            var dto = new PatientDto
            {
                FullName = "John",
                Email = ""
            };

            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.AddPatientAsync(dto));
        }

        [Fact]
        public async Task AddPatientAsync_Should_Throw_When_Email_Already_Exists()
        {
            var dto = new PatientDto
            {
                FullName = "John",
                Email = "john@test.com"
            };

            _repoMock.Setup(x => x.EmailExistsAsync(dto.Email))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<ConflictException>(
                () => _service.AddPatientAsync(dto));
        }

        [Fact]
        public async Task AddPatientAsync_Should_Throw_When_Save_Fails()
        {
            var dto = new PatientDto
            {
                FullName = "John",
                Email = "john@test.com"
            };

            var patient = new Patient();

            _repoMock.Setup(x => x.EmailExistsAsync(dto.Email))
                .ReturnsAsync(false);

            _mapperMock.Setup(x => x.Map<Patient>(dto))
                .Returns(patient);

            _repoMock.Setup(x => x.addAsync(patient))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.AddPatientAsync(dto));
        }

        [Fact]
        public async Task GetPagedPatientsAsync_Should_Return_Data()
        {
            var patients = new List<Patient>
            {
                new Patient()
            };

            var dtos = new List<PatientDto>
            {
                new PatientDto()
            };

            _repoMock.Setup(x =>
                x.GetPagedPatientsAsync(1, 10, null))
                .ReturnsAsync((patients, 1));

            _mapperMock.Setup(x =>
                x.Map<List<PatientDto>>(patients))
                .Returns(dtos);

            var result = await _service.GetPagedPatientsAsync(1, 10);

            Assert.Equal(1, result.TotalCount);
            Assert.Single(result.Items);
        }

        [Fact]
        public async Task GetPagedPatientsAsync_Should_Throw_Invalid_Page_Number()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.GetPagedPatientsAsync(0, 10));
        }

        [Fact]
        public async Task GetPagedPatientsAsync_Should_Throw_Invalid_Page_Size()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.GetPagedPatientsAsync(1, 0));
        }

        [Fact]
        public async Task GetPatientByIdAsync_Should_Return_Patient()
        {
            var patient = new Patient
            {
                PatientId = 1
            };

            var dto = new PatientDto
            {
                PatientId = 1
            };

            _repoMock.Setup(x => x.getbyidAsync(1))
                .ReturnsAsync(patient);

            _mapperMock.Setup(x => x.Map<PatientDto>(patient))
                .Returns(dto);

            var result = await _service.GetPatientByIdAsync(1);

            Assert.Equal(1, result.PatientId);
        }

        [Fact]
        public async Task GetPatientByIdAsync_Should_Throw_Invalid_Id()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.GetPatientByIdAsync(0));
        }

        [Fact]
        public async Task GetPatientByIdAsync_Should_Throw_NotFound()
        {
            _repoMock.Setup(x => x.getbyidAsync(1))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.GetPatientByIdAsync(1));
        }

        [Fact]
        public async Task GetMyProfileAsync_Should_Return_Profile()
        {
            var patient = new Patient();
            var dto = new PatientDto();

            _repoMock.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync(patient);

            _mapperMock.Setup(x => x.Map<PatientDto>(patient))
                .Returns(dto);

            var result = await _service.GetMyProfileAsync("user1");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetMyProfileAsync_Should_Throw_Invalid_User()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.GetMyProfileAsync(""));
        }

        [Fact]
        public async Task GetMyProfileAsync_Should_Throw_Profile_NotFound()
        {
            _repoMock.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.GetMyProfileAsync("user1"));
        }
        [Fact]
        public async Task UpdateMyProfileAsync_Should_Return_Updated_Patient()
        {
            var existingPatient = new Patient
            {
                PatientId = 1,
                IdentityUserId = "user1",
                FullName = "Old",
                PhoneNumber = "1234567890"
            };

            var updateDto = new PatientUpdateDto
            {
                FullName = "John Doe",
                PhoneNumber = "9876543210",
                DateOfBirth = DateTime.Now.AddYears(-20),
                InsuranceId = "INS001"
            };

            var updatedPatient = new Patient
            {
                PatientId = 1,
                IdentityUserId = "user1",
                FullName = updateDto.FullName,
                PhoneNumber = updateDto.PhoneNumber,
                DateOfBirth = updateDto.DateOfBirth.Value,
                InsuranceId = updateDto.InsuranceId
            };

            var patientDto = new PatientDto
            {
                PatientId = 1,
                FullName = updateDto.FullName
            };

            _repoMock.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync(existingPatient);

            _repoMock.Setup(x => x.updateAsync(1, It.IsAny<Patient>()))
                .ReturnsAsync(updatedPatient);

            _mapperMock.Setup(x => x.Map<PatientDto>(updatedPatient))
                .Returns(patientDto);

            var result = await _service.UpdateMyProfileAsync("user1", updateDto);

            Assert.NotNull(result);
            Assert.Equal(1, result.PatientId);
        }
        [Fact]
        public async Task UpdateMyProfileAsync_Should_Throw_Invalid_User()
        {
            var dto = new PatientUpdateDto();

            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.UpdateMyProfileAsync("", dto));
        }

        [Fact]
        public async Task UpdateMyProfileAsync_Should_Throw_When_Dto_Null()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(
                   () => _service.UpdateMyProfileAsync("user1", null!));

        }
        [Fact]
        public async Task UpdateMyProfileAsync_Should_Throw_When_Name_Empty()
        {
            var dto = new PatientUpdateDto
            {
                FullName = "",
                PhoneNumber = "9876543210",
                DateOfBirth = DateTime.Today
            };

            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.UpdateMyProfileAsync("user1", dto));
        }
        [Fact]
        public async Task UpdateMyProfileAsync_Should_Throw_When_Phone_Empty()
        {
            var dto = new PatientUpdateDto
            {
                FullName = "John",
                PhoneNumber = "",
                DateOfBirth = DateTime.Today
            };

            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.UpdateMyProfileAsync("user1", dto));
        }
        [Fact]
        public async Task UpdateMyProfileAsync_Should_Throw_When_Dob_Missing()
        {
            var dto = new PatientUpdateDto
            {
                FullName = "John",
                PhoneNumber = "9876543210"
            };

            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.UpdateMyProfileAsync("user1", dto));
        }

        [Fact]
        public async Task UpdateMyProfileAsync_Should_Throw_Profile_NotFound()
        {
            var dto = new PatientUpdateDto
            {
                FullName = "John",
                PhoneNumber = "9876543210",
                DateOfBirth = DateTime.Today
            };

            _repoMock.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync((Patient)null!);
            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.UpdateMyProfileAsync("user1", dto));
        }
        [Fact]
        public async Task UpdateMyProfileAsync_Should_Throw_When_Update_Fails()
        {
            var existingPatient = new Patient
            {
                PatientId = 1,
                IdentityUserId = "user1"
            };

            var dto = new PatientUpdateDto
            {
                FullName = "John",
                PhoneNumber = "9876543210",
                DateOfBirth = DateTime.Today
            };

            _repoMock.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync(existingPatient);

            _repoMock.Setup(x => x.updateAsync(1, It.IsAny<Patient>()))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.UpdateMyProfileAsync("user1", dto));
        }
        [Fact]
        public async Task UpdateMyProfileAsync_Should_Set_InsuranceId_Null()
        {
            var existingPatient = new Patient
            {
                PatientId = 1,
                IdentityUserId = "user1"
            };

            var dto = new PatientUpdateDto
            {
                FullName = "John",
                PhoneNumber = "9876543210",
                DateOfBirth = DateTime.Today,
                InsuranceId = ""
            };

            _repoMock.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync(existingPatient);

            _repoMock.Setup(x => x.updateAsync(1, It.IsAny<Patient>()))
                .ReturnsAsync(existingPatient);

            _mapperMock.Setup(x => x.Map<PatientDto>(existingPatient))
                .Returns(new PatientDto());

            await _service.UpdateMyProfileAsync("user1", dto);

            Assert.Null(existingPatient.InsuranceId);
        }
    }
}