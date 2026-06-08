using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareApi.Exceptions
{
    public class BusinessRuleException : HealthcareAppException
    {
        public BusinessRuleException(string message)
            : base(message)
        {
        }
    }
}