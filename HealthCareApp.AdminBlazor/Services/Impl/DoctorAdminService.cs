using HealthCareApp.AdminBlazor.Services.Interfaces;
using HealthCareApp.Shared.Dtos.DoctorLeaves;
using HealthCareApp.Shared.Dtos.Doctors;
using HealthCareApp.Shared.Dtos.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HealthCareApp.AdminBlazor.Services.Impl
{
    public class DoctorAdminService : BaseApiService, IDoctorAdminService
    {
        private const string AdminDoctorsEndpoint = "api/Admin/doctors";
        private const string DoctorsEndpoint = "api/Doctors";
        private const string DoctorLeavesEndpoint = "api/DoctorLeaves";


        public DoctorAdminService(
      HttpClient httpClient,
      IJSRuntime jsRuntime,
      NavigationManager navigationManager)
      : base(httpClient, jsRuntime, navigationManager)
        {
        }
        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var query = new DoctorPaginationQueryDto
            {
                PageNumber = 1,
                PageSize = 100
            };

            var response = await GetDoctorsPagedAsync(query);

            return response.Items;
        }
        public async Task<List<DoctorLeaveDto>> GetDoctorLeavesAsync(int doctorId)
        {
            if (doctorId <= 0)
            {
                return new List<DoctorLeaveDto>();
            }

            return await GetAuthorizedAsync<List<DoctorLeaveDto>>(
                $"{DoctorLeavesEndpoint}/doctor/{doctorId}",
                "Your admin session is not authorized to load doctor leave history.");
        }

        public async Task<PagedResponse<DoctorDto>> GetDoctorsPagedAsync(DoctorPaginationQueryDto query)
        {
            string endpoint = BuildDoctorsEndpoint(query);

            return await GetAuthorizedAsync<PagedResponse<DoctorDto>>(
                endpoint,
                "Your admin session is not authorized to load doctor data.");
        }

        public async Task<DoctorDto?> GetDoctorByIdAsync(int doctorId)
        {
            if (doctorId <= 0)
            {
                return null;
            }

            return await GetAuthorizedAsync<DoctorDto>(
                $"{DoctorsEndpoint}/{doctorId}",
                "Your admin session is not authorized to load doctor details.");
        }

        public async Task<DoctorCreatedResponseDto> CreateDoctorAsync(CreateDoctorDto doctorDto)
        {
            return await PostAuthorizedAsync<CreateDoctorDto, DoctorCreatedResponseDto>(
                AdminDoctorsEndpoint,
                doctorDto,
                "Your admin session is not authorized to create doctor accounts.");
        }

        public async Task<DoctorDto?> UpdateDoctorAsync(int doctorId, UpdateDoctorDto doctorDto)
        {
            if (doctorId <= 0)
            {
                return null;
            }

            return await PutAuthorizedAsync<UpdateDoctorDto, DoctorDto>(
                $"{AdminDoctorsEndpoint}/{doctorId}",
                doctorDto,
                "Your admin session is not authorized to update doctor profiles.");
        }

        public async Task<DoctorDto?> ToggleDoctorStatusAsync(int doctorId)
        {
            var doctor = await GetDoctorByIdAsync(doctorId);

            if (doctor is null)
            {
                return null;
            }

            var updateDoctorDto = new UpdateDoctorDto
            {
                FullName = doctor.FullName,
                Specialisation = doctor.Specialisation,
                PracticeStartDate = GetApproximatePracticeStartDate(doctor.YearsOfExperience),
                ConsultationFee = doctor.ConsultationFee,
                IsActive = !doctor.IsActive
            };

            return await UpdateDoctorAsync(doctorId, updateDoctorDto);
        }

   

        private static string BuildDoctorsEndpoint(DoctorPaginationQueryDto query)
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

            if (query.Specialisation is not null)
            {
                queryParameters.Add($"specialisation={query.Specialisation}");
            }

            if (query.IsActive is not null)
            {
                queryParameters.Add($"isActive={query.IsActive.Value.ToString().ToLowerInvariant()}");
            }

            return $"{AdminDoctorsEndpoint}?{string.Join("&", queryParameters)}";
        }

        private static DateTime GetApproximatePracticeStartDate(int yearsOfExperience)
        {
            if (yearsOfExperience <= 0)
            {
                return DateTime.Today;
            }

            return DateTime.Today.AddYears(-yearsOfExperience);
        }
    }
}
