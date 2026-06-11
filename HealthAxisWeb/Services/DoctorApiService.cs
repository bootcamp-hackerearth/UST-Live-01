using HealthAxis.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HealthAxis.Shared.Dtos;

namespace HealthAxis.Web.Services
{
    public class DoctorApiService : IDoctorApiService
    {
        private readonly HttpClient _httpClient;

        public DoctorApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DoctorDto>> GetAll(Specialisation? specialisation)
        {
            string url = "doctor";

            if (specialisation.HasValue)
                url += $"?specialisation={specialisation.Value}";

            var res = await _httpClient.GetAsync(url);
            var json = await res.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<DoctorDto>>(json);
        }
        public async Task ToggleStatus(int id)
        {
            await _httpClient.PutAsync($"doctor/{id}/toggle", null);
        }

        public async Task<DoctorDto> GetById(int id)
        {
            var res = await _httpClient.GetAsync($"doctor/{id}");

            if (!res.IsSuccessStatusCode)
                return null;

            var json = await res.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<DoctorDto>(json);
        }

        public async Task Create(CreateDoctorDto dto)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            await _httpClient.PostAsync("doctor", content);
        }

        public async Task Update(int id, UpdateDoctorDto dto)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            await _httpClient.PutAsync($"doctor/{id}", content);
        }
    }
}