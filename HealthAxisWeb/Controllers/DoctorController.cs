using HealthAxis.Shared.Dtos;
using HealthAxis.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAxis.Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorApiService _doctorService;

        public DoctorController(IDoctorApiService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpPost]
        public async Task<JsonResult> ToggleStatus(int id)
        {
            await _doctorService.ToggleStatus(id);
            return Json(true);
        }

        public async Task<ActionResult> Index(
    Specialisation? specialisation,
    bool? isActive)
        {
            var doctors = await _doctorService.GetAll(specialisation);

            if (isActive.HasValue)
            {
                doctors = doctors
                    .Where(d => d.IsActive == isActive.Value)
                    .ToList();
            }

            return View(doctors);
        }

        public async Task<ActionResult> Details(int id)
        {
            var doctor = await _doctorService.GetById(id);

            if (doctor == null)
                return HttpNotFound();

            return View(doctor);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(DoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _doctorService.Create(new CreateDoctorDto
                {
                    FullName = dto.FullName,
                    Specialisation = dto.Specialisation,
                    YearsOfExperience = dto.YearsOfExperience,
                    ConsultationFee = dto.ConsultationFee
                });

                TempData["Success"] = "Doctor added successfully!";
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
            var d = await _doctorService.GetById(id);

            if (d == null)
                return HttpNotFound();

            return View(new UpdateDoctorDto
            {
                FullName = d.FullName,
                Specialisation = d.Specialisation,
                YearsOfExperience = d.YearsOfExperience,
                ConsultationFee = d.ConsultationFee,
                IsActive = d.IsActive
            });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, UpdateDoctorDto dto)
        {
            if (dto.ConsultationFee <= 0)
                ModelState.AddModelError("", "Consultation fee must be positive");

            if (!ModelState.IsValid)
                return View(dto);

            await _doctorService.Update(id, dto);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<JsonResult> GetBySpecialisation(string spec)
        {
            if (!Enum.TryParse(spec, out Specialisation parsedSpec))
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }

            var doctors = await _doctorService.GetAll(parsedSpec);

            return Json(doctors, JsonRequestBehavior.AllowGet);
        }
    }
}