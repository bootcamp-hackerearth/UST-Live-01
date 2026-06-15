using S3_HealthAxisApi.Models;

namespace S3_HealthAxisApi.Services.Interface
{
    public interface IDoctorService
    {
        Task<IEnumerable<Doctor>> GetAllDoctorsAsync();

        Task<IEnumerable<Doctor>> GetDoctorsBySpecialisationAsync(int specialisation);

        Task<IEnumerable<Doctor>> GetActiveDoctorsBySpecialisationAsync(int specialisation);

        Task<Doctor?> GetDoctorByIdAsync(int id);

        Task AddDoctorAsync(Doctor doctor);

        Task UpdateDoctorAsync(int id, Doctor doctor);
    }
}
