using Torque3D;
using Torque3D.Engine;
using Torque3D.Util;

namespace Game.Sys
{
    public class Canvas
    {
        public static void Init()
        {
            // Constants for referencing video resolution preferences
            Globals.SetInt("WORD::RES_X", 0);
            Globals.SetInt("WORD::RES_Y", 1);
            Globals.SetInt("WORD::FULLSCREEN", 2);
            Globals.SetInt("WORD::BITDEPTH", 3);
            Globals.SetInt("WORD::REFRESH", 4);
            Globals.SetInt("WORD::AA", 5);
        }

        public static void createCanvas(string windowTitle)
        {
            // Create the Canvas
            GuiCanvas canvas = new DisplayCursorCanvas()
            {
                Name = "Canvas",

                //TODO: showWindow = false
            };
            canvas.registerObject();

            // Set the window title
            if (Global.isObject("Canvas"))
            {
                canvas.setWindowTitle(windowTitle);
                configureCanvas();
            }
            else
            {
                Global.error("Canvas creation failed. Shutting down.");
                Global.quit();
            }
        }

        private static void configureCanvas()
        {
            // Setup a good default if we don't have one already.
            if (string.IsNullOrWhiteSpace(Globals.GetString("pref::Video::mode")))
                Globals.SetString("pref::Video::mode", "800 600 false 32 60 0");

            string mode = Globals.GetString("pref::Video::mode");
            string[] modeSplit = mode.Split(' ');
            uint resX = uint.Parse(modeSplit[0]);
            uint resY = uint.Parse(modeSplit[1]);
            bool fs = bool.Parse(modeSplit[2]);
            string _bpp = modeSplit[3];
            uint bpp = uint.Parse(_bpp);
            uint rate = uint.Parse(modeSplit[4]);
            uint fsaa = uint.Parse(modeSplit[5]);

            Global.echo("--------------");
            Global.echo("Attempting to set resolution to \"" + mode + "\"");

            Point3F deskRes = Global.getDesktopResolution();

            // We shouldn't be getting this any more but just in case...
            if (_bpp == "Default")
                bpp = (uint) deskRes.Z;

            GuiCanvas canvas = Sim.FindObjectByName<GuiCanvas>("Canvas");

            // Make sure we are running at a valid resolution
            if (!fs)
            {
                // Windowed mode has to use the same bit depth as the desktop
                bpp = (uint) deskRes.Z;

                // Windowed mode also has to run at a smaller resolution than the desktop
                if ((resX >= deskRes.X) || (resY >= deskRes.Y))
                {
                    Global.warn(
                        "Warning: The requested windowed resolution is equal to or larger than the current desktop resolution. Attempting to find a better resolution");

                    int resCount = canvas.getModeCount();
                    for (int i = (resCount - 1); i >= 0; i--)
                    {
                        string testRes = canvas.getMode(i);
                        string[] testResSplit = testRes.Split(' ');
                        uint testResX = uint.Parse(testResSplit[0]);
                        uint testResY = uint.Parse(testResSplit[1]);
                        uint testBPP = uint.Parse(testResSplit[2]);

                        if (testBPP != bpp)
                            continue;

                        if ((testResX < deskRes.X) && (testResY < deskRes.Y))
                        {
                            // This will work as our new resolution
                            resX = testResX;
                            resY = testResY;

                            Global.warn($"Warning: Switching to \"{resX} {resY} {bpp}\"");

                            break;
                        }
                    }
                }
            }

            Globals.SetString("pref::Video::mode", $"{resX} {resY} {fs} {bpp} {rate} {fsaa}");

            string fsLabel = "No";
            if (fs)
                fsLabel = "Yes";

            Global.echo("Accepted Mode: \n" +
                        $"--Resolution : {resX} {resY}\n" +
                        $"--Full Screen : {fsLabel} \n" +
                        $"--Bits Per Pixel : {bpp} \n" +
                        $"--Refresh Rate : {rate} \n" +
                        $"--FSAA Level : {fsaa} \n" +
                        "--------------");

            // Actually set the new video mode
            canvas.setVideoMode(resX, resY, fs, bpp, rate, fsaa);

            // FXAA piggybacks on the FSAA setting in $pref::Video::mode.
            if (Global.isObject("FXAA_PostEffect"))
            {
                PostEffect FXAA_PostEffect = Sim.FindObjectByName<PostEffect>("FXAA_PostEffect");
                FXAA_PostEffect.IsEnabled = (fsaa > 0);
            }
        }
    }
}