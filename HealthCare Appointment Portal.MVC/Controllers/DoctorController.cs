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

        private const string ErrorKey = "Error";
        private const string IndexAction = "Index";
        private const string DetailsAction = "Details";

        private readonly IDoctorApiService _doctorService;

        public DoctorController(IDoctorApiService doctorService)
        {
            _doctorService = doctorService;
        }

        public async Task<ActionResult> Index(
    Specialisation? specialisation,
    string searchQuery = null)
        {
            try
            {
                ViewBag.CurrentSearch =
                    searchQuery;

                ViewBag.CurrentSpecialisation =
                    specialisation;

                IEnumerable<DoctorDto> doctors;

                if (!string.IsNullOrWhiteSpace(
                    searchQuery))
                {
                    doctors =
                        await _doctorService
                            .GetDoctorsByNameAsync(
                                searchQuery);
                }
                else if (specialisation.HasValue)
                {
                    doctors =
                        await _doctorService
                            .GetDoctorsBySpecialisationAsync(
                                specialisation.Value);
                }
                else
                {
                    doctors =
                        await _doctorService
                            .GetAllDoctorsAsync();
                }

                return View(
                    IndexAction,
                    doctors);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] =
                    ex.Message;

                return View(IndexAction);
            }
        }


        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(id);

                return View(DetailsAction, doctor);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return RedirectToAction(IndexAction);
            }
        }

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
                await _doctorService
                    .CreateDoctorAsync(doctor);

                TempData["SuccessMessage"] =
                    "Doctor added successfully.";

                return RedirectToAction(IndexAction);
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

                TempData["SuccessMessage"] =
                    "Doctor updated successfully.";

                return RedirectToAction(IndexAction);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Edit", doctor);
            }
        }


        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(id);

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
                await _doctorService
                    .DeleteDoctorAsync(id);

                TempData["SuccessMessage"] =
                    "Doctor deleted successfully.";

                return RedirectToAction(
                    IndexAction);
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;

                return RedirectToAction(
                    IndexAction);
            }
        }
    }
}