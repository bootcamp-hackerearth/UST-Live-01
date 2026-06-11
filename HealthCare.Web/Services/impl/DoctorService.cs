using HealthCare.Shared;
using HealthCare.Shared.DTOs.Doctor;
using HealthCare.Web.Services.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Web.Services
{
    public class DoctorService : IDoctorService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string baseUrl = "https://localhost:44384/api/doctors";

        public async Task<PagedResult<DoctorDto>> GetDoctorsAsync(
            string specialization,
            string searchTerm,
            bool orderByDescending,
            int pageNumber,
            int pageSize)
        {
            string url = $"{baseUrl}?specialization={specialization}&searchTerm={searchTerm}&orderByDescending={orderByDescending}&pageNumber={pageNumber}&pageSize={pageSize}";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new PagedResult<DoctorDto>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PagedResult<DoctorDto>>(json);
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            var response = await client.GetAsync($"{baseUrl}/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<DoctorDto>(json);
        }

        public async Task<bool> CreateAsync(CreateDoctorDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(baseUrl, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(UpdateDoctorDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{baseUrl}/{dto.DoctorId}", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await client.DeleteAsync($"{baseUrl}/{id}");

            return response.IsSuccessStatusCode;
        }

        
        public async Task<List<DoctorLookupDto>> GetDoctorsBySpecializationAsync(string specialization)
        {
            var response = await client.GetAsync($"doctors/specialization/{specialization}");

            if (!response.IsSuccessStatusCode)
                return new List<DoctorLookupDto>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DoctorLookupDto>>(json);
        }
    }
}