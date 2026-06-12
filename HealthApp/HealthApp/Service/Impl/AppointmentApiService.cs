using HealthApp.Service.Interface;
using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthApp.Service.Impl
{
    public class AppointmentApiService : IAppointmentApiService
    {
        private readonly string baseUrl = "https://localhost:44339/api/appointments";

        private string ExtractError(string error)
        {
            return error.Replace("{", "")
                        .Replace("}", "")
                        .Replace("\"", "")
                        .Replace("Message:", "")
                        .Trim();
        }

        public async Task<List<AppointmentDto>> GetAll()
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync(baseUrl);

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(ExtractError(error));
                }

                return await res.Content.ReadAsAsync<List<AppointmentDto>>();
            }
        }

        public async Task<AppointmentDto> GetById(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/{id}");

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(ExtractError(error));
                }

                return await res.Content.ReadAsAsync<AppointmentDto>();
            }
        }

        public async Task<List<AppointmentDto>> GetByPatient(int patientId)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/patient/{patientId}");

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(ExtractError(error));
                }

                return await res.Content.ReadAsAsync<List<AppointmentDto>>();
            }
        }

        public async Task Create(AppointmentDto dto)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.PostAsJsonAsync(baseUrl, dto);

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(ExtractError(error));
                }
            }
        }

        public async Task Confirm(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.PutAsync($"{baseUrl}/{id}/confirm", null);

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(ExtractError(error));
                }
            }
        }

        public async Task Cancel(int id, string reason)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.PutAsync($"{baseUrl}/{id}/cancel?reason={Uri.EscapeDataString(reason)}", null);

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(ExtractError(error));
                }
            }
        }

        public async Task<List<string>> CheckAvailability(int doctorId, DateTime date)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/availability?doctorId={doctorId}&date={date:yyyy-MM-dd}");

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(ExtractError(error));
                }

                return await res.Content.ReadAsAsync<List<string>>();
            }
        }

        public async Task MarkCompleted(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.PutAsync($"{baseUrl}/{id}/complete", null);

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(ExtractError(error));
                }
            }
        }

        public async Task<PatientLookupDto> GetPatient(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/patient/{id}");

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(ExtractError(error));
                }

                return await res.Content.ReadAsAsync<PatientLookupDto>();
            }
        }


        public async Task<List<DoctorLookupDto>> GetDoctors(string specialization)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/doctors?specialization={specialization}");

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(ExtractError(error));
                }

                return await res.Content.ReadAsAsync<List<DoctorLookupDto>>();
            }
        }


        public async Task<List<string>> GetAvailableSlots(int doctorId, DateTime date)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync($"{baseUrl}/availability?doctorId={doctorId}&date={date:yyyy-MM-dd}");

                if (!res.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadAsStringAsync();
                    throw new Exception(ExtractError(error));
                }

                return await res.Content.ReadAsAsync<List<string>>();
            }
        }
    }
}
