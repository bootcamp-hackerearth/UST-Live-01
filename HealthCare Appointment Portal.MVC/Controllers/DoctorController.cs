using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class DoctorController : Controller
    {
        // ==================================
        // CONSTANTS (Fixes Sonar S1192)
        // ==================================
        private const string ErrorKey = "Error";
        private const string IndexAction = "Index";
        private const string DetailsAction = "Details";

        private readonly IDoctorApiService _doctorService;

        public DoctorController(IDoctorApiService doctorService)
        {
            _doctorService = doctorService;
        }

        public async Task<ActionResult> Index(Specialisation? specialisation)
        {
            try
            {
                if (specialisation.HasValue)
                {
                    var doctors = await _doctorService.GetDoctorsBySpecialisationAsync(specialisation.Value);
                    return View(IndexAction, doctors);
                }

                var allDoctors = await _doctorService.GetAllDoctorsAsync();
                return View(IndexAction, allDoctors);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return View(IndexAction);
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(id);

                // Explicit view name prevents Sonar duplicate method implementation issue
                return View(DetailsAction, doctor);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return RedirectToAction(IndexAction);
            }
        }

        // ==================================
        // ADMIN ONLY
        // ==================================

        public ActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateDoctorDto doctor)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", doctor);
            }

            try
            {
                int doctorId = await _doctorService.CreateDoctorAsync(doctor);
                return RedirectToAction(DetailsAction, new { id = doctorId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Create", doctor);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(id);

                return View("Edit", new UpdateDoctorDto
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
                TempData[ErrorKey] = ex.Message;
                return RedirectToAction(IndexAction);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UpdateDoctorDto doctor)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", doctor);
            }

            try
            {
                await _doctorService.UpdateDoctorAsync(id, doctor);
                return RedirectToAction(DetailsAction, new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Edit", doctor);
            }
        }

        // ==================================
        // UPDATED: Deactivate is now Delete
        // ==================================

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(id);

                // Explicit view name prevents Sonar duplicate method implementation issue
                return View("Delete", doctor);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return RedirectToAction(IndexAction);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _doctorService.DeleteDoctorAsync(id);
                return RedirectToAction(IndexAction);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var doctor = await _doctorService.GetDoctorByIdAsync(id);
                return View("Delete", doctor);
            }
        }
    }
}