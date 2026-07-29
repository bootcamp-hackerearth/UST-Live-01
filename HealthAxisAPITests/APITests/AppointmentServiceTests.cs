using AutoMapper;
using FluentAssertions;
using HealthAxisApplicn.Dto.Appointments;
using HealthAxisApplicn.Messaging.Contracts;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Repositories;
using HealthAxisApplicn.Services;
using HealthAxisApplicn.Services.Impl;
using MassTransit;
using MassTransit.Transports;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AppointmentService _service;
    private readonly Mock<IHealthRecordService> _healthRecordMock;
    private readonly Mock<IPublishEndpoint> _publishMock;


    public AppointmentServiceTests()
    {
        _repoMock = new Mock<IAppointmentRepository>();
        _mapperMock = new Mock<IMapper>();
        _healthRecordMock = new Mock<IHealthRecordService>();
        _publishMock = new Mock<IPublishEndpoint>();
        _service = new AppointmentService(
            _repoMock.Object,
            _healthRecordMock.Object,
            _mapperMock.Object,
            _publishMock.Object);

    }

    [Fact]
    public async Task CreateAsync_Should_Create_Appointment()
    {
        var dto = new CreateAppointmentDto
        {
            ScheduledDate = DateTime.UtcNow.AddDays(1), 
            TimeSlot = "10:00",
            DoctorId = 1
        };

        var appointment = new Appointment();
        var resultDto = new AppointmentDto();
        var patientId = 1;

        _mapperMock.Setup(m => m.Map<Appointment>(dto))
                   .Returns(appointment);

        _repoMock.Setup(r =>
            r.DoctorHasConflictAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), default))
            .ReturnsAsync(false);

        _repoMock.Setup(r =>
            r.PatientHasConflictAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), default))
            .ReturnsAsync(false);

        _repoMock.Setup(r =>
            r.PatientHasAppointmentOnDateAsync(It.IsAny<int>(), It.IsAny<DateTime>(), default))
            .ReturnsAsync(false);

        _repoMock.Setup(r =>
            r.CreateAsync(It.IsAny<Appointment>(), default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(m => m.Map<AppointmentDto>(appointment))
                   .Returns(resultDto);

        var result = await _service.CreateAsync(dto, patientId);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_List()
    {
        var list = new List<Appointment>
    {
        new Appointment { AppointmentId = 1 }
    };

        var dtos = new List<AppointmentDto>
    {
        new AppointmentDto { AppointmentId = 1 }
    };

        _repoMock.Setup(r =>r.GetAllAppointmentsAsync(1, 10)).ReturnsAsync(list);

        _mapperMock.Setup(m =>m.Map<List<AppointmentDto>>(list)).Returns(dtos);

        var result =
            await _service.GetAllAsync(1, 10);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Appointment()
    {
        var appointment = new Appointment { AppointmentId = 1 };
        var dto = new AppointmentDto { AppointmentId = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(appointment);
        _mapperMock.Setup(m => m.Map<AppointmentDto?>(appointment)).Returns(dto);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.AppointmentId.Should().Be(1);
    }

    [Fact]
    public async Task DeleteAppointmentAsync_Should_Return_True_When_Deleted()
    {
        _repoMock.Setup(r => r.DeleteAsync(1, default)).ReturnsAsync(true);

        var result = await _service.DeleteAppointmentAsync(1);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetAppointmentsByDoctorIdAsync_Should_Return_List()
    {
        var list = new List<Appointment> { new Appointment() };
        var dtos = new List<AppointmentDto> { new AppointmentDto() };


        _repoMock.Setup(r =>r.GetUpcomingAppointmentsByDoctorIdAsync(1,1,10,default))
        .ReturnsAsync(list);        
        _mapperMock.Setup(m => m.Map<List<AppointmentDto>>(list)).Returns(dtos);


        var result = await _service.GetAppointmentsByDoctorIdAsync(1, 1, 10);


        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetAppointmentsByPatientIdAsync_Should_Return_List()
    {
        var list = new List<Appointment> { new Appointment() };
        var dtos = new List<AppointmentDto> { new AppointmentDto() };


        _repoMock.Setup(r =>r.GetAppointmentsByPatientIdAsync(1,1,10,default)).ReturnsAsync(list);
        _mapperMock.Setup(m => m.Map<List<AppointmentDto>>(list)).Returns(dtos);


        var result = await _service.GetAppointmentsByPatientIdAsync(1, 1, 10);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Null_When_NotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync((Appointment?)null);

        var result = await _service.UpdateAsync(1, new UpdateAppointmentStatusDto(), "Doctor");
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Completed()
    {
        var existing = new Appointment { AppointmentId = 1, Status = "Completed" };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(existing);


        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateAsync(1, new UpdateAppointmentStatusDto { Status = "Cancelled" }, "Doctor"));

    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Cancelled_Without_Reason()
    {
        var existing = new Appointment { AppointmentId = 1, Status = "Scheduled" };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(existing);


        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateAsync(1, new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = ""
            }, "Doctor"));

    }

    [Fact]
    public async Task UpdateAsync_Should_Update_When_Valid()
    {
        var existing = new Appointment { AppointmentId = 1, Status = "Scheduled" };
        var updated = new Appointment { AppointmentId = 1, Status = "Cancelled" };
        var dto = new AppointmentDto { AppointmentId = 1, Status = "Cancelled" };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.UpdateAsync(1, existing, default)).ReturnsAsync(updated);
        _mapperMock.Setup(m => m.Map<AppointmentDto?>(updated)).Returns(dto);


        var result = await _service.UpdateAsync(1, new UpdateAppointmentStatusDto
        {
            Status = "Cancelled",
            CancellationReason = "Patient request"
        }, "Doctor");


        result.Status.Should().Be("Cancelled");
    }

    [Fact]
    public async Task UpdateAsync_Should_Call_CreateFromAppointment_When_Completed()
    {
        var existing = new Appointment { AppointmentId = 1, Status = "Confirmed" };

        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.UpdateAsync(1, existing, default))
                 .ReturnsAsync(existing);

        _mapperMock.Setup(m => m.Map<AppointmentDto>(existing))
                   .Returns(new AppointmentDto());


        await _service.UpdateAsync(1, new UpdateAppointmentStatusDto
        {
            Status = "Completed"
        }, "Doctor");


        _healthRecordMock.Verify(x =>
            x.CreateFromAppointment(It.IsAny<Appointment>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Booking_In_Past()
    {
        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateTime.UtcNow.AddDays(-1),
            TimeSlot = "10:00"
        };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(dto, 1));
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_TimeSlot_Invalid()
    {
        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateTime.UtcNow.AddDays(1),
            TimeSlot = "INVALID"
        };

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateAsync(dto, 1));
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Doctor_Has_Conflict()
    {
        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateTime.UtcNow.AddDays(1),
            TimeSlot = "10:00"
        };

        _repoMock.Setup(x =>
            x.DoctorHasConflictAsync(
                dto.DoctorId,
                dto.ScheduledDate,
                dto.TimeSlot,
                default))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(dto, 1));
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Patient_Has_Conflict()
    {
        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateTime.UtcNow.AddDays(1),
            TimeSlot = "10:00"
        };

        _repoMock.Setup(x =>
            x.DoctorHasConflictAsync(
                It.IsAny<int>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>(),
                default))
            .ReturnsAsync(false);

        _repoMock.Setup(x =>
            x.PatientHasConflictAsync(
                It.IsAny<int>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>(),
                default))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(dto, 1));
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Daily_Limit_Reached()
    {
        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateTime.UtcNow.AddDays(1),
            TimeSlot = "10:00"
        };

        _repoMock.Setup(x => x.DoctorHasConflictAsync(
            It.IsAny<int>(),
            It.IsAny<DateTime>(),
            It.IsAny<string>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.PatientHasConflictAsync(
            It.IsAny<int>(),
            It.IsAny<DateTime>(),
            It.IsAny<string>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.PatientHasAppointmentOnDateAsync(
            It.IsAny<int>(),
            It.IsAny<DateTime>(),
            default))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(dto, 1));
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Status_Invalid()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Pending"
        };

        _repoMock.Setup(x =>
            x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = "InvalidStatus"
                },
                "Doctor"));
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Patient_Tries_To_Confirm()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Pending"
        };

        _repoMock.Setup(x =>
            x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = "Confirmed"
                },
                "Patient"));
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Patient_Cancels_Confirmed_Appointment()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Confirmed"
        };

        _repoMock.Setup(x =>
            x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = "Cancelled",
                    CancellationReason = "Personal"
                },
                "Patient"));
    }

    [Fact]
    public async Task UpdateAsync_Should_Allow_Doctor_To_Confirm_Pending()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Pending"
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        _repoMock.Setup(x => x.UpdateAsync(1, appointment, default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(x => x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        var result = await _service.UpdateAsync(
            1,
            new UpdateAppointmentStatusDto
            {
                Status = "Confirmed"
            },
            "Doctor");

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Pending_To_Completed_Directly()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Pending"
        };

        _repoMock.Setup(x =>
            x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = "Completed"
                },
                "Doctor"));
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Confirmed_To_Pending()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Confirmed"
        };

        _repoMock.Setup(x =>
            x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = "Pending"
                },
                "Doctor"));
    }

    [Fact]
    public async Task UpdateAsync_Should_Clear_CancellationReason_When_Not_Cancelled()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Confirmed",
            CancellationReason = "Old reason"
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        _repoMock.Setup(x => x.UpdateAsync(1, appointment, default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(x => x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        await _service.UpdateAsync(
            1,
            new UpdateAppointmentStatusDto
            {
                Status = "Completed"
            },
            "Doctor");

        appointment.CancellationReason.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_Should_Set_PatientId()
    {
        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateTime.UtcNow.AddDays(1),
            TimeSlot = "10:00"
        };

        var appointment = new Appointment();

        _mapperMock.Setup(x => x.Map<Appointment>(dto))
            .Returns(appointment);

        _repoMock.Setup(x => x.DoctorHasConflictAsync(
            It.IsAny<int>(),
            It.IsAny<DateTime>(),
            It.IsAny<string>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.PatientHasConflictAsync(
            It.IsAny<int>(),
            It.IsAny<DateTime>(),
            It.IsAny<string>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.PatientHasAppointmentOnDateAsync(
            It.IsAny<int>(),
            It.IsAny<DateTime>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.CreateAsync(appointment, default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(x => x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        await _service.CreateAsync(dto, 99);

        appointment.PatientId.Should().Be(99);
    }

    [Fact]
    public async Task CreateAsync_Should_Publish_Event()
    {
        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateTime.UtcNow.AddDays(1),
            TimeSlot = "10:00"
        };

        var appointment = new Appointment
        {
            AppointmentId = 1,
            DoctorId = 1,
            PatientId = 2,
            ScheduledDate = dto.ScheduledDate,
            TimeSlot = dto.TimeSlot
        };

        _mapperMock.Setup(x => x.Map<Appointment>(dto))
            .Returns(appointment);

        _repoMock.Setup(x => x.DoctorHasConflictAsync(It.IsAny<int>(),
            It.IsAny<DateTime>(),
            It.IsAny<string>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.PatientHasConflictAsync(It.IsAny<int>(),
            It.IsAny<DateTime>(),
            It.IsAny<string>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.PatientHasAppointmentOnDateAsync(It.IsAny<int>(),
            It.IsAny<DateTime>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.CreateAsync(It.IsAny<Appointment>(), default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(x => x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        await _service.CreateAsync(dto, 2);

        _publishMock.Verify(
            x => x.Publish(
                It.IsAny<BookAppointmentEvent>(),
                default),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAppointmentAsync_Should_Return_False()
    {
        _repoMock.Setup(x => x.DeleteAsync(1, default))
            .ReturnsAsync(false);

        var result =
            await _service.DeleteAppointmentAsync(1);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null()
    {
        _repoMock.Setup(x => x.GetByIdAsync(100, default))
            .ReturnsAsync((Appointment?)null);

        _mapperMock.Setup(x =>
            x.Map<AppointmentDto?>((Appointment?)null))
            .Returns((AppointmentDto?)null);

        var result = await _service.GetByIdAsync(100);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAppointmentsByDoctorNameAsync_Should_Return_List()
    {
        var appointments = new List<Appointment>
    {
        new()
    };

        var dtos = new List<AppointmentDto>
    {
        new()
    };

        _repoMock.Setup(x =>
            x.GetAppointmentsByDoctorNameAsync("John", default))
            .ReturnsAsync(appointments);

        _mapperMock.Setup(x =>
            x.Map<List<AppointmentDto>>(appointments))
            .Returns(dtos);

        var result =
            await _service.GetAppointmentsByDoctorNameAsync("John");

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetAppointmentsByPatientNameAsync_Should_Return_List()
    {
        var appointments = new List<Appointment>
    {
        new()
    };

        var dtos = new List<AppointmentDto>
    {
        new()
    };

        _repoMock.Setup(x =>
            x.GetAppointmentsByPatientNameAsync("John", default))
            .ReturnsAsync(appointments);

        _mapperMock.Setup(x =>
            x.Map<List<AppointmentDto>>(appointments))
            .Returns(dtos);

        var result =
            await _service.GetAppointmentsByPatientNameAsync("John");

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetTodayAppointmentsAsync_Should_Return_List()
    {
        var appointments = new List<Appointment>
    {
        new()
    };

        var dtos = new List<AppointmentDto>
    {
        new()
    };

        _repoMock.Setup(x =>
            x.GetTodayAppointmentsAsync(
                1,
                1,
                10,
                default))
            .ReturnsAsync(appointments);

        _mapperMock.Setup(x =>
            x.Map<List<AppointmentDto>>(appointments))
            .Returns(dtos);

        var result =
            await _service.GetTodayAppointmentsAsync(
                1,
                1,
                10);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task UpdateAsync_Should_Allow_Pending_To_Cancelled()
    {
        var appointment = new Appointment
        {
            Status = "Pending"
        };

        _repoMock.Setup(x =>
            x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        _repoMock.Setup(x =>
            x.UpdateAsync(1, appointment, default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(x =>
            x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        var result = await _service.UpdateAsync(
            1,
            new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = "Reason"
            },
            "Doctor");

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Allow_Confirmed_To_Cancelled()
    {
        var appointment = new Appointment
        {
            Status = "Confirmed"
        };

        _repoMock.Setup(x =>
            x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        _repoMock.Setup(x =>
            x.UpdateAsync(1, appointment, default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(x =>
            x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        var result = await _service.UpdateAsync(
            1,
            new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = "Doctor unavailable"
            },
            "Doctor");

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Set_CancellationReason()
    {
        var appointment = new Appointment
        {
            Status = "Pending"
        };

        _repoMock.Setup(x =>
            x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        _repoMock.Setup(x =>
            x.UpdateAsync(1, appointment, default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(x =>
            x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        await _service.UpdateAsync(
            1,
            new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = "Reason"
            },
            "Doctor");

        appointment.CancellationReason.Should().Be("Reason");
    }

    [Fact]
    public async Task UpdateAsync_Should_Not_Create_HealthRecord_For_Cancelled()
    {
        var appointment = new Appointment
        {
            Status = "Confirmed"
        };

        _repoMock.Setup(x =>
            x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        _repoMock.Setup(x =>
            x.UpdateAsync(1, appointment, default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(x =>
            x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        await _service.UpdateAsync(
            1,
            new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = "Reason"
            },
            "Doctor");

        _healthRecordMock.Verify(
            x => x.CreateFromAppointment(
                It.IsAny<Appointment>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Previous_Status_Is_Cancelled()
    {
        var appointment = new Appointment
        {
            AppointmentId = 1,
            Status = "Cancelled"
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateAsync(
                1,
                new UpdateAppointmentStatusDto
                {
                    Status = "Confirmed"
                },
                "Doctor"));
    }

    [Fact]
    public async Task UpdateAsync_Should_Allow_Patient_To_Cancel_Pending()
    {
        var appointment = new Appointment
        {
            Status = "Pending"
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        _repoMock.Setup(x => x.UpdateAsync(1, appointment, default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(x => x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        var result = await _service.UpdateAsync(
            1,
            new UpdateAppointmentStatusDto
            {
                Status = "Cancelled",
                CancellationReason = "Patient request"
            },
            "Patient");

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Allow_Confirmed_To_Completed()
    {
        var appointment = new Appointment
        {
            Status = "Confirmed"
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        _repoMock.Setup(x => x.UpdateAsync(1, appointment, default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(x => x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        var result = await _service.UpdateAsync(
            1,
            new UpdateAppointmentStatusDto
            {
                Status = "Completed"
            },
            "Doctor");

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Status()
    {
        var appointment = new Appointment
        {
            Status = "Pending"
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        _repoMock.Setup(x => x.UpdateAsync(1, appointment, default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(x => x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        await _service.UpdateAsync(
            1,
            new UpdateAppointmentStatusDto
            {
                Status = "Confirmed"
            },
            "Doctor");

        appointment.Status.Should().Be("Confirmed");
    }

    [Fact]
    public async Task UpdateAsync_Should_Call_Update_Repository()
    {
        var appointment = new Appointment
        {
            Status = "Pending"
        };

        _repoMock.Setup(x => x.GetByIdAsync(1, default))
            .ReturnsAsync(appointment);

        _repoMock.Setup(x => x.UpdateAsync(1, appointment, default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(x => x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        await _service.UpdateAsync(
            1,
            new UpdateAppointmentStatusDto
            {
                Status = "Confirmed"
            },
            "Doctor");

        _repoMock.Verify(
            x => x.UpdateAsync(1, appointment, default),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Should_Set_Status_To_Pending()
    {
        var appointment = new Appointment();

        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateTime.UtcNow.AddDays(1),
            TimeSlot = "10:00"
        };

        _mapperMock.Setup(x => x.Map<Appointment>(dto))
            .Returns(appointment);

        _repoMock.Setup(x => x.DoctorHasConflictAsync(
            It.IsAny<int>(),
            It.IsAny<DateTime>(),
            It.IsAny<string>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.PatientHasConflictAsync(
            It.IsAny<int>(),
            It.IsAny<DateTime>(),
            It.IsAny<string>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.PatientHasAppointmentOnDateAsync(
            It.IsAny<int>(),
            It.IsAny<DateTime>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.CreateAsync(appointment, default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(x => x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        await _service.CreateAsync(dto, 1);

        appointment.Status.Should().Be("Pending");
    }

    [Fact]
    public async Task CreateAsync_Should_Call_CreateAsync_Repository()
    {
        var appointment = new Appointment();

        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateTime.UtcNow.AddDays(1),
            TimeSlot = "10:00"
        };

        _mapperMock.Setup(x => x.Map<Appointment>(dto))
            .Returns(appointment);

        _repoMock.Setup(x => x.DoctorHasConflictAsync(
            It.IsAny<int>(),
            It.IsAny<DateTime>(),
            It.IsAny<string>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.PatientHasConflictAsync(
            It.IsAny<int>(),
            It.IsAny<DateTime>(),
            It.IsAny<string>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.PatientHasAppointmentOnDateAsync(
            It.IsAny<int>(),
            It.IsAny<DateTime>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.CreateAsync(appointment, default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(x => x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        await _service.CreateAsync(dto, 1);

        _repoMock.Verify(
            x => x.CreateAsync(appointment, default),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Should_Not_Publish_Event_When_DoctorConflict()
    {
        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateTime.UtcNow.AddDays(1),
            TimeSlot = "10:00"
        };

        _repoMock.Setup(x =>
            x.DoctorHasConflictAsync(
                It.IsAny<int>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>(),
                default))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(dto, 1));

        _publishMock.Verify(
            x => x.Publish(
                It.IsAny<BookAppointmentEvent>(),
                default),
            Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(x =>
            x.GetAllAppointmentsAsync(1, 10))
            .ReturnsAsync(new List<Appointment>());

        _mapperMock.Setup(x =>
            x.Map<List<AppointmentDto>>(
                It.IsAny<List<Appointment>>()))
            .Returns(new List<AppointmentDto>());

        var result = await _service.GetAllAsync(1, 10);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTodayAppointmentsAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(x =>
            x.GetTodayAppointmentsAsync(
                1,
                1,
                10,
                default))
            .ReturnsAsync(new List<Appointment>());

        _mapperMock.Setup(x =>
            x.Map<List<AppointmentDto>>(
                It.IsAny<List<Appointment>>()))
            .Returns(new List<AppointmentDto>());

        var result =
            await _service.GetTodayAppointmentsAsync(
                1,
                1,
                10);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAppointmentsByDoctorNameAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(x =>
            x.GetAppointmentsByDoctorNameAsync("Unknown", default))
            .ReturnsAsync(new List<Appointment>());

        _mapperMock.Setup(x =>
            x.Map<List<AppointmentDto>>(It.IsAny<List<Appointment>>()))
            .Returns(new List<AppointmentDto>());

        var result =
            await _service.GetAppointmentsByDoctorNameAsync("Unknown");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAppointmentsByPatientNameAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(x =>
            x.GetAppointmentsByPatientNameAsync("Unknown", default))
            .ReturnsAsync(new List<Appointment>());

        _mapperMock.Setup(x =>
            x.Map<List<AppointmentDto>>(It.IsAny<List<Appointment>>()))
            .Returns(new List<AppointmentDto>());

        var result =
            await _service.GetAppointmentsByPatientNameAsync("Unknown");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAppointmentsByDoctorIdAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(x =>
            x.GetUpcomingAppointmentsByDoctorIdAsync(
                1, 1, 10, default))
            .ReturnsAsync(new List<Appointment>());

        _mapperMock.Setup(x =>
            x.Map<List<AppointmentDto>>(It.IsAny<List<Appointment>>()))
            .Returns(new List<AppointmentDto>());

        var result =
            await _service.GetAppointmentsByDoctorIdAsync(1, 1, 10);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAppointmentsByPatientIdAsync_Should_Return_Empty_List()
    {
        _repoMock.Setup(x =>
            x.GetAppointmentsByPatientIdAsync(
                1, 1, 10, default))
            .ReturnsAsync(new List<Appointment>());

        _mapperMock.Setup(x =>
            x.Map<List<AppointmentDto>>(It.IsAny<List<Appointment>>()))
            .Returns(new List<AppointmentDto>());

        var result =
            await _service.GetAppointmentsByPatientIdAsync(1, 1, 10);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateAsync_Should_Call_Publish_Once()
    {
        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateTime.UtcNow.AddDays(1),
            TimeSlot = "10:00"
        };

        var appointment = new Appointment();

        _mapperMock.Setup(x => x.Map<Appointment>(dto))
            .Returns(appointment);

        _repoMock.Setup(x => x.DoctorHasConflictAsync(
            It.IsAny<int>(),
            It.IsAny<DateTime>(),
            It.IsAny<string>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.PatientHasConflictAsync(
            It.IsAny<int>(),
            It.IsAny<DateTime>(),
            It.IsAny<string>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x => x.PatientHasAppointmentOnDateAsync(
            It.IsAny<int>(),
            It.IsAny<DateTime>(),
            default))
            .ReturnsAsync(false);

        _repoMock.Setup(x =>
            x.CreateAsync(It.IsAny<Appointment>(), default))
            .ReturnsAsync(appointment);

        _mapperMock.Setup(x =>
            x.Map<AppointmentDto>(appointment))
            .Returns(new AppointmentDto());

        await _service.CreateAsync(dto, 1);

        _publishMock.Verify(
            x => x.Publish(
                It.IsAny<BookAppointmentEvent>(),
                default),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Should_Not_Call_CreateAsync_When_DoctorConflict()
    {
        var dto = new CreateAppointmentDto
        {
            DoctorId = 1,
            ScheduledDate = DateTime.UtcNow.AddDays(1),
            TimeSlot = "10:00"
        };

        _repoMock.Setup(x =>
            x.DoctorHasConflictAsync(
                It.IsAny<int>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>(),
                default))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(dto, 1));

        _repoMock.Verify(
            x => x.CreateAsync(
                It.IsAny<Appointment>(),
                default),
            Times.Never);
    }

}