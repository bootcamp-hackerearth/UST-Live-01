using HealthCare.Shared.DTOs.Patient;
using HealthCare.Web.Services.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;
using HealthCare.Shared;
using HealthCare.Web.Services;

namespace HealthCare.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _service;
        private const int PageSize = 10;

        public PatientController(IPatientService service)
        {
            _service = service;
        }

        // LIST → Patient/List.cshtml
        public async Task<ActionResult> List(string searchTerm, int pageNumber = 1)
        {
            var result = await _service.GetPatientsAsync(searchTerm, pageNumber, PageSize);
            return View(result);
        }

        // PROFILE → Patient/Profile.cshtml
        public async Task<ActionResult> Profile(int id)
        {
            if (id == 0)
                TempData["Error"] = "Patient does not exist.";

            var patient = await _service.GetByIdAsync(id);

            if (patient == null)
                TempData["Error"] = "Patient does not exist.";

            return View("List", patient);
        }

        // REGISTER (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(PatientDto dto)
        {
            if (!ModelState.IsValid)
                return PartialView("_RegisterPartial", dto);

            var result = await _service.CreateAsync(dto);

            if (result)
            {
                TempData["Success"] = "Patient created successfully.";
                return RedirectToAction("List");
            }

            ModelState.AddModelError("", "");
            return PartialView("_RegisterPartial", dto);
        }

        // EDIT (GET)
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
                return PartialView("_EditPartial", id);

            var patient = await _service.GetByIdAsync(id);

            if (patient == null)
                return PartialView("_EditPartial", patient);

            return PartialView("_EditPartial", patient);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PagedResult<PatientDto> dto)
        {
            if (!ModelState.IsValid)
                return PartialView("_EditPartial", dto);

            var result = await _service.UpdateAsync(dto.Items[0]);

            if (result)
            {
                TempData["Success"] = "Patient updated successfully.";
                return RedirectToAction("List");
            }

            ModelState.AddModelError("", "Error updating patient");
            return PartialView("_EditPartial", dto);
        }

        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (result)
                TempData["Success"] = "Patient deleted.";
            else
                TempData["Error"] = "Delete failed.";

            return RedirectToAction("List");
        }

        public ActionResult RegisterPartial()
        {
            return PartialView("_RegisterPartial");
        }

        public async Task<ActionResult> EditPartial(int id)
        {
            var patient = await _service.GetByIdAsync(id);
            return PartialView("_EditPartial", patient);
        }

        public async Task<ActionResult> ViewPartial(int id)
        {
            var patient = await _service.GetByIdAsync(id);
            return PartialView("_ViewPartial", patient);
        }
    }
}