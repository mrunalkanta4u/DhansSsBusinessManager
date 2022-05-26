//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using Windows.ApplicationModel.UserDataAccounts;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using Windows.UI.Xaml;
using System.Collections.ObjectModel;
using SDKTemplate.Model;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.Serialization;
using System.IO;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Documents;
using DataAccounts;
using Windows.ApplicationModel.Contacts;
using Windows.Storage.Pickers;
using System.Xml;
using System.IO.Compression;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Windows.ApplicationModel.Email;
using System.ComponentModel;
using Windows.UI.Xaml.Media.Animation;
using Windows.Data.Json;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media.Imaging;
using System.Text;
using Windows.UI.Xaml.Data;
using Windows.System;
using Windows.UI.Xaml.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;

namespace SDKTemplate
{

    public sealed partial class Miscellaneous : Page
    {
        private MainPage rootPage = MainPage.Current;
        ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
        //private bool isLaunched = false;
        //public static ObservableCollection<Model.Contact> tempContacts = new ObservableCollection<Model.Contact>();
        //Model.Contact contact = new Model.Contact();
        //private static ObservableCollection<CallLog> CallLogs = new ObservableCollection<CallLog>();
        //TextBlock tBlk = new TextBlock();
        //tBlk.TextWrapping = TextWrapping.Wrap;
        //public static Model.Contact tempContact = new Model.Contact();

        public Miscellaneous()
        {
            this.InitializeComponent();
            DateTime thisDay = DateTime.Today;
            LogsStartDate.Date = LogsEndDate.Date = LogsStartDate.MaxDate = LogsEndDate.MaxDate = thisDay;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
        
        private void SaveItem(object sender, RoutedEventArgs e)
        {

        }
        private void RefreshPage(object sender, RoutedEventArgs e)
        {
            //ReadLogs();
            //PopulateLogs();
            ////if (PageSizeStatesGroup.CurrentState == NarrowState)
            //{
            //    VisualStateManager.GoToState(this, MasterState.Name, true);
            //}
            //else
            //{
            //    VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
            //}
        }
        private void NewMiscellaneousPage(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(AddNewMiscellaneous), Contacts);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (roamingSettings.Values.ContainsKey("directReferalCount"))
                DirectReferalCountTextbox.Text = roamingSettings.Values["directReferalCount"].ToString();
            else
                DirectReferalCountTextbox.Text = "0";

            if (roamingSettings.Values.ContainsKey("systemCount"))
                SystemCountTextbox.Text = roamingSettings.Values["systemCount"].ToString();
            else
                SystemCountTextbox.Text = "0";
            if (roamingSettings.Values.ContainsKey("uvCount"))
                UVCounterTextbox.Text = roamingSettings.Values["uvCount"].ToString();
            else
                UVCounterTextbox.Text = "0";
        }

        public void NotifyUser(string strMessage, NotifyType type)
        {

        }

        private void ImportAllContacts(object sender, RoutedEventArgs e)
        {

        }

        public async void UploadContacts(object sender, RoutedEventArgs e)
        {
            //Utils.ReadContacts();
            Button button = sender as Button;
            if (button.Content.Equals("Yes"))
            {
                Utils.Contacts.Clear();
            }
            if (button.Content.Equals("No"))
            {
                Utils.ReadContacts();
            }
            FileOpenPicker opener = new FileOpenPicker();
            opener.ViewMode = PickerViewMode.Thumbnail;

            opener.FileTypeFilter.Add(".csv");
            opener.FileTypeFilter.Add(".txt");
            StorageFile file = await opener.PickSingleFileAsync();
            //ProcessProgressRing.IsEnabled = true;

            //await Task.Delay(1000);
            StorageFolder appFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            await file.CopyAsync(appFolder, file.Name.ToString(), NameCollisionOption.ReplaceExisting);
            string tempFile = appFolder.Path.ToString() + "\\" + file.Name.ToString();
            bool exists = File.Exists(tempFile);

            string name = file.Name.ToString();
            StorageFile csvFile = await appFolder.GetFileAsync(name);
            bool isExists = File.Exists(csvFile.Path.ToString());

            try
            {
                if (await appFolder.GetFileAsync(name) != null)
                {
                    int fileCount = 0;
                    using (StreamReader rd = File.OpenText(tempFile))
                    {
                        while (!rd.EndOfStream)
                        {
                            Model.Contact tempContact = new Model.Contact();
                            var splits = rd.ReadLine().Split('|');
                            //for(int i= 0; i < 7; i++)
                            //{
                            //    if (splits[i].ToString().Trim().Equals(String.Empty))
                            //        tempContact.Name = splits[i].ToString();
                            //}
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
                            AddContact(tempContact as Model.Contact);
                            fileCount++;
                        }

                        if (fileCount > 0)
                        {
                            //StatusBlock.Text = "Imported " + fileCount.ToString() + " contacts successfully!!! \nTotal contacts in NameList is " + Utils.Contacts.Count;
                            rootPage.NotifyUser("Imported " + fileCount.ToString() + " contacts successfully!!! \nTotal contacts in NameList is " + Utils.Contacts.Count, NotifyType.StatusMessage); // CRASHING IN WINDOWS MODE
                        }
                    }
                }
                Utils.SaveContacts();
                File.Delete(tempFile);
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
                //return Data;//(ObservableCollection<T>)null;
            }
            
        }
        public async void DownloadContacts(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Pipe Separated File", new List<string>() { ".csv" });
            savePicker.FileTypeChoices.Add("Text File", new List<string>() { ".txt" });
            string fileName = "NameList" + DateTime.Now.ToString("-dd-MMMM-yyyy-hh:mm:ss") + ".csv";
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = fileName;

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                Windows.Storage.CachedFileManager.DeferUpdates(file);


                Utils.ReadContacts();
                //StorageFolder storageFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                //StorageFolder finalStorageFolder = KnownFolders.DocumentsLibrary;

                ////StorageFile csvFile = await appFolder.CreateFileAsync(name);
                //StorageFile file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                StringBuilder tempString = new StringBuilder();
                try
                {
                    foreach (Model.Contact contact in Utils.Contacts)
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
                        //tempString.Append("\n");
                        .AppendLine();
                    }
                }
                catch (Exception ex)
                {
                    String ExceptionMessage = ex.Message;
                }


                // write to file
                await Windows.Storage.FileIO.WriteTextAsync(file, tempString.ToString());
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    //this.StatusBlock.Text = "Name List has been exported to " + file.Name.ToString() + " successfully!!!";
                    rootPage.NotifyUser("Name List has been exported to " + file.Name.ToString() + " successfully!!!", NotifyType.StatusMessage);
                }
                else
                {
                    //this.StatusBlock.Text = "File " + file.Name + " couldn't be saved.";
                    rootPage.NotifyUser("File " + file.Name + " couldn't be saved.", NotifyType.StatusMessage);
                }
            }
            else
            {
                //this.StatusBlock.Text = "Operation cancelled.";
                rootPage.NotifyUser("Operation cancelled.", NotifyType.StatusMessage);
            }
            //await FileIO.WriteTextAsync(file, tempString.ToString());
            //await file.CopyAsync(KnownFolders.DocumentsLibrary, file.Name.ToString(), NameCollisionOption.ReplaceExisting);
            //File.Delete(file.Path.ToString());
            //if (File.Exists(file.Path.ToString()))
            //{
            //    StatusBlock.Text = "Name List has been exported to" + file.Name.ToString() + " file under " + KnownFolders.DocumentsLibrary.Path.ToString() + " directory successfully!!!";
            //}
        }

        public void AddContact(Model.Contact sender)
        {
            if (sender != null)
            {
                Utils.Contacts.Add(sender);
            }
        }

        private void AppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private async void BackupButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            ProcessProgressBar.Visibility = Visibility.Visible;
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Zip File", new List<string>() { ".zip" });
            savePicker.FileTypeChoices.Add("Rar File", new List<string>() { ".rar" });
            string backupFileName = "Backup" + DateTime.Now.ToString("-dd-MMMM-yyyy-hh:mm:ss") + ".zip";
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = backupFileName;

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            Utils.Download<SDKTemplate.Model.Contact>(file.Path);
            Utils.Download<SDKTemplate.Model.Dream>(file.Path);
            Utils.Download<SDKTemplate.Model.TeamMember>(file.Path);
            Utils.Download<SDKTemplate.Model.CallLog>(file.Path);

            var tempFolder = Windows.Storage.ApplicationData.Current.TemporaryFolder;
      
            StorageFolder backupFolder = await tempFolder.CreateFolderAsync("DhansSsBackup\\",CreationCollisionOption.ReplaceExisting);

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

            //await file.DeleteAsync(StorageDeleteOption.Default);
            Utils.Archive(backupFolder, file);
            ProcessProgressBar.Visibility = Visibility.Collapsed;
        }
        private async void RestoreButtonTapped(object sender, RoutedEventArgs e)
        {
            ProcessProgressBar.Visibility = Visibility.Visible;
            Button button = sender as Button;
            if (button.Content.Equals("Yes"))
            {
                Utils.Contacts.Clear();
                Utils.Dreams.Clear();
                Utils.TeamMembers.Clear();
                Utils.CallLogs.Clear();
            }
            if (button.Content.Equals("No"))
            {
                Utils.ReadContacts();
                Utils.ReadDreams();
                Utils.ReadTeamMembers();
                Utils.ReadLogs();
            }
            FileOpenPicker opener = new FileOpenPicker();
            opener.ViewMode = PickerViewMode.List;

            opener.FileTypeFilter.Add(".zip");
            opener.FileTypeFilter.Add(".rar");
            StorageFile file = await opener.PickSingleFileAsync();
            //ProcessProgressRing.IsEnabled = true;
           

            //await Task.Delay(1000);
            StorageFolder appFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var tempFolder = Windows.Storage.ApplicationData.Current.TemporaryFolder;

            StorageFolder backupFolder = await tempFolder.CreateFolderAsync("DhansSsBackup", CreationCollisionOption.ReplaceExisting);
            // need to clear contents before archive
            Utils.Unarchive(file, backupFolder);
            await Task.Delay(10000);
            foreach (StorageFile sf in await backupFolder.GetFilesAsync())
            {
                Utils.Upload(sf);
            }
            ProcessProgressBar.Visibility = Visibility.Collapsed;
        }

        private void ResetButtonTapped(object sender, RoutedEventArgs e)
        {
            ProcessProgressBar.Visibility = Visibility.Visible;
            Utils.Contacts.Clear();
            Utils.SaveContacts();
            Utils.Dreams.Clear();
            Utils.SaveDreams();
            Utils.TeamMembers.Clear();
            Utils.SaveTeamMembers();
            Utils.CallLogs.Clear();
            Utils.WriteLogs();
            ResetConfirmationFlyout.Hide();
            ProcessProgressBar.Visibility = Visibility.Collapsed;
        }

        //private void SendViaMessage_Click(object sender, RoutedEventArgs e)
        //{
        //    Windows.ApplicationModel.Contacts.Contact contact = new Windows.ApplicationModel.Contacts.Contact();
        //    Windows.ApplicationModel.Contacts.ContactPhone phoneNo = new Windows.ApplicationModel.Contacts.ContactPhone();
        //    contact.Name = "Mrunal Kanta Muduli";
        //    phoneNo.Number = "9663865026";
        //    contact.Phones.Add(phoneNo);
        //    Populate108Message(DateTime.Today);
        //    Utils.SendMessage(DetailedMessage.Text.ToString(), contact);
        //}
        //private void SendViaMail_Click(object sender, RoutedEventArgs e)
        //{
        //    Populate108Message(DateTime.Today);
        //    Utils.SendMail(DetailedMessage.Text.ToString(), "108 Message for " + DateTime.Today.ToString("dd MMMM yyyy , dddd"));
        //}
        private void Send108MessageButton_Click(object sender, RoutedEventArgs e)
        {
            bool includeCallLogs = false;
            Button button = sender as Button;
            if (button.Content.Equals("Yes"))
            {
                includeCallLogs = true;
            }
            if (button.Content.Equals("No"))
            {
                includeCallLogs = false;
            }

            int reportingMode = 0;
            int reportingCounter = 1;
            string uplinePhoneNo = "9663865026";
            string uplineEmailID = "mrunalkanta4u@gmail.com";

            if (roamingSettings.Values.ContainsKey("reportingMode"))
            {
                reportingMode = (int)roamingSettings.Values["reportingMode"];
            }
            if (roamingSettings.Values.ContainsKey("reportingCounter"))
            {
                reportingCounter = (int)roamingSettings.Values["reportingCounter"];
            }
            if (roamingSettings.Values.ContainsKey("uplinePhoneNo"))
            {
                uplinePhoneNo = roamingSettings.Values["uplinePhoneNo"].ToString();
            }
            if (roamingSettings.Values.ContainsKey("uplineEmailID"))
            {
                uplineEmailID = roamingSettings.Values["uplineEmailID"].ToString();
            }
            string messageContent = Populate108Message(DateTime.Today, reportingCounter, includeCallLogs);
            reportingCounter = reportingCounter + 1;
            if (reportingCounter > 108)
                reportingCounter = 1;
            roamingSettings.Values["reportingCounter"] = reportingCounter;

            if (reportingMode == 1)
            {
                Utils.SendMail(messageContent, "108 Message for " + DateTime.Today.ToString("dd MMMM yyyy , dddd"), uplineEmailID);
            }
            else
            { 
                Windows.ApplicationModel.Contacts.Contact contact = new Windows.ApplicationModel.Contacts.Contact();
                Windows.ApplicationModel.Contacts.ContactPhone phoneNo = new Windows.ApplicationModel.Contacts.ContactPhone();
                contact.Name = "Mrunal Kanta Muduli";
                phoneNo.Number = uplinePhoneNo;
                contact.Phones.Add(phoneNo);
                Utils.SendMessage(messageContent, contact);
            }
        }
        private string Populate108Message(DateTime date, int count, bool callLogsIncluded = false)
        {
            int infoCalls = 0;
            int inviteCalls = 0;
            int totalCalls = 0;
            string details = string.Empty;

            StringBuilder builder = new StringBuilder();

            Utils.ReadLogs();
            foreach (CallLog callLog in Utils.CallLogs)
            {
                if (date.Date.ToString("dd MMMM yyyy , dddd ").Trim().ToLower() == callLog.Date.ToString().Trim().ToLower())
                {
                    totalCalls++;
                    details = details + "Called " + callLog.Name + " for " + callLog.TypeOfCall;
                    if (!(callLog.Remark.Trim().ToString().Equals(string.Empty)))
                    {
                        details = details + "\n" + callLog.Remark;
                    }
                    details = details + "\n";
                    if (callLog.TypeOfCall.ToString().Trim().ToLower().Equals("info"))
                        infoCalls++;
                    if (callLog.TypeOfCall.ToString().Trim().ToLower().Equals("invite"))
                        inviteCalls++;
                }
            }
            
            builder.Append(count + "#108")
            .AppendLine()
            .AppendLine("Info Count :" + infoCalls)
            .AppendLine("Invite Count :" + inviteCalls)
            .AppendLine("Direct Referals :" + DirectReferalCountTextbox.Text)
            .AppendLine("System Count :" + SystemCountTextbox.Text)
            .AppendLine("UVs for the week :" + UVCounterTextbox.Text)
            .AppendLine()
            .AppendLine();
            if (callLogsIncluded)
                builder.AppendLine(details);
            builder.AppendLine()
            .AppendLine(DetailedMessage.Text.ToString());

            //DetailedMessage.Text = String.Empty;
            return builder.ToString();
        }

        private void DirectReferalCountTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            roamingSettings.Values["directReferalCount"] = DirectReferalCountTextbox.Text;
        }

        private void SystemCountTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            roamingSettings.Values["systemCount"] = SystemCountTextbox.Text;
        }

        private void UVCounterTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            roamingSettings.Values["uvCount"] = UVCounterTextbox.Text;
        }

        private void GenerateLogsButton_Click(object sender, RoutedEventArgs e)
        {
            //CallLogDetails.Text = "Call Logs of "+ LogsForDate.Date.Value.Date.ToString("dd MMMM yyyy , dddd \n")
            //+ Utils.PopulateLogsByDate(LogsForDate.Date.Value.Date);
            //popUpLogs.IsOpen = true;
            string tempStr = String.Empty;
            LogsHeader.Text = CallLogDetails.Text = String.Empty;
            if (LogsStartDate.Date.Value == LogsEndDate.Date.Value)
            {
                LogsHeader.Text = "Call Logs of " + LogsStartDate.Date.Value.Date.ToString("dd MMMM yyyy");
                CallLogDetails.Text += Utils.PopulateLogsByDate(LogsStartDate.Date.Value.Date);
                if (CallLogDetails.Text.ToString().Trim().Equals(String.Empty))
                    CallLogDetails.Text += "No calls done.";
            }
            else
            {
                LogsHeader.Text = "Call Logs between " + LogsStartDate.Date.Value.Date.ToString("dd MMMM yyyy & ") 
                + LogsEndDate.Date.Value.Date.ToString("dd MMMM yyyy");
                for(DateTime dt = LogsStartDate.Date.Value.Date; dt <= LogsEndDate.Date.Value.Date; dt = dt.AddDays(1))
                {
                    CallLogDetails.Text += (dt.ToString("dd MMMM yyyy , dddd : \n\n"));
                    tempStr = Utils.PopulateLogsByDate(dt);
                    if(!tempStr.ToString().Trim().Equals(String.Empty))
                        CallLogDetails.Text += tempStr;
                    else
                        CallLogDetails.Text += "No calls done.";
                    CallLogDetails.Text += "\n\n";
                }
            }


            popUpLogs.IsOpen = true;

            // Getting system selected colour
            var color = Application.Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush;
            //popUpLogspanel.BorderBrush = color;
            //(Brush)Application.Current.Resources["PhoneAccentColor"];
            LogsHeader.Foreground = color;
            //if (!UpdateEmailPopup.IsOpen)
            //{
            //    RootPopupBorderEmailID.Width = 200;
            //    RootPopupBorderEmailID.Height = 100;
            //    EmailToUpdate.Text = tb.Text;
            //    UpdateEmailPopup.HorizontalOffset = tb.BaselineOffset; //Window.Current.Bounds.Width - 300;
            //    UpdateEmailPopup.VerticalOffset = tb.BaselineOffset; //Window.Current.Bounds.Height - 200;
            //    UpdateEmailPopup.IsOpen = true;
            //}
        }

        private void OnExitTapped(object sender, TappedRoutedEventArgs e)
        {
            CallLogDetails.Text = String.Empty;
            popUpLogs.IsOpen = false;
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            DataPackage myPackage = new DataPackage();
            myPackage.SetText(CallLogDetails.Text.ToString());
            Clipboard.SetContent(myPackage);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            int reportingMode = 0;
            string uplinePhoneNo = "9663865026";
            string uplineEmailID = "mrunalkanta4u@gmail.com";
            if (roamingSettings.Values.ContainsKey("reportingMode"))
            {
                reportingMode = (int)roamingSettings.Values["reportingMode"];
            }
            if (roamingSettings.Values.ContainsKey("uplinePhoneNo"))
            {
                uplinePhoneNo = roamingSettings.Values["uplinePhoneNo"].ToString();
            }
            if (roamingSettings.Values.ContainsKey("uplineEmailID"))
            {
                uplineEmailID = roamingSettings.Values["uplineEmailID"].ToString();
            }

            if (reportingMode == 1)
            {
                Utils.SendMail(CallLogDetails.Text.ToString(), "108 Message for " + DateTime.Today.ToString("dd MMMM yyyy , dddd"), uplineEmailID);
            }
            else
            {
                Windows.ApplicationModel.Contacts.Contact contact = new Windows.ApplicationModel.Contacts.Contact();
                Windows.ApplicationModel.Contacts.ContactPhone phoneNo = new Windows.ApplicationModel.Contacts.ContactPhone();
                contact.Name = "Mrunal Kanta Muduli";
                phoneNo.Number = uplinePhoneNo;
                contact.Phones.Add(phoneNo);
                Utils.SendMessage(CallLogDetails.Text.ToString(), contact);
            }
        }

        private void LogsStartDate_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            LogsEndDate.MinDate = LogsStartDate.Date.Value;
        }

        private void LogsEndDate_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            LogsStartDate.MaxDate = LogsEndDate.Date.Value;
        }

        private void PurgeFrequencyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AutoPurgeButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
