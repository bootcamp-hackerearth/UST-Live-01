using HealthApp.AdminPortal.Models;
using HealthApp.AdminPortal.Services.Interface;
using HealthApp.Shared.Dtos;
using System.Net.Http.Json;

namespace HealthApp.AdminPortal.Services.Impl
{
    public class PatientService : BaseApiService, IPatientService
    {
        public PatientService(HttpClient http, ITokenService tokenService)
            : base(http, tokenService)
        {
        }

        public async Task<ApiResult<List<PatientDto>>> GetAll()
        {
            await AddAuthHeaderAsync();

            var response = await _http.GetAsync("api/patients");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);
                return ApiResult<List<PatientDto>>.Failure(message);
            }

            var patients = await response.Content.ReadFromJsonAsync<List<PatientDto>>();

            return ApiResult<List<PatientDto>>.Success(
                patients ?? new List<PatientDto>(),
                "Patients loaded successfully.");
        }

        public async Task<ApiResult<PatientDto>> GetById(int id)
        {
            await AddAuthHeaderAsync();

            var response = await _http.GetAsync($"api/patients/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);
                return ApiResult<PatientDto>.Failure(message);
            }

            var patient = await response.Content.ReadFromJsonAsync<PatientDto>();

            if (patient is null)
            {
                return ApiResult<PatientDto>.Failure("Patient details were not found.");
            }

            return ApiResult<PatientDto>.Success(patient, "Patient loaded successfully.");
        }

        public async Task<ApiResult<List<PatientDto>>> Search(string? name, string? email)
        {
            await AddAuthHeaderAsync();

            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(name))
            {
                queryParams.Add($"name={Uri.EscapeDataString(name)}");
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                queryParams.Add($"email={Uri.EscapeDataString(email)}");
            }

            var queryString = queryParams.Any()
                ? "?" + string.Join("&", queryParams)
                : string.Empty;

            var response = await _http.GetAsync($"api/patients/search{queryString}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);
                return ApiResult<List<PatientDto>>.Failure(message);
            }

            var patients = await response.Content.ReadFromJsonAsync<List<PatientDto>>();

            return ApiResult<List<PatientDto>>.Success(
                patients ?? new List<PatientDto>(),
                "Patients loaded successfully.");
        }
    }
}