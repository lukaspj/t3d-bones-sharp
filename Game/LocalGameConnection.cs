using Torque3D;
using Torque3D.Engine;
using Torque3D.Util;

namespace Game.Game
{
    [ConsoleClass(pConsoleName: "GameConnection")]
    public class LocalGameConnection : GameConnection
    {
        //-----------------------------------------------------------------------------
        // Called when all datablocks have been transmitted.
        public void onEnterGame()
        {
            //TODO should this be pClient
            //GameConnection client = Sim.FindObject<GameConnection>(pClient);

            // Create a camera for the client.
            Camera theCamera = new Camera()
            {
                Name = "TheCamera",
                DataBlock = Sim.FindObjectByName<CameraData>("Observer")
            };
            theCamera.registerObject();
            theCamera.setTransform(new TransformF(new Point3F(0, 0, 2), new AngAxisF(1, 0, 0, 0)));

            // Cameras are not ghosted (sent across the network) by default; we need to
            // do it manually for the client that owns the camera or things will go south
            // quickly.
            theCamera.scopeToClient(this);
            // And let the client control the camera.
            this.setControlObject(theCamera);
            // Add the camera to the group of game objects so that it's cleaned up when
            // we close the game.
            SimGroup gameGroup = Sim.FindObject<SimGroup>("GameGroup");
            gameGroup.add(theCamera);
            // Activate HUD which allows us to see the game. This should technically be
            // a commandToClient, but since the client and server are on the same
            // machine...
            GuiCanvas canvas = Sim.FindObject<GuiCanvas>("Canvas");
            canvas.setContent(Sim.FindObject<GuiTSCtrl>("PlayGui"));
            Global.activateDirectInput();
        }

        // Called when we connect to the local game.
        public void onConnect()
        {
            transmitDataBlocks(0);
        }

        // Called when all datablocks from above have been transmitted.
        public void onDataBlocksDone()
        {
            //closeSplashWindow();
            GuiCanvas canvas = Sim.FindObjectByName<GuiCanvas>("Canvas");
            canvas.showWindow();

            // Start sending ghosts to the client.
            activateGhosting();
            onEnterGame();
        }
    }
}