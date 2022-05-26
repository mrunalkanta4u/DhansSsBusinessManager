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
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Animation;
using System.Globalization;
using DataAccounts;
using System.Linq;
using Windows.UI.Xaml.Media;

namespace SDKTemplate
{
    public sealed partial class AddNewDream : Page
    {
        private MainPage rootPage = MainPage.Current;
        static ObservableCollection<Dream> Dreams = new ObservableCollection<Dream>();
        private string imageFileName = String.Empty; 
        BitmapImage bmp = new BitmapImage(new Uri("ms-appx:///Assets/placeholder.jpg"));
        String imageUrl = "Assets/placeholder.jpg";
        ImageBrush myBrush = new ImageBrush();
        Dream SelectedDream;

        public AddNewDream()
        {
            this.InitializeComponent();
            DateTime thisDay = DateTime.Today;
            TargetDateOfDream.Date = TargetDateOfDream.MinDate = thisDay;
        }

        protected override  void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter as Dream != null)
            {
                TiteOfPage.Text = "Edit Dream";
                Dream dream = (e.Parameter as Dream);
                DreamTextBox.Text = dream.DreamName;
                DescriptionTextBox.Text = dream.Details;

                //DateTime dt = 
                //TargetDateOfDream.SetDisplayDate(DateTime.Today.AddDays(-3)); 
                TargetDateOfDream.Date = DateTime.ParseExact(dream.TatgetDate, "dd MMMM yyyy , dddd ", CultureInfo.InvariantCulture);
                 switch (dream.Category.Trim().ToLower())
                {
                    case "long term":
                        CategoryComboBox.SelectedIndex = 0;
                        break;
                    case "mid term":
                        CategoryComboBox.SelectedIndex = 1;
                        break;
                    case "short term":
                        CategoryComboBox.SelectedIndex = 2;
                        break;
                    default:
                        CategoryComboBox.SelectedIndex = -1;
                        break;
                }
                
                SelectedDream = Utils.Dreams.Where((item) => {
                    return
                        (item.DreamName.Trim().ToLower().Equals(dream.DreamName.Trim().ToLower()) &&
                        item.Category.Trim().ToLower().Equals(dream.Category.Trim().ToLower()))
                        ;
                }).FirstOrDefault();
                if (!SelectedDream.ImageUrl.Equals("Assets/placeholder.jpg"))
                {
                    myBrush.ImageSource = new BitmapImage(new Uri(SelectedDream.ImageUrl));
                    PhotoRectangle.Fill = myBrush;
                }
            }
        }

        private void SaveItem(object sender, RoutedEventArgs e)
        {
            bool dreamAchieved = false;
            if (DreamTextBox.Text.ToString().Trim().Equals(string.Empty))
            {
                //NameTextBox.Background = new SolidColorBrush(Colors.OrangeRed);
                DreamTextBox.PlaceholderText = "Dream is Required !!!";
                DreamTextBox.Focus(FocusState.Keyboard);
                //NameTextBox.Foreground = new SolidColorBrush(Colors.White);
                return;
            }
            if (TiteOfPage.Text.ToString().Equals("Edit Dream"))
            {
                dreamAchieved = SelectedDream.Achieved;
                bmp = new BitmapImage(new Uri(SelectedDream.ImageUrl));
                imageUrl = SelectedDream.ImageUrl;
                Utils.Dreams.Remove(SelectedDream);
            }
            this.Frame.Navigate(typeof(DreamList), new Model.Dream
            {
                DreamName = DreamTextBox.Text.ToString(),
                Details = DescriptionTextBox.Text.ToString(),
                //Photo = bmp, //imageUri,   
                ImageUrl = imageUrl,
                //Photo = System.IO.File.ReadAllBytes(imageUri),
                Category = (CategoryComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                TatgetDate = TargetDateOfDream.Date.Value.ToString("dd MMMM yyyy , dddd "),
                Achieved = dreamAchieved,
                Remark = string.Empty
            });
            //Frame.Navigate(typeof(DreamList), "Delete", new EntranceNavigationTransitionInfo());
        }


  

        private void CancelSave(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DreamList), "Cancel", new EntranceNavigationTransitionInfo());
        }
     
        private async void Reactangle_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
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
                BitmapImage imagetoResize;

                if (imgFile != null)
                {
                    IRandomAccessStream stream = await imgFile.OpenAsync(FileAccessMode.Read);
                    imagetoResize = bmp;

                    bmp.SetSource(stream);
                    myBrush.ImageSource = bmp;
                    PhotoRectangle.Fill = myBrush;
                    this.DataContext = imgFile;
                    string imgFileName = Utils.GetRandomFileName();
                    Utils.SaveImage(imgFile, imgFileName);
                    imageUrl = imgFile.Path.ToString();
                    StorageFolder appFolder = ApplicationData.Current.RoamingFolder;
                    StorageFile imageFile = await appFolder.GetFileAsync(imgFileName);
                    bool isExists = File.Exists(imageFile.Path.ToString());
                    imageUrl = imageFile.Path.ToString();
                    if (TiteOfPage.Text.ToString().Equals("Edit Dream"))
                    {
                        SelectedDream.ImageUrl = imageUrl;
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
    }
}
