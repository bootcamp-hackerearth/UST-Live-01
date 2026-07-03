using HealthAxis.Shared.DTO.AdminDtos;
using System.Net.Http.Json;
using System.Text.Json;

namespace HealthAxis_Admin.Services
{
    public sealed class AppointmentReportAdminService
    {
        private const string AppointmentDetailsEndpoint =
            "api/admin/reports/appointments/details";

        private const string AppointmentStatusEndpoint =
            "api/admin/appointments";

        private readonly HttpClient _httpClient;

        public AppointmentReportAdminService(HttpClient httpClient)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            _httpClient = httpClient;
        }

        public async Task<List<AdminAppointmentDetailDto>> GetAppointmentDetailsAsync()
        {
            var reports = await _httpClient.GetFromJsonAsync<List<AdminAppointmentDetailDto>>(
                AppointmentDetailsEndpoint);

            return reports ?? new List<AdminAppointmentDetailDto>();
        }

        public Task<(bool Success, string Message)> ConfirmAppointmentAsync(
            int appointmentId)
        {
            return UpdateAppointmentStatusAsync(
                appointmentId,
                "Confirmed",
                "Appointment confirmed successfully.");
        }

        public Task<(bool Success, string Message)> CancelAppointmentAsync(
            int appointmentId)
        {
            return UpdateAppointmentStatusAsync(
                appointmentId,
                "Cancelled",
                "Appointment cancelled successfully.");
        }

        private async Task<(bool Success, string Message)> UpdateAppointmentStatusAsync(
            int appointmentId,
            string status,
            string successMessage)
        {
            if (appointmentId <= 0)
            {
                return (false, "Invalid appointment selected.");
            }

            var statusDto = new AdminUpdateAppointmentStatusDto
            {
                Status = status
            };

            using var response = await _httpClient.PutAsJsonAsync(
                $"{AppointmentStatusEndpoint}/{appointmentId}/status",
                statusDto);

            if (response.IsSuccessStatusCode)
            {
                return (true, successMessage);
            }

            var errorMessage = await ReadErrorMessageAsync(response);

            return (false, errorMessage);
        }

        private static async Task<string> ReadErrorMessageAsync(
            HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return "Request failed.";
            }

            try
            {
                using var document = JsonDocument.Parse(content);

                if (document.RootElement.TryGetProperty("message", out var messageElement))
                {
                    return messageElement.GetString() ?? "Request failed.";
                }

                if (document.RootElement.TryGetProperty("title", out var titleElement))
                {
                    return titleElement.GetString() ?? "Request failed.";
                }
            }
            catch (JsonException)
            {
                return content;
            }

            return "Request failed.";
        }
    }
}