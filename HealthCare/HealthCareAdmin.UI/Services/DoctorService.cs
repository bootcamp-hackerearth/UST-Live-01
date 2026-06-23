using Healthcare.Shared.DTOs.Doctor;
using System.Net.Http.Json;

namespace HealthCareAdmin.UI.Services
{
    public class DoctorService
    {
        private readonly HttpClient _http;

        public DoctorService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<DoctorListDto>> GetDoctors()
        {
            return await _http.GetFromJsonAsync<List<DoctorListDto>>("api/admin/doctors");
        }

        public async Task DeleteDoctor(int id)
        {
            await _http.DeleteAsync($"api/admin/doctors/{id}");
        }
    }
}
