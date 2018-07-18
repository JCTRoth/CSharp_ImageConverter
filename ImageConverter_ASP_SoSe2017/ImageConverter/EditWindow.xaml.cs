using System;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;


namespace ImageConverter
{
    /// <summary>
    /// Interaktionslogik für CompressWindow.xaml
    /// </summary>
    public partial class EditorWindow
    {
        EditorOptions editorOptions = new EditorOptions();
        String _newPathEditor;

        private bool _firstLoadEditor;
        private FileCollection _fileCollectionEditor;
        public EditorWindow(FileCollection fileCollection)
          {
            InitializeComponent();
            _firstLoadEditor = false;
              _fileCollectionEditor = fileCollection;

          }


        private async void Button_Click_Edit(object sender, RoutedEventArgs e)
        {

            Editor edit = new Editor();


            Console.WriteLine(KontrastSliderQuality.Value);

            // Set's Slider Level
            editorOptions.EditorSliderLevel = (short)KontrastSliderQuality.Value;


            // Set Kontrast Option
            if (editorOptions.EditorSliderLevel > 0)
            {
                editorOptions.ManulpilationMod = ManulpilationMod.Kontrast;
            }

            // Start Image List Compression
           MessageBox.Show(await edit.ImageList(_fileCollectionEditor, editorOptions, _newPathEditor));


        }

        private void Button_Click_ChooseFolderEditor(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog selectFolder = new FolderBrowserDialog();
            selectFolder.ShowDialog();
            LabelFolderEditor.Content = selectFolder.SelectedPath;


            _newPathEditor = LabelFolderEditor.Content.ToString();
            Console.WriteLine(_newPathEditor);

        }

        private void RadioButton_Checked_SaveASCopy_Editor(object sender, RoutedEventArgs e)
        {
            if (!_firstLoadEditor) {
                ButtonChoseFolderEditor.IsEnabled = true;
                LabelFolderEditor.Visibility = Visibility.Visible;
            }
            //Set Variale
            editorOptions.OutputMod = OutputMod.SaveTo;

        }


        private void Kontrast_SliderQuality_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
        private void RadioButton_Checked_Overwrite_Editor(object sender, RoutedEventArgs e)
        {
        }


        //
        //Set's Rotation Level's
        //

        private void RadioButton_Rotation45(object sender, RoutedEventArgs e)
        {
            editorOptions.RotationLevel = 45;
            editorOptions.ManulpilationMod = ManulpilationMod.Rotate;

        }

        private void RadioButton_Rotation90(object sender, RoutedEventArgs e)
        {
            editorOptions.RotationLevel = 90;
            editorOptions.ManulpilationMod = ManulpilationMod.Rotate;


        }

        private void RadioButton_Rotation180(object sender, RoutedEventArgs e)
        {
                //Set's 
            editorOptions.RotationLevel = 180;
            editorOptions.ManulpilationMod = ManulpilationMod.Rotate;

        }


        private void RadioButton_Rotation270(object sender, RoutedEventArgs e)
        {
            editorOptions.RotationLevel = 270;
            editorOptions.ManulpilationMod = ManulpilationMod.Rotate;

        }


        private void RadioButton_Rotation0(object sender, RoutedEventArgs e)
        {
            editorOptions.RotationLevel = 0;
            editorOptions.ManulpilationMod = ManulpilationMod.Kontrast;

        }
    }
}
