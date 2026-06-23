
using HealthApp.API.Models;
using HealthApp.Shared.Enums;

namespace HealthApp.API.Repository.Interface;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<List<Appointment>> GetByPatientIdAsync(int patientId, CancellationToken ct = default);
    Task<List<Appointment>> GetByDoctorIdAsync(int doctorId, CancellationToken ct = default);
    Task<List<Appointment>> GetByStatusAsync(AppointmentStatus status, CancellationToken ct = default);
    Task<List<Appointment>> GetTodayConfirmedAppointmentsByDoctorIdAsync(int doctorId, CancellationToken ct = default);
    Task<bool> IsSlotBookedAsync(int doctorId, DateTime date, string timeSlot, CancellationToken ct = default);
    Task<bool> PatientHasActiveAppointmentWithDoctorOnDateAsync(
        int patientId,
        int doctorId,
        DateTime date,
        CancellationToken ct = default);
    Task<bool> PatientHasActiveAppointmentOnDateAndSlotAsync(
        int patientId,
        DateTime date,
        string timeSlot,
        CancellationToken ct = default);
    Task<Appointment?> GetByIdWithDetailsAsync(int appointmentId, CancellationToken ct = default);
    
}