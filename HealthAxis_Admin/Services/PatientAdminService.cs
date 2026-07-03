using HealthAxis.Shared.DTO.AdminDtos;
using System.Net.Http.Json;
using System.Text.Json;

namespace HealthAxis_Admin.Services
{
    public sealed class PatientAdminService
    {
        private const string PatientsEndpoint = "api/admin/patients";

        private readonly HttpClient _httpClient;

        public PatientAdminService(HttpClient httpClient)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            _httpClient = httpClient;
        }

        public async Task<List<AdminPatientDto>> GetPatientsAsync()
        {
            var patients = await _httpClient.GetFromJsonAsync<List<AdminPatientDto>>(
                PatientsEndpoint);

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

            using var response = await _httpClient.PutAsJsonAsync(
                $"{PatientsEndpoint}/{patientId}",
                patientDto);

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

            var appointments = await _httpClient.GetFromJsonAsync<List<AdminPatientAppointmentDto>>(
                $"{PatientsEndpoint}/{patientId}/appointments");

            return appointments ?? new List<AdminPatientAppointmentDto>();
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