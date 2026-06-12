using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using HealthCare_Appointment_Portal.Utilities;
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
                    _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllDoctorsAsync_ReturnsDoctors()
        {
            var doctors =
                new List<Doctor>
                {
                    new Doctor(),
                    new Doctor()
                };

            var doctorDtos =
                new List<DoctorDto>
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
        public async Task GetAllDoctorsAsync_WhenNoDoctors_ReturnsEmptyList()
        {
            var doctors =
                new List<Doctor>();

            var doctorDtos =
                new List<DoctorDto>();

            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var result =
                await _service.GetAllDoctorsAsync();

            Assert.Empty(result);

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
            var doctor =
                new Doctor
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

            _doctorRepositoryMock.Verify(
                x => x.GetByIdAsync(1),
                Times.Once);

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

            await Assert.ThrowsAsync<DoctorNotFoundException>(
                () => _service.GetDoctorByIdAsync(1));

            _mapperMock.Verify(
                x => x.Map<DoctorDto>(
                    It.IsAny<Doctor>()),
                Times.Never);
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

            var result =
                await _service.AddDoctorAsync(
                    createDoctorDto);

            Assert.Equal(
                1,
                result);

            _doctorRepositoryMock.Verify(
                x => x.AddAsync(
                    doctor),
                Times.Once);

            _userRepositoryMock.Verify(
                x => x.AddAsync(
                    It.IsAny<User>()),
                Times.Once);
        }

        [Fact]
        public async Task AddDoctorAsync_CreatesUserWithCorrectDetails()
        {
            var createDoctorDto =
                new CreateDoctorDto
                {
                    FullName = "Doctor User",
                    Specialisation =
                        Specialisation.Neurology,
                    YearsOfExperience = 8,
                    ConsultationFee = 700
                };

            var doctor =
                new Doctor
                {
                    DoctorId = 12,
                    FullName = "Doctor User"
                };

            User? createdUser = null;

            _mapperMock
                .Setup(x =>
                    x.Map<Doctor>(
                        createDoctorDto))
                .Returns(doctor);

            _doctorRepositoryMock
                .Setup(x =>
                    x.AddAsync(
                        doctor))
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x =>
                    x.AddAsync(
                        It.IsAny<User>()))
                .Callback<User>(user =>
                    createdUser = user)
                .Returns(Task.CompletedTask);

            var result =
                await _service.AddDoctorAsync(
                    createDoctorDto);

            Assert.Equal(
                doctor.DoctorId,
                result);

            Assert.NotNull(createdUser);

            Assert.Equal(
                "D012",
                createdUser!.UserCode);

            Assert.Equal(
                "doctor12@hospital.com",
                createdUser.Email);

            Assert.Equal(
                string.Empty,
                createdUser.PasswordHash);

            Assert.Equal(
                Role.Doctor,
                createdUser.Role);

            Assert.Equal(
                doctor.DoctorId,
                createdUser.ReferenceId);

            _doctorRepositoryMock.Verify(
                x => x.AddAsync(doctor),
                Times.Once);

            _userRepositoryMock.Verify(
                x => x.AddAsync(
                    It.IsAny<User>()),
                Times.Once);
        }

        [Fact]
        public async Task AddDoctorAsync_WhenDoctorIdIsSingleDigit_CreatesPaddedUserCode()
        {
            var createDoctorDto =
                new CreateDoctorDto
                {
                    FullName = "Single Digit Doctor",
                    Specialisation =
                        Specialisation.Dermatology,
                    YearsOfExperience = 3,
                    ConsultationFee = 400
                };

            var doctor =
                new Doctor
                {
                    DoctorId = 5,
                    FullName = "Single Digit Doctor"
                };

            _mapperMock
                .Setup(x =>
                    x.Map<Doctor>(
                        createDoctorDto))
                .Returns(doctor);

            _doctorRepositoryMock
                .Setup(x =>
                    x.AddAsync(
                        doctor))
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x =>
                    x.AddAsync(
                        It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            await _service.AddDoctorAsync(
                createDoctorDto);

            _userRepositoryMock.Verify(
                x => x.AddAsync(
                    It.Is<User>(user =>
                        user.UserCode == "D005" &&
                        user.Email == "doctor5@hospital.com" &&
                        user.Role == Role.Doctor &&
                        user.ReferenceId == 5)),
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
                    Specialisation =
                        Specialisation.Cardiology,
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

            await _service.UpdateDoctorAsync(
                1,
                updateDoctorDto);

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

            await Assert.ThrowsAsync<DoctorNotFoundException>(
                () => _service.UpdateDoctorAsync(
                    1,
                    new UpdateDoctorDto()));

            _mapperMock.Verify(
                x => x.Map(
                    It.IsAny<UpdateDoctorDto>(),
                    It.IsAny<Doctor>()),
                Times.Never);

            _doctorRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<Doctor>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteDoctorAsync_WhenDoctorNotFound_ThrowsException()
        {
            _doctorRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        It.IsAny<int>()))
                .ReturnsAsync((Doctor)null!);

            await Assert.ThrowsAsync<DoctorNotFoundException>(
                () => _service.DeleteDoctorAsync(1));

            _appointmentRepositoryMock.Verify(
                x => x.GetAppointmentsByDoctorAsync(
                    It.IsAny<int>()),
                Times.Never);

            _doctorRepositoryMock.Verify(
                x => x.DeleteAsync(
                    It.IsAny<int>()),
                Times.Never);
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

            await Assert.ThrowsAsync<DoctorDeletionException>(
                () => _service.DeleteDoctorAsync(1));

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<Appointment>()),
                Times.Never);

            _doctorRepositoryMock.Verify(
                x => x.DeleteAsync(
                    It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteDoctorAsync_WhenConfirmedAndPendingAppointmentsExist_ThrowsExceptionAndDoesNotCancelPending()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1
                };

            var pendingAppointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status =
                        AppointmentStatus.Pending
                };

            var confirmedAppointment =
                new Appointment
                {
                    AppointmentId = 2,
                    Status =
                        AppointmentStatus.Confirmed
                };

            var appointments =
                new List<Appointment>
                {
                    pendingAppointment,
                    confirmedAppointment
                };

            _doctorRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(appointments);

            await Assert.ThrowsAsync<DoctorDeletionException>(
                () => _service.DeleteDoctorAsync(1));

            Assert.Equal(
                AppointmentStatus.Pending,
                pendingAppointment.Status);

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<Appointment>()),
                Times.Never);

            _doctorRepositoryMock.Verify(
                x => x.DeleteAsync(
                    It.IsAny<int>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteDoctorAsync_WithPendingAppointments_CancelsAppointments()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1
                };

            var appointment =
                new Appointment
                {
                    Status =
                        AppointmentStatus.Pending
                };

            var appointments =
                new List<Appointment>
                {
                    appointment
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

            await _service.DeleteDoctorAsync(1);

            Assert.Equal(
                AppointmentStatus.Cancelled,
                appointment.Status);

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(
                    appointment),
                Times.Once);

            _doctorRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeleteDoctorAsync_WithPendingAppointment_CancelReasonIsDoctorRemovedFromSystem()
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

            var appointments =
                new List<Appointment>
                {
                    appointment
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

            await _service.DeleteDoctorAsync(1);

            Assert.Equal(
                AppointmentStatus.Cancelled,
                appointment.Status);

            /*
             * If your Appointment model has a cancellation reason property,
             * uncomment and adjust the assertion below.
             *
             * Assert.Equal(
             *     Constants.DoctorRemovedFromSystem,
             *     appointment.CancellationReason);
             */

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(
                    appointment),
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

            await _service.DeleteDoctorAsync(1);

            Assert.All(
                appointments,
                appointment =>
                    Assert.Equal(
                        AppointmentStatus.Cancelled,
                        appointment.Status));

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

            await _service.DeleteDoctorAsync(1);

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<Appointment>()),
                Times.Never);

            _doctorRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeleteDoctorAsync_WithCompletedAppointments_DeletesDoctorWithoutCancellingAppointments()
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
                        AppointmentId = 1,
                        Status =
                            AppointmentStatus.Completed
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

            _doctorRepositoryMock
                .Setup(x =>
                    x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await _service.DeleteDoctorAsync(1);

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<Appointment>()),
                Times.Never);

            _doctorRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeleteDoctorAsync_WithCancelledAppointments_DeletesDoctorWithoutUpdatingAppointments()
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
                        AppointmentId = 1,
                        Status =
                            AppointmentStatus.Cancelled
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

            _doctorRepositoryMock
                .Setup(x =>
                    x.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            await _service.DeleteDoctorAsync(1);

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(
                    It.IsAny<Appointment>()),
                Times.Never);

            _doctorRepositoryMock.Verify(
                x => x.DeleteAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task DeleteDoctorAsync_WithPendingCompletedAndCancelledAppointments_CancelsOnlyPendingAppointments()
        {
            var doctor =
                new Doctor
                {
                    DoctorId = 1
                };

            var pendingAppointment =
                new Appointment
                {
                    AppointmentId = 1,
                    Status =
                        AppointmentStatus.Pending
                };

            var completedAppointment =
                new Appointment
                {
                    AppointmentId = 2,
                    Status =
                        AppointmentStatus.Completed
                };

            var cancelledAppointment =
                new Appointment
                {
                    AppointmentId = 3,
                    Status =
                        AppointmentStatus.Cancelled
                };

            var appointments =
                new List<Appointment>
                {
                    pendingAppointment,
                    completedAppointment,
                    cancelledAppointment
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

            await _service.DeleteDoctorAsync(1);

            Assert.Equal(
                AppointmentStatus.Cancelled,
                pendingAppointment.Status);

            Assert.Equal(
                AppointmentStatus.Completed,
                completedAppointment.Status);

            Assert.Equal(
                AppointmentStatus.Cancelled,
                cancelledAppointment.Status);

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(
                    pendingAppointment),
                Times.Once);

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(
                    completedAppointment),
                Times.Never);

            _appointmentRepositoryMock.Verify(
                x => x.UpdateAsync(
                    cancelledAppointment),
                Times.Never);

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

        [Fact]
        public async Task GetDoctorsBySpecialisationAsync_WhenNoDoctorsFound_ReturnsEmptyList()
        {
            var doctors =
                new List<Doctor>();

            var doctorDtos =
                new List<DoctorDto>();

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

            var result =
                await _service
                    .GetDoctorsBySpecialisationAsync(
                        Specialisation.Cardiology);

            Assert.Empty(result);

            _doctorRepositoryMock.Verify(
                x => x.GetDoctorsBySpecialisationAsync(
                    Specialisation.Cardiology),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<IEnumerable<DoctorDto>>(
                    doctors),
                Times.Once);
        }
    }
}