using System.Security.Claims;

namespace HealthApp.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static int? GetPatientId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue("PatientId");

            return int.TryParse(value, out var patientId)
                ? patientId
                : null;
        }

        public static int? GetDoctorId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue("DoctorId");

            return int.TryParse(value, out var doctorId)
                ? doctorId
                : null;
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole("Admin");
        }

        public static bool IsPatient(this ClaimsPrincipal user)
        {
            return user.IsInRole("Patient");
        }

        public static bool IsDoctor(this ClaimsPrincipal user)
        {
            return user.IsInRole("Doctor");
        }
    }
}
