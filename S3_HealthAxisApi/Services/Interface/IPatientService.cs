using S3_HealthAxisApi.Models;

namespace S3_HealthAxisApi.Services.Interface
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAllPatientsAsync();

        Task<Patient?> GetPatientByIdAsync(int id);

        Task<IEnumerable<Patient>> SearchPatientsAsync(string name);

        Task AddPatientAsync(Patient patient);

        Task UpdatePatientAsync(int id, Patient patient);
    }
}
