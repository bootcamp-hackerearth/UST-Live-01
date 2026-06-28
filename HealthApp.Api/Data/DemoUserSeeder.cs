using HealthApp.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Data
{
    public static class DemoUserSeeder
    {
        private const string DoctorRole = "Doctor";
        private const string PatientRole = "Patient";
        private const string DefaultUserName = "User";
        private const string DoctorTitle = "Dr.";
        private const string PasswordSuffix = "@123";
        private const string ErrorSeparator = " ";

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

                    if (!await userManager.IsInRoleAsync(
                            existingUser,
                            DoctorRole))
                    {
                        await userManager.AddToRoleAsync(
                            existingUser,
                            DoctorRole);
                    }

                    continue;
                }

                var password = GenerateDefaultPassword(
                    doctor.FullName!);

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
                    var errors = GetIdentityErrors(createResult);

                    throw new InvalidOperationException(
                        $"Failed to create doctor user for {doctor.DoctorEmail}. {errors}");
                }

                var roleResult = await userManager.AddToRoleAsync(
                    user,
                    DoctorRole);

                if (!roleResult.Succeeded)
                {
                    var errors = GetIdentityErrors(roleResult);

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

                    if (!await userManager.IsInRoleAsync(
                            existingUser,
                            PatientRole))
                    {
                        await userManager.AddToRoleAsync(
                            existingUser,
                            PatientRole);
                    }

                    continue;
                }

                var password = GenerateDefaultPassword(
                    patient.FullName!);

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
                    var errors = GetIdentityErrors(createResult);

                    throw new InvalidOperationException(
                        $"Failed to create patient user for {patient.Email}. {errors}");
                }

                var roleResult = await userManager.AddToRoleAsync(
                    user,
                    PatientRole);

                if (!roleResult.Succeeded)
                {
                    var errors = GetIdentityErrors(roleResult);

                    throw new InvalidOperationException(
                        $"Failed to assign Patient role for {patient.Email}. {errors}");
                }
            }
        }

        private static string GenerateDefaultPassword(
            string fullName)
        {
            var firstName = GetFirstName(fullName);

            return $"{firstName}{PasswordSuffix}";
        }

        private static string GetFirstName(
            string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return DefaultUserName;
            }

            var cleanName = fullName
                .Replace(
                    DoctorTitle,
                    string.Empty,
                    StringComparison.OrdinalIgnoreCase)
                .Trim();

            var firstName = cleanName
                .Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();

            return string.IsNullOrWhiteSpace(firstName)
                ? DefaultUserName
                : firstName;
        }

        private static string GetIdentityErrors(
            IdentityResult identityResult)
        {
            return string.Join(
                ErrorSeparator,
                identityResult.Errors.Select(error => error.Description));
        }
    }
}