using HealthApp.ConsoleApp.Models;
namespace HealthApp.ConsoleApp.Databases
{
    public class DoctorDb
    {
        public List<Doctor> Doctors { get; set; }

        public DoctorDb()
        {
            Doctors = new List<Doctor>
            {
                // Pre-populated doctors with available slots and dates
                //DoctorId starts from 201 to avoid conflict with test data
                new Doctor
                {
                    DoctorId = 201,
                    Name = "John Smith",
                    Specialisation = "Cardiology",
                    YearsOfExperience = 15,
                    ConsultationFee = 500,
                    IsActive = true,

                    AvailableSlots = new()
                    {
                        "09:00 AM",
                        "10:00 AM",
                        "11:00 AM"
                    },

                    AvailableDates = new()
                    {
                        DateTime.Today,
                        DateTime.Today.AddDays(1),
                        DateTime.Today.AddDays(2)
                    }
                },

                new Doctor
                {
                    DoctorId = 202,
                    Name = "Emily Davis",
                    Specialisation = "Dermatology",
                    YearsOfExperience = 10,
                    ConsultationFee = 400,
                    IsActive = true,

                    AvailableSlots = new()
                    {
                        "12:00 PM",
                        "02:00 PM",
                        "04:00 PM"
                    },

                    AvailableDates = new()
                    {
                        DateTime.Today.AddDays(1),
                        DateTime.Today.AddDays(2),
                        DateTime.Today.AddDays(3)
                    }
                },
                //Doctor with no available slots
                new Doctor
                {
                    DoctorId = 203,
                    Name = "Michael Brown",
                    Specialisation = "Orthopedics",
                    YearsOfExperience = 20,
                    ConsultationFee = 600,
                    IsActive = false,

                    AvailableSlots = new()
                    {
                    },

                    AvailableDates = new()
                    {

                    }
                },

                new Doctor
                {
                    DoctorId = 204,
                    Name = "Sarah Johnson",
                    Specialisation = "Pediatrics",
                    YearsOfExperience = 8,
                    ConsultationFee = 350,
                    IsActive = true,

                    AvailableSlots = new()
                    {
                        "10:00 AM",
                        "01:00 PM",
                        "03:00 PM"
                    },

                    AvailableDates = new()
                    {
                        DateTime.Today,
                        DateTime.Today.AddDays(2)
                    }
                },

                new Doctor
                {
                    DoctorId = 205,
                    Name = "David Wilson",
                    Specialisation = "Neurology",
                    YearsOfExperience = 12,
                    ConsultationFee = 700,
                    IsActive = true,

                    AvailableSlots = new()
                    {
                        "11:00 AM",
                        "02:00 PM"
                    },

                    AvailableDates = new()
                    {
                        DateTime.Today.AddDays(4),
                        DateTime.Today.AddDays(5)
                    }
                },

                new Doctor
                {
                    DoctorId = 206,
                    Name = "Loki",
                    Specialisation = "Skin",
                    YearsOfExperience = 3,
                    ConsultationFee = 300,
                    IsActive = true,

                    AvailableSlots = new()
                    {
                        "09:00 AM",
                        "12:00 PM",
                        "04:00 PM"
                    },

                    AvailableDates = new()
                    {
                        DateTime.Today,
                        DateTime.Today.AddDays(1)
                    }
                }
            };
        }
    }
}