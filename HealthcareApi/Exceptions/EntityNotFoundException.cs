using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareApi.Exceptions
{
    public class EntityNotFoundException : HealthcareAppException
    {
        public string EntityName { get; private set; }

        public int EntityId { get; private set; }

        public EntityNotFoundException(string entityName, int entityId)
            : base(string.Format("{0} with ID {1} was not found.", entityName, entityId))
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}
