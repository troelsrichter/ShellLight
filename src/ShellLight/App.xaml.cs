using System;
using System.IO.IsolatedStorage;
using System.Windows;

namespace ShellLight
{
    public partial class App : Application
    {
        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;
            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new MainPage();
            if (IsRunningOutOfBrowser)
            {
                RestoreWindowUserSettings();
                this.CheckAndDownloadUpdateCompleted += App_CheckAndDownloadUpdateCompleted;
                this.CheckAndDownloadUpdateAsync();
            }
        }

        void App_CheckAndDownloadUpdateCompleted(object sender, CheckAndDownloadUpdateCompletedEventArgs e)
        {
            if (e.UpdateAvailable)
            {
                ViewHelper.ShowMessage(
                    "A new version of " + Config.ApplicationName +
                    " is downloaded.\nIt will be installed automatically after restarting the application.\nPlease enjoy!",
                    "New version is downloaded");
            }
            else if (e.Error != null)
            {
                ViewHelper.ShowError(
                   "A new version of " + Config.ApplicationName + " is available, but an error has occurred during the automatic update.\n" +
                   "Please uninstall and goto the application home page for the new version");
            }
        }

        private void Application_Exit(object sender, EventArgs e)
        {
            if (IsRunningOutOfBrowser)
            {
                SaveWindowUserSettings();
            }
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }

            ViewHelper.ShowError(e.ExceptionObject.ToString());
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight 2 Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }

        private void SaveWindowUserSettings()
        {
            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

            if (MainWindow.WindowState == WindowState.Normal)
            {
                appSettings[Config.IsolatedStorage.WindowTop] = MainWindow.Top;
                appSettings[Config.IsolatedStorage.WindowLeft] = MainWindow.Left;
                appSettings[Config.IsolatedStorage.WindowWidth] = MainWindow.Width;
                //if you minimize the window in height it is possible to set the height to (double)UInt32.MaxValue+1 which is invalid
                appSettings[Config.IsolatedStorage.WindowHeight] = (MainWindow.Height == (double)UInt32.MaxValue+1) ? 0 : MainWindow.Height;
            }
            appSettings[Config.IsolatedStorage.WindowState] = (UInt32)MainWindow.WindowState;
        }

        private void RestoreWindowUserSettings()
        {
            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
            if (appSettings.Contains(Config.IsolatedStorage.WindowState))
            {
                MainWindow.WindowState = (WindowState)(UInt32)appSettings[Config.IsolatedStorage.WindowState];
            }

            if (MainWindow.WindowState == WindowState.Normal && appSettings.Contains(Config.IsolatedStorage.WindowTop))
            {
                MainWindow.Top = (double)appSettings[Config.IsolatedStorage.WindowTop];
                MainWindow.Left = (double)appSettings[Config.IsolatedStorage.WindowLeft];
                MainWindow.Width = (double)appSettings[Config.IsolatedStorage.WindowWidth];
                MainWindow.Height = (double)appSettings[Config.IsolatedStorage.WindowHeight];
            }
        }
    }
}