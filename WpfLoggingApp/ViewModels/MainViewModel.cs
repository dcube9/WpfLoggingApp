using System;
using WpfLoggingApp.Services;

namespace WpfLoggingApp.ViewModels
{
    public class MainViewModel
    {
        private readonly ILoggerService loggerService;
        
        public event EventHandler<string> LogMessageReceived;

        public MainViewModel(ILoggerService loggerService)
        {
            this.loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
            
            // Subscribe to logger service events
            this.loggerService.LogMessageReceived += OnLoggerServiceMessageReceived;
        }

        private void OnLoggerServiceMessageReceived(object sender, string message)
        {
            // Forward the event to UI
            LogMessageReceived?.Invoke(this, message);
        }

        public void OnStartClicked()
        {
            loggerService.LogInfo("Start button pressed");
        }

        public void OnStopClicked()
        {
            loggerService.LogInfo("Stop button pressed");
        }
    }
}
