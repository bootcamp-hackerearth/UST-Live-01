using HealthCare.Shared;
using HealthCare.Shared.DTOs.Doctor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare.Web.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<PagedResult<DoctorDto>> GetDoctorsAsync(string specialization, string searchTerm, bool orderByDescending, int pageNumber, int pageSize);
        Task<DoctorDto> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateDoctorDto dto);
        Task<bool> UpdateAsync(DoctorDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<DoctorDto>> GetAllAsync();
    }
}