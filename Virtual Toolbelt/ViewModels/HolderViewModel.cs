using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace Virtual_Toolbelt.ViewModels
{
    public class HolderViewModel : BaseViewModel
    {
        //wrapped holder
        private Models.Holder holder;

        public HolderViewModel(Models.Holder h)
        {
            holder = h;
            //commands initialization
            OpenCommand = new Utilities.ArgsCommand<MouseButtonEventArgs>(OpenMethod);
            MouseEnterCommand = new Utilities.ArgsCommand<object>(MouseEnterMethod);
            MouseLeaveCommand = new Utilities.ArgsCommand<object>(MouseLeaveMethod);
            MouseRightButtonDownCommand = new Utilities.ArgsCommand<object>(MouseRightButtonDownMethod);
        }

        //holder's properties access
        public string Path
        {
            set
            {
                holder.Path = value;
                Notify();
            }
            get { return holder.Path; }
        }

        public string PathName
        {
            get
            {
                if (Path == null)
                {
                    return "";
                }
                return System.IO.Path.GetFileNameWithoutExtension(holder.PathName);
            }
        }

        public System.Windows.Media.ImageSource Icon
        {
            get
            {
                return holder.Icon;
            }
        }
        private double iconWidth = 50;
        public double IconWidth
        {
            get
            {
                if (Path == null)
                {
                    return 0;
                }
                return iconWidth;
            }
            set
            {
                iconWidth = value;
                Notify();
            }
        }

        //binded to the circle around the icon in the view
        System.Windows.Media.Brush ellipseColor= System.Windows.Media.Brushes.DarkBlue;
        public System.Windows.Media.Brush EllipseColor
        {
            get { return ellipseColor; }
            set { ellipseColor = value; Notify(); }
        }

        //binded to the text under the icon in the view
        System.Windows.Media.Brush textColor = System.Windows.Media.Brushes.White;
        public System.Windows.Media.Brush TextColor
        {
            get { return textColor; }
            set { textColor = value; Notify(); }
        }

        //opens the application of the holder when double-clicked
        public Utilities.ArgsCommand<MouseButtonEventArgs> OpenCommand { get; private set; }
        private void OpenMethod(MouseButtonEventArgs e)
        {
            if (File.Exists(Path) || (Directory.Exists(Path)))
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    System.Diagnostics.Process.Start(Path);
                }
            }
        }

        //highlights the HolderView when hovered with the mouse
        public Utilities.ArgsCommand<object> MouseEnterCommand { get; private set; }
        private void MouseEnterMethod(object args)
        {
            EllipseColor=System.Windows.Media.Brushes.Blue;
            TextColor = System.Windows.Media.Brushes.Yellow;
        }

        //unhighlights the HolderView when the mouse leaves
        public Utilities.ArgsCommand<object> MouseLeaveCommand { get; private set; }
        public void MouseLeaveMethod(object args)
        {
            EllipseColor = System.Windows.Media.Brushes.DarkBlue;
            TextColor = System.Windows.Media.Brushes.White;
        }

        //reference to the ObservableCollection of which this HolderViewModel is part
        public ObservableCollection<HolderViewModel> ParentCollection { get; set; }

        //deletes this HolderViewModel when right-clicked
        public Utilities.ArgsCommand<object> MouseRightButtonDownCommand { get; private set; }
        private void MouseRightButtonDownMethod(object args)
        {
            ParentCollection.Remove(this);
        }
    }
}