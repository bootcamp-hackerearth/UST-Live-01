using Healthcare.Shared.DTOs.Doctor;
using Healthcare.Shared.DTOs.Patient;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class PatientService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;

    public PatientService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public async Task<PagedPatientResponse> GetPatients()
    {
        var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);


        return await _http.GetFromJsonAsync<PagedPatientResponse>("api/admin/patients")
               ?? new PagedPatientResponse();

    }

    public async Task DeletePatient(int id)
    {
        var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        await _http.DeleteAsync($"api/admin/patients/{id}");
    }

    public async Task UpdateInsuranceStatus(int id, bool status)
    {
        var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        await _http.PatchAsJsonAsync($"api/admin/patients/{id}/insurance", status);
    }
    public async Task UpdateStatus(int id, bool status)
    {
        var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.PatchAsJsonAsync(
            $"api/admin/patients/{id}/status",
            status
        );

        response.EnsureSuccessStatusCode(); 
    }

}
