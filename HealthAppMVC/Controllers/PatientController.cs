using HealthAppMVC.Services.Interface;
using HealthAppWebAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAppMVC.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IHealthRecordService _healthRecordService;

        public PatientController(
            IPatientService patientService,
            IAppointmentService appointmentService,
            IHealthRecordService healthRecordService)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _healthRecordService = healthRecordService;
        }

        public async Task<ActionResult> Index(string search)
        {
            try
            {
                ViewBag.Search = search;

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var result = await _patientService.SearchByNameAsync(search);
                    return View(result);
                }

                var patients = await _patientService.GetAllPatientsAsync();
                return View(patients);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("_CreatePatientModal", new CreatePatientDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreatePatientModal", dto);
            }

            try
            {
                await _patientService.RegisterPatientAsync(dto);

                return Json(new
                {
                    success = true,
                    message = "Patient registered successfully."
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_CreatePatientModal", dto);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var patient = await _patientService.GetPatientByIdAsync(id);

                var dto = new CreatePatientDto
                {
                    FullName = patient.FullName,
                    DateOfBirth = patient.DateOfBirth,
                    Gender = patient.Gender,
                    Email = patient.Email,
                    PhoneNumber = patient.Phone,
                    InsuranceId = patient.InsuranceId
                };

                ViewBag.PatientId = id;

                return PartialView("_EditPatientModal", dto);
            }
            catch (Exception ex)
            {
                return Content("<div class='modal-body'><div class='alert alert-danger'>" + ex.Message + "</div></div>");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PatientId = id;
                return PartialView("_EditPatientModal", dto);
            }

            try
            {
                await _patientService.UpdatePatientAsync(id, dto);

                return Json(new
                {
                    success = true,
                    message = "Patient updated successfully."
                });
            }
            catch (Exception ex)
            {
                ViewBag.PatientId = id;
                ModelState.AddModelError("", ex.Message);
                return PartialView("_EditPatientModal", dto);
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var patient =
                    await _patientService.GetPatientByIdAsync(id);

                if (patient == null)
                {
                    TempData["Error"] = "Patient not found.";
                    return RedirectToAction("Index");
                }

                try
                {
                    var appointments =
                        await _appointmentService.GetAppointmentsByPatientAsync(id);

                    ViewBag.Appointments = appointments;
                }
                catch
                {
                    ViewBag.Appointments =
                        new List<HealthAppWebAPI.Models.Dtos.AppointmentDto>();
                }

                try
                {
                    var healthRecords =
                        await _healthRecordService.GetPatientHistoryAsync(id);

                    ViewBag.HealthRecords = healthRecords;
                }
                catch
                {
                    ViewBag.HealthRecords =
                        new List<HealthAppWebAPI.Models.Dtos.HealthRecordDto>();
                }

                try
                {
                    var appointmentCount =
                        await _patientService.GetAppointmentCountAsync(id);

                    ViewBag.AppointmentCount = appointmentCount;
                }
                catch
                {
                    ViewBag.AppointmentCount = 0;
                }

                return View(patient);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}