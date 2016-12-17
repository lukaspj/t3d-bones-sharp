using Torque3D;
using Torque3D.Engine;
using Torque3D.Util;

namespace Game.Game
{
    public class PlayGui : GameTSCtrl
    {
        public PlayGui()
            : base()
        {
            Name = "PlayGui";
            IsContainer = true;
            Profile = Sim.FindObject<GuiControlProfile>("GuiContentProfile");
            HorizSizing = GuiHorizontalSizing.ResizeRight;
            VertSizing = GuiVerticalSizing.ResizeBottom;
            Position = new Point2I(0, 0);
            Extent = new Point2I(1024, 768);
            MinExtent = new Point2I(8, 8);
            CanSave = true;
            Visible = true;
            Margin = new RectSpacingI(0, 0, 0, 0);
            Padding = new RectSpacingI(0, 0, 0, 0);
            AnchorTop = true;
            AnchorBottom = false;
            AnchorLeft = true;
            AnchorRight = false;
            CameraZRot = 0.0f;
            ForceFOV = 0.0f;
            /* TODO: Are these necessary? If they are, set them with setField from SimObject
               enabled = "1";
               helpTag = "0";
               noCursor = "0";
            */
        }
    }
}