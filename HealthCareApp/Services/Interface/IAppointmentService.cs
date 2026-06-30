using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Enums;
using HealthCareApp.Shared.Dtos.Appointments;

namespace HealthCareApp.Services

{
    public interface IAppointmentService
    {
        Task<AppointmentDailyStatusSummaryDto> GetDailyStatusSummaryAsync(DateTime date);
        Task<AppointmentFilterOptionsDto> GetAppointmentFilterOptionsAsync();
        Task<List<AppointmentDto>> GetAllAppointmentsAsync();

        Task<AppointmentDto> GetAppointmentByIdAsync(int appointmentId);

        Task<List<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId);

        Task<List<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId);

        Task<List<AppointmentDto>> GetAppointmentsByStatusAsync(AppointmentStatus status);

        Task<List<AppointmentDto>> GetUpcomingAppointmentsAsync();

        Task<List<AppointmentDto>> GetUpcomingAppointmentsByPatientIdAsync(int patientId);

        Task<List<AppointmentDto>> GetUpcomingAppointmentsByDoctorIdAsync(int doctorId);

        Task<List<AppointmentDto>> GetPendingAppointmentsByPatientIdAsync(int patientId);

        Task<List<AppointmentDto>> GetPendingAppointmentsByDoctorIdAsync(int doctorId);

        Task<List<AppointmentDto>> GetTodayConfirmedAppointmentsByDoctorIdAsync(int doctorId);

        Task<AppointmentDto> BookAppointmentAsync(BookAppointmentDto dto);

        Task<AppointmentDto> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDto dto);

        Task<AppointmentDto> ConfirmAppointmentAsync(int appointmentId);

        Task<AppointmentDto> CompleteAppointmentAsync(int appointmentId);

        Task<AppointmentDto> CancelAppointmentAsync(CancelAppointmentDto dto);

        Task<AppointmentDto> DeleteAppointmentAsync(int appointmentId);

        Task<List<AppointmentDto>> GetMyAppointmentsForPatientAsync(string identityUserId);

        Task<List<AppointmentDto>> GetMyUpcomingAppointmentsForPatientAsync(string identityUserId);

        Task<List<AppointmentDto>> GetMyPendingAppointmentsForPatientAsync(string identityUserId);

        Task<PagedResponse<AppointmentDto>> GetMyAppointmentsForPatientPagedAsync(string identityUserId, AppointmentPaginationQueryDto query);

        Task<PagedResponse<AppointmentDto>> GetMyAppointmentsForDoctorPagedAsync( string identityUserId,AppointmentPaginationQueryDto query);

        Task<AppointmentDto> GetAppointmentByIdForPatientAsync(int appointmentId, string identityUserId);

        Task<AppointmentDto> BookAppointmentForPatientAsync(BookAppointmentDto dto, string identityUserId);

        Task<AppointmentDto> CancelAppointmentForPatientAsync(CancelAppointmentDto dto, string identityUserId);

        Task<List<AppointmentDto>> GetMyAppointmentsForDoctorAsync(string identityUserId);

        Task<List<AppointmentDto>> GetMyUpcomingAppointmentsForDoctorAsync(string identityUserId);

        Task<List<AppointmentDto>> GetMyPendingAppointmentsForDoctorAsync(string identityUserId);

        Task<List<AppointmentDto>> GetMyTodayConfirmedAppointmentsForDoctorAsync(string identityUserId);

        Task<AppointmentDto> GetAppointmentByIdForDoctorAsync(int appointmentId, string identityUserId);

        Task<AppointmentDto> ConfirmAppointmentForDoctorAsync(int appointmentId, string identityUserId);

        Task<AppointmentDto> CompleteAppointmentForDoctorAsync(int appointmentId, string identityUserId);

        Task<AppointmentDto> CancelAppointmentForDoctorAsync(CancelAppointmentDto dto,string identityUserId);

        Task<PagedResponse<AppointmentDto>> GetAllAppointmentsPagedAsync(AppointmentPaginationQueryDto query);
    }
}