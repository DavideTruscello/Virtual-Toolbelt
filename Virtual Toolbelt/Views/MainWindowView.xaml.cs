using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Virtual_Toolbelt.Views
{
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

        //allows the user to drag the window when the top label is grabbed
        //it is in the View because it does not interact with underlying data
        //in any way
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void TopLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.WindowState = WindowState.Minimized;
            }
        }
    }
}