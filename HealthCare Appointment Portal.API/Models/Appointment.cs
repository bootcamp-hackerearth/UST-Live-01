using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare_Appointment_Portal.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId
        {
            get;
            set;
        }

        [Required(ErrorMessage = Constants.PatientRequired)]
        public int PatientId
        {
            get;
            set;
        }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient
        {
            get;
            set;
        }

        [Required(ErrorMessage = Constants.DoctorRequired)]
        public int DoctorId
        {
            get;
            set;
        }

        [ForeignKey(nameof(DoctorId))]
        public virtual Doctor Doctor
        {
            get;
            set;
        }

        [Required(ErrorMessage = Constants.ScheduledDateRequired)]
        [DataType(DataType.Date)]
        public DateTime ScheduledDate
        {
            get;
            set;
        }

        [Required(ErrorMessage = Constants.TimeSlotRequired)]
        [StringLength(ValidationLimits.TimeSlotLength)]
        public string TimeSlot
        {
            get;
            set;
        } = string.Empty;

        [Required(ErrorMessage = Constants.AppointmentStatusRequired)]
        public AppointmentStatus Status
        {
            get;
            set;
        }

        [StringLength(
            ValidationLimits.CancellationReasonLength)]
        public string CancellationReason
        {
            get;
            set;
        } = string.Empty;

        public virtual ICollection<HealthRecord>
            HealthRecords
        {
            get;
            set;
        }
            = new List<HealthRecord>();

        public void Confirm()
        {
            Status =
                AppointmentStatus.Confirmed;
        }

        public void Cancel(
            string reason)
        {
            Status =
                AppointmentStatus.Cancelled;

            CancellationReason =
                reason;
        }

        public void Complete()
        {
            Status =
                AppointmentStatus.Completed;
        }

        public bool IsUpcoming()
        {
            return ScheduledDate.Date >=
                   DateTime.Today
                   &&
                   Status !=
                   AppointmentStatus.Cancelled;
        }

        public bool IsCancelled()
        {
            return Status ==
                   AppointmentStatus.Cancelled;
        }

        public bool IsCompleted()
        {
            return Status ==
                   AppointmentStatus.Completed;
        }
    }
}