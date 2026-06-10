using HealthCare_Appointment_Portal.Enums;
using System;

namespace HealthCare_Appointment_Portal.DTOs.PatientDtos
{
    public class PatientDto
    {
        public int PatientId
        {
            get;
            set;
        }

        public string FullName
        {
            get;
            set;
        }

        public DateTime DateOfBirth
        {
            get;
            set;
        }

        public int Age
        {
            get;
            set;
        }

        public Gender Gender
        {
            get;
            set;
        }

        public string PhoneNumber
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public int AppointmentCount
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