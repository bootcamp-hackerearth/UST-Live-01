using HealthAppWebApi.Models;
using HealthAppWebApi.Repositories.Interface;
using HealthAppWebApi.Services.Impl;
using Moq;
using SharedDto.DoctorDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HealthAppWebApi.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository>
            _doctorRepositoryMock;

        private readonly DoctorService
            _service;

        public DoctorServiceTests()
        {
            _doctorRepositoryMock =
                new Mock<IDoctorRepository>();

            _service =
                new DoctorService(
                    _doctorRepositoryMock.Object);
        }

        #region GetAllDoctorsAsync

        [Fact]
        public async Task
            GetAllDoctorsAsync_ReturnsDoctors()
        {
            // Arrange

            var doctors =
                new List<Doctor>
                {
                    new Doctor
                    {
                        DoctorId = 1,
                        FullName = "Dr John",
                        Specialisation =
                            SpecialisationType.Cardiologist,
                        ConsultationFee = 500,
                        IsActive = true,
                        DoctorEmail =
                            "john@test.com",
                        DoctorPhoneNo =
                            "9999999999"
                    }
                };

            _doctorRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(doctors);

            // Act

            var result =
                await _service
                    .GetAllDoctorsAsync();

            // Assert

            Assert.Single(result);

            Assert.Equal(
                "Dr John",
                result[0].FullName);

            Assert.Equal(
                "Cardiologist",
                result[0].Specialisation);
        }

        #endregion

        #region GetDoctorByIdAsync

        [Fact]
        public async Task
            GetDoctorByIdAsync_ValidId_ReturnsDoctor()
        {
            // Arrange

            var doctor =
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr Smith",
                    Specialisation =
                        SpecialisationType.Neurologist,
                    ConsultationFee = 1000,
                    IsActive = true,
                    DoctorEmail =
                        "smith@test.com",
                    DoctorPhoneNo =
                        "9876543210"
                };

            _doctorRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            // Act

            var result =
                await _service
                    .GetDoctorByIdAsync(1);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(
                "Dr Smith",
                result.FullName);

            Assert.Equal(
                "Neurologist",
                result.Specialisation);
        }

        [Fact]
        public async Task
            GetDoctorByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange

            _doctorRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Doctor)null);

            // Act

            var result =
                await _service
                    .GetDoctorByIdAsync(1);

            // Assert

            Assert.Null(result);
        }

        #endregion

        #region AddDoctorAsync

        [Fact]
        public async Task
            AddDoctorAsync_ValidDoctor_AddsDoctor()
        {
            // Arrange

            var dto =
                new CreateDoctorDto
                {
                    FullName = "Dr John",
                    Specialisation =
                        "Cardiologist",
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    DoctorEmail =
                        "john@test.com",
                    DoctorPhoneNo =
                        "9999999999"
                };

            // Act

            await _service
                .AddDoctorAsync(dto);

            // Assert

            _doctorRepositoryMock
                .Verify(
                    r => r.AddAsync(
                        It.Is<Doctor>(
                            d =>
                                d.FullName ==
                                "Dr John" &&
                                d.IsActive)),
                    Times.Once);
        }

        [Fact]
        public async Task
            AddDoctorAsync_InvalidSpecialisation_ThrowsException()
        {
            // Arrange

            var dto =
                new CreateDoctorDto
                {
                    FullName = "Dr John",
                    Specialisation =
                        "ABC",
                    ConsultationFee = 500,
                    YearsOfExperience = 10
                };

            // Act & Assert

            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service
                            .AddDoctorAsync(dto));

            Assert.Equal(
                "Invalid Specialisation.",
                ex.Message);
        }

        [Fact]
        public async Task
            AddDoctorAsync_InvalidFee_ThrowsException()
        {
            var dto =
                new CreateDoctorDto
                {
                    FullName = "Dr John",
                    Specialisation =
                        "Cardiologist",
                    ConsultationFee = 0,
                    YearsOfExperience = 10
                };

            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service
                            .AddDoctorAsync(dto));

            Assert.Equal(
                "Consultation fee must be greater than zero.",
                ex.Message);
        }

        [Fact]
        public async Task
            AddDoctorAsync_NegativeExperience_ThrowsException()
        {
            var dto =
                new CreateDoctorDto
                {
                    FullName = "Dr John",
                    Specialisation =
                        "Cardiologist",
                    ConsultationFee = 500,
                    YearsOfExperience = -1
                };

            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service
                            .AddDoctorAsync(dto));

            Assert.Equal(
                "Years of experience cannot be negative.",
                ex.Message);
        }

        #endregion

        #region UpdateDoctorAsync

        [Fact]
        public async Task
            UpdateDoctorAsync_DoctorNotFound_ThrowsException()
        {
            _doctorRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Doctor)null);

            var dto =
                new CreateDoctorDto
                {
                    FullName = "Dr Updated",
                    Specialisation =
                        "Cardiologist",
                    ConsultationFee = 500,
                    YearsOfExperience = 10
                };

            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service
                            .UpdateDoctorAsync(
                                1,
                                dto));

            Assert.Equal(
                "Doctor not found.",
                ex.Message);
        }

        [Fact]
        public async Task
            UpdateDoctorAsync_InvalidSpecialisation_ThrowsException()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1
                };

            _doctorRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            var dto =
                new CreateDoctorDto
                {
                    Specialisation =
                        "INVALID"
                };

            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service
                            .UpdateDoctorAsync(
                                1,
                                dto));

            Assert.Equal(
                "Invalid Specialisation.",
                ex.Message);
        }

        [Fact]
        public async Task
            UpdateDoctorAsync_ValidDoctor_UpdatesDoctor()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Old Name",
                    ConsultationFee = 500
                };

            _doctorRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            var dto =
                new CreateDoctorDto
                {
                    FullName =
                        "New Name",
                    Specialisation =
                        "Cardiologist",
                    YearsOfExperience = 12,
                    ConsultationFee = 1000,
                    DoctorEmail =
                        "new@test.com",
                    DoctorPhoneNo =
                        "8888888888"
                };

            await _service
                .UpdateDoctorAsync(
                    1,
                    dto);

            Assert.Equal(
                "New Name",
                doctor.FullName);

            Assert.Equal(
                1000,
                doctor.ConsultationFee);

            _doctorRepositoryMock
                .Verify(
                    r => r.UpdateAsync(
                        doctor),
                    Times.Once);
        }

        #endregion

        #region ChangeStatusAsync

        [Fact]
        public async Task
            ChangeStatusAsync_DoctorNotFound_ThrowsException()
        {
            _doctorRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Doctor)null);

            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service
                            .ChangeStatusAsync(
                                1,
                                false));

            Assert.Equal(
                "Doctor not found.",
                ex.Message);
        }

        [Fact]
        public async Task
            ChangeStatusAsync_ValidDoctor_CallsRepository()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr John"
                };

            _doctorRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            await _service
                .ChangeStatusAsync(
                    1,
                    false);

            _doctorRepositoryMock
                .Verify(
                    r => r.ChangeStatusAsync(
                        1,
                        false),
                    Times.Once);
        }

        #endregion

        #region GetDoctorsBySpecialisationAsync

        [Fact]
        public async Task
            GetDoctorsBySpecialisationAsync_InvalidSpecialisation_ThrowsException()
        {
            var ex =
                await Assert.ThrowsAsync<Exception>(
                    () =>
                        _service
                            .GetDoctorsBySpecialisationAsync(
                                "ABC"));

            Assert.Equal(
                "Invalid Specialisation.",
                ex.Message);
        }

        [Fact]
        public async Task
            GetDoctorsBySpecialisationAsync_ReturnsDoctors()
        {
            var doctors =
                new List<Doctor>
                {
                    new Doctor
                    {
                        DoctorId = 1,
                        FullName = "Dr Heart",
                        Specialisation =
                            SpecialisationType.Cardiologist
                    }
                };

            _doctorRepositoryMock
                .Setup(r =>
                    r.GetBySpecialisationAsync(
                        SpecialisationType.Cardiologist))
                .ReturnsAsync(doctors);

            var result =
                await _service
                    .GetDoctorsBySpecialisationAsync(
                        "Cardiologist");

            Assert.Single(result);

            Assert.Equal(
                "Dr Heart",
                result[0].FullName);
        }

        #endregion

        #region SearchByNameAsync

        [Fact]
        public async Task
            SearchByNameAsync_ReturnsDoctors()
        {
            var doctors =
                new List<Doctor>
                {
                    new Doctor
                    {
                        DoctorId = 1,
                        FullName = "Dr John",
                        Specialisation =
                            SpecialisationType.Cardiologist
                    }
                };

            _doctorRepositoryMock
                .Setup(r =>
                    r.SearchByNameAsync(
                        "John"))
                .ReturnsAsync(doctors);

            var result =
                await _service
                    .SearchByNameAsync(
                        "John");

            Assert.Single(result);

            Assert.Equal(
                "Dr John",
                result[0].FullName);
        }

        [Fact]
        public async Task
            SearchByNameAsync_NoResults_ReturnsEmptyList()
        {
            _doctorRepositoryMock
                .Setup(r =>
                    r.SearchByNameAsync(
                        "XYZ"))
                .ReturnsAsync(
                    new List<Doctor>());

            var result =
                await _service
                    .SearchByNameAsync(
                        "XYZ");

            Assert.Empty(result);
        }

        #endregion
    }
}