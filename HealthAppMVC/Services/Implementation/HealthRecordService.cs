using HealthAppMVC.Helper;
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

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    ApiErrorHelper.GetApiMessage(error));
            }

            string data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<HealthRecordDto>>(data);
        }

        public async Task<int> GetPatientIdByAppointmentAsync(int appointmentId)
        {
            var response = await _httpClient.GetAsync(
                $"appointments/{appointmentId}");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    ApiErrorHelper.GetApiMessage(error));
            }

            string data = await response.Content.ReadAsStringAsync();

            AppointmentDto appointment =
                JsonConvert.DeserializeObject<AppointmentDto>(data);

            return appointment.PatientId;
        }

        public async Task<HealthRecordDto> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(
                $"healthrecords/{id}");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    ApiErrorHelper.GetApiMessage(error));
            }

            string data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<HealthRecordDto>(data);
        }

        public async Task AddHealthRecordAsync(CreateHealthRecordDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "healthrecords",
                dto);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    ApiErrorHelper.GetApiMessage(error));
            }
        }

        public async Task<List<HealthRecordDto>> GetPatientHistoryAsync(int patientId)
        {
            var response = await _httpClient.GetAsync(
                $"healthrecords/patient/{patientId}");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    ApiErrorHelper.GetApiMessage(error));
            }

            string data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<HealthRecordDto>>(data);
        }
    }
}