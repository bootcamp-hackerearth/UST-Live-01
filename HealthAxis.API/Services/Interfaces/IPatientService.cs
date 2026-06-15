using HealthAxis.DTO.PatientDto;

namespace HealthAxis.API.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();

        Task<PatientDto?> GetByIdAsync(int id);

        Task<PatientDto> AddAsync(PatientDto patientDto);

        Task<PatientDto?> UpdateAsync( int id, PatientDto patientDto);
    }
}