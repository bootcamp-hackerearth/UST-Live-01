using System.Collections.Generic;
using System.Threading.Tasks;
using HealthAxis.Shared.Dtos;

public interface IPatientApiService
{
    Task<List<PatientDto>> GetAllAsync();

    Task<PatientDto> GetByIdAsync(int id);

    Task AddAsync(PatientDto patientDto);

    Task UpdateAsync(int id, PatientDto patientDto);
}