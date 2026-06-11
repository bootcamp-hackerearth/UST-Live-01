using SharedClasses.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthcareWeb.Services
{
    public interface IAppointmentApiService
    {
        Task<List<AppointmentDto>> GetAllAsync();

        Task<AppointmentDto> GetByIdAsync(int id);

        Task<List<AppointmentDto>> GetByPatientAsync(int patientId);

        Task<List<AppointmentDto>> GetByDoctorAsync(int doctorId);

        Task<List<AppointmentDto>> GetUpcomingByPatientAsync(int patientId);

        Task<List<AppointmentDto>> GetUpcomingByDoctorAsync(int doctorId);

        Task<List<AppointmentDto>> GetCancelledByPatientAsync(int patientId);

        Task<AppointmentDto> BookAsync(BookAppointmentDto dto);

        Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto dto);

        Task<AppointmentDto> DeleteAsync(int id);

        Task<AppointmentDto> ConfirmAsync(int id, ConfirmAppointmentDto dto);

        Task<AppointmentDto> CancelByPatientAsync(int id, CancelByPatientDto dto);

        Task<AppointmentDto> CancelByDoctorAsync(int id, CancelByDoctorDto dto);

        Task<AppointmentDto> CompleteAsync(int id, CompleteAppointmentDto dto);

        Task<List<AppointmentDto>> GetCancelledByDoctorAsync(int doctorId);

        Task<List<AppointmentDto>> SearchAsync(string query);

        Task<List<AppointmentDto>> SearchCancelledByPatientAsync(
            int patientId,
            string query);

        Task<List<AppointmentDto>> SearchCancelledByDoctorAsync(
            int doctorId,
            string query);

        Task<List<AppointmentDto>> SearchUpcomingByDoctorAsync(
            int doctorId,
            string query);

    }
}