using System.Net;

namespace HealthAxisCore_Api.Exceptions
{
    public class EntityNotFoundException : HealthCareAppException
    {
        public EntityNotFoundException(string message)
            : base(message, (int)HttpStatusCode.NotFound)
        {
        }
    }
}