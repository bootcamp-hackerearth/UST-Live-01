using System.Globalization;
using HealthApp.ConsoleApp.Models;
using System.Text.RegularExpressions;

namespace HealthApp.ConsoleApp.Helpers
{
public static class ConsoleHelper {
    public static void PrintHeader(string title)
            {
                Console.WriteLine($"\n  ╔══════════════════════════════════════════════════╗");
                Console.WriteLine($"  ║  {title,-48}║");
                Console.WriteLine($"  ╚══════════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine();
            }

            public static void PrintSuccess(string msg)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{msg}");
                Console.ResetColor();
            }

            public static void PrintError(string msg)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n{msg}");
                Console.ResetColor();
            }

            public static void Pause()
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n  Press any key to continue...");
                Console.ResetColor();
                Console.ReadKey(intercept: true);
            }
    }
}