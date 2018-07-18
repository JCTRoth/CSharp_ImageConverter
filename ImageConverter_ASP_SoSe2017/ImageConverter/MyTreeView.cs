using System.Windows;
using System.Windows.Controls;

namespace ImageConverter
{

    class MyTreeView : TreeView
    {
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
