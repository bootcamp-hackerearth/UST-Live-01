using HealthApp.AdminPortal.Models;
using HealthApp.AdminPortal.Services.Interface;
using HealthApp.Shared.Dtos;
using System.Net.Http.Json;

namespace HealthApp.AdminPortal.Services.Impl
{
    public class ReportService : BaseApiService, IReportService
    {
        public ReportService(HttpClient http, ITokenService tokenService)
            : base(http, tokenService)
        {
        }

        public async Task<ApiResult<List<AppointmentReportDto>>> GetAppointmentReports()
        {
            await AddAuthHeaderAsync();

            var response = await _http.GetAsync("api/admin/reports/appointments");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);
                return ApiResult<List<AppointmentReportDto>>.Failure(message);
            }

            var reports = await response.Content.ReadFromJsonAsync<List<AppointmentReportDto>>();

            return ApiResult<List<AppointmentReportDto>>.Success(
                reports ?? new List<AppointmentReportDto>(),
                "Reports loaded successfully.");
        }
    }
}