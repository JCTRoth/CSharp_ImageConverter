using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageConverter
{
    /// <summary>
    /// Presenter for the MainWindow, this class should contain all logic behind the mainwindow in the future, so that only Event handlers remain in MainWindow
    /// </summary>
    public class MainWindowPresenter
    {
        private static readonly List<string> CompressableTypes = new List<string>{"jpg" , "raw" , "gif"};//List that stores all compressable formats by their extension

        /// <summary>
        /// Converts a Byte lenght to user readable string 
        /// </summary>
        /// <param name="len">double, the size of the file in Bytes</param>
        /// <returns>A user readable String of len, with proper Units at the end</returns>
        public string MakeFileSizeToReadableString(double len)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            var order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
        /// <summary>
        /// Checks if a file has a ending, that might be compressable, used for filtering only compressable files in the TreeView of MainWindow
        /// </summary>
        /// <param name="fileName">The FileName that is checked</param>
        /// <returns>true or false</returns>
        public bool IsFileCompressable(string fileName)
        {
            if (fileName == null) return false;
            fileName = Path.GetExtension(fileName).Length > 0 ? Path.GetExtension(fileName).Substring(1) : "Unknown";
            return CompressableTypes.Any(type => fileName.ToLower().Equals(type.ToLower()));
        }

        /// <summary>
        /// Check if the fullpath is allready in the FileCollection.Files Observable List
        /// </summary>
        /// <param name="path">File Path</param>
        /// <param name="fileCollection">Collection of all List Elements</param>
        /// <returns></returns>
        public bool PathAllreadyInList(string path , FileCollection fileCollection)
        {
            var addFile = true;
            var listViewSize = fileCollection.Files.Count;
            if (listViewSize != 0)
            {
                for (var i = 0; i < listViewSize; i++)
                {
                    if (fileCollection.Files[i].FullPath.Equals(path))
                    {
                        addFile = false;
                    }
                }
            }
            return addFile;
        }

    }
}
