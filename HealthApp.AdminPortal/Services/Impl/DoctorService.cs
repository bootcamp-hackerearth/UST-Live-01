using HealthApp.AdminPortal.Models;
using HealthApp.AdminPortal.Services.Interface;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
using System.Net.Http.Json;

namespace HealthApp.AdminPortal.Services.Impl
{
    public class DoctorService : BaseApiService, IDoctorService
    {
        public DoctorService(
            HttpClient http,
            ITokenService tokenService)
            : base(http, tokenService)
        {
        }

        public async Task<ApiResult<PagedResultDto<DoctorDto>>> GetAll(
            int pageNumber = 1,
            int pageSize = 10)
        {
            await AddAuthHeaderAsync();

            var response = await _http.GetAsync(
                $"api/admin/doctors?pageNumber={pageNumber}&pageSize={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);

                return ApiResult<PagedResultDto<DoctorDto>>.Failure(message);
            }

            var doctors = await response.Content
                .ReadFromJsonAsync<PagedResultDto<DoctorDto>>();

            return ApiResult<PagedResultDto<DoctorDto>>.Success(
                doctors ?? new PagedResultDto<DoctorDto>(),
                "Doctors loaded successfully.");
        }

        public async Task<ApiResult<PagedResultDto<DoctorDto>>> Search(
            string? search,
            SpecialisationType? specialisation,
            bool? isActive,
            int pageNumber = 1,
            int pageSize = 10)
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
                    $"search={Uri.EscapeDataString(search)}");
            }

            if (specialisation.HasValue)
            {
                queryParams.Add(
                    $"specialisation={specialisation.Value}");
            }

            if (isActive.HasValue)
            {
                queryParams.Add(
                    $"isActive={isActive.Value.ToString().ToLower()}");
            }

            var queryString = string.Join("&", queryParams);

            var response = await _http.GetAsync(
                $"api/admin/doctors?{queryString}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);

                return ApiResult<PagedResultDto<DoctorDto>>.Failure(message);
            }

            var doctors = await response.Content
                .ReadFromJsonAsync<PagedResultDto<DoctorDto>>();

            return ApiResult<PagedResultDto<DoctorDto>>.Success(
                doctors ?? new PagedResultDto<DoctorDto>(),
                "Doctors loaded successfully.");
        }

        public async Task<ApiResult> Update(
            int id,
            DoctorCreateDto dto)
        {
            await AddAuthHeaderAsync();

            var response = await _http.PutAsJsonAsync(
                $"api/admin/doctors/{id}",
                dto);

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);

                return ApiResult.Failure(message);
            }

            return ApiResult.Success(
                "Doctor updated successfully.");
        }

        public async Task<ApiResult> ChangeStatus(
            int id,
            bool isActive)
        {
            await AddAuthHeaderAsync();

            var response = await _http.PatchAsync(
                $"api/admin/doctors/{id}/status?isActive={isActive}",
                null);

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);

                return ApiResult.Failure(message);
            }

            return ApiResult.Success(
                "Doctor status updated successfully.");
        }
    }
}
