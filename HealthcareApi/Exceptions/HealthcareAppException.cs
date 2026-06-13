using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareApi.Exceptions
{
    public abstract class HealthcareAppException : Exception
    {
        protected HealthcareAppException(string message)
            : base(message)
        {
        }

        protected HealthcareAppException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}