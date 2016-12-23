using Torque3D;
using Torque3D.Engine;
using Torque3D.Util;

namespace Game.Lib.Console
{
   class Main
   {
      public static void Init()
      {
         Profiles.Init();
         Console.Init();
         ActionMap globalActionMap = Sim.FindObjectByName<ActionMap>("GlobalActionMap");

         globalActionMap.bindCmd("keyboard", "tilde", "toggleConsole");
      }

      public static void ToggleConsole(string make)
      {
         if (GenericMarshal.StringToBool(make))
         {
            if (Console.ConsoleDlg.isAwake())
            {
               // Deactivate the console
               Sys.Canvas.GameCanvas.popDialog(Console.ConsoleDlg);
            }
            else
            {
               Sys.Canvas.GameCanvas.pushDialog(Console.ConsoleDlg.Name, 99);
            }
         }
      }
   }
}