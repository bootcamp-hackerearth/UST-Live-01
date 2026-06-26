using HealthApp.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Data
{
    public static class DemoUserSeeder
    {
        public static async Task SeedDoctorAndPatientUsersAsync(
            IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<HealthAppDbContext>();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            await SeedDoctorUsersAsync(
                context,
                userManager);

            await SeedPatientUsersAsync(
                context,
                userManager);
        }

        private static async Task SeedDoctorUsersAsync(
            HealthAppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            var doctors = await context.Doctors
                .AsNoTracking()
                .ToListAsync();

            foreach (var doctor in doctors)
            {
                if (string.IsNullOrWhiteSpace(doctor.DoctorEmail))
                {
                    continue;
                }

                var existingUser = await userManager.FindByEmailAsync(
                    doctor.DoctorEmail);

                if (existingUser != null)
                {
                    if (existingUser.DoctorId == null)
                    {
                        existingUser.DoctorId = doctor.DoctorId;
                        existingUser.PatientId = null;

                        await userManager.UpdateAsync(existingUser);
                    }

                    if (!await userManager.IsInRoleAsync(existingUser, "Doctor"))
                    {
                        await userManager.AddToRoleAsync(
                            existingUser,
                            "Doctor");
                    }

                    continue;
                }

                var password = GenerateDoctorPassword(
                    doctor.FullName);

                var user = new ApplicationUser
                {
                    UserName = doctor.DoctorEmail,
                    Email = doctor.DoctorEmail,
                    EmailConfirmed = true,
                    DoctorId = doctor.DoctorId,
                    PatientId = null
                };

                var createResult = await userManager.CreateAsync(
                    user,
                    password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        " ",
                        createResult.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"Failed to create doctor user for {doctor.DoctorEmail}. {errors}");
                }

                var roleResult = await userManager.AddToRoleAsync(
                    user,
                    "Doctor");

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        " ",
                        roleResult.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"Failed to assign Doctor role for {doctor.DoctorEmail}. {errors}");
                }
            }
        }

        private static async Task SeedPatientUsersAsync(
            HealthAppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            var patients = await context.Patients
                .AsNoTracking()
                .ToListAsync();

            foreach (var patient in patients)
            {
                if (string.IsNullOrWhiteSpace(patient.Email))
                {
                    continue;
                }

                var existingUser = await userManager.FindByEmailAsync(
                    patient.Email);

                if (existingUser != null)
                {
                    if (existingUser.PatientId == null)
                    {
                        existingUser.PatientId = patient.PatientId;
                        existingUser.DoctorId = null;

                        await userManager.UpdateAsync(existingUser);
                    }

                    if (!await userManager.IsInRoleAsync(existingUser, "Patient"))
                    {
                        await userManager.AddToRoleAsync(
                            existingUser,
                            "Patient");
                    }

                    continue;
                }

                var password = GeneratePatientPassword(
                    patient.FullName);

                var user = new ApplicationUser
                {
                    UserName = patient.Email,
                    Email = patient.Email,
                    EmailConfirmed = true,
                    PatientId = patient.PatientId,
                    DoctorId = null
                };

                var createResult = await userManager.CreateAsync(
                    user,
                    password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        " ",
                        createResult.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"Failed to create patient user for {patient.Email}. {errors}");
                }

                var roleResult = await userManager.AddToRoleAsync(
                    user,
                    "Patient");

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        " ",
                        roleResult.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"Failed to assign Patient role for {patient.Email}. {errors}");
                }
            }
        }

        private static string GenerateDoctorPassword(
            string fullName)
        {
            var firstName = GetFirstName(fullName);

            return $"{firstName}@123";
        }

        private static string GeneratePatientPassword(
            string fullName)
        {
            var firstName = GetFirstName(fullName);

            return $"{firstName}@123";
        }

        private static string GetFirstName(
            string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return "User";
            }

            var cleanName = fullName
                .Replace("Dr.", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Trim();

            var firstName = cleanName
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();

            return string.IsNullOrWhiteSpace(firstName)
                ? "User"
                : firstName;
        }
    }
}