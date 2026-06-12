using HealthCare.Shared;
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
    public class PatientServiceTests
    {
        private Mock<IPatientRepository> _repoMock;
        private PatientService _service;

        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IPatientRepository>();

            _service = new PatientService(_repoMock.Object);
        }

        //  GET BY ID
        [TestMethod]
        public async Task GetPatientById_ShouldReturnPatient()
        {
            var patient = new Patient { PatientId = 1 };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                .Returns(Task.FromResult(patient));

            var result = await _service.GetPatientByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.PatientId);
        }

        //  GET PAGINATED
        [TestMethod]
        public async Task GetPaginatedPatient_ShouldReturnPagedResult()
        {
            var data = new PagedResult<Patient>
            {
                Items = new List<Patient>(),
                TotalCount = 0
            };

            _repoMock.Setup(r => r.GetPaginatedPatientsAsync(null, 1, 10))
                .Returns(Task.FromResult(data));

            var result = await _service.GetPaginatedPatientAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalCount);
        }

        //  ADD PATIENT
        [TestMethod]
        public async Task AddPatient_ShouldSetActiveAndAdd()
        {
            var patient = new Patient
            {
                FullName = "Test"
            };

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Patient>()))
                .Returns(Task.CompletedTask);

            var result = await _service.AddPatientAsync(patient);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsActive);

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Patient>()), Times.Once);
        }

        //  UPDATE SUCCESS
        [TestMethod]
        public async Task UpdatePatient_ShouldUpdateFields()
        {
            var existing = new Patient
            {
                PatientId = 1,
                FullName = "Old"
            };

            var updated = new Patient
            {
                PatientId = 1,
                FullName = "New",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = "Male",
                PhoneNumber = "123"
            };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                .Returns(Task.FromResult(existing));

            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Patient>()))
                .Returns(Task.CompletedTask);

            var result = await _service.UpdatePatientAsync(updated);

            Assert.IsNotNull(result);
            Assert.AreEqual("New", result.FullName);
            Assert.AreEqual("Male", result.Gender);
        }

        //  UPDATE FAIL (NOT FOUND)
        [TestMethod]
        public async Task UpdatePatient_ShouldReturnNull_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                .Returns(Task.FromResult<Patient>(null));

            var result = await _service.UpdatePatientAsync(new Patient { PatientId = 1 });

            Assert.IsNull(result);
        }

        //  DELETE SUCCESS
        [TestMethod]
        public async Task DeletePatient_ShouldSoftDelete()
        {
            var patient = new Patient
            {
                PatientId = 1,
                IsActive = true
            };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                .Returns(Task.FromResult(patient));

            _repoMock.Setup(r => r.UpdateAsync(patient))
                .Returns(Task.CompletedTask);

            var result = await _service.DeletePatientAsync(1);

            Assert.IsTrue(result);
            Assert.IsFalse(patient.IsActive);
        }

        //  DELETE FAIL
        [TestMethod]
        public async Task DeletePatient_ShouldReturnFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                .Returns(Task.FromResult<Patient>(null));

            var result = await _service.DeletePatientAsync(1);

            Assert.IsFalse(result);
        }
    }
}