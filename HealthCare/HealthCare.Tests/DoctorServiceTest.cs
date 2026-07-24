using AutoMapper;
using Healthcare.Shared.DTOs.Authentication;
using Healthcare.Shared.DTOs.Doctor;
using HealthCare.Api.Data;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace HealthCare.Api.Tests
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _repoMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly HealthCareDbContext _context;
        private readonly DoctorService _service;
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly Mock<ILogger<DoctorService>> _loggerMock;

        public DoctorServiceTests()
        {
            _repoMock = new Mock<IDoctorRepository>();
            _appointmentRepoMock = new Mock<IAppointmentRepository>();
            _mapperMock = new Mock<IMapper>();
            _cacheMock = new Mock<IDistributedCache>();
           _loggerMock = new Mock<ILogger<DoctorService>>();

            var options = new DbContextOptionsBuilder<HealthCareDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new HealthCareDbContext(options);

            _service = new DoctorService(
                _repoMock.Object,
                _context,
                _mapperMock.Object,
                _appointmentRepoMock.Object,
                 _loggerMock.Object
            );
        }

        //  Add
        [Fact]
        public async Task AddAsync_ShouldAddDoctor()
        {
            var dto = new DoctorRegisterDto();
            var doctor = new Doctor();

            _mapperMock.Setup(m => m.Map<Doctor>(dto)).Returns(doctor);

            await _service.AddAsync(dto);

            _repoMock.Verify(r => r.AddAsync(doctor), Times.Once);
        }

        //  Update
        [Fact]
        public async Task UpdateAsync_ShouldUpdateDoctor()
        {
            var doctor = new Doctor();

            _repoMock.Setup(r => r.GetProfileAsync(1)).ReturnsAsync(doctor);

            await _service.UpdateAsync(1, new UpdateDoctorDto());

            _repoMock.Verify(r => r.UpdateAsync(doctor), Times.Once);
        }

        //  Update Not Found
        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetProfileAsync(1))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<DoctorNotFoundException>(() =>
                _service.UpdateAsync(1, new UpdateDoctorDto()));
        }

        //  Delete
        [Fact]
        public async Task DeleteAsync_ShouldDeleteDoctor()
        {
            var doctor = new Doctor();

            _repoMock.Setup(r => r.GetProfileAsync(1)).ReturnsAsync(doctor);

            await _service.DeleteAsync(1);

            _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        //  Delete Not Found
        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetProfileAsync(1))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<DoctorNotFoundException>(() =>
                _service.DeleteAsync(1));
        }

        //  GetById
        [Fact]
        public async Task GetByIdAsync_ShouldReturnDoctor()
        {
            var doctor = new Doctor();
            var dto = new DoctorListDto();

            _repoMock.Setup(r => r.GetProfileAsync(1)).ReturnsAsync(doctor);
            _mapperMock.Setup(m => m.Map<DoctorListDto>(doctor)).Returns(dto);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
        }

        //  GetAll
        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedDoctors()
        {

            _context.Doctors.Add(new Doctor
            {
                DoctorId = 1,
                UserId = "user-1",
                FullName = "Doctor One",
                Specialisation = "Cardiology"
            });


            await _context.SaveChangesAsync();

            _mapperMock
                .Setup(m => m.Map<IEnumerable<DoctorListDto>>(
                    It.IsAny<IEnumerable<Doctor>>()))
                .Returns(new List<DoctorListDto>
                {
            new DoctorListDto()
                });

            var result = await _service.GetAllAsync(new DoctorFilter());

            Assert.NotNull(result);
            Assert.Equal(1, result.TotalCount);
        }

        //  Update Status
        [Fact]
        public async Task UpdateStatusAsync_ShouldUpdateStatus()
        {
            var doctor = new Doctor { IsActive = true };

            _repoMock.Setup(r => r.GetProfileAsync(1)).ReturnsAsync(doctor);

            await _service.UpdateStatusAsync(1, false);

            Assert.False(doctor.IsActive);
            _repoMock.Verify(r => r.UpdateAsync(doctor), Times.Once);
        }

        //  GetSlots
        [Fact]
        public async Task GetSlots_ShouldReturnSlots()
        {
            _repoMock.Setup(r => r.GetSlots(1))
                .ReturnsAsync(new List<string> { "10:00 AM" });

            var result = await _service.GetSlots(1);

            Assert.Single(result);
        }

        //  GetSlots No Data
        [Fact]
        public async Task GetSlots_ShouldThrow_WhenEmpty()
        {
            _repoMock.Setup(r => r.GetSlots(1))
                .ReturnsAsync(new List<string>());

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.GetSlots(1));
        }

        //  CreateSlots
        [Fact]
        public async Task CreateSlots_ShouldCallRepository()
        {
            var slots = new List<string> { "10:00 AM" };

            await _service.CreateSlots(1, slots);

            _repoMock.Verify(r => r.CreateSlots(1, slots), Times.Once);
        }

        //  AvailableTimeSlotsCheck
        [Fact]
        public async Task AvailableTimeSlotsCheck_ShouldReturnAvailableSlots()
        {
            var date = DateOnly.FromDateTime(DateTime.Now);

            _repoMock.Setup(r => r.GetSlots(1))
                .ReturnsAsync(new List<string> { "10", "11" });

            _appointmentRepoMock.Setup(a => a.AvailableTimeSlots(date, 1))
                .ReturnsAsync(new List<string> { "11" });

            var result = await _service.AvailableTimeSlotsCheck(date, 1);

            Assert.Single(result);
            Assert.Contains("10", result);
        }

        //Available Doctors
        [Fact]
        public async Task AvailableDoctors_ShouldReturnDoctors()
        {
            var doctors = new List<DoctorListDto>
        {
        new DoctorListDto { FullName = "Dr. A" }
        };

            _repoMock.Setup(r => r.AvailableDoctors("Cardiology", It.IsAny<DateOnly>()))
                .ReturnsAsync(doctors);

            var result = await _service.AvailableDoctors("Cardiology", DateOnly.FromDateTime(DateTime.Now));

            Assert.Single(result);
        }

        //create leave with duplicate dates
        [Fact]
        public async Task CreateLeave_ShouldSkipDuplicateDates()
        {
            var doctorId = 1;
            var date = DateOnly.FromDateTime(DateTime.Now);

            _repoMock.Setup(r => r.GetLeavesByDoctorId(doctorId))
                .ReturnsAsync(new List<DoctorLeaves>
                {
            new DoctorLeaves { LeaveDate = date }
                });

            var leaves = new List<CreateLeaveDto>
           {
            new CreateLeaveDto { LeaveDate = date }
             };

            var result = await _service.CreateLeave(doctorId, leaves);

            Assert.Single(result.SkippedDates);
            _repoMock.Verify(r => r.CreateLeaves(It.IsAny<int>(), It.IsAny<List<CreateLeaveDto>>()), Times.Never);
        }

        //Cancel Leave if doctor is in leave
        [Fact]
        public async Task CreateLeave_ShouldCancelAppointments_WhenSlotsNotFullyAvailable()
        {
            var doctorId = 1;
            var date = DateOnly.FromDateTime(DateTime.Now);

            _repoMock.Setup(r => r.GetLeavesByDoctorId(doctorId))
                .ReturnsAsync(new List<DoctorLeaves>());

            // Mock slots
            _repoMock.Setup(r => r.GetSlots(doctorId))
                .ReturnsAsync(new List<string> { "10", "11" });

            _appointmentRepoMock.Setup(a => a.AvailableTimeSlots(date, doctorId))
                .ReturnsAsync(new List<string> { "10" }); // missing → triggers cancel

            var leaves = new List<CreateLeaveDto>
            {
             new CreateLeaveDto { LeaveDate = date }
             };

            await _service.CreateLeave(doctorId, leaves);

            _appointmentRepoMock.Verify(a =>
                a.CancelAppointmentsByDoctorDate(doctorId, date), Times.Once);
        }
        //Apply Leave
        [Fact]
        public async Task CreateLeave_ShouldCreateLeaves_WhenValid()
        {
            var doctorId = 1;
            var date = DateOnly.FromDateTime(DateTime.Now);

            _repoMock.Setup(r => r.GetLeavesByDoctorId(doctorId))
                .ReturnsAsync(new List<DoctorLeaves>());

            _repoMock.Setup(r => r.GetSlots(doctorId))
                .ReturnsAsync(new List<string> { "10" });

            _appointmentRepoMock.Setup(a => a.AvailableTimeSlots(date, doctorId))
                .ReturnsAsync(new List<string> { "10" });

            var leaves = new List<CreateLeaveDto>
           {
            new CreateLeaveDto { LeaveDate = date }
           };

            await _service.CreateLeave(doctorId, leaves);

            _repoMock.Verify(r =>
                r.CreateLeaves(doctorId, It.IsAny<List<CreateLeaveDto>>()), Times.Once);
        }

        //Get Doctor by leave
        [Fact]
        public async Task GetLeavesByDoctorIdAsync_ShouldReturnOrderedLeaves()
        {
            var leaves = new List<DoctorLeaves>
            {
                new() { LeaveDate = new DateOnly(2025,12,10) },
                new() { LeaveDate = new DateOnly(2025,12,01) }
            };

            _repoMock.Setup(r => r.GetLeavesByDoctorId(1)).ReturnsAsync(leaves);
            var result = await _service.GetLeavesByDoctorIdAsync(1);

            Assert.Equal(2, result.Count);
            Assert.True(result[0].LeaveDate < result[1].LeaveDate);
        }

        //Profile
        [Fact]
        public async Task GetMyProfileAsync_ShouldReturnDoctor()
        {
            var user = new IdentityUser{Id = "u1",Email = "doctor@test.com"}; 

            var doctor = new Doctor
            {
                DoctorId = 1,
                UserId = "u1",
                FullName = "Doctor",
                Specialisation = "Cardiology"
            };

            _context.Users.Add(user);
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            var result = await _service.GetMyProfileAsync(1);

            Assert.NotNull(result);
            Assert.Equal("doctor@test.com", result.Email);
        }

        //Profile Not Found
        [Fact]
        public async Task GetMyProfileAsync_ShouldThrow_WhenDoctorNotFound()
        {
            await Assert.ThrowsAsync<DoctorNotFoundException>(
                () => _service.GetMyProfileAsync(999));
        }

        //Available Doctors with Cache
        [Fact]
        public async Task AvailableDoctors_ShouldReturnFromCache_WhenCacheExists()
        {
            var doctors = new List<DoctorListDto>
            {
                new DoctorListDto { FullName = "Dr. A" }
            };

            var cachedData = JsonSerializer.Serialize(doctors);
            _cacheMock.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(System.Text.Encoding.UTF8.GetBytes(cachedData));

            var result = await _service.AvailableDoctors("Cardiology",DateOnly.FromDateTime(DateTime.Today));
            Assert.Single(result);

            _repoMock.Verify(r =>r.AvailableDoctors(It.IsAny<string>(), It.IsAny<DateOnly>()),Times.Never);
        }


        //Available Doctors with Cache Miss
        [Fact]
        public async Task AvailableDoctors_ShouldLoadFromRepository_WhenCacheMiss()
        {
            var doctors = new List<DoctorListDto>
            {
                new DoctorListDto { FullName = "Dr. A" }
            };

            _cacheMock.Setup(c => c.GetAsync(It.IsAny<string>(),default)).ReturnsAsync((byte[]?)null);

            _repoMock.Setup(r =>r.AvailableDoctors("Cardiology", It.IsAny<DateOnly>())).ReturnsAsync(doctors);

            var result = await _service.AvailableDoctors("Cardiology",DateOnly.FromDateTime(DateTime.Today));

            Assert.Single(result);

            _repoMock.Verify(r =>r.AvailableDoctors("Cardiology", It.IsAny<DateOnly>()),Times.Once);
        }

    }
}