using NLog;
using System;

namespace WpfLoggingApp.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        public event EventHandler<string> LogMessageReceived;

        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogWarn(string message)
        {
            logger.Warn(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }

        public void NotifyLogMessage(string message)
        {
            LogMessageReceived?.Invoke(this, message);
        }
    }
}
