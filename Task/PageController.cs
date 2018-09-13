using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Exceptions;
using System.Windows.Threading;

namespace TestTask.UI
{
    class PageController
    {
        private static MainWindow window;

        public static void InitializePageController(MainWindow _window)
        {
            window = _window;
        }

        public static void ShowConnectionMessage(string message)
        {
            window.ConnectionMessage(message);
        }

        public static void AddToConnectionMessage(string message)
        {
            window.AddConnectionMessage(message);
        }

        public static void CleanConnectionMessage()
        {
            window.ConnectionMessage("");
        }

        internal static void RepaintContentDropbox()
        {
            window.RepaintContentDropbox();
            //throw new NotImplementedException();
        }

        internal static void DeleteItem(string fileName)
        {
            window.RemoveFile(fileName);
            //throw new NotImplementedException();
        }
    }
}
