using HealthApp.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.API.Service.Interface
{
    public interface IPatientService
    {
        Task RegisterPatient(PatientDto patientDto);

        Task<PatientDto> GetPatientById(int id);

        Task<List<PatientDto>> GetAll();

        Task UpdatePatientById(int id, PatientDto patientDto);
    }
}