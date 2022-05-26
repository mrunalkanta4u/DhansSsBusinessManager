using SDKTemplate.Model;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Calls;
using System;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using System.IO;

namespace SDKTemplate
{
    public sealed partial class DetailsPage : Page
    {
        private Contact SelectedContact { set; get; }
        public DetailsPage()
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

            SelectedContact = e.Parameter as Contact;
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
            Frame.Navigate(typeof(NameList), "Back", new EntranceNavigationTransitionInfo());
        }
        void NavigateBackForWideState(bool useTransition)
        {
            // Evict this page from the cache as we may not need it again.
            NavigationCacheMode = NavigationCacheMode.Disabled;

            if (useTransition)
            {
                Frame.GoBack(new EntranceNavigationTransitionInfo());
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
            Frame.Navigate(typeof(NameList), "Delete", new EntranceNavigationTransitionInfo());
        }
        private void EditItem(object sender, RoutedEventArgs e)
        {

            //ContactPhoto.ImageSource
            //ContactPhoto.BaseUri;
            //Photo = new BitmapImage()
          
            Model.Contact cn = new Model.Contact();
            //BitmapImage cnPhoto = new BitmapImage(new Uri("ms-appx:///Assets/Account.png"));
            //cnPhoto = ContactPhoto.Source as BitmapImage;
            
            this.Frame.Navigate(typeof(AddNewContact), new Model.Contact
            {
                Name = Name.Text.ToString(),
                PhoneNumber = PhoneNumberData.Text.ToString(),
                Location = LocationData.Text.ToString(),
                Category = CategoryData.Text.ToString(),
                Info = InfoData.Text.ToString(),
                Invite = InviteData.Text.ToString(),
                Remark = RemarkData.Text.ToString(),
                //Photo = ContactPhoto.Name //new BitmapImage(ContactPhoto.BaseUri)
                //Photo = new BitmapImage(new Uri("ms-appx:///Assets/Account.png"))
                ImageUrl = SelectedContact.ImageUrl
            });
        }

        private void OnTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.Calls.CallsPhoneContract", 1, 0))
            {
                TextBlock tblock = sender as TextBlock;
                PhoneCallManager.ShowPhoneCallUI(tblock.Text, tblock.Text);

                // Code to directly call
                //PhoneCallStore PhoneCallStore = await PhoneCallManager.RequestStoreAsync();
                //Guid LineGuid = await PhoneCallStore.GetDefaultLineAsync();
                //PhoneLine pl = await PhoneLine.FromIdAsync(LineGuid);               
                //pl.Dial(tblock.Text, tblock.Text);
            }
        }
    }
}
