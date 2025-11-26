using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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

            UpdateMaximizeIcon();

            this.StateChanged += MainWindow_StateChanged;
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

        // Title bar drag and double-click handling
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // If the click originated on a titlebar control (button, path, etc.), don't start DragMove
            if (e.OriginalSource is DependencyObject dep && IsClickOnInteractiveElement(dep))
                return;

            if (e.ClickCount == 2)
            {
                ToggleMaximizeRestore();
            }
            else
            {
                try
                {
                    DragMove();
                }
                catch { }
            }
        }

        private bool IsClickOnInteractiveElement(DependencyObject source)
        {
            while (source != null)
            {
                if (source is System.Windows.Controls.Button) return true;
                if (source is System.Windows.Shapes.Path) return true;
                if (source is System.Windows.Controls.Primitives.ButtonBase) return true;
                source = VisualTreeHelper.GetParent(source);
            }
            return false;
        }

        private void ToggleMaximizeRestore()
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        // Window control buttons handlers
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleMaximizeRestore();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            UpdateMaximizeIcon();
        }

        private void UpdateMaximizeIcon()
        {
            if (WindowState == WindowState.Maximized)
            {
                // Set Restore icon (two overlapping squares)
                MaximizeIcon.Data = Geometry.Parse("M2,3 L7,3 L7,8 L2,8 Z M3,1 L8,1 L8,6 L7,6");
            }
            else
            {
                // Set Maximize icon (single square)
                MaximizeIcon.Data = Geometry.Parse("M1,1 L9,1 L9,9 L1,9 Z");
            }
        }
    }
}
