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
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _service = new DoctorService(
                _unitOfWorkMock.Object,
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

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock.Setup(x =>
                    x.Map<IEnumerable<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var result =
                await _service.GetAllDoctorsAsync();

            Assert.Equal(2, result.Count());

            _unitOfWorkMock.Verify(
                x => x.Doctors.GetAllAsync(),
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

            var doctorDto = new DoctorDto();

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _mapperMock.Setup(x =>
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
            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<DoctorNotFoundException>(
                () => _service.GetDoctorByIdAsync(1));
        }

        [Fact]
        public async Task AddDoctorAsync_ReturnsDoctorId()
        {
            var createDoctorDto =
                new CreateDoctorDto
                {
                    FullName = "Test Doctor",
                    Specialisation = Specialisation.Cardiology,
                    YearsOfExperience = 5,
                    ConsultationFee = 500
                };

            var doctor =
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Test Doctor"
                };

            _mapperMock.Setup(x =>
                    x.Map<Doctor>(createDoctorDto))
                .Returns(doctor);

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.AddAsync(It.IsAny<Doctor>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x =>
                    x.Users.AddAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            var result =
                await _service.AddDoctorAsync(
                    createDoctorDto);

            Assert.Equal(1, result);

            _mapperMock.Verify(
                x => x.Map<Doctor>(createDoctorDto),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.Doctors.AddAsync(It.IsAny<Doctor>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Exactly(2));

            _unitOfWorkMock.Verify(
                x => x.Users.AddAsync(It.Is<User>(
                    u =>
                        u.UserCode == "D001" &&
                        u.Email == "doctor1@hospital.com" &&
                        u.Role == Role.Doctor &&
                        u.ReferenceId == 1)),
                Times.Once);
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

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.UpdateAsync(doctor))
                .Returns(Task.CompletedTask);

            await _service.UpdateDoctorAsync(
                1,
                updateDoctorDto);

            _mapperMock.Verify(
                x => x.Map(updateDoctorDto, doctor),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.Doctors.UpdateAsync(doctor),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateDoctorAsync_WhenDoctorNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<DoctorNotFoundException>(
                () => _service.UpdateDoctorAsync(
                    1,
                    new UpdateDoctorDto()));
        }

        [Fact]
        public async Task DeleteDoctorAsync_WhenDoctorNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<DoctorNotFoundException>(
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
                        Status = AppointmentStatus.Confirmed
                    }
                };

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(appointments);

            await Assert.ThrowsAsync<DoctorDeletionException>(
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
                        Status = AppointmentStatus.Pending
                    }
                };

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(appointments);

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.UpdateAsync(
                        It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await _service.DeleteDoctorAsync(1);

            _unitOfWorkMock.Verify(
                x => x.Appointments.UpdateAsync(
                    It.IsAny<Appointment>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.Doctors.DeleteAsync(1),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
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
                        Status = AppointmentStatus.Pending
                    },
                    new Appointment
                    {
                        Status = AppointmentStatus.Pending
                    }
                };

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(appointments);

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.UpdateAsync(
                        It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await _service.DeleteDoctorAsync(1);

            _unitOfWorkMock.Verify(
                x => x.Appointments.UpdateAsync(
                    It.IsAny<Appointment>()),
                Times.Exactly(2));

            _unitOfWorkMock.Verify(
                x => x.Doctors.DeleteAsync(1),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
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

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(new List<Appointment>());

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await _service.DeleteDoctorAsync(1);

            _unitOfWorkMock.Verify(
                x => x.Doctors.DeleteAsync(1),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
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

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetDoctorsBySpecialisationAsync(
                        specialisation))
                .ReturnsAsync(doctors);

            _mapperMock.Setup(x =>
                    x.Map<IEnumerable<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var result =
                await _service.GetDoctorsBySpecialisationAsync(
                    specialisation);

            Assert.Single(result);

            _unitOfWorkMock.Verify(
                x => x.Doctors.GetDoctorsBySpecialisationAsync(
                    specialisation),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<IEnumerable<DoctorDto>>(doctors),
                Times.Once);
        }
    }
}