using HealthCare.Api.DTOs;
using HealthCare.Api.DTOs.Doctor;


namespace HealthCare.Api.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<DoctorListDto?> GetByIdAsync(int id);
        Task<PagedResult<DoctorListDto>> GetAllAsync(DoctorFilter filter);

        Task AddAsync(CreateDoctorDto dto);

        Task UpdateAsync(int id, UpdateDoctorDto dto);
        Task DeleteAsync(int id);
    }
}
