using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorApiService _doctorService;

        public DoctorController(
            IDoctorApiService doctorService)
        {
            _doctorService = doctorService;
        }

        #region Private Helpers

        private int GetDoctorId()
        {
            return Convert.ToInt32(
                Session[Constants.ReferenceIdKey]);
        }

        #endregion

        // ==================================
        // DOCTOR
        // ==================================

        public async Task<ActionResult> Dashboard()
        {
            try
            {
                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(
                            GetDoctorId());

                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.IndexAction);
            }
        }

        public async Task<ActionResult> MyProfile()
        {
            try
            {
                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(
                            GetDoctorId());

                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.IndexAction);
            }
        }

        public async Task<ActionResult> EditMyProfile()
        {
            try
            {
                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(
                            GetDoctorId());

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
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.MyProfileAction);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMyProfile(
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
                        GetDoctorId(),
                        doctor);

                return RedirectToAction(
                    Constants.MyProfileAction);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(doctor);
            }
        }

        public async Task<ActionResult> ToggleStatus()
        {
            try
            {
                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(
                            GetDoctorId());

                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.MyProfileAction);
            }
        }

        [HttpPost]
        [ActionName("ToggleStatus")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ToggleStatusConfirmed()
        {
            try
            {
                int doctorId =
                    GetDoctorId();

                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(
                            doctorId);

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
                            !doctor.IsActive
                    };

                await _doctorService
                    .UpdateDoctorAsync(
                        doctorId,
                        updateDoctor);

                return RedirectToAction(
                    Constants.MyProfileAction);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.MyProfileAction);
            }
        }

        // ==================================
        // ADMIN & PATIENT
        // ==================================

        public async Task<ActionResult> Index(
            Specialisation? specialisation)
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
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return View();
            }
        }

        public async Task<ActionResult> Details(
            int id)
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
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.IndexAction);
            }
        }

        // ==================================
        // ADMIN ONLY
        // ==================================

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            CreateDoctorDto doctor)
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

                return RedirectToAction(
                    Constants.DetailsAction,
                    new
                    {
                        id = doctorId
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(doctor);
            }
        }

        public async Task<ActionResult> Edit(
            int id)
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
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.IndexAction);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
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

                return RedirectToAction(
                    Constants.DetailsAction,
                    new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(doctor);
            }
        }

        public async Task<ActionResult> Deactivate(
            int id)
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
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.IndexAction);
            }
        }

        [HttpPost]
        [ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeactivateConfirmed(
            int id)
        {
            try
            {
                await _doctorService
                    .DeleteDoctorAsync(id);

                return RedirectToAction(
                    Constants.IndexAction);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(id);

                return View(doctor);
            }
        }
    }
}