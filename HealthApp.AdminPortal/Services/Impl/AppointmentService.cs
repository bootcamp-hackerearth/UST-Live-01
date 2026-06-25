using HealthApp.AdminPortal.Models;
using HealthApp.AdminPortal.Services.Interface;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
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
            int? doctorId = null,
            int? patientId = null,
            AppointmentStatus? status = null,
            DateOnly? date = null,
            DateOnly? fromDate = null,
            DateOnly? toDate = null,
            bool onlyUpcoming = false,
            int pageNumber = 1,
            int pageSize = 10)
        {
            await AddAuthHeaderAsync();

            var queryParams = new List<string>
            {
                $"pageNumber={pageNumber}",
                $"pageSize={pageSize}"
            };

            if (doctorId.HasValue)
            {
                queryParams.Add($"doctorId={doctorId.Value}");
            }

            if (patientId.HasValue)
            {
                queryParams.Add($"patientId={patientId.Value}");
            }

            if (status.HasValue)
            {
                queryParams.Add($"status={status.Value}");
            }

            if (date.HasValue)
            {
                queryParams.Add($"date={date.Value:yyyy-MM-dd}");
            }

            if (fromDate.HasValue)
            {
                queryParams.Add($"fromDate={fromDate.Value:yyyy-MM-dd}");
            }

            if (toDate.HasValue)
            {
                queryParams.Add($"toDate={toDate.Value:yyyy-MM-dd}");
            }

            if (onlyUpcoming)
            {
                queryParams.Add("onlyUpcoming=true");
            }

            var queryString = "?" + string.Join("&", queryParams);

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