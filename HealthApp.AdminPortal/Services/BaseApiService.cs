using HealthApp.AdminPortal.Services.Interface;
using HealthApp.Shared.Dtos;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace HealthApp.AdminPortal.Services.Impl
{
    public class BaseApiService
    {
        protected readonly HttpClient _http;
        private readonly ITokenService _tokenService;

        public BaseApiService(HttpClient http, ITokenService tokenService)
        {
            _http = http;
            _tokenService = tokenService;
        }

        protected async Task AddAuthHeaderAsync()
        {
            var token = await _tokenService.GetToken();

            _http.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        protected async Task<string> ReadErrorMessageAsync(HttpResponseMessage response)
        {
            try
            {
                var errorResponse =
                    await response.Content.ReadFromJsonAsync<ErrorResponse>();

                if (!string.IsNullOrWhiteSpace(errorResponse?.Message))
                {
                    return errorResponse.Message;
                }
            }
            catch (JsonException)
            {
                // Response was not in the expected ErrorResponse JSON format.
                // Fall back to raw response text.
            }
            catch (NotSupportedException)
            {
                // Response content type was not supported for JSON deserialization.
                // Fall back to raw response text.
            }

            try
            {
                var rawResponse = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(rawResponse))
                {
                    return rawResponse;
                }
            }
            catch
            {
                // Could not read response content.
                // Fall back to status-code based message below.
            }

            return response.StatusCode switch
            {
                HttpStatusCode.BadRequest =>
                    "Invalid request. Please check your input.",

                HttpStatusCode.Unauthorized =>
                    "Please login to continue.",

                HttpStatusCode.Forbidden =>
                    "You do not have permission to perform this action.",

                HttpStatusCode.NotFound =>
                    "Requested data was not found.",

                HttpStatusCode.Conflict =>
                    "This action could not be completed because of a conflict.",

                HttpStatusCode.InternalServerError =>
                    "Server error. Please try again later.",

                _ =>
                    "Something went wrong. Please try again."
            };
        }
    }
}