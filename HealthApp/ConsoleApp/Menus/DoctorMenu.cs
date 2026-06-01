using HealthApp.ConsoleApp.Exceptions;
using HealthApp.ConsoleApp.Helpers;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Models;

namespace HealthApp.ConsoleApp.Menus
{
    // Menu class to handle all doctor-related user interactions
    public class DoctorMenu
    {
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        private const string ReturnToMenu = "\n  Returning to menu...";

        // Constructor to inject required services for doctor management and appointments
        public DoctorMenu(IDoctorService doctorService, IAppointmentService appointmentService)
        {
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        // Doctor sub-menu with full management options
        public void ShowDoctorMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("  ╔══════════════════════════════════╗");
                Console.WriteLine("  ║         DOCTOR MENU              ║");
                Console.WriteLine("  ╠══════════════════════════════════╣");
                Console.WriteLine("  ║  1.  Add Doctor                  ║");
                Console.WriteLine("  ║  2.  Search by Specialisation    ║");
                Console.WriteLine("  ║  3.  Get Doctor by ID            ║");
                Console.WriteLine("  ║  4.  View All Doctors            ║");
                Console.WriteLine("  ║  5.  Update Doctor               ║");
                Console.WriteLine("  ║  6.  View Appointments by Doctor ║");
                Console.WriteLine("  ║  7.  Back                        ║");
                Console.WriteLine("  ╚══════════════════════════════════╝");
                Console.Write("\n  Choose an option : ");

                switch (Console.ReadLine()?.Trim() ?? "")
                {
                    case "1": AddDoctor(); break;
                    case "2": SearchBySpecialisation(); break;
                    case "3": GetDoctorById(); break;
                    case "4": ViewAllDoctors(); break;
                    case "5": UpdateDoctor(); break;
                    case "6": GetAppointmentsByDoctorId(); break;
                    case "7": return;
                    default:
                        ConsoleHelper.PrintError("Invalid choice.");
                        Thread.Sleep(800);
                        break;
                }
            }
        }

        // Add a new doctor with leave dates and available slots
        public void AddDoctor()
        {
            try
            {
                Console.Clear();
                ConsoleHelper.PrintHeader("ADD NEW DOCTOR");
                Console.WriteLine("  Type 'q' or 'back' anytime to return.\n");

                // Get validated full name
                string fullName = InputValidator.GetValidatedInput(
                    "  Full Name              : ",
                    InputValidator.IsValidName,
                    "  Name cannot be empty or contain numbers.")!;

                // Get validated specialisation
                string spec = InputValidator.GetValidatedInput(
                    "  Specialisation         : ",
                    InputValidator.IsValidName,
                    "  Specialisation cannot be empty and cannot contain numbers.")!;

                // Get validated years of experience
                string yearsRaw = InputValidator.GetValidatedInput(
                    "  Years of Experience    : ",
                    InputValidator.IsValidExperience,
                    "  Please enter a valid non-negative number.")!;

                // Get validated consultation fee
                string feeRaw = InputValidator.GetValidatedInput(
                    "  Consultation Fee (Rs.) : ",
                    InputValidator.IsValidFee,
                    "  Please enter a valid non-negative amount.")!;

                int years = int.Parse(yearsRaw);
                decimal fee = decimal.Parse(feeRaw);

                // Collect leave dates to exclude from availability
                List<DateTime> leaveDates = new();
                DateTime startDate = DateTime.Today;
                DateTime endDate = DateTime.Today.AddDays(30);

                Console.WriteLine("\n  Doctor will be available for the next 30 days.");
                Console.WriteLine("  Enter leave dates one by one. Type 'done' when finished.\n");

                List<DateTime> availableDates = BuildAvailableDates();
                List<string> selectedSlots = AcceptSlotTimes();

                // Build and save the doctor
                Doctor doctor = new()
                {
                    FullName = fullName,
                    Specialisation = spec,
                    YearsOfExperience = years,
                    ConsultationFee = fee,
                    IsActive = true,
                    AvailableDates = availableDates,
                    AvailableSlots = selectedSlots
                };

                _doctorService.AddDoctor(doctor);

                ConsoleHelper.PrintSuccess("Doctor Added Successfully!");
                Console.WriteLine(doctor);
                Console.WriteLine($"Slots : {string.Join(", ", doctor.AvailableSlots)}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine(ReturnToMenu);
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }

            ConsoleHelper.Pause();
        }

        public static List<DateTime> AcceptLeaveDates(DateTime startDate, DateTime endDate)
        {
            List<DateTime> leaveDates = new();

            while (true)
                {
                    Console.Write("  Leave Date (dd/MM/yyyy) or 'done' : ");
                    string input = Console.ReadLine()?.Trim() ?? "";

                    if (input.Equals("q", StringComparison.OrdinalIgnoreCase) ||
                        input.Equals("back", StringComparison.OrdinalIgnoreCase))
                        throw new OperationCanceledException();

                    if (input.Equals("done", StringComparison.OrdinalIgnoreCase)) break;

                    if (!DateTime.TryParseExact(input, "dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out DateTime leaveDate))
                    { ConsoleHelper.PrintError("Invalid format. Use dd/MM/yyyy."); continue; }

                    if (leaveDate < startDate || leaveDate > endDate)
                    { ConsoleHelper.PrintError("Only dates within the next 30 days allowed."); continue; }

                    if (leaveDates.Contains(leaveDate))
                    { ConsoleHelper.PrintError("Already added."); continue; }

                    leaveDates.Add(leaveDate);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"     Leave added : {leaveDate:dd/MM/yyyy}");
                    Console.ResetColor();
                }

                return leaveDates;
        }

        public static List<DateTime> BuildAvailableDates()
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(30);

            List<DateTime> leaveDates = AcceptLeaveDates(startDate, endDate);

            List<DateTime> availableDates = new();
                for (DateTime d = startDate; d <= endDate; d = d.AddDays(1))
                    if (!leaveDates.Contains(d))
                        availableDates.Add(d);

            return availableDates;
        }

        // Display and select time slots
        public static List<string> AcceptSlotTimes()
        {
                SlotHelper slotHelper = new();
                List<string> allSlots = slotHelper.AvailableSlots;
                List<string> selectedSlots = new();

                while (true)
                {
                    Console.WriteLine("\n  Available Time Slots:");
                    Console.WriteLine("  " + new string('─', 30));
                    for (int i = 0; i < allSlots.Count; i++)
                        Console.WriteLine($"    {i + 1}.  {allSlots[i]}");
                    Console.WriteLine("  " + new string('─', 30));

                    string slotInput = InputValidator.GetValidatedInput(
                        "  Select slots (e.g. 1,2,3) : ",
                        InputValidator.IsNonEmpty,
                        "  Please select at least one slot.")!;

                    List<string> picked = new();
                    bool valid = true;

                    foreach (string c in slotInput.Split(','))
                    {
                        if (!int.TryParse(c.Trim(), out int idx) ||
                            idx < 1 || idx > allSlots.Count)
                        { ConsoleHelper.PrintError($"Invalid slot: {c.Trim()}"); valid = false; break; }

                        string slot = allSlots[idx - 1];
                        if (!picked.Contains(slot)) picked.Add(slot);
                    }

                    if (!valid || picked.Count == 0) continue;

                    selectedSlots = picked;
                    break;
                }

                // Build and save the doctor
                Doctor doctor = new()
                {
                    Name = fullName,
                    Specialisation = spec,
                    YearsOfExperience = years,
                    ConsultationFee = fee,
                    IsActive = true,
                    AvailableDates = availableDates,
                    AvailableSlots = selectedSlots
                };

                _doctorService.AddDoctor(doctor);

                PrintSuccess("Doctor Added Successfully!");
                Console.WriteLine($"\n  {doctor.GetScheduleSummary()}");
                Console.WriteLine($"  Slots : {string.Join(", ", doctor.AvailableSlots)}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\n  Returning to menu...");
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }

                return selectedSlots;
        }
        // Update an existing doctor's details keeping old values where not changed
        public void UpdateDoctor()
        {
            try
            {
                Console.Clear();
                ConsoleHelper.PrintHeader("UPDATE DOCTOR");
                Console.WriteLine("  Type 'q' or 'back' anytime to return.\n");

                // Get and validate doctor ID
                string rawId = InputValidator.GetValidatedInput(
                    "  Enter Doctor ID : ",
                    InputValidator.IsValidId,
                    "  Please enter a valid positive number.")!;

                var existing = _doctorService.GetDoctorById(int.Parse(rawId));

                if (existing == null)
                {
                    ConsoleHelper.PrintError($"No doctor found with ID {rawId}.");
                    ConsoleHelper.Pause();
                    return;
                }

                // Show current details before editing
                Console.WriteLine("\n  Current Details:");
                Console.WriteLine("  " + new string('─', 50));
                Console.WriteLine($"  ID             : {existing.DoctorId}");
                Console.WriteLine($"  Name           : {existing.FullName}");
                Console.WriteLine($"  Specialisation : {existing.Specialisation}");
                Console.WriteLine($"  Experience     : {existing.YearsOfExperience} years");
                Console.WriteLine($"  Fee            : Rs.{existing.ConsultationFee}");
                Console.WriteLine($"  Active         : {(existing.IsActive ? "Yes" : "No")}");
                Console.WriteLine($"  Slots          : {string.Join(", ", existing.AvailableSlots)}");
                Console.WriteLine("  " + new string('─', 50));
                Console.WriteLine("\n  Press ENTER to keep existing value.\n");

                // Get optional updated name
                string? name = InputValidator.GetValidatedInput(
                    "  Full Name              : ",
                    InputValidator.IsValidName,
                    "  Name cannot be empty or contain numbers.",
                    allowEmpty: true);

                // Get optional updated specialisation
                string? spec = InputValidator.GetValidatedInput(
                    "  Specialisation         : ",
                    InputValidator.IsValidName,
                    "  Specialisation cannot be empty.",
                    allowEmpty: true);

                // Get optional updated years of experience
                string? yearsRaw = InputValidator.GetValidatedInput(
                    "  Years of Experience    : ",
                    InputValidator.IsValidExperience,
                    "  Please enter a valid non-negative number.",
                    allowEmpty: true);

                // Get optional updated consultation fee
                string? feeRaw = InputValidator.GetValidatedInput(
                    "  Consultation Fee (Rs.) : ",
                    InputValidator.IsValidFee,
                    "  Please enter a valid non-negative amount.",
                    allowEmpty: true);

                // Get optional updated active status
                Console.Write("  Is Active (true/false) : ");
                string? activeRaw = Console.ReadLine()?.Trim();
                bool isActive = string.IsNullOrWhiteSpace(activeRaw)
                    ? existing.IsActive
                    : activeRaw.Equals("true", StringComparison.OrdinalIgnoreCase);

                // Get optional updated slots
                List<string> updatedSlots = existing.AvailableSlots;

                Console.Write("\n  Update available slots? (Y/N) : ");
                if (Console.ReadLine()?.Trim().ToUpper() == "Y")
                {
                    updatedSlots = AcceptSlotTimes();
                }

                        if (!valid || picked.Count == 0) continue;

                        updatedSlots = picked;
                        break;
                    }
                }

                // Build updated doctor — fall back to existing values where unchanged
                Doctor updated = new()
                {
                    DoctorId = existing.DoctorId,
                    FullName = name ?? existing.FullName,
                    Specialisation = spec ?? existing.Specialisation,
                    YearsOfExperience = yearsRaw != null ? int.Parse(yearsRaw) : existing.YearsOfExperience,
                    ConsultationFee = feeRaw != null ? decimal.Parse(feeRaw) : existing.ConsultationFee,
                    IsActive = isActive,
                    AvailableDates = existing.AvailableDates,
                    AvailableSlots = updatedSlots
                };

                _doctorService.UpdateDoctor(updated);

                ConsoleHelper.PrintSuccess("Doctor Updated Successfully!");
                Console.WriteLine($"\n  ID             : {updated.DoctorId}");
                Console.WriteLine($"  Name           : {updated.FullName}");
                Console.WriteLine($"  Specialisation : {updated.Specialisation}");
                Console.WriteLine($"  Experience     : {updated.YearsOfExperience} years");
                Console.WriteLine($"  Fee            : Rs.{updated.ConsultationFee}");
                Console.WriteLine($"  Active         : {(updated.IsActive ? "Yes" : "No")}");
                Console.WriteLine($"  Slots          : {string.Join(", ", updated.AvailableSlots)}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine(ReturnToMenu);
            }
            catch (DoctorNotFoundException ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }

            ConsoleHelper.Pause();
        }

        // Search active doctors by specialisation and show their details
        public void SearchBySpecialisation()
        {
            try
            {
                Console.Clear();
                ConsoleHelper.PrintHeader("SEARCH BY SPECIALISATION");
                Console.WriteLine("  Type 'q' or 'back' to return.\n");

                // Get validated specialisation keyword
                string query = InputValidator.GetValidatedInput(
                    "  Specialisation : ",
                    InputValidator.IsValidName,
                    "  Specialisation cannot be empty.")!;

                var results = _doctorService.GetDoctorsBySpecialisation(query);

                Console.WriteLine($"\n  {results.Count} doctor(s) found:\n");
                Console.WriteLine("  " + new string('─', 55));

                foreach (var d in results)
                {
                    Console.WriteLine($"  [{d.DoctorId}]  {d.FullName}  —  {d.Specialisation}");
                    Console.WriteLine($"       Experience : {d.YearsOfExperience} yrs  |  Fee : Rs.{d.ConsultationFee}");
                    Console.WriteLine($"       Status     : {(d.IsActive ? "Active" : "Inactive")}");
                    Console.WriteLine(d.AvailableSlots.Count > 0
                        ? $"       Slots      : {string.Join(", ", d.AvailableSlots)}"
                        : "       Slots      : None configured");

                    // Show IsAvailable() result — required spec method
                    bool today = d.IsAvailable(DateTime.Today);
                    Console.ForegroundColor = today ? ConsoleColor.Green : ConsoleColor.Yellow;
                    Console.WriteLine($"       Available Today : {(today ? "Yes" : "No")}");
                    Console.ResetColor();

                    // Show GetScheduleSummary() — required spec method
                    try
                    {
                        Console.WriteLine($"       {d.GetScheduleSummary(_appointmentService.GetAppointmentsByDoctorId(d.DoctorId))}");
                    } catch (AppointmentNotFoundException)
                    {
                        Console.WriteLine("");
                    }
                    Console.WriteLine("  " + new string('─', 70));
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine(ReturnToMenu);
            }
            catch (SpecialisationNotFoundException ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }

            ConsoleHelper.Pause();
        }

        // Get and display a single doctor by their ID
        public void GetDoctorById()
        {
            try
            {
                Console.Clear();
                ConsoleHelper.PrintHeader("GET DOCTOR BY ID");
                Console.WriteLine("  Type 'q' or 'back' to return.\n");

                // Get validated doctor ID
                string raw = InputValidator.GetValidatedInput(
                    "  Enter Doctor ID : ",
                    InputValidator.IsValidId,
                    "  Please enter a valid positive number.")!;

                var doctor = _doctorService.GetDoctorById(int.Parse(raw));

                if (doctor == null)
                {
                    PrintError($"No doctor found with ID {raw}.");
                    Pause();
                    return;
                }

                Console.WriteLine("  " + new string('─', 40));
                Console.WriteLine($"  ID             : {doctor.DoctorId}");
                Console.WriteLine($"  Name           : {doctor.FullName}");
                Console.WriteLine($"  Specialisation : {doctor.Specialisation}");
                Console.WriteLine($"  Experience     : {doctor.YearsOfExperience} years");
                Console.WriteLine($"  Fee            : Rs.{doctor.ConsultationFee}");
                Console.WriteLine($"  Active         : {(doctor.IsActive ? "Yes" : "No")}");
                Console.WriteLine($"  Slots          : {string.Join(", ", doctor.AvailableSlots)}");
                Console.WriteLine("  " + new string('─', 40));
            }
            catch (DoctorNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine(ReturnToMenu);
            }

            ConsoleHelper.Pause();
        }

        // Display all doctors currently in the system
        public void ViewAllDoctors()
        {
            Console.Clear();
            ConsoleHelper.PrintHeader("ALL DOCTORS");

            var doctors = _doctorService.GetAllDoctors();

            if (doctors == null || doctors.Count == 0)
            {
                ConsoleHelper.PrintError("No doctors available.");
                ConsoleHelper.Pause();
                return;
            }

            foreach (var d in doctors)
            {
                Console.WriteLine("  " + new string('─', 50));
                Console.WriteLine($"  [{d.DoctorId}]  {d.FullName}  —  {d.Specialisation}");
                Console.WriteLine($"       Experience : {d.YearsOfExperience} yrs  |  Fee : Rs.{d.ConsultationFee}");
                Console.WriteLine($"       Status     : {(d.IsActive ? "Active" : "Inactive")}");
            }

            Console.WriteLine("  " + new string('─', 50));
            ConsoleHelper.Pause();
        }

        // Fetch and display all appointments for a specific doctor
        public void GetAppointmentsByDoctorId()
        {
            try
            {
                Console.Clear();
                ConsoleHelper.PrintHeader("APPOINTMENTS BY DOCTOR");
                Console.WriteLine("  Type 'q' or 'back' to return.\n");

                // Get and validate doctor ID
                string raw = InputValidator.GetValidatedInput(
                    "  Enter Doctor ID : ",
                    InputValidator.IsValidId,
                    "  Please enter a valid positive number.")!;

                int doctorId = int.Parse(raw);

                // Verify doctor exists before fetching appointments
                var doctor = _doctorService.GetDoctorById(doctorId);
                if (doctor == null)
                {
                    PrintError($"No doctor found with ID {doctorId}.");
                    Pause();
                    return;
                }

                Console.WriteLine($"\n  Dr. {doctor.FullName}  —  {doctor.Specialisation}\n");

                // Fetch all appointments for this doctor
                var appointments = _appointmentService.GetAppointmentsByDoctorId(doctorId);

                if (appointments.Count == 0)
                {
                    PrintError("No appointments found for this doctor.");
                    Pause();
                    return;
                }

                Console.WriteLine($"  {appointments.Count} appointment(s) found:\n");

                // Group by status for better readability
                foreach (var status in new[] {
            AppointmentStatus.Confirmed,
            AppointmentStatus.Pending,
            AppointmentStatus.Completed,
            AppointmentStatus.Cancelled })
                {
                    var group = appointments.Where(a => a.Status == status).ToList();
                    if (group.Count == 0) continue;
                    Console.WriteLine($"  ── {status} ({group.Count}) ──────────────────────");
                    foreach (var appt in group)
                    {
                        Console.WriteLine($"  [{appt.AppointmentId}]  " +
                                          $"{appt.Patient.FullName,-15}  |  " +
                                          $"{appt.ScheduledDate:dd/MM/yyyy}  |  " +
                                          $"{appt.TimeSlot}");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine("  " + new string('─', 50));
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine(ReturnToMenu);
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }

            ConsoleHelper.Pause();
        }
    }
}