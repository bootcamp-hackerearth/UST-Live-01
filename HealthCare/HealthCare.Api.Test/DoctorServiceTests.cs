using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Implementations;
using HealthCare.Shared.DTOs;
using HealthCare.Shared.DTOs.Doctor;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace HealthCare.Api.Tests
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _repoMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly HealthCareDbContext _context;
        private readonly DoctorService _service;
        private readonly Mock<ILogger<DoctorService>> _loggerMock;

        public DoctorServiceTests()
        {
            _repoMock = new Mock<IDoctorRepository>();
            _appointmentRepoMock = new Mock<IAppointmentRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<DoctorService>>();

            var options = new DbContextOptionsBuilder<HealthCareDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new HealthCareDbContext(options);

            _service = new DoctorService(
                _repoMock.Object,
                _appointmentRepoMock.Object,
                _context,
                _mapperMock.Object

            );
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDoctor()
        {
            var user = new IdentityUser
            {
                Id = "u1",
                Email = "cardio@test.com",
                UserName = "cardio@test.com"
            };

            var doctor = new Doctor
            {
                DoctorId = 1,
                UserId = user.Id,
                FullName = "Test Doctor",
                Specialisation = "Cardiology",
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };

            _context.Users.Add(user);
            _context.Doctors.Add(doctor);

            await _context.SaveChangesAsync();

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.DoctorId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var result = await _service.GetByIdAsync(999);

            Assert.Null(result);
        }
        [Fact]
        public async Task GetByIdAsync_ShouldReturnEmailFromUserTable()
        {
            var user = new IdentityUser
            {
                Id = "u1",
                Email = "cardio@test.com",
                UserName = "cardio@test.com"
            };

            var doctor = new Doctor
            {
                DoctorId = 10,
                UserId = "u1",
                FullName = "Doctor",
                Specialisation = "Cardiology",
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            };

            _context.Users.Add(user);
            _context.Doctors.Add(doctor);

            await _context.SaveChangesAsync();

            var result = await _service.GetByIdAsync(10);

            Assert.NotNull(result);
            Assert.Equal("cardio@test.com", result.Email);
        }

        //  GetAll
        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedResult()
        {
            var doctors = new List<Doctor> { new Doctor() };

            var paged = new PagedResult<Doctor>
            {
                Items = doctors,
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 1
            };

            _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Doctor, bool>>>(),
                It.IsAny<Func<IQueryable<Doctor>, IOrderedQueryable<Doctor>>>()))
                .ReturnsAsync(paged);


            _mapperMock.Setup(m => m.Map<IEnumerable<DoctorListDto>>(doctors))
                .Returns(new List<DoctorListDto> { new DoctorListDto() });

            var result = await _service.GetAllAsync(new DoctorFilter());

            Assert.Equal(1, result.TotalCount);
        }

        //  AddAsync
        [Fact]
        public async Task AddAsync_ShouldCreateDoctorAndSlots()
        {
            var dto = new CreateDoctorDto
            {
                TimeSlots = new List<string> { "09:00-10:00" }
            };

            var doctor = new Doctor { DoctorId = 1 };

            _mapperMock.Setup(m => m.Map<Doctor>(dto)).Returns(doctor);

            await _service.AddAsync(dto);

            _repoMock.Verify(r => r.AddAsync(doctor), Times.Once);
            _repoMock.Verify(r => r.CreateSlots(doctor.DoctorId, dto.TimeSlots), Times.Once);
        }

        //  UpdateAsync
        [Fact]
        public async Task UpdateAsync_ShouldUpdateDoctor()
        {
            var doctor = new Doctor();

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(doctor);

            await _service.UpdateAsync(1, new UpdateDoctorDto());

            _repoMock.Verify(r => r.UpdateAsync(doctor), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.UpdateAsync(1, new UpdateDoctorDto()));
        }

        //  UpdateStatus
        [Fact]
        public async Task UpdateStatusAsync_ShouldChangeStatus()
        {
            var doctor = new Doctor { IsActive = true };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(doctor);

            await _service.UpdateStatusAsync(1, false);

            Assert.False(doctor.IsActive);
        }

        //  Delete
        [Fact]
        public async Task DeleteAsync_ShouldDeleteDoctor()
        {
            var doctor = new Doctor();

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(doctor);

            await _service.DeleteAsync(1);

            _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.DeleteAsync(1));
        }

        //  GetSlots
        [Fact]
        public async Task GetSlots_ShouldReturnSlots()
        {
            _repoMock.Setup(r => r.GetSlots(1))
                .ReturnsAsync(new List<string> { "09:00-10:00" });

            var result = await _service.GetSlots(1);

            Assert.Single(result);
        }

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
            await _service.CreateSlots(1, new List<string> { "09:00-10:00" });

            _repoMock.Verify(r => r.CreateSlots(1, It.IsAny<List<string>>()), Times.Once);
        }

        //  CreateLeave - skip duplicate
        [Fact]
        public async Task CreateLeave_ShouldSkipExistingLeave()
        {
            _repoMock.Setup(r => r.GetLeavesByDoctorId(1))
                .ReturnsAsync(new List<DoctorLeaves>
                {
                    new DoctorLeaves { LeaveDate = new DateOnly(2026,7,1) }
                });

            var result = await _service.CreateLeave(1,
                new List<CreateLeaveDto>
                {
                    new CreateLeaveDto { LeaveDate = new DateOnly(2026,7,1) }
                });

            Assert.Single(result.SkippedDates);
        }

        //  CreateLeave - cancel appointments when conflict
        [Fact]
        public async Task CreateLeave_ShouldCancelAppointments_WhenSlotsMismatch()
        {
            // Arrange

            _context.Doctors.Add(new Doctor
            {
                DoctorId = 1,
                UserId = "user1",
                FullName = "Doctor",
                Specialisation = "Cardiology",
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            });

            await _context.SaveChangesAsync();

            _repoMock.Setup(r => r.GetLeavesByDoctorId(1))
                .ReturnsAsync(new List<DoctorLeaves>());

            _repoMock.Setup(r => r.GetSlots(1))
                .ReturnsAsync(new List<string>
                {
            "09:00-10:00",
            "10:00-11:00"
                });

            _appointmentRepoMock.Setup(r =>
                r.BookedTimeSlots(It.IsAny<DateOnly>(), 1))
                .ReturnsAsync(new List<string>
                {
            "09:00-10:00"
                });

            var leaves = new List<CreateLeaveDto>
    {
        new CreateLeaveDto
        {
            LeaveDate = new DateOnly(2026, 7, 2)
        }
    };

            // Act

            var result = await _service.CreateLeave(1, leaves);

            // Assert

            _appointmentRepoMock.Verify(r =>
                r.CancelAppointmentsByDoctorDate(
                    1,
                    It.IsAny<DateOnly>()),
                Times.Once);

            Assert.Single(result.CreatedWithCancelledAppointments);
        }

        //  AvailableDoctors
        [Fact]
        public async Task AvailableDoctors_ShouldReturnDoctors()
        {
            _context.Doctors.Add(new Doctor
            {
                DoctorId = 1,
                FullName = "Test Doctor",
                Specialisation = "Cardiology",
                IsActive = true
            });

            await _context.SaveChangesAsync();

            _mapperMock.Setup(m =>
                m.Map<List<DoctorListDto>>(It.IsAny<List<Doctor>>()))
                .Returns(new List<DoctorListDto>
                {
            new DoctorListDto()
                });

            // Act 
            var result = await _service.AvailableDoctors(
                "Cardiology",
                DateOnly.FromDateTime(DateTime.Today)
            );

            // Assert 
            Assert.NotNull(result);
            Assert.NotEmpty(result.Doctors);
            Assert.Equal(string.Empty, result.Message);
        }

        [Fact]
        public async Task GetDashboardAsync_ShouldReturnDashboardData()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            _context.Appointments.AddRange(
                new Appointment
                {
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = today,
                    TimeSlot = "09:00-10:00",
                    Status = "Confirmed"
                },
                new Appointment
                {
                    PatientId = 2,
                    DoctorId = 1,
                    ScheduledDate = today.AddDays(1),
                    TimeSlot = "10:00-11:00",
                    Status = "Pending"
                },
                new Appointment
                {
                    PatientId = 3,
                    DoctorId = 1,
                    ScheduledDate = today,
                    TimeSlot = "11:00-12:00",
                    Status = "Completed"
                }
            );

            _context.DoctorLeaves.Add(
                new DoctorLeaves
                {
                    DoctorId = 1,
                    LeaveDate = today.AddDays(2)
                });

            await _context.SaveChangesAsync();

            var result = await _service.GetDashboardAsync(1);

            Assert.Equal(3, result.UpcomingAppointments);
            Assert.Equal(1, result.PatientsTreated);
            Assert.Equal(1, result.UpcomingLeaves);
            Assert.Equal(2, result.TodaysSchedule.Count);
        }

        [Fact]
        public async Task GetDashboardAsync_ShouldReturnEmptyDashboard()
        {
            var result = await _service.GetDashboardAsync(1);

            Assert.NotNull(result);
            Assert.Equal(0, result.UpcomingAppointments);
            Assert.Equal(0, result.PatientsTreated);
            Assert.Equal(0, result.UpcomingLeaves);
            Assert.Empty(result.TodaysSchedule);
        }

        [Fact]
        public async Task GetDashboardAsync_ShouldExcludeCancelledAppointments()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            _context.Appointments.AddRange(
                new Appointment
                {
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = today,
                    TimeSlot = "09:00-10:00",
                    Status = "Cancelled"
                },
                new Appointment
                {
                    PatientId = 2,
                    DoctorId = 1,
                    ScheduledDate = today,
                    TimeSlot = "10:00-11:00",
                    Status = "Confirmed"
                }
            );

            await _context.SaveChangesAsync();

            var result = await _service.GetDashboardAsync(1);

            Assert.Equal(1, result.UpcomingAppointments);
            Assert.Single(result.TodaysSchedule);
            Assert.Equal("10:00-11:00", result.TodaysSchedule[0]);
        }

        [Fact]
        public async Task AvailableDoctors_ShouldReturnMessage_WhenNoDoctorsFound()
        {
            var result = await _service.AvailableDoctors(
                "Cardiology",
                DateOnly.FromDateTime(DateTime.Today));

            Assert.Empty(result.Doctors);
            Assert.Equal(
                "No doctors available for this specialization",
                result.Message);
        }

        [Fact]
        public async Task AvailableDoctors_ShouldReturnMessage_WhenDoctorInactive()
        {
            _context.Doctors.Add(
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Doctor",
                    Specialisation = "Cardiology",
                    YearsOfExperience = 5,
                    ConsultationFee = 500,
                    IsActive = false
                });

            await _context.SaveChangesAsync();

            var result = await _service.AvailableDoctors(
                "Cardiology",
                DateOnly.FromDateTime(DateTime.Today));

            Assert.Empty(result.Doctors);
            Assert.Equal("Doctor is not active", result.Message);
        }

        [Fact]
        public async Task AvailableDoctors_ShouldReturnMessage_WhenDoctorOnLeave()
        {
            var date = DateOnly.FromDateTime(DateTime.Today);

            _context.Doctors.Add(
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Doctor",
                    Specialisation = "Cardiology",
                    YearsOfExperience = 5,
                    ConsultationFee = 500,
                    IsActive = true
                });

            _context.DoctorLeaves.Add(
                new DoctorLeaves
                {
                    DoctorId = 1,
                    LeaveDate = date
                });

            await _context.SaveChangesAsync();

            _mapperMock.Setup(m =>
                m.Map<List<DoctorListDto>>(It.IsAny<List<Doctor>>()))
                .Returns(new List<DoctorListDto>());

            var result = await _service.AvailableDoctors(
                "Cardiology",
                date);

            Assert.Empty(result.Doctors);
            Assert.Equal("Doctor is on leave", result.Message);
        }

        [Fact]
        public async Task GetSummaryAsync_ShouldReturnDoctorSummary()
        {
            _context.Doctors.AddRange(
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Doctor1",
                    Specialisation = "Cardiology",
                    YearsOfExperience = 5,
                    ConsultationFee = 500,
                    IsActive = true,
                    CreatedDate = DateTimeOffset.UtcNow
                },
                new Doctor
                {
                    DoctorId = 2,
                    FullName = "Doctor2",
                    Specialisation = "Dental",
                    YearsOfExperience = 8,
                    ConsultationFee = 700,
                    IsActive = false,
                    CreatedDate = DateTimeOffset.UtcNow
                });

            await _context.SaveChangesAsync();

            var result = await _service.GetSummaryAsync();

            Assert.Equal(2, result.TotalDoctors);
            Assert.Equal(1, result.ActiveDoctors);
            Assert.Equal(1, result.InactiveDoctors);
        }

        [Fact]
        public async Task GetSummaryAsync_ShouldReturnEmptySummary_WhenNoDoctorsExist()
        {
            var result = await _service.GetSummaryAsync();

            Assert.NotNull(result);
            Assert.Equal(0, result.TotalDoctors);
            Assert.Equal(0, result.ActiveDoctors);
            Assert.Equal(0, result.InactiveDoctors);
        }

        [Fact]
        public async Task GetAllAsync_ShouldHandleAscendingExperienceOrder()
        {
            var filter = new DoctorFilter
            {
                ExperienceOrder = "asc"
            };

            _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Doctor, bool>>>(),
                It.IsAny<Func<IQueryable<Doctor>, IOrderedQueryable<Doctor>>>()))
                .ReturnsAsync(new PagedResult<Doctor>
                {
                    Items = new List<Doctor>(),
                    PageNumber = 1,
                    PageSize = 10,
                    TotalCount = 0
                });

            var result = await _service.GetAllAsync(filter);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldHandleDescendingExperienceOrder()
        {
            var filter = new DoctorFilter
            {
                ExperienceOrder = "desc"
            };

            _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Doctor, bool>>>(),
                It.IsAny<Func<IQueryable<Doctor>, IOrderedQueryable<Doctor>>>()))
                .ReturnsAsync(new PagedResult<Doctor>
                {
                    Items = new List<Doctor>(),
                    PageNumber = 1,
                    PageSize = 10,
                    TotalCount = 0
                });

            var result = await _service.GetAllAsync(filter);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrow_WhenDoctorNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.UpdateStatusAsync(1, true));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEmptyEmail_WhenUserEmailNull()
        {
            var user = new IdentityUser
            {
                Id = "u1",
                Email = null
            };

            var doctor = new Doctor
            {
                DoctorId = 1,
                UserId = "u1",
                FullName = "Doctor",
                Specialisation = "Cardiology",
                YearsOfExperience = 5,
                ConsultationFee = 500
            };

            _context.Users.Add(user);
            _context.Doctors.Add(doctor);

            await _context.SaveChangesAsync();

            var result = await _service.GetByIdAsync(1);

            Assert.Equal(string.Empty, result!.Email);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowFriendlyMessage_WhenDbFails()
        {
            var doctor = new Doctor();

            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _repoMock.Setup(r => r.DeleteAsync(1))
                .ThrowsAsync(new DbUpdateException());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.DeleteAsync(1));

            Assert.Equal(
                "Failed to delete Doctor. It may be referenced by existing appointments or health records.",
                ex.Message);

            Assert.IsType<DbUpdateException>(ex.InnerException);
        }



    }
}