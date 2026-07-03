using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.Enums;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentResponseDto>> GetAllAsync();

        Task<AppointmentResponseDto?> GetByIdAsync(int id);

        Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<AppointmentResponseDto>> GetByDoctorAsync(int doctorId);

        Task<IEnumerable<AppointmentResponseDto>> GetByPatientAsync(int patientId);

        Task<IEnumerable<AppointmentResponseDto>> FilterAsync(
            AppointmentStatus? status,
            DateTime? startDate,
            DateTime? endDate
        );

        Task<bool> ConfirmAsync(int id);

        Task<bool> CompleteAsync(int id);

        Task<bool> CancelAsync(int id, string reason);

        Task<IEnumerable<string>> GetBookedSlotsAsync(int doctorId, DateTime date);

        Task<PagedResponseDto<AppointmentResponseDto>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? search,
            AppointmentStatus? status,
            DateTime? startDate,
            DateTime? endDate
        );
    }
}