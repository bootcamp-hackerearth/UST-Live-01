using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Healthaxis2.Shared.DTOs;
using Healthaxis2.Web.Services.Interfaces;

namespace Healthaxis2.Web.Services
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl = "https://localhost:44366/api/healthrecords/";

        public HealthRecordService()
        {
            _client = new HttpClient();
        }

        public async Task<List<HealthRecordDto>> GetByPatient(int patientId)
        {
            return await _client.GetFromJsonAsync<List<HealthRecordDto>>(
                baseUrl + "patient/" + patientId);
        }

        public async Task<bool> Create(HealthRecordDto dto)
        {
            var response = await _client.PostAsJsonAsync(baseUrl, dto);
            return response.IsSuccessStatusCode;
        }
    }
}