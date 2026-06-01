using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthApp.ConsoleApp.Helpers
{
    public class SlotHelper
    {
        // Fixed clinic slots
        public List<string> AvailableSlots { get; set; }= new List<string>
        {
            "09:00 AM",
            "10:00 AM",
            "11:00 AM",
            "12:00 PM",
            "02:00 PM",
            "03:00 PM",
            "04:00 PM",
            "05:00 PM"
        };

        // Displays slots and returns the one the user picks
        public string PickSlot()
        {
            Console.WriteLine("\nAvailable Time Slots:");
            for (int i = 0; i < AvailableSlots.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {AvailableSlots[i]}");
            }
            if (!int.TryParse("  Choose slot (1-8): ", out int slotChoice)   // Validate choice is in range
                || slotChoice < 1 || slotChoice > 8)
            {
                ConsoleHelper.PrintError("Please enter a number between 1 and 8.");
                ConsoleHelper.Pause(); 
                return "";
            }

            // // Validate choice is in range
            // if (choice < 1 || choice > AvailableSlots.Count)
            //     throw new ArgumentException("Invalid slot choice.");

            return AvailableSlots[slotChoice - 1]; // return the actual string
        }
        private static void PrintError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{msg}");
            Console.ResetColor();
        }
        private static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(intercept: true);
        }
    }
}