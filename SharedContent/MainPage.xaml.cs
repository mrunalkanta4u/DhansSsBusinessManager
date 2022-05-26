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

using SDKTemplate.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DataAccounts;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Input;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using BackGroundTasks;
using Windows.Storage;
using System.IO;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SDKTemplate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        //private static ObservableCollection<CallLog> CallLogs = new ObservableCollection<CallLog>();

        public MainPage()
        {
            this.InitializeComponent();

            // This is a static public property that allows downstream pages to get a handle to the MainPage instance
            // in order to call methods that are in this class.
            Current = this;
            SampleTitle.Text = FEATURE_NAME;
            //RegisterBackgroundTask();
            //AutoBackUp(true, 1);

        }

        //public async void AutoBackUp(bool isBackupEnabled,int frequency)
        //{
        //    //string backupFileName = "Backup" + DateTime.Now.ToString("-dd-MMMM-yyyy-hh:mm:ss") + ".zip";
        //    ////StorageFolder backupFolder = await KnownFolders.DocumentsLibrary.CreateFolderAsync("DhansSsBizMgrBackup\\", CreationCollisionOption.ReplaceExisting);
        //    ////StorageFile backupFile = await backupFolder.CreateFileAsync(backupFileName);
        //    //var tempFolder = Windows.Storage.ApplicationData.Current.TemporaryFolder;
        //    ////tempFolder = await tempFolder.CreateFolderAsync("DhansSsBizMgrBackup\\", CreationCollisionOption.ReplaceExisting);
        //    //StorageFile fileToCopy = await tempFolder.CreateFileAsync("Mrunal.txt");
        //    //await fileToCopy.CopyAsync(KnownFolders.DocumentsLibrary, "Mrunal.txt", NameCollisionOption.ReplaceExisting);
        //    //await fileToCopy.DeleteAsync(StorageDeleteOption.Default);
        //    try
        //    {
        //        var tempFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
        //        //tempFolder = await tempFolder.GetFoldersAsync("DhansSsBizMgrBackup\\", CreationCollisionOption.ReplaceExisting);
        //        StorageFile backupFile = await tempFolder.GetFileAsync("MRUNALTEST2.txt");
        //        if (File.Exists(backupFile.Path))
        //        {
        //            string ab = backupFile.Path;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        String msg = ex.Message;
        //    }

        //}

        //private async void RegisterBackgroundTask()
        //{
        //    var taskName = "BackgroundTasks";
        //    // On Windows, this method presents the user with a dialog box that requests that 
        //    // an app be added to the lock screen. On Windows Phone, this method does not prompt 
        //    // the user, but must be called before registering any background tasks. 
        //    var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
        //    if (backgroundAccessStatus == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity
        //            || backgroundAccessStatus == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
        //    {
        //        foreach (var task in BackgroundTaskRegistration.AllTasks)
        //        {
        //            if (task.Value.Name == taskName)
        //            {
        //                return;
        //            }
        //        }
        //        // Create BackgroundTaskBuilder instance, setting the task entry point 
        //        // and set a TimeTrigger (should be at least 15 minutes) 
        //        BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
        //        taskBuilder.Name = taskName;
        //        taskBuilder.TaskEntryPoint = typeof(BackGroundTasks.AutomaticBackupTask).FullName;
        //        taskBuilder.SetTrigger(new TimeTrigger(120, false));
        //        // should be at least 15 minutes 

        //        // Register the background task with the system 
        //        var registration = taskBuilder.Register();
        //    }
        //}


        private void ShowSliptView(object sender, RoutedEventArgs e)
        {
            MySamplesPane.SamplesSplitView.IsPaneOpen = !MySamplesPane.SamplesSplitView.IsPaneOpen;
            if (!MySamplesPane.SamplesSplitView.IsPaneOpen)
                this.BottomAppBar.Visibility = Visibility.Collapsed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Populate the scenario list from the SampleConfiguration.cs file
            ScenarioControl.ItemsSource = _scenarios;
            if (Window.Current.Bounds.Width < 640)
            {
                ScenarioControl.SelectedIndex = -1;
            }
            else
            {
                ScenarioControl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Called whenever the user changes selection in the scenarios list.  This method will navigate to the respective
        /// sample scenario page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScenarioControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear the status block when navigating scenarios.
            NotifyUser(String.Empty, NotifyType.StatusMessage);

            ListView scenarioListBox = sender as ListView;
            Scenario s = scenarioListBox.SelectedItem as Scenario;
            if (s != null)
            {
                ScenarioFrame.Navigate(s.ClassType);
                
                if (Window.Current.Bounds.Width < 640)
                {
                    Splitter.IsPaneOpen = false;
                }
            }
        }

        //public List<Scenario> Scenarios
        //{
        //    get { return this.scenarios; }
        //}


        /// <summary>
        /// Used to display messages to the user
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="type"></param>
        public void NotifyUser(string strMessage, NotifyType type)
        {
            switch (type)
            {
                case NotifyType.StatusMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                    break;
                case NotifyType.WarningMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
                    break;
                case NotifyType.ErrorMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                    break;
            }
            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            StatusBorder.Visibility = (StatusBlock.Text != String.Empty) ? Visibility.Visible : Visibility.Collapsed;
            if (StatusBlock.Text != String.Empty)
            {
                StatusBorder.Visibility = Visibility.Visible;
                //StatusPanel.Visibility = Visibility.Visible;
            }
            else
            {
                StatusBorder.Visibility = Visibility.Collapsed;
                //StatusPanel.Visibility = Visibility.Collapsed;
            }
            //StatusBorder.Tapped += StatusBorder_GotFocus;
            //EnterStoryboard.Begin();
            //Task.Delay(5000);
            //ExitStoryboard.Begin();
        }
        //private void StatusBorder_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    ExitStoryboard.Begin();
        //}

        private void StatusBorder_LostFocus(object sender, RoutedEventArgs e)
        {
            EnterStoryboard.Begin();
            //StatusPanel.Visibility = Visibility.Collapsed;
        }
        private void Rectangle_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            EnterStoryboard.Begin();
            //StatusPanel.Visibility = Visibility.Collapsed;
        }

        private void Rectangle_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //ExitStoryboard.Begin();
        }

        async void Footer_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(((HyperlinkButton)sender).Tag.ToString()));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
            ScenarioControl.SelectedIndex = -1;
#if !WINDOWS_PHONE
            Frame.Navigate(typeof(MainPage), "Home", new EntranceNavigationTransitionInfo());
#endif
        }

        private void ScenareoList_ItemClick(object sender, ItemClickEventArgs e)
        {
            //content.Text = (e.ClickedItem as NavLink).Label + " Page";
        }

        private void OnExitTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            App.Current.Exit();
        }

        private void OnTitleTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), "Home", new EntranceNavigationTransitionInfo());
        }

        private void OnSettingTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ScenarioFrame.Navigate(typeof(Setting));

            if (Window.Current.Bounds.Width < 640)
            {
                Splitter.IsPaneOpen = false;
            }
        }

      
    }
    public enum NotifyType
    {
        StatusMessage,
        WarningMessage,
        ErrorMessage
    };
    //private void togglePaneButton_Click(object sender, RoutedEventArgs e)
    //{
    //    if (Window.Current.Bounds.Width >= 640)
    //    {
    //        if (splitView.IsPaneOpen)
    //        {
    //            splitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
    //            splitView.IsPaneOpen = false;
    //        }
    //        else
    //        {
    //            splitView.IsPaneOpen = true;
    //            splitView.DisplayMode = SplitViewDisplayMode.Inline;
    //        }
    //    }
    //    else
    //    {
    //        splitView.IsPaneOpen = !splitView.IsPaneOpen;
    //    }
    //}
    public class ScenarioBindingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Scenario s = value as Scenario;
            //return (MainPage.Current.Scenarios.IndexOf(s) + 1) + ") " + s.Title;
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return true;
        }
    }
    
}
