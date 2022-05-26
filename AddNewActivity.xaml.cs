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

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using Windows.UI.Xaml;
using SDKTemplate.Model;
using DataAccounts;
using Windows.UI.Xaml.Media.Animation;
using System.Linq;

namespace SDKTemplate
{

    public sealed partial class AddNewActivity : Page
    {
        private MainPage rootPage = MainPage.Current;
        //private bool isLaunched = false;
        //private static ObservableCollection<Contact> Contacts = new ObservableCollection<Contact>();
        //private static ObservableCollection<CallLog> CallLogs = new ObservableCollection<CallLog>();
        ////Utils utility;
        public AddNewActivity()
        {
            this.InitializeComponent();
            //this.ProspectNameAutoSuggestBox.KeyDown += new EventHandler(this.asugbox_SuggestionChosen);
            //this.ProspectNameAutoSuggestBox.KeyUp += new EventHandler(asugbox_SuggestionChosen);
            DateTime thisDay = DateTime.Today;
            DateOfCall.Date = DateOfCall.MaxDate = thisDay;
        }

        //public async void SaveContacts()
        //{        
            
        //    StorageFile userdetailsfile = await ApplicationData.Current.LocalFolder.CreateFileAsync
        //    ("NameList", CreationCollisionOption.OpenIfExists);
        //    IRandomAccessStream raStream = await userdetailsfile.OpenAsync(FileAccessMode.ReadWrite);

        //    using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
        //    {
        //        // Serialize the Session State. 
        //        DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<Model.Contact>));
        //        serializer.WriteObject(outStream.AsStreamForWrite(), Contacts);
        //        await outStream.FlushAsync();
        //        outStream.Dispose();
        //        raStream.Dispose();
        //    }
        //}
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //ObservableCollection<Contact> ProspectList = e.Parameter as ObservableCollection<Contact>;
            //ProspectList.OrderByDescending(a => a.Name);
            //ProspectNameComboBox.Items.OrderByDescending(a => a.ToString());
            //LoadNamesAsync(ProspectNameComboBox, ProspectList);
            //LoadNamesAsync(ProspectNameComboBox, e.Parameter as ObservableCollection<Contact>);
            //ProspectNameComboBox.SelectedIndex = 0;
            //TextBox searchNames = new TextBox();            
            //ProspectNameComboBox.Items.Add(searchNames);            
        }
        // Fill the specified ComboBox with user Names.
        //public void LoadNamesAsync(ComboBox comboBox, ObservableCollection<Contact> Contacts)
        //{
        //    Utils.ReadContacts();


        //    if (Contacts != null)
        //    {
        //        //comboBox.DataContext = new ObservableCollection<Contact>(Contacts);
        //        comboBox.ItemsSource = Contacts;
        //        if (Contacts.Count > 0)
        //        {
        //            comboBox.SelectedIndex = 0;
        //        }
        //    }
        //    //else
        //    //{
        //    //    // If the store is null, that means all access to Contacts, Calendar and Email data 
        //    //    // has been revoked.
        //    //    NotifyUser("Access to Contacts, Calendar and Email has been revoked", NotifyType.ErrorMessage);
        //    //}
        //}
        //public async void ReadContacts()
        //{
        //    var Serializer = new DataContractSerializer(typeof(ObservableCollection<Contact>));

        //    if (await ApplicationData.Current.LocalFolder.TryGetItemAsync("NameList") != null)
        //    {
        //        using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("NameList"))
        //        {
        //            Contacts = (ObservableCollection<Contact>)Serializer.ReadObject(stream);
        //        }
        //    }
        //}
        //public async void ReadContacts()
        //{

        //    var Serializer = new DataContractSerializer(typeof(ObservableCollection<Model.Contact>));

        //    if (await ApplicationData.Current.LocalFolder.TryGetItemAsync("NameList") != null)
        //    {
        //        using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("NameList"))
        //        {
        //            Contacts = (ObservableCollection<Model.Contact>)Serializer.ReadObject(stream);
        //        }

        //    }
        //}
        private void CancelSave(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Activity), "Cancel", new EntranceNavigationTransitionInfo());
            //if (PageSizeStatesGroup.CurrentState == NarrowState)
            //{
            //    VisualStateManager.GoToState(this, MasterState.Name, true);
            //}
            //else
            //{
            //    VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
            //}
        }
        //private void AddContact(string newName, string newPhoneNumber, string newLocation, string newCategory, string newInfo, string newInvite, string newRemark)
        //{
        //    //readContacts();
        //    //Contacts.Add(new Model.Contact { Name = newName, PhoneNumber = newPhoneNumber, Location = newLocation, Category = newCategory, Info = newInfo, Invite = newInvite, Remark = newRemark });
        //    this.Frame.Navigate(typeof(NameList));
        //}
        private void SaveItem(object sender, RoutedEventArgs e)
        {
            string prospectName = getProspectName(ProspectNameAutoSuggestBox.Text.ToString());
            if (prospectName != null)
            {
                ProspectNameAutoSuggestBox.Text = prospectName;
                //Contact SelectedProspect = (Contact)ProspectNameComboBox.SelectedValue;
                //string NameOfSelectedProspect = SelectedProspect.Name.ToString();
                this.Frame.Navigate(typeof(Activity), new Model.CallLog
                {
                    //Name = NameOfSelectedProspect,
                    Name = ProspectNameAutoSuggestBox.Text.ToString(),
                    Date = DateOfCall.Date.Value.ToString("dd MMMM yyyy , dddd "),
                    TypeOfCall = (TypeOfCallComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                    Remark = RemarkTextBox.Text.ToString()
                });            
            }
            else
            {
                ProspectNameAutoSuggestBox.Text = "";
                ProspectNameAutoSuggestBox.PlaceholderText = "No such Prospect exists in NameList !!!";
                return;
            }
        }
        private void ResetPage(object sender, RoutedEventArgs e)
        {
            //if (PageSizeStatesGroup.CurrentState == NarrowState)
            //{
            //    VisualStateManager.GoToState(this, MasterState.Name, true);
            //}
            //else
            //{
            //    VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
            //}
        }
        
        //private async void Fix_Click(object sender, RoutedEventArgs e)
        //{
        //    // only one instance of the UserDataAccountManager pane can be launched at once per thread
        //    //if (isLaunched == false)
        //    //{
        //    //    isLaunched = true;
        //    //    rootPage.NotifyUser("", NotifyType.StatusMessage);

        //    //    var selectedAccount = (UserDataAccount)ProspectNameComboBox.SelectedValue;
        //    //    if (selectedAccount != null)
        //    //    {
        //    //        //if (await IsFixNeededAsync(selectedAccount))
        //    //        //{
        //    //        //    await UserDataAccountManager.ShowAccountErrorResolverAsync(selectedAccount.Id);
        //    //        //}
        //    //        //else
        //    //        //{
        //    //        //    rootPage.NotifyUser("Account is not in an error state", NotifyType.ErrorMessage);
        //    //        //}

        //    //        await rootPage.LoadDataAccountsAsync(ProspectNameComboBox);
        //    //    }

        //    //    isLaunched = false;
        //    //}
        //}

        private void asugbox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (string.IsNullOrEmpty(ProspectNameAutoSuggestBox.Text))
            {
                this.ProspectNameAutoSuggestBox.ItemsSource = Utils.Contacts.OrderBy(o => o.Name).ToList(); ;
            }
            this.ProspectNameAutoSuggestBox.ItemsSource = Utils.Contacts.Where((item) => { return item.Name.ToString().ToLower().Contains(ProspectNameAutoSuggestBox.Text.ToString().ToLower()); });
        }

        private void asugbox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            String prospectName = getProspectName(args.QueryText as string);
            if (prospectName != null)
            {
                ProspectNameAutoSuggestBox.Text = prospectName;
            }
            else
            {
                ProspectNameAutoSuggestBox.Text = "";
                ProspectNameAutoSuggestBox.PlaceholderText = "No such Prospect exists in Name List !!!";
            }
            //foreach (Contact cn in Utils.Contacts)
            //{
            //}
        }

        private string getProspectName(string prospectName)
        {
            var suggestionListFound = Utils.Contacts.Where((item) => { return item.Name.Trim().ToLower().Equals(prospectName.Trim().ToLower()); });
            if (suggestionListFound.FirstOrDefault() != null)
                return suggestionListFound.FirstOrDefault().Name.ToString();
            else
                return null;            
        }

        private void asugbox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Contact cn = args.SelectedItem as Contact;
            ProspectNameAutoSuggestBox.Text = cn.Name;
        }

        private void ProspectNameAutoSuggestBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            //Contact cn = args.SelectedItem as Contact;
            //ProspectNameAutoSuggestBox.Text = cn.Name;
        }

        private void ProspectNameAutoSuggestBox_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            //Contact cn = args.SelectedItem as Contact;
            //ProspectNameAutoSuggestBox.Text = cn.Name;
        }

        /// <summary>
        /// Check the mailboxes, calendars, and teamMember lists for sync errors
        /// </summary>
        /// <param name="selectedAccount"></param>
        /// <returns>True if there is an sync error for email, calendar, or teamMembers. False if nothing is syncing or everything is syncing correctly</returns>
        //private async Task<bool> IsFixNeededAsync(UserDataAccount selectedAccount)
        //{
        //    foreach (EmailMailbox mailbox in await selectedAccount.FindEmailMailboxesAsync())
        //    {
        //        if (mailbox.SyncManager != null)
        //        {
        //            switch (mailbox.SyncManager.Status)
        //            {
        //                case EmailMailboxSyncStatus.AuthenticationError:
        //                case EmailMailboxSyncStatus.PolicyError:
        //                case EmailMailboxSyncStatus.UnknownError:
        //                    return true;
        //            }
        //        }
        //    }

        //    foreach (AppointmentCalendar calendar in await selectedAccount.FindAppointmentCalendarsAsync())
        //    {
        //        if (calendar.SyncManager != null)
        //        {
        //            switch (calendar.SyncManager.Status)
        //            {
        //                case AppointmentCalendarSyncStatus.AuthenticationError:
        //                case AppointmentCalendarSyncStatus.PolicyError:
        //                case AppointmentCalendarSyncStatus.UnknownError:
        //                    return true;
        //            }
        //        }
        //    }

        //    foreach (ContactList contactList in await selectedAccount.FindContactListsAsync())
        //    {
        //        if (contactList.SyncManager != null)
        //        {
        //            switch (contactList.SyncManager.Status)
        //            {
        //                case ContactListSyncStatus.AuthenticationError:
        //                case ContactListSyncStatus.PolicyError:
        //                case ContactListSyncStatus.UnknownError:
        //                    return true;
        //            }
        //        }
        //    }

        //    return false;
        //}
    }
}
