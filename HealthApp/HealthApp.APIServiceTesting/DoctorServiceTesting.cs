using Xunit;
using Moq;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthApp.API.Service.Impl;
using HealthApp.API.Repository.Interface;
using HealthApp.Shared.DTOs;
using HealthApp.API.Data;
using HealthApp.Shared.Constant;

namespace HealthApp.APIServiceTesting
{
    public class DoctorServiceTesting
    {
        private readonly Mock<IDoctorRepository> _repo;
        private readonly Mock<IMapper> _mapper;
        private readonly DoctorService _service;

        public DoctorServiceTesting()
        {
            _repo = new Mock<IDoctorRepository>();
            _mapper = new Mock<IMapper>();

            _service = new DoctorService(_repo.Object, _mapper.Object);
        }

        // -------------------- ADD DOCTOR --------------------

        [Fact]
        public async Task AddDoctor_ShouldThrow_WhenNameMissing()
        {
            var dto = new DoctorDto
            {
                FullName = ""
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _service.AddDoctor(dto));
        }

        [Fact]
        public async Task AddDoctor_ShouldAddDoctor_WhenValid()
        {
            var dto = new DoctorDto
            {
                FullName = "Dr John"
            };

            var mappedDoctor = new Doctor();

            _mapper.Setup(x => x.Map<Doctor>(dto))
                .Returns(mappedDoctor);

            await _service.AddDoctor(dto);

            _repo.Verify(x => x.AddAsync(It.IsAny<Doctor>()), Times.Once);
            Assert.True(mappedDoctor.IsActive);
        }

        // -------------------- GET ALL --------------------

        [Fact]
        public async Task GetAllDoctors_ShouldReturnList()
        {
            var doctors = new List<Doctor>
            {
                new Doctor(),
                new Doctor()
            };

            var dtoList = new List<DoctorDto>
            {
                new DoctorDto(),
                new DoctorDto()
            };

            _repo.Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapper.Setup(x => x.Map<List<DoctorDto>>(doctors))
                .Returns(dtoList);

            var result = await _service.GetAllDoctors();

            Assert.Equal(2, result.Count);
        }

        // -------------------- GET BY ID --------------------

        [Fact]
        public async Task GetDoctorById_ShouldThrow_WhenNotFound()
        {
            _repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Doctor)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.GetDoctorById(1));
        }

        [Fact]
        public async Task GetDoctorById_ShouldReturnDto_WhenFound()
        {
            var doctor = new Doctor();
            var dto = new DoctorDto();

            _repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _mapper.Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(dto);

            var result = await _service.GetDoctorById(1);

            Assert.NotNull(result);
        }

        // -------------------- SEARCH --------------------

        [Fact]
        public async Task SearchBySpecialisation_ShouldReturnFilteredDoctors()
        {
            var doctors = new List<Doctor>
            {
                new Doctor { Specialisation = "Cardiology" },
                new Doctor { Specialisation = "Neurology" }
            };

            var filteredDtos = new List<DoctorDto>
            {
                new DoctorDto()
            };

            _repo.Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapper.Setup(x => x.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
                .Returns(filteredDtos);

            var result = await _service.SearchBySpecialisation(SpecialisationType.Cardiologist);

            Assert.Single(result);
        }

        // -------------------- CHANGE STATUS --------------------

        [Fact]
        public async Task ChangeDoctorStatus_ShouldThrow_WhenDoctorNotFound()
        {
            _repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Doctor)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.ChangeDoctorStatus(1));
        }

        [Fact]
        public async Task ChangeDoctorStatus_ShouldToggleStatus()
        {
            var doctor = new Doctor
            {
                IsActive = true
            };

            _repo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            await _service.ChangeDoctorStatus(1);

            Assert.False(doctor.IsActive);
            _repo.Verify(x => x.UpdateAsync(doctor), Times.Once);
        }
    }
}