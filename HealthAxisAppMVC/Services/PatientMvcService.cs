using HealthAxisApp.Shared.DTOs;
using HealthAxisAppMVC.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace HealthAxisAppMVC.Services
{
    public class PatientMvcService : ApiServiceBase, IPatientMvcService
    {
        public IEnumerable<PatientDto> GetAll(string insuranceStatus = null, string searchText = null)
        {
            using (var client = CreateClient())
            {
                string url = "patients?";

                if (!string.IsNullOrWhiteSpace(insuranceStatus))
                {
                    url += "insuranceStatus=" + insuranceStatus + "&";
                }

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    url += "searchText=" + searchText;
                }

                url = url.TrimEnd('&', '?');

                var response = client.GetAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return new List<PatientDto>();
                }

                string json = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<IEnumerable<PatientDto>>(json);
            }
        }

        public PatientDto GetById(int id)
        {
            using (var client = CreateClient())
            {
                var response = client.GetAsync("patients/" + id).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string json = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<PatientDto>(json);
            }
        }

        public bool Create(
    PatientDto dto,
    out string error,
    out int patientId)
        {
            return SendWithId(
                "patients",
                dto,
                "POST",
                "PatientId",
                out error,
                out patientId);
        }

        public bool Update(PatientDto dto, out string error)
        {
            return Send(
                "patients/" + dto.PatientId,
                dto,
                "PUT",
                out error);
        }

        public bool Delete(int id, out string error)
        {
            error = string.Empty;

            using (var client = CreateClient())
            {
                var response = client.DeleteAsync("patients/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                error = response.Content.ReadAsStringAsync().Result;
                return false;
            }
        }
        private bool SendWithId(
    string url,
    object dto,
    string method,
    string idPropertyName,
    out string error,
    out int createdId)
        {
            error = string.Empty;
            createdId = 0;

            using (var client = CreateClient())
            {
                string json = JsonConvert.SerializeObject(dto);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage response;

                if (method == "POST")
                {
                    response = client.PostAsync(url, content).Result;
                }
                else
                {
                    response = client.PutAsync(url, content).Result;
                }

                string responseText = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = JObject.Parse(responseText);

                    if (responseJson[idPropertyName] != null)
                    {
                        createdId = responseJson[idPropertyName].Value<int>();
                    }

                    return true;
                }

                error = ExtractApiErrorMessage(responseText);
                return false;
            }
        }
        private string ExtractApiErrorMessage(string apiResponse)
        {
            if (string.IsNullOrWhiteSpace(apiResponse))
            {
                return "An unexpected error occurred.";
            }

            try
            {
                var json = JObject.Parse(apiResponse);

                if (json["Message"] != null)
                {
                    return json["Message"].ToString();
                }

                if (json["message"] != null)
                {
                    return json["message"].ToString();
                }

                if (json["MessageDetail"] != null)
                {
                    return json["MessageDetail"].ToString();
                }
            }
            catch
            {
                // Not JSON.
            }

            return apiResponse.Replace("\"", "");
        }
        private bool Send(
            string url,
            object dto,
            string method,
            out string error)
        {
            error = string.Empty;

            using (var client = CreateClient())
            {
                string json = JsonConvert.SerializeObject(dto);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage response;

                if (method == "POST")
                {
                    response = client.PostAsync(url, content).Result;
                }
                else
                {
                    response = client.PutAsync(url, content).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                error = response.Content.ReadAsStringAsync().Result;
                return false;
            }
        }
    }
}