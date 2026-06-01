namespace HealthApp.ConsoleApp.Exceptions
{
    public class UserExitException : Exception
    {
        public UserExitException(string message) : base(message)
        {
            Console.WriteLine("User exit: " + message);
        }
    }
}