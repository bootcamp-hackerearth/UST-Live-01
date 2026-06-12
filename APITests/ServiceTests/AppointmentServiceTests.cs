namespace APITests.ServiceTests
{
    using Xunit;
    using Moq;
    using System;
    using System.Collections.Generic;
    using HealthAxisApp.Data;
    using HealthAxisApp.Repositories;
    using HealthAxisApp.Services.Impl;
    using HealthAxisApp.Shared.DTOs;
    using HealthAxisApp.Shared.Enums;

    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepo;
        private readonly Mock<IPatientRepository> _patientRepo;
        private readonly Mock<IDoctorRepository> _doctorRepo;

        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _appointmentRepo = new Mock<IAppointmentRepository>();
            _patientRepo = new Mock<IPatientRepository>();
            _doctorRepo = new Mock<IDoctorRepository>();

            _service = new AppointmentService(
                _appointmentRepo.Object,
                _patientRepo.Object,
                _doctorRepo.Object
            );

            _patientRepo.Setup(x => x.GetById(It.IsAny<int>()))
                        .Returns(new Patient());

            _doctorRepo.Setup(x => x.GetById(It.IsAny<int>()))
                       .Returns(new Doctor());

            _appointmentRepo.Setup(x =>
                x.IsSlotAvailable(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(true);
        }

        private AppointmentDto GetValidDto()
        {
            return new AppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00-11:00"
            };
        }

        [Fact]
        public void Book_InvalidPatient_ReturnsFalse()
        {
            var dto = GetValidDto();

            _patientRepo.Setup(x => x.GetById(dto.PatientId))
                        .Returns((Patient)null);

            var result = _service.Book(dto, out string error);

            Assert.False(result);
            Assert.Equal("Invalid patient.", error);
        }

        [Fact]
        public void Book_InvalidDoctor_ReturnsFalse()
        {
            var dto = GetValidDto();

            _doctorRepo.Setup(x => x.GetById(dto.DoctorId))
                       .Returns((Doctor)null);

            var result = _service.Book(dto, out string error);

            Assert.False(result);
            Assert.Equal("Invalid doctor.", error);
        }

        [Fact]
        public void Book_PastDate_ReturnsFalse()
        {
            var dto = GetValidDto();
            dto.ScheduledDate = DateTime.Today.AddDays(-1);

            var result = _service.Book(dto, out string error);

            Assert.False(result);
            Assert.Equal("Appointment date cannot be in the past.", error);
        }

        [Fact]
        public void Book_SlotUnavailable_ReturnsFalse()
        {
            var dto = GetValidDto();

            _appointmentRepo.Setup(x =>
                x.IsSlotAvailable(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot))
                .Returns(false);

            var result = _service.Book(dto, out string error);

            Assert.False(result);
            Assert.Equal("This slot is already booked.", error);
        }

        [Fact]
        public void Book_ValidRequest_ReturnsTrue()
        {
            var dto = GetValidDto();

            var result = _service.Book(dto, out string error);

            Assert.True(result);
            Assert.Equal(string.Empty, error);

            _appointmentRepo.Verify(x =>
                x.Add(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public void Book_ValidRequest_SavesCorrectAppointment()
        {
            var dto = GetValidDto();

            Appointment saved = null;

            _appointmentRepo.Setup(x => x.Add(It.IsAny<Appointment>()))
                .Callback<Appointment>(a => saved = a);

            _service.Book(dto, out _);

            Assert.NotNull(saved);
            Assert.Equal(dto.PatientId, saved.PatientId);
            Assert.Equal(dto.DoctorId, saved.DoctorId);
            Assert.Equal(dto.TimeSlot, saved.TimeSlot);
            Assert.Equal(AppointmentStatusEnum.Pending.ToString(), saved.Status);
        }

        [Fact]
        public void UpdateStatus_CancelWithoutReason_ReturnsFalse()
        {
            var dto = new AppointmentStatusUpdateDto
            {
                Status = AppointmentStatusEnum.Cancelled,
                CancellationReason = null
            };

            var result = _service.UpdateStatus(1, dto, out string error);

            Assert.False(result);
            Assert.Equal("Cancellation reason is required.", error);
        }

        [Fact]
        public void UpdateStatus_Valid_ReturnsTrue()
        {
            var dto = new AppointmentStatusUpdateDto
            {
                Status = AppointmentStatusEnum.Completed,
                CancellationReason = null
            };

            _appointmentRepo.Setup(x =>
                x.UpdateStatus(1, dto.Status.ToString(), dto.CancellationReason))
                .Returns(true);

            var result = _service.UpdateStatus(1, dto, out string error);

            Assert.True(result);
            Assert.Equal(string.Empty, error);
        }

        [Fact]
        public void Delete_ValidId_ReturnsTrue()
        {
            _appointmentRepo.Setup(x => x.Delete(1)).Returns(true);

            var result = _service.Delete(1);

            Assert.True(result);
        }

        [Fact]
        public void GetAll_ReturnsDtos()
        {
            _appointmentRepo.Setup(x => x.GetAll())
                .Returns(new List<Appointment>
                {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = DateTime.Today,
                    TimeSlot = "10:00-11:00",
                    Status = "Pending"
                }
                });

            var result = _service.GetAll();

            Assert.NotNull(result);
            Assert.Single(result);
        }
    }
}
