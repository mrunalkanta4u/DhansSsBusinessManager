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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using System.Globalization;
using DataAccounts;
using System.Linq;

namespace SDKTemplate
{

    public sealed partial class AddNewTeamMember : Page
    {
        private MainPage rootPage = MainPage.Current;
        //private bool isLaunched = false;
        static ObservableCollection<TeamMember> TeamMembers = new ObservableCollection<TeamMember>();
        BitmapImage bmp = new BitmapImage(new Uri("ms-appx:///Assets/Account.png"));
        String imageUrl = "Assets/Account.png";
        ImageBrush myBrush = new ImageBrush();
        //Image img = new Image();
        TeamMember SelectedTeamMember;
        public AddNewTeamMember()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter as TeamMember != null)
            {
                TiteOfPage.Text = "Edit TeamMember";
                TeamMember teamMember = (e.Parameter as TeamMember);
                NameTextBox.Text = teamMember.Name;
                IRIDTextBox.Text = teamMember.IRId;
                IRPasswordTextBox.Text = teamMember.Password;
                CPAPINTextBox.Text = teamMember.CPAPassword;
                PhoneNumberTextBox.Text = teamMember.PhoneNumber;
                MailIDTextBox.Text = teamMember.MailId;
                MailIDPasswordTextBox.Text = teamMember.MailIdPassword;
                SecurityQATextBox.Text = teamMember.SecurityQA;
                SecurityWordTextBox.Text = teamMember.SecurityWord;
                RemarkTextBox.Text = teamMember.Remark;


                SelectedTeamMember = Utils.TeamMembers.Where((item) =>
                {
                    return
                        (item.Name.Trim().ToLower().Equals(teamMember.Name.Trim().ToLower()) &&
                        item.IRId.Trim().ToLower().Equals(teamMember.IRId.Trim().ToLower()))
                        ;
                }).FirstOrDefault();
                if (!SelectedTeamMember.ImageUrl.Equals("Assets/Account.png"))
                {
                    myBrush.ImageSource = new BitmapImage(new Uri(SelectedTeamMember.ImageUrl));
                    PhotoEllipse.Fill = myBrush;
                }
            }
        }

        private void SaveItem(object sender, RoutedEventArgs e)
        {
            bool KYCDone = false;
     
            if (NameTextBox.Text.ToString().Trim().Equals(string.Empty))
            {
                NameTextBox.PlaceholderText = "Name is Required !!!";
                NameTextBox.Focus(FocusState.Keyboard);
                return;
            }
            if (IRIDTextBox.Text.ToString().Trim().Equals(string.Empty))
            {
                IRIDTextBox.PlaceholderText = "IR ID is Required !!!";
                IRIDTextBox.Focus(FocusState.Keyboard);
                return;
            }
            if (TiteOfPage.Text.ToString().Equals("Edit TeamMember"))
            {
                KYCDone = SelectedTeamMember.KYCDone;
                //bmp = new BitmapImage(new Uri(SelectedTeamMember.ImageUrl));
                imageUrl = SelectedTeamMember.ImageUrl;
                Utils.TeamMembers.Remove(SelectedTeamMember);
                
            }
            this.Frame.Navigate(typeof(TeamList), new Model.TeamMember {
                Name = NameTextBox.Text.ToString(),
                PhoneNumber = PhoneNumberTextBox.Text.ToString(),
                MailId= MailIDTextBox.Text.ToString().ToLower(),
                MailIdPassword = MailIDPasswordTextBox.Text.ToString(),
                KYCDone = KYCDone,
                IRId = IRIDTextBox.Text.ToString().ToUpper(),
                Password = IRPasswordTextBox.Text.ToString(),
                CPAPassword = CPAPINTextBox.Text.ToString(),
                SecurityQA = SecurityQATextBox.Text.ToString(),
                SecurityWord = SecurityWordTextBox.Text.ToString(),
                Remark = RemarkTextBox.Text.ToString(),
                //Photo = bmp
                ImageUrl= imageUrl
            });
                //Frame.Navigate(typeof(TeamList), "Delete", new EntranceNavigationTransitionInfo());
        }
        private void CancelSave(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TeamList), "Cancel", new EntranceNavigationTransitionInfo());
        }
        
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
                BitmapImage imagetoResize;

                if (imgFile != null)
                {
                    IRandomAccessStream stream = await imgFile.OpenAsync(FileAccessMode.Read);
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
                    if (TiteOfPage.Text.ToString().Equals("Edit TeamMember"))
                    {
                        SelectedTeamMember.ImageUrl = imageUrl;
                    }
                }
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
            }
        }
    }
}
