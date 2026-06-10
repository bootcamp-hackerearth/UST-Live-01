using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
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

        public DoctorController(
            IDoctorApiService doctorService,
            IAppointmentApiService appointmentService)
        {
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        // ==================================
        // DOCTOR DASHBOARD
        // ==================================

        public async Task<ActionResult> Dashboard()
        {
            try
            {
                if (Session["ReferenceId"] == null)
                {
                    return RedirectToAction("Create");
                }

                int doctorId =
                    Convert.ToInt32(Session["ReferenceId"]);

                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(doctorId);

                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("Create");
            }
        }

        // ==================================
        // EXISTING DOCTOR LOGIN
        // ==================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExistingDoctorLogin(int doctorId)
        {
            try
            {
                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(doctorId);

                if (doctor == null)
                {
                    TempData["LoginError"] =
                        "No doctor found with this Doctor Id.";

                    return RedirectToAction("Create");
                }

                Session["ReferenceId"] = doctor.DoctorId;

                return RedirectToAction("Dashboard");
            }
            catch
            {
                TempData["LoginError"] =
                    "No doctor found with this Doctor Id.";

                return RedirectToAction("Create");
            }
        }

        // ==================================
        // DOCTOR UPCOMING APPOINTMENTS
        // ==================================

        public async Task<ActionResult> UpcomingAppointments()
        {
            try
            {
                if (Session["ReferenceId"] == null)
                {
                    return RedirectToAction("Create");
                }

                int doctorId =
                    Convert.ToInt32(Session["ReferenceId"]);

                var appointments =
                    await _appointmentService
                        .GetAllAppointmentsAsync();

                var doctorAppointments =
                    appointments
                        .Where(a => a.DoctorId == doctorId)
                        .OrderBy(a => a.ScheduledDate)
                        .ThenBy(a => a.TimeSlot)
                        .ToList();

                return View(doctorAppointments);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("Dashboard");
            }
        }

        // ==================================
        // DOCTOR PROFILE
        // ==================================

        public async Task<ActionResult> MyProfile()
        {
            try
            {
                if (Session["ReferenceId"] == null)
                {
                    return RedirectToAction("Create");
                }

                int doctorId =
                    Convert.ToInt32(Session["ReferenceId"]);

                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(doctorId);

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
            {
                return RedirectToAction("Create");
            }

            try
            {
                int doctorId =
                    Convert.ToInt32(Session["ReferenceId"]);

                var doctor =
                    await _doctorService.GetDoctorByIdAsync(doctorId);

                var dto =
                    new UpdateDoctorDto
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
                TempData["Error"] =
                    ex.Message;

                return RedirectToAction("Dashboard");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMyProfile(UpdateDoctorDto dto)
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction("Create");
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                int doctorId =
                    Convert.ToInt32(Session["ReferenceId"]);

                await _doctorService.UpdateDoctorAsync(doctorId, dto);

                ViewBag.Success =
                    "Doctor profile updated successfully.";

                return View(dto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(dto);
            }
        }

        // ==================================
        // TOGGLE STATUS
        // ==================================

        public async Task<ActionResult> ToggleStatus()
        {
            try
            {
                if (Session["ReferenceId"] == null)
                {
                    return RedirectToAction("Create");
                }

                int doctorId =
                    Convert.ToInt32(Session["ReferenceId"]);

                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(doctorId);

                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("Dashboard");
            }
        }

        [HttpPost]
        [ActionName("ToggleStatus")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ToggleStatusConfirmed()
        {
            try
            {
                if (Session["ReferenceId"] == null)
                {
                    return RedirectToAction("Create");
                }

                int doctorId =
                    Convert.ToInt32(Session["ReferenceId"]);

                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(doctorId);

                var updateDoctor =
                    new UpdateDoctorDto
                    {
                        FullName = doctor.FullName,
                        Specialisation = doctor.Specialisation,
                        YearsOfExperience = doctor.YearsOfExperience,
                        ConsultationFee = doctor.ConsultationFee,
                        IsActive = !doctor.IsActive
                    };

                await _doctorService
                    .UpdateDoctorAsync(doctorId, updateDoctor);

                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("Dashboard");
            }
        }

        // ==================================
        // ADMIN & PATIENT DOCTOR LIST
        // ==================================

        public async Task<ActionResult> Index(Specialisation? specialisation)
        {
            try
            {
                if (specialisation.HasValue)
                {
                    var doctors =
                        await _doctorService
                            .GetDoctorsBySpecialisationAsync(
                                specialisation.Value);

                    return View(doctors);
                }

                var allDoctors =
                    await _doctorService
                        .GetAllDoctorsAsync();

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
                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(id);

                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("Index");
            }
        }

        // ==================================
        // NEW DOCTOR REGISTRATION
        // ==================================

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateDoctorDto doctor)
        {
            if (!ModelState.IsValid)
            {
                return View(doctor);
            }

            try
            {
                int doctorId =
                    await _doctorService
                        .CreateDoctorAsync(doctor);

                Session["ReferenceId"] = doctorId;

                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(doctor);
            }
        }

        // ==================================
        // ADMIN EDIT DOCTOR
        // ==================================

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(id);

                return View(
                    new UpdateDoctorDto
                    {
                        FullName = doctor.FullName,
                        Specialisation = doctor.Specialisation,
                        YearsOfExperience = doctor.YearsOfExperience,
                        ConsultationFee = doctor.ConsultationFee,
                        IsActive = doctor.IsActive
                    });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UpdateDoctorDto doctor)
        {
            if (!ModelState.IsValid)
            {
                return View(doctor);
            }

            try
            {
                await _doctorService
                    .UpdateDoctorAsync(id, doctor);

                return RedirectToAction(
                    "Details",
                    new
                    {
                        id
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(doctor);
            }
        }

        // ==================================
        // ADMIN DEACTIVATE DOCTOR
        // ==================================

        public async Task<ActionResult> Deactivate(int id)
        {
            try
            {
                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(id);

                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeactivateConfirmed(int id)
        {
            try
            {
                await _doctorService
                    .DeleteDoctorAsync(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(id);

                return View(doctor);
            }
        }
    }
}