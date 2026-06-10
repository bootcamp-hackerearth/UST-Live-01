using HealthCare.Shared;
using HealthCare.Shared.DTOs.Patient;
using HealthCare.Web.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Web.Services
{
    public class PatientService : IPatientService
    {
        // Single shared HttpClient — never create one per request
        private static readonly HttpClient _client = new HttpClient();
        private readonly string _baseUrl = "https://localhost:44368/api/patients";

        public async Task<PagedResult<PatientDto>> GetPatientsAsync(
            string searchTerm, int pageNumber, int pageSize)
        {
            string url = $"{_baseUrl}?searchTerm={Uri.EscapeDataString(searchTerm ?? "")}" +
                         $"&pageNumber={pageNumber}&pageSize={pageSize}";

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new PagedResult<PatientDto>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PagedResult<PatientDto>>(json)
                   ?? new PagedResult<PatientDto>();
        }

        public async Task<PatientDto> GetByIdAsync(int id)
        {
            var response = await _client.GetAsync($"{_baseUrl}/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PatientDto>(json);
        }

        public async Task<bool> CreateAsync(CreatePatientDto patient)
        {
            var json = JsonConvert.SerializeObject(patient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_baseUrl, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(PatientDto patient)
        {
            // Build the update payload — matches UpdatePatient DTO on the API side
            var updatePayload = new
            {
                patient.FullName,
                patient.DateOfBirth,
                patient.Gender,
                patient.Email,
                patient.PhoneNumber,
                patient.InsuranceId
            };

            var json = JsonConvert.SerializeObject(updatePayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{_baseUrl}/{patient.PatientId}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _client.DeleteAsync($"{_baseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }


    }
}
