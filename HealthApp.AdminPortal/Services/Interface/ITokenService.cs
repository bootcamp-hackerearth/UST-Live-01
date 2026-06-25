namespace HealthApp.AdminPortal.Services.Interface
{
    public interface ITokenService
    {
        Task SetToken(string token);
        Task<string?> GetToken();
        Task RemoveToken();
    }
}
