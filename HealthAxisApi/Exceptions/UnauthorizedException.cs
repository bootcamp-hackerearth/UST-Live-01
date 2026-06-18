using System.Net;

namespace HealthAxisCore_Api.Exceptions
{
    public class UnauthorizedException : HealthCareAppException
    {
        public UnauthorizedException(string message)
            : base(message, (int)HttpStatusCode.Unauthorized)
        {
        }
    }
}