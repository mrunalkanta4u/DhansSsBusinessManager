using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;
using System.Linq;
using System.Xml.Serialization;
using Windows.Graphics.Imaging;
using System.IO;

namespace SDKTemplate.Model
{
    public class Dream
    {
        private Dream dream;
        #region Properties
        public string DreamName { get; set; }
        public string Details { get; set; }
        public string Category { get; set; }
        public string TatgetDate { get; set; }
        public Boolean Achieved { get; set; }
        public string Remark { get; set; }
        public string ImageUrl { get; set; }
        #endregion

        public Dream()
        {
            // default values for each property.            
            DreamName = string.Empty;
            Details = string.Empty;
            //Photo = new BitmapImage(new Uri("ms-appx:///Assets/placeholder.jpg"));//null; //new byte[0];
            ImageUrl = "Assets/placeholder.jpg";
            Category = "Short Term";
            TatgetDate = string.Empty;
            Achieved = false;
            Remark = string.Empty;
        }

        public Dream(Dream dream)
        {
            this.dream = dream;
        }
    }
}
