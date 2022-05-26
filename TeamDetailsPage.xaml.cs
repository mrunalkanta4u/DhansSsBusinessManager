using DataAccounts;
using SDKTemplate.Model;
using System;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace SDKTemplate
{
    /// <summary>
    /// TeamMemberDetailsPage is simply a blank page with a button to navigate back to the MainPage. 
    /// It's purpose is just to reset the list on the main page so that the implementation of ListViewPersistenceHelper can be demonstrated
    /// </summary>
    public sealed partial class TeamDetailsPage : Page
    {
        private MainPage rootPage = MainPage.Current;
        private TeamMember SelectedTeamMember { set; get; }
        public TeamDetailsPage()
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

            SelectedTeamMember = e.Parameter as TeamMember;
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
            Frame.Navigate(typeof(TeamList), "Back", new EntranceNavigationTransitionInfo());
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
            Frame.Navigate(typeof(TeamList), "Delete", new EntranceNavigationTransitionInfo());
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddNewTeamMember), new Model.TeamMember
            {
                Name = IRNameTextBlock.Text.ToString(),
                PhoneNumber = PhoneNumberTextBlock.Text.ToString(),
                MailId = MailIDTextBlock.Text.ToString().ToLower(),
                MailIdPassword = MailIDPasswordTextBlock.Text.ToString(),
                KYCDone = KYCStatus.IsOn,
                IRId = IRIDTextBlock.Text.ToString().ToUpper(),
                Password = IRPasswordTextBlock.Text.ToString(),
                CPAPassword = CPAPasswordTextBlock.Text.ToString(),
                SecurityQA = SecurityQATextBlock.Text.ToString(),
                SecurityWord = SecurityWordTextBlock.Text.ToString(),
                Remark = RemarkTextBlock.Text.ToString(),
                //Photo = new BitmapImage(new Uri("ms-appx:///Assets/Account.png"))
                ImageUrl = SelectedTeamMember.ImageUrl
            });
        }

        private void OnToggled(object sender, RoutedEventArgs e)
        {
            if (!SelectedTeamMember.KYCDone)
            {
                var dialog = new MessageDialog("Is KYC done for " + this.SelectedTeamMember.Name + " ?", "KYC Status");

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
                KYCStatus.IsOn = true;
            }
            //if (!KYCStatus.IsOn)
            //{
            //    return;
            //}
        }
        private void CommandInvokedHandler(IUICommand command)
        {
            //if (command.Id.Equals(1))
            if(command.Label.Equals("Yes"))
            {
                Utils.CompleteKYC(this.SelectedTeamMember.Name);
                Utils.SaveTeamMembers();
                //KYCStatus.IsOn = true;
                //KYCStatus.IsEnabled = false;
                //// Display message showing the label of the command that was invoked
                //rootPage.NotifyUser("KYC Done for " + this.SelectedTeamMember.Name ,
                //    NotifyType.StatusMessage);
            }
            else
            {
                //return;
                //KYCStatus.IsOn = false;
                //KYCStatus.IsEnabled = true;
                //rootPage.NotifyUser("The '" + command.Label + "' command has been selected.",
                //    NotifyType.StatusMessage);
            }
            
        }
    }
}
