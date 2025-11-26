using System;
using NLog;

namespace WpfLoggingApp.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly Logger logger;
        
        public event EventHandler<string> LogMessageReceived;

        public LoggerService()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

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

        internal void RaiseLogMessageReceived(string message)
        {
            LogMessageReceived?.Invoke(this, message);
        }
    }
}
