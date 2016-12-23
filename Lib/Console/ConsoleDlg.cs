using Torque3D;
using Torque3D.Engine;
using Torque3D.Util;

namespace Game.Lib.Console
{
   [ConsoleClass]
   class ConsoleDlg : GuiControl
   {
      public ConsoleDlg()
      {
         Name = "ConsoleDlg";
         Profile = Sim.FindObjectByName<GuiControlProfile>("GuiDefaultProfile");
         HorizSizing = GuiHorizontalSizing.ResizeRight;
         VertSizing = GuiVerticalSizing.ResizeBottom;
         Position = new Point2I(0, 0);
         Extent = new Point2I(640, 480);
         MinExtent = new Point2I(8, 8);
         Visible = true;
      }

      public void hideWindow()
      {
         findObjectByInternalName("Scroll").As<GuiScrollCtrl>().setVisible(false);
      }

      public void showWindow()
      {
         findObjectByInternalName("Scroll").As<GuiScrollCtrl>().setVisible(true);
      }

      public void setAlpha(string alpha)
      {
         if (string.IsNullOrEmpty(alpha))
         {
            Profiles.ConsoleScrollProfile.FillColor = new ColorI(Globals.GetString("ConsoleDefaultFillColor"));
         }
         else
         {
            Profiles.ConsoleScrollProfile.FillColor =
               new ColorI(Global.getWords(Globals.GetString("ConsoleDefaultFillColor"), 0, 2) + " " +
                          float.Parse(alpha)*255.0);
         }
      }
   }
}