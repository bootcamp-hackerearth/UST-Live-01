namespace Healthcare.Shared.DTOs.Doctor
{
    public class DoctorFilter : PaginationParam
    {
        public string? FullName { get; set; }
        public string ?Specialisation { get; set; }
        public int? MinExperience { get; set; }

        public bool? IsActive {  get; set; }
    }
}
