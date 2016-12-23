using Torque3D;
using Torque3D.Util;

namespace Game.Sys.GfxData
{
   class ScatterSky
   {
      public static void Init()
      {
         new GFXStateBlockData("ScatterSkySBData")
         {
            CullDefined = true,
            CullMode = GFXCullMode.None,
            ZDefined = true,
            ZEnable = true,
            ZWriteEnable = false,
            ZFunc = GFXCmpFunc.LessEqual,
            SamplersDefined = true,
            SamplerStates =
            {
               [0] = CommonMaterialData.SamplerClampLinear,
               [1] = CommonMaterialData.SamplerClampLinear
            },
            VertexColorEnable = true
         }.registerObject();

         new ShaderData("ScatterSkyShaderData")
         {
            DXVertexShaderFile = "shaders/common/scatterSkyV.hlsl",
            DXPixelShaderFile = "shaders/common/scatterSkyP.hlsl",
            OGLVertexShaderFile = "shaders/common/gl/scatterSkyV.glsl",
            OGLPixelShaderFile = "shaders/common/gl/scatterSkyP.glsl",
            PixVersion = 2.0f
         }.registerObject();
      }
   }
}