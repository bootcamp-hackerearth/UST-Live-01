using HealthCare.Shared;
using HealthCare.Shared.DTOs.HealthRecord;
using HealthCare.Web.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Web.Services
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly HttpClient _client;

        public HealthRecordService(HttpClient client)
        {
            _client = client;
        }
        private readonly string baseUrl = "https://localhost:44327/api/healthrecords";

        //  GET PAGINATED HISTORY
        public async Task<PagedResult<HealthRecordDto>> GetPatientHealthHistoryAsync(
            int patientId,
            int pageNumber,
            int pageSize)
        {
            string url = $"{baseUrl}/patient/{patientId}?pageNumber={pageNumber}&pageSize={pageSize}";

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new PagedResult<HealthRecordDto>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PagedResult<HealthRecordDto>>(json);
        }

        //  CREATE RECORD
        public async Task<bool> CreateAsync(CreateHealthRecordDto dto)
        {
            System.Diagnostics.Debug.WriteLine(dto.AppointmentId);
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            System.Diagnostics.Debug.WriteLine(content);

            var response = await _client.PostAsync(baseUrl, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<HealthRecordDto> GetByAppointmentIdAsync(int appointmentId)
        {
            var response = await _client.GetAsync($"{baseUrl}/appointment/{appointmentId}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HealthRecordDto>(json);
        }

        public async Task<HealthRecordDto> GetByIdAsync(int recordId)
        {
            var response = await _client.GetAsync($"{baseUrl}/record/{recordId}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HealthRecordDto>(json);
        }
    }
}