using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ImageConverter
{
    public class MyTreeViewItem : TreeViewItem , INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        private bool? _isChecked;

        public bool? IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsChecked"));
                }
            }
        }
        public string FileType { get; set; }
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MyTreeViewItem();

        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is MyTreeViewItem;
        }
    }


}
