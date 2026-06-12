using HealthAxis.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAxis.Web.Services
{
    public interface IAppointmentApiService
    {
        Task<ApiResponseDto> Book(BookAppointmentDto dto);

        Task<List<AppointmentDto>> GetByPatient(int patientId);

        Task<List<AppointmentDto>> GetByDoctor(int doctorId);

        Task<AppointmentDto> GetById(int appointmentId);

        Task<ApiResponseDto> UpdateStatus(int id, UpdateAppointmentStatusDto dto);

        Task<ApiResponseDto> Cancel(int id, UpdateAppointmentStatusDto dto);
    }
}