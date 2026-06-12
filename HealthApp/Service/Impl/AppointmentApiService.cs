using HealthApp.Service.Interface;
using HealthApp.Shared.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Service.Impl
{
    public class AppointmentApiService : IAppointmentApiService
    {
        private readonly string baseUrl = "https://localhost:44317/api/appointments";

        private string ExtractError(string error)
        {
            return error.Replace("{", "")
                        .Replace("}", "")
                        .Replace("\"", "")
                        .Replace("Message:", "")
                        .Replace("message:", "")
                        .Trim();
        }

        public async Task<List<AppointmentDto>> GetAll()
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync(baseUrl);
                var json = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                    throw new Exception(ExtractError(json));

                return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
            }
        }

        public async Task<AppointmentDto> GetById(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/{id}");
                var json = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                    throw new Exception(ExtractError(json));

                return JsonConvert.DeserializeObject<AppointmentDto>(json);
            }
        }

        public async Task<List<AppointmentDto>> GetByPatient(int patientId)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/patient/{patientId}");
                var json = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                    throw new Exception(ExtractError(json));

                return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
            }
        }

        // ✅ CREATE
        public async Task Create(AppointmentDto dto)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PostAsync(baseUrl, content);
                var responseText = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                    throw new Exception(ExtractError(responseText));
            }
        }

        // ✅ CONFIRM
        public async Task Confirm(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.PutAsync($"{baseUrl}/{id}/confirm", null);
                var responseText = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                    throw new Exception(ExtractError(responseText));
            }
        }

        // ✅ CANCEL
        public async Task Cancel(int id, string reason)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.PutAsync(
                    $"{baseUrl}/{id}/cancel?reason={Uri.EscapeDataString(reason)}",
                    null);

                var responseText = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                    throw new Exception(ExtractError(responseText));
            }
        }

        // ✅ CHECK AVAILABILITY
        public async Task<List<string>> CheckAvailability(int doctorId, DateTime date)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync(
                    $"{baseUrl}/availability?doctorId={doctorId}&date={date:yyyy-MM-dd}");

                var json = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                    throw new Exception(ExtractError(json));

                return JsonConvert.DeserializeObject<List<string>>(json);
            }
        }

        // ✅ COMPLETE
        public async Task MarkCompleted(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PutAsync($"{baseUrl}/{id}/complete", null);
                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(ExtractError(responseText));
                }
            }
        }
    }
}
