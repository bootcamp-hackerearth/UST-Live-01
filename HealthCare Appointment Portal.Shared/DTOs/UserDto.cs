using HealthCare_Appointment_Portal.Enums;

namespace HealthCare_Appointment_Portal.DTOs.UserDtos
{
    public class UserDto
    {
        public int UserId
        {
            get;
            set;
        }

        public string UserCode
        {
            get;
            set;
        }

        public Role Role
        {
            get;
            set;
        }

        public int ReferenceId
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }
    }
}