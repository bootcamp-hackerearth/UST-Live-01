using AutoMapper;
using Healthcare.Shared.DTOs;
using Healthcare.Shared.DTOs.Appointments;
using Healthcare.Shared.Events;
using HealthCare.Api.Data;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Implementations;
using HealthCare.Api.Services.Interfaces;
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
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly HealthCareDbContext _context;
        private readonly AppointmentService _service;
        private readonly Mock<ILogger<AppointmentService>> _loggerMock;
        private readonly Mock<IDistributedCache> _cacheMock;

        public AppointmentServiceTests()
        {
            _repoMock = new Mock<IAppointmentRepository>();
            _doctorServiceMock = new Mock<IDoctorService>();
            _mapperMock = new Mock<IMapper>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _loggerMock = new Mock<ILogger<AppointmentService>>();
            _cacheMock = new Mock<IDistributedCache>();

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
                _loggerMock.Object,
                _cacheMock.Object
            );
        }

        //  Add
        [Fact]
        public async Task AddAsync_ShouldAddAppointment()
        {
            var patient = new Patient{ UserId="1",PatientId = 1,FullName = "Test Patient", Gender = "Male" };
            var doctor = new Doctor{ UserId="as", DoctorId = 1, FullName = "Test Doctor",Specialisation = "Cardiology"};
            _context.Patients.Add(patient);
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            var dto = new CreateAppointmentDto();
            var appointment = new Appointment
            {
                DoctorId = 1,
                Doctor = doctor,
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today),
                TimeSlot = "10:00"
            };

            _mapperMock.Setup(m => m.Map<Appointment>(dto)).Returns(appointment);
            await _service.AddAsync(dto, 1);

            _repoMock.Verify(r => r.AddAsync(appointment), Times.Once);
            _publishEndpointMock.Verify( p => p.Publish(It.IsAny<AppointmentBookedEvent>(),default),Times.Once);

            _cacheMock.Verify(c => c.RemoveAsync(It.IsAny<string>(),default),Times.Once);
        }

        //  Update
        [Fact]
        public async Task UpdateAsync_ShouldUpdateAppointment()
        {
            var appointment = new Appointment();

            _repoMock.Setup(r => r.GetProfileAsync(1)).ReturnsAsync(appointment);

            await _service.UpdateAsync(1, new UpdateAppointmentDto());

            _repoMock.Verify(r => r.UpdateAsync(appointment), Times.Once);
        }

        //  Update Not Found
        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetProfileAsync(1)).ReturnsAsync((Appointment?)null);
            await Assert.ThrowsAsync<AppointmentNotFoundException>(() =>_service.UpdateAsync(1, new UpdateAppointmentDto()));
        }

        //  Delete
        [Fact]
        public async Task DeleteAsync_ShouldDeleteAppointment()
        {
            var appointment = new Appointment();

            _repoMock.Setup(r => r.GetProfileAsync(1)).ReturnsAsync(appointment);

            await _service.DeleteAsync(1);

            _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        //  Delete Not Found
        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetProfileAsync(1))
                .ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(() =>
                _service.DeleteAsync(1));
        }

        //  GetById
        [Fact]
        public async Task GetByIdAsync_ShouldReturnAppointment()
        {
            var appointment = new Appointment();
            var dto = new AppointmentListDto();

            _repoMock.Setup(r => r.GetProfileAsync(1)).ReturnsAsync(appointment);
            _mapperMock.Setup(m => m.Map<AppointmentListDto>(appointment)).Returns(dto);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetProfileAsync(1)).ReturnsAsync((Appointment?)null);

            var result = await _service.GetByIdAsync(1);

            Assert.Null(result);
        }

        //  GetAll
        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedAppointments()
        {
            var appointments = new List<Appointment> { new Appointment() };

            var paged = new PagedResult<Appointment>
            {
                Items = appointments,
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 1
            };

            _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Appointment, bool>>>(),
                It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>()))
                .ReturnsAsync(paged);

            _mapperMock.Setup(m => m.Map<IEnumerable<AppointmentListDto>>(appointments))
                .Returns(new List<AppointmentListDto> { new AppointmentListDto() });

            var result = await _service.GetAllAsync(new AppointmentFilter());

            Assert.NotNull(result);
            Assert.Equal(1, result.TotalCount);
        }

        //  Update Status
        [Fact]
        public async Task UpdateStatusAsync_ShouldUpdateStatus()
        {
            var appointment = new Appointment
            {
                ScheduledDate = DateOnly.FromDateTime(DateTime.Today),
                Doctor = new Doctor { Specialisation = "Cardiology" }
            };

            _repoMock.Setup(r => r.GetProfileAsync(1)).ReturnsAsync(appointment);

            await _service.UpdateStatusAsync(1, new UpdateAppointmentDto
            {
                Status = "Cancelled",
                CancellationReason = "Patient request"
            });

            Assert.Equal("Cancelled", appointment.Status);
            _repoMock.Verify(r => r.UpdateAsync(appointment), Times.Once);
        }

        //  AvailableTimeSlots
        [Fact]
        public async Task AvailableTimeSlots_ShouldReturnFreeSlots()
        {
            var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

            _doctorServiceMock.Setup(d => d.GetSlots(1))
                .ReturnsAsync(new List<string> { "10", "11" });

            _repoMock.Setup(r => r.AvailableTimeSlots(date, 1))
                .ReturnsAsync(new List<string> { "11" });

            var result = await _service.AvailableTimeSlots(date, 1);

            Assert.Single(result);
            Assert.Contains("10", result);
        }

        //  AvailableTimeSlots Past Date
        [Fact]
        public async Task AvailableTimeSlots_ShouldThrow_ForPastDate()
        {
            var pastDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.AvailableTimeSlots(pastDate, 1));
        }

        //  IsAvailable
        [Fact]
        public async Task IsAvailable_ShouldReturnTrue_WhenAvailable()
        {
            _repoMock.Setup(r => r.IsAvailable(It.IsAny<DateOnly>(), 1, "10"))
                .ReturnsAsync(true);

            var result = await _service.IsAvailable(DateOnly.FromDateTime(DateTime.Today), 1, "10");

            Assert.True(result);
        }

        //  IsAvailable Not Available
        [Fact]
        public async Task IsAvailable_ShouldThrow_WhenNotAvailable()
        {
            _repoMock.Setup(r => r.IsAvailable(It.IsAny<DateOnly>(), 1, "10"))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.IsAvailable(DateOnly.FromDateTime(DateTime.Today), 1, "10"));
        }

        //Daily Report
        [Fact]
        public async Task GetDailyReport_ShouldReturnData()
        {
            var report = new List<AppointmentReportDto>
            {
                new AppointmentReportDto()
            };

            _repoMock.Setup(r =>r.GetDailyReport(It.IsAny<DateOnly>(), It.IsAny<DateOnly>())).ReturnsAsync(report);

            var result = await _service.GetDailyReport(DateOnly.FromDateTime(DateTime.Today),DateOnly.FromDateTime(DateTime.Today));
            Assert.Single(result);
        }

        //Get Doctor Schedule
        [Fact]
        public async Task GetDoctorSchedule_ShouldReturnSchedule_WhenExists()
        {
            var date = DateOnly.FromDateTime(DateTime.Now);

            var data = new List<AppointmentListDto>
             {
             new AppointmentListDto()
             };

            _repoMock.Setup(r => r.GetDoctorSchedule(date, 1))
                .ReturnsAsync(data);

            var result = await _service.GetDoctorSchedule(date, 1);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetDoctorSchedule_ShouldReturnEmpty_WhenNoData()
        {
            var date = DateOnly.FromDateTime(DateTime.Now);

            _repoMock.Setup(r => r.GetDoctorSchedule(date, 1))
                .ReturnsAsync(new List<AppointmentListDto>());

            var result = await _service.GetDoctorSchedule(date, 1);

            Assert.Empty(result);
        }

        //Patient Schedule
        [Fact]
        public async Task GetPatientSchedule_ShouldReturnSchedule_WhenExists()
        {
            var date = DateOnly.FromDateTime(DateTime.Now);

            var data = new List<AppointmentListDto>
            {
            new AppointmentListDto()
             };

            _repoMock.Setup(r => r.GetPatientSchedule(date, 1))
                .ReturnsAsync(data);

            var result = await _service.GetPatientSchedule(date, 1);

            Assert.Single(result);
        }

        //Get Appointment By patient Id
        [Fact]
        public async Task GetAppointmentByPatient_ShouldReturnAppointments_WhenExists()
        {
            var data = new List<AppointmentListDto>
    {
        new AppointmentListDto()
    };

            _repoMock.Setup(r => r.GetAppointmentByPatient(1))
                .ReturnsAsync(data);

            var result = await _service.GetAppointmentByPatient(1);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAppointmentByPatient_ShouldReturnEmpty_WhenNoData()
        {
            _repoMock.Setup(r => r.GetAppointmentByPatient(1))
                .ReturnsAsync(new List<AppointmentListDto>());

            var result = await _service.GetAppointmentByPatient(1);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetPatientSchedule_ShouldReturnEmpty_WhenNoData()
        {
            var date = DateOnly.FromDateTime(DateTime.Now);

            _repoMock.Setup(r => r.GetPatientSchedule(date, 1))
                .ReturnsAsync(new List<AppointmentListDto>());

            var result = await _service.GetPatientSchedule(date, 1);

            Assert.Empty(result);
        }

        //Get Appointment By Doctor
        [Fact]
        public async Task GetAppointmentByDoctor_ShouldReturnAppointments_WhenExists()
        {
            var data = new List<AppointmentListDto>
    {
        new AppointmentListDto()
    };

            _repoMock.Setup(r => r.GetAppointmentByDoctor(1))
                .ReturnsAsync(data);

            var result = await _service.GetAppointmentByDoctor(1);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAppointmentByDoctor_ShouldReturnEmpty_WhenNoData()
        {
            _repoMock.Setup(r => r.GetAppointmentByDoctor(1))
                .ReturnsAsync(new List<AppointmentListDto>());

            var result = await _service.GetAppointmentByDoctor(1);

            Assert.Empty(result);
        }

        //  CancelAppointmentsByDoctorDate
        [Fact]
        public async Task CancelAppointmentsByDoctorDate_ShouldCallRepository()
        {
            await _service.CancelAppointmentsByDoctorDate(1, DateOnly.FromDateTime(DateTime.Today));

            _repoMock.Verify(r => r.CancelAppointmentsByDoctorDate(1, It.IsAny<DateOnly>()), Times.Once);
        }
    }
}