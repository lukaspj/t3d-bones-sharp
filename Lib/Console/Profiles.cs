using Torque3D;
using Torque3D.Util;

namespace Game.Lib.Console
{
   class Profiles
   {
      public static GuiControlProfile ConsoleScrollProfile { get; set; }

      public static void Init()
      {
         string platform = Globals.GetString("platform");
         if (!Global.isObject("GuiConsoleProfile"))
         {
            GuiControlProfile GuiConsoleProfile = new GuiControlProfile("GuiConsoleProfile")
            {
               FontType = (platform == "macos") ? "Monaco" : "Lucida Console",
               FontSize = (platform == "macos") ? 13 : 12,
               FontColor = ColorI.WHITE,
               FontColorHL = new ColorI(0, 255, 255),
               FontColorNA = new ColorI(255, 0, 0),
               FontColors =
               {
                  [6] = new ColorI(100, 100, 100),
                  [7] = new ColorI(100, 100, 0),
                  [8] = new ColorI(100, 100, 200),
                  [9] = new ColorI(50, 200, 50)
               },
               Category = "Core"
            };
            GuiConsoleProfile.registerObject();
         }
         if (!Global.isObject("GuiConsoleTextProfile"))
         {
            GuiControlProfile GuiConsoleTextProfile = new GuiControlProfile("GuiConsoleTextProfile")
            {
               FontColor = ColorI.BLACK,
               AutoSizeWidth = true,
               AutoSizeHeight = true,
               TextOffset = new Point2I(2, 2),
               Opaque = true,
               FillColor = ColorI.WHITE,
               Border = 1,
               BorderThickness = 1,
               BorderColor = ColorI.BLACK,
               Category = "Core"
            };
            GuiConsoleTextProfile.registerObject();
         }
         if (!Global.isObject("ConsoleScrollProfile"))
         {
            ConsoleScrollProfile = new GuiControlProfile("ConsoleScrollProfile")
            {
               Opaque = true,
               FillColor = new ColorI(20, 20, 20, 128),
               Border = 1,
               //BorderThickness = 0,
               BorderColor = ColorI.BLACK,
               Category = "Core"
            };
            ConsoleScrollProfile.registerObject();
         }
         if (!Global.isObject("ConsoleTextEditProfile"))
         {
            GuiControlProfile ConsoleTextEditProfile = new GuiControlProfile("ConsoleTextEditProfile",
               "GuiTextEditProfile")
            {
               FillColor = new ColorI(242, 241, 240),
               FillColorHL = ColorI.WHITE,
               Category = "Core"
            };
            ConsoleTextEditProfile.registerObject();
         }
      }
   }
}