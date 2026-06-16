using HealthAxisCore_Api.DTOs.Appointment;
using HealthAxisCore_Api.Enums;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentResponseDTO>> GetAllAsync();

        Task<AppointmentResponseDTO?> GetByIdAsync(int id);

        Task<AppointmentResponseDTO> CreateAsync(CreateAppointmentDTO dto);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<AppointmentResponseDTO>> GetByDoctorAsync(int doctorId);

        Task<IEnumerable<AppointmentResponseDTO>> GetByPatientAsync(int patientId);

        Task<IEnumerable<AppointmentResponseDTO>> FilterAsync(
            AppointmentStatus? status,
            DateTime? startDate,
            DateTime? endDate);

        Task<bool> CancelAsync(int id, string reason);

        Task<bool> ConfirmAsync(int id);
    }
}