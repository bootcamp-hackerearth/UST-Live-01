using HealthAxis.Shared.Dtos;
using HealthAxis.Web.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace HealthAxis.Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorApiService _doctorService;
        private readonly IAppointmentApiService _appointmentService;

        public DoctorController(IDoctorApiService doctorService, IAppointmentApiService appointmentService)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
        }

        public async Task<ActionResult> Index(Specialisation? specialisation,bool? isActive)
        {
            var doctors = await _doctorService.GetAll(specialisation);

            if (doctors == null)
            {
                doctors = new List<DoctorDto>();
            }

            if (isActive.HasValue)
            {
                doctors = doctors
                    .Where(d => d.IsActive == isActive.Value)
                    .ToList();
            }

            ViewBag.Total = doctors.Count;
            ViewBag.Active = doctors.Count(d => d.IsActive);
            ViewBag.Inactive = doctors.Count(d => !d.IsActive);

            return View(doctors);
        }
        public async Task<ActionResult> Details(int id)
        {
            var doctor = await _doctorService.GetById(id);

            if (doctor == null)
            {
                ViewBag.Error = "Doctor not found";
                return RedirectToAction("Index");
            }

            return View(doctor);
        }

        public ActionResult Create()
        {
            return View(new CreateDoctorDto());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", dto);
            }

            var result = await _doctorService.Create(dto);

            if (result == null)
            {
                ViewBag.Error = "Something went wrong";
                return View("Create", dto);
            }

            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View("Create", dto);
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var doctor = await _doctorService.GetById(id);

            if (doctor == null)
            {
                ViewBag.Error = "Doctor not found";
                return RedirectToAction("Index");
            }

            var dto = new UpdateDoctorDto
            {
                FullName = doctor.FullName,
                Specialisation = doctor.Specialisation,
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = doctor.IsActive
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, UpdateDoctorDto dto)
        {
            if (dto.ConsultationFee <= 0)
            {
                ModelState.AddModelError("", "Consultation fee must be positive");
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result = await _doctorService.Update(id, dto);

            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View(dto);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> ToggleStatus(int id)
        {
            await _doctorService.ToggleStatus(id);
            return Json(true);
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
        public async Task<ActionResult> DoctorSchedule(int id, int? patientId)
        {
            var appointments = await _appointmentService.GetByDoctor(id);


            if (appointments == null)
                appointments = new List<AppointmentDto>();

            appointments = appointments
                .OrderByDescending(a => a.ScheduledDate)
                .ThenByDescending(a => a.AppointmentId)
                .ToList();

            if (patientId.HasValue)
            {
                appointments = appointments
                    .Where(a => a.PatientId == patientId.Value)
                    .ToList();
            }

            return View(appointments);
        }

    }
}