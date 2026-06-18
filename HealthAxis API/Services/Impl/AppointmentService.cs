using AutoMapper;
using HealthAxis.API.DTOs.Appointments;
using HealthAxis.API.Enums;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;

namespace HealthAxis.API.Services
{
    public class AppointmentService
        : Service<Appointment, AppointmentReadDto, AppointmentCreateDto, AppointmentUpdateDto>,
          IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IMapper mapper)
            : base(appointmentRepository, mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<AppointmentReadDto?> UpdateStatusAsync(
            int appointmentId,
            AppointmentStatusUpdateDto statusUpdateDto,
            CancellationToken ct = default)
        {
            Appointment? appointment =
                await _appointmentRepository.GetByIdAsync(appointmentId, ct);

            if (appointment == null)
            {
                return null;
            }

            if (statusUpdateDto.Status == AppointmentStatus.Confirmed)
            {
                appointment.Confirm();
            }
            else if (statusUpdateDto.Status == AppointmentStatus.Cancelled)
            {
                appointment.Cancel(statusUpdateDto.CancellationReason);
            }
            else if (statusUpdateDto.Status == AppointmentStatus.Completed)
            {
                appointment.Complete();
            }
            else
            {
                appointment.Status = statusUpdateDto.Status;
            }

            await _appointmentRepository.SaveChangesAsync(ct);

            return _mapper.Map<AppointmentReadDto>(appointment);
        }

        public async Task<AppointmentReportDto> GetAppointmentReportAsync(
            CancellationToken ct = default)
        {
            int totalAppointments =
                await _appointmentRepository.CountAsync(null, ct);

            int scheduledAppointments =
                await _appointmentRepository.CountAsync(
                    appointment => appointment.Status == AppointmentStatus.Scheduled,
                    ct);

            int confirmedAppointments =
                await _appointmentRepository.CountAsync(
                    appointment => appointment.Status == AppointmentStatus.Confirmed,
                    ct);

            int completedAppointments =
                await _appointmentRepository.CountAsync(
                    appointment => appointment.Status == AppointmentStatus.Completed,
                    ct);

            int cancelledAppointments =
                await _appointmentRepository.CountAsync(
                    appointment => appointment.Status == AppointmentStatus.Cancelled,
                    ct);

            return new AppointmentReportDto
            {
                TotalAppointments = totalAppointments,
                ScheduledAppointments = scheduledAppointments,
                ConfirmedAppointments = confirmedAppointments,
                CompletedAppointments = completedAppointments,
                CancelledAppointments = cancelledAppointments
            };
        }
    }
}
