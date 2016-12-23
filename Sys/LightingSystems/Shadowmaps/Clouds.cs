using Torque3D;

namespace Game.Sys.LightingSystems.Shadowmaps
{
   class ShadowMaps
   {
      public static void Init()
      {
         new ShaderData("BlurDepthShader")
         {
            DXVertexShaderFile = "shaders/common/lighting/shadowMap/boxFilterV.hlsl",
            DXPixelShaderFile = "shaders/common/lighting/shadowMap/boxFilterP.hlsl",
            OGLVertexShaderFile = "shaders/common/lighting/shadowMap/gl/boxFilterV.glsl",
            OGLPixelShaderFile = "shaders/common/lighting/shadowMap/gl/boxFilterP.glsl",
            PixVersion = 2.0f
         }.registerObject();
      }
   }
}