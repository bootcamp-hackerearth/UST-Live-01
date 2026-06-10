using HealthCare_Appointment_Portal.DTOs.UserDtos;
using HealthCare_Appointment_Portal_MVC.Helpers;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal_MVC.Services
{
    public class UserApiService : IUserApiService
    {
        private readonly HttpClient _client;

        public UserApiService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(
                ConfigurationManager.AppSettings["ApiBaseUrl"]);
        }

        public async Task<UserDto> GetUserByCodeAsync(string userCode)
        {
            HttpResponseMessage response =
                await _client.GetAsync($"api/users/code/{userCode}");

            // Handle 404 (User not found)
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            // Handle other errors
            await ApiResponseHelper.EnsureSuccessAsync(response);

            return await response.Content.ReadAsAsync<UserDto>();
        }
    }
}
