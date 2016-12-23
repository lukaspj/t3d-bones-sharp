using Torque3D;

namespace Game.Sys
{
    public class Lighting
    {
        public static void Init()
        {
        }

        [ConsoleFunction]
        public static void initLightingSystems(string manager)
        {
            Global.echo("\nInitializing Lighting Systems");

            // TODO initialize all lighting subsystems
            LightingSystems.Shadowmaps.ShadowMaps.Init();

            // Try the preferred one first.
            bool success = Global.setLightManager(manager);

            // Did we completely fail to initialize a light manager?
            if (!success)
            {
                // If we completely failed to initialize a light
                // manager then the 3d scene cannot be rendered.
                Global.quitWithErrorMessage("Failed to set a light manager!");
            }
        }

        //---------------------------------------------------------------------------------------------

        [ConsoleFunction]
        public static void onLightManagerActivate(string lmName)
        {
            // Call activation callbacks.
            string activateNewFn = "onActivate" + Global.getWord(lmName, 0) + "LM";
            if (Global.isFunction(activateNewFn))
                Global.eval(activateNewFn + "();");
        }

        //---------------------------------------------------------------------------------------------

        [ConsoleFunction]
        public static void onLightManagerDeactivate(string lmName)
        {
            // Call deactivation callback.
            string deactivateOldFn = "onDeactivate" + Global.getWord(lmName, 0) + "LM";
            if (Global.isFunction(deactivateOldFn))
                Global.eval(deactivateOldFn + "();");
        }
    }
}