using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal.Utilities;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public partial class AppointmentController : Controller
    {
        private readonly IAppointmentApiService
            _appointmentService;

        private readonly IDoctorApiService
            _doctorService;

        public AppointmentController(
            IAppointmentApiService appointmentService,
            IDoctorApiService doctorService)
        {
            _appointmentService =
                appointmentService;

            _doctorService =
                doctorService;
        }

        #region Private Helpers

        private bool IsLoggedIn()
        {
            return Session[
                Constants.ReferenceIdKey]
                != null;
        }

        private int GetReferenceId()
        {
            return Convert.ToInt32(
                Session[
                    Constants.ReferenceIdKey]);
        }

        private RedirectToRouteResult
            RedirectToLogin()
        {
            return RedirectToAction(
                Constants.LoginAction,
                Constants.UserController);
        }

        private async Task
            LoadDoctorsAsync()
        {
            var doctors =
                (await _doctorService
                    .GetAllDoctorsAsync())
                .Where(
                    d => d.IsActive);

            ViewBag.Doctors = new SelectList(
                doctors.Select(d => new
                {
                     DoctorId = d.DoctorId,
                     DisplayText = d.FullName + " (" + d.Specialisation + ")"
                }),
                "DoctorId",
                "DisplayText");
        }

        #endregion

        // =====================================
        // ADMIN
        // =====================================

        public async Task<ActionResult>
            Index(
                string appointmentId = "",
                string patientName = "",
                string doctorName = "",
                string status = "",
                int page = 1)
        {
            try
            {
                var appointments =
                    (await _appointmentService
                        .GetAllAppointmentsAsync())
                    .ToList();

                if (!string.IsNullOrWhiteSpace(
                    appointmentId))
                {
                    appointments =
                        appointments
                        .Where(a =>
                            a.AppointmentId
                             .ToString()
                             .Contains(
                                appointmentId))
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(
                    patientName))
                {
                    appointments =
                        appointments
                        .Where(a =>
                            !string.IsNullOrWhiteSpace(
                                a.PatientName)
                            &&
                            a.PatientName
                             .IndexOf(
                                patientName,
                                StringComparison
                                    .OrdinalIgnoreCase)
                             >= 0)
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(
                    doctorName))
                {
                    appointments =
                        appointments
                        .Where(a =>
                            !string.IsNullOrWhiteSpace(
                                a.DoctorName)
                            &&
                            a.DoctorName
                             .IndexOf(
                                doctorName,
                                StringComparison
                                    .OrdinalIgnoreCase)
                             >= 0)
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(
                    status))
                {
                    appointments =
                        appointments
                        .Where(a =>
                            a.Status
                             .ToString()
                             .Equals(
                                status,
                                StringComparison
                                    .OrdinalIgnoreCase))
                        .ToList();
                }

                const int pageSize = 5;

                int totalRecords =
                    appointments.Count;

                int totalPages =
                    Math.Max(
                        1,
                        (int)Math.Ceiling(
                            (double)totalRecords /
                            pageSize));

                if (page < 1)
                {
                    page = 1;
                }

                if (page > totalPages)
                {
                    page = totalPages;
                }

                appointments =
                    appointments
                    .OrderByDescending(
                        a => a.AppointmentId)
                    .Skip(
                        (page - 1)
                        * pageSize)
                    .Take(pageSize)
                    .ToList();

                ViewBag.CurrentPage =
                    page;

                ViewBag.TotalPages =
                    totalPages;

                ViewBag.TotalRecords =
                    totalRecords;

                ViewBag.AppointmentId =
                    appointmentId;

                ViewBag.PatientName =
                    patientName;

                ViewBag.DoctorName =
                    doctorName;

                ViewBag.Status =
                    status;

                return View(
                    appointments);
            }
            catch (Exception ex)
            {
                TempData[
                    Constants.ErrorKey] =
                    ex.Message;

                return View(
                    Enumerable.Empty<
                        AppointmentDto>());
            }
        }

        // =====================================
        // COMMON
        // =====================================

        public async Task<ActionResult>
            Details(
                int id)
        {
            try
            {
                var appointment =
                    await _appointmentService
                        .GetAppointmentByIdAsync(
                            id);

                return View(
                    appointment);
            }
            catch (Exception ex)
            {
                TempData[
                    Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.IndexAction);
            }
        }

        public async Task<ActionResult>
            GetDetailsModal(
                int id)
        {
            try
            {
                var appointment =
                    await _appointmentService
                        .GetAppointmentByIdAsync(
                            id);

                return PartialView(
                    "_AppointmentDetailsModal",
                    appointment);
            }
            catch (Exception ex)
            {
                return Content(
                    $"<div class='alert alert-danger'>{ex.Message}</div>");
            }
        }

        public async Task<ActionResult>
            ConfirmModal(
                int id)
        {
            try
            {
                var appointment =
                    await _appointmentService
                        .GetAppointmentByIdAsync(
                            id);

                return PartialView(
                    "_ConfirmAppointmentModal",
                    appointment);
            }
            catch (Exception ex)
            {
                return Content(
                    $"<div class='alert alert-danger'>{ex.Message}</div>");
            }
        }

        public async Task<ActionResult>
            CompleteModal(
                int id)
        {
            try
            {
                var appointment =
                    await _appointmentService
                        .GetAppointmentByIdAsync(
                            id);

                return PartialView(
                    "_CompleteAppointmentModal",
                    appointment);
            }
            catch (Exception ex)
            {
                return Content(
                    $"<div class='alert alert-danger'>{ex.Message}</div>");
            }
        }

        public async Task<ActionResult>
            CancelModal(
                int id)
        {
            try
            {
                var appointment =
                    await _appointmentService
                        .GetAppointmentByIdAsync(
                            id);

                return PartialView(
                    "_CancelAppointmentModal",
                    appointment);
            }
            catch (Exception ex)
            {
                return Content(
                    $"<div class='alert alert-danger'>{ex.Message}</div>");
            }
        }
        // =====================================
        // PATIENT
        // =====================================

        public async Task<ActionResult>
            Create()
        {
            if (!IsLoggedIn())
            {
                return RedirectToLogin();
            }

            await LoadDoctorsAsync();

            return View(
                new CreateAppointmentDto
                {
                    ScheduledDate =
                        DateTime.Today
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Create(
                CreateAppointmentDto dto)
        {
            if (!IsLoggedIn())
            {
                return RedirectToLogin();
            }

            if (!ModelState.IsValid)
            {
                await LoadDoctorsAsync();

                return View(dto);
            }

            try
            {
                dto.PatientId =
                    GetReferenceId();

                int appointmentId =
                    await _appointmentService
                        .CreateAppointmentAsync(
                            dto);

                TempData[
                    Constants.SuccessKey] =
                    "Appointment created successfully.";

                return RedirectToAction(
                    Constants.DetailsAction,
                    new
                    {
                        id = appointmentId
                    });
            }
            catch (Exception ex)
            {
                await LoadDoctorsAsync();

                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(dto);
            }
        }

        public async Task<ActionResult>
            MyAppointments(
                string appointmentId = "",
                string doctor = "",
                string date = "",
                string timeSlot = "",
                string status = "",
                int page = 1)
        {
            try
            {
                if (!IsLoggedIn())
                {
                    return RedirectToLogin();
                }

                var allAppointments =
                    (await _appointmentService
                        .GetAppointmentsByPatientAsync(
                            GetReferenceId()))
                    .ToList();

                var appointments =
                    ApplyFilters(
                        allAppointments,
                        appointmentId,
                        doctor,
                        date,
                        timeSlot,
                        status);

                const int pageSize = 5;

                int totalRecords =
                    appointments.Count;

                int totalPages =
                    Math.Max(
                        1,
                        (int)Math.Ceiling(
                            (double)totalRecords /
                            pageSize));

                if (page < 1)
                {
                    page = 1;
                }

                if (page > totalPages)
                {
                    page = totalPages;
                }

                appointments =
                    appointments
                    .OrderByDescending(
                        a => a.AppointmentId)
                    .Skip(
                        (page - 1)
                        * pageSize)
                    .Take(pageSize)
                    .ToList();

                ViewBag.TotalRecords =
                    totalRecords;

                ViewBag.CurrentPage =
                    page;

                ViewBag.TotalPages =
                    totalPages;

                ViewBag.AppointmentId =
                    appointmentId;

                ViewBag.Doctor =
                    doctor;

                ViewBag.Date =
                    date;

                ViewBag.TimeSlot =
                    timeSlot;

                ViewBag.Status =
                    status;

                ViewBag.TimeSlots =
                    GetTimeSlots();

                ViewBag.Doctors =
                    allAppointments
                    .Select(a => a.DoctorName)
                    .Distinct()
                    .OrderBy(d => d)
                    .ToList();

                ViewBag.Dates =
                    allAppointments
                    .Select(a => a.ScheduledDate.Date)
                    .Distinct()
                    .OrderBy(d => d)
                    .ToList();

                return View(
                    appointments);
            }
            catch (Exception ex)
            {
                TempData[
                    Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.DashboardAction,
                    Constants.PatientController);
            }
        }

        // =====================================
        // DOCTOR
        // =====================================

        public async Task<ActionResult>
            TodaySchedule(
                string appointmentId = "",
                string patientName = "",
                string timeSlot = "",
                string status = "",
                int page = 1)
        {
            try
            {
                if (!IsLoggedIn())
                {
                    return RedirectToLogin();
                }

                var appointments =
                    (await _appointmentService
                        .GetTodayScheduleAsync(
                            GetReferenceId()))
                    .ToList();

                appointments =
                    ApplyDoctorFilters(
                        appointments,
                        appointmentId,
                        patientName,
                        timeSlot,
                        status);

                const int pageSize = 5;

                int totalRecords =
                    appointments.Count;

                int totalPages =
                    Math.Max(
                        1,
                        (int)Math.Ceiling(
                            (double)totalRecords /
                            pageSize));

                if (page < 1)
                {
                    page = 1;
                }

                if (page > totalPages)
                {
                    page = totalPages;
                }

                appointments =
                    ApplyPagination(
                        appointments
                        .OrderBy(
                            a => a.TimeSlot)
                        .ToList(),
                        page,
                        pageSize);

                ViewBag.TotalRecords =
                    totalRecords;

                ViewBag.CurrentPage =
                    page;

                ViewBag.TotalPages =
                    totalPages;

                ViewBag.AppointmentId =
                    appointmentId;

                ViewBag.PatientName =
                    patientName;

                ViewBag.TimeSlot =
                    timeSlot;

                ViewBag.Status =
                    status;

                ViewBag.TimeSlots =
                    GetTimeSlots();

                return View(
                    appointments);
            }
            catch (Exception ex)
            {
                TempData[
                    Constants.ErrorKey] =
                    ex.Message;

                return View(
                    Enumerable.Empty<
                        AppointmentDto>());
            }
        }

        public async Task<ActionResult>
            WeeklySchedule(
                string appointmentId = "",
                string patientName = "",
                string timeSlot = "",
                string status = "",
                int page = 1)
        {
            try
            {
                if (!IsLoggedIn())
                {
                    return RedirectToLogin();
                }

                var appointments =
                    (await _appointmentService
                        .GetWeeklyScheduleAsync(
                            GetReferenceId()))
                    .ToList();

                appointments =
                    ApplyDoctorFilters(
                        appointments,
                        appointmentId,
                        patientName,
                        timeSlot,
                        status);

                const int pageSize = 5;

                int totalRecords =
                    appointments.Count;

                int totalPages =
                    Math.Max(
                        1,
                        (int)Math.Ceiling(
                            (double)totalRecords /
                            pageSize));

                if (page < 1)
                {
                    page = 1;
                }

                if (page > totalPages)
                {
                    page = totalPages;
                }

                appointments =
                    ApplyPagination(
                        appointments
                        .OrderBy(
                            a => a.ScheduledDate)
                        .ThenBy(
                            a => a.TimeSlot)
                        .ToList(),
                        page,
                        pageSize);

                ViewBag.TotalRecords =
                    totalRecords;

                ViewBag.CurrentPage =
                    page;

                ViewBag.TotalPages =
                    totalPages;

                ViewBag.AppointmentId =
                    appointmentId;

                ViewBag.PatientName =
                    patientName;

                ViewBag.TimeSlot =
                    timeSlot;

                ViewBag.Status =
                    status;

                ViewBag.TimeSlots =
                    GetTimeSlots();

                return View(
                    appointments);
            }
            catch (Exception ex)
            {
                TempData[
                    Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.DashboardAction,
                    Constants.DoctorController);
            }
        }
        // =====================================
        // AJAX ACTIONS
        // =====================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult>
            ConfirmAppointment(
                int id)
        {
            try
            {
                await _appointmentService
                    .ConfirmAppointmentAsync(
                        id);

                return Json(
                    new
                    {
                        success = true,
                        message =
                            "Appointment confirmed successfully."
                    });
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false,
                        message =
                            ex.Message
                    });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult>
            CompleteAppointment(
                int id)
        {
            try
            {
                await _appointmentService
                    .CompleteAppointmentAsync(
                        id);

                var appointment =
                    await _appointmentService
                        .GetAppointmentByIdAsync(
                            id);

                return Json(
                    new
                    {
                        success = true,
                        message =
                            "Appointment completed successfully.",
                        appointmentId =
                            appointment.AppointmentId,
                        patientId =
                            appointment.PatientId
                    });
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false,
                        message =
                            ex.Message
                    });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult>
            CancelAppointment(
                int id,
                string reason)
        {
            try
            {
                await _appointmentService
                    .CancelAppointmentAsync(
                        id,
                        reason);

                return Json(
                    new
                    {
                        success = true,
                        message =
                            "Appointment cancelled successfully."
                    });
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false,
                        message =
                            ex.Message
                    });
            }
        }

        // =====================================
        // FILTERS
        // =====================================

        private static List<AppointmentDto>
            ApplyFilters(
                List<AppointmentDto> appointments,
                string appointmentId,
                string doctor,
                string date,
                string timeSlot,
                string status)
        {
            if (!string.IsNullOrWhiteSpace(
                appointmentId))
            {
                appointments =
                    appointments
                    .Where(a =>
                        a.AppointmentId
                         .ToString()
                         .Contains(
                            appointmentId))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(
                doctor))
            {
                appointments =
                    appointments
                    .Where(a =>
                        !string.IsNullOrWhiteSpace(
                            a.DoctorName)
                        &&
                        a.DoctorName
                         .IndexOf(
                            doctor,
                            StringComparison
                                .OrdinalIgnoreCase)
                         >= 0)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(
                date))
            {
                appointments =
                    appointments
                    .Where(a =>
                        a.ScheduledDate
                         .ToString(
                            "yyyy-MM-dd")
                         .Equals(
                            date))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(
                timeSlot))
            {
                appointments =
                    appointments
                    .Where(a =>
                        a.TimeSlot ==
                        timeSlot)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(
                status))
            {
                appointments =
                    appointments
                    .Where(a =>
                        a.Status
                         .ToString()
                         .Equals(
                            status,
                            StringComparison
                                .OrdinalIgnoreCase))
                    .ToList();
            }

            return appointments;
        }

        private static List<AppointmentDto>
            ApplyDoctorFilters(
                List<AppointmentDto> appointments,
                string appointmentId,
                string patientName,
                string timeSlot,
                string status)
        {
            if (!string.IsNullOrWhiteSpace(
                appointmentId))
            {
                appointments =
                    appointments
                    .Where(a =>
                        a.AppointmentId
                         .ToString()
                         .Contains(
                            appointmentId))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(
                patientName))
            {
                appointments =
                    appointments
                    .Where(a =>
                        !string.IsNullOrWhiteSpace(
                            a.PatientName)
                        &&
                        a.PatientName
                         .IndexOf(
                            patientName,
                            StringComparison
                                .OrdinalIgnoreCase)
                         >= 0)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(
                timeSlot))
            {
                appointments =
                    appointments
                    .Where(a =>
                        a.TimeSlot ==
                        timeSlot)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(
                status))
            {
                appointments =
                    appointments
                    .Where(a =>
                        a.Status
                         .ToString()
                         .Equals(
                            status,
                            StringComparison
                                .OrdinalIgnoreCase))
                    .ToList();
            }

            return appointments;
        }

        private static List<AppointmentDto>
            ApplyPagination(
                List<AppointmentDto> appointments,
                int page,
                int pageSize)
        {
            return appointments
                .Skip(
                    (page - 1)
                    * pageSize)
                .Take(
                    pageSize)
                .ToList();
        }

        private static List<SelectListItem>
            GetTimeSlots()
        {
            return new List<SelectListItem>
            {
               new SelectListItem { Text = "06:00 - 07:00", Value = "06:00 - 07:00" },
               new SelectListItem { Text = "07:00 - 08:00", Value = "07:00 - 08:00" },
               new SelectListItem { Text = "08:00 - 09:00", Value = "08:00 - 09:00" },
               new SelectListItem { Text = "09:00 - 10:00", Value = "09:00 - 10:00" },
               new SelectListItem { Text = "10:00 - 11:00", Value = "10:00 - 11:00" },
               new SelectListItem { Text = "11:00 - 12:00", Value = "11:00 - 12:00" },
               new SelectListItem { Text = "12:00 - 13:00", Value = "12:00 - 13:00" },
               new SelectListItem { Text = "13:00 - 14:00", Value = "13:00 - 14:00" },
               new SelectListItem { Text = "14:00 - 15:00", Value = "14:00 - 15:00" },
               new SelectListItem { Text = "15:00 - 16:00", Value = "15:00 - 16:00" },
               new SelectListItem { Text = "16:00 - 17:00", Value = "16:00 - 17:00" },
               new SelectListItem { Text = "17:00 - 18:00", Value = "17:00 - 18:00" },
               new SelectListItem { Text = "18:00 - 19:00", Value = "18:00 - 19:00" },
               new SelectListItem { Text = "19:00 - 20:00", Value = "19:00 - 20:00" },
               new SelectListItem { Text = "20:00 - 21:00", Value = "20:00 - 21:00" },
               new SelectListItem { Text = "21:00 - 22:00", Value = "21:00 - 22:00" },
               new SelectListItem { Text = "22:00 - 23:00", Value = "22:00 - 23:00" },
               new SelectListItem { Text = "23:00 - 00:00", Value = "23:00 - 00:00" },
               new SelectListItem { Text = "00:00 - 01:00", Value = "00:00 - 01:00" },
               new SelectListItem { Text = "01:00 - 02:00", Value = "01:00 - 02:00" },
               new SelectListItem { Text = "02:00 - 03:00", Value = "02:00 - 03:00" },
               new SelectListItem { Text = "03:00 - 04:00", Value = "03:00 - 04:00" },
               new SelectListItem { Text = "04:00 - 05:00", Value = "04:00 - 05:00" },
               new SelectListItem { Text = "05:00 - 06:00", Value = "05:00 - 06:00" }
            };
        }
    }
}