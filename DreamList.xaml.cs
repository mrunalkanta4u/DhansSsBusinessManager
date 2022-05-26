using SDKTemplate.Model;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Animation;
using DataAccounts;
using Windows.UI.Popups;

namespace SDKTemplate
{
    public sealed partial class DreamList : Page
    {
        // Global variables for page. Static persisted variables are used by ListViewPersistenceHelper
        // We need to save the item container height if the items have variable heights. If all items have a constant fixed height, you can manually 
        // set the height to the fixed value in ItemsListView_ContainerContentChanging
        //private ObservableCollection<Item> _items;
        //private static double _persistedItemContainerHeight = -1;
        //private static string _persistedItemKey = "";
        //private static string _persistedPosition = "";
        private MainPage rootPage = MainPage.Current;
        //private bool isLaunched;
        private Dream selectedDream;
        //private BitmapImage tempImage = new BitmapImage();

        //private static ObservableCollection<Dream> Dreams = new ObservableCollection<Dream>();

        public DreamList()
        {                    
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Utils.ReadDreams();
            if (Utils.Dreams != null)
            {
                if (selectedDream == null && Utils.Dreams.Count > 0)
                {
                    selectedDream = Utils.Dreams[0];
                    MasterListView.ItemsSource = Utils.Dreams.OrderBy(o => o.Category).ToList();
                    MasterListView.SelectedIndex = 0;
                }
                else
                {
                    // Details view is collapsed, in case there is not items.
                    DetailContentPresenter.Visibility = Visibility.Collapsed;
                    selectedDream = null;
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
                MasterListView.SelectedItem = selectedDream;
            }
            else
            {
                new InvalidOperationException();
            }
        }
     
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PageSizeStatesGroup.CurrentState == WideState)
            {
                if (MasterListView.SelectedItems.Count == 1)
                {
                    selectedDream = MasterListView.SelectedItem as Dream;
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
        private void EnableContentTransitions()
        {
            DetailContentPresenter.ContentTransitions.Clear();
            DetailContentPresenter.ContentTransitions.Add(new EntranceThemeTransition());
        }

        #region Commands
        private void AddItem(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddNewDream));
        }
        private void AddDream(Dream sender)
        {
            if (sender != null)
            {
                Utils.Dreams.Add(sender);
            }
            //SaveDreams();
        }
        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (selectedDream != null)
            {
                DeleteDream(selectedDream);
                //SaveDreams();
                if (MasterListView.Items.Count > 0)
                {
                    MasterListView.SelectedIndex = 0;
                    selectedDream = MasterListView.SelectedItem as Dream;
                }
                else
                {
                    // Details view is collapsed, in case there is not items.
                    DetailContentPresenter.Visibility = Visibility.Collapsed;
                    selectedDream = null;
                }
            }
        }

        private void DeleteDream(Dream itemToDelete)
        {
            try
            {
                //Dream det = (Dream)selectedDream;
                Dream item = Utils.Dreams.Where(x => x.DreamName == itemToDelete.DreamName).FirstOrDefault();
                if (item != null)
                {
                    Utils.Dreams.RemoveAt(Utils.Dreams.IndexOf(item));
                }
                Utils.RemoveImage(itemToDelete.ImageUrl);
            }
            catch (Exception ex) { String msg = ex.Message; };
        }
          
      
        private void DeleteItems(object sender, RoutedEventArgs e)
        {
            if (MasterListView.SelectedIndex != -1)
            {
                List<Dream> selectedItems = new List<Dream>();
                foreach (Dream dream in MasterListView.SelectedItems)
                {
                    selectedItems.Add(dream);
                }
                foreach (Dream dream in selectedItems)
                {
                    DeleteDream(dream);
                }

                Utils.SaveDreams();

                MasterListView.SelectionMode = ListViewSelectionMode.None;
                VisualStateManager.GoToState(this, MasterState.Name, true);
                if (Utils.Dreams != null)
                {
                    MasterListView.ItemsSource = Utils.Dreams.OrderBy(o => o.Category).ToList();
                    if (Utils.Dreams.Count > 0)
                        MasterListView.SelectedIndex = 0;
                }

                if (MasterListView.Items.Count > 0)
                {
                    selectedDream = MasterListView.SelectedItem as Dream;
                }
                else
                {
                    DetailContentPresenter.Visibility = Visibility.Collapsed;
                    selectedDream = null;
                }
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

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            //this.Frame.Navigate(typeof(Page2));
            // The clicked item it is the new selected Dream
            selectedDream = e.ClickedItem as Dream;
            if (PageSizeStatesGroup.CurrentState == NarrowState)
            {
                // Go to the details page and display the item 
                Frame.Navigate(typeof(DreamDetailsPage), selectedDream, new DrillInNavigationTransitionInfo());
            }
            //else
            {
                // Play a refresh animation when the user switches detail items.
                //EnableContentTransitions();
            }
        }
        private void OnCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            bool isNarrow = e.NewState == NarrowState;
            if (isNarrow)
            {
                Frame.Navigate(typeof(DetailsPage), selectedDream, new SuppressNavigationTransitionInfo());
            }
            else
            {
                VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
                MasterListView.SelectionMode = ListViewSelectionMode.Extended;
                MasterListView.SelectedItem = selectedDream;
            }

            EntranceNavigationTransitionInfo.SetIsTargetElement(MasterListView, isNarrow);
            if (DetailContentPresenter != null)
            {
                EntranceNavigationTransitionInfo.SetIsTargetElement(DetailContentPresenter, !isNarrow);
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Details view can remove an item from the list.
            if (e.Parameter as string == "Delete")
            {
                DeleteDream(selectedDream);
                Utils.SaveDreams();
            }
            if (e.Parameter as Dream != null)
            {
                AddDream(e.Parameter as Dream);
                Utils.SaveDreams();

            }
            base.OnNavigatedTo(e);
            if (Utils.Dreams != null)
            {
                MasterListView.ItemsSource = Utils.Dreams.OrderBy(o => o.Category).ToList();
                if(Utils.Dreams.Count > 0)
                    MasterListView.SelectedIndex = 0;
            }
            if (IsMobile)
            {
                //NameListCmdBarBtn.Margin
            }
        }
        public static bool IsMobile
        {
            get
            {
                var qualifiers = Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().QualifierValues;
                return (qualifiers.ContainsKey("DeviceFamily") && qualifiers["DeviceFamily"] == "Mobile");
            }
        }
        private void ItemsListView_ItemClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DreamDetailsPage));
        }
        private void OnChecked(object sender, RoutedEventArgs e)
        {
            //if (!this.Achieved)
            //{
            //    var dialog = new MessageDialog("Do you want to tick the dream which was " + this.SelectedDream.DreamName + " ?", "Congradulations!!!");

            //    dialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(this.CommandInvokedHandler), commandId: 1));
            //    dialog.Commands.Add(new UICommand("No", new UICommandInvokedHandler(this.CommandInvokedHandler), commandId: 2));
            //    if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
            //    {
            //        // Adding a 3rd command will crash the app when running on Mobile !!!
            //        dialog.Commands.Add(new UICommand("Maybe later", new UICommandInvokedHandler(this.CommandInvokedHandler), commandId: 3));
            //    }
            //    dialog.DefaultCommandIndex = 0;
            //    dialog.CancelCommandIndex = 1;
            //    var result = dialog.ShowAsync();
            //}
            //else
            //{
            //    DreamAchieved.IsOn = true;
            //}
        }
        private void CommandInvokedHandler(IUICommand command)
        {
            //    if (command.Label.Equals("Yes"))
            //    {
            //        Utils.TickDream(this.SelectedDream.DreamName);
            //        Utils.SaveDreams();
            //    }
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {

        }
        //private void OnMasterListViewLoaded(object sender, RoutedEventArgs e)
        //{

        //}

        //private void OnListLayoutUpdated(object sender, object e)
        //{

        //}
    }
}