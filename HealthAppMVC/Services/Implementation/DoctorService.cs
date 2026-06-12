using HealthAppMVC.Services.Interface;
using HealthAppWebAPI.Models.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class DoctorService : IDoctorService
{
    private readonly HttpClient _httpClient;

    public DoctorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
    {
        var response = await _httpClient.GetAsync("doctors");

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        var data = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<DoctorDto>>(data);
    }

    public async Task<DoctorDto> GetDoctorByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"doctors/{id}");

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        var data = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<DoctorDto>(data);
    }

    public async Task AddDoctorAsync(CreateDoctorDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("doctors", dto);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }

    public async Task UpdateDoctorAsync(int id, CreateDoctorDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"doctors/{id}", dto);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }

    public async Task ChangeDoctorStatusAsync(int doctorId, bool isActive)
    {
        var request = new HttpRequestMessage(
            new HttpMethod("PATCH"),
            $"doctors/{doctorId}/status?isActive={isActive.ToString().ToLower()}");

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }

    public async Task<IEnumerable<DoctorDto>> SearchBySpecialisationAsync(string specialisation)
    {
        var response = await _httpClient.GetAsync(
            $"doctors/specialisation/{specialisation}");

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        var data = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<DoctorDto>>(data);
    }

    public async Task<IEnumerable<DoctorDto>> SearchByNameAsync(string name)
    {
        var response = await _httpClient.GetAsync(
            $"doctors/search?name={name}");

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        var data = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<DoctorDto>>(data);
    }
}