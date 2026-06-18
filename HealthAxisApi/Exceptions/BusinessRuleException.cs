using System.Net;

namespace HealthAxisCore_Api.Exceptions
{
    public class BusinessRuleException : HealthCareAppException
    {
        public BusinessRuleException(string message)
            : base(message, (int)HttpStatusCode.BadRequest)
        {
        }
    }
}