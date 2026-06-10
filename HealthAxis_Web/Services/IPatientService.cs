using HealthAxis.Shared.Dtos;
using System.Collections.Generic;

namespace HealthAxis.Api.Services
{
    public interface IPatientService
    {
        PatientDto Register(PatientDto dto);

        List<PatientDto> GetAllPatients();

        PatientDto GetById(int id);

        PatientDto Update(int id, PatientDto dto);
    }
}
