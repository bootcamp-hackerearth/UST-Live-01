using Healthcare.Shared.DTOs.Patient;
using System.Net.Http.Json;

namespace HealthCareAdmin.UI.Services
{
    public class PatientService
    {
        private readonly HttpClient _http;

        public PatientService(HttpClient http)
        {
            _http = http;
        }

        public async Task<PagedPatientResponse> GetPatients()
        {
            return await _http.GetFromJsonAsync<PagedPatientResponse>("api/admin/patients")
                   ?? new PagedPatientResponse();
        }

        public async Task DeletePatient(int id)
        {
            await _http.DeleteAsync($"api/admin/patients/{id}");
        }

        public async Task UpdateInsuranceStatus(int id, bool status)
        {
            await _http.PatchAsJsonAsync($"api/admin/patients/{id}/insurance", status);
        }

        public async Task UpdateStatus(int id, bool status)
        {
            var response = await _http.PatchAsJsonAsync(
                $"api/admin/patients/{id}/status", status);

            response.EnsureSuccessStatusCode();
        }
    }
}