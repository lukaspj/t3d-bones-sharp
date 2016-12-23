using Torque3D;

namespace Game.Sys.GfxData
{
   class TerrainBlock
   {
      public static void Init()
      {
         /// Used when generating the blended base texture.
         new ShaderData("TerrainBlendShader")
         {
            DXVertexShaderFile = "shaders/common/terrain/blendV.hlsl",
            DXPixelShaderFile = "shaders/common/terrain/blendP.hlsl",
            OGLVertexShaderFile = "shaders/common/terrain/gl/blendV.glsl",
            OGLPixelShaderFile = "shaders/common/terrain/gl/blendP.glsl",
            PixVersion = 2.0f
         }.registerObject();
      }
   }
}