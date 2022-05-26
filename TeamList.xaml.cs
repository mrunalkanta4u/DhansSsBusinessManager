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
using Windows.UI.Core;

namespace SDKTemplate
{
    public sealed partial class TeamList : Page
    {
        private MainPage rootPage = MainPage.Current;
        //private bool isLaunched;
        private TeamMember selectedTeamMember;

        //private static ObservableCollection<TeamMember> TeamMembers = new ObservableCollection<TeamMember>();

        public TeamList()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Details view can remove an item from the list.
            if (e.Parameter as string == "Delete")
            {
                DeleteTeamMember(selectedTeamMember);
                Utils.SaveTeamMembers();
            }
            //if (e.Parameter as string == "DeleteItems")
            //{
            //    //DeleteTeamMember(selectedTeamMember);
            //    SaveTeamMembers();
            //}
            
            if (e.Parameter as TeamMember != null)
            {
                AddTeamMember(e.Parameter as TeamMember);
                Utils.SaveTeamMembers();
                
            }
            base.OnNavigatedTo(e);
            //readTeamMembers();
            if (Utils.TeamMembers != null)
            {
                MasterListView.ItemsSource = Utils.TeamMembers.OrderBy(o => o.IRId).ToList();
                if(Utils.TeamMembers.Count > 0)
                    MasterListView.SelectedIndex = 0;
            }
        }
        //private void OnBackRequested(object sender, BackRequestedEventArgs e)
        //{
        //    // Mark event as handled so we don't get bounced out of the app.
        //    e.Handled = true;
        //    // Page above us will be our master view.
        //    // Make sure we are using the "drill out" animation in this transition.
        //    Frame.Navigate(typeof(MainPage), "Back", new EntranceNavigationTransitionInfo());
        //}
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Utils.ReadTeamMembers();
            if (Utils.TeamMembers != null)
            {
                if (selectedTeamMember == null && Utils.TeamMembers.Count > 0)
                {
                    selectedTeamMember = Utils.TeamMembers[0];
                    MasterListView.ItemsSource = Utils.TeamMembers;
                    MasterListView.SelectedIndex = 0;
                }
                else
                {
                    // Details view is collapsed, in case there is not items.
                    DetailContentPresenter.Visibility = Visibility.Collapsed;
                    selectedTeamMember = null;
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
                MasterListView.SelectedItem = selectedTeamMember;
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
                Frame.Navigate(typeof(DetailsPage), selectedTeamMember, new SuppressNavigationTransitionInfo());
            }
            else
            {
                VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
                MasterListView.SelectionMode = ListViewSelectionMode.Extended;
                MasterListView.SelectedItem = selectedTeamMember;
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
                    selectedTeamMember = MasterListView.SelectedItem as TeamMember;
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
            selectedTeamMember = e.ClickedItem as TeamMember;
            if (PageSizeStatesGroup.CurrentState == NarrowState)
            {
                // Go to the details page and display the item 
                Frame.Navigate(typeof(TeamDetailsPage), selectedTeamMember, new DrillInNavigationTransitionInfo());
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
            this.Frame.Navigate(typeof(AddNewTeamMember));
        }
        private void AddTeamMember(TeamMember sender)
        {
            if (sender != null)
            {
                Utils.TeamMembers.Add(sender);
            }
            //SaveTeamMembers();
        }
        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (selectedTeamMember != null)
            {
                DeleteTeamMember(selectedTeamMember);
                //SaveTeamMembers();
                if (MasterListView.Items.Count > 0)
                {
                    MasterListView.SelectedIndex = 0;
                    selectedTeamMember = MasterListView.SelectedItem as TeamMember;
                }
                else
                {
                    // Details view is collapsed, in case there is not items.
                    DetailContentPresenter.Visibility = Visibility.Collapsed;
                    selectedTeamMember = null;
                }
            }
        }

        private void DeleteTeamMember(TeamMember itemToDelete)
        {
            try
            {
                //TeamMember itemToDelete = (TeamMember)selectedTeamMember;
                TeamMember item = Utils.TeamMembers.Where(x => x.Name == itemToDelete.Name).FirstOrDefault();
                if (item != null)
                {
                    Utils.TeamMembers.RemoveAt(Utils.TeamMembers.IndexOf(item));
                }
                Utils.RemoveImage(itemToDelete.ImageUrl);
            }
            catch (Exception ex) { String msg = ex.Message; };

        }

        private void DeleteItems(object sender, RoutedEventArgs e)
        {
            if (MasterListView.SelectedIndex != -1)
            {
                List<TeamMember> selectedItems = new List<TeamMember>();
                foreach (TeamMember teamMember in MasterListView.SelectedItems)
                {
                    selectedItems.Add(teamMember);
                }
                foreach (TeamMember teamMember in selectedItems)
                {
                    DeleteTeamMember(teamMember);
                }
                
                Utils.SaveTeamMembers();
                MasterListView.SelectionMode = ListViewSelectionMode.None;
                VisualStateManager.GoToState(this, MasterState.Name, true);
                if (Utils.TeamMembers != null)
                {
                    MasterListView.ItemsSource = Utils.TeamMembers.OrderBy(o => o.IRId).ToList();
                    if (Utils.TeamMembers.Count > 0)
                        MasterListView.SelectedIndex = 0;
                }

                if (MasterListView.Items.Count > 0)
                {
                    //MasterListView.SelectedIndex = 0;
                    selectedTeamMember = MasterListView.SelectedItem as TeamMember;
                }
                else
                {
                    DetailContentPresenter.Visibility = Visibility.Collapsed;
                    selectedTeamMember = null;
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
        //public TeamList()
        //{
        //    this.InitializeComponent();
        //    isLaunched = false;
        //}

        /// <summary>
        /// //
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        //private async void AddAccount_Click(object sender, RoutedEventArgs e)
        //{
        //    // only one instance of the UserDataAccountManager pane can be launched at once per thread
        //    if (isLaunched == false)
        //    {
        //        isLaunched = true;

        //        // Allow user to add email, teamMembers and appointment providing accounts.
        //        String userDataAccountId = await UserDataAccountManager.ShowAddAccountAsync(UserDataAccountContentKinds.Email | UserDataAccountContentKinds.Appointment | UserDataAccountContentKinds.TeamMember);

        //        if (String.IsNullOrEmpty(userDataAccountId))
        //        {
        //            rootPage.NotifyUser("User cancelled or add account failed", NotifyType.StatusMessage);
        //        }
        //        else
        //        {
        //            await DisplayUserInformationAsync(userDataAccountId);
        //        }

        //        isLaunched = false;
        //    }
        //}


        



        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(SearchResultsPage));
        }
        private async Task DisplayUserInformationAsync(String id)
        {

            // Requires the email, teamMembers, or appointments in the app's manifest
            UserDataAccountStore store = await UserDataAccountManager.RequestStoreAsync(UserDataAccountStoreAccessType.AllAccountsReadOnly);

            // If the store is null, that means all access to TeamMembers, Calendar and Email data 
            // has been revoked.
            if (store == null)
            {
                rootPage.NotifyUser("Access to TeamMembers, Calendar and Email has been revoked", NotifyType.ErrorMessage);
                return;
            }

            UserDataAccount account = await store.GetAccountAsync(id);
            rootPage.NotifyUser("Added account: " + account.UserDisplayName, NotifyType.StatusMessage);

        }

        private void EditItem(object sender, RoutedEventArgs e)
        {

        }

        private void SearchNameList(object sender, RoutedEventArgs e)
        {
            TeamListSearchTextBox.Visibility = Utils.toggleVisibility(TeamListSearchTextBox);
        }

        private void OnSearchBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TeamListSearchTextBox.Text))
            {
                this.MasterListView.ItemsSource = Utils.TeamMembers.OrderBy(o => o.Name).ToList();
            }
            this.MasterListView.ItemsSource = Utils.TeamMembers.Where((item) => { return item.Name.ToString().ToLower().Contains(TeamListSearchTextBox.Text.ToString().ToLower()); });

        }
    }
}
