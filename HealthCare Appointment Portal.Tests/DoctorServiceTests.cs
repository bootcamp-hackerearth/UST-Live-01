using AutoMapper;
using Moq;
using Xunit;

using HealthCare_Appointment_Portal.Services;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IUnitOfWork>
            _unitOfWorkMock;

        private readonly Mock<IMapper>
            _mapperMock;

        private readonly DoctorService
            _service;

        public DoctorServiceTests()
        {
            _unitOfWorkMock =
                new Mock<IUnitOfWork>();

            _mapperMock =
                new Mock<IMapper>();

            _service =
                new DoctorService(
                    _unitOfWorkMock.Object,
                    _mapperMock.Object);
        }

        [Fact]
        public async Task
            GetAllDoctorsAsync_ReturnsDoctors()
        {
            var doctors =
                new List<Doctor>
                {
                    new Doctor
                    {
                        DoctorId = 1,
                        FullName = "Vyshnavi"
                    }
                };

            var doctorDtos =
                new List<DoctorDto>
                {
                    new DoctorDto
                    {
                        DoctorId = 1,
                        FullName = "Vyshnavi"
                    }
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Doctors.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var result =
                await _service
                    .GetAllDoctorsAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task
            GetDoctorByIdAsync_ReturnsDoctor()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1
                };

            var dto =
                new DoctorDto
                {
                    DoctorId = 1
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(x =>
                    x.Map<DoctorDto>(doctor))
                .Returns(dto);

            var result =
                await _service
                    .GetDoctorByIdAsync(1);

            Assert.Equal(
                1,
                result.DoctorId);
        }

        [Fact]
        public async Task
            GetDoctorByIdAsync_NotFound_ThrowsException()
        {
            _unitOfWorkMock
                .Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync((Doctor)null);

            await Assert.ThrowsAsync<
                DoctorNotFoundException>(
                    () =>
                        _service
                            .GetDoctorByIdAsync(1));
        }

        [Fact]
        public async Task
            AddDoctorAsync_ReturnsDoctorId()
        {
            var createDto =
                new CreateDoctorDto
                {
                    FullName = "Doctor One"
                };

            var doctor =
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Doctor One"
                };

            _mapperMock
                .Setup(x =>
                    x.Map<Doctor>(
                        It.IsAny<CreateDoctorDto>()))
                .Returns(doctor);

            _unitOfWorkMock
                .Setup(x =>
                    x.Doctors.AddAsync(
                        It.IsAny<Doctor>()))
                .Returns(Task.CompletedTask);

            var result =
                await _service
                    .AddDoctorAsync(createDto);

            Assert.True(result >= 0);
        }

        [Fact]
        public async Task
            UpdateDoctorAsync_NotFound_ThrowsException()
        {
            _unitOfWorkMock
                .Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync((Doctor)null);

            await Assert.ThrowsAsync<
                DoctorNotFoundException>(
                    () =>
                        _service
                            .UpdateDoctorAsync(
                                1,
                                new UpdateDoctorDto()));
        }

        [Fact]
        public async Task
            UpdateDoctorAsync_ValidDoctor_UpdatesSuccessfully()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            await _service
                .UpdateDoctorAsync(
                    1,
                    new UpdateDoctorDto());

            _unitOfWorkMock.Verify(
                x => x.Doctors.UpdateAsync(doctor),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task
            DeleteDoctorAsync_NotFound_ThrowsException()
        {
            _unitOfWorkMock
                .Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync((Doctor)null);

            await Assert.ThrowsAsync<
                DoctorNotFoundException>(
                    () =>
                        _service
                            .DeleteDoctorAsync(1));
        }

        [Fact]
        public async Task
            DeleteDoctorAsync_WithConfirmedAppointments_ThrowsException()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _unitOfWorkMock
                .Setup(x =>
                    x.Appointments
                     .GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(
                    new List<Appointment>
                    {
                        new Appointment
                        {
                            Status =
                                AppointmentStatus
                                .Confirmed
                        }
                    });

            await Assert.ThrowsAsync<
                DoctorDeletionException>(
                    () =>
                        _service
                            .DeleteDoctorAsync(1));
        }

        [Fact]
        public async Task
            DeleteDoctorAsync_WithPendingAppointments_CancelsAppointments()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1
                };

            var appointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status =
                        AppointmentStatus.Pending
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _unitOfWorkMock
                .Setup(x =>
                    x.Appointments
                     .GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(
                    new List<Appointment>
                    {
                        appointment
                    });

            await _service
                .DeleteDoctorAsync(1);

            _unitOfWorkMock.Verify(
                x => x.Appointments
                      .UpdateAsync(
                          It.IsAny<Appointment>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.Doctors.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task
            DeleteDoctorAsync_DeletesSuccessfully()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _unitOfWorkMock
                .Setup(x =>
                    x.Appointments
                     .GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(
                    new List<Appointment>());

            await _service
                .DeleteDoctorAsync(1);

            _unitOfWorkMock.Verify(
                x => x.Doctors.DeleteAsync(1),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task
            GetDoctorsBySpecialisationAsync_ReturnsDoctors()
        {
            var doctors =
                new List<Doctor>
                {
                    new Doctor()
                };

            var doctorDtos =
                new List<DoctorDto>
                {
                    new DoctorDto()
                };

            _unitOfWorkMock
                .Setup(x =>
                    x.Doctors
                     .GetDoctorsBySpecialisationAsync(
                         Specialisation.Cardiology))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var result =
                await _service
                    .GetDoctorsBySpecialisationAsync(
                        Specialisation.Cardiology);

            Assert.Single(result);
        }
    }
}