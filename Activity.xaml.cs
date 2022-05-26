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
using Windows.UI.Xaml.Markup;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.ViewManagement;
using System.Globalization;
//using System.Windows.Controls;

namespace SDKTemplate
{

    public sealed partial class Activity : Page
    {
        private MainPage rootPage = MainPage.Current;
        ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

        Contact ProspectToUpdate;
        ScrollViewer scrV = new ScrollViewer();
        ScrollViewer scrV1 = new ScrollViewer();
        ScrollViewer scrV2 = new ScrollViewer();
        ScrollViewer scrV3 = new ScrollViewer();
        ScrollViewer scrV4 = new ScrollViewer();
        ScrollViewer scrV5 = new ScrollViewer();
        ScrollViewer scrV6 = new ScrollViewer();
        ScrollViewer scrV7 = new ScrollViewer();
        TextBlock tBlk = new TextBlock();
        TextBlock tBlk1 = new TextBlock();
        TextBlock tBlk2 = new TextBlock();
        TextBlock tBlk3 = new TextBlock();
        TextBlock tBlk4 = new TextBlock();
        TextBlock tBlk5 = new TextBlock();
        TextBlock tBlk6 = new TextBlock();
        TextBlock tBlk7 = new TextBlock();
        
        //var sec = MyHub.Sections[2];
        //var btn = sec.FindVisualChild("MyButton") as Button;

        public Activity()
        {
            this.InitializeComponent();           
            this.Loaded += OnLoaded;
            this.Loaded += HubPage_Loaded;
            //this.UpdateLayout();
            //// Give your hub a name using x:Name=
            //var item = MyHubSection; // Retrieve your hubsection here!
            ////var container = this.MyHub.ContainerFromItem(item);
            //// NPE safety, deny first
            //var container = MyHub as Hub;
            //if (container == null)
            //    return;
            //var datalayer = FindElementByName<StackPanel>(MyHub, "callLogDisplayPage");
            //// And again deny if we got null
            //if (datalayer == null)
            //return;
            //var myXaml = "<TextBlock x:Name=\"tBlk\">The most recent news goes here<TextBlock/>";
            //var template = XamlReader.Load("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">" + myXaml + "</DataTemplate>");
            //MyHubSection.ContentTemplate = template as DataTemplate;
        }

        public T FindElementByName<T>(DependencyObject element, string sChildName) where T : FrameworkElement
        {
            T childElement = null;
            var nChildCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < nChildCount; i++)
            {
                FrameworkElement child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;

                if (child == null)
                    continue;

                if (child is T && child.Name.Equals(sChildName))
                {
                    childElement = (T)child;
                    break;
                }

                childElement = FindElementByName<T>(child, sChildName);

                if (childElement != null)
                    break;
            }
            return childElement;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Utils.ReadContacts();
            //Utils.ReadLogs();
            //PopulateLogs();
        }
        //public async void ReadLogs()
        //{
        //    var Serializer = new DataContractSerializer(typeof(ObservableCollection<CallLog>));

        //    if (await ApplicationData.Current.LocalFolder.TryGetItemAsync("CallLog") != null)
        //    {
        //        using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("CallLog"))
        //        {
        //            Utils.CallLogs = (ObservableCollection<CallLog>)Serializer.ReadObject(stream);
        //        }

        //    }
        //}

        //public async void WriteLogs()
        //{

        //    //StorageFile userdetailsfile = await ApplicationData.Current.LocalFolder.CreateFileAsync
        //    //("NameList", CreationCollisionOption.OpenIfExists);
        //    StorageFile userdetailsfile = await ApplicationData.Current.LocalFolder.CreateFileAsync
        //    ("CallLog", CreationCollisionOption.OpenIfExists);
        //    IRandomAccessStream raStream = await userdetailsfile.OpenAsync(FileAccessMode.ReadWrite);

        //    using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
        //    {
        //        // Serialize the Session State. 
        //        DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<Model.CallLog>));
        //        serializer.WriteObject(outStream.AsStreamForWrite(), Utils.CallLogs);
        //        await outStream.FlushAsync();
        //        outStream.Dispose();
        //        raStream.Dispose();
        //    }
        //}
        //private void Page_Loaded(object sender, RoutedEventArgs e)
        //{
        //    Utils.ReadContacts();
        //    ////Utils.ReadLogs();
        //    PopulateLogs();
        //}
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //await rootPage.LoadDataAccountsAsync(ProspectNameComboBox);
            if (e.Parameter as CallLog != null)
            {
                CallLog callLog = e.Parameter as CallLog;
                AddLog(callLog);
                Utils.WriteLogs();

                if (callLog.TypeOfCall.Trim().ToLower().Equals("info"))
                {
                    ProspectToUpdate = Utils.Contacts.Where((item) => { return item.Name.Trim().ToLower().Equals(callLog.Name.Trim().ToLower()); }).FirstOrDefault();
                    ProspectToUpdate.Info= DateTime.Today.ToString("dd MMMM yyyy , dddd ");
                    Utils.SaveContacts();
                }
                if (callLog.TypeOfCall.Trim().ToLower().Equals("invite"))
                {
                    ProspectToUpdate = Utils.Contacts.Where((item) => { return item.Name.Trim().ToLower().Equals(callLog.Name.Trim().ToLower()); }).FirstOrDefault();
                    ProspectToUpdate.Invite = DateTime.Today.ToString("dd MMMM yyyy , dddd ");
                    Utils.SaveContacts();
                }
            }

            if (Utils.CallLogs != null)
            {
                scrV1.Height = scrV2.Height = scrV3.Height = scrV4.Height = scrV5.Height = scrV6.Height = scrV7.Height = 350;
                scrV1.VerticalScrollBarVisibility =
                scrV2.VerticalScrollBarVisibility =
                scrV3.VerticalScrollBarVisibility =
                scrV4.VerticalScrollBarVisibility =
                scrV5.VerticalScrollBarVisibility =
                scrV6.VerticalScrollBarVisibility =
                scrV7.VerticalScrollBarVisibility =
                    ScrollBarVisibility.Hidden;
                tBlk1.TextWrapping =
                tBlk2.TextWrapping =
                tBlk3.TextWrapping =
                tBlk4.TextWrapping =
                tBlk5.TextWrapping =
                tBlk6.TextWrapping =
                tBlk7.TextWrapping =
                    TextWrapping.Wrap;

                tBlk1.Text = Utils.PopulateLogsByDate(DateTime.Today);
                scrV1.Content = tBlk1;
                tBlk2.Text = Utils.PopulateLogsByDate(DateTime.Today.AddDays(-1));
                scrV2.Content = tBlk2;
                tBlk3.Text = Utils.PopulateLogsByDate(DateTime.Today.AddDays(-2));
                scrV3.Content = tBlk3;
                tBlk4.Text = Utils.PopulateLogsByDate(DateTime.Today.AddDays(-3));
                scrV4.Content = tBlk4;
                tBlk5.Text = Utils.PopulateLogsByDate(DateTime.Today.AddDays(-4));
                scrV5.Content = tBlk5;
                tBlk6.Text = Utils.PopulateLogsByDate(DateTime.Today.AddDays(-5));
                scrV6.Content = tBlk6;
                tBlk7.Text = Utils.PopulateLogsByDate(DateTime.Today.AddDays(-6));
                scrV7.Content = tBlk7;

            }
            base.OnNavigatedTo(e);
        }
        private void AddLog(CallLog sender)
        {
            if (sender != null)
            {
                Utils.CallLogs.Add(sender);
            }
            //SaveContacts();
        }

        private void SaveItem(object sender, RoutedEventArgs e)
        {
            
        }
        private void RefreshPage(object sender, RoutedEventArgs e)
        {          
            
            tBlk.Text = Utils.PopulateLogsByDate(DateTime.Today); 
            scrV.Content = tBlk;
        }

        public string PopulateLogs()
        {
            Utils.ReadLogs();

            string details = string.Empty;
            int NumberOfCalls = 0;
            foreach (CallLog callLog in Utils.CallLogs)
            {
                NumberOfCalls++;
                details = details + "[ " + NumberOfCalls.ToString() + " ] Called " + callLog.Name + " for " + callLog.TypeOfCall + " on " + callLog.Date + ".";
                if (!(callLog.Remark.Trim().ToString().Equals(string.Empty)))
                {
                    details = details + "\n" + callLog.Remark;
                }
                details = details + "\n\n";
            }

            scrV.Height = 350;
            scrV.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            tBlk.TextWrapping = TextWrapping.Wrap;
            //tBlk.Text = details;
            //scrV.Content = tBlk;

            return details;
        }
        //public string PopulateLogsByDate(DateTime date)
        //{
        //    Utils.ReadLogs();

        //    string details = string.Empty;
        //    int NumberOfCalls = 0;
        //    foreach (CallLog callLog in Utils.CallLogs)
        //    {
        //        if (date.Date.ToString("dd MMMM yyyy , dddd ").Trim().ToLower() == callLog.Date.ToString().Trim().ToLower())
        //        {
        //            NumberOfCalls++;
        //            details = details + "[ " + NumberOfCalls.ToString() + " ] Called " + callLog.Name + " for " + callLog.TypeOfCall;
        //            if (!(callLog.Remark.Trim().ToString().Equals(string.Empty)))
        //            {
        //                details = details + "\n" + callLog.Remark;
        //            }
        //            details = details + "\n\n";
        //        }
        //    }

        //    //scrV.Height = 350;
        //    //scrV.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        //    //tBlk.TextWrapping = TextWrapping.Wrap;
        //    //tBlk.Text = details;
        //    //scrV.Content = tBlk;

        //    return details;
        //}
        private void NewActivityPage(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddNewActivity), Utils.Contacts);
        }

        public void NotifyUser(string strMessage, NotifyType type)
        {
            //switch (type)
            //{
            //    case NotifyType.StatusMessage:
            //        StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
            //        break;
            //    case NotifyType.ErrorMessage:
            //        StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
            //        break;
            //}
            //StatusBlock.Text = strMessage;

            //// Collapse the StatusBlock if it has no text to conserve real estate.
            //StatusBorder.Visibility = (StatusBlock.Text != String.Empty) ? Visibility.Visible : Visibility.Collapsed;
            //if (StatusBlock.Text != String.Empty)
            //{
            //    StatusBorder.Visibility = Visibility.Visible;
            //    StatusPanel.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    StatusBorder.Visibility = Visibility.Collapsed;
            //    StatusPanel.Visibility = Visibility.Collapsed;
            //}
        }

        private void SendMail(object sender, RoutedEventArgs e)
        {
            Utils.SendMail(Utils.PopulateLogsByDate(DateTime.Today).ToString());
        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.Contacts.Contact contact = new Windows.ApplicationModel.Contacts.Contact();
            Windows.ApplicationModel.Contacts.ContactPhone phoneNo= new Windows.ApplicationModel.Contacts.ContactPhone();
            contact.Name = "Mrunal Kanta Muduli";
            phoneNo.Number = "9663865026";
            contact.Phones.Add(phoneNo);
            Utils.SendMessage(Utils.PopulateLogsByDate(DateTime.Today).ToString(), contact);
        }

        private void ResetPage(object sender, RoutedEventArgs e)
        {
            Utils.CallLogs.Clear();
            Utils.WriteLogs();
            tBlk.Text = "";
            //PopulateLogs();
            tBlk.Text = Utils.PopulateLogsByDate(DateTime.Today);
            scrV.Content = tBlk;
            ClrConfirmationFlyout.Hide();
        }

        private void callLogDisplayPanel1Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel callLogDisplayPage1 =  sender as StackPanel;
            callLogDisplayPage1.Children.Add(scrV1);
            //DateTime.Today.DayOfWeek.ToString() + " , " +
        }
        private void callLogDisplayPanel2Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel callLogDisplayPage2 = sender as StackPanel;
            callLogDisplayPage2.Children.Add(scrV2);
        }
        private void callLogDisplayPanel3Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel callLogDisplayPage3 = sender as StackPanel;
            callLogDisplayPage3.Children.Add(scrV3);
        }
        private void callLogDisplayPanel4Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel callLogDisplayPage4 = sender as StackPanel;
            callLogDisplayPage4.Children.Add(scrV4);
        }
        private void callLogDisplayPanel5Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel callLogDisplayPage5 = sender as StackPanel;
            callLogDisplayPage5.Children.Add(scrV5);
        }
        private void callLogDisplayPanel6Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel callLogDisplayPage6 = sender as StackPanel;
            callLogDisplayPage6.Children.Add(scrV6);
        }
        private void callLogDisplayPanel7Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel callLogDisplayPage7 = sender as StackPanel;
            callLogDisplayPage7.Children.Add(scrV7);
        }
        void HubPage_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> sections = new List<string>();
            MyHubSection1.Header = DateTime.Today.ToString("dd MMMM yyyy , dddd ");
            MyHubSection2.Header = DateTime.Today.AddDays(-1).ToString("dd MMMM yyyy , dddd ");
            MyHubSection3.Header = DateTime.Today.AddDays(-2).ToString("dd MMMM yyyy , dddd ");
            MyHubSection4.Header = DateTime.Today.AddDays(-3).ToString("dd MMMM yyyy , dddd ");
            MyHubSection5.Header = DateTime.Today.AddDays(-4).ToString("dd MMMM yyyy , dddd ");
            MyHubSection6.Header = DateTime.Today.AddDays(-5).ToString("dd MMMM yyyy , dddd ");
            MyHubSection7.Header = DateTime.Today.AddDays(-6).ToString("dd MMMM yyyy , dddd ");
            //MyHubSection1.Foreground = new Brush(UISettings.GetColorValue(UIColorType.AccentDark3));
            foreach (HubSection section in MyHub.Sections)
            {
                if (section.Header != null)
                {
                    sections.Add(section.Header.ToString());
                }
            }
            ZoomedOutList.ItemsSource = sections;
        }

        private void PurgeLogs(object sender, RoutedEventArgs e)
        {
            ProcessProgressBar.Visibility = Visibility.Visible;
            PurgeConfirmationFlyout.Hide();
             int purgeFrequency = 1;
            DateTime purgeDate = DateTime.Now.Date;
            if (roamingSettings.Values.ContainsKey("autoPurgeFrequency"))
            {
                purgeFrequency = (int)roamingSettings.Values["autoPurgeFrequency"];
            }
            switch (purgeFrequency)
            {
                case 0:
                    DeleteLogs(purgeDate.AddMonths(-1));
                    break;
                case 1:
                    DeleteLogs(purgeDate.AddMonths(-6));
                    break;
                case 2:
                    DeleteLogs(purgeDate.AddYears(-1));
                    break;
                default:
                    DeleteLogs(purgeDate.AddMonths(-6));
                    break;
            }
            ProcessProgressBar.Visibility = Visibility.Collapsed;
        }

        private void DeleteLogs(DateTime purgeDate)
        {
            Utils.ReadLogs();
            ObservableCollection<SDKTemplate.Model.CallLog> tempCallLogs = new ObservableCollection<SDKTemplate.Model.CallLog>(Utils.CallLogs);
            //ObservableCollection<SDKTemplate.Model.CallLog> tempCallLogs = new ObservableCollection<SDKTemplate.Model.CallLog>();
            try
            {
                foreach (CallLog callLog in Utils.CallLogs)
                {
                    if (purgeDate >= DateTime.ParseExact(callLog.Date, "dd MMMM yyyy , dddd ", CultureInfo.InvariantCulture))
                    {
                        //tempCallLogs.RemoveAt(Utils.CallLogs.IndexOf(callLog));    
                        tempCallLogs.Remove(callLog);
                    }
                }
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
            }
            Utils.CallLogs = tempCallLogs;
            Utils.WriteLogs();
        }
    }
}
