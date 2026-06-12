using HealthAxis.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAxis.Web.Services
{
    public interface IDoctorApiService
    {
        Task<List<DoctorDto>> GetAll(Specialisation? specialisation);

        Task<DoctorDto> GetById(int id);

        Task<ApiResponseDto> Create(CreateDoctorDto dto);

        Task<ApiResponseDto> Update(int id, UpdateDoctorDto dto);

        Task ToggleStatus(int id);
    }
}