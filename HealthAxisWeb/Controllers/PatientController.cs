using HealthAxis.Shared.Dtos;
using HealthAxis.Web.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;

namespace HealthAxis.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientApiService _patientService;

        public PatientController(IPatientApiService patientService)
        {
            _patientService = patientService;
        }

        public async Task<ActionResult> Index(bool? isActive, bool? insured)
        {
            var patients = await _patientService.GetAll();

            if (isActive.HasValue)
            {
                patients = patients
                    .Where(p => p.IsActive == isActive.Value)
                    .ToList();
            }

            if (insured.HasValue)
            {
                patients = patients
                    .Where(p => insured.Value
                        ? !string.IsNullOrEmpty(p.InsuranceId)
                        : string.IsNullOrEmpty(p.InsuranceId))
                    .ToList();
            }

            return View(patients);
        }

        public async Task<ActionResult> Details(int id)
        {
            var patient = await _patientService.GetById(id);

            if (patient == null)
                return HttpNotFound();

            return View(patient);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(PatientDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                var result = await _patientService.Register(dto);

                if (!result.Success)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(dto);
                }

                TempData["Success"] = "Patient added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var p = await _patientService.GetById(id);

            if (p == null)
                return HttpNotFound();

            return View(p);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, PatientDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _patientService.Update(id, dto);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(dto);
            }

            TempData["Success"] = "Patient updated successfully!";
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public async Task<JsonResult> Deactivate(int id)
        {
            var result = await _patientService.Deactivate(id);
            return Json(result.Success);
        }
    }
}