using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bNesis.Sdk.FileStorages.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestTask
{
    /// <summary>
    /// Interaction logic for DropboxFileExplorer.xaml
    /// </summary>
    public partial class DropboxFileExplorer : Page
    {
        public DropboxFileExplorer()
        {
            InitializeComponent();
        }
        
        public void ShowContent(FileStorageItem[] items)
        {
            FileGrid.AddChildren(items);
        }

        public void Refresh()
        {
            FileGrid.CleanGrid();
            //throw new NotImplementedException();
        }

        internal void RemoveFile(string fileName)
        {
            DropboxConnect.RemoveFile(fileName);
            FileGrid.RemoveItem(fileName);
            //throw new NotImplementedException();
        }
    }
}
