using HealthApp.AdminPortal.Models;
using HealthApp.AdminPortal.Services.Interface;
using HealthApp.Shared.Dtos;
using System.Net.Http.Json;

namespace HealthApp.AdminPortal.Services.Impl
{
    public class AdminDoctorLeaveService
        : BaseApiService, IAdminDoctorLeaveService
    {
        public AdminDoctorLeaveService(
            HttpClient http,
            ITokenService tokenService)
            : base(http, tokenService)
        {
        }

        public async Task<ApiResult<List<AdminDoctorLeaveDto>>> GetDoctorLeaves(
            string? search,
            string? status,
            DateOnly? fromDate,
            DateOnly? toDate,
            int pageNumber = 1,
            int pageSize = 100)
        {
            await AddAuthHeaderAsync();

            var queryParams = new List<string>
            {
                $"pageNumber={pageNumber}",
                $"pageSize={pageSize}"
            };

            if (!string.IsNullOrWhiteSpace(search))
            {
                queryParams.Add(
                    $"search={Uri.EscapeDataString(search.Trim())}");
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                queryParams.Add(
                    $"status={Uri.EscapeDataString(status)}");
            }

            if (fromDate.HasValue)
            {
                queryParams.Add(
                    $"fromDate={fromDate.Value:yyyy-MM-dd}");
            }

            if (toDate.HasValue)
            {
                queryParams.Add(
                    $"toDate={toDate.Value:yyyy-MM-dd}");
            }

            var queryString = string.Join("&", queryParams);

            var response = await _http.GetAsync(
                $"api/admin/doctor-leaves?{queryString}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);

                return ApiResult<List<AdminDoctorLeaveDto>>
                    .Failure(message);
            }

            var leaves = await response.Content
                .ReadFromJsonAsync<List<AdminDoctorLeaveDto>>();

            return ApiResult<List<AdminDoctorLeaveDto>>.Success(
                leaves ?? new List<AdminDoctorLeaveDto>(),
                "Doctor leaves loaded successfully.");
        }
    }
}