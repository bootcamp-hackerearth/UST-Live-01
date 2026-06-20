using System.Security.Claims;

namespace HealthAxisCore_Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetRole(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value;
        }

        public static int? GetPatientId(this ClaimsPrincipal user)
        {
            var value = user.FindFirst("PatientId")?.Value;

            return int.TryParse(value, out var patientId)
                ? patientId
                : null;
        }

        public static int? GetDoctorId(this ClaimsPrincipal user)
        {
            var value = user.FindFirst("DoctorId")?.Value;

            return int.TryParse(value, out var doctorId)
                ? doctorId
                : null;
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.GetRole() == "Admin";
        }

        public static bool IsDoctor(this ClaimsPrincipal user)
        {
            return user.GetRole() == "Doctor";
        }

        public static bool IsPatient(this ClaimsPrincipal user)
        {
            return user.GetRole() == "Patient";
        }
    }
}