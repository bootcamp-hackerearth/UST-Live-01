using HealthAxis.Shared.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HealthAxisWeb.Services
{
    public class DoctorApiService : IDoctorApiService
    {
        private readonly HttpClient _httpClient;

        public DoctorApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<DoctorDto> AddAsync(DoctorDto doctorDto)
        {
            var json = JsonConvert.SerializeObject(doctorDto);
            var contents = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/doctor", contents);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DoctorDto>(result);
        }

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/doctor");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DoctorDto>>(result);
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/doctor/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DoctorDto>(result);
        }

        public async Task<DoctorDto> UpdateAsync(int id, DoctorDto doctorDto)
        {
            var json = JsonConvert.SerializeObject(doctorDto); //got json
            var contents = new StringContent(json, Encoding.UTF8, "application/json");// to send we need a http content
            var response = await _httpClient.PutAsync($"api/doctor/{id}", contents);//send a request to server , _httpvlient is the communication mechanism here
            response.EnsureSuccessStatusCode(); // to check response is succes or not 
            var result = await response.Content.ReadAsStringAsync(); // getting response as a string
            return JsonConvert.DeserializeObject<DoctorDto>(result); // convert to dto
        }
    }
}