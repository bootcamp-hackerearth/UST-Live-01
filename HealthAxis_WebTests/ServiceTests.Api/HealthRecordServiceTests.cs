using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using HealthAxis.Api.Services;
using HealthAxis.Api.Repositories;
using HealthAxis.Api.Models;
using HealthAxis.Shared.Dtos;

namespace HealthAxis.Api.Tests
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _repo;
        private readonly Mock<IAppointmentRepository> _appointmentRepo;
        private readonly HealthRecordServiceImpl _service;

        public HealthRecordServiceTests()
        {
            _repo = new Mock<IHealthRecordRepository>();
            _appointmentRepo = new Mock<IAppointmentRepository>();
            _service = new HealthRecordServiceImpl(_repo.Object, _appointmentRepo.Object);
        }

        [Fact]
        public void Create_Valid_ReturnsSuccess()
        {
            var dto = new CreateHealthRecordDto
            {
                AppointmentId = 1,
                Diagnosis = "Fever"
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1
            };

            _repo.Setup(r => r.ExistsByAppointment(1)).Returns(false);
            _appointmentRepo.Setup(a => a.GetById(1)).Returns(appointment);

            var result = _service.Create(dto);

            Assert.True(result.Success);
        }

        [Fact]
        public void Create_NullDto_ReturnsFailure()
        {
            var result = _service.Create(null);

            Assert.False(result.Success);
        }

        [Fact]
        public void Create_RecordExists_ReturnsFailure()
        {
            var dto = new CreateHealthRecordDto { AppointmentId = 1 };

            _repo.Setup(r => r.ExistsByAppointment(1)).Returns(true);

            var result = _service.Create(dto);

            Assert.False(result.Success);
        }

        [Fact]
        public void Create_InvalidAppointment_ReturnsFailure()
        {
            var dto = new CreateHealthRecordDto { AppointmentId = 1 };

            _repo.Setup(r => r.ExistsByAppointment(1)).Returns(false);
            _appointmentRepo.Setup(a => a.GetById(1)).Returns((Appointment)null);

            var result = _service.Create(dto);

            Assert.False(result.Success);
        }

        [Fact]
        public void Create_CallsAdd()
        {
            var dto = new CreateHealthRecordDto { AppointmentId = 1 };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1
            };

            _repo.Setup(r => r.ExistsByAppointment(1)).Returns(false);
            _appointmentRepo.Setup(a => a.GetById(1)).Returns(appointment);

            _service.Create(dto);

            _repo.Verify(r => r.Add(It.IsAny<HealthRecord>()), Times.Once);
        }

        [Fact]
        public void Create_CallsSave()
        {
            var dto = new CreateHealthRecordDto { AppointmentId = 1 };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1
            };

            _repo.Setup(r => r.ExistsByAppointment(1)).Returns(false);
            _appointmentRepo.Setup(a => a.GetById(1)).Returns(appointment);

            _service.Create(dto);

            _repo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void Create_UpdatesAppointmentStatus()
        {
            var dto = new CreateHealthRecordDto { AppointmentId = 1 };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1
            };

            _repo.Setup(r => r.ExistsByAppointment(1)).Returns(false);
            _appointmentRepo.Setup(a => a.GetById(1)).Returns(appointment);

            _service.Create(dto);

            Assert.Equal("Completed", appointment.Status);
        }

        [Fact]
        public void Create_CallsAppointmentUpdate()
        {
            var dto = new CreateHealthRecordDto { AppointmentId = 1 };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1
            };

            _repo.Setup(r => r.ExistsByAppointment(1)).Returns(false);
            _appointmentRepo.Setup(a => a.GetById(1)).Returns(appointment);

            _service.Create(dto);

            _appointmentRepo.Verify(a => a.Update(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public void GetByPatient_ReturnsData()
        {
            _repo.Setup(r => r.GetByPatient(1))
                .Returns(new List<HealthRecord>
                {
                    new HealthRecord { RecordId = 1 }
                });

            var result = _service.GetByPatient(1);

            Assert.Single(result);
        }

        [Fact]
        public void GetByPatient_Empty_ReturnsEmpty()
        {
            _repo.Setup(r => r.GetByPatient(1))
                .Returns(new List<HealthRecord>());

            var result = _service.GetByPatient(1);

            Assert.Empty(result);
        }
    }
}
