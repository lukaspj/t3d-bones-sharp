using Torque3D;
using Torque3D.Engine;

namespace Game
{
    public class Main
    {
        [ScriptEntryPoint]
        public static void main()
        {
            // Enable console logging, which creates the file console.log each time you run
            // the engine.
            Global.setLogMode(2);

            // Display a splash window immediately to improve app responsiveness before
            // engine is initialized and main window created
            Global.displaySplashWindow("splash.bmp");

            //-----------------------------------------------------------------------------
            // Load up scripts to initialise subsystems.
            //exec("sys/main.cs");

            // The canvas needs to be initialized before any gui scripts are run since
            // some of the controls assume that the canvas exists at load time.
            createCanvas("T3Dbones");

            // Start rendering and stuff.
            initRenderManager();
            initLightingSystems("Basic Lighting");

            // Start audio.
            sfxStartup();

            //-----------------------------------------------------------------------------
            // Load console.
            //exec("lib/console/main.cs");

            // Load up game code.
            //exec("game/main.cs");

            // Create a local game server and connect to it.
            SimGroup serverGroup = new SimGroup("ServerGroup");
            serverGroup.registerObject();
            GameConnection serverConnection = new GameConnection();
            serverConnection.Name = "ServerConnection";
            serverConnection.registerObject();

            // This calls GameConnection::onConnect.
            serverConnection.connectLocal();

            // Start game-specific scripts.
            //onStart();
        }


        // Provide stubs so we don't get console errors. If you actually want to use
        // any of these functions, be sure to remove the empty definition here.
        [ConsoleFunction]
        public static void onDatablockObjectReceived() {}
        [ConsoleFunction]
        public static void onGhostAlwaysObjectReceived() {}
        [ConsoleFunction]
        public static void onGhostAlwaysStarted() {}
        [ConsoleFunction]
        public static void updateTSShapeLoadProgress() {}

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
            //onEnd();

            // Delete server-side objects and datablocks.
            serverGroup.delete();
            //deleteDataBlocks();
        }
    }
}