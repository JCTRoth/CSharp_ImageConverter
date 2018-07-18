using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageConverter
{
    public class FileCollection : INotifyPropertyChanged
    {
        public bool PauseBinding { get; set; }
        ObservableCollection<FileItem> _files;
        public ObservableCollection<FileItem> Files
        {
            get { return _files; }
            set
            {
                _files = value;
                if (PauseBinding == false)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FileItem"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
