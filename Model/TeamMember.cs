using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using Windows.UI.Xaml.Media.Imaging;

namespace SDKTemplate.Model
{
    public class TeamMember
    {
        private TeamMember teamMember;

        #region Properties

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string IRId { get; set; }
        public string Password { get; set; }
        public string CPAPassword { get; set; }
        public string SecurityQA { get; set; }
        public string SecurityWord { get; set; }
        public string MailId { get; set; }
        public string MailIdPassword { get; set; }
        public bool KYCDone { get; set; }
        public string Remark { get; set; }
        public string ImageUrl { get; set; }

        #endregion

        public TeamMember()
        {
            Name = string.Empty;
            IRId = string.Empty;
            Password = string.Empty;
            CPAPassword = string.Empty;
            SecurityQA = string.Empty;
            SecurityWord = string.Empty;
            MailId = string.Empty;
            MailIdPassword = string.Empty;
            PhoneNumber = string.Empty;
            KYCDone = false;   
            Remark = string.Empty;
            //Photo = new BitmapImage(new Uri("ms-appx:///Assets/Account.png"));
            ImageUrl = "Assets/Account.png";
        }

        public TeamMember(TeamMember teamMember)
        {
            this.teamMember = teamMember;
        }     
    }
}
