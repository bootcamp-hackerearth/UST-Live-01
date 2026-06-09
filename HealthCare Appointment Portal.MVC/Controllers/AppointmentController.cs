using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal.Utilities;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentApiService _appointmentService;
        private readonly IDoctorApiService _doctorService;

        public AppointmentController(
            IAppointmentApiService appointmentService,
            IDoctorApiService doctorService)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
        }

        #region Private Helpers

        private bool IsLoggedIn()
        {
            return Session[Constants.ReferenceIdKey] != null;
        }

        private int GetReferenceId()
        {
            return Convert.ToInt32(
                Session[Constants.ReferenceIdKey]);
        }

        private RedirectToRouteResult RedirectToLogin()
        {
            return RedirectToAction(
                Constants.LoginAction,
                Constants.UserController);
        }

        private async Task LoadDoctorsAsync()
        {
            var doctors =
                (await _doctorService
                    .GetAllDoctorsAsync())
                .Where(d => d.IsActive);

            ViewBag.Doctors =
                new SelectList(
                    doctors,
                    "DoctorId",
                    "FullName");
        }

        #endregion

        // ==================================
        // ADMIN
        // ==================================

        public async Task<ActionResult> Index()
        {
            try
            {
                var appointments =
                    await _appointmentService
                        .GetAllAppointmentsAsync();

                return View(appointments);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return View(
                    Enumerable.Empty<AppointmentDto>());
            }
        }

        // ==================================
        // COMMON
        // ==================================

        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var appointment =
                    await _appointmentService
                        .GetAppointmentByIdAsync(id);

                return View(appointment);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.IndexAction);
            }
        }

        // ==================================
        // PATIENT
        // ==================================

        public async Task<ActionResult> Create()
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
        public async Task<ActionResult> Create(
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
                        .CreateAppointmentAsync(dto);

                TempData[Constants.SuccessKey] =
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

        public async Task<ActionResult> MyAppointments()
        {
            try
            {
                if (!IsLoggedIn())
                {
                    return RedirectToLogin();
                }

                var appointments =
                    await _appointmentService
                        .GetAppointmentsByPatientAsync(
                            GetReferenceId());

                return View(appointments);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.DashboardAction,
                    Constants.PatientController);
            }
        }

        // ==================================
        // DOCTOR
        // ==================================

        public async Task<ActionResult> TodaySchedule()
        {
            try
            {
                if (!IsLoggedIn())
                {
                    return RedirectToLogin();
                }

                var appointments =
                    await _appointmentService
                        .GetTodayScheduleAsync(
                            GetReferenceId());

                return View(appointments);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.DashboardAction,
                    Constants.DoctorController);
            }
        }

        public async Task<ActionResult> WeeklySchedule()
        {
            try
            {
                if (!IsLoggedIn())
                {
                    return RedirectToLogin();
                }

                var appointments =
                    await _appointmentService
                        .GetWeeklyScheduleAsync(
                            GetReferenceId());

                return View(appointments);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.DashboardAction,
                    Constants.DoctorController);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(int id)
        {
            try
            {
                await _appointmentService
                    .ConfirmAppointmentAsync(id);

                TempData[Constants.SuccessKey] =
                    "Appointment confirmed successfully.";

                return RedirectToAction(
                    Constants.DetailsAction,
                    new { id });
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.DetailsAction,
                    new { id });
            }
        }

        public async Task<ActionResult> Cancel(int id)
        {
            try
            {
                var appointment =
                    await _appointmentService
                        .GetAppointmentByIdAsync(id);

                return View(appointment);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.DetailsAction,
                    new { id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(
            int id,
            string reason)
        {
            try
            {
                await _appointmentService
                    .CancelAppointmentAsync(
                        id,
                        reason);

                TempData[Constants.SuccessKey] =
                    "Appointment cancelled successfully.";

                return RedirectToAction(
                    Constants.DetailsAction,
                    new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                var appointment =
                    await _appointmentService
                        .GetAppointmentByIdAsync(id);

                return View(appointment);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Complete(int id)
        {
            try
            {
                await _appointmentService
                    .CompleteAppointmentAsync(id);

                var appointment =
                    await _appointmentService
                        .GetAppointmentByIdAsync(id);

                TempData[Constants.SuccessKey] =
                    "Appointment completed successfully.";

                return RedirectToAction(
                    Constants.CreateAction,
                    Constants.HealthRecordController,
                    new
                    {
                        appointmentId =
                            appointment.AppointmentId,

                        patientId =
                            appointment.PatientId
                    });
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.DetailsAction,
                    new { id });
            }
        }
    }
}