using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        DropboxConnect connection;


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(LoginPage), new PropertyMetadata(""));

        public LoginPage()
        {
            InitializeComponent();
            ConnectionMessage("Starting...");
            connection = new DropboxConnect();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            bNesis.Sdk.FileStorages.Common.FileStorageItem[] collection = DropboxConnect.GetItemsFromStorage();
            foreach (var item in collection)
            {

            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectionMessage("Starting...");
            if (connection.IsClosed())
            {
                connection = new DropboxConnect();
            }
            else
                ConnectionMessage("Connection is stable.");

        }

        internal void AddConnectionMessage(string errorText)
        {
            Message += errorText;
        }

        internal void ConnectionMessage(string errorText)
        {
            Message = errorText;
        }

        private void SetDIDButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DevID = DevID.Text;
            Properties.Settings.Default.Save();
        }
    }
}
