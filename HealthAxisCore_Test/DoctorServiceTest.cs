using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Implementation;
using Moq;
using System.Security.Claims;

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

        private static ClaimsPrincipal CreateUser(
            string role,
            int? doctorId = null,
            int? patientId = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, role)
            };

            if (doctorId.HasValue)
            {
                claims.Add(new Claim("DoctorId", doctorId.Value.ToString()));
            }

            if (patientId.HasValue)
            {
                claims.Add(new Claim("PatientId", patientId.Value.ToString()));
            }

            return new ClaimsPrincipal(
                new ClaimsIdentity(claims, "TestAuth"));
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
        public async Task GetDoctorsAsync_WhenUserIsDoctorAndDoctorExists_ShouldReturnOnlyOwnDoctor()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Doctor",
                doctorId: 10);

            var doctor = CreateDoctor(
                doctorId: 10,
                doctorName: "Doctor Ten");

            var doctorDto = CreateDoctorDto(
                doctorId: 10,
                doctorName: "Doctor Ten");

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(10, ct))
                .ReturnsAsync(doctor);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(doctorDto);

            var service = CreateService(
                repositoryMock,
                mapperMock);

            var result = await service.GetDoctorsAsync(
                specialisation: "Cardiologist",
                user: user,
                ct: ct);

            Assert.Single(result);
            Assert.Equal(10, result[0].DoctorId);
            Assert.Equal("Doctor Ten", result[0].DoctorName);

            repositoryMock.Verify(x => x.GetByIdAsync(10, ct), Times.Once);
            repositoryMock.Verify(x => x.GetDoctorsAsync(
                    It.IsAny<string?>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);

            mapperMock.Verify(x => x.Map<DoctorDto>(doctor), Times.Once);
        }

        [Fact]
        public async Task GetDoctorsAsync_WhenDoctorIdClaimMissing_ShouldThrowUnauthorizedException()
        {
            var ct = CancellationToken.None;

            var user = CreateUser("Doctor");

            var service = CreateService();

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetDoctorsAsync(null, user, ct));

            Assert.Equal("DoctorId claim missing", exception.Message);
        }

        [Fact]
        public async Task GetDoctorsAsync_WhenUserIsDoctorAndDoctorNotFound_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Doctor",
                doctorId: 99);

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(99, ct))
                .ReturnsAsync((Doctor?)null);

            var service = CreateService(repositoryMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.GetDoctorsAsync(null, user, ct));

            Assert.Equal("Doctor not found", exception.Message);

            repositoryMock.Verify(x => x.GetByIdAsync(99, ct), Times.Once);
        }

        [Fact]
        public async Task GetDoctorsAsync_WhenUserIsPatient_ShouldReturnMappedDoctors()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Patient",
                patientId: 1);

            var doctors = new List<Doctor>
            {
                CreateDoctor(doctorId: 1, doctorName: "Doctor One"),
                CreateDoctor(doctorId: 2, doctorName: "Doctor Two", specialisation: "Neurologist")
            };

            var expectedDtos = new List<DoctorDto>
            {
                CreateDoctorDto(doctorId: 1, doctorName: "Doctor One"),
                CreateDoctorDto(doctorId: 2, doctorName: "Doctor Two", specialisation: "Neurologist")
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
                repositoryMock,
                mapperMock);

            var result = await service.GetDoctorsAsync(null, user, ct);

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].DoctorId);
            Assert.Equal(2, result[1].DoctorId);

            repositoryMock.Verify(x => x.GetDoctorsAsync(null, ct), Times.Once);
            mapperMock.Verify(x => x.Map<List<DoctorDto>>(doctors), Times.Once);
        }

        [Fact]
        public async Task GetDoctorsAsync_WhenUserIsAdminAndSpecialisationProvided_ShouldReturnFilteredMappedDoctors()
        {
            var ct = CancellationToken.None;

            var user = CreateUser("Admin");

            var specialisation = "Cardiologist";

            var doctors = new List<Doctor>
            {
                CreateDoctor(doctorId: 1, specialisation: specialisation)
            };

            var expectedDtos = new List<DoctorDto>
            {
                CreateDoctorDto(doctorId: 1, specialisation: specialisation)
            };

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetDoctorsAsync(specialisation, ct))
                .ReturnsAsync(doctors);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(expectedDtos);

            var service = CreateService(repositoryMock, mapperMock);

            var result = await service.GetDoctorsAsync(specialisation, user, ct);

            Assert.Single(result);
            Assert.Equal("Cardiologist", result[0].Specialisation);

            repositoryMock.Verify(x => x.GetDoctorsAsync(specialisation, ct), Times.Once);
        }

        [Fact]
        public async Task GetPagedAsync_WhenPageNumberAndPageSizeAreInvalid_ShouldNormalizeValues()
        {
            var ct = CancellationToken.None;

            var user = CreateUser("Admin");

            var doctors = new List<Doctor>
            {
                CreateDoctor(doctorId: 1)
            };

            var doctorDtos = new List<DoctorDto>
            {
                CreateDoctorDto(doctorId: 1)
            };

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.CountDoctorsAsync(null, ct))
                .ReturnsAsync(1);

            repositoryMock
                .Setup(x => x.GetPagedDoctorsAsync(null, 1, 10, ct))
                .ReturnsAsync(doctors);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var service = CreateService(repositoryMock, mapperMock);

            var result = await service.GetPagedAsync(
                specialisation: null,
                user: user,
                pageNumber: 0,
                pageSize: 0,
                ct: ct);

            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(1, result.TotalCount);
            Assert.Equal(1, result.TotalPages);
            Assert.Single(result.Items);

            repositoryMock.Verify(x => x.GetPagedDoctorsAsync(null, 1, 10, ct), Times.Once);
        }

        [Fact]
        public async Task GetPagedAsync_WhenUserIsDoctorAndDoctorExists_ShouldReturnOwnDoctorPagedResult()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Doctor",
                doctorId: 5);

            var doctor = CreateDoctor(
                doctorId: 5,
                doctorName: "Doctor Five");

            var doctorDto = CreateDoctorDto(
                doctorId: 5,
                doctorName: "Doctor Five");

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(5, ct))
                .ReturnsAsync(doctor);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(doctorDto);

            var service = CreateService(repositoryMock, mapperMock);

            var result = await service.GetPagedAsync(
                specialisation: "Cardiologist",
                user: user,
                pageNumber: 3,
                pageSize: 20,
                ct: ct);

            Assert.Single(result.Items);
            Assert.Equal(5, result.Items[0].DoctorId);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(1, result.PageSize);
            Assert.Equal(1, result.TotalCount);
            Assert.Equal(1, result.TotalPages);

            repositoryMock.Verify(x => x.GetByIdAsync(5, ct), Times.Once);
            repositoryMock.Verify(x => x.CountDoctorsAsync(
                    It.IsAny<string?>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
            repositoryMock.Verify(x => x.GetPagedDoctorsAsync(
                    It.IsAny<string?>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetPagedAsync_WhenDoctorIdClaimMissing_ShouldThrowUnauthorizedException()
        {
            var ct = CancellationToken.None;

            var user = CreateUser("Doctor");

            var service = CreateService();

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetPagedAsync(null, user, 1, 10, ct));

            Assert.Equal("DoctorId claim missing", exception.Message);
        }

        [Fact]
        public async Task GetPagedAsync_WhenUserIsDoctorAndDoctorNotFound_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Doctor",
                doctorId: 99);

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(99, ct))
                .ReturnsAsync((Doctor?)null);

            var service = CreateService(repositoryMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.GetPagedAsync(null, user, 1, 10, ct));

            Assert.Equal("Doctor not found", exception.Message);
        }

        [Fact]
        public async Task GetPagedAsync_WhenUserIsAdmin_ShouldReturnPagedDoctorResult()
        {
            var ct = CancellationToken.None;

            var user = CreateUser("Admin");

            var specialisation = "Neurologist";

            var doctors = new List<Doctor>
            {
                CreateDoctor(doctorId: 1, specialisation: specialisation),
                CreateDoctor(doctorId: 2, doctorName: "Doctor Two", specialisation: specialisation)
            };

            var doctorDtos = new List<DoctorDto>
            {
                CreateDoctorDto(doctorId: 1, specialisation: specialisation),
                CreateDoctorDto(doctorId: 2, doctorName: "Doctor Two", specialisation: specialisation)
            };

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.CountDoctorsAsync(specialisation, ct))
                .ReturnsAsync(12);

            repositoryMock
                .Setup(x => x.GetPagedDoctorsAsync(specialisation, 2, 5, ct))
                .ReturnsAsync(doctors);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var service = CreateService(repositoryMock, mapperMock);

            var result = await service.GetPagedAsync(
                specialisation,
                user,
                pageNumber: 2,
                pageSize: 5,
                ct);

            Assert.Equal(2, result.PageNumber);
            Assert.Equal(5, result.PageSize);
            Assert.Equal(12, result.TotalCount);
            Assert.Equal(3, result.TotalPages);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal("Neurologist", result.Items[0].Specialisation);

            repositoryMock.Verify(x => x.CountDoctorsAsync(specialisation, ct), Times.Once);
            repositoryMock.Verify(x => x.GetPagedDoctorsAsync(specialisation, 2, 5, ct), Times.Once);
            mapperMock.Verify(x => x.Map<List<DoctorDto>>(doctors), Times.Once);
        }

        [Fact]
        public async Task GetPagedAsync_WhenNoDoctorsFound_ShouldReturnEmptyPagedResult()
        {
            var ct = CancellationToken.None;

            var user = CreateUser("Patient", patientId: 1);

            var doctors = new List<Doctor>();

            var doctorDtos = new List<DoctorDto>();

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.CountDoctorsAsync("Cardiologist", ct))
                .ReturnsAsync(0);

            repositoryMock
                .Setup(x => x.GetPagedDoctorsAsync("Cardiologist", 1, 10, ct))
                .ReturnsAsync(doctors);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var service = CreateService(repositoryMock, mapperMock);

            var result = await service.GetPagedAsync(
                "Cardiologist",
                user,
                1,
                10,
                ct);

            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
            Assert.Equal(0, result.TotalPages);

            repositoryMock.Verify(x => x.CountDoctorsAsync("Cardiologist", ct), Times.Once);
            repositoryMock.Verify(x => x.GetPagedDoctorsAsync("Cardiologist", 1, 10, ct), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorUserViewsOwnProfile_ShouldReturnDoctorDto()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Doctor",
                doctorId: 7);

            var doctor = CreateDoctor(
                doctorId: 7,
                doctorName: "Doctor Seven");

            var doctorDto = CreateDoctorDto(
                doctorId: 7,
                doctorName: "Doctor Seven");

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(7, ct))
                .ReturnsAsync(doctor);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(doctorDto);

            var service = CreateService(repositoryMock, mapperMock);

            var result = await service.GetByIdAsync(7, user, ct);

            Assert.Equal(7, result.DoctorId);
            Assert.Equal("Doctor Seven", result.DoctorName);

            repositoryMock.Verify(x => x.GetByIdAsync(7, ct), Times.Once);
            mapperMock.Verify(x => x.Map<DoctorDto>(doctor), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorIdClaimMissing_ShouldThrowUnauthorizedException()
        {
            var ct = CancellationToken.None;

            var user = CreateUser("Doctor");

            var service = CreateService();

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetByIdAsync(7, user, ct));

            Assert.Equal("DoctorId claim missing", exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorUserViewsAnotherDoctorProfile_ShouldThrowUnauthorizedException()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Doctor",
                doctorId: 7);

            var service = CreateService();

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetByIdAsync(8, user, ct));

            Assert.Equal("You can view only your own doctor profile", exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_WhenUserIsAdminAndDoctorExists_ShouldReturnDoctorDto()
        {
            var ct = CancellationToken.None;

            var user = CreateUser("Admin");

            var doctor = CreateDoctor(
                doctorId: 8,
                doctorName: "Doctor Eight");

            var doctorDto = CreateDoctorDto(
                doctorId: 8,
                doctorName: "Doctor Eight");

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(8, ct))
                .ReturnsAsync(doctor);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(doctorDto);

            var service = CreateService(repositoryMock, mapperMock);

            var result = await service.GetByIdAsync(8, user, ct);

            Assert.Equal(8, result.DoctorId);
            Assert.Equal("Doctor Eight", result.DoctorName);

            repositoryMock.Verify(x => x.GetByIdAsync(8, ct), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorDoesNotExist_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var user = CreateUser("Patient", patientId: 1);

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(99, ct))
                .ReturnsAsync((Doctor?)null);

            var service = CreateService(repositoryMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.GetByIdAsync(99, user, ct));

            Assert.Equal("Doctor not found", exception.Message);

            repositoryMock.Verify(x => x.GetByIdAsync(99, ct), Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDateIsPast_ShouldThrowInvalidException()
        {
            var ct = CancellationToken.None;

            var service = CreateService();

            var pastDate = DateTime.UtcNow.Date.AddDays(-1);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.GetAvailabilityAsync(1, pastDate, ct));

            Assert.Equal("Cannot check past date", exception.Message);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDateIsToday_ShouldReturnAvailableSlots()
        {
            var ct = CancellationToken.None;

            var today = DateTime.UtcNow.Date;

            var slots = new List<string>
            {
                "09:00",
                "10:00",
                "11:00"
            };

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetAvailableSlotsAsync(1, today, ct))
                .ReturnsAsync(slots);

            var service = CreateService(repositoryMock);

            var result = await service.GetAvailabilityAsync(1, today, ct);

            Assert.Equal(3, result.Count);
            Assert.Contains("09:00", result);
            Assert.Contains("10:00", result);
            Assert.Contains("11:00", result);

            repositoryMock.Verify(x => x.GetAvailableSlotsAsync(1, today, ct), Times.Once);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDateIsFuture_ShouldReturnAvailableSlots()
        {
            var ct = CancellationToken.None;

            var futureDate = DateTime.UtcNow.Date.AddDays(2);

            var slots = new List<string>
            {
                "14:00",
                "15:00"
            };

            var repositoryMock = new Mock<IDoctorRepository>();

            repositoryMock
                .Setup(x => x.GetAvailableSlotsAsync(2, futureDate, ct))
                .ReturnsAsync(slots);

            var service = CreateService(repositoryMock);

            var result = await service.GetAvailabilityAsync(2, futureDate, ct);

            Assert.Equal(2, result.Count);
            Assert.Contains("14:00", result);
            Assert.Contains("15:00", result);

            repositoryMock.Verify(x => x.GetAvailableSlotsAsync(2, futureDate, ct), Times.Once);
        }
    }
}