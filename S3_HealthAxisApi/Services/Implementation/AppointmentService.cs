using S3_HealthAxisApi.DTOs.Appointment;
using S3_HealthAxisApi.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Interface;

namespace S3_HealthAxisApi.Services.Implementation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _appointmentRepository.GetAllAsync();

            return appointments.Select(MapToAppointmentDto);
        }

        public async Task<AppointmentDetailsDto?> GetByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
                return null;

            return new AppointmentDetailsDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient.FullName,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor.FullName,
                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = (int)appointment.TimeSlot,
                Status = (int)appointment.Status,
                CancellationReason = appointment.CancellationReason
            };
        }

        public async Task<IEnumerable<PatientAppointmentHistoryDto>>
            GetPatientHistoryAsync(int patientId)
        {
            var appointments =
                await _appointmentRepository.GetByPatientIdAsync(patientId);

            return appointments.Select(a =>
                new PatientAppointmentHistoryDto
                {
                    AppointmentId = a.AppointmentId,
                    ScheduledDate = a.ScheduledDate,
                    TimeSlot = (int)a.TimeSlot,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    Status = (int)a.Status
                });
        }

        public async Task<IEnumerable<DoctorScheduleItemDto>>
            GetDoctorTodayScheduleAsync(int doctorId)
        {
            var appointments =
                await _appointmentRepository.GetDoctorTodayScheduleAsync(
                    doctorId,
                    DateOnly.FromDateTime(DateTime.Today));

            return appointments.Select(MapDoctorScheduleItem);
        }

        public async Task<IEnumerable<DoctorScheduleItemDto>>
            GetDoctorWeekScheduleAsync(
                int doctorId,
                DateOnly startDate,
                DateOnly endDate)
        {
            var appointments =
                await _appointmentRepository.GetDoctorWeekScheduleAsync(
                    doctorId,
                    startDate,
                    endDate);

            return appointments.Select(MapDoctorScheduleItem);
        }

        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
        {
            await ValidateBookingAsync(
                dto.PatientId,
                dto.DoctorId,
                dto.ScheduledDate,
                dto.TimeSlot);

            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                ScheduledDate = dto.ScheduledDate,
                TimeSlot = (AppointmentTimeSlot)dto.TimeSlot,
                Status = AppointmentStatus.Pending
            };

            await _appointmentRepository.AddAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            return MapToAppointmentDto(appointment);
        }

        public async Task UpdateAsync(
            int id,
            UpdateAppointmentDto dto)
        {
            var appointment =
                await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
                throw new KeyNotFoundException(
                    $"Appointment {id} not found.");

            if (appointment.Status == AppointmentStatus.Completed)
                throw new InvalidOperationException(
                    "Completed appointments cannot be modified.");

            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new InvalidOperationException(
                    "Cancelled appointments cannot be modified.");

            await ValidateUpdateBookingAsync(
                appointment.AppointmentId,
                appointment.PatientId,
                dto.DoctorId,
                dto.ScheduledDate,
                dto.TimeSlot);

            appointment.DoctorId = dto.DoctorId;
            appointment.ScheduledDate = dto.ScheduledDate;
            appointment.TimeSlot =
                (AppointmentTimeSlot)dto.TimeSlot;

            await _appointmentRepository.UpdateAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(
    int id,
    UpdateAppointmentStatusDto dto)
        {
            var appointment =
                await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
                throw new KeyNotFoundException(
                    $"Appointment {id} not found.");

            if (!Enum.IsDefined(
                    typeof(AppointmentStatus),
                    dto.Status))
            {
                throw new ArgumentException(
                    "Invalid appointment status.");
            }

            var newStatus =
                (AppointmentStatus)dto.Status;

            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException(
                    "Completed appointments cannot be modified.");
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "Cancelled appointments cannot be modified.");
            }

            switch (newStatus)
            {
                case AppointmentStatus.Pending:
                    throw new InvalidOperationException(
                        "Cannot manually change appointment back to Pending.");

                case AppointmentStatus.Confirmed:

                    if (appointment.Status != AppointmentStatus.Pending)
                    {
                        throw new InvalidOperationException(
                            "Only pending appointments can be confirmed.");
                    }

                    appointment.Status =
                        AppointmentStatus.Confirmed;
                    break;

                case AppointmentStatus.Completed:

                    if (appointment.Status != AppointmentStatus.Confirmed)
                    {
                        throw new InvalidOperationException(
                            "Only confirmed appointments can be completed.");
                    }

                    appointment.Status =
                        AppointmentStatus.Completed;
                    break;

                case AppointmentStatus.Cancelled:

                    if (string.IsNullOrWhiteSpace(
                            dto.CancellationReason))
                    {
                        throw new ArgumentException(
                            "Cancellation reason is required.");
                    }

                    appointment.Status =
                        AppointmentStatus.Cancelled;

                    appointment.CancellationReason =
                        dto.CancellationReason.Trim();

                    break;

                default:
                    throw new ArgumentException(
                        "Invalid appointment status.");
            }

            await _appointmentRepository.UpdateAsync(
                appointment);

            await _appointmentRepository.SaveChangesAsync();
        }

        public async Task ConfirmAsync(int id)
        {
            var appointment =
                await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
                throw new KeyNotFoundException();

            if (appointment.Status != AppointmentStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending appointments can be confirmed.");

            appointment.Status = AppointmentStatus.Confirmed;

            await _appointmentRepository.UpdateAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();
        }

        public async Task CompleteAsync(int id)
        {
            var appointment =
                await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
                throw new KeyNotFoundException();

            if (appointment.Status != AppointmentStatus.Confirmed)
                throw new InvalidOperationException(
                    "Only confirmed appointments can be completed.");

            appointment.Status = AppointmentStatus.Completed;

            await _appointmentRepository.UpdateAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();
        }

        public async Task CancelAsync(
            int id,
            CancelAppointmentDto dto)
        {
            var appointment =
                await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
                throw new KeyNotFoundException();

            if (appointment.Status == AppointmentStatus.Completed)
                throw new InvalidOperationException(
                    "Completed appointments cannot be cancelled.");

            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new InvalidOperationException(
                    "Appointment already cancelled.");

            if (string.IsNullOrWhiteSpace(dto.CancellationReason))
                throw new ArgumentException(
                    "Cancellation reason is required.");

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.CancellationReason =
                dto.CancellationReason.Trim();

            await _appointmentRepository.UpdateAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<DoctorScheduleItemDto>> GetDoctorUpcomingScheduleAsync(int doctorId)
        {
            var startDate =
                DateOnly.FromDateTime(DateTime.Today);

            var endDate =
                startDate.AddDays(7);

            var appointments =
                await _appointmentRepository
                    .GetDoctorWeekScheduleAsync(
                        doctorId,
                        startDate,
                        endDate);

            return appointments.Select(MapDoctorScheduleItem);
        }

        private async Task ValidateBookingAsync(
            int patientId,
            int doctorId,
            DateOnly date,
            int timeSlot)
        {
            var patient =
                await _patientRepository.GetByIdAsync(patientId);

            if (patient == null)
                throw new KeyNotFoundException(
                    "Patient not found.");

            if (!patient.IsActive)
                throw new InvalidOperationException(
                    "Inactive patients cannot book appointments.");

            var doctor =
                await _doctorRepository.GetByIdAsync(doctorId);

            if (doctor == null)
                throw new KeyNotFoundException(
                    "Doctor not found.");

            if (!doctor.IsActive)
                throw new InvalidOperationException(
                    "Inactive doctor.");

            if (date < DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException(
                    "Appointment date cannot be in the past.");

            if (!Enum.IsDefined(
                    typeof(AppointmentTimeSlot),
                    timeSlot))
            {
                throw new ArgumentException(
                    "Invalid appointment slot.");
            }

            if (await _appointmentRepository
                .ExistsSamePatientSameDoctorSameDateAsync(
                    patientId,
                    doctorId,
                    date))
            {
                throw new InvalidOperationException(
                    "Patient already has an appointment with this doctor on the selected date.");
            }

            if (await _appointmentRepository
                .ExistsSamePatientSameSlotSameDateAsync(
                    patientId,
                    date,
                    timeSlot))
            {
                throw new InvalidOperationException(
                    "Patient already has another appointment in this time slot.");
            }

            if (await _appointmentRepository
                .ExistsSameDoctorSameSlotSameDateAsync(
                    doctorId,
                    date,
                    timeSlot))
            {
                throw new InvalidOperationException(
                    "Doctor is already booked for this time slot.");
            }
        }

        private async Task ValidateUpdateBookingAsync(int appointmentId,int patientId,int doctorId,DateOnly date,int timeSlot)
        {
            var patient =
                await _patientRepository.GetByIdAsync(patientId);

            if (patient == null)
                throw new KeyNotFoundException(
                    "Patient not found.");

            if (!patient.IsActive)
                throw new InvalidOperationException(
                    "Inactive patients cannot book appointments.");

            var doctor =
                await _doctorRepository.GetByIdAsync(doctorId);

            if (doctor == null)
                throw new KeyNotFoundException(
                    "Doctor not found.");

            if (!doctor.IsActive)
                throw new InvalidOperationException(
                    "Inactive doctor.");

            if (date < DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException(
                    "Appointment date cannot be in the past.");

            if (!Enum.IsDefined(
                    typeof(AppointmentTimeSlot),
                    timeSlot))
            {
                throw new ArgumentException(
                    "Invalid appointment slot.");
            }

            if (await _appointmentRepository
                .ExistsSamePatientSameDoctorSameDateAsync(
                    patientId,
                    doctorId,
                    date,
                    appointmentId))
            {
                throw new InvalidOperationException(
                    "Patient already has an appointment with this doctor on the selected date.");
            }

            if (await _appointmentRepository
                .ExistsSamePatientSameSlotSameDateAsync(
                    patientId,
                    date,
                    timeSlot,
                    appointmentId))
            {
                throw new InvalidOperationException(
                    "Patient already has another appointment in this time slot.");
            }

            if (await _appointmentRepository
                .ExistsSameDoctorSameSlotSameDateAsync(
                    doctorId,
                    date,
                    timeSlot,
                    appointmentId))
            {
                throw new InvalidOperationException(
                    "Doctor is already booked for this time slot.");
            }
        }

        private static AppointmentDto MapToAppointmentDto(
            Appointment appointment)
        {
            return new AppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = (int)appointment.TimeSlot,
                Status = (int)appointment.Status,
                CancellationReason = appointment.CancellationReason
            };
        }

        private static DoctorScheduleItemDto MapDoctorScheduleItem(
            Appointment appointment)
        {
            return new DoctorScheduleItemDto
            {
                AppointmentId = appointment.AppointmentId,
                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = (int)appointment.TimeSlot,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient.FullName,
                Status = (int)appointment.Status
            };
        }
    }
}