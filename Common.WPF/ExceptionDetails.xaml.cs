using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace Common.WPF
{
    /// <summary>
    /// Interaction logic for ExceptionDetails.xaml
    /// </summary>
    public partial class ExceptionDetails : Window, INotifyPropertyChanged
    {
        private List<TreeItem> _items;

        public List<TreeItem> Items
        {
            get { return _items; }
            set { ChangeAndNotify(ref _items, value); }
        }

        private string _itemContent;

        public string ItemContent
        {
            get { return _itemContent; }
            set { ChangeAndNotify(ref _itemContent, value); }
        }

        public ExceptionDetails(Exception ex)
        {
            InitializeComponent();
            DataContext = this;

            TreeItem treeItem = new TreeItem();
            treeItem.Text = "Exception";
            treeItem.IsExpanded = true;
            treeItem.Content = ex.ToString();
            buildTreeLayer(ex, treeItem);
            var items = new List<TreeItem>();
            items.Add(treeItem);
            Items = items;

            treeView1.SelectedItemChanged += treeView1_SelectedItemChanged;
        }

        private void treeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeItem = e.NewValue as TreeItem;
            if (treeItem != null)
            {
                ItemContent = treeItem.Content;
            }
            else ItemContent = string.Empty;
        }

        private void buildTreeLayer(Exception ex, TreeItem parent)
        {
            parent.Children.Add(new TreeItem() {Text = "Type", Content = ex.GetType().ToString()});
            PropertyInfo[] memberList = ex.GetType().GetProperties();
            foreach (PropertyInfo info in memberList)
            {
                var value = info.GetValue(ex, null);
                if (value != null)
                {
                    if (info.Name == "InnerException")
                    {
                        TreeItem treeItem = new TreeItem();
                        treeItem.Text = info.Name;
                        treeItem.Content = ex.ToString();
                        buildTreeLayer(ex.InnerException, treeItem);
                        parent.Children.Add(treeItem);
                    }
                    else
                    {
                        TreeItem treeStringSet = new TreeItem() {Text = info.Name, Content = value.ToString()};
                        parent.Children.Add(treeStringSet);
                    }
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void ChangeAndNotify<T>(ref T storage, T value,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return;

            storage = value;

            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void Notify([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TreeItem
    {
        public TreeItem()
        {
            Children = new List<TreeItem>();
        }

        public string Text { get; set; }
        public string Content { get; set; }
        public IList<TreeItem> Children { get; set; }
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
    }
}