using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Healthaxis2.Shared.DTOs;
using Healthaxis2.Web.Services.Interfaces;

namespace Healthaxis2.Web.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl = "https://localhost:44366/api/appointments/";

        public AppointmentService()
        {
            _client = new HttpClient();
        }

        public async Task<List<AppointmentDto>> GetAll()
        {
            return await _client.GetFromJsonAsync<List<AppointmentDto>>(baseUrl);
        }

        public async Task<bool> Create(AppointmentDto dto)
        {
            var response = await _client.PostAsJsonAsync(baseUrl, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateStatus(int id, string status, string reason)
        {
            var url = $"{baseUrl}{id}/status?status={status}&reason={reason}";
            var response = await _client.PutAsync(url, null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _client.DeleteAsync(baseUrl + id);
            return response.IsSuccessStatusCode;
        }
    }
}