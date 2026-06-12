using System.Data.Entity.ModelConfiguration;
using HealthCareApi.Models;

namespace HealthCareApi.Data.Configurations
{
    public class PatientConfiguration : EntityTypeConfiguration<Patient>
    {
        public PatientConfiguration()
        {
            ToTable("Patients");

            HasKey(p => p.PatientId);

            Property(p => p.FullName)
                .IsRequired()
                .HasMaxLength(100);

            Property(p => p.DateOfBirth)
                .HasColumnType("date");

            Property(p => p.Gender)
                .HasMaxLength(10);

            Property(p => p.PhoneNumber)
                .HasMaxLength(20);

            Property(p => p.Email)
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.InsuranceId)
                .HasMaxLength(50);

            Property(p => p.CreatedDate)
                .HasColumnType("datetime2")
                .IsRequired();

            HasIndex(p => p.Email).IsUnique();

            // Relationships
            HasMany(p => p.Appointments)
                .WithRequired(a => a.Patient)
                .HasForeignKey(a => a.PatientId);

            HasMany(p => p.HealthRecords)
                .WithRequired(h => h.Patient)
                .HasForeignKey(h => h.PatientId);
        }
    }
}