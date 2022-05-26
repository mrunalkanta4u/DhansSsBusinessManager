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
//using System.Windows.Data;
//using System.Windows.Media.Imaging;

namespace DataAccounts
{
    public static class Utils
    {
        public static ObservableCollection<SDKTemplate.Model.Contact> Contacts = new ObservableCollection<SDKTemplate.Model.Contact>();
        public static ObservableCollection<SDKTemplate.Model.Dream> Dreams = new ObservableCollection<SDKTemplate.Model.Dream>();
        public static ObservableCollection<SDKTemplate.Model.CallLog> CallLogs = new ObservableCollection<SDKTemplate.Model.CallLog>();
        public static ObservableCollection<SDKTemplate.Model.TeamMember> TeamMembers = new ObservableCollection<SDKTemplate.Model.TeamMember>();
        public static StorageFolder localFolder = ApplicationData.Current.RoamingFolder;
        public static WriteableBitmap wbm = new WriteableBitmap(600, 800);
        public static string imageUri = String.Empty;

        /// <summary>
        /// GENERIC FUNCTIONS START
        /// </summary>

        public static void ReadContacts()
        {
            Contacts = ReadDataFile<SDKTemplate.Model.Contact>(Contacts, "MyNameList.json");//"MyNameList.xml"); // passing Contacts just to initialise the object
            //ReadContactFromFile("MyNameList.xml");
        }
        public static void SaveContacts()
        {
            WriteDataFile<SDKTemplate.Model.Contact>(Contacts, "MyNameList.json");// "MyNameList.xml");
            //WriteContactToFile("MyNameList.xml");
        }
        public static void ReadDreams()
        {
            Dreams = ReadDataFile<SDKTemplate.Model.Dream>(Dreams, "MyDreamList.json");
        }
        public static void SaveDreams()
        {
            WriteDataFile<SDKTemplate.Model.Dream>(Dreams, "MyDreamList.json");
        }
        public static void ReadTeamMembers()
        {
            TeamMembers = ReadDataFile<TeamMember>(TeamMembers, "MyTeamList.json");
        }
        public static void SaveTeamMembers()
        {
            WriteDataFile<TeamMember>(TeamMembers, "MyTeamList.json");
        }
        public static void ReadLogs()
        {
            CallLogs = ReadDataFile<CallLog>(CallLogs, "MyCallLogs.json");
        }
        public static void WriteLogs()
        {
            WriteDataFile<CallLog>(CallLogs, "MyCallLogs.json");
        }

        internal static void TickDream(string dreamName)
        {
            Dream DreamToTick = Utils.Dreams.Where((item) => { return item.DreamName.Trim().ToLower().Equals(dreamName.Trim().ToLower()); }).FirstOrDefault();
            DreamToTick.Achieved = true;
        }
        internal static void CompleteKYC(string name)
        {
            //Utils.ReadTeamMembers();
            TeamMember IRToUpdate = Utils.TeamMembers.Where((item) => { return item.Name.Trim().ToLower().Equals(name.Trim().ToLower()); }).FirstOrDefault();
            IRToUpdate.KYCDone = true;
            //Utils.SaveTeamMembers();
        }

        public static ObservableCollection<T> ReadDataFile<T>(ObservableCollection<T> Data, String fileName)
        {
            try
            {
                var localFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
                fileName = localFolder.Path + "\\" + fileName;
                //if (!File.Exists(fileName))
                //    File.CreateText(fileName);
                using (StreamReader file = File.OpenText(fileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Data = (ObservableCollection<T>)serializer.Deserialize(file, typeof(ObservableCollection<T>));
                }
                return Data;
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
                return Data;//(ObservableCollection<T>)null;
            }
        }
        public static void WriteDataFile<T>(ObservableCollection<T> Data, String fileName)
        {
            try
            {
                var localFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
                fileName = localFolder.Path + "\\" + fileName;
                using (StreamWriter file = File.CreateText(fileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, Data);
                    string abs = file.ToString();
                }
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
            }
        }

        public static void Upload(StorageFile backupFile)
        {
            if (backupFile != null)
            {
                //Windows.Storage.CachedFileManager.DeferUpdates(backupFile);
                try
                {
                    if (backupFile.Name.Equals("Contact.csv"))
                    {
                        int fileCount = 0;
                        using (StreamReader rd = File.OpenText(backupFile.Path))
                        {
                            while (!rd.EndOfStream)
                            {
                                SDKTemplate.Model.Contact tempContact = new SDKTemplate.Model.Contact();
                                var splits = rd.ReadLine().Split('|');

                                if (!splits[0].ToString().Trim().Equals(String.Empty))
                                    tempContact.Name = splits[0].ToString();
                                if (!splits[1].ToString().Trim().Equals(String.Empty))
                                    tempContact.PhoneNumber = splits[1].ToString();
                                if (!splits[2].ToString().Trim().Equals(String.Empty))
                                    tempContact.Location = splits[2].ToString();
                                if (!splits[3].ToString().Trim().Equals(String.Empty))
                                    tempContact.Category = splits[3].ToString();
                                if (!splits[4].ToString().Trim().Equals(String.Empty))
                                    tempContact.Info = splits[4].ToString();
                                if (!splits[5].ToString().Trim().Equals(String.Empty))
                                    tempContact.Invite = splits[5].ToString();
                                if (!splits[6].ToString().Trim().Equals(String.Empty))
                                    tempContact.Remark = splits[6].ToString();
                                if (!splits[7].ToString().Trim().Equals(String.Empty))
                                    tempContact.ImageUrl = splits[7].ToString();
                                AddContact(tempContact as SDKTemplate.Model.Contact);
                                fileCount++;
                            }

                            if (fileCount > 0)
                            {
                                //StatusBlock.Text = "Imported " + fileCount.ToString() + " contacts successfully!!! \nTotal contacts in NameList is " + Utils.Contacts.Count;
                                //rootPage.NotifyUser("Imported " + fileCount.ToString() + " contacts successfully!!! \nTotal contacts in NameList is " + Utils.Contacts.Count, NotifyType.StatusMessage); // CRASHING IN WINDOWS MODE
                            }
                            Utils.SaveContacts();
                        }
                    }

                    if (backupFile.Name.Equals("Dream.csv"))
                    {
                        int fileCount = 0;
                        using (StreamReader rd = File.OpenText(backupFile.Path))
                        {
                            while (!rd.EndOfStream)
                            {
                                SDKTemplate.Model.Dream tempDream = new SDKTemplate.Model.Dream();
                                var splits = rd.ReadLine().Split('|');

                                if (!splits[0].ToString().Trim().Equals(String.Empty))
                                    tempDream.DreamName = splits[0].ToString();
                                if (!splits[1].ToString().Trim().Equals(String.Empty))
                                    tempDream.Details = splits[1].ToString();
                                if (!splits[2].ToString().Trim().Equals(String.Empty))
                                    tempDream.Category = splits[2].ToString();
                                if (!splits[3].ToString().Trim().Equals(String.Empty))
                                    tempDream.TatgetDate = splits[3].ToString();
                                if (!splits[4].ToString().Trim().Equals(String.Empty))
                                    tempDream.Achieved = Convert.ToBoolean(splits[4].ToString());
                                if (!splits[5].ToString().Trim().Equals(String.Empty))
                                    tempDream.Remark = splits[5].ToString();
                                if (!splits[6].ToString().Trim().Equals(String.Empty))
                                    tempDream.ImageUrl = splits[6].ToString();
                                AddDream(tempDream as SDKTemplate.Model.Dream);
                                fileCount++;
                            }

                            if (fileCount > 0)
                            {
                                //StatusBlock.Text = "Imported " + fileCount.ToString() + " contacts successfully!!! \nTotal contacts in NameList is " + Utils.Contacts.Count;
                                //rootPage.NotifyUser("Imported " + fileCount.ToString() + " contacts successfully!!! \nTotal contacts in NameList is " + Utils.Contacts.Count, NotifyType.StatusMessage); // CRASHING IN WINDOWS MODE
                            }
                            Utils.SaveDreams();
                        }
                    }

                    if (backupFile.Name.Equals("TeamMember.csv"))
                    {
                        int fileCount = 0;
                        using (StreamReader rd = File.OpenText(backupFile.Path))
                        {
                            while (!rd.EndOfStream)
                            {
                                SDKTemplate.Model.TeamMember tempTeamMember = new SDKTemplate.Model.TeamMember();
                                var splits = rd.ReadLine().Split('|');

                                if (!splits[0].ToString().Trim().Equals(String.Empty))
                                    tempTeamMember.Name = splits[0].ToString();
                                if (!splits[1].ToString().Trim().Equals(String.Empty))
                                    tempTeamMember.PhoneNumber = splits[1].ToString();
                                if (!splits[2].ToString().Trim().Equals(String.Empty))
                                    tempTeamMember.IRId = splits[2].ToString();
                                if (!splits[3].ToString().Trim().Equals(String.Empty))
                                    tempTeamMember.Password = splits[3].ToString();
                                if (!splits[4].ToString().Trim().Equals(String.Empty))
                                    tempTeamMember.CPAPassword = splits[4].ToString();
                                if (!splits[5].ToString().Trim().Equals(String.Empty))
                                    tempTeamMember.SecurityQA = splits[5].ToString();
                                if (!splits[6].ToString().Trim().Equals(String.Empty))
                                    tempTeamMember.SecurityWord = splits[6].ToString();
                                if (!splits[7].ToString().Trim().Equals(String.Empty))
                                    tempTeamMember.MailId = splits[7].ToString();
                                if (!splits[8].ToString().Trim().Equals(String.Empty))
                                    tempTeamMember.MailIdPassword = splits[8].ToString();
                                if (!splits[9].ToString().Trim().Equals(String.Empty))
                                    tempTeamMember.KYCDone = Convert.ToBoolean(splits[9].ToString());
                                if (!splits[10].ToString().Trim().Equals(String.Empty))
                                    tempTeamMember.Remark = splits[10].ToString();
                                if (!splits[11].ToString().Trim().Equals(String.Empty))
                                    tempTeamMember.ImageUrl = splits[11].ToString();
                                AddTeamMember(tempTeamMember as SDKTemplate.Model.TeamMember);
                                fileCount++;
                            }

                            if (fileCount > 0)
                            {
                                //StatusBlock.Text = "Imported " + fileCount.ToString() + " contacts successfully!!! \nTotal contacts in NameList is " + Utils.Contacts.Count;
                                //rootPage.NotifyUser("Imported " + fileCount.ToString() + " contacts successfully!!! \nTotal contacts in NameList is " + Utils.Contacts.Count, NotifyType.StatusMessage); // CRASHING IN WINDOWS MODE
                            }
                            Utils.SaveTeamMembers();
                        }
                    }

                    if (backupFile.Name.Equals("CallLog.csv"))
                    {
                        int fileCount = 0;
                        using (StreamReader rd = File.OpenText(backupFile.Path))
                        {
                            while (!rd.EndOfStream)
                            {
                                SDKTemplate.Model.CallLog tempCallLog = new SDKTemplate.Model.CallLog();
                                var splits = rd.ReadLine().Split('|');

                                if (!splits[0].ToString().Trim().Equals(String.Empty))
                                    tempCallLog.Name = splits[0].ToString();
                                if (!splits[1].ToString().Trim().Equals(String.Empty))
                                    tempCallLog.TypeOfCall = splits[1].ToString();
                                if (!splits[2].ToString().Trim().Equals(String.Empty))
                                    tempCallLog.Date = splits[2].ToString();
                                if (!splits[3].ToString().Trim().Equals(String.Empty))
                                    tempCallLog.Remark = splits[3].ToString();
                                AddCallLog(tempCallLog as SDKTemplate.Model.CallLog);
                                fileCount++;
                            }

                            if (fileCount > 0)
                            {
                                //StatusBlock.Text = "Imported " + fileCount.ToString() + " contacts successfully!!! \nTotal contacts in NameList is " + Utils.Contacts.Count;
                                //rootPage.NotifyUser("Imported " + fileCount.ToString() + " contacts successfully!!! \nTotal contacts in NameList is " + Utils.Contacts.Count, NotifyType.StatusMessage); // CRASHING IN WINDOWS MODE
                            }
                            Utils.WriteLogs();
                        }
                    }
                    File.Delete(backupFile.Path);
                }
                catch (Exception ex)
                {
                    String ExceptionMessage = ex.Message;
                    //return Data;//(ObservableCollection<T>)null;
                }
            }
        }
        public static async void Download<T>(string path)
        {
            Type type = typeof(T);

            string directory = Path.GetDirectoryName(path);
            var tempFolder = Windows.Storage.ApplicationData.Current.TemporaryFolder;
            var file = await tempFolder.CreateFileAsync(type.Name.ToString() + ".csv", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            //await Windows.Storage.FileIO.WriteTextAsync(file, content);
            if (file != null)
            {
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                StringBuilder tempString = new StringBuilder();
                try
                {
                    if (type.Name.Equals("Contact"))
                    {
                        Utils.ReadContacts();
                        foreach (SDKTemplate.Model.Contact contact in Utils.Contacts)
                        {
                            tempString.Append(contact.Name.ToString())
                            .Append("|")
                            .Append(contact.PhoneNumber.ToString())
                            .Append("|")
                            .Append(contact.Location.ToString())
                            .Append("|")
                            .Append(contact.Category.ToString())
                            .Append("|")
                            .Append(contact.Info.ToString())
                            .Append("|")
                            .Append(contact.Invite.ToString())
                            .Append("|")
                            .Append(contact.Remark.ToString())
                            .Append("|")
                            .Append(contact.ImageUrl.ToString())
                            .AppendLine();
                        }
                    }
                    if (type.Name.Equals("Dream"))
                    {
                        Utils.ReadDreams();
                        foreach (SDKTemplate.Model.Dream dream in Utils.Dreams)
                        {
                            tempString.Append(dream.DreamName.ToString())
                            .Append("|")
                            .Append(dream.Details.ToString())
                            .Append("|")
                            .Append(dream.Category.ToString())
                            .Append("|")
                            .Append(dream.TatgetDate.ToString())
                            .Append("|")
                            .Append(dream.Achieved.ToString())
                            .Append("|")
                            .Append(dream.Remark.ToString())
                            .Append("|")
                            .Append(dream.ImageUrl.ToString())
                            .AppendLine();
                        }
                    }
                    if (type.Name.Equals("TeamMember"))
                    {
                        Utils.ReadTeamMembers();
                        foreach (SDKTemplate.Model.TeamMember teamMember in Utils.TeamMembers)
                        {
                            tempString.Append(teamMember.Name.ToString())
                            .Append("|")
                            .Append(teamMember.PhoneNumber.ToString())
                            .Append("|")
                            .Append(teamMember.IRId.ToString())
                            .Append("|")
                            .Append(teamMember.Password.ToString())
                            .Append("|")
                            .Append(teamMember.CPAPassword.ToString())
                            .Append("|")
                            .Append(teamMember.SecurityQA.ToString())
                            .Append("|")
                            .Append(teamMember.SecurityWord.ToString())
                            .Append("|")
                            .Append(teamMember.MailId.ToString())
                            .Append("|")
                            .Append(teamMember.MailIdPassword.ToString())
                            .Append("|")
                            .Append(teamMember.KYCDone.ToString())
                            .Append("|")
                            .Append(teamMember.Remark.ToString())
                            .Append("|")
                            .Append(teamMember.ImageUrl.ToString())
                            .AppendLine();
                        }
                    }
                    if (type.Name.Equals("CallLog"))
                    {
                        Utils.ReadLogs();
                        foreach (SDKTemplate.Model.CallLog callLog in Utils.CallLogs)
                        {
                            tempString.Append(callLog.Name.ToString())
                            .Append("|")
                            .Append(callLog.TypeOfCall.ToString())
                            .Append("|")
                            .Append(callLog.Date.ToString())
                            .Append("|")
                            .Append(callLog.Remark.ToString())
                            .AppendLine();
                        }
                    }
                }
                catch (Exception ex)
                {
                    String ExceptionMessage = ex.Message;
                }
                await Windows.Storage.FileIO.WriteTextAsync(file, tempString.ToString());
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    //this.StatusBlock.Text = "Name List has been exported to " + file.Name.ToString() + " successfully!!!";
                    //rootPage.NotifyUser("Name List has been exported to " + file.Name.ToString() + " successfully!!!", NotifyType.StatusMessage);
                }
                else
                {
                    //this.StatusBlock.Text = "File " + file.Name + " couldn't be saved.";
                    //rootPage.NotifyUser("File " + file.Name + " couldn't be saved.", NotifyType.StatusMessage);
                }
            }
            else
            {
                //this.StatusBlock.Text = "Operation cancelled.";
                //rootPage.NotifyUser("Operation cancelled.", NotifyType.StatusMessage);
            }
        }
        public static void AddContact(SDKTemplate.Model.Contact sender)
        {
            if (sender != null)
            {
                Utils.Contacts.Add(sender);
            }
        }
        public static void AddDream(SDKTemplate.Model.Dream sender)
        {
            if (sender != null)
            {
                Utils.Dreams.Add(sender);
            }
        }
        public static void AddTeamMember(SDKTemplate.Model.TeamMember sender)
        {
            if (sender != null)
            {
                Utils.TeamMembers.Add(sender);
            }
        }
        public static void AddCallLog(SDKTemplate.Model.CallLog sender)
        {
            if (sender != null)
            {
                Utils.CallLogs.Add(sender);
            }
        }
        //public static Boolean ToBoolean(this string str)
        //{
        //    String cleanValue = (str ?? "").Trim();
        //    if (String.Equals(cleanValue, "False", StringComparison.OrdinalIgnoreCase))
        //        return false;
        //    return
        //        (String.Equals(cleanValue, "True", StringComparison.OrdinalIgnoreCase)) ||
        //        (cleanValue != "0");
        //}
       
        public static async void Reset<T>(ObservableCollection<T> Data, String fileName, Boolean OverWrite)
        {
            //Button button = sender as Button;
            if (OverWrite)
                Utils.Contacts.Clear();
            else
                Utils.ReadContacts();

            FileOpenPicker opener = new FileOpenPicker();
            opener.ViewMode = PickerViewMode.Thumbnail;
            opener.FileTypeFilter.Add(".csv");
            opener.FileTypeFilter.Add(".txt");
            StorageFile file = await opener.PickSingleFileAsync();
            StorageFolder appFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            string name = file.Name.ToString();
            await file.CopyAsync(appFolder, name, NameCollisionOption.ReplaceExisting);
            string tempFile = appFolder.Path.ToString() + "\\" + name;
            StorageFile csvFile = await appFolder.GetFileAsync(name);
            try
            {
                if (await appFolder.GetFileAsync(name) != null)
                {
                    int fileCount = 0;
                    using (StreamReader rd = File.OpenText(tempFile))
                    {
                        while (!rd.EndOfStream)
                        {
                            //T tempContact = new Model.Contact();
                            //var splits = rd.ReadLine().Split('|');

                            //if (!splits[0].ToString().Trim().Equals(String.Empty))
                            //    tempContact.Name = splits[0].ToString();
                            //if (!splits[1].ToString().Trim().Equals(String.Empty))
                            //    tempContact.PhoneNumber = splits[1].ToString();
                            //if (!splits[2].ToString().Trim().Equals(String.Empty))
                            //    tempContact.Location = splits[2].ToString();
                            //if (!splits[3].ToString().Trim().Equals(String.Empty))
                            //    tempContact.Category = splits[3].ToString();
                            //if (!splits[4].ToString().Trim().Equals(String.Empty))
                            //    tempContact.Info = splits[4].ToString();
                            //if (!splits[5].ToString().Trim().Equals(String.Empty))
                            //    tempContact.Invite = splits[5].ToString();
                            //if (!splits[6].ToString().Trim().Equals(String.Empty))
                            //    tempContact.Remark = splits[6].ToString();
                            //AddContact(tempContact as Model.Contact);
                            fileCount++;
                        }

                        if (fileCount > 0)
                        {
                            //rootPage.NotifyUser("Imported " + fileCount.ToString() + " contacts successfully!!! \nTotal contacts in NameList is " + Utils.Contacts.Count, NotifyType.StatusMessage); // CRASHING IN WINDOWS MODE
                        }
                    }
                }
                Utils.SaveContacts();
                File.Delete(tempFile);
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
            }
        }
        public static async void Archive(StorageFolder directory, StorageFile backupFileName)
        {
            string fileName = GetRandomFileName() + ".zip";
            string zipFilePath = System.IO.Path.Combine(ApplicationData.Current.TemporaryFolder.Path, fileName);
            try
            {
                await Task.Run(() => ZipFile.CreateFromDirectory(directory.Path, zipFilePath));
                StorageFile zipFile = await ApplicationData.Current.TemporaryFolder.GetFileAsync(fileName);
                await zipFile.CopyAndReplaceAsync(backupFileName);
                await zipFile.DeleteAsync(StorageDeleteOption.Default);
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
            }
        }
        public static async void Unarchive(StorageFile archiveFile, StorageFolder backupFolder)
        {
            string fileName = GetRandomFileName() + ".zip";
            try
            {
                StorageFile tempZipFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fileName);
                await archiveFile.CopyAndReplaceAsync(tempZipFile);
                ZipFile.ExtractToDirectory(tempZipFile.Path, backupFolder.Path);
                await tempZipFile.DeleteAsync(StorageDeleteOption.Default);
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
            }
        }

        public static async Task<StorageFile> GetTextFile(string fileName, string content)
        {
            var localFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            var file = await localFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(file, content);
            return file;
        }
        public static async void SendMail(string body, string subject = "Call Logs", string to = "mrunalkanta4u@gmail.com")
        {
            try
            {
                EmailMessage email = new EmailMessage();
                email.To.Add(new EmailRecipient(to));
                email.Subject = subject;
                body = "Hi !!!\nHere is your call logs.\n\n" + body; 

                email.Body = body;
                var file = await GetTextFile("CallLogs.txt",body);
                
                email.Attachments.Add(new EmailAttachment(file.Name, file));
                await EmailManager.ShowComposeNewEmailAsync(email);              
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
            }
        }
        public static async void SendMessage(string message, Windows.ApplicationModel.Contacts.Contact recipient)
        {
            try
            {
                var chatMessage = new ChatMessage();
                chatMessage.Body = message;
                var phone = recipient.Phones.FirstOrDefault<Windows.ApplicationModel.Contacts.ContactPhone>();
                if (phone != null)
                {
                    chatMessage.Recipients.Add(phone.Number);
                }
                await Windows.ApplicationModel.Chat.ChatMessageManager.ShowComposeSmsMessageAsync(chatMessage);
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
            }           
        }
        public static void SendWhatsappMessage()
        {
            
        }

        public static Visibility isVisible(Control c)
        {
            if (c.Visibility == Visibility.Visible)
                return Visibility.Visible;
            //else
            //    if (c.Parent != null)
            //    return isVisible(c);
            else
                return Visibility.Collapsed;
        }
        public static Visibility toggleVisibility(Control c)
        {
            if (c.Visibility == Visibility.Collapsed)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public static async void SaveImage(StorageFile storageFile, string targetFileName)
        {
            StorageFolder folder = ApplicationData.Current.RoamingFolder;
            
            if (folder != null)
            {
                // need to add all images to a folder
                StorageFile file = await folder.CreateFileAsync(targetFileName, CreationCollisionOption.ReplaceExisting);
                using (IRandomAccessStream storageStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    BitmapEncoder encoder = await Windows.Graphics.Imaging.BitmapEncoder.CreateAsync(Windows.Graphics.Imaging.BitmapEncoder.JpegEncoderId, storageStream);
                    Stream pixelStream = wbm.PixelBuffer.AsStream();
                    byte[] pixels = new byte[pixelStream.Length];
                    await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)wbm.PixelWidth, (uint)wbm.PixelHeight, 48, 48, pixels);
                    await encoder.FlushAsync();
                }
                imageUri = file.Path.ToString();
                await storageFile.CopyAsync(folder, targetFileName, NameCollisionOption.ReplaceExisting);
                StorageFile csvFile = await folder.GetFileAsync(targetFileName);
                bool isExists = File.Exists(csvFile.Path.ToString());
            }
        }
        public static async void RetrieveImage(string imageFileName)
        {
            //string fileName = "imagefile.jpg";
            StorageFolder myfolder = ApplicationData.Current.RoamingFolder;
            BitmapImage bitmapImage = new BitmapImage();
            StorageFile file = await myfolder.GetFileAsync(imageFileName);
            var myimage = await Windows.Storage.FileIO.ReadBufferAsync(file);
            Uri uri = new Uri(file.Path);
            BitmapImage img = new BitmapImage(new Uri(file.Path));
            //image.Source = img;
        }
        public static async void RemoveImage(string targetFileName)
        {
            StorageFolder folder = ApplicationData.Current.RoamingFolder;

            if (folder != null)
            {
                if (!(targetFileName.Equals("Assets/Account.png") || targetFileName.Equals("Assets/placeholder.jpg")))
                {
                    string fileName = Path.GetFileName(targetFileName);
                    StorageFile fileToDelete = await folder.GetFileAsync(fileName);
                    if (File.Exists(fileToDelete.Path.ToString()))
                    {
                        //bool isExists = File.Exists(fileToDelete.Path.ToString());
                        await fileToDelete.DeleteAsync(StorageDeleteOption.Default);
                        //isExists = File.Exists(fileToDelete.Path.ToString());
                    }
                }
            }
        }

        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public static string GetRandomFileName()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
        public static string PopulateLogsByDate(DateTime date)
        {
            Utils.ReadLogs();

            string details = string.Empty;
            int NumberOfCalls = 0;
            foreach (CallLog callLog in Utils.CallLogs)
            {
                if (date.Date.ToString("dd MMMM yyyy , dddd ").Trim().ToLower() == callLog.Date.ToString().Trim().ToLower())
                {
                    NumberOfCalls++;
                    details = details + "[ " + NumberOfCalls.ToString() + " ] Called " + callLog.Name + " for " + callLog.TypeOfCall;
                    if (!(callLog.Remark.Trim().ToString().Equals(string.Empty)))
                    {
                        details = details + "\n" + callLog.Remark;
                    }
                    details = details + "\n\n";
                }
            }
            return details;
        }
    }


    ////
    //// Register a background task with the specified taskEntryPoint, name, trigger,
    //// and condition (optional).
    ////
    //// taskEntryPoint: Task entry point for the background task.
    //// taskName: A name for the background task.
    //// trigger: The trigger for the background task.
    //// condition: Optional parameter. A conditional event that must be true for the task to fire.
    ////
    //public static BackgroundTaskRegistration RegisterBackgroundTask(string taskEntryPoint,
    //                                                                string taskName,
    //                                                                IBackgroundTrigger trigger,
    //                                                                IBackgroundCondition condition)
    //{
    //    //
    //    // Check for existing registrations of this background task.
    //    //

    //    foreach (var cur in BackgroundTaskRegistration.AllTasks)
    //    {

    //        if (cur.Value.Name == taskName)
    //        {
    //            //
    //            // The task is already registered.
    //            //

    //            return (BackgroundTaskRegistration)(cur.Value);
    //        }
    //    }

    //    //
    //    // Register the background task.
    //    //

    //    var builder = new BackgroundTaskBuilder();

    //    builder.Name = taskName;
    //    builder.TaskEntryPoint = taskEntryPoint;
    //    builder.SetTrigger(trigger);

    //    if (condition != null)
    //    {

    //        builder.AddCondition(condition);
    //    }

    //    BackgroundTaskRegistration task = builder.Register();

    //    return task;
    //}
}
