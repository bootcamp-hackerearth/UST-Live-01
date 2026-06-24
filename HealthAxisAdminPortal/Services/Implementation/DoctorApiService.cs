using HealthAxisAdminPortal.Services.FrontEndMemory;
using HealthAxisAdminPortal.Services.Interfaces;
using HealthAxisApplicn.Dto.Doctors;
using System.Net.Http.Json;

namespace HealthAxisAdminPortal.Services.Implementation
{

    public class DoctorApiService : IDoctorApiService
    {
        private readonly HttpClient http;

        public DoctorApiService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<List<DoctorDto>?> GetAllAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/api/doctors");

                Console.WriteLine($"TOKEN: {TokenStore.AccessToken}");


                // ✅ Attach token HERE (correct way)
                if (!string.IsNullOrEmpty(TokenStore.AccessToken))
                {
                    request.Headers.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue(
                            "Bearer",
                            TokenStore.AccessToken
                        );
                }

                var response = await http.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    return new List<DoctorDto>();

                return await response.Content.ReadFromJsonAsync<List<DoctorDto>>();
            }
            catch
            {
                return new List<DoctorDto>();
            }
        }


        public async Task<bool> CreateAsync(CreateDoctorDto dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/doctors")
            {
                Content = JsonContent.Create(dto)
            };

            if (!string.IsNullOrEmpty(TokenStore.AccessToken))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        TokenStore.AccessToken
                    );
            }

            var response = await http.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdateDoctorDto dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/doctors/{id}")
            {
                Content = JsonContent.Create(dto)
            };

            if (!string.IsNullOrEmpty(TokenStore.AccessToken))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        TokenStore.AccessToken
                    );
            }

            var response = await http.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ToggleActiveAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/doctors/toggle/{id}");

            if (!string.IsNullOrEmpty(TokenStore.AccessToken))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        TokenStore.AccessToken
                    );
            }

            var response = await http.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
        public async Task<List<DoctorDto>?> SearchAsync(string query)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"/api/doctors/search?query={query}"
            );

            Console.WriteLine($"Search Token: {TokenStore.AccessToken}");

            if (!string.IsNullOrEmpty(TokenStore.AccessToken))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        TokenStore.AccessToken
                    );
            }

            var response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return new List<DoctorDto>();

            return await response.Content.ReadFromJsonAsync<List<DoctorDto>>();
        }
    }

}
