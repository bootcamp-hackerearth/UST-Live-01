using HealthApp.AdminPortal.Models;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;

namespace HealthApp.AdminPortal.Services.Interface
{
    public interface IDoctorService
    {
        Task<ApiResult<PagedResultDto<DoctorDto>>> GetAll(
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResult<PagedResultDto<DoctorDto>>> Search(
            string? search,
            SpecialisationType? specialisation,
            bool? isActive,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResult> Update(
            int id,
            DoctorCreateDto dto);

        Task<ApiResult> ChangeStatus(
            int id,
            bool isActive);
    }
}