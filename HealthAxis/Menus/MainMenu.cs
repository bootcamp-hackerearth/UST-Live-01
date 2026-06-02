using HealthAxis.Functions;
using HealthAxis.Helpers;
using System.Diagnostics.CodeAnalysis;
using static HealthAxis.Common.ApplicationConstant;

namespace HealthAxis.Menus
{
    [ExcludeFromCodeCoverage]
    public class MainMenu
    {
        private readonly Function functions;

        public MainMenu(Function functions)
        {
            this.functions = functions;
        }

        public void Show()
        {
            while (true)
            {
                MenuHelper.DisplayMenu("HealthAxis Portal",
                    "1. Patient",
                    "2. Doctor",
                    "3. Admin",
                    "4. Exit");

                Console.Write(Option);

                switch (Console.ReadLine())
                {
                    case "1":
                        new PatientMenu(functions).Show();
                        break;

                    case "2":
                        new DoctorMenu(functions).Show();
                        break;

                    case "3":
                        new AdminMenu(functions).Show();
                        break;

                    case "4":
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}