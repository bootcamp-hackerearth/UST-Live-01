namespace HealthCare.Api.Exceptions
{

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
            : base("User not found")
        {
        }
    }

}
