using HealthApp.ConsoleApp.Exceptions;
using HealthApp.ConsoleApp.Helpers;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Models;

namespace HealthApp.ConsoleApp.Menus
{
    // Menu class to handle all health record-related user interactions
    public class HealthRecordMenu
    {
        private readonly IHealthRecordService _healthRecordService;
        private readonly IAppointmentService _appointmentService;
        private const string ValidPositiveNumber = "  Please enter a valid positive number.";
        // Constructor to inject required services for health record management and appointments
        public HealthRecordMenu(IHealthRecordService healthRecordService,
                                IAppointmentService appointmentService)
        {
            _healthRecordService = healthRecordService;
            _appointmentService = appointmentService;
        }

        // Add a health record tied to a completed appointment
        public void AddHealthRecord()
        {
            try
            {
                Console.Clear();
                ConsoleHelper.PrintHeader("ADD HEALTH RECORD");
                Console.WriteLine("  Type 'q' or 'back' to return.\n");

                // Show all completed appointments to help user find the right ID
                var allCompleted = _appointmentService.GetAllAppointments()
                    .Where(a => a.Status == AppointmentStatus.Completed).ToList();

                if (allCompleted.Count == 0)
                {
                    ConsoleHelper.PrintError("No completed appointments found.");
                    Console.WriteLine("  Use option 6 → Mark as Completed first.");
                    ConsoleHelper.Pause();
                    return;
                }

                Console.WriteLine("  COMPLETED APPOINTMENTS");
                Console.WriteLine("  " + new string('─', 60));
                foreach (var a in allCompleted)
                    Console.WriteLine($"  [{a.AppointmentId}]  {a.Patient.FullName}  →  " +
                                      $"Dr. {a.Doctor.FullName}  |  {a.ScheduledDate:dd MMM yyyy}");
                Console.WriteLine("  " + new string('─', 60) + "\n");

                // Get and validate appointment ID
                string rawId = InputValidator.GetValidatedInput(
                    "  Enter Appointment ID : ",
                    InputValidator.IsValidId,
                    ValidPositiveNumber)!;

                // Fetch appointment and handle not found
                Appointment appointment;
                try
                {
                    appointment = _appointmentService.GetAppointmentById(int.Parse(rawId));
                }
                catch (AppointmentNotFoundException)
                {
                    ConsoleHelper.PrintError($"No appointment found with ID {rawId}.");
                    ConsoleHelper.Pause();
                    return;
                }

                // Validate the appointment is actually completed
                if (appointment.Status != AppointmentStatus.Completed)
                {
                    ConsoleHelper.PrintError($"Appointment {rawId} status is '{appointment.Status}'.");
                    Console.WriteLine("  Use option 6 → Mark as Completed before adding a record.");
                    ConsoleHelper.Pause();
                    return;
                }

                // Get diagnosis
                string diagnosis = InputValidator.GetValidatedInput(
                    "  Diagnosis    : ",
                    InputValidator.IsValidPrescription,
                    "  Diagnosis must have more than 3 characters and must not include only numbers.")!;

                // Get prescription
                string prescription = InputValidator.GetValidatedInput(
                    "  Prescription : ",
                    InputValidator.IsValidPrescription,
                    "  Prescription must have more than 3 characters and must not include only numbers.")!;

                // Get doctor notes
                string doctorNotes = InputValidator.GetValidatedInput(
                    "  Doctor Notes : ",
                    InputValidator.IsValidDoctorNotes,
                    "  Doctor notes must have more than 3 characters and must not include only numbers.")!;

                // Build and persist the health record
                var record = new HealthRecord
                {
                    Patient = appointment.Patient,
                    Doctor = appointment.Doctor,
                    VisitDate = appointment.ScheduledDate,
                    Diagnosis = diagnosis,
                    Prescription = prescription,
                    DoctorNotes = doctorNotes
                };

                string result = _healthRecordService.AddHealthRecord(record);
                ConsoleHelper.PrintSuccess(result);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\n  Returning to menu...");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }

            ConsoleHelper.Pause();
        }

        // View health records by patient, doctor, or record ID
        public void ViewRecord()
        {
            try
            {
                Console.Clear();
                ConsoleHelper.PrintHeader("VIEW HEALTH RECORDS");
                Console.WriteLine("  Type 'q' or 'back' to return.\n");             
                Console.WriteLine("  ╔══════════════════════════════╗");
                Console.WriteLine("  ║  1.  By Patient ID           ║");
                Console.WriteLine("  ║  2.  By Doctor ID            ║");
                Console.WriteLine("  ║  3.  By Record ID            ║");
                Console.WriteLine("  ║  4.  Back                    ║");
                Console.WriteLine("  ╚══════════════════════════════╝");
                Console.Write("\n  Choose an option : ");

                switch (Console.ReadLine()?.Trim() ?? "")
                {
                    case "1":
                        ViewByEntity("Patient", _healthRecordService.GetByPatientIdOrderByVisitDateDesc);
                        break;
                    case "2":
                        ViewByEntity("Doctor", _healthRecordService.GetByDoctorIdOrderByVisitDateDesc);
                        break;
                    case "3":
                        ViewSingleRecord();
                        break;
                    case "4":
                        return;
                    default:
                        ConsoleHelper.PrintError("Invalid choice.");
                        break;
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\n  Returning to menu...");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }

            ConsoleHelper.Pause();
        }

        // Update an existing health record keeping old values where not changed
        public void UpdateHealthRecord()
        {
            try
            {
                Console.Clear();
                ConsoleHelper.PrintHeader("UPDATE HEALTH RECORD");
                Console.WriteLine("  Type 'q' or 'back' to return.\n");

                // Get and validate record ID
                string rawId = InputValidator.GetValidatedInput(
                    "  Enter Record ID : ",
                    InputValidator.IsValidId,
                    ValidPositiveNumber)!;

                HealthRecord existing;
                try
                {
                    existing = _healthRecordService.GetRecordById(int.Parse(rawId))!;
                }
                catch (HealthRecordNotFoundException ex)
                {
                    ConsoleHelper.PrintError(ex.Message);
                    ConsoleHelper.Pause();
                    return;
                }

                // Show current record before editing
                Console.WriteLine("\n  Current Record:");
                Console.WriteLine("  " + new string('─', 55));
                Console.WriteLine($"  {existing.GetSummary()}");
                Console.WriteLine("  " + new string('─', 55));
                Console.WriteLine("\n  Press ENTER to keep existing value.\n");

                // Get optional updated fields
                DateTime? newDate = InputValidator.GetOptionalDate(
                    "  Visit Date (dd/MM/yyyy) : ");

                string? newDiagnosis = InputValidator.GetValidatedInput(
                    "  Diagnosis    : ",
                    InputValidator.IsNonEmpty,
                    "  Invalid input.", allowEmpty: true);

                string? newPrescription = InputValidator.GetValidatedInput(
                    "  Prescription : ",
                    InputValidator.IsNonEmpty,
                    "  Invalid input.", allowEmpty: true);

                string? newNotes = InputValidator.GetValidatedInput(
                    "  Doctor Notes : ",
                    InputValidator.IsNonEmpty,
                    "  Invalid input.", allowEmpty: true);

                // Build updated record — fall back to existing values where unchanged
                var updated = new HealthRecord
                {
                    RecordId = existing.RecordId,
                    Patient = existing.Patient,
                    Doctor = existing.Doctor,
                    VisitDate = newDate ?? existing.VisitDate,
                    Diagnosis = newDiagnosis ?? existing.Diagnosis,
                    Prescription = newPrescription ?? existing.Prescription,
                    DoctorNotes = newNotes ?? existing.DoctorNotes
                };

                var result = _healthRecordService.UpdateHealthRecord(updated);
                ConsoleHelper.PrintSuccess("Record updated successfully!");
                Console.WriteLine($"  {result.GetSummary()}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\n  Returning to menu...");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }

            ConsoleHelper.Pause();
        }


        // Fetch and display records by patient or doctor ID
        private static void ViewByEntity(string entityName, Func<int, List<HealthRecord>> fetch)
        {
            try
            {
                string rawId = InputValidator.GetValidatedInput(
                    $"  Enter {entityName} ID : ",
                    InputValidator.IsValidId,
                    ValidPositiveNumber)!;

                var records = fetch(int.Parse(rawId));

                Console.WriteLine($"\n  {records.Count} record(s) found:\n");

                foreach (var r in records)
                {
                    Console.WriteLine("  " + new string('─', 55));
                    Console.WriteLine($"  {r.GetSummary()}");
                }

                Console.WriteLine("  " + new string('─', 55));
            }
            catch (PatientNotFoundException ex) { ConsoleHelper.PrintError(ex.Message); }
            catch (DoctorNotFoundException ex) { ConsoleHelper.PrintError(ex.Message); }
            catch (HealthRecordNotFoundException ex) { ConsoleHelper.PrintError(ex.Message); }
        }

        // Fetch and display a single record by record ID
        private void ViewSingleRecord()
        {
            try
            {
                string rawId = InputValidator.GetValidatedInput(
                    "  Enter Record ID : ",
                    InputValidator.IsValidId,
                    ValidPositiveNumber)!;

                var record = _healthRecordService.GetRecordById(int.Parse(rawId));

                Console.WriteLine("\n  " + new string('─', 55));
                Console.WriteLine($"  {record!.GetSummary()}");
                Console.WriteLine("  " + new string('─', 55));
            }
            catch (HealthRecordNotFoundException ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }
        }
    }
}