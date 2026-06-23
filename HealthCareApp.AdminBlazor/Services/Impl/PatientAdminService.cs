using HealthCareApp.AdminBlazor.Services.Interfaces;
using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Dtos.Patients;
using HealthCareApp.Shared.Enums;
using Microsoft.JSInterop;

namespace HealthCareApp.AdminBlazor.Services.Impl
{
    public class PatientAdminService : BaseApiService, IPatientAdminService
    {
        private const string PatientsEndpoint = "api/Patients";

        public PatientAdminService(
            HttpClient httpClient,
            IJSRuntime jsRuntime)
            : base(httpClient, jsRuntime)
        {
        }

        public async Task<List<PatientDto>> GetAllPatientsAsync()
        {
            var query = new PatientPaginationQueryDto
            {
                PageNumber = 1,
                PageSize = 100
            };

            var response = await GetPatientsPagedAsync(query);

            return response.Items;
        }

        public async Task<PagedResponse<PatientDto>> GetPatientsPagedAsync(PatientPaginationQueryDto query)
        {
            string endpoint = BuildPatientsEndpoint(query);

            return await GetAuthorizedAsync<PagedResponse<PatientDto>>(
                endpoint,
                "Your admin session is not authorized to load patient data.");
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int patientId)
        {
            if (patientId <= 0)
            {
                return null;
            }

            return await GetAuthorizedAsync<PatientDto>(
                $"{PatientsEndpoint}/{patientId}",
                "Your admin session is not authorized to load patient details.");
        }

        public async Task<PatientDto?> CreatePatientAsync(CreatePatientDto patientDto)
        {
            return await PostAuthorizedAsync<CreatePatientDto, PatientDto>(
                PatientsEndpoint,
                patientDto,
                "Your admin session is not authorized to create patient profiles.");
        }

        public async Task<PatientDto?> UpdatePatientAsync(int patientId, UpdatePatientDto patientDto)
        {
            if (patientId <= 0)
            {
                return null;
            }

            return await PutAuthorizedAsync<UpdatePatientDto, PatientDto>(
                $"{PatientsEndpoint}/{patientId}",
                patientDto,
                "Your admin session is not authorized to update patient profiles.");
        }

        private static string BuildPatientsEndpoint(PatientPaginationQueryDto query)
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

            if (query.Gender is not null)
            {
                queryParameters.Add($"gender={query.Gender}");
            }

            if (query.HasInsurance is not null)
            {
                queryParameters.Add($"hasInsurance={query.HasInsurance.Value.ToString().ToLowerInvariant()}");
            }

            return $"{PatientsEndpoint}?{string.Join("&", queryParameters)}";
        }
    }
}