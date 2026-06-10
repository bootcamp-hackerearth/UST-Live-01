using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Moq;

namespace HealthCare_Appointment_Portal.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IUnitOfWork>
            _unitOfWorkMock;

        private readonly Mock<IMapper>
            _mapperMock;

        private readonly PatientService
            _service;

        public PatientServiceTests()
        {
            _unitOfWorkMock =
                new Mock<IUnitOfWork>();

            _mapperMock =
                new Mock<IMapper>();

            _service =
                new PatientService(
                    _unitOfWorkMock.Object,
                    _mapperMock.Object);
        }

        [Fact]
        public async Task
            GetAllPatientsAsync_ReturnsPatients()
        {
            // Arrange

            var patients =
                new List<Patient>
                {
                    new Patient
                    {
                        PatientId = 1,
                        FullName = "Vyshnavi"
                    }
                };

            var patientDtos =
                new List<PatientDto>
                {
                    new PatientDto
                    {
                        PatientId = 1,
                        FullName = "Vyshnavi"
                    }
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Patients.GetAllAsync())
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<PatientDto>>(patients))
                .Returns(patientDtos);

            // Act

            var result =
                await _service
                    .GetAllPatientsAsync();

            // Assert

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task
            GetPatientByIdAsync_ValidId_ReturnsPatient()
        {
            // Arrange

            var patient =
                new Patient
                {
                    PatientId = 1,
                    FullName = "Vyshnavi"
                };

            var patientDto =
                new PatientDto
                {
                    PatientId = 1,
                    FullName = "Vyshnavi"
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(x =>
                    x.Map<PatientDto>(patient))
                .Returns(patientDto);

            // Act

            var result =
                await _service
                    .GetPatientByIdAsync(1);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                1,
                result.PatientId);

            Assert.Equal(
                "Vyshnavi",
                result.FullName);
        }

        [Fact]
        public async Task
            GetPatientByIdAsync_InvalidId_ThrowsException()
        {
            // Arrange

            _unitOfWorkMock
                .Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync((Patient)null);

            // Act & Assert

            await Assert.ThrowsAsync<
                PatientNotFoundException>(
                    () =>
                        _service
                            .GetPatientByIdAsync(1));
        }

        [Fact]
        public async Task
            AddPatientAsync_ValidPatient_ReturnsId()
        {
            // Arrange

            var createDto =
                new CreatePatientDto
                {
                    FullName = "Vyshnavi",
                    Email = "vyshu@gmail.com"
                };

            var patient =
                new Patient
                {
                    PatientId = 1,
                    FullName = "Vyshnavi",
                    Email = "vyshu@gmail.com"
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Patients
                     .GetPatientByEmailAsync(
                         createDto.Email))
                .ReturnsAsync((Patient)null);

            _mapperMock
                .Setup(x =>
                    x.Map<Patient>(
                        It.IsAny<CreatePatientDto>()))
                .Returns(patient);

            _unitOfWorkMock
                .Setup(x =>
                    x.Patients.AddAsync(
                        It.IsAny<Patient>()))
                .Returns(Task.CompletedTask);

            // Act

            var result =
                await _service
                    .AddPatientAsync(createDto);

            // Assert

            Assert.True(result >= 0);

            _unitOfWorkMock.Verify(
                x => x.Patients.AddAsync(
                    It.IsAny<Patient>()),
                Times.Once);
        }

        [Fact]
        public async Task
            AddPatientAsync_DuplicateEmail_ThrowsException()
        {
            // Arrange

            var createDto =
                new CreatePatientDto
                {
                    Email = "vyshu@gmail.com"
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Patients
                     .GetPatientByEmailAsync(
                         createDto.Email))
                .ReturnsAsync(
                    new Patient());

            // Act & Assert

            await Assert.ThrowsAsync<
                DuplicatePatientException>(
                    () =>
                        _service
                            .AddPatientAsync(createDto));
        }

        [Fact]
        public async Task
            UpdatePatientAsync_PatientNotFound_ThrowsException()
        {
            // Arrange

            _unitOfWorkMock
                .Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync((Patient)null);

            // Act & Assert

            await Assert.ThrowsAsync<
                PatientNotFoundException>(
                    () =>
                        _service
                            .UpdatePatientAsync(
                                1,
                                new UpdatePatientDto()));
        }

        [Fact]
        public async Task
            DeletePatientAsync_NotFound_ThrowsException()
        {
            // Arrange

            _unitOfWorkMock
                .Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync((Patient)null);

            // Act & Assert

            await Assert.ThrowsAsync<
                PatientNotFoundException>(
                    () =>
                        _service
                            .DeletePatientAsync(1));
        }

        [Fact]
        public async Task
            DeletePatientAsync_WithConfirmedAppointment_ThrowsException()
        {
            // Arrange

            var patient =
                new Patient
                {
                    PatientId = 1
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _unitOfWorkMock
                .Setup(x =>
                    x.Appointments
                     .GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(
                    new List<Appointment>
                    {
                        new Appointment
                        {
                            Status =
                                AppointmentStatus
                                .Confirmed
                        }
                    });

            // Act & Assert

            await Assert.ThrowsAsync<
                PatientDeletionException>(
                    () =>
                        _service
                            .DeletePatientAsync(1));
        }

        [Fact]
        public async Task
            DeletePatientAsync_ValidPatient_DeletesSuccessfully()
        {
            // Arrange

            var patient =
                new Patient
                {
                    PatientId = 1
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _unitOfWorkMock
                .Setup(x =>
                    x.Appointments
                     .GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(
                    new List<Appointment>());

            // Act

            await _service
                .DeletePatientAsync(1);

            // Assert

            _unitOfWorkMock.Verify(
                x => x.Patients.DeleteAsync(1),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }
    }
}

