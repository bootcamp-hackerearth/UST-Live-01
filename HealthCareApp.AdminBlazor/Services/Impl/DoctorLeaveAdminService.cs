using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using HealthCareApp.AdminBlazor.Services.Interfaces;
using HealthCareApp.Shared.Dtos.DoctorLeaves;
using Microsoft.JSInterop;

namespace HealthCareApp.AdminBlazor.Services.Implementations
{
    public class DoctorLeaveAdminService : IDoctorLeaveAdminService
    {
        private const string TokenStorageKey = "token";

        private readonly HttpClient httpClient;

        private readonly IJSRuntime jsRuntime;

        public DoctorLeaveAdminService(
            HttpClient httpClient,
            IJSRuntime jsRuntime)
        {
            this.httpClient = httpClient;
            this.jsRuntime = jsRuntime;
        }

        public async Task<List<DoctorLeaveDto>> GetDoctorLeavesAsync(
            int doctorId)
        {
            await AttachBearerTokenAsync();

            var response = await httpClient.GetAsync(
                $"api/DoctorLeaves/doctor/{doctorId}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException(
                    "Your admin session is not authorized. Please login again.");
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedAccessException(
                    "You do not have permission to view doctor leave history.");
            }

            response.EnsureSuccessStatusCode();

            var leaves =
                await response.Content.ReadFromJsonAsync<List<DoctorLeaveDto>>();

            return leaves ?? new List<DoctorLeaveDto>();
        }

        private async Task AttachBearerTokenAsync()
        {
            var token = await jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                TokenStorageKey);

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new UnauthorizedAccessException(
                    "Admin token was not found. Please login again.");
            }

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);
        }
    }
}