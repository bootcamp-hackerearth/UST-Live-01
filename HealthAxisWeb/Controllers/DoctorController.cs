using HealthAxis.Shared.Dtos;
using HealthAxis.Web.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAxis.Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorApiService _doctorService;
        private readonly IAppointmentApiService _appointmentService;

        public DoctorController(IDoctorApiService doctorService,
                                IAppointmentApiService appointmentService)
        {
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        public async Task<ActionResult> Index()
        {
            var doctors = await _doctorService.GetAllDoctors();
            return View(doctors);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(DoctorDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _doctorService.AddDoctor(dto);
            TempData["Success"] = "Doctor added successfully!";
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return View("EnterDoctorId");

            return RedirectToAction("EditDoctor", new { id });
        }

        public async Task<ActionResult> EditDoctor(int id)
        {
            var doctor = await _doctorService.GetDoctorById(id);

            if (doctor == null)
            {
                ViewBag.Error = "Invalid Doctor ID";
                return View("EnterDoctorId");
            }

            return View("Edit", doctor);
        }

        [HttpPost]
        public async Task<ActionResult> EditDoctor(int id, DoctorDto dto)
        {
            if (!ModelState.IsValid) return View("Edit", dto);

            await _doctorService.UpdateDoctor(id, dto);
            TempData["Success"] = "Doctor updated successfully!";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public JsonResult ValidateDoctor(int id)
        {
            var doctor = _doctorService.GetDoctorById(id).Result;

            if (doctor == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AppointmentList(int id)
        {
            var appointments = await _appointmentService.GetByDoctor(id);

            if (appointments == null)
            {
                ViewBag.Error = "Invalid Doctor ID";
                return View("EnterDoctorId");
            }

            return View(appointments);
        }
        public async Task<ActionResult> ByDoctor(int id)
        {
            var appointments = await _appointmentService.GetByDoctor(id);

            if (appointments == null)
            {
                ViewBag.Error = "Invalid Doctor ID";
                return View();
            }

            return View(appointments);
        }
        public async Task<ActionResult> Confirm(int id)
        {
            await _appointmentService.UpdateStatus(id, AppointmentStatus.Confirmed);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public async Task<ActionResult> Complete(int id)
        {
            await _appointmentService.UpdateStatus(id, AppointmentStatus.Completed);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Cancel(int id)
        {
            return View(new CancelAppointmentDto());
        }

        [HttpPost]
        public async Task<ActionResult> Cancel(int id, CancelAppointmentDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _appointmentService.Cancel(id, dto);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ViewPatients()
        {
            var patients = await _doctorService.GetAllPatients();
            return View(patients);
        }
    }
}
