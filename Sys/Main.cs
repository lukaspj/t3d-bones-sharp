using System.Configuration;
using Torque3D;

namespace Game.Sys
{
    // ----------------------------------------------------------------------------
    // Initialize core sub system functionality such as audio, the Canvas, PostFX,
    // rendermanager, light managers, etc.
    //
    // Note that not all of these need to be initialized before the client, although
    // the audio should and the canvas definitely needs to be.  I've put things here
    // to distinguish between the purpose and functionality of the various client
    // scripts.  Game specific script isn't needed until we reach the shell menus
    // and start a game or connect to a server. We get the various subsystems ready
    // to go, and then use initClient() to handle the rest of the startup sequence.
    //
    // If this is too convoluted we can reduce this complexity after futher testing
    // to find exactly which subsystems should be readied before kicking things off.
    // ----------------------------------------------------------------------------

    public class Main
    {
        public static void Init()
        {
            // We need some of the default GUI profiles in order to get the canvas and
            // other aspects of the GUI system ready.
            Profiles.Init();

            // Initialization of the various subsystems requires some of the preferences
            // to be loaded... so do that first.
            Config.Init();

            Canvas.Init();
            Cursor.Init();
        }
    }
}