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
        }

        [Fact]
        public async Task GetPatientByIdAsync_WhenNotFound_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        It.IsAny<int>()))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<
                PatientNotFoundException>(
                () => _service.GetPatientByIdAsync(1));
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
        }

        [Fact]
        public async Task GetPatientByEmailAsync_WhenNotFound_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x =>
                    x.GetPatientByEmailAsync(
                        It.IsAny<string>()))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<
                PatientNotFoundException>(
                () => _service.GetPatientByEmailAsync(
                    "abc@test.com"));
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
                x => x.AddAsync(
                    It.IsAny<Patient>()),
                Times.Once);

            _userRepositoryMock.Verify(
                x => x.AddAsync(
                    It.IsAny<User>()),
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

            await Assert.ThrowsAsync<
                DuplicatePatientException>(
                () => _service.AddPatientAsync(dto));
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
        public async Task UpdatePatientAsync_WhenNotFound_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<
                PatientNotFoundException>(
                () => _service.UpdatePatientAsync(
                    1,
                    new UpdatePatientDto()));
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

            await Assert.ThrowsAsync<
                DuplicatePatientException>(
                () => _service.UpdatePatientAsync(
                    1,
                    dto));
        }

        [Fact]
        public async Task DeletePatientAsync_WhenPatientNotFound_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<
                PatientNotFoundException>(
                () => _service.DeletePatientAsync(1));
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

            await Assert.ThrowsAsync<
                PatientDeletionException>(
                () => _service.DeletePatientAsync(1));
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
        }
    }
}