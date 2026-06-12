using System.Data.Entity.ModelConfiguration;
using HealthCareApi.Models;

namespace HealthCareApi.Data.Configurations
{
    public class HealthRecordConfiguration : EntityTypeConfiguration<HealthRecord>
    {
        public HealthRecordConfiguration()
        {
            ToTable("HealthRecords");

            HasKey(h => h.RecordId);

            Property(h => h.VisitDate)
                .HasColumnType("datetime2")
                .IsRequired();

            Property(h => h.CreatedDate)
                .HasColumnType("datetime2")
                .IsRequired();

            Property(h => h.Diagnosis)
                .HasMaxLength(500);

            Property(h => h.Prescription)
                .HasMaxLength(500);

            Property(h => h.Notes)
                .HasMaxLength(1000)
                .IsOptional();

            // Relationship: HealthRecord → Patient
            HasRequired(h => h.Patient)
                .WithMany(p => p.HealthRecords)
                .HasForeignKey(h => h.PatientId)
                .WillCascadeOnDelete(false);

            // Relationship: HealthRecord → Doctor
            HasRequired(h => h.Doctor)
                .WithMany(d => d.HealthRecords)
                .HasForeignKey(h => h.DoctorId)
                .WillCascadeOnDelete(false);

            // NEW: Relationship → Appointment
            //HasRequired(h => h.Appointment)
            //    .WithMany(a => a.HealthRecords)
            //    .HasForeignKey(h => h.AppointmentId)
            //    .WillCascadeOnDelete(false);
        }
    }
}