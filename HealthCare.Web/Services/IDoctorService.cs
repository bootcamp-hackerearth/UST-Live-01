using HealthCare.Shared;
using HealthCare.Shared.DTOs.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HealthCare.Web.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<PagedResult<DoctorDto>> GetDoctorsAsync(
                string specialization,
                string searchTerm,
                bool orderByDescending,
                int pageNumber,
                int pageSize);

        Task<PagedResult<DoctorDto>> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateDoctorDto dto);
        Task<List<DoctorLookupDto>> GetDoctorsBySpecializationAsync(string specialization);
        Task<bool> UpdateAsync(UpdateDoctorDto dto);
        Task<bool> DeleteAsync(int id);

    }
}