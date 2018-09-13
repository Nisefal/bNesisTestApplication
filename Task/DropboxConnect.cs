using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using TestTask.Exceptions;
using System.Threading.Tasks;
using bNesis.Sdk;
using bNesis.Sdk.FileStorages.Common;

namespace TestTask
{
    public class DropboxConnect
    {
        private static bNesis.Sdk.FileStorages.Dropbox.Dropbox dropbox;
        private static ServiceManager manager;
        private static string bNesisAPIEndPoint = "";
        private static string redirectUrl = "http://localhost:809/";
        private static string bNesisDeveloperId = Properties.Settings.Default.DevID;
        private static string appKey = Properties.Settings.Default.appKey;
        private static string appSecret = Properties.Settings.Default.appSecret;

        public DropboxConnect()
        {
            Task.Run(() =>
            {
                manager = new ServiceManager();

                int SDKInitializeResult = manager.InitializeRich(bNesisAPIEndPoint);

                if (SDKInitializeResult != ServiceManager.errorCodeNoError)
                {
                    #region Error output
                    switch (SDKInitializeResult)
                    {
                        case ServiceManager.errorCodeNotConnected:
                            {
                                UI.PageController.ShowConnectionMessage($"Error code {SDKInitializeResult}: {ServiceManager.errorCodeNotConnectedDesctiption}");
                                break;
                            }
                        case ServiceManager.errorCodeBadServerName:
                            {
                                UI.PageController.ShowConnectionMessage($"Error code {SDKInitializeResult}: {ServiceManager.errorCodeBadServerNameDescription}");
                                break;
                            }
                        case ServiceManager.errorCodeBadUrl:
                            {
                                UI.PageController.ShowConnectionMessage($"Error code {SDKInitializeResult}: {ServiceManager.errorCodeBadUrlDescription}");
                                break;
                            }
                        case ServiceManager.errorCodeLibraryLocation:
                            {
                                UI.PageController.ShowConnectionMessage($"Error code {SDKInitializeResult}: {ServiceManager.errorCodeLibraryLocationDesctiption}");
                                break;
                            }
                        case ServiceManager.errorCodeNotLibraryIsLoaded:
                            {
                                UI.PageController.ShowConnectionMessage($"Error code {SDKInitializeResult}: {ServiceManager.errorCodeNotLibraryIsLoadedDesctiption}");
                                break;
                            }
                        case ServiceManager.errorCodeNotSlash:
                            {
                                UI.PageController.ShowConnectionMessage($"Error code {SDKInitializeResult}: {ServiceManager.errorCodeNotSlashDesctiption}");
                                break;
                            }
                        case ServiceManager.errorCodeProviderDoesNotExist:
                            {
                                UI.PageController.ShowConnectionMessage($"Error code {SDKInitializeResult}: {ServiceManager.errorCodeProviderDoesNotExistDescription}");
                                break;
                            }
                        case ServiceManager.errorCodeServiceDoesNotExist:
                            {
                                UI.PageController.ShowConnectionMessage($"Error code {SDKInitializeResult}: {ServiceManager.errorCodeServiceDoesNotExistDescription}");
                                break;
                            }
                        case ServiceManager.errorCodeUnknownServerConnection:
                            {
                                UI.PageController.ShowConnectionMessage($"Error code {SDKInitializeResult}: {ServiceManager.errorCodeUnknownServerConnectionDescription}");
                                break;
                            }
                        default:
                            break;
                    }
                    #endregion
                }
                else
                {
                    //Succesful Initialization 
                    Task.Run(() =>
                    {
                        try
                        {
                            dropbox = manager.CreateInstanceDropbox(bNesisDeveloperId, appKey, appSecret, redirectUrl);

                            if (manager.GetLastError() == "Access is denied")
                            {
                                var response = System.Windows.MessageBox.Show("Need fix to access rights (Administrator rights will be demanded)", "Fix rights", System.Windows.MessageBoxButton.YesNo);
                                if (response == System.Windows.MessageBoxResult.Yes)
                                {
                                    GrantRights();
                                }

                                try
                                {
                                    dropbox.Auth(bNesisDeveloperId, appKey, appSecret, redirectUrl);
                                }
                                catch
                                {
                                    UI.PageController.ShowConnectionMessage("Unknown exception while connecting dropbox.");
                                    return;
                                }
                            }
                        
                            if (string.IsNullOrEmpty(dropbox.bNesisToken))
                            {
                                //if the bNesisToken is empty, use the GetLastError method to get an error description
                                if (!string.IsNullOrEmpty(manager.GetLastError())) UI.PageController.ShowConnectionMessage("Authorization problem: " + manager.GetLastError());
                                else UI.PageController.ShowConnectionMessage("An Authorization problem, please check parameters.\n");

                                // auth error

                                return;
                            }

                            UI.PageController.ShowConnectionMessage("An authorization success! The Dropbox instance is created.\n");
                            // auth success

                        }
                        catch(NotCreatedException)
                        {
                            throw;
                        }
                        catch (Exception ex)
                        {

                        }
                        GetItemsFromStorage();
                    });
                }
            });
        }

        internal static void RemoveFile(string fileName)
        {
            if (dropbox.bNesisToken!=null)
            {
                dropbox.Delete("\\"+fileName);
            }
        }

        public static string Upload(string filePath)
        {
            string uploadResult = String.Empty;
            if (dropbox.bNesisToken != null)
            {
                Stream uploadStream = File.OpenRead(filePath);
                string destinationFileName = Path.GetFileName(filePath);
                
                uploadResult = dropbox.UploadFile(uploadStream, destinationFileName);
            }
            else
            {
                UI.PageController.ShowConnectionMessage("Upload impossible. Dropbox is unaccesible.");
            }
            UI.PageController.RepaintContentDropbox();
            return uploadResult;
        }

        internal bool IsClosed()
        {
            if (dropbox == null)
                return true;
            if (string.IsNullOrEmpty(dropbox.bNesisToken))
            {
                return true;
            }
            return false;   
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        private void GrantRights()
        {
            string bat = @"RightsFix.bat";
            if (!System.IO.File.Exists(bat))
            {
                UI.PageController.ShowConnectionMessage("Rights fixer file was lost");
                return;
            }

            var psi = new ProcessStartInfo();
            string path = AppDomain.CurrentDomain.BaseDirectory + bat;
            psi.CreateNoWindow = true;
            psi.FileName = @"cmd.exe";
            psi.Verb = "runas";
            psi.Arguments = "/c " +path;

            Process.Start(psi);
        }

        public static FileStorageItem[] GetItemsFromStorage(string path = "")
        {
            if (dropbox.bNesisToken!=null)
            {
                try
                {
                    FileStorageItem[] files = null;
                    FileStorageItem[] folders = null;
                    if (dropbox != null)
                    {
                        files = dropbox.GetFiles($"\\{path}");
                        folders = dropbox.GetFolders($"\\{path}");
                        var temp = files.Concat(folders);
                        return (FileStorageItem[])temp;
                    }
                }
                catch (Exception exc)
                {

                }
            }
            return null;
        }
    }
}
