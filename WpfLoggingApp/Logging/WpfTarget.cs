using System;
using NLog;
using NLog.Targets;
using WpfLoggingApp.Services;

namespace WpfLoggingApp.Logging
{
    [Target("WpfTarget")]
    public sealed class WpfTarget : TargetWithLayout
    {
        protected override void Write(LogEventInfo logEvent)
        {
            var logMessage = Layout.Render(logEvent);
            
            try
            {
                var loggerService = ServiceLocator.Instance.Resolve<ILoggerService>() as LoggerService;
                if (loggerService != null)
                {
                    loggerService.RaiseLogMessageReceived(logMessage);
                }
            }
            catch (Exception)
            {
                // Ignore if service is not yet registered
            }
        }
    }
}
