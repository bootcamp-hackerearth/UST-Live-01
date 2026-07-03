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

        private const string DoctorEndpoint = "api/admin/doctors";
        private const string DoctorPagedEndpoint = "api/admin/doctors/paged";

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
            _httpClient = httpClient;
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            using var cancellationTokenSource = CreateTimeoutToken();

            var doctors = await _httpClient.GetFromJsonAsync<List<DoctorDto>>(
                DoctorEndpoint,
                JsonOptions,
                cancellationTokenSource.Token);

            return doctors ?? new List<DoctorDto>();
        }

        public async Task<PagedResponseDto<DoctorDto>> GetDoctorsPageAsync(
            int pageNumber)
        {
            using var cancellationTokenSource = CreateTimeoutToken();

            var endpoint =
                $"{DoctorPagedEndpoint}?pageNumber={pageNumber}&pageSize={DoctorPageSize}";

            var response = await _httpClient.GetFromJsonAsync<PagedResponseDto<DoctorDto>>(
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

        public async Task<(bool Success, string Message, DoctorCreatedDto? Doctor)> RegisterDoctorAsync(
            CreateDoctorDto doctorDto)
        {
            ArgumentNullException.ThrowIfNull(doctorDto);

            using var cancellationTokenSource = CreateTimeoutToken();

            try
            {
                using var response = await _httpClient.PostAsJsonAsync(
                    DoctorEndpoint,
                    doctorDto,
                    JsonOptions,
                    cancellationTokenSource.Token);

                if (response.IsSuccessStatusCode)
                {
                    var createdDoctor = await response.Content.ReadFromJsonAsync<DoctorCreatedDto>(
                        JsonOptions,
                        cancellationTokenSource.Token);

                    return (
                        true,
                        "Doctor registered successfully.",
                        createdDoctor);
                }

                var errorMessage = await ReadErrorMessageAsync(response);

                return (false, errorMessage, null);
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

        public async Task<(bool Success, string Message)> UpdateDoctorAsync(
            int doctorId,
            CreateDoctorDto doctorDto)
        {
            ArgumentNullException.ThrowIfNull(doctorDto);

            using var cancellationTokenSource = CreateTimeoutToken();

            var updateDto = new UpdateDoctorDto
            {
                FullName = doctorDto.FullName,
                Email = doctorDto.Email,
                Specialisation = doctorDto.Specialisation,
                YearsOfExperience = doctorDto.YearsOfExperience,
                ConsultationFee = doctorDto.ConsultationFee,
                IsActive = doctorDto.IsActive
            };

            using var response = await _httpClient.PutAsJsonAsync(
                $"{DoctorEndpoint}/{doctorId}",
                updateDto,
                JsonOptions,
                cancellationTokenSource.Token);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Doctor updated successfully.");
            }

            var errorMessage = await ReadErrorMessageAsync(response);

            return (false, errorMessage);
        }

        public async Task<(bool Success, string Message)> DeleteDoctorAsync(
            int doctorId)
        {
            using var cancellationTokenSource = CreateTimeoutToken();

            using var response = await _httpClient.DeleteAsync(
                $"{DoctorEndpoint}/{doctorId}",
                cancellationTokenSource.Token);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Doctor deleted successfully.");
            }

            var errorMessage = await ReadErrorMessageAsync(response);

            return (false, errorMessage);
        }

        public async Task<(bool Success, string Message)> ToggleDoctorStatusAsync(
            DoctorDto doctor)
        {
            ArgumentNullException.ThrowIfNull(doctor);

            using var cancellationTokenSource = CreateTimeoutToken();

            var updateDto = new UpdateDoctorDto
            {
                FullName = doctor.FullName,
                Email = doctor.Email,
                Specialisation = doctor.Specialisation,
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = !doctor.IsActive
            };

            using var response = await _httpClient.PutAsJsonAsync(
                $"{DoctorEndpoint}/{doctor.DoctorId}",
                updateDto,
                JsonOptions,
                cancellationTokenSource.Token);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Doctor status updated successfully.");
            }

            var errorMessage = await ReadErrorMessageAsync(response);

            return (false, errorMessage);
        }

        private static CancellationTokenSource CreateTimeoutToken()
        {
            return new CancellationTokenSource(
                TimeSpan.FromSeconds(RequestTimeoutSeconds));
        }

        private static async Task<string> ReadErrorMessageAsync(
            HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return "Request failed.";
            }

            try
            {
                using var document = JsonDocument.Parse(content);

                if (document.RootElement.TryGetProperty(
                        "message",
                        out var messageElement))
                {
                    return messageElement.GetString() ?? "Request failed.";
                }
            }
            catch (JsonException)
            {
                return content;
            }

            return "Request failed.";
        }
    }
}