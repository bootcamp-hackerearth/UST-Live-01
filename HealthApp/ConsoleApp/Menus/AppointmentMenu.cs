using HealthApp.ConsoleApp.Exceptions;
using HealthApp.ConsoleApp.Helpers;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Models;

namespace HealthApp.ConsoleApp.Menus
{
    // Menu class to handle all appointment-related user interactions
    public class AppointmentMenu
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private const string ValidPositiveNumber = "  Please enter a valid positive number.";
        private const string ReturnToMenu = "\n  Returning to menu...";
        // Constructor to inject required services for appointments, patients, and doctors
        public AppointmentMenu(IAppointmentService appointmentService,
                               IPatientService patientService,
                               IDoctorService doctorService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        // Sub-menu to choose between confirm/cancel or complete
        public void UpdateAppointmentMenu()
        {
            Console.Clear();
            Console.WriteLine("  ╔══════════════════════════════════╗");
            Console.WriteLine("  ║     UPDATE APPOINTMENT STATUS    ║");
            Console.WriteLine("  ╠══════════════════════════════════╣");
            Console.WriteLine("  ║  1.  Confirm / Cancel            ║");
            Console.WriteLine("  ║  2.  Mark as Completed           ║");
            Console.WriteLine("  ║  3.  Back                        ║");
            Console.WriteLine("  ╚══════════════════════════════════╝");
            Console.Write("\n  Choose an option : ");

            switch (Console.ReadLine()?.Trim() ?? "")
            {
                case "1": ConfirmOrCancel(); break;
                case "2": CompleteAppointment(); break;
                case "3": return;
                default:
                    ConsoleHelper.PrintError("Invalid choice.");
                    Thread.Sleep(800);
                    break;
            }
        }

        // Book a new appointment for a patient with a chosen doctor and slot
        public void BookAppointment()
        {
            try
            {
                Console.Clear();
                ConsoleHelper.PrintHeader("BOOK APPOINTMENT");
                Console.WriteLine("  Type 'q' or 'back' at any prompt to return.\n");

                // Get and validate patient ID
                string rawPatient = InputValidator.GetValidatedInput(
                    "  Enter Patient ID : ",
                    InputValidator.IsValidId,
                    ValidPositiveNumber)!;

                var patient = _patientService.GetPatientById(int.Parse(rawPatient));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n     Patient : {patient.FullName}");
                Console.ResetColor();

                // Display all doctors for selection
                Console.WriteLine("\n  AVAILABLE DOCTORS");
                Console.WriteLine("  " + new string('─', 65));
                foreach (var doc in _doctorService.GetAllDoctors())
                    Console.WriteLine($"  [{doc.DoctorId}]  {doc.FullName}  |  {doc.Specialisation}  |  " +
                                      $"Rs.{doc.ConsultationFee}  |  " +
                                      $"{(doc.IsActive ? "ACTIVE" : "INACTIVE")}");
                Console.WriteLine("  " + new string('─', 65));

                // Get and validate doctor ID with active check
                Doctor? doctor = null;
                while (true)
                {
                    string rawDoctor = InputValidator.GetValidatedInput(
                        "\n  Enter Doctor ID : ",
                        InputValidator.IsValidId,
                        ValidPositiveNumber)!;

                    doctor = _doctorService.GetDoctorById(int.Parse(rawDoctor));
                    if (!doctor.IsActive) { ConsoleHelper.PrintError($"Dr. {doctor.FullName} is inactive."); continue; }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n     Doctor : {doctor.FullName}  ({doctor.Specialisation})");
                    Console.ResetColor();
                    break;
                }

                // Show doctor's available dates
                Console.WriteLine("\n  AVAILABLE DAYS");
                Console.WriteLine("  " + new string('─', 30));
                for (int i = 0; i < doctor.AvailableDates.Count; i++)
                    Console.WriteLine($"    {i + 1}.  {doctor.AvailableDates[i]:dd/MM/yyyy}");
                Console.WriteLine("  " + new string('─', 30));

                // Get and validate appointment date

                DateTime selectedDate = InputValidator.GetValidAppointmentDate(
                    "\n  Appointment Date (dd/MM/yyyy) : ",
                    doctor.AvailableDates
                );


                // Calculate which slots are still free
                var bookedSlots = _appointmentService
                    .GetAllAppointments()
                    .Where(a => a.ScheduledDate.Date == selectedDate.Date &&
                                a.Status != AppointmentStatus.Cancelled)
                    .Select(a => a.TimeSlot).ToList();

                var freeSlots = doctor.AvailableSlots.Except(bookedSlots).ToList();

                if (freeSlots.Count == 0)
                {
                    ConsoleHelper.PrintError("No slots available on that date. Try a different date.");
                    ConsoleHelper.Pause();
                    return;
                }

                // Display free slots for selection
                Console.WriteLine("\n  AVAILABLE SLOTS");
                Console.WriteLine("  " + new string('─', 25));
                for (int i = 0; i < freeSlots.Count; i++)
                    Console.WriteLine($"    {i + 1}.  {freeSlots[i]}");
                Console.WriteLine("  " + new string('─', 25));


                // Get and validate slot choice
                string selectedSlot = GetSlot(freeSlots);

                // Show booking summary for confirmation
                Console.WriteLine("\n  " + new string('─', 40));
                Console.WriteLine("  BOOKING SUMMARY");
                Console.WriteLine("  " + new string('─', 40));
                Console.WriteLine($"  Patient : {patient.FullName}");
                Console.WriteLine($"  Doctor  : {doctor.FullName}  ({doctor.Specialisation})");
                Console.WriteLine($"  Date    : {selectedDate:dd/MM/yyyy}");
                Console.WriteLine($"  Slot    : {selectedSlot}");
                Console.WriteLine("  " + new string('─', 40));
                Console.Write("\n  Confirm booking? (Y/N) : ");

                if (Console.ReadLine()?.Trim().ToUpper() != "Y")
                {
                    Console.WriteLine("\n  Booking cancelled.");
                    ConsoleHelper.Pause();
                    return;
                }

                // Save the appointment
                var appt = _appointmentService.BookAppointment(
                    patient, doctor, selectedDate, selectedSlot);

                ConsoleHelper.PrintSuccess("Appointment booked successfully!");
                Console.WriteLine($"\n{appt}");
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

        public static string GetSlot(List<string> freeSlots)
        {
            string selectedSlot = "";
            while (true)
            {
                string rawSlot = InputValidator.GetValidatedInput(
                    "\n  Choose slot number : ",
                    InputValidator.IsValidId,
                    ValidPositiveNumber)!;

                int idx = int.Parse(rawSlot);
                if (idx < 1 || idx > freeSlots.Count)
                { ConsoleHelper.PrintError($"Enter a number between 1 and {freeSlots.Count}."); continue; }

                selectedSlot = freeSlots[idx - 1];
                break;
            }
            return selectedSlot;
        }

        // View all appointments for a specific patient
        public void ViewPatientAppointments()
        {
            try
            {
                Console.Clear();
                ConsoleHelper.PrintHeader("PATIENT APPOINTMENTS");
                Console.WriteLine("  Type 'q' or 'back' to return.\n");

                // Get and validate patient ID
                string raw = InputValidator.GetValidatedInput(
                    "  Enter Patient ID : ",
                    InputValidator.IsValidId,
                    ValidPositiveNumber)!;

                var appointments = _appointmentService.GetAppointmentsByPatientId(int.Parse(raw));

                Console.WriteLine($"\n  {appointments.Count} appointment(s) found:\n");

                foreach (var appt in appointments)
                {
                    Console.WriteLine("  " + new string('─', 50));
                    Console.WriteLine(appt.GetDetails());
                }

                Console.WriteLine("  " + new string('─', 50));
            }
            catch (AppointmentNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine(ReturnToMenu);
            }

            ConsoleHelper.Pause();
        }

        // Confirm or cancel a pending/confirmed upcoming appointment
        public void ConfirmOrCancel()
        {
            try
            {
                Console.Clear();
                ConsoleHelper.PrintHeader("CONFIRM / CANCEL APPOINTMENT");

                // Show all upcoming appointments
                var upcoming = _appointmentService.GetUpcomingAppointments();

                if (upcoming.Count == 0)
                {
                    ConsoleHelper.PrintError("No upcoming appointments found.");
                    ConsoleHelper.Pause();
                    return;
                }

                Console.WriteLine($"  {upcoming.Count} upcoming appointment(s):\n");
                foreach (var a in upcoming)
                {
                    Console.WriteLine("  " + new string('─', 50));
                    Console.WriteLine(a.GetDetails());
                }
                Console.WriteLine("  " + new string('─', 50));

                // Get and validate appointment ID
                string raw = InputValidator.GetValidatedInput(
                    "\n  Enter Appointment ID : ",
                    InputValidator.IsValidId,
                    ValidPositiveNumber)!;

                var appointment = _appointmentService.GetAppointmentById(int.Parse(raw));

                // Choose action
                Console.Write("\n  [C] Confirm   [X] Cancel : ");
                string action = Console.ReadLine()?.Trim().ToUpper() ?? "";

                if (action == "C")
                {
                    appointment.Confirm();
                    ConsoleHelper.PrintSuccess("Appointment confirmed.");
                }
                else if (action == "X")
                {
                    // Get cancellation reason
                    string reason = InputValidator.GetValidatedInput(
                        "  Reason for cancellation : ",
                        InputValidator.IsValidCancellationReason,
                        "  Reason cannot be empty.")!;

                    _appointmentService.CancelAppointment(appointment.AppointmentId, reason);
                    ConsoleHelper.PrintSuccess("Appointment cancelled.");
                }
                else
                {
                    ConsoleHelper.PrintError("Invalid action. Enter C or X.");
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine(ReturnToMenu);
            }
            catch (AppointmentNotFoundException ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }

            ConsoleHelper.Pause();
        }

        // Mark a confirmed appointment as completed so a health record can be added
        public void CompleteAppointment()
        {
            try
            {
                Console.Clear();
                ConsoleHelper.PrintHeader("COMPLETE APPOINTMENT");

                // Show only confirmed appointments eligible for completion
                var confirmed = _appointmentService.GetUpcomingAppointments()
                    .Where(a => a.Status == AppointmentStatus.Confirmed).ToList();

                if (confirmed.Count == 0)
                {
                    ConsoleHelper.PrintError("No confirmed appointments to complete.");
                    ConsoleHelper.Pause();
                    return;
                }

                Console.WriteLine($"  {confirmed.Count} confirmed appointment(s):\n");
                foreach (var a in confirmed)
                {
                    Console.WriteLine("  " + new string('─', 50));
                    Console.WriteLine(a.GetDetails());
                }
                Console.WriteLine("  " + new string('─', 50));

                // Get and validate appointment ID
                string raw = InputValidator.GetValidatedInput(
                    "\n  Enter Appointment ID : ",
                    InputValidator.IsValidId,
                    ValidPositiveNumber)!;

                var appointment = _appointmentService.GetAppointmentById(int.Parse(raw));

                // Mark as completed — enables health record creation
                appointment.Complete();
                ConsoleHelper.PrintSuccess($"Appointment {raw} marked as Completed.");
                Console.WriteLine("  You can now add a health record via option 7.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine(ReturnToMenu);
            }
            catch (AppointmentNotFoundException ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }

            ConsoleHelper.Pause();
        }
    }
}