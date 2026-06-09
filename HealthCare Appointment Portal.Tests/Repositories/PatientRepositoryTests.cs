using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Repositories;
using HealthCare_Appointment_Portal.Tests.Helpers;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Repositories
{
    public class PatientRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _contextMock;
        private readonly Mock<DbSet<Patient>> _patientDbSetMock;
        private readonly PatientRepository _repository;

        public PatientRepositoryTests()
        {
            _contextMock = new Mock<ApplicationDbContext>();

            _patientDbSetMock =
                new Mock<DbSet<Patient>>();

            _contextMock
                .Setup(c => c.Patients)
                .Returns(_patientDbSetMock.Object);

            _repository =
                new PatientRepository(
                    _contextMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsPatient_WhenPatientExists()
        {
            // Arrange
            var patient =
                new Patient
                {
                    PatientId = 1,
                    FullName = "John Doe"
                };

            _patientDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync(patient);

            // Act
            var result =
                await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(
                1,
                result.PatientId);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenPatientDoesNotExist()
        {
            // Arrange
            _patientDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync((Patient)null!);

            // Act
            var result =
                await _repository.GetByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllPatients()
        {
            // Arrange
            var patients =
                new List<Patient>
                {
                    new Patient
                    {
                        PatientId = 1,
                        FullName = "John"
                    },
                    new Patient
                    {
                        PatientId = 2,
                        FullName = "Jane"
                    }
                };

            var mockSet =
                DbSetMockHelper
                    .CreateMockDbSet(
                        patients);

            _contextMock
                .Setup(c => c.Patients)
                .Returns(mockSet.Object);

            var repository =
                new PatientRepository(
                    _contextMock.Object);

            // Act
            var result =
                await repository.GetAllAsync();

            // Assert
            Assert.Equal(
                2,
                result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsPatientToDbSet()
        {
            // Arrange
            var patient =
                new Patient
                {
                    PatientId = 1,
                    FullName = "John Doe"
                };

            // Act
            await _repository.AddAsync(
                patient);

            // Assert
            _patientDbSetMock.Verify(
                d => d.Add(patient),
                Times.Once);
        }


        [Fact]
        public async Task DeleteAsync_RemovesPatient_WhenPatientExists()
        {
            // Arrange
            var patient =
                new Patient
                {
                    PatientId = 1
                };

            _patientDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync(patient);

            // Act
            await _repository.DeleteAsync(1);

            // Assert
            _patientDbSetMock.Verify(
                d => d.Remove(patient),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DoesNothing_WhenPatientNotFound()
        {
            // Arrange
            _patientDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync((Patient)null!);

            // Act
            await _repository.DeleteAsync(1);

            // Assert
            _patientDbSetMock.Verify(
                d => d.Remove(
                    It.IsAny<Patient>()),
                Times.Never);
        }

        [Fact]
        public async Task GetPatientByEmailAsync_ReturnsPatient_WhenEmailExists()
        {
            // Arrange
            var patients =
                new List<Patient>
                {
                    new Patient
                    {
                        PatientId = 1,
                        Email = "john@test.com"
                    },
                    new Patient
                    {
                        PatientId = 2,
                        Email = "jane@test.com"
                    }
                };

            var mockSet =
                DbSetMockHelper
                    .CreateMockDbSet(
                        patients);

            _contextMock
                .Setup(c => c.Patients)
                .Returns(mockSet.Object);

            var repository =
                new PatientRepository(
                    _contextMock.Object);

            // Act
            var result =
                await repository
                    .GetPatientByEmailAsync(
                        "john@test.com");

            // Assert
            Assert.NotNull(result);

            Assert.Equal(
                1,
                result.PatientId);
        }

        [Fact]
        public async Task GetPatientByEmailAsync_ReturnsNull_WhenEmailDoesNotExist()
        {
            // Arrange
            var patients =
                new List<Patient>
                {
                    new Patient
                    {
                        PatientId = 1,
                        Email = "john@test.com"
                    }
                };

            var mockSet =
                DbSetMockHelper
                    .CreateMockDbSet(
                        patients);

            _contextMock
                .Setup(c => c.Patients)
                .Returns(mockSet.Object);

            var repository =
                new PatientRepository(
                    _contextMock.Object);

            // Act
            var result =
                await repository
                    .GetPatientByEmailAsync(
                        "unknown@test.com");

            // Assert
            Assert.Null(result);
        }
    }
}