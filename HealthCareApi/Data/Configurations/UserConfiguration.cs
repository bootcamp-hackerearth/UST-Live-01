using System.Data.Entity.ModelConfiguration;
using HealthCareApi.Models;

namespace HealthCareApi.Data.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users");

            HasKey(u => u.UserId);

            Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            Property(u => u.PasswordHash)
                .HasMaxLength(256);

            Property(u => u.Role)
                .HasMaxLength(20);

            // Unique Email
            HasIndex(u => u.Email).IsUnique();

            // Patients → 1-to-many
            HasMany(u => u.Patients)
                .WithRequired(p => p.User)
                .HasForeignKey(p => p.UserId);

            // Doctor → 1-to-1
            HasOptional(u => u.Doctor)
                .WithRequired(d => d.User);
        }
    }
}