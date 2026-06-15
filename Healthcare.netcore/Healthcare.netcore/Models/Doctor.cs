using HealthAxis.API.Enums;
using HealthAxis.API.Models;

namespace HealthAxis.API.Models;

public class Doctor
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public Specialization Specialisation { get; set; }

    public DateOnly PracticeStartDate { get; set; }

    public decimal ConsultationFee { get; set; }

    public bool IsAvailable { get; set; } = true;

    public User? User { get; set; }
}