using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.Events;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Implementations;
using HealthCare.Api.Services.Interfaces;
using HealthCare.Shared.DTOs;
using HealthCare.Shared.DTOs.Appointment;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace HealthCare.Api.Tests
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _repoMock;
        private readonly Mock<IDoctorService> _doctorServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly HealthCareDbContext _context;
        private readonly AppointmentService _service;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly Mock<ILogger<AppointmentService>> _loggerMock;

        public AppointmentServiceTests()
        {
            _repoMock = new Mock<IAppointmentRepository>();
            _doctorServiceMock = new Mock<IDoctorService>();
            _mapperMock = new Mock<IMapper>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _loggerMock = new Mock<ILogger<AppointmentService>>();


            var options = new DbContextOptionsBuilder<HealthCareDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new HealthCareDbContext(options);

            _service = new AppointmentService(
                _repoMock.Object,
                _doctorServiceMock.Object,
                _context,
                _mapperMock.Object,
                _publishEndpointMock.Object,
                _loggerMock.Object
            );
        }

        //  GetById
        [Fact]
        public async Task GetByIdAsync_ShouldReturnAppointment()
        {
            var appointment = new Appointment();
            var dto = new AppointmentListDto();

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appointment);
            _mapperMock.Setup(m => m.Map<AppointmentListDto>(appointment)).Returns(dto);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrow_WhenAppointmentNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.UpdateStatusAsync(1,
                    new UpdateAppointmentDto
                    {
                        Status = "Confirmed"
                    }));
        }

        [Fact]
        public async Task AvailableTimeSlots_ShouldReturnEmpty_WhenAllSlotsBooked()
        {
            var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

            _doctorServiceMock.Setup(d => d.GetSlots(1))
                .ReturnsAsync(new List<string>
                {
            "09:00",
            "10:00"
                });

            _repoMock.Setup(r => r.BookedTimeSlots(date, 1))
                .ReturnsAsync(new List<string>
                {
            "09:00",
            "10:00"
                });

            var result = await _service.AvailableTimeSlots(date, 1);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetDoctorSchedule_ShouldReturnSchedule()
        {
            var schedule = new List<AppointmentListDto>
    {
        new AppointmentListDto()
    };

            _repoMock.Setup(r =>
                r.GetDoctorSchedule(It.IsAny<DateOnly>(), 1))
                .ReturnsAsync(schedule);

            var result = await _service.GetDoctorSchedule(
                DateOnly.FromDateTime(DateTime.Today),
                1);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetPatientSchedule_ShouldReturnSchedule()
        {
            var schedule = new List<AppointmentListDto>
    {
        new AppointmentListDto()
    };

            _repoMock.Setup(r =>
                r.GetPatientSchedule(It.IsAny<DateOnly>(), 1))
                .ReturnsAsync(schedule);

            var result = await _service.GetPatientSchedule(
                DateOnly.FromDateTime(DateTime.Today),
                1);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetDailyReport_ShouldReturnReport()
        {
            var reports = new List<AppointmentReportDto>
    {
        new AppointmentReportDto()
    };

            _repoMock.Setup(r => r.GetDailyReport())
                .ReturnsAsync(reports);

            var result = await _service.GetDailyReport();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetSummaryAsync_ShouldReturnEmptySummary_WhenNoAppointments()
        {
            var result = await _service.GetSummaryAsync();

            Assert.NotNull(result);
            Assert.Equal(0, result.PendingCount);
            Assert.Equal(0, result.ConfirmedCount);
            Assert.Equal(0, result.CancelledCount);
            Assert.Equal(0, result.CompletedCount);
            Assert.Equal(0, result.TotalRevenue);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenRepositoryFails()
        {
            var dto = new CreateAppointmentDto
            {
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                DoctorId = 1,
                TimeSlot = "09:00-10:00"
            };

            var appointment = new Appointment();

            _repoMock.Setup(r =>
                r.BookedTimeSlots(dto.ScheduledDate, 1))
                .ReturnsAsync(new List<string>());

            _mapperMock.Setup(m =>
                m.Map<Appointment>(dto))
                .Returns(appointment);

            _repoMock.Setup(r =>
                r.AddAsync(It.IsAny<Appointment>()))
                .ThrowsAsync(new DbUpdateException());
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.AddAsync(dto, 1));

            Assert.Equal("Failed to book the appointment.", ex.Message);
            Assert.IsType<DbUpdateException>(ex.InnerException);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenDeleteFails()
        {
            var appointment = new Appointment();

            _repoMock.Setup(r =>
                r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _repoMock.Setup(r =>
                r.DeleteAsync(1))
                .ThrowsAsync(new DbUpdateException());

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
    _service.DeleteAsync(1));

            Assert.Equal(
                "Failed to delete appointment. It may be referenced by existing health records.",
                ex.Message);

            Assert.IsType<DbUpdateException>(ex.InnerException);
        }


        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.GetByIdAsync(1));
        }

        //  AddAsync - success
        [Fact]
        public async Task AddAsync_ShouldCreateAppointment()
        {
            // Arrange

            _context.Patients.Add(new Patient
            {
                PatientId = 5,
                FullName = "Test Patient",
                Email = "patient@test.com",
                PhoneNumber = "9876543210",
                Gender = "Female"
            });

            await _context.SaveChangesAsync();

            var dto = new CreateAppointmentDto
            {
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                DoctorId = 1,
                TimeSlot = "09:00-10:00"
            };

            _repoMock.Setup(r =>
                    r.BookedTimeSlots(dto.ScheduledDate, dto.DoctorId))
                .ReturnsAsync(new List<string>());

            var appointment = new Appointment
            {
                AppointmentId = 100,
                DoctorId = 1,
                ScheduledDate = dto.ScheduledDate,
                TimeSlot = dto.TimeSlot
            };

            _mapperMock.Setup(m => m.Map<Appointment>(dto))
                .Returns(appointment);

            // Act

            await _service.AddAsync(dto, 5);

            // Assert

            _repoMock.Verify(r =>
                r.AddAsync(It.IsAny<Appointment>()),
                Times.Once);

            _publishEndpointMock.Verify(p =>
                p.Publish(
                    It.IsAny<AppointmentBookedEvent>(),
                    default),
                Times.Once);
        }

        //  AddAsync - past date
        [Fact]
        public async Task AddAsync_ShouldThrow_WhenPastDate()
        {
            var dto = new CreateAppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1))
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.AddAsync(dto, 1));
        }

        //  AddAsync - slot not available
        [Fact]
        public async Task AddAsync_ShouldThrow_WhenSlotUnavailable()
        {
            var dto = new CreateAppointmentDto
            {
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                DoctorId = 1,
                TimeSlot = "09:00-10:00"
            };

            _repoMock.Setup(r => r.BookedTimeSlots(dto.ScheduledDate, 1))
    .ReturnsAsync(new List<string>
    {
        "09:00-10:00"
    });

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.AddAsync(dto, 1));
        }

        //  UpdateAsync
        [Fact]
        public async Task UpdateAsync_ShouldUpdateAppointment()
        {
            var appointment = new Appointment();

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appointment);

            await _service.UpdateAsync(1, new UpdateAppointmentDto());

            _repoMock.Verify(r => r.UpdateAsync(appointment), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.UpdateAsync(1, new UpdateAppointmentDto()));
        }

        //  UpdateStatus
        [Fact]
        public async Task UpdateStatusAsync_ShouldUpdateStatus()
        {
            var appointment = new Appointment();

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appointment);

            var dto = new UpdateAppointmentDto
            {
                Status = "Cancelled",
                CancellationReason = "Test"
            };

            await _service.UpdateStatusAsync(1, dto);

            Assert.Equal("Cancelled", appointment.Status);
        }

        //  Delete
        [Fact]
        public async Task DeleteAsync_ShouldDeleteAppointment()
        {
            var appointment = new Appointment();

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(appointment);

            await _service.DeleteAsync(1);

            _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.DeleteAsync(1));
        }

        //  AvailableTimeSlots
        [Fact]
        public async Task AvailableTimeSlots_ShouldReturnFreeSlots()
        {
            var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

            _doctorServiceMock.Setup(d => d.GetSlots(1))
                .ReturnsAsync(new List<string> { "09:00", "10:00" });

            _repoMock.Setup(r => r.BookedTimeSlots(date, 1))
                .ReturnsAsync(new List<string> { "09:00" });

            var result = await _service.AvailableTimeSlots(date, 1);

            Assert.Single(result);
            Assert.Contains("10:00", result);
        }

        //  AvailableTimeSlots - past date
        [Fact]
        public async Task AvailableTimeSlots_ShouldThrow_WhenPast()
        {
            var date = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.AvailableTimeSlots(date, 1));
        }

        //  IsAvailable
        [Fact]
        public async Task IsAvailable_ShouldReturnTrue()
        {
            _repoMock.Setup(r =>
                r.BookedTimeSlots(It.IsAny<DateOnly>(), 1))
                .ReturnsAsync(new List<string>());

            var result = await _service.IsAvailable(
                DateOnly.FromDateTime(DateTime.Today),
                1,
                "09:00-10:00");

            Assert.True(result);
        }

        [Fact]
        public async Task IsAvailable_ShouldThrow_WhenFalse()
        {
            _repoMock.Setup(r =>
                r.BookedTimeSlots(It.IsAny<DateOnly>(), 1))
                .ReturnsAsync(new List<string>
                {
            "09:00-10:00"
                });

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.IsAvailable(
                    DateOnly.FromDateTime(DateTime.Today),
                    1,
                    "09:00-10:00"));
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrow_WhenInvalidStatus()
        {
            var appointment = new Appointment();

            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentDto
            {
                Status = "WrongStatus"
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.UpdateStatusAsync(1, dto));
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldThrow_WhenCancelledWithoutReason()
        {
            var appointment = new Appointment();

            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentDto
            {
                Status = "Cancelled",
                CancellationReason = ""
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.UpdateStatusAsync(1, dto));
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldUpdateConfirmedStatus()
        {
            var appointment = new Appointment();

            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            var dto = new UpdateAppointmentDto
            {
                Status = "Confirmed"
            };

            await _service.UpdateStatusAsync(1, dto);

            Assert.Equal("Confirmed", appointment.Status);
        }

        //  GetDailyReport
        [Fact]
        public async Task GetDailyReport_ShouldReturnList()
        {
            _repoMock.Setup(r => r.GetDailyReport())
                .ReturnsAsync(new List<AppointmentReportDto>());

            var result = await _service.GetDailyReport();

            Assert.NotNull(result);
        }

        //  Schedule methods
        [Fact]
        public async Task GetDoctorSchedule_ShouldReturnList()
        {
            _repoMock.Setup(r => r.GetDoctorSchedule(It.IsAny<DateOnly>(), 1))
                .ReturnsAsync(new List<AppointmentListDto>());

            var result = await _service.GetDoctorSchedule(DateOnly.FromDateTime(DateTime.Today), 1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetPatientSchedule_ShouldReturnList()
        {
            _repoMock.Setup(r => r.GetPatientSchedule(It.IsAny<DateOnly>(), 1))
                .ReturnsAsync(new List<AppointmentListDto>());

            var result = await _service.GetPatientSchedule(DateOnly.FromDateTime(DateTime.Today), 1);

            Assert.NotNull(result);
        }

        //  CancelAppointments
        [Fact]
        public async Task CancelAppointments_ShouldCallRepository()
        {
            await _service.CancelAppointmentsByDoctorDate(1, DateOnly.FromDateTime(DateTime.Today));

            _repoMock.Verify(r =>
                r.CancelAppointmentsByDoctorDate(1, It.IsAny<DateOnly>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldFilter_ByStatusAndDate()
        {
            var filter = new AppointmentFilter
            {
                Status = "Scheduled",
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today)
            };

            var repoResult = new PagedResult<Appointment>
            {
                Items = new List<Appointment>(),
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 0
            };

            _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Appointment, bool>>>(),
                It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>()))
                .ReturnsAsync(repoResult);

            var result = await _service.GetAllAsync(filter);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldFilter_ByStatusOnly()
        {
            var filter = new AppointmentFilter
            {
                Status = "Cancelled"
            };

            var repoResult = new PagedResult<Appointment>
            {
                Items = new List<Appointment>(),
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 0
            };

            _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Appointment, bool>>>(),
                It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>()))
                .ReturnsAsync(repoResult);

            var result = await _service.GetAllAsync(filter);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldFilter_ByDateOnly()
        {
            var filter = new AppointmentFilter
            {
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today)
            };

            var repoResult = new PagedResult<Appointment>
            {
                Items = new List<Appointment>(),
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 0
            };

            _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Appointment, bool>>>(),
                It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>()))
                .ReturnsAsync(repoResult);

            var result = await _service.GetAllAsync(filter);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAll_WhenNoFilters()
        {
            var filter = new AppointmentFilter(); // no values set

            var repoResult = new PagedResult<Appointment>
            {
                Items = new List<Appointment>(),
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 0
            };

            _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                null,
                It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>()))
                .ReturnsAsync(repoResult);

            var result = await _service.GetAllAsync(filter);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAppointmentByDoctor_ShouldReturnOnlyFuturePendingAndConfirmedAppointments()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var appointments = new List<AppointmentListDto>
    {
        new()
        {
            ScheduledDate = today.AddDays(1),
            Status = "Pending",
            TimeSlot = "09:00"
        },
        new()
        {
            ScheduledDate = today.AddDays(1),
            Status = "Confirmed",
            TimeSlot = "10:00"
        },
        new()
        {
            ScheduledDate = today.AddDays(1),
            Status = "Cancelled",
            TimeSlot = "11:00"
        },
        new()
        {
            ScheduledDate = today.AddDays(-1),
            Status = "Pending",
            TimeSlot = "12:00"
        }
    };

            _repoMock.Setup(r => r.GetAppointmentByDoctor(1))
                .ReturnsAsync(appointments);

            var result = await _service.GetAppointmentByDoctor(1);

            Assert.Equal(2, result.Count);

            Assert.All(result, a =>
                Assert.True(
                    a.Status == "Pending" ||
                    a.Status == "Confirmed"));
        }

        [Fact]
        public async Task GetAppointmentByPatient_ShouldReturnOnlyPendingAndConfirmedAppointments()
        {
            var appointments = new List<AppointmentListDto>
    {
        new()
        {
            Status = "Pending",
            ScheduledDate = DateOnly.FromDateTime(DateTime.Today)
        },
        new()
        {
            Status = "Confirmed",
            ScheduledDate = DateOnly.FromDateTime(DateTime.Today)
        },
        new()
        {
            Status = "Cancelled",
            ScheduledDate = DateOnly.FromDateTime(DateTime.Today)
        }
    };

            _repoMock.Setup(r => r.GetAppointmentByPatient(1))
                .ReturnsAsync(appointments);

            var result = await _service.GetAppointmentByPatient(1);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetReportByDateRange_ShouldReturnReport()
        {
            var doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Dr Test",
                Specialisation = "Cardiology",
                YearsOfExperience = 10,
                ConsultationFee = 500
            };

            _context.Doctors.Add(doctor);

            _context.Appointments.AddRange(
                new Appointment
                {
                    PatientId = 1,
                    DoctorId = doctor.DoctorId,
                    Doctor = doctor,
                    ScheduledDate = DateOnly.FromDateTime(DateTime.Today),
                    TimeSlot = "09:00-10:00",
                    Status = "Pending"
                },
                new Appointment
                {
                    PatientId = 2,
                    DoctorId = doctor.DoctorId,
                    Doctor = doctor,
                    ScheduledDate = DateOnly.FromDateTime(DateTime.Today),
                    TimeSlot = "10:00-11:00",
                    Status = "Completed"
                }
            );

            await _context.SaveChangesAsync();

            var result = await _service.GetReportByDateRange(
                DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
                DateOnly.FromDateTime(DateTime.Today.AddDays(1)));

            Assert.Single(result);
            Assert.Equal(1, result[0].PendingCount);
            Assert.Equal(1, result[0].CompletedCount);
            Assert.Equal(500, result[0].Revenue);
        }

        [Fact]
        public async Task GetSummaryAsync_ShouldReturnSummary()
        {
            var doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Dr Smith",
                Specialisation = "Neurology",
                YearsOfExperience = 8,
                ConsultationFee = 600
            };

            _context.Doctors.Add(doctor);

            _context.Appointments.AddRange(
                new Appointment
                {
                    PatientId = 1,
                    DoctorId = doctor.DoctorId,
                    Doctor = doctor,
                    ScheduledDate = DateOnly.FromDateTime(DateTime.Today),
                    TimeSlot = "09:00-10:00",
                    Status = "Pending"
                },
                new Appointment
                {
                    PatientId = 2,
                    DoctorId = doctor.DoctorId,
                    Doctor = doctor,
                    ScheduledDate = DateOnly.FromDateTime(DateTime.Today),
                    TimeSlot = "10:00-11:00",
                    Status = "Confirmed"
                },
                new Appointment
                {
                    PatientId = 3,
                    DoctorId = doctor.DoctorId,
                    Doctor = doctor,
                    ScheduledDate = DateOnly.FromDateTime(DateTime.Today),
                    TimeSlot = "11:00-12:00",
                    Status = "Completed"
                }
            );

            await _context.SaveChangesAsync();

            var result = await _service.GetSummaryAsync();

            Assert.NotNull(result);
            Assert.Equal(1, result.PendingCount);
            Assert.Equal(1, result.ConfirmedCount);
            Assert.Equal(0, result.CancelledCount);
            Assert.Equal(1, result.CompletedCount);
            Assert.Equal(600, result.TotalRevenue);
        }


    }
}