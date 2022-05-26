using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
//using System.Windows.Media.Imaging;

namespace SDKTemplate.Model
{
    public class Contact
    {
        private Contact contact;

        #region Properties
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Info { get; set; }
        public string Invite { get; set; }
        public string Remark { get; set; }
        public string ImageUrl { get; set; }
      
        #endregion
        //public static byte[] ImageToBytes(BitmapImage imageSource)
        //{
        //    //using (MemoryStream ms = new MemoryStream())
        //    //{
        //    //    BitmapImage btmMap = new BitmapImage(img.UriSource);

        //    //    Windows.UI.Xaml.Media.Imaging.Extensions.SaveJpeg(btmMap, ms, img.PixelWidth, img.PixelHeight, 0, 100);
        //    //    img = null;
        //    //    return ms.ToArray();
        //    //}

        //    byte[] data = null;
        //    using (MemoryStream stream = new MemoryStream())
        //    {

        //        WriteableBitmap wb = new WriteableBitmap(imageSource.PixelWidth, imageSource.PixelHeight);

        //        var streamFile = await GetFileStream(myFile);
        //        await wb.SetSourceAsync(streamFile);
        //        wb = wb.Convolute(WriteableBitmapExtensions.KernelGaussianBlur5x5);


        //        WriteableBitmap wBitmap = new WriteableBitmap(100,100);
        //        wBitmap.SaveJpeg(stream, wBitmap.PixelWidth, wBitmap.PixelHeight, 0, 100);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        data = stream.GetBuffer();
        //    }

        //    return data;

        //    ////return buffer;
        //    //WriteableBitmap myWriteableBitmap = new WriteableBitmap(100, 100);
        //    //myWriteableBitmap.SetSource = imageSource;
        //    //byte[] pixelArray = myWriteableBitmap.PixelBuffer.ToArray(); // convert to Array
        //    //Stream pixelStream = wb.PixelBuffer.AsStream();
        //}

        //public BitmapImage ImageFromBuffer(Byte[] bytes)
        //{
        //    MemoryStream stream = new MemoryStream(bytes);
        //    BitmapImage image = new BitmapImage();
        //    image.BeginInit();
        //    image.StreamSource = stream;
        //    image.EndInit();
        //    return image;
        //}

        public Contact()
        {
            // default values for each property.
            Name = string.Empty;
            PhoneNumber = string.Empty;
            Location = string.Empty;
            Category = "Warm";
            Info = string.Empty;
            Invite = string.Empty;
            Remark = string.Empty;
            ImageUrl = "Assets/Account.png";
        }

        public Contact(Contact contact)
        {
            this.contact = contact;
        }

        //public Contact(const Contact& a)
        //{
        //    // default values for each property.

        //    Name = string.Empty;
        //    PhoneNumber = string.Empty;
        //    Location = string.Empty;
        //    Category = string.Empty;
        //    Info = string.Empty;
        //    Invite = string.Empty;
        //    Remark = string.Empty;
        //}
        //#region Public Methods
        public static Contact GetNewContact()
        {
            return new Contact()
            {
               // not implemnted
            };
        }
        //public static ObservableCollection<Contact> GetContacts(int numberOfContacts)
        //{
        //    ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();

        //    for (int i = 0; i < numberOfContacts; i++)
        //    {
        //        contacts.Add(GetNewContact());
        //    }
        //    return contacts;
        //}
        //public static ObservableCollection<Contact> GetContactsFromFile(int numberOfContacts)
        //{
        //    ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();

        //    for (int i = 0; i < numberOfContacts; i++)
        //    {
        //        contacts.Add(GetNewContact());
        //    }
        //    return contacts;
        //}

        //public static ObservableCollection<GroupInfoList> GetContactsGrouped(int numberOfContacts)
        //{
        //    ObservableCollection<GroupInfoList> groups = new ObservableCollection<GroupInfoList>();

        //    var query = from item in GetContacts(numberOfContacts)
        //                group item by item.Info[0] into g
        //                orderby g.Key
        //                select new { GroupName = g.Key, Items = g };

        //    foreach (var g in query)
        //    {
        //        GroupInfoList info = new GroupInfoList();
        //        info.Key = g.GroupName;
        //        foreach (var item in g.Items)
        //        {
        //            info.Add(item);
        //        }
        //        groups.Add(info);
        //    }

        //    return groups;
        //}
        //#endregion

        //#region Helpers
        //private static string GenerateCategory()
        //{
        //    List<string> category = new List<string>() { "Hot", "Warm", "Cold" };
        //    return category[random.Next(0, category.Count)];
        //}
        //private static string GetRemark()
        //{
        //    List<string> remarks = new List<string>()
        //    {
        //        @"He was my school friend and working in wipro now.",
        //        @"She got married and busy in family now.",
        //        @"No contacts since years. recently spoke to him after 10 Years",
        //        };
        //    return remarks[random.Next(0, remarks.Count)];
        //}

        //private static string GeneratePhoneNumber()
        //{
        //    return string.Format("{0:(###)} {1:###}-{2:####}", random.Next(100, 999), random.Next(100, 999), random.Next(1000, 9999));
        //}
        //private static string GenerateFirstName()
        //{
        //    List<string> names = new List<string>() { "Manas", "Ravi", "Kanchana", "Ankita", "Sidharth"};
        //    return names[random.Next(0, names.Count)];
        //}
        //private static string GenerateLastName()
        //{
        //    List<string> lastnames = new List<string>() { "Mohanty", "Kochar", "Das", "Patel" };
        //    return lastnames[random.Next(0, lastnames.Count)];
        //}
        //#endregion
    }
}
