using HealthCare_Appointment_Portal_MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal_MVC.Helpers
{
    public static class ApiResponseHelper
    {
        public static async Task EnsureSuccessAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            string message = "Something went wrong.";

            try
            {
                var error =
                    await response.Content.ReadAsAsync<ApiErrorResponse>();

                if (error != null && !string.IsNullOrEmpty(error.Message))
                {
                    message = error.Message;
                }
            }
            catch
            {
                message = response.ReasonPhrase;
            }

            throw new Exception(message);
        }
    }
}
