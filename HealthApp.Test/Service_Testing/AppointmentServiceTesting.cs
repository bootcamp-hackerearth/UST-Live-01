using AutoMapper;
using FluentAssertions;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Impl;
using HealthApp.Shared.Dto;
using Moq;


namespace HealthApp.Test.Service_Testing
{
    public class AppointmentServiceTesting
    {
        private readonly Mock<IAppointmentRepository> _repo;
        private readonly Mock<IPatientRepository> _patientRepo;
        private readonly Mock<IDoctorRepository> _doctorRepo;
        private readonly Mock<IMapper> _mapper;

        private readonly AppointmentService _service;

        public AppointmentServiceTesting()
        {
            _repo = new Mock<IAppointmentRepository>();
            _patientRepo = new Mock<IPatientRepository>();
            _doctorRepo = new Mock<IDoctorRepository>();
            _mapper = new Mock<IMapper>();

            _service = new AppointmentService(
                _repo.Object,
                _patientRepo.Object,
                _doctorRepo.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task Add_ShouldCreateAppointment_WhenValid()
        {
            var dto = new AppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Now,
                TimeSlot = "10:00 AM"
            };

            _patientRepo.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync(new Patient { PatientId = 5 });

            _repo.Setup(x => x.IsSlotBookedAsync(dto.DoctorId, dto.ScheduledDate, dto.TimeSlot))
                .ReturnsAsync(false);

            var result = await _service.Add(dto, "user1");

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Add_ShouldThrow_WhenSlotBooked()
        {
            var dto = new AppointmentDto
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Now,
                TimeSlot = "10:00 AM"
            };

            _patientRepo.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync(new Patient { PatientId = 5 });

            _repo.Setup(x => x.IsSlotBookedAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<AppointmentRuleException>(() =>
                _service.Add(dto, "user1"));
        }

        [Fact]
        public async Task Add_ShouldThrow_WhenPatientNotFound()
        {
            var dto = new AppointmentDto();

            _patientRepo.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync((Patient)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.Add(dto, "user1"));
        }

        [Fact]
        public async Task Confirm_ShouldUpdateStatus()
        {
            var appointment = new Appointment();

            _repo.Setup(x => x.UpdateStatusAsync(1, "Confirmed"))
                .ReturnsAsync(appointment);

            _mapper.Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(new AppointmentDto());

            var result = await _service.ConfirmAppointment(1);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Confirm_ShouldThrow_WhenNotFound()
        {
            _repo.Setup(x => x.UpdateStatusAsync(1, "Confirmed"))
                .ReturnsAsync((Appointment)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.ConfirmAppointment(1));
        }

        [Fact]
        public async Task Complete_ShouldUpdateStatus()
        {
            var appointment = new Appointment();

            _repo.Setup(x => x.UpdateStatusAsync(1, "Completed"))
                .ReturnsAsync(appointment);

            _mapper.Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(new AppointmentDto());

            var result = await _service.CompleteAppointment(1);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Cancel_ShouldReturnAppointment()
        {
            var appointment = new Appointment();

            _repo.Setup(x => x.CancelAppointmentAsync(1, "Reason"))
                .ReturnsAsync(appointment);

            _mapper.Setup(x => x.Map<AppointmentDto>(appointment))
                .Returns(new AppointmentDto());

            var result = await _service.CancelAppointment(1, "Reason");

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Cancel_ShouldThrow_WhenReasonEmpty()
        {
            await Assert.ThrowsAsync<AppointmentRuleException>(() =>
                _service.CancelAppointment(1, ""));
        }

        [Fact]
        public async Task GetAppointmentsByDoctor_ShouldReturnList()
        {
            _doctorRepo.Setup(x => x.GetByIdentityUserIdAsync("doc1"))
                .ReturnsAsync(new Doctor { DoctorId = 1 });

            _repo.Setup(x => x.GetByDoctorIdAsync(1))
                .ReturnsAsync(new List<Appointment>());

            _mapper.Setup(x => x.Map<List<AppointmentDto>>(It.IsAny<object>()))
                .Returns(new List<AppointmentDto>());

            var result = await _service.GetAppointmentsByDoctorAsync("doc1");

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAppointmentsByUser_ShouldReturnList()
        {
            _patientRepo.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync(new Patient { PatientId = 1 });

            _repo.Setup(x => x.GetByPatientIdAsync(1))
                .ReturnsAsync(new List<Appointment>());

            _mapper.Setup(x => x.Map<List<AppointmentDto>>(It.IsAny<object>()))
                .Returns(new List<AppointmentDto>());

            var result = await _service.GetAppointmentsByUserAsync("user1");

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task CheckAvailability_ShouldReturnSlots()
        {
            _repo.Setup(x => x.GetBookedSlotsAsync(1, It.IsAny<DateTime>()))
                .ReturnsAsync(new List<string> { "10:00 AM" });

            var result = await _service.CheckDoctorAvailability(1, DateTime.Now);

            result.Should().Contain("10:00 AM");
        }

        [Fact]
        public async Task IsSlotBooked_ShouldReturnTrue()
        {
            _repo.Setup(x => x.IsSlotBookedAsync(1, It.IsAny<DateTime>(), "10:00 AM"))
                .ReturnsAsync(true);

            var result = await _service.IsSlotBooked(1, DateTime.Now, "10:00 AM");

            result.Should().BeTrue();
        }
    }
}
