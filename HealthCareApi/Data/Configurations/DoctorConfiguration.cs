using HealthCareApi.Models;
using System.Data.Entity.ModelConfiguration;

namespace HealthCareApi.Data.Configurations
{
    public class DoctorConfiguration : EntityTypeConfiguration<Doctor>
    {
        public DoctorConfiguration()
        {
            ToTable("Doctors");
            HasKey(d => d.DoctorId);

            Property(d => d.UserId)
                .IsRequired();

            HasIndex(d => d.UserId).IsUnique();

            Property(d => d.FullName).IsRequired().HasMaxLength(100);
            Property(d => d.Specialisation).IsRequired().HasMaxLength(50);
            Property(d => d.ConsultationFee).HasPrecision(10, 2);
            Property(d => d.IsActive).IsRequired();
            Property(d => d.CreatedDate).IsRequired();
        }
    }
}