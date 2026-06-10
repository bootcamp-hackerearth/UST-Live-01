using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
using HealthCare_Appointment_Portal_MVC.Helpers;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal_MVC.Services
{
    public class HealthRecordApiService : IHealthRecordApiService

    {
        private readonly HttpClient
            _client;

        public HealthRecordApiService()
        {
            _client =
                new HttpClient();

            _client.BaseAddress =
                new Uri(
                    ConfigurationManager
                        .AppSettings[
                            "ApiBaseUrl"]);
        }

        public async Task<
            IEnumerable<HealthRecordDto>>
            GetAllHealthRecordsAsync()
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    "api/health-records");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            return await response
                .Content
                .ReadAsAsync<
                    IEnumerable<
                        HealthRecordDto>>();
        }

        public async Task<
            HealthRecordDto>
            GetHealthRecordByIdAsync(
                int id)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"api/health-records/{id}");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            return await response
                .Content
                .ReadAsAsync<
                    HealthRecordDto>();
        }

        public async Task<
            IEnumerable<HealthRecordDto>>
            GetRecordsByPatientAsync(
                int patientId)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"api/health-records/patient/{patientId}");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            return await response
                .Content
                .ReadAsAsync<
                    IEnumerable<
                        HealthRecordDto>>();
        }

        public async Task<
            IEnumerable<HealthRecordDto>>
            GetRecordsByDoctorAsync(
                int doctorId)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"api/health-records/doctor/{doctorId}");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            return await response
                .Content
                .ReadAsAsync<
                    IEnumerable<
                        HealthRecordDto>>();
        }

        public async Task<int>
            CreateHealthRecordAsync(
                CreateHealthRecordDto dto)
        {
            HttpResponseMessage response =
                await _client.PostAsJsonAsync(
                    "api/health-records",
                    dto);

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            dynamic result =
                await response
                    .Content
                    .ReadAsAsync<dynamic>();

            return (int)
                result.RecordId;
        }

        public async Task
            UpdateHealthRecordAsync(
                int id,
                UpdateHealthRecordDto dto)
        {
            HttpResponseMessage response =
                await _client.PutAsJsonAsync(
                    $"api/health-records/{id}",
                    dto);

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);
        }

        public async Task
            DeleteHealthRecordAsync(
                int id)
        {
            HttpResponseMessage response =
                await _client.DeleteAsync(
                    $"api/health-records/{id}");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);
        }

        public Task<object> GetHealthRecordsByPatientAsync(int value)
        {
            throw new NotImplementedException();
        }
    }
}