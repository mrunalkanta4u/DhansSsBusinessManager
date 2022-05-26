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
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Animation;
using DataAccounts;
using System.Linq;
using XamlCropControl;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;
using Windows.UI.Input;
using Windows.Foundation;
using WinRTXamlToolkit.Controls.Extensions;


namespace SDKTemplate
{
    public sealed partial class CropPhoto : Page
    {
        private MainPage rootPage = MainPage.Current;
        BitmapImage bmp = new BitmapImage(new Uri("ms-appx:///Assets/Account.png"));
        //String imageUrl = "Assets/Account.png";
        ImageBrush myBrush = new ImageBrush();
        public CropPhoto()
        {
            this.InitializeComponent();
        }

        private void SaveItem(object sender, RoutedEventArgs e)
        {

        }

        private void CancelSave(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NameList), "Cancel", new EntranceNavigationTransitionInfo());
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter as StorageFile != null)
            {
                //imageOfcontact.Source = e.Parameter as BitmapImage;
                //await this.ImageCropper.LoadImage(e.Parameter as StorageFile);
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
