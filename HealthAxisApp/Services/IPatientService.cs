using HealthAxisApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxisApp.Services
{
    public interface IPatientService
    {
        IEnumerable<PatientDto> GetAll(string insuranceStatus = null, string searchText = null);

        PatientDto GetById(int id);

        bool Create(
    PatientDto dto,
    out string errorMessage,
    out int patientId);

        bool Update(
            int id,
            PatientDto dto,
            out string errorMessage);

        bool Delete(int id);
    }
}
