using AutoMapper;
using HealthAxis.API.DTOs.Admin;
using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.DTOs.HealthRecords;
using HealthAxis.API.DTOs.Patients;
using HealthAxis.API.DTOs.Appointments;
using HealthAxis.API.Enums;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;
using HealthAxis.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Numerics;
using System.Security.Claims;

namespace HealthAxis.API.Tests.Services
{
    public class AdminServiceTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IHealthRecordRepository> _healthRecordRepositoryMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AdminService _adminService;

        public AdminServiceTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _userManagerMock = CreateUserManagerMock();
            _mapperMock = new Mock<IMapper>();

            _adminService = new AdminService(
                _patientRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _healthRecordRepositoryMock.Object,
                _appointmentRepositoryMock.Object,
                _userManagerMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetPatientsAsync_WhenPatientsExist_ReturnsMappedPatients()
        {
            // Arrange
            List<Patient> patients = new()
            {
                new Patient
                {
                    PatientId = 1,
                    FullName = "Patient One",
                    Email = "patient1@test.com",
                    PhoneNumber = "1111111111"
                }
            };

            List<PatientReadDto> expectedDtos = new()
            {
                new PatientReadDto
                {
                    PatientId = 1,
                    FullName = "Patient One",
                    Email = "patient1@test.com",
                    PhoneNumber = "1111111111"
                }
            };

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(mapper => mapper.Map<List<PatientReadDto>>(patients))
                .Returns(expectedDtos);

            // Act
            List<PatientReadDto> result =
                await _adminService.GetPatientsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal(expectedDtos[0].PatientId, result[0].PatientId);

            _patientRepositoryMock.Verify(
                repository => repository.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenPatientExists_ReturnsUpdatedPatient()
        {
            // Arrange
            const int patientId = 1;

            Patient patient = new()
            {
                PatientId = patientId,
                FullName = "Old Name",
                Email = "old@test.com",
                PhoneNumber = "1111111111"
            };

            PatientUpdateDto updateDto = new()
            {
                FullName = "Updated Name",
                Email = "updated@test.com",
                PhoneNumber = "2222222222"
            };

            PatientReadDto expectedDto = new()
            {
                PatientId = patientId,
                FullName = "Updated Name",
                Email = "updated@test.com",
                PhoneNumber = "2222222222"
            };

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(mapper => mapper.Map(updateDto, patient));

            _patientRepositoryMock
                .Setup(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(mapper => mapper.Map<PatientReadDto>(patient))
                .Returns(expectedDto);

            // Act
            PatientReadDto result =
                await _adminService.UpdatePatientAsync(patientId, updateDto);

            // Assert
            Assert.Equal(expectedDto.PatientId, result.PatientId);
            Assert.Equal(expectedDto.FullName, result.FullName);

            _patientRepositoryMock.Verify(
                repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenPatientDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            const int patientId = 404;

            PatientUpdateDto updateDto = new()
            {
                FullName = "Updated Name",
                Email = "updated@test.com",
                PhoneNumber = "2222222222"
            };

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _adminService.UpdatePatientAsync(patientId, updateDto));
        }

        [Fact]
        public async Task GetDoctorsAsync_WhenDoctorsExist_ReturnsMappedDoctors()
        {
            // Arrange
            List<Doctor> doctors = new()
            {
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr. Test",
                    Specialisation = Specialisation.Cardiology,
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true
                }
            };

            List<DoctorReadDto> expectedDtos = new()
            {
                new DoctorReadDto
                {
                    DoctorId = 1,
                    FullName = "Dr. Test",
                    Specialisation = Specialisation.Cardiology,
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true
                }
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(mapper => mapper.Map<List<DoctorReadDto>>(doctors))
                .Returns(expectedDtos);

            // Act
            List<DoctorReadDto> result =
                await _adminService.GetDoctorsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal(expectedDtos[0].DoctorId, result[0].DoctorId);
        }

        [Fact]
        public async Task CreateDoctorAsync_WhenPasswordDoesNotMatch_ThrowsBadRequestException()
        {
            // Arrange
            AdminDoctorCreateDto dto = CreateAdminDoctorCreateDto();
            dto.ConfirmPassword = "Different@123";

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() =>
                _adminService.CreateDoctorAsync(dto));
        }

        [Fact]
        public async Task CreateDoctorAsync_WhenEmailAlreadyExists_ThrowsConflictException()
        {
            // Arrange
            AdminDoctorCreateDto dto = CreateAdminDoctorCreateDto();

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(dto.Email))
                .ReturnsAsync(new IdentityUser
                {
                    Email = dto.Email,
                    UserName = dto.Email
                });

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() =>
                _adminService.CreateDoctorAsync(dto));
        }

        [Fact]
        public async Task CreateDoctorAsync_WhenValidRequest_CreatesDoctorAndIdentityUser()
        {
            // Arrange
            AdminDoctorCreateDto dto = CreateAdminDoctorCreateDto();

            Doctor createdDoctor = new()
            {
                DoctorId = 10,
                FullName = dto.FullName,
                Specialisation = dto.Specialisation,
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = dto.IsActive
            };

            DoctorReadDto expectedDto = new()
            {
                DoctorId = 10,
                FullName = dto.FullName,
                Specialisation = dto.Specialisation,
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = dto.IsActive
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser?)null);

            _doctorRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<Doctor>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdDoctor);

            _userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    dto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(manager => manager.AddClaimAsync(
                    It.IsAny<IdentityUser>(),
                    It.Is<Claim>(claim =>
                        claim.Type == ClaimTypes.Role &&
                        claim.Value == "Doctor")))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(manager => manager.AddClaimAsync(
                    It.IsAny<IdentityUser>(),
                    It.Is<Claim>(claim =>
                        claim.Type == "ReferenceId" &&
                        claim.Value == createdDoctor.DoctorId.ToString())))
                .ReturnsAsync(IdentityResult.Success);

            _mapperMock
                .Setup(mapper => mapper.Map<DoctorReadDto>(createdDoctor))
                .Returns(expectedDto);

            // Act
            DoctorReadDto result =
                await _adminService.CreateDoctorAsync(dto);

            // Assert
            Assert.Equal(expectedDto.DoctorId, result.DoctorId);
            Assert.Equal(expectedDto.FullName, result.FullName);

            _doctorRepositoryMock.Verify(
                repository => repository.CreateAsync(
                    It.IsAny<Doctor>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _userManagerMock.Verify(
                manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    dto.Password),
                Times.Once);

            _userManagerMock.Verify(
                manager => manager.AddClaimAsync(
                    It.IsAny<IdentityUser>(),
                    It.IsAny<Claim>()),
                Times.Exactly(2));
        }

        [Fact]
        public async Task CreateDoctorAsync_WhenIdentityUserCreationFails_DeletesCreatedDoctorAndThrowsBadRequestException()
        {
            // Arrange
            AdminDoctorCreateDto dto = CreateAdminDoctorCreateDto();

            Doctor createdDoctor = new()
            {
                DoctorId = 12,
                FullName = dto.FullName,
                Specialisation = dto.Specialisation,
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = dto.IsActive
            };

            IdentityError identityError = new()
            {
                Description = "Password is invalid."
            };

            _userManagerMock
                .Setup(manager => manager.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser?)null);

            _doctorRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<Doctor>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdDoctor);

            _userManagerMock
                .Setup(manager => manager.CreateAsync(
                    It.IsAny<IdentityUser>(),
                    dto.Password))
                .ReturnsAsync(IdentityResult.Failed(identityError));

            _doctorRepositoryMock
                .Setup(repository => repository.DeleteAsync(
                    createdDoctor.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdDoctor);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() =>
                _adminService.CreateDoctorAsync(dto));

            _doctorRepositoryMock.Verify(
                repository => repository.DeleteAsync(
                    createdDoctor.DoctorId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenDoctorExists_ReturnsUpdatedDoctor()
        {
            // Arrange
            const int doctorId = 1;

            Doctor doctor = new()
            {
                DoctorId = doctorId,
                FullName = "Old Doctor",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 5,
                ConsultationFee = 300,
                IsActive = true
            };

            DoctorUpdateDto updateDto = new()
            {
                FullName = "Updated Doctor",
                Specialisation = Specialisation.GeneralMedicine,
                YearsOfExperience = 6,
                ConsultationFee = 400,
                IsActive = true
            };

            DoctorReadDto expectedDto = new()
            {
                DoctorId = doctorId,
                FullName = "Updated Doctor",
                Specialisation = Specialisation.GeneralMedicine,
                YearsOfExperience = 6,
                ConsultationFee = 400,
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(doctorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(mapper => mapper.Map(updateDto, doctor));

            _doctorRepositoryMock
                .Setup(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(mapper => mapper.Map<DoctorReadDto>(doctor))
                .Returns(expectedDto);

            // Act
            DoctorReadDto result =
                await _adminService.UpdateDoctorAsync(doctorId, updateDto);

            // Assert
            Assert.Equal(expectedDto.DoctorId, result.DoctorId);
            Assert.Equal(expectedDto.FullName, result.FullName);
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            const int doctorId = 404;

            DoctorUpdateDto updateDto = new()
            {
                FullName = "Updated Doctor",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 5,
                ConsultationFee = 300,
                IsActive = true
            };

            _doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(doctorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _adminService.UpdateDoctorAsync(doctorId, updateDto));
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenRecordExists_ReturnsUpdatedRecord()
        {
            // Arrange
            const int recordId = 1;

            HealthRecord healthRecord = new()
            {
                RecordId = recordId,
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1,
                VisitDate = DateTime.UtcNow.Date,
                Diagnosis = "Old Diagnosis",
                Prescription = "Old Prescription",
                Notes = "Old Notes"
            };

            HealthRecordUpdateDto updateDto = new()
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1,
                VisitDate = DateTime.UtcNow.Date,
                Diagnosis = "Updated Diagnosis",
                Prescription = "Updated Prescription",
                Notes = "Updated Notes"
            };

            HealthRecordReadDto expectedDto = new()
            {
                RecordId = recordId,
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1,
                VisitDate = updateDto.VisitDate,
                Diagnosis = updateDto.Diagnosis,
                Prescription = updateDto.Prescription,
                Notes = updateDto.Notes
            };

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(recordId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(healthRecord);

            _mapperMock
                .Setup(mapper => mapper.Map(updateDto, healthRecord));

            _healthRecordRepositoryMock
                .Setup(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(mapper => mapper.Map<HealthRecordReadDto>(healthRecord))
                .Returns(expectedDto);

            // Act
            HealthRecordReadDto result =
                await _adminService.UpdateHealthRecordAsync(recordId, updateDto);

            // Assert
            Assert.Equal(expectedDto.RecordId, result.RecordId);
            Assert.Equal(expectedDto.Diagnosis, result.Diagnosis);
        }

        [Fact]
        public async Task UpdateHealthRecordAsync_WhenRecordDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            const int recordId = 404;

            HealthRecordUpdateDto updateDto = new()
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1,
                VisitDate = DateTime.UtcNow.Date,
                Diagnosis = "Diagnosis",
                Prescription = "Prescription",
                Notes = "Notes"
            };

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetByIdAsync(recordId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HealthRecord?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _adminService.UpdateHealthRecordAsync(recordId, updateDto));
        }

        [Fact]
        public async Task GetAppointmentReportAsync_WhenAppointmentsExist_ReturnsGroupedReportByDate()
        {
            // Arrange
            DateTime firstDate = new(2026, 6, 14);
            DateTime secondDate = new(2026, 6, 15);

            List<Appointment> appointments = new()
            {
                new Appointment
                {
                    AppointmentId = 1,
                    ScheduledDate = firstDate,
                    Status = AppointmentStatus.Scheduled
                },
                new Appointment
                {
                    AppointmentId = 2,
                    ScheduledDate = firstDate,
                    Status = AppointmentStatus.Confirmed
                },
                new Appointment
                {
                    AppointmentId = 3,
                    ScheduledDate = firstDate,
                    Status = AppointmentStatus.Completed
                },
                new Appointment
                {
                    AppointmentId = 4,
                    ScheduledDate = secondDate,
                    Status = AppointmentStatus.Cancelled
                }
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            // Act
            List<AppointmentReportDto> result =
                await _adminService.GetAppointmentReportAsync();

            // Assert
            Assert.Equal(2, result.Count);

            AppointmentReportDto firstReport =
                result.Single(report => report.Date == firstDate.Date);

            Assert.Equal(3, firstReport.TotalCount);
            Assert.Equal(1, firstReport.ScheduledCount);
            Assert.Equal(1, firstReport.ConfirmedCount);
            Assert.Equal(1, firstReport.CompletedCount);
            Assert.Equal(0, firstReport.CancelledCount);

            AppointmentReportDto secondReport =
                result.Single(report => report.Date == secondDate.Date);

            Assert.Equal(1, secondReport.TotalCount);
            Assert.Equal(0, secondReport.ScheduledCount);
            Assert.Equal(0, secondReport.ConfirmedCount);
            Assert.Equal(0, secondReport.CompletedCount);
            Assert.Equal(1, secondReport.CancelledCount);
        }

        private static AdminDoctorCreateDto CreateAdminDoctorCreateDto()
        {
            return new AdminDoctorCreateDto
            {
                Email = "doctor@test.com",
                Password = "Doctor@123",
                ConfirmPassword = "Doctor@123",
                FullName = "Dr. Test",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };
        }

        private static Mock<UserManager<IdentityUser>> CreateUserManagerMock()
        {
            Mock<IUserStore<IdentityUser>> storeMock = new();

            return new Mock<UserManager<IdentityUser>>(
                storeMock.Object,
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                Array.Empty<IUserValidator<IdentityUser>>(),
                Array.Empty<IPasswordValidator<IdentityUser>>(),
                Mock.Of<ILookupNormalizer>(),
                new IdentityErrorDescriber(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());
        }
    }
}
