using HealthCare.Shared;
using HealthCare.Shared.DTOs.Appointment;
using HealthCare.Web.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Web.Services
{
    public class AppointmentService : IAppointmentService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string baseUrl = "https://localhost:44384/api/appointments";

        
        public async Task<PagedResult<AppointmentDto>> GetAppointmentsAsync(
            int? patientId,
            int? doctorId,
            string status,
            int pageNumber,
            int pageSize)
        {
            string url;

            
            if (patientId.HasValue)
            {
                url = $"{baseUrl}/patient/{patientId.Value}?pageNumber={pageNumber}&pageSize={pageSize}";
            }
           
            else if (doctorId.HasValue)
            {
                url = $"{baseUrl}/doctor/{doctorId.Value}?pageNumber={pageNumber}&pageSize={pageSize}";
            }
            
            else
            {
                url = $"{baseUrl}/upcoming?pageNumber={pageNumber}&pageSize={pageSize}";
            }

            
            if (!string.IsNullOrEmpty(status))
            {
                url += $"&status={Uri.EscapeDataString(status)}";
            }

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new PagedResult<AppointmentDto>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PagedResult<AppointmentDto>>(json)
                   ?? new PagedResult<AppointmentDto>();
        }

        public async Task<IEnumerable<AppointmentDto>> GetTodayAppointmentsAsync(int doctorId)
        {
            var res = await client.GetAsync($"{baseUrl}/doctor/{doctorId}/today");

            if (!res.IsSuccessStatusCode) return new List<AppointmentDto>();

            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<AppointmentDto>>(json);
        }

        public async Task<IEnumerable<AppointmentDto>> GetWeeklyAppointmentsAsync(int doctorId)
        {
            var res = await client.GetAsync($"{baseUrl}/doctor/{doctorId}/week");

            if (!res.IsSuccessStatusCode) return new List<AppointmentDto>();

            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<AppointmentDto>>(json);
        }

        
        public async Task<IEnumerable<AppointmentDto>> GetByDateAsync(DateTime date)
        {
            var res = await client.GetAsync($"{baseUrl}/date?date={date:yyyy-MM-dd}");

            if (!res.IsSuccessStatusCode) return new List<AppointmentDto>();

            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<AppointmentDto>>(json);
        }

       
        public async Task<bool> BookAsync(AppointmentDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PostAsync(baseUrl, content);

            return res.IsSuccessStatusCode;
        }

        
        public async Task<bool> ConfirmAsync(int id)
        {
            var res = await client.PutAsync($"{baseUrl}/{id}/confirm", null);
            return res.IsSuccessStatusCode;
        }

       
        public async Task<bool> CancelAsync(int id, string reason)
        {
            var obj = new { Reason = reason };
            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PutAsync($"{baseUrl}/{id}/cancel", content);
            return res.IsSuccessStatusCode;
        }

        
        public async Task<List<string>> GetAvailableSlotsAsync(int doctorId, DateTime date)
        {
            var response = await client.GetAsync(
                $"appointments/slots?doctorId={doctorId}&date={date:yyyy-MM-dd}");

            if (!response.IsSuccessStatusCode)
                return new List<string>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<string>>(json);
        }

       
        public async Task<PagedResult<AppointmentDto>> GetUpcomingAppointmentsAsync(
            int? patientId,
            int? doctorId,
            string status,
            int pageNumber,
            int pageSize)
        {
            var queryParams = new List<string>
    {
        $"pageNumber={pageNumber}",
        $"pageSize={pageSize}"
    };

            if (patientId.HasValue)
                queryParams.Add($"patientId={patientId.Value}");

            if (doctorId.HasValue)
                queryParams.Add($"doctorId={doctorId.Value}");

            if (!string.IsNullOrEmpty(status))
                queryParams.Add($"status={Uri.EscapeDataString(status)}");

            string url = $"{baseUrl}/upcoming?" + string.Join("&", queryParams);

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new PagedResult<AppointmentDto>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PagedResult<AppointmentDto>>(json)
                   ?? new PagedResult<AppointmentDto>();
        }
    }
}