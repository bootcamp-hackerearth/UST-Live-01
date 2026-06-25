using HealthApp.AdminPortal.Models;
using HealthApp.Shared.Dtos;

namespace HealthApp.AdminPortal.Services.Interface
{
    public interface IReportService
    {
        Task<ApiResult<List<AppointmentReportDto>>> GetAppointmentReports();
    }
}