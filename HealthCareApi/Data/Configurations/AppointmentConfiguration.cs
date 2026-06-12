using System.Data.Entity.ModelConfiguration;
using HealthCareApi.Models;

namespace HealthCareApi.Data.Configurations
{
    public class AppointmentConfiguration : EntityTypeConfiguration<Appointment>
    {
        public AppointmentConfiguration()
        {
            ToTable("Appointments");

            HasKey(a => a.AppointmentId);

            Property(a => a.ScheduledDate)
                .HasColumnType("date")
                .IsRequired();

            Property(a => a.TimeSlot)
                .HasMaxLength(20);

            Property(a => a.Status)
                .HasMaxLength(20);

            Property(a => a.CancellationReason)
                .HasMaxLength(500)
                .IsOptional();

            Property(a => a.CreatedDate)
                .HasColumnType("datetime2")
                .IsRequired();

            // Relationship: Appointment → Patient
            HasRequired(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .WillCascadeOnDelete(false);

            // Relationship: Appointment → Doctor
            HasRequired(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .WillCascadeOnDelete(false);

            // 1-to-1 relationship with HealthRecord
            HasOptional(a => a.HealthRecord)
                .WithRequired(hr => hr.Appointment);
        }
    }
}