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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Documents;
using DataAccounts;
using Windows.ApplicationModel.Contacts;
using Windows.Storage.Pickers;
using System.Xml;
using System.IO.Compression;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Windows.ApplicationModel.Email;
using System.ComponentModel;
using Windows.UI.Xaml.Media.Animation;
using Windows.Data.Json;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media.Imaging;
using System.Text;
using Windows.UI.Xaml.Data;
using Windows.System;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Windows.UI.Core;

namespace SDKTemplate
{

    public sealed partial class Setting : Page
    {
        private MainPage rootPage = MainPage.Current;
        ApplicationDataContainer roamingSettings = null;
        //const string settingName = "AllCompositeSetting";
        const string autoBackupEnabledSettingName = "autoBackupEnabled";
        const string autoBackupFrequencySettingName = "autoBackupFrequency";
        const string autoPurgeEnabledSettingName = "autoPurgeEnabled";
        const string autoPurgeFrequencySettingName = "autoPurgeFrequency";
        const string displayThemeSettingName = "displayTheme";
        const string uplinePhoneNoSettingName = "uplinePhoneNo";
        const string uplineEmailIDSettingName = "uplineEmailID";
        const string reportingModeSettingName = "reportingMode";
        const string reportingCounterSettingName = "reportingCounter";
        const string directReferalCountSettingName = "directReferalCount";
        const string systemCountSettingName = "systemCount";
        const string uvCountSettingName = "uvCount";

        public Setting()
        {
            this.InitializeComponent();
            //roamingSettings = ApplicationData.Current.LocalSettings;
            roamingSettings = ApplicationData.Current.RoamingSettings;
            if (roamingSettings == null)
            {
                roamingSettings.Values[autoBackupEnabledSettingName] = false;
                roamingSettings.Values[autoBackupFrequencySettingName] = 1;
                roamingSettings.Values[autoPurgeEnabledSettingName] = false;
                roamingSettings.Values[autoPurgeFrequencySettingName] = 1;
                roamingSettings.Values[displayThemeSettingName] = 0;
                roamingSettings.Values[uplinePhoneNoSettingName] = "9663865026";
                roamingSettings.Values[uplineEmailIDSettingName] = "mrunalkanta4u@gmail.com";
                roamingSettings.Values[reportingModeSettingName] = 0;
                roamingSettings.Values[reportingCounterSettingName] = 0;
                roamingSettings.Values[directReferalCountSettingName] = 0;
                roamingSettings.Values[systemCountSettingName] = 0;
                roamingSettings.Values[uvCountSettingName] = 0;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (roamingSettings.Values[autoBackupEnabledSettingName] != null)
                AutoBackupSwitch.IsOn = (bool)roamingSettings.Values[autoBackupEnabledSettingName]; // Values: 1 = On , 0 = Off
            else
                AutoBackupSwitch.IsOn = false;
            BackupFrequencyComboBox.IsEnabled = AutoBackupSwitch.IsOn;
            if (roamingSettings.Values[autoBackupFrequencySettingName] != null)
                BackupFrequencyComboBox.SelectedIndex = (int)roamingSettings.Values[autoBackupFrequencySettingName]; // Values: 0 = Daily , 1 = Weekly , 2 = Monthly
            else
                BackupFrequencyComboBox.SelectedIndex = 1;
            //if (roamingSettings.Values[autoPurgeEnabledSettingName] != null)
            //    AutoPurgeSwitch.IsOn = (bool)roamingSettings.Values[autoPurgeEnabledSettingName]; // Values: 1 = On , 0 = Off
            //else
            //    AutoPurgeSwitch.IsOn = false;
            //PurgeFrequencyComboBox.IsEnabled = AutoPurgeSwitch.IsOn;

            if (roamingSettings.Values[autoPurgeFrequencySettingName] != null)
                PurgeFrequencyComboBox.SelectedIndex = (int)roamingSettings.Values[autoPurgeFrequencySettingName]; // Values: 0 = Daily , 1 = Weekly , 2 = Monthly
            else
                PurgeFrequencyComboBox.SelectedIndex = 2;
            if (roamingSettings.Values[displayThemeSettingName] != null)
                ThemeSelectedComboBox.SelectedIndex = (int)roamingSettings.Values[displayThemeSettingName];
            else
                ThemeSelectedComboBox.SelectedIndex = 0;
            if (roamingSettings.Values[uplinePhoneNoSettingName] != null)
                UplinePhoneNoTextBox.Text = roamingSettings.Values[uplinePhoneNoSettingName].ToString();
            else
                UplinePhoneNoTextBox.Text = "9663865026";
            if (roamingSettings.Values[uplineEmailIDSettingName] != null)
                UplineEMailIDTextBox.Text = roamingSettings.Values[uplineEmailIDSettingName].ToString();
            else
                UplineEMailIDTextBox.Text = "mrunalkanta4u@gmail.com";
            if (roamingSettings.Values[reportingModeSettingName] != null)
                ReportingModeComboBox.SelectedIndex = (int)roamingSettings.Values[reportingModeSettingName];
            else
                ReportingModeComboBox.SelectedIndex = 0;
            if (roamingSettings.Values[reportingCounterSettingName] != null)
                Counter.Text = "108 Counter : " + (int)roamingSettings.Values[reportingCounterSettingName];
            else
                Counter.Text = "108 Counter : " + 1; //+ 0.ToString();
        }


        //void DeleteCompositeSetting_Click(Object sender, RoutedEventArgs e)
        //{
        //    roamingSettings.Values.Remove(settingName);
        //}
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == BackgroundTaskSample.SampleBackgroundTaskName)
                {
                    AttachProgressAndCompletedHandlers(task.Value);
                    BackgroundTaskSample.UpdateBackgroundTaskStatus(BackgroundTaskSample.SampleBackgroundTaskName, true);
                    break;
                }
            }
            UpdateUI();
        }

        /// <summary>
        /// Register a TimeTriggeredTask.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RegisterBackgroundTask(object sender, RoutedEventArgs e)
        {
            var task = BackgroundTaskSample.RegisterBackgroundTask(BackgroundTaskSample.SampleBackgroundTaskEntryPoint,
                                                                   BackgroundTaskSample.TimeTriggeredTaskName,
                                                                   new TimeTrigger(15, false),
                                                                   null);
            await task;
            AttachProgressAndCompletedHandlers(task.Result);
            UpdateUI();
        }

        /// <summary>
        /// Unregister a TimeTriggeredTask.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnregisterBackgroundTask(object sender, RoutedEventArgs e)
        {
            BackgroundTaskSample.UnregisterBackgroundTasks(BackgroundTaskSample.TimeTriggeredTaskName);
            UpdateUI();
        }

        /// <summary>
        /// Attach progress and completed handers to a background task.
        /// </summary>
        /// <param name="task">The task to attach progress and completed handlers to.</param>
        private void AttachProgressAndCompletedHandlers(IBackgroundTaskRegistration task)
        {
            task.Progress += new BackgroundTaskProgressEventHandler(OnProgress);
            task.Completed += new BackgroundTaskCompletedEventHandler(OnCompleted);
        }

        /// <summary>
        /// Handle background task progress.
        /// </summary>
        /// <param name="task">The task that is reporting progress.</param>
        /// <param name="e">Arguments of the progress report.</param>
        private void OnProgress(IBackgroundTaskRegistration task, BackgroundTaskProgressEventArgs args)
        {
            var progress = "Auto BackUp Progress: " + args.Progress + "%";
            BackgroundTaskSample.TimeTriggeredTaskProgress = progress;
            UpdateUI();
        }

        /// <summary>
        /// Handle background task completion.
        /// </summary>
        /// <param name="task">The task that is reporting completion.</param>
        /// <param name="e">Arguments of the completion report.</param>
        private void OnCompleted(IBackgroundTaskRegistration task, BackgroundTaskCompletedEventArgs args)
        {

            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var key = task.TaskId.ToString();
            //var message = settings.Values[key].ToString();
            //notification
            //Show notification
            try
            {
                var xml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText01);

                //Set toast image
                ((Windows.Data.Xml.Dom.XmlElement)xml.GetElementsByTagName("image")[0]).SetAttribute("src", "Assets/DhansSsBizMgr.png");

                //Set toast text
                xml.GetElementsByTagName("text")[0].AppendChild(xml.CreateTextNode("DhansSs Business Manager Auto Backup Complete!"));

                //Show toast
                ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(xml));
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
            }
            UpdateUI();

        }

        /// <summary>
        /// Update the scenario UI.
        /// </summary>
        private async void UpdateUI()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                //RegisterButton.IsEnabled = !BackgroundTaskSample.TimeTriggeredTaskRegistered;
                //UnregisterButton.IsEnabled = BackgroundTaskSample.TimeTriggeredTaskRegistered;
                //Progress.Text = BackgroundTaskSample.TimeTriggeredTaskProgress;
                //Status.Text = BackgroundTaskSample.GetBackgroundTaskStatus(BackgroundTaskSample.TimeTriggeredTaskName);
            });
        }

        private void AutoBackupSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            roamingSettings.Values[autoBackupEnabledSettingName] = AutoBackupSwitch.IsOn;
            BackupFrequencyComboBox.IsEnabled = AutoBackupSwitch.IsOn;
            EnableAutoBackup();
        }
        private async void EnableAutoRestore()
        {
            var roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            try
            {
                if (Directory.Exists(Path.Combine(roamingFolder.Path.ToString(), "DhansSsBackup")))
                    {
                        StorageFolder backupFolder = await roamingFolder.GetFolderAsync("DhansSsBackup\\");
                    //StorageFile backupFile = await roamingFolder.CreateFileAsync("DhansSsBizMgrAutoBackup.zip", CreationCollisionOption.ReplaceExisting);

                    if (File.Exists(Path.Combine(backupFolder.Path.ToString(), "MyNameList.json")))
                    {
                        StorageFile fileToCopy = await backupFolder.GetFileAsync("MyNameList.json");
                        await fileToCopy.CopyAsync(roamingFolder, "MyNameList.json", NameCollisionOption.ReplaceExisting);
                    }
                    if (File.Exists(Path.Combine(backupFolder.Path.ToString(), "MyDreamList.json")))
                    {
                        StorageFile fileToCopy = await backupFolder.GetFileAsync("MyDreamList.json");
                        await fileToCopy.CopyAsync(roamingFolder, "MyDreamList.json", NameCollisionOption.ReplaceExisting);
                    }
                    if (File.Exists(Path.Combine(backupFolder.Path.ToString(), "MyTeamList.json")))
                    {
                        StorageFile fileToCopy = await backupFolder.GetFileAsync("MyTeamList.json");
                        await fileToCopy.CopyAsync(roamingFolder, "MyTeamList.json", NameCollisionOption.ReplaceExisting);
                    }
                    if (File.Exists(Path.Combine(backupFolder.Path.ToString(), "MyCallLogs.json")))
                    {
                        StorageFile fileToCopy = await backupFolder.GetFileAsync("MyCallLogs.json");
                        await fileToCopy.CopyAsync(roamingFolder, "MyCallLogs.json", NameCollisionOption.ReplaceExisting);
                    }
                }
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
            }
        }
        private async void EnableAutoBackup()
        {
            uint backupFrequency = 10080; // default is week
            try
            {
                if (AutoBackupSwitch.IsOn)
                {
                    if (BackupFrequencyComboBox.SelectedIndex == 0) //SelectedItem.ToString().Trim().Equals("Daily"))
                        backupFrequency = 1440;
                    if (BackupFrequencyComboBox.SelectedIndex == 1) //SelectedItem.ToString().Trim().Equals("Weekly"))
                        backupFrequency = 10080;
                    if (BackupFrequencyComboBox.SelectedIndex == 2) //SelectedItem.ToString().Trim().Equals("Monthly"))
                        backupFrequency = 43800;

                    //BackgroundTaskRegistration task = await RegisterBackgroundTask("BackGroundTasks.AutomaticBackupTask", "AutomaticBackupTask",
                    //    new TimeTrigger(backupFrequency, false), null);

                    var task = BackgroundTaskSample.RegisterBackgroundTask(BackgroundTaskSample.SampleBackgroundTaskEntryPoint,
                                                                   BackgroundTaskSample.SampleBackgroundTaskName,
                                                                   new TimeTrigger(backupFrequency, false),
                                                                   null);
                    //task.Completed += new BackgroundTaskCompletedEventHandler(OnCompleted);
                    await task;
                    AttachProgressAndCompletedHandlers(task.Result);
                    UpdateUI();
                }
                else
                {
                    BackgroundTaskSample.UnregisterBackgroundTasks(BackgroundTaskSample.SampleBackgroundTaskName);
                    UpdateUI();
                }
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
            }
        }
        
        private void BackupFrequencyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            roamingSettings.Values[autoBackupFrequencySettingName] = BackupFrequencyComboBox.SelectedIndex; // Values: 0 = Daily , 1 = Weekly , 1 = Monthly
            BackgroundTaskSample.UnregisterBackgroundTasks(BackgroundTaskSample.SampleBackgroundTaskName);

            EnableAutoBackup();
        }

        private void ThemeSelectedComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            roamingSettings.Values[displayThemeSettingName] = ThemeSelectedComboBox.SelectedIndex;
            var settings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            string currentTheme = null;
            if (settings.Values.ContainsKey("currentTheme"))
            {
                currentTheme = settings.Values["currentTheme"] as string;
                settings.Values.Remove("currentTheme");
            }

            settings.Values.Add("currentTheme", ThemeSelectedComboBox.SelectedIndex == 0 ? "dark" : "light");

            if (settings.Values.ContainsKey("currentTheme") && (string)settings.Values["currentTheme"] == "light")
                RequestedTheme = ElementTheme.Light;
            else
                RequestedTheme = ElementTheme.Dark;
        }

        private void UplineContactNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void UplineEMailIDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        

        private void UplinePhoneNoButton_Click(object sender, RoutedEventArgs e)
        {
            roamingSettings.Values[uplinePhoneNoSettingName] = UplinePhoneNoTextBox.Text.Trim().ToString();
        }

        private void UplineEMailIDButton_Click(object sender, RoutedEventArgs e)
        {
            roamingSettings.Values[uplineEmailIDSettingName] = UplineEMailIDTextBox.Text.Trim().ToString();
        }

        private void ReportingModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            roamingSettings.Values[reportingModeSettingName] = ReportingModeComboBox.SelectedIndex;
        }

        private void Reset108CounterButton_Click(object sender, RoutedEventArgs e)
        {
            roamingSettings.Values[reportingCounterSettingName] = 1;
            Counter.Text = "108 Counter : " + 1;
            CounterResetConfirmationFlyout.Hide();
        }

        private void AutoBackupSwitch_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void AutoRestoreButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            EnableAutoRestore();
            AutoRestoreConfirmationFlyout.Hide();
        }

        //private void AutoPurgeSwitch_Toggled(object sender, RoutedEventArgs e)
        //{
        //    roamingSettings.Values[autoPurgeEnabledSettingName] = AutoPurgeSwitch.IsOn;
        //    PurgeFrequencyComboBox.IsEnabled = AutoPurgeSwitch.IsOn;
        //    EnableAutoPurge();
        //}

        //private async void EnableAutoPurge()
        //{
        //    uint purgeFrequency = 10080; // default is week
        //    try
        //    {
        //        if (AutoPurgeSwitch.IsOn)
        //        {
        //            if (PurgeFrequencyComboBox.SelectedIndex == 0) //SelectedItem.ToString().Trim().Equals("Weekly"))
        //                purgeFrequency = 10080;
        //            if (PurgeFrequencyComboBox.SelectedIndex == 1) //SelectedItem.ToString().Trim().Equals("Monthly"))
        //                purgeFrequency = 43800;
        //            if (PurgeFrequencyComboBox.SelectedIndex == 2) //SelectedItem.ToString().Trim().Equals("Yearly"))
        //                purgeFrequency = 525600;

        //            //BackgroundTaskRegistration task = await RegisterBackgroundTask("BackGroundTasks.AutomaticPurgeTask", "AutomaticPurgeTask",
        //            //    new TimeTrigger(purgeFrequency, false), null);

        //            var task = BackgroundTaskSample.RegisterBackgroundTask(BackgroundTaskSample.SampleBackgroundTaskEntryPoint2,
        //                                                           BackgroundTaskSample.SampleBackgroundTaskName2,
        //                                                           new TimeTrigger(purgeFrequency, false),
        //                                                           null);
        //            //task.Completed += new BackgroundTaskCompletedEventHandler(OnCompleted);
        //            await task;
        //            AttachProgressAndCompletedHandlers(task.Result);
        //            UpdateUI();
        //        }
        //        else
        //        {
        //            BackgroundTaskSample.UnregisterBackgroundTasks(BackgroundTaskSample.SampleBackgroundTaskName);
        //            UpdateUI();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        String ExceptionMessage = ex.Message;
        //    }

        //}

        private void PurgeFrequencyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            roamingSettings.Values[autoPurgeFrequencySettingName] = PurgeFrequencyComboBox.SelectedIndex; // Values: 0 = Daily , 1 = Weekly , 1 = Monthly
            BackgroundTaskSample.UnregisterBackgroundTasks(BackgroundTaskSample.SampleBackgroundTaskName);
            //EnableAutoPurge();
        }

        //private void AutoPurgeSwitch_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{

        //}

        //private void AutoPurgeButton_Click(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
