public class CancelAppointmentDto
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public string CancellationReason { get; set; }
}
