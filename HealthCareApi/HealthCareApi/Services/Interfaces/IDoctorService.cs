using HealthCare.Shared;
using System.Threading.Tasks;

namespace HealthCareApi.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<Doctor> GetDoctorByIdAsync(int id);
        Task<PagedResult<Doctor>> GetFilteredDoctorsAsync(
            string specialization = null,
            string searchTerm = null,
            bool orderByDescending = false,
            int pageNumber = 1,
            int pageSize = 10);

        Task<Doctor> AddDoctorAsync(Doctor doctor);
        Task<Doctor> UpdateDoctorAsync(Doctor updatedDoctor);
        Task<bool> DeleteDoctorAsync(int id);
    }
}