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
            Mock<IHealthRecordRepository>? healthRepo = null,
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
                claims.Add(new Claim("PatientId", patientId.Value.ToString()));

            if (doctorId.HasValue)
                claims.Add(new Claim("DoctorId", doctorId.Value.ToString()));

            return new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));
        }

        private static Patient CreatePatient(int id = 1)
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

        private static PatientDto CreatePatientDto(int id = 1)
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

        private static HealthRecord CreateHealthRecord(int doctorId = 10)
        {
            var patient = CreatePatient(1);

            var doctor = new Doctor
            {
                DoctorId = doctorId,
                DoctorName = "Doc",
                Specialisation = "Cardiologist",
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = doctorId,
                ScheduledDate = DateTime.UtcNow,
                TimeSlot = "10:00",
                Status = "Completed",
                CancellationReason = "",
                Patient = patient,
                Doctor = doctor
            };

            return new HealthRecord
            {
                PatientId = 1,
                DoctorId = doctorId,
                AppointmentId = 1,
                Patient = patient,
                Doctor = doctor,
                Appointment = appointment,
                Diagnosis = "Test",
                Prescription = "Test"
            };
        }

        // ✅ GetAll
        [Fact]
        public async Task GetAllAsync_Admin_ShouldReturn()
        {
            var repo = new Mock<IPatientRepository>();
            repo.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Patient> { CreatePatient() });

            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<List<PatientDto>>(It.IsAny<List<Patient>>()))
                .Returns(new List<PatientDto> { CreatePatientDto() });

            var service = CreateService(repo, mapper: mapper);
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

        // ✅ GetById
        [Fact]
        public async Task GetByIdAsync_PatientOwn_ShouldReturn()
        {
            var repo = new Mock<IPatientRepository>();
            repo.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreatePatient(1));

            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<PatientDto>(It.IsAny<Patient>()))
                .Returns(CreatePatientDto(1));

            var service = CreateService(repo, mapper: mapper);
            var user = CreateUser("Patient", patientId: 1);

            var result = await service.GetByIdAsync(1, user);

            Assert.Equal(1, result.PatientId);
        }

        [Fact]
        public async Task GetByIdAsync_PatientOther_ShouldThrow()
        {
            var service = CreateService();
            var user = CreateUser("Patient", patientId: 1);

            await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.GetByIdAsync(2, user));
        }

        // ✅ Update
        [Fact]
        public async Task UpdatePatientAsync_Owner_ShouldUpdate()
        {
            var patient = CreatePatient(1);

            var repo = new Mock<IPatientRepository>();
            repo.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            repo.Setup(x => x.UpdateAsync(1, It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map(It.IsAny<UpdatePatientDto>(), patient));

            mapper.Setup(x => x.Map<PatientDto>(patient))
                .Returns(CreatePatientDto(1));

            var service = CreateService(repo, mapper: mapper);
            var user = CreateUser("Patient", patientId: 1);

            var result = await service.UpdatePatientAsync(1, CreateUpdateDto(), user);

            Assert.Equal(1, result.PatientId);
        }

        [Fact]
        public async Task UpdatePatientAsync_NotOwner_ShouldThrow()
        {
            var service = CreateService();
            var user = CreateUser("Patient", patientId: 1);

            await Assert.ThrowsAsync<UnauthorizedException>(
                () => service.UpdatePatientAsync(2, CreateUpdateDto(), user));
        }

        // ✅ Health Records
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
                CreateHealthRecord(10)
            };

            repo.Setup(x => x.GetHealthRecordsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(records);

            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<List<HealthRecordDto>>(records))
                .Returns(new List<HealthRecordDto>());

            var service = CreateService(repo, mapper: mapper);
            var user = CreateUser("Patient", patientId: 1);

            var result = await service.GetHealthRecordsAsync(1, user);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetHealthRecordsAsync_Doctor_ShouldFilter()
        {
            var repo = new Mock<IPatientRepository>();

            var records = new List<HealthRecord>
            {
                CreateHealthRecord(10),
                CreateHealthRecord(20)
            };

            repo.Setup(x => x.GetHealthRecordsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(records);

            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<List<HealthRecordDto>>(It.IsAny<List<HealthRecord>>()))
                .Returns(new List<HealthRecordDto>());

            var service = CreateService(repo, mapper: mapper);
            var user = CreateUser("Doctor", doctorId: 10);

            var result = await service.GetHealthRecordsAsync(1, user);

            Assert.NotNull(result);
        }
    }
}