using AutoMapper;
using HealthAxisHealth.Shared.DTOs.AppointmentDtos;
using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.API.Exceptions;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Services.Interfaces;
using HealthAxisHealth.API.UnitOfWork;
using System.Globalization;
using HealthAxisHealth.Shared.Utilities;

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
                    Constants.AppointmentNotFound);
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
                    Constants.AppointmentPastDate);
            }

            if (dto.ScheduledDate.Date >
                DateTime.Today.AddMonths(6))
            {
                throw new BadRequestException(
                    Constants.AppointmentFutureLimit);
            }

            if (string.IsNullOrWhiteSpace(
                    dto.TimeSlot) ||
                !dto.TimeSlot.Contains('-'))
            {
                throw new BadRequestException(
                    Constants.InvalidTimeSlotFormat);
            }

            Patient? patient =
                await _unitOfWork.Patients
                    .GetByUserIdAsync(userId);

            if (patient == null)
            {
                throw new NotFoundException(
                    Constants.PatientNotFound);
            }

            Doctor? doctor =
                await _unitOfWork.Doctors
                    .GetByIdAsync(dto.DoctorId);

            if (doctor == null)
            {
                throw new NotFoundException(
                    Constants.DoctorNotFound);
            }

            if (!doctor.IsActive)
            {
                throw new BadRequestException(
                    Constants.DoctorUnavailable);
            }

            if (dto.ScheduledDate.Date ==
                DateTime.Today)
            {
                string startTime =
                    dto.TimeSlot
                        .Split('-')[0]
                        .Trim();

                TimeSpan slotTime = TimeSpan.Parse(
                        startTime,
                        CultureInfo.InvariantCulture);

                    if (slotTime <=
                    DateTime.Now.TimeOfDay)
                {
                    throw new BadRequestException(
                        Constants.TimeSlotPassed);
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
                    Constants.TimeSlotUnavailable);
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
                    Constants.AppointmentNotFound);
            }

            ValidateStatusTransition(appointment, dto);

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

            ValidateDeleteOperation(appointment);

            await _unitOfWork.Appointments
                .DeleteAsync(appointment);

            await _unitOfWork.CommitAsync();
        }
        #endregion

        #region Validation Methods

        private static void ValidateStatusTransition(
            Appointment appointment,
            UpdateAppointmentStatusDto dto)
        {
            ValidateDuplicateStatus(appointment, dto);

            ValidateCompletedTransitions(appointment, dto);

            ValidateCancelledTransitions(appointment, dto);

            ValidatePendingTransitions(appointment, dto);

            ValidateCancellationReason(dto);
        }

        private static void ValidateDuplicateStatus(
            Appointment appointment,
            UpdateAppointmentStatusDto dto)
        {
            if (appointment.Status == AppointmentStatus.Confirmed &&
                dto.Status == AppointmentStatus.Confirmed)
            {
                throw new BadRequestException(
                    Constants.AppointmentAlreadyConfirmed);
            }

            if (appointment.Status == AppointmentStatus.Cancelled &&
                dto.Status == AppointmentStatus.Cancelled)
            {
                throw new BadRequestException(
                    Constants.AppointmentAlreadyCancelled);
            }

            if (appointment.Status == AppointmentStatus.Completed &&
                dto.Status == AppointmentStatus.Completed)
            {
                throw new BadRequestException(
                    Constants.AppointmentAlreadyCompleted);
            }
        }

        private static void ValidateCompletedTransitions(
            Appointment appointment,
            UpdateAppointmentStatusDto dto)
        {
            if (appointment.Status == AppointmentStatus.Completed &&
                dto.Status == AppointmentStatus.Cancelled)
            {
                throw new BadRequestException(
                    Constants.CompletedCannotBeCancelled);
            }

            if (appointment.Status == AppointmentStatus.Completed &&
                dto.Status != AppointmentStatus.Completed)
            {
                throw new BadRequestException(
                    Constants.CompletedCannotBeModified);
            }
        }

        private static void ValidateCancelledTransitions(
            Appointment appointment,
            UpdateAppointmentStatusDto dto)
        {
            if (appointment.Status == AppointmentStatus.Cancelled &&
                dto.Status == AppointmentStatus.Confirmed)
            {
                throw new BadRequestException(
                    Constants.CancelledCannotBeConfirmed);
            }

            if (appointment.Status == AppointmentStatus.Cancelled &&
                dto.Status == AppointmentStatus.Completed)
            {
                throw new BadRequestException(
                    Constants.CancelledCannotBeCompleted);
            }
        }

        private static void ValidatePendingTransitions(
            Appointment appointment,
            UpdateAppointmentStatusDto dto)
        {
            if (appointment.Status == AppointmentStatus.Pending &&
                dto.Status == AppointmentStatus.Completed)
            {
                throw new BadRequestException(
                    Constants.PendingMustBeConfirmed);
            }
        }

        private static void ValidateCancellationReason(
            UpdateAppointmentStatusDto dto)
        {
            if (dto.Status == AppointmentStatus.Cancelled &&
                string.IsNullOrWhiteSpace(
                    dto.CancellationReason))
            {
                throw new BadRequestException(
                    Constants.CancellationReasonRequired);
            }
        }

        private static void ValidateDeleteOperation(
            Appointment appointment)
        {
            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new BadRequestException(
                    Constants.CompletedCannotBeDeleted);
            }

            if (appointment.Status == AppointmentStatus.Confirmed)
            {
                throw new BadRequestException(
                    Constants.ConfirmedCannotBeDeleted);
            }
        }

        #endregion
    }
}