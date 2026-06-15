namespace HealthAxis.API.Dtos.PatientDtos;

public class PatientDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; }

    public string Gender { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
}