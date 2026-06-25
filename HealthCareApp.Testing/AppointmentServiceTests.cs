using AutoMapper;
using FluentAssertions;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services.Impl;
using HealthCareApp.Shared.Constants;
using HealthCareApp.Shared.Dtos.Appointments;
using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Enums;
using Moq;

namespace HealthCareApp.Testing.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> appointmentRepositoryMock;

        private readonly Mock<IPatientRepository> patientRepositoryMock;

        private readonly Mock<IDoctorRepository> doctorRepositoryMock;

        private readonly Mock<IHealthRecordRepository> healthRecordRepositoryMock;

        private readonly Mock<IMapper> mapperMock;

        private readonly AppointmentService appointmentService;

        public AppointmentServiceTests()
        {
            appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            patientRepositoryMock = new Mock<IPatientRepository>();

            doctorRepositoryMock = new Mock<IDoctorRepository>();

            healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();

            mapperMock = new Mock<IMapper>();

            SetupMapper();

            appointmentService = new AppointmentService(
                appointmentRepositoryMock.Object,
                patientRepositoryMock.Object,
                doctorRepositoryMock.Object,
                healthRecordRepositoryMock.Object,
                mapperMock.Object);
        }

        [Fact]
        public async Task GetDailyStatusSummaryAsync_ShouldReturnCorrectCounts()
        {
            var appointments = GetAppointments();

            appointmentRepositoryMock
                .Setup(repository => repository.GetAppointmentsByDateAsync(
                    DateTime.Today,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            var result = await appointmentService.GetDailyStatusSummaryAsync(DateTime.Today);

            result.Date.Should().Be(DateTime.Today.ToString("yyyy-MM-dd"));

            result.Total.Should().Be(5);

            result.Pending.Should().Be(1);

            result.Confirmed.Should().Be(2);

            result.Completed.Should().Be(1);

            result.Cancelled.Should().Be(1);
        }

        [Fact]
        public async Task GetAppointmentFilterOptionsAsync_ShouldReturnUniquePatientsAndDoctors()
        {
            var appointments = GetAppointments();

            appointmentRepositoryMock
                .Setup(repository => repository.GetAppointmentsForFilterOptionsAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            var result = await appointmentService.GetAppointmentFilterOptionsAsync();

            result.Patients.Should().HaveCount(2);

            result.Doctors.Should().HaveCount(2);

            result.Patients.Should().Contain(patient => patient.Id == 1);

            result.Doctors.Should().Contain(doctor => doctor.Id == 1);
        }

        [Fact]
        public async Task GetAllAppointmentsAsync_ShouldReturnMappedAppointments()
        {
            var appointments = GetAppointments();

            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            var result = await appointmentService.GetAllAppointmentsAsync();

            result.Should().HaveCount(5);

            result[0].AppointmentId.Should().Be(1);
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_WhenQueryIsNull_ShouldUseDefaultPagination()
        {
            var appointments = GetAppointments();

            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            var result = await appointmentService.GetAllAppointmentsPagedAsync(null!);

            result.PageNumber.Should().Be(1);

            result.PageSize.Should().Be(10);

            result.TotalRecords.Should().Be(5);

            result.TotalPages.Should().Be(1);

            result.Items.Should().HaveCount(5);
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_WhenPageValuesAreInvalid_ShouldNormalizeValues()
        {
            var appointments = GetAppointments();

            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            var query = new AppointmentPaginationQueryDto
            {
                PageNumber = 0,

                PageSize = 0
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.PageNumber.Should().Be(1);

            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_WhenPageSizeGreaterThan100_ShouldCapPageSize()
        {
            var appointments = GetAppointments();

            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            var query = new AppointmentPaginationQueryDto
            {
                PageNumber = 1,

                PageSize = 500
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.PageSize.Should().Be(100);
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_ShouldFilterBySearchTerm()
        {
            var appointments = GetAppointments();

            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            var query = new AppointmentPaginationQueryDto
            {
                SearchTerm = "Rishi"
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.TotalRecords.Should().BeGreaterThan(0);

            result.Items.Should().OnlyContain(appointment =>
                appointment.PatientName != null &&
                appointment.PatientName.Contains("Rishi"));
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_ShouldFilterByPatientId()
        {
            var appointments = GetAppointments();

            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            var query = new AppointmentPaginationQueryDto
            {
                PatientId = 1
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.Items.Should().OnlyContain(appointment =>
                appointment.PatientId == 1);
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_ShouldFilterByDoctorId()
        {
            var appointments = GetAppointments();

            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            var query = new AppointmentPaginationQueryDto
            {
                DoctorId = 1
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.Items.Should().OnlyContain(appointment =>
                appointment.DoctorId == 1);
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_ShouldFilterByStatus()
        {
            var appointments = GetAppointments();

            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            var query = new AppointmentPaginationQueryDto
            {
                Status = AppointmentStatus.Cancelled
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.TotalRecords.Should().Be(1);

            result.Items.Should().OnlyContain(appointment =>
                appointment.Status == AppointmentStatus.Cancelled);
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_ShouldFilterByScheduledDate()
        {
            var appointments = GetAppointments();

            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            var query = new AppointmentPaginationQueryDto
            {
                ScheduledDate = DateTime.Today
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.Items.Should().OnlyContain(appointment =>
                DateTime.Parse(appointment.ScheduledDate).Date == DateTime.Today);
        }

        [Fact]
        public async Task GetAllAppointmentsPagedAsync_WhenUpcomingOnlyIsTrue_ShouldReturnUpcomingActiveAppointments()
        {
            var appointments = GetAppointments();

            appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            var query = new AppointmentPaginationQueryDto
            {
                UpcomingOnly = true
            };

            var result = await appointmentService.GetAllAppointmentsPagedAsync(query);

            result.Items.Should().OnlyContain(appointment =>
                DateTime.Parse(appointment.ScheduledDate).Date >= DateTime.Today &&
                appointment.Status != AppointmentStatus.Cancelled &&
                appointment.Status != AppointmentStatus.Completed);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WhenIdIsInvalid_ShouldThrowAppointmentRuleException()
        {
            Func<Task> action = async () =>
                await appointmentService.GetAppointmentByIdAsync(0);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Please provide a valid appointment reference.");
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WhenAppointmentDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Appointment?)null);

            Func<Task> action = async () =>
                await appointmentService.GetAppointmentByIdAsync(99);

            await action.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WhenAppointmentExists_ShouldReturnMappedAppointment()
        {
            var appointment = GetAppointments().First();

            appointmentRepositoryMock
                .Setup(repository => repository.GetByIdAsync(appointment.AppointmentId))
                .ReturnsAsync(appointment);

            var result = await appointmentService.GetAppointmentByIdAsync(
                appointment.AppointmentId);

            result.AppointmentId.Should().Be(appointment.AppointmentId);
        }

        [Fact]
        public async Task GetAppointmentsByPatientIdAsync_WhenPatientExists_ShouldReturnAppointments()
        {
            var patient = GetPatient();

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(patient.PatientId))
                .ReturnsAsync(patient);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByPatientIdAsync(
                    patient.PatientId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.PatientId == patient.PatientId)
                    .ToList());

            var result = await appointmentService.GetAppointmentsByPatientIdAsync(
                patient.PatientId);

            result.Should().OnlyContain(appointment =>
                appointment.PatientId == patient.PatientId);
        }

        [Fact]
        public async Task GetAppointmentsByDoctorIdAsync_WhenDoctorExists_ShouldReturnAppointments()
        {
            var doctor = GetDoctor();

            doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(doctor.DoctorId))
                .ReturnsAsync(doctor);

            appointmentRepositoryMock
                .Setup(repository => repository.GetByDoctorIdAsync(
                    doctor.DoctorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.DoctorId == doctor.DoctorId)
                    .ToList());

            var result = await appointmentService.GetAppointmentsByDoctorIdAsync(
                doctor.DoctorId);

            result.Should().OnlyContain(appointment =>
                appointment.DoctorId == doctor.DoctorId);
        }

        [Fact]
        public async Task GetAppointmentsByStatusAsync_ShouldReturnAppointmentsByStatus()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetByStatusAsync(
                    AppointmentStatus.Pending,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.Status == AppointmentStatus.Pending)
                    .ToList());

            var result = await appointmentService.GetAppointmentsByStatusAsync(
                AppointmentStatus.Pending);

            result.Should().OnlyContain(appointment =>
                appointment.Status == AppointmentStatus.Pending);
        }

        [Fact]
        public async Task GetUpcomingAppointmentsAsync_ShouldReturnUpcomingAppointments()
        {
            appointmentRepositoryMock
                .Setup(repository => repository.GetUpcomingAppointmentsAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetAppointments()
                    .Where(appointment => appointment.ScheduledDate.Date >= DateTime.Today)
                    .ToList());

            var result = await appointmentService.GetUpcomingAppointmentsAsync();

            result.Should().OnlyContain(appointment =>
                DateTime.Parse(appointment.ScheduledDate).Date >= DateTime.Today);
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenDtoIsNull_ShouldThrowAppointmentRuleException()
        {
            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(null!);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Appointment details are required.");
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenDoctorIsInactive_ShouldThrowAppointmentRuleException()
        {
            var dto = GetValidBookAppointmentDto();

            var doctor = GetDoctor();

            doctor.IsActive = false;

            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(doctor);

            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Doctor is inactive. Appointment cannot be booked.");
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenAppointmentDateIsPast_ShouldThrowAppointmentRuleException()
        {
            var dto = GetValidBookAppointmentDto();

            dto.ScheduledDate = DateTime.Today.AddDays(-1);

            SetupPatientAndDoctor(dto);

            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Appointment date cannot be in the past.");
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenTimeSlotIsEmpty_ShouldThrowAppointmentRuleException()
        {
            var dto = GetValidBookAppointmentDto();

            dto.TimeSlot = "";

            SetupPatientAndDoctor(dto);

            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Time slot is required.");
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenTimeSlotIsInvalid_ShouldThrowAppointmentRuleException()
        {
            var dto = GetValidBookAppointmentDto();

            dto.TimeSlot = "Invalid Slot";

            SetupPatientAndDoctor(dto);

            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<AppointmentRuleException>()
                .WithMessage("Invalid time slot selected.");
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenSlotAlreadyBooked_ShouldThrowConflictException()
        {
            var dto = GetValidBookAppointmentDto();

            SetupPatientAndDoctor(dto);

            appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("This time slot is already booked for the selected doctor.");
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenPatientHasSameSlot_ShouldThrowConflictException()
        {
            var dto = GetValidBookAppointmentDto();

            SetupPatientAndDoctor(dto);

            appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            appointmentRepositoryMock
                .Setup(repository => repository.PatientHasActiveAppointmentOnDateAndSlotAsync(
                    dto.PatientId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Patient already has an active appointment in this time slot.");
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenPatientHasAppointmentWithDoctor_ShouldThrowConflictException()
        {
            var dto = GetValidBookAppointmentDto();

            SetupPatientAndDoctor(dto);

            appointmentRepositoryMock
                .Setup(repository => repository.IsSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            appointmentRepositoryMock
                .Setup(repository => repository.PatientHasActiveAppointmentOnDateAndSlotAsync(
                    dto.PatientId,
                    dto.ScheduledDate.Date,
                    dto.TimeSlot,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            appointmentRepositoryMock
                .Setup(repository => repository.PatientHasActiveAppointmentWithDoctorOnDateAsync(
                    dto.PatientId,
                    dto.DoctorId,
                    dto.ScheduledDate.Date,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            Func<Task> action = async () =>
                await appointmentService.BookAppointmentAsync(dto);

            await action.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Patient already has an active appointment with this doctor on the selected date.");
        }

        [Fact]
        public async Task BookAppointmentAsync_WhenValid_ShouldCreatePendingAppointment()
        {
            var dto = GetValidBookAppointmentDto();

            SetupPatientAndDoctor(dto);

            appointmentRepositoryMock
                .Setup(repository => repository.CreateAsync(
                    It.IsAny<Appointment>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Appointment appointment, CancellationToken cancellationToken) =>
                {
                    appointment.AppointmentId = 100;

                    return appointment;
                });

            var result = await appointmentService.BookAppointmentAsync(dto);

            result.AppointmentId.Should().Be(100);

            result.Status.Should().Be(AppointmentStatus.Pending);

            appointmentRepositoryMock.Verify(
                repository => repository.CreateAsync(
                    It.Is<Appointment>(appointment =>
                        appointment.PatientId == dto.PatientId &&
                        appointment.DoctorId == dto.DoctorId &&
                        appointment.ScheduledDate == dto.ScheduledDate.Date &&
                        appointment.Status == AppointmentStatus.Pending &&
                        appointment.CancellationReason == null &&
                        appointment.CreatedDate != default),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private void SetupPatientAndDoctor(BookAppointmentDto dto)
        {
            patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.PatientId))
                .ReturnsAsync(GetPatient());

            doctorRepositoryMock
                .Setup(repository => repository.GetByIdAsync(dto.DoctorId))
                .ReturnsAsync(GetDoctor());
        }

        private void SetupMapper()
        {
            mapperMock
                .Setup(mapper => mapper.Map<List<AppointmentDto>>(It.IsAny<object>()))
                .Returns((object source) =>
                {
                    var appointments = ((IEnumerable<Appointment>)source).ToList();

                    return appointments.Select(MapToAppointmentDto).ToList();
                });

            mapperMock
                .Setup(mapper => mapper.Map<AppointmentDto>(It.IsAny<object>()))
                .Returns((object source) =>
                    MapToAppointmentDto((Appointment)source));

            mapperMock
                .Setup(mapper => mapper.Map<Appointment>(It.IsAny<object>()))
                .Returns((object source) =>
                {
                    return source switch
                    {
                        BookAppointmentDto bookDto => new Appointment
                        {
                            PatientId = bookDto.PatientId,
                            DoctorId = bookDto.DoctorId,
                            ScheduledDate = bookDto.ScheduledDate,
                            TimeSlot = bookDto.TimeSlot,
                            Status = AppointmentStatus.Pending
                        },

                        _ => new Appointment
                        {
                            PatientId = 1,
                            DoctorId = 1,
                            ScheduledDate = DateTime.Today,
                            TimeSlot = TimeSlots.Slots.First(),
                            Status = AppointmentStatus.Pending
                        }
                    };
                });

            mapperMock
                .Setup(mapper => mapper.Map(
                    It.IsAny<UpdateAppointmentDto>(),
                    It.IsAny<Appointment>()))
                .Callback<UpdateAppointmentDto, Appointment>((dto, appointment) =>
                {
                    appointment.PatientId = dto.PatientId;
                    appointment.DoctorId = dto.DoctorId;
                    appointment.ScheduledDate = dto.ScheduledDate;
                    appointment.TimeSlot = dto.TimeSlot;
                })
                .Returns((UpdateAppointmentDto dto, Appointment appointment) => appointment);
        }

        private static AppointmentDto MapToAppointmentDto(Appointment appointment)
        {
            return new AppointmentDto
            {
                AppointmentId = appointment.AppointmentId,

                PatientId = appointment.PatientId,

                PatientName = appointment.Patient?.PatientName,

                DoctorId = appointment.DoctorId,

                DoctorName = appointment.Doctor?.DoctorName,

                ScheduledDate = appointment.ScheduledDate.ToString("yyyy-MM-dd"),

                TimeSlot = appointment.TimeSlot,

                Status = appointment.Status,

                CancellationReason = appointment.CancellationReason
            };
        }

        private static List<Appointment> GetAppointments()
        {
            var patient = GetPatient();

            var doctor = GetDoctor();

            return new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    Patient = patient,
                    DoctorId = 1,
                    Doctor = doctor,
                    ScheduledDate = DateTime.Today,
                    TimeSlot = TimeSlots.Slots.First(),
                    Status = AppointmentStatus.Pending
                },

                new Appointment
                {
                    AppointmentId = 2,
                    PatientId = 1,
                    Patient = patient,
                    DoctorId = 1,
                    Doctor = doctor,
                    ScheduledDate = DateTime.Today,
                    TimeSlot = TimeSlots.Slots.Skip(1).First(),
                    Status = AppointmentStatus.Confirmed
                },

                new Appointment
                {
                    AppointmentId = 3,
                    PatientId = 1,
                    Patient = patient,
                    DoctorId = 1,
                    Doctor = doctor,
                    ScheduledDate = DateTime.Today,
                    TimeSlot = TimeSlots.Slots.Skip(2).First(),
                    Status = AppointmentStatus.Completed
                },

                new Appointment
                {
                    AppointmentId = 4,
                    PatientId = 2,
                    Patient = GetSecondPatient(),
                    DoctorId = 2,
                    Doctor = GetSecondDoctor(),
                    ScheduledDate = DateTime.Today,
                    TimeSlot = TimeSlots.Slots.Skip(3).First(),
                    Status = AppointmentStatus.Cancelled,
                    CancellationReason = "Patient unavailable"
                },

                new Appointment
                {
                    AppointmentId = 5,
                    PatientId = 2,
                    Patient = GetSecondPatient(),
                    DoctorId = 2,
                    Doctor = GetSecondDoctor(),
                    ScheduledDate = DateTime.Today.AddDays(1),
                    TimeSlot = TimeSlots.Slots.Skip(4).First(),
                    Status = AppointmentStatus.Confirmed
                }
            };
        }

        private static Patient GetPatient()
        {
            return new Patient
            {
                PatientId = 1,
                PatientName = "Rishi Patient",
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = GenderType.Male,
                Email = "rishi@example.com",
                PhoneNumber = "9876543210",
                InsuranceID = "INS100",
                IdentityUserId = "patient-user"
            };
        }

        private static Patient GetSecondPatient()
        {
            return new Patient
            {
                PatientId = 2,
                PatientName = "Meera Patient",
                DateOfBirth = new DateTime(1998, 5, 10),
                Gender = GenderType.Female,
                Email = "meera@example.com",
                PhoneNumber = "9876543211",
                InsuranceID = "INS200",
                IdentityUserId = "patient-user-2"
            };
        }

        private static Doctor GetDoctor()
        {
            return new Doctor
            {
                DoctorId = 1,
                DoctorName = "Rishi Doctor",
                Email = "rishi.doctor@example.com",
                Specialisation = SpecialisationType.GeneralPractitioner,
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IdentityUserId = "doctor-user",
                IsActive = true
            };
        }

        private static Doctor GetSecondDoctor()
        {
            return new Doctor
            {
                DoctorId = 2,
                DoctorName = "Meera Doctor",
                Email = "meera.doctor@example.com",
                Specialisation = SpecialisationType.Cardiologist,
                YearsOfExperience = 8,
                ConsultationFee = 900,
                IdentityUserId = "doctor-user-2",
                IsActive = true
            };
        }

        private static BookAppointmentDto GetValidBookAppointmentDto()
        {
            return new BookAppointmentDto
            {
                PatientId = 1,

                DoctorId = 1,

                ScheduledDate = DateTime.Today.AddDays(1),

                TimeSlot = TimeSlots.Slots.First()
            };
        }
    }
}