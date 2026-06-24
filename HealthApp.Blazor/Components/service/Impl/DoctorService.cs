
using HealthApp.Blazor.Components.service.Interface;
using HealthApp.Shared.Dto;
using System.Net.Http.Headers;
using System.Text.Json;


namespace HealthApp.Blazor.Components.service.Impl
{
    public class DoctorService : IDoctorService
    {

        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public DoctorService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private void AddAuthHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(_authService.Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _authService.Token);
            }
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            try
            {
                AddAuthHeader();

                var response = await _httpClient.GetAsync("api/doctors/all");

                Console.WriteLine($"STATUS: {response.StatusCode}");

                var raw = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"RAW DATA: {raw}");

                if (!response.IsSuccessStatusCode)
                    return new List<DoctorDto>();

                return JsonSerializer.Deserialize<List<DoctorDto>>(raw,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<DoctorDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return new List<DoctorDto>();
            }
        }


        public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
        {
            try
            {
                AddAuthHeader();

                var response = await _httpClient.GetAsync($"api/doctors/{id}");

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<DoctorDto>();
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> GetDoctorCountAsync()
        {
            try
            {
                var doctors = await GetAllDoctorsAsync();
                return doctors.Count;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<bool> UpdateDoctorAsync(int id, DoctorDto dto)
        {
            try
            {
                AddAuthHeader();

                var response = await _httpClient.PutAsJsonAsync($"api/doctors/{id}", dto);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }


        public async Task<List<DoctorDto>> SearchBySpecialisationAsync(string type)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/doctors/specialisation/{type}");

                if (!response.IsSuccessStatusCode)
                    return new List<DoctorDto>();

                return await response.Content.ReadFromJsonAsync<List<DoctorDto>>()
                       ?? new List<DoctorDto>();
            }
            catch
            {
                return new List<DoctorDto>();
            }
        }

        public async Task<bool> CreateDoctorAsync(DoctorDto dto)
        {
            try
            {
                AddAuthHeader();

                var request = new DoctorRegisterDto

                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    Specialisation = dto.Specialisation,
                    DoctorPhoneNumber = dto.DoctorPhoneNumber,
                    ConsultationFee = dto.ConsultationFee,
                    PracticeStartDate = dto.PracticeStartDate,
                    IsActive = dto.IsActive
                };

                var response = await _httpClient.PostAsJsonAsync("api/auth/doctorregister",request);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

    }
}
