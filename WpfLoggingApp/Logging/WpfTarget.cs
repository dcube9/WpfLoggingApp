using NLog;
using NLog.Common;
using NLog.Targets;
using System;
using WpfLoggingApp.Services;

namespace WpfLoggingApp.Logging
{
    [Target("WpfTarget")]
    public sealed class WpfTarget : TargetWithLayout
    {
        // Cached logger service to avoid repeated resolves
        private volatile ILoggerService loggerService;
        private readonly object serviceLock = new object();

        protected override void Write(LogEventInfo logEvent)
        {
            var logMessage = Layout.Render(logEvent);

            // Try to get cached service reference first
            var svc = loggerService;

            if (svc == null)
            {
                try
                {
                    lock (serviceLock)
                    {
                        if (loggerService == null)
                        {
                            loggerService = ServiceLocator.Instance.Resolve<ILoggerService>();
                        }
                        svc = loggerService;
                    }
                }
                catch (InvalidOperationException)
                {
                    // Service not registered yet; ignore and return early
                    return;
                }
                catch (Exception ex)
                {
                    // Unexpected error - write to NLog internal logger to aid diagnostics
                    InternalLogger.Warn(ex, "WpfTarget: failed to resolve ILoggerService");
                    return;
                }
            }

            try
            {
                svc?.NotifyLogMessage(logMessage);
            }
            catch (Exception ex)
            {
                // Protect target from throwing; log internally for diagnostics
                InternalLogger.Warn(ex, "WpfTarget: failed while notifying log message");
            }
        }
    }
}
