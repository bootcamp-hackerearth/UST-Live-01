using HealthApp.AdminPortal.Models;
using HealthApp.Shared.Dtos;

namespace HealthApp.AdminPortal.Services.Interface
{
    public interface IPatientService
    {
        Task<ApiResult<PagedResultDto<PatientDto>>> GetAll(
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResult<PatientDto>> GetById(
            int id);

        Task<ApiResult<PagedResultDto<PatientDto>>> Search(
            string? name,
            string? email,
            int pageNumber = 1,
            int pageSize = 10);
    }
}