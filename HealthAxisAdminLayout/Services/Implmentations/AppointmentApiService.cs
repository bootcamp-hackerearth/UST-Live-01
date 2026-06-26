using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.Enums;
using HealthAxisAdminLayout.Services.Interfaces;

namespace HealthAxisAdminLayout.Services.Implementations
{
    public class AppointmentApiService : IAppointmentApiService
    {
        private readonly HttpClient _http;

        public AppointmentApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<AppointmentResponseDTO>> GetAppointmentsAsync()
        {
            var result = await _http.GetFromJsonAsync<List<AppointmentResponseDTO>>(
                "api/appointment"
            );

            return result ?? new List<AppointmentResponseDTO>();
        }

        public async Task<List<AppointmentResponseDTO>> GetAppointmentsByDoctorAsync(int doctorId)
        {
            var response = await _http.GetAsync(
                $"api/appointment/doctor/{doctorId}"
            );

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<AppointmentResponseDTO>>();

                return result ?? new List<AppointmentResponseDTO>();
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                var allAppointments = await GetAppointmentsAsync();

                return allAppointments
                    .Where(a => a.DoctorId == doctorId)
                    .ToList();
            }

            var error = await response.Content.ReadAsStringAsync();

            throw new HttpRequestException(
                $"Failed to load appointments for doctor {doctorId}. Status: {(int)response.StatusCode}. Response: {error}"
            );
        }

        public async Task<PagedResponseDTO<AppointmentResponseDTO>> GetAppointmentsPagedAsync(
            int pageNumber,
            int pageSize,
            string? search,
            AppointmentStatus? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            var query = new List<string>
            {
                $"pageNumber={pageNumber}",
                $"pageSize={pageSize}"
            };

            if (!string.IsNullOrWhiteSpace(search))
            {
                query.Add($"search={Uri.EscapeDataString(search)}");
            }

            if (status.HasValue)
            {
                query.Add($"status={Uri.EscapeDataString(status.Value.ToString())}");
            }

            if (startDate.HasValue)
            {
                query.Add($"startDate={startDate.Value:yyyy-MM-dd}");
            }

            if (endDate.HasValue)
            {
                query.Add($"endDate={endDate.Value:yyyy-MM-dd}");
            }

            var url = $"api/appointment/paged?{string.Join("&", query)}";

            var result = await _http.GetFromJsonAsync<PagedResponseDTO<AppointmentResponseDTO>>(url);

            return result ?? new PagedResponseDTO<AppointmentResponseDTO>();
        }

        public async Task<List<AppointmentResponseDTO>> FilterAppointmentsAsync(
            AppointmentStatus? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            var query = new List<string>();

            if (status.HasValue)
            {
                query.Add($"status={status.Value}");
            }

            if (startDate.HasValue)
            {
                query.Add($"startDate={startDate.Value:yyyy-MM-dd}");
            }

            if (endDate.HasValue)
            {
                query.Add($"endDate={endDate.Value:yyyy-MM-dd}");
            }

            var url = query.Count == 0
                ? "api/appointment/filter"
                : $"api/appointment/filter?{string.Join("&", query)}";

            var result = await _http.GetFromJsonAsync<List<AppointmentResponseDTO>>(url);

            return result ?? new List<AppointmentResponseDTO>();
        }

        public async Task<bool> ConfirmAppointmentAsync(int appointmentId)
        {
            var response = await _http.PostAsync(
                $"api/appointment/confirm/{appointmentId}",
                null
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CancelAppointmentAsync(int appointmentId, string reason)
        {
            var encodedReason = Uri.EscapeDataString(reason);

            var response = await _http.PostAsync(
                $"api/appointment/cancel/{appointmentId}?reason={encodedReason}",
                null
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAppointmentAsync(int appointmentId)
        {
            var response = await _http.DeleteAsync(
                $"api/appointment/{appointmentId}"
            );

            return response.IsSuccessStatusCode;
        }
    }
}