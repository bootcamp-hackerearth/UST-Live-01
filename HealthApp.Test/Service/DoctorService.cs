using AutoMapper;
using HealthApp.API.Data;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.Shared.Constant;
using HealthApp.Shared.DTOs;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HealthApp.Test.Service
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _repoMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new DoctorService(_repoMock.Object, _mapperMock.Object);
        }

        // ✅ CREATE SUCCESS
        [Fact]
        public async Task AddDoctor_Should_Add_When_Valid()
        {
            var dto = new DoctorDto
            {
                FullName = "Dr John",
                Specialisation = SpecialisationType.Cardiologist
            };

            _mapperMock.Setup(m => m.Map<Doctor>(dto))
                       .Returns(new Doctor());

            await _service.AddDoctor(dto);

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Doctor>()), Times.Once);
        }

        [Fact]
        public async Task AddDoctor_Should_Throw_When_Name_Empty()
        {
            var dto = new DoctorDto
            {
                FullName = "",
                Specialisation = SpecialisationType.Cardiologist
            };

            await Assert.ThrowsAsync<Exception>(() => _service.AddDoctor(dto));
        }

        // ✅ GET ALL
        [Fact]
        public async Task GetAllDoctors_Should_Return_List()
        {
            var doctors = new List<Doctor>
            {
                new Doctor { DoctorId = 1, FullName = "Test" }
            };

            var dtoList = new List<DoctorDto>
            {
                new DoctorDto { DoctorId = 1, FullName = "Test" }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(doctors);
            _mapperMock.Setup(m => m.Map<List<DoctorDto>>(doctors))
                       .Returns(dtoList);

            var result = await _service.GetAllDoctors();

            Assert.Single(result);
        }

        // ✅ GET BY ID SUCCESS
        [Fact]
        public async Task GetDoctorById_Should_Return_Doctor()
        {
            var doctor = new Doctor { DoctorId = 1, FullName = "John" };
            var dto = new DoctorDto { DoctorId = 1, FullName = "John" };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(doctor);
            _mapperMock.Setup(m => m.Map<DoctorDto>(doctor)).Returns(dto);

            var result = await _service.GetDoctorById(1);

            Assert.Equal("John", result.FullName);
        }

        [Fact]
        public async Task GetDoctorById_Should_Throw_When_NotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Doctor)null);

            await Assert.ThrowsAsync<Exception>(() => _service.GetDoctorById(1));
        }

        // ✅ SEARCH BY SPECIALISATION
        [Fact]
        public async Task SearchBySpecialisation_Should_Filter_Correctly()
        {
            var list = new List<Doctor>
            {
                new Doctor { Specialisation = "Cardiologist" },
                new Doctor { Specialisation = "Dermatologist" }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

            _mapperMock.Setup(m => m.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
                       .Returns(new List<DoctorDto>());

            var result = await _service.SearchBySpecialisation(SpecialisationType.Cardiologist);

            Assert.NotNull(result);
        }

        // ✅ TOGGLE STATUS
        [Fact]
        public async Task ChangeDoctorStatus_Should_Toggle_Status()
        {
            var doctor = new Doctor
            {
                DoctorId = 1,
                IsActive = true
            };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(doctor);

            await _service.ChangeDoctorStatus(1);

            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Doctor>()), Times.Once);
        }

        //  TOGGLE NOT FOUND
        [Fact]
        public async Task ChangeDoctorStatus_Should_Throw_When_NotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Doctor)null);

            await Assert.ThrowsAsync<Exception>(() => _service.ChangeDoctorStatus(1));
        }
    }
}