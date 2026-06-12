using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Healthaxis2.Shared.DTOs;
using Healthaxis2.Web.Services.Interfaces;

namespace Healthaxis2.Web.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl = "https://localhost:44366/api/doctors/";

        public DoctorService()
        {
            _client = new HttpClient();
        }

        public async Task<List<DoctorDto>> GetAll()
        {
            return await _client.GetFromJsonAsync<List<DoctorDto>>(baseUrl);
        }

        public async Task<DoctorDto> GetById(int id)
        {
            return await _client.GetFromJsonAsync<DoctorDto>(baseUrl + id);
        }

        public async Task<bool> Create(DoctorDto dto)
        {
            var response = await _client.PostAsJsonAsync(baseUrl, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(int id, DoctorDto dto)
        {
            var response = await _client.PutAsJsonAsync(baseUrl + id, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _client.DeleteAsync(baseUrl + id);
            return response.IsSuccessStatusCode;
        }
    }
}