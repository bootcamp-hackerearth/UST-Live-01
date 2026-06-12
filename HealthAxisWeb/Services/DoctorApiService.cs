using HealthAxis.Shared.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<DoctorDto>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<DoctorDto>>(json);
        }

        public async Task<DoctorDto> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"doctor/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<DoctorDto>(json);
        }

        public async Task<ApiResponseDto> Create(CreateDoctorDto dto)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("doctor", content);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponseDto>(json);
        }

        public async Task<ApiResponseDto> Update(int id, UpdateDoctorDto dto)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PutAsync($"doctor/{id}", content);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponseDto>(json);
        }

        public async Task ToggleStatus(int id)
        {
            await _httpClient.PutAsync($"doctor/{id}/toggle", null);
        }
    }
}