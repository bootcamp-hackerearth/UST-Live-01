using HealthCare_Appointment_Portal_MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal_MVC.Helpers
{
    public static class ApiResponseHelper
    {
        public static async Task
            EnsureSuccessAsync(
                HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content
                        .ReadAsAsync<ApiErrorResponse>();

                throw new Exception(
                    error.Message);
            }
        }
    }
}