using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Virtual_Toolbelt.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            //holders gets initialized and loaded with the paths from previous time
            HolderVMs = new ObservableCollection<HolderViewModel>();
            HolderVMs.CollectionChanged += HoldersChanged;
            LoadSavedPaths();
            //commands initialization
            ExitButtonEnteredCommand = new Utilities.ArgsCommand<object>(ExitButtonEnteredMethod);
            ExitButtonLeftCommand = new Utilities.ArgsCommand<object>(ExitButtonLeftMethod);
            ExitButtonLeftMouseClickCommand = new Utilities.ArgsCommand<object>(ExitButtonLeftMouseClickMethod);
            DropFileCommand = new Utilities.ArgsCommand<DragEventArgs>(DropFileMethod);
            //when the application is closed all the loaded paths gets saved
            Application.Current.Exit += SavePaths;
        }

        //collection of Holder that will be displayed in the Window 
        public ObservableCollection<HolderViewModel> HolderVMs { get; private set; }

        //when Holders changes, notifies for HoldersCount(the try-catch is to avoid
        //the exceptions that would happen during the loading when the window is not
        //ready and IPropertyChanged would crash)
        private void HoldersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                Notify(nameof(HoldersCount));
            }
            catch (NullReferenceException) { }
        }

        //the maximum number of holders accepted
        private int maxHolders = 30;
        public int MaxHolders
        {
            get { return maxHolders; }
            private set
            {
                maxHolders = value;
                Notify();
            }
        }

        //binds to the window to return how many holders are in Holders
        public int HoldersCount
        {
            get { return HolderVMs.Count; }
        }

        private void LoadSavedPaths()
        {
            //path to the save file
            string loadPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString() + @"\Paths.txt";
            //if it does not exist then start without loading anything
            if (File.Exists(loadPath))
            {
                using (var file = File.Open(loadPath, FileMode.Open))
                using (var stream = new StreamReader(file))
                {
                    while (!stream.EndOfStream)
                    {
                        string holderPath = stream.ReadLine();
                        if (IsFilePathValid(holderPath))
                        {
                            HolderVMs.Add(new HolderViewModel(new Models.Holder() { Path = holderPath }) { ParentCollection=HolderVMs});
                        }
                    }
                }
            }
        }

        private void SavePaths(object sender, ExitEventArgs e)
        {
            //path to the save file
            string savePath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString() + @"\Paths.txt";
            //if it exists already, delete it and replace it with the new one
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            using (var file = File.Open(savePath, FileMode.Append))
            using (var stream = new StreamWriter(file))
            {
                //writes all the paths of the HolderViewModels
                foreach(HolderViewModel h in HolderVMs)
                {
                    stream.WriteLine(h.Path);
                }
            }
        }

        //If the path has already been added to Holders or does not exists, returns false
        private bool IsFilePathValid(string path)
        {
            bool alreadyAdded = false;
            foreach (HolderViewModel h in HolderVMs)
            {
                if (h.Path == path)
                {
                    alreadyAdded = true;
                    break;
                }
            }
            return ((File.Exists(path)
                || Directory.Exists(path))
                && !alreadyAdded);
        }

        //Binded to the exit button in the Window to determine which image to load
        private BitmapImage exitButtonSource= 
            new BitmapImage(new Uri(@"pack://application:,,,/images/ExitButton.png", UriKind.Absolute));
        public BitmapImage ExitButtonSource
        {
            get { return exitButtonSource; }
            set { exitButtonSource = value; Notify(); }
        }

        //makes the exit button white when hovered with the mouse
        public Utilities.ArgsCommand<object> ExitButtonEnteredCommand { get; private set; }
        private void ExitButtonEnteredMethod(object args)
        {
            ExitButtonSource = new BitmapImage(new Uri(@"pack://application:,,,/images/ExitButtonWhite.png", UriKind.Absolute));
        }

        //turn the exit button back to red
        public Utilities.ArgsCommand<object> ExitButtonLeftCommand { get; private set; }
        private void ExitButtonLeftMethod(object args)
        {
            ExitButtonSource = new BitmapImage(new Uri(@"pack://application:,,,/images/ExitButton.png", UriKind.Absolute));

        }

        //closes the application when the exit button is clicked
        public Utilities.ArgsCommand<object> ExitButtonLeftMouseClickCommand { get; private set; }
        private void ExitButtonLeftMouseClickMethod(object args)
        {
            Application.Current.Shutdown();
        }

        //adds the file or folder path(as a HolderViewModel) to Holders when dragged and dropped
        public Utilities.ArgsCommand<DragEventArgs> DropFileCommand { get; private set; }
        private void DropFileMethod(DragEventArgs e)
        {
            if (HolderVMs.Count < MaxHolders)
            {
                if (e.Data.GetDataPresent("FileName"))
                {
                    string[] folderPath = (string[])e.Data.GetData("FileName");
                    if (IsFilePathValid(folderPath[0]))
                        HolderVMs.Add(new HolderViewModel(new Models.Holder() { Path = folderPath[0] }) { ParentCollection = HolderVMs });
                }
            }
        }
    }
}