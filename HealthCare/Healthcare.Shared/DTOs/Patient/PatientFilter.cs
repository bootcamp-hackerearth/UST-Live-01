namespace Healthcare.Shared.DTOs.Patient
{
    public class PatientFilter : PaginationParam
    {
        public bool? HasInsurance { get; set; }

        public string? FullName { get; set; }

        public bool ? IsActive { get; set; }

    }
}
