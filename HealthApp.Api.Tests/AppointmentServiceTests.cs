using AutoMapper;
using HealthApp.Api.Constants;
using HealthApp.Api.Dtos;
using HealthApp.Api.Enums;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Impl;
using Moq;
using Xunit;

namespace HealthApp.Api.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepo;
        private readonly Mock<IPatientRepository> _patientRepo;
        private readonly Mock<IDoctorRepository> _doctorRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _appointmentRepo = new Mock<IAppointmentRepository>();
            _patientRepo = new Mock<IPatientRepository>();
            _doctorRepo = new Mock<IDoctorRepository>();
            _mapper = new Mock<IMapper>();

            _service = new AppointmentService(
                _appointmentRepo.Object,
                _patientRepo.Object,
                _doctorRepo.Object,
                _mapper.Object
            );
        }

        private static AppointmentCreateDto ValidDto() => new()
        {
            PatientId = 1,
            DoctorId = 1,
            ScheduledDate = DateTime.Today.AddDays(1),
            TimeSlot = "10AM"
        };

        private static Patient Patient() => new()
        {
            PatientId = 1,
            FullName = "John"
        };

        private static Doctor Doctor(bool active = true) => new()
        {
            DoctorId = 1,
            FullName = "Dr A",
            IsActive = active,
            Specialisation = SpecialisationType.Cardiologist
        };

        [Fact]
        public async Task GetAppointmentById_InvalidId_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.GetAppointmentByIdAsync(0));
        }

        [Fact]
        public async Task GetAppointmentById_NotFound_ShouldThrow()
        {
            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.GetAppointmentByIdAsync(1));
        }

        [Fact]
        public async Task GetAppointmentById_Valid_ShouldReturnDto()
        {
            var appt = new Appointment { AppointmentId = 1, PatientId = 1, DoctorId = 1 };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(appt);
            _patientRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(Patient());
            _doctorRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(Doctor());

            _mapper.Setup(x => x.Map<AppointmentDto>(appt))
                .Returns(new AppointmentDto { AppointmentId = 1 });

            var result = await _service.GetAppointmentByIdAsync(1);

            Assert.Equal(1, result.AppointmentId);
        }

        [Fact]
        public async Task BookAppointment_NullDto_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.BookAppointmentAsync(null!));
        }

        [Fact]
        public async Task BookAppointment_PastDate_ShouldThrow()
        {
            var dto = ValidDto();
            dto.ScheduledDate = DateTime.Today.AddDays(-1);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(
                () => _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task BookAppointment_PatientNotFound_ShouldThrow()
        {
            var dto = ValidDto();

            _patientRepo.Setup(x => x.GetByIdAsync(1, default))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task BookAppointment_DoctorInactive_ShouldThrow()
        {
            var dto = ValidDto();

            _patientRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(Patient());
            _doctorRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(Doctor(false));

            await Assert.ThrowsAsync<BusinessRuleViolationException>(
                () => _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task BookAppointment_DoctorSlotBooked_ShouldThrow()
        {
            var dto = ValidDto();

            _patientRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(Patient());
            _doctorRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(Doctor());

            _appointmentRepo.Setup(x => x.IsDoctorSlotBookedAsync(
                It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<string>(), default))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(
                () => _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task BookAppointment_Valid_ShouldReturnDto()
        {
            var dto = ValidDto();

            var entity = new Appointment { AppointmentId = 100 };

            _patientRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(Patient());
            _doctorRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(Doctor());

            _appointmentRepo.Setup(x =>
                x.HasAppointmentWithDoctorOnSameDayAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateOnly>(), default))
                .ReturnsAsync(false);

            _appointmentRepo.Setup(x =>
                x.HasPatientSlotConflictAsync(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<string>(), default))
                .ReturnsAsync(false);

            _appointmentRepo.Setup(x =>
                x.IsDoctorSlotBookedAsync(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<string>(), default))
                .ReturnsAsync(false);

            _mapper.Setup(x => x.Map<Appointment>(dto)).Returns(new Appointment());

            _appointmentRepo.Setup(x => x.Add(It.IsAny<Appointment>(), default))
                .ReturnsAsync(entity);

            _mapper.Setup(x => x.Map<AppointmentDto>(entity))
                .Returns(new AppointmentDto { AppointmentId = 100 });

            var result = await _service.BookAppointmentAsync(dto);

            Assert.Equal(100, result.AppointmentId);
        }

        [Fact]
        public async Task UpdateStatus_InvalidId_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.UpdateAppointmentStatusAsync(0, AppointmentStatus.Pending));
        }

        [Fact]
        public async Task UpdateStatus_Completed_ShouldThrow()
        {
            var appt = new Appointment { AppointmentId = 1, Status = AppointmentStatus.Completed };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(appt);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(
                () => _service.UpdateAppointmentStatusAsync(1, AppointmentStatus.Cancelled));
        }

        [Fact]
        public async Task UpdateStatus_CancelWithoutReason_ShouldThrow()
        {
            var appt = new Appointment { AppointmentId = 1, Status = AppointmentStatus.Pending };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(appt);

            await Assert.ThrowsAsync<InvalidRequestException>(
                () => _service.UpdateAppointmentStatusAsync(1, AppointmentStatus.Cancelled, null));
        }

        [Fact]
        public async Task UpdateStatus_Valid_ShouldUpdate()
        {
            var appt = new Appointment { AppointmentId = 1, Status = AppointmentStatus.Pending };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(appt);

            await _service.UpdateAppointmentStatusAsync(1, AppointmentStatus.Confirmed);

            _appointmentRepo.Verify(x => x.Update(1, It.IsAny<Appointment>(), default), Times.Once);
        }

        [Fact]
        public async Task Delete_NotCancelled_ShouldThrow()
        {
            var appt = new Appointment { AppointmentId = 1, Status = AppointmentStatus.Pending };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(appt);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(
                () => _service.DeleteAppointmentAsync(1));
        }

        [Fact]
        public async Task Delete_Valid_ShouldDelete()
        {
            var appt = new Appointment { AppointmentId = 1, Status = AppointmentStatus.Cancelled };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(appt);
            _appointmentRepo.Setup(x => x.DeleteAsync(1)).ReturnsAsync(true);

            await _service.DeleteAppointmentAsync(1);

            _appointmentRepo.Verify(x => x.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetAvailableSlots_DoctorInactive_ShouldThrow()
        {
            _doctorRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(Doctor(false));

            await Assert.ThrowsAsync<BusinessRuleViolationException>(
                () => _service.GetAvailableSlotsAsync(1, DateOnly.FromDateTime(DateTime.Today.AddDays(1))));
        }

        [Fact]
        public async Task GetAvailableSlots_ShouldReturnSlots()
        {
            _doctorRepo.Setup(x => x.GetByIdAsync(1, default)).ReturnsAsync(Doctor());

            _appointmentRepo.Setup(x => x.IsDoctorSlotBookedAsync(
                It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<string>(), default))
                .ReturnsAsync(false);

            var result = await _service.GetAvailableSlotsAsync(
                1,
                DateOnly.FromDateTime(DateTime.Today.AddDays(1)));

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetAppointments_ShouldReturnAppointments()
        {
            var list = new List<Appointment>
    {
        new Appointment { PatientId = 1, DoctorId = 1 }
    };

            _appointmentRepo.Setup(x => x.GetAppointmentsAsync(null, null, false))
                .ReturnsAsync(list);

            _patientRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(Patient());

            _doctorRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(Doctor());

            _mapper.Setup(x => x.Map<IEnumerable<AppointmentDto>>(list))
                .Returns(new List<AppointmentDto> { new AppointmentDto() });

            var result = await _service.GetAppointmentsAsync();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task BookAppointment_InvalidPatientId_ShouldThrow()
        {
            var dto = ValidDto();
            dto.PatientId = 0;

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task BookAppointment_NoTimeSlot_ShouldThrow()
        {
            var dto = ValidDto();
            dto.TimeSlot = "";

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task BookAppointment_SameDoctorSameDay_ShouldThrow()
        {
            var dto = ValidDto();

            _patientRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(Patient());
            _doctorRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(Doctor());

            _appointmentRepo.Setup(x =>
                x.HasAppointmentWithDoctorOnSameDayAsync(1, 1, It.IsAny<DateOnly>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
                _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task BookAppointment_PatientSlotConflict_ShouldThrow()
        {
            var dto = ValidDto();

            _patientRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(Patient());
            _doctorRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(Doctor());

            _appointmentRepo.Setup(x => x.HasAppointmentWithDoctorOnSameDayAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateOnly>()))
                .ReturnsAsync(false);

            _appointmentRepo.Setup(x => x.HasPatientSlotConflictAsync(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
                _service.BookAppointmentAsync(dto));
        }

        [Fact]
        public async Task UpdateStatus_NotFound_ShouldThrow()
        {
            _appointmentRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Appointment?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.UpdateAppointmentStatusAsync(1, AppointmentStatus.Pending));
        }

        [Fact]
        public async Task GetAvailableSlots_InvalidDoctor_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.GetAvailableSlotsAsync(0, DateOnly.FromDateTime(DateTime.Today)));
        }

        [Fact]
        public async Task GetAvailableSlots_DoctorNotFound_ShouldThrow()
        {
            _doctorRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.GetAvailableSlotsAsync(1, DateOnly.FromDateTime(DateTime.Today)));
        }

        [Fact]
        public async Task Delete_InvalidId_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _service.DeleteAppointmentAsync(0));
        }

        [Fact]
        public async Task Delete_Failure_ShouldThrow()
        {
            var appt = new Appointment { AppointmentId = 1, Status = AppointmentStatus.Cancelled };

            _appointmentRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(appt);
            _appointmentRepo.Setup(x => x.DeleteAsync(1)).ReturnsAsync(false);

            await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
                _service.DeleteAppointmentAsync(1));
        }
    }
}