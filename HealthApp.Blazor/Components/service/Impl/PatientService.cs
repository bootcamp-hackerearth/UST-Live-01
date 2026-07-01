using HealthApp.Blazor.Components.service.Interface;
using HealthApp.Shared.Dto;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HealthApp.Blazor.Components.Services
{
    public class PatientService : IPatientService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public PatientService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private void AddAuthHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(_authService.Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _authService.Token);
            }
        }

        private class ErrorResponse
        {
            public string Message { get; set; } = "";
        }

        private async Task<string> ReadError(HttpResponseMessage response)
        {
            try
            {
                var err = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                return err?.Message ?? "Something went wrong";
            }
            catch
            {
                return await response.Content.ReadAsStringAsync() ?? "Unknown error";
            }
        }

        public async Task<PagedResponse<PatientDto>> GetPagedPatientsAsync(
            int pageNumber,
            int pageSize,
            string? search = null)
        {
            try
            {
                AddAuthHeader();

                var url = $"api/patientapi/paged?pageNumber={pageNumber}&pageSize={pageSize}";

                if (!string.IsNullOrWhiteSpace(search))
                {
                    url += $"&search={search}";
                }

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(await ReadError(response));
                    return new PagedResponse<PatientDto>();
                }

                return await response.Content
                    .ReadFromJsonAsync<PagedResponse<PatientDto>>()
                       ?? new PagedResponse<PatientDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new PagedResponse<PatientDto>();
            }
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int id)
        {
            try
            {
                AddAuthHeader();

                var response = await _httpClient.GetAsync($"api/patientapi/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(await ReadError(response));
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<PatientDto>();
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> GetPatientCountAsync()
        {
            var result = await GetPagedPatientsAsync(1, 1);
            return result.TotalRecords;
        }
    }
}