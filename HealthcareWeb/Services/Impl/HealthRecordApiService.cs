using Newtonsoft.Json;
using SharedClasses.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareWeb.Services
{
    public class HealthRecordApiService : IHealthRecordApiService
    {
        private readonly HttpClient _httpClient;

        public HealthRecordApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<HealthRecordDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("healthrecords");
            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<HealthRecordDto>>(json);
        }

        public async Task<HealthRecordDto> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync("healthrecords/" + id);
            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HealthRecordDto>(json);
        }

        public async Task<List<HealthRecordDto>> GetByPatientAsync(int patientId)
        {
            var response = await _httpClient.GetAsync("healthrecords/patient/" + patientId);
            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<HealthRecordDto>>(json);
        }

        public async Task<List<HealthRecordDto>> GetByDoctorAsync(int doctorId)
        {
            var response = await _httpClient.GetAsync("healthrecords/doctor/" + doctorId);
            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<HealthRecordDto>>(json);
        }

        public async Task<List<HealthRecordDto>> GetByAppointmentAsync(int appointmentId)
        {
            var response = await _httpClient.GetAsync(
                "healthrecords/appointment/" + appointmentId);

            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<HealthRecordDto>>(json);
        }

        public async Task<List<HealthRecordDto>> SearchAsync(string query)
        {
            string encodedQuery = Uri.EscapeDataString(query ?? string.Empty);

            var response = await _httpClient.GetAsync(
                "healthrecords/search?query=" + encodedQuery);

            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<HealthRecordDto>>(json);
        }

        public async Task<List<HealthRecordDto>> SearchByPatientAsync(int patientId, string query)
        {
            string encodedQuery = Uri.EscapeDataString(query ?? string.Empty);

            string url = "healthrecords/patient/" + patientId + "/search?query=" + encodedQuery;

            var response = await _httpClient.GetAsync(url);

            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<HealthRecordDto>>(json);
        }

        public async Task<List<HealthRecordDto>> SearchByDoctorAsync(int doctorId, string query)
        {
            string encodedQuery = Uri.EscapeDataString(query ?? string.Empty);
            string url = "healthrecords/doctor/" + doctorId + "/search?query=" + encodedQuery;
            var response = await _httpClient.GetAsync(url);
            await EnsureSuccessWithMessageAsync(response);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<HealthRecordDto>>(json);
        }

        public async Task<HealthRecordDto> AddAsync(AddHealthRecordDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("healthrecords", content);
            await EnsureSuccessWithMessageAsync(response);

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HealthRecordDto>(responseJson);
        }

        public async Task<HealthRecordDto> UpdateAsync(int id, UpdateHealthRecordDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("healthrecords/" + id, content);
            await EnsureSuccessWithMessageAsync(response);

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HealthRecordDto>(responseJson);
        }

        public async Task<HealthRecordDto> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync("healthrecords/" + id);
            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HealthRecordDto>(json);
        }

        private async Task EnsureSuccessWithMessageAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            string content = await response.Content.ReadAsStringAsync();
            string message = ExtractErrorMessage(content);

            if (string.IsNullOrWhiteSpace(message))
            {
                message = "API request failed.";
            }

            throw new Exception(message);
        }

        private string ExtractErrorMessage(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return "API request failed.";
            }

            try
            {
                var json = Newtonsoft.Json.Linq.JObject.Parse(content);

                if (json["message"] != null)
                {
                    return json["message"].ToString();
                }

                if (json["Message"] != null)
                {
                    return json["Message"].ToString();
                }

                if (json["modelState"] != null)
                {
                    return json["modelState"].ToString();
                }

                if (json["ModelState"] != null)
                {
                    return json["ModelState"].ToString();
                }
            }
            catch
            {
                // Response was not JSON. Return raw content.
            }

            return content;
        }
    }
}