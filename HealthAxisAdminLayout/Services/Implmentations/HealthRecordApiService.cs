using System.Net.Http.Json;
using HealthAxis.Shared.DTOs.HealthRecord;
using HealthAxisAdminLayout.Services.Interfaces;

namespace HealthAxisAdminLayout.Services.Implementations
{
    public class HealthRecordApiService : IHealthRecordApiService
    {
        private readonly HttpClient _http;

        public HealthRecordApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<HealthRecordResponseDTO>> GetHealthRecordsAsync()
        {
            var result = await _http.GetFromJsonAsync<List<HealthRecordResponseDTO>>(
                "api/healthrecord"
            );

            return result ?? new List<HealthRecordResponseDTO>();
        }

        public async Task<bool> DeleteHealthRecordAsync(int id)
        {
            var response = await _http.DeleteAsync(
                $"api/healthrecord/{id}"
            );

            return response.IsSuccessStatusCode;
        }
    }
}
