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
        private readonly HttpClient _client;

        public AppointmentService(HttpClient client)
        {
            _client = client;
        }

        private readonly string baseUrl = "https://localhost:44384/api/appointments";

        //  GET PATIENT APPOINTMENTS (PAGED)
        public async Task<PagedResult<AppointmentDto>> GetAppointmentsAsync(
            int? patientId,
            int? doctorId,
            string status,
            int pageNumber,
            int pageSize)
        {
            string url;

            //  Case 1: Patient search
            if (patientId.HasValue)
            {
                url = $"{baseUrl}/patient/{patientId.Value}?pageNumber={pageNumber}&pageSize={pageSize}";
            }
            //  Case 2: Doctor search
            else if (doctorId.HasValue)
            {
                url = $"{baseUrl}/doctor/{doctorId.Value}?pageNumber={pageNumber}&pageSize={pageSize}";
            }
            //  Case 3: Default → upcoming
            else
            {
                url = $"{baseUrl}/upcoming?pageNumber={pageNumber}&pageSize={pageSize}";
            }

            //  Add status filter if present
            if (!string.IsNullOrEmpty(status))
            {
                url += $"&status={Uri.EscapeDataString(status)}";
            }

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new PagedResult<AppointmentDto>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PagedResult<AppointmentDto>>(json)
                   ?? new PagedResult<AppointmentDto>();
        }

        //  BY DATE
        public async Task<IEnumerable<AppointmentDto>> GetByDateAsync(DateTime date)
        {
            var res = await _client.GetAsync($"{baseUrl}/date?date={date:yyyy-MM-dd}");

            if (!res.IsSuccessStatusCode) return new List<AppointmentDto>();

            var json = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<AppointmentDto>>(json);
        }

        //  BOOK
        public async Task<bool> BookAsync(AppointmentDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await _client.PostAsync(baseUrl, content);

            if (res.IsSuccessStatusCode)
                return true;

            // READ ERROR MESSAGE FROM API
            var errorMessage = await res.Content.ReadAsStringAsync();

            // Optional: clean it if API returns JSON
            try
            {
                var errorObj = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                errorMessage = errorObj?.message ?? errorMessage;
            }
            catch
            {
                // keep raw message
            }

            throw new Exception(errorMessage); // 🔥 IMPORTANT
        }

        //  CONFIRM
        public async Task<bool> ConfirmAsync(int id)
        {
            var res = await _client.PutAsync($"{baseUrl}/{id}/confirm", null);
            return res.IsSuccessStatusCode;
        }

        //  CANCEL
        public async Task<bool> CancelAsync(int id, string reason)
        {
            var obj = new { Reason = reason };
            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await _client.PutAsync($"{baseUrl}/{id}/cancel", content);
            return res.IsSuccessStatusCode;
        }

        //  Get available slots
        public async Task<List<string>> GetAvailableSlotsAsync(int doctorId, DateTime date)
        {
            var response = await _client.GetAsync(
                $"appointments/slots?doctorId={doctorId}&date={date:yyyy-MM-dd}");

            if (!response.IsSuccessStatusCode)
                return new List<string>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<string>>(json);
        }

        // UPCOMING APPOINTMENTS (PAGED)
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

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new PagedResult<AppointmentDto>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PagedResult<AppointmentDto>>(json)
                   ?? new PagedResult<AppointmentDto>();
        }
    }
}