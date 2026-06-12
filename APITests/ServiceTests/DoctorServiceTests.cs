namespace APITests.ServiceTests
{
    using Xunit;
    using Moq;
    using System.Collections.Generic;
    using HealthAxisApp.Data;
    using HealthAxisApp.Repositories;
    using HealthAxisApp.Services.Impl;
    using HealthAxisApp.Shared.DTOs;
    using HealthAxisApp.Shared.Enums;

    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _doctorRepo;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _doctorRepo = new Mock<IDoctorRepository>();

            _service = new DoctorService(_doctorRepo.Object);

            _doctorRepo.Setup(x => x.GetAll(null, null, false))
                       .Returns(new List<Doctor>());
        }

        private DoctorDto GetValidDto()
        {
            return new DoctorDto
            {
                FullName = "Dr John",
                Specialisation = SpecialisationEnum.Cardiologist,
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            };
        }

        [Fact]
        public void GetAll_ReturnsMappedDtos()
        {
            _doctorRepo.Setup(x => x.GetAll(null, null, false))
                .Returns(new List<Doctor>
                {
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr A",
                    Specialisation = "Cardiology",
                    YearsOfExperience = 5,
                    ConsultationFee = 500,
                    IsActive = true
                }
                });

            var result = _service.GetAll();

            Assert.Single(result);
        }

        [Fact]
        public void GetById_NotFound_ReturnsNull()
        {
            _doctorRepo.Setup(x => x.GetById(1))
                       .Returns((Doctor)null);

            var result = _service.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public void GetById_Valid_ReturnsDoctor()
        {
            _doctorRepo.Setup(x => x.GetById(1))
                .Returns(new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr A",
                    Specialisation = "Cardiology",
                    YearsOfExperience = 5,
                    ConsultationFee = 500,
                    IsActive = true
                });

            _doctorRepo.Setup(x => x.GetUpcomingAppointmentCount(1))
                       .Returns(3);

            var result = _service.GetById(1);

            Assert.NotNull(result);
            Assert.Equal(3, result.UpcomingAppointmentCount);
        }

        [Fact]
        public void Create_NegativeFee_ReturnsFalse()
        {
            var dto = GetValidDto();
            dto.ConsultationFee = -10;

            var result = _service.Create(dto, out string error, out int id);

            Assert.False(result);
            Assert.Equal("Fee cannot be negative.", error);
            Assert.Equal(0, id);
        }

        [Fact]
        public void Create_Valid_ReturnsTrue()
        {
            var dto = GetValidDto();

            _doctorRepo.Setup(x => x.Add(It.IsAny<Doctor>()))
                       .Returns(new Doctor { DoctorId = 10 });

            var result = _service.Create(dto, out string error, out int id);

            Assert.True(result);
            Assert.Equal(string.Empty, error);
            Assert.Equal(10, id);
        }

        [Fact]
        public void Update_NegativeFee_ReturnsFalse()
        {
            var dto = GetValidDto();
            dto.ConsultationFee = -5;

            var result = _service.Update(1, dto, out string error);

            Assert.False(result);
            Assert.Equal("Fee cannot be negative.", error);
        }

        [Fact]
        public void Update_DoctorNotFound_ReturnsFalse()
        {
            var dto = GetValidDto();

            _doctorRepo.Setup(x => x.Update(It.IsAny<Doctor>()))
                       .Returns(false);

            var result = _service.Update(1, dto, out string error);

            Assert.False(result);
            Assert.Equal("Doctor not found.", error);
        }

        [Fact]
        public void Update_Valid_ReturnsTrue()
        {
            var dto = GetValidDto();

            _doctorRepo.Setup(x => x.Update(It.IsAny<Doctor>()))
                       .Returns(true);

            var result = _service.Update(1, dto, out string error);

            Assert.True(result);
            Assert.Equal(string.Empty, error);
        }

        [Fact]
        public void ToggleStatus_Valid_ReturnsTrue()
        {
            _doctorRepo.Setup(x => x.ToggleStatus(1))
                       .Returns(true);

            var result = _service.ToggleStatus(1);

            Assert.True(result);
        }

        [Fact]
        public void ToggleStatus_Invalid_ReturnsFalse()
        {
            _doctorRepo.Setup(x => x.ToggleStatus(1))
                       .Returns(false);

            var result = _service.ToggleStatus(1);

            Assert.False(result);
        }
    }
}
