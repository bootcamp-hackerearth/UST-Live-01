namespace APITests.ServiceTests
{
    using Xunit;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using HealthAxisApp.Data;
    using HealthAxisApp.Repositories;
    using HealthAxisApp.Services.Impl;
    using HealthAxisApp.Shared.DTOs;
    using HealthAxisApp.Shared.Enums;

    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _patientRepo;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _patientRepo = new Mock<IPatientRepository>();
            _service = new PatientService(_patientRepo.Object);

            _patientRepo.Setup(x => x.GetAll(null, null))
                        .Returns(new List<Patient>());
        }

        private PatientDto GetValidDto()
        {
            return new PatientDto
            {
                FullName = "John Doe",
                DateOfBirth = DateTime.Today.AddYears(-25),
                Gender = GenderEnum.Male,
                PhoneNumber = "9876543210",
                Email = "john@test.com",
                InsuranceID = "INS123"
            };
        }

        [Fact]
        public void GetAll_ReturnsMappedDtos()
        {
            _patientRepo.Setup(x => x.GetAll(null, null))
                .Returns(new List<Patient>
                {
                new Patient
                {
                    PatientId = 1,
                    FullName = "John",
                    Gender = "Male"
                }
                });

            var result = _service.GetAll();

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public void GetById_NotFound_ReturnsNull()
        {
            _patientRepo.Setup(x => x.GetById(1))
                        .Returns((Patient)null);

            var result = _service.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public void GetById_Valid_ReturnsMappedDto()
        {
            _patientRepo.Setup(x => x.GetById(1))
                .Returns(new Patient
                {
                    PatientId = 1,
                    FullName = "John",
                    Gender = "Male"
                });

            _patientRepo.Setup(x => x.GetAppointmentCount(1))
                        .Returns(5);

            var result = _service.GetById(1);

            Assert.NotNull(result);
            Assert.Equal("John", result.FullName);
            Assert.Equal(5, result.AppointmentCount);
        }

        [Fact]
        public void Create_DuplicateEmail_ReturnsFalse()
        {
            var dto = GetValidDto();

            _patientRepo.Setup(x => x.GetByEmail(dto.Email))
                        .Returns(new Patient());

            var result = _service.Create(dto, out string error, out int id);

            Assert.False(result);
            Assert.Equal("A patient with this email already exists.", error);
            Assert.Equal(0, id);
        }

        [Fact]
        public void Create_DuplicateInsurance_ReturnsFalse()
        {
            var dto = GetValidDto();

            _patientRepo.Setup(x => x.GetByEmail(dto.Email))
                        .Returns((Patient)null);

            _patientRepo.Setup(x => x.GetAll(null, null))
                .Returns(new List<Patient>
                {
                new Patient { InsuranceID = dto.InsuranceID }
                });

            var result = _service.Create(dto, out string error, out int id);

            Assert.False(result);
            Assert.Equal("Insurance ID already exists.", error);
            Assert.Equal(0, id);
        }

        [Fact]
        public void Create_Valid_ReturnsTrue()
        {
            var dto = GetValidDto();

            _patientRepo.Setup(x => x.GetByEmail(dto.Email))
                        .Returns((Patient)null);

            _patientRepo.Setup(x => x.GetAll(null, null))
                        .Returns(new List<Patient>());

            _patientRepo.Setup(x => x.Add(It.IsAny<Patient>()))
                        .Returns(new Patient { PatientId = 10 });

            var result = _service.Create(dto, out string error, out int id);

            Assert.True(result);
            Assert.Equal(string.Empty, error);
            Assert.Equal(10, id);
        }

        [Fact]
        public void Create_Valid_SavesCorrectData()
        {
            var dto = GetValidDto();

            _patientRepo.Setup(x => x.GetByEmail(dto.Email))
                        .Returns((Patient)null);

            _patientRepo.Setup(x => x.GetAll(null, null))
                        .Returns(new List<Patient>());

            Patient saved = null;

            _patientRepo.Setup(x => x.Add(It.IsAny<Patient>()))
                .Callback<Patient>(p => saved = p)
                .Returns(new Patient { PatientId = 20 });

            _service.Create(dto, out _, out _);

            Assert.NotNull(saved);
            Assert.Equal(dto.FullName, saved.FullName);
            Assert.Equal(dto.Email, saved.Email);
            Assert.Equal(dto.PhoneNumber, saved.PhoneNumber);
        }

        [Fact]
        public void Update_DuplicateEmail_ReturnsFalse()
        {
            var dto = GetValidDto();

            _patientRepo.Setup(x => x.GetByEmail(dto.Email))
                .Returns(new Patient { PatientId = 2 });

            var result = _service.Update(1, dto, out string error);

            Assert.False(result);
            Assert.Equal("Another patient with this email already exists.", error);
        }

        [Fact]
        public void Update_NotFound_ReturnsFalse()
        {
            var dto = GetValidDto();

            _patientRepo.Setup(x => x.GetByEmail(dto.Email))
                        .Returns((Patient)null);

            _patientRepo.Setup(x => x.Update(It.IsAny<Patient>()))
                        .Returns(false);

            var result = _service.Update(1, dto, out string error);

            Assert.False(result);
            Assert.Equal("Patient not found.", error);
        }

        [Fact]
        public void Update_Valid_ReturnsTrue()
        {
            var dto = GetValidDto();

            _patientRepo.Setup(x => x.GetByEmail(dto.Email))
                        .Returns((Patient)null);

            _patientRepo.Setup(x => x.Update(It.IsAny<Patient>()))
                        .Returns(true);

            var result = _service.Update(1, dto, out string error);

            Assert.True(result);
            Assert.Equal(string.Empty, error);
        }

        [Fact]
        public void Delete_Valid_ReturnsTrue()
        {
            _patientRepo.Setup(x => x.Delete(1))
                        .Returns(true);

            var result = _service.Delete(1);

            Assert.True(result);
        }

        [Fact]
        public void Delete_Invalid_ReturnsFalse()
        {
            _patientRepo.Setup(x => x.Delete(1))
                        .Returns(false);

            var result = _service.Delete(1);

            Assert.False(result);
        }
    }

}
