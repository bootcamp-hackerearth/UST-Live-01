using HealthApp.AdminBlazor.Models;

namespace HealthApp.AdminBlazor.Services;

public class InMemoryAdminDataService
{
    private int nextDoctorId = 6;
    private int nextUserId = 8;

    public List<string> Specialisations { get; } = new()
    {
        "Endocrinologist",
        "Oncologist",
        "Gynecologist",
        "OrthopedicSurgeon",
        "Psychiatrist",
        "Pediatrician",
        "Neurologist",
        "Dermatologist",
        "Cardiologist",
        "GeneralPractitioner"
    };

    public List<DoctorDto> Doctors { get; } = new()
    {
        new() { DoctorId = 1, FullName = "Vignesh Kumar", Email = "vignesh.doctor@healthapp.com", Specialisation = "Cardiologist", YearsOfExperience = 14, ConsultationFee = 1500, IsActive = true, MustChangePassword = false },
        new() { DoctorId = 2, FullName = "Sneha Paul", Email = "sneha.doctor@healthapp.com", Specialisation = "Dermatologist", YearsOfExperience = 20, ConsultationFee = 1800, IsActive = true, MustChangePassword = false },
        new() { DoctorId = 3, FullName = "Hari Narayanan", Email = "hari.doctor@healthapp.com", Specialisation = "Neurologist", YearsOfExperience = 9, ConsultationFee = 900, IsActive = true, MustChangePassword = false },
        new() { DoctorId = 4, FullName = "Martin Smith", Email = "martin.doctor@healthapp.com", Specialisation = "Psychiatrist", YearsOfExperience = 11, ConsultationFee = 2000, IsActive = false, MustChangePassword = false },
        new() { DoctorId = 5, FullName = "Bharath Raj", Email = "bharath.doctor@healthapp.com", Specialisation = "GeneralPractitioner", YearsOfExperience = 25, ConsultationFee = 1000, IsActive = true, MustChangePassword = false }
    };

    public List<UserDto> Users { get; } = new()
    {
        new() { UserId = 1, FullName = "System Admin", Email = "admin@healthapp.com", Role = "Admin", MustChangePassword = false },
        new() { UserId = 2, FullName = "John Mathew", Email = "johnmathew@gmail.com", Role = "Patient", MustChangePassword = false },
        new() { UserId = 3, FullName = "Vignesh Kumar", Email = "vignesh.doctor@healthapp.com", Role = "Doctor", MustChangePassword = false },
        new() { UserId = 4, FullName = "Sneha Paul", Email = "sneha.doctor@healthapp.com", Role = "Doctor", MustChangePassword = false }
    };

    public List<AppointmentDto> Appointments { get; } = new()
    {
        new() { AppointmentId = 1, DoctorName = "Vignesh Kumar", PatientName = "John Mathew", ScheduledDate = DateTime.Today, TimeSlot = "09:00 AM - 09:30 AM", Status = "Confirmed" },
        new() { AppointmentId = 2, DoctorName = "Sneha Paul", PatientName = "Anu Joseph", ScheduledDate = DateTime.Today, TimeSlot = "10:00 AM - 10:30 AM", Status = "Completed" },
        new() { AppointmentId = 3, DoctorName = "Hari Narayanan", PatientName = "Kevin Thomas", ScheduledDate = DateTime.Today.AddDays(1), TimeSlot = "02:00 PM - 02:30 PM", Status = "Pending" },
        new() { AppointmentId = 4, DoctorName = "Bharath Raj", PatientName = "Neha George", ScheduledDate = DateTime.Today.AddDays(-1), TimeSlot = "04:30 PM - 05:00 PM", Status = "Cancelled" },
        new() { AppointmentId = 5, DoctorName = "Vignesh Kumar", PatientName = "Ajay Kumar", ScheduledDate = DateTime.Today.AddDays(-2), TimeSlot = "11:00 AM - 11:30 AM", Status = "Completed" }
    };

    public DoctorDto AddDoctor(CreateDoctorDto dto)
    {
        var doctor = new DoctorDto
        {
            DoctorId = nextDoctorId++,
            FullName = dto.FullName.Trim(),
            Email = dto.Email.Trim(),
            Specialisation = dto.Specialisation,
            YearsOfExperience = CalculateExperience(dto.PracticeStartDate),
            ConsultationFee = dto.ConsultationFee,
            IsActive = true,
            MustChangePassword = true
        };

        Doctors.Add(doctor);

        Users.Add(new UserDto
        {
            UserId = nextUserId++,
            FullName = doctor.FullName,
            Email = doctor.Email,
            Role = "Doctor",
            MustChangePassword = true
        });

        return doctor;
    }

    public void UpdateDoctor(int doctorId, UpdateDoctorDto dto)
    {
        var doctor = Doctors.FirstOrDefault(d => d.DoctorId == doctorId);
        if (doctor is null)
        {
            return;
        }

        doctor.FullName = dto.FullName.Trim();
        doctor.Specialisation = dto.Specialisation;
        doctor.YearsOfExperience = CalculateExperience(dto.PracticeStartDate);
        doctor.ConsultationFee = dto.ConsultationFee;
        doctor.IsActive = dto.IsActive;

        var user = Users.FirstOrDefault(u => u.Email == doctor.Email);
        if (user is not null)
        {
            user.FullName = doctor.FullName;
        }
    }

    public void ToggleDoctorActive(int doctorId)
    {
        var doctor = Doctors.FirstOrDefault(d => d.DoctorId == doctorId);
        if (doctor is not null)
        {
            doctor.IsActive = !doctor.IsActive;
        }
    }

    public List<AppointmentReportDto> GetAppointmentReports()
    {
        return Appointments
            .GroupBy(a => a.ScheduledDate.Date)
            .OrderByDescending(g => g.Key)
            .Select(g => new AppointmentReportDto
            {
                Date = g.Key,
                Pending = g.Count(a => a.Status == "Pending"),
                Confirmed = g.Count(a => a.Status == "Confirmed"),
                Cancelled = g.Count(a => a.Status == "Cancelled"),
                Completed = g.Count(a => a.Status == "Completed")
            })
            .ToList();
    }

    private static int CalculateExperience(DateTime startDate)
    {
        var years = DateTime.Today.Year - startDate.Year;

        if (startDate.Date > DateTime.Today.AddYears(-years))
        {
            years--;
        }

        return Math.Max(0, years);
    }
}
