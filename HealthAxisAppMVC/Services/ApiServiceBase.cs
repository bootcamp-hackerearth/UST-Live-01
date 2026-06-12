using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HealthAxisAppMVC.Services
{
    public abstract class ApiServiceBase
    {
        protected HttpClient CreateClient()
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var client = new HttpClient(handler);

            var baseUrl = ConfigurationManager.AppSettings["HealthAxisApiBaseUrl"];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new Exception("HealthAxisApiBaseUrl is missing in Web.config");
            }

            client.BaseAddress = new Uri(baseUrl);

            client.Timeout = TimeSpan.FromSeconds(30); // ✅ explicit timeout

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

    }
}