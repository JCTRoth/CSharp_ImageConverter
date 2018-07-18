using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ImageConverter
{
    [ValueConversion(typeof(string), typeof(bool))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static string File { get; } = "File";
        private const string Folder = "Folder";
        private const string Drive = "Drive";
        public static HeaderToImageConverter Instance =
            new HeaderToImageConverter();

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            BitmapImage source = null;
            var s = value as string;
            //if (s != null && s.Contains(@":\") && s.Length < 4)
            if(s != null && s.Equals(Drive))
            {
                Uri uri = new Uri
                    ("pack://application:,,,/Images/diskdrive.bmp");
                try
                {
                    source = new BitmapImage(uri);
                }
                catch (Exception ex1)
                {
                    MessageBox.Show(ex1.Message);
                }
                return source;
            }
            else // has to be a folder or a file then...
            {
                if (s != null && s.Equals(Folder))
                {
                    Uri uri = new Uri("pack://application:,,,/Images/folder.bmp");
                    try
                    {
                        source = new BitmapImage(uri);
                    }
                    catch (Exception ex1)
                    {
                        MessageBox.Show(ex1.Message);
                    }
                    return source;
                }
                else {
                    Uri uri = new Uri("pack://application:,,,/Images/file.png");
                    try
                    {
                        source = new BitmapImage(uri);
                    }
                    catch (Exception ex1)
                    {
                        MessageBox.Show(ex1.Message);
                    }
                    return source;
                }
            }
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}
