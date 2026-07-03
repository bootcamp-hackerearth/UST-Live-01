using HealthAxisApplicn.Dto.Doctors;
using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Repositories
{
    public interface IHealthRecordRepository: IRepository<HealthRecord>
    {
        Task<List<HealthRecord>> GetRecordsByPatientIdAsync(int patientId, CancellationToken ct = default);
        Task<List<HealthRecord>> GetRecordsByDoctorIdAsync(int doctorId, CancellationToken ct = default);
        Task<List<HealthRecord>> GetRecordsByPatientNameAsync(string patientName, CancellationToken ct = default);
        Task<List<HealthRecord>> GetRecordsByDoctorNameAsync(string doctorName, CancellationToken ct = default);
        Task<HealthRecord?> GetByAppointmentIdAsync(int appointmentId, CancellationToken ct = default);
        Task<bool> ExistsForAppointmentAsync(int appointmentId, CancellationToken ct = default);
        Task<List<DoctorPatientListDto>> GetDoctorPatientsAsync(int doctorId);
        Task<List<HealthRecord>> GetRecordsForDoctorPatientAsync( int doctorId,int patientId);


    }
}
