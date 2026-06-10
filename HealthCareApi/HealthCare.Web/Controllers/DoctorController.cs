using HealthCare.Shared.DTOs.Doctor;
using HealthCare.Web.Services;
using HealthCare.Web.Services.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare.Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _service;
        private const int PageSize = 10;

        public DoctorController()
        {
            _service = new DoctorService();
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

    
        public async Task<ActionResult> List(
            string specialization,
            string searchTerm,
            bool orderByDescending = false,
            int pageNumber = 1)
        {
            var result = await _service.GetDoctorsAsync(
                specialization, searchTerm, orderByDescending, pageNumber, PageSize);

            return View(result);
        }

 
        public ActionResult RegisterPartial()
        {
            return PartialView("_RegisterPartial", new CreateDoctorDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
                return PartialView("_RegisterPartial", dto);

            var result = await _service.CreateAsync(dto);

            if (result)
                return Json(new { success = true, message = "Doctor added successfully." });

            ModelState.AddModelError("", "Error adding doctor. Please try again.");
            return PartialView("_RegisterPartial", dto);
        }

     
        public async Task<ActionResult> ViewPartial(int id)
        {
            var doctor = await _service.GetByIdAsync(id);

            if (doctor == null)
                return HttpNotFound();

            return PartialView("_ViewPartial", doctor);
        }

       
        public async Task<ActionResult> EditPartial(int id)
        {
            var doctor = await _service.GetByIdAsync(id);

            if (doctor == null)
                return HttpNotFound();

            return PartialView("_EditPartial", doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DoctorDto dto)
        {
            if (!ModelState.IsValid)
                return PartialView("_EditPartial", dto);

            var result = await _service.UpdateAsync(dto);

            if (result)
                return Json(new { success = true, message = "Doctor updated successfully." });

            ModelState.AddModelError("", "Update failed. Please try again.");
            return PartialView("_EditPartial", dto);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            TempData[result ? "Success" : "Error"] =
                result ? "Doctor deleted successfully." : "Delete failed.";

            return RedirectToAction("List");
        }
    }
}