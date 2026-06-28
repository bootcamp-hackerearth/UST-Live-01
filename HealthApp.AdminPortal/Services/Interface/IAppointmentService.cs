using HealthApp.AdminPortal.Models;
using HealthApp.Shared.Dtos;

namespace HealthApp.AdminPortal.Services.Interface
{
    public interface IAppointmentService
    {
        Task<ApiResult<PagedResultDto<AppointmentDto>>> GetAppointments(
            AppointmentFilterDto filter);

        Task<ApiResult<AppointmentDto>> GetById(int id);

        Task<ApiResult> Confirm(int id);

        Task<ApiResult> Complete(int id);

        Task<ApiResult> Cancel(int id, CancelAppointmentDto dto);

        Task<ApiResult<List<string>>> GetSlots(int doctorId, DateOnly date);
    }
}