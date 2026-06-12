using HealthAxis.Shared.DTOs;
using System.Collections.Generic;

namespace HealthAxis.Api.Services.Interfaces
{
    public interface IDoctorService
    {
        IEnumerable<DoctorDto> GetAll(
            string specialisation = null,
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
        IEnumerable<DoctorDto> Search(string searchValue);
    }
}