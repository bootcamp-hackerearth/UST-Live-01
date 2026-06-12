using HealthApp.Service.Interface;
using HealthApp.Shared.DTOs;
using System;
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

        public async Task<ActionResult> GetById(int id)
        {
            var patient = await _service.GetById(id);
            return View(patient);
        }

        public ActionResult Create()
        {
            return View();
        }

        // ✅ ✅ ✅ FIXED CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PatientDto dto)
        {
            try
            {
                // ✅ IMPORTANT: validate DTO
                if (!ModelState.IsValid)
                {
                    return View(dto); // stay on same page and show red errors
                }

                await _service.Create(dto);

                return RedirectToAction("PatientIndex"); // only when valid
            }
            catch (Exception ex)
            {
                // ✅ show backend error also in red
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        public async Task<ActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
                return RedirectToAction("PatientIndex");

            var patients = await _service.GetAll();

            if (int.TryParse(query, out int id))
            {
                var result = patients.Find(p => p.PatientId == id);
                return View("GetById", result);
            }

            var filtered = patients
                .FindAll(p => p.FullName.ToLower().Contains(query.ToLower()));

            return View("PatientIndex", filtered);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var patient = await _service.GetById(id);
            return View(patient);
        }

        // ✅ ✅ ✅ FIXED EDIT ALSO (same logic)
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
