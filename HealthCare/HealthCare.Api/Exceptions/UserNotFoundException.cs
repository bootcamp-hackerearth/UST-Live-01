namespace HealthCare.Api.Exceptions
{

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
            : base("User not found")
        {
        }
    }

    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException()
            : base("Invalid email or password")
        {
        }

    }
}
