using HealthApp.Service.Interface;
using HealthApp.Shared.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthApp.Service.Impl
{
    public class HealthRecordApiService : IHealthRecordApiService
    {
        private readonly string baseUrl = "https://localhost:44339/api/healthrecords";

        public async Task<List<HealthRecordDto>> GetAll()
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync(baseUrl);
                if (res.IsSuccessStatusCode)
                    return await res.Content.ReadAsAsync<List<HealthRecordDto>>();
            }
            return new List<HealthRecordDto>();
        }

        public async Task<List<HealthRecordDto>> GetByPatient(int patientId)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/patient/{patientId}");
                if (res.IsSuccessStatusCode)
                    return await res.Content.ReadAsAsync<List<HealthRecordDto>>();
            }
            return new List<HealthRecordDto>();
        }

        // ✅ ✅ UPDATED METHOD (IMPORTANT)
        public async Task<List<HealthRecordDto>> GetRecordsByDoctorAndPatient(int? doctorId, int? patientId)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{baseUrl}/filter?";

                if (doctorId.HasValue)
                    url += $"doctorId={doctorId.Value}&";

                if (patientId.HasValue)
                    url += $"patientId={patientId.Value}&";

                var res = await client.GetAsync(url);

                if (res.IsSuccessStatusCode)
                    return await res.Content.ReadAsAsync<List<HealthRecordDto>>();
            }

            return new List<HealthRecordDto>();
        }


        // ✅ ✅ NEW METHOD (FOR BUTTON)
        public async Task<List<HealthRecordDto>> GetPatientRecords(int patientId)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/patient/{patientId}");

                if (res.IsSuccessStatusCode)
                    return await res.Content.ReadAsAsync<List<HealthRecordDto>>();
            }

            return new List<HealthRecordDto>();
        }

        public async Task Create(HealthRecordDto dto)
        {
            using (HttpClient client = new HttpClient())
            {
                await client.PostAsJsonAsync(baseUrl, dto);
            }
        }
    }
}