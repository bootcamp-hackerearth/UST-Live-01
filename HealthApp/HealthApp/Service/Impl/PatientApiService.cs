using HealthApp.Shared.DTOs;
using HealthApp.Service.Interface;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthApp.Service.Impl
{
    public class PatientApiService : IPatientApiService
    {
        private readonly string baseUrl = "https://localhost:44339/api/patients";

        public async Task<List<PatientDto>> GetAll()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(baseUrl);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<PatientDto>>();
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
                    return await response.Content.ReadAsAsync<PatientDto>();
                }
            }

            return null;
        }

        public async Task Create(PatientDto dto)
        {
            using (HttpClient client = new HttpClient())
            {
                await client.PostAsJsonAsync(baseUrl, dto);
            }
        }

        public async Task Update(int id, PatientDto dto)
        {
            using (HttpClient client = new HttpClient())
            {
                await client.PutAsJsonAsync($"{baseUrl}/{id}", dto);
            }
        }
    }
}