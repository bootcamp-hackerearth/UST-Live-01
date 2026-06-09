using AutoMapper;
using Moq;
using Xunit;

using HealthCare_Appointment_Portal.Services;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Enums;

using System.Collections.Generic;
using System.Threading.Tasks;

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
            var patients =
                new List<Patient>
                {
                    new Patient
                    {
                        PatientId = 1,
                        FullName = "Aniket"
                    }
                };

            var patientDtos =
                new List<PatientDto>
                {
                    new PatientDto
                    {
                        PatientId = 1,
                        FullName = "Aniket"
                    }
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Patients.GetAllAsync())
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<PatientDto>>(
                        patients))
                .Returns(patientDtos);

            var result =
                await _service
                    .GetAllPatientsAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task
            GetPatientByIdAsync_ValidId_ReturnsPatient()
        {
            var patient =
                new Patient
                {
                    PatientId = 1,
                    FullName = "Aniket"
                };

            var dto =
                new PatientDto
                {
                    PatientId = 1,
                    FullName = "Aniket"
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(x =>
                    x.Map<PatientDto>(patient))
                .Returns(dto);

            var result =
                await _service
                    .GetPatientByIdAsync(1);

            Assert.NotNull(result);

            Assert.Equal(
                1,
                result.PatientId);
        }

        [Fact]
        public async Task
            GetPatientByIdAsync_InvalidId_ThrowsException()
        {
            _unitOfWorkMock
                .Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync((Patient)null);

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
            var dto =
                new CreatePatientDto
                {
                    FullName = "Aniket",
                    Email = "aniket@gmail.com"
                };

            var patient =
                new Patient
                {
                    PatientId = 1,
                    FullName = "Aniket",
                    Email = "aniket@gmail.com"
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Patients
                     .GetPatientByEmailAsync(
                         dto.Email))
                .ReturnsAsync((Patient)null);

            _mapperMock
                .Setup(x =>
                    x.Map<Patient>(dto))
                .Returns(patient);

            var result =
                await _service
                    .AddPatientAsync(dto);

            Assert.Equal(
                1,
                result);
        }

        [Fact]
        public async Task
            AddPatientAsync_DuplicateEmail_ThrowsException()
        {
            var dto =
                new CreatePatientDto
                {
                    Email = "aniket@gmail.com"
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Patients
                     .GetPatientByEmailAsync(
                         dto.Email))
                .ReturnsAsync(
                    new Patient());

            await Assert.ThrowsAsync<
                DuplicatePatientException>(
                    () =>
                        _service
                            .AddPatientAsync(dto));
        }

        [Fact]
        public async Task
            UpdatePatientAsync_PatientNotFound_ThrowsException()
        {
            _unitOfWorkMock
                .Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync((Patient)null);

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
            _unitOfWorkMock
                .Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync((Patient)null);

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

            await _service
                .DeletePatientAsync(1);

            _unitOfWorkMock
                .Verify(x =>
                    x.Patients.DeleteAsync(1),
                    Times.Once);

            _unitOfWorkMock
                .Verify(x =>
                    x.CommitAsync(),
                    Times.Once);
        }
    }
}