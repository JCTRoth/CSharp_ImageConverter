using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;


namespace ImageConverter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MainWindowPresenter myPresenter = new MainWindowPresenter(); // We want to push all logic of MainWindow into the MainWindowPresenter in the future, only Handlers shall remain in this class here
        private GridViewColumnHeader _lastHeaderClicked;//stores the last clicked header item of ListViewSelectedFiles
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;// set the last sort order to Ascending, so that we can apply descending sort when first clicking on Column Header of GridViewSelectedFiles
        private const string File = "File"; // Constants for FileTypes, these are needed for the HeaderToImageConverter
        private const string Folder = "Folder";
        private const string Drive = "Drive";
        private readonly object _dummyNode = null;//this node is needed for folder_expanded event handler. This allows us to check if a folder can be expanded and has not been expanded allready
        private readonly FileCollection _fileCollection;//holds the selected files, this collection is Two-way DataBound to populate ListViewSelectedFiles
        public MainWindow()
        {
            // ------ for testing/debugging ------
            //Compress compress = new Compress();
            //compress.EvaluateQualityLevel(" ");
            //
            InitializeComponent();
            FillTree();
            _fileCollection = new FileCollection {Files = new ObservableCollection<FileItem>()};
            DataContext = _fileCollection;
        }
        /// <summary>
        /// Fill the three with the highest layer ( drive layer ) (initialisation of the FileTree)
        /// </summary>
        private void FillTree()
        {
            foreach (var s in Directory.GetLogicalDrives())
            {
                MyTreeViewItem item = new MyTreeViewItem
                {
                    Header = s,
                    Tag = s,
                    FontWeight = FontWeights.Normal,
                    FileType = Drive,
                    IsChecked = false
                };
                item.Items.Add(_dummyNode);
                item.Expanded += Folder_Expanded;
                TreeViewDirectory.Items.Add(item);
            }
        }
        /// <summary>
        /// Folder expanded Event handler, fired when user clicks on a folder to expand, 
        /// tree is filled on the fly, only when a folder is expanded we add its 
        /// contents as a new layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (MyTreeViewItem) sender;
            if (item.Items.Count != 1 || item.Items[0] != _dummyNode) return; //_dummyNode is needed to know if we can do a folder expansion here, if there is no dummynode, we have allready expanded it, or it isn´t an expandable item
            item.Items.Clear(); // remove the dummy Node
            try
            {
                foreach (var s in Directory.GetFiles(item.Tag.ToString()))
                {
                    // add each file under the current directory to the treeview...
                    if (!myPresenter.IsFileCompressable(s)) continue;
                    var subitem = new MyTreeViewItem
                    {
                        Header = s.Substring(s.LastIndexOf("\\", StringComparison.Ordinal) + 1),
                        Tag = s,
                        FileType = File,
                        FontWeight = FontWeights.Normal,
                        IsChecked = false
                    };
                    item.Items.Add(subitem);
                }
                foreach (var s in Directory.GetDirectories(item.Tag.ToString()))
                {
                    // add each directory under the current directory to the treeview...
                    var subitem = new MyTreeViewItem
                    {
                        Header = s.Substring(s.LastIndexOf("\\", StringComparison.Ordinal) + 1),
                        Tag = s,
                        FileType = Folder,
                        FontWeight = FontWeights.Normal,
                        IsChecked = false
                    };
                    subitem.Items.Add(_dummyNode);//each new folder gets a dummynode, could be a expandable folder, also a newly added folder has not been expanded yet
                    subitem.Expanded += Folder_Expanded;
                    item.Items.Add(subitem);
                }
            }
            catch (DriveNotFoundException ex1)
            {
                MessageBox.Show(ex1.Message);
            }
            catch (IOException ex1)
            {
                MessageBox.Show(ex1.Message);
            }
            catch (UnauthorizedAccessException ex1)
            {
                MessageBox.Show(ex1.Message);
            }
        }
        /// <summary>
        /// TODO:This needs to fire the OnMouseClickCheckBoxEvent manually in the future,
        /// TODO:but how do we give this event the checkbox then?
        /// This is used to check the checkbox when user only clicks on name in tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FoldersItem_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var myItem = (MyTreeViewItem) TreeViewDirectory.SelectedItem;
            myItem.IsChecked = true; // causes trouble, because we have handled the mouseclick event to check a box, but this checking of a box won´t fire this event
        }

        private static T FindAncestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T) current;
                }
                current = VisualTreeHelper.GetParent(current);
            } while (current != null);
            return null;
        }
        /// <summary>
        /// Handler for Mouseclick on Checkbox, we need this because we want to use tree state Checkboxes in the future, but the unknown state shall never be setted by the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseClickCheckBox(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = FindAncestor<MyTreeViewItem>((DependencyObject) e.OriginalSource);
            if (treeViewItem.IsChecked != null && (bool) !treeViewItem.IsChecked)
            {
                SetTreeViewItemChecked(true , treeViewItem , true);
            }
            else
            {
                SetTreeViewItemChecked(false, treeViewItem, true);
            }
            e.Handled = true;
        }
        /// <summary>
        /// This will check all treeviewitems underneath a treeview item, but only files.
        /// </summary>
        /// <param name="check">shall the treeview items be checked or not?</param>
        /// <param name="treeViewItem">The Parent TreeViewItem</param>
        /// <param name="updateChildFiles">Shall we update the child files or not?</param>
        private static void SetTreeViewItemChecked(bool check , MyTreeViewItem treeViewItem , bool updateChildFiles )
        { 
            treeViewItem.IsExpanded = check;
            treeViewItem.IsSelected = true;
            treeViewItem.IsChecked = check;
            //int test = treeViewItem.Items.Count;
            if (treeViewItem.Items.Count <= 0 || !updateChildFiles) return;
            foreach (object t in treeViewItem.Items)
            {
                var currentTreeViewItem = t as MyTreeViewItem;
                if (currentTreeViewItem != null && currentTreeViewItem.FileType == Folder) break; // Folders are always added at the end so we can do this
                if (currentTreeViewItem != null) currentTreeViewItem.IsChecked = check;
            }
        }
        /// <summary>
        /// Event Handler for Click on Add Button, this starts the tree traversing and will add selected files to the ListViewSelectedFiles by adding them to _fileCollection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            foreach (var t in TreeViewDirectory.Items)
            {
                GetNextContainer((MyTreeViewItem)t);
            }
        }

        /// <summary>
        /// Recursively traverses the tree and adds all checked items, which are files to the FileList
        /// </summary>
        /// <param name="parentContainer"></param>
        private void GetNextContainer( MyTreeViewItem parentContainer ) {
            
            foreach (var item in parentContainer.Items) { 
                MyTreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as MyTreeViewItem;

                if (currentContainer == null) continue;
                if (currentContainer.IsChecked != null && currentContainer.IsChecked == true && currentContainer.FileType == File)
                {
                    var addFile = myPresenter.PathAllreadyInList(currentContainer.Tag.ToString(),_fileCollection);
                    if (addFile)
                    {
                        //ListViewSelectedFiles.Items.Add(currentContainer.Tag);
                        var add = new FileItem();
                        var myInfo = new FileInfo(currentContainer.Tag.ToString());
                        add.FullPath = currentContainer.Tag.ToString();
                        add.Name = Path.GetFileNameWithoutExtension(currentContainer.Header.ToString());
                        double len = myInfo.Length;
                        add.Size = myPresenter.MakeFileSizeToReadableString(len);
                        add.Changed = myInfo.LastWriteTime.ToString(CultureInfo.InvariantCulture);
                        add.Type = Path.GetExtension(add.FullPath).Length > 0 ? Path.GetExtension(add.FullPath).Substring(1) : "Unknown";
                        _fileCollection.Files.Add(add);
                    }                        
                }
                // If the sub containers of current item is ready, we can directly go to the next
                // iteration to expand them.
                GetNextContainer(currentContainer);
            }
        }
        /// <summary>
        /// Event Handler for Mouse clicks on ListViewSelectedFiles Header, in this case we want to sort our ListView
        /// Problems with SortDescriptions, see Issue in Gitlab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewHeaderClick(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is GridViewColumnHeader column)) return;
            var sortBy = column.Tag.ToString();
            if (_lastHeaderClicked != null)
            {
                ListViewSelectedFiles.Items.SortDescriptions.Clear();
            }
            var newDir = ListSortDirection.Ascending;
            if (_lastHeaderClicked != null && (_lastHeaderClicked.Equals(column) && _lastDirection == newDir)) { 
                newDir = ListSortDirection.Descending; 
            }
            _lastHeaderClicked = column;
            ListViewSelectedFiles.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
            _lastDirection = newDir;
        }
        /// <summary>
        /// Opens the Compression Options Window as a Dialogue ( MainWindow stays inactive during dialogue time )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Compress(object sender, RoutedEventArgs e)
        {
            var compressOptions = new CompressWindow(_fileCollection);
            compressOptions.ShowDialog();
        }

        private void Button_Click_Remove_File(object sender, RoutedEventArgs e)
        {
            MoveSelectedFileItems(0); // lazy guys like us use this function also for deleting (-;
        }

        private void Button_Click_Move_File_Down(object sender, RoutedEventArgs e)
        {
            MoveSelectedFileItems(1); // down direction
        }

        private void Button_Click_Move_File_Up(object sender, RoutedEventArgs e)
        {
            MoveSelectedFileItems( -1 ); // up direction
        }

        /// <summary>
        /// dir = 1, move Selected files down
        /// dir = -1, move Selected files up
        /// dir = 0, remove Selected files from List
        /// </summary>
        /// <param name="dir">Item Move Direction</param>
        private void MoveSelectedFileItems( int dir)
        {
            var itemsSelected = ListViewSelectedFiles.SelectedItems;
            var selectedItemsIndex = (from object selectedItem in itemsSelected select ListViewSelectedFiles.Items.IndexOf(selectedItem)).ToList();
            selectedItemsIndex = dir == -1 ? selectedItemsIndex.OrderBy(item => item).ToList() : selectedItemsIndex.OrderByDescending(item => item).ToList();
            ListViewSelectedFiles.Items.SortDescriptions.Clear();
            foreach (var currentPosition in selectedItemsIndex)
            {
                if (currentPosition < 1 && dir == -1) break;
                if (currentPosition >= _fileCollection.Files.Count - 1 && dir == 1) break;
                if (dir == 0)
                    _fileCollection.Files.RemoveAt(currentPosition);
                else
                    _fileCollection.Files.Move(currentPosition, currentPosition + dir);
            }
        }


        private EditorWindow Button_Click_EditorOptions()
        {
            return new EditorWindow(_fileCollection);
        }
        private void Button_Click_Editor(object sender, RoutedEventArgs e)
        {
            EditorWindow compressOptions = Button_Click_EditorOptions();
            compressOptions.ShowDialog();
        }
    }
}
