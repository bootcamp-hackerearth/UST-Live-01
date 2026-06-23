using HealthCareApp.AdminBlazor.Services.Interfaces;
using HealthCareApp.Shared.Dtos.Appointments;
using HealthCareApp.Shared.Dtos.Pagination;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace HealthCareApp.AdminBlazor.Services.Impl
{
    public class AppointmentAdminService : IAppointmentAdminService
    {
        private const string TokenStorageKey = "token";

        private const string AppointmentsEndpoint = "api/Appointments";

        private readonly HttpClient _httpClient;

        private readonly IJSRuntime _jsRuntime;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public AppointmentAdminService(
            HttpClient httpClient,
            IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;

            _jsRuntime = jsRuntime;
        }

        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
        {
            var query = new AppointmentPaginationQueryDto
            {
                PageNumber = 1,

                PageSize = 100
            };

            var response = await GetAppointmentsPagedAsync(query);

            return response.Items ?? new List<AppointmentDto>();
        }

        public async Task<PagedResponse<AppointmentDto>> GetAppointmentsPagedAsync(
            AppointmentPaginationQueryDto query)
        {
            string endpoint = BuildAppointmentsEndpoint(query);

            using var response = await SendAuthorizedRequestAsync(
                HttpMethod.Get,
                endpoint);

            EnsureAuthorizedResponse(
                response,
                "Your admin session is not authorized to load appointment data.");

            response.EnsureSuccessStatusCode();

            var pagedResponse = await response.Content
                .ReadFromJsonAsync<PagedResponse<AppointmentDto>>(JsonOptions);

            return pagedResponse ?? new PagedResponse<AppointmentDto>
            {
                Items = new List<AppointmentDto>(),

                PageNumber = query.PageNumber,

                PageSize = query.PageSize,

                TotalRecords = 0,

                TotalPages = 0
            };
        }

        public async Task<AppointmentDto?> GetAppointmentByIdAsync(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                return null;
            }

            using var response = await SendAuthorizedRequestAsync(
                HttpMethod.Get,
                $"{AppointmentsEndpoint}/{appointmentId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            EnsureAuthorizedResponse(
                response,
                "Your admin session is not authorized to load appointment details.");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AppointmentDto>(JsonOptions);
        }

        public async Task<AppointmentDto> CreateAppointmentAsync(BookAppointmentDto request)
        {
            using var response = await SendAuthorizedRequestAsync(
                HttpMethod.Post,
                AppointmentsEndpoint,
                request);

            EnsureAuthorizedResponse(
                response,
                "Your admin session is not authorized to create appointments.");

            response.EnsureSuccessStatusCode();

            var appointment = await response.Content.ReadFromJsonAsync<AppointmentDto>(JsonOptions);

            return appointment ?? new AppointmentDto();
        }

        public Task<AppointmentDto?> UpdateAppointmentAsync(
            int appointmentId,
            UpdateAppointmentDto request)
        {
            return Task.FromResult<AppointmentDto?>(null);
        }

        public Task<bool> DeleteAppointmentAsync(int appointmentId)
        {
            return Task.FromResult(false);
        }

        private static string BuildAppointmentsEndpoint(AppointmentPaginationQueryDto query)
        {
            var queryParameters = new List<string>
            {
                $"pageNumber={query.PageNumber}",

                $"pageSize={query.PageSize}"
            };

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                queryParameters.Add($"searchTerm={Uri.EscapeDataString(query.SearchTerm)}");
            }

            if (query.PatientId is not null)
            {
                queryParameters.Add($"patientId={query.PatientId.Value}");
            }

            if (query.DoctorId is not null)
            {
                queryParameters.Add($"doctorId={query.DoctorId.Value}");
            }

            if (query.Status is not null)
            {
                queryParameters.Add($"status={query.Status.Value}");
            }

            if (query.ScheduledDate is not null)
            {
                queryParameters.Add($"scheduledDate={query.ScheduledDate.Value:yyyy-MM-dd}");
            }

            if (query.UpcomingOnly is not null)
            {
                queryParameters.Add($"upcomingOnly={query.UpcomingOnly.Value.ToString().ToLowerInvariant()}");
            }

            return $"{AppointmentsEndpoint}?{string.Join("&", queryParameters)}";
        }

        private async Task<HttpResponseMessage> SendAuthorizedRequestAsync(
            HttpMethod method,
            string endpoint,
            object? requestData = null)
        {
            var token = await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                TokenStorageKey);

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