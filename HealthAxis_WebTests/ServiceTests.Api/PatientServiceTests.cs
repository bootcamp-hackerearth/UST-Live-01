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
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _repo;
        private readonly PatientServiceImpl _service;

        public PatientServiceTests()
        {
            _repo = new Mock<IPatientRepository>();
            _service = new PatientServiceImpl(_repo.Object);
        }

        [Fact]
        public void Create_Valid_ReturnsSuccess()
        {
            var dto = new CreatePatientDto
            {
                FullName = "Test",
                DateOfBirth = DateTime.Today.AddYears(-20)
            };

            var result = _service.Create(dto);

            Assert.True(result.Success);
        }

        [Fact]
        public void Create_CallsAdd()
        {
            var dto = new CreatePatientDto
            {
                DateOfBirth = DateTime.Today.AddYears(-20)
            };

            _service.Create(dto);

            _repo.Verify(r => r.Add(It.IsAny<Patient>()), Times.Once);
        }

        [Fact]
        public void Create_CallsSave()
        {
            var dto = new CreatePatientDto
            {
                DateOfBirth = DateTime.Today.AddYears(-20)
            };

            _service.Create(dto);

            _repo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void GetAllPatients_ReturnsList()
        {
            _repo.Setup(r => r.GetAll()).Returns(new List<Patient>
            {
                new Patient(),
                new Patient()
            });

            var result = _service.GetAllPatients();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetById_Valid_ReturnsPatient()
        {
            _repo.Setup(r => r.GetById(1))
                 .Returns(new Patient());

            _repo.Setup(r => r.GetAppointmentsByPatientId(1))
                 .Returns(new List<Appointment>());

            var result = _service.GetById(1);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetById_Invalid_ReturnsNull()
        {
            _repo.Setup(r => r.GetById(1))
                 .Returns((Patient)null);

            var result = _service.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public void GetById_UpcomingAppointments_Correct()
        {
            _repo.Setup(r => r.GetById(1))
                 .Returns(new Patient());

            _repo.Setup(r => r.GetAppointmentsByPatientId(1))
                 .Returns(new List<Appointment>
                 {
                     new Appointment
                     {
                         ScheduledDate = DateTime.Today.AddDays(1),
                         Status = "Active"
                     },
                     new Appointment
                     {
                         ScheduledDate = DateTime.Today.AddDays(-1),
                         Status = "Active"
                     },
                     new Appointment
                     {
                         ScheduledDate = DateTime.Today.AddDays(1),
                         Status = "Cancelled"
                     }
                 });

            var result = _service.GetById(1);

            Assert.Equal(1, result.UpcomingAppointments);
        }

        [Fact]
        public void Update_Valid_ReturnsUpdated()
        {
            var patient = new Patient();

            _repo.Setup(r => r.GetById(1))
                 .Returns(patient);

            var dto = new PatientDto
            {
                FullName = "Updated"
            };

            var result = _service.Update(1, dto);

            Assert.NotNull(result);
        }

        [Fact]
        public void Update_Invalid_ReturnsNull()
        {
            _repo.Setup(r => r.GetById(1))
                 .Returns((Patient)null);

            var result = _service.Update(1, new PatientDto());

            Assert.Null(result);
        }

        [Fact]
        public void Deactivate_Valid_ReturnsTrue()
        {
            _repo.Setup(r => r.GetById(1))
                 .Returns(new Patient());

            var result = _service.Deactivate(1);

            Assert.True(result);
        }

        [Fact]
        public void Deactivate_Invalid_ReturnsFalse()
        {
            _repo.Setup(r => r.GetById(1))
                 .Returns((Patient)null);

            var result = _service.Deactivate(1);

            Assert.False(result);
        }
    }
}