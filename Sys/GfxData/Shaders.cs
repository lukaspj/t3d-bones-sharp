using Torque3D;

namespace Game.Sys.GfxData
{
   class Shaders
   {
      public static void Init()
      {
         //-----------------------------------------------------------------------------
         //  This file contains shader data necessary for various engine utility functions
         //-----------------------------------------------------------------------------


         new ShaderData("_DebugInterior_")
         {
            DXVertexShaderFile = "shaders/common/debugInteriorsV.hlsl",
            DXPixelShaderFile = "shaders/common/debugInteriorsP.hlsl",
            OGLVertexShaderFile = "shaders/common/gl/debugInteriorsV.glsl",
            OGLPixelShaderFile = "shaders/common/gl/debugInteriorsP.glsl",
            SamplerNames = {[0] = "$diffuseMap"},
            PixVersion = 1.1f
         }.registerObject();

         new ShaderData("ParticlesShaderData")
         {
            DXVertexShaderFile = "shaders/common/particlesV.hlsl",
            DXPixelShaderFile = "shaders/common/particlesP.hlsl",
            OGLVertexShaderFile = "shaders/common/gl/particlesV.glsl",
            OGLPixelShaderFile = "shaders/common/gl/particlesP.glsl",
            SamplerNames =
            {
               [0] = "$diffuseMap",
               [1] = "$prepassTex",
               [2] = "$paraboloidLightMap"
            },
            PixVersion = 2.0f
         }.registerObject();

         new ShaderData("OffscreenParticleCompositeShaderData")
         {
            DXVertexShaderFile = "shaders/common/particleCompositeV.hlsl",
            DXPixelShaderFile = "shaders/common/particleCompositeP.hlsl",
            OGLVertexShaderFile = "shaders/common/gl/particleCompositeV.glsl",
            OGLPixelShaderFile = "shaders/common/gl/particleCompositeP.glsl",
            SamplerNames =
            {
               [0] = "$colorSource",
               [1] = "$edgeSource"
            },
            PixVersion = 2.0f
         }.registerObject();

         //-----------------------------------------------------------------------------
         // Planar Reflection
         //-----------------------------------------------------------------------------
         new ShaderData("ReflectBump")
         {
            DXVertexShaderFile = "shaders/common/planarReflectBumpV.hlsl",
            DXPixelShaderFile = "shaders/common/planarReflectBumpP.hlsl",
            OGLVertexShaderFile = "shaders/common/gl/planarReflectBumpV.glsl",
            OGLPixelShaderFile = "shaders/common/gl/planarReflectBumpP.glsl",
            SamplerNames =
            {
               [0] = "$diffuseMap",
               [1] = "$refractMap",
               [2] = "$bumpMap"
            },
            PixVersion = 2.0f
         }.registerObject();

         new ShaderData("Reflect")
         {
            DXVertexShaderFile = "shaders/common/planarReflectV.hlsl",
            DXPixelShaderFile = "shaders/common/planarReflectP.hlsl",
            OGLVertexShaderFile = "shaders/common/gl/planarReflectV.glsl",
            OGLPixelShaderFile = "shaders/common/gl/planarReflectP.glsl",
            SamplerNames =
            {
               [0] = "$diffuseMap",
               [1] = "$refractMap"
            },
            PixVersion = 1.4f
         }.registerObject();

         //-----------------------------------------------------------------------------
         // fxFoliageReplicator
         //-----------------------------------------------------------------------------
         new ShaderData("fxFoliageReplicatorShader")
         {
            DXVertexShaderFile = "shaders/common/fxFoliageReplicatorV.hlsl",
            DXPixelShaderFile = "shaders/common/fxFoliageReplicatorP.hlsl",
            OGLVertexShaderFile = "shaders/common/gl/fxFoliageReplicatorV.glsl",
            OGLPixelShaderFile = "shaders/common/gl/fxFoliageReplicatorP.glsl",
            SamplerNames =
            {
               [0] = "$diffuseMap",
               [1] = "$alphaMap"
            },
            PixVersion = 1.4f
         }.registerObject();
      }
   }
}