using System;
using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using SharedClasses.Dtos;
using SharedClasses.Enums;
using HealthcareApi.Exceptions;
using HealthcareApi.Models;
using HealthcareApi.Repositories;
using HealthcareApi.Services.Implementations;

namespace HealthcareMvcApiTests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _doctorRepoMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DoctorService _sut;

        public DoctorServiceTests()
        {
            _doctorRepoMock = new Mock<IDoctorRepository>();
            _appointmentRepoMock = new Mock<IAppointmentRepository>();
            _mapperMock = new Mock<IMapper>();

            _sut = new DoctorService(_doctorRepoMock.Object, _appointmentRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void GetDoctorById_ValidId_ReturnsDoctorDto()
        {
            // Arrange
            int doctorId = 1;

            var doctorEntity = new Doctor
            {
                DoctorId = doctorId,
                FullName = "Doctor Smith",
                IsActive = true
            };

            var doctorDto = new DoctorDto
            {
                DoctorId = doctorId,
                FullName = "Doctor Smith",
                IsActive = true
            };

            _doctorRepoMock.Setup(repo => repo.GetById(doctorId)).Returns(doctorEntity);
            _mapperMock.Setup(m => m.Map<DoctorDto>(doctorEntity)).Returns(doctorDto);

            // Act
            DoctorDto result = _sut.GetDoctorById(doctorId);

            // Assert
            result.Should().NotBeNull();
            result.DoctorId.Should().Be(doctorId);
            result.FullName.Should().Be("Doctor Smith");
            result.IsActive.Should().BeTrue();

            _doctorRepoMock.Verify(repo => repo.GetById(doctorId), Times.Once);
        }

        [Fact]
        public void GetDoctorById_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            int doctorId = 99;

            _doctorRepoMock.Setup(repo => repo.GetById(doctorId)).Returns((Doctor)null);

            // Act
            Action act = () => _sut.GetDoctorById(doctorId);

            // Assert
            act.Should().Throw<EntityNotFoundException>();

            _doctorRepoMock.Verify(repo => repo.GetById(doctorId), Times.Once);
        }

        [Fact]
        public void GetDoctorById_ZeroId_ThrowsBusinessRuleException()
        {
            // Arrange
            int doctorId = 0;

            // Act
            Action act = () => _sut.GetDoctorById(doctorId);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Valid Doctor ID is required.");

            _doctorRepoMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void GetDoctorById_NegativeId_ThrowsBusinessRuleException()
        {
            // Arrange
            int doctorId = -1;

            // Act
            Action act = () => _sut.GetDoctorById(doctorId);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Valid Doctor ID is required.");

            _doctorRepoMock.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void AddDoctor_ValidDetails_ReturnsSavedDoctorDto()
        {
            // Arrange
            var createDto = new CreateDoctorDto
            {
                FullName = "Doctor Adams",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = 150,
                PracticeStartDate = DateTime.Today.AddYears(-5)
            };

            var mappedDoctor = new Doctor
            {
                FullName = "Doctor Adams",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = 150,
                PracticeStartDate = DateTime.Today.AddYears(-5)
            };

            var savedDoctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Doctor Adams",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = 150,
                PracticeStartDate = DateTime.Today.AddYears(-5),
                IsActive = true
            };

            var resultDto = new DoctorDto
            {
                DoctorId = 1,
                FullName = "Doctor Adams",
                Specialisation = Specialisation.Cardiology,
                IsActive = true
            };

            _mapperMock.Setup(m => m.Map<Doctor>(createDto)).Returns(mappedDoctor);
            _doctorRepoMock.Setup(repo => repo.Add(It.IsAny<Doctor>())).Returns(savedDoctor);
            _mapperMock.Setup(m => m.Map<DoctorDto>(savedDoctor)).Returns(resultDto);

            // Act
            DoctorDto result = _sut.AddDoctor(createDto);

            // Assert
            result.Should().NotBeNull();
            result.DoctorId.Should().Be(1);
            result.FullName.Should().Be("Doctor Adams");
            result.IsActive.Should().BeTrue();

            _doctorRepoMock.Verify(
                repo => repo.Add(It.Is<Doctor>(d =>
                    d.FullName == "Doctor Adams" &&
                    d.IsActive == true &&
                    d.ConsultationFee == 150)),
                Times.Once);
        }

        [Fact]
        public void AddDoctor_NullDto_ThrowsBusinessRuleException()
        {
            // Act
            Action act = () => _sut.AddDoctor(null);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Doctor details are required.");

            _doctorRepoMock.Verify(repo => repo.Add(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public void AddDoctor_TrimsFullNameBeforeSaving()
        {
            // Arrange
            var createDto = new CreateDoctorDto
            {
                FullName = "  Doctor Adams  ",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = 150,
                PracticeStartDate = DateTime.Today.AddYears(-5)
            };

            var mappedDoctor = new Doctor
            {
                FullName = "Doctor Adams",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = 150,
                PracticeStartDate = DateTime.Today.AddYears(-5)
            };

            var savedDoctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Doctor Adams",
                IsActive = true
            };

            var resultDto = new DoctorDto
            {
                DoctorId = 1,
                FullName = "Doctor Adams",
                IsActive = true
            };

            _mapperMock.Setup(m => m.Map<Doctor>(createDto)).Returns(mappedDoctor);
            _doctorRepoMock.Setup(repo => repo.Add(It.IsAny<Doctor>())).Returns(savedDoctor);
            _mapperMock.Setup(m => m.Map<DoctorDto>(savedDoctor)).Returns(resultDto);

            // Act
            DoctorDto result = _sut.AddDoctor(createDto);

            // Assert
            result.Should().NotBeNull();
            createDto.FullName.Should().Be("Doctor Adams");

            _doctorRepoMock.Verify(repo => repo.Add(It.IsAny<Doctor>()), Times.Once);
        }

        [Fact]
        public void AddDoctor_MissingFullName_ThrowsBusinessRuleException()
        {
            // Arrange
            var createDto = new CreateDoctorDto
            {
                FullName = "",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = 150,
                PracticeStartDate = DateTime.Today.AddYears(-5)
            };

            var mappedDoctor = new Doctor
            {
                FullName = "",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = 150,
                PracticeStartDate = DateTime.Today.AddYears(-5)
            };

            _mapperMock.Setup(m => m.Map<Doctor>(createDto)).Returns(mappedDoctor);

            // Act
            Action act = () => _sut.AddDoctor(createDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Doctor full name is required.");

            _doctorRepoMock.Verify(repo => repo.Add(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public void AddDoctor_InvalidFullName_ThrowsBusinessRuleException()
        {
            // Arrange
            var createDto = new CreateDoctorDto
            {
                FullName = "Doctor123",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = 150,
                PracticeStartDate = DateTime.Today.AddYears(-5)
            };

            var mappedDoctor = new Doctor
            {
                FullName = "Doctor123",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = 150,
                PracticeStartDate = DateTime.Today.AddYears(-5)
            };

            _mapperMock.Setup(m => m.Map<Doctor>(createDto)).Returns(mappedDoctor);

            // Act
            Action act = () => _sut.AddDoctor(createDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Full name can contain only letters and spaces.");

            _doctorRepoMock.Verify(repo => repo.Add(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public void AddDoctor_FuturePracticeStartDate_ThrowsBusinessRuleException()
        {
            // Arrange
            var createDto = new CreateDoctorDto
            {
                FullName = "Doctor Adams",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = 150,
                PracticeStartDate = DateTime.Today.AddDays(1)
            };

            var mappedDoctor = new Doctor
            {
                FullName = "Doctor Adams",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = 150,
                PracticeStartDate = DateTime.Today.AddDays(1)
            };

            _mapperMock.Setup(m => m.Map<Doctor>(createDto)).Returns(mappedDoctor);

            // Act
            Action act = () => _sut.AddDoctor(createDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Practice start date cannot be in the future.");

            _doctorRepoMock.Verify(repo => repo.Add(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public void AddDoctor_NegativeFee_ThrowsBusinessRuleException()
        {
            // Arrange
            var createDto = new CreateDoctorDto
            {
                FullName = "Doctor Adams",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = -50,
                PracticeStartDate = DateTime.Today.AddYears(-5)
            };

            var mappedDoctor = new Doctor
            {
                FullName = "Doctor Adams",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = -50,
                PracticeStartDate = DateTime.Today.AddYears(-5)
            };

            _mapperMock.Setup(m => m.Map<Doctor>(createDto)).Returns(mappedDoctor);

            // Act
            Action act = () => _sut.AddDoctor(createDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Consultation fee cannot be negative.");

            _doctorRepoMock.Verify(repo => repo.Add(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public void AddDoctor_FeeAboveLimit_ThrowsBusinessRuleException()
        {
            // Arrange
            var createDto = new CreateDoctorDto
            {
                FullName = "Doctor Adams",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = 100001,
                PracticeStartDate = DateTime.Today.AddYears(-5)
            };

            var mappedDoctor = new Doctor
            {
                FullName = "Doctor Adams",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = 100001,
                PracticeStartDate = DateTime.Today.AddYears(-5)
            };

            _mapperMock.Setup(m => m.Map<Doctor>(createDto)).Returns(mappedDoctor);

            // Act
            Action act = () => _sut.AddDoctor(createDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Consultation fee cannot exceed 100000.");

            _doctorRepoMock.Verify(repo => repo.Add(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public void UpdateDoctor_ValidDetails_ReturnsUpdatedDoctorDto()
        {
            // Arrange
            int doctorId = 1;

            var updateDto = new UpdateDoctorDto
            {
                FullName = "Doctor Updated",
                Specialisation = Specialisation.Neurology,
                ConsultationFee = 250,
                PracticeStartDate = DateTime.Today.AddYears(-10),
                IsActive = true
            };

            var existingDoctor = new Doctor
            {
                DoctorId = doctorId,
                FullName = "Doctor Adams",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = 150,
                PracticeStartDate = DateTime.Today.AddYears(-5),
                IsActive = true
            };

            var updatedDoctor = new Doctor
            {
                DoctorId = doctorId,
                FullName = "Doctor Updated",
                Specialisation = Specialisation.Neurology,
                ConsultationFee = 250,
                PracticeStartDate = DateTime.Today.AddYears(-10),
                IsActive = true
            };

            var resultDto = new DoctorDto
            {
                DoctorId = doctorId,
                FullName = "Doctor Updated",
                Specialisation = Specialisation.Neurology,
                IsActive = true
            };

            _doctorRepoMock.Setup(repo => repo.GetById(doctorId)).Returns(existingDoctor);
            _doctorRepoMock.Setup(repo => repo.Update(doctorId, It.IsAny<Doctor>())).Returns(updatedDoctor);
            _mapperMock.Setup(m => m.Map<DoctorDto>(updatedDoctor)).Returns(resultDto);

            // Act
            DoctorDto result = _sut.UpdateDoctor(doctorId, updateDto);

            // Assert
            result.Should().NotBeNull();
            result.FullName.Should().Be("Doctor Updated");
            result.Specialisation.Should().Be(Specialisation.Neurology);

            _doctorRepoMock.Verify(
                repo => repo.Update(
                    doctorId,
                    It.Is<Doctor>(d =>
                        d.FullName == "Doctor Updated" &&
                        d.Specialisation == Specialisation.Neurology &&
                        d.ConsultationFee == 250 &&
                        d.IsActive == true)),
                Times.Once);
        }

        [Fact]
        public void UpdateDoctor_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            int doctorId = 99;

            var updateDto = new UpdateDoctorDto
            {
                FullName = "Doctor Updated",
                Specialisation = Specialisation.Neurology,
                ConsultationFee = 250,
                PracticeStartDate = DateTime.Today.AddYears(-10),
                IsActive = true
            };

            _doctorRepoMock.Setup(repo => repo.GetById(doctorId)).Returns((Doctor)null);

            // Act
            Action act = () => _sut.UpdateDoctor(doctorId, updateDto);

            // Assert
            act.Should().Throw<EntityNotFoundException>();

            _doctorRepoMock.Verify(repo => repo.Update(It.IsAny<int>(), It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public void UpdateDoctor_InvalidFullName_ThrowsBusinessRuleException()
        {
            // Arrange
            int doctorId = 1;

            var updateDto = new UpdateDoctorDto
            {
                FullName = "Doctor@Updated",
                Specialisation = Specialisation.Neurology,
                ConsultationFee = 250,
                PracticeStartDate = DateTime.Today.AddYears(-10),
                IsActive = true
            };

            var existingDoctor = new Doctor
            {
                DoctorId = doctorId,
                FullName = "Doctor Adams",
                Specialisation = Specialisation.Cardiology,
                ConsultationFee = 150,
                PracticeStartDate = DateTime.Today.AddYears(-5),
                IsActive = true
            };

            _doctorRepoMock.Setup(repo => repo.GetById(doctorId)).Returns(existingDoctor);

            // Act
            Action act = () => _sut.UpdateDoctor(doctorId, updateDto);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Full name can contain only letters and spaces.");

            _doctorRepoMock.Verify(repo => repo.Update(It.IsAny<int>(), It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public void DeactivateDoctor_ActiveDoctor_ReturnsInactiveDoctorDto()
        {
            // Arrange
            int doctorId = 1;

            var doctor = new Doctor
            {
                DoctorId = doctorId,
                FullName = "Doctor Adams",
                IsActive = true
            };

            var updatedDoctor = new Doctor
            {
                DoctorId = doctorId,
                FullName = "Doctor Adams",
                IsActive = false
            };

            var resultDto = new DoctorDto
            {
                DoctorId = doctorId,
                FullName = "Doctor Adams",
                IsActive = false
            };

            _doctorRepoMock.Setup(repo => repo.GetById(doctorId)).Returns(doctor);
            _doctorRepoMock.Setup(repo => repo.Update(doctorId, It.IsAny<Doctor>())).Returns(updatedDoctor);
            _mapperMock.Setup(m => m.Map<DoctorDto>(updatedDoctor)).Returns(resultDto);

            // Act
            DoctorDto result = _sut.DeactivateDoctor(doctorId);

            // Assert
            result.Should().NotBeNull();
            result.IsActive.Should().BeFalse();

            _doctorRepoMock.Verify(
                repo => repo.Update(
                    doctorId,
                    It.Is<Doctor>(d => d.IsActive == false)),
                Times.Once);
        }

        [Fact]
        public void DeactivateDoctor_AlreadyInactive_ThrowsBusinessRuleException()
        {
            // Arrange
            int doctorId = 1;

            var doctor = new Doctor
            {
                DoctorId = doctorId,
                FullName = "Doctor Adams",
                IsActive = false
            };

            _doctorRepoMock.Setup(repo => repo.GetById(doctorId)).Returns(doctor);

            // Act
            Action act = () => _sut.DeactivateDoctor(doctorId);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Doctor is already inactive.");

            _doctorRepoMock.Verify(repo => repo.Update(It.IsAny<int>(), It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public void ReactivateDoctor_InactiveDoctor_ReturnsActiveDoctorDto()
        {
            // Arrange
            int doctorId = 1;

            var doctor = new Doctor
            {
                DoctorId = doctorId,
                FullName = "Doctor Adams",
                IsActive = false
            };

            var updatedDoctor = new Doctor
            {
                DoctorId = doctorId,
                FullName = "Doctor Adams",
                IsActive = true
            };

            var resultDto = new DoctorDto
            {
                DoctorId = doctorId,
                FullName = "Doctor Adams",
                IsActive = true
            };

            _doctorRepoMock.Setup(repo => repo.GetById(doctorId)).Returns(doctor);
            _doctorRepoMock.Setup(repo => repo.Update(doctorId, It.IsAny<Doctor>())).Returns(updatedDoctor);
            _mapperMock.Setup(m => m.Map<DoctorDto>(updatedDoctor)).Returns(resultDto);

            // Act
            DoctorDto result = _sut.ReactivateDoctor(doctorId);

            // Assert
            result.Should().NotBeNull();
            result.IsActive.Should().BeTrue();

            _doctorRepoMock.Verify(
                repo => repo.Update(
                    doctorId,
                    It.Is<Doctor>(d => d.IsActive == true)),
                Times.Once);
        }

        [Fact]
        public void ReactivateDoctor_AlreadyActive_ThrowsBusinessRuleException()
        {
            // Arrange
            int doctorId = 1;

            var doctor = new Doctor
            {
                DoctorId = doctorId,
                FullName = "Doctor Adams",
                IsActive = true
            };

            _doctorRepoMock.Setup(repo => repo.GetById(doctorId)).Returns(doctor);

            // Act
            Action act = () => _sut.ReactivateDoctor(doctorId);

            // Assert
            act.Should().Throw<BusinessRuleException>()
                .WithMessage("Doctor is already active.");

            _doctorRepoMock.Verify(repo => repo.Update(It.IsAny<int>(), It.IsAny<Doctor>()), Times.Never);
        }
    }
}
