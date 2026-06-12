using HealthAxis.Shared.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxis.Web.Services
{
    public class PatientApiService : IPatientApiService
    {
        private readonly HttpClient _httpClient;

        public PatientApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponseDto> Register(PatientDto dto)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("patient", content);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponseDto>(json);
        }

        public async Task<List<PatientDto>> GetAll()
        {
            var response = await _httpClient.GetAsync("patient");

            if (!response.IsSuccessStatusCode)
                return new List<PatientDto>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<PatientDto>>(json);
        }

        public async Task<PatientDto> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"patient/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PatientDto>(json);
        }

        public async Task<ApiResponseDto> Update(int id, PatientDto dto)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PutAsync($"patient/{id}", content);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponseDto>(json);
        }

        public async Task<ApiResponseDto> Deactivate(int id)
        {
            var response = await _httpClient.PutAsync($"patient/{id}/deactivate", null);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponseDto>(json);
        }

        public async Task<List<HealthRecordDto>> GetHealthRecords(int patientId)
        {
            var response = await _httpClient.GetAsync($"healthrecord/patient/{patientId}");

            if (!response.IsSuccessStatusCode)
                return new List<HealthRecordDto>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<HealthRecordDto>>(json);
        }

        public async Task<ApiResponseDto> Create(CreatePatientDto dto)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("patient/create", content);

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponseDto
                {
                    Success = false,
                    Message = "API call failed"
                };
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiResponseDto>(json);
        }
    }
}