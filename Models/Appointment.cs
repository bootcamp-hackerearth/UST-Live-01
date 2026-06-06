using HealthcareMvcApp.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareMvcApp.Models
{
    public class Appointment
    {
        public Appointment()
        {
            Status = AppointmentStatus.Pending;
            CancellationReason = string.Empty;
        }

        public Appointment(int patientId, int doctorId, DateTime scheduledDate, int slotNumber)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            ScheduledDate = scheduledDate.Date;
            SlotNumber = slotNumber;
            Status = AppointmentStatus.Pending;
            CancellationReason = string.Empty;
        }

        public Appointment(
            int appointmentId,
            int patientId,
            int doctorId,
            DateTime scheduledDate,
            int slotNumber,
            AppointmentStatus status,
            string cancellationReason)
        {
            AppointmentId = appointmentId;
            PatientId = patientId;
            DoctorId = doctorId;
            ScheduledDate = scheduledDate.Date;
            SlotNumber = slotNumber;
            Status = status;
            CancellationReason = cancellationReason ?? string.Empty;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Appointment ID")]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Patient ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Valid Patient ID is required.")]
        [Display(Name = "Patient ID")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Valid Doctor ID is required.")]
        [Display(Name = "Doctor ID")]
        public int DoctorId { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual Doctor Doctor { get; set; }

        [Required(ErrorMessage = "Appointment date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Appointment Date")]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Slot number is required.")]
        [Range(1, 10, ErrorMessage = "Slot number must be between 1 and 10.")]
        [Display(Name = "Slot Number")]
        public int SlotNumber { get; set; }

        [Display(Name = "Status")]
        public AppointmentStatus Status { get; set; }

        [Display(Name = "Cancellation Reason")]
        public string CancellationReason { get; set; }

        public bool CanConfirm()
        {
            return Status == AppointmentStatus.Pending;
        }

        public bool CanCancel()
        {
            return Status != AppointmentStatus.Completed &&
                   Status != AppointmentStatus.Cancelled;
        }

        public bool CanComplete()
        {
            return Status == AppointmentStatus.Confirmed;
        }

        public void Confirm()
        {
            Status = AppointmentStatus.Confirmed;
        }

        public void Cancel(string reason)
        {
            Status = AppointmentStatus.Cancelled;
            CancellationReason = reason ?? string.Empty;
        }

        public void Complete()
        {
            Status = AppointmentStatus.Completed;
        }

        public Appointment GetDetails()
        {
            return this;
        }
    }
}