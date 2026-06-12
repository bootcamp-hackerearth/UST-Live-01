using Xunit;
using Moq;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthApp.API.Service.Impl;
using HealthApp.API.Repository.Interface;
using HealthApp.Shared.DTOs;
using HealthApp.API.Data;
using HealthApp.Shared.Constant;

namespace HealthApp.APIServiceTesting
{
    public class AppointmentServiceTesting
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepo;
        private readonly Mock<IDoctorRepository> _doctorRepo;
        private readonly Mock<IPatientRepository> _patientRepo;
        private readonly Mock<IMapper> _mapper;

        private readonly AppointmentService _service;

        public AppointmentServiceTesting()
        {
            _appointmentRepo = new Mock<IAppointmentRepository>();
            _doctorRepo = new Mock<IDoctorRepository>();
            _patientRepo = new Mock<IPatientRepository>();
            _mapper = new Mock<IMapper>();

            _service = new AppointmentService(
                _appointmentRepo.Object,
                _doctorRepo.Object,
                _patientRepo.Object,
                _mapper.Object);
        }

        // -------------------- ADD --------------------

        [Fact]
        public async Task Add_ShouldThrow_WhenDateIsPast()
        {
            var dto = new AppointmentDto
            {
                ScheduledDate = DateTime.Today.AddDays(-1)
            };

            await Assert.ThrowsAsync<Exception>(() => _service.Add(dto));
        }

        [Fact]
        public async Task Add_ShouldThrow_WhenPatientInvalid()
        {
            var dto = GetValidDto();

            _patientRepo.Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync((Patient)null);

            await Assert.ThrowsAsync<Exception>(() => _service.Add(dto));
        }

        [Fact]
        public async Task Add_ShouldThrow_WhenDoctorInactive()
        {
            var dto = GetValidDto();

            _patientRepo.Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(new Patient());

            _doctorRepo.Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(new Doctor { IsActive = false });

            await Assert.ThrowsAsync<Exception>(() => _service.Add(dto));
        }

        [Fact]
        public async Task Add_ShouldThrow_WhenSlotAlreadyBooked()
        {
            var dto = GetValidDto();

            SetupValidDoctorAndPatient(dto);

            _appointmentRepo.Setup(x =>
                x.ExistsPatientBookingAsync(dto.PatientId, dto.DoctorId, dto.ScheduledDate, dto.TimeSlot))
                .ReturnsAsync(false);

            _appointmentRepo.Setup(x =>
                x.IsSlotBookedAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<Exception>(() => _service.Add(dto));
        }

        [Fact]
        public async Task Add_ShouldAddAppointment_WhenValid()
        {
            var dto = GetValidDto();

            SetupValidDoctorAndPatient(dto);

            _appointmentRepo.Setup(x =>
                x.ExistsPatientBookingAsync(dto.PatientId, dto.DoctorId, dto.ScheduledDate, dto.TimeSlot))
                .ReturnsAsync(false);

            _appointmentRepo.Setup(x =>
                x.IsSlotBookedAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot))
                .ReturnsAsync(false);

            var appointment = new Appointment();

            _mapper.Setup(x => x.Map<Appointment>(dto))
                .Returns(appointment);

            await _service.Add(dto);

            _appointmentRepo.Verify(x => x.AddAsync(It.IsAny<Appointment>()), Times.Once);
        }

        // -------------------- GET BY ID --------------------

        [Fact]
        public async Task GetAppointmentById_ShouldThrow_WhenNotFound()
        {
            _appointmentRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsAsync<Exception>(() => _service.GetAppointmentById(1));
        }

        [Fact]
        public async Task GetAppointmentById_ShouldReturnDto_WhenFound()
        {
            var appointment = new Appointment();
            var dto = new AppointmentDto();

            _appointmentRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _mapper.Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(dto);

            var result = await _service.GetAppointmentById(1);

            Assert.NotNull(result);
        }

        // -------------------- CANCEL --------------------

        [Fact]
        public async Task CancelAppointment_ShouldThrow_WhenNotFound()
        {
            _appointmentRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.CancelAppointment(1, "test"));
        }

        [Fact]
        public async Task CancelAppointment_ShouldUpdateStatus()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending
            };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.CancelAppointment(1, "reason");

            Assert.Equal(AppointmentStatus.Cancelled, appointment.Status);
            _appointmentRepo.Verify(x => x.SaveAsync(), Times.Once);
        }

        // -------------------- CONFIRM --------------------

        [Fact]
        public async Task ConfirmAppointment_ShouldThrow_WhenAlreadyConfirmed()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.ConfirmAppointment(1));
        }

        [Fact]
        public async Task ConfirmAppointment_ShouldUpdateStatus()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending
            };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.ConfirmAppointment(1);

            Assert.Equal(AppointmentStatus.Confirmed, appointment.Status);
            _appointmentRepo.Verify(x => x.SaveAsync(), Times.Once);
        }

        // -------------------- COMPLETE --------------------

        [Fact]
        public async Task CompleteAppointment_ShouldThrow_WhenNotConfirmed()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending
            };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.CompleteAppointment(1));
        }

        [Fact]
        public async Task CompleteAppointment_ShouldUpdateStatus()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.CompleteAppointment(1);

            Assert.Equal(AppointmentStatus.Completed, appointment.Status);
            _appointmentRepo.Verify(x => x.SaveAsync(), Times.Once);
        }

        // -------------------- AVAILABILITY --------------------

        [Fact]
        public async Task CheckDoctorAvailability_ShouldThrow_WhenPastDate()
        {
            await Assert.ThrowsAsync<Exception>(() =>
                _service.CheckDoctorAvailability(1, DateTime.Today.AddDays(-1)));
        }

        [Fact]
        public async Task CheckDoctorAvailability_ShouldReturnSlots()
        {
            var date = DateTime.Today.AddDays(1);

            _appointmentRepo.Setup(x =>
                x.GetBookedSlotsAsync(1, date))
                .ReturnsAsync(new List<string> { TimeSlots.Slots[0] });

            var result = await _service.CheckDoctorAvailability(1, date);

            Assert.NotEmpty(result);
        }

        // -------------------- HELPERS --------------------

        private AppointmentDto GetValidDto()
        {
            return new AppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = TimeSlots.Slots[0]
            };
        }

        private void SetupValidDoctorAndPatient(AppointmentDto dto)
        {
            _patientRepo.Setup(x => x.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(new Patient());

            _doctorRepo.Setup(x => x.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(new Doctor { IsActive = true });
        }
    }
}