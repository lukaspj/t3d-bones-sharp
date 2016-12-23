using Torque3D;

namespace Game.Sys.GfxData
{
   class Clouds
   {
      public static void Init()
      {
         //------------------------------------------------------------------------------
         // CloudLayer
         //------------------------------------------------------------------------------

         new ShaderData("CloudLayerShader")
         {
            DXVertexShaderFile = "shaders/common/cloudLayerV.hlsl",
            DXPixelShaderFile = "shaders/common/cloudLayerP.hlsl",
            OGLVertexShaderFile = "shaders/common/gl/cloudLayerV.glsl",
            OGLPixelShaderFile = "shaders/common/gl/cloudLayerP.glsl",
            PixVersion = 2.0f
         }.registerObject();

         //------------------------------------------------------------------------------
         // BasicClouds
         //------------------------------------------------------------------------------

         new ShaderData("BasicCloudsShader")
         {
            DXVertexShaderFile = "shaders/common/basicCloudsV.hlsl",
            DXPixelShaderFile = "shaders/common/basicCloudsP.hlsl",

            //OGLVertexShaderFile = "shaders/common/gl/basicCloudsV.glsl",
            //OGLPixelShaderFile = "shaders/common/gl/basicCloudsP.glsl",

            PixVersion = 2.0f
         }.registerObject();
      }
   }
}