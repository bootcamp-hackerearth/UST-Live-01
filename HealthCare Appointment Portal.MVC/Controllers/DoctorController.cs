using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorApiService _doctorService;
        private readonly IAppointmentApiService _appointmentService;
        private readonly IHealthRecordApiService _healthRecordService;

        public DoctorController(
            IDoctorApiService doctorService,
            IAppointmentApiService appointmentService,
            IHealthRecordApiService healthRecordService)
        {
            _doctorService = doctorService;
            _appointmentService = appointmentService;
            _healthRecordService = healthRecordService;
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateDoctorDto doctor)
        {
            if (!ModelState.IsValid)
                return View(doctor);

            try
            {
                int doctorId = await _doctorService.CreateDoctorAsync(doctor);

                Session["ReferenceId"] = doctorId;

                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(doctor);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExistingDoctorLogin(int doctorId)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);

                if (doctor == null)
                {
                    TempData["LoginError"] = "No doctor found with this Doctor Id.";
                    return RedirectToAction("Create");
                }

                Session["ReferenceId"] = doctor.DoctorId;

                return RedirectToAction("Dashboard");
            }
            catch
            {
                TempData["LoginError"] = "No doctor found with this Doctor Id.";
                return RedirectToAction("Create");
            }
        }

        public async Task<ActionResult> Dashboard()
        {
            if (Session["ReferenceId"] == null)
                return RedirectToAction("Create");

            try
            {
                int doctorId = Convert.ToInt32(Session["ReferenceId"]);

                var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);

                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Create");
            }
        }

        public async Task<ActionResult> UpcomingAppointments()
        {
            if (Session["ReferenceId"] == null)
                return RedirectToAction("Create");

            try
            {
                int doctorId = Convert.ToInt32(Session["ReferenceId"]);

                var appointments = await _appointmentService.GetAllAppointmentsAsync();

                var upcoming = appointments
                    .Where(a => a.DoctorId == doctorId &&
                                a.Status.ToString() != "Completed" &&
                                a.Status.ToString() != "Cancelled")
                    .OrderBy(a => a.ScheduledDate)
                    .ThenBy(a => a.TimeSlot)
                    .ToList();

                return View(upcoming);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Dashboard");
            }
        }

        public async Task<ActionResult> CompletedAppointments()
        {
            if (Session["ReferenceId"] == null)
                return RedirectToAction("Create");

            try
            {
                int doctorId = Convert.ToInt32(Session["ReferenceId"]);

                var appointments = await _appointmentService.GetAllAppointmentsAsync();

                var completed = appointments
                    .Where(a => a.DoctorId == doctorId &&
                                a.Status.ToString() == "Completed")
                    .OrderByDescending(a => a.ScheduledDate)
                    .ThenBy(a => a.TimeSlot)
                    .ToList();

                var records = await _healthRecordService.GetRecordsByDoctorAsync(doctorId);

                ViewBag.HealthRecords = records
                    .GroupBy(r => r.AppointmentId)
                    .ToDictionary(g => g.Key, g => g.First());

                return View(completed);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Dashboard");
            }
        }
        public async Task<ActionResult> CancelledAppointments()
        {
            if (Session["ReferenceId"] == null)
                return RedirectToAction("Create");

            try
            {
                int doctorId = Convert.ToInt32(Session["ReferenceId"]);

                var appointments = await _appointmentService.GetAllAppointmentsAsync();

                var cancelled = appointments
                    .Where(a => a.DoctorId == doctorId &&
                                a.Status.ToString() == "Cancelled")
                    .OrderByDescending(a => a.ScheduledDate)
                    .ThenBy(a => a.TimeSlot)
                    .ToList();

                return View(cancelled);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Dashboard");
            }
        }

        public async Task<ActionResult> MyProfile()
        {
            if (Session["ReferenceId"] == null)
                return RedirectToAction("Create");

            try
            {
                int doctorId = Convert.ToInt32(Session["ReferenceId"]);

                var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);

                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Dashboard");
            }
        }

        public async Task<ActionResult> EditMyProfile()
        {
            if (Session["ReferenceId"] == null)
                return RedirectToAction("Create");

            try
            {
                int doctorId = Convert.ToInt32(Session["ReferenceId"]);

                var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);

                var dto = new UpdateDoctorDto
                {
                    FullName = doctor.FullName,
                    Specialisation = doctor.Specialisation,
                    YearsOfExperience = doctor.YearsOfExperience,
                    ConsultationFee = doctor.ConsultationFee,
                    IsActive = doctor.IsActive
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Dashboard");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMyProfile(UpdateDoctorDto dto)
        {
            if (Session["ReferenceId"] == null)
                return RedirectToAction("Create");

            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                int doctorId = Convert.ToInt32(Session["ReferenceId"]);

                await _doctorService.UpdateDoctorAsync(doctorId, dto);

                ViewBag.Success = "Doctor profile updated successfully.";

                return View(dto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        public async Task<ActionResult> SearchDoctors()
        {
            try
            {
                var doctors = await _doctorService.GetAllDoctorsAsync();

                var activeDoctors = doctors
                    .Where(d => d.IsActive)
                    .ToList();

                return View("~/Views/Doctor/SearchDoctors.cshtml", activeDoctors);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Dashboard", "Patient");
            }
        }

        public async Task<ActionResult> Index(Specialisation? specialisation)
        {
            try
            {
                if (specialisation.HasValue)
                {
                    var doctors = await _doctorService
                        .GetDoctorsBySpecialisationAsync(specialisation.Value);

                    return View(doctors);
                }

                var allDoctors = await _doctorService.GetAllDoctorsAsync();

                return View(allDoctors);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(id);

                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}