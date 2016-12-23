using Torque3D;
using Torque3D.Engine;
using Torque3D.Util;

namespace Game.Lib.Console
{
   class Console
   {
      public static GuiControl ConsoleDlg { get; set; }

      public static void Init()
      {
         ConsoleDlg = new ConsoleDlg();
         ConsoleDlg.registerObject();

         GuiConsoleEditCtrl ConsoleEntry = new GuiConsoleEditCtrl();
         ConsoleEntry.registerObject();
         ConsoleDlg.add(ConsoleEntry);

         GuiScrollCtrl scrollCtrl = new GuiScrollCtrl
         {
            InternalName = "Scroll",
            Profile = Sim.FindObjectByName<GuiControlProfile>("ConsoleScrollProfile"),
            HorizSizing = GuiHorizontalSizing.ResizeWidth,
            VertSizing = GuiVerticalSizing.ResizeHeight,
            Position = Point2I.Zero,
            Extent = new Point2I(640, 462),
            MinExtent = new Point2I(8, 8),
            Visible = true,
            //TODO HelpTag = "0",
            WillFirstRespond = true,
            HScrollBar = GuiScrollBarBehavior.AlwaysOn,
            VScrollBar = GuiScrollBarBehavior.AlwaysOn,
            LockHorizScroll = false,
            LockVertScroll = false,
            ConstantThumbHeight = false,
            ChildMargin = new Point2I(0, 0)
         };
         scrollCtrl.registerObject();
         GuiConsole ConsoleMessageLogView = new GuiConsole("ConsoleMessageLogView")
         {
            Profile = Sim.FindObjectByName<GuiControlProfile>("GuiConsoleProfile"),
            Position = Point2I.Zero
         };
         ConsoleMessageLogView.registerObject();
         scrollCtrl.add(ConsoleMessageLogView);
         ConsoleDlg.add(scrollCtrl);
      }
   }
}