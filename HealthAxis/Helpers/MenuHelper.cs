using System;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxis.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class MenuHelper
    {
        public static void DisplayMenu(string title, params string[] options)
        {
            Console.WriteLine();
            Console.WriteLine($"===== {title} =====");
            foreach (var option in options)
            {
                Console.WriteLine(option);
            }
        }
    }
}