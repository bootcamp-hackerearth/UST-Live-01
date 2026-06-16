using AutoMapper;
using HealthAxisHealth.Shared.DTOs.AppointmentDtos;
using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.API.Exceptions;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Services.Interfaces;
using HealthAxisHealth.API.UnitOfWork;

namespace HealthAxisHealth.API.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public AppointmentService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region Get Methods

        public async Task<PagedResultDto<AppointmentDto>>
            GetPagedAsync(
                PaginationParams pagination)
        {
            PagedResultDto<Appointment> result =
                await _unitOfWork.Appointments
                    .GetPagedAsync(pagination);

            return new PagedResultDto<AppointmentDto>
            {
                Items =
                    _mapper.Map<List<AppointmentDto>>(
                        result.Items),

                PageNumber =
                    result.PageNumber,

                PageSize =
                    result.PageSize,

                TotalRecords =
                    result.TotalRecords
            };
        }

        public async Task<IEnumerable<AppointmentDto>>
            GetByPatientIdAsync(
                int patientId)
        {
            IEnumerable<Appointment> appointments =
                await _unitOfWork.Appointments
                    .GetByPatientIdAsync(patientId);

            return _mapper.Map<List<AppointmentDto>>(
                appointments);
        }

        public async Task<IEnumerable<AppointmentDto>>
            GetByDoctorIdAsync(
                int doctorId)
        {
            IEnumerable<Appointment> appointments =
                await _unitOfWork.Appointments
                    .GetByDoctorIdAsync(doctorId);

            return _mapper.Map<List<AppointmentDto>>(
                appointments);
        }

        public async Task<AppointmentDto?>
            GetByIdAsync(
                int appointmentId)
        {
            Appointment? appointment =
                await _unitOfWork.Appointments
                    .GetAppointmentWithDetailsAsync(
                        appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException(
                    "Appointment not found.");
            }

            return _mapper.Map<AppointmentDto>(
                appointment);
        }

        public async Task<IEnumerable<AppointmentDto>>
            GetDoctorAppointmentsByDateAsync(
                int doctorId,
                DateTime date)
        {
            IEnumerable<Appointment> appointments =
                await _unitOfWork.Appointments
                    .GetDoctorAppointmentsByDateAsync(
                        doctorId,
                        date);

            return _mapper.Map<List<AppointmentDto>>(
                appointments);
        }

        #endregion

        #region Create Appointment

        public async Task<int>
            CreateAsync(
                int userId,
                CreateAppointmentDto dto)
        {
            if (dto.ScheduledDate.Date <
                DateTime.Today)
            {
                throw new BadRequestException(
                    "Appointments cannot be booked for past dates.");
            }

            if (dto.ScheduledDate.Date >
                DateTime.Today.AddMonths(6))
            {
                throw new BadRequestException(
                    "Appointments can only be booked up to 6 months in advance.");
            }

            if (string.IsNullOrWhiteSpace(
                    dto.TimeSlot) ||
                !dto.TimeSlot.Contains('-'))
            {
                throw new BadRequestException(
                    "Invalid time slot format.");
            }

            Patient? patient =
                await _unitOfWork.Patients
                    .GetByUserIdAsync(userId);

            if (patient == null)
            {
                throw new NotFoundException(
                    "Patient not found.");
            }

            Doctor? doctor =
                await _unitOfWork.Doctors
                    .GetByIdAsync(dto.DoctorId);

            if (doctor == null)
            {
                throw new NotFoundException(
                    "Doctor not found.");
            }

            if (!doctor.IsActive)
            {
                throw new BadRequestException(
                    "Doctor is not available for appointments.");
            }

            if (dto.ScheduledDate.Date ==
                DateTime.Today)
            {
                string startTime =
                    dto.TimeSlot
                        .Split('-')[0]
                        .Trim();

                TimeSpan slotTime =
                    TimeSpan.Parse(startTime);

                if (slotTime <=
                    DateTime.Now.TimeOfDay)
                {
                    throw new BadRequestException(
                        "The selected time slot has already passed.");
                }
            }

            bool available =
                await _unitOfWork.Appointments
                    .IsSlotAvailableAsync(
                        dto.DoctorId,
                        dto.ScheduledDate,
                        dto.TimeSlot);

            if (!available)
            {
                throw new BadRequestException(
                    "Selected time slot is not available.");
            }

            Appointment appointment =
                new Appointment
                {
                    PatientId =
                        patient.PatientId,

                    DoctorId =
                        dto.DoctorId,

                    ScheduledDate =
                        dto.ScheduledDate,

                    TimeSlot =
                        dto.TimeSlot,

                    Status =
                        AppointmentStatus.Pending
                };

            await _unitOfWork.Appointments
                .AddAsync(appointment);

            await _unitOfWork.CommitAsync();

            return appointment.AppointmentId;
        }

        #endregion
        #region Update Appointment Status

        public async Task UpdateStatusAsync(
            int appointmentId,
            UpdateAppointmentStatusDto dto)
        {
            Appointment? appointment =
                await _unitOfWork.Appointments
                    .GetByIdAsync(appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException(
                    "Appointment not found.");
            }

            // Prevent confirming an already confirmed appointment
            if (appointment.Status == AppointmentStatus.Confirmed &&
                dto.Status == AppointmentStatus.Confirmed)
            {
                throw new BadRequestException(
                    "Appointment is already confirmed.");
            }

            // Prevent cancelling an already cancelled appointment
            if (appointment.Status == AppointmentStatus.Cancelled &&
                dto.Status == AppointmentStatus.Cancelled)
            {
                throw new BadRequestException(
                    "Appointment is already cancelled.");
            }

            // Prevent completing an already completed appointment
            if (appointment.Status == AppointmentStatus.Completed &&
                dto.Status == AppointmentStatus.Completed)
            {
                throw new BadRequestException(
                    "Appointment is already completed.");
            }

            // Prevent cancelling a completed appointment
            if (appointment.Status == AppointmentStatus.Completed &&
                dto.Status == AppointmentStatus.Cancelled)
            {
                throw new BadRequestException(
                    "Completed appointments cannot be cancelled.");
            }

            // Prevent confirming a cancelled appointment
            if (appointment.Status == AppointmentStatus.Cancelled &&
                dto.Status == AppointmentStatus.Confirmed)
            {
                throw new BadRequestException(
                    "Cancelled appointments cannot be confirmed.");
            }

            // Prevent completing a cancelled appointment
            if (appointment.Status == AppointmentStatus.Cancelled &&
                dto.Status == AppointmentStatus.Completed)
            {
                throw new BadRequestException(
                    "Cancelled appointments cannot be completed.");
            }

            // Prevent modifying a completed appointment
            if (appointment.Status == AppointmentStatus.Completed &&
                dto.Status != AppointmentStatus.Completed)
            {
                throw new BadRequestException(
                    "Completed appointments cannot be modified.");
            }

            // Prevent completing a pending appointment directly
            if (appointment.Status == AppointmentStatus.Pending &&
                dto.Status == AppointmentStatus.Completed)
            {
                throw new BadRequestException(
                    "Pending appointments must be confirmed before completion.");
            }

            // Require cancellation reason
            if (dto.Status == AppointmentStatus.Cancelled &&
                string.IsNullOrWhiteSpace(dto.CancellationReason))
            {
                throw new BadRequestException(
                    "Cancellation reason is required.");
            }

            switch (dto.Status)
            {
                case AppointmentStatus.Confirmed:
                    appointment.Confirm();
                    break;

                case AppointmentStatus.Cancelled:
                    appointment.Cancel(dto.CancellationReason!);
                    break;

                case AppointmentStatus.Completed:
                    appointment.Complete();
                    break;

                default:
                    appointment.Status = dto.Status;
                    break;
            }

            await _unitOfWork.Appointments.UpdateAsync(appointment);

            await _unitOfWork.CommitAsync();
        }

        #endregion

        #region Delete Appointment

        public async Task DeleteAsync(
            int appointmentId)
        {
            Appointment? appointment =
                await _unitOfWork.Appointments
                    .GetByIdAsync(appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException(
                    "Appointment not found.");
            }

            // Prevent deleting completed appointments
            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new BadRequestException(
                    "Completed appointments cannot be deleted.");
            }

            // Prevent deleting confirmed appointments
            if (appointment.Status == AppointmentStatus.Confirmed)
            {
                throw new BadRequestException(
                    "Confirmed appointments cannot be deleted.");
            }

            await _unitOfWork.Appointments.DeleteAsync(appointment);

            await _unitOfWork.CommitAsync();
        }

        #endregion
    }
}