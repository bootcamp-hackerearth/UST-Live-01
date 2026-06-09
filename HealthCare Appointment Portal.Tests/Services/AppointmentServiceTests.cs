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
        private readonly Mock<IAppointmentRepository>
            _appointmentRepositoryMock;

        private readonly Mock<IPatientRepository>
            _patientRepositoryMock;

        private readonly Mock<IDoctorRepository>
            _doctorRepositoryMock;

        private readonly Mock<IHealthRecordRepository>
            _healthRecordRepositoryMock;

        private readonly Mock<IMapper>
            _mapperMock;


        private readonly AppointmentService
            _service;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock =
                new Mock<IAppointmentRepository>();

            _patientRepositoryMock =
                new Mock<IPatientRepository>();

            _doctorRepositoryMock =
                new Mock<IDoctorRepository>();

            _healthRecordRepositoryMock =
                new Mock<IHealthRecordRepository>();

            _mapperMock =
                new Mock<IMapper>();

            _service =
                new AppointmentService(
                    _appointmentRepositoryMock.Object,
                    _patientRepositoryMock.Object,
                    _doctorRepositoryMock.Object,
                    _healthRecordRepositoryMock.Object,
                    null!,
                    _mapperMock.Object);
        }
        [Fact]
        public async Task GetAllAppointmentsAsync_ReturnsAppointments()
        {
            var appointments =
                new List<Appointment>
                {
            new Appointment(),
            new Appointment()
                };

            var appointmentDtos =
                new List<AppointmentDto>
                {
            new AppointmentDto(),
            new AppointmentDto()
                };

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAllAsync())
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<AppointmentDto>>(
                        appointments))
                .Returns(appointmentDtos);

            var result =
                await _service
                    .GetAllAppointmentsAsync();

            Assert.Equal(
                2,
                result.Count());
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_ReturnsAppointment()
        {
            var appointment =
                new Appointment();

            var dto =
                new AppointmentDto();

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(x =>
                    x.Map<AppointmentDto>(
                        appointment))
                .Returns(dto);

            var result =
                await _service
                    .GetAppointmentByIdAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WhenNotFound_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        It.IsAny<int>()))
                .ReturnsAsync((Appointment)null!);

            await Assert.ThrowsAsync<
                AppointmentNotFoundException>(
                () => _service
                    .GetAppointmentByIdAsync(1));
        }

        [Fact]
        public async Task AddAppointmentAsync_WhenPatientNotFound_ThrowsException()
        {
            var dto =
                new CreateAppointmentDto
                {
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate =
                        DateTime.Today.AddDays(1),
                    TimeSlot = "10:00 AM"
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        dto.PatientId))
                .ReturnsAsync((Patient)null!);

            await Assert.ThrowsAsync<
                PatientNotFoundException>(
                () => _service
                    .AddAppointmentAsync(dto));
        }

        [Fact]
        public async Task AddAppointmentAsync_WhenDoctorNotFound_ThrowsException()
        {
            var dto =
                new CreateAppointmentDto
                {
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate =
                        DateTime.Today.AddDays(1),
                    TimeSlot = "10:00 AM"
                };

            _patientRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        dto.PatientId))
                .ReturnsAsync(
                    new Patient());

            _doctorRepositoryMock
                .Setup(x =>
                    x.GetByIdAsync(
                        dto.DoctorId))
                .ReturnsAsync((Doctor)null!);

            await Assert.ThrowsAsync<
                DoctorNotFoundException>(
                () => _service
                    .AddAppointmentAsync(dto));
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

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Doctor
                {
                    IsActive = true
                });

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

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Doctor
                {
                    IsActive = true
                });

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

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
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

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Doctor
                {
                    IsActive = true
                });

            _appointmentRepositoryMock
                .Setup(x => x.IsSlotAvailableAsync(
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

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Patient());

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Doctor
                {
                    IsActive = true
                });

            _appointmentRepositoryMock
                .Setup(x => x.IsSlotAvailableAsync(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(true);

            _mapperMock
                .Setup(x => x.Map<Appointment>(dto))
                .Returns(appointment);

            await Assert.ThrowsAsync<NullReferenceException>(
                () => _service.AddAppointmentAsync(dto));
        }

        [Fact]
        public async Task UpdateAppointmentAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(
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

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(x => x.IsSlotAvailableAsync(
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

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(x => x.IsSlotAvailableAsync(
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

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(x => x.IsSlotAvailableAsync(
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

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock
                .Setup(x => x.IsSlotAvailableAsync(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<NullReferenceException>(
                () => _service.UpdateAppointmentAsync(
                    1,
                    dto));
        }

        [Fact]
        public async Task DeleteAppointmentAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(
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

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
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

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
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

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<NullReferenceException>(
                () => _service.DeleteAppointmentAsync(1));
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(
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

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
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

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<NullReferenceException>(
                () => _service.ConfirmAppointmentAsync(1));
        }

        [Fact]
        public async Task CancelAppointmentAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(
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

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
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

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<NullReferenceException>(
                () => _service.CancelAppointmentAsync(
                    1,
                    "Patient Request"));
        }

        [Fact]
        public async Task CancelAppointmentAsync_FromConfirmed_Success()
        {
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Confirmed
            };

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<NullReferenceException>(
                () => _service.CancelAppointmentAsync(
                    1,
                    "Doctor Unavailable"));
        }

        [Fact]
        public async Task CompleteAppointmentAsync_WhenAppointmentNotFound_ThrowsException()
        {
            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(
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

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
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

            _appointmentRepositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(appointment);

            await Assert.ThrowsAsync<NullReferenceException>(
                () => _service.CompleteAppointmentAsync(1));
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

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByPatientAsync(1))
                .ReturnsAsync(appointments);

            _mapperMock
                .Setup(x =>
                    x.Map<IEnumerable<AppointmentDto>>(
                        appointments))
                .Returns(appointmentDtos);

            var result =
                await _service
                    .GetAppointmentsByPatientAsync(1);

            Assert.Equal(
                2,
                result.Count());

            _appointmentRepositoryMock.Verify(
                x => x.GetAppointmentsByPatientAsync(1),
                Times.Once);

            _mapperMock.Verify(
                x => x.Map<IEnumerable<AppointmentDto>>(
                    appointments),
                Times.Once);
        }

        [Fact]
        public async Task GetAppointmentsByDoctorAsync_ReturnsAppointments()
        {
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

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetAppointmentsByDoctorAsync(1))
                .ReturnsAsync(appointments);

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetRecordedAppointmentIdsAsync())
                .ReturnsAsync(
                    new List<int> { 1 });

            _mapperMock
                .Setup(x =>
                    x.Map<List<AppointmentDto>>(
                        appointments))
                .Returns(appointmentDtos);

            var result =
                (await _service
                    .GetAppointmentsByDoctorAsync(1))
                .ToList();

            Assert.Equal(
                2,
                result.Count);

            Assert.True(
                result.First().HasHealthRecord);

            Assert.False(
                result.Last().HasHealthRecord);

            _appointmentRepositoryMock.Verify(
                x => x.GetAppointmentsByDoctorAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task GetTodayScheduleAsync_ReturnsAppointments()
        {
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

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetTodayScheduleAsync(1))
                .ReturnsAsync(appointments);

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetRecordedAppointmentIdsAsync())
                .ReturnsAsync(
                    new List<int> { 1 });

            _mapperMock
                .Setup(x =>
                    x.Map<List<AppointmentDto>>(
                        appointments))
                .Returns(appointmentDtos);

            var result =
                (await _service
                    .GetTodayScheduleAsync(1))
                .ToList();

            Assert.Single(result);

            Assert.True(
                result[0].HasHealthRecord);

            _appointmentRepositoryMock.Verify(
                x => x.GetTodayScheduleAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task GetWeeklyScheduleAsync_ReturnsAppointments()
        {
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

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetWeeklyScheduleAsync(1))
                .ReturnsAsync(appointments);

            _healthRecordRepositoryMock
                .Setup(x =>
                    x.GetRecordedAppointmentIdsAsync())
                .ReturnsAsync(
                    new List<int> { 1 });

            _mapperMock
                .Setup(x =>
                    x.Map<List<AppointmentDto>>(
                        appointments))
                .Returns(appointmentDtos);

            var result =
                (await _service
                    .GetWeeklyScheduleAsync(1))
                .ToList();

            Assert.Single(result);

            Assert.True(
                result[0].HasHealthRecord);

            _appointmentRepositoryMock.Verify(
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

            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetNextAppointmentByPatientAsync(1))
                .ReturnsAsync(appointment);

            _mapperMock
                .Setup(x =>
                    x.Map<AppointmentDto>(
                        appointment))
                .Returns(appointmentDto);

            var result =
                await _service
                    .GetNextAppointmentByPatientAsync(1);

            Assert.NotNull(result);

            Assert.Equal(
                1,
                result.AppointmentId);

            _mapperMock.Verify(
                x => x.Map<AppointmentDto>(
                    appointment),
                Times.Once);
        }

        [Fact]
        public async Task GetNextAppointmentByPatientAsync_ReturnsNull()
        {
            _appointmentRepositoryMock
                .Setup(x =>
                    x.GetNextAppointmentByPatientAsync(1))
                .ReturnsAsync((Appointment)null!);

            var result =
                await _service
                    .GetNextAppointmentByPatientAsync(1);

            Assert.Null(result);
        }
    }

}
