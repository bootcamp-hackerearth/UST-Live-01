using HealthAxis.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAxis.Web.Services
{
    public interface IAppointmentApiService
    {
        Task<List<AppointmentDto>> GetByDoctor(int doctorId);

        Task<List<AppointmentDto>> GetByPatient(int patientId);

        Task<AppointmentDto> GetById(int appointmentId);

        Task<ApiResponseDto> Book(BookAppointmentDto dto);

        Task UpdateStatus(int id, AppointmentStatus status);

        Task Cancel(int id, UpdateAppointmentStatusDto dto);
    }
}