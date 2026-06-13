using HealthcareApi.Exceptions;
using HealthcareApi.Models;
using HealthcareApi.Repositories;
using HealthcareApi.Services.Implementations;
using Moq;
using SharedClasses.Dtos;
using SharedClasses.Enums;
using System;
using System.Collections.Generic;
using Xunit;

namespace HealthcareApi.Tests.Services
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _healthRecordRepo;
        private readonly Mock<IAppointmentRepository> _appointmentRepo;
        private readonly Mock<IPatientRepository> _patientRepo;
        private readonly Mock<IDoctorRepository> _doctorRepo;

        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            _healthRecordRepo = new Mock<IHealthRecordRepository>();
            _appointmentRepo = new Mock<IAppointmentRepository>();
            _patientRepo = new Mock<IPatientRepository>();
            _doctorRepo = new Mock<IDoctorRepository>();

            _service = new HealthRecordService(
                _healthRecordRepo.Object,
                _appointmentRepo.Object,
                _patientRepo.Object,
                _doctorRepo.Object
            );
        }

        [Fact]
        public void GetAllRecords_ShouldReturnRecords()
        {
            var records = new List<HealthRecord>
            {
                CreateHealthRecord()
            };

            _healthRecordRepo.Setup(r => r.GetAll()).Returns(records);

            var result = _service.GetAllRecords();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result[0].HealthRecordId);
        }

        [Fact]
        public void GetRecordById_WhenExists_ShouldReturnRecord()
        {
            var record = CreateHealthRecord();

            _healthRecordRepo.Setup(r => r.GetById(1)).Returns(record);

            var result = _service.GetRecordById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.HealthRecordId);
        }

        [Fact]
        public void GetRecordById_WhenNotExists_ShouldThrow()
        {
            _healthRecordRepo.Setup(r => r.GetById(99)).Returns((HealthRecord)null);

            Assert.Throws<EntityNotFoundException>(() =>
                _service.GetRecordById(99));
        }

        [Fact]
        public void GetRecordsByPatient_WhenInvalidId_ShouldThrow()
        {
            Assert.Throws<HealthRecordRuleException>(() =>
                _service.GetRecordsByPatient(0));
        }

        [Fact]
        public void GetRecordsByDoctor_WhenInvalidId_ShouldThrow()
        {
            Assert.Throws<HealthRecordRuleException>(() =>
                _service.GetRecordsByDoctor(0));
        }

        [Fact]
        public void AddRecord_WhenDtoIsNull_ShouldThrow()
        {
            Assert.Throws<HealthRecordRuleException>(() =>
                _service.AddRecord(null));
        }

        [Fact]
        public void AddRecord_WhenAppointmentNotCompleted_ShouldThrow()
        {
            var dto = CreateAddDto();

            var appointment = CreateAppointment();
            appointment.Status = AppointmentStatus.Pending;

            _appointmentRepo.Setup(r => r.GetById(dto.AppointmentId))
                .Returns(appointment);

            Assert.Throws<HealthRecordRuleException>(() =>
                _service.AddRecord(dto));
        }

        [Fact]
        public void AddRecord_WhenAlreadyExists_ShouldThrow()
        {
            var dto = CreateAddDto();
            var appointment = CreateCompletedAppointment();

            _appointmentRepo.Setup(r => r.GetById(dto.AppointmentId))
                .Returns(appointment);

            _healthRecordRepo.Setup(r =>
                r.ExistsByAppointmentId(dto.AppointmentId))
                .Returns(true);

            Assert.Throws<HealthRecordRuleException>(() =>
                _service.AddRecord(dto));
        }

        [Fact]
        public void AddRecord_WhenValid_ShouldAdd()
        {
            var dto = CreateAddDto();
            var appointment = CreateCompletedAppointment();

            _appointmentRepo.Setup(r => r.GetById(dto.AppointmentId))
                .Returns(appointment);

            _healthRecordRepo.Setup(r =>
                r.ExistsByAppointmentId(dto.AppointmentId))
                .Returns(false);

            _healthRecordRepo.Setup(r => r.Add(It.IsAny<HealthRecord>()))
                .Returns((HealthRecord hr) =>
                {
                    hr.HealthRecordId = 1;
                    return hr;
                });

            var result = _service.AddRecord(dto);

            Assert.NotNull(result);
            Assert.Equal(1, result.HealthRecordId);
            Assert.Equal(dto.Diagnosis, result.Diagnosis);
        }

        [Fact]
        public void UpdateRecord_WhenNullDto_ShouldThrow()
        {
            Assert.Throws<HealthRecordRuleException>(() =>
                _service.UpdateRecord(1, null));
        }

        [Fact]
        public void UpdateRecord_WhenNotExists_ShouldThrow()
        {
            _healthRecordRepo.Setup(r => r.GetById(1))
                .Returns((HealthRecord)null);

            var dto = CreateUpdateDto();

            Assert.Throws<EntityNotFoundException>(() =>
                _service.UpdateRecord(1, dto));
        }

        [Fact]
        public void UpdateRecord_WhenValid_ShouldUpdate()
        {
            var record = CreateHealthRecord();

            _healthRecordRepo.Setup(r => r.GetById(1))
                .Returns(record);

            _healthRecordRepo.Setup(r =>
                r.Update(1, It.IsAny<HealthRecord>()))
                .Returns(record);

            var dto = CreateUpdateDto();

            var result = _service.UpdateRecord(1, dto);

            Assert.Equal(dto.Diagnosis, result.Diagnosis);
        }

        [Fact]
        public void DeleteRecord_WhenInvalidId_ShouldThrow()
        {
            Assert.Throws<HealthRecordRuleException>(() =>
                _service.DeleteRecord(0));
        }

        [Fact]
        public void DeleteRecord_WhenNotFound_ShouldThrow()
        {
            _healthRecordRepo.Setup(r => r.Delete(1))
                .Returns((HealthRecord)null);

            Assert.Throws<EntityNotFoundException>(() =>
                _service.DeleteRecord(1));
        }

        [Fact]
        public void DeleteRecord_WhenValid_ShouldDelete()
        {
            var record = CreateHealthRecord();

            _healthRecordRepo.Setup(r => r.Delete(1))
                .Returns(record);

            var result = _service.DeleteRecord(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.HealthRecordId);
        }

        private HealthRecord CreateHealthRecord()
        {
            return new HealthRecord
            {
                HealthRecordId = 1,
                PatientId = 1,
                DoctorId = 2,
                AppointmentId = 10,
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                Notes = "Rest"
            };
        }

        private Appointment CreateAppointment()
        {
            return new Appointment
            {
                AppointmentId = 10,
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today,
                Status = AppointmentStatus.Pending
            };
        }

        private Appointment CreateCompletedAppointment()
        {
            var appt = CreateAppointment();
            appt.Status = AppointmentStatus.Completed;
            return appt;
        }

        private AddHealthRecordDto CreateAddDto()
        {
            return new AddHealthRecordDto
            {
                AppointmentId = 10,
                Diagnosis = "Fever",
                Prescription = "Medicine",
                Notes = "Take rest"
            };
        }

        private UpdateHealthRecordDto CreateUpdateDto()
        {
            return new UpdateHealthRecordDto
            {
                Diagnosis = "Updated",
                Prescription = "Updated Med",
                Notes = "Updated Notes"
            };
        }
    }
}