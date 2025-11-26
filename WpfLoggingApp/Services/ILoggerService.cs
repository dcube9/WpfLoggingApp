using System;

namespace WpfLoggingApp.Services
{
    public interface ILoggerService
    {
        event EventHandler<string> LogMessageReceived;
        
        void LogInfo(string message);
        void LogDebug(string message);
        void LogWarn(string message);
        void LogError(string message);
    }
}
