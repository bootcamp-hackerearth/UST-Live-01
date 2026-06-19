using HealthAxis.API.Enums;

namespace HealthAxis.API.DTO.DoctorDtos
{
    public class DoctorCreatedDto
    {
            public int DoctorId { get; set; }

            public string FullName { get; set; } = string.Empty;

            public string Email { get; set; } = string.Empty;

            public Specialisation Specialisation { get; set; }

            public int YearsOfExperience { get; set; }

            public decimal ConsultationFee { get; set; }

            public bool IsActive { get; set; }

            public string TemporaryPassword { get; set; } = string.Empty;
        }
    }
