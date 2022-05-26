using SDKTemplate.Model;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Foundation;
using DataAccounts;
using System;
using Windows.UI.Xaml.Media.Imaging;

namespace SDKTemplate
{
    /// <summary>
    /// DreamDetailsPage is simply a blank page with a button to navigate back to the MainPage. 
    /// It's purpose is just to reset the list on the main page so that the implementation of ListViewPersistenceHelper can be demonstrated
    /// </summary>
    public sealed partial class DreamDetailsPage : Page
    {
        private Dream SelectedDream { set; get; }
        public DreamDetailsPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (ShouldGoToWideState())
            {
                // We shouldn't see this page since we are in "wide master-detail" mode.
                // Play a transition as we are navigating from a separate page.
                NavigateBackForWideState(useTransition: true);
            }
            else
            {
                // Realize the main page content.
                FindName("RootPanel");
            }

            Window.Current.SizeChanged += OnWindowSizeChanged;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SelectedDream = e.Parameter as Dream;
            // Register for hardware and software back request from the system
            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested += OnBackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested -= OnBackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }
        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            // Mark event as handled so we don't get bounced out of the app.
            e.Handled = true;
            // Page above us will be our master view.
            // Make sure we are using the "drill out" animation in this transition.
            Frame.Navigate(typeof(DreamList), "Back", new EntranceNavigationTransitionInfo());
        }
        void NavigateBackForWideState(bool useTransition)
        {
            // Evict this page from the cache as we may not need it again.
            NavigationCacheMode = NavigationCacheMode.Disabled;

            if (useTransition)
            {
                Frame.GoBack(new DrillInNavigationTransitionInfo());
            }
            else
            {
                Frame.GoBack(new SuppressNavigationTransitionInfo());
            }
        }
        private bool ShouldGoToWideState()
        {
            return Window.Current.Bounds.Width >= 720;
        }
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= OnWindowSizeChanged;
        }
        private void OnWindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (ShouldGoToWideState())
            {
                // Make sure we are no longer listening to window change events.
                Window.Current.SizeChanged -= OnWindowSizeChanged;

                // We shouldn't see this page since we are in "wide master-detail" mode.
                NavigateBackForWideState(useTransition: false);
            }
        }
        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DreamList), "Delete", new EntranceNavigationTransitionInfo());
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddNewDream), new Model.Dream
            {
                DreamName = DreamName.Text.ToString(),
                Details = DetailsTextBlock.Text.ToString(),
                //Photo = new BitmapImage(new Uri("ms-appx:///Assets/placeholder.png")), //imageUri,   
                ImageUrl = SelectedDream.ImageUrl,
                //Photo = System.IO.File.ReadAllBytes(imageUri),
                Category = CategoryTextBlock.Text,
                TatgetDate = TatgetDateTextBlock.Text,
                Achieved = DreamAchieved.IsOn,
                Remark = RemarkTextBlock.Text
            });
        }

        private void OnChecked(object sender, RoutedEventArgs e)
        {
            if (!SelectedDream.Achieved)
            {
                var dialog = new MessageDialog("Do you want to tick the dream which was " + this.SelectedDream.DreamName + " ?", "Congradulations!!!");

                dialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(this.CommandInvokedHandler), commandId: 1));
                dialog.Commands.Add(new UICommand("No", new UICommandInvokedHandler(this.CommandInvokedHandler), commandId: 2));
                if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
                {
                    // Adding a 3rd command will crash the app when running on Mobile !!!
                    dialog.Commands.Add(new UICommand("Maybe later", new UICommandInvokedHandler(this.CommandInvokedHandler), commandId: 3));
                }
                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;
                var result = dialog.ShowAsync();
            }
            else
            {
                DreamAchieved.IsOn = true;
            }
        }
        private void CommandInvokedHandler(IUICommand command)
        {
            if (command.Label.Equals("Yes"))
            {
                Utils.TickDream(this.SelectedDream.DreamName);
                Utils.SaveDreams();
            }
        }
    }
}
