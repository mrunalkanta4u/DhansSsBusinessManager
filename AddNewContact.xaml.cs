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
using System.IO;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using DataAccounts;
using System.Linq;

namespace SDKTemplate
{

    public sealed partial class AddNewContact : Page
    {
        private MainPage rootPage = MainPage.Current;
        private bool isLaunched = false;
        static ObservableCollection<Contact> Contacts = new ObservableCollection<Contact>();
        BitmapImage bmp = new BitmapImage(new Uri("ms-appx:///Assets/Account.png"));
        String imageUrl = "Assets/Account.png";
        ImageBrush myBrush = new ImageBrush();
        //Image img = new Image();
        Contact selectedContact;
        //PhotoUploadCropper;

        public AddNewContact()
        {
            this.InitializeComponent();
        }
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);

        //    //SelectedDream = e.Parameter as Dream;
        //    // Register for hardware and software back request from the system
        //    SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
        //    systemNavigationManager.BackRequested += OnBackRequested;
        //    systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        //}
        //protected override void OnNavigatedFrom(NavigationEventArgs e)
        //{
        //    base.OnNavigatedFrom(e);

        //    SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
        //    systemNavigationManager.BackRequested -= OnBackRequested;
        //    systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        //}
        //private void OnBackRequested(object sender, BackRequestedEventArgs e)
        //{
        //    // Mark event as handled so we don't get bounced out of the app.
        //    e.Handled = true;
        //    // Page above us will be our master view.
        //    // Make sure we are using the "drill out" animation in this transition.
        //    Frame.Navigate(typeof(DreamList), "Back", new EntranceNavigationTransitionInfo());
        //}
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           // Details view can remove an item from the list.
            if (e.Parameter as string == "Delete")
            {
                //DeleteContact(selectedContact);
            }
            if (e.Parameter as Contact != null)
            {
                TiteOfPage.Text = "Edit Contact";
                Contact contact = (e.Parameter as Contact);
                NameTextBox.Text = contact.Name;
                PhoneNumberTextBox.Text = contact.PhoneNumber;
                LocationTextBox.Text = contact.Location;
                //Uri uri = contact.Photo.UriSource;
                //myBrush.ImageSource = bmp;
                //PhotoEllipse.Fill = myBrush;
                switch (contact.Category.Trim().ToLower())
                {
                    case "hot":
                        CategoryComboBox.SelectedIndex = 0;
                        break;
                    case "oye buddy":
                        CategoryComboBox.SelectedIndex = 1;
                        break;
                    case "warm":
                        CategoryComboBox.SelectedIndex = 2;
                        break;
                    case "cold":
                        CategoryComboBox.SelectedIndex = 3;
                        break;
                    default:
                        CategoryComboBox.SelectedIndex = -1;
                        break;
                }
                RemarkTextBox.Text = contact.Remark;
                selectedContact = Utils.Contacts.Where((item) => { return
                    (item.Name.Trim().ToLower().Equals(contact.Name.Trim().ToLower()) &&
                    item.PhoneNumber.Trim().ToLower().Equals(contact.PhoneNumber.Trim().ToLower()))
                    ; }).FirstOrDefault();
                if (!selectedContact.ImageUrl.Equals("Assets/Account.png"))
                {
                    myBrush.ImageSource = new BitmapImage(new Uri(selectedContact.ImageUrl));
                    PhotoEllipse.Fill = myBrush;
                }
            }
        }

        private void SaveItem(object sender, RoutedEventArgs e)
        {
            string selectedInfo = string.Empty;
            string selectedInvite = string.Empty;

            if (NameTextBox.Text.ToString().Trim().Equals(string.Empty))
            {
                //NameTextBox.Background = new SolidColorBrush(Colors.OrangeRed);
                NameTextBox.PlaceholderText = "Name is Required !!!";
                NameTextBox.Focus(FocusState.Keyboard);
                //NameTextBox.Foreground = new SolidColorBrush(Colors.White);
                return;
            }
            if (TiteOfPage.Text.ToString().Equals("Edit Contact"))
            {
                selectedInfo = selectedContact.Info;
                selectedInvite = selectedContact.Invite;
                //if (!selectedContact.ImageUrl.Equals("Assets/Account.png"))
                //{
                //    bmp = new BitmapImage(new Uri(selectedContact.ImageUrl));
                //}
                imageUrl = selectedContact.ImageUrl;
                Utils.Contacts.Remove(selectedContact);
                
            }
            this.Frame.Navigate(typeof(NameList), new Model.Contact { Name = NameTextBox.Text.ToString(), PhoneNumber = PhoneNumberTextBox.Text.ToString(),
                Location = LocationTextBox.Text.ToString(), Category = (CategoryComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                Info = selectedInfo, Invite = selectedInvite, Remark = RemarkTextBox.Text.ToString(), ImageUrl = imageUrl});
            //Frame.Navigate(typeof(NameList), "Delete", new EntranceNavigationTransitionInfo());
        }
        //private async void OpenButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //FileOpenPicker openPicker = new FileOpenPicker();
        //    //openPicker.ViewMode = PickerViewMode.Thumbnail;
        //    //openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        //    //openPicker.FileTypeFilter.Add(".jpg");
        //    //openPicker.FileTypeFilter.Add(".jpeg");
        //    //openPicker.FileTypeFilter.Add(".bmp");
        //    //openPicker.FileTypeFilter.Add(".png");
        //    //StorageFile imgFile = await openPicker.PickSingleFileAsync();
        //    //if (imgFile != null)
        //    //{
        //    //    await this.ImageCropper.LoadImage(imgFile);
        //    //}
        //}
        private void CancelSave(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NameList), "Cancel", new EntranceNavigationTransitionInfo());
            //if (PageSizeStatesGroup.CurrentState == NarrowState)
            //{
            //    VisualStateManager.GoToState(this, MasterState.Name, true);
            //}
            //else
            //{
            //    VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
            //}
        }
        private async void Fix_Click(object sender, RoutedEventArgs e)
        {
            // only one instance of the UserDataAccountManager pane can be launched at once per thread
            if (isLaunched == false)
            {
                isLaunched = true;
                //rootPage.NotifyUser(string.Empty, NotifyType.StatusMessage);

                var selectedAccount = (UserDataAccount)CategoryComboBox.SelectedValue;
                if (selectedAccount != null)
                {
                    //if (await IsFixNeededAsync(selectedAccount))
                    //{
                    //    await UserDataAccountManager.ShowAccountErrorResolverAsync(selectedAccount.Id);
                    //}
                    //else
                    //{
                    //    rootPage.NotifyUser("Account is not in an error state", NotifyType.ErrorMessage);
                    //}
                    await rootPage.LoadDataAccountsAsync(CategoryComboBox);
                }

                isLaunched = false;
            }
        }

        private void cropBtn_Click(object sender, RoutedEventArgs e)
        {
            //PhotoChooserTask open = new PhotoChooserTask();
            //open.PixelHeight = 50;
            //open.PixelWidth = 50;
            //open.ShowCamera = true;
            //open.Completed += open_Completed;
            //open.Show();
        }

        //void open_Completed(object sender, PhotoResult e)
        //{
        //    BitmapImage image = new BitmapImage();
        //    image.SetSource(e.ChosenPhoto);
        //    cropedImage.Source = image;
        //}
        private async void Ellipse_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".bmp");
                openPicker.FileTypeFilter.Add(".png");
                StorageFile imgFile = await openPicker.PickSingleFileAsync();
                //if (imgFile != null)
                //{
                //    await this.ImageCropper.LoadImage(imgFile);
                //}              
                BitmapImage imagetoResize;

                if (imgFile != null)
                {
                    IRandomAccessStream stream = await imgFile.OpenAsync(FileAccessMode.Read);
                    //BackgroundLogo.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    //imageCrop.Opacity = 1;
                    //imageCrop.ImageSource = bmp;
                    //ProfileSetupStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    imagetoResize = bmp;
                    bmp.SetSource(stream);
                    myBrush.ImageSource = bmp;  
                    PhotoEllipse.Fill = myBrush;
                    this.DataContext = imgFile;
                    string imgFileName = Utils.GetRandomFileName();
                    Utils.SaveImage(imgFile, imgFileName);
                    imageUrl = imgFile.Path.ToString();
                    StorageFolder appFolder = ApplicationData.Current.RoamingFolder;
                    StorageFile imageFile = await appFolder.GetFileAsync(imgFileName);
                    bool isExists = File.Exists(imageFile.Path.ToString());
                    imageUrl = imageFile.Path.ToString();
                    if (TiteOfPage.Text.ToString().Equals("Edit Contact"))
                    {
                        //selectedContact.Photo = bmp;
                        selectedContact.ImageUrl = imageUrl;
                    }

                    //this.Frame.Navigate(typeof(CropPhoto), imgFile);
                }
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
                //rootPage.NotifyUser(ex.Message,NotifyType.ErrorMessage);
            }
        }
       
        //public static WriteableBitmap Crop(this WriteableBitmap source, int x1, int y1, int x2, int y2)
        //{
        //    if (x1 >= x2 ||
        //        y1 >= y2 ||
        //        x1 < 0 ||
        //        y1 < 0 ||
        //        x2 < 0 ||
        //        y2 < 0 ||
        //        x1 > source.PixelWidth ||
        //        y1 > source.PixelHeight ||
        //        x2 > source.PixelWidth ||
        //        y2 > source.PixelHeight)
        //    {
        //        throw new ArgumentException();
        //    }

        //    //var buffer = source.PixelBuffer.GetPixels();
        //    var cw = x2 - x1;
        //    var ch = y2 - y1;
        //    var target = new WriteableBitmap(cw, ch);

        //    var croppedBytes =
        //        new byte[4 * cw * ch];
        //    var inputStream = source.PixelBuffer.AsStream();
        //    inputStream.Seek(4 * (source.PixelWidth * y1 + x1), SeekOrigin.Current);
        //    for (int i = 0; i < ch; i++)
        //    {
        //        inputStream.Read(croppedBytes, 4 * cw * i, 4 * cw);
        //        inputStream.Seek(4 * (source.PixelWidth - cw), SeekOrigin.Current);
        //    }

        //    var outputStream = target.PixelBuffer.AsStream();
        //    outputStream.Seek(0, SeekOrigin.Begin);
        //    outputStream.Write(croppedBytes, 0, croppedBytes.Length);
        //    target.Invalidate();

        //    return target;
        //}

        ////private void AcceptPhotoImageCropClick(object sender, RoutedEventArgs e)
        //{
        //    WriteableBitmap bmp = new WriteableBitmap(0, 0).FromContent(imagetoResize);
        //    var croppedBmp = bmp.Crop(0, 0, bmp.PixelWidth / 2, bmp.PixelHeight / 2);
        //    croppedBmp.SaveToMediaLibrary("ProfilePhoto.jpg");
        //}

        //async Task<WriteableBitmap> LoadBitmap(StorageFile file)
        //{
        //    int cropx = PhotoUploadCropper.CropTop;
        //    int cropy = PhotoUploadCropper.CropLeft;
        //    int cropW = PhotoUploadCropper.CropWidth;
        //    int cropH = PhotoUploadCropper.CropHeight;

        //    using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
        //    {
        //        var bmp = await BitmapFactory.New(1, 1).FromStream(fileStream);
        //        var croppedBmp = bmp.Crop(cropy, cropx, cropW, cropH);
        //        var resizedcroppedBmp = croppedBmp.Resize(200, 200, WriteableBitmapExtensions.Interpolation.Bilinear);

        //        return resizedcroppedBmp;
        //    }
        //}

        //private async void AcceptPhotoImageCropClick(object sender, RoutedEventArgs e)
        //{

        //    var CroppedBMP = await CropBitmap(fileclone);

        //    using (IRandomAccessStream fileStream = new InMemoryRandomAccessStream())
        //    {
        //        string filename = Path.GetRandomFileName() + ".JPG";
        //        var file = await Windows.Storage.ApplicationData.Current.TemporaryFolder.CreateFileAsync(filename, CreationCollisionOption.GenerateUniqueName);
        //        using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
        //        {
        //            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
        //            Stream pixelStream = CroppedBMP.PixelBuffer.AsStream();
        //            byte[] pixels = new byte[pixelStream.Length];
        //            await pixelStream.ReadAsync(pixels, 0, pixels.Length);
        //            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)CroppedBMP.PixelWidth, (uint)CroppedBMP.PixelHeight, 96.0, 96.0, pixels);
        //            await encoder.FlushAsync();
        //        }

        //        ProfilePhotoButtonsGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        //        BackgroundLogo.Visibility = Windows.UI.Xaml.Visibility.Visible;
        //        PhotoUploadCropper.IsEnabled = false;
        //        PhotoUploadCropper.Opacity = 0;
        //        ProfileSetupStackPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;

        //        if (fileStream != null)
        //        {
        //            UploadFile(file);
        //        }
        //    }
        //}

        private void OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {        //private void CategoryComboBox_Loaded(object sender, RoutedEventArgs e)
        //{
        //    CategoryComboBox.Items.Add("Hot");
        //    CategoryComboBox.Items.Add("Oye Buddy");
        //    CategoryComboBox.Items.Add("Warm");
        //    CategoryComboBox.Items.Add("Cold");
        //    this.CategoryComboBox.SelectedIndex = 2;
        //}
            //NameTextBox.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void ImageBrush_ImageOpened(object sender, RoutedEventArgs e)
        {
            //rootPage.NotifyUser("iamge opened ",NotifyType.StatusMessage);
        }

        //private void CategoryComboBox_Loaded(object sender, RoutedEventArgs e)
        //{
        //    CategoryComboBox.SelectedIndex = 2;
        //}
    }
}
