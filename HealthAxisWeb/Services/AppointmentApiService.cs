using HealthAxis.Shared.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxis.Web.Services
{
    public class AppointmentApiService : IAppointmentApiService
    {
        private readonly HttpClient _httpClient;

        public AppointmentApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponseDto> Book(BookAppointmentDto dto)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("appointment", content);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponseDto>(json);
        }

        public async Task<List<AppointmentDto>> GetByPatient(int patientId)
        {
            var response = await _httpClient.GetAsync($"appointment/patient/{patientId}");

            if (!response.IsSuccessStatusCode)
                return new List<AppointmentDto>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
        }

        public async Task<List<AppointmentDto>> GetByDoctor(int doctorId)
        {
            var response = await _httpClient.GetAsync($"appointment/doctor/{doctorId}");

            if (!response.IsSuccessStatusCode)
                return new List<AppointmentDto>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
        }

        public async Task<AppointmentDto> GetById(int appointmentId)
        {
            var response = await _httpClient.GetAsync($"appointment/{appointmentId}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AppointmentDto>(json);
        }

        public async Task<ApiResponseDto> UpdateStatus(int id, UpdateAppointmentStatusDto dto)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PutAsync($"appointment/{id}/status", content);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponseDto>(json);
        }

        public async Task<ApiResponseDto> Cancel(int id, UpdateAppointmentStatusDto dto)
        {
            dto.Status = AppointmentStatus.Cancelled;

            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PutAsync($"appointment/{id}/status", content);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponseDto>(json);
        }
    }
}
