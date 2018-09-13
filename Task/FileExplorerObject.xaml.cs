using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using bNesis.Sdk.FileStorages.Common;

namespace TestTask
{
    /// <summary>
    /// Interaction logic for FileExplorerObject.xaml
    /// </summary>
    public partial class FileExplorerObject : UserControl
    {
        private FileStorageItem item;

        public BitmapImage Image
        {
            get { return (BitmapImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(BitmapImage), typeof(FileExplorerObject), new PropertyMetadata(null));
        
        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(FileExplorerObject), new PropertyMetadata(""));

        public FileExplorerObject(FileStorageItem item)
        {
            this.item = item;
            if (item.ItemType == FileStorageItemType.File)
                Image = new BitmapImage(new Uri(@"media/file.jpg", UriKind.RelativeOrAbsolute));
            else
                Image = new BitmapImage(new Uri(@"media/folder.jpg", UriKind.RelativeOrAbsolute));

            FileName = item.Name;

            InitializeComponent();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            UI.PageController.DeleteItem(this.FileName);
        }
    }
}
