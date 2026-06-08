using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Services
{
    public partial class AppointmentServiceTests
    {
        private readonly Mock<IUnitOfWork>
         _unitOfWorkMock;

        private readonly Mock<IAppointmentRepository>
            _appointmentRepoMock;

        private readonly Mock<IHealthRecordRepository>
            _healthRecordRepoMock;

        private readonly Mock<IMapper>
            _mapperMock;


        private readonly AppointmentService
            _service;

        public AppointmentServiceTests()
        {
            _unitOfWorkMock =
                new Mock<IUnitOfWork>();

            _appointmentRepoMock =
                new Mock<IAppointmentRepository>();

            _healthRecordRepoMock =
                new Mock<IHealthRecordRepository>();

            _mapperMock =
                new Mock<IMapper>();

            _unitOfWorkMock
                .Setup(x => x.Appointments)
                .Returns(_appointmentRepoMock.Object);

            _unitOfWorkMock
                .Setup(x => x.HealthRecords)
                .Returns(_healthRecordRepoMock.Object);

            _service =
                new AppointmentService(
                    _unitOfWorkMock.Object,
                    _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAppointmentsAsync_ReturnsAppointments()
        {
            var appointments = new List<Appointment>
        {
            new Appointment(),
            new Appointment()
        };

            var appointmentDtos = new List<AppointmentDto>
        {
            new AppointmentDto(),
            new AppointmentDto()
        };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock.Setup(x =>
                    x.Map<IEnumerable<AppointmentDto>>(appointments))
                .Returns(appointmentDtos);

            var result =
                await _service.GetAllAppointmentsAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_ReturnsAppointment()
        {
            var appointment = new Appointment();

            var dto = new AppointmentDto();

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _mapperMock.Setup(x =>
                    x.Map<AppointmentDto>(appointment))
                .Returns(dto);

            var result =
                await _service.GetAppointmentByIdAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WhenNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Appointment)null!);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(
                () => _service.GetAppointmentByIdAsync(1));
        }

        [Fact]
        public async Task AddAppointmentAsync_WhenPatientNotFound_ThrowsException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(dto.PatientId))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<PatientNotFoundException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [Fact]
        public async Task AddAppointmentAsync_WhenDoctorNotFound_ThrowsException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(new Patient());

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync((Doctor)null!);

            await Assert.ThrowsAsync<DoctorNotFoundException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [Fact]
        public async Task AddAppointmentAsync_WhenPastDate_ThrowsException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(-1),
                TimeSlot = "10:00 AM"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(new Doctor { IsActive = true });

            await Assert.ThrowsAsync<PastDateException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [Fact]
        public async Task AddAppointmentAsync_WhenDateBeyondSixMonths_ThrowsException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddMonths(7),
                TimeSlot = "10:00 AM"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(new Doctor { IsActive = true });

            await Assert.ThrowsAsync<AdvanceBookingLimitException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [Fact]
        public async Task AddAppointmentAsync_WhenDoctorInactive_ThrowsException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(new Doctor
                {
                    IsActive = false
                });

            await Assert.ThrowsAsync<DoctorUnavailableException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [Fact]
        public async Task AddAppointmentAsync_WhenSlotUnavailable_ThrowsException()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(new Doctor
                {
                    IsActive = true
                });

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.IsSlotAvailableAsync(
                        dto.DoctorId,
                        dto.ScheduledDate,
                        dto.TimeSlot))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<AppointmentConflictException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [Fact]
        public async Task AddAppointmentAsync_ReturnsAppointmentId()
        {
            var dto = new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            var appointment = new Appointment
            {
                AppointmentId = 1
            };

            _unitOfWorkMock.Setup(x =>
                    x.Patients.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _unitOfWorkMock.Setup(x =>
                    x.Doctors.GetByIdAsync(1))
                .ReturnsAsync(new Doctor
                {
                    IsActive = true
                });

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.IsSlotAvailableAsync(
                        dto.DoctorId,
                        dto.ScheduledDate,
                        dto.TimeSlot))
                .ReturnsAsync(true);

            _mapperMock.Setup(x =>
                    x.Map<Appointment>(dto))
                .Returns(appointment);

            var result =
                await _service.AddAppointmentAsync(dto);

            Assert.Equal(1, result);

            Assert.Equal(
                AppointmentStatus.Pending,
                appointment.Status);

            _unitOfWorkMock.Verify(
                x => x.Appointments.AddAsync(
                    It.IsAny<Appointment>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(
                        It.IsAny<int>()))
                .ReturnsAsync((Appointment)null!);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(
                () => _service.UpdateAppointmentAsync(
                    1,
                    new UpdateAppointmentDto()));
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenSlotUnavailable_ThrowsException()
        {
            var appointment = new Appointment();

            var dto = new UpdateAppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.IsSlotAvailableAsync(
                        dto.DoctorId,
                        dto.ScheduledDate,
                        dto.TimeSlot))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<AppointmentConflictException>(
                () => _service.UpdateAppointmentAsync(
                    1,
                    dto));
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenCompleted_ThrowsException()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Completed
            };

            var dto = new UpdateAppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.IsSlotAvailableAsync(
                        dto.DoctorId,
                        dto.ScheduledDate,
                        dto.TimeSlot))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.UpdateAppointmentAsync(
                    1,
                    dto));
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenCancelled_ThrowsException()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Cancelled
            };

            var dto = new UpdateAppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.IsSlotAvailableAsync(
                        dto.DoctorId,
                        dto.ScheduledDate,
                        dto.TimeSlot))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.UpdateAppointmentAsync(
                    1,
                    dto));
        }

        [Fact]
        public async Task UpdateAppointmentAsync_UpdatesSuccessfully()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = AppointmentStatus.Pending
            };

            var dto = new UpdateAppointmentDto
            {
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(2),
                TimeSlot = "11:00 AM"
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.IsSlotAvailableAsync(
                        dto.DoctorId,
                        dto.ScheduledDate,
                        dto.TimeSlot))
                .ReturnsAsync(true);

            await _service.UpdateAppointmentAsync(
                1,
                dto);

            Assert.Equal(dto.DoctorId, appointment.DoctorId);
            Assert.Equal(dto.TimeSlot, appointment.TimeSlot);

            _unitOfWorkMock.Verify(
                x => x.Appointments.UpdateAsync(
                    appointment),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(
                        It.IsAny<int>()))
                .ReturnsAsync((Appointment)null!);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(
                () => _service.DeleteAppointmentAsync(1));
        }

        [Fact]
        public async Task DeleteAppointmentAsync_WhenPending_ThrowsException()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<AppointmentDeletionException>(
                () => _service.DeleteAppointmentAsync(1));
        }

        [Fact]
        public async Task DeleteAppointmentAsync_WhenConfirmed_ThrowsException()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Confirmed
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<AppointmentDeletionException>(
                () => _service.DeleteAppointmentAsync(1));
        }

        [Fact]
        public async Task DeleteAppointmentAsync_DeletesSuccessfully()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Completed
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.DeleteAppointmentAsync(1);

            _unitOfWorkMock.Verify(
                x => x.Appointments.DeleteAsync(1),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(
                        It.IsAny<int>()))
                .ReturnsAsync((Appointment)null!);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(
                () => _service.ConfirmAppointmentAsync(1));
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenStatusNotPending_ThrowsException()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Confirmed
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.ConfirmAppointmentAsync(1));
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_Success()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.ConfirmAppointmentAsync(1);

            Assert.Equal(
                AppointmentStatus.Confirmed,
                appointment.Status);

            _unitOfWorkMock.Verify(
                x => x.Appointments.UpdateAsync(
                    appointment),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(
                        It.IsAny<int>()))
                .ReturnsAsync((Appointment)null!);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(
                () => _service.CancelAppointmentAsync(
                    1,
                    "Cancelled"));
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenStatusInvalid_ThrowsException()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Completed
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.CancelAppointmentAsync(
                    1,
                    "Cancelled"));
        }

        [Fact]
        public async Task CancelAppointmentAsync_FromPending_Success()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.CancelAppointmentAsync(
                1,
                "Patient Request");

            Assert.Equal(
                AppointmentStatus.Cancelled,
                appointment.Status);

            Assert.Equal(
                "Patient Request",
                appointment.CancellationReason);

            _unitOfWorkMock.Verify(
                x => x.Appointments.UpdateAsync(
                    appointment),
                Times.Once);
        }

        [Fact]
        public async Task CancelAppointmentAsync_FromConfirmed_Success()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Confirmed
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.CancelAppointmentAsync(
                1,
                "Doctor Unavailable");

            Assert.Equal(
                AppointmentStatus.Cancelled,
                appointment.Status);

            _unitOfWorkMock.Verify(
                x => x.Appointments.UpdateAsync(
                    appointment),
                Times.Once);
        }

        [Fact]
        public async Task CompleteAppointmentAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(
                        It.IsAny<int>()))
                .ReturnsAsync((Appointment)null!);

            await Assert.ThrowsAsync<AppointmentNotFoundException>(
                () => _service.CompleteAppointmentAsync(1));
        }

        [Fact]
        public async Task CompleteAppointmentAsync_WhenStatusNotConfirmed_ThrowsException()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<InvalidAppointmentStatusException>(
                () => _service.CompleteAppointmentAsync(1));
        }

        [Fact]
        public async Task CompleteAppointmentAsync_Success()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Confirmed
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await _service.CompleteAppointmentAsync(1);

            Assert.Equal(
                AppointmentStatus.Completed,
                appointment.Status);

            _unitOfWorkMock.Verify(
                x => x.Appointments.UpdateAsync(
                    appointment),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetAppointmentsByPatientAsync_ReturnsAppointments()
        {
            var appointments = new List<Appointment>
    {
        new Appointment(),
        new Appointment()
    };

            var appointmentDtos = new List<AppointmentDto>
    {
        new AppointmentDto(),
        new AppointmentDto()
    };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(appointments);

            _mapperMock.Setup(x =>
                    x.Map<IEnumerable<AppointmentDto>>(appointments))
                .Returns(appointmentDtos);

            var result =
                await _service.GetAppointmentsByPatientAsync(1);

            Assert.Equal(2, result.Count());

            _unitOfWorkMock.Verify(
                x => x.Appointments.GetAppointmentsByPatientAsync(1),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<IEnumerable<AppointmentDto>>(appointments),
                Times.Once);
        }

        [Fact]
        public async Task
     GetAppointmentsByDoctorAsync_ReturnsAppointments()
        {
            // Arrange

            var appointments =
                new List<Appointment>
                {
            new Appointment
            {
                AppointmentId = 1,
                Patient = new Patient
                {
                    FullName = "Arjun Kumar"
                },
                Doctor = new Doctor
                {
                    FullName = "Rajesh Kumar"
                }
            },
            new Appointment
            {
                AppointmentId = 2,
                Patient = new Patient
                {
                    FullName = "John"
                },
                Doctor = new Doctor
                {
                    FullName = "Smith"
                }
            }
                };

            var appointmentDtos =
                new List<AppointmentDto>
                {
            new AppointmentDto
            {
                AppointmentId = 1
            },
            new AppointmentDto
            {
                AppointmentId = 2
            }
                };

            _appointmentRepoMock
                .Setup(x =>
                    x.GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(appointments);

            _healthRecordRepoMock
                .Setup(x =>
                    x.GetRecordedAppointmentIdsAsync())
                .ReturnsAsync(
                    new List<int> { 1 });

            _mapperMock
                .Setup(x =>
                    x.Map<List<AppointmentDto>>(
                        appointments))
                .Returns(appointmentDtos);

            // Act

            var result =
                (await _service
                    .GetAppointmentsByDoctorAsync(1))
                .ToList();

            // Assert

            Assert.Equal(2, result.Count);

            Assert.True(
                result.First().HasHealthRecord);

            Assert.False(
                result.Last().HasHealthRecord);

            _appointmentRepoMock.Verify(
                x => x.GetAppointmentsByDoctorAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task
    GetTodayScheduleAsync_ReturnsAppointments()
        {
            // Arrange

            var appointments =
                new List<Appointment>
                {
            new Appointment
            {
                AppointmentId = 1,
                Patient = new Patient
                {
                    FullName = "Arjun Kumar"
                },
                Doctor = new Doctor
                {
                    FullName = "Rajesh Kumar"
                }
            }
                };

            var appointmentDtos =
                new List<AppointmentDto>
                {
            new AppointmentDto
            {
                AppointmentId = 1
            }
                };

            _appointmentRepoMock
                .Setup(x =>
                    x.GetTodayScheduleAsync(1))
                .ReturnsAsync(appointments);

            _healthRecordRepoMock
                .Setup(x =>
                    x.GetRecordedAppointmentIdsAsync())
                .ReturnsAsync(
                    new List<int> { 1 });

            _mapperMock
                .Setup(x =>
                    x.Map<List<AppointmentDto>>(
                        appointments))
                .Returns(appointmentDtos);

            // Act

            var result =
                (await _service
                    .GetTodayScheduleAsync(1))
                .ToList();

            // Assert

            Assert.Single(result);

            Assert.True(
                result[0].HasHealthRecord);

            _appointmentRepoMock.Verify(
                x => x.GetTodayScheduleAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task
    GetWeeklyScheduleAsync_ReturnsAppointments()
        {
            // Arrange

            var appointments =
                new List<Appointment>
                {
            new Appointment
            {
                AppointmentId = 1,
                Patient = new Patient
                {
                    FullName = "Arjun Kumar"
                },
                Doctor = new Doctor
                {
                    FullName = "Rajesh Kumar"
                }
            }
                };

            var appointmentDtos =
                new List<AppointmentDto>
                {
            new AppointmentDto
            {
                AppointmentId = 1
            }
                };

            _appointmentRepoMock
                .Setup(x =>
                    x.GetWeeklyScheduleAsync(1))
                .ReturnsAsync(appointments);

            _healthRecordRepoMock
                .Setup(x =>
                    x.GetRecordedAppointmentIdsAsync())
                .ReturnsAsync(
                    new List<int> { 1 });

            _mapperMock
                .Setup(x =>
                    x.Map<List<AppointmentDto>>(
                        appointments))
                .Returns(appointmentDtos);

            // Act

            var result =
                (await _service
                    .GetWeeklyScheduleAsync(1))
                .ToList();

            // Assert

            Assert.Single(result);

            Assert.True(
                result[0].HasHealthRecord);

            _appointmentRepoMock.Verify(
                x => x.GetWeeklyScheduleAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task GetNextAppointmentByPatientAsync_ReturnsAppointment()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1
            };

            var appointmentDto = new AppointmentDto
            {
                AppointmentId = 1
            };

            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetNextAppointmentByPatientAsync(1))
                .ReturnsAsync(appointment);

            _mapperMock.Setup(x =>
                    x.Map<AppointmentDto>(appointment))
                .Returns(appointmentDto);

            var result =
                await _service.GetNextAppointmentByPatientAsync(1);

            Assert.NotNull(result);

            Assert.Equal(
                1,
                result.AppointmentId);

            _mapperMock.Verify(
                x => x.Map<AppointmentDto>(appointment),
                Times.Once);
        }

        [Fact]
        public async Task GetNextAppointmentByPatientAsync_ReturnsNull()
        {
            _unitOfWorkMock.Setup(x =>
                    x.Appointments.GetNextAppointmentByPatientAsync(1))
                .ReturnsAsync((Appointment)null!);

            var result =
                await _service.GetNextAppointmentByPatientAsync(1);

            Assert.Null(result);
        }



    }

}
