using Microsoft.JSInterop;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HealthCareApp.AdminBlazor.Services.Impl
{
    public abstract class BaseApiService
    {
        private const string TokenStorageKey = "token";

        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        protected BaseApiService(
            HttpClient httpClient,
            IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        protected async Task<TResponse> GetAuthorizedAsync<TResponse>(
            string endpoint,
            string unauthorizedMessage)
            where TResponse : new()
        {
            using var response = await SendAuthorizedAsync(
                HttpMethod.Get,
                endpoint);

            EnsureAuthorizedResponse(response, unauthorizedMessage);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TResponse>();

            return result ?? new TResponse();
        }

        protected async Task<TResponse> PostAuthorizedAsync<TRequest, TResponse>(
            string endpoint,
            TRequest requestData,
            string unauthorizedMessage)
            where TResponse : new()
        {
            using var response = await SendAuthorizedAsync(
                HttpMethod.Post,
                endpoint,
                requestData);

            EnsureAuthorizedResponse(response, unauthorizedMessage);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TResponse>();

            return result ?? new TResponse();
        }

        protected async Task<TResponse> PutAuthorizedAsync<TRequest, TResponse>(
            string endpoint,
            TRequest requestData,
            string unauthorizedMessage)
            where TResponse : new()
        {
            using var response = await SendAuthorizedAsync(
                HttpMethod.Put,
                endpoint,
                requestData);

            EnsureAuthorizedResponse(response, unauthorizedMessage);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TResponse>();

            return result ?? new TResponse();
        }

        protected async Task<TResponse> DeleteAuthorizedAsync<TResponse>(
            string endpoint,
            string unauthorizedMessage)
            where TResponse : new()
        {
            using var response = await SendAuthorizedAsync(
                HttpMethod.Delete,
                endpoint);

            EnsureAuthorizedResponse(response, unauthorizedMessage);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TResponse>();

            return result ?? new TResponse();
        }

        private async Task<HttpResponseMessage> SendAuthorizedAsync(
            HttpMethod method,
            string endpoint,
            object? requestData = null)
        {
            var token = await GetTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new UnauthorizedAccessException(
                    "Authentication token was not found. Please login again.");
            }

            using var request = new HttpRequestMessage(method, endpoint);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            if (requestData is not null)
            {
                request.Content = JsonContent.Create(requestData);
            }

            return await _httpClient.SendAsync(request);
        }

        private async Task<string?> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                TokenStorageKey);
        }

        private static void EnsureAuthorizedResponse(
            HttpResponseMessage response,
            string unauthorizedMessage)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized ||
                response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedAccessException(unauthorizedMessage);
            }
        }
    }
}