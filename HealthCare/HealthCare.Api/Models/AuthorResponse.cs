using Microsoft.Identity.Client;

namespace HealthCare.Api.Models
{
    public class AuthorResponse
    {
        public string? AccessToken { get; set; }
        public string? Message { get; set; }

        public int ExpiresIn { get; set; }


    }
}
