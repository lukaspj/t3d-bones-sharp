using Torque3D;
using Torque3D.Engine;

namespace Game.Sys
{
    public class Cursor
    {
        public static void Init()
        {
            Globals.SetBool("cursorControlled", true);
        }

        //---------------------------------------------------------------------------------------------
        // Cursor toggle functions.
        //---------------------------------------------------------------------------------------------
        public static void showCursor()
        {
            if (Globals.GetBool("cursorControlled"))
                Global.lockMouse(false);
            GuiCanvas canvas = Sim.FindObjectByName<GuiCanvas>("Canvas");
            canvas.cursorOn();
        }

        public static void hideCursor()
        {
            if (Globals.GetBool("cursorControlled"))
                Global.lockMouse(true);
            GuiCanvas canvas = Sim.FindObjectByName<GuiCanvas>("Canvas");
            canvas.cursorOff();
        }
    }
}