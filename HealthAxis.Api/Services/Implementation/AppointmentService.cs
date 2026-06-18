using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Interfaces;
using System.Security.Claims;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class AppointmentService(
        IAppointmentRepository appointmentRepository,
        IDoctorRepository doctorRepository,
        IMapper mapper
    ) : IAppointmentService
    {
        public async Task<List<AppointmentDto>> GetAppointmentsAsync(
            int? patientId,
            int? doctorId,
            DateTime? date,
            ClaimsPrincipal user,
            CancellationToken ct = default
        )
        {
            var role = user.FindFirst(ClaimTypes.Role)?.Value;

            if (role == "Patient")
                patientId = Convert.ToInt32(
                    user.FindFirst("PatientId")?.Value
                );

            if (role == "Doctor")
                doctorId = Convert.ToInt32(
                    user.FindFirst("DoctorId")?.Value
                );

            return mapper.Map<List<AppointmentDto>>(
                await appointmentRepository.GetAppointmentsAsync(
                    patientId,
                    doctorId,
                    date,
                    ct
                )
            );
        }

        public async Task<AppointmentDto> CreateAsync(
            CreateAppointmentDto request,
            ClaimsPrincipal user,
            CancellationToken ct = default
        )
        {
            var patientId = Convert.ToInt32(
                user.FindFirst("PatientId")?.Value
            );

            if (request.ScheduledDate.Date < DateTime.UtcNow.Date)
                throw new InvalidException(
                    "Cannot book past date"
                );

            var slots = await doctorRepository.GetAvailableSlotsAsync(
                request.DoctorId,
                request.ScheduledDate,
                ct
            );

            if (!slots.Contains(request.TimeSlot))
                throw new InvalidException(
                    "Slot not available"
                );

            var appt = mapper.Map<Appointment>(request);

            appt.PatientId = patientId;

            var saved = await appointmentRepository.CreateAsync(
                appt,
                ct
            );

            return mapper.Map<AppointmentDto>(
                await appointmentRepository.GetDetailsAsync(
                    saved.AppointmentId,
                    ct
                ) ?? saved
            );
        }

        public async Task<AppointmentDto> UpdateStatusAsync(
            int id,
            UpdateAppointmentStatusDto request,
            ClaimsPrincipal user,
            CancellationToken ct = default
        )
        {
            var appt =
                await appointmentRepository.GetDetailsAsync(id, ct)
                ?? throw new NotFoundException(
                    "Appointment not found"
                );

            var role = user.FindFirst(ClaimTypes.Role)?.Value;

            if (
                role == "Patient" &&
                request.Status != "Cancelled"
            )
            {
                throw new UnauthorizedException(
                    "Patients can only cancel appointments"
                );
            }

            appt.Status = request.Status;
            appt.CancellationReason =
                request.CancellationReason ?? string.Empty;

            var updated = await appointmentRepository.UpdateAsync(
                id,
                appt,
                ct
            ) ?? throw new NotFoundException(
                "Appointment not found"
            );

            return mapper.Map<AppointmentDto>(
                await appointmentRepository.GetDetailsAsync(
                    updated.AppointmentId,
                    ct
                ) ?? updated
            );
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken ct = default
        )
        {
            if (await appointmentRepository.DeleteAsync(id, ct) == null)
                throw new NotFoundException(
                    "Appointment not found"
                );
        }
    }
}