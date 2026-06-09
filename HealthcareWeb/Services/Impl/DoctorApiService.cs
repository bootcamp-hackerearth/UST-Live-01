using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedClasses.Dtos;
using SharedClasses.Enums;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareWeb.Services
{
    public class DoctorApiService : IDoctorApiService
    {
        private readonly HttpClient _httpClient;

        public DoctorApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("doctors");
            await ThrowIfErrorAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<DoctorDto>>(json);
        }

        public async Task<List<DoctorDto>> GetAllActiveAsync()
        {
            var response = await _httpClient.GetAsync("doctors/active");
            await ThrowIfErrorAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<DoctorDto>>(json);
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync("doctors/" + id);
            await ThrowIfErrorAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<DoctorDto>(json);
        }

        public async Task<List<DoctorDto>> SearchBySpecialisationAsync(Specialisation specialisation)
        {
            var response = await _httpClient.GetAsync("doctors/specialisation/" + (int)specialisation);
            await ThrowIfErrorAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<DoctorDto>>(json);
        }

        public async Task<DoctorDto> AddAsync(CreateDoctorDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("doctors", content);
            await ThrowIfErrorAsync(response);

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<DoctorDto>(responseJson);
        }

        public async Task<DoctorDto> UpdateAsync(int id, UpdateDoctorDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("doctors/" + id, content);
            await ThrowIfErrorAsync(response);

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<DoctorDto>(responseJson);
        }

        public async Task<DoctorDto> DeactivateAsync(int id)
        {
            var response = await _httpClient.DeleteAsync("doctors/" + id);
            await ThrowIfErrorAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<DoctorDto>(json);
        }

        public async Task<DoctorDto> ReactivateAsync(int id)
        {
            var response = await _httpClient.PostAsync("doctors/" + id + "/reactivate", null);
            await ThrowIfErrorAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<DoctorDto>(json);
        }

        private async Task ThrowIfErrorAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            string content = await response.Content.ReadAsStringAsync();
            string message = ExtractErrorMessage(content);

            throw new Exception(message);
        }

        private string ExtractErrorMessage(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return "The request could not be completed.";
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
            }
            catch
            {
            }

            return content;
        }
    }
}
