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
    public class PatientServiceTests
    {
        private static PatientService CreateService(
            Mock<IPatientRepository>? patientRepo = null,
            Mock<IAppointmentRepository>? appointmentRepo = null,
            Mock<IMapper>? mapper = null)
        {
            return new PatientService(
                patientRepo?.Object ?? new Mock<IPatientRepository>().Object,
                appointmentRepo?.Object ?? new Mock<IAppointmentRepository>().Object,
                mapper?.Object ?? new Mock<IMapper>().Object);
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

            return new ClaimsPrincipal(
                new ClaimsIdentity(claims, "TestAuth"));
        }

        private static Patient CreatePatient(
            int id = 1)
        {
            return new Patient
            {
                PatientId = id,
                PatientName = "Test User",
                Email = "test@test.com",
                PhoneNumber = "9999999999",
                Gender = "Male",
                InsuranceID = "INS1",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                IsActive = true
            };
        }

        private static Doctor CreateDoctor(
            int id = 10)
        {
            return new Doctor
            {
                DoctorId = id,
                DoctorName = "Doctor One",
                Specialisation = "Cardiologist",
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };
        }

        private static PatientDto CreatePatientDto(
            int id = 1)
        {
            return new PatientDto
            {
                PatientId = id,
                PatientName = "Test User",
                Email = "test@test.com",
                PhoneNumber = "9999999999",
                Gender = "Male",
                InsuranceID = "INS1",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                IsActive = true
            };
        }

        private static UpdatePatientDto CreateUpdateDto()
        {
            return new UpdatePatientDto
            {
                PatientName = "Updated",
                PhoneNumber = "9999999999",
                Gender = "Male",
                InsuranceID = "INS1",
                DateOfBirth = DateTime.UtcNow.AddYears(-25)
            };
        }

        private static Appointment CreateAppointment(
            int appointmentId = 1,
            int patientId = 1,
            int doctorId = 10,
            string status = "Completed")
        {
            return new Appointment
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                Patient = CreatePatient(patientId),
                DoctorId = doctorId,
                Doctor = CreateDoctor(doctorId),
                ScheduledDate = DateTime.UtcNow.Date,
                TimeSlot = "10:00",
                Status = status,
                CancellationReason = string.Empty
            };
        }

        private static HealthRecord CreateHealthRecord(
            int doctorId = 10,
            int patientId = 1)
        {
            var patient = CreatePatient(patientId);

            var doctor = CreateDoctor(doctorId);

            var appointment = CreateAppointment(
                appointmentId: 1,
                patientId: patientId,
                doctorId: doctorId,
                status: "Completed");

            return new HealthRecord
            {
                HealthRecordId = 1,
                PatientId = patientId,
                Patient = patient,
                DoctorId = doctorId,
                Doctor = doctor,
                AppointmentId = appointment.AppointmentId,
                Appointment = appointment,
                VisitDate = DateTime.UtcNow,
                Diagnosis = "Test",
                Prescription = "Test",
                Notes = "Test notes"
            };
        }

        [Fact]
        public async Task GetAllAsync_Admin_ShouldReturn()
        {
            var repo = new Mock<IPatientRepository>();

            repo.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Patient>
                {
                    CreatePatient()
                });

            var mapper = new Mock<IMapper>();

            mapper.Setup(x => x.Map<List<PatientDto>>(It.IsAny<List<Patient>>()))
                .Returns(new List<PatientDto>
                {
                    CreatePatientDto()
                });

            var service = CreateService(
                patientRepo: repo,
                mapper: mapper);

            var user = CreateUser("Admin");

            var result = await service.GetAllAsync(user);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllAsync_NotAdmin_ShouldThrow()
        {
            var service = CreateService();

            var user = CreateUser("Patient");

            await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetAllAsync(user));
        }

        [Fact]
        public async Task GetByIdAsync_Admin_ShouldReturnPatient()
        {
            var ct = CancellationToken.None;

            var patient = CreatePatient(1);

            var repo = new Mock<IPatientRepository>();

            repo.Setup(x => x.GetByIdAsync(1, ct))
                .ReturnsAsync(patient);

            var mapper = new Mock<IMapper>();

            mapper.Setup(x => x.Map<PatientDto>(patient))
                .Returns(CreatePatientDto(1));

            var service = CreateService(
                patientRepo: repo,
                mapper: mapper);

            var user = CreateUser("Admin");

            var result = await service.GetByIdAsync(1, user, ct);

            Assert.Equal(1, result.PatientId);

            repo.Verify(x => x.GetByIdAsync(1, ct), Times.Once);
            mapper.Verify(x => x.Map<PatientDto>(patient), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_PatientOwn_ShouldReturn()
        {
            var repo = new Mock<IPatientRepository>();

            repo.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreatePatient(1));

            var mapper = new Mock<IMapper>();

            mapper.Setup(x => x.Map<PatientDto>(It.IsAny<Patient>()))
                .Returns(CreatePatientDto(1));

            var service = CreateService(
                patientRepo: repo,
                mapper: mapper);

            var user = CreateUser(
                "Patient",
                patientId: 1);

            var result = await service.GetByIdAsync(1, user);

            Assert.Equal(1, result.PatientId);
        }

        [Fact]
        public async Task GetByIdAsync_PatientOther_ShouldThrow()
        {
            var service = CreateService();

            var user = CreateUser(
                "Patient",
                patientId: 1);

            await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetByIdAsync(2, user));
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientClaimMissing_ShouldThrowUnauthorizedException()
        {
            var service = CreateService();

            var user = CreateUser("Patient");

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetByIdAsync(1, user));

            Assert.Equal("PatientId claim missing", exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientNotFound_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var repo = new Mock<IPatientRepository>();

            repo.Setup(x => x.GetByIdAsync(1, ct))
                .ReturnsAsync((Patient?)null);

            var service = CreateService(
                patientRepo: repo);

            var user = CreateUser("Admin");

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.GetByIdAsync(1, user, ct));

            Assert.Equal("Patient not found", exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_DoctorWithValidAppointment_ShouldReturnPatient()
        {
            var ct = CancellationToken.None;

            var patientId = 1;
            var doctorId = 10;

            var patient = CreatePatient(patientId);

            var appointments = new List<Appointment>
            {
                CreateAppointment(
                    appointmentId: 1,
                    patientId: patientId,
                    doctorId: doctorId,
                    status: "Completed")
            };

            var patientRepo = new Mock<IPatientRepository>();

            patientRepo.Setup(x => x.GetByIdAsync(patientId, ct))
                .ReturnsAsync(patient);

            var appointmentRepo = new Mock<IAppointmentRepository>();

            appointmentRepo.Setup(x => x.GetAppointmentsAsync(
                    patientId,
                    doctorId,
                    null,
                    ct))
                .ReturnsAsync(appointments);

            var mapper = new Mock<IMapper>();

            mapper.Setup(x => x.Map<PatientDto>(patient))
                .Returns(CreatePatientDto(patientId));

            var service = CreateService(
                patientRepo: patientRepo,
                appointmentRepo: appointmentRepo,
                mapper: mapper);

            var user = CreateUser(
                "Doctor",
                doctorId: doctorId);

            var result = await service.GetByIdAsync(
                patientId,
                user,
                ct);

            Assert.Equal(patientId, result.PatientId);

            appointmentRepo.Verify(x => x.GetAppointmentsAsync(patientId, doctorId, null, ct), Times.Once);
            patientRepo.Verify(x => x.GetByIdAsync(patientId, ct), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_DoctorWithoutClaim_ShouldThrowUnauthorizedException()
        {
            var service = CreateService();

            var user = CreateUser("Doctor");

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetByIdAsync(1, user));

            Assert.Equal("DoctorId claim missing", exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_DoctorWithoutValidAppointment_ShouldThrowUnauthorizedException()
        {
            var ct = CancellationToken.None;

            var patientId = 1;
            var doctorId = 10;

            var appointmentRepo = new Mock<IAppointmentRepository>();

            appointmentRepo.Setup(x => x.GetAppointmentsAsync(
                    patientId,
                    doctorId,
                    null,
                    ct))
                .ReturnsAsync(new List<Appointment>());

            var service = CreateService(
                appointmentRepo: appointmentRepo);

            var user = CreateUser(
                "Doctor",
                doctorId: doctorId);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetByIdAsync(patientId, user, ct));

            Assert.Equal("You can view only patients with a valid appointment with you", exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_DoctorWithOnlyCancelledAppointment_ShouldThrowUnauthorizedException()
        {
            var ct = CancellationToken.None;

            var patientId = 1;
            var doctorId = 10;

            var appointments = new List<Appointment>
            {
                CreateAppointment(
                    appointmentId: 1,
                    patientId: patientId,
                    doctorId: doctorId,
                    status: "Cancelled")
            };

            appointments[0].CancellationReason = "Cancelled";

            var appointmentRepo = new Mock<IAppointmentRepository>();

            appointmentRepo.Setup(x => x.GetAppointmentsAsync(
                    patientId,
                    doctorId,
                    null,
                    ct))
                .ReturnsAsync(appointments);

            var service = CreateService(
                appointmentRepo: appointmentRepo);

            var user = CreateUser(
                "Doctor",
                doctorId: doctorId);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetByIdAsync(patientId, user, ct));

            Assert.Equal("You can view only patients with a valid appointment with you", exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_UnsupportedRole_ShouldThrowUnauthorizedException()
        {
            var service = CreateService();

            var user = CreateUser("Unknown");

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetByIdAsync(1, user));

            Assert.Equal("Unauthorized access", exception.Message);
        }

        [Fact]
        public async Task UpdatePatientAsync_Owner_ShouldUpdate()
        {
            var patient = CreatePatient(1);

            var repo = new Mock<IPatientRepository>();

            repo.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            repo.Setup(x => x.UpdateAsync(
                    1,
                    It.IsAny<Patient>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            var mapper = new Mock<IMapper>();

            mapper.Setup(x => x.Map(It.IsAny<UpdatePatientDto>(), patient));

            mapper.Setup(x => x.Map<PatientDto>(patient))
                .Returns(CreatePatientDto(1));

            var service = CreateService(
                patientRepo: repo,
                mapper: mapper);

            var user = CreateUser(
                "Patient",
                patientId: 1);

            var result = await service.UpdatePatientAsync(
                1,
                CreateUpdateDto(),
                user);

            Assert.Equal(1, result.PatientId);
        }

        [Fact]
        public async Task UpdatePatientAsync_NotOwner_ShouldThrow()
        {
            var service = CreateService();

            var user = CreateUser(
                "Patient",
                patientId: 1);

            await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.UpdatePatientAsync(2, CreateUpdateDto(), user));
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenDateOfBirthIsFuture_ShouldThrowInvalidException()
        {
            var request = CreateUpdateDto();

            request.DateOfBirth = DateTime.UtcNow.Date.AddDays(1);

            var service = CreateService();

            var user = CreateUser(
                "Patient",
                patientId: 1);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.UpdatePatientAsync(1, request, user));

            Assert.Equal("Date of birth cannot be greater than today's date", exception.Message);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenDateOfBirthIsTooOld_ShouldThrowInvalidException()
        {
            var request = CreateUpdateDto();

            request.DateOfBirth = DateTime.UtcNow.Date.AddYears(-121);

            var service = CreateService();

            var user = CreateUser(
                "Patient",
                patientId: 1);

            var exception = await Assert.ThrowsAsync<InvalidException>(
                () => service.UpdatePatientAsync(1, request, user));

            Assert.Equal("Please enter a valid date of birth", exception.Message);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenPatientNotFound_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var repo = new Mock<IPatientRepository>();

            repo.Setup(x => x.GetByIdAsync(1, ct))
                .ReturnsAsync((Patient?)null);

            var service = CreateService(
                patientRepo: repo);

            var user = CreateUser(
                "Patient",
                patientId: 1);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.UpdatePatientAsync(1, CreateUpdateDto(), user, ct));

            Assert.Equal("Patient not found", exception.Message);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenUpdateReturnsNull_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var patient = CreatePatient(1);

            var repo = new Mock<IPatientRepository>();

            repo.Setup(x => x.GetByIdAsync(1, ct))
                .ReturnsAsync(patient);

            repo.Setup(x => x.UpdateAsync(1, patient, ct))
                .ReturnsAsync((Patient?)null);

            var mapper = new Mock<IMapper>();

            mapper.Setup(x => x.Map(It.IsAny<UpdatePatientDto>(), patient));

            var service = CreateService(
                patientRepo: repo,
                mapper: mapper);

            var user = CreateUser(
                "Patient",
                patientId: 1);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.UpdatePatientAsync(1, CreateUpdateDto(), user, ct));

            Assert.Equal("Patient not found", exception.Message);
        }

        [Fact]
        public async Task GetHealthRecordsAsync_Admin_ShouldThrow()
        {
            var service = CreateService();

            var user = CreateUser("Admin");

            await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetHealthRecordsAsync(1, user));
        }

        [Fact]
        public async Task GetHealthRecordsAsync_PatientOwn_ShouldReturn()
        {
            var repo = new Mock<IPatientRepository>();

            var records = new List<HealthRecord>
            {
                CreateHealthRecord(doctorId: 10, patientId: 1)
            };

            repo.Setup(x => x.GetHealthRecordsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(records);

            var mapper = new Mock<IMapper>();

            mapper.Setup(x => x.Map<List<HealthRecordDto>>(records))
                .Returns(new List<HealthRecordDto>
                {
                    new HealthRecordDto
                    {
                        HealthRecordId = 1,
                        PatientId = 1,
                        DoctorId = 10,
                        Diagnosis = "Test",
                        Prescription = "Test"
                    }
                });

            var service = CreateService(
                patientRepo: repo,
                mapper: mapper);

            var user = CreateUser(
                "Patient",
                patientId: 1);

            var result = await service.GetHealthRecordsAsync(1, user);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetHealthRecordsAsync_WhenPatientClaimMissing_ShouldThrowUnauthorizedException()
        {
            var service = CreateService();

            var user = CreateUser("Patient");

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetHealthRecordsAsync(1, user));

            Assert.Equal("PatientId claim missing", exception.Message);
        }

        [Fact]
        public async Task GetHealthRecordsAsync_WhenPatientRequestsAnotherPatientsRecords_ShouldThrowUnauthorizedException()
        {
            var service = CreateService();

            var user = CreateUser(
                "Patient",
                patientId: 2);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetHealthRecordsAsync(1, user));

            Assert.Equal("You can view only your own health records", exception.Message);
        }

        [Fact]
        public async Task GetHealthRecordsAsync_Doctor_ShouldFilter()
        {
            var repo = new Mock<IPatientRepository>();

            var records = new List<HealthRecord>
            {
                CreateHealthRecord(doctorId: 10, patientId: 1),
                CreateHealthRecord(doctorId: 20, patientId: 1)
            };

            repo.Setup(x => x.GetHealthRecordsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(records);

            var mapper = new Mock<IMapper>();

            mapper.Setup(x => x.Map<List<HealthRecordDto>>(
                    It.IsAny<List<HealthRecord>>()))
                .Returns(new List<HealthRecordDto>());

            var service = CreateService(
                patientRepo: repo,
                mapper: mapper);

            var user = CreateUser(
                "Doctor",
                doctorId: 10);

            var result = await service.GetHealthRecordsAsync(1, user);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetHealthRecordsAsync_Doctor_ShouldMapOnlyDoctorRecords()
        {
            var ct = CancellationToken.None;

            var patientId = 1;
            var doctorId = 10;

            var records = new List<HealthRecord>
            {
                CreateHealthRecord(doctorId: doctorId, patientId: patientId),
                CreateHealthRecord(doctorId: 20, patientId: patientId)
            };

            var patientRepo = new Mock<IPatientRepository>();

            patientRepo.Setup(x => x.GetHealthRecordsAsync(patientId, ct))
                .ReturnsAsync(records);

            var mapper = new Mock<IMapper>();

            mapper.Setup(x => x.Map<List<HealthRecordDto>>(
                    It.Is<List<HealthRecord>>(list =>
                        list.Count == 1 &&
                        list[0].DoctorId == doctorId)))
                .Returns(new List<HealthRecordDto>
                {
                    new HealthRecordDto
                    {
                        HealthRecordId = 1,
                        PatientId = patientId,
                        DoctorId = doctorId,
                        Diagnosis = "Test",
                        Prescription = "Test"
                    }
                });

            var service = CreateService(
                patientRepo: patientRepo,
                mapper: mapper);

            var user = CreateUser(
                "Doctor",
                doctorId: doctorId);

            var result = await service.GetHealthRecordsAsync(
                patientId,
                user,
                ct);

            Assert.Single(result);
            Assert.Equal(doctorId, result[0].DoctorId);

            mapper.Verify(x => x.Map<List<HealthRecordDto>>(
                    It.Is<List<HealthRecord>>(list =>
                        list.Count == 1 &&
                        list[0].DoctorId == doctorId)),
                Times.Once);
        }

        [Fact]
        public async Task GetHealthRecordsAsync_WhenDoctorClaimMissing_ShouldThrowUnauthorizedException()
        {
            var service = CreateService();

            var user = CreateUser("Doctor");

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetHealthRecordsAsync(1, user));

            Assert.Equal("DoctorId claim missing", exception.Message);
        }

        [Fact]
        public async Task GetHealthRecordsAsync_WhenUnsupportedRole_ShouldThrowUnauthorizedException()
        {
            var service = CreateService();

            var user = CreateUser("Unknown");

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetHealthRecordsAsync(1, user));

            Assert.Equal("Unauthorized access", exception.Message);
        }
    }
}