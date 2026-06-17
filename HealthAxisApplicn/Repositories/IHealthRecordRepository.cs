using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Repositories
{
    public interface IHealthRecordRepository: IRepository<HealthRecord>
    {
        Task<List<HealthRecord>> GetRecordByPatientIDAsync(int patientId, CancellationToken ct = default);
        Task<List<HealthRecord>> GetRecordsByDoctorIDAsync(int doctorId, CancellationToken ct = default);
        Task<List<HealthRecord>> GetRecordsByPatientNameAsync(string patientName, CancellationToken ct = default);
        Task<List<HealthRecord>> GetRecordsByDoctorNameAsync(string doctorName, CancellationToken ct = default);


    }
}
