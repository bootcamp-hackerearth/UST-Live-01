using HealthCareApp.Models;
using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Enums;

namespace HealthCareApp.Repository.Interface
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime scheduledDate,CancellationToken ct = default);
        Task<List<Appointment>> GetAppointmentsForFilterOptionsAsync(CancellationToken ct = default);

        Task<List<Appointment>> GetByPatientIdAsync(int patientId, CancellationToken ct = default);

        Task<List<Appointment>> GetByDoctorIdAsync(int doctorId, CancellationToken ct = default);

        Task<List<Appointment>> GetByStatusAsync(AppointmentStatus status, CancellationToken ct = default);

        Task<List<Appointment>> GetUpcomingAppointmentsAsync(CancellationToken ct = default);

        Task<List<Appointment>> GetUpcomingAppointmentsByPatientIdAsync(int patientId, CancellationToken ct = default);

        Task<List<Appointment>> GetUpcomingAppointmentsByDoctorIdAsync(int doctorId, CancellationToken ct = default);

        Task<List<Appointment>> GetPendingAppointmentsByPatientIdAsync(int patientId, CancellationToken ct = default);

        Task<List<Appointment>> GetPendingAppointmentsByDoctorIdAsync(int doctorId, CancellationToken ct = default);

        Task<List<Appointment>> GetTodayConfirmedAppointmentsByDoctorIdAsync(int doctorId, CancellationToken ct = default);

        Task<List<Appointment>> GetCancelledAppointmentsByPatientIdAsync(int patientId, CancellationToken ct = default);

        Task<List<Appointment>> GetCancelledAppointmentsByDoctorIdAsync(int doctorId, CancellationToken ct = default);

        Task<int> CountActiveAppointmentsByDoctorAndDateAsync(int doctorId, DateTime date, CancellationToken ct = default);

        Task<bool> IsSlotBookedAsync(int doctorId, DateTime date, string timeSlot, CancellationToken ct = default);

        Task<bool> PatientHasActiveAppointmentWithDoctorOnDateAsync(int patientId, int doctorId, DateTime date, CancellationToken ct = default);

        Task<bool> PatientHasActiveAppointmentOnDateAndSlotAsync(int patientId, DateTime date, string timeSlot, CancellationToken ct = default);

        Task<List<string>> GetBookedTimeSlotsByDoctorAndDateAsync( int doctorId, DateTime date,CancellationToken ct = default);

        Task<(List<Appointment> Items, int TotalRecords)> GetPagedAppointmentsAsync( AppointmentPaginationQueryDto query,CancellationToken ct = default);
    }
}