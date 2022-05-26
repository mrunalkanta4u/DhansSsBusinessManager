using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.UserDataAccounts;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using SDKTemplate.Model;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.Serialization;
using System.IO;

namespace SDKTemplate.Model
{
    public class CallLog
    {
        //private CallLog callLog;

        #region Properties
        public string Name { get; set; }
        public string TypeOfCall { get; set; }
        public string Date { get; set; }
        public string Remark { get; set; }
        
        #endregion

        public CallLog()
        {
            // default values for each property.
            DateTime thisDay = DateTime.Today;
            Name = string.Empty;
            TypeOfCall = string.Empty;
            Date = thisDay.ToString();
            Remark = string.Empty;
        }
        public CallLog(CallLog callLog )
        {
            this.Name = callLog.Name;
            this.TypeOfCall = callLog.TypeOfCall;
            this.Date = callLog.Date;
            this.Remark = callLog.Remark;
        }
    }
}
