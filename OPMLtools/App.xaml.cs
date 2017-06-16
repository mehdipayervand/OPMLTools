using System.Windows;
using log4net.Config;
using System.Linq;
namespace OPMLtools
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    using OPMLtools.Infrastructure.Models;

    using log4net;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public App()
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                MessageBox.Show(
                     "OPMLtools is already running ...",
                     "OPMLtools",
                     MessageBoxButton.OK,
                     MessageBoxImage.Information);
                this.Shutdown();
            }

            //Global exception handaling
            this.DispatcherUnhandledException += this.OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
        }

        void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
            Logger.Error(exception);

            Common.MVVM.App.AddMessage(
                "An unhandled exception has occurred.Please report it." + Environment.NewLine + exception.Message,
                LogModel.LogType.Error);
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Error(e.Exception);
            e.Handled = true;
        }
    }
}
