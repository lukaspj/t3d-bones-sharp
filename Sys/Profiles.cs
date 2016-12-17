using Torque3D;
using Torque3D.Util;

namespace Game.Sys
{
    public class Profiles
    {
        public static void Init()
        {
            // Set font cache path if it doesn't already exist.
            if(string.IsNullOrEmpty(Globals.GetString("Gui::fontCacheDirectory")))
            {
                Globals.SetString("Gui::fontCacheDirectory", Global.expandFilename("sys/fonts"));
            }

            // ----------------------------------------------------------------------------
            // GuiDefaultProfile is a special profile that all other profiles inherit
            // defaults from. It must exist.
            // ----------------------------------------------------------------------------
            if (!Global.isObject("GuiDefaultProfile"))
            {
                GuiControlProfile guiDefaultProfile = new GuiControlProfile ()
                {
                    Name = "GuiDefaultProfile",
                    Tab = false,
                    CanKeyFocus = false,
                    HasBitmapArray = false,
                    MouseOverSelected = false,

                    // fill color
                    Opaque = false,
                    FillColor = new ColorI(242, 241, 240),
                    FillColorHL = new ColorI(228, 228, 235),
                    FillColorSEL = new ColorI(98, 100, 137),
                    FillColorNA = ColorI.WHITE,

                    // border color
                    Border = 0,
                    BorderColor   = new ColorI(100, 100, 100),
                    BorderColorHL = new ColorI(50, 50, 50, 50),
                    BorderColorNA = new ColorI(75, 75, 75),

                    // font
                    FontType = "Arial",
                    FontSize = 14,
                    FontCharset = GuiFontCharset.ANSI,

                    FontColor = ColorI.BLACK,
                    FontColorHL = ColorI.BLACK,
                    FontColorNA = ColorI.BLACK,
                    FontColorSEL= ColorI.WHITE,

                    // bitmap information
                    Bitmap = "",
                    //TODO BitmapBase = "",
                    TextOffset = new Point2I(0, 0),

                    // used by guiTextControl
                    Modal = true,
                    Justify = GuiAlignmentType.Left,
                    AutoSizeWidth = false,
                    AutoSizeHeight = false,
                    ReturnTab = false,
                    NumbersOnly = false,
                    CursorColor = ColorI.BLACK
                };
                guiDefaultProfile.registerObject();
            }

            if (!Global.isObject("GuiToolTipProfile"))
            {
                GuiControlProfile tooltipProfile = new GuiControlProfile()
                {
                    Name = "tooltipProfile",

                    // fill color
                    FillColor = new ColorI(239, 237, 222),

                    // border color
                    BorderColor = new ColorI(138, 134, 122),

                    // font
                    FontType = "Arial",
                    FontSize = 14,
                    FontColor = ColorI.BLACK,

                    Category = "Core"
                };
                tooltipProfile.registerObject();
            }

            if (!Global.isObject("GuiWindowProfile"))
            {
                GuiControlProfile windowProfile = new GuiControlProfile()
                {
                    Name = "GuiWindowProfile",
                    
                    Opaque = false,
                    Border = 2,
                    FillColor = new ColorI(242, 241, 240),
                    FillColorHL = new ColorI(221, 221, 221),
                    FillColorNA = new ColorI(200, 200, 200),
                    FontColor = new ColorI(50, 50, 50),
                    FontColorHL = ColorI.BLACK,
                    BevelColorHL = ColorI.WHITE,
                    BevelColorLL = ColorI.BLACK,
                    //TODO: Text = "untitled",
                    Bitmap = "./images/window",
                    TextOffset = new Point2I(8, 4),
                    HasBitmapArray = true,
                    Justify = GuiAlignmentType.Left,
                    Category = "Core"
                };
                windowProfile.registerObject();
            }

            if (!Global.isObject("GuiTextEditProfile"))
            {
                GuiControlProfile textEditProfile = new GuiControlProfile()
                {
                    Name = "GuiTextEditProfile",
                    
                    Opaque = true,
                    Bitmap = "./images/textEdit",
                    HasBitmapArray = true,
                    Border = -2,
                    FillColor = new ColorI(242, 241, 240, 0),
                    FillColorHL = ColorI.WHITE,
                    FontColor = ColorI.BLACK,
                    FontColorHL = ColorI.WHITE,
                    FontColorSEL = new ColorI(98, 100, 137),
                    FontColorNA = new ColorI(200, 200, 200),
                    TextOffset = new Point2I(4, 2),
                    AutoSizeWidth = false,
                    AutoSizeHeight = true,
                    Justify = GuiAlignmentType.Left,
                    Tab = true,
                    CanKeyFocus = true,
                    Category = "Core"
                };
                textEditProfile.registerObject();
            }

            if (!Global.isObject("GuiScrollProfile"))
            {
                GuiControlProfile scrollProfile = new GuiControlProfile()
                {
                    Name = "GuiScrollProfile",

                    Opaque = true,
                    FillColor = ColorI.WHITE,
                    FontColor = ColorI.BLACK,
                    FontColorHL = new ColorI(150, 150, 150),
                    Border = 1,
                    Bitmap = "./images/scrollBar",
                    HasBitmapArray = true,
                    Category = "Core",
                };
                scrollProfile.registerObject();
            }

            if (!Global.isObject("GuiOverlayProfile"))
            {
                GuiControlProfile overlayProfile = new GuiControlProfile()
                {
                    Name = "GuiOverlayProfile",

                    Opaque = true,
                    FillColor = ColorI.BLACK,
                    FontColor = ColorI.BLACK,
                    FontColorHL = ColorI.WHITE,
                    Category = "Core",
                };
            }
        }
    }
}