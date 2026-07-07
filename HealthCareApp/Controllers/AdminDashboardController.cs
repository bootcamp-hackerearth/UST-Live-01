using HealthCareApp.Data;
using HealthCareApp.Shared.Dtos.Dashboard;
using HealthCareApp.Shared.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Admin")]
    public class AdminDashboardController(HealthAxisDbContext dbContext) : ControllerBase
    {
        [HttpGet("dashboard")]
        public async Task<ActionResult<AdminDashboardReportDto>> GetDashboardReport()
        {
            DateTime today = DateTime.UtcNow.Date;
            DateTime tomorrow = today.AddDays(1);

            int totalDoctors = await dbContext.Doctors.CountAsync();

            int totalPatients = await dbContext.Patients.CountAsync();

            int totalAppointments = await dbContext.Appointments.CountAsync();

            int todaysAppointments = await dbContext.Appointments
                .CountAsync(appointment =>
                    appointment.ScheduledDate >= today &&
                    appointment.ScheduledDate < tomorrow);

            int todaysPatients = await dbContext.Patients
                .CountAsync(patient =>
                    patient.CreatedDate >= today &&
                    patient.CreatedDate < tomorrow);

            int completedToday = await dbContext.Appointments
                .CountAsync(appointment =>
                    appointment.ScheduledDate >= today &&
                    appointment.ScheduledDate < tomorrow &&
                    appointment.Status == AppointmentStatus.Completed);

            int pendingToday = await dbContext.Appointments
                .CountAsync(appointment =>
                    appointment.ScheduledDate >= today &&
                    appointment.ScheduledDate < tomorrow &&
                    appointment.Status == AppointmentStatus.Pending);

            int cancelledToday = await dbContext.Appointments
                .CountAsync(appointment =>
                    appointment.ScheduledDate >= today &&
                    appointment.ScheduledDate < tomorrow &&
                    appointment.Status == AppointmentStatus.Cancelled);

            var dashboardReport = new AdminDashboardReportDto
            {
                TotalDoctors = totalDoctors,
                TotalPatients = totalPatients,
                TotalAppointments = totalAppointments,
                TodaysAppointments = todaysAppointments,
                TodaysPatients = todaysPatients,
                CompletedToday = completedToday,
                PendingToday = pendingToday,
                CancelledToday = cancelledToday
            };

            return Ok(dashboardReport);
        }
    }
}