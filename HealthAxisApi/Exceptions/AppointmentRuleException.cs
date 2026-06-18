using System.Net;

namespace HealthAxisCore_Api.Exceptions
{
    public class AppointmentRuleException : BusinessRuleException
    {
        public AppointmentRuleException(string message)
            : base(message)
        {
        }
    }
}
