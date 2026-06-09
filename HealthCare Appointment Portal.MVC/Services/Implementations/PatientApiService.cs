using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal_MVC.Helpers;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal_MVC.Services
{
    public class PatientApiService : IPatientApiService
    {
        private readonly HttpClient _client;

        public PatientApiService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiBaseUrl"]);
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync(string searchTerm = null)
        {
            string url = "api/patients";

            // If a search term is provided, append it to the URL
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                url += $"?searchTerm={Uri.EscapeDataString(searchTerm)}";
            }

            HttpResponseMessage response = await _client.GetAsync(url);
            await ApiResponseHelper.EnsureSuccessAsync(response);

            return await response.Content.ReadAsAsync<IEnumerable<PatientDto>>();
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/patients/{id}");
            await ApiResponseHelper.EnsureSuccessAsync(response);

            return await response.Content.ReadAsAsync<PatientDto>();
        }

        public async Task<PatientDto> GetPatientByEmailAsync(string email)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/patients/email?email={email}");
            await ApiResponseHelper.EnsureSuccessAsync(response);

            return await response.Content.ReadAsAsync<PatientDto>();
        }

        public async Task<int> CreatePatientAsync(CreatePatientDto dto)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/patients", dto);
            await ApiResponseHelper.EnsureSuccessAsync(response);

            var createdPatient = await response.Content.ReadAsAsync<PatientDto>();
            return createdPatient.PatientId;
        }

        public async Task UpdatePatientAsync(int id, UpdatePatientDto dto)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/patients/{id}", dto);
            await ApiResponseHelper.EnsureSuccessAsync(response);
        }

        public async Task DeletePatientAsync(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/patients/{id}");
            await ApiResponseHelper.EnsureSuccessAsync(response);
        }
    }
}