using Torque3D;
using Torque3D.Engine;
using Torque3D.Util;

namespace Game.Lib.Console
{
   [ConsoleClass]
   class ConsoleEntry : GuiConsoleEditCtrl
   {
      public ConsoleEntry()
      {
         Name = "ConsoleEntry";
         Profile = Sim.FindObjectByName<GuiControlProfile>("ConsoleTextEditProfile");
         HorizSizing = GuiHorizontalSizing.ResizeWidth;
         VertSizing = GuiVerticalSizing.ResizeTop;
         Position = new Point2I(0, 462);
         Extent = new Point2I(640, 18);
         MinExtent = new Point2I(8, 8);
         Visible = true;
         AltCommand = "ConsoleEntry::eval();";
         //TODO HelpTag = "0";
         MaxLength = 255;
         HistorySize = 40;
         Password = false;
         TabComplete = false;
         SinkAllKeyEvents = true;
         UseSiblingScroller = true;
      }

      public void eval()
      {
         string text = getValue().Trim();
         if (string.IsNullOrEmpty(text))
            return;

         // If it's missing a trailing () and it's not a variable,
         // append the parentheses.
         if (text.IndexOf('(') == -1 && !Global.isDefined(text))
         {
            if (text.IndexOf('=') == -1 && text.IndexOf(' ') == -1)
            {
               if (text.IndexOf('{') == -1 && text.IndexOf('}') == -1)
               {
                  text = text + "()";
               }
            }
         }

         // Append a semicolon if need be.
         int pos = text.Length - 1;
         if (text.IndexOf(';', pos) == -1 && text.IndexOf('}') == -1)
         {
            text = text + ";";
         }

         // Turn off warnings for assigning from void
         // and evaluate the snippet.
         bool oldWarnVoidAssignment;
         if (!Global.isDefined("$Con::warnVoidAssignment"))
            oldWarnVoidAssignment = true;
         else
            oldWarnVoidAssignment = Globals.GetBool("Con::warnVoidAssignment");
         Globals.SetBool("Con::warnVoidAssignment", false);

         Global.echo("==>" + text);
         string result;
         if (!text.StartsWith("function ")
             && !text.StartsWith("datablock ")
             && !text.StartsWith("foreach(")
             && !text.StartsWith("foreach$(")
             && !text.StartsWith("if(")
             && !text.StartsWith("while(")
             && !text.StartsWith("for(")
             && !text.StartsWith("switch(")
             && !text.StartsWith("switch$("))
            result = Global.eval(text);
         else
            result = Global.eval(text);
         Globals.SetBool("Con::warnVoidAssignment", oldWarnVoidAssignment);

         setValue("");

         // Echo result.
         if (!string.IsNullOrEmpty(result))
            Global.echo(result);
      }
   }
}