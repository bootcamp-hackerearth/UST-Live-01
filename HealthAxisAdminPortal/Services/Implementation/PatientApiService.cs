namespace HealthAxisAdminPortal.Services.Implementation
{
    using HealthAxisAdminPortal.Services.FrontEndMemory;
    using HealthAxisAdminPortal.Services.Interfaces;
    using HealthAxisApplicn.Dto.Patients;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;

    public class PatientApiService(HttpClient http) : IPatientApiService
    {
        public async Task<List<PatientDto>?> GetAllAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/patients");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenStore.AccessToken);

            var response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return new List<PatientDto>();

            return await response.Content.ReadFromJsonAsync<List<PatientDto>>();
        }

        public async Task<List<PatientDto>?> SearchAsync(string? name, string? phone)
        {
            var url = $"/api/patients/search?name={name}&phone={phone}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenStore.AccessToken);

            var response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return new List<PatientDto>();

            return await response.Content.ReadFromJsonAsync<List<PatientDto>>();
        }

        public async Task<bool> ToggleAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Put,
                $"/api/patients/toggle/{id}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenStore.AccessToken);

            var response = await http.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdatePatientDto dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Put,
                $"/api/patients/{id}")
            {
                Content = JsonContent.Create(dto)
            };

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenStore.AccessToken);

            var response = await http.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}
