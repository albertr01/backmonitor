namespace WebApplication1.Utils
{
    public static class ManagementExceptions
    {
        public static string GetMessageSystemError(Exception exception)
        {
            string message;
            string stackTrace;

            if (exception.InnerException != null)
            {
                message = exception.InnerException.Message;
                if (String.IsNullOrEmpty(exception.InnerException.StackTrace))
                {
                    stackTrace = exception.StackTrace;
                }
                else
                {
                    stackTrace = exception.InnerException.StackTrace;
                }
            }
            else
            {
                message = exception.Message;
                if (!String.IsNullOrEmpty(exception.StackTrace))
                {
                    stackTrace = exception.StackTrace;
                }
                else
                {
                    stackTrace = "Desconocido";
                }
            }

            return $"Message: {message}; StackTrace: {stackTrace};";
        }
    }
}