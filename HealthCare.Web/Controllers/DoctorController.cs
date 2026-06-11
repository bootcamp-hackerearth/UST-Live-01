using HealthCare.Shared.DTOs.Doctor;
using HealthCare.Web.Services.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;
using HealthCare.Web.Services;

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

        public async Task<ActionResult> List(
            string specialization,
            string searchTerm,
            bool orderByDescending = false,
            int pageNumber = 1)
        {
            var result = await _service.GetDoctorsAsync(
                specialization,
                searchTerm,
                orderByDescending,
                pageNumber,
                PageSize);

            return View("List", result);
        }

        
        public async Task<ActionResult> Profile(int id)
        {
            var doctor = await _service.GetByIdAsync(id);

            if (doctor == null)
                return HttpNotFound();

            return View("Profile", doctor);
        }

        public ActionResult Add()
        {
            return View("Add");
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _service.CreateAsync(dto);

            if (result)
            {
                TempData["Success"] = "Doctor created successfully.";
                return RedirectToAction("List");
            }

            ModelState.AddModelError("", "Error creating doctor");
            return View(dto);
        }

      
        public async Task<ActionResult> Edit(int id)
        {
            var doctor = await _service.GetByIdAsync(id);

            if (doctor == null)
                return HttpNotFound();

            return View("Edit", doctor);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateDoctorDto dto)
        {
            if (!ModelState.IsValid)
                return View("Edit", dto);

            var result = await _service.UpdateAsync(dto);

            if (result)
            {
                TempData["Success"] = "Doctor updated successfully.";
                return RedirectToAction("List", new { id = dto.DoctorId });
            }

            ModelState.AddModelError("", "Error updating doctor");
            return View("Edit", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (result)
                TempData["Success"] = "Doctor deleted.";
            else
                TempData["Error"] = "Delete failed.";

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<ActionResult> GetDoctors(string specialization)
        {
            var doctors = await _service
                .GetDoctorsBySpecializationAsync(specialization);

            return Json(doctors, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddPartial()
        {
            return PartialView("_AddPartial");
        }

        public async Task<ActionResult> EditPartial(int id)
        {
            var doctor = await _service.GetByIdAsync(id);
            return PartialView("_EditPartial", doctor);
        }

        public async Task<ActionResult> ViewPartial(int id)
        {
            var doctor = await _service.GetByIdAsync(id);
            return PartialView("_ViewPartial", doctor);
        }
    }
}