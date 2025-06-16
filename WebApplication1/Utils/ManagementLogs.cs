namespace WebApplication1.Utils
{
    public class ManagementLogs
    {
        private readonly ILogger _logger;
        public ManagementLogs(ILogger logger)
        {
            _logger = logger;
        }
        public void WriteLog(string exception, string message, IDictionary<string, object> data)
        {
            string dataFormat = String.Empty;
            foreach (KeyValuePair<string, object> d in data)
            {
                dataFormat += $"Key: {d.Key} - Value: {d.Value}; ";
            }
            _logger.LogError("-------------------------[ERROR BEGIN]-------------------------");
            _logger.LogError("[EXCEPTION] ===> {0}", exception);
            _logger.LogError("[MESSAGE]   ===> {0}", message);
            _logger.LogError("[DATA]      ===> {0}", dataFormat);
            _logger.LogError("[DATE]      ===> {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            _logger.LogError("-------------------------[ERROR  ENDS]-------------------------");
        }
    }
}
