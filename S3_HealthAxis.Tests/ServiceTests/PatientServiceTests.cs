using FluentAssertions;
using Moq;
using S3_HealthAxis.Shared.DTOs.Patient;
using S3_HealthAxis.Shared.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Implementation;
using Xunit;

namespace S3_HealthAxis.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _service = new PatientService(_patientRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                BuildPatient(1, "Rahul Sharma", true),
                BuildPatient(2, "Anita Menon", false)
            };

            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(patients);

            // Act
            var result = (await _service.GetAllAsync()).ToList();

            // Assert
            result.Should().HaveCount(2);

            result[0].PatientId.Should().Be(1);
            result[0].FullName.Should().Be("Rahul Sharma");
            result[0].DateOfBirth.Should().Be(new DateOnly(1995, 5, 20));
            result[0].Gender.Should().Be(Gender.Male);
            result[0].PhoneNumber.Should().Be("9999999999");
            result[0].Email.Should().Be("patient1@test.com");
            result[0].InsuranceId.Should().Be("INS-1");
            result[0].IsActive.Should().BeTrue();

            result[1].PatientId.Should().Be(2);
            result[1].FullName.Should().Be("Anita Menon");
            result[1].IsActive.Should().BeFalse();

            _patientRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoPatients_ShouldReturnEmptyList()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            // Act
            var result = (await _service.GetAllAsync()).ToList();

            // Assert
            result.Should().BeEmpty();

            _patientRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedPatient_WhenPatientExists()
        {
            // Arrange
            var patient = BuildPatient(10, "Rahul Sharma", true);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(patient);

            // Act
            var result = await _service.GetByIdAsync(10);

            // Assert
            result.Should().NotBeNull();
            result!.PatientId.Should().Be(10);
            result.FullName.Should().Be("Rahul Sharma");
            result.Gender.Should().Be(Gender.Male);
            result.PhoneNumber.Should().Be("9999999999");
            result.Email.Should().Be("patient10@test.com");
            result.InsuranceId.Should().Be("INS-10");
            result.IsActive.Should().BeTrue();

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(10), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenPatientDoesNotExist()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Patient?)null);

            // Act
            var result = await _service.GetByIdAsync(99);

            // Assert
            result.Should().BeNull();

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task SearchByNameAsync_ShouldReturnMappedSearchResults()
        {
            // Arrange
            var patients = new List<Patient>
            {
                BuildPatient(1, "Rahul Sharma", true),
                BuildPatient(2, "Rahul Kumar", false)
            };

            _patientRepositoryMock
                .Setup(x => x.SearchByNameAsync("rahul"))
                .ReturnsAsync(patients);

            // Act
            var result = (await _service.SearchByNameAsync("rahul")).ToList();

            // Assert
            result.Should().HaveCount(2);

            result[0].PatientId.Should().Be(1);
            result[0].FullName.Should().Be("Rahul Sharma");
            result[0].IsActive.Should().BeTrue();

            result[1].PatientId.Should().Be(2);
            result[1].FullName.Should().Be("Rahul Kumar");
            result[1].IsActive.Should().BeFalse();

            _patientRepositoryMock.Verify(x => x.SearchByNameAsync("rahul"), Times.Once);
        }

        [Fact]
        public async Task SearchByNameAsync_WhenNoMatch_ShouldReturnEmptyList()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.SearchByNameAsync("missing"))
                .ReturnsAsync(new List<Patient>());

            // Act
            var result = (await _service.SearchByNameAsync("missing")).ToList();

            // Assert
            result.Should().BeEmpty();

            _patientRepositoryMock.Verify(x => x.SearchByNameAsync("missing"), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreatePatient_WhenRequestIsValid()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                FullName = "  Rahul Sharma  ",
                DateOfBirth = new DateOnly(1995, 5, 20),
                Gender = Gender.Male,
                PhoneNumber = " 9999999999 ",
                Email = " patient@test.com ",
                InsuranceNumber = " INS-001 "
            };

            Patient? capturedPatient = null;

            _patientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Patient>()))
                .Callback<Patient>(p =>
                {
                    capturedPatient = p;
                    p.PatientId = 100;
                })
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            capturedPatient.Should().NotBeNull();
            capturedPatient!.PatientId.Should().Be(100);
            capturedPatient.FullName.Should().Be("Rahul Sharma");
            capturedPatient.DateOfBirth.Should().Be(new DateOnly(1995, 5, 20));
            capturedPatient.Gender.Should().Be(Gender.Male);
            capturedPatient.PhoneNumber.Should().Be("9999999999");
            capturedPatient.Email.Should().Be("patient@test.com");
            capturedPatient.InsuranceNumber.Should().Be("INS-001");
            capturedPatient.IsActive.Should().BeTrue();

            result.PatientId.Should().Be(100);
            result.FullName.Should().Be("Rahul Sharma");
            result.DateOfBirth.Should().Be(new DateOnly(1995, 5, 20));
            result.Gender.Should().Be(Gender.Male);
            result.PhoneNumber.Should().Be("9999999999");
            result.Email.Should().Be("patient@test.com");
            result.InsuranceId.Should().Be("INS-001");
            result.IsActive.Should().BeTrue();

            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Once);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreatePatientWithEmptyEmail_WhenEmailIsNull()
        {
            // Arrange
            var dto = BuildValidCreateDto();
            dto.Email = null;

            Patient? capturedPatient = null;

            _patientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Patient>()))
                .Callback<Patient>(p =>
                {
                    capturedPatient = p;
                    p.PatientId = 101;
                })
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            capturedPatient.Should().NotBeNull();
            capturedPatient!.Email.Should().BeEmpty();

            result.Email.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreatePatientWithNullInsurance_WhenInsuranceNumberIsNull()
        {
            // Arrange
            var dto = BuildValidCreateDto();
            dto.InsuranceNumber = null;

            Patient? capturedPatient = null;

            _patientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Patient>()))
                .Callback<Patient>(p =>
                {
                    capturedPatient = p;
                    p.PatientId = 102;
                })
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            capturedPatient.Should().NotBeNull();
            capturedPatient!.InsuranceNumber.Should().BeNull();

            result.InsuranceId.Should().BeNull();
        }

        [Theory]
        [InlineData("", "9999999999", "Patient name is required.")]
        [InlineData("   ", "9999999999", "Patient name is required.")]
        [InlineData("Rahul Sharma", "", "Phone number is required.")]
        [InlineData("Rahul Sharma", "   ", "Phone number is required.")]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenRequiredFieldsAreInvalid(
            string fullName,
            string phoneNumber,
            string expectedMessage)
        {
            // Arrange
            var dto = BuildValidCreateDto();
            dto.FullName = fullName;
            dto.PhoneNumber = phoneNumber;

            // Act
            var act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage(expectedMessage);

            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Never);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenDateOfBirthIsFutureDate()
        {
            // Arrange
            var dto = BuildValidCreateDto();
            dto.DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

            // Act
            var act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("Date of birth cannot be in the future.");

            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenDateOfBirthIsOlderThan120Years()
        {
            // Arrange
            var dto = BuildValidCreateDto();
            dto.DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-121));

            // Act
            var act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("Invalid date of birth.");

            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdatePatient_WhenRequestIsValid()
        {
            // Arrange
            var patient = BuildPatient(10, "Old Name", true);

            var dto = new UpdatePatientDto
            {
                FullName = "  Updated Name  ",
                DateOfBirth = new DateOnly(1998, 1, 10),
                Gender = Gender.Female,
                PhoneNumber = " 8888888888 ",
                Email = " updated@test.com ",
                InsuranceNumber = " INS-UPDATED "
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x => x.UpdateAsync(patient))
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(10, dto);

            // Assert
            patient.FullName.Should().Be("Updated Name");
            patient.DateOfBirth.Should().Be(new DateOnly(1998, 1, 10));
            patient.Gender.Should().Be(Gender.Female);
            patient.PhoneNumber.Should().Be("8888888888");
            patient.Email.Should().Be("updated@test.com");
            patient.InsuranceNumber.Should().Be("INS-UPDATED");

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(10), Times.Once);
            _patientRepositoryMock.Verify(x => x.UpdateAsync(patient), Times.Once);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldSetEmptyEmail_WhenEmailIsNull()
        {
            // Arrange
            var patient = BuildPatient(10, "Old Name", true);

            var dto = BuildValidUpdateDto();
            dto.Email = null;

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x => x.UpdateAsync(patient))
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(10, dto);

            // Assert
            patient.Email.Should().BeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_ShouldSetNullInsurance_WhenInsuranceNumberIsNull()
        {
            // Arrange
            var patient = BuildPatient(10, "Old Name", true);

            var dto = BuildValidUpdateDto();
            dto.InsuranceNumber = null;

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x => x.UpdateAsync(patient))
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(10, dto);

            // Assert
            patient.InsuranceNumber.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            var dto = BuildValidUpdateDto();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Patient?)null);

            // Act
            var act = async () => await _service.UpdateAsync(99, dto);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Patient with Id 99 not found.");

            _patientRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Patient>()), Times.Never);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Theory]
        [InlineData("", "9999999999", "Patient name is required.")]
        [InlineData("   ", "9999999999", "Patient name is required.")]
        [InlineData("Rahul Sharma", "", "Phone number is required.")]
        [InlineData("Rahul Sharma", "   ", "Phone number is required.")]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenRequiredFieldsAreInvalid(
            string fullName,
            string phoneNumber,
            string expectedMessage)
        {
            // Arrange
            var dto = BuildValidUpdateDto();
            dto.FullName = fullName;
            dto.PhoneNumber = phoneNumber;

            // Act
            var act = async () => await _service.UpdateAsync(10, dto);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage(expectedMessage);

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _patientRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenDateOfBirthIsFutureDate()
        {
            // Arrange
            var dto = BuildValidUpdateDto();
            dto.DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

            // Act
            var act = async () => await _service.UpdateAsync(10, dto);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("Date of birth cannot be in the future.");

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenDateOfBirthIsOlderThan120Years()
        {
            // Arrange
            var dto = BuildValidUpdateDto();
            dto.DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-121));

            // Act
            var act = async () => await _service.UpdateAsync(10, dto);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("Invalid date of birth.");

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeactivateAsync_ShouldDeactivatePatient_WhenPatientExists()
        {
            // Arrange
            var patient = BuildPatient(10, "Rahul Sharma", true);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x => x.UpdateAsync(patient))
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeactivateAsync(10);

            // Assert
            patient.IsActive.Should().BeFalse();

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(10), Times.Once);
            _patientRepositoryMock.Verify(x => x.UpdateAsync(patient), Times.Once);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeactivateAsync_ShouldThrowKeyNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Patient?)null);

            // Act
            var act = async () => await _service.DeactivateAsync(99);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Patient with Id 99 not found.");

            _patientRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Patient>()), Times.Never);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task ActivateAsync_ShouldActivatePatient_WhenPatientExists()
        {
            // Arrange
            var patient = BuildPatient(10, "Rahul Sharma", false);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x => x.UpdateAsync(patient))
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.ActivateAsync(10);

            // Assert
            patient.IsActive.Should().BeTrue();

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(10), Times.Once);
            _patientRepositoryMock.Verify(x => x.UpdateAsync(patient), Times.Once);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ActivateAsync_ShouldThrowKeyNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Patient?)null);

            // Act
            var act = async () => await _service.ActivateAsync(99);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Patient with Id 99 not found.");

            _patientRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Patient>()), Times.Never);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        private static CreatePatientDto BuildValidCreateDto()
        {
            return new CreatePatientDto
            {
                FullName = "Rahul Sharma",
                DateOfBirth = new DateOnly(1995, 5, 20),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = "rahul@test.com",
                InsuranceNumber = "INS-001"
            };
        }

        private static UpdatePatientDto BuildValidUpdateDto()
        {
            return new UpdatePatientDto
            {
                FullName = "Updated Patient",
                DateOfBirth = new DateOnly(1996, 6, 21),
                Gender = Gender.Female,
                PhoneNumber = "8888888888",
                Email = "updated@test.com",
                InsuranceNumber = "INS-UPDATED"
            };
        }

        private static Patient BuildPatient(
            int id,
            string fullName,
            bool isActive)
        {
            return new Patient
            {
                PatientId = id,
                FullName = fullName,
                DateOfBirth = new DateOnly(1995, 5, 20),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = $"patient{id}@test.com",
                InsuranceNumber = $"INS-{id}",
                IsActive = isActive
            };
        }
    }
}

