using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Virtual_Toolbelt
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //initialization of the window and binding to the ViewModel
            Window main = new Views.MainWindowView() { DataContext = new ViewModels.MainWindowViewModel() };
            main.WindowStartupLocation = WindowStartupLocation.Manual;
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            main.Left = desktopWorkingArea.Right - main.Width;
            main.Top = desktopWorkingArea.Bottom - main.Height;
            main.Show();
        }

    } 
}
