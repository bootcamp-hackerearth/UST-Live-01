using S3_HealthAxisApi.Models;

namespace S3_HealthAxisApi.Repository.Interface
{
    public interface IHealthRecordRepository
    {
        Task<HealthRecord?> GetByIdAsync(int id);

        Task<HealthRecord?> GetByAppointmentIdAsync(int appointmentId);

        Task<IEnumerable<HealthRecord>> GetByPatientIdAsync(int patientId);

        Task AddAsync(HealthRecord record);

        Task UpdateAsync(HealthRecord record);

        Task SaveChangesAsync();
    }
}