﻿using SDKTemplate.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SDKTemplate
{
    public sealed partial class SimpleListViewSample : Page
    {
        public SimpleListViewSample()
        {
            this.InitializeComponent();
            //ContactsCVS.Source = Contact.GetContactsGrouped(250);
        }
        private void ShowSliptView(object sender, RoutedEventArgs e)
        {
            MySamplesPane.SamplesSplitView.IsPaneOpen = !MySamplesPane.SamplesSplitView.IsPaneOpen;
        }
    }
}
