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
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var doctor = await _service.GetById(id);
                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("DoctorIndex");
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
                    return View("GetById", doctor);
                }

                var filtered = doctors
                    .Where(d => d.FullName.ToLower().Contains(query.ToLower()))
                    .ToList();

                return View("DoctorIndex", filtered);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("DoctorIndex");
            }
        }
    }
}