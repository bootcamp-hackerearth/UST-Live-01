using HealthAppMVC.Models;
using HealthAppMVC.Services.Interface;
using HealthAppWebAPI.Models.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthAppMVC.Services.Implementation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly HttpClient _httpClient;

        public AppointmentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
        {
            var response = await _httpClient.GetAsync("appointments");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<AppointmentDto>>(data);
        }

        public async Task<AppointmentDto> GetAppointmentByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"appointments/{id}");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AppointmentDto>(data);
        }

        public async Task BookAppointmentAsync(CreateAppointmentDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("appointments", dto);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task ConfirmAppointmentAsync(int appointmentId)
        {
            var response = await _httpClient.PostAsync(
                $"appointments/{appointmentId}/confirm", null);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task CancelAppointmentAsync(int appointmentId, string reason)
        {
            var dto = new CancelAppointmentDto
            {
                CancellationReason = reason
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"appointments/{appointmentId}/cancel", dto);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientAsync(int patientId)
        {
            var response = await _httpClient.GetAsync($"appointments/patient/{patientId}");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<AppointmentDto>>(data);
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorAsync(int doctorId)
        {
            var response = await _httpClient.GetAsync($"appointments/doctor/{doctorId}");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<AppointmentDto>>(data);
        }

        public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync()
        {
            var response = await _httpClient.GetAsync("appointments/upcoming");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<AppointmentDto>>(data);
        }

        public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsByDoctorAsync(int doctorId)
        {
            var response = await _httpClient.GetAsync($"appointments/upcoming/doctor/{doctorId}");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<AppointmentDto>>(data);
        }

        public async Task<IEnumerable<string>> GetAvailableSlotsAsync(int doctorId, DateTime scheduledDate)
        {
            string date = scheduledDate.ToString("yyyy-MM-dd");

            var response = await _httpClient.GetAsync(
                $"appointments/slots?doctorId={doctorId}&date={date}");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<string>>(data);
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientNameAsync(string patientName)
        {
            var response = await _httpClient.GetAsync(
                $"appointments/search?patientName={patientName}");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<AppointmentDto>>(data);
        }

        public async Task<bool> HealthRecordExistsAsync(int appointmentId)
        {
            var response = await _httpClient.GetAsync(
                $"appointments/{appointmentId}/healthrecord");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(data);
        }
    }
}