using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.Enums;

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
            DateTime? endDate
        );

        Task<bool> ConfirmAsync(int id);

        Task<bool> CompleteAsync(int id);

        Task<bool> CancelAsync(int id, string reason);

        Task<PagedResponseDTO<AppointmentResponseDTO>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? search,
            AppointmentStatus? status,
            DateTime? startDate,
            DateTime? endDate
        );
    }
}
