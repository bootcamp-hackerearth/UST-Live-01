using HealthAxis.Shared.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxis.Web.Services
{
    public class DoctorApiService : IDoctorApiService
    {
        private readonly HttpClient _httpClient;

        public DoctorApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DoctorDto>> GetAllDoctors()
        {
            var res = await _httpClient.GetAsync("doctor");
            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DoctorDto>>(json);
        }

        public async Task<DoctorDto> GetDoctorById(int id)
        {
            var res = await _httpClient.GetAsync($"doctor/{id}");
            if (!res.IsSuccessStatusCode) return null;

            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DoctorDto>(json);
        }

        public async Task<bool> AddDoctor(DoctorDto dto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync("doctor", content);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDoctor(int id, DoctorDto dto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var res = await _httpClient.PutAsync($"doctor/{id}", content);
            return res.IsSuccessStatusCode;
        }

        public async Task<List<DoctorDto>> GetBySpecialisation(Specialisation spec)
        {
            var res = await _httpClient.GetAsync($"doctor/specialisation/{spec}");
            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DoctorDto>>(json);
        }

        public async Task<List<PatientDto>> GetAllPatients()
        {
            var res = await _httpClient.GetAsync("patient");
            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PatientDto>>(json);
        }
    }
}