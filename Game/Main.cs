using Torque3D;
using Torque3D.Engine;
using Torque3D.Util;

namespace Game.Game
{
   public class Main
   {
      public static void Init()
      {
         //-----------------------------------------------------------------------------
         // Load up our main GUI which lets us see the game.
         PlayGui playGui = new PlayGui();
         playGui.registerObject();

         //-----------------------------------------------------------------------------
         // Create a datablock for the observer camera.
         new CameraData("Observer").registerObject();

         //-----------------------------------------------------------------------------
         // And a material to give the ground some colour (even if it's just white).
         new Material("BlankWhite")
         {
            DiffuseColor = {[0] = ColorF.WHITE}
         }.registerObject();
      }


      //-----------------------------------------------------------------------------
      // Called when the engine has been initialised.
      [ConsoleFunction]
      public static void onStart()
      {
         // Create objects!
         SimGroup gameGroup = new SimGroup("GameGroup");
         gameGroup.Name = "GameGroup";
         gameGroup.registerObject();

         LevelInfo levelInfo = new LevelInfo("TheLevelInfo")
         {
            CanvasClearColor = new ColorI(0, 0, 0, 0)
         };
         levelInfo.registerObject();

         GroundPlane groundPlane = new GroundPlane("TheGround")
         {
            Position = Point3F.Zero,
            Material = "BlankWhite"
         };
         groundPlane.registerObject();

         Sun sun = new Sun("TheSun")
         {
            Azimuth = 230,
            Elevation = 45,
            Color = ColorF.WHITE,
            Ambient = new ColorF(0.1f, 0.1f, 0.1f, 1),
            CastShadows = true
         };
         sun.registerObject();

         gameGroup.add(levelInfo, groundPlane, sun);

         // Allow us to exit the game...
         ActionMap globalActionMap = Sim.FindObjectByName<ActionMap>("GlobalActionMap");
         globalActionMap.bindCmd("keyboard", "escape", "quit");
      }

      //-----------------------------------------------------------------------------
      // Called when the engine is shutting down.
      [ConsoleFunction]
      public static void onEnd()
      {
         // Delete the objects we created.
         SimGroup gameGroup = Sim.FindObject<SimGroup>("GameGroup");
         gameGroup.delete();
      }
   }
}