using HealthApp.AdminPortal.Models;
using HealthApp.AdminPortal.Services.Interface;
using HealthApp.Shared.Dtos;
using System.Net.Http.Json;

namespace HealthApp.AdminPortal.Services.Impl
{
    public class AppointmentService : BaseApiService, IAppointmentService
    {
        public AppointmentService(HttpClient http, ITokenService tokenService)
            : base(http, tokenService)
        {
        }

        public async Task<ApiResult<PagedResultDto<AppointmentDto>>> GetAppointments(
            AppointmentFilterDto filter)
        {
            await AddAuthHeaderAsync();

            filter ??= new AppointmentFilterDto();

            var queryParams = new List<string>
            {
                $"pageNumber={filter.PageNumber}",
                $"pageSize={filter.PageSize}"
            };

            if (filter.DoctorId.HasValue)
            {
                queryParams.Add($"doctorId={filter.DoctorId.Value}");
            }

            if (filter.PatientId.HasValue)
            {
                queryParams.Add($"patientId={filter.PatientId.Value}");
            }

            if (filter.Status.HasValue)
            {
                queryParams.Add($"status={filter.Status.Value}");
            }

            if (filter.Date.HasValue)
            {
                queryParams.Add($"date={filter.Date.Value:yyyy-MM-dd}");
            }

            if (filter.FromDate.HasValue)
            {
                queryParams.Add($"fromDate={filter.FromDate.Value:yyyy-MM-dd}");
            }

            if (filter.ToDate.HasValue)
            {
                queryParams.Add($"toDate={filter.ToDate.Value:yyyy-MM-dd}");
            }

            if (filter.OnlyUpcoming)
            {
                queryParams.Add("onlyUpcoming=true");
            }

            var queryString = $"?{string.Join("&", queryParams)}";

            var response = await _http.GetAsync($"api/appointments{queryString}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);

                return ApiResult<PagedResultDto<AppointmentDto>>.Failure(message);
            }

            var pagedAppointments =
                await response.Content.ReadFromJsonAsync<PagedResultDto<AppointmentDto>>();

            return ApiResult<PagedResultDto<AppointmentDto>>.Success(
                pagedAppointments ?? new PagedResultDto<AppointmentDto>(),
                "Appointments loaded successfully.");
        }

        public async Task<ApiResult<AppointmentDto>> GetById(int id)
        {
            await AddAuthHeaderAsync();

            var response = await _http.GetAsync($"api/appointments/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);

                return ApiResult<AppointmentDto>.Failure(message);
            }

            var appointment =
                await response.Content.ReadFromJsonAsync<AppointmentDto>();

            if (appointment is null)
            {
                return ApiResult<AppointmentDto>.Failure("Appointment was not found.");
            }

            return ApiResult<AppointmentDto>.Success(
                appointment,
                "Appointment loaded successfully.");
        }

        public async Task<ApiResult> Confirm(int id)
        {
            await AddAuthHeaderAsync();

            var response = await _http.PostAsync(
                $"api/appointments/{id}/confirm",
                null);

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);

                return ApiResult.Failure(message);
            }

            return ApiResult.Success("Appointment confirmed successfully.");
        }

        public async Task<ApiResult> Complete(int id)
        {
            await AddAuthHeaderAsync();

            var response = await _http.PostAsync(
                $"api/appointments/{id}/complete",
                null);

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);

                return ApiResult.Failure(message);
            }

            return ApiResult.Success("Appointment completed successfully.");
        }

        public async Task<ApiResult> Cancel(int id, CancelAppointmentDto dto)
        {
            await AddAuthHeaderAsync();

            var response = await _http.PostAsJsonAsync(
                $"api/appointments/{id}/cancel",
                dto);

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);

                return ApiResult.Failure(message);
            }

            return ApiResult.Success("Appointment cancelled successfully.");
        }

        public async Task<ApiResult<List<string>>> GetSlots(int doctorId, DateOnly date)
        {
            await AddAuthHeaderAsync();

            var response = await _http.GetAsync(
                $"api/appointments/slots?doctorId={doctorId}&date={date:yyyy-MM-dd}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await ReadErrorMessageAsync(response);

                return ApiResult<List<string>>.Failure(message);
            }

            var slots =
                await response.Content.ReadFromJsonAsync<List<string>>();

            return ApiResult<List<string>>.Success(
                slots ?? new List<string>(),
                "Slots loaded successfully.");
        }
    }
}