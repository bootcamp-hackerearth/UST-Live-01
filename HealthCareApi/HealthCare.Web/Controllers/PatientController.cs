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

        // ✅ Redirect default
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        // ✅ LIST PAGE
        public async Task<ActionResult> List(string searchTerm, int pageNumber = 1)
        {
            var result = await _service.GetPatientsAsync(searchTerm, pageNumber, PageSize);
            return View(result);
        }

        // ✅ PROFILE
        public async Task<ActionResult> Profile(int id)
        {
            var patient = await _service.GetByIdAsync(id);

            if (patient == null)
                return HttpNotFound();

            return View(patient);
        }

        // ✅ REGISTER (GET) - Optional (not used for modal)
        public ActionResult Register()
        {
            return View();
        }

        // ✅ ✅ ✅ REGISTER (POST) — FIXED FOR MODAL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data";
                return RedirectToAction("List");
            }

            var result = await _service.CreateAsync(dto);

            if (result)
            {
                TempData["Success"] = "Patient created successfully.";
            }
            else
            {
                TempData["Error"] = "Error creating patient";
            }

            return RedirectToAction("List");
        }
        // ✅ EDIT (GET)
        public async Task<ActionResult> Edit(int id)
        {
            var patient = await _service.GetByIdAsync(id);

            if (patient == null)
                return HttpNotFound();

            return View(patient);
        }

        // ✅ EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public async Task<ActionResult> Edit(PatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data";
                return RedirectToAction("Edit", new { id = dto.PatientId }); 
            }

            var result = await _service.UpdateAsync(dto);

            if (result)
            {
                TempData["Success"] = "Patient updated successfully.";
                return RedirectToAction("List");  
            }
            else
            {
                TempData["Error"] = "Update failed.";
                return RedirectToAction("Edit", new { id = dto.PatientId });
            }
        }


        // ✅ DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            TempData[result ? "Success" : "Error"] =
                result ? "Patient deleted." : "Delete failed.";

            return RedirectToAction("List");
        }
    }
}