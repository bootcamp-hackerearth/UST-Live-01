using HealthApp.AdminPortal.Models;
using HealthApp.AdminPortal.Services.Interface;
using HealthApp.Shared.Dtos;
using System.Net.Http.Json;

namespace HealthApp.AdminPortal.Services.Impl
{
    public class PatientService : BaseApiService, IPatientService
    {
        public PatientService(
            HttpClient http,
            ITokenService tokenService)
            : base(http, tokenService)
        {
        }

        public async Task<ApiResult<PagedResultDto<PatientDto>>> GetAll(
            int pageNumber = 1,
            int pageSize = 10)
        {
            await AddAuthHeaderAsync();

            var response = await _http.GetAsync(
                $"api/patients?pageNumber={pageNumber}&pageSize={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);

                return ApiResult<PagedResultDto<PatientDto>>.Failure(message);
            }

            var patients = await response.Content
                .ReadFromJsonAsync<PagedResultDto<PatientDto>>();

            return ApiResult<PagedResultDto<PatientDto>>.Success(
                patients ?? new PagedResultDto<PatientDto>(),
                "Patients loaded successfully.");
        }

        public async Task<ApiResult<PatientDto>> GetById(
            int id)
        {
            await AddAuthHeaderAsync();

            var response = await _http.GetAsync(
                $"api/patients/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);

                return ApiResult<PatientDto>.Failure(message);
            }

            var patient = await response.Content
                .ReadFromJsonAsync<PatientDto>();

            if (patient is null)
            {
                return ApiResult<PatientDto>.Failure(
                    "Patient details were not found.");
            }

            return ApiResult<PatientDto>.Success(
                patient,
                "Patient loaded successfully.");
        }

        public async Task<ApiResult<PagedResultDto<PatientDto>>> Search(
            string? name,
            string? email,
            int pageNumber = 1,
            int pageSize = 10)
        {
            await AddAuthHeaderAsync();

            var queryParams = new List<string>
            {
                $"pageNumber={pageNumber}",
                $"pageSize={pageSize}"
            };

            if (!string.IsNullOrWhiteSpace(name))
            {
                queryParams.Add(
                    $"name={Uri.EscapeDataString(name)}");
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                queryParams.Add(
                    $"email={Uri.EscapeDataString(email)}");
            }

            var queryString = string.Join("&", queryParams);

            var response = await _http.GetAsync(
                $"api/patients/search?{queryString}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);

                return ApiResult<PagedResultDto<PatientDto>>.Failure(message);
            }

            var patients = await response.Content
                .ReadFromJsonAsync<PagedResultDto<PatientDto>>();

            return ApiResult<PagedResultDto<PatientDto>>.Success(
                patients ?? new PagedResultDto<PatientDto>(),
                "Patients loaded successfully.");
        }
    }
}