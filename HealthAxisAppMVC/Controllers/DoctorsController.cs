using HealthAxisApp.Shared.DTOs;
using HealthAxisApp.Shared.Enums;
using HealthAxisAppMVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthAxisAppMVC.Controllers
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


        public ActionResult Index(string specialisation, string searchText)
        {
            LoadSpecialisation();

            ViewBag.Specialisation = specialisation;
            ViewBag.SearchText = searchText;

            var doctors = _doctors.GetAll(specialisation, searchText);

            return View(doctors);
        }

        public ActionResult PatientList(string insuranceStatus, string sortOrder)
        {
            ViewBag.InsuranceStatus = insuranceStatus;
            ViewBag.SortOrder = sortOrder;

            var patients = _patients.GetAll(insuranceStatus);

            if (sortOrder == "name_desc")
            {
                patients = patients.OrderByDescending(p => p.FullName).ToList();
            }
            else
            {
                patients = patients.OrderBy(p => p.FullName).ToList();
            }

            return View(patients);
        }

        public ActionResult Details(int id)
        {
            var doctor = _doctors.GetById(id);

            if (doctor == null)
            {
                return HttpNotFound();
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