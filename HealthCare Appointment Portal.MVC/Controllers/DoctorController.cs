using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorApiService _doctorService;

        public DoctorController(IDoctorApiService doctorService)
        {
            _doctorService = doctorService;
        }

        // GET: Doctor/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Doctor/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                ViewBag.Error = "Please enter Doctor ID.";
                return View();
            }

            int doctorId;

            if (!int.TryParse(userId, out doctorId))
            {
                ViewBag.Error = "Invalid Doctor ID. Please enter numbers only.";
                return View();
            }

            if (doctorId <= 0)
            {
                ViewBag.Error = "Invalid Doctor ID. Please enter a valid Doctor ID.";
                return View();
            }

            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);

                if (doctor == null)
                {
                    ViewBag.Error = "Doctor not found. Please enter a valid Doctor ID.";
                    return View();
                }

                Session["ReferenceId"] = doctorId;

                return RedirectToAction("Dashboard");
            }
            catch (Exception)
            {
                ViewBag.Error = "Doctor not found. Please enter a valid Doctor ID.";
                return View();
            }
        }

        // ==================================
        // DOCTOR
        // ==================================

        // GET: Doctor/Dashboard
        public async Task<ActionResult> Dashboard()
        {
            try
            {
                int doctorId;

                if (!TryGetDoctorIdFromSession(out doctorId))
                {
                    TempData["Error"] = "Session expired or invalid. Please login again.";
                    return RedirectToAction("Login");
                }

                var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);

                if (doctor == null)
                {
                    Session.Remove("ReferenceId");
                    TempData["Error"] = "Doctor not found. Please login with a valid Doctor ID.";
                    return RedirectToAction("Login");
                }

                return View(doctor);
            }
            catch (Exception)
            {
                Session.Remove("ReferenceId");
                TempData["Error"] = "Unable to load doctor dashboard. Please login with a valid Doctor ID.";
                return RedirectToAction("Login");
            }
        }

        // GET: Doctor/MyProfile
        public async Task<ActionResult> MyProfile()
        {
            try
            {
                int doctorId;

                if (!TryGetDoctorIdFromSession(out doctorId))
                {
                    TempData["Error"] = "Session expired or invalid. Please login again.";
                    return RedirectToAction("Login");
                }

                var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);

                if (doctor == null)
                {
                    Session.Remove("ReferenceId");
                    TempData["Error"] = "Doctor not found. Please login with a valid Doctor ID.";
                    return RedirectToAction("Login");
                }

                return View(doctor);
            }
            catch (Exception)
            {
                Session.Remove("ReferenceId");
                TempData["Error"] = "Unable to load doctor profile. Please login again.";
                return RedirectToAction("Login");
            }
        }

        // GET: Doctor/EditMyProfile
        public async Task<ActionResult> EditMyProfile()
        {
            try
            {
                int doctorId;

                if (!TryGetDoctorIdFromSession(out doctorId))
                {
                    TempData["Error"] = "Session expired or invalid. Please login again.";
                    return RedirectToAction("Login");
                }

                var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);

                if (doctor == null)
                {
                    Session.Remove("ReferenceId");
                    TempData["Error"] = "Doctor not found. Please login with a valid Doctor ID.";
                    return RedirectToAction("Login");
                }

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
            catch (Exception)
            {
                Session.Remove("ReferenceId");
                TempData["Error"] = "Unable to load profile for editing. Please login again.";
                return RedirectToAction("Login");
            }
        }

        // POST: Doctor/EditMyProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMyProfile(UpdateDoctorDto doctor)
        {
            if (!ModelState.IsValid)
            {
                return View(doctor);
            }

            try
            {
                int doctorId;

                if (!TryGetDoctorIdFromSession(out doctorId))
                {
                    TempData["Error"] = "Session expired or invalid. Please login again.";
                    return RedirectToAction("Login");
                }

                await _doctorService.UpdateDoctorAsync(doctorId, doctor);

                return RedirectToAction("MyProfile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(doctor);
            }
        }

        // GET: Doctor/ToggleStatus
        public async Task<ActionResult> ToggleStatus()
        {
            try
            {
                int doctorId;

                if (!TryGetDoctorIdFromSession(out doctorId))
                {
                    TempData["Error"] = "Session expired or invalid. Please login again.";
                    return RedirectToAction("Login");
                }

                var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);

                if (doctor == null)
                {
                    Session.Remove("ReferenceId");
                    TempData["Error"] = "Doctor not found. Please login with a valid Doctor ID.";
                    return RedirectToAction("Login");
                }

                return View(doctor);
            }
            catch (Exception)
            {
                Session.Remove("ReferenceId");
                TempData["Error"] = "Unable to load status. Please login again.";
                return RedirectToAction("Login");
            }
        }

        // POST: Doctor/ToggleStatus
        [HttpPost]
        [ActionName("ToggleStatus")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ToggleStatusConfirmed()
        {
            try
            {
                int doctorId;

                if (!TryGetDoctorIdFromSession(out doctorId))
                {
                    TempData["Error"] = "Session expired or invalid. Please login again.";
                    return RedirectToAction("Login");
                }

                var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);

                if (doctor == null)
                {
                    Session.Remove("ReferenceId");
                    TempData["Error"] = "Doctor not found. Please login with a valid Doctor ID.";
                    return RedirectToAction("Login");
                }

                var updateDoctor = new UpdateDoctorDto
                {
                    FullName = doctor.FullName,
                    Specialisation = doctor.Specialisation,
                    YearsOfExperience = doctor.YearsOfExperience,
                    ConsultationFee = doctor.ConsultationFee,
                    IsActive = !doctor.IsActive
                };

                await _doctorService.UpdateDoctorAsync(doctorId, updateDoctor);

                return RedirectToAction("MyProfile");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("MyProfile");
            }
        }

        // ==================================
        // ADMIN & PATIENT
        // ==================================

        // GET: Doctor
        public async Task<ActionResult> Index(Specialisation? specialisation)
        {
            try
            {
                if (specialisation.HasValue)
                {
                    var doctors =
                        await _doctorService
                            .GetDoctorsBySpecialisationAsync(specialisation.Value);

                    return View(doctors);
                }

                var allDoctors = await _doctorService.GetAllDoctorsAsync();

                return View(allDoctors);
            }
            catch (Exception)
            {
                TempData["Error"] = "Unable to load doctors.";

                return View(new List<DoctorDto>());
            }
        }

        // GET: Doctor/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Invalid Doctor ID.";
                    return RedirectToAction("Index");
                }

                var doctor = await _doctorService.GetDoctorByIdAsync(id);

                if (doctor == null)
                {
                    TempData["Error"] = "Doctor not found.";
                    return RedirectToAction("Index");
                }

                return View(doctor);
            }
            catch (Exception)
            {
                TempData["Error"] = "Doctor not found.";

                return RedirectToAction("Index");
            }
        }

        // ==================================
        // ADMIN ONLY
        // ==================================

        // GET: Doctor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doctor/Create
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
                    await _doctorService.CreateDoctorAsync(doctor);

                return RedirectToAction(
                    "Details",
                    new
                    {
                        id = doctorId
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(doctor);
            }
        }

        // GET: Doctor/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Invalid Doctor ID.";
                    return RedirectToAction("Index");
                }

                var doctor = await _doctorService.GetDoctorByIdAsync(id);

                if (doctor == null)
                {
                    TempData["Error"] = "Doctor not found.";
                    return RedirectToAction("Index");
                }

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
            catch (Exception)
            {
                TempData["Error"] = "Doctor not found.";

                return RedirectToAction("Index");
            }
        }

        // POST: Doctor/Edit/5
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
                if (id <= 0)
                {
                    TempData["Error"] = "Invalid Doctor ID.";
                    return RedirectToAction("Index");
                }

                await _doctorService.UpdateDoctorAsync(id, doctor);

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

        // GET: Doctor/Deactivate/5
        public async Task<ActionResult> Deactivate(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Invalid Doctor ID.";
                    return RedirectToAction("Index");
                }

                var doctor = await _doctorService.GetDoctorByIdAsync(id);

                if (doctor == null)
                {
                    TempData["Error"] = "Doctor not found.";
                    return RedirectToAction("Index");
                }

                return View(doctor);
            }
            catch (Exception)
            {
                TempData["Error"] = "Doctor not found.";

                return RedirectToAction("Index");
            }
        }

        // POST: Doctor/Deactivate/5
        [HttpPost]
        [ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeactivateConfirmed(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Invalid Doctor ID.";
                    return RedirectToAction("Index");
                }

                await _doctorService.DeleteDoctorAsync(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                try
                {
                    var doctor = await _doctorService.GetDoctorByIdAsync(id);

                    return View(doctor);
                }
                catch
                {
                    TempData["Error"] = "Unable to deactivate doctor.";

                    return RedirectToAction("Index");
                }
            }
        }

        // ==================================
        // PRIVATE HELPER
        // ==================================

        private bool TryGetDoctorIdFromSession(out int doctorId)
        {
            doctorId = 0;

            if (Session["ReferenceId"] == null)
            {
                return false;
            }

            return int.TryParse(
                Session["ReferenceId"].ToString(),
                out doctorId);
        }
    }
}