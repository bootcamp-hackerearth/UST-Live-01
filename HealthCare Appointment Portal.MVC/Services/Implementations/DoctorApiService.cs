using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal_MVC.Helpers;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal_MVC.Services
{
    public class DoctorApiService : IDoctorApiService
    {
        private readonly HttpClient
            _client;

        public DoctorApiService()
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
            IEnumerable<DoctorDto>>
            GetAllDoctorsAsync()
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    "api/doctors");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            return await response
                .Content
                .ReadAsAsync<
                    IEnumerable<DoctorDto>>();
        }

        public async Task<DoctorDto>
            GetDoctorByIdAsync(
                int id)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"api/doctors/{id}");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            return await response
                .Content
                .ReadAsAsync<
                    DoctorDto>();
        }

        public async Task<
            IEnumerable<DoctorDto>>
            GetDoctorsBySpecialisationAsync(
                Specialisation specialisation)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"api/doctors/specialisation/{specialisation}");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            return await response
                .Content
                .ReadAsAsync<
                    IEnumerable<DoctorDto>>();
        }

        public async Task<int>
            CreateDoctorAsync(
                CreateDoctorDto dto)
        {
            HttpResponseMessage response =
                await _client.PostAsJsonAsync(
                    "api/doctors",
                    dto);

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            var createdDoctor =
                await response
                    .Content
                    .ReadAsAsync<
                        DoctorDto>();

            return createdDoctor
                .DoctorId;
        }

        public async Task
            UpdateDoctorAsync(
                int id,
                UpdateDoctorDto dto)
        {
            HttpResponseMessage response =
                await _client.PutAsJsonAsync(
                    $"api/doctors/{id}",
                    dto);

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);
        }

        public async Task
            DeleteDoctorAsync(
                int id)
        {
            HttpResponseMessage response =
                await _client.DeleteAsync(
                    $"api/doctors/{id}");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);
        }
    }
}