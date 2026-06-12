using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;
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

        public DoctorController(
            IDoctorApiService doctorService)
        {
            _doctorService =
                doctorService;
        }

        #region Private Helpers

        private int GetDoctorId()
        {
            return Convert.ToInt32(
                Session[
                    Constants.ReferenceIdKey]);
        }

        private async Task<UpdateDoctorDto>
            BuildUpdateDoctorDtoAsync(
                int doctorId)
        {
            var doctor =
                await _doctorService
                    .GetDoctorByIdAsync(
                        doctorId);

            return new UpdateDoctorDto
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
            };
        }

        #endregion

        // ==================================
        // DOCTOR
        // ==================================

        public async Task<ActionResult>
            Dashboard()
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
                TempData[
                    Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.IndexAction);
            }
        }

        public async Task<ActionResult>
            MyProfile()
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
                TempData[
                    Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.IndexAction);
            }
        }

        public async Task<ActionResult>
            EditMyProfile()
        {
            try
            {
                return View(
                    await BuildUpdateDoctorDtoAsync(
                        GetDoctorId()));
            }
            catch (Exception ex)
            {
                TempData[
                    Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.MyProfileAction);
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
                await _doctorService
                    .UpdateDoctorAsync(
                        GetDoctorId(),
                        doctor);

                TempData[
                    Constants.SuccessKey] =
                    "Profile updated successfully.";

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

        public async Task<ActionResult>
            ToggleStatus()
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
                TempData[
                    Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.MyProfileAction);
            }
        }

        [HttpPost]
        [ActionName("ToggleStatus")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            ToggleStatusConfirmed()
        {
            try
            {
                int doctorId =
                    GetDoctorId();

                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(
                            doctorId);

                await _doctorService
                    .UpdateDoctorAsync(
                        doctorId,
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
                        });

                TempData[
                    Constants.SuccessKey] =
                    "Status updated successfully.";

                return RedirectToAction(
                    Constants.MyProfileAction);
            }
            catch (Exception ex)
            {
                TempData[
                    Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.MyProfileAction);
            }
        }

        // ==================================
        // ADMIN & PATIENT
        // ==================================

        public async Task<ActionResult>
            Index(
                string doctorName = "",
                string specialisation = "",
                int page = 1)
        {
            try
            {
                var doctors =
                    (await _doctorService
                        .GetAllDoctorsAsync())
                    .ToList();

                ViewBag.Specialisations =
                    doctors
                    .Select(d =>
                        d.Specialisation
                         .ToString())
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
                if (!string.IsNullOrWhiteSpace(
                    doctorName))
                {
                    doctorName =
                        doctorName.Trim();

                    doctors =
                        doctors
                        .Where(d =>
                            !string.IsNullOrWhiteSpace(
                                d.FullName)
                            &&
                            d.FullName.IndexOf(
                                doctorName,
                                StringComparison
                                    .OrdinalIgnoreCase) >= 0)
                        .ToList();
                }
                if (!string.IsNullOrWhiteSpace(
                    specialisation))
                {
                    doctors =
                        doctors
                        .Where(d =>
                            d.Specialisation
                             .ToString()
                             .Equals(
                                specialisation,
                                StringComparison
                                    .OrdinalIgnoreCase))
                        .ToList();
                }

                const int pageSize = 5;

                int totalRecords =
                    doctors.Count;

                int totalPages =
                    (int)Math.Ceiling(
                        (double)totalRecords
                        / pageSize);

                var pagedDoctors =
                    doctors
                    .Skip(
                        (page - 1)
                        * pageSize)
                    .Take(
                        pageSize)
                    .ToList();

                ViewBag.CurrentPage =
                    page;

                ViewBag.TotalPages =
                    totalPages;

                ViewBag.TotalRecords =
                    totalRecords;

                ViewBag.DoctorName =
                    doctorName;

                ViewBag.Specialisation =
                    specialisation;

                return View(
                    pagedDoctors);
            }
            catch (Exception ex)
            {
                TempData[
                    Constants.ErrorKey] =
                    ex.Message;

                return View();
            }
        }

        public async Task<ActionResult>
            Details(
                int id)
        {
            try
            {
                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(
                            id);

                return View(doctor);
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

        [HttpGet]
        public async Task<ActionResult>
            GetDoctorDetailsModal(
                int id)
        {
            try
            {
                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(
                            id);

                return PartialView(
                    "_DoctorDetailsModal",
                    doctor);
            }
            catch (Exception ex)
            {
                return Content(
                    $"<div class='alert alert-danger'>{ex.Message}</div>");
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
        public async Task<ActionResult>
            Create(
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

        public async Task<ActionResult>
            Edit(
                int id)
        {
            try
            {
                return View(
                    await BuildUpdateDoctorDtoAsync(
                        id));
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

                TempData[
                    Constants.SuccessKey] =
                    "Doctor updated successfully.";

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

        public async Task<ActionResult>
            Deactivate(
                int id)
        {
            try
            {
                var doctor =
                    await _doctorService
                        .GetDoctorByIdAsync(
                            id);

                return View(doctor);
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

        [HttpPost]
        [ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            DeactivateConfirmed(
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