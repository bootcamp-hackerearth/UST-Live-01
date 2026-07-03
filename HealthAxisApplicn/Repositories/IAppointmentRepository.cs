using HealthAxisApplicn.Models;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;

namespace HealthAxisApplicn.Repositories
{
    public interface IAppointmentRepository: IRepository<Appointment>
    {
        Task<List<Appointment>> GetAppointmentsByPatientIdAsync(int patientId, CancellationToken ct = default);
        Task<List<Appointment>> GetUpcomingAppointmentsByDoctorIdAsync(int doctorId, CancellationToken ct = default);
        Task<List<Appointment>> GetAppointmentsByPatientNameAsync(string patientName, CancellationToken ct = default);
        Task<List<Appointment>> GetAppointmentsByDoctorNameAsync(string doctorName, CancellationToken ct = default);
        Task<bool> DoctorHasConflictAsync(int doctorId, DateTime date, string timeSlot, CancellationToken ct = default);
        Task<bool> PatientHasConflictAsync(int patientId, DateTime date, string timeSlot, CancellationToken ct = default);
        Task<bool> PatientHasAppointmentOnDateAsync(int patientId, DateTime date, CancellationToken ct = default);
        Task<List<Appointment>> GetTodayAppointmentsAsync(int doctorId, CancellationToken ct = default);

    }
}
