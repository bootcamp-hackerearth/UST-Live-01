using HealthApp.Service.Interface;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Constant;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthApp.Service.Impl
{
    public class DoctorApiService : IDoctorApiService
    {
        private readonly string baseUrl = "https://localhost:44339/api/doctors";

        private string CleanError(string error)
        {
            return error.Replace("{", "")
                        .Replace("}", "")
                        .Replace("\"", "")
                        .Replace("Message:", "")
                        .Replace("message:", "")
                        .Trim();
        }


        public async Task<List<DoctorDto>> GetAll()
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync(baseUrl);

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(CleanError(error));
                }

                return await res.Content.ReadAsAsync<List<DoctorDto>>();
            }
        }

        public async Task<DoctorDto> GetById(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/{id}");

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();

                    var cleanError = error.Replace("{", "")
                                          .Replace("}", "")
                                          .Replace("\"", "")
                                          .Replace("Message:", "")
                                          .Trim();

                    throw new Exception(cleanError);
                }

                return await res.Content.ReadAsAsync<DoctorDto>();
            }
        }


        public async Task Create(DoctorDto dto)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.PostAsJsonAsync(baseUrl, dto);

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(CleanError(error));
                }
            }
        }

        public async Task<List<DoctorDto>> SearchBySpecialisation(SpecialisationType type)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/specialisation/{type}");

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(CleanError(error));
                }

                return await res.Content.ReadAsAsync<List<DoctorDto>>();
            }
        }

        public async Task ToggleStatus(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.PutAsync($"{baseUrl}/{id}/toggle", null);

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(CleanError(error));
                }
            }
        }

        public async Task Update(int id, DoctorDto dto)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.PutAsJsonAsync($"{baseUrl}/{id}", dto);

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(CleanError(error));
                }
            }
        }
    }
}