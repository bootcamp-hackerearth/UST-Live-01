using AutoMapper;
using HealthCare.Shared;
using HealthCare.Shared.DTOs.Doctor;
using HealthCareApi;
using HealthCareApi.Repositories.Interfaces;
using HealthCareApi.Services.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare.Tests
{
    [TestClass]
    public class DoctorServiceTests
    {
        private Mock<IDoctorRepository> _repoMock;
        private Mock<IMapper> _mapperMock;
        private DoctorService _service;

        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new DoctorService(
                _repoMock.Object,
                _mapperMock.Object
            );
        }

        //GET BY ID
        [TestMethod]
        public async Task GetDoctorById_ShouldReturnDoctor()
        {
            var doctor = new Doctor { DoctorId = 1 };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(doctor);

            var result = await _service.GetDoctorByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.DoctorId);
        }

        //GET FILTERED DOCTORS
        [TestMethod]
        public async Task GetFilteredDoctors_ShouldReturnPagedResult()
        {
            var data = new PagedResult<Doctor>
            {
                Items = new List<Doctor>(),
                TotalCount = 0
            };

            _repoMock.Setup(r => r.GetDoctorsAsync(null, null, false, 1, 10))
                .ReturnsAsync(data);

            var result = await _service.GetFilteredDoctorsAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalCount);
        }

        //ADD DOCTOR WITH SLOTS
        [TestMethod]
        public async Task AddDoctor_ShouldCreateDoctorAndSlots()
        {
            var dto = new CreateDoctorDto
            {
                FullName = "Dr Test",
                Specialisation = "Cardiology",
                TimeSlots = new List<string> { "10:00", "11:00" }
            };

            var doctor = new Doctor { DoctorId = 1 };

            _mapperMock.Setup(m => m.Map<Doctor>(dto)).Returns(doctor);

            _repoMock.Setup(r => r.AddAsync(doctor))
                .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.AddRangeAsync(It.IsAny<List<DoctorAvailableSlot>>()))
                .Returns(Task.CompletedTask);

            var result = await _service.AddDoctorAsync(dto);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.DoctorId);
            Assert.IsTrue(result.IsActive);

            _repoMock.Verify(r => r.AddRangeAsync(It.IsAny<List<DoctorAvailableSlot>>()), Times.Once);
        }

        // ADD DOCTOR WITHOUT SLOTS
        [TestMethod]
        public async Task AddDoctor_ShouldWorkWithoutSlots()
        {
            var dto = new CreateDoctorDto
            {
                FullName = "Dr Test",
                Specialisation = "Cardiology",
                TimeSlots = null
            };

            var doctor = new Doctor { DoctorId = 1 };

            _mapperMock.Setup(m => m.Map<Doctor>(dto)).Returns(doctor);

            _repoMock.Setup(r => r.AddAsync(doctor))
                .Returns(Task.CompletedTask);

            var result = await _service.AddDoctorAsync(dto);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsActive);

            _repoMock.Verify(r => r.AddRangeAsync(It.IsAny<List<DoctorAvailableSlot>>()), Times.Never);
        }

        // GET BY SPECIALIZATION SUCCESS
        [TestMethod]
        public async Task GetBySpecialization_ShouldReturnDoctors()
        {
            var doctors = new List<Doctor>
            {
                new Doctor { DoctorId = 1 }
            };

            _repoMock.Setup(r => r.GetBySpecializationAsync("Cardiology"))
                .ReturnsAsync(doctors);

            var result = await _service.GetBySpecializationAsync("Cardiology");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        // GET BY SPECIALIZATION FAIL
        [TestMethod]
        public async Task GetBySpecialization_ShouldThrow_WhenEmpty()
        {
            await Assert.ThrowsExceptionAsync<Exception>(() =>
                _service.GetBySpecializationAsync(""));
        }

        // UPDATE SUCCESS
        [TestMethod]
        public async Task UpdateDoctor_ShouldUpdateFields()
        {
            var existing = new Doctor
            {
                DoctorId = 1,
                FullName = "Old"
            };

            var updated = new Doctor
            {
                DoctorId = 1,
                FullName = "New",
                Specialisation = "Ortho",
                YearsOfExperience = 5,
                ConsultationFee = 100
            };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Doctor>()))
                .Returns(Task.CompletedTask);

            var result = await _service.UpdateDoctorAsync(updated);

            Assert.IsNotNull(result);
            Assert.AreEqual("New", result.FullName);
            Assert.AreEqual("Ortho", result.Specialisation);
        }

        // UPDATE FAIL
        [TestMethod]
        public async Task UpdateDoctor_ShouldReturnNull_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Doctor)null);

            var result = await _service.UpdateDoctorAsync(new Doctor { DoctorId = 1 });

            Assert.IsNull(result);
        }

        // DELETE SUCCESS
        [TestMethod]
        public async Task DeleteDoctor_ShouldSoftDelete()
        {
            var doctor = new Doctor
            {
                DoctorId = 1,
                IsActive = true
            };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(doctor);

            _repoMock.Setup(r => r.UpdateAsync(doctor))
                .Returns(Task.CompletedTask);

            var result = await _service.DeleteDoctorAsync(1);

            Assert.IsTrue(result);
            Assert.IsFalse(doctor.IsActive);
        }

        // DELETE FAIL
        [TestMethod]
        public async Task DeleteDoctor_ShouldReturnFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Doctor)null);

            var result = await _service.DeleteDoctorAsync(1);

            Assert.IsFalse(result);
        }
    }
}
