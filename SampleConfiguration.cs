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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserDataAccounts;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using Windows.ApplicationModel.Email;
using SDKTemplate.Model;
using System.ComponentModel;
using Windows.UI.Xaml.Media.Animation;
using Windows.Data.Json;
using Windows.Storage;
using System.Linq;
using Windows.UI.Xaml.Media;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using System.Runtime.Serialization;
using System.Text;
using System.IO;
using Windows.ApplicationModel.Background;

namespace SDKTemplate
{
    public partial class MainPage : Page
    {
        public const string FEATURE_NAME = "My Business Management";

        //List<Scenario> scenarios = new List<Scenario>
        private ObservableCollection<Scenario> _scenarios = new ObservableCollection<Scenario>()
        {
            new Scenario() { Title="Name List", Symbol = Windows.UI.Xaml.Controls.Symbol.ContactInfo, ClassType=typeof(NameList)},
            new Scenario() { Title="Dream List", Symbol = Windows.UI.Xaml.Controls.Symbol.LeaveChat, ClassType=typeof(DreamList)},
            new Scenario() { Title="Team List", Symbol = Windows.UI.Xaml.Controls.Symbol.People, ClassType=typeof(TeamList)},
            new Scenario() { Title="Activities", Symbol = Windows.UI.Xaml.Controls.Symbol.PhoneBook, ClassType=typeof(Activity)},
            new Scenario() { Title="Miscellaneous", Symbol = Windows.UI.Xaml.Controls.Symbol.SwitchApps, ClassType=typeof(Miscellaneous)},
        };
        public ObservableCollection<Scenario> Scenarios
        {
            get { return _scenarios; }
        }

        // Fill the specified ComboBox with user data accounts.
        public async Task LoadDataAccountsAsync(ComboBox comboBox)
        {
            // Requires the email, contacts, or appointments capability in the app's manifest.
            UserDataAccountStore store = await UserDataAccountManager.RequestStoreAsync(UserDataAccountStoreAccessType.AllAccountsReadOnly);

            if (store != null)
            {
                IReadOnlyList<UserDataAccount> userDataAccounts = await store.FindAccountsAsync();
                comboBox.DataContext = new ObservableCollection<UserDataAccount>(userDataAccounts);

                if (userDataAccounts.Count > 0)
                {
                    comboBox.SelectedIndex = 0;
                }
            }
            else
            {
                // If the store is null, that means all access to Contacts, Calendar and Email data 
                // has been revoked.
                NotifyUser("Access to Contacts, Calendar and Email has been revoked", NotifyType.ErrorMessage);
            }
        }
    }

    public class Scenario
    {
        public string Title { get; set; }
        public Symbol Symbol { get; set; }
        public Type ClassType { get; set; }
    }
}
namespace SDKTemplate
{
    class BackgroundTaskSample
    {
        public const string SampleBackgroundTaskEntryPoint = "BackGroundTasks.AutomaticBackupTask";
        public const string SampleBackgroundTaskName = "AutomaticBackupTask";
        public static string SampleBackgroundTaskProgress = "";
        public static bool SampleBackgroundTaskRegistered = false;

        public const string SampleBackgroundTaskEntryPoint2 = "BackGroundTasks.AutomaticPurgeTask";
        public const string SampleBackgroundTaskName2 = "AutomaticPurgeTask";
        public static string SampleBackgroundTaskProgress2 = "";
        public static bool SampleBackgroundTaskRegistered2 = false;

        public const string SampleBackgroundTaskWithConditionName = "AutomaticBackupTaskWithCondition";
        public static string SampleBackgroundTaskWithConditionProgress = "";
        public static bool SampleBackgroundTaskWithConditionRegistered = false;

        public const string ServicingCompleteTaskEntryPoint = "BackGroundTasks.ServicingComplete";
        public const string ServicingCompleteTaskName = "ServicingCompleteTask";
        public static string ServicingCompleteTaskProgress = "";
        public static bool ServicingCompleteTaskRegistered = false;

        public const string TimeTriggeredTaskName = "TimeTriggeredTask";
        public static string TimeTriggeredTaskProgress = "";
        public static bool TimeTriggeredTaskRegistered = false;

        public const string ApplicationTriggerTaskName = "ApplicationTriggerTask";
        public static string ApplicationTriggerTaskProgress = "";
        public static string ApplicationTriggerTaskResult = "";
        public static bool ApplicationTriggerTaskRegistered = false;

        /// <summary>
        /// Register a background task with the specified taskEntryPoint, name, trigger,
        /// and condition (optional).
        /// </summary>
        /// <param name="taskEntryPoint">Task entry point for the background task.</param>
        /// <param name="name">A name for the background task.</param>
        /// <param name="trigger">The trigger for the background task.</param>
        /// <param name="condition">An optional conditional event that must be true for the task to fire.</param>
        public static async Task<BackgroundTaskRegistration> RegisterBackgroundTask(String taskEntryPoint, String name, IBackgroundTrigger trigger, IBackgroundCondition condition)
        {
            if (TaskRequiresBackgroundAccess(name))
            {
                await BackgroundExecutionManager.RequestAccessAsync();
            }
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                if (cur.Value.Name == name)
                {
                    return (BackgroundTaskRegistration)(cur.Value);
                }
            }
            var builder = new BackgroundTaskBuilder();

            builder.Name = name;
            builder.TaskEntryPoint = taskEntryPoint;
            builder.SetTrigger(trigger);

            if (condition != null)
            {
                builder.AddCondition(condition);

                //
                // If the condition changes while the background task is executing then it will
                // be canceled.
                //
                builder.CancelOnConditionLoss = true;
            }

            BackgroundTaskRegistration task = builder.Register();

            UpdateBackgroundTaskStatus(name, true);

            //
            // Remove previous completion status from local settings.
            //
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values.Remove(name);

            return task;
        }

        /// <summary>
        /// Unregister background tasks with specified name.
        /// </summary>
        /// <param name="name">Name of the background task to unregister.</param>
        public static void UnregisterBackgroundTasks(String name)
        {
            //
            // Loop through all background tasks and unregister any with SampleBackgroundTaskName or
            // SampleBackgroundTaskWithConditionName.
            //
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                if (cur.Value.Name == name)
                {
                    cur.Value.Unregister(true);
                }
            }

            UpdateBackgroundTaskStatus(name, false);
        }

        /// <summary>
        /// Store the registration status of a background task with a given name.
        /// </summary>
        /// <param name="name">Name of background task to store registration status for.</param>
        /// <param name="registered">TRUE if registered, FALSE if unregistered.</param>
        public static void UpdateBackgroundTaskStatus(String name, bool registered)
        {
            switch (name)
            {
                case SampleBackgroundTaskName:
                    SampleBackgroundTaskRegistered = registered;
                    break;
                case SampleBackgroundTaskWithConditionName:
                    SampleBackgroundTaskWithConditionRegistered = registered;
                    break;
                case ServicingCompleteTaskName:
                    ServicingCompleteTaskRegistered = registered;
                    break;
                case TimeTriggeredTaskName:
                    TimeTriggeredTaskRegistered = registered;
                    break;
                case ApplicationTriggerTaskName:
                    ApplicationTriggerTaskRegistered = registered;
                    break;
            }
        }

        /// <summary>
        /// Get the registration / completion status of the background task with
        /// given name.
        /// </summary>
        /// <param name="name">Name of background task to retreive registration status.</param>
        public static String GetBackgroundTaskStatus(String name)
        {
            var registered = false;
            switch (name)
            {
                case SampleBackgroundTaskName:
                    registered = SampleBackgroundTaskRegistered;
                    break;
                case SampleBackgroundTaskWithConditionName:
                    registered = SampleBackgroundTaskWithConditionRegistered;
                    break;
                case ServicingCompleteTaskName:
                    registered = ServicingCompleteTaskRegistered;
                    break;
                case TimeTriggeredTaskName:
                    registered = TimeTriggeredTaskRegistered;
                    break;
                case ApplicationTriggerTaskName:
                    registered = ApplicationTriggerTaskRegistered;
                    break;
            }

            var status = registered ? "Registered" : "Unregistered";

            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey(name))
            {
                status += " - " + settings.Values[name].ToString();
            }

            return status;
        }

        /// <summary>
        /// Determine if task with given name requires background access.
        /// </summary>
        /// <param name="name">Name of background task to query background access requirement.</param>
        public static bool TaskRequiresBackgroundAccess(String name)
        {
            if ((name == TimeTriggeredTaskName) ||
                (name == ApplicationTriggerTaskName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}