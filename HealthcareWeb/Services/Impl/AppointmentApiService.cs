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
    public class AppointmentApiService : IAppointmentApiService
    {
        private readonly HttpClient _httpClient;

        public AppointmentApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("appointments");
            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
        }

        public async Task<AppointmentDto> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync("appointments/" + id);
            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AppointmentDto>(json);
        }

        public async Task<List<AppointmentDto>> GetByPatientAsync(int patientId)
        {
            var response = await _httpClient.GetAsync("appointments/patient/" + patientId);
            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
        }

        public async Task<List<AppointmentDto>> GetByDoctorAsync(int doctorId)
        {
            var response = await _httpClient.GetAsync("appointments/doctor/" + doctorId);
            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
        }

        public async Task<List<AppointmentDto>> GetUpcomingByPatientAsync(int patientId)
        {
            var response = await _httpClient.GetAsync(
                "appointments/patient/" + patientId + "/upcoming");

            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
        }

        public async Task<List<AppointmentDto>> GetUpcomingByDoctorAsync(int doctorId)
        {
            var response = await _httpClient.GetAsync(
                "appointments/doctor/" + doctorId + "/upcoming");

            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
        }

        public async Task<List<AppointmentDto>> GetCancelledByPatientAsync(int patientId)
        {
            var response = await _httpClient.GetAsync(
                "appointments/patient/" + patientId + "/cancelled");

            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
        }

        public async Task<List<AppointmentDto>> GetCancelledByDoctorAsync(int doctorId)
        {
            var response = await _httpClient.GetAsync(
                "appointments/doctor/" + doctorId + "/cancelled");

            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AppointmentDto>>(json);
        }

        public async Task<AppointmentDto> BookAsync(BookAppointmentDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("appointments/book", content);
            await EnsureSuccessWithMessageAsync(response);

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AppointmentDto>(responseJson);
        }

        public async Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("appointments/" + id, content);
            await EnsureSuccessWithMessageAsync(response);

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AppointmentDto>(responseJson);
        }

        public async Task<AppointmentDto> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync("appointments/" + id);
            await EnsureSuccessWithMessageAsync(response);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AppointmentDto>(json);
        }

        public async Task<AppointmentDto> ConfirmAsync(int id, ConfirmAppointmentDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                "appointments/" + id + "/confirm",
                content);

            await EnsureSuccessWithMessageAsync(response);

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AppointmentDto>(responseJson);
        }

        public async Task<AppointmentDto> CancelByPatientAsync(int id, CancelByPatientDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                "appointments/" + id + "/cancel-by-patient",
                content);

            await EnsureSuccessWithMessageAsync(response);

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AppointmentDto>(responseJson);
        }

        public async Task<AppointmentDto> CancelByDoctorAsync(int id, CancelByDoctorDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                "appointments/" + id + "/cancel-by-doctor",
                content);

            await EnsureSuccessWithMessageAsync(response);

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AppointmentDto>(responseJson);
        }

        public async Task<AppointmentDto> CompleteAsync(int id, CompleteAppointmentDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                "appointments/" + id + "/complete",
                content);

            await EnsureSuccessWithMessageAsync(response);

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AppointmentDto>(responseJson);
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

                if (json["modelState"] != null)
                {
                    List<string> errors = new List<string>();

                    foreach (JProperty property in json["modelState"].Children<JProperty>())
                    {
                        foreach (JToken error in property.Value)
                        {
                            errors.Add(error.ToString());
                        }
                    }

                    if (errors.Count > 0)
                    {
                        return string.Join(" ", errors);
                    }
                }

                if (json["ModelState"] != null)
                {
                    List<string> errors = new List<string>();

                    foreach (JProperty property in json["ModelState"].Children<JProperty>())
                    {
                        foreach (JToken error in property.Value)
                        {
                            errors.Add(error.ToString());
                        }
                    }

                    if (errors.Count > 0)
                    {
                        return string.Join(" ", errors);
                    }
                }

                if (json["message"] != null)
                {
                    return json["message"].ToString();
                }

                if (json["Message"] != null)
                {
                    return json["Message"].ToString();
                }

                if (json["exceptionMessage"] != null)
                {
                    return json["exceptionMessage"].ToString();
                }

                if (json["ExceptionMessage"] != null)
                {
                    return json["ExceptionMessage"].ToString();
                }
            }
            catch
            {
                // If response is not JSON, return raw content.
            }

            return content;
        }
    }
}