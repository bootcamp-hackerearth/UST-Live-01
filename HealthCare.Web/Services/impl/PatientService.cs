using HealthCare.Shared.DTOs.Patient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Web.Services.Interfaces;
using HealthCare.Shared;

namespace HealthCare.Web.Services
{
    public class PatientService : IPatientService
    {
        private readonly HttpClient _client;

        public PatientService(HttpClient client)
        {
            _client = client;
        }
        private readonly string baseUrl = "https://localhost:44384/api/patients";

        public async Task<PagedResult<PatientDto>> GetPatientsAsync(
            string searchTerm,
            int pageNumber,
            int pageSize)
        {
            string url = $"{baseUrl}?searchTerm={searchTerm}&pageNumber={pageNumber}&pageSize={pageSize}";

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new PagedResult<PatientDto>(); // return empty object (not list)

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PagedResult<PatientDto>>(json);
        }

        public async Task<PagedResult<PatientDto>> GetByIdAsync(int id)
        {
            var response = await _client.GetAsync($"{baseUrl}/{id}");

            var result = new PagedResult<PatientDto>();

            if (!response.IsSuccessStatusCode)
            {
                result.PageNumber = 1;
                result.PageSize = 1;
                result.TotalCount = 0;
                return result;
            }

            var json = await response.Content.ReadAsStringAsync();
            var patient = JsonConvert.DeserializeObject<PatientDto>(json);

            result.Items = new List<PatientDto> { patient };
            result.PageNumber = 1;
            result.PageSize = 1;
            result.TotalCount = 1;

            return result;
        }

        public async Task<bool> CreateAsync(PatientDto patient)
        {
            var json = JsonConvert.SerializeObject(patient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(baseUrl, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(PatientDto patient)
        {
            var json = JsonConvert.SerializeObject(patient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{baseUrl}/{patient.PatientId}", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _client.DeleteAsync($"{baseUrl}/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}