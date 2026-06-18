using FluentAssertions;
using Moq;
using S3_HealthAxisApi.DTOs.Patient;
using S3_HealthAxisApi.Enums;
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
        public async Task GetAllAsync_ShouldReturnMappedPatients_WhenPatientsExist()
        {
            // Arrange
            var patients = new List<Patient>
            {
                CreatePatient(
                    patientId: 1,
                    fullName: "Rahul Sharma",
                    dateOfBirth: new DateOnly(1995, 5, 20),
                    gender: Gender.Male,
                    phoneNumber: "9999999999",
                    email: "rahul@test.com",
                    insuranceNumber: "INS001",
                    isActive: true),

                CreatePatient(
                    patientId: 2,
                    fullName: "Anita Roy",
                    dateOfBirth: new DateOnly(1992, 10, 10),
                    gender: Gender.Female,
                    phoneNumber: "8888888888",
                    email: "anita@test.com",
                    insuranceNumber: "INS002",
                    isActive: false)
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
            result[0].Email.Should().Be("rahul@test.com");
            result[0].InsuranceId.Should().Be("INS001");
            result[0].IsActive.Should().BeTrue();

            result[1].PatientId.Should().Be(2);
            result[1].FullName.Should().Be("Anita Roy");
            result[1].DateOfBirth.Should().Be(new DateOnly(1992, 10, 10));
            result[1].Gender.Should().Be(Gender.Female);
            result[1].PhoneNumber.Should().Be("8888888888");
            result[1].Email.Should().Be("anita@test.com");
            result[1].InsuranceId.Should().Be("INS002");
            result[1].IsActive.Should().BeFalse();

            _patientRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyCollection_WhenNoPatientsExist()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _patientRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
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
            _patientRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedPatient_WhenPatientExists()
        {
            // Arrange
            var patient = CreatePatient(
                patientId: 10,
                fullName: "Abhishek Upadhyay",
                dateOfBirth: new DateOnly(1994, 7, 1),
                gender: Gender.Male,
                phoneNumber: "7777777777",
                email: "abhishek@test.com",
                insuranceNumber: "INS100",
                isActive: true);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(patient);

            // Act
            var result = await _service.GetByIdAsync(10);

            // Assert
            result.Should().NotBeNull();
            result!.PatientId.Should().Be(10);
            result.FullName.Should().Be("Abhishek Upadhyay");
            result.DateOfBirth.Should().Be(new DateOnly(1994, 7, 1));
            result.Gender.Should().Be(Gender.Male);
            result.PhoneNumber.Should().Be("7777777777");
            result.Email.Should().Be("abhishek@test.com");
            result.InsuranceId.Should().Be("INS100");
            result.IsActive.Should().BeTrue();

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(10), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task SearchByNameAsync_ShouldReturnMappedSearchResults_WhenPatientsMatch()
        {
            // Arrange
            var patients = new List<Patient>
            {
                CreatePatient(
                    patientId: 1,
                    fullName: "Rahul Sharma",
                    dateOfBirth: new DateOnly(1995, 5, 20),
                    gender: Gender.Male,
                    phoneNumber: "9999999999",
                    email: "rahul@test.com",
                    insuranceNumber: "INS001",
                    isActive: true),

                CreatePatient(
                    patientId: 2,
                    fullName: "Rahul Verma",
                    dateOfBirth: new DateOnly(1990, 2, 10),
                    gender: Gender.Male,
                    phoneNumber: "8888888888",
                    email: "rahulv@test.com",
                    insuranceNumber: "INS002",
                    isActive: false)
            };

            _patientRepositoryMock
                .Setup(x => x.SearchByNameAsync("Rahul"))
                .ReturnsAsync(patients);

            // Act
            var result = (await _service.SearchByNameAsync("Rahul")).ToList();

            // Assert
            result.Should().HaveCount(2);

            result[0].PatientId.Should().Be(1);
            result[0].FullName.Should().Be("Rahul Sharma");
            result[0].IsActive.Should().BeTrue();

            result[1].PatientId.Should().Be(2);
            result[1].FullName.Should().Be("Rahul Verma");
            result[1].IsActive.Should().BeFalse();

            _patientRepositoryMock.Verify(x => x.SearchByNameAsync("Rahul"), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task SearchByNameAsync_ShouldReturnEmptyCollection_WhenNoPatientsMatch()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.SearchByNameAsync("Nobody"))
                .ReturnsAsync(new List<Patient>());

            // Act
            var result = await _service.SearchByNameAsync("Nobody");

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _patientRepositoryMock.Verify(x => x.SearchByNameAsync("Nobody"), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenPatientNameIsMissing()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                FullName = "   ",
                DateOfBirth = new DateOnly(1995, 5, 20),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = "patient@test.com",
                InsuranceNumber = "INS001"
            };

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Patient name is required*");

            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Never);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenDateOfBirthIsInFuture()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                FullName = "Test Patient",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = "patient@test.com",
                InsuranceNumber = "INS001"
            };

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Date of birth cannot be in the future*");

            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Never);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenDateOfBirthIsOlderThan120Years()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                FullName = "Test Patient",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-121)),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = "patient@test.com",
                InsuranceNumber = "INS001"
            };

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Invalid date of birth*");

            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Never);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenPhoneNumberIsMissing()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                FullName = "Test Patient",
                DateOfBirth = new DateOnly(1995, 5, 20),
                Gender = Gender.Male,
                PhoneNumber = "   ",
                Email = "patient@test.com",
                InsuranceNumber = "INS001"
            };

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Phone number is required*");

            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Never);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
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
                Email = "  rahul@test.com  ",
                InsuranceNumber = "  INS001  "
            };

            Patient? capturedPatient = null;

            _patientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Patient>()))
                .Callback<Patient>(patient =>
                {
                    capturedPatient = patient;
                    patient.PatientId = 101; // simulate DB-generated identity
                })
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            capturedPatient.Should().NotBeNull();
            capturedPatient!.PatientId.Should().Be(101);
            capturedPatient.FullName.Should().Be("Rahul Sharma");
            capturedPatient.DateOfBirth.Should().Be(new DateOnly(1995, 5, 20));
            capturedPatient.Gender.Should().Be(Gender.Male);
            capturedPatient.PhoneNumber.Should().Be("9999999999");
            capturedPatient.Email.Should().Be("rahul@test.com");
            capturedPatient.InsuranceNumber.Should().Be("INS001");
            capturedPatient.IsActive.Should().BeTrue();

            result.Should().NotBeNull();
            result.PatientId.Should().Be(101);
            result.FullName.Should().Be("Rahul Sharma");
            result.DateOfBirth.Should().Be(new DateOnly(1995, 5, 20));
            result.Gender.Should().Be(Gender.Male);
            result.PhoneNumber.Should().Be("9999999999");
            result.Email.Should().Be("rahul@test.com");
            result.InsuranceId.Should().Be("INS001");
            result.IsActive.Should().BeTrue();

            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Once);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldSetEmptyEmailAndNullInsurance_WhenOptionalFieldsAreMissing()
        {
            // Arrange
            var dto = new CreatePatientDto
            {
                FullName = "Patient Without Optional Fields",
                DateOfBirth = new DateOnly(1998, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "9876543210",
                Email = null,
                InsuranceNumber = null
            };

            Patient? capturedPatient = null;

            _patientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Patient>()))
                .Callback<Patient>(patient =>
                {
                    capturedPatient = patient;
                    patient.PatientId = 102;
                })
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            capturedPatient.Should().NotBeNull();
            capturedPatient!.Email.Should().Be(string.Empty);
            capturedPatient.InsuranceNumber.Should().BeNull();

            result.PatientId.Should().Be(102);
            result.Email.Should().Be(string.Empty);
            result.InsuranceId.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenPatientNameIsMissing()
        {
            // Arrange
            var dto = new UpdatePatientDto
            {
                FullName = "   ",
                DateOfBirth = new DateOnly(1995, 5, 20),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = "patient@test.com",
                InsuranceNumber = "INS001"
            };

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Patient name is required*");

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenDateOfBirthIsInFuture()
        {
            // Arrange
            var dto = new UpdatePatientDto
            {
                FullName = "Updated Patient",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = "patient@test.com",
                InsuranceNumber = "INS001"
            };

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Date of birth cannot be in the future*");

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenDateOfBirthIsOlderThan120Years()
        {
            // Arrange
            var dto = new UpdatePatientDto
            {
                FullName = "Updated Patient",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-121)),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = "patient@test.com",
                InsuranceNumber = "INS001"
            };

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Invalid date of birth*");

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenPhoneNumberIsMissing()
        {
            // Arrange
            var dto = new UpdatePatientDto
            {
                FullName = "Updated Patient",
                DateOfBirth = new DateOnly(1995, 5, 20),
                Gender = Gender.Male,
                PhoneNumber = "   ",
                Email = "patient@test.com",
                InsuranceNumber = "INS001"
            };

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Phone number is required*");

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            var dto = new UpdatePatientDto
            {
                FullName = "Updated Patient",
                DateOfBirth = new DateOnly(1995, 5, 20),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = "patient@test.com",
                InsuranceNumber = "INS001"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(50))
                .ReturnsAsync((Patient?)null);

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(50, dto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Patient with Id 50 not found*");

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(50), Times.Once);
            _patientRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Patient>()), Times.Never);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdatePatient_WhenRequestIsValid()
        {
            // Arrange
            var existingPatient = CreatePatient(
                patientId: 10,
                fullName: "Old Name",
                dateOfBirth: new DateOnly(1990, 1, 1),
                gender: Gender.Male,
                phoneNumber: "1111111111",
                email: "old@test.com",
                insuranceNumber: "OLD001",
                isActive: true);

            var dto = new UpdatePatientDto
            {
                FullName = "  New Name  ",
                DateOfBirth = new DateOnly(1992, 2, 2),
                Gender = Gender.Female,
                PhoneNumber = " 9999999999 ",
                Email = "  new@test.com  ",
                InsuranceNumber = "  NEW001  "
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(existingPatient);

            _patientRepositoryMock
                .Setup(x => x.UpdateAsync(existingPatient))
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(10, dto);

            // Assert
            existingPatient.FullName.Should().Be("New Name");
            existingPatient.DateOfBirth.Should().Be(new DateOnly(1992, 2, 2));
            existingPatient.Gender.Should().Be(Gender.Female);
            existingPatient.PhoneNumber.Should().Be("9999999999");
            existingPatient.Email.Should().Be("new@test.com");
            existingPatient.InsuranceNumber.Should().Be("NEW001");

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(10), Times.Once);
            _patientRepositoryMock.Verify(x => x.UpdateAsync(existingPatient), Times.Once);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldSetEmptyEmailAndNullInsurance_WhenOptionalFieldsAreMissing()
        {
            // Arrange
            var existingPatient = CreatePatient(
                patientId: 11,
                fullName: "Old Name",
                dateOfBirth: new DateOnly(1990, 1, 1),
                gender: Gender.Male,
                phoneNumber: "1111111111",
                email: "old@test.com",
                insuranceNumber: "OLD001",
                isActive: true);

            var dto = new UpdatePatientDto
            {
                FullName = "Updated Name",
                DateOfBirth = new DateOnly(1991, 3, 3),
                Gender = Gender.Female,
                PhoneNumber = "8888888888",
                Email = null,
                InsuranceNumber = null
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(11))
                .ReturnsAsync(existingPatient);

            _patientRepositoryMock
                .Setup(x => x.UpdateAsync(existingPatient))
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(11, dto);

            // Assert
            existingPatient.FullName.Should().Be("Updated Name");
            existingPatient.Email.Should().Be(string.Empty);
            existingPatient.InsuranceNumber.Should().BeNull();

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(11), Times.Once);
            _patientRepositoryMock.Verify(x => x.UpdateAsync(existingPatient), Times.Once);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeactivateAsync_ShouldThrowKeyNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(70))
                .ReturnsAsync((Patient?)null);

            // Act
            Func<Task> act = async () => await _service.DeactivateAsync(70);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Patient with Id 70 not found*");

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(70), Times.Once);
            _patientRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Patient>()), Times.Never);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeactivateAsync_ShouldSetPatientInactive_WhenPatientExists()
        {
            // Arrange
            var patient = CreatePatient(
                patientId: 70,
                fullName: "Active Patient",
                dateOfBirth: new DateOnly(1990, 1, 1),
                gender: Gender.Male,
                phoneNumber: "9999999999",
                email: "active@test.com",
                insuranceNumber: "INS001",
                isActive: true);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(70))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x => x.UpdateAsync(patient))
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeactivateAsync(70);

            // Assert
            patient.IsActive.Should().BeFalse();

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(70), Times.Once);
            _patientRepositoryMock.Verify(x => x.UpdateAsync(patient), Times.Once);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ActivateAsync_ShouldThrowKeyNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(80))
                .ReturnsAsync((Patient?)null);

            // Act
            Func<Task> act = async () => await _service.ActivateAsync(80);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Patient with Id 80 not found*");

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(80), Times.Once);
            _patientRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Patient>()), Times.Never);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task ActivateAsync_ShouldSetPatientActive_WhenPatientExists()
        {
            // Arrange
            var patient = CreatePatient(
                patientId: 80,
                fullName: "Inactive Patient",
                dateOfBirth: new DateOnly(1990, 1, 1),
                gender: Gender.Female,
                phoneNumber: "8888888888",
                email: "inactive@test.com",
                insuranceNumber: "INS002",
                isActive: false);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(80))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x => x.UpdateAsync(patient))
                .Returns(Task.CompletedTask);

            _patientRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.ActivateAsync(80);

            // Assert
            patient.IsActive.Should().BeTrue();

            _patientRepositoryMock.Verify(x => x.GetByIdAsync(80), Times.Once);
            _patientRepositoryMock.Verify(x => x.UpdateAsync(patient), Times.Once);
            _patientRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        private static Patient CreatePatient(
            int patientId,
            string fullName,
            DateOnly dateOfBirth,
            Gender gender,
            string phoneNumber,
            string email,
            string? insuranceNumber,
            bool isActive)
        {
            return new Patient
            {
                PatientId = patientId,
                FullName = fullName,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                PhoneNumber = phoneNumber,
                Email = email,
                InsuranceNumber = insuranceNumber,
                IsActive = isActive
            };
        }
    }
}