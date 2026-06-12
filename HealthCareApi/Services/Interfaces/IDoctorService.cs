using HealthCare.Shared;
using HealthCare.Shared.DTOs.Doctor;
using System.Collections.Generic;
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
        Task<Doctor> UpdateDoctorAsync(Doctor updatedDoctor);
        Task<List<Doctor>> GetBySpecializationAsync(string specialization);
        Task<bool> DeleteDoctorAsync(int id);
        Task<Doctor> AddDoctorAsync(CreateDoctorDto dto);
    }
}