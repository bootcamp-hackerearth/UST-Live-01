using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.Shared.Utilities
{
    [ExcludeFromCodeCoverage]
    public static class CustomValidators
    {
        public static ValidationResult? ValidateScheduledDate(
            DateTime scheduledDate,
            ValidationContext context)
        {
            if (scheduledDate.Date < DateTime.Today)
            {
                return new ValidationResult(
                    ValidationMessages.ScheduledDateCannotBePast);
            }

            return ValidationResult.Success;
        }

        public static ValidationResult? ValidateDateOfBirth(
            DateTime dateOfBirth,
            ValidationContext context)
        {
            if (dateOfBirth.Date > DateTime.Today)
            {
                return new ValidationResult(
                    ValidationMessages.DateOfBirthCannotBeFuture);
            }

            if (CalculateAge(dateOfBirth) > ValidationLimits.MaximumAge)
            {
                return new ValidationResult(
                    ValidationMessages.InvalidAge);
            }

            return ValidationResult.Success;
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = DateTime.Today.Year - dateOfBirth.Year;

            if (dateOfBirth > DateTime.Today.AddYears(-age))
            {

                age--;
            }
            return age;
        }
    }
}
