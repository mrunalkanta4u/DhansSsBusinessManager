using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.System.Threading;
using Windows.UI.Notifications;

namespace BackGroundTasks
{
    public sealed class AutomaticPurgeTask : IBackgroundTask
    {
        BackgroundTaskCancellationReason _cancelReason = BackgroundTaskCancellationReason.Abort;
        volatile bool _cancelRequested = false;
        BackgroundTaskDeferral _deferral = null;
        ThreadPoolTimer _periodicTimer = null;
        uint _progress = 0;
        IBackgroundTaskInstance _taskInstance = null;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {


                Debug.WriteLine("ServicingComplete " + taskInstance.Task.Name + " starting...");

                //
                // Associate a cancellation handler with the background task.
                //
                taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);

                //
                // Do background task activity for servicing complete.
                //
                uint Progress;
                for (Progress = 0; Progress <= 100; Progress += 10)
                {
                    //
                    // If the cancellation handler indicated that the task was canceled, stop doing the task.
                    //
                    if (_cancelRequested)
                    {
                        break;
                    }

                    //
                    // Indicate progress to foreground application.
                    //
                    taskInstance.Progress = Progress;
                }

                var settings = ApplicationData.Current.LocalSettings;
                var key = taskInstance.Task.Name;

                //
                // Write to LocalSettings to indicate that this background task ran.
                //
                settings.Values[key] = (Progress < 100) ? "Canceled" : "Completed";
                Debug.WriteLine("ServicingComplete " + taskInstance.Task.Name + ((Progress < 100) ? " Canceled" : " Completed"));
                
            //
            // Query BackgroundWorkCost
            // Guidance: If BackgroundWorkCost is high, then perform only the minimum amount
            // of work in the background task and return immediately.
            //
            var cost = BackgroundWorkCost.CurrentBackgroundWorkCost;
            settings.Values["BackgroundWorkCost"] = cost.ToString();

            
            _deferral = taskInstance.GetDeferral();
            _taskInstance = taskInstance;

            _periodicTimer = ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler(PeriodicTimerCallback), TimeSpan.FromSeconds(1));

            var roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            StorageFolder purgeFolder = await roamingFolder.CreateFolderAsync("DhansSsPurge\\", CreationCollisionOption.ReplaceExisting);
            StorageFile purgeFile = await roamingFolder.CreateFileAsync("DhansSsBizMgrAutoPurge.zip", CreationCollisionOption.ReplaceExisting);

            if (File.Exists(Path.Combine( roamingFolder.Path.ToString(), "MyNameList.json")))
            {
                StorageFile fileToCopy = await roamingFolder.GetFileAsync("MyNameList.json");
                await fileToCopy.CopyAsync(purgeFolder, "MyNameList.json", NameCollisionOption.ReplaceExisting);
            }
            if (File.Exists(Path.Combine(roamingFolder.Path.ToString(), "MyDreamList.json")))
            {
                StorageFile fileToCopy = await roamingFolder.GetFileAsync("MyDreamList.json");
                await fileToCopy.CopyAsync(purgeFolder, "MyDreamList.json", NameCollisionOption.ReplaceExisting);
            }
            if (File.Exists(Path.Combine(roamingFolder.Path.ToString(), "MyTeamList.json")))
            {
                StorageFile fileToCopy = await roamingFolder.GetFileAsync("MyTeamList.json");
                await fileToCopy.CopyAsync(purgeFolder, "MyTeamList.json", NameCollisionOption.ReplaceExisting);
            }
            if (File.Exists(Path.Combine(roamingFolder.Path.ToString(), "MyCallLogs.json")))
            {
                StorageFile fileToCopy = await roamingFolder.GetFileAsync("MyCallLogs.json");
                await fileToCopy.CopyAsync(purgeFolder, "MyCallLogs.json", NameCollisionOption.ReplaceExisting);
            }

            //Archive(purgeFolder, purgeFile);
            // purge to cloud or local storage
            
            await purgeFile.DeleteAsync(StorageDeleteOption.Default);

            _deferral.Complete();
        }
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
                ((XmlElement)xml.GetElementsByTagName("image")[0]).SetAttribute("src", "Assets/SmallTile-sdk.scale-200.png");

                //Set toast text
                xml.GetElementsByTagName("text")[0].AppendChild(xml.CreateTextNode("DhansSs Business Manager Auto Purge Complete!"));

                //Show toast
                ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(xml));
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
            }
        }

        //
        // Handles background task cancellation.
        //
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            //
            // Indicate that the background task is canceled.
            //
            _cancelRequested = true;
            _cancelReason = reason;

            Debug.WriteLine("Background " + sender.Task.Name + " Cancel Requested...");
        }

        //
        // Simulate the background task activity.
        //
        private void PeriodicTimerCallback(ThreadPoolTimer timer)
        {
            if ((_cancelRequested == false) && (_progress < 100))
            {
                _progress += 10;
                _taskInstance.Progress = _progress;
            }
            else
            {
                _periodicTimer.Cancel();

                var settings = ApplicationData.Current.LocalSettings;
                var key = _taskInstance.Task.Name;

                //
                // Write to LocalSettings to indicate that this background task ran.
                //
                settings.Values[key] = (_progress < 100) ? "Canceled with reason: " + _cancelReason.ToString() : "Completed";
                Debug.WriteLine("Background " + _taskInstance.Task.Name + settings.Values[key]);

                //
                // Indicate that the background task has completed.
                //
                _deferral.Complete();
            }
        }

        public static async void Archive(StorageFolder directory, StorageFile purgeFileName)
        {
            string fileName = GetRandomFileName() + ".zip";
            string zipFilePath = System.IO.Path.Combine(ApplicationData.Current.TemporaryFolder.Path, fileName);
            try
            {
                await Task.Run(() => ZipFile.CreateFromDirectory(directory.Path, zipFilePath));
                StorageFile zipFile = await ApplicationData.Current.TemporaryFolder.GetFileAsync(fileName);
                await zipFile.CopyAndReplaceAsync(purgeFileName);
                await zipFile.DeleteAsync(StorageDeleteOption.Default);
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
            }
        }
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public static string GetRandomFileName()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }


    }

    public sealed class AutomaticPurgeTask : IBackgroundTask
    {
        BackgroundTaskCancellationReason _cancelReason = BackgroundTaskCancellationReason.Abort;
        volatile bool _cancelRequested = false;
        BackgroundTaskDeferral _deferral = null;
        ThreadPoolTimer _periodicTimer = null;
        uint _progress = 0;
        IBackgroundTaskInstance _taskInstance = null;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {


            Debug.WriteLine("ServicingComplete " + taskInstance.Task.Name + " starting...");

            //
            // Associate a cancellation handler with the background task.
            //
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);

            //
            // Do background task activity for servicing complete.
            //
            uint Progress;
            for (Progress = 0; Progress <= 100; Progress += 10)
            {
                //
                // If the cancellation handler indicated that the task was canceled, stop doing the task.
                //
                if (_cancelRequested)
                {
                    break;
                }

                //
                // Indicate progress to foreground application.
                //
                taskInstance.Progress = Progress;
            }

            var settings = ApplicationData.Current.LocalSettings;
            var key = taskInstance.Task.Name;

            //
            // Write to LocalSettings to indicate that this background task ran.
            //
            settings.Values[key] = (Progress < 100) ? "Canceled" : "Completed";
            Debug.WriteLine("ServicingComplete " + taskInstance.Task.Name + ((Progress < 100) ? " Canceled" : " Completed"));

            //
            // Query BackgroundWorkCost
            // Guidance: If BackgroundWorkCost is high, then perform only the minimum amount
            // of work in the background task and return immediately.
            //
            var cost = BackgroundWorkCost.CurrentBackgroundWorkCost;
            settings.Values["BackgroundWorkCost"] = cost.ToString();


            _deferral = taskInstance.GetDeferral();
            _taskInstance = taskInstance;

            _periodicTimer = ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler(PeriodicTimerCallback), TimeSpan.FromSeconds(1));

            var roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            StorageFolder purgeFolder = await roamingFolder.CreateFolderAsync("DhansSsPurge\\", CreationCollisionOption.ReplaceExisting);
            StorageFile purgeFile = await roamingFolder.CreateFileAsync("DhansSsBizMgrAutoPurge.zip", CreationCollisionOption.ReplaceExisting);

            if (File.Exists(Path.Combine(roamingFolder.Path.ToString(), "MyNameList.json")))
            {
                StorageFile fileToCopy = await roamingFolder.GetFileAsync("MyNameList.json");
                await fileToCopy.CopyAsync(purgeFolder, "MyNameList.json", NameCollisionOption.ReplaceExisting);
            }
            if (File.Exists(Path.Combine(roamingFolder.Path.ToString(), "MyDreamList.json")))
            {
                StorageFile fileToCopy = await roamingFolder.GetFileAsync("MyDreamList.json");
                await fileToCopy.CopyAsync(purgeFolder, "MyDreamList.json", NameCollisionOption.ReplaceExisting);
            }
            if (File.Exists(Path.Combine(roamingFolder.Path.ToString(), "MyTeamList.json")))
            {
                StorageFile fileToCopy = await roamingFolder.GetFileAsync("MyTeamList.json");
                await fileToCopy.CopyAsync(purgeFolder, "MyTeamList.json", NameCollisionOption.ReplaceExisting);
            }
            if (File.Exists(Path.Combine(roamingFolder.Path.ToString(), "MyCallLogs.json")))
            {
                StorageFile fileToCopy = await roamingFolder.GetFileAsync("MyCallLogs.json");
                await fileToCopy.CopyAsync(purgeFolder, "MyCallLogs.json", NameCollisionOption.ReplaceExisting);
            }

            //Archive(purgeFolder, purgeFile);
            // purge to cloud or local storage

            await purgeFile.DeleteAsync(StorageDeleteOption.Default);

            _deferral.Complete();
        }
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
                ((XmlElement)xml.GetElementsByTagName("image")[0]).SetAttribute("src", "Assets/SmallTile-sdk.scale-200.png");

                //Set toast text
                xml.GetElementsByTagName("text")[0].AppendChild(xml.CreateTextNode("DhansSs Business Manager Auto Purge Complete!"));

                //Show toast
                ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(xml));
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
            }
        }

        //
        // Handles background task cancellation.
        //
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            //
            // Indicate that the background task is canceled.
            //
            _cancelRequested = true;
            _cancelReason = reason;

            Debug.WriteLine("Background " + sender.Task.Name + " Cancel Requested...");
        }

        //
        // Simulate the background task activity.
        //
        private void PeriodicTimerCallback(ThreadPoolTimer timer)
        {
            if ((_cancelRequested == false) && (_progress < 100))
            {
                _progress += 10;
                _taskInstance.Progress = _progress;
            }
            else
            {
                _periodicTimer.Cancel();

                var settings = ApplicationData.Current.LocalSettings;
                var key = _taskInstance.Task.Name;

                //
                // Write to LocalSettings to indicate that this background task ran.
                //
                settings.Values[key] = (_progress < 100) ? "Canceled with reason: " + _cancelReason.ToString() : "Completed";
                Debug.WriteLine("Background " + _taskInstance.Task.Name + settings.Values[key]);

                //
                // Indicate that the background task has completed.
                //
                _deferral.Complete();
            }
        }

        public static async void Archive(StorageFolder directory, StorageFile purgeFileName)
        {
            string fileName = GetRandomFileName() + ".zip";
            string zipFilePath = System.IO.Path.Combine(ApplicationData.Current.TemporaryFolder.Path, fileName);
            try
            {
                await Task.Run(() => ZipFile.CreateFromDirectory(directory.Path, zipFilePath));
                StorageFile zipFile = await ApplicationData.Current.TemporaryFolder.GetFileAsync(fileName);
                await zipFile.CopyAndReplaceAsync(purgeFileName);
                await zipFile.DeleteAsync(StorageDeleteOption.Default);
            }
            catch (Exception ex)
            {
                String ExceptionMessage = ex.Message;
            }
        }
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public static string GetRandomFileName()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }


    }

}
