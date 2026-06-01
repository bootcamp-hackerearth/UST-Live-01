using System.Globalization;
using HealthApp.ConsoleApp.Models;
using System.Text.RegularExpressions;

namespace HealthApp.ConsoleApp.Helpers
{
    // Used for validating user input in the console application.
    public static class InputValidator
    {
        // Prompts the user for input and validates it using the provided validator function.
        public static string? GetValidatedInput(
            string prompt,
            Func<string, bool> validator,
            string errorMessage,
            bool allowEmpty = false)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (input?.ToLower() == "q" || input?.ToLower() == "back")
                    throw new OperationCanceledException();

                if (allowEmpty && string.IsNullOrWhiteSpace(input))
                    return null;

                if (!string.IsNullOrWhiteSpace(input) && validator(input))
                    return input.Trim();

                Console.WriteLine(errorMessage);
            }
        }
        // Prompts the user for a valid date input in the format "dd/MM/yyyy".
        public static DateTime GetValidDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();

                if (input?.ToLower() == "q" || input?.ToLower() == "back")
                    throw new OperationCanceledException();

                if (DateTime.TryParseExact(
                    input,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date))
                {
                    return date;
                }

                Console.WriteLine("Invalid date.");
            }
        }
        // Prompts the user for an optional date input in the format "dd/MM/yyyy". Returns null if the user enters an empty string.
        public static DateTime? GetOptionalDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (input?.ToLower() == "q" || input?.ToLower() == "back")
                    throw new OperationCanceledException();

                if (DateTime.TryParseExact(
                    input,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date))
                {
                    return date;
                }

                Console.WriteLine("Invalid date.");
            }
        }

        // Prompts the user for an optional gender input. Returns null if the user enters an empty string.
        public static GenderType? GetOptionalGender(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (input?.ToLower() == "q" || input?.ToLower() == "back")
                    throw new OperationCanceledException();

                if (Enum.TryParse<GenderType>(input, true, out var gender))
                    return gender;

                Console.WriteLine("Invalid gender.");
            }
        }
        // Prompts the user for a valid gender input. Continues to prompt until a valid input is received or the user chooses to quit.
        public static GenderType GetValidGender(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine()?.Trim();

                if (input?.ToLower() == "q" || input?.ToLower() == "back")
                    throw new OperationCanceledException();

                if (Enum.GetNames(typeof(GenderType))
                        .Any(n => n.Equals(input, StringComparison.OrdinalIgnoreCase)))
                {
                    return Enum.Parse<GenderType>(input!, true);
                }

                Console.WriteLine("Invalid gender.");
            }
        }
        public static bool IsValidText(string input) =>
            !string.IsNullOrWhiteSpace(input) &&
            input.Length >= 5 &&
            input.Any(char.IsLetter);

        public static bool IsValidCancellationReason(string input) =>
            IsValidText(input);

        public static bool IsValidDiagnosis(string input) =>
            IsValidText(input);

        public static bool IsValidPrescription(string input) =>
            IsValidText(input);

        public static bool IsValidDoctorNotes(string input) =>
            IsValidText(input);
        //Validates all the input fields for doctor and patient details.
        public static bool IsValidName(string input) =>
        !string.IsNullOrWhiteSpace(input) && !input.Any(char.IsDigit);

        public static bool IsValidPhone(string input)
        {
            //Regex pattern for validating Indian phone numbers (10 digits starting with 6-9)
            string pattern = @"^[6-9]\d{9}$";
            return Regex.IsMatch(input, pattern);
        }

        public static bool IsValidEmail(string input)
        {
            //Regex pattern for validating email addresses
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(input, pattern);
        }

        //Assuming insurance ID is a non-negative integer. Adjust validation as needed based on actual format.
        public static bool IsValidInsuranceId(string input) =>
             !string.IsNullOrWhiteSpace(input) &&
             Regex.IsMatch(input, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$");

        //Assuming experience is a non-negative integer representing years of experience. Adjust validation as needed based on actual requirements.
        public static bool IsValidExperience(string input) =>
            int.TryParse(input, out int val) && val >= 0 && val <= 50;

        //Assuming fee is a non-negative decimal value. Adjust validation as needed based on actual requirements.
        public static bool IsValidFee(string input) =>
            decimal.TryParse(input, out decimal val) && val >= 0;

        //Assuming IDs are positive integers. Adjust validation as needed based on actual format.
        public static bool IsValidId(string input) =>
            int.TryParse(input, out int id) && id > 0 && id <= 999;

        // Validates that the input is not null, empty, or whitespace.
        public static bool IsNonEmpty(string input) =>
            !string.IsNullOrWhiteSpace(input);
    }
}