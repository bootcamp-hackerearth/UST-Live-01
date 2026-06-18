namespace HealthCare.Api.DTOs.Patient
{
    public class PatientFilter : PaginationParam
    {
        public bool? HasInsurance { get; set; }

        public string? FullName { get; set; }

    }
}
