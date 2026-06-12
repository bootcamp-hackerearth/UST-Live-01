using HealthAxisApp.Shared.DTOs;
using System.Collections.Generic;

namespace HealthAxisAppMVC.Services.Interfaces
{
    public interface IDoctorMvcService
    {
    IEnumerable<DoctorDto> GetAll(
    string specialisation = null,
    string searchText = null,
    bool activeOnly = false);

        DoctorDto GetById(int id);

        bool Create(
    DoctorDto dto,
    out string error,
    out int doctorId);

        bool Update(
            DoctorDto dto,
            out string error);

        bool ToggleStatus(
            int id,
            out string error);
    }
}
