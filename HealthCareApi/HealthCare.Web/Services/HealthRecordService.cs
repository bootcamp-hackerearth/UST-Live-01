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
        private static readonly HttpClient client = new HttpClient();
        private readonly string baseUrl = "https://localhost:44373/api/healthrecords";

        // ✅ GET PAGINATED HISTORY
        public async Task<PagedResult<HealthRecordDto>> GetPatientHealthHistoryAsync(
            int patientId,
            int pageNumber,
            int pageSize)
        {
            string url = $"{baseUrl}/patient/{patientId}?pageNumber={pageNumber}&pageSize={pageSize}";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new PagedResult<HealthRecordDto>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PagedResult<HealthRecordDto>>(json);
        }

        // ✅ CREATE RECORD
        public async Task<bool> CreateAsync(CreateHealthRecordDto dto)
        {
            System.Diagnostics.Debug.WriteLine(dto.AppointmentId);
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            System.Diagnostics.Debug.WriteLine(content);

            var response = await client.PostAsync(baseUrl, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<HealthRecordDto> GetByIdAsync(int appointmentId)
        {
            var response = await client.GetAsync($"{baseUrl}/{appointmentId}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HealthRecordDto>(json);
        }
    }
}