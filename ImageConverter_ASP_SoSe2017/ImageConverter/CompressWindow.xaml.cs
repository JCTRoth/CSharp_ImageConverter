using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;




namespace ImageConverter
{
    /// <summary>
    /// Interaktionslogik für CompressWindow.xaml
    /// </summary>
    public partial class CompressWindow
    {
        CompressOptions compressOptions = new CompressOptions();
        String newPath = null;

        private bool _isManualMode;
        private bool _firstLoad;
        private FileCollection _fileCollection;
        public CompressWindow(FileCollection fileCollection)
          {
            InitializeComponent();
            _isManualMode = true;
            _firstLoad = false;
              _fileCollection = fileCollection;

          }


        private async void Button_Click_Compress(object sender, RoutedEventArgs e)
        {

            Compress compress = new Compress();


            // Set's to Automated or to Manual(slider)

            compressOptions.UserMod = _isManualMode ? UserMod.ManualMod : UserMod.AutoMod;

         
            
            // Set's Slider Level
            compressOptions.SliderLevel = (short)Manual_SliderQuality.Value;

            // Start Image List Compression
            // Prints Message Box is async task done
            MessageBox.Show(await compress.ImageList(_fileCollection, compressOptions, newPath));




        }

        private void Button_Click_ChooseFolder(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog selectFolder = new FolderBrowserDialog();
            selectFolder.ShowDialog();
            LabelFolder.Content = selectFolder.SelectedPath;

            
            newPath = LabelFolder.Content.ToString();
            Console.WriteLine(newPath);

        }

        private void RadioButton_Checked_SaveASCopy(object sender, RoutedEventArgs e)
        {
            if (!_firstLoad) { 
                ButtonChoseFolder.IsEnabled = true;
                LabelFolder.Visibility = Visibility.Visible;
            }
            //Set Variale
            compressOptions.OutputMod = OutputMod.SaveTo;

        }

        // Set Visibility of RadioButton (Checked)
        private void RadioButton_Checked_Overwrite(object sender, RoutedEventArgs e)
        {
            if (!_firstLoad)
            {
                if (LabelFolder != null) LabelFolder.Visibility = Visibility.Hidden;
            }
            //Set Variale
            compressOptions.OutputMod = OutputMod.ReplaceImages;
        }

        private void Button_Click_ManualMode(object sender, RoutedEventArgs e)
        {
            _isManualMode = _isManualMode != true;
            if (_isManualMode)
            {
                StackPanelEasy.Visibility = Visibility.Hidden;
                Manual_SliderQuality.Visibility = Visibility.Visible;
                LabelQualitySliderValue.Visibility = Visibility.Visible;
                ButtonChangeMode.Content = "Switch to Automated";


            }
            else { 
                StackPanelEasy.Visibility = Visibility.Visible;
                Manual_SliderQuality.Visibility = Visibility.Hidden;
                LabelQualitySliderValue.Visibility = Visibility.Hidden;
                ButtonChangeMode.Content = "Switch to Manual";

            }
        }

        private void Manual_SliderQuality_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void RadioButton_Level_High(object sender, RoutedEventArgs e)
        {

            compressOptions.QualitiLevelCheckBox = QualitiLevelCheckBox.High;
        }


        private void RadioButton_Level_Medium(object sender, RoutedEventArgs e)
        {

            compressOptions.QualitiLevelCheckBox = QualitiLevelCheckBox.Middle;
        }

        private void RadioButton_Level_Low(object sender, RoutedEventArgs e)
        {

            compressOptions.QualitiLevelCheckBox = QualitiLevelCheckBox.Low;
        }
    }
}
