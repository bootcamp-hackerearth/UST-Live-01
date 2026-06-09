using HealthAxis.Shared.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class PatientApiService : IPatientApiService
{
    private readonly HttpClient _httpClient;

    public PatientApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PatientDto>> GetAllAsync()
    {
        var res = await _httpClient.GetAsync("api/patient");
        res.EnsureSuccessStatusCode();

        return JsonConvert.DeserializeObject<List<PatientDto>>(
            await res.Content.ReadAsStringAsync());
    }

    public async Task<PatientDto> GetByIdAsync(int id)
    {
        var res = await _httpClient.GetAsync($"api/patient/{id}");
        if (!res.IsSuccessStatusCode) return null;

        return JsonConvert.DeserializeObject<PatientDto>(
            await res.Content.ReadAsStringAsync());
    }

    public async Task AddAsync(PatientDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var res = await _httpClient.PostAsync("api/patient",
            new StringContent(json, Encoding.UTF8, "application/json"));

        res.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(int id, PatientDto dto)
    {
        var json = JsonConvert.SerializeObject(dto);
        var res = await _httpClient.PutAsync($"api/patient/{id}",
            new StringContent(json, Encoding.UTF8, "application/json"));

        res.EnsureSuccessStatusCode();
    }
}