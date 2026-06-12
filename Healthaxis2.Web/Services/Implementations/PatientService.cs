using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Healthaxis2.Shared.DTOs;
using Healthaxis2.Web.Services.Interfaces;

namespace Healthaxis2.Web.Services
{
    public class PatientService : IPatientService
    {
        private readonly HttpClient _client;

        private readonly string baseUrl = "https://localhost:44366/api/patients/";

        public PatientService()
        {
            _client = new HttpClient();
        }

        // ✅ GET ALL
        public async Task<List<PatientDto>> GetAll()
        {
            return await _client.GetFromJsonAsync<List<PatientDto>>(baseUrl);
        }

        // ✅ GET BY ID
        public async Task<PatientDto> GetById(int id)
        {
            return await _client.GetFromJsonAsync<PatientDto>(baseUrl + id);
        }

        // ✅ CREATE
        public async Task<bool> Create(PatientDto dto)
        {
            var response = await _client.PostAsJsonAsync(baseUrl, dto);
            return response.IsSuccessStatusCode;
        }

        // ✅ UPDATE
        public async Task<bool> Update(int id, PatientDto dto)
        {
            var response = await _client.PutAsJsonAsync(baseUrl + id, dto);
            return response.IsSuccessStatusCode;
        }

        // ✅ DELETE
        public async Task<bool> Delete(int id)
        {
            var response = await _client.DeleteAsync(baseUrl + id);
            return response.IsSuccessStatusCode;
        }
        public async Task<PatientDto> CreateAndReturn(PatientDto dto)
        {
            var response = await _client.PostAsJsonAsync(baseUrl, dto);

            return await response.Content.ReadFromJsonAsync<PatientDto>();
        }
    }
}