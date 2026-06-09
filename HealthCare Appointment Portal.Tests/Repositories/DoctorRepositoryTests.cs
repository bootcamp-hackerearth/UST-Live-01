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
    public class DoctorRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _contextMock;
        private readonly Mock<DbSet<Doctor>> _doctorDbSetMock;
        private readonly DoctorRepository _repository;

        public DoctorRepositoryTests()
        {
            _contextMock =
                new Mock<ApplicationDbContext>();

            _doctorDbSetMock =
                new Mock<DbSet<Doctor>>();

            _contextMock
                .Setup(c => c.Doctors)
                .Returns(_doctorDbSetMock.Object);

            _repository =
                new DoctorRepository(
                    _contextMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsDoctor_WhenDoctorExists()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Doctor One"
                };

            _doctorDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync(doctor);

            var result =
                await _repository.GetByIdAsync(1);

            Assert.NotNull(result);

            Assert.Equal(
                1,
                result.DoctorId);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenDoctorDoesNotExist()
        {
            _doctorDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync((Doctor)null!);

            var result =
                await _repository.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllDoctors()
        {
            var doctors =
                new List<Doctor>
                {
                    new Doctor
                    {
                        DoctorId = 1,
                        FullName = "Doctor One"
                    },
                    new Doctor
                    {
                        DoctorId = 2,
                        FullName = "Doctor Two"
                    }
                };

            var mockSet =
                DbSetMockHelper
                    .CreateMockDbSet(
                        doctors);

            _contextMock
                .Setup(c => c.Doctors)
                .Returns(mockSet.Object);

            var repository =
                new DoctorRepository(
                    _contextMock.Object);

            var result =
                await repository.GetAllAsync();

            Assert.Equal(
                2,
                result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsDoctorToDbSet()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1
                };

            await _repository.AddAsync(
                doctor);

            _doctorDbSetMock.Verify(
                d => d.Add(doctor),
                Times.Once);
        }
 
        [Fact]
        public async Task DeleteAsync_RemovesDoctor_WhenDoctorExists()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1
                };

            _doctorDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync(doctor);

            await _repository.DeleteAsync(1);

            _doctorDbSetMock.Verify(
                d => d.Remove(doctor),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DoesNothing_WhenDoctorNotFound()
        {
            _doctorDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync((Doctor)null!);

            await _repository.DeleteAsync(1);

            _doctorDbSetMock.Verify(
                d => d.Remove(
                    It.IsAny<Doctor>()),
                Times.Never);
        }

        [Fact]
        public async Task GetDoctorsBySpecialisationAsync_ReturnsMatchingDoctors()
        {
            var doctors =
                new List<Doctor>
                {
                    new Doctor
                    {
                        DoctorId = 1,
                        Specialisation =
                            Specialisation.Cardiology
                    },
                    new Doctor
                    {
                        DoctorId = 2,
                        Specialisation =
                            Specialisation.Dermatology
                    }
                };

            var mockSet =
                DbSetMockHelper
                    .CreateMockDbSet(
                        doctors);

            _contextMock
                .Setup(c => c.Doctors)
                .Returns(mockSet.Object);

            var repository =
                new DoctorRepository(
                    _contextMock.Object);

            var result =
                await repository
                    .GetDoctorsBySpecialisationAsync(
                        Specialisation.Cardiology);

            Assert.Single(result);

            Assert.Equal(
                1,
                result.First().DoctorId);
        }

        [Fact]
        public async Task GetDoctorsBySpecialisationAsync_ReturnsEmpty_WhenNoDoctorMatches()
        {
            var doctors =
                new List<Doctor>
                {
                    new Doctor
                    {
                        DoctorId = 1,
                        Specialisation =
                            Specialisation.Dermatology
                    }
                };

            var mockSet =
                DbSetMockHelper
                    .CreateMockDbSet(
                        doctors);

            _contextMock
                .Setup(c => c.Doctors)
                .Returns(mockSet.Object);

            var repository =
                new DoctorRepository(
                    _contextMock.Object);

            var result =
                await repository
                    .GetDoctorsBySpecialisationAsync(
                        Specialisation.Cardiology);

            Assert.Empty(result);
        }
    }
}