using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Tests
{
    public class DoctorServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;

        private Mock<IDoctorRepository> _doctorRepositoryMock;
        private Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;

        private DoctorService _service;
        [TestMethod]
        public async Task AddDoctorAsync_ValidDoctor_ShouldReturnDoctorId()
        {
            // Arrange

            var dto = new CreateDoctorDto();

            var doctor = new Doctor
            {
                DoctorId = 1
            };

            _mapperMock
                .Setup(x => x.Map<Doctor>(dto))
                .Returns(doctor);

            // Act

            int result =
                await _service.AddDoctorAsync(dto);

            // Assert

            Assert.AreEqual(1, result);

            _doctorRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Doctor>()),
                Times.Once);

            _userRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<User>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Exactly(2));
        }

        [TestMethod]
        public async Task GetDoctorByIdAsync_InvalidId_ShouldThrowDoctorNotFoundException()
        {
            // Arrange

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Doctor)null);

            // Act + Assert

            await Assert.ThrowsExceptionAsync<DoctorNotFoundException>(
                () => _service.GetDoctorByIdAsync(1));
        }

        [TestMethod]
        public async Task UpdateDoctorAsync_ValidDoctor_ShouldUpdateDoctor()
        {
            // Arrange

            var doctor = new Doctor
            {
                DoctorId = 1
            };

            var dto = new UpdateDoctorDto();

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            // Act

            await _service.UpdateDoctorAsync(
                1,
                dto);

            // Assert

            _doctorRepositoryMock.Verify(
                x => x.UpdateAsync(doctor),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [TestMethod]
        public async Task DeleteDoctorAsync_ConfirmedAppointments_ShouldThrowDoctorDeletionException()
        {
            // Arrange

            var doctor = new Doctor
            {
                DoctorId = 1
            };

            var appointments =
                new List<Appointment>
                {
            new Appointment
            {
                Status =
                    AppointmentStatus.Confirmed
            }
                };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(appointments);

            // Act + Assert

            await Assert.ThrowsExceptionAsync<DoctorDeletionException>(
                () => _service.DeleteDoctorAsync(1));
        }


        [TestMethod]
        public async Task DeleteDoctorAsync_PendingAppointments_ShouldCancelAppointments()
        {
            // Arrange

            var doctor = new Doctor
            {
                DoctorId = 1
            };

            var appointments =
                new List<Appointment>
                {
            new Appointment
            {
                Status =
                    AppointmentStatus.Pending
            }
                };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(appointments);

            // Act

            await _service.DeleteDoctorAsync(1);

            // Assert

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<Appointment>()),
                Times.Once);

            _doctorRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [TestMethod]
        public async Task DeleteDoctorAsync_NoAppointments_ShouldDeleteDoctor()
        {
            // Arrange

            var doctor = new Doctor
            {
                DoctorId = 1
            };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(
                    new List<Appointment>());

            // Act

            await _service.DeleteDoctorAsync(1);

            // Assert

            _doctorRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [TestMethod]
        public async Task GetDoctorsBySpecialisationAsync_ShouldReturnDoctors()
        {
            // Arrange

            var doctors =
                new List<Doctor>
                {
            new Doctor
            {
                DoctorId = 1
            }
                };

            var doctorDtos =
                new List<DoctorDto>
                {
            new DoctorDto
            {
                DoctorId = 1
            }
                };

            _doctorRepositoryMock
                .Setup(x =>
                    x.GetDoctorsBySpecialisationAsync(
                        Specialisation.Cardiology))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<DoctorDto>>(
                        doctors))
                .Returns(doctorDtos);

            // Act

            var result =
                await _service
                    .GetDoctorsBySpecialisationAsync(
                        Specialisation.Cardiology);

            // Assert

            Assert.AreEqual(
                1,
                result.Count());
        }
    }
}
