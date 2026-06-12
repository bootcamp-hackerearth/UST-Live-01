using HealthApp.Service.Interface;
using HealthApp.Shared.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Service.Impl
{
    public class HealthRecordApiService : IHealthRecordApiService
    {
        private readonly string baseUrl = "https://localhost:44317/api/healthrecords";

        public async Task<List<HealthRecordDto>> GetAll()
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync(baseUrl);

                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<HealthRecordDto>>(json);
                }
            }

            return new List<HealthRecordDto>();
        }

        public async Task<List<HealthRecordDto>> GetByPatient(int patientId)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/patient/{patientId}");

                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<HealthRecordDto>>(json);
                }
            }

            return new List<HealthRecordDto>();
        }

        public async Task<List<HealthRecordDto>> GetByDoctorAndPatient(int doctorId, int patientId)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/filter?doctorId={doctorId}&patientId={patientId}");

                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<HealthRecordDto>>(json);
                }
            }

            return new List<HealthRecordDto>();
        }

        public async Task Create(HealthRecordDto dto)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PostAsync(baseUrl, content);

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new System.Exception(error);
                }
            }
        }
    }
}