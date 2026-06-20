using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.DTOs;
using HealthCare.Api.DTOs.Patient;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace HealthCare.Api.Tests
{
    public class PatientServiceTests
    {
        private readonly Mock<IRepository<Patient>> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly HealthCareDbContext _context;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _repoMock = new Mock<IRepository<Patient>>();
            _mapperMock = new Mock<IMapper>();

            var options = new DbContextOptionsBuilder<HealthCareDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new HealthCareDbContext(options);

            _service = new PatientService(
                _repoMock.Object,
                _context,
                _mapperMock.Object
            );
        }


        //  GetById Success
        [Fact]
        public async Task GetByIdAsync_ShouldReturnPatient_WhenExists()
        {
            var patient = new Patient { PatientId = 1 };
            var dto = new PatientListDto();

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(patient);
            _mapperMock.Setup(m => m.Map<PatientListDto>(patient)).Returns(dto);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
        }
        //  GetById Not Found
        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync((Patient)null);

            await Assert.ThrowsAsync<PatientNotFoundException>(() =>
                _service.GetByIdAsync(1));
        }

        //  Add
        [Fact]
        public async Task AddAsync_ShouldAddPatient()
        {
            var dto = new CreatePatientDto
            {
                FullName = "Test User"
            };

            await _service.AddAsync(dto);

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Patient>()), Times.Once);
        }

        //  Update
        [Fact]
        public async Task UpdateAsync_ShouldUpdatePatient()
        {
            var patient = new Patient { PatientId = 1 };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(patient);

            await _service.UpdateAsync(1, new UpdatePatientDto
            {
                FullName = "Updated"
            });

            _repoMock.Verify(r => r.UpdateAsync(patient), Times.Once);
        }

        //  Update Not Found
        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync((Patient)null);


            await Assert.ThrowsAsync<PatientNotFoundException>(() =>
                _service.UpdateAsync(1, new UpdatePatientDto()));

        }

        //  Delete
        [Fact]
        public async Task DeleteAsync_ShouldDeletePatient()
        {
            var patient = new Patient { PatientId = 1 };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(patient);

            await _service.DeleteAsync(1);

            _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        //  Delete Not Found
        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync((Patient)null);


            await Assert.ThrowsAsync<PatientNotFoundException>(() =>
                _service.DeleteAsync(1));

        }

        //  Search (uses DB, not repo)
        [Fact]
        public async Task SearchByNameAsync_ShouldReturnMatchingPatients()
        {
            _context.Patients.Add(new Patient { FullName = "Sam" , Gender = "Male" });
            _context.Patients.Add(new Patient { FullName = "Sanjay", Gender = "Female" });
            await _context.SaveChangesAsync();

            var patients = await _context.Patients.ToListAsync();
            Assert.Equal(2, patients.Count);

        }
        //userId
        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnPatient()
        {
            var patient = new Patient { UserId = "user1", FullName = "Sam" };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            _mapperMock.Setup(m => m.Map<PatientListDto>(It.IsAny<Patient>()))
                .Returns(new PatientListDto { FullName = "Sam" });

            var result = await _service.GetByUserIdAsync("user1");

            Assert.NotNull(result);
        }

        //Not found user
        [Fact]
        public async Task GetByUserIdAsync_ShouldThrow_WhenNotFound()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.GetByUserIdAsync("invalid"));
        }

        [Fact]
        public async Task UpdateByUserIdAsync_ShouldUpdatePatient()
        {
            var patient = new Patient { UserId = "user1", FullName = "Old" };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            var dto = new UpdatePatientDto { FullName = "Updated" };

            _mapperMock.Setup(m => m.Map(dto, patient))
                .Callback(() => patient.FullName = "Updated");

            await _service.UpdateByUserIdAsync("user1", dto);

            Assert.Equal("Updated", patient.FullName);
        }

        [Fact]
        public async Task UpdateByUserIdAsync_ShouldThrow_WhenNotFound()
        {
            var dto = new UpdatePatientDto();

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.UpdateByUserIdAsync("invalid", dto));
        }
        //filter

        [Fact]
        public async Task GetAllAsync_ShouldFilter_ByName_Only()
        {
            var filter = new PatientFilter
            {
                FullName = "John",
                HasInsurance = null
            };

            var patients = new List<Patient> { new Patient { FullName = "John" } };

            var paged = new PagedResult<Patient>
            {
                Items = patients,
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 1
            };

            _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Patient, bool>>>()))
                .ReturnsAsync(paged);

            _mapperMock.Setup(m => m.Map<IEnumerable<PatientListDto>>(patients))
                .Returns(new List<PatientListDto> { new PatientListDto() });

            var result = await _service.GetAllAsync(filter);

            Assert.NotNull(result);
        }
        //without filter
        [Fact]
        public async Task GetAllAsync_ShouldFilter_ByName_And_HasInsurance()
        {
            var filter = new PatientFilter
            {
                FullName = "John",
                HasInsurance = true
            };

            var patients = new List<Patient> { new Patient { FullName = "John" } };

            var paged = new PagedResult<Patient>
            {
                Items = patients,
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 1
            };

            _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Patient, bool>>>()))
                .ReturnsAsync(paged);

            _mapperMock.Setup(m => m.Map<IEnumerable<PatientListDto>>(patients))
                .Returns(new List<PatientListDto> { new PatientListDto() });

            var result = await _service.GetAllAsync(filter);

            Assert.NotNull(result);
        }


        //  Update Status
        [Fact]
        public async Task UpdateStatusAsync_ShouldUpdateStatus()
        {
            var patient = new Patient { PatientId = 1, IsActive = true };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(patient);

            await _service.UpdateStatusAsync(1, false);

            Assert.False(patient.IsActive);
            _repoMock.Verify(r => r.UpdateAsync(patient), Times.Once);
        }

        //Paging
        [Fact]
        public async Task GetAllAsync_ShouldApplyFilters_AndReturnPagedResult()
        {
            _context.Patients.AddRange(
                new Patient { FullName = "Sam", InsuranceId = "1" },
                new Patient { FullName = "Sanjay", InsuranceId = null },
                new Patient { FullName = "Samuel", InsuranceId = "2" }
            );

            await _context.SaveChangesAsync();

            var filter = new PatientFilter
            {
                FullName = "Sam",
                HasInsurance = true,
                PageNumber = 1,
                PageSize = 10
            };

            _mapperMock.Setup(m => m.Map<IEnumerable<PatientListDto>>(It.IsAny<IEnumerable<Patient>>()))
                .Returns((IEnumerable<Patient> src) =>
                    src.Select(p => new PatientListDto { FullName = p.FullName }));

            // Act
            var result = await _service.GetAllAsync(filter);

            // Assert
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Items.Count());
        }
    }
}