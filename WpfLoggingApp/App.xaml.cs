using System.Windows;
using WpfLoggingApp.Services;

namespace WpfLoggingApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize the ServiceLocator with dependencies
            ServiceLocator.Instance.RegisterSingleton<ILoggerService, LoggerService>();

            // MainWindow is created automatically via StartupUri in App.xaml
        }
    }
}
