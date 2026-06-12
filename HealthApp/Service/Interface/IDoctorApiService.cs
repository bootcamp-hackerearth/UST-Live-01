using HealthApp.Shared.DTOs;
using HealthApp.Shared.Constant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.Service.Interface
{
    public interface IDoctorApiService
    {
        Task<List<DoctorDto>> GetAll();
        Task<DoctorDto> GetById(int id);
        Task Create(DoctorDto dto);
        Task<List<DoctorDto>> SearchBySpecialisation(SpecialisationType type);
        Task ToggleStatus(int id);
    }
}