using HealthAxis.Shared.DTO.AdminDtos;
using System.Net.Http.Json;
using System.Text.Json;

namespace HealthAxis_Admin.Services
{
    public sealed class AppointmentReportAdminService
    {
        private const int RequestTimeoutSeconds = 20;

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
            using var cancellationTokenSource = CreateTimeoutToken();

            var reports = await _httpClient.GetFromJsonAsync<List<AdminAppointmentDetailDto>>(
                AppointmentDetailsEndpoint,
                cancellationTokenSource.Token);

            return reports ?? new List<AdminAppointmentDetailDto>();
        }

        public Task<(bool Success, string Message)> ConfirmAppointmentAsync(
            int appointmentId)
        {
            return UpdateAppointmentStatusAsync(
                appointmentId,
                "Confirmed",
                null,
                "Appointment confirmed successfully.");
        }

        public Task<(bool Success, string Message)> CancelAppointmentAsync(
            int appointmentId)
        {
            return UpdateAppointmentStatusAsync(
                appointmentId,
                "Cancelled",
                null,
                "Appointment cancelled successfully.");
        }

        private async Task<(bool Success, string Message)> UpdateAppointmentStatusAsync(
            int appointmentId,
            string status,
            string? cancellationReason,
            string successMessage)
        {
            if (appointmentId <= 0)
            {
                return (false, "Invalid appointment selected.");
            }

            var statusDto = new AdminUpdateAppointmentStatusDto
            {
                Status = status,
                CancellationReason = cancellationReason
            };

            using var cancellationTokenSource = CreateTimeoutToken();

            using var response = await _httpClient.PutAsJsonAsync(
                $"{AppointmentStatusEndpoint}/{appointmentId}/status",
                statusDto,
                cancellationTokenSource.Token);

            if (response.IsSuccessStatusCode)
            {
                return (true, successMessage);
            }

            var errorMessage = await ReadErrorMessageAsync(response);

            return (false, errorMessage);
        }

        private static CancellationTokenSource CreateTimeoutToken()
        {
            return new CancellationTokenSource(
                TimeSpan.FromSeconds(RequestTimeoutSeconds));
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

                if (document.RootElement.TryGetProperty(
                        "message",
                        out var messageElement))
                {
                    return messageElement.GetString() ?? "Request failed.";
                }

                if (document.RootElement.TryGetProperty(
                        "title",
                        out var titleElement))
                {
                    return titleElement.GetString() ?? "Request failed.";
                }

                if (document.RootElement.TryGetProperty(
                        "errors",
                        out var errorsElement))
                {
                    return ReadValidationErrors(errorsElement);
                }
            }
            catch (JsonException)
            {
                return content;
            }

            return "Request failed.";
        }

        private static string ReadValidationErrors(JsonElement errorsElement)
        {
            var errors = new List<string>();

            foreach (var property in errorsElement.EnumerateObject())
            {
                if (property.Value.ValueKind != JsonValueKind.Array)
                {
                    continue;
                }

                foreach (var error in property.Value.EnumerateArray())
                {
                    var errorMessage = error.GetString();

                    if (!string.IsNullOrWhiteSpace(errorMessage))
                    {
                        errors.Add(errorMessage);
                    }
                }
            }

            return errors.Count == 0
                ? "Validation failed."
                : string.Join(" ", errors);
        }
    }
}