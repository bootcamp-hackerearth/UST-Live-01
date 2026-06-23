using Healthcare.Shared.DTOs.Authentication;
using Healthcare.Shared.DTOs.Doctor;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class DoctorService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;

    public DoctorService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public async Task<PagedDoctorResponse> GetDoctors()
    {
        var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        return await _http.GetFromJsonAsync<PagedDoctorResponse>("api/admin/doctors")
               ?? new PagedDoctorResponse();
    }


    public async Task DeleteDoctor(int id)
    {
        var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");

        if (!string.IsNullOrEmpty(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        await _http.DeleteAsync($"api/admin/doctors/{id}");
    }

    public async Task<bool> RegisterDoctor(DoctorRegisterDto dto)
    {
        var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");

        if (string.IsNullOrEmpty(token))
            throw new Exception("User not authenticated");

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.PostAsJsonAsync("api/auth/register-doctor", dto);

        return response.IsSuccessStatusCode;
    }

    public async Task UpdateStatus(int id, bool status)
    {
        await _http.PatchAsJsonAsync($"api/admin/doctors/{id}/status", status);
    }
}
