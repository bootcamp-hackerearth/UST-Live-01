using HealthCare.Shared;
using HealthCare.Shared.DTOs.Patient;
using HealthCare.Web.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Web.Services
{
    public class PatientService : IPatientService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string baseUrl = "https://localhost:44368/api/patients";

        public async Task<PagedResult<PatientDto>> GetPatientsAsync(
            string searchTerm,
            int pageNumber,
            int pageSize)
        {
            
            string url = $"{baseUrl}?searchTerm={Uri.EscapeDataString(searchTerm ?? "")}&pageNumber={pageNumber}&pageSize={pageSize}";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new PagedResult<PatientDto>();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PagedResult<PatientDto>>(json);

            return result ?? new PagedResult<PatientDto>();
        
        }

        public async Task<PatientDto> GetByIdAsync(int id)
        {
            var response = await client.GetAsync($"{baseUrl}/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PatientDto>(json);
        }

        public async Task<bool> CreateAsync(CreatePatientDto patient)
        {
            var json = JsonConvert.SerializeObject(patient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(baseUrl, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(PatientDto patient)
        {
            var json = JsonConvert.SerializeObject(patient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{baseUrl}/{patient.PatientId}", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await client.DeleteAsync($"{baseUrl}/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}