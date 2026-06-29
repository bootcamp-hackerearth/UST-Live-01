namespace HealthAxisAdminPortal.Services.Implementation
{
    using HealthAxisAdminPortal.Models;
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
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/patients/{id}")
            {
                Content = JsonContent.Create(dto)
            };

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenStore.AccessToken);

            var response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var problem = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

                if (problem?.Errors != null && problem.Errors.Count > 0)
                {
                    var messages = problem.Errors
                        .SelectMany(e => e.Value)
                        .Where(m => !string.IsNullOrWhiteSpace(m));

                    throw new Exception(string.Join(" *** ", messages)); 
        }

                var content = await response.Content.ReadAsStringAsync();
                throw new Exception(content);
            }

            return true;
        }
    }
}
