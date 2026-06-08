using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal_MVC.Helpers;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal_MVC.Services
{
    public class AppointmentApiService : IAppointmentApiService
    {
        private readonly HttpClient
            _client;

        public AppointmentApiService()
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
            IEnumerable<AppointmentDto>>
            GetAllAppointmentsAsync()
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    "api/appointments");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            return await response
                .Content
                .ReadAsAsync<
                    IEnumerable<AppointmentDto>>();
        }

        public async Task<AppointmentDto>
            GetAppointmentByIdAsync(
                int id)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"api/appointments/{id}");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            return await response
                .Content
                .ReadAsAsync<
                    AppointmentDto>();
        }

        public async Task<int>
            CreateAppointmentAsync(
                CreateAppointmentDto dto)
        {
            HttpResponseMessage response =
                await _client.PostAsJsonAsync(
                    "api/appointments",
                    dto);

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            var appointment =
                await response.Content
                    .ReadAsAsync<
                        AppointmentDto>();

            return appointment
                .AppointmentId;
        }

        public async Task
            UpdateAppointmentAsync(
                int id,
                UpdateAppointmentDto dto)
        {
            HttpResponseMessage response =
                await _client.PutAsJsonAsync(
                    $"api/appointments/{id}",
                    dto);

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);
        }

        public async Task
            DeleteAppointmentAsync(
                int id)
        {
            HttpResponseMessage response =
                await _client.DeleteAsync(
                    $"api/appointments/{id}");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);
        }

        public async Task<
            IEnumerable<AppointmentDto>>
            GetAppointmentsByPatientAsync(
                int patientId)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"api/appointments/patient/{patientId}");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            return await response
                .Content
                .ReadAsAsync<
                    IEnumerable<AppointmentDto>>();
        }

        public async Task<AppointmentDto>
            GetNextAppointmentByPatientAsync(
                int patientId)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"api/appointments/patient/{patientId}/next");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            return await response
                .Content
                .ReadAsAsync<
                    AppointmentDto>();
        }

        public async Task<
            IEnumerable<AppointmentDto>>
            GetAppointmentsByDoctorAsync(
                int doctorId)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"api/appointments/doctor/{doctorId}");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            return await response
                .Content
                .ReadAsAsync<
                    IEnumerable<AppointmentDto>>();
        }

        public async Task<
            IEnumerable<AppointmentDto>>
            GetTodayScheduleAsync(
                int doctorId)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"api/appointments/doctor/{doctorId}/today");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            return await response
                .Content
                .ReadAsAsync<
                    IEnumerable<AppointmentDto>>();
        }

        public async Task<
            IEnumerable<AppointmentDto>>
            GetWeeklyScheduleAsync(
                int doctorId)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"api/appointments/doctor/{doctorId}/weekly");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            return await response
                .Content
                .ReadAsAsync<
                    IEnumerable<AppointmentDto>>();
        }

        public async Task
            ConfirmAppointmentAsync(
                int appointmentId)
        {
            HttpResponseMessage response =
                await _client.PutAsync(
                    $"api/appointments/{appointmentId}/confirm",
                    null);

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);
        }

        public async Task
            CompleteAppointmentAsync(
                int appointmentId)
        {
            HttpResponseMessage response =
                await _client.PutAsync(
                    $"api/appointments/{appointmentId}/complete",
                    null);

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);
        }

        public async Task
            CancelAppointmentAsync(
                int appointmentId,
                string reason)
        {
            HttpResponseMessage response =
                await _client.PutAsync(
                    $"api/appointments/{appointmentId}/cancel?reason={Uri.EscapeDataString(reason)}",
                    null);

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);
        }
    }
}