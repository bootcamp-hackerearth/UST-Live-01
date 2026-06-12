using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository>
            _patientRepositoryMock;

        private readonly Mock<IUserRepository>
            _userRepositoryMock;

        private readonly Mock<IAppointmentRepository>
            _appointmentRepositoryMock;

        private readonly Mock<IMapper>
            _mapperMock;

        private readonly PatientService
            _service;

        public PatientServiceTests()
        {
            _patientRepositoryMock =
                new Mock<IPatientRepository>();

            _userRepositoryMock =
                new Mock<IUserRepository>();

            _appointmentRepositoryMock =
                new Mock<IAppointmentRepository>();

            _mapperMock =
                new Mock<IMapper>();

            _service =
                new PatientService(
                    _patientRepositoryMock.Object,
                    _userRepositoryMock.Object,
                    _appointmentRepositoryMock.Object,
                    _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ReturnsPatients()
        {
            var patients =
                new List<Patient>
                {
                    new Patient(),
                    new Patient()
                };

            var patientDtos =
                new List<PatientDto>
                {
                    new PatientDto(),
                    new PatientDto()
                };

            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<PatientDto>>(patients))
                .Returns(patientDtos);

            var result =
                await _service.GetAllPatientsAsync();

            Assert.Equal(
                2,
                result.Count());

            _patientRepositoryMock.Verify(
                x => x.GetAllAsync(),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<IEnumerable<PatientDto>>(patients),
                Times.Once);
        }

        [Fact]
        public async Task GetAllPatientsAsync_WhenNoPatients_ReturnsEmptyList()
        {
            var patients =
                new List<Patient>();

            var patientDtos =
                new List<PatientDto>();

            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<PatientDto>>(patients))
                .Returns(patientDtos);

            var result =
                await _service.GetAllPatientsAsync();

            Assert.Empty(result);

            _patientRepositoryMock.Verify(
                x => x.GetAllAsync(),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<IEnumerable<PatientDto>>(patients),
                Times.Once);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ReturnsPatient()
        {
            var patient =
                new Patient
                {
                    PatientId = 1
                };

            var patientDto =
                new PatientDto();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(x =>
                    x.Map<PatientDto>(patient))
                .Returns(patientDto);

            var result =
                await _service.GetPatientByIdAsync(1);

            Assert.NotNull(result);

            _patientRepositoryMock.Verify(
                x => x.GetByIdAsync(1),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<PatientDto>(patient),
                Times.Once);
        }

        [Fact]
        public async Task GetPatientByIdAsync_WhenNotFound_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        It.IsAny<int>()))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<PatientNotFoundException>(
                () => _service.GetPatientByIdAsync(1));

            _mapperMock.Verify(
                x => x.Map<PatientDto>(
                    It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task GetPatientByEmailAsync_ReturnsPatient()
        {
            var patient =
                new Patient
                {
                    PatientId = 1,
                    Email = "test@test.com"
                };

            var patientDto =
                new PatientDto();

            _patientRepositoryMock
                .Setup(x =>
                    x.GetPatientByEmailAsync(
                        patient.Email))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(x =>
                    x.Map<PatientDto>(patient))
                .Returns(patientDto);

            var result =
                await _service.GetPatientByEmailAsync(
                    patient.Email);

            Assert.NotNull(result);

            _patientRepositoryMock.Verify(
                x => x.GetPatientByEmailAsync(patient.Email),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<PatientDto>(patient),
                Times.Once);
        }

        [Fact]
        public async Task GetPatientByEmailAsync_WhenNotFound_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x =>
                    x.GetPatientByEmailAsync(
                        It.IsAny<string>()))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<PatientNotFoundException>(
                () => _service.GetPatientByEmailAsync(
                    "abc@test.com"));

            _mapperMock.Verify(
                x => x.Map<PatientDto>(
                    It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task AddPatientAsync_ReturnsPatientId()
        {
            var dto =
                new CreatePatientDto
                {
                    FullName = "John",
                    Email = "john@test.com"
                };

            var patient =
                new Patient
                {
                    PatientId = 1,
                    Email = dto.Email
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetPatientByEmailAsync(
                        dto.Email))
                .ReturnsAsync((Patient)null!);

            _mapperMock
                .Setup(x =>
                    x.Map<Patient>(dto))
                .Returns(patient);

            _patientRepositoryMock
                .Setup(x =>
                    x.AddAsync(
                        It.IsAny<Patient>()))
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x =>
                    x.AddAsync(
                        It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            var result =
                await _service.AddPatientAsync(dto);

            Assert.Equal(
                1,
                result);

            _patientRepositoryMock.Verify(
                x => x.GetPatientByEmailAsync(dto.Email),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<Patient>(dto),
                Times.Once);

            _patientRepositoryMock.Verify(
                x => x.AddAsync(patient),
                Times.Once);

            _userRepositoryMock.Verify(
                x => x.AddAsync(
                    It.IsAny<User>()),
                Times.Once);
        }

        [Fact]
        public async Task AddPatientAsync_CreatesUserWithCorrectDetails()
        {
            var dto =
                new CreatePatientDto
                {
                    FullName = "John",
                    Email = "john@test.com"
                };

            var patient =
                new Patient
                {
                    PatientId = 12,
                    Email = dto.Email
                };

            User? createdUser = null;

            _patientRepositoryMock
                .Setup(x =>
                    x.GetPatientByEmailAsync(dto.Email))
                .ReturnsAsync((Patient)null!);

            _mapperMock
                .Setup(x =>
                    x.Map<Patient>(dto))
                .Returns(patient);

            _patientRepositoryMock
                .Setup(x =>
                    x.AddAsync(patient))
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x =>
                    x.AddAsync(It.IsAny<User>()))
                .Callback<User>(user =>
                    createdUser = user)
                .Returns(Task.CompletedTask);

            var result =
                await _service.AddPatientAsync(dto);

            Assert.Equal(
                patient.PatientId,
                result);

            Assert.NotNull(createdUser);

            Assert.Equal(
                "P012",
                createdUser!.UserCode);

            Assert.Equal(
                dto.Email,
                createdUser.Email);

            Assert.Equal(
                string.Empty,
                createdUser.PasswordHash);

            Assert.Equal(
                Role.Patient,
                createdUser.Role);

            Assert.Equal(
                patient.PatientId,
                createdUser.ReferenceId);

            _userRepositoryMock.Verify(
                x => x.AddAsync(
                    It.IsAny<User>()),
                Times.Once);
        }

        [Fact]
        public async Task AddPatientAsync_WhenPatientIdIsSingleDigit_CreatesPaddedUserCode()
        {
            var dto =
                new CreatePatientDto
                {
                    FullName = "Single Digit Patient",
                    Email = "single@test.com"
                };

            var patient =
                new Patient
                {
                    PatientId = 5,
                    Email = dto.Email
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetPatientByEmailAsync(dto.Email))
                .ReturnsAsync((Patient)null!);

            _mapperMock
                .Setup(x =>
                    x.Map<Patient>(dto))
                .Returns(patient);

            _patientRepositoryMock
                .Setup(x =>
                    x.AddAsync(patient))
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x =>
                    x.AddAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            await _service.AddPatientAsync(dto);

            _userRepositoryMock.Verify(
                x => x.AddAsync(
                    It.Is<User>(user =>
                        user.UserCode == "P005" &&
                        user.Email == dto.Email &&
                        user.PasswordHash == string.Empty &&
                        user.Role == Role.Patient &&
                        user.ReferenceId == 5)),
                Times.Once);
        }

        [Fact]
        public async Task AddPatientAsync_WhenEmailExists_ThrowsException()
        {
            var dto =
                new CreatePatientDto
                {
                    Email = "john@test.com"
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetPatientByEmailAsync(
                        dto.Email))
                .ReturnsAsync(
                    new Patient());

            await Assert.ThrowsAsync<DuplicatePatientException>(
                () => _service.AddPatientAsync(dto));

            _mapperMock.Verify(
                x => x.Map<Patient>(
                    It.IsAny<CreatePatientDto>()),
                Times.Never);

            _patientRepositoryMock.Verify(
                x => x.AddAsync(
                    It.IsAny<Patient>()),
                Times.Never);

            _userRepositoryMock.Verify(
                x => x.AddAsync(
                    It.IsAny<User>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdatePatientAsync_UpdatesSuccessfully()
        {
            var patient =
                new Patient
                {
                    PatientId = 1,
                    Email = "old@test.com"
                };

            var dto =
                new UpdatePatientDto
                {
                    Email = "new@test.com"
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x =>
                    x.GetPatientByEmailAsync(
                        dto.Email))
                .ReturnsAsync((Patient)null!);

            _patientRepositoryMock
                .Setup(x =>
                    x.UpdateAsync(patient))
                .Returns(Task.CompletedTask);

            await _service.UpdatePatientAsync(
                1,
                dto);

            _mapperMock.Verify(
                x => x.Map(
                    dto,
                    patient),
                Times.Once);

            _patientRepositoryMock.Verify(
                x => x.UpdateAsync(patient),
                Times.Once);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenSamePatientHasEmail_UpdatesSuccessfully()
        {
            var patient =
                new Patient
                {
                    PatientId = 1,
                    Email = "same@test.com"
                };

            var existingPatient =
                new Patient
                {
                    PatientId = 1,
                    Email = "same@test.com"
                };

            var dto =
                new UpdatePatientDto
                {
                    Email = "same@test.com"
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x =>
                    x.GetPatientByEmailAsync(dto.Email))
                .ReturnsAsync(existingPatient);

            _patientRepositoryMock
                .Setup(x =>
                    x.UpdateAsync(patient))
                .Returns(Task.CompletedTask);

            await _service.UpdatePatientAsync(
                1,
                dto);

            _mapperMock.Verify(
                x => x.Map(dto, patient),
                Times.Once);

            _patientRepositoryMock.Verify(
                x => x.UpdateAsync(patient),
                Times.Once);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenNotFound_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<PatientNotFoundException>(
                () => _service.UpdatePatientAsync(
                    1,
                    new UpdatePatientDto()));

            _patientRepositoryMock.Verify(
                x => x.GetPatientByEmailAsync(
                    It.IsAny<string>()),
                Times.Never);

            _mapperMock.Verify(
                x => x.Map(
                    It.IsAny<UpdatePatientDto>(),
                    It.IsAny<Patient>()),
                Times.Never);

            _patientRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenDuplicateEmailExists_ThrowsException()
        {
            var patient =
                new Patient
                {
                    PatientId = 1
                };

            var existing =
                new Patient
                {
                    PatientId = 2
                };

            var dto =
                new UpdatePatientDto
                {
                    Email = "duplicate@test.com"
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x =>
                    x.GetPatientByEmailAsync(
                        dto.Email))
                .ReturnsAsync(existing);

            await Assert.ThrowsAsync<DuplicatePatientException>(
                () => _service.UpdatePatientAsync(
                    1,
                    dto));

            _mapperMock.Verify(
                x => x.Map(
                    It.IsAny<UpdatePatientDto>(),
                    It.IsAny<Patient>()),
                Times.Never);

            _patientRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task DeletePatientAsync_WhenPatientNotFound_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<PatientNotFoundException>(
                () => _service.DeletePatientAsync(1));

            _appointmentRepositoryMock.Verify(
                x => x.GetAppointmentsByPatientAsync(
                    It.IsAny<int>()),
                Times.Never);

            _patientRepositoryMock.Verify(
                x => x.DeleteAsync(
                    It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task DeletePatientAsync_WhenConfirmedAppointmentExists_ThrowsException()
        {
            var patient =
                new Patient
                {
                    PatientId = 1
                };

            var appointments =
                new List<Appointment>
                {
                    new Appointment
                    {
                        Status =
                            AppointmentStatus.Confirmed
                    }
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(appointments);

            await Assert.ThrowsAsync<PatientDeletionException>(
                () => _service.DeletePatientAsync(1));

            _patientRepositoryMock.Verify(
                x => x.DeleteAsync(
                    It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task DeletePatientAsync_WhenConfirmedAndPendingAppointmentsExist_ThrowsException()
        {
            var patient =
                new Patient
                {
                    PatientId = 1
                };

            var appointments =
                new List<Appointment>
                {
                    new Appointment
                    {
                        Status =
                            AppointmentStatus.Pending
                    },
                    new Appointment
                    {
                        Status =
                            AppointmentStatus.Confirmed
                    }
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(appointments);

            await Assert.ThrowsAsync<PatientDeletionException>(
                () => _service.DeletePatientAsync(1));

            _patientRepositoryMock.Verify(
                x => x.DeleteAsync(
                    It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task DeletePatientAsync_DeletesSuccessfully()
        {
            var patient =
                new Patient
                {
                    PatientId = 1
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(
                    new List<Appointment>());

            _patientRepositoryMock
                .Setup(x =>
                    x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await _service.DeletePatientAsync(1);

            _appointmentRepositoryMock.Verify(
                x => x.GetAppointmentsByPatientAsync(1),
                Times.Once);

            _patientRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeletePatientAsync_WithPendingAppointments_DeletesPatient()
        {
            var patient =
                new Patient
                {
                    PatientId = 1
                };

            var appointments =
                new List<Appointment>
                {
                    new Appointment
                    {
                        Status =
                            AppointmentStatus.Pending
                    }
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(appointments);

            _patientRepositoryMock
                .Setup(x =>
                    x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await _service.DeletePatientAsync(1);

            _patientRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeletePatientAsync_WithCompletedAppointments_DeletesPatient()
        {
            var patient =
                new Patient
                {
                    PatientId = 1
                };

            var appointments =
                new List<Appointment>
                {
                    new Appointment
                    {
                        Status =
                            AppointmentStatus.Completed
                    }
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(appointments);

            _patientRepositoryMock
                .Setup(x =>
                    x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await _service.DeletePatientAsync(1);

            _patientRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeletePatientAsync_WithCancelledAppointments_DeletesPatient()
        {
            var patient =
                new Patient
                {
                    PatientId = 1
                };

            var appointments =
                new List<Appointment>
                {
                    new Appointment
                    {
                        Status =
                            AppointmentStatus.Cancelled
                    }
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(appointments);

            _patientRepositoryMock
                .Setup(x =>
                    x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await _service.DeletePatientAsync(1);

            _patientRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeletePatientAsync_WithPendingCompletedAndCancelledAppointments_DeletesPatient()
        {
            var patient =
                new Patient
                {
                    PatientId = 1
                };

            var appointments =
                new List<Appointment>
                {
                    new Appointment
                    {
                        Status =
                            AppointmentStatus.Pending
                    },
                    new Appointment
                    {
                        Status =
                            AppointmentStatus.Completed
                    },
                    new Appointment
                    {
                        Status =
                            AppointmentStatus.Cancelled
                    }
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(appointments);

            _patientRepositoryMock
                .Setup(x =>
                    x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await _service.DeletePatientAsync(1);

            _patientRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Theory]
        [InlineData(InsuranceStatus.Active)]
        [InlineData(InsuranceStatus.Expired)]
        [InlineData(InsuranceStatus.Pending)]
        public async Task GetPatientsByInsuranceStatusAsync_ReturnsPatients(
            InsuranceStatus status)
        {
            var patients =
                new List<Patient>
                {
                    new Patient()
                };

            var patientDtos =
                new List<PatientDto>
                {
                    new PatientDto()
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetPatientsByInsuranceStatusAsync(
                        status))
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<PatientDto>>(
                        patients))
                .Returns(patientDtos);

            var result =
                await _service
                    .GetPatientsByInsuranceStatusAsync(
                        status);

            Assert.Single(result);

            _patientRepositoryMock.Verify(
                x => x.GetPatientsByInsuranceStatusAsync(status),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<IEnumerable<PatientDto>>(patients),
                Times.Once);
        }

        [Fact]
        public async Task GetPatientsByInsuranceStatusAsync_WhenNoPatientsFound_ReturnsEmptyList()
        {
            var patients =
                new List<Patient>();

            var patientDtos =
                new List<PatientDto>();

            _patientRepositoryMock
                .Setup(x =>
                    x.GetPatientsByInsuranceStatusAsync(
                        InsuranceStatus.Active))
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<PatientDto>>(
                        patients))
                .Returns(patientDtos);

            var result =
                await _service
                    .GetPatientsByInsuranceStatusAsync(
                        InsuranceStatus.Active);

            Assert.Empty(result);

            _patientRepositoryMock.Verify(
                x => x.GetPatientsByInsuranceStatusAsync(
                    InsuranceStatus.Active),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<IEnumerable<PatientDto>>(
                    patients),
                Times.Once);
        }
    }
}