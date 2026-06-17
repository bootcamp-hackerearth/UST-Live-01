using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.API.Configurations
{
    [ExcludeFromCodeCoverage]
    public class JwtSettings
    {

        #region Properties

        public string SecretKey { get; set; } = string.Empty;

        public string Issuer { get; set; } = string.Empty;

        public string Audience {  get; set; } = string.Empty;
        
        public int ExpiryMinutes { get; set; }
       
        #endregion
    }
}
