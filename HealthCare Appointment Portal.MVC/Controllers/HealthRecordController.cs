using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class HealthRecordController : Controller
    {
        private readonly IHealthRecordApiService
            _healthRecordService;

        private readonly IAppointmentApiService
            _appointmentService;

        public HealthRecordController(
            IHealthRecordApiService healthRecordService,
            IAppointmentApiService appointmentService)
        {
            _healthRecordService =
                healthRecordService;

            _appointmentService =
                appointmentService;
        }

        // ==================================
        // ALL RECORDS + SEARCH
        // ==================================

        public async Task<ActionResult>
            Index(string search)
        {
            try
            {
                var records =
                    await _healthRecordService
                        .GetAllHealthRecordsAsync();

                // SEARCH

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search =
                        search.ToLower();

                    records =
                        records.Where(r =>
                            r.PatientName.ToLower().Contains(search)
                            ||
                            r.DoctorName.ToLower().Contains(search)
                            ||
                            r.Diagnosis.ToLower().Contains(search)
                            ||
                            r.RecordId.ToString() == search);
                }

                return View(records);
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;

                return View();
            }
        }

        // ==================================
        // DETAILS
        // ==================================

        public async Task<ActionResult>
            Details(int id)
        {
            try
            {
                var record =
                    await _healthRecordService
                        .GetHealthRecordByIdAsync(id);

                return View(record);
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;

                return RedirectToAction(
                    "Index");
            }
        }

        // ==================================
        // CREATE
        // ==================================

        public async Task<ActionResult>
            Create()
        {
            // GET EXISTING HEALTH RECORDS

            var records =
                await _healthRecordService
                    .GetAllHealthRecordsAsync();

            // USED APPOINTMENT IDS

            var usedAppointmentIds =
                records.Select(r => r.AppointmentId)
                       .ToList();

            // ONLY COMPLETED APPOINTMENTS
            // WITHOUT HEALTH RECORDS

            var appointments =
                (await _appointmentService
                    .GetAllAppointmentsAsync())
                .Where(a =>
                    a.Status.ToString() == "Completed"
                    &&
                    !usedAppointmentIds.Contains(
                        a.AppointmentId))
                .ToList();

            // APPOINTMENT DROPDOWN

            ViewBag.Appointments =
                new SelectList(
                    appointments.Select(a => new
                    {
                        AppointmentId =
                            a.AppointmentId,

                        DisplayText =
                            $"Appointment #{a.AppointmentId} - {a.PatientName} - Dr. {a.DoctorName}"
                    }),
                    "AppointmentId",
                    "DisplayText");

            // APPOINTMENT DATES

            ViewBag.AppointmentDates =
                appointments.ToDictionary(
                    a => a.AppointmentId.ToString(),
                    a => a.ScheduledDate
                            .ToString("dd MMM yyyy"));

            return View(
                new CreateHealthRecordDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Create(
                CreateHealthRecordDto dto)
        {
            // RELOAD DROPDOWN IF MODEL INVALID

            if (!ModelState.IsValid)
            {
                var records =
                    await _healthRecordService
                        .GetAllHealthRecordsAsync();

                var usedAppointmentIds =
                    records.Select(r => r.AppointmentId)
                           .ToList();

                var appointments =
                    (await _appointmentService
                        .GetAllAppointmentsAsync())
                    .Where(a =>
                        a.Status.ToString() == "Completed"
                        &&
                        !usedAppointmentIds.Contains(
                            a.AppointmentId))
                    .ToList();

                ViewBag.Appointments =
                    new SelectList(
                        appointments.Select(a => new
                        {
                            AppointmentId =
                                a.AppointmentId,

                            DisplayText =
                                $"Appointment #{a.AppointmentId} - {a.PatientName} - Dr. {a.DoctorName}"
                        }),
                        "AppointmentId",
                        "DisplayText");

                ViewBag.AppointmentDates =
                    appointments.ToDictionary(
                        a => a.AppointmentId.ToString(),
                        a => a.ScheduledDate
                                .ToString("dd MMM yyyy"));

                return View(dto);
            }

            try
            {
                // GET SELECTED APPOINTMENT

                var appointment =
                    await _appointmentService
                        .GetAppointmentByIdAsync(
                            dto.AppointmentId);

                // AUTO MAP PATIENT

                dto.PatientId =
                    appointment.PatientId;

                // AUTO MAP DOCTOR

                dto.DoctorId =
                    appointment.DoctorId;

                // AUTO MAP VISIT DATE

                dto.VisitDate =
                    appointment.ScheduledDate;

                // CREATE HEALTH RECORD

                int recordId =
                    await _healthRecordService
                        .CreateHealthRecordAsync(dto);

                TempData["Success"] =
                    "Health record created successfully.";

                return RedirectToAction(
                    "Details",
                    new
                    {
                        id = recordId
                    });
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;

                var records =
                    await _healthRecordService
                        .GetAllHealthRecordsAsync();

                var usedAppointmentIds =
                    records.Select(r => r.AppointmentId)
                           .ToList();

                var appointments =
                    (await _appointmentService
                        .GetAllAppointmentsAsync())
                    .Where(a =>
                        a.Status.ToString() == "Completed"
                        &&
                        !usedAppointmentIds.Contains(
                            a.AppointmentId))
                    .ToList();

                ViewBag.Appointments =
                    new SelectList(
                        appointments.Select(a => new
                        {
                            AppointmentId =
                                a.AppointmentId,

                            DisplayText =
                                $"Appointment #{a.AppointmentId} - {a.PatientName} - Dr. {a.DoctorName}"
                        }),
                        "AppointmentId",
                        "DisplayText");

                ViewBag.AppointmentDates =
                    appointments.ToDictionary(
                        a => a.AppointmentId.ToString(),
                        a => a.ScheduledDate
                                .ToString("dd MMM yyyy"));

                return View(dto);
            }
        }

        // ==================================
        // EDIT
        // ==================================

        public async Task<ActionResult>
            Edit(int id)
        {
            try
            {
                var record =
                    await _healthRecordService
                        .GetHealthRecordByIdAsync(id);

                ViewBag.RecordId =
                    record.RecordId;

                return View(
                    new UpdateHealthRecordDto
                    {
                        Diagnosis =
                            record.Diagnosis,

                        Prescription =
                            record.Prescription,

                        Notes =
                            record.Notes
                    });
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;

                return RedirectToAction(
                    "Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Edit(
                int id,
                UpdateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RecordId = id;

                return View(dto);
            }

            try
            {
                await _healthRecordService
                    .UpdateHealthRecordAsync(
                        id,
                        dto);

                TempData["Success"] =
                    "Health record updated successfully.";

                return RedirectToAction(
                    "Details",
                    new
                    {
                        id
                    });
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;

                ViewBag.RecordId = id;

                return View(dto);
            }
        }

        // ==================================
        // DELETE
        // ==================================

        public async Task<ActionResult>
            Delete(int id)
        {
            try
            {
                var record =
                    await _healthRecordService
                        .GetHealthRecordByIdAsync(id);

                return View(record);
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;

                return RedirectToAction(
                    "Index");
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            DeleteConfirmed(int id)
        {
            try
            {
                await _healthRecordService
                    .DeleteHealthRecordAsync(id);

                TempData["Success"] =
                    "Health record deleted successfully.";

                return RedirectToAction(
                    "Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;

                return RedirectToAction(
                    "Delete",
                    new
                    {
                        id
                    });
            }
        }
    }
}