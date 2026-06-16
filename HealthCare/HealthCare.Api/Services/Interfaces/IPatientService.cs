using HealthCare.Api.DTOs.Patient;

namespace HealthCare.Api.Services.Interfaces
{
    public interface IPatientService
    {
        Task<PatientListDto> GetByIdAsync(int id);
        Task<IEnumerable<PatientListDto>> GetAllAsync();

        Task AddAsync(CreatePatientDto dto);

        Task UpdateAsync(int id,UpdatePatientDto dto);
        Task DeleteAsync(int id);
    }
}
