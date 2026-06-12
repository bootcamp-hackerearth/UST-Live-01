using AutoMapper;
using HealthApp.API.Data;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.Shared.Constant;
using HealthApp.Shared.DTOs;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HealthApp.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _repoMock = new Mock<IAppointmentRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new AppointmentService(_repoMock.Object, _mapperMock.Object);
        }


        private AppointmentDto CreateDto()
        {
            return new AppointmentDto
            {
                DoctorId = 2,
                PatientId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };
        }

        private Appointment CreateAppointment()
        {
            return new Appointment
            {
                AppointmentId = 10,
                DoctorId = 2,
                PatientId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM",
                Status = AppointmentStatus.Pending
            };
        }

        private Doctor CreateDoctor()
        {
            return new Doctor
            {
                DoctorId = 2,
                IsActive = true
            };
        }

        [Fact]
        public async Task Add_WhenPastDate_ShouldThrow()
        {
            var dto = CreateDto();
            dto.ScheduledDate = DateTime.Today.AddDays(-1);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.Add(dto));
        }

        [Fact]
        public async Task Add_WhenDoctorNotFound_ShouldThrow()
        {
            var dto = CreateDto();

            _repoMock.Setup(r => r.GetDoctorById(dto.DoctorId))
                     .ReturnsAsync((Doctor)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.Add(dto));
        }

        [Fact]
        public async Task Add_WhenDoctorInactive_ShouldThrow()
        {
            var dto = CreateDto();

            _repoMock.Setup(r => r.GetDoctorById(dto.DoctorId))
                     .ReturnsAsync(new Doctor { DoctorId = 2, IsActive = false });

            await Assert.ThrowsAsync<Exception>(() =>
                _service.Add(dto));
        }

        [Fact]
        public async Task Add_WhenInvalidSlot_ShouldThrow()
        {
            var dto = CreateDto();
            dto.TimeSlot = "99:99 AM";

            _repoMock.Setup(r => r.GetDoctorById(dto.DoctorId))
                     .ReturnsAsync(CreateDoctor());

            await Assert.ThrowsAsync<Exception>(() =>
                _service.Add(dto));
        }

        [Fact]
        public async Task Add_WhenSlotAlreadyBooked_ShouldThrow()
        {
            var dto = CreateDto();

            _repoMock.Setup(r => r.GetDoctorById(dto.DoctorId))
                     .ReturnsAsync(CreateDoctor());

            _repoMock.Setup(r => r.IsSlotBooked(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot))
                     .ReturnsAsync(true);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.Add(dto));
        }

        [Fact]
        public async Task Add_WhenValid_ShouldCallRepoAdd()
        {
            var dto = CreateDto();

            _repoMock.Setup(r => r.GetDoctorById(dto.DoctorId))
                     .ReturnsAsync(CreateDoctor());

            _repoMock.Setup(r => r.IsSlotBooked(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                     .ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map<Appointment>(It.IsAny<AppointmentDto>()))
                       .Returns(CreateAppointment());

            _repoMock.Setup(r => r.Add(It.IsAny<Appointment>()))
                     .Returns(Task.CompletedTask);

            await _service.Add(dto);

            _repoMock.Verify(r => r.Add(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnList()
        {
            var data = new List<Appointment> { CreateAppointment() };

            _repoMock.Setup(r => r.GetAll())
                     .ReturnsAsync(data);

            _mapperMock.Setup(m => m.Map<List<AppointmentDto>>(data))
                       .Returns(new List<AppointmentDto> { CreateDto() });

            var result = await _service.GetAllAppointments();

            Assert.Single(result);
        }


        [Fact]
        public async Task GetById_WhenNotFound_ShouldThrow()
        {
            _repoMock.Setup(r => r.GetById(1))
                     .ReturnsAsync((Appointment)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.GetAppointmentById(1));
        }

        

        [Fact]
        public async Task Cancel_WhenNotFound_ShouldThrow()
        {
            _repoMock.Setup(r => r.GetById(1))
                     .ReturnsAsync((Appointment)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.CancelAppointment(1, "reason"));
        }


        [Fact]
        public async Task Confirm_WhenAlreadyConfirmed_ShouldThrow()
        {
            var appt = CreateAppointment();
            appt.Status = AppointmentStatus.Confirmed;

            _repoMock.Setup(r => r.GetById(appt.AppointmentId))
                     .ReturnsAsync(appt);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.ConfirmAppointment(appt.AppointmentId));
        }


        [Fact]
        public async Task Complete_WhenNotConfirmed_ShouldThrow()
        {
            var appt = CreateAppointment();
            appt.Status = AppointmentStatus.Pending;

            _repoMock.Setup(r => r.GetById(appt.AppointmentId))
                     .ReturnsAsync(appt);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.CompleteAppointment(appt.AppointmentId));
        }


        [Fact]
        public async Task Availability_ShouldReturnSlots()
        {
            _repoMock.Setup(r => r.GetBookedSlots(2, It.IsAny<DateTime>()))
                     .ReturnsAsync(new List<string> { "10:00 AM" });

            var result = await _service.CheckDoctorAvailability(2, DateTime.Today.AddDays(1));

            Assert.NotNull(result);
        }
    }
}