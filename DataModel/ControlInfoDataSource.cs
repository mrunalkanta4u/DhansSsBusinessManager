//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model.  The property names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs. If using this model, you might improve app 
// responsiveness by initiating the data loading task in the code behind for App.xaml when the app 
// is first launched.

namespace AppUIBasics.Data
{
    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class ControlInfoDataItem
    {
        public ControlInfoDataItem(String Name, String ContactNumber, String Location, String imagePath, String Remark, String Category)
        {
            this.Name = Name;
            this.ContactNumber = ContactNumber;
            this.Location = Location;
            this.Remark = Remark;
            this.ImagePath = imagePath;
            this.Category = Category;
            this.Docs = new ObservableCollection<ControlInfoDocLink>();
            this.RelatedControls = new ObservableCollection<string>();
        }

        public string Name { get; private set; }
        public string ContactNumber { get; private set; }
        public string Location { get; private set; }
        public string Remark { get; private set; }
        public string ImagePath { get; private set; }
        public string Category { get; private set; }
        public ObservableCollection<ControlInfoDocLink> Docs { get; private set; }
        public ObservableCollection<string> RelatedControls { get; private set; }

        public override string ToString()
        {
            return this.ContactNumber;
        }
    }

    public class ControlInfoDocLink
    {
        public ControlInfoDocLink(string ContactNumber, string uri)
        {
            this.ContactNumber = ContactNumber;
            this.Uri = uri;
        }
        public string ContactNumber { get; private set; }
        public string Uri { get; private set; }
    }


    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class ControlInfoDataGroup
    {
        public ControlInfoDataGroup(String Name, String ContactNumber, String Location, String imagePath, String Remark)
        {
            this.Name = Name;
            this.ContactNumber = ContactNumber;
            this.Location = Location;
            this.Remark = Remark;
            this.ImagePath = imagePath;
            this.Items = new ObservableCollection<ControlInfoDataItem>();
        }

        public string Name { get; private set; }
        public string ContactNumber { get; private set; }
        public string Location { get; private set; }
        public string Remark { get; private set; }
        public string ImagePath { get; private set; }
        public ObservableCollection<ControlInfoDataItem> Items { get; private set; }

        public override string ToString()
        {
            return this.ContactNumber;
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with Category read from a static json file.
    /// 
    /// ControlInfoSource initializes with data read from a static json file included in the 
    /// project.  This provides sample data at both design-time and run-time.
    /// </summary>
    public sealed class ControlInfoDataSource
    {
        private static ControlInfoDataSource _controlInfoDataSource = new ControlInfoDataSource();
        private static readonly object _lock = new object();

        private ObservableCollection<ControlInfoDataGroup> _groups = new ObservableCollection<ControlInfoDataGroup>();
        public ObservableCollection<ControlInfoDataGroup> Groups
        {
            get { return this._groups; }
        }

        public static async Task<IEnumerable<ControlInfoDataGroup>> GetGroupsAsync()
        {
            await _controlInfoDataSource.GetControlInfoDataAsync();

            return _controlInfoDataSource.Groups;
        }

        public static async Task<ControlInfoDataGroup> GetGroupAsync(string Name)
        {
            await _controlInfoDataSource.GetControlInfoDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _controlInfoDataSource.Groups.Where((group) => group.Name.Equals(Name));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<ControlInfoDataItem> GetItemAsync(string Name)
        {
            await _controlInfoDataSource.GetControlInfoDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _controlInfoDataSource.Groups.SelectMany(group => group.Items).Where((item) => item.Name.Equals(Name));
            if (matches.Count() > 0) return matches.First();
            return null;
        }

        private async Task GetControlInfoDataAsync()
        {
            lock (_lock)
            {
                if (this._groups.Count != 0)
                    return;
            }

            Uri dataUri = new Uri("ms-appx:///DataModel/ControlInfoData.json");

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);

            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["Groups"].GetArray();

            lock (_lock)
            {
                foreach (JsonValue groupValue in jsonArray)
                {
                    JsonObject groupObject = groupValue.GetObject();
                    ControlInfoDataGroup group = new ControlInfoDataGroup(groupObject["Name"].GetString(),
                                                                          groupObject["ContactNumber"].GetString(),
                                                                          groupObject["Location"].GetString(),
                                                                          groupObject["ImagePath"].GetString(),
                                                                          groupObject["Remark"].GetString());


                    foreach (JsonValue itemValue in groupObject["Items"].GetArray())
                    {
                        JsonObject itemObject = itemValue.GetObject();
                        var item = new ControlInfoDataItem(itemObject["Name"].GetString(),
                                                                itemObject["ContactNumber"].GetString(),
                                                                itemObject["Location"].GetString(),
                                                                itemObject["ImagePath"].GetString(),
                                                                itemObject["Remark"].GetString(),
                                                                itemObject["Category"].GetString());
                        if (itemObject.ContainsKey("Docs"))
                        {
                            foreach (JsonValue docValue in itemObject["Docs"].GetArray())
                            {
                                JsonObject docObject = docValue.GetObject();
                                item.Docs.Add(new ControlInfoDocLink(docObject["ContactNumber"].GetString(), docObject["Uri"].GetString()));
                            }
                        }
                        if (itemObject.ContainsKey("RelatedControls"))
                        {
                            foreach (JsonValue relatedControlValue in itemObject["RelatedControls"].GetArray())
                            {
                                item.RelatedControls.Add(relatedControlValue.GetString());
                            }
                        }
                        group.Items.Add(item);
                    }
                    if (!this.Groups.Any(g => g.ContactNumber == group.ContactNumber))
                        this.Groups.Add(group);
                }
            }
        }
    }
}
