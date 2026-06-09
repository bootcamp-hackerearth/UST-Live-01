using SharedClasses.Dtos;
using SharedClasses.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthcareWeb.Services
{
    public interface IDoctorApiService
    {
        Task<List<DoctorDto>> GetAllAsync();

        Task<List<DoctorDto>> GetAllActiveAsync();

        Task<DoctorDto> GetByIdAsync(int id);

        Task<List<DoctorDto>> SearchBySpecialisationAsync(Specialisation specialisation);

        Task<DoctorDto> AddAsync(CreateDoctorDto dto);

        Task<DoctorDto> UpdateAsync(int id, UpdateDoctorDto dto);

        Task<DoctorDto> DeactivateAsync(int id);

        Task<DoctorDto> ReactivateAsync(int id);
    }
}
