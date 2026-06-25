using HealthApp.AdminPortal.Models;
using HealthApp.Shared.Dtos;

namespace HealthApp.AdminPortal.Services.Interface
{
    public interface IPatientService
    {
        Task<ApiResult<List<PatientDto>>> GetAll();

        Task<ApiResult<PatientDto>> GetById(int id);

        Task<ApiResult<List<PatientDto>>> Search(string? name, string? email);
    }
}