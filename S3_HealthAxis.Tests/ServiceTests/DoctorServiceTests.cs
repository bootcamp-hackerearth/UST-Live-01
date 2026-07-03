using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Moq;
using S3_HealthAxis.Shared.DTOs.Doctor;
using S3_HealthAxis.Shared.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Implementation;
using S3_HealthAxisApi.Services.Interface;
using Xunit;

namespace S3_HealthAxis.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _userServiceMock = new Mock<IUserService>();

            _service = new DoctorService(
                _doctorRepositoryMock.Object,
                _userServiceMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDoctors()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                BuildDoctor(1, "Dr. A", "a@test.com", DoctorSpecialisation.Cardiologist, 5, 500, true),
                BuildDoctor(2, "Dr. B", "b@test.com", DoctorSpecialisation.Neurologist, 10, 800, false)
            };

            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync("name", null))
                .ReturnsAsync(doctors);

            // Act
            var result = (await _service.GetAllAsync("name", null)).ToList();

            // Assert
            result.Should().HaveCount(2);

            result[0].DoctorId.Should().Be(1);
            result[0].FullName.Should().Be("Dr. A");
            result[0].Email.Should().Be("a@test.com");
            result[0].Specialisation.Should().Be((int)DoctorSpecialisation.Cardiologist);
            result[0].YearsOfExperience.Should().Be(5);
            result[0].ConsultationFee.Should().Be(500);
            result[0].IsActive.Should().BeTrue();

            result[1].DoctorId.Should().Be(2);
            result[1].IsActive.Should().BeFalse();

            _doctorRepositoryMock.Verify(x => x.GetAllAsync("name", null), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoDoctors_ShouldReturnEmptyList()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync(null, null))
                .ReturnsAsync(new List<Doctor>());

            // Act
            var result = (await _service.GetAllAsync(null, null)).ToList();

            // Assert
            result.Should().BeEmpty();

            _doctorRepositoryMock.Verify(x => x.GetAllAsync(null, null), Times.Once);
        }

        [Fact]
        public async Task GetActiveBySpecialisationAsync_ShouldReturnActiveDoctors_WhenSpecialisationIsValid()
        {
            // Arrange
            var specialisation = (int)DoctorSpecialisation.Dermatologist;

            var doctors = new List<Doctor>
            {
                BuildDoctor(1, "Dr. Skin", "skin@test.com", DoctorSpecialisation.Dermatologist, 7, 700, true)
            };

            _doctorRepositoryMock
                .Setup(x => x.GetActiveBySpecialisationAsync(specialisation))
                .ReturnsAsync(doctors);

            // Act
            var result = (await _service.GetActiveBySpecialisationAsync(specialisation)).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].FullName.Should().Be("Dr. Skin");
            result[0].Specialisation.Should().Be(specialisation);
            result[0].IsActive.Should().BeTrue();

            _doctorRepositoryMock.Verify(x => x.GetActiveBySpecialisationAsync(specialisation), Times.Once);
        }

        [Fact]
        public async Task GetActiveBySpecialisationAsync_ShouldThrowArgumentException_WhenSpecialisationIsInvalid()
        {
            // Arrange
            var invalidSpecialisation = 999;

            // Act
            var act = async () => await _service.GetActiveBySpecialisationAsync(invalidSpecialisation);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("Invalid doctor specialisation.");

            _doctorRepositoryMock.Verify(
                x => x.GetActiveBySpecialisationAsync(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDoctor_WhenDoctorExists()
        {
            // Arrange
            var doctor = BuildDoctor(
                10,
                "Dr. Test",
                "doctor@test.com",
                DoctorSpecialisation.Pediatrician,
                12,
                900,
                true);

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(doctor);

            // Act
            var result = await _service.GetByIdAsync(10);

            // Assert
            result.Should().NotBeNull();
            result!.DoctorId.Should().Be(10);
            result.FullName.Should().Be("Dr. Test");
            result.Email.Should().Be("doctor@test.com");
            result.Specialisation.Should().Be((int)DoctorSpecialisation.Pediatrician);
            result.YearsOfExperience.Should().Be(12);
            result.ConsultationFee.Should().Be(900);
            result.IsActive.Should().BeTrue();

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(10), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenDoctorDoesNotExist()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            // Act
            var result = await _service.GetByIdAsync(99);

            // Assert
            result.Should().BeNull();

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateDoctor_WhenRequestIsValid()
        {
            // Arrange
            var dto = BuildValidCreateDoctorDto();
            dto.FullName = "  Dr. Strange  ";
            dto.Email = "  DOCTOR@Test.COM  ";

            Doctor? capturedDoctor = null;

            _doctorRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Doctor>()))
                .Callback<Doctor>(doctor =>
                {
                    capturedDoctor = doctor;
                    doctor.DoctorId = 50;
                })
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            capturedDoctor.Should().NotBeNull();
            capturedDoctor!.FullName.Should().Be("Dr. Strange");
            capturedDoctor.Email.Should().Be("doctor@test.com");
            capturedDoctor.Specialisation.Should().Be(DoctorSpecialisation.Cardiologist);
            capturedDoctor.YearsOfExperience.Should().Be(8);
            capturedDoctor.ConsultationFee.Should().Be(750);
            capturedDoctor.IsActive.Should().BeTrue();

            result.DoctorId.Should().Be(50);
            result.FullName.Should().Be("Dr. Strange");
            result.Email.Should().Be("doctor@test.com");
            result.Specialisation.Should().Be((int)DoctorSpecialisation.Cardiologist);
            result.YearsOfExperience.Should().Be(8);
            result.ConsultationFee.Should().Be(750);
            result.IsActive.Should().BeTrue();

            _doctorRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Doctor>()), Times.Once);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData("", "doctor@test.com", 2, 5, 500, "Doctor name is required.")]
        [InlineData("   ", "doctor@test.com", 2, 5, 500, "Doctor name is required.")]
        [InlineData("Dr. Test", "", 2, 5, 500, "Email is required.")]
        [InlineData("Dr. Test", "   ", 2, 5, 500, "Email is required.")]
        [InlineData("Dr. Test", "doctor@test.com", 999, 5, 500, "Invalid doctor specialisation.")]
        [InlineData("Dr. Test", "doctor@test.com", 2, -1, 500, "Experience must be between 0 and 60 years.")]
        [InlineData("Dr. Test", "doctor@test.com", 2, 61, 500, "Experience must be between 0 and 60 years.")]
        [InlineData("Dr. Test", "doctor@test.com", 2, 5, 0, "Consultation fee must be greater than zero.")]
        [InlineData("Dr. Test", "doctor@test.com", 2, 5, -10, "Consultation fee must be greater than zero.")]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenValidationFails(
            string fullName,
            string email,
            int specialisation,
            int yearsOfExperience,
            decimal consultationFee,
            string expectedMessage)
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                FullName = fullName,
                Email = email,
                Specialisation = specialisation,
                YearsOfExperience = yearsOfExperience,
                ConsultationFee = consultationFee
            };

            // Act
            var act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage(expectedMessage);

            _doctorRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Doctor>()), Times.Never);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateDoctor_WhenRequestIsValid()
        {
            // Arrange
            var doctor = BuildDoctor(
                10,
                "Old Name",
                "old@test.com",
                DoctorSpecialisation.Cardiologist,
                5,
                500,
                true);

            var dto = new UpdateDoctorDto
            {
                FullName = "  Updated Doctor  ",
                Specialisation = (int)DoctorSpecialisation.Neurologist,
                YearsOfExperience = 15,
                ConsultationFee = 1200
            };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(x => x.UpdateAsync(doctor))
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(10, dto);

            // Assert
            doctor.FullName.Should().Be("Updated Doctor");
            doctor.Email.Should().Be("old@test.com");
            doctor.Specialisation.Should().Be(DoctorSpecialisation.Neurologist);
            doctor.YearsOfExperience.Should().Be(15);
            doctor.ConsultationFee.Should().Be(1200);

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(10), Times.Once);
            _doctorRepositoryMock.Verify(x => x.UpdateAsync(doctor), Times.Once);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            var dto = BuildValidUpdateDoctorDto();

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            // Act
            var act = async () => await _service.UpdateAsync(99, dto);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Doctor with Id 99 not found.");

            _doctorRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Doctor>()), Times.Never);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Theory]
        [InlineData("", 2, 5, 500, "Doctor name is required.")]
        [InlineData("   ", 2, 5, 500, "Doctor name is required.")]
        [InlineData("Dr. Test", 999, 5, 500, "Invalid doctor specialisation.")]
        [InlineData("Dr. Test", 2, -1, 500, "Experience must be between 0 and 60 years.")]
        [InlineData("Dr. Test", 2, 61, 500, "Experience must be between 0 and 60 years.")]
        [InlineData("Dr. Test", 2, 5, 0, "Consultation fee must be greater than zero.")]
        [InlineData("Dr. Test", 2, 5, -1, "Consultation fee must be greater than zero.")]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenValidationFails(
            string fullName,
            int specialisation,
            int yearsOfExperience,
            decimal consultationFee,
            string expectedMessage)
        {
            // Arrange
            var dto = new UpdateDoctorDto
            {
                FullName = fullName,
                Specialisation = specialisation,
                YearsOfExperience = yearsOfExperience,
                ConsultationFee = consultationFee
            };

            // Act
            var act = async () => await _service.UpdateAsync(10, dto);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage(expectedMessage);

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _doctorRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public async Task GetAvailabilityAsync_ShouldReturnAvailableSlots_WhenDoctorExists()
        {
            // Arrange
            var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
            var doctor = BuildDoctor(10, "Dr. Available", "available@test.com");

            var bookedSlots = new List<int>
            {
                (int)AppointmentTimeSlot.TenAM,
                (int)AppointmentTimeSlot.ElevenAM,
                (int)AppointmentTimeSlot.ThreeThirtyPM
            };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(x => x.GetBookedSlotsAsync(10, date))
                .ReturnsAsync(bookedSlots);

            // Act
            var result = (await _service.GetAvailabilityAsync(10, date)).ToList();

            // Assert
            result.Should().NotContain((int)AppointmentTimeSlot.TenAM);
            result.Should().NotContain((int)AppointmentTimeSlot.ElevenAM);
            result.Should().NotContain((int)AppointmentTimeSlot.ThreeThirtyPM);

            result.Should().Contain((int)AppointmentTimeSlot.TenThirtyAM);
            result.Should().Contain((int)AppointmentTimeSlot.TwelvePM);
            result.Should().Contain((int)AppointmentTimeSlot.TwoPM);

            result.Should().HaveCount(Enum.GetValues<AppointmentTimeSlot>().Length - bookedSlots.Count);

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(10), Times.Once);
            _doctorRepositoryMock.Verify(x => x.GetBookedSlotsAsync(10, date), Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_ShouldReturnAllSlots_WhenNoSlotsAreBooked()
        {
            // Arrange
            var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
            var doctor = BuildDoctor(10, "Dr. Available", "available@test.com");

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(x => x.GetBookedSlotsAsync(10, date))
                .ReturnsAsync(new List<int>());

            // Act
            var result = (await _service.GetAvailabilityAsync(10, date)).ToList();

            // Assert
            result.Should().HaveCount(Enum.GetValues<AppointmentTimeSlot>().Length);
            result.Should().Contain((int)AppointmentTimeSlot.TenAM);
            result.Should().Contain((int)AppointmentTimeSlot.ThreeThirtyPM);
        }

        [Fact]
        public async Task GetAvailabilityAsync_ShouldThrowKeyNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            // Act
            var act = async () => await _service.GetAvailabilityAsync(99, date);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Doctor not found.");

            _doctorRepositoryMock.Verify(x => x.GetBookedSlotsAsync(It.IsAny<int>(), It.IsAny<DateOnly>()), Times.Never);
        }

        [Fact]
        public async Task CreateDoctorWithAccountAsync_ShouldThrowArgumentException_WhenEmailAlreadyExists()
        {
            // Arrange
            var dto = BuildValidCreateDoctorDto();

            _userServiceMock
                .Setup(x => x.EmailExistsAsync(dto.Email))
                .ReturnsAsync(true);

            // Act
            var act = async () => await _service.CreateDoctorWithAccountAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("Email already exists.");

            _doctorRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Doctor>()), Times.Never);
            _userServiceMock.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task CreateDoctorWithAccountAsync_ShouldCreateDoctorAndUser_WhenRequestIsValid()
        {
            // Arrange
            var dto = BuildValidCreateDoctorDto();
            dto.FullName = "  Dr. New  ";
            dto.Email = "  NEWDOC@Test.COM  ";

            Doctor? capturedDoctor = null;
            User? capturedUser = null;

            _userServiceMock
                .Setup(x => x.EmailExistsAsync(dto.Email))
                .ReturnsAsync(false);

            _doctorRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Doctor>()))
                .Callback<Doctor>(doctor =>
                {
                    capturedDoctor = doctor;
                    doctor.DoctorId = 900;
                })
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            _userServiceMock
                .Setup(x => x.CreateAsync(It.IsAny<User>()))
                .Callback<User>(user =>
                {
                    capturedUser = user;
                    user.UserId = 901;
                })
                .Returns(Task.CompletedTask);

            _userServiceMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateDoctorWithAccountAsync(dto);

            // Assert
            capturedDoctor.Should().NotBeNull();
            capturedDoctor!.DoctorId.Should().Be(900);
            capturedDoctor.FullName.Should().Be("Dr. New");
            capturedDoctor.Email.Should().Be("newdoc@test.com");
            capturedDoctor.Specialisation.Should().Be(DoctorSpecialisation.Cardiologist);
            capturedDoctor.IsActive.Should().BeTrue();

            result.DoctorId.Should().Be(900);
            result.FullName.Should().Be("Dr. New");
            result.Email.Should().Be("newdoc@test.com");
            result.TemporaryPassword.Should().NotBeNullOrWhiteSpace();
            result.TemporaryPassword.Should().StartWith("Doc@");
            result.TemporaryPassword.Length.Should().BeGreaterThanOrEqualTo(10);

            capturedUser.Should().NotBeNull();
            capturedUser!.Email.Should().Be("newdoc@test.com");
            capturedUser.Role.Should().Be(UserRole.Doctor);
            capturedUser.ReferenceId.Should().Be(900);
            capturedUser.MustChangePassword.Should().BeTrue();
            capturedUser.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
            capturedUser.PasswordHash.Should().Be(ComputeSha256Base64(result.TemporaryPassword));

            _doctorRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Doctor>()), Times.Once);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            _userServiceMock.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
            _userServiceMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData("", "doctor@test.com", 2, 5, 500, "Doctor name is required.")]
        [InlineData("Dr. Test", "", 2, 5, 500, "Email is required.")]
        [InlineData("Dr. Test", "doctor@test.com", 999, 5, 500, "Invalid doctor specialisation.")]
        [InlineData("Dr. Test", "doctor@test.com", 2, -1, 500, "Experience must be between 0 and 60 years.")]
        [InlineData("Dr. Test", "doctor@test.com", 2, 61, 500, "Experience must be between 0 and 60 years.")]
        [InlineData("Dr. Test", "doctor@test.com", 2, 5, 0, "Consultation fee must be greater than zero.")]
        public async Task CreateDoctorWithAccountAsync_ShouldThrowArgumentException_WhenValidationFails(
            string fullName,
            string email,
            int specialisation,
            int yearsOfExperience,
            decimal consultationFee,
            string expectedMessage)
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                FullName = fullName,
                Email = email,
                Specialisation = specialisation,
                YearsOfExperience = yearsOfExperience,
                ConsultationFee = consultationFee
            };

            // Act
            var act = async () => await _service.CreateDoctorWithAccountAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage(expectedMessage);

            _userServiceMock.Verify(x => x.EmailExistsAsync(It.IsAny<string>()), Times.Never);
            _doctorRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public async Task ActivateAsync_ShouldActivateDoctor_WhenDoctorExists()
        {
            // Arrange
            var doctor = BuildDoctor(10, "Dr. Inactive", "inactive@test.com", isActive: false);

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(x => x.UpdateAsync(doctor))
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.ActivateAsync(10);

            // Assert
            doctor.IsActive.Should().BeTrue();

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(10), Times.Once);
            _doctorRepositoryMock.Verify(x => x.UpdateAsync(doctor), Times.Once);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ActivateAsync_ShouldThrowKeyNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            // Act
            var act = async () => await _service.ActivateAsync(99);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Doctor with Id 99 not found.");

            _doctorRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public async Task DeactivateAsync_ShouldDeactivateDoctor_WhenDoctorExists()
        {
            // Arrange
            var doctor = BuildDoctor(10, "Dr. Active", "active@test.com", isActive: true);

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(x => x.UpdateAsync(doctor))
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeactivateAsync(10);

            // Assert
            doctor.IsActive.Should().BeFalse();

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(10), Times.Once);
            _doctorRepositoryMock.Verify(x => x.UpdateAsync(doctor), Times.Once);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeactivateAsync_ShouldThrowKeyNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            // Act
            var act = async () => await _service.DeactivateAsync(99);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Doctor with Id 99 not found.");

            _doctorRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Doctor>()), Times.Never);
        }

        private static CreateDoctorDto BuildValidCreateDoctorDto()
        {
            return new CreateDoctorDto
            {
                FullName = "Dr. Test",
                Email = "doctor@test.com",
                Specialisation = (int)DoctorSpecialisation.Cardiologist,
                YearsOfExperience = 8,
                ConsultationFee = 750
            };
        }

        private static UpdateDoctorDto BuildValidUpdateDoctorDto()
        {
            return new UpdateDoctorDto
            {
                FullName = "Dr. Updated",
                Specialisation = (int)DoctorSpecialisation.Neurologist,
                YearsOfExperience = 12,
                ConsultationFee = 1000
            };
        }

        private static Doctor BuildDoctor(
            int id,
            string fullName,
            string email,
            DoctorSpecialisation specialisation = DoctorSpecialisation.GeneralPractitioner,
            int yearsOfExperience = 5,
            decimal consultationFee = 500,
            bool isActive = true)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = fullName,
                Email = email,
                Specialisation = specialisation,
                YearsOfExperience = yearsOfExperience,
                ConsultationFee = consultationFee,
                IsActive = isActive
            };
        }

        private static string ComputeSha256Base64(string password)
        {
            using var sha256 = SHA256.Create();

            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}
