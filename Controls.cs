using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace DhansSsBusinessManager.Controls
{
    class Controls
    {
        public class SearchableComboBox : ComboBox
        {

            public ObservableCollection<string> ItemsSourceList
            {
                get { return (ObservableCollection<string>)GetValue(ItemsSourceListProperty); }
                set
                {
                    SetValue(ItemsSourceListProperty, value);
                    this.ItemsSource = ItemsSourceList;
                }
            }

            public static readonly DependencyProperty ItemsSourceListProperty =
                DependencyProperty.Register("ItemsSourceList", typeof(ObservableCollection<string>), typeof(SearchableComboBox),
                new PropertyMetadata(null, ItemsSourceListChanged));

            private static void ItemsSourceListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var plv = d as SearchableComboBox;
                plv.ItemsSource = e.NewValue;
            }

            public Collection<string> ItemsSourceListBase
            {
                get { return (Collection<string>)GetValue(ItemsSourceListBaseProperty); }
                set
                {
                    SetValue(ItemsSourceListBaseProperty, value);
                    this.ItemsSource = ItemsSourceListBase;
                }
            }

            public static readonly DependencyProperty ItemsSourceListBaseProperty =
                DependencyProperty.Register("ItemsSourceListBase", typeof(Collection<string>), typeof(SearchableComboBox),
                new PropertyMetadata(null, ItemsSourceListBaseChanged));

            private static void ItemsSourceListBaseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var plv = d as SearchableComboBox;
                plv.ItemsSource = e.NewValue;
            }

            string FilterString = "";
            protected override void OnKeyDown(KeyRoutedEventArgs e)
            {
                if (IsLetterOrSpace(e.Key))
                {
                    if (e.Key == VirtualKey.Space)
                        FilterString += " ";
                    else
                        FilterString += e.Key.ToString();
                    FilterList(FilterString);
                    this.ItemsSource = ItemsSourceList;
                    if (ItemsSourceList.Count > 0)
                        this.SelectedIndex = 0;
                }
                else if (e.Key == VirtualKey.Back)
                {
                    if (FilterString.Length > 0)
                    {
                        FilterString = FilterString.Substring(0, FilterString.Length - 1);
                        FilterList(FilterString);
                        this.ItemsSource = ItemsSourceList;
                        if (ItemsSourceList.Count > 0)
                            this.SelectedIndex = 0;
                    }
                }

                if (e.Key != VirtualKey.Space)
                {
                    base.OnKeyDown(e);
                }
            }

            protected override void OnGotFocus(RoutedEventArgs e)
            {
                string selectedValue = "";
                if (this.SelectedValue != null)
                    selectedValue = (string)this.SelectedValue;
                FilterString = "";
                FilterList(FilterString);
                if (!string.IsNullOrEmpty(selectedValue))
                    this.SelectedValue = selectedValue;
            }

            internal void FilterList(string FilterString)
            {
                ItemsSourceList.Clear();
                IEnumerable<string> list;
                if (!string.IsNullOrEmpty(FilterString))
                    list = ItemsSourceListBase.Where(x => x.StartsWith(FilterString));
                else
                    list = ItemsSourceListBase;
                foreach (var item in list)
                    ItemsSourceList.Add(item);
            }

            private bool IsLetterOrSpace(VirtualKey key)
            {
                return (key == VirtualKey.A
                    || key == VirtualKey.B
                    || key == VirtualKey.C
                    || key == VirtualKey.D
                    || key == VirtualKey.E
                    || key == VirtualKey.F
                    || key == VirtualKey.G
                    || key == VirtualKey.H
                    || key == VirtualKey.I
                    || key == VirtualKey.J
                    || key == VirtualKey.K
                    || key == VirtualKey.L
                    || key == VirtualKey.M
                    || key == VirtualKey.N
                    || key == VirtualKey.O
                    || key == VirtualKey.P
                    || key == VirtualKey.Q
                    || key == VirtualKey.R
                    || key == VirtualKey.S
                    || key == VirtualKey.T
                    || key == VirtualKey.U
                    || key == VirtualKey.V
                    || key == VirtualKey.W
                    || key == VirtualKey.X
                    || key == VirtualKey.Y
                    || key == VirtualKey.Z
                    || key == VirtualKey.Space);
            }
        }
    }
}
