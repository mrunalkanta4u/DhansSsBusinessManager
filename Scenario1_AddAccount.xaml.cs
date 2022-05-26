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

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.UserDataAccounts;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.ApplicationModel.Email;
using SDKTemplate.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml.Media.Animation;
using Windows.Data.Json;
using Windows.Storage;
using System.Linq;
using Windows.UI.Xaml.Media;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using System.Runtime.Serialization;
using System.Text;
using System.IO;
using DataAccounts;

namespace SDKTemplate
{
    public sealed partial class NameList : Page
    {
        private MainPage rootPage = MainPage.Current;
        private bool isLaunched;
        private Contact selectedContact;

        //private static ObservableCollection<Contact> Contacts = new ObservableCollection<Contact>();

        public NameList()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }
        //public async void SaveContacts()
        //{
        //    List<Contact> Contacts = new List<Contact>();
        //    Contacts.Add(new Contact { Name = "Mrunal", PhoneNumber = "966387686", Location = "banglore", Category = "hot", Info = "info given", Invite = "invite given", Remark = "he is my school frnd" });

        //    StorageFile userdetailsfile = await ApplicationData.Current.LocalFolder.CreateFileAsync
        //    ("MyList", CreationCollisionOption.ReplaceExisting);
        //    IRandomAccessStream raStream = await userdetailsfile.OpenAsync(FileAccessMode.ReadWrite);

        //    using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
        //    {
        //        // Serialize the Session State. 
        //        DataContractSerializer serializer = new DataContractSerializer(typeof(List<Contact>));
        //        serializer.WriteObject(outStream.AsStreamForWrite(), Contacts);
        //        await outStream.FlushAsync();
        //        outStream.Dispose();   
        //        raStream.Dispose();
        //    }
        //}
        //public async void ReadContacts()
        //{
        //    try
        //    {
        //        var Serializer = new DataContractSerializer(typeof(ObservableCollection<Contact>));

        //        if (await ApplicationData.Current.LocalFolder.TryGetItemAsync("NameList") != null)
        //        {
        //            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("NameList"))
        //            {
        //                Contacts = (ObservableCollection<Contact>)Serializer.ReadObject(stream);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        String ExceptionMessage = ex.Message;
        //    }
        //}

        //public async void SaveContacts()
        //{
        //    try
        //    {
        //        //StorageFile userdetailsfile = await ApplicationData.Current.LocalFolder.CreateFileAsync
        //        //("NameList", CreationCollisionOption.OpenIfExists);
        //        StorageFile userdetailsfile = await ApplicationData.Current.LocalFolder.CreateFileAsync
        //        ("NameList", CreationCollisionOption.OpenIfExists);
        //        IRandomAccessStream raStream = await userdetailsfile.OpenAsync(FileAccessMode.ReadWrite);

        //        using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
        //        {
        //            // Serialize the Session State. 
        //            DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<Model.Contact>));
        //            serializer.WriteObject(outStream.AsStreamForWrite(), Contacts);
        //            await outStream.FlushAsync();
        //            outStream.Dispose();
        //            raStream.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        String ExceptionMessage = ex.Message;
        //    }
        //}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Details view can remove an item from the list.
            if (e.Parameter as string == "Delete")
            {
                DeleteContact(selectedContact);
                Utils.SaveContacts();
            }
            //if (e.Parameter as string == "DeleteItems")
            //{
            //    //DeleteContact(selectedContact);
            //    SaveContacts();
            //}
            
            if (e.Parameter as Contact != null)
            {
                AddContact(e.Parameter as Contact);
                Utils.SaveContacts();
                
            }
            base.OnNavigatedTo(e);
            //readContacts();
            if (Utils.Contacts != null && Utils.Contacts.Count > 0)
            {
                MasterListView.ItemsSource = Utils.Contacts;
                MasterListView.SelectedIndex = 0;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Utils.ReadContacts();
            if (Utils.Contacts != null)
            {
                if (selectedContact == null && Utils.Contacts.Count > 0)
                {
                    selectedContact = Utils.Contacts[0];
                    MasterListView.SelectedIndex = 0;
                }
                else
                {
                    // Details view is collapsed, in case there is not items.
                    DetailContentPresenter.Visibility = Visibility.Collapsed;
                    selectedContact = null;
                }
            }
            // If the app starts in narrow mode - showing only the Master listView - 
            // it is necessary to set the commands and the selection mode.
            if (PageSizeStatesGroup.CurrentState == NarrowState)
            {
                VisualStateManager.GoToState(this, MasterState.Name, true);
            }
            else if (PageSizeStatesGroup.CurrentState == WideState)
            {
                // In this case, the app starts is wide mode, Master/Details view, 
                // so it is necessary to set the commands and the selection mode.
                VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
                MasterListView.SelectionMode = ListViewSelectionMode.Extended;
                MasterListView.SelectedItem = selectedContact;
            }
            else
            {
                new InvalidOperationException();
            }
        }
        private void OnCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            bool isNarrow = e.NewState == NarrowState;
            if (isNarrow)
            {
                Frame.Navigate(typeof(DetailsPage), selectedContact, new SuppressNavigationTransitionInfo());
            }
            else
            {
                VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
                MasterListView.SelectionMode = ListViewSelectionMode.Extended;
                MasterListView.SelectedItem = selectedContact;
            }

            EntranceNavigationTransitionInfo.SetIsTargetElement(MasterListView, isNarrow);
            if (DetailContentPresenter != null)
            {
                EntranceNavigationTransitionInfo.SetIsTargetElement(DetailContentPresenter, !isNarrow);
            }
        }
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PageSizeStatesGroup.CurrentState == WideState)
            {
                if (MasterListView.SelectedItems.Count == 1)
                {
                    selectedContact = MasterListView.SelectedItem as Contact;
                    EnableContentTransitions();
                }
                // Entering in Extended selection
                else if (MasterListView.SelectedItems.Count > 1
                     && MasterDetailsStatesGroup.CurrentState == MasterDetailsState)
                {
                    VisualStateManager.GoToState(this, ExtendedSelectionState.Name, true);
                }
            }
            // Exiting Extended selection
            if (MasterDetailsStatesGroup.CurrentState == ExtendedSelectionState &&
                MasterListView.SelectedItems.Count == 1)
            {
                VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
            }
        }
        // ItemClick event only happens when user is a Master state. In this state, 
        // selection mode is none and click event navigates to the details view.
        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            // The clicked item it is the new selected teamMember
            selectedContact = e.ClickedItem as Contact;
            if (PageSizeStatesGroup.CurrentState == NarrowState)
            {
                // Go to the details page and display the item 
                Frame.Navigate(typeof(DetailsPage), selectedContact, new DrillInNavigationTransitionInfo());
            }
            //else
            {
                // Play a refresh animation when the user switches detail items.
                //EnableContentTransitions();
            }
        }
        private void EnableContentTransitions()
        {
            DetailContentPresenter.ContentTransitions.Clear();
            DetailContentPresenter.ContentTransitions.Add(new EntranceThemeTransition());
        }
        #region Commands
        private void AddItem(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddNewContact));
        }
        private void AddContact(Contact sender)
        {
            if (sender != null)
            {
                Utils.Contacts.Add(sender);
            }
            //SaveContacts();
        }
        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (selectedContact != null)
            {
                DeleteContact(selectedContact);
                //SaveContacts();
                if (MasterListView.Items.Count > 0)
                {
                    MasterListView.SelectedIndex = 0;
                    selectedContact = MasterListView.SelectedItem as Contact;
                }
                else
                {
                    // Details view is collapsed, in case there is not items.
                    DetailContentPresenter.Visibility = Visibility.Collapsed;
                    selectedContact = null;
                }
            }
        }

        private void DeleteContact(Contact itemToDelete)
        {
            try
            {
                //Contact itemToDelete = (Contact)selectedContact;
                Contact item = Utils.Contacts.Where(x => x.Name == itemToDelete.Name).FirstOrDefault();
                if (item != null)
                {
                    Utils.Contacts.RemoveAt(Utils.Contacts.IndexOf(item));

                }
                //SaveContacts();
            }
            catch (Exception ex) { String msg = ex.Message; };

        }

        private void DeleteItems(object sender, RoutedEventArgs e)
        {
            if (MasterListView.SelectedIndex != -1)
            {
                List<Contact> selectedItems = new List<Contact>();
                foreach (Contact teamMember in MasterListView.SelectedItems)
                {
                    selectedItems.Add(teamMember);
                }
                foreach (Contact teamMember in selectedItems)
                {
                    DeleteContact(teamMember);
                }
                Utils.SaveContacts();
                if (Utils.Contacts != null && Utils.Contacts.Count > 0)
                {
                    MasterListView.ItemsSource = Utils.Contacts;
                    MasterListView.SelectedIndex = 0;
                }
                //ReadContacts(); // can be commented

                if (MasterListView.Items.Count > 0)
                {
                    MasterListView.SelectedIndex = 0;
                    selectedContact = MasterListView.SelectedItem as Contact;
                }
                else
                {
                    DetailContentPresenter.Visibility = Visibility.Collapsed;
                }
                //if (PageSizeStatesGroup.CurrentState == NarrowState)
                //{
                //    VisualStateManager.GoToState(this, MasterState.Name, true);
                //}
                //else
                //{
                //    VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
                //}
            }
        }
        private void SelectItems(object sender, RoutedEventArgs e)
        {
            if (MasterListView.Items.Count > 0)
            {
                VisualStateManager.GoToState(this, MultipleSelectionState.Name, true);
            }
        }
        private void CancelSelection(object sender, RoutedEventArgs e)
        {
            if (PageSizeStatesGroup.CurrentState == NarrowState)
            {
                VisualStateManager.GoToState(this, MasterState.Name, true);
            }
            else
            {
                VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
            }
        }
      
        private void ShowSliptView(object sender, RoutedEventArgs e)
        {
            // Clearing the cache
            int cacheSize = ((Frame)Parent).CacheSize;
            ((Frame)Parent).CacheSize = 0;
            ((Frame)Parent).CacheSize = cacheSize;

           // MySamplesPane.SamplesSplitView.IsPaneOpen = !MySamplesPane.SamplesSplitView.IsPaneOpen;
        }
        #endregion
        //public NameList()
        //{
        //    this.InitializeComponent();
        //    isLaunched = false;
        //}

        /// <summary>
        /// //
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private async void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            // only one instance of the UserDataAccountManager pane can be launched at once per thread
            if (isLaunched == false)
            {
                isLaunched = true;

                // Allow user to add email, teamMembers and appointment providing accounts.
                String userDataAccountId = await UserDataAccountManager.ShowAddAccountAsync(UserDataAccountContentKinds.Email | UserDataAccountContentKinds.Appointment | UserDataAccountContentKinds.Contact);

                if (String.IsNullOrEmpty(userDataAccountId))
                {
                    rootPage.NotifyUser("User cancelled or add account failed", NotifyType.StatusMessage);
                }
                else
                {
                    await DisplayUserInformationAsync(userDataAccountId);
                }

                isLaunched = false;
            }
        }


        



        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(SearchResultsPage));
        }
        private async Task DisplayUserInformationAsync(String id)
        {

            // Requires the email, teamMembers, or appointments in the app's manifest
            UserDataAccountStore store = await UserDataAccountManager.RequestStoreAsync(UserDataAccountStoreAccessType.AllAccountsReadOnly);

            // If the store is null, that means all access to Contacts, Calendar and Email data 
            // has been revoked.
            if (store == null)
            {
                rootPage.NotifyUser("Access to Contacts, Calendar and Email has been revoked", NotifyType.ErrorMessage);
                return;
            }

            UserDataAccount account = await store.GetAccountAsync(id);
            rootPage.NotifyUser("Added account: " + account.UserDisplayName, NotifyType.StatusMessage);

        }

        private async void ImportItem(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.Contacts.ContactPicker contactPicker = new Windows.ApplicationModel.Contacts.ContactPicker();
            contactPicker.SelectionMode = Windows.ApplicationModel.Contacts.ContactSelectionMode.Fields;
            contactPicker.DesiredFieldsWithContactFieldType.Add(Windows.ApplicationModel.Contacts.ContactFieldType.Email);
            contactPicker.DesiredFieldsWithContactFieldType.Add(Windows.ApplicationModel.Contacts.ContactFieldType.PhoneNumber);
            Windows.ApplicationModel.Contacts.Contact contact = await contactPicker.PickContactAsync();

            if (contact != null)
            {

                SDKTemplate.Model.Contact importedContact = new SDKTemplate.Model.Contact();
                importedContact.Name = contact.DisplayName;

                foreach (Windows.ApplicationModel.Contacts.ContactEmail email in contact.Emails)
                {
                    //email.Kind;
                    importedContact.Location =  email.Address.ToString();
                }

                foreach (Windows.ApplicationModel.Contacts.ContactPhone phone in contact.Phones)
                {
                    //phone.Kind, 
                    importedContact.PhoneNumber = phone.Number.ToString();
                }
                //if(contact.SourceDisplayPicture)
                //    importedContact.Photo.SetSource(contact.SourceDisplayPicture);
                //// get Source disp contact.SourceDisplayPicture
                //importedContact.Photo.SetSource( contact.LargeDisplayPicture);
                //importedContact.PhoneNumber = contact.Phones.First<Contact>
                //Windows.ApplicationModel.Contacts.ContactAddress addr = new Windows.ApplicationModel.Contacts.ContactAddress();
                //importedContact.Location = contact.Addresses.FirstOrDefault<Windows.ApplicationModel.Contacts.ContactAddress>().ToString();
                AddContact(importedContact as SDKTemplate.Model.Contact);
                Utils.SaveContacts();

            }
            else
            {
                rootPage.NotifyUser("No contact was selected",NotifyType.ErrorMessage);
            }
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {

        }
    }
}
