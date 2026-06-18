namespace HealthCare.Api.Exceptions
{
    public class HealthRecordNotFoundException : Exception
    {
        public HealthRecordNotFoundException(int id)
                    : base($"Health record with ID {id} not found") { }


    }
}
