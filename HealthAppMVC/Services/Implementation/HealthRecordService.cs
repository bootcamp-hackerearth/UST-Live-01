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
    public class HealthRecordService : IHealthRecordService
    {
        private readonly HttpClient _httpClient;

        public HealthRecordService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<HealthRecordDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("healthrecords");
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<HealthRecordDto>>(data);
        }


        public async Task<int> GetPatientIdByAppointmentAsync(int appointmentId)
        {
            var response = await _httpClient.GetAsync($"Appointment/{appointmentId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            AppointmentDto appointment=JsonConvert.DeserializeObject<AppointmentDto>(result);
            return appointment.PatientId;
        }

        public async Task<HealthRecordDto> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"healthrecords/{id}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Health record not found.");

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HealthRecordDto>(data);
        }

        public async Task AddHealthRecordAsync(CreateHealthRecordDto dto)
        {
            var response =
                await _httpClient.PostAsJsonAsync("healthrecords", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content.ReadAsStringAsync();

                throw new Exception(error);
            }
        }

        public async Task<List<HealthRecordDto>>
            GetPatientHistoryAsync(
                int patientId)
        {
            HttpResponseMessage response =
                await _httpClient.GetAsync(
                    $"healthrecords/patient/{patientId}");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <List<HealthRecordDto>>(json);
        }
    }

}