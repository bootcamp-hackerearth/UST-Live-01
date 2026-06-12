using Newtonsoft.Json;

namespace HealthAppMVC.Helper
{
    public static class ApiErrorHelper
    {
        public static string GetApiMessage(string error)
        {
            if (string.IsNullOrWhiteSpace(error))
            {
                return "Something went wrong. Please try again.";
            }

            try
            {
                dynamic obj =
                    JsonConvert.DeserializeObject(error);

                if (obj.Message != null)
                {
                    return obj.Message.ToString();
                }

                if (obj.message != null)
                {
                    return obj.message.ToString();
                }

                if (obj.ExceptionMessage != null)
                {
                    return obj.ExceptionMessage.ToString();
                }
            }
            catch
            {
                // If response is not JSON, return original error.
            }

            return error;
        }
    }
}