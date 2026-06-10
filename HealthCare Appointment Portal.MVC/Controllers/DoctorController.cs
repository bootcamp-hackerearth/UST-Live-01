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
        private readonly IDoctorApiService
            _doctorService;

        public DoctorController(
            IDoctorApiService doctorService)
        {
            _doctorService =
                doctorService;
        }

        // ==================================
        // DASHBOARD
        // ==================================

        public async Task<ActionResult>
            Dashboard()
        {
            try
            {
                int doctorId = 1;

                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(
                            doctorId);

                return View(doctor);
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
        // PROFILE
        // ==================================

        public async Task<ActionResult>
            MyProfile()
        {
            try
            {
                int doctorId = 1;

                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(
                            doctorId);

                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;

                return RedirectToAction(
                    "Index");
            }
        }

        public async Task<ActionResult>
            EditMyProfile()
        {
            try
            {
                int doctorId = 1;

                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(
                            doctorId);

                return View(
                    new UpdateDoctorDto
                    {
                        FullName =
                            doctor.FullName,

                        Specialisation =
                            doctor.Specialisation,

                        YearsOfExperience =
                            doctor.YearsOfExperience,

                        ConsultationFee =
                            doctor.ConsultationFee,

                        IsActive =
                            doctor.IsActive
                    });
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;

                return RedirectToAction(
                    "MyProfile");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            EditMyProfile(
                UpdateDoctorDto doctor)
        {
            if (!ModelState.IsValid)
            {
                return View(doctor);
            }

            try
            {
                int doctorId = 1;

                await _doctorService
                    .UpdateDoctorAsync(
                        doctorId,
                        doctor);

                TempData["Success"] =
                    "Profile updated successfully.";

                return RedirectToAction(
                    "MyProfile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(doctor);
            }
        }

        // ==================================
        // LIST + SEARCH + FILTER
        // ==================================

        public async Task<ActionResult>
            Index(
                string search,
                Specialisation? specialisation)
        {
            try
            {
                var doctors =
                    await _doctorService
                        .GetAllDoctorsAsync();

                // SEARCH

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search =
                        search.ToLower();

                    doctors =
                        doctors.Where(d =>
                            d.FullName.ToLower().Contains(search)
                            ||
                            d.DoctorId.ToString() == search);
                }

                // FILTER

                if (specialisation.HasValue)
                {
                    doctors =
                        doctors.Where(d =>
                            d.Specialisation ==
                            specialisation.Value);
                }

                return View(doctors);
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
                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(id);

                return View(doctor);
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

        public ActionResult
            Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Create(CreateDoctorDto doctor)
        {
            if (!ModelState.IsValid)
            {
                return View(doctor);
            }

            try
            {
                int doctorId =
                    await _doctorService
                        .CreateDoctorAsync(
                            doctor);

                TempData["Success"] =
                    "Doctor created successfully.";

                return RedirectToAction(
                    "Details",
                    new
                    {
                        id = doctorId
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(doctor);
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
                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(id);

                return View(
                    new UpdateDoctorDto
                    {
                        FullName =
                            doctor.FullName,

                        Specialisation =
                            doctor.Specialisation,

                        YearsOfExperience =
                            doctor.YearsOfExperience,

                        ConsultationFee =
                            doctor.ConsultationFee,

                        IsActive =
                            doctor.IsActive
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
                UpdateDoctorDto doctor)
        {
            if (!ModelState.IsValid)
            {
                return View(doctor);
            }

            try
            {
                await _doctorService
                    .UpdateDoctorAsync(
                        id,
                        doctor);

                TempData["Success"] =
                    "Doctor updated successfully.";

                return RedirectToAction(
                    "Details",
                    new
                    {
                        id
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(doctor);
            }
        }

        // ==================================
        // ACTIVATE
        // ==================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Activate(int id)
        {
            try
            {
                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(id);

                var updateDoctor =
                    new UpdateDoctorDto
                    {
                        FullName =
                            doctor.FullName,

                        Specialisation =
                            doctor.Specialisation,

                        YearsOfExperience =
                            doctor.YearsOfExperience,

                        ConsultationFee =
                            doctor.ConsultationFee,

                        IsActive =
                            true
                    };

                await _doctorService
                    .UpdateDoctorAsync(
                        id,
                        updateDoctor);

                TempData["Success"] =
                    "Doctor activated successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;
            }

            return RedirectToAction(
                "Index");
        }

        // ==================================
        // DEACTIVATE
        // ==================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Deactivate(int id)
        {
            try
            {
                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(id);

                var updateDoctor =
                    new UpdateDoctorDto
                    {
                        FullName =
                            doctor.FullName,

                        Specialisation =
                            doctor.Specialisation,

                        YearsOfExperience =
                            doctor.YearsOfExperience,

                        ConsultationFee =
                            doctor.ConsultationFee,

                        IsActive =
                            false
                    };

                await _doctorService
                    .UpdateDoctorAsync(
                        id,
                        updateDoctor);

                TempData["Success"] =
                    "Doctor deactivated successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;
            }

            return RedirectToAction(
                "Index");
        }
    }
}