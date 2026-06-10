using HealthAxis.Shared.Dtos;
using HealthAxis.Web.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAxis.Web.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentApiService _service;

        public AppointmentController(IAppointmentApiService service)
        {
            _service = service;
        }

        public async Task<ActionResult> ByDoctor(int id)
        {
            var data = await _service.GetByDoctor(id);

            if (data == null)
            {
                ViewBag.Error = "Invalid Doctor ID";
                return View("EnterDoctorId");
            }

            return View(data);
        }

        public async Task<ActionResult> ByPatient(int id)
        {
            var data = await _service.GetByPatient(id);

            if (data == null)
            {
                ViewBag.Error = "Invalid Patient ID";
                return View("EnterPatientId");
            }

            return View(data);
        }

        public async Task<ActionResult> Confirm(int id)
        {
            var appointment = await _service.GetById(id);

            if (appointment == null)
            {
                TempData["Error"] = "Invalid appointment";
                return RedirectToAction("Index", "Doctor");
            }

            if (appointment.Status != AppointmentStatus.Pending)
            {
                TempData["Error"] = "Only pending appointments can be confirmed";
                return Redirect(Request.UrlReferrer.ToString());
            }

            await _service.UpdateStatus(id, AppointmentStatus.Confirmed);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public async Task<ActionResult> Complete(int id)
        {
            var appointment = await _service.GetById(id);

            if (appointment == null)
            {
                TempData["Error"] = "Invalid appointment";
                return RedirectToAction("Index", "Doctor");
            }

            if (appointment.Status != AppointmentStatus.Confirmed)
            {
                TempData["Error"] = "Only confirmed appointments can be completed";
                return Redirect(Request.UrlReferrer.ToString());
            }

            await _service.UpdateStatus(id, AppointmentStatus.Completed);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Cancel(int id)
        {
            return View(new CancelAppointmentDto());
        }

        [HttpPost]
        public async Task<ActionResult> Cancel(int id, CancelAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var appointment = await _service.GetById(id);

            if (appointment == null)
            {
                TempData["Error"] = "Invalid appointment";
                return RedirectToAction("Index", "Doctor");
            }

            if (appointment.Status != AppointmentStatus.Pending &&
                appointment.Status != AppointmentStatus.Confirmed)
            {
                TempData["Error"] = "Only pending or confirmed appointments can be cancelled";
                return Redirect(Request.UrlReferrer.ToString());
            }

            await _service.Cancel(id, dto);

            TempData["Success"] = "Appointment cancelled successfully";
            return RedirectToAction("Index", "Doctor");
        }
    }
}