using System;
using System.Windows;
using WpfLoggingApp.Services;
using WpfLoggingApp.ViewModels;

namespace WpfLoggingApp
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            
            // Get logger service from DI container
            var loggerService = ServiceLocator.Instance.Resolve<ILoggerService>();
            
            // Initialize ViewModel
            viewModel = new MainViewModel(loggerService);
            DataContext = viewModel;
            
            // Subscribe to log messages for UI updates
            viewModel.LogMessageReceived += OnLogMessageReceived;
        }

        private void OnLogMessageReceived(object sender, string logMessage)
        {
            // Ensure we're on the UI thread
            Dispatcher.Invoke(() =>
            {
                // Add new log message
                LogTextBox.AppendText(logMessage + Environment.NewLine);
                
                // Maintain max 10000 lines
                var lines = LogTextBox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                if (lines.Length > 10000)
                {
                    var linesToRemove = lines.Length - 10000;
                    var newText = string.Join(Environment.NewLine, lines, linesToRemove, 10000);
                    LogTextBox.Text = newText;
                }
                
                // Auto-scroll to bottom
                LogScrollViewer.ScrollToEnd();
            });
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.OnStartClicked();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.OnStopClicked();
        }

        protected override void OnClosed(EventArgs e)
        {
            viewModel.LogMessageReceived -= OnLogMessageReceived;
            base.OnClosed(e);
        }
    }
}
