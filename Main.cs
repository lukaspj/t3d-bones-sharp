using System.Reflection;
using Torque3D;
using Torque3D.Engine;
using Path = System.IO.Path;

namespace Game
{
   public class Main
   {
      [ScriptEntryPoint]
      public static void main()
      {
         // --- Boilerplate C#-specific setup. Normally Torque uses the main.cs file to set these variables, here we have to do it ourselves.
         string CSDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Replace('\\', '/');
         Global.setMainDotCsDir(CSDir);
         Global.setCurrentDirectory(CSDir);
         // ---

         // Enable console logging, which creates the file console.log each time you run
         // the engine.
         Global.setLogMode(6);

         /*Global.eval(@"echo(""------CONSOLE CLASSES BEGIN------"");
dumpConsoleClasses(false, true);
echo(""------CONSOLE CLASSES END------"");
echo(""------CONSOLE FUNCTIONS BEGIN------"");
dumpConsoleFunctions(false, true);
echo(""------CONSOLE FUNCTIONS END------"");
quit();");*/

         // Display a splash window immediately to improve app responsiveness before
         // engine is initialized and main window created
         Global.displaySplashWindow("splash.bmp");

         //-----------------------------------------------------------------------------
         // Load up scripts to initialise subsystems.
         Sys.Main.Init();

         // The GameCanvas needs to be initialized before any gui scripts are run since
         // some of the controls assume that the GameCanvas exists at load time.
         Sys.Canvas.createCanvas("T#bones");

         // Start rendering and stuff.
         Sys.RenderManager.initRenderManager();
         Sys.Lighting.initLightingSystems("Basic Lighting");

         // Start audio.
         Sys.Audio.sfxStartup();

         //-----------------------------------------------------------------------------
         // Load console.
         Lib.Console.Main.Init();

         // Load up game code.
         Game.Main.Init();

         // Create a local game server and connect to it.
         SimGroup serverGroup = new SimGroup("ServerGroup");
         serverGroup.registerObject();
         GameConnection serverConnection = new GameConnection();
         serverConnection.Name = "ServerConnection";
         serverConnection.registerObject();

         // This calls GameConnection::onConnect.
         serverConnection.connectLocal();

         // Start game-specific scripts.
         Game.Main.onStart();
      }


      // Provide stubs so we don't get console errors. If you actually want to use
      // any of these functions, be sure to remove the empty definition here.
      [ConsoleFunction]
      public static void onDataBlockObjectReceived(string index, string total)
      {
      }

      [ConsoleFunction]
      public static void onGhostAlwaysObjectReceived()
      {
      }

      [ConsoleFunction]
      public static void onGhostAlwaysStarted(string ghostCount)
      {
      }

      [ConsoleFunction]
      public static void updateTSShapeLoadProgress()
      {
      }

      //-----------------------------------------------------------------------------
      // Called when the engine is shutting down.
      [ConsoleFunction]
      public static void onExit()
      {
         GameConnection serverConnection = Sim.FindObjectByName<GameConnection>("ServerConnection");
         SimGroup serverGroup = Sim.FindObjectByName<SimGroup>("ServerGroup");

         // Clean up ghosts.
         serverConnection.delete();

         // Clean up game objects and so on.
         Game.Main.onEnd();

         // Delete server-side objects and datablocks.
         serverGroup.delete();
         Global.deleteDataBlocks();
      }
   }
}