using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAppTesting
{
    using AutoMapper;
    using global::HealthAppWebAPI;
    using global::HealthAppWebAPI.Models.Dtos;
    using global::HealthAppWebAPI.Repositories.Interfaces;
    using global::HealthAppWebAPI.Services.Impl;
    using HealthAppMVC.Enums;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    namespace HealthAppWebAPI.Tests.Services
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

                _service = new DoctorService(
                    _repoMock.Object,
                    _mapperMock.Object);
            }

            private static CreateDoctorDto GetValidCreateDoctorDto()
            {
                return new CreateDoctorDto
                {
                    FullName = "Dr John",
                    Specialisation = SpecialisationType.Cardiologist.ToString(),
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    DoctorEmail = " doctor@test.com ",
                    DoctorPhoneNo = "9876543210"
                };
            }

            [Fact]
            public async Task GetDoctorByIdAsync_WhenDoctorNotFound_ThrowsKeyNotFoundException()
            {
                int doctorId = 1;

                _repoMock
                    .Setup(r => r.GetByIdAsync(doctorId))
                    .ReturnsAsync((Doctor)null);

                var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _service.GetDoctorByIdAsync(doctorId));

                Assert.Equal("Doctor not found.", exception.Message);
            }

            [Fact]
            public async Task AddDoctorAsync_WhenDtoIsNull_ThrowsArgumentException()
            {
                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => _service.AddDoctorAsync(null));

                Assert.Equal("Doctor data is required.", exception.Message);

                _repoMock.Verify(
                    r => r.AddAsync(It.IsAny<Doctor>()),
                    Times.Never);
            }

            [Fact]
            public async Task AddDoctorAsync_WhenFullNameIsEmpty_ThrowsArgumentException()
            {
                var dto = GetValidCreateDoctorDto();
                dto.FullName = " ";

                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => _service.AddDoctorAsync(dto));

                Assert.Equal("Doctor name is required.", exception.Message);

                _repoMock.Verify(
                    r => r.AddAsync(It.IsAny<Doctor>()),
                    Times.Never);
            }

            [Fact]
            public async Task AddDoctorAsync_WhenSpecialisationIsEmpty_ThrowsArgumentException()
            {
                var dto = GetValidCreateDoctorDto();
                dto.Specialisation = " ";

                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => _service.AddDoctorAsync(dto));

                Assert.Equal("Specialisation is required.", exception.Message);

                _repoMock.Verify(
                    r => r.AddAsync(It.IsAny<Doctor>()),
                    Times.Never);
            }

            [Fact]
            public async Task AddDoctorAsync_WhenSpecialisationIsInvalid_ThrowsArgumentException()
            {
                var dto = GetValidCreateDoctorDto();
                dto.Specialisation = "InvalidSpecialisation";

                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => _service.AddDoctorAsync(dto));

                Assert.Equal("Invalid Specialisation.", exception.Message);

                _repoMock.Verify(
                    r => r.AddAsync(It.IsAny<Doctor>()),
                    Times.Never);
            }

            [Fact]
            public async Task AddDoctorAsync_WhenEmailIsEmpty_ThrowsArgumentException()
            {
                var dto = GetValidCreateDoctorDto();
                dto.DoctorEmail = " ";

                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => _service.AddDoctorAsync(dto));

                Assert.Equal("Doctor email is required.", exception.Message);

                _repoMock.Verify(
                    r => r.AddAsync(It.IsAny<Doctor>()),
                    Times.Never);
            }

            [Fact]
            public async Task AddDoctorAsync_WhenEmailAlreadyExists_ThrowsInvalidOperationException()
            {
                var dto = GetValidCreateDoctorDto();

                _repoMock
                    .Setup(r => r.EmailExistsAsync(dto.DoctorEmail.Trim()))
                    .ReturnsAsync(true);

                var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                    () => _service.AddDoctorAsync(dto));

                Assert.Equal(
                    "A doctor with this email already exists.",
                    exception.Message);

                _repoMock.Verify(
                    r => r.EmailExistsAsync(dto.DoctorEmail.Trim()),
                    Times.Once);

                _repoMock.Verify(
                    r => r.AddAsync(It.IsAny<Doctor>()),
                    Times.Never);
            }

            [Fact]
            public async Task AddDoctorAsync_WhenValid_AddsDoctor()
            {
                var dto = GetValidCreateDoctorDto();

                Doctor addedDoctor = null;

                _repoMock
                    .Setup(r => r.EmailExistsAsync(dto.DoctorEmail.Trim()))
                    .ReturnsAsync(false);

                _mapperMock
                    .Setup(m => m.Map<Doctor>(dto))
                    .Returns(new Doctor());

                _repoMock
                    .Setup(r => r.AddAsync(It.IsAny<Doctor>()))
                    .Callback<Doctor>(doctor =>
                    {
                        addedDoctor = doctor;
                    })
                    .Returns(Task.CompletedTask);

                await _service.AddDoctorAsync(dto);

                Assert.NotNull(addedDoctor);
                Assert.Equal(dto.DoctorEmail.Trim(), addedDoctor.DoctorEmail);
                Assert.Equal(
                    SpecialisationType.Cardiologist.ToString(),
                    addedDoctor.Specialisation);

                Assert.True(addedDoctor.IsActive.Value);

                _repoMock.Verify(
                    r => r.EmailExistsAsync(dto.DoctorEmail.Trim()),
                    Times.Once);

                _repoMock.Verify(
                    r => r.AddAsync(It.IsAny<Doctor>()),
                    Times.Once);
            }

            [Fact]
            public async Task UpdateDoctorAsync_WhenDoctorNotFound_ThrowsKeyNotFoundException()
            {
                int doctorId = 1;
                var dto = GetValidCreateDoctorDto();

                _repoMock
                    .Setup(r => r.GetByIdAsync(doctorId))
                    .ReturnsAsync((Doctor)null);

                var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _service.UpdateDoctorAsync(doctorId, dto));

                Assert.Equal("Doctor not found.", exception.Message);

                _repoMock.Verify(
                    r => r.UpdateAsync(It.IsAny<Doctor>()),
                    Times.Never);
            }

            [Fact]
            public async Task UpdateDoctorAsync_WhenDtoIsNull_ThrowsArgumentException()
            {
                int doctorId = 1;

                var doctor = new Doctor
                {
                    DoctorId = doctorId,
                    FullName = "Dr John"
                };

                _repoMock
                    .Setup(r => r.GetByIdAsync(doctorId))
                    .ReturnsAsync(doctor);

                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => _service.UpdateDoctorAsync(doctorId, null));

                Assert.Equal("Doctor data is required.", exception.Message);

                _repoMock.Verify(
                    r => r.UpdateAsync(It.IsAny<Doctor>()),
                    Times.Never);
            }

            [Fact]
            public async Task UpdateDoctorAsync_WhenFullNameIsEmpty_ThrowsArgumentException()
            {
                int doctorId = 1;

                var doctor = new Doctor
                {
                    DoctorId = doctorId,
                    FullName = "Dr John"
                };

                var dto = GetValidCreateDoctorDto();
                dto.FullName = " ";

                _repoMock
                    .Setup(r => r.GetByIdAsync(doctorId))
                    .ReturnsAsync(doctor);

                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => _service.UpdateDoctorAsync(doctorId, dto));

                Assert.Equal("Doctor name is required.", exception.Message);

                _repoMock.Verify(
                    r => r.UpdateAsync(It.IsAny<Doctor>()),
                    Times.Never);
            }

            [Fact]
            public async Task UpdateDoctorAsync_WhenSpecialisationIsEmpty_ThrowsArgumentException()
            {
                int doctorId = 1;

                var doctor = new Doctor
                {
                    DoctorId = doctorId,
                    FullName = "Dr John"
                };

                var dto = GetValidCreateDoctorDto();
                dto.Specialisation = " ";

                _repoMock
                    .Setup(r => r.GetByIdAsync(doctorId))
                    .ReturnsAsync(doctor);

                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => _service.UpdateDoctorAsync(doctorId, dto));

                Assert.Equal("Specialisation is required.", exception.Message);

                _repoMock.Verify(
                    r => r.UpdateAsync(It.IsAny<Doctor>()),
                    Times.Never);
            }

            [Fact]
            public async Task UpdateDoctorAsync_WhenSpecialisationIsInvalid_ThrowsArgumentException()
            {
                int doctorId = 1;

                var doctor = new Doctor
                {
                    DoctorId = doctorId,
                    FullName = "Dr John"
                };

                var dto = GetValidCreateDoctorDto();
                dto.Specialisation = "InvalidSpecialisation";

                _repoMock
                    .Setup(r => r.GetByIdAsync(doctorId))
                    .ReturnsAsync(doctor);

                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => _service.UpdateDoctorAsync(doctorId, dto));

                Assert.Equal("Invalid Specialisation.", exception.Message);

                _repoMock.Verify(
                    r => r.UpdateAsync(It.IsAny<Doctor>()),
                    Times.Never);
            }

            [Fact]
            public async Task UpdateDoctorAsync_WhenEmailIsEmpty_ThrowsArgumentException()
            {
                int doctorId = 1;

                var doctor = new Doctor
                {
                    DoctorId = doctorId,
                    FullName = "Dr John"
                };

                var dto = GetValidCreateDoctorDto();
                dto.DoctorEmail = " ";

                _repoMock
                    .Setup(r => r.GetByIdAsync(doctorId))
                    .ReturnsAsync(doctor);

                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => _service.UpdateDoctorAsync(doctorId, dto));

                Assert.Equal("Doctor email is required.", exception.Message);

                _repoMock.Verify(
                    r => r.UpdateAsync(It.IsAny<Doctor>()),
                    Times.Never);
            }

            [Fact]
            public async Task UpdateDoctorAsync_WhenEmailUsedByAnotherDoctor_ThrowsInvalidOperationException()
            {
                int doctorId = 1;

                var doctor = new Doctor
                {
                    DoctorId = doctorId,
                    FullName = "Dr John"
                };

                var dto = GetValidCreateDoctorDto();

                _repoMock
                    .Setup(r => r.GetByIdAsync(doctorId))
                    .ReturnsAsync(doctor);

                _repoMock
                    .Setup(r => r.EmailExistsForOtherDoctorAsync(
                        doctorId,
                        dto.DoctorEmail.Trim()))
                    .ReturnsAsync(true);

                var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                    () => _service.UpdateDoctorAsync(doctorId, dto));

                Assert.Equal(
                    "Another doctor already uses this email.",
                    exception.Message);

                _repoMock.Verify(
                    r => r.UpdateAsync(It.IsAny<Doctor>()),
                    Times.Never);
            }

            [Fact]
            public async Task UpdateDoctorAsync_WhenValid_UpdatesDoctor()
            {
                int doctorId = 1;

                var doctor = new Doctor
                {
                    DoctorId = doctorId,
                    FullName = "Old Name",
                    DoctorEmail = "old@test.com",
                    Specialisation = SpecialisationType.ENT.ToString()
                };

                var dto = GetValidCreateDoctorDto();

                _repoMock
                    .Setup(r => r.GetByIdAsync(doctorId))
                    .ReturnsAsync(doctor);

                _repoMock
                    .Setup(r => r.EmailExistsForOtherDoctorAsync(
                        doctorId,
                        dto.DoctorEmail.Trim()))
                    .ReturnsAsync(false);

                _repoMock
                    .Setup(r => r.UpdateAsync(It.IsAny<Doctor>()))
                    .Returns(Task.CompletedTask);

                await _service.UpdateDoctorAsync(doctorId, dto);

                Assert.Equal(dto.DoctorEmail.Trim(), doctor.DoctorEmail);
                Assert.Equal(
                    SpecialisationType.Cardiologist.ToString(),
                    doctor.Specialisation);

                _repoMock.Verify(
                    r => r.UpdateAsync(doctor),
                    Times.Once);
            }

            [Fact]
            public async Task ChangeStatusAsync_WhenDoctorNotFound_ThrowsKeyNotFoundException()
            {
                int doctorId = 1;

                _repoMock
                    .Setup(r => r.GetByIdAsync(doctorId))
                    .ReturnsAsync((Doctor)null);

                var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _service.ChangeStatusAsync(doctorId, false));

                Assert.Equal("Doctor not found.", exception.Message);

                _repoMock.Verify(
                    r => r.ChangeStatusAsync(
                        It.IsAny<int>(),
                        It.IsAny<bool>()),
                    Times.Never);
            }

            [Fact]
            public async Task ChangeStatusAsync_WhenDoctorExists_ChangesStatus()
            {
                int doctorId = 1;

                var doctor = new Doctor
                {
                    DoctorId = doctorId,
                    FullName = "Dr John",
                    IsActive = true
                };

                _repoMock
                    .Setup(r => r.GetByIdAsync(doctorId))
                    .ReturnsAsync(doctor);

                _repoMock
                    .Setup(r => r.ChangeStatusAsync(doctorId, false))
                    .Returns(Task.CompletedTask);

                await _service.ChangeStatusAsync(doctorId, false);

                _repoMock.Verify(
                    r => r.ChangeStatusAsync(doctorId, false),
                    Times.Once);
            }

            [Fact]
            public async Task GetDoctorsBySpecialisationAsync_WhenSpecialisationIsEmpty_ThrowsArgumentException()
            {
                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => _service.GetDoctorsBySpecialisationAsync(" "));

                Assert.Equal("Specialisation is required.", exception.Message);
            }

            [Fact]
            public async Task GetDoctorsBySpecialisationAsync_WhenSpecialisationIsInvalid_ThrowsArgumentException()
            {
                var exception = await Assert.ThrowsAsync<ArgumentException>(
                    () => _service.GetDoctorsBySpecialisationAsync("InvalidSpecialisation"));

                Assert.Equal("Invalid Specialisation.", exception.Message);
            }
        }
    }
}
