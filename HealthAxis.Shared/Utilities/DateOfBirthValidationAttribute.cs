using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
    public class DateOfBirthValidationAttribute : ValidationAttribute
{
    private static readonly DateTime MinimumDateOfBirth =
        new(1900, 1, 1);

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is not DateTime dateOfBirth)
        {
            return new ValidationResult(
                ErrorMessage ?? "Date of birth is required.");
        }

        DateTime today =
            DateTime.Today;

        if (dateOfBirth.Date < MinimumDateOfBirth)
        {
            return new ValidationResult(
                ErrorMessage ?? "Date of birth cannot be before 01-Jan-1900.");
        }

        if (dateOfBirth.Date > today)
        {
            return new ValidationResult(
                ErrorMessage ?? "Date of birth cannot be a future date.");
        }

        return ValidationResult.Success;
    }
}




