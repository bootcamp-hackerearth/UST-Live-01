using HealthAxis.Shared.DTO.AdminDtos;
using HealthAxis.Shared.DTO.AuthDtos;
using System.Net.Http.Json;
using System.Text.Json;

namespace HealthAxis_Admin.Services
{
    public sealed class PatientAdminService
    {
        private const int RequestTimeoutSeconds = 20;

        private const string PatientsEndpoint = "api/admin/patients";
        private const string ResetPasswordEndpoint = "api/Auth/admin/reset-password";

        private readonly HttpClient _httpClient;

        public PatientAdminService(HttpClient httpClient)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            _httpClient = httpClient;
        }

        public async Task<List<AdminPatientDto>> GetPatientsAsync()
        {
            using var cancellationTokenSource = CreateTimeoutToken();

            var patients = await _httpClient.GetFromJsonAsync<List<AdminPatientDto>>(
                PatientsEndpoint,
                cancellationTokenSource.Token);

            return patients ?? new List<AdminPatientDto>();
        }

        public async Task<(bool Success, string Message)> UpdatePatientAsync(
            int patientId,
            UpdateAdminPatientDto patientDto)
        {
            if (patientId <= 0)
            {
                return (false, "Invalid patient selected.");
            }

            ArgumentNullException.ThrowIfNull(patientDto);

            using var cancellationTokenSource = CreateTimeoutToken();

            using var response = await _httpClient.PutAsJsonAsync(
                $"{PatientsEndpoint}/{patientId}",
                patientDto,
                cancellationTokenSource.Token);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Patient updated successfully.");
            }

            return (false, await ReadErrorMessageAsync(response));
        }

        public async Task<List<AdminPatientAppointmentDto>> GetPatientAppointmentsAsync(
            int patientId)
        {
            if (patientId <= 0)
            {
                return new List<AdminPatientAppointmentDto>();
            }

            using var cancellationTokenSource = CreateTimeoutToken();

            var appointments = await _httpClient.GetFromJsonAsync<List<AdminPatientAppointmentDto>>(
                $"{PatientsEndpoint}/{patientId}/appointments",
                cancellationTokenSource.Token);

            return appointments ?? new List<AdminPatientAppointmentDto>();
        }

        public async Task<(bool Success, string Message)> ResetPasswordAsync(
            AdminResetPasswordDto request)
        {
            ArgumentNullException.ThrowIfNull(request);

            using var cancellationTokenSource = CreateTimeoutToken();

            try
            {
                using var response = await _httpClient.PostAsJsonAsync(
                    ResetPasswordEndpoint,
                    request,
                    cancellationTokenSource.Token);

                var message = await ReadErrorMessageAsync(response);

                if (response.IsSuccessStatusCode)
                {
                    return (
                        true,
                        string.IsNullOrWhiteSpace(message)
                            ? "Password reset successfully."
                            : message);
                }

                return (false, message);
            }
            catch (TaskCanceledException)
            {
                return (
                    false,
                    "API request timed out. Please check if HealthAxis.API is running.");
            }
            catch (HttpRequestException)
            {
                return (
                    false,
                    "Unable to connect to API. Please run HealthAxis.API and try again.");
            }
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
                return response.IsSuccessStatusCode
                    ? string.Empty
                    : "Request failed.";
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

            return response.IsSuccessStatusCode
                ? string.Empty
                : "Request failed.";
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