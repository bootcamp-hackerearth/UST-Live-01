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

        public async Task<List<AppointmentReportDto>> GetAppointmentReportAsync(
            CancellationToken ct = default)
        {
            List<Appointment> appointments =
                await _appointmentRepository.GetAllAsync(ct);

            List<AppointmentReportDto> report =
                appointments
                    .GroupBy(appointment => appointment.ScheduledDate.Date)
                    .Select(group => new AppointmentReportDto
                    {
                        Date = group.Key,

                        TotalCount = group.Count(),

                        ScheduledCount = group.Count(appointment =>
                            appointment.Status == AppointmentStatus.Scheduled),

                        ConfirmedCount = group.Count(appointment =>
                            appointment.Status == AppointmentStatus.Confirmed),

                        CancelledCount = group.Count(appointment =>
                            appointment.Status == AppointmentStatus.Cancelled),

                        CompletedCount = group.Count(appointment =>
                            appointment.Status == AppointmentStatus.Completed)
                    })
                    .OrderBy(reportItem => reportItem.Date)
                    .ToList();

            return report;
        }
    }
}
