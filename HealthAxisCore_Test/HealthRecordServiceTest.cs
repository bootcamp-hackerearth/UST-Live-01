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
    public class HealthRecordServiceTests
    {
        private static HealthRecordService CreateService(
            Mock<IHealthRecordRepository>? healthRecordRepositoryMock = null,
            Mock<IAppointmentRepository>? appointmentRepositoryMock = null,
            Mock<IMapper>? mapperMock = null)
        {
            return new HealthRecordService(
                healthRecordRepositoryMock?.Object ?? new Mock<IHealthRecordRepository>().Object,
                appointmentRepositoryMock?.Object ?? new Mock<IAppointmentRepository>().Object,
                mapperMock?.Object ?? new Mock<IMapper>().Object);
        }

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

        private static Patient CreatePatient(
            int patientId = 1,
            string patientName = "Patient One",
            bool isActive = true)
        {
            return new Patient
            {
                PatientId = patientId,
                PatientName = patientName,
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = "Male",
                Email = "patient@test.com",
                PhoneNumber = "9876543210",
                InsuranceID = "INS001",
                IsActive = isActive
            };
        }

        private static Doctor CreateDoctor(
            int doctorId = 1,
            string doctorName = "Doctor One",
            bool isActive = true)
        {
            return new Doctor
            {
                DoctorId = doctorId,
                DoctorName = doctorName,
                Specialisation = "Cardiologist",
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = isActive
            };
        }

        private static Appointment CreateAppointment(
            int appointmentId = 1,
            int patientId = 10,
            int doctorId = 20,
            string status = "Confirmed")
        {
            return new Appointment
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                Patient = CreatePatient(patientId),
                DoctorId = doctorId,
                Doctor = CreateDoctor(doctorId),
                ScheduledDate = DateTime.UtcNow.Date.AddDays(1),
                TimeSlot = "09:00",
                Status = status,
                CancellationReason = string.Empty
            };
        }

        private static HealthRecord CreateHealthRecord(
            int healthRecordId = 1,
            int patientId = 10,
            int doctorId = 20,
            int appointmentId = 100)
        {
            return new HealthRecord
            {
                HealthRecordId = healthRecordId,
                PatientId = patientId,
                Patient = CreatePatient(patientId),
                DoctorId = doctorId,
                Doctor = CreateDoctor(doctorId),
                AppointmentId = appointmentId,
                Appointment = CreateAppointment(
                    appointmentId: appointmentId,
                    patientId: patientId,
                    doctorId: doctorId),
                VisitDate = DateTime.UtcNow,
                Diagnosis = "Diagnosis One",
                Prescription = "Prescription One",
                Notes = "Notes One"
            };
        }

        private static HealthRecordDto CreateHealthRecordDto(
            int healthRecordId = 1,
            int patientId = 10,
            int doctorId = 20,
            int appointmentId = 100)
        {
            return new HealthRecordDto
            {
                HealthRecordId = healthRecordId,
                PatientId = patientId,
                PatientName = "Patient One",
                DoctorId = doctorId,
                DoctorName = "Doctor One",
                AppointmentId = appointmentId,
                VisitDate = DateTime.UtcNow,
                Diagnosis = "Diagnosis One",
                Prescription = "Prescription One",
                Notes = "Notes One"
            };
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenRecordsExist_ShouldReturnMappedHealthRecordDtos()
        {
            var ct = CancellationToken.None;

            var patientId = 10;

            var user = CreateUser(
                role: "Patient",
                patientId: patientId);

            var records = new List<HealthRecord>
            {
                CreateHealthRecord(
                    healthRecordId: 1,
                    patientId: patientId,
                    doctorId: 20,
                    appointmentId: 100),
                CreateHealthRecord(
                    healthRecordId: 2,
                    patientId: patientId,
                    doctorId: 21,
                    appointmentId: 101)
            };

            var expectedDtos = new List<HealthRecordDto>
            {
                CreateHealthRecordDto(
                    healthRecordId: 1,
                    patientId: patientId,
                    doctorId: 20,
                    appointmentId: 100),
                CreateHealthRecordDto(
                    healthRecordId: 2,
                    patientId: patientId,
                    doctorId: 21,
                    appointmentId: 101)
            };

            var healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();

            healthRecordRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(patientId, ct))
                .ReturnsAsync(records);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<HealthRecordDto>>(records))
                .Returns(expectedDtos);

            var service = CreateService(
                healthRecordRepositoryMock: healthRecordRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetByPatientIdAsync(
                patientId,
                user,
                ct);

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].HealthRecordId);
            Assert.Equal(2, result[1].HealthRecordId);
            Assert.Equal(patientId, result[0].PatientId);
            Assert.Equal(patientId, result[1].PatientId);

            healthRecordRepositoryMock.Verify(x => x.GetByPatientIdAsync(patientId, ct), Times.Once);
            mapperMock.Verify(x => x.Map<List<HealthRecordDto>>(records), Times.Once);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenNoRecordsExist_ShouldReturnEmptyList()
        {
            var ct = CancellationToken.None;

            var patientId = 10;

            var user = CreateUser(
                role: "Admin");

            var records = new List<HealthRecord>();

            var expectedDtos = new List<HealthRecordDto>();

            var healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();

            healthRecordRepositoryMock
                .Setup(x => x.GetByPatientIdAsync(patientId, ct))
                .ReturnsAsync(records);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<HealthRecordDto>>(records))
                .Returns(expectedDtos);

            var service = CreateService(
                healthRecordRepositoryMock: healthRecordRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetByPatientIdAsync(
                patientId,
                user,
                ct);

            Assert.Empty(result);

            healthRecordRepositoryMock.Verify(x => x.GetByPatientIdAsync(patientId, ct), Times.Once);
            mapperMock.Verify(x => x.Map<List<HealthRecordDto>>(records), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenHealthRecordExists_ShouldReturnMappedHealthRecordDto()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Doctor",
                doctorId: 20);

            var record = CreateHealthRecord(
                healthRecordId: 5,
                patientId: 10,
                doctorId: 20,
                appointmentId: 100);

            var expectedDto = CreateHealthRecordDto(
                healthRecordId: 5,
                patientId: 10,
                doctorId: 20,
                appointmentId: 100);

            var healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();

            healthRecordRepositoryMock
                .Setup(x => x.GetDetailsAsync(5, ct))
                .ReturnsAsync(record);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<HealthRecordDto>(record))
                .Returns(expectedDto);

            var service = CreateService(
                healthRecordRepositoryMock: healthRecordRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetByIdAsync(
                5,
                user,
                ct);

            Assert.Equal(5, result.HealthRecordId);
            Assert.Equal(10, result.PatientId);
            Assert.Equal(20, result.DoctorId);
            Assert.Equal(100, result.AppointmentId);

            healthRecordRepositoryMock.Verify(x => x.GetDetailsAsync(5, ct), Times.Once);
            mapperMock.Verify(x => x.Map<HealthRecordDto>(record), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenHealthRecordDoesNotExist_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var user = CreateUser(
                role: "Admin");

            var healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();

            healthRecordRepositoryMock
                .Setup(x => x.GetDetailsAsync(99, ct))
                .ReturnsAsync((HealthRecord?)null);

            var mapperMock = new Mock<IMapper>();

            var service = CreateService(
                healthRecordRepositoryMock: healthRecordRepositoryMock,
                mapperMock: mapperMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.GetByIdAsync(99, user, ct));

            Assert.Equal("Health record not found", exception.Message);

            healthRecordRepositoryMock.Verify(x => x.GetDetailsAsync(99, ct), Times.Once);
            mapperMock.Verify(x => x.Map<HealthRecordDto>(It.IsAny<HealthRecord>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenAppointmentDoesNotExist_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var doctorId = 20;

            var user = CreateUser(
                role: "Doctor",
                doctorId: doctorId);

            var request = new CreateHealthRecordDto
            {
                PatientId = 10,
                AppointmentId = 100,
                Diagnosis = "Diagnosis One",
                Prescription = "Prescription One",
                Notes = "Notes One"
            };

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .Setup(x => x.GetDetailsAsync(request.AppointmentId, ct))
                .ReturnsAsync((Appointment?)null);

            var service = CreateService(
                appointmentRepositoryMock: appointmentRepositoryMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.CreateAsync(request, user, ct));

            Assert.Equal("Appointment not found", exception.Message);

            appointmentRepositoryMock.Verify(x => x.GetDetailsAsync(request.AppointmentId, ct), Times.Once);
            appointmentRepositoryMock.Verify(x => x.UpdateAsync(
                    It.IsAny<int>(),
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenAppointmentBelongsToAnotherDoctor_ShouldThrowUnauthorizedException()
        {
            var ct = CancellationToken.None;

            var loggedInDoctorId = 20;
            var appointmentDoctorId = 99;

            var user = CreateUser(
                role: "Doctor",
                doctorId: loggedInDoctorId);

            var request = new CreateHealthRecordDto
            {
                PatientId = 10,
                AppointmentId = 100,
                Diagnosis = "Diagnosis One",
                Prescription = "Prescription One",
                Notes = "Notes One"
            };

            var appointment = CreateAppointment(
                appointmentId: request.AppointmentId,
                patientId: request.PatientId,
                doctorId: appointmentDoctorId);

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .Setup(x => x.GetDetailsAsync(request.AppointmentId, ct))
                .ReturnsAsync(appointment);

            var service = CreateService(
                appointmentRepositoryMock: appointmentRepositoryMock);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.CreateAsync(request, user, ct));

            Assert.Equal("Cannot complete another doctor's appointment", exception.Message);

            appointmentRepositoryMock.Verify(x => x.GetDetailsAsync(request.AppointmentId, ct), Times.Once);
            appointmentRepositoryMock.Verify(x => x.UpdateAsync(
                    It.IsAny<int>(),
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenValidRequestAndDetailsExist_ShouldCreateRecordCompleteAppointmentAndReturnMappedDto()
        {
            var ct = CancellationToken.None;

            var doctorId = 20;
            var patientId = 10;
            var appointmentId = 100;

            var user = CreateUser(
                role: "Doctor",
                doctorId: doctorId);

            var request = new CreateHealthRecordDto
            {
                PatientId = patientId,
                AppointmentId = appointmentId,
                Diagnosis = "Diagnosis One",
                Prescription = "Prescription One",
                Notes = "Notes One"
            };

            var appointment = CreateAppointment(
                appointmentId: appointmentId,
                patientId: patientId,
                doctorId: doctorId,
                status: "Confirmed");

            var mappedRecord = new HealthRecord
            {
                HealthRecordId = 0,
                PatientId = request.PatientId,
                Patient = CreatePatient(patientId),
                DoctorId = null,
                Doctor = CreateDoctor(doctorId),
                AppointmentId = request.AppointmentId,
                Appointment = appointment,
                VisitDate = default,
                Diagnosis = request.Diagnosis,
                Prescription = request.Prescription,
                Notes = request.Notes
            };

            var savedRecord = CreateHealthRecord(
                healthRecordId: 50,
                patientId: patientId,
                doctorId: doctorId,
                appointmentId: appointmentId);

            var detailedRecord = CreateHealthRecord(
                healthRecordId: 50,
                patientId: patientId,
                doctorId: doctorId,
                appointmentId: appointmentId);

            var expectedDto = CreateHealthRecordDto(
                healthRecordId: 50,
                patientId: patientId,
                doctorId: doctorId,
                appointmentId: appointmentId);

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .Setup(x => x.GetDetailsAsync(appointmentId, ct))
                .ReturnsAsync(appointment);

            appointmentRepositoryMock
                .Setup(x => x.UpdateAsync(appointmentId, appointment, ct))
                .ReturnsAsync(appointment);

            var healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();

            healthRecordRepositoryMock
                .Setup(x => x.CreateAsync(
                    It.Is<HealthRecord>(r =>
                        r.PatientId == patientId &&
                        r.DoctorId == doctorId &&
                        r.AppointmentId == appointmentId &&
                        r.Diagnosis == request.Diagnosis &&
                        r.Prescription == request.Prescription),
                    ct))
                .ReturnsAsync(savedRecord);

            healthRecordRepositoryMock
                .Setup(x => x.GetDetailsAsync(savedRecord.HealthRecordId, ct))
                .ReturnsAsync(detailedRecord);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<HealthRecord>(request))
                .Returns(mappedRecord);

            mapperMock
                .Setup(x => x.Map<HealthRecordDto>(detailedRecord))
                .Returns(expectedDto);

            var service = CreateService(
                healthRecordRepositoryMock: healthRecordRepositoryMock,
                appointmentRepositoryMock: appointmentRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.CreateAsync(
                request,
                user,
                ct);

            Assert.Equal(50, result.HealthRecordId);
            Assert.Equal(patientId, result.PatientId);
            Assert.Equal(doctorId, result.DoctorId);
            Assert.Equal(appointmentId, result.AppointmentId);

            Assert.Equal("Completed", appointment.Status);
            Assert.Equal(doctorId, mappedRecord.DoctorId);
            Assert.NotEqual(default, mappedRecord.VisitDate);

            appointmentRepositoryMock.Verify(x => x.GetDetailsAsync(appointmentId, ct), Times.Once);
            appointmentRepositoryMock.Verify(x => x.UpdateAsync(appointmentId, appointment, ct), Times.Once);

            healthRecordRepositoryMock.Verify(x => x.CreateAsync(
                    It.Is<HealthRecord>(r =>
                        r.PatientId == patientId &&
                        r.DoctorId == doctorId &&
                        r.AppointmentId == appointmentId),
                    ct),
                Times.Once);

            healthRecordRepositoryMock.Verify(x => x.GetDetailsAsync(savedRecord.HealthRecordId, ct), Times.Once);

            mapperMock.Verify(x => x.Map<HealthRecord>(request), Times.Once);
            mapperMock.Verify(x => x.Map<HealthRecordDto>(detailedRecord), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenSavedDetailsDoesNotExist_ShouldMapSavedRecord()
        {
            var ct = CancellationToken.None;

            var doctorId = 20;
            var patientId = 10;
            var appointmentId = 100;

            var user = CreateUser(
                role: "Doctor",
                doctorId: doctorId);

            var request = new CreateHealthRecordDto
            {
                PatientId = patientId,
                AppointmentId = appointmentId,
                Diagnosis = "Diagnosis Two",
                Prescription = "Prescription Two",
                Notes = null
            };

            var appointment = CreateAppointment(
                appointmentId: appointmentId,
                patientId: patientId,
                doctorId: doctorId,
                status: "Confirmed");

            var mappedRecord = new HealthRecord
            {
                HealthRecordId = 0,
                PatientId = request.PatientId,
                Patient = CreatePatient(patientId),
                DoctorId = null,
                Doctor = CreateDoctor(doctorId),
                AppointmentId = request.AppointmentId,
                Appointment = appointment,
                VisitDate = default,
                Diagnosis = request.Diagnosis,
                Prescription = request.Prescription,
                Notes = request.Notes
            };

            var savedRecord = CreateHealthRecord(
                healthRecordId: 60,
                patientId: patientId,
                doctorId: doctorId,
                appointmentId: appointmentId);

            var expectedDto = CreateHealthRecordDto(
                healthRecordId: 60,
                patientId: patientId,
                doctorId: doctorId,
                appointmentId: appointmentId);

            expectedDto.Diagnosis = "Diagnosis Two";
            expectedDto.Prescription = "Prescription Two";
            expectedDto.Notes = null;

            var appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            appointmentRepositoryMock
                .Setup(x => x.GetDetailsAsync(appointmentId, ct))
                .ReturnsAsync(appointment);

            appointmentRepositoryMock
                .Setup(x => x.UpdateAsync(appointmentId, appointment, ct))
                .ReturnsAsync(appointment);

            var healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();

            healthRecordRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<HealthRecord>(), ct))
                .ReturnsAsync(savedRecord);

            healthRecordRepositoryMock
                .Setup(x => x.GetDetailsAsync(savedRecord.HealthRecordId, ct))
                .ReturnsAsync((HealthRecord?)null);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<HealthRecord>(request))
                .Returns(mappedRecord);

            mapperMock
                .Setup(x => x.Map<HealthRecordDto>(savedRecord))
                .Returns(expectedDto);

            var service = CreateService(
                healthRecordRepositoryMock: healthRecordRepositoryMock,
                appointmentRepositoryMock: appointmentRepositoryMock,
                mapperMock: mapperMock);

            var result = await service.CreateAsync(
                request,
                user,
                ct);

            Assert.Equal(60, result.HealthRecordId);
            Assert.Equal(patientId, result.PatientId);
            Assert.Equal(doctorId, result.DoctorId);
            Assert.Equal(appointmentId, result.AppointmentId);
            Assert.Null(result.Notes);

            Assert.Equal("Completed", appointment.Status);
            Assert.Equal(doctorId, mappedRecord.DoctorId);
            Assert.NotEqual(default, mappedRecord.VisitDate);

            healthRecordRepositoryMock.Verify(x => x.GetDetailsAsync(savedRecord.HealthRecordId, ct), Times.Once);
            mapperMock.Verify(x => x.Map<HealthRecordDto>(savedRecord), Times.Once);
        }
        [Fact]
        public async Task GetByPatientIdAsync_DoctorWithoutClaim_Throws()
        {
            var service = CreateService();

            var user = CreateUser("Doctor");

            await Assert.ThrowsAsync<UnauthorizedException>(() =>
                service.GetByPatientIdAsync(10, user));
        }

        [Fact]
        public async Task GetByPatientIdAsync_InvalidRole_Throws()
        {
            var service = CreateService();

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            await Assert.ThrowsAsync<UnauthorizedException>(() =>
                service.GetByPatientIdAsync(10, user));
        }

        [Fact]
        public async Task GetByIdAsync_PatientAccessingOthersRecord_Throws()
        {
            var ct = CancellationToken.None;

            var record = CreateHealthRecord(patientId: 99);

            var repo = new Mock<IHealthRecordRepository>();
            repo.Setup(x => x.GetDetailsAsync(1, ct)).ReturnsAsync(record);

            var service = CreateService(healthRecordRepositoryMock: repo);

            var user = CreateUser("Patient", patientId: 10);

            await Assert.ThrowsAsync<UnauthorizedException>(() =>
                service.GetByIdAsync(1, user, ct));
        }

        [Fact]
        public async Task GetByIdAsync_DoctorAccessingOthersRecord_Throws()
        {
            var ct = CancellationToken.None;

            var record = CreateHealthRecord(doctorId: 99);

            var repo = new Mock<IHealthRecordRepository>();
            repo.Setup(x => x.GetDetailsAsync(1, ct)).ReturnsAsync(record);

            var service = CreateService(healthRecordRepositoryMock: repo);

            var user = CreateUser("Doctor", doctorId: 10);

            await Assert.ThrowsAsync<UnauthorizedException>(() =>
                service.GetByIdAsync(1, user, ct));
        }

        [Fact]
        public async Task GetByIdAsync_PatientWithoutClaim_Throws()
        {
            var ct = CancellationToken.None;

            var record = CreateHealthRecord();

            var repo = new Mock<IHealthRecordRepository>();
            repo.Setup(x => x.GetDetailsAsync(1, ct)).ReturnsAsync(record);

            var service = CreateService(healthRecordRepositoryMock: repo);

            await Assert.ThrowsAsync<UnauthorizedException>(() =>
                service.GetByIdAsync(1, CreateUser("Patient"), ct));
        }

        [Fact]
        public async Task GetByIdAsync_DoctorWithoutClaim_Throws()
        {
            var ct = CancellationToken.None;

            var record = CreateHealthRecord();

            var repo = new Mock<IHealthRecordRepository>();
            repo.Setup(x => x.GetDetailsAsync(1, ct)).ReturnsAsync(record);

            var service = CreateService(healthRecordRepositoryMock: repo);

            await Assert.ThrowsAsync<UnauthorizedException>(() =>
                service.GetByIdAsync(1, CreateUser("Doctor"), ct));
        }
    }
}