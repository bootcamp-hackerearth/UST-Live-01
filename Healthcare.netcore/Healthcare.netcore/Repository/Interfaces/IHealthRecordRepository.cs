using HealthAxis.API.Models;

namespace HealthAxis.API.Repositories.Interfaces
{
    public interface IHealthRecordRepository : IRepository<HealthRecord>
    {

        Task<IEnumerable<HealthRecord>> GetByPatientIdAsync(int patientId);

        Task<IEnumerable<HealthRecord>> GetByDoctorIdAsync(int doctorId);
        Task<IEnumerable<HealthRecord>> GetByAppointmentIdAsync(int appointmentId);

    }
}