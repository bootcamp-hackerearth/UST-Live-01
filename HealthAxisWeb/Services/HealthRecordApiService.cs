using HealthAxis.Shared.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxis.Web.Services
{
    public class HealthRecordApiService : IHealthRecordApiService
    {
        private readonly HttpClient _httpClient;

        public HealthRecordApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponseDto> Create(CreateHealthRecordDto dto)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("healthrecord", content);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponseDto>(json);
        }
        public async Task<List<HealthRecordDto>> GetByPatient(int patientId)
        {
            var response = await _httpClient.GetAsync($"healthrecord/patient/{patientId}");

            if (!response.IsSuccessStatusCode)
                return new List<HealthRecordDto>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<HealthRecordDto>>(json);
        }
    }
}