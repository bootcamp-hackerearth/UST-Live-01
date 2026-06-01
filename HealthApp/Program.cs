using Microsoft.Extensions.DependencyInjection;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Services;
using HealthApp.ConsoleApp.Repositories;
using HealthApp.ConsoleApp.Menus;
using HealthApp.ConsoleApp.Databases;

// Register all dependencies
var services = new ServiceCollection();
// Register databases as singletons
services.AddSingleton<DoctorDb>();
services.AddSingleton<AppointmentDb>();
services.AddSingleton<HealthRecordDb>();
services.AddSingleton<PatientDb>();
// Register repositories as singletons for shared in-memory data access
services.AddSingleton<IPatientRepository, PatientRepository>();
services.AddSingleton<IDoctorRepository, DoctorRepository>();
services.AddSingleton<IAppointmentRepository, AppointmentRepository>();
services.AddSingleton<IHealthRecordRepository, HealthRecordRepository>();
// Register services as scoped for business logic operations
services.AddScoped<IPatientService, PatientService>();
services.AddScoped<IDoctorService, DoctorService>();
services.AddScoped<IAppointmentService, AppointmentService>();
services.AddScoped<IHealthRecordService, HealthRecordService>();
// Register menus as scoped for user interaction handling
services.AddScoped<PatientMenu>();
services.AddScoped<DoctorMenu>();
services.AddScoped<AppointmentMenu>();
services.AddScoped<HealthRecordMenu>();
// Build the service provider and resolve the main menus
var provider = services.BuildServiceProvider();
var patientMenu = provider.GetRequiredService<PatientMenu>();
var doctorMenu = provider.GetRequiredService<DoctorMenu>();
var appointmentMenu = provider.GetRequiredService<AppointmentMenu>();
var healthRecordMenu = provider.GetRequiredService<HealthRecordMenu>();

// Main application loop
bool running = true;
while (running)
{
    Console.Clear();
    PrintBanner();
    Console.Write("  Choose an option : ");

    switch (Console.ReadLine()?.Trim() ?? "")
    {
        case "1": patientMenu.RegisterPatient(); break;
        case "2": doctorMenu.AddDoctor(); break;
        case "3": doctorMenu.SearchBySpecialisation(); break;
        case "4": appointmentMenu.BookAppointment(); break;
        case "5": appointmentMenu.ViewPatientAppointments(); break;
        case "6": appointmentMenu.UpdateAppointmentMenu(); break;
        case "7": healthRecordMenu.AddHealthRecord(); break;
        case "8": healthRecordMenu.ViewRecord(); break;
        case "9": ShowDetailedMenus(); break;
        case "0":
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n  Thank you for using HealthAxis. Goodbye!\n");
            Console.ResetColor();
            running = false;
            break;
        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n  Invalid option. Enter a number between 0 and 9.");
            Console.ResetColor();
            Thread.Sleep(1000);
            break;
    }
}

// Print the main portal banner
void PrintBanner()
{
    Console.WriteLine("  ╔══════════════════════════════════════════════════╗");
    Console.WriteLine("  ║           HealthAxis Patient Portal              ║");
    Console.WriteLine("  ╠══════════════════════════════════════════════════╣");
    Console.WriteLine("  ║  1.  Register a new patient                      ║");
    Console.WriteLine("  ║  2.  Add a new doctor                            ║");
    Console.WriteLine("  ║  3.  Search doctors by specialisation            ║");
    Console.WriteLine("  ║  4.  Book an appointment                         ║");
    Console.WriteLine("  ║  5.  View appointments for a patient             ║");
    Console.WriteLine("  ║  6.  Update appointment status                   ║");
    Console.WriteLine("  ║  7.  Add a health record                         ║");
    Console.WriteLine("  ║  8.  View health history                         ║");
    Console.WriteLine("  ║  9.  Patient / Doctor detailed menus             ║");
    Console.WriteLine("  ║  0.  Exit                                        ║");
    Console.WriteLine("  ╚══════════════════════════════════════════════════╝");
    Console.WriteLine("  Type 'q' or 'back' at any prompt to return here.\n");
}

// Sub-menu to navigate into Patient or Doctor detailed menus
void ShowDetailedMenus()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("  ╔══════════════════════════════╗");
        Console.WriteLine("  ║        DETAILED MENUS        ║");
        Console.WriteLine("  ╠══════════════════════════════╣");
        Console.WriteLine("  ║  1.  Patient Menu            ║");
        Console.WriteLine("  ║  2.  Doctor Menu             ║");
        Console.WriteLine("  ║  3.  Back                    ║");
        Console.WriteLine("  ╚══════════════════════════════╝");
        Console.Write("\n  Choose an option : ");

        switch (Console.ReadLine()?.Trim() ?? "")
        {
            case "1": patientMenu.ShowPatientMenu(); break;
            case "2": doctorMenu.ShowDoctorMenu(); break;
            case "3": return;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  Invalid choice.");
                Console.ResetColor();
                Thread.Sleep(800);
                break;
        }
    }
}