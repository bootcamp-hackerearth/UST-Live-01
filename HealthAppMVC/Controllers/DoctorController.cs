using HealthAppMVC.Services.Interface;
using HealthAppWebAPI.Models.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAppMVC.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;

        public DoctorController(
            IDoctorService doctorService,
            IAppointmentService appointmentService)
        {
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        public async Task<ActionResult> Index(string search, string specialisation)
        {
            try
            {
                ViewBag.Search = search;
                ViewBag.Specialisation = specialisation;

                var allDoctors = await _doctorService.GetAllDoctorsAsync();

                ViewBag.Specialisations = allDoctors
                    .Select(d => d.Specialisation)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct()
                    .OrderBy(s => s)
                    .ToList();

                var filteredDoctors = allDoctors;

                if (!string.IsNullOrWhiteSpace(search))
                {
                    filteredDoctors = filteredDoctors
                        .Where(d =>
                            d.FullName != null &&
                            d.FullName.ToLower().Contains(search.ToLower()))
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(specialisation))
                {
                    filteredDoctors = filteredDoctors
                        .Where(d =>
                            d.Specialisation != null &&
                            d.Specialisation.Equals(
                                specialisation,
                                StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                return View(filteredDoctors);
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
            return PartialView("_CreateDoctorModal", new CreateDoctorDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateDoctorModal", dto);
            }

            try
            {
                await _doctorService.AddDoctorAsync(dto);

                return Json(new
                {
                    success = true,
                    message = "Doctor added successfully."
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("_CreateDoctorModal", dto);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(id);

                var dto = new CreateDoctorDto
                {
                    FullName = doctor.FullName,
                    Specialisation = doctor.Specialisation,
                    YearsOfExperience = doctor.YearsOfExperience,
                    ConsultationFee = doctor.ConsultationFee,
                    DoctorEmail = doctor.DoctorEmail,
                    DoctorPhoneNo = doctor.DoctorPhoneNo
                };

                ViewBag.DoctorId = id;

                return PartialView("_EditDoctorModal", dto);
            }
            catch (Exception ex)
            {
                return Content(
                    "<div class='modal-body'>" +
                        "<div class='alert alert-danger'>" +
                            Server.HtmlEncode(ex.Message) +
                        "</div>" +
                    "</div>"
                );
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DoctorId = id;
                return PartialView("_EditDoctorModal", dto);
            }

            try
            {
                await _doctorService.UpdateDoctorAsync(id, dto);

                return Json(new
                {
                    success = true,
                    message = "Doctor updated successfully."
                });
            }
            catch (Exception ex)
            {
                ViewBag.DoctorId = id;
                ModelState.AddModelError("", ex.Message);
                return PartialView("_EditDoctorModal", dto);
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(id);
                var appointments = await _appointmentService.GetAppointmentsByDoctorAsync(id);

                ViewBag.Appointments = appointments;

                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> ChangeStatus(int id, bool? isActive)
        {
            bool newStatus;
            try
            {
                if (isActive.HasValue)
                {
                    newStatus = isActive.Value;
                }
                else
                {
                    var doctor = await _doctorService.GetDoctorByIdAsync(id);
                    newStatus = !doctor.IsActive;
                }

                await _doctorService.ChangeDoctorStatusAsync(id, newStatus);

                TempData["Success"] = newStatus
                    ? "Doctor marked as available."
                    : "Doctor marked as not available.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }


    }
}