using HealthCare.Shared.DTOs.Patient;
using HealthCare.Web.Services.Interfaces;
using HealthCare.Web.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _service;
        private const int PageSize = 10;

        public PatientController()
        {
            _service = new PatientService();
        }

        // Redirect default
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public async Task<ActionResult> List(string searchTerm, int pageNumber = 1)
        {
            var result = await _service.GetPatientsAsync(searchTerm, pageNumber, PageSize);
            return View(result);
        }


        public ActionResult RegisterPartial()
        {
            return PartialView("_RegisterPartial", new CreatePatientDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Return the partial with validation errors so the modal stays open
                return PartialView("_RegisterPartial", dto);
            }

            var result = await _service.CreateAsync(dto);

            if (result)
                return Json(new { success = true, message = "Patient registered successfully." });

            ModelState.AddModelError("", "Error saving patient. Please try again.");
            return PartialView("_RegisterPartial", dto);
        }


        public async Task<ActionResult> ViewPartial(int id)
        {
            var patient = await _service.GetByIdAsync(id);
            if (patient == null)
                return HttpNotFound();

            return PartialView("_ViewPartial", patient);
        }


        public async Task<ActionResult> EditPartial(int id)
        {
            var patient = await _service.GetByIdAsync(id);
            if (patient == null)
                return HttpNotFound();

            return PartialView("_EditPartial", patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditPartial", dto);
            }

            var result = await _service.UpdateAsync(dto);

            if (result)
                return Json(new { success = true, message = "Patient updated successfully." });

            ModelState.AddModelError("", "Update failed. Please try again.");
            return PartialView("_EditPartial", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            TempData[result ? "Success" : "Error"] =
                result ? "Patient deleted successfully." : "Delete failed.";

            return RedirectToAction("List");
        }
    }
}
