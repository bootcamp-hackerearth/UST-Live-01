using HealthApp.Service.Interface;
using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthApp.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientApiService _service;

        public PatientController(IPatientApiService service)
        {
            _service = service;
        }

        public async Task<ActionResult> PatientIndex()
        {
            var patients = await _service.GetAll();
            return View(patients);
        }

        // ✅ JSON for modal view
        public async Task<JsonResult> GetById(int id)
        {
            try
            {
                var patient = await _service.GetById(id);
                return Json(patient, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Create()
        {
            return View();
        }

        // ✅ CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PatientDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                await _service.Create(dto);

                return RedirectToAction("PatientIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        // ✅ SEARCH (ID or NAME)
        public async Task<ActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
                return RedirectToAction("PatientIndex");

            try
            {
                // ✅ SEARCH BY ID (use service → exception comes from backend)
                if (int.TryParse(query, out int id))
                {
                    var result = await _service.GetById(id);

                    return View("PatientIndex", new List<PatientDto> { result });
                }

                // ✅ SEARCH BY NAME
                var patients = await _service.GetAll();

                var filtered = patients
                    .Where(p => p.FullName.ToLower().Contains(query.ToLower()))
                    .ToList();

                if (filtered.Count == 0)
                {
                    TempData["Error"] = "No patients found";
                }

                return View("PatientIndex", filtered);
            }
            catch (Exception ex)
            {
                TempData["Error"] = string.IsNullOrWhiteSpace(ex.Message)
                    ? "Patient not found"
                    : ex.Message;

                return RedirectToAction("PatientIndex");
            }
        }


        // ✅ EDIT (GET)
        public async Task<ActionResult> Edit(int id)
        {
            var patient = await _service.GetById(id);
            return View(patient);
        }

        // ✅ EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, PatientDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                await _service.Update(id, dto);

                return RedirectToAction("PatientIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }
    }
}
