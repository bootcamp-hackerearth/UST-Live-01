using HealthAxisApp.Shared.DTOs;
using System.Collections.Generic;

namespace HealthAxisApp.Services
{
    public interface IDoctorService
    {
        IEnumerable<DoctorDto> GetAll(
            string specialisation = null,
            string searchText = null,
            bool activeOnly = false);

        DoctorDto GetById(int id);

        bool Create(
    DoctorDto dto,
    out string errorMessage,
    out int doctorId);

        bool Update(
            int id,
            DoctorDto dto,
            out string errorMessage);

        bool ToggleStatus(int id);
    }
}
