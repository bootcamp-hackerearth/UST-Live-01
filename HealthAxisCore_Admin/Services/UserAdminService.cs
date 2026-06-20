using HealthAxisCore_Admin.Models;

namespace HealthAxisCore_Admin.Services
{
    public class UserAdminService
    {
        private static readonly List<UserDto> Users = new()
        {
            new UserDto
            {
                Id = "admin-1",
                Email = "admin@healthcare.com",
                Role = "Admin",
                IsActive = true
            },
            new UserDto
            {
                Id = "patient-1",
                Email = "arun.kumar@example.com",
                Role = "Patient",
                IsActive = true
            },
            new UserDto
            {
                Id = "patient-2",
                Email = "meera.nair@example.com",
                Role = "Patient",
                IsActive = true
            },
            new UserDto
            {
                Id = "patient-3",
                Email = "rahul.menon@example.com",
                Role = "Patient",
                IsActive = true
            },
            new UserDto
            {
                Id = "doctor-1",
                Email = "arvind.sharma@healthaxis.com",
                Role = "Doctor",
                IsActive = true
            },
            new UserDto
            {
                Id = "doctor-2",
                Email = "neha.kapoor@healthaxis.com",
                Role = "Doctor",
                IsActive = true
            },
            new UserDto
            {
                Id = "doctor-3",
                Email = "inactive.doctor@healthaxis.com",
                Role = "Doctor",
                IsActive = false
            }
        };

        public Task<List<UserDto>> GetUsersAsync(string? role = null)
        {
            /*
             * ============================================================
             * TEMPORARY DISCONNECTED VERSION
             * ============================================================
             * Returns hardcoded users instead of calling:
             * GET api/admin/users
             * GET api/admin/users?role=Doctor
             * ============================================================
             */

            var result = Users.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(role))
            {
                result = result.Where(user => user.Role == role);
            }

            return Task.FromResult(
                result
                    .OrderBy(user => user.Role)
                    .ThenBy(user => user.Email)
                    .ToList());
        }
    }
}