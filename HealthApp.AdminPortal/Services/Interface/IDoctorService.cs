using HealthApp.AdminPortal.Models;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;

namespace HealthApp.AdminPortal.Services.Interface
{
    public interface IDoctorService
    {
        Task<ApiResult<List<DoctorDto>>> GetAll();

        Task<ApiResult<List<DoctorDto>>> Search(
            string? search,
            SpecialisationType? specialisation,
            bool? isActive);

        Task<ApiResult> Update(int id, DoctorCreateDto dto);

        Task<ApiResult> ChangeStatus(int id, bool isActive);
    }
}