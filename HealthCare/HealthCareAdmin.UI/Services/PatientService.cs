//using Healthcare.Shared.DTOs.Patient;
//using Healthcare.Shared.DTOs.Appointment;
//using Microsoft.JSInterop;
//using System.Net.Http.Headers;
//using System.Net.Http.Json;
//using Healthcare.Shared.DTOs.Doctor;

//public class PatientService
//{
//    private readonly HttpClient _http;
//    private readonly IJSRuntime _js;

//    public PatientService(HttpClient http, IJSRuntime js)
//    {
//        _http = http;
//        _js = js;
//    }

//    // ✅ Common method to set JWT token
//    private async Task SetAuthHeader()
//    {
//        var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");

//        if (!string.IsNullOrEmpty(token))
//        {
//            _http.DefaultRequestHeaders.Authorization =
//                new AuthenticationHeaderValue("Bearer", token);
//        }
//    }

//    // ✅ GET ALL PATIENTS (Admin)
//    public async Task<PagedDoctorResponse> GetPatients()
//    {
//        await SetAuthHeader();

//        return await _http.GetFromJsonAsync<PagedDoctorResponse>("api/admin/patients")
//               ?? new PagedPatientResponse();
//    }

//    // ✅ DELETE PATIENT
//    public async Task DeletePatient(int id)
//    {
//        await SetAuthHeader();

//        await _http.DeleteAsync($"api/admin/patients/{id}");
//    }

//    // ✅ TOGGLE ACTIVE / INACTIVE
//    public async Task UpdateStatus(int id, bool status)
//    {
//        await SetAuthHeader();

//        await _http.PatchAsJsonAsync($"api/admin/patients/{id}/status", status);
//    }

//    // ✅ GET APPOINTMENTS OF A PATIENT
//    public async Task<List<AppointmentDto>> GetPatientAppointments(int patientId)
//    {
//        await SetAuthHeader();

//        return await _http.GetFromJsonAsync<List<AppointmentDto>>(
//            $"api/admin/patients/{patientId}/appointments")
//               ?? new List<AppointmentDto>();
//    }
//}