using System.Web.Mvc;

public class DashboardController : Controller
{
    public ActionResult PatientDashboard()
    {
        return View();
    }

    public ActionResult DoctorDashboard()
    {
        return View();
    }

    public ActionResult AdminDashboard()
    {
        return View();
    }
}