using HealthApp.AdminPortal.Models;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;

namespace HealthApp.AdminPortal.Services.Interface
{
    public interface IAppointmentService
    {
        Task<ApiResult<List<AppointmentDto>>> GetAppointments(
            int? doctorId = null,
            int? patientId = null,
            AppointmentStatus? status = null,
            DateOnly? date = null,
            DateOnly? fromDate = null,
            DateOnly? toDate = null,
            bool onlyUpcoming = false);

        Task<ApiResult<AppointmentDto>> GetById(int id);

        Task<ApiResult> Confirm(int id);

        Task<ApiResult> Complete(int id);

        Task<ApiResult> Cancel(int id, CancelAppointmentDto dto);

        Task<ApiResult<List<string>>> GetSlots(int doctorId, DateOnly date);
    }
}