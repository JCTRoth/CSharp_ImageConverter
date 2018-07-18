using System;
using ImageMagick;
using System.IO;
using System.Threading.Tasks;

namespace ImageConverter
{
    class Compress
    {
        /// <summary>
        /// gets a FileCollection and applies the compress options to them, 
        /// then it either saves them to a new folder or overwrites the existing ones
        /// - Works async
        /// </summary>
        /// <param name="fileCollection">the filecollection</param>
        /// <param name="options">the compress options</param>
        /// <param name="newPath">the new Path</param>
        
#pragma warning disable 1998
        public async Task<string> ImageList(FileCollection fileCollection, CompressOptions options, String newPath)
#pragma warning restore 1998
        {

            // SliderMod - Replace Images in Filder
            if (options.UserMod == UserMod.ManualMod && options.OutputMod == OutputMod.ReplaceImages)
            {
                for (int i = 0; i < fileCollection.Files.Count; ++i)
                {
                    MagickImage image =
                        new MagickImage(fileCollection.Files[i].FullPath);
                    image.Format = MagickFormat.Unknown;
                    image.Quality = options.SliderLevel;
                    image.Write(fileCollection.Files[i].FullPath);
                }

            }

            // SliderMod - SaveTo

            //Console.WriteLine("SliderMod SliderMod");

            if (options.UserMod == UserMod.ManualMod && options.OutputMod == OutputMod.SaveTo)
            {
                for (int i = 0; i < fileCollection.Files.Count; ++i)
                {
                    MagickImage image =
                        new MagickImage(fileCollection.Files[i].FullPath);
                    image.Format = MagickFormat.Unknown;
                    image.Quality = options.SliderLevel;
                    image.Write(newPath + "//" + fileCollection.Files[i].Name + "." + fileCollection.Files[i].Type);
                }
            }

            // AutoMod High - Replace Images in Filder

            if (options.UserMod == UserMod.AutoMod && options.OutputMod == OutputMod.ReplaceImages)
            {
                foreach (FileItem t in fileCollection.Files)
                {
                    MagickImage image =
                        new MagickImage(t.FullPath);
                    image.Format = MagickFormat.Unknown;
                    image.Quality = EvaluateQualityLevel(t.FullPath, options.QualitiLevelCheckBox);
                    image.Write(t.FullPath);
                }
            }

            // AutoMod High - SaveTo
            if (options.UserMod == UserMod.AutoMod && options.OutputMod == OutputMod.SaveTo)
            {
                Console.WriteLine(@"Replace Replace");
                foreach (FileItem t in fileCollection.Files)
                {
                    MagickImage image = new MagickImage(t.FullPath);
                    image.Format = MagickFormat.Unknown;
                    image.Quality = EvaluateQualityLevel(t.FullPath, options.QualitiLevelCheckBox);
                    image.Write(newPath + "//" + t.Name + "." + t.Type);
                }
            }
            return "Compression done";
        }


        /// <summary>
        /// Used to evaluate the quality levels of images for our "Simple Mode"
        /// TODO: Add simple Mode Functionality
        /// </summary>
        /// <param name="filePath">a path to a file </param>
        /// <param name="qualitiLevelCheckBox">Checkbox parameter ( High Medium Low )</param>
        /// <returns>the qualityvalue for an image</returns>
        public short EvaluateQualityLevel(string filePath, QualitiLevelCheckBox qualitiLevelCheckBox)
        {
  
            MagickImageInfo image = new MagickImageInfo(filePath);

            //calc resulution
            double resulution = (1000000.0 / (image.Height * image.Width));
            Console.WriteLine(@"Resulution: " + resulution);

            //get File Size
            long fileSize = new FileInfo(filePath).Length;


            if(qualitiLevelCheckBox == QualitiLevelCheckBox.High)
            {

                Console.WriteLine(@"Resulution: " + resulution);

                // >= 20Mpix
                if (resulution >= 20)
                {
                    if (fileSize >= 5)
                    {
                        return 55;
                    }

                    if (fileSize >= 2)
                    {
                        return 70;
                    }

                    if (fileSize >= 1)
                    {
                        return 85;
                    }
                    else return 95;

                }

            // >= 10Mpix

            if (resulution >= 10)
                {
                    if (fileSize >= 4)
                    {
                        return 45;
                    }

                    if (fileSize >= 2)
                    {
                        return 70;
                    }

                    if (fileSize >= 1)
                    {
                        return 79;
                    }

                    if (fileSize <= 0.75)
                    {
                        return 100;
                    }

                }

            // >= 5Mpix

            if (resulution >= 5)
                {
                    if (fileSize >= 4)
                    {
                        return 45;
                    }

                    if (fileSize >= 2)
                    {
                        return 70;
                    }

                    if (fileSize >= 1)
                    {
                        return 79;
                    }

                    if (fileSize <= 0.75)
                    {
                        return 100;
                    }

                }

                if (resulution <= 4)
                {

                    return 76;

                }

                if (resulution <= 3)
                {

                    return 80;

                }
            }



            // Calulate with factor

            if (qualitiLevelCheckBox == QualitiLevelCheckBox.Middle)
            {

                double middleRes = ((fileSize * 1000)  / resulution);

                Console.WriteLine(@"middle middle middle  " +  middleRes);
                Console.Write(@"FileSize: " + fileSize + @" ");

                return (short) middleRes;

            }

            if (qualitiLevelCheckBox == QualitiLevelCheckBox.Low)
            {
                Console.WriteLine(@"Low Low Low Low " + (short)(resulution * 5) / (fileSize / 100));
                Console.Write(@"FileSize: " + fileSize + @" ");
                return (short)(2 * ((110 * resulution) / fileSize));

            }



            return 0;

        }

    }


    }
