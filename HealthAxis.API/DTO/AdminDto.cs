namespace HealthAxis.API.DTO
{
    public class AdminDto
    {
            public DateTime Date { get; set; }

            public int ConfirmedCount { get; set; }

            public int CancelledCount { get; set; }

            public int CompletedCount { get; set; }
        }
}
