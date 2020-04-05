using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Virtual_Toolbelt.Models
{
    //holds a path and provides the necessary property to be used by the ViewModel
    public class Holder
    {
        public string Path { get; set; }

        public string PathName {
            get
            {
                string name = "";
                if (System.IO.File.Exists(Path) 
                    || Directory.Exists(Path))
                {
                    name = System.IO.Path.GetFileName(Path);
                    if (name.Length == 0)
                    {
                        name = System.IO.Path.GetDirectoryName(Path);
                    }
                    if (name.Length >= 13)
                    {
                        int fullLenght = name.Length;
                        name = name.Substring(0, 13);
                        int i = 12;
                        while(name[i] == ' ')
                        {
                            --i;
                            name = name.Substring(0, i);
                        }
                        if (fullLenght > 13)
                            name += "....";                       
                    }
                }
                return name;
            }
        }

        public System.Windows.Media.ImageSource Icon
        {
            get
            {
                //if a file, display its icon
                if (System.IO.File.Exists(Path))
                {
                    {
                        using (System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon(Path))
                        {
                            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                                      sysicon.Handle,
                                      System.Windows.Int32Rect.Empty,
                                      BitmapSizeOptions.FromEmptyOptions());
                        }
                    }
                }
                else
                //if a folder, display the generic folder icon
                if (Directory.Exists(Path))
                { 
                    return new BitmapImage(new Uri(@"pack://application:,,,/images/FolderIcon.png", UriKind.Absolute));
                }
                //if non of the above, display the fallback image
                return new BitmapImage(new Uri(@"pack://application:,,,/images/EmptyIcon.jpg", UriKind.Absolute));
            }
        }
    }
}