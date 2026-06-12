using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HealthAxis.Mvc.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IDoctorMvcService _doctors;
        private readonly IPatientMvcService _patients;

        public DoctorsController(IDoctorMvcService doctors, IPatientMvcService patients)
        {
            _doctors = doctors;
            _patients = patients;
        }

        public ActionResult Index(SpecialisationEnum? specialisation)
        {
            LoadSpecialisation();

            string selectedSpecialisation = specialisation.HasValue
                ? specialisation.Value.ToString()
                : null;

            var doctors = _doctors.GetAll(selectedSpecialisation);

            return View(doctors);
        }

        public new ActionResult Profile(int? id, int? selectedDoctorId)
        {
            int? finalDoctorId = selectedDoctorId ?? id;

            if (!finalDoctorId.HasValue)
            {
                TempData["Error"] = "Please select a doctor.";
                return RedirectToAction("Index");
            }

            var doctor = _doctors.GetById(finalDoctorId.Value);

            if (doctor == null)
            {
                TempData["Error"] = "Doctor not found.";
                return RedirectToAction("Index");
            }

            return View(doctor);
        }

        public JsonResult Search(string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }

            var doctors = _doctors.Search(searchValue);

            var result = doctors.Select(d => new
            {
                DoctorId = d.DoctorId,
                FullName = d.FullName,
                Specialisation = d.Specialisation.ToString()
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PatientList(
    string insuranceStatus,
    string sortOrder,
    string searchValue,
    int page = 1)
        {
            int pageSize = 10;

            ViewBag.InsuranceStatus = insuranceStatus;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SearchValue = searchValue;

            var patients = _patients.GetAll(insuranceStatus);

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                string searchText = searchValue.Trim().ToLower();

                patients = patients.Where(p =>
                    p.PatientId.ToString().Contains(searchText) ||
                    (!string.IsNullOrWhiteSpace(p.FullName) &&
                        p.FullName.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrWhiteSpace(p.PhoneNumber) &&
                        p.PhoneNumber.Contains(searchText)) ||
                    (!string.IsNullOrWhiteSpace(p.Email) &&
                        p.Email.ToLower().Contains(searchText)));
            }

            if (sortOrder == "name_desc")
            {
                patients = patients.OrderByDescending(p => p.FullName);
            }
            else
            {
                patients = patients.OrderBy(p => p.FullName);
            }

            int totalPatients = patients.Count();

            if (totalPatients == 0)
            {
                ViewBag.CurrentPage = 0;
                ViewBag.TotalPages = 0;
                ViewBag.TotalPatients = 0;

                return View(new List<HealthAxis.Shared.DTOs.PatientDto>());
            }

            int totalPages = (int)Math.Ceiling((double)totalPatients / pageSize);

            if (page < 1)
            {
                page = 1;
            }

            if (page > totalPages)
            {
                page = totalPages;
            }

            var pagedPatients = patients
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalPatients = totalPatients;

            return View(pagedPatients);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Please enter a valid Doctor ID.";
                return RedirectToAction("Profile");
            }

            var doctor = _doctors.GetById(id.Value);

            if (doctor == null)
            {
                TempData["Error"] = "Doctor not found. Please check the ID.";
                return RedirectToAction("Profile");
            }

            return View(doctor);
        }

        public ActionResult Create()
        {
            LoadSpecialisation();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DoctorDto dto)
        {
            ModelState.Remove("DoctorId");
            if (!ModelState.IsValid)
            {
                LoadSpecialisation();
                return View(dto);
            }

            string errorMessage;
            int doctorId;

            bool result = _doctors.Create(
                dto,
                out errorMessage,
                out doctorId);

            if (!result)
            {
                ModelState.AddModelError("", errorMessage);
                LoadSpecialisation();
                return View(dto);
            }

            TempData["Success"] =
                "Doctor registered successfully. Doctor ID is " + doctorId + ".";

            return RedirectToAction(
                "Details",
                new { id = doctorId });
        }

        public ActionResult Edit(int id)
        {
            var doctor = _doctors.GetById(id);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            LoadSpecialisation();

            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadSpecialisation();
                return View(dto);
            }

            string errorMessage;

            bool result = _doctors.Update(dto, out errorMessage);

            if (!result)
            {
                ModelState.AddModelError("", errorMessage);

                LoadSpecialisation();
                return View(dto);
            }

            return RedirectToAction("Index");
        }

        public ActionResult ToggleStatus(int id)
        {
            string errorMessage;

            bool result = _doctors.ToggleStatus(id, out errorMessage);

            if (!result)
            {
                TempData["Error"] = errorMessage;
            }

            return RedirectToAction("Index");
        }

        private void LoadSpecialisation()
        {
            ViewBag.SpecialisationList = new SelectList(
                Enum.GetValues(typeof(SpecialisationEnum)));
        }
    }
}