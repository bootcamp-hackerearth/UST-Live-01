using HealthApp.Service.Interface;
using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HealthApp.Service.Impl
{
    public class PatientApiService : IPatientApiService
    {
        private readonly string baseUrl = "https://localhost:44317/api/patients";

        public async Task<List<PatientDto>> GetAll()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(baseUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<PatientDto>>(json);
                }
            }

            return new List<PatientDto>();
        }

        public async Task<PatientDto> GetById(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"{baseUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<PatientDto>(json);
                }
            }

            return null;
        }

        public async Task Create(PatientDto dto)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(baseUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception(error);
                }
            }
        }

        public async Task Update(int id, PatientDto dto)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{baseUrl}/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception(error);
                }
            }
        }
    }
}