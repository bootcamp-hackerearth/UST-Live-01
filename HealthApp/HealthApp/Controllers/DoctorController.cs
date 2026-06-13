using HealthApp.Service.Interface;
using HealthApp.Shared.Constant;
using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthApp.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorApiService _service;

        public DoctorController(IDoctorApiService service)
        {
            _service = service;
        }

        // GET ALL
        public async Task<ActionResult> DoctorIndex()
        {
            var doctors = await _service.GetAll();
            return View(doctors);
        }

        // CREATE
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(DoctorDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(dto);

                await _service.Create(dto);

                TempData["Success"] = "Doctor added successfully!";
                return RedirectToAction("DoctorIndex");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(dto);
            }
        }

        // SEARCH BY SPECIALISATION
        public async Task<ActionResult> SearchBySpecialisation(SpecialisationType? specialisation)
        {
            if (specialisation == null)
                return RedirectToAction("DoctorIndex");

            var doctors = await _service.SearchBySpecialisation(specialisation.Value);
            return View("DoctorIndex", doctors);
        }

        // GET BY ID
        public async Task<JsonResult> GetById(int id)
        {
            try
            {
                var doctor = await _service.GetById(id);
                return Json(doctor, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }



        // TOGGLE STATUS
        public async Task<ActionResult> ToggleStatus(int id)
        {
            try
            {
                await _service.ToggleStatus(id);
                TempData["Success"] = "Status updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("DoctorIndex");
        }

        // SEARCH
        public async Task<ActionResult> Search(string query)
{
    try
    {
        if (string.IsNullOrEmpty(query))
            return RedirectToAction("DoctorIndex");

        var doctors = await _service.GetAll();

        if (int.TryParse(query, out int id))
        {
            var doctor = await _service.GetById(id);

            return View("DoctorIndex", new List<DoctorDto> { doctor });
        }

        var filtered = doctors
            .Where(d => d.FullName.ToLower().Contains(query.ToLower()))
            .ToList();

        if (filtered.Count == 0)
        {
            TempData["Error"] = "No doctors found";
        }

        return View("DoctorIndex", filtered);
    }
    catch (Exception ex)
    {
        // ✅ FIX: ensure message always present
        TempData["Error"] = ex.Message;

        return RedirectToAction("DoctorIndex");
    }
}

   
        // ✅ EDIT GET
        public async Task<ActionResult> Edit(int id)
        {
            var doctor = await _service.GetById(id);
            return View(doctor);
        }

        // ✅ EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, DoctorDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(dto);

                await _service.Update(id, dto);

                TempData["Success"] = "Doctor updated successfully!";
                return RedirectToAction("DoctorIndex");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(dto);
            }
        }

    }
}