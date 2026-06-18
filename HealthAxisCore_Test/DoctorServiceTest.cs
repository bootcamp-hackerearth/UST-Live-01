using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Implementation;
using Moq;
using Xunit;

namespace HealthAxisCore_Api.Tests.Services
{
    public class DoctorServiceTests
    {
        private static DoctorService CreateService(
            Mock<IDoctorRepository>? repositoryMock = null,
            Mock<IMapper>? mapperMock = null)
        {
            return new DoctorService(
                repositoryMock?.Object ?? new Mock<IDoctorRepository>().Object,
                mapperMock?.Object ?? new Mock<IMapper>().Object);
        }

        private static Doctor CreateDoctor(
            int doctorId = 1,
            string doctorName = "Doctor One",
            string specialisation = "Cardiologist",
            int yearsOfExperience = 10,
            int consultationFee = 500,
            bool isActive = true)
        {
            return new Doctor
            {
                DoctorId = doctorId,
                DoctorName = doctorName,
                Specialisation = specialisation,
                YearsOfExperience = yearsOfExperience,
                ConsultationFee = consultationFee,
                IsActive = isActive
            };
        }

        private static DoctorDto CreateDoctorDto(
            int doctorId = 1,
            string doctorName = "Doctor One",
            string specialisation = "Cardiologist",
            int yearsOfExperience = 10,
            int consultationFee = 500,
            bool isActive = true)
        {
            return new DoctorDto
            {
                DoctorId = doctorId,
                DoctorName = doctorName,
                Specialisation = specialisation,
                YearsOfExperience = yearsOfExperience,
                ConsultationFee = consultationFee,
                IsActive = isActive
            };
        }

        [Fact]
        public async Task GetDoctorsAsync_WhenSpecialisationIsNull_ShouldReturnMappedDoctors()
        {
            var ct = CancellationToken.None;

            var doctors = new List<Doctor>
            {
                CreateDoctor(
                    doctorId: 1,
                    doctorName: "Doctor One",
                    specialisation: "Cardiologist"),
                CreateDoctor(
                    doctorId: 2,
                    doctorName: "Doctor Two",
                    specialisation: "Neurologist")
            };

            var expectedDtos = new List<DoctorDto>
            {
                CreateDoctorDto(
                    doctorId: 1,
                    doctorName: "Doctor One",
                    specialisation: "Cardiologist"),
                CreateDoctorDto(
                    doctorId: 2,
                    doctorName: "Doctor Two",
                    specialisation: "Neurologist")
            };

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetDoctorsAsync(null, ct))
                .ReturnsAsync(doctors);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(expectedDtos);

            var service = CreateService(
                repositoryMock: repositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetDoctorsAsync(null, ct);

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].DoctorId);
            Assert.Equal("Doctor One", result[0].DoctorName);
            Assert.Equal("Cardiologist", result[0].Specialisation);
            Assert.Equal(2, result[1].DoctorId);
            Assert.Equal("Doctor Two", result[1].DoctorName);
            Assert.Equal("Neurologist", result[1].Specialisation);

            repositoryMock.Verify(x => x.GetDoctorsAsync(null, ct), Times.Once);
            mapperMock.Verify(x => x.Map<List<DoctorDto>>(doctors), Times.Once);
        }

        [Fact]
        public async Task GetDoctorsAsync_WhenSpecialisationIsProvided_ShouldReturnMappedDoctors()
        {
            var ct = CancellationToken.None;

            var specialisation = "Cardiologist";

            var doctors = new List<Doctor>
            {
                CreateDoctor(
                    doctorId: 1,
                    doctorName: "Doctor One",
                    specialisation: specialisation)
            };

            var expectedDtos = new List<DoctorDto>
            {
                CreateDoctorDto(
                    doctorId: 1,
                    doctorName: "Doctor One",
                    specialisation: specialisation)
            };

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetDoctorsAsync(specialisation, ct))
                .ReturnsAsync(doctors);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(expectedDtos);

            var service = CreateService(
                repositoryMock: repositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetDoctorsAsync(specialisation, ct);

            Assert.Single(result);
            Assert.Equal(1, result[0].DoctorId);
            Assert.Equal("Cardiologist", result[0].Specialisation);

            repositoryMock.Verify(x => x.GetDoctorsAsync(specialisation, ct), Times.Once);
            mapperMock.Verify(x => x.Map<List<DoctorDto>>(doctors), Times.Once);
        }

        [Fact]
        public async Task GetDoctorsAsync_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyList()
        {
            var ct = CancellationToken.None;

            var doctors = new List<Doctor>();

            var expectedDtos = new List<DoctorDto>();

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetDoctorsAsync("Cardiologist", ct))
                .ReturnsAsync(doctors);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(expectedDtos);

            var service = CreateService(
                repositoryMock: repositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetDoctorsAsync("Cardiologist", ct);

            Assert.Empty(result);

            repositoryMock.Verify(x => x.GetDoctorsAsync("Cardiologist", ct), Times.Once);
            mapperMock.Verify(x => x.Map<List<DoctorDto>>(doctors), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorExists_ShouldReturnMappedDoctorDto()
        {
            var ct = CancellationToken.None;

            var doctor = CreateDoctor(
                doctorId: 10,
                doctorName: "Doctor Ten",
                specialisation: "Dermatologist",
                yearsOfExperience: 7,
                consultationFee: 750,
                isActive: true);

            var expectedDto = CreateDoctorDto(
                doctorId: 10,
                doctorName: "Doctor Ten",
                specialisation: "Dermatologist",
                yearsOfExperience: 7,
                consultationFee: 750,
                isActive: true);

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(10, ct))
                .ReturnsAsync(doctor);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(expectedDto);

            var service = CreateService(
                repositoryMock: repositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetByIdAsync(10, ct);

            Assert.Equal(10, result.DoctorId);
            Assert.Equal("Doctor Ten", result.DoctorName);
            Assert.Equal("Dermatologist", result.Specialisation);
            Assert.Equal(7, result.YearsOfExperience);
            Assert.Equal(750, result.ConsultationFee);
            Assert.True(result.IsActive);

            repositoryMock.Verify(x => x.GetByIdAsync(10, ct), Times.Once);
            mapperMock.Verify(x => x.Map<DoctorDto>(doctor), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorDoesNotExist_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(99, ct))
                .ReturnsAsync((Doctor?)null);

            var mapperMock = new Mock<IMapper>();

            var service = CreateService(
                repositoryMock: repositoryMock,
                mapperMock: mapperMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.GetByIdAsync(99, ct));

            Assert.Equal("Doctor not found", exception.Message);

            repositoryMock.Verify(x => x.GetByIdAsync(99, ct), Times.Once);
            mapperMock.Verify(x => x.Map<DoctorDto>(It.IsAny<Doctor>()), Times.Never);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDateIsPast_ShouldThrowInvalidException()
        {
            var ct = CancellationToken.None;

            var repositoryMock = new Mock<IDoctorRepository>();

            var service = CreateService(repositoryMock: repositoryMock);

            var pastDate = DateTime.UtcNow.Date.AddDays(-1);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.GetAvailabilityAsync(1, pastDate, ct));

            Assert.Equal("Cannot check past date", exception.Message);

            repositoryMock.Verify(x => x.GetAvailableSlotsAsync(
                    It.IsAny<int>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDateIsToday_ShouldReturnAvailableSlots()
        {
            var ct = CancellationToken.None;

            var doctorId = 5;

            var today = DateTime.UtcNow.Date;

            var slots = new List<string>
            {
                "09:00",
                "10:00",
                "11:00"
            };

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetAvailableSlotsAsync(doctorId, today, ct))
                .ReturnsAsync(slots);

            var service = CreateService(repositoryMock: repositoryMock);

            var result = await service.GetAvailabilityAsync(doctorId, today, ct);

            Assert.Equal(3, result.Count);
            Assert.Contains("09:00", result);
            Assert.Contains("10:00", result);
            Assert.Contains("11:00", result);

            repositoryMock.Verify(x => x.GetAvailableSlotsAsync(doctorId, today, ct), Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDateIsFuture_ShouldReturnAvailableSlots()
        {
            var ct = CancellationToken.None;

            var doctorId = 6;

            var futureDate = DateTime.UtcNow.Date.AddDays(2);

            var slots = new List<string>
            {
                "14:00",
                "15:00"
            };

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetAvailableSlotsAsync(doctorId, futureDate, ct))
                .ReturnsAsync(slots);

            var service = CreateService(repositoryMock: repositoryMock);

            var result = await service.GetAvailabilityAsync(doctorId, futureDate, ct);

            Assert.Equal(2, result.Count);
            Assert.Contains("14:00", result);
            Assert.Contains("15:00", result);

            repositoryMock.Verify(x => x.GetAvailableSlotsAsync(doctorId, futureDate, ct), Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyList()
        {
            var ct = CancellationToken.None;

            var doctorId = 7;

            var futureDate = DateTime.UtcNow.Date.AddDays(3);

            var slots = new List<string>();

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetAvailableSlotsAsync(doctorId, futureDate, ct))
                .ReturnsAsync(slots);

            var service = CreateService(repositoryMock: repositoryMock);

            var result = await service.GetAvailabilityAsync(doctorId, futureDate, ct);

            Assert.Empty(result);

            repositoryMock.Verify(x => x.GetAvailableSlotsAsync(doctorId, futureDate, ct), Times.Once);
        }
    }
}