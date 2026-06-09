using System.ComponentModel.DataAnnotations;


namespace SharedClasses.Dtos
{
    public class CancelByDoctorDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; }
    }
}