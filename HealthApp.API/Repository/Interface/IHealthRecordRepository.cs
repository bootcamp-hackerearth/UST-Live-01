using HealthApp.API.Models;
namespace HealthApp.API.Repository.Interface;
public interface IHealthRecordRepository : IRepository<HealthRecord>
{
    Task<List<HealthRecord>> GetByPatientIdAsync(int patientId, CancellationToken ct = default);
    Task<List<HealthRecord>> GetByDoctorIdAsync(int doctorId, CancellationToken ct = default);
    Task<List<HealthRecord>> GetByAppointmentIdAsync(int appointmentId, CancellationToken ct = default);
    Task<bool> ExistsByAppointmentIdAsync(int appointmentId, CancellationToken ct = default);
}
