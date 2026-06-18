using FluentAssertions;
using Moq;
using S3_HealthAxisApi.DTOs.Doctor;
using S3_HealthAxisApi.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Implementation;
using Xunit;

namespace S3_HealthAxis.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _service = new DoctorService(_doctorRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDoctors_WhenDoctorsExist()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                CreateDoctor(
                    doctorId: 1,
                    fullName: "Dr. A",
                    specialisation: DoctorSpecialisation.Cardiologist,
                    yearsOfExperience: 10,
                    consultationFee: 500,
                    isActive: true),

                CreateDoctor(
                    doctorId: 2,
                    fullName: "Dr. B",
                    specialisation: DoctorSpecialisation.Neurologist,
                    yearsOfExperience: 12,
                    consultationFee: 800,
                    isActive: false)
            };

            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync("name", (int)DoctorSpecialisation.Cardiologist))
                .ReturnsAsync(doctors);

            // Act
            var result = (await _service.GetAllAsync("name", (int)DoctorSpecialisation.Cardiologist)).ToList();

            // Assert
            result.Should().HaveCount(2);

            result[0].DoctorId.Should().Be(1);
            result[0].FullName.Should().Be("Dr. A");
            result[0].Specialisation.Should().Be((int)DoctorSpecialisation.Cardiologist);
            result[0].YearsOfExperience.Should().Be(10);
            result[0].ConsultationFee.Should().Be(500);
            result[0].IsActive.Should().BeTrue();

            result[1].DoctorId.Should().Be(2);
            result[1].FullName.Should().Be("Dr. B");
            result[1].Specialisation.Should().Be((int)DoctorSpecialisation.Neurologist);
            result[1].YearsOfExperience.Should().Be(12);
            result[1].ConsultationFee.Should().Be(800);
            result[1].IsActive.Should().BeFalse();

            _doctorRepositoryMock.Verify(
                x => x.GetAllAsync("name", (int)DoctorSpecialisation.Cardiologist),
                Times.Once);

            _doctorRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyCollection_WhenNoDoctorsExist()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync(null, null))
                .ReturnsAsync(new List<Doctor>());

            // Act
            var result = await _service.GetAllAsync(null, null);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _doctorRepositoryMock.Verify(x => x.GetAllAsync(null, null), Times.Once);
            _doctorRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetActiveBySpecialisationAsync_ShouldThrowArgumentException_WhenSpecialisationIsInvalid()
        {
            // Arrange
            var invalidSpecialisation = 999;

            // Act
            Func<Task> act = async () => await _service.GetActiveBySpecialisationAsync(invalidSpecialisation);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Invalid doctor specialisation*");

            _doctorRepositoryMock.Verify(
                x => x.GetActiveBySpecialisationAsync(It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task GetActiveBySpecialisationAsync_ShouldReturnMappedDoctors_WhenSpecialisationIsValid()
        {
            // Arrange
            var specialisation = (int)DoctorSpecialisation.Cardiologist;

            var doctors = new List<Doctor>
            {
                CreateDoctor(
                    doctorId: 1,
                    fullName: "Dr. A",
                    specialisation: DoctorSpecialisation.Cardiologist,
                    yearsOfExperience: 8,
                    consultationFee: 700,
                    isActive: true),

                CreateDoctor(
                    doctorId: 2,
                    fullName: "Dr. B",
                    specialisation: DoctorSpecialisation.Cardiologist,
                    yearsOfExperience: 15,
                    consultationFee: 1000,
                    isActive: true)
            };

            _doctorRepositoryMock
                .Setup(x => x.GetActiveBySpecialisationAsync(specialisation))
                .ReturnsAsync(doctors);

            // Act
            var result = (await _service.GetActiveBySpecialisationAsync(specialisation)).ToList();

            // Assert
            result.Should().HaveCount(2);
            result.All(x => x.Specialisation == specialisation).Should().BeTrue();
            result.All(x => x.IsActive).Should().BeTrue();

            _doctorRepositoryMock.Verify(x => x.GetActiveBySpecialisationAsync(specialisation), Times.Once);
            _doctorRepositoryMock.VerifyNoOtherCalls();
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
            _doctorRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDoctor_WhenDoctorExists()
        {
            // Arrange
            var doctor = CreateDoctor(
                doctorId: 10,
                fullName: "Dr. Meera",
                specialisation: DoctorSpecialisation.Dermatologist,
                yearsOfExperience: 6,
                consultationFee: 900,
                isActive: true);

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(doctor);

            // Act
            var result = await _service.GetByIdAsync(10);

            // Assert
            result.Should().NotBeNull();
            result!.DoctorId.Should().Be(10);
            result.FullName.Should().Be("Dr. Meera");
            result.Specialisation.Should().Be((int)DoctorSpecialisation.Dermatologist);
            result.YearsOfExperience.Should().Be(6);
            result.ConsultationFee.Should().Be(900);
            result.IsActive.Should().BeTrue();

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(10), Times.Once);
            _doctorRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenDoctorNameIsMissing()
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                FullName = "   ",
                Specialisation = (int)DoctorSpecialisation.Cardiologist,
                YearsOfExperience = 10,
                ConsultationFee = 600
            };

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Doctor name is required*");

            _doctorRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Doctor>()), Times.Never);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenSpecialisationIsInvalid()
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                FullName = "Dr. Test",
                Specialisation = 999,
                YearsOfExperience = 10,
                ConsultationFee = 600
            };

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Invalid doctor specialisation*");

            _doctorRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Doctor>()), Times.Never);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenExperienceIsLessThanZero()
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                FullName = "Dr. Test",
                Specialisation = (int)DoctorSpecialisation.Cardiologist,
                YearsOfExperience = -1,
                ConsultationFee = 600
            };

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Experience must be between 0 and 60 years*");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenExperienceIsGreaterThanSixty()
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                FullName = "Dr. Test",
                Specialisation = (int)DoctorSpecialisation.Cardiologist,
                YearsOfExperience = 61,
                ConsultationFee = 600
            };

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Experience must be between 0 and 60 years*");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenConsultationFeeIsZeroOrLess()
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                FullName = "Dr. Test",
                Specialisation = (int)DoctorSpecialisation.Cardiologist,
                YearsOfExperience = 10,
                ConsultationFee = 0
            };

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Consultation fee must be greater than zero*");
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateDoctor_WhenRequestIsValid()
        {
            // Arrange
            var dto = new CreateDoctorDto
            {
                FullName = "  Dr. New Doctor  ",
                Specialisation = (int)DoctorSpecialisation.Pediatrician,
                YearsOfExperience = 9,
                ConsultationFee = 1200
            };

            Doctor? capturedDoctor = null;

            _doctorRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Doctor>()))
                .Callback<Doctor>(doctor =>
                {
                    capturedDoctor = doctor;
                    doctor.DoctorId = 101; // simulate DB-generated id
                })
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            capturedDoctor.Should().NotBeNull();
            capturedDoctor!.FullName.Should().Be("Dr. New Doctor");
            capturedDoctor.Specialisation.Should().Be(DoctorSpecialisation.Pediatrician);
            capturedDoctor.YearsOfExperience.Should().Be(9);
            capturedDoctor.ConsultationFee.Should().Be(1200);
            capturedDoctor.IsActive.Should().BeTrue();

            result.Should().NotBeNull();
            result.DoctorId.Should().Be(101);
            result.FullName.Should().Be("Dr. New Doctor");
            result.Specialisation.Should().Be((int)DoctorSpecialisation.Pediatrician);
            result.YearsOfExperience.Should().Be(9);
            result.ConsultationFee.Should().Be(1200);
            result.IsActive.Should().BeTrue();

            _doctorRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Doctor>()), Times.Once);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenDoctorNameIsMissing()
        {
            // Arrange
            var dto = new UpdateDoctorDto
            {
                FullName = "   ",
                Specialisation = (int)DoctorSpecialisation.Cardiologist,
                YearsOfExperience = 10,
                ConsultationFee = 700
            };

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Doctor name is required*");

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenSpecialisationIsInvalid()
        {
            // Arrange
            var dto = new UpdateDoctorDto
            {
                FullName = "Dr. Test",
                Specialisation = 999,
                YearsOfExperience = 10,
                ConsultationFee = 700
            };

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Invalid doctor specialisation*");

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenExperienceIsInvalid()
        {
            // Arrange
            var dto = new UpdateDoctorDto
            {
                FullName = "Dr. Test",
                Specialisation = (int)DoctorSpecialisation.Cardiologist,
                YearsOfExperience = -10,
                ConsultationFee = 700
            };

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Experience must be between 0 and 60 years*");

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenConsultationFeeIsInvalid()
        {
            // Arrange
            var dto = new UpdateDoctorDto
            {
                FullName = "Dr. Test",
                Specialisation = (int)DoctorSpecialisation.Cardiologist,
                YearsOfExperience = 10,
                ConsultationFee = -1
            };

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(1, dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Consultation fee must be greater than zero*");

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            var dto = new UpdateDoctorDto
            {
                FullName = "Dr. Updated",
                Specialisation = (int)DoctorSpecialisation.Cardiologist,
                YearsOfExperience = 15,
                ConsultationFee = 1500
            };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(50))
                .ReturnsAsync((Doctor?)null);

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(50, dto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Doctor with Id 50 not found*");

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(50), Times.Once);
            _doctorRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Doctor>()), Times.Never);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateDoctor_WhenRequestIsValid()
        {
            // Arrange
            var existingDoctor = CreateDoctor(
                doctorId: 10,
                fullName: "Dr. Old",
                specialisation: DoctorSpecialisation.Cardiologist,
                yearsOfExperience: 5,
                consultationFee: 500,
                isActive: true);

            var dto = new UpdateDoctorDto
            {
                FullName = "  Dr. Updated Name  ",
                Specialisation = (int)DoctorSpecialisation.Neurologist,
                YearsOfExperience = 18,
                ConsultationFee = 2200
            };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync(existingDoctor);

            _doctorRepositoryMock
                .Setup(x => x.UpdateAsync(existingDoctor))
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(10, dto);

            // Assert
            existingDoctor.FullName.Should().Be("Dr. Updated Name");
            existingDoctor.Specialisation.Should().Be(DoctorSpecialisation.Neurologist);
            existingDoctor.YearsOfExperience.Should().Be(18);
            existingDoctor.ConsultationFee.Should().Be(2200);

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(10), Times.Once);
            _doctorRepositoryMock.Verify(x => x.UpdateAsync(existingDoctor), Times.Once);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_ShouldThrowKeyNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            var doctorId = 100;
            var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(doctorId))
                .ReturnsAsync((Doctor?)null);

            // Act
            Func<Task> act = async () => await _service.GetAvailabilityAsync(doctorId, date);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Doctor not found*");

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(doctorId), Times.Once);
            _doctorRepositoryMock.Verify(x => x.GetBookedSlotsAsync(It.IsAny<int>(), It.IsAny<DateOnly>()), Times.Never);
        }

        [Fact]
        public async Task GetAvailabilityAsync_ShouldReturnAllAvailableSlots_WhenNoSlotsAreBooked()
        {
            // Arrange
            var doctorId = 101;
            var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(doctorId))
                .ReturnsAsync(CreateDoctor(
                    doctorId: doctorId,
                    fullName: "Dr. Available",
                    specialisation: DoctorSpecialisation.Cardiologist,
                    yearsOfExperience: 10,
                    consultationFee: 700,
                    isActive: true));

            _doctorRepositoryMock
                .Setup(x => x.GetBookedSlotsAsync(doctorId, date))
                .ReturnsAsync(new List<int>());

            // Act
            var result = (await _service.GetAvailabilityAsync(doctorId, date)).ToList();

            // Assert
            var allExpectedSlots = Enum.GetValues<AppointmentTimeSlot>().Select(x => (int)x).ToList();

            result.Should().BeEquivalentTo(allExpectedSlots);
            result.Should().HaveCount(allExpectedSlots.Count);

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(doctorId), Times.Once);
            _doctorRepositoryMock.Verify(x => x.GetBookedSlotsAsync(doctorId, date), Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_ShouldReturnOnlyUnbookedSlots_WhenSomeSlotsAreBooked()
        {
            // Arrange
            var doctorId = 102;
            var date = DateOnly.FromDateTime(DateTime.Today.AddDays(2));

            var bookedSlots = new List<int>
            {
                (int)AppointmentTimeSlot.TenAM,
                (int)AppointmentTimeSlot.ElevenAM,
                (int)AppointmentTimeSlot.ThreePM
            };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(doctorId))
                .ReturnsAsync(CreateDoctor(
                    doctorId: doctorId,
                    fullName: "Dr. Busy",
                    specialisation: DoctorSpecialisation.Neurologist,
                    yearsOfExperience: 12,
                    consultationFee: 1500,
                    isActive: true));

            _doctorRepositoryMock
                .Setup(x => x.GetBookedSlotsAsync(doctorId, date))
                .ReturnsAsync(bookedSlots);

            // Act
            var result = (await _service.GetAvailabilityAsync(doctorId, date)).ToList();

            // Assert
            result.Should().NotContain((int)AppointmentTimeSlot.TenAM);
            result.Should().NotContain((int)AppointmentTimeSlot.ElevenAM);
            result.Should().NotContain((int)AppointmentTimeSlot.ThreePM);

            result.Should().Contain((int)AppointmentTimeSlot.TenThirtyAM);
            result.Should().Contain((int)AppointmentTimeSlot.TwelvePM);
            result.Should().Contain((int)AppointmentTimeSlot.TwoPM);

            var allSlots = Enum.GetValues<AppointmentTimeSlot>().Select(x => (int)x).ToList();
            var expectedAvailable = allSlots.Except(bookedSlots).ToList();

            result.Should().BeEquivalentTo(expectedAvailable);

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(doctorId), Times.Once);
            _doctorRepositoryMock.Verify(x => x.GetBookedSlotsAsync(doctorId, date), Times.Once);
        }

        [Fact]
        public async Task ActivateAsync_ShouldThrowKeyNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(77))
                .ReturnsAsync((Doctor?)null);

            // Act
            Func<Task> act = async () => await _service.ActivateAsync(77);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Doctor with Id 77 not found*");

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(77), Times.Once);
            _doctorRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Doctor>()), Times.Never);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task ActivateAsync_ShouldSetDoctorActive_WhenDoctorExists()
        {
            // Arrange
            var doctor = CreateDoctor(
                doctorId: 77,
                fullName: "Dr. Inactive",
                specialisation: DoctorSpecialisation.Cardiologist,
                yearsOfExperience: 10,
                consultationFee: 1000,
                isActive: false);

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(77))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(x => x.UpdateAsync(doctor))
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.ActivateAsync(77);

            // Assert
            doctor.IsActive.Should().BeTrue();

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(77), Times.Once);
            _doctorRepositoryMock.Verify(x => x.UpdateAsync(doctor), Times.Once);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeactivateAsync_ShouldThrowKeyNotFoundException_WhenDoctorDoesNotExist()
        {
            // Arrange
            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(88))
                .ReturnsAsync((Doctor?)null);

            // Act
            Func<Task> act = async () => await _service.DeactivateAsync(88);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*Doctor with Id 88 not found*");

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(88), Times.Once);
            _doctorRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Doctor>()), Times.Never);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeactivateAsync_ShouldSetDoctorInactive_WhenDoctorExists()
        {
            // Arrange
            var doctor = CreateDoctor(
                doctorId: 88,
                fullName: "Dr. Active",
                specialisation: DoctorSpecialisation.Cardiologist,
                yearsOfExperience: 10,
                consultationFee: 1000,
                isActive: true);

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(88))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(x => x.UpdateAsync(doctor))
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeactivateAsync(88);

            // Assert
            doctor.IsActive.Should().BeFalse();

            _doctorRepositoryMock.Verify(x => x.GetByIdAsync(88), Times.Once);
            _doctorRepositoryMock.Verify(x => x.UpdateAsync(doctor), Times.Once);
            _doctorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        private static Doctor CreateDoctor(
            int doctorId,
            string fullName,
            DoctorSpecialisation specialisation,
            int yearsOfExperience,
            decimal consultationFee,
            bool isActive)
        {
            return new Doctor
            {
                DoctorId = doctorId,
                FullName = fullName,
                Specialisation = specialisation,
                YearsOfExperience = yearsOfExperience,
                ConsultationFee = consultationFee,
                IsActive = isActive
            };
        }
    }
}