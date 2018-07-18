
namespace ImageConverter
{
    public enum ManulpilationMod{Negate, Rotate, Resize, Kontrast};

    public enum UserMod{AutoMod,ManualMod};

    public enum QualitiLevelCheckBox {High, Middle, Low}

    public enum OutputMod {ReplaceImages, SaveTo}

    //public enum RotationLevelCheckBox { Ninety, Onehundredeighty, Twohundredseventy, Threehundredsixty}

    class CompressOptions
    {
        public short SliderLevel;
        public QualitiLevelCheckBox QualitiLevelCheckBox;
        public UserMod UserMod;
        public OutputMod OutputMod;


        public CompressOptions()
        {
            
        }

        public CompressOptions(short sliderLevel, QualitiLevelCheckBox qLevelCheckBox, UserMod userMod, OutputMod outMode)
        {
            SliderLevel = sliderLevel;
            QualitiLevelCheckBox = qLevelCheckBox;
            UserMod = userMod;
            OutputMod = outMode;
        }



    }

    class EditorOptions
    {

        public short EditorSliderLevel;
        public int RotationLevel;
        public EditorOptions EditorOps { get; }
        public OutputMod OutputMod;
        public ManulpilationMod ManulpilationMod;


        public EditorOptions()
        {

        }

        public EditorOptions(short sliderLevel, int rotationLevel, EditorOptions editorOps, OutputMod outMode, ManulpilationMod manulpilationMod)
        {
            EditorSliderLevel = sliderLevel;
            OutputMod = outMode;
            ManulpilationMod = manulpilationMod;
            RotationLevel = rotationLevel;
            EditorOps = editorOps;
        }
    }

    ///TODO: RenameOptions
    class RenameOptions
        {
    //        public RenameOptions()
    //        {

    //        }

        }


}
