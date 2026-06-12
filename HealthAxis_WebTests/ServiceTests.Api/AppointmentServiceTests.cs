using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using HealthAxis.Api.Services;
using HealthAxis.Api.Repositories;
using HealthAxis.Shared.Dtos;
using HealthAxis.Api.Models;

namespace HealthAxis.Api.Tests
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _repo;
        private readonly AppointmentServiceImpl _service;

        public AppointmentServiceTests()
        {
            _repo = new Mock<IAppointmentRepository>();
            _service = new AppointmentServiceImpl(_repo.Object);
        }

        [Fact]
        public void Book_Valid_ReturnsSuccess()
        {
            var dto = new BookAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                TimeSlot = "10:00 AM : 11:00 AM",
                ScheduledDate = DateTime.Today.AddDays(1)
            };

            _repo.Setup(r => r.ExistsSameDay(1, 1, dto.ScheduledDate)).Returns(false);
            _repo.Setup(r => r.IsSlotTaken(1, dto.ScheduledDate, dto.TimeSlot)).Returns(false);
            _repo.Setup(r => r.GetBookedSlots(1, dto.ScheduledDate)).Returns(new List<string>());

            var result = _service.Book(dto);

            Assert.True(result.Success);
        }

        [Fact]
        public void Book_EmptyTimeSlot_ReturnsFailure()
        {
            var dto = new BookAppointmentDto { TimeSlot = "" };

            var result = _service.Book(dto);

            Assert.False(result.Success);
        }

        [Fact]
        public void Book_InvalidTimeSlot_ReturnsFailure()
        {
            var dto = new BookAppointmentDto { TimeSlot = "Select Doctor & Date" };

            var result = _service.Book(dto);

            Assert.False(result.Success);
        }

        [Fact]
        public void Book_PastDate_ReturnsFailure()
        {
            var dto = new BookAppointmentDto
            {
                TimeSlot = "10:00 AM : 11:00 AM",
                ScheduledDate = DateTime.Today.AddDays(-1)
            };

            var result = _service.Book(dto);

            Assert.False(result.Success);
        }

        [Fact]
        public void Book_AlreadyBookedSameDay_ReturnsFailure()
        {
            var dto = new BookAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                TimeSlot = "10:00 AM : 11:00 AM",
                ScheduledDate = DateTime.Today.AddDays(1)
            };

            _repo.Setup(r => r.ExistsSameDay(1, 1, dto.ScheduledDate)).Returns(true);

            var result = _service.Book(dto);

            Assert.False(result.Success);
        }

        [Fact]
        public void Book_SlotTaken_ReturnsFailure()
        {
            var dto = new BookAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                TimeSlot = "10:00 AM : 11:00 AM",
                ScheduledDate = DateTime.Today.AddDays(1)
            };

            _repo.Setup(r => r.ExistsSameDay(1, 1, dto.ScheduledDate)).Returns(false);
            _repo.Setup(r => r.IsSlotTaken(1, dto.ScheduledDate, dto.TimeSlot)).Returns(true);

            var result = _service.Book(dto);

            Assert.False(result.Success);
        }

        [Fact]
        public void Book_NoSlotsAvailable_ReturnsFailure()
        {
            var dto = new BookAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                TimeSlot = "10:00 AM : 11:00 AM",
                ScheduledDate = DateTime.Today.AddDays(1)
            };

            _repo.Setup(r => r.ExistsSameDay(1, 1, dto.ScheduledDate)).Returns(false);
            _repo.Setup(r => r.IsSlotTaken(1, dto.ScheduledDate, dto.TimeSlot)).Returns(false);
            _repo.Setup(r => r.GetBookedSlots(1, dto.ScheduledDate))
                 .Returns(new List<string>
                 {
                     "10:00",
                     "11:00",
                     "1:00",
                     "2:00"
                 });

            var result = _service.Book(dto);

            Assert.False(result.Success);
        }

        [Fact]
        public void GetByPatient_ReturnsList()
        {
            _repo.Setup(r => r.GetByPatient(1)).Returns(new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    Status = "Pending",
                    Doctor = new Doctor { FullName = "Doc" }
                }
            });

            var result = _service.GetByPatient(1);

            Assert.Single(result);
        }

        [Fact]
        public void GetByDoctor_ReturnsList()
        {
            _repo.Setup(r => r.GetByDoctor(1)).Returns(new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    Status = "Pending",
                    Patient = new Patient { FullName = "Pat" }
                }
            });

            var result = _service.GetByDoctor(1);

            Assert.Single(result);
        }

        [Fact]
        public void UpdateStatus_Success()
        {
            var appointment = new Appointment { AppointmentId = 1 };

            _repo.Setup(r => r.GetById(1)).Returns(appointment);

            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Completed
            };

            var result = _service.UpdateStatus(1, dto);

            Assert.True(result.Success);
        }
    }
}
