

namespace Healthcare.Shared.DTOs.Authentication
{
    public class AuthorResponseDto
    {
        public string? AccessToken { get; set; }
        public string? Role{ get; set; }

        public int ExpiresIn { get; set; }


    }
}
