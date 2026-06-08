using HealthCare_Appointment_Portal.DTOs.UserDtos;
using HealthCare_Appointment_Portal_MVC.Helpers;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal_MVC.Services
{
    public class UserApiService
        : IUserApiService
    {
        private readonly HttpClient
            _client;

        public UserApiService()
            : this(
                CreateHttpClient())
        {
        }

        public UserApiService(
            HttpClient client)
        {
            _client =
                client ??
                throw new ArgumentNullException(
                    nameof(client));
        }

        private static HttpClient
            CreateHttpClient()
        {
            return new HttpClient
            {
                BaseAddress =
                    new Uri(
                        ConfigurationManager
                            .AppSettings[
                                "ApiBaseUrl"])
            };
        }

        public async Task<UserDto>
            GetUserByCodeAsync(
                string userCode)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"api/users/code/{userCode}");

            await ApiResponseHelper
                .EnsureSuccessAsync(
                    response);

            return await response
                .Content
                .ReadAsAsync<UserDto>();
        }
    }
}