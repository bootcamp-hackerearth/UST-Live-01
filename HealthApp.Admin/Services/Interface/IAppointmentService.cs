using HealthApp.Shared.Dto;

namespace HealthApp.Admin.Services.Interface
{
    public interface IAppointmentService
    {
        Task<PagedResponse<AppointmentDto>> GetPagedAppointmentsAsync(int pageNumber, int pageSize);

        Task<PagedResponse<AppointmentDto>> GetFilteredAppointmentsAsync(
            int? patientId,
            int? doctorId,
            int pageNumber,
            int pageSize);

        Task<AppointmentDto?> GetAppointmentByIdAsync(int id);

        Task<List<string>> CheckDoctorAvailabilityAsync(int doctorId, DateTime date);

        Task<List<AppointmentDto>> GetUpcomingAppointmentsAsync(
            int doctorId,
            DateTime fromDate,
            DateTime toDate);

        Task<int> GetAppointmentCountAsync();
    }
}