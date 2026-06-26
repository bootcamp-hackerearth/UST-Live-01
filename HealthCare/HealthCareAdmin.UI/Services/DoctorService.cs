using Healthcare.Shared.DTOs.Authentication;
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

        public async Task<PagedDoctorResponse> GetDoctors()
        {
            return await _http.GetFromJsonAsync<PagedDoctorResponse>("api/admin/doctors")
                   ?? new PagedDoctorResponse();
        }

        public async Task<DoctorListDto> GetDoctorById(int id)
        {
            return await _http.GetFromJsonAsync<DoctorListDto>($"api/admin/doctors/{id}");
        }

        public async Task DeleteDoctor(int id)
        {
            await _http.DeleteAsync($"api/admin/doctors/{id}");
        }

        public async Task<string?> RegisterDoctor(DoctorRegisterDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register-doctor", dto);

            if (response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsStringAsync();
        }

        public async Task UpdateDoctor(int id, UpdateDoctorDto dto)
        {
            await _http.PutAsJsonAsync($"api/admin/doctors/{id}", dto);
        }

        public async Task UpdateStatus(int id, bool status)
        {
            await _http.PatchAsJsonAsync($"api/admin/doctors/{id}/status", status);
        }
    }
}