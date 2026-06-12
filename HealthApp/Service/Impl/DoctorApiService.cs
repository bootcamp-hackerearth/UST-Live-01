using HealthApp.Service.Interface;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Constant;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Service.Impl
{
    public class DoctorApiService : IDoctorApiService
    {
        private readonly string baseUrl = "https://localhost:44317/api/doctors";

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

                var json = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                    throw new Exception(CleanError(json));

                return JsonConvert.DeserializeObject<List<DoctorDto>>(json);
            }
        }

        public async Task<DoctorDto> GetById(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/{id}");

                var json = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                    throw new Exception(CleanError(json));

                return JsonConvert.DeserializeObject<DoctorDto>(json);
            }
        }

        public async Task Create(DoctorDto dto)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PostAsync(baseUrl, content);

                var responseText = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                    throw new Exception(CleanError(responseText));
            }
        }

        public async Task<List<DoctorDto>> SearchBySpecialisation(SpecialisationType type)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/specialisation/{type}");

                var json = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                    throw new Exception(CleanError(json));

                return JsonConvert.DeserializeObject<List<DoctorDto>>(json);
            }
        }

        public async Task ToggleStatus(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.PutAsync($"{baseUrl}/{id}/toggle", null);

                var responseText = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                    throw new Exception(CleanError(responseText));
            }
        }
    }
}