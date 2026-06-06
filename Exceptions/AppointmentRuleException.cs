using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareMvcApp.Exceptions
{
    public class AppointmentRuleException : BusinessRuleException
    {
        public AppointmentRuleException(string message)
            : base(message)
        {
        }
    }
}