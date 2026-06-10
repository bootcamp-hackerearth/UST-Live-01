using HealthAxis.Shared.Dtos;
using System.Collections.Generic;

namespace HealthAxis.Api.Services
{
    public interface IAppointmentService
    {
        List<AppointmentDto> GetByDoctor(int doctorId);

        List<AppointmentDto> GetByPatient(int patientId);

        AppointmentDto GetById(int id);

        ApiResponseDto Book(BookAppointmentDto dto);

        ApiResponseDto UpdateStatus(int id, AppointmentStatus status);

        ApiResponseDto Cancel(int id, CancelAppointmentDto dto);
    }
}