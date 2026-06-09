using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Tests.Services
{
    [TestClass]
    public class PatientServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;

        private Mock<IMapper> _mapperMock;

        private Mock<IPatientRepository> _patientRepositoryMock;

        private PatientService _service;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock =
                new Mock<IUnitOfWork>();

            _mapperMock =
                new Mock<IMapper>();

            _patientRepositoryMock =
                new Mock<IPatientRepository>();

            _unitOfWorkMock
                .Setup(x => x.Patients)
                .Returns(_patientRepositoryMock.Object);

            _service =
                new PatientService(
                    _unitOfWorkMock.Object,
                    _mapperMock.Object);
        }

        [TestMethod]
        public async Task GetPatientByIdAsync_ExistingId_ShouldReturnPatient()
        {
            // Arrange

            Patient patient = new Patient
            {
                PatientId = 1,
                FullName = "Anand"
            };

            PatientDto patientDto = new PatientDto
            {
                PatientId = 1,
                FullName = "Anand"
            };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(x =>
                    x.Map<PatientDto>(patient))
                .Returns(patientDto);

            // Act

            PatientDto result =
                await _service
                    .GetPatientByIdAsync(1);

            // Assert

            Assert.IsNotNull(result);

            Assert.AreEqual(
                1,
                result.PatientId);

            Assert.AreEqual(
                "Anand",
                result.FullName);
        }

        [TestMethod]
        public async Task GetPatientByIdAsync_InvalidId_ShouldThrowPatientNotFoundException()
        {
            // Arrange

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(100))
                .ReturnsAsync((Patient)null);

            // Act + Assert

            await Assert.ThrowsExceptionAsync<PatientNotFoundException>(
                async () =>
                {
                    await _service
                        .GetPatientByIdAsync(100);
                });
        }

        [TestMethod]
        public async Task GetPatientByEmailAsync_InvalidEmail_ShouldThrowPatientNotFoundException()
        {
            // Arrange

            _patientRepositoryMock
                .Setup(x => x.GetPatientByEmailAsync("test@test.com"))
                .ReturnsAsync((Patient)null);

            // Act + Assert

            await Assert.ThrowsExceptionAsync<PatientNotFoundException>(
                async () =>
                {
                    await _service
                        .GetPatientByEmailAsync("test@test.com");
                });
        }

        [TestMethod]
        public async Task AddPatientAsync_DuplicateEmail_ShouldThrowDuplicatePatientException()
        {
            // Arrange

            CreatePatientDto dto = new CreatePatientDto
            {
                Email = "anand@gmail.com"
            };

            _patientRepositoryMock
                .Setup(x => x.GetPatientByEmailAsync(dto.Email))
                .ReturnsAsync(new Patient());

            // Act + Assert

            await Assert.ThrowsExceptionAsync<DuplicatePatientException>(
                async () =>
                {
                    await _service.AddPatientAsync(dto);
                });
        }

        [TestMethod]
        public async Task GetAllPatientsAsync_ShouldReturnPatients()
        {
            // Arrange

            var patients = new List<Patient>
    {
        new Patient { PatientId = 1, FullName = "Anand" }
    };

            var patientDtos = new List<PatientDto>
    {
        new PatientDto { PatientId = 1, FullName = "Anand" }
    };

            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<PatientDto>>(patients))
                .Returns(patientDtos);

            // Act

            var result =
                await _service.GetAllPatientsAsync();

            // Assert

            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public async Task GetPatientByEmailAsync_ExistingEmail_ShouldReturnPatient()
        {
            // Arrange

            Patient patient = new Patient
            {
                PatientId = 1,
                Email = "anand@gmail.com"
            };

            PatientDto dto = new PatientDto
            {
                PatientId = 1
            };

            _patientRepositoryMock
                .Setup(x => x.GetPatientByEmailAsync(patient.Email))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(x => x.Map<PatientDto>(patient))
                .Returns(dto);

            // Act

            var result =
                await _service.GetPatientByEmailAsync(patient.Email);

            // Assert

            Assert.AreEqual(1, result.PatientId);
        }



        [TestMethod]
        public async Task UpdatePatientAsync_ValidPatient_ShouldUpdatePatient()
        {
            // Arrange

            var patient = new Patient
            {
                PatientId = 1,
                Email = "old@gmail.com"
            };

            var dto = new UpdatePatientDto
            {
                Email = "new@gmail.com"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x => x.GetPatientByEmailAsync(dto.Email))
                .ReturnsAsync((Patient)null);

            // Act

            await _service.UpdatePatientAsync(1, dto);

            // Assert

            _patientRepositoryMock.Verify(
                x => x.UpdateAsync(patient),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [TestMethod]
        public async Task UpdatePatientAsync_InvalidPatient_ShouldThrowPatientNotFoundException()
        {
            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Patient)null);

            await Assert.ThrowsExceptionAsync<PatientNotFoundException>(
                () => _service.UpdatePatientAsync(
                    1,
                    new UpdatePatientDto()));
        }

        [TestMethod]
        public async Task UpdatePatientAsync_DuplicateEmail_ShouldThrowDuplicatePatientException()
        {
            var patient = new Patient
            {
                PatientId = 1
            };

            var existing = new Patient
            {
                PatientId = 2
            };

            var dto = new UpdatePatientDto
            {
                Email = "duplicate@gmail.com"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x => x.GetPatientByEmailAsync(dto.Email))
                .ReturnsAsync(existing);

            await Assert.ThrowsExceptionAsync<DuplicatePatientException>(
                () => _service.UpdatePatientAsync(1, dto));
        }

        [TestMethod]
        public async Task DeletePatientAsync_InvalidPatient_ShouldThrowPatientNotFoundException()
        {
            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Patient)null);

            await Assert.ThrowsExceptionAsync<PatientNotFoundException>(
                () => _service.DeletePatientAsync(1));
        }
       

        [TestMethod]
        public async Task GetPatientsByInsuranceStatusAsync_ShouldReturnPatients()
        {
            var patients = new List<Patient>
    {
        new Patient { PatientId = 1 }
    };

            var dtos = new List<PatientDto>
    {
        new PatientDto { PatientId = 1 }
    };

            _patientRepositoryMock
                .Setup(x => x.GetPatientsByInsuranceStatusAsync(
                    InsuranceStatus.Active))
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<PatientDto>>(patients))
                .Returns(dtos);

            var result =
                await _service.GetPatientsByInsuranceStatusAsync(
                    InsuranceStatus.Active);

            Assert.AreEqual(1, result.Count());
        }
    }
}