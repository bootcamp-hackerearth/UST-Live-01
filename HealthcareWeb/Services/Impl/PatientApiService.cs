using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedClasses.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareWeb.Services
{
    public class PatientApiService : IPatientApiService
    {
        private readonly HttpClient _httpClient;

        public PatientApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PatientDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("patients");

            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<PatientDto>>(json);
        }

        public async Task<PatientDto> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync("patients/" + id);

            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PatientDto>(json);
        }

        public async Task<List<PatientDto>> SearchAsync(string query)
        {
            string encodedQuery = Uri.EscapeDataString(query ?? string.Empty);

            var response = await _httpClient.GetAsync("patients/search?query=" + encodedQuery);

            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<PatientDto>>(json);
        }
        public async Task<PatientDto> AddAsync(CreatePatientDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("patients", content);

            await EnsureSuccessWithMessageAsync(response);

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PatientDto>(responseJson);
        }

        public async Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("patients/" + id, content);

            await EnsureSuccessWithMessageAsync(response);

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PatientDto>(responseJson);
        }

        public async Task<PatientDto> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync("patients/" + id);

            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PatientDto>(json);
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
                JObject json = JObject.Parse(content);

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
                // Response was not JSON. Return raw response text.
            }

            return content;
        }
    }
}