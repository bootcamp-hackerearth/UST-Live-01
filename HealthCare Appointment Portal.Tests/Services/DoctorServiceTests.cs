using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository>
     _doctorRepositoryMock;

        private readonly Mock<IAppointmentRepository>
            _appointmentRepositoryMock;

        private readonly Mock<IUserRepository>
            _userRepositoryMock;

        private readonly Mock<IMapper>
            _mapperMock;

        private readonly DoctorService
            _service;

        public DoctorServiceTests()
        {
            _doctorRepositoryMock =
                new Mock<IDoctorRepository>();

            _appointmentRepositoryMock =
                new Mock<IAppointmentRepository>();

            _userRepositoryMock =
                new Mock<IUserRepository>();

            _mapperMock =
                new Mock<IMapper>();

            _service =
                new DoctorService(
                    _doctorRepositoryMock.Object,
                    _appointmentRepositoryMock.Object,
                    _userRepositoryMock.Object,
                    null!,
                    _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllDoctorsAsync_ReturnsDoctors()
        {
            var doctors = new List<Doctor>
    {
        new Doctor(),
        new Doctor()
    };

            var doctorDtos = new List<DoctorDto>
    {
        new DoctorDto(),
        new DoctorDto()
    };

            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var result =
                await _service.GetAllDoctorsAsync();

            Assert.Equal(
                2,
                result.Count());

            _doctorRepositoryMock.Verify(
                x => x.GetAllAsync(),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<IEnumerable<DoctorDto>>(doctors),
                Times.Once);
        }

        [Fact]
        public async Task GetDoctorByIdAsync_ReturnsDoctor()
        {
            var doctor = new Doctor
            {
                DoctorId = 1
            };

            var doctorDto =
                new DoctorDto();

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _mapperMock
                .Setup(x =>
                    x.Map<DoctorDto>(doctor))
                .Returns(doctorDto);

            var result =
                await _service.GetDoctorByIdAsync(1);

            Assert.NotNull(result);

            _mapperMock.Verify(
                x => x.Map<DoctorDto>(doctor),
                Times.Once);
        }

        [Fact]
        public async Task GetDoctorByIdAsync_WhenDoctorNotFound_ThrowsException()
        {
            _doctorRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        It.IsAny<int>()))
                .ReturnsAsync((Doctor)null!);

            await Assert.ThrowsAsync<
                DoctorNotFoundException>(
                () => _service.GetDoctorByIdAsync(1));
        }

        [Fact]
        public async Task AddDoctorAsync_ReturnsDoctorId()
        {
            var createDoctorDto =
                new CreateDoctorDto
                {
                    FullName = "Test Doctor",
                    Specialisation =
                        Specialisation.Cardiology,
                    YearsOfExperience = 5,
                    ConsultationFee = 500
                };

            var doctor =
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Test Doctor"
                };

            _mapperMock
                .Setup(x =>
                    x.Map<Doctor>(
                        createDoctorDto))
                .Returns(doctor);

            _doctorRepositoryMock
                .Setup(x =>
                    x.AddAsync(
                        It.IsAny<Doctor>()))
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x =>
                    x.AddAsync(
                        It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<
                NullReferenceException>(
                () => _service.AddDoctorAsync(
                    createDoctorDto));
        }
        [Fact]
        public async Task UpdateDoctorAsync_UpdatesSuccessfully()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1
                };

            var updateDoctorDto =
                new UpdateDoctorDto
                {
                    FullName = "Updated Doctor",
                    Specialisation = Specialisation.Cardiology,
                    YearsOfExperience = 10,
                    ConsultationFee = 1000,
                    IsActive = true
                };

            _doctorRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _doctorRepositoryMock
                .Setup(x =>
                    x.UpdateAsync(doctor))
                .Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<
                NullReferenceException>(
                () => _service.UpdateDoctorAsync(
                    1,
                    updateDoctorDto));

            _mapperMock.Verify(
                x => x.Map(
                    updateDoctorDto,
                    doctor),
                Times.Once);

            _doctorRepositoryMock.Verify(
                x => x.UpdateAsync(
                    doctor),
                Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenDoctorNotFound_ThrowsException()
        {
            _doctorRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        It.IsAny<int>()))
                .ReturnsAsync((Doctor)null!);

            await Assert.ThrowsAsync<
                DoctorNotFoundException>(
                () => _service.UpdateDoctorAsync(
                    1,
                    new UpdateDoctorDto()));
        }

        [Fact]
        public async Task DeleteDoctorAsync_WhenDoctorNotFound_ThrowsException()
        {
            _doctorRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        It.IsAny<int>()))
                .ReturnsAsync((Doctor)null!);

            await Assert.ThrowsAsync<
                DoctorNotFoundException>(
                () => _service.DeleteDoctorAsync(1));
        }

        [Fact]
        public async Task DeleteDoctorAsync_WhenConfirmedAppointmentExists_ThrowsException()
        {
            var doctor =
                new Doctor
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
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(appointments);

            await Assert.ThrowsAsync<
                DoctorDeletionException>(
                () => _service.DeleteDoctorAsync(1));
        }

        [Fact]
        public async Task DeleteDoctorAsync_WithPendingAppointments_CancelsAppointments()
        {
            var doctor =
                new Doctor
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
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(appointments);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.UpdateAsync(
                        It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x =>
                    x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<
                NullReferenceException>(
                () => _service.DeleteDoctorAsync(1));

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<Appointment>()),
                Times.Once);

            _doctorRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeleteDoctorAsync_MultiplePendingAppointments_CancelsAllAppointments()
        {
            var doctor =
                new Doctor
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
            },
            new Appointment
            {
                Status =
                    AppointmentStatus.Pending
            }
                };

            _doctorRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(appointments);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.UpdateAsync(
                        It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            _doctorRepositoryMock
                .Setup(x =>
                    x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<
                NullReferenceException>(
                () => _service.DeleteDoctorAsync(1));

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<Appointment>()),
                Times.Exactly(2));

            _doctorRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeleteDoctorAsync_NoAppointments_DeletesDoctor()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1
                };

            _doctorRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(
                    new List<Appointment>());

            _doctorRepositoryMock
                .Setup(x =>
                    x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<
                NullReferenceException>(
                () => _service.DeleteDoctorAsync(1));

            _doctorRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Theory]
        [InlineData(Specialisation.Cardiology)]
        [InlineData(Specialisation.Neurology)]
        [InlineData(Specialisation.Dermatology)]
        [InlineData(Specialisation.Orthopedics)]
        [InlineData(Specialisation.Pediatrics)]
        [InlineData(Specialisation.Gynecology)]
        [InlineData(Specialisation.Oncology)]
        [InlineData(Specialisation.Psychiatry)]
        [InlineData(Specialisation.Ophthalmology)]
        [InlineData(Specialisation.ENT)]
        [InlineData(Specialisation.Pulmonology)]
        [InlineData(Specialisation.Gastroenterology)]
        [InlineData(Specialisation.Nephrology)]
        [InlineData(Specialisation.Urology)]
        [InlineData(Specialisation.Endocrinology)]
        [InlineData(Specialisation.Radiology)]
        [InlineData(Specialisation.GeneralSurgery)]
        [InlineData(Specialisation.Anesthesiology)]
        [InlineData(Specialisation.EmergencyMedicine)]
        [InlineData(Specialisation.GeneralMedicine)]
        public async Task GetDoctorsBySpecialisationAsync_ReturnsDoctors(
            Specialisation specialisation)
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

            _doctorRepositoryMock
                .Setup(x =>
                    x.GetDoctorsBySpecialisationAsync(
                        specialisation))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<DoctorDto>>(
                        doctors))
                .Returns(doctorDtos);

            var result =
                await _service
                    .GetDoctorsBySpecialisationAsync(
                        specialisation);

            Assert.Single(result);

            _doctorRepositoryMock.Verify(
                x => x.GetDoctorsBySpecialisationAsync(
                    specialisation),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<IEnumerable<DoctorDto>>(
                    doctors),
                Times.Once);
        }
    }
}