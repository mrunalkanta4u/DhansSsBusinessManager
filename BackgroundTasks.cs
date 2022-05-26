using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.ApplicationModel.UserDataAccounts;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using SDKTemplate.Model;
using Windows.Storage.Streams;
using System.Threading;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Chat;
using Windows.ApplicationModel.Contacts;
using System.ComponentModel;
using Windows.UI.Xaml.Media.Animation;
using Windows.Data.Json;
using Windows.UI.Xaml.Media;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media.Imaging;
using System.Xml.Serialization;
using Windows.Storage.Pickers;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Globalization;
using System.IO.Compression;
using Windows.ApplicationModel.Background;
using DataAccounts;
//using System.Windows.Data;
//using System.Windows.Media.Imaging;

namespace DataAccounts
{
    public sealed class BackgroundTasks : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskinstance)
        {
            BackgroundTaskDeferral deferral = taskinstance.GetDeferral();

            // Do background work
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            string backupFileName = "Backup" + DateTime.Now.ToString("-dd-MMMM-yyyy-hh:mm:ss") + ".zip";
            StorageFolder backupFolder = await KnownFolders.DocumentsLibrary.CreateFolderAsync("DhansSsBizMgrBackup\\", CreationCollisionOption.ReplaceExisting);
            StorageFile backupFile = await backupFolder.CreateFileAsync(backupFileName);
            var tempFolder = Windows.Storage.ApplicationData.Current.TemporaryFolder;
            tempFolder = await tempFolder.CreateFolderAsync("DhansSsBizMgrBackup\\", CreationCollisionOption.ReplaceExisting);

            Utils.Download<SDKTemplate.Model.Contact>(tempFolder.Path);
            Utils.Download<SDKTemplate.Model.Dream>(tempFolder.Path);
            Utils.Download<SDKTemplate.Model.TeamMember>(tempFolder.Path);
            Utils.Download<SDKTemplate.Model.CallLog>(tempFolder.Path);

            StorageFile fileToCopy = await tempFolder.GetFileAsync("Contact.csv");
            await fileToCopy.CopyAsync(backupFolder, "Contact.csv", NameCollisionOption.ReplaceExisting);
            await fileToCopy.DeleteAsync(StorageDeleteOption.Default);
            fileToCopy = await tempFolder.GetFileAsync("Dream.csv");
            await fileToCopy.CopyAsync(backupFolder, "Dream.csv", NameCollisionOption.ReplaceExisting);
            await fileToCopy.DeleteAsync(StorageDeleteOption.Default);
            fileToCopy = await tempFolder.GetFileAsync("TeamMember.csv");
            await fileToCopy.CopyAsync(backupFolder, "TeamMember.csv", NameCollisionOption.ReplaceExisting);
            await fileToCopy.DeleteAsync(StorageDeleteOption.Default);
            fileToCopy = await tempFolder.GetFileAsync("CallLog.csv");
            await fileToCopy.CopyAsync(backupFolder, "CallLog.csv", NameCollisionOption.ReplaceExisting);
            await fileToCopy.DeleteAsync(StorageDeleteOption.Default);

            Utils.Archive(backupFolder, backupFile);
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            deferral.Complete();
        }
        BackgroundTasks()
        {
        }
    }
}