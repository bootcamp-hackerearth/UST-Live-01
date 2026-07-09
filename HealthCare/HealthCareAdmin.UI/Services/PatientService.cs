using Healthcare.Shared.DTOs;
using Healthcare.Shared.DTOs.Doctor;
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


        public async Task<PagedResult<PatientListDto>> GetPatients(PatientFilter filter)
        {
            var query = $"api/admin/patients?pageNumber={filter.PageNumber}&pageSize={filter.PageSize}";

            if (!string.IsNullOrEmpty(filter.FullName))
                query += $"&fullName={filter.FullName}";

            if (filter.HasInsurance.HasValue)
                query += $"&hasInsurance={filter.HasInsurance}";

            if (filter.IsActive.HasValue)
                query += $"&isActive={filter.IsActive}";

            return await _http.GetFromJsonAsync<PagedResult<PatientListDto>>(query) ?? new PagedResult<PatientListDto>();
        }


        public async Task DeletePatient(int id)
        {
            await _http.DeleteAsync($"api/admin/patients/{id}");
        }

 

        public async Task UpdateStatus(int id, bool status)
        {
            var response = await _http.PatchAsJsonAsync(
                $"api/admin/patients/{id}/status", status);

            response.EnsureSuccessStatusCode();
        }
    }
}