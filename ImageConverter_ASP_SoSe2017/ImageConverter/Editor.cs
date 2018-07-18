using ImageMagick;
using System;
using System.Threading.Tasks;

namespace ImageConverter
{
    class Editor
    {


#pragma warning disable 1998
        public async Task<String> ImageList(FileCollection fileCollection, EditorOptions editorOptions, String newPath)
#pragma warning restore 1998
        {

            // AutoMod High - SaveTo 
            
            if (editorOptions.ManulpilationMod == ManulpilationMod.Rotate && editorOptions.OutputMod == OutputMod.SaveTo)
            {
                Console.WriteLine(@"Rotation Replace");
                foreach (FileItem t in fileCollection.Files)
                {
                    var image = new MagickImage(t.FullPath) {Format = MagickFormat.Unknown};
                    image.Rotate(editorOptions.RotationLevel);
                    Console.WriteLine(@"RotationLevel at AutoMod High - SaveTo " + editorOptions.RotationLevel);
                    image.Write(newPath + "//" + t.Name + "." + t.Type);
                }
            }


            // AutoMod High - Replace 

            if (editorOptions.ManulpilationMod == ManulpilationMod.Rotate &&
                editorOptions.OutputMod == OutputMod.ReplaceImages)
            {
                Console.WriteLine(@"Rotation Replace");
                foreach (FileItem t in fileCollection.Files)
                {
                    MagickImage image =
                        new MagickImage(t.FullPath);

                    //get given Rotation value of picture from metadata
                    //MagickImageInfo magickImageInfo = new MagickImageInfo(t.FullPath);

                    image.Format = MagickFormat.Unknown;
                    image.Rotate(editorOptions.RotationLevel);
                    image.Write(t.FullPath);
                }
            }

            // Kontrast - SaveTo 
            if (editorOptions.ManulpilationMod == ManulpilationMod.Kontrast && editorOptions.OutputMod == OutputMod.SaveTo)
            {
                Console.WriteLine(@"Kontrast Replace");
                foreach (FileItem t in fileCollection.Files)
                {
                    var image = new MagickImage(t.FullPath);
                    image.Format = MagickFormat.Unknown;

                    Percentage percentage = new Percentage(editorOptions.EditorSliderLevel);
                    image.BrightnessContrast(percentage, percentage);
                    Console.WriteLine(@" Kontrast - SaveTo " + editorOptions.EditorSliderLevel);
                    image.Write(newPath + "//" + t.Name + "." + t.Type);
                }
            }

            // Kontrast - ReplaceImages
            if (editorOptions.ManulpilationMod == ManulpilationMod.Kontrast && editorOptions.OutputMod == OutputMod.ReplaceImages)
            {
                Console.WriteLine(@"Contrast Replace");
                for (int i = 0; i < fileCollection.Files.Count; ++i)
                {
                    MagickImage image =
                        new MagickImage(fileCollection.Files[i].FullPath);
                    image.Format = MagickFormat.Unknown;

                    Percentage percentage = new Percentage(editorOptions.EditorSliderLevel);

                    image.BrightnessContrast(percentage, percentage);
                    Console.WriteLine(@"Kontrast - ReplaceImages " + editorOptions.RotationLevel);
                    image.Write(fileCollection.Files[i].FullPath);
                }
            }

            return ("Pictures are now Edited");
        }
    }
}
