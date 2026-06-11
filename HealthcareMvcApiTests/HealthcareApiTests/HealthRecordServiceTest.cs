using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using SharedClasses.Dtos;
using SharedClasses.Enums;
using HealthcareApi.Exceptions;
using HealthcareApi.Models;
using HealthcareApi.Repositories;
using HealthcareApi.Services.Implementations;

namespace HealthcareMvcApiTests.Services
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _healthRecordRepoMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepoMock;
        private readonly Mock<IPatientRepository> _patientRepoMock;
        private readonly Mock<IDoctorRepository> _doctorRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly HealthRecordService _sut;

        public HealthRecordServiceTests()
        {
            _healthRecordRepoMock = new Mock<IHealthRecordRepository>();
            _appointmentRepoMock = new Mock<IAppointmentRepository>();
            _patientRepoMock = new Mock<IPatientRepository>();
            _doctorRepoMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();

            _sut = new HealthRecordService(
                _healthRecordRepoMock.Object,
                _appointmentRepoMock.Object,
                _patientRepoMock.Object,
                _doctorRepoMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public void AddRecord_ValidDetails_ReturnsHealthRecordDto()
        {
            // Arrange
            var addDto = new AddHealthRecordDto
            {
                AppointmentId = 1,
                Diagnosis = "Flu",
                Prescription = "Rest",
                Notes = "Drink water"
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                PatientId = 10,
                DoctorId = 20,
                ScheduledDate = DateTime.Today,
                Status = AppointmentStatus.Completed
            };

            var savedRecord = new HealthRecord { Diagnosis = "Flu" };
            var resultDto = new HealthRecordDto { Diagnosis = "Flu" };

            _appointmentRepoMock.Setup(r => r.GetById(addDto.AppointmentId)).Returns(appointment);
            _healthRecordRepoMock.Setup(r => r.ExistsByAppointmentId(addDto.AppointmentId)).Returns(false);
            _healthRecordRepoMock.Setup(r => r.Add(It.IsAny<HealthRecord>())).Returns(savedRecord);
            _mapperMock.Setup(m => m.Map<HealthRecordDto>(savedRecord)).Returns(resultDto);
            SetupPatientAndDoctorNames();

            // Act
            var result = _sut.AddRecord(addDto);

            // Assert
            result.Should().NotBeNull();
            result.Diagnosis.Should().Be("Flu");

            _healthRecordRepoMock.Verify(r => r.Add(It.Is<HealthRecord>(h =>
                h.AppointmentId == 1 &&
                h.PatientId == 10 &&
                h.Diagnosis == "Flu"
            )), Times.Once);
        }

        [Fact]
        public void AddRecord_AppointmentNotCompleted_ThrowsHealthRecordRuleException()
        {
            // Arrange
            var addDto = new AddHealthRecordDto { AppointmentId = 1 };
            var appointment = new Appointment { AppointmentId = 1, Status = AppointmentStatus.Pending }; // Not completed

            _appointmentRepoMock.Setup(r => r.GetById(addDto.AppointmentId)).Returns(appointment);

            // Act
            Action act = () => _sut.AddRecord(addDto);

            // Assert
            act.Should().Throw<HealthRecordRuleException>()
               .WithMessage("Health record can be added only for completed appointments.");
        }

        [Fact]
        public void AddRecord_RecordAlreadyExists_ThrowsHealthRecordRuleException()
        {
            // Arrange
            var addDto = new AddHealthRecordDto { AppointmentId = 1 };
            var appointment = new Appointment { AppointmentId = 1, Status = AppointmentStatus.Completed };

            _appointmentRepoMock.Setup(r => r.GetById(addDto.AppointmentId)).Returns(appointment);
            _healthRecordRepoMock.Setup(r => r.ExistsByAppointmentId(addDto.AppointmentId)).Returns(true); // Already exists

            // Act
            Action act = () => _sut.AddRecord(addDto);

            // Assert
            act.Should().Throw<HealthRecordRuleException>()
               .WithMessage("Health record already exists for this appointment.");
        }

        [Fact]
        public void AddRecord_EmptyDiagnosis_ThrowsHealthRecordRuleException()
        {
            // Arrange
            var addDto = new AddHealthRecordDto
            {
                AppointmentId = 1,
                Diagnosis = "   ", // Invalid
                Prescription = "Rest"
            };
            var appointment = new Appointment { AppointmentId = 1, Status = AppointmentStatus.Completed };

            _appointmentRepoMock.Setup(r => r.GetById(addDto.AppointmentId)).Returns(appointment);
            _healthRecordRepoMock.Setup(r => r.ExistsByAppointmentId(addDto.AppointmentId)).Returns(false);

            // Act
            Action act = () => _sut.AddRecord(addDto);

            // Assert
            act.Should().Throw<HealthRecordRuleException>()
               .WithMessage("Diagnosis is required.");
        }

        [Fact]
        public void DeleteRecord_ValidId_ReturnsHealthRecordDto()
        {
            // Arrange
            int recordId = 5;
            var deletedRecord = new HealthRecord { Diagnosis = "Cold" };
            var resultDto = new HealthRecordDto { Diagnosis = "Cold" };

            _healthRecordRepoMock.Setup(r => r.Delete(recordId)).Returns(deletedRecord);
            _mapperMock.Setup(m => m.Map<HealthRecordDto>(deletedRecord)).Returns(resultDto);
            SetupPatientAndDoctorNames();

            // Act
            var result = _sut.DeleteRecord(recordId);

            // Assert
            result.Should().NotBeNull();
            _healthRecordRepoMock.Verify(r => r.Delete(recordId), Times.Once);
        }
        private void SetupPatientAndDoctorNames(
            int patientId = 1,
            int doctorId = 1,
            string patientName = "Jane Doe",
            string doctorName = "Doctor Adams")
        {
            _patientRepoMock.Setup(r => r.GetById(patientId))
                .Returns(new Patient
                {
                    PatientId = patientId,
                    FullName = patientName
                });

            _doctorRepoMock.Setup(r => r.GetById(doctorId))
                .Returns(new Doctor
                {
                    DoctorId = doctorId,
                    FullName = doctorName,
                    IsActive = true
                });
        }
    }
}