using HealthAxis.Shared.DTO.AdminDtos;
using HealthAxis.Shared.DTO.AuthDtos;
using HealthAxis.Shared.DTO.CommonDtos;
using HealthAxis.Shared.DTO.DoctorDtos;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthAxis_Admin.Services
{
    public sealed class DoctorAdminService
    {
        private const int DoctorPageSize = 6;
        private const int RequestTimeoutSeconds = 20;

        private const string DoctorEndpoint =
            "/api/admin/doctors";

        private const string DoctorPagedEndpoint =
            "/api/admin/doctors/paged";

        private const string ResetPasswordEndpoint =
            "/api/Auth/admin/reset-password";

        private const string AppointmentDetailsEndpoint =
            "/api/admin/reports/appointments/details";

        private const string RequestFailedMessage =
            "Request failed.";

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        private readonly HttpClient _httpClient;

        public DoctorAdminService(HttpClient httpClient)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            _httpClient = httpClient;
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            using var cancellationTokenSource =
                CreateTimeoutToken();

            var doctors =
                await _httpClient.GetFromJsonAsync<List<DoctorDto>>(
                    DoctorEndpoint,
                    JsonOptions,
                    cancellationTokenSource.Token);

            return doctors ?? new List<DoctorDto>();
        }

        public async Task<PagedResponseDto<DoctorDto>>
            GetDoctorsPageAsync(int pageNumber)
        {
            using var cancellationTokenSource =
                CreateTimeoutToken();

            var endpoint =
                $"{DoctorPagedEndpoint}?pageNumber={pageNumber}" +
                $"&pageSize={DoctorPageSize}";

            var response =
                await _httpClient
                    .GetFromJsonAsync<PagedResponseDto<DoctorDto>>(
                        endpoint,
                        JsonOptions,
                        cancellationTokenSource.Token);

            return response ?? new PagedResponseDto<DoctorDto>
            {
                Items = new List<DoctorDto>(),
                PageNumber = pageNumber,
                PageSize = DoctorPageSize,
                TotalRecords = 0
            };
        }

        public async Task<List<AdminAppointmentDetailDto>>
            GetDoctorAppointmentsAsync(int doctorId)
        {
            if (doctorId <= 0)
            {
                return new List<AdminAppointmentDetailDto>();
            }

            using var cancellationTokenSource =
                CreateTimeoutToken();

            var appointments =
                await _httpClient
                    .GetFromJsonAsync<List<AdminAppointmentDetailDto>>(
                        AppointmentDetailsEndpoint,
                        JsonOptions,
                        cancellationTokenSource.Token);

            return appointments?
                .Where(appointment =>
                    appointment.DoctorId == doctorId)
                .OrderByDescending(appointment =>
                    appointment.ScheduledDate)
                .ThenBy(appointment =>
                    appointment.TimeSlot)
                .ToList()
                ?? new List<AdminAppointmentDetailDto>();
        }

        public async Task<(
            bool Success,
            string Message,
            DoctorCreatedDto? Doctor)> RegisterDoctorAsync(
                CreateDoctorDto doctorDto)
        {
            ArgumentNullException.ThrowIfNull(doctorDto);

            using var cancellationTokenSource =
                CreateTimeoutToken();

            try
            {
                using var response =
                    await _httpClient.PostAsJsonAsync(
                        DoctorEndpoint,
                        doctorDto,
                        JsonOptions,
                        cancellationTokenSource.Token);

                if (response.IsSuccessStatusCode)
                {
                    var createdDoctor =
                        await response.Content
                            .ReadFromJsonAsync<DoctorCreatedDto>(
                                JsonOptions,
                                cancellationTokenSource.Token);

                    return (
                        true,
                        "Doctor registered successfully.",
                        createdDoctor);
                }

                var errorMessage =
                    await ReadErrorMessageAsync(response);

                return (
                    false,
                    errorMessage,
                    null);
            }
            catch (TaskCanceledException)
            {
                return (
                    false,
                    "API request timed out. Please check if HealthAxis.API is running.",
                    null);
            }
            catch (HttpRequestException)
            {
                return (
                    false,
                    "Unable to connect to API. Please run HealthAxis.API and try again.",
                    null);
            }
            catch (JsonException)
            {
                return (
                    false,
                    "Doctor registered, but response format is not matching frontend DTO.",
                    null);
            }
        }

        public async Task<(bool Success, string Message)>
            UpdateDoctorAsync(
                int doctorId,
                CreateDoctorDto doctorDto)
        {
            ArgumentNullException.ThrowIfNull(doctorDto);

            using var cancellationTokenSource =
                CreateTimeoutToken();

            var updateDto = new UpdateDoctorDto
            {
                FullName = doctorDto.FullName,
                Email = doctorDto.Email,
                Specialisation = doctorDto.Specialisation,
                YearsOfExperience =
                    doctorDto.YearsOfExperience,
                ConsultationFee =
                    doctorDto.ConsultationFee,
                IsActive = doctorDto.IsActive
            };

            using var response =
                await _httpClient.PutAsJsonAsync(
                    $"{DoctorEndpoint}/{doctorId}",
                    updateDto,
                    JsonOptions,
                    cancellationTokenSource.Token);

            if (response.IsSuccessStatusCode)
            {
                return (
                    true,
                    "Doctor updated successfully.");
            }

            var errorMessage =
                await ReadErrorMessageAsync(response);

            return (false, errorMessage);
        }

        public async Task<(bool Success, string Message)>
            ToggleDoctorStatusAsync(DoctorDto doctor)
        {
            ArgumentNullException.ThrowIfNull(doctor);

            using var cancellationTokenSource =
                CreateTimeoutToken();

            var updateDto = new UpdateDoctorDto
            {
                FullName = doctor.FullName,
                Email = doctor.Email,
                Specialisation = doctor.Specialisation,
                YearsOfExperience =
                    doctor.YearsOfExperience,
                ConsultationFee =
                    doctor.ConsultationFee,
                IsActive = !doctor.IsActive
            };

            using var response =
                await _httpClient.PutAsJsonAsync(
                    $"{DoctorEndpoint}/{doctor.DoctorId}",
                    updateDto,
                    JsonOptions,
                    cancellationTokenSource.Token);

            if (response.IsSuccessStatusCode)
            {
                return (
                    true,
                    "Doctor status updated successfully.");
            }

            var errorMessage =
                await ReadErrorMessageAsync(response);

            return (false, errorMessage);
        }

        public async Task<(bool Success, string Message)>
            ResetPasswordAsync(AdminResetPasswordDto request)
        {
            ArgumentNullException.ThrowIfNull(request);

            using var cancellationTokenSource =
                CreateTimeoutToken();

            try
            {
                using var response =
                    await _httpClient.PostAsJsonAsync(
                        ResetPasswordEndpoint,
                        request,
                        JsonOptions,
                        cancellationTokenSource.Token);

                var message =
                    await ReadErrorMessageAsync(response);

                if (response.IsSuccessStatusCode)
                {
                    return (
                        true,
                        string.IsNullOrWhiteSpace(message)
                            ? "Password reset successfully."
                            : message);
                }

                return (false, message);
            }
            catch (TaskCanceledException)
            {
                return (
                    false,
                    "API request timed out. Please check if HealthAxis.API is running.");
            }
            catch (HttpRequestException)
            {
                return (
                    false,
                    "Unable to connect to API. Please run HealthAxis.API and try again.");
            }
        }

        private static CancellationTokenSource
            CreateTimeoutToken()
        {
            return new CancellationTokenSource(
                TimeSpan.FromSeconds(
                    RequestTimeoutSeconds));
        }

        private static async Task<string>
            ReadErrorMessageAsync(HttpResponseMessage response)
        {
            var content =
                await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return response.IsSuccessStatusCode
                    ? string.Empty
                    : RequestFailedMessage;
            }

            try
            {
                using var document =
                    JsonDocument.Parse(content);

                if (document.RootElement.TryGetProperty(
                        "message",
                        out var messageElement))
                {
                    return messageElement.GetString()
                        ?? RequestFailedMessage;
                }

                if (document.RootElement.TryGetProperty(
                        "title",
                        out var titleElement))
                {
                    return titleElement.GetString()
                        ?? RequestFailedMessage;
                }

                if (document.RootElement.TryGetProperty(
                        "errors",
                        out var errorsElement))
                {
                    return ReadValidationErrors(
                        errorsElement);
                }
            }
            catch (JsonException)
            {
                return content;
            }

            return response.IsSuccessStatusCode
                ? string.Empty
                : RequestFailedMessage;
        }

        private static string ReadValidationErrors(
            JsonElement errorsElement)
        {
            var errors = errorsElement
                .EnumerateObject()
                .Select(property => property.Value)
                .Where(value =>
                    value.ValueKind == JsonValueKind.Array)
                .SelectMany(value =>
                    value.EnumerateArray())
                .Select(error =>
                    error.GetString())
                .Where(errorMessage =>
                    !string.IsNullOrWhiteSpace(
                        errorMessage))
                .Select(errorMessage =>
                    errorMessage!)
                .ToList();

            return errors.Count == 0
                ? "Validation failed."
                : string.Join(" ", errors);
        }
    }
}
