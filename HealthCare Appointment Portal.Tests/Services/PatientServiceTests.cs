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
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _service = new PatientService(
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ReturnsPatients()
        {
            var patients = new List<Patient>
            {
                new Patient(),
                new Patient()
            };

            var patientDtos = new List<PatientDto>
            {
                new PatientDto(),
                new PatientDto()
            };

            _unitOfWorkMock.Setup(x => x.Patients.GetAllAsync())
                .ReturnsAsync(patients);

            _mapperMock.Setup(x =>
                    x.Map<IEnumerable<PatientDto>>(patients))
                .Returns(patientDtos);

            var result = await _service.GetAllPatientsAsync();

            Assert.Equal(2, result.Count());

            _unitOfWorkMock.Verify(
                x => x.Patients.GetAllAsync(),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<IEnumerable<PatientDto>>(patients),
                Times.Once);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ReturnsPatient()
        {
            var patient = new Patient
            {
                PatientId = 1
            };

            var dto = new PatientDto();

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _mapperMock.Setup(x =>
                    x.Map<PatientDto>(patient))
                .Returns(dto);

            var result = await _service.GetPatientByIdAsync(1);

            Assert.NotNull(result);

            _mapperMock.Verify(
                x => x.Map<PatientDto>(patient),
                Times.Once);
        }

        [Fact]
        public async Task GetPatientByIdAsync_WhenNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<PatientNotFoundException>(
                () => _service.GetPatientByIdAsync(1));
        }

        [Fact]
        public async Task GetPatientByEmailAsync_ReturnsPatient()
        {
            var patient = new Patient();
            var dto = new PatientDto();

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetPatientByEmailAsync("test@test.com"))
                .ReturnsAsync(patient);

            _mapperMock.Setup(x =>
                    x.Map<PatientDto>(patient))
                .Returns(dto);

            var result =
                await _service.GetPatientByEmailAsync(
                    "test@test.com");

            Assert.NotNull(result);

            _mapperMock.Verify(
                x => x.Map<PatientDto>(patient),
                Times.Once);
        }

        [Fact]
        public async Task GetPatientByEmailAsync_WhenNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetPatientByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<PatientNotFoundException>(
                () => _service.GetPatientByEmailAsync("test@test.com"));
        }

        [Fact]
        public async Task AddPatientAsync_ReturnsPatientId()
        {
            var dto = new CreatePatientDto
            {
                Email = "patient@test.com"
            };

            var patient = new Patient
            {
                PatientId = 1,
                Email = "patient@test.com"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetPatientByEmailAsync(dto.Email))
                .ReturnsAsync((Patient?)null);

            _mapperMock.Setup(x =>
                    x.Map<Patient>(dto))
                .Returns(patient);

            _unitOfWorkMock.Setup(x =>
                    x.Patients.AddAsync(It.IsAny<Patient>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x =>
                    x.Users.AddAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            var result =
                await _service.AddPatientAsync(dto);

            Assert.Equal(1, result);

            _unitOfWorkMock.Verify(
                x => x.Patients.AddAsync(It.IsAny<Patient>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Exactly(2));

            _mapperMock.Verify(
                x => x.Map<Patient>(dto),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.Users.AddAsync(It.Is<User>(
                    u =>
                        u.UserCode == "P001" &&
                        u.Email == "patient@test.com" &&
                        u.Role == Role.Patient &&
                        u.ReferenceId == 1)),
                Times.Once);
        }

        [Fact]
        public async Task AddPatientAsync_DuplicatePatient_ThrowsException()
        {
            var dto = new CreatePatientDto
            {
                Email = "patient@test.com"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetPatientByEmailAsync(dto.Email))
                .ReturnsAsync(new Patient());

            await Assert.ThrowsAsync<DuplicatePatientException>(
                () => _service.AddPatientAsync(dto));
        }

        [Fact]
        public async Task UpdatePatientAsync_UpdatesSuccessfully()
        {
            var patient = new Patient
            {
                PatientId = 1
            };

            var dto = new UpdatePatientDto
            {
                Email = "updated@test.com"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetPatientByEmailAsync(dto.Email))
                .ReturnsAsync((Patient?)null);

            _unitOfWorkMock.Setup(x =>
                    x.Patients.UpdateAsync(patient))
                .Returns(Task.CompletedTask);

            await _service.UpdatePatientAsync(1, dto);

            _mapperMock.Verify(
                x => x.Map(dto, patient),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.Patients.UpdateAsync(patient),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenPatientNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<PatientNotFoundException>(
                () => _service.UpdatePatientAsync(
                    1,
                    new UpdatePatientDto()));
        }

        [Fact]
        public async Task UpdatePatientAsync_DuplicateEmail_ThrowsException()
        {
            var patient = new Patient { PatientId = 1 };

            var existingPatient = new Patient
            {
                PatientId = 2
            };

            var dto = new UpdatePatientDto
            {
                Email = "duplicate@test.com"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetPatientByEmailAsync(dto.Email))
                .ReturnsAsync(existingPatient);

            await Assert.ThrowsAsync<DuplicatePatientException>(
                () => _service.UpdatePatientAsync(1, dto));
        }

        [Fact]
        public async Task UpdatePatientAsync_SameEmailSamePatient_UpdatesSuccessfully()
        {
            var patient = new Patient
            {
                PatientId = 1,
                Email = "test@test.com"
            };

            var dto = new UpdatePatientDto
            {
                Email = "test@test.com"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetPatientByEmailAsync(dto.Email))
                .ReturnsAsync(patient);

            _unitOfWorkMock.Setup(x =>
                    x.Patients.UpdateAsync(patient))
                .Returns(Task.CompletedTask);

            await _service.UpdatePatientAsync(1, dto);

            _unitOfWorkMock.Verify(
                x => x.Patients.UpdateAsync(patient),
                Times.Once);
        }

        [Fact]
        public async Task DeletePatientAsync_WhenPatientNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<PatientNotFoundException>(
                () => _service.DeletePatientAsync(1));
        }

        [Fact]
        public async Task DeletePatientAsync_WhenConfirmedAppointmentExists_ThrowsException()
        {
            var patient = new Patient { PatientId = 1 };

            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    Status = AppointmentStatus.Confirmed
                }
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(appointments);

            await Assert.ThrowsAsync<PatientDeletionException>(
                () => _service.DeletePatientAsync(1));
        }

        [Fact]
        public async Task DeletePatientAsync_DeletesSuccessfully()
        {
            var patient = new Patient { PatientId = 1 };

            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    Status = AppointmentStatus.Pending
                }
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(appointments);

            _unitOfWorkMock.Setup(x =>
                    x.Patients.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await _service.DeletePatientAsync(1);

            _unitOfWorkMock.Verify(
                x => x.Patients.DeleteAsync(1),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task DeletePatientAsync_NoAppointments_DeletesSuccessfully()
        {
            var patient = new Patient
            {
                PatientId = 1
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(new List<Appointment>());

            _unitOfWorkMock.Setup(x =>
                    x.Patients.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await _service.DeletePatientAsync(1);

            _unitOfWorkMock.Verify(
                x => x.Patients.DeleteAsync(1),
                Times.Once);
        }

        [Theory]
        [InlineData(InsuranceStatus.Active)]
        [InlineData(InsuranceStatus.Expired)]
        [InlineData(InsuranceStatus.Suspended)]
        [InlineData(InsuranceStatus.Pending)]
        public async Task GetPatientsByInsuranceStatusAsync_ReturnsPatients(
            InsuranceStatus status)
        {
            var patients = new List<Patient>
            {
                new Patient()
            };

            var patientDtos = new List<PatientDto>
            {
                new PatientDto()
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetPatientsByInsuranceStatusAsync(status))
                .ReturnsAsync(patients);

            _mapperMock.Setup(x =>
                    x.Map<IEnumerable<PatientDto>>(patients))
                .Returns(patientDtos);

            var result =
                await _service.GetPatientsByInsuranceStatusAsync(status);

            Assert.Single(result);

            _unitOfWorkMock.Verify(
                x => x.Patients.GetPatientsByInsuranceStatusAsync(status),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<IEnumerable<PatientDto>>(patients),
                Times.Once);
        }
    }
}