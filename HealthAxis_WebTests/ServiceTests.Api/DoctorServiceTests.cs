using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using HealthAxis.Api.Services;
using HealthAxis.Api.Repositories;
using HealthAxis.Api.Models;
using HealthAxis.Shared.Dtos;

namespace HealthAxis.Api.Tests
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _repo;
        private readonly DoctorServiceImpl _service;

        public DoctorServiceTests()
        {
            _repo = new Mock<IDoctorRepository>();
            _service = new DoctorServiceImpl(_repo.Object);
        }

        [Fact]
        public void GetAll_NoFilter_ReturnsAllDoctors()
        {
            _repo.Setup(r => r.GetAll()).Returns(new List<Doctor>
            {
                new Doctor { DoctorId = 1 },
                new Doctor { DoctorId = 2 }
            });

            var result = _service.GetAll(null);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetAll_WithFilter_ReturnsFiltered()
        {
            _repo.Setup(r => r.GetAll()).Returns(new List<Doctor>
            {
                new Doctor { Specialisation = "Cardiology" },
                new Doctor { Specialisation = "Dermatology" }
            });

            var result = _service.GetAll("Cardiology");

            Assert.Single(result);
        }

        [Fact]
        public void GetAll_MapsToDto()
        {
            _repo.Setup(r => r.GetAll()).Returns(new List<Doctor>
            {
                new Doctor { DoctorId = 1, FullName = "Test" }
            });

            var result = _service.GetAll(null);

            Assert.Equal("Test", result.First().FullName);
        }

        [Fact]
        public void GetById_Valid_ReturnsDoctor()
        {
            _repo.Setup(r => r.GetById(1)).Returns(new Doctor
            {
                DoctorId = 1,
                FullName = "Doc"
            });

            var result = _service.GetById(1);

            Assert.NotNull(result);
            Assert.Equal("Doc", result.FullName);
        }

        [Fact]
        public void GetById_Invalid_ReturnsNull()
        {
            _repo.Setup(r => r.GetById(1)).Returns((Doctor)null);

            var result = _service.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public void Add_ValidDto_CallsAdd()
        {
            var dto = new CreateDoctorDto
            {
                FullName = "Doc",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 5,
                ConsultationFee = 500
            };

            _service.Add(dto);

            _repo.Verify(r => r.Add(It.IsAny<Doctor>()), Times.Once);
        }

        [Fact]
        public void Add_CallsSave()
        {
            var dto = new CreateDoctorDto
            {
                FullName = "Doc",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 5,
                ConsultationFee = 500
            };

            _service.Add(dto);

            _repo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void Add_MapsValuesCorrectly()
        {
            Doctor captured = null;

            _repo.Setup(r => r.Add(It.IsAny<Doctor>()))
                .Callback<Doctor>(d => captured = d);

            var dto = new CreateDoctorDto
            {
                FullName = "Doc",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 5,
                ConsultationFee = 500
            };

            _service.Add(dto);

            Assert.Equal("Doc", captured.FullName);
            Assert.Equal("Cardiology", captured.Specialisation);
        }

        [Fact]
        public void Update_Valid_UpdatesDoctor()
        {
            var doctor = new Doctor { DoctorId = 1 };

            _repo.Setup(r => r.GetById(1)).Returns(doctor);

            var dto = new UpdateDoctorDto
            {
                FullName = "Updated",
                Specialisation = "Dermatology",
                YearsOfExperience = 10,
                ConsultationFee = 1000,
                IsActive = true
            };

            _service.Update(1, dto);

            Assert.Equal("Updated", doctor.FullName);
        }

        [Fact]
        public void Update_CallsSave()
        {
            var doctor = new Doctor { DoctorId = 1 };

            _repo.Setup(r => r.GetById(1)).Returns(doctor);

            var dto = new UpdateDoctorDto();

            _service.Update(1, dto);

            _repo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void Update_InvalidId_DoesNothing()
        {
            _repo.Setup(r => r.GetById(1)).Returns((Doctor)null);

            var dto = new UpdateDoctorDto();

            _service.Update(1, dto);

            _repo.Verify(r => r.Update(It.IsAny<Doctor>()), Times.Never);
        }
    }
}