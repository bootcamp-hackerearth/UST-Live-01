using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.DTOs.Appointment;
using HealthCare.Api.Events;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Implementations;
using HealthCare.Api.Services.Interfaces;
using HealthCare.Shared.DTOs.Appointment;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HealthCare.Api.Tests;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _repositoryMock;
    private readonly Mock<IDoctorService> _doctorServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;

    private readonly HealthCareDbContext _context;
    private readonly AppointmentService _service;

    public AppointmentServiceTests()
    {
        _repositoryMock = new Mock<IAppointmentRepository>();
        _doctorServiceMock = new Mock<IDoctorService>();
        _mapperMock = new Mock<IMapper>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();

        var options = new DbContextOptionsBuilder<HealthCareDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new HealthCareDbContext(options);

        SeedPatients();
        SeedDoctorsAndAppointments();

        _service = new AppointmentService(
            _repositoryMock.Object,
            _doctorServiceMock.Object,
            _context,
            _mapperMock.Object,
            _publishEndpointMock.Object
          );
    }

    private void SeedPatients()
    {
        _context.Patients.AddRange(
            new Patient
            {
                PatientId = 5,
                FullName = "Test Patient",
                UserId = "user-5",
                PhoneNumber = "9999999999",
                Gender = "Female",
                InsuranceId = "INS001",
                IsActive = true,
                CreatedDate = DateTimeOffset.UtcNow
            },
            new Patient
            {
                PatientId = 50,
                FullName = "Another Patient",
                UserId = "user-50",
                PhoneNumber = "8888888888",
                Gender = "Female",
                InsuranceId = "INS050",
                IsActive = true,
                CreatedDate = DateTimeOffset.UtcNow
            }
        );

        _context.SaveChanges();
    }
    private void SeedDoctorsAndAppointments()
    {
        var doctor1 = new Doctor
        {
            DoctorId = 1,
            FullName = "Dr John",
            Specialisation = "Cardiology",
            YearsOfExperience = 10,
            ConsultationFee = 500,
            IsActive = true,
            UserId = "doctor-user-1"
        };

        var doctor2 = new Doctor
        {
            DoctorId = 2,
            FullName = "Dr Smith",
            Specialisation = "Dermatology",
            YearsOfExperience = 5,
            ConsultationFee = 300,
            IsActive = true,
            UserId = "doctor-user-2"
        };

        _context.Doctors.AddRange(doctor1, doctor2);

        var patient1 = _context.Patients.First(p => p.PatientId == 5);
        var patient2 = _context.Patients.First(p => p.PatientId == 50);

        var today = DateOnly.FromDateTime(DateTime.Today);

        _context.Appointments.AddRange(
            new Appointment
            {
                AppointmentId = 1,
                PatientId = 5,
                DoctorId = 1,
                Patient = patient1,
                Doctor = doctor1,
                ScheduledDate = today.AddDays(1),
                TimeSlot = "09:00",
                Status = "Pending"
            },
            new Appointment
            {
                AppointmentId = 2,
                PatientId = 50,
                DoctorId = 2,
                Patient = patient2,
                Doctor = doctor2,
                ScheduledDate = today.AddDays(2),
                TimeSlot = "10:00",
                Status = "Confirmed"
            },
            new Appointment
            {
                AppointmentId = 3,
                PatientId = 5,
                DoctorId = 1,
                Patient = patient1,
                Doctor = doctor1,
                ScheduledDate = today.AddDays(3),
                TimeSlot = "11:00",
                Status = "Cancelled"
            }
        );

        _context.SaveChanges();
    }



    [Fact]
    public async Task GetByIdAsync_ReturnsAppointmentDto_WhenExists()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 5,
            DoctorId = 10,
            TimeSlot = "09:00",
            ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            Status = "Pending",
            Patient = new Patient
            {
                PatientId = 5,
                FullName = "Test Patient"
            },
            Doctor = new Doctor
            {
                DoctorId = 10,
                FullName = "Dr Test"
            }
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(appointment);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.AppointmentId);
        Assert.Equal(5, result.PatientId);
        Assert.Equal("Test Patient", result.PatientName);
        Assert.Equal("Dr Test", result.DoctorName);
        Assert.Equal("09:00", result.TimeSlot);
        Assert.Equal("Pending", result.Status);
    }

    [Fact]
    public async Task GetByIdAsync_Throws_WhenNotFound()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetByIdAsync(2))
            .ReturnsAsync((Appointment?)null);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.GetByIdAsync(2));

        // Assert
        Assert.Equal("Appointment not found.", exception.Message);
    }

    
    [Fact]
    public async Task AddAsync_PublishesEvent_OnSuccess()
    {
        // Arrange
        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            TimeSlot = "09:00"
        };

        var appointmentEntity = new Appointment
        {
            AppointmentId = 100,
            DoctorId = dto.DoctorId,
            PatientId = 5,
            ScheduledDate = dto.ScheduledDate,
            TimeSlot = dto.TimeSlot,
            Status = "Pending"
        };

        _repositoryMock
            .Setup(r => r.IsAvailable(dto.ScheduledDate, dto.DoctorId, dto.TimeSlot))
            .ReturnsAsync(true);

        _mapperMock
            .Setup(m => m.Map<Appointment>(dto))
            .Returns(appointmentEntity);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Appointment>()))
            .Returns(Task.CompletedTask);

        _publishEndpointMock
            .Setup(p => p.Publish(
                It.IsAny<AppointmentBookedEvent>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.AddAsync(dto, 5);

        // Assert
        _repositoryMock.Verify(
            r => r.IsAvailable(dto.ScheduledDate, dto.DoctorId, dto.TimeSlot),
            Times.Once
        );

        _repositoryMock.Verify(
            r => r.AddAsync(It.Is<Appointment>(a =>
                a.PatientId == 5 &&
                a.DoctorId == dto.DoctorId &&
                a.ScheduledDate == dto.ScheduledDate &&
                a.TimeSlot == dto.TimeSlot &&
                a.Status == "Pending")),
            Times.Once
        );

        _publishEndpointMock.Verify(
            p => p.Publish(
                It.Is<AppointmentBookedEvent>(e =>
                    e.AppointmentId == 100 &&
                    e.PatientName == "Test Patient" &&
                    e.DoctorId == dto.DoctorId &&
                    e.ScheduledDate == dto.ScheduledDate &&
                    e.TimeSlot == dto.TimeSlot),
                It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task AvailableTimeSlots_ReturnsFreeSlots()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var doctorId = 1;

        _doctorServiceMock
            .Setup(d => d.GetSlots(doctorId))
            .ReturnsAsync(new List<string> { "09:00", "10:00" });

        _repositoryMock
            .Setup(r => r.BookedTimeSlots(date, doctorId))
            .ReturnsAsync(new List<string> { "09:00" });

        // Act
        var result = await _service.AvailableTimeSlots(date, doctorId);

        // Assert
        Assert.Single(result);
        Assert.Contains("10:00", result);
        Assert.DoesNotContain("09:00", result);
    }

    
    [Fact]
    public async Task AvailableTimeSlots_ReturnsEmptyList_WhenDoctorHasNoSlots()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var doctorId = 1;

        _doctorServiceMock
            .Setup(d => d.GetSlots(doctorId))
            .ReturnsAsync(new List<string>());

        // Act
        var result = await _service.AvailableTimeSlots(date, doctorId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        _repositoryMock.Verify(
            r => r.BookedTimeSlots(It.IsAny<DateOnly>(), It.IsAny<int>()),
            Times.Never
        );
    }

    [Fact]
    public async Task IsAvailable_Throws_WhenUnavailable()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var doctorId = 1;
        var timeSlot = "09:00";

        _repositoryMock
            .Setup(r => r.IsAvailable(date, doctorId, timeSlot))
            .ReturnsAsync(false);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.IsAvailable(date, doctorId, timeSlot));

        // Assert
        Assert.Equal("This time slot is already booked.", exception.Message);

        _repositoryMock.Verify(
            r => r.IsAvailable(date, doctorId, timeSlot),
            Times.Once
        );
    }

    [Fact]
    public async Task IsAvailable_ReturnsTrue_WhenAvailable()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var doctorId = 1;
        var timeSlot = "09:00";

        _repositoryMock
            .Setup(r => r.IsAvailable(date, doctorId, timeSlot))
            .ReturnsAsync(true);

        // Act
        var result = await _service.IsAvailable(date, doctorId, timeSlot);

        // Assert
        Assert.True(result);

        _repositoryMock.Verify(
            r => r.IsAvailable(date, doctorId, timeSlot),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateStatusAsync_UpdatesStatus_WhenAppointmentExists()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 5,
            DoctorId = 10,
            ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            TimeSlot = "09:00",
            Status = "Pending"
        };

        var dto = new UpdateAppointmentDto
        {
            Status = "Cancelled",
            CancellationReason = "Doctor unavailable"
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(appointment);

        _repositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateStatusAsync(1, dto);

        // Assert
        Assert.Equal("Cancelled", appointment.Status);
        Assert.Equal("Doctor unavailable", appointment.CancellationReason);

        _repositoryMock.Verify(
            r => r.UpdateAsync(It.Is<Appointment>(a =>
                a.AppointmentId == 1 &&
                a.Status == "Cancelled" &&
                a.CancellationReason == "Doctor unavailable")),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateStatusAsync_Throws_WhenAppointmentNotFound()
    {
        // Arrange
        var dto = new UpdateAppointmentDto
        {
            Status = "Cancelled",
            CancellationReason = "Patient request"
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Appointment?)null);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateStatusAsync(99, dto));

        // Assert
        Assert.Equal("Appointment not found.", exception.Message);

        _repositoryMock.Verify(
            r => r.UpdateAsync(It.IsAny<Appointment>()),
            Times.Never
        );
    }

    [Fact]
    public async Task DeleteAsync_ThrowsAppointmentNotFoundException_WhenAppointmentNotFound()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Appointment?)null);

        // Act and Assert
        await Assert.ThrowsAsync<AppointmentNotFoundException>(() =>
            _service.DeleteAsync(99));

        _repositoryMock.Verify(
            r => r.DeleteAsync(It.IsAny<int>()),
            Times.Never
        );
    }

    [Fact]
    public async Task DeleteAsync_DeletesAppointment_WhenAppointmentExists()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentId = 1,
            PatientId = 5,
            DoctorId = 10,
            ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            TimeSlot = "09:00",
            Status = "Pending"
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(appointment);

        _repositoryMock
            .Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _repositoryMock.Verify(
            r => r.DeleteAsync(1),
            Times.Once
        );
    }

    [Fact]
    public async Task GetReport_ReturnsReport_WhenDataExists()
    {
        // Arrange
        var fromDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-7));
        var toDate = DateOnly.FromDateTime(DateTime.Today);

        var filter = new AppointmentReportFilter
        {
            FromDate = fromDate,
            ToDate = toDate
        };

        var reports = new List<AppointmentReportDto>
        {
            new AppointmentReportDto
            {
                Date = DateOnly.FromDateTime(DateTime.Today),
                PendingCount = 2,
                ConfirmedCount = 3,
                CancelledCount = 1,
                CompletedCount = 4,
                Revenue = 2500
            }
        };

        _repositoryMock
            .Setup(r => r.GetReport(fromDate, toDate))
            .ReturnsAsync(reports);

        // Act
        var result = await _service.GetReport(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(DateOnly.FromDateTime(DateTime.Today), result[0].Date);
        Assert.Equal(2, result[0].PendingCount);
        Assert.Equal(3, result[0].ConfirmedCount);
        Assert.Equal(1, result[0].CancelledCount);
        Assert.Equal(4, result[0].CompletedCount);
        Assert.Equal(2500, result[0].Revenue);

        _repositoryMock.Verify(
            r => r.GetReport(fromDate, toDate),
            Times.Once
        );
    }

    [Fact]
    public async Task GetReport_ReturnsEmptyList_WhenNoDataExists()
    {
        // Arrange
        var fromDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-7));
        var toDate = DateOnly.FromDateTime(DateTime.Today);

        var filter = new AppointmentReportFilter
        {
            FromDate = fromDate,
            ToDate = toDate
        };

        _repositoryMock
            .Setup(r => r.GetReport(fromDate, toDate))
            .ReturnsAsync(new List<AppointmentReportDto>());

        // Act
        var result = await _service.GetReport(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        _repositoryMock.Verify(
            r => r.GetReport(fromDate, toDate),
            Times.Once
        );
    }

    [Fact]
    public async Task GetSummaryAsync_ReturnsSummary()
    {
        // Arrange
        var summary = new AppointmentSummaryDto
        {
            PendingCount = 3,
            ConfirmedCount = 4,
            CancelledCount = 1,
            CompletedCount = 2,
            TotalRevenue = 2500
        };

        _repositoryMock
            .Setup(r => r.GetSummaryAsync())
            .ReturnsAsync(summary);

        // Act
        var result = await _service.GetSummaryAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.PendingCount);
        Assert.Equal(4, result.ConfirmedCount);
        Assert.Equal(1, result.CancelledCount);
        Assert.Equal(2, result.CompletedCount);
        Assert.Equal(2500, result.TotalRevenue);

        _repositoryMock.Verify(
            r => r.GetSummaryAsync(),
            Times.Once
        );
    }

    [Fact]
    public async Task GetDashboardSummaryAsync_ReturnsSummary()
    {
        // Arrange
        var summary = new AppointmentSummaryDto
        {
            PendingCount = 5,
            ConfirmedCount = 8,
            CancelledCount = 2,
            CompletedCount = 5,
            TotalRevenue = 5000
        };

        _repositoryMock
            .Setup(r => r.GetDashboardSummaryAsync())
            .ReturnsAsync(summary);

        // Act
        var result = await _service.GetDashboardSummaryAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.PendingCount);
        Assert.Equal(8, result.ConfirmedCount);
        Assert.Equal(2, result.CancelledCount);
        Assert.Equal(5, result.CompletedCount);
        Assert.Equal(5000, result.TotalRevenue);

        _repositoryMock.Verify(
            r => r.GetDashboardSummaryAsync(),
            Times.Once
        );
    }

    [Fact]
    public async Task GetAllAsync_ReturnsPagedAppointments_WithSearchAndStatus()
    {
        // Arrange
        var filter = new AppointmentFilter
        {
            Search = "Test",
            Status = "Pending",
            PageNumber = 1,
            PageSize = 10,
            IsDescending = false
        };

        _repositoryMock
            .Setup(r => r.GetQueryable())
            .Returns(_context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .AsQueryable());

        // Act
        var result = await _service.GetAllAsync(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal("Test Patient", result.Items.First().PatientName);
        Assert.Equal("Dr John", result.Items.First().DoctorName);
        Assert.Equal("Pending", result.Items.First().Status);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsPagedAppointments_DescendingOrder()
    {
        // Arrange
        var filter = new AppointmentFilter
        {
            PageNumber = 1,
            PageSize = 10,
            IsDescending = true
        };

        _repositoryMock
            .Setup(r => r.GetQueryable())
            .Returns(_context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .AsQueryable());

        // Act
        var result = await _service.GetAllAsync(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(3, result.Items.First().AppointmentId);
    }

    [Fact]
    public async Task AddAsync_Throws_WhenTimeSlotUnavailable()
    {
        // Arrange
        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            TimeSlot = "09:00"
        };

        _repositoryMock
            .Setup(r => r.IsAvailable(dto.ScheduledDate, dto.DoctorId, dto.TimeSlot))
            .ReturnsAsync(false);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.AddAsync(dto, 5));

        // Assert
        Assert.Equal("This time slot is already booked.", exception.Message);

        _repositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Appointment>()),
            Times.Never
        );

        _publishEndpointMock.Verify(
            p => p.Publish(It.IsAny<AppointmentBookedEvent>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async Task AddAsync_PublishesEvent_WithUnknownPatient_WhenPatientNotFound()
    {
        // Arrange
        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            TimeSlot = "09:00"
        };

        var appointmentEntity = new Appointment
        {
            AppointmentId = 200,
            DoctorId = dto.DoctorId,
            PatientId = 999,
            ScheduledDate = dto.ScheduledDate,
            TimeSlot = dto.TimeSlot,
            Status = "Pending"
        };

        _repositoryMock
            .Setup(r => r.IsAvailable(dto.ScheduledDate, dto.DoctorId, dto.TimeSlot))
            .ReturnsAsync(true);

        _mapperMock
            .Setup(m => m.Map<Appointment>(dto))
            .Returns(appointmentEntity);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Appointment>()))
            .Returns(Task.CompletedTask);

        _publishEndpointMock
            .Setup(p => p.Publish(
                It.IsAny<AppointmentBookedEvent>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.AddAsync(dto, 999);

        // Assert
        _publishEndpointMock.Verify(
            p => p.Publish(
                It.Is<AppointmentBookedEvent>(e =>
                    e.AppointmentId == 200 &&
                    e.PatientName == "Unknown" &&
                    e.DoctorId == dto.DoctorId &&
                    e.ScheduledDate == dto.ScheduledDate &&
                    e.TimeSlot == dto.TimeSlot),
                It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
    [Fact]
    public async Task AddAsync_ThrowsInvalidOperationException_WhenDbUpdateExceptionOccurs()
    {
        // Arrange
        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            TimeSlot = "09:00"
        };

        var appointmentEntity = new Appointment
        {
            AppointmentId = 300,
            DoctorId = dto.DoctorId,
            PatientId = 5,
            ScheduledDate = dto.ScheduledDate,
            TimeSlot = dto.TimeSlot,
            Status = "Pending"
        };

        _repositoryMock
            .Setup(r => r.IsAvailable(dto.ScheduledDate, dto.DoctorId, dto.TimeSlot))
            .ReturnsAsync(true);

        _mapperMock
            .Setup(m => m.Map<Appointment>(dto))
            .Returns(appointmentEntity);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Appointment>()))
            .ThrowsAsync(new DbUpdateException("Database error"));

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.AddAsync(dto, 5));

        // Assert
        Assert.Equal("Failed to book the appointment.", exception.Message);

        _publishEndpointMock.Verify(
            p => p.Publish(It.IsAny<AppointmentBookedEvent>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }
    [Fact]
    public async Task UpdateAsync_UpdatesAppointment_WhenAppointmentExists()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Pending"
        };

        var dto = new UpdateAppointmentDto
        {
            Status = "Confirmed"
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(appointment);

        _repositoryMock
            .Setup(r => r.UpdateAsync(appointment))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(1, dto);

        // Assert
        _mapperMock.Verify(
            m => m.Map(dto, appointment),
            Times.Once
        );

        _repositoryMock.Verify(
            r => r.UpdateAsync(appointment),
            Times.Once
        );
    }
    [Fact]
    public async Task UpdateAsync_Throws_WhenAppointmentNotFound()
    {
        // Arrange
        var dto = new UpdateAppointmentDto
        {
            Status = "Confirmed"
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Appointment?)null);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateAsync(99, dto));

        // Assert
        Assert.Equal("Appointment not found.", exception.Message);

        _repositoryMock.Verify(
            r => r.UpdateAsync(It.IsAny<Appointment>()),
            Times.Never
        );
    }
    [Fact]
    public async Task UpdateAsync_ThrowsInvalidOperationException_WhenDbUpdateExceptionOccurs()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Pending"
        };

        var dto = new UpdateAppointmentDto
        {
            Status = "Confirmed"
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(appointment);

        _repositoryMock
            .Setup(r => r.UpdateAsync(appointment))
            .ThrowsAsync(new DbUpdateException("Database error"));

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateAsync(1, dto));

        // Assert
        Assert.Equal("Failed to update appointment.", exception.Message);
    }
    [Fact]
    public async Task UpdateStatusAsync_ThrowsInvalidOperationException_WhenDbUpdateExceptionOccurs()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Pending"
        };

        var dto = new UpdateAppointmentDto
        {
            Status = "Cancelled",
            CancellationReason = "Patient request"
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(appointment);

        _repositoryMock
            .Setup(r => r.UpdateAsync(appointment))
            .ThrowsAsync(new DbUpdateException("Database error"));

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateStatusAsync(1, dto));

        // Assert
        Assert.Equal("Failed to update appointment status.", exception.Message);
    }
    [Fact]
    public async Task DeleteAsync_ThrowsInvalidOperationException_WhenDbUpdateExceptionOccurs()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentId = 1
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(appointment);

        _repositoryMock
            .Setup(r => r.DeleteAsync(1))
            .ThrowsAsync(new DbUpdateException("Database error"));

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.DeleteAsync(1));

        // Assert
        Assert.Equal(
            "Failed to delete appointment. It may be referenced by existing health records.",
            exception.Message
        );
    }
    [Fact]
    public async Task GetReport_UsesDefaultDates_WhenFilterDatesAreNull()
    {
        // Arrange
        var filter = new AppointmentReportFilter
        {
            FromDate = null,
            ToDate = null
        };

        var expectedFromDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-7));
        var expectedToDate = DateOnly.FromDateTime(DateTime.Today);

        var reports = new List<AppointmentReportDto>
    {
        new AppointmentReportDto
        {
            Date = expectedToDate,
            PendingCount = 1,
            ConfirmedCount = 2,
            CancelledCount = 0,
            CompletedCount = 3,
            Revenue = 1000
        }
    };

        _repositoryMock
            .Setup(r => r.GetReport(expectedFromDate, expectedToDate))
            .ReturnsAsync(reports);

        // Act
        var result = await _service.GetReport(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(1000, result[0].Revenue);

        _repositoryMock.Verify(
            r => r.GetReport(expectedFromDate, expectedToDate),
            Times.Once
        );
    }
    [Fact]
    public async Task GetDoctorSchedule_ReturnsEmptyList_WhenNoScheduleExists()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today);
        var doctorId = 1;

        _repositoryMock
            .Setup(r => r.GetDoctorSchedule(date, doctorId))
            .ReturnsAsync(new List<AppointmentListDto>());

        // Act
        var result = await _service.GetDoctorSchedule(date, doctorId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    [Fact]
    public async Task GetDoctorSchedule_ReturnsSchedule_WhenScheduleExists()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today);
        var doctorId = 1;

        var schedule = new List<AppointmentListDto>
    {
        new AppointmentListDto
        {
            AppointmentId = 1,
            DoctorName = "Dr John",
            PatientName = "Test Patient",
            ScheduledDate = date,
            TimeSlot = "09:00",
            Status = "Pending"
        }
    };

        _repositoryMock
            .Setup(r => r.GetDoctorSchedule(date, doctorId))
            .ReturnsAsync(schedule);

        // Act
        var result = await _service.GetDoctorSchedule(date, doctorId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Dr John", result[0].DoctorName);
    }

    [Fact]
    public async Task GetPatientSchedule_ReturnsEmptyList_WhenNoScheduleExists()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today);
        var patientId = 5;

        _repositoryMock
            .Setup(r => r.GetPatientSchedule(date, patientId))
            .ReturnsAsync(new List<AppointmentListDto>());

        // Act
        var result = await _service.GetPatientSchedule(date, patientId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    [Fact]
    public async Task GetPatientSchedule_ReturnsSchedule_WhenScheduleExists()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today);
        var patientId = 5;

        var schedule = new List<AppointmentListDto>
    {
        new AppointmentListDto
        {
            AppointmentId = 1,
            PatientId = patientId,
            PatientName = "Test Patient",
            DoctorName = "Dr John",
            ScheduledDate = date,
            TimeSlot = "09:00",
            Status = "Pending"
        }
    };

        _repositoryMock
            .Setup(r => r.GetPatientSchedule(date, patientId))
            .ReturnsAsync(schedule);

        // Act
        var result = await _service.GetPatientSchedule(date, patientId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test Patient", result[0].PatientName);
    }
    [Fact]
    public async Task GetAppointmentByPatient_ReturnsEmptyList_WhenNoAppointmentsExist()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetAppointmentByPatient(5))
            .ReturnsAsync(new List<AppointmentListDto>());

        // Act
        var result = await _service.GetAppointmentByPatient(5);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    [Fact]
    public async Task GetAppointmentByPatient_ReturnsAppointments_WhenAppointmentsExist()
    {
        // Arrange
        var appointments = new List<AppointmentListDto>
    {
        new AppointmentListDto
        {
            AppointmentId = 1,
            PatientId = 5,
            PatientName = "Test Patient",
            DoctorName = "Dr John",
            TimeSlot = "09:00",
            Status = "Confirmed"
        }
    };

        _repositoryMock
            .Setup(r => r.GetAppointmentByPatient(5))
            .ReturnsAsync(appointments);

        // Act
        var result = await _service.GetAppointmentByPatient(5);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Confirmed", result[0].Status);
    }
    [Fact]
    public async Task GetAppointmentByDoctor_ReturnsEmptyList_WhenNoAppointmentsExist()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetAppointmentByDoctor(1))
            .ReturnsAsync(new List<AppointmentListDto>());

        // Act
        var result = await _service.GetAppointmentByDoctor(1);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    [Fact]
    public async Task GetAppointmentByDoctor_ReturnsAppointments_WhenAppointmentsExist()
    {
        // Arrange
        var appointments = new List<AppointmentListDto>
    {
        new AppointmentListDto
        {
            AppointmentId = 1,
            DoctorName = "Dr John",
            PatientName = "Test Patient",
            TimeSlot = "09:00",
            Status = "Confirmed"
        }
    };

        _repositoryMock
            .Setup(r => r.GetAppointmentByDoctor(1))
            .ReturnsAsync(appointments);

        // Act
        var result = await _service.GetAppointmentByDoctor(1);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Confirmed", result[0].Status);
    }
    [Fact]
    public async Task CancelAppointmentsByDoctorDate_CallsRepository_WhenSuccessful()
    {
        // Arrange
        var doctorId = 1;
        var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

        _repositoryMock
            .Setup(r => r.CancelAppointmentsByDoctorDate(doctorId, date))
            .Returns(Task.CompletedTask);

        // Act
        await _service.CancelAppointmentsByDoctorDate(doctorId, date);

        // Assert
        _repositoryMock.Verify(
            r => r.CancelAppointmentsByDoctorDate(doctorId, date),
            Times.Once
        );
    }
    [Fact]
    public async Task CancelAppointmentsByDoctorDate_ThrowsInvalidOperationException_WhenDbUpdateExceptionOccurs()
    {
        // Arrange
        var doctorId = 1;
        var date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

        _repositoryMock
            .Setup(r => r.CancelAppointmentsByDoctorDate(doctorId, date))
            .ThrowsAsync(new DbUpdateException("Database error"));

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CancelAppointmentsByDoctorDate(doctorId, date));

        // Assert
        Assert.Equal("Failed to cancel appointments.", exception.Message);
    }
}
