using HealthAxisAdminPortal.Models;
using HealthAxisAdminPortal.Services.FrontEndMemory;
using HealthAxisAdminPortal.Services.Interfaces;
using HealthAxisApplicn.Dto.Auth;
using HealthAxisApplicn.Dto.Doctors;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

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

        public async Task<bool> CreateAsync(RegisterDto register, CreateDoctorDto doctor)
        {
            // ✅ STEP 1 → Register user
            var registerRequest = new HttpRequestMessage(HttpMethod.Post, "/api/auth/register")
            {
                Content = JsonContent.Create(register)
            };

            registerRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenStore.AccessToken);

            var registerResponse = await http.SendAsync(registerRequest);

            if (!registerResponse.IsSuccessStatusCode)
            {
                var content = await registerResponse.Content.ReadAsStringAsync();
                throw new Exception(content);
            }

            var registerResult = await registerResponse.Content.ReadFromJsonAsync<AuthRegisterResponse>();

            // ✅ STEP 2 → Use UserId for doctor creation
            doctor.UserId = registerResult!.UserId;
            doctor.Email = register.Email;

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/doctors")
            {
                Content = JsonContent.Create(doctor)
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


        public async Task<bool> UpdateAsync(int id, UpdateDoctorDto dto)
            {
                var request = new HttpRequestMessage(HttpMethod.Put, $"/api/doctors/{id}")
                {
                    Content = JsonContent.Create(dto)
                };

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", TokenStore.AccessToken);

                var response = await http.SendAsync(request);

                // ✅ ONLY handle errors when request fails
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
