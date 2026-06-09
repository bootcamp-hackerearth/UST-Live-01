using System.ComponentModel.DataAnnotations;


namespace SharedClasses.Dtos
{
    public class CancelAppointmentDto
    {
        [Required]
        [StringLength(500)]
        public string Reason { get; set; }
    }
}