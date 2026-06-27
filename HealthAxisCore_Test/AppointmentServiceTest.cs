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
    public class AppointmentServiceTests
    {
        private static ClaimsPrincipal CreateUser(
            string role,
            int? patientId = null,
            int? doctorId = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, role)
            };

            if (patientId.HasValue)
            {
                claims.Add(new Claim("PatientId", patientId.Value.ToString()));
            }

            if (doctorId.HasValue)
            {
                claims.Add(new Claim("DoctorId", doctorId.Value.ToString()));
            }

            var identity = new ClaimsIdentity(claims, "TestAuth");

            return new ClaimsPrincipal(identity);
        }

        private static AppointmentService CreateService(
            Mock<IAppointmentRepository>? appointmentRepositoryMock = null,
            Mock<IDoctorRepository>? doctorRepositoryMock = null,
            Mock<IMapper>? mapperMock = null)
        {
            return new AppointmentService(
                appointmentRepositoryMock?.Object ?? new Mock<IAppointmentRepository>().Object,
                doctorRepositoryMock?.Object ?? new Mock<IDoctorRepository>().Object,
                mapperMock?.Object ?? new Mock<IMapper>().Object);
        }

        private static Appointment CreateAppointment(
            int appointmentId = 1,
            int patientId = 10,
            int doctorId = 20,
            string status = "Pending",
            string cancellationReason = "")
        {
            return new Appointment
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                Patient = new Patient
                {
                    PatientId = patientId,
                    PatientName = "Patient One",
                    DateOfBirth = new DateTime(2000, 1, 1),
                    Gender = "Male",
                    Email = "patient@test.com",
                    PhoneNumber = "9876543210",
                    InsuranceID = "INS001",
                    IsActive = true
                },
                DoctorId = doctorId,
                Doctor = new Doctor
                {
                    DoctorId = doctorId,
                    DoctorName = "Doctor One",
                    Specialisation = "Cardiologist",
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true
                },
                ScheduledDate = DateTime.UtcNow.Date.AddDays(1),
                TimeSlot = "09:00",
                Status = status,
                CancellationReason = cancellationReason
            };
        }

        private static AppointmentDto CreateAppointmentDto(
            int appointmentId = 1,
            int patientId = 10,
            int doctorId = 20,
            string status = "Pending",
            string cancellationReason = "")
        {
            return new AppointmentDto
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                PatientName = "Patient One",
                DoctorId = doctorId,
                DoctorName = "Doctor One",
                Specialisation = "Cardiologist",
                ScheduledDate = DateTime.UtcNow.Date.AddDays(1),
                TimeSlot = "09:00",
                Status = status,
                CancellationReason = cancellationReason
            };
        }

        [Fact]
        public async Task GetAppointmentsAsync_WhenUserIsPatient_ShouldOverridePatientIdFromClaims()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Patient",
                patientId: 11);

            var appointments = new List<Appointment>
            {
                CreateAppointment(patientId: 11)
            };

            var appointmentDtos = new List<AppointmentDto>
            {
                CreateAppointmentDto(patientId: 11)
            };

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .Setup(x => x.GetAppointmentsAsync(
                    11,
                    null,
                    null,
                    ct))
                .ReturnsAsync(appointments);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<AppointmentDto>>(appointments))
                .Returns(appointmentDtos);

            var service = CreateService(
                appointmentRepositoryMock: appointmentRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetAppointmentsAsync(
                patientId: 999,
                doctorId: null,
                date: null,
                user: user,
                ct: ct);

            Assert.Single(result);
            Assert.Equal(11, result[0].PatientId);

            appointmentRepositoryMock.Verify(x => x.GetAppointmentsAsync(
                    11,
                    null,
                    null,
                    ct),
                Times.Once);

            mapperMock.Verify(x => x.Map<List<AppointmentDto>>(appointments), Times.Once);
        }

        [Fact]
        public async Task GetAppointmentsAsync_WhenUserIsDoctor_ShouldOverrideDoctorIdFromClaims()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Doctor",
                doctorId: 22);

            var appointments = new List<Appointment>
            {
                CreateAppointment(doctorId: 22)
            };

            var appointmentDtos = new List<AppointmentDto>
            {
                CreateAppointmentDto(doctorId: 22)
            };

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .Setup(x => x.GetAppointmentsAsync(
                    null,
                    22,
                    null,
                    ct))
                .ReturnsAsync(appointments);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<AppointmentDto>>(appointments))
                .Returns(appointmentDtos);

            var service = CreateService(
                appointmentRepositoryMock: appointmentRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetAppointmentsAsync(
                patientId: null,
                doctorId: 999,
                date: null,
                user: user,
                ct: ct);

            Assert.Single(result);
            Assert.Equal(22, result[0].DoctorId);

            appointmentRepositoryMock.Verify(x => x.GetAppointmentsAsync(
                    null,
                    22,
                    null,
                    ct),
                Times.Once);

            mapperMock.Verify(x => x.Map<List<AppointmentDto>>(appointments), Times.Once);
        }

        [Fact]
        public async Task GetAppointmentsAsync_WhenUserIsAdmin_ShouldUsePassedFilters()
        {
            var ct = CancellationToken.None;

            var date = DateTime.UtcNow.Date.AddDays(1);

            var user = CreateUser("Admin");

            var appointments = new List<Appointment>
            {
                CreateAppointment(patientId: 12, doctorId: 24)
            };

            var appointmentDtos = new List<AppointmentDto>
            {
                CreateAppointmentDto(patientId: 12, doctorId: 24)
            };

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .Setup(x => x.GetAppointmentsAsync(
                    12,
                    24,
                    date,
                    ct))
                .ReturnsAsync(appointments);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<AppointmentDto>>(appointments))
                .Returns(appointmentDtos);

            var service = CreateService(
                appointmentRepositoryMock: appointmentRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetAppointmentsAsync(
                patientId: 12,
                doctorId: 24,
                date: date,
                user: user,
                ct: ct);

            Assert.Single(result);
            Assert.Equal(12, result[0].PatientId);
            Assert.Equal(24, result[0].DoctorId);

            appointmentRepositoryMock.Verify(x => x.GetAppointmentsAsync(
                    12,
                    24,
                    date,
                    ct),
                Times.Once);

            mapperMock.Verify(x => x.Map<List<AppointmentDto>>(appointments), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenScheduledDateIsPast_ShouldThrowInvalidException()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Patient",
                patientId: 10);

            var request = new CreateAppointmentDto
            {
                DoctorId = 20,
                ScheduledDate = DateTime.UtcNow.Date.AddDays(-1),
                TimeSlot = "09:00"
            };

            var service = CreateService();

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.CreateAsync(request, user, ct));

            Assert.Equal("Cannot book past date", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenSlotIsNotAvailable_ShouldThrowInvalidException()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Patient",
                patientId: 10);

            var request = new CreateAppointmentDto
            {
                DoctorId = 20,
                ScheduledDate = DateTime.UtcNow.Date.AddDays(1),
                TimeSlot = "09:00"
            };

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetAvailableSlotsAsync(
                    request.DoctorId,
                    request.ScheduledDate,
                    ct))
                .ReturnsAsync(new List<string> { "10:00", "11:00" });

            var service = CreateService(
                doctorRepositoryMock: doctorRepositoryMock);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.CreateAsync(request, user, ct));

            Assert.Equal("Slot not available", exception.Message);

            doctorRepositoryMock.Verify(x => x.GetAvailableSlotsAsync(
                    request.DoctorId,
                    request.ScheduledDate,
                    ct),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenValidRequestAndDetailsExist_ShouldCreateAndReturnMappedDto()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Patient",
                patientId: 10);

            var request = new CreateAppointmentDto
            {
                DoctorId = 20,
                ScheduledDate = DateTime.UtcNow.Date.AddDays(1),
                TimeSlot = "09:00"
            };

            var mappedAppointment = CreateAppointment(
                appointmentId: 0,
                patientId: 0,
                doctorId: request.DoctorId);

            var savedAppointment = CreateAppointment(
                appointmentId: 100,
                patientId: 10,
                doctorId: request.DoctorId);

            var detailedAppointment = CreateAppointment(
                appointmentId: 100,
                patientId: 10,
                doctorId: request.DoctorId);

            var expectedDto = CreateAppointmentDto(
                appointmentId: 100,
                patientId: 10,
                doctorId: request.DoctorId);

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetAvailableSlotsAsync(
                    request.DoctorId,
                    request.ScheduledDate,
                    ct))
                .ReturnsAsync(new List<string> { "09:00", "10:00" });

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .Setup(x => x.CreateAsync(
                    It.Is<Appointment>(a =>
                        a.PatientId == 10 &&
                        a.DoctorId == request.DoctorId),
                    ct))
                .ReturnsAsync(savedAppointment);

            appointmentRepositoryMock
                .Setup(x => x.GetDetailsAsync(savedAppointment.AppointmentId, ct))
                .ReturnsAsync(detailedAppointment);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<Appointment>(request))
                .Returns(mappedAppointment);

            mapperMock
                .Setup(x => x.Map<AppointmentDto>(detailedAppointment))
                .Returns(expectedDto);

            var service = CreateService(
                appointmentRepositoryMock: appointmentRepositoryMock,
                doctorRepositoryMock: doctorRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.CreateAsync(request, user, ct);

            Assert.Equal(100, result.AppointmentId);
            Assert.Equal(10, result.PatientId);
            Assert.Equal(20, result.DoctorId);

            doctorRepositoryMock.Verify(x => x.GetAvailableSlotsAsync(
                    request.DoctorId,
                    request.ScheduledDate,
                    ct),
                Times.Once);

            appointmentRepositoryMock.Verify(x => x.CreateAsync(
                    It.Is<Appointment>(a =>
                        a.PatientId == 10 &&
                        a.DoctorId == request.DoctorId),
                    ct),
                Times.Once);

            appointmentRepositoryMock.Verify(x => x.GetDetailsAsync(
                    savedAppointment.AppointmentId,
                    ct),
                Times.Once);

            mapperMock.Verify(x => x.Map<Appointment>(request), Times.Once);
            mapperMock.Verify(x => x.Map<AppointmentDto>(detailedAppointment), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenDetailsDoesNotExist_ShouldMapSavedAppointment()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Patient",
                patientId: 10);

            var request = new CreateAppointmentDto
            {
                DoctorId = 20,
                ScheduledDate = DateTime.UtcNow.Date.AddDays(1),
                TimeSlot = "10:00"
            };

            var mappedAppointment = CreateAppointment(
                appointmentId: 0,
                patientId: 0,
                doctorId: request.DoctorId);

            var savedAppointment = CreateAppointment(
                appointmentId: 101,
                patientId: 10,
                doctorId: request.DoctorId);

            var expectedDto = CreateAppointmentDto(
                appointmentId: 101,
                patientId: 10,
                doctorId: request.DoctorId);

            var doctorRepositoryMock = new Mock<IDoctorRepository>();

            doctorRepositoryMock
                .Setup(x => x.GetAvailableSlotsAsync(
                    request.DoctorId,
                    request.ScheduledDate,
                    ct))
                .ReturnsAsync(new List<string> { "10:00" });

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Appointment>(), ct))
                .ReturnsAsync(savedAppointment);

            appointmentRepositoryMock
                .Setup(x => x.GetDetailsAsync(savedAppointment.AppointmentId, ct))
                .ReturnsAsync((Appointment?)null);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<Appointment>(request))
                .Returns(mappedAppointment);

            mapperMock
                .Setup(x => x.Map<AppointmentDto>(savedAppointment))
                .Returns(expectedDto);

            var service = CreateService(
                appointmentRepositoryMock: appointmentRepositoryMock,
                doctorRepositoryMock: doctorRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.CreateAsync(request, user, ct);

            Assert.Equal(101, result.AppointmentId);
            Assert.Equal(10, result.PatientId);

            appointmentRepositoryMock.Verify(x => x.GetDetailsAsync(
                    savedAppointment.AppointmentId,
                    ct),
                Times.Once);

            mapperMock.Verify(x => x.Map<AppointmentDto>(savedAppointment), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenAppointmentDoesNotExist_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var user = CreateUser("Admin");

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Confirmed",
                CancellationReason = ""
            };

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .Setup(x => x.GetDetailsAsync(99, ct))
                .ReturnsAsync((Appointment?)null);

            var service = CreateService(
                appointmentRepositoryMock: appointmentRepositoryMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.UpdateStatusAsync(99, request, user, ct));

            Assert.Equal("Appointment not found", exception.Message);

            appointmentRepositoryMock.Verify(x => x.GetDetailsAsync(99, ct), Times.Once);
            appointmentRepositoryMock.Verify(x => x.UpdateAsync(
                    It.IsAny<int>(),
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenPatientTriesNonCancelledStatus_ShouldThrowUnauthorizedException()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Patient",
                patientId: 10);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Confirmed",
                CancellationReason = ""
            };

            var appointment = CreateAppointment(
                appointmentId: 1,
                patientId: 10,
                status: "Pending");

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .Setup(x => x.GetDetailsAsync(1, ct))
                .ReturnsAsync(appointment);

            var service = CreateService(
                appointmentRepositoryMock: appointmentRepositoryMock);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.UpdateStatusAsync(1, request, user, ct));

            Assert.Equal("Patients can only cancel appointments", exception.Message);

            appointmentRepositoryMock.Verify(x => x.GetDetailsAsync(1, ct), Times.Once);
            appointmentRepositoryMock.Verify(x => x.UpdateAsync(
                    It.IsAny<int>(),
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenUpdateReturnsNull_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var user = CreateUser("Admin");

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Confirmed",
                CancellationReason = "Approved"
            };

            var appointment = CreateAppointment(
                appointmentId: 2,
                status: "Pending");

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .Setup(x => x.GetDetailsAsync(2, ct))
                .ReturnsAsync(appointment);

            appointmentRepositoryMock
                .Setup(x => x.UpdateAsync(2, appointment, ct))
                .ReturnsAsync((Appointment?)null);

            var service = CreateService(
                appointmentRepositoryMock: appointmentRepositoryMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.UpdateStatusAsync(2, request, user, ct));

            Assert.Equal("Appointment not found", exception.Message);

            appointmentRepositoryMock.Verify(x => x.GetDetailsAsync(2, ct), Times.Once);
            appointmentRepositoryMock.Verify(x => x.UpdateAsync(2, appointment, ct), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenValidAndDetailsExist_ShouldUpdateAndReturnMappedDto()
        {
            var ct = CancellationToken.None;

            var user = CreateUser("Doctor", doctorId: 20);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Completed",
                CancellationReason = "Completed consultation"
            };

            var appointment = CreateAppointment(
                appointmentId: 3,
                doctorId: 20,
                status: "Confirmed");

            var updatedAppointment = CreateAppointment(
                appointmentId: 3,
                doctorId: 20,
                status: "Completed",
                cancellationReason: "Completed consultation");

            var detailedAppointment = CreateAppointment(
                appointmentId: 3,
                doctorId: 20,
                status: "Completed",
                cancellationReason: "Completed consultation");

            var expectedDto = CreateAppointmentDto(
                appointmentId: 3,
                doctorId: 20,
                status: "Completed",
                cancellationReason: "Completed consultation");

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .SetupSequence(x => x.GetDetailsAsync(3, It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment)
                .ReturnsAsync(detailedAppointment);

            appointmentRepositoryMock
                .Setup(x => x.UpdateAsync(3, It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedAppointment);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<AppointmentDto>(detailedAppointment))
                .Returns(expectedDto);

            var service = CreateService(
                appointmentRepositoryMock: appointmentRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.UpdateStatusAsync(3, request, user, ct);

            Assert.Equal("Completed", result.Status);
            Assert.Equal("Completed consultation", result.CancellationReason);
            Assert.Equal("Completed", appointment.Status);
            Assert.Equal("Completed consultation", appointment.CancellationReason);

            appointmentRepositoryMock.Verify(x => x.GetDetailsAsync(3, It.IsAny<CancellationToken>()), Times.Exactly(2));
            appointmentRepositoryMock.Verify(x => x.UpdateAsync(3, It.IsAny<Appointment>(), It.IsAny<CancellationToken>()), Times.Once);
            // combined verification for GetDetailsAsync above covers both calls
            mapperMock.Verify(x => x.Map<AppointmentDto>(detailedAppointment), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenDetailsAfterUpdateDoesNotExist_ShouldMapUpdatedAppointment()
        {
            var ct = CancellationToken.None;

            var user = CreateUser("Patient", patientId: 10);

            var request = new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = null
            };

            var appointment = CreateAppointment(
                appointmentId: 4,
                patientId: 10,
                status: "Pending");

            var updatedAppointment = CreateAppointment(
                appointmentId: 4,
                patientId: 10,
                status: "Cancelled",
                cancellationReason: "");

            var expectedDto = CreateAppointmentDto(
                appointmentId: 4,
                patientId: 10,
                status: "Cancelled",
                cancellationReason: "");

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .SetupSequence(x => x.GetDetailsAsync(4, It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointment)
                .ReturnsAsync((Appointment?)null);

            appointmentRepositoryMock
                .Setup(x => x.UpdateAsync(4, It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedAppointment);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<AppointmentDto>(updatedAppointment))
                .Returns(expectedDto);

            var service = CreateService(
                appointmentRepositoryMock: appointmentRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.UpdateStatusAsync(4, request, user, ct);

            Assert.Equal("Cancelled", result.Status);
            Assert.Equal(string.Empty, result.CancellationReason);
            Assert.Equal("Cancelled", appointment.Status);
            Assert.Equal(string.Empty, appointment.CancellationReason);

            mapperMock.Verify(x => x.Map<AppointmentDto>(updatedAppointment), Times.Once);
            appointmentRepositoryMock.Verify(x => x.UpdateAsync(4, It.IsAny<Appointment>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenAppointmentDoesNotExist_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .Setup(x => x.DeleteAsync(55, ct))
                .ReturnsAsync((Appointment?)null);

            var service = CreateService(
                appointmentRepositoryMock: appointmentRepositoryMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.DeleteAsync(55, ct));

            Assert.Equal("Appointment not found", exception.Message);

            appointmentRepositoryMock.Verify(x => x.DeleteAsync(55, ct), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenAppointmentExists_ShouldDeleteSuccessfully()
        {
            var ct = CancellationToken.None;

            var deletedAppointment = CreateAppointment(
                appointmentId: 66);

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .Setup(x => x.DeleteAsync(66, ct))
                .ReturnsAsync(deletedAppointment);

            var service = CreateService(
                appointmentRepositoryMock: appointmentRepositoryMock);

            await service.DeleteAsync(66, ct);

            appointmentRepositoryMock.Verify(x => x.DeleteAsync(66, ct), Times.Once);
        }
    }
}