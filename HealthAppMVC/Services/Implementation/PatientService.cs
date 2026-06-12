using HealthAppMVC.Models;
using HealthAppMVC.Services.Interface;
using HealthAppWebAPI.Models.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace HealthAppMVC.Services.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly HttpClient _httpClient;

        public PatientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var response = await _httpClient.GetAsync("patients");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PatientDto>>(data);
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"patients/{id}");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PatientDto>(data);
        }

        public async Task RegisterPatientAsync(CreatePatientDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("patients", dto);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task UpdatePatientAsync(int id, CreatePatientDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"patients/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public async Task<IEnumerable<PatientDto>> SearchByNameAsync(string name)
        {
            var response = await _httpClient.GetAsync(
                $"patients/search?name={name}");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PatientDto>>(data);
        }

        public async Task<int> GetAppointmentCountAsync(int patientId)
        {
            var response = await _httpClient.GetAsync(
                $"patients/{patientId}/appointments/count");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(data);
        }
    }
}