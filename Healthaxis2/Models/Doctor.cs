using Healthaxis2.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Doctor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DoctorId { get; set; }

    // ✅ REQUIRED
    [Required(ErrorMessage = "Doctor Name is required")]
    [StringLength(30)]
    [RegularExpression(@"^[A-Za-z\s\.]+$", ErrorMessage = "Invalid name")]
    public string DoctorName { get; set; }

    // ✅ REQUIRED ENUM-LIKE
    [Required(ErrorMessage = "Specialisation is required")]
    public string Specialisation { get; set; }

    // ✅ REQUIRED + RANGE
    [Required]
    [Range(1, 50, ErrorMessage = "Experience must be between 1–50 years")]
    public int Experience { get; set; }

    // ✅ REQUIRED + RANGE
    [Required]
    [Range(100, 5000, ErrorMessage = "Fees must be realistic")]
    public int Fees { get; set; }

    // ✅ REQUIRED
    [Required]
    public bool IsActive { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; }
    public virtual ICollection<HealthRecord> HealthRecords { get; set; }
}